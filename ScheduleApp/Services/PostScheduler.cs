using System;
using System.Collections.Generic;
using System.Linq;
using GuardScheduler.Data;
using GuardScheduler.Models;

namespace GuardScheduler.Services
{
    public class PostScheduler
    {
        private readonly IPersonRepository _personRepo;
        private readonly IShiftSlotRepository _shiftSlotRepo;
        private readonly IAssignmentRepository _assignmentRepo;
        private readonly AssignmentTracker _tracker;

        private readonly Dictionary<Role, RotationQueue<int>> _roleQueues = new Dictionary<Role, RotationQueue<int>>();
        private readonly Dictionary<int, RotationQueue<int>> _postQueues = new Dictionary<int, RotationQueue<int>>();

        public PostScheduler(IPersonRepository personRepo, IShiftSlotRepository shiftSlotRepo,
            IAssignmentRepository assignmentRepo, AssignmentTracker tracker)
        {
            _personRepo = personRepo;
            _shiftSlotRepo = shiftSlotRepo;
            _assignmentRepo = assignmentRepo;
            _tracker = tracker;

            InitRoleQueues();
        }

        private void InitRoleQueues()
        {
            var people = _personRepo.GetAll().Where(p => p.Available).ToList();
            foreach (Role role in Enum.GetValues(typeof(Role)))
            {
                var list = people.Where(p => p.PrimaryRole == role)
                                 .OrderBy(p => p.RotationOrder)
                                 .Select(p => p.Id)
                                 .ToList();
                _roleQueues[role] = new RotationQueue<int>(list);
            }
        }

        public ScheduleDay SchedulePostsForDate(DateTime date, IEnumerable<Post> posts)
        {
            var day = new ScheduleDay { Date = date };
            var assignedByPost = new Dictionary<string, HashSet<int>>();

            var rolePools = Enum.GetValues(typeof(Role))
                .Cast<Role>()
                .ToDictionary(r => r, r => _personRepo.GetAll()
                    .Where(p => p.PrimaryRole == r && p.Available)
                    .OrderBy(p => p.RotationOrder)
                    .Select(p => p.Id)
                    .ToList());

            foreach (var post in posts)
            {
                if (post.SlotsPerDay <= 0) continue;
                if (!assignedByPost.ContainsKey(post.Name))
                    assignedByPost[post.Name] = new HashSet<int>();

                AssignPostShifts(day, post, assignedByPost, rolePools);
            }

            return day;
        }

        private void AssignPostShifts(ScheduleDay day, Post post, Dictionary<string, HashSet<int>> assignedByPost, Dictionary<Role, List<int>> rolePools)
        {
            // Assign shifts based on post role type
            if (post.AllowedRoles.Contains(Role.Negahban))
                AssignShiftsForRole(day, post, Role.Negahban, 3, assignedByPost, rolePools, new[] { 0, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22 }, 2);
            else if (post.AllowedRoles.Contains(Role.Dezhban) && post.Name != "نیروی آماده")
                AssignShiftsForRole(day, post, Role.Dezhban, 3, assignedByPost, rolePools, new[] { 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22 }, 2);
            else if (post.AllowedRoles.Contains(Role.PasBakhsh) && post.Name != "نیروی آماده")
                AssignShiftsForRole(day, post, Role.PasBakhsh, 2, assignedByPost, rolePools, new[] { 1, 5, 9, 13, 17, 21 }, 4);
            else if (post.Name == "نیروی آماده")
                AssignReadyForce(day, post, assignedByPost);
            else
                AssignDefaultShifts(day, post, assignedByPost);
        }

        private void AssignShiftsForRole(ScheduleDay day, Post post, Role role, int count, Dictionary<string, HashSet<int>> assignedByPost, Dictionary<Role, List<int>> rolePools, int[] startHours, int duration)
        {
            SetupPostQueueFromPool(post, role, count, rolePools);

            foreach (var start in startHours)
            {
                var personId = GetNextAvailablePerson(post.Id, assignedByPost, post.Name);
                if (personId.HasValue)
                {
                    AddShift(day, post.Id, start, duration, personId.Value);
                    assignedByPost[post.Name].Add(personId.Value);
                }
            }
        }

        private void AssignReadyForce(ScheduleDay day, Post post, Dictionary<string, HashSet<int>> assignedByPost)
        {
            var personId = AssignFromQueueAvoidingOtherPosts(Role.PasBakhsh, assignedByPost)
                           ?? AssignFromQueueAvoidingOtherPosts(Role.Dezhban, assignedByPost);
            if (personId.HasValue)
            {
                AddShift(day, post.Id, 0, 24, personId.Value);
                assignedByPost[post.Name].Add(personId.Value);
            }
        }

        private void AssignDefaultShifts(ScheduleDay day, Post post, Dictionary<string, HashSet<int>> assignedByPost)
        {
            for (int idx = 0; idx < post.SlotsPerDay; idx++)
            {
                int startHour = (24 / Math.Max(1, post.SlotsPerDay)) * idx;
                var slot = new ShiftSlot
                {
                    Date = day.Date,
                    PostId = post.Id,
                    Start = TimeSpan.FromHours(startHour),
                    DurationHours = post.SlotDurationHours,
                    SlotIndex = idx
                };
                slot.Id = _shiftSlotRepo.Insert(slot);
                day.ShiftSlots.Add(slot);

                var assignedPersonId = AssignPersonToSlotAvoidingOtherPosts(post, assignedByPost);
                if (assignedPersonId.HasValue)
                {
                    AddShift(day, post.Id, startHour, post.SlotDurationHours, assignedPersonId.Value);
                    assignedByPost[post.Name].Add(assignedPersonId.Value);
                }
            }
        }

        // --- Person assignment helpers ---
        private int? AssignFromQueueAvoidingOtherPosts(Role role, Dictionary<string, HashSet<int>> assignedByPost)
        {
            if (_roleQueues.TryGetValue(role, out var queue))
            {
                int attempts = queue.Count;
                while (attempts-- > 0)
                {
                    var personId = queue.DequeueAndRotate();
                    var person = _personRepo.GetById(personId);
                    if (person == null || !_tracker.CanAssign(person)) continue;

                    if (!assignedByPost.Any(kvp => kvp.Value.Contains(personId)))
                    {
                        _tracker.RecordAssignment(personId);
                        return personId;
                    }
                }
            }
            return null;
        }

        private int? AssignPersonToSlotAvoidingOtherPosts(Post post, Dictionary<string, HashSet<int>> assignedByPost)
        {
            var availablePersons = _personRepo.GetAll()
                .Where(p => p.Available)
                .Where(p => post.AllowedRoles.Contains(p.PrimaryRole))
                .Where(p => !assignedByPost.Any(kvp => kvp.Key != post.Name && kvp.Value.Contains(p.Id)))
                .ToList();

            if (post.Name != "ضلع غربی")
                availablePersons = availablePersons.Where(p => p.PrimaryRole != Role.MoafAzRazm).ToList();

            availablePersons = availablePersons.Where(p => _tracker.CanAssign(p)).ToList();

            if (!availablePersons.Any()) return null;

            var role = availablePersons.First().PrimaryRole;
            var selectedId = AssignFromQueueAvoidingOtherPosts(role, assignedByPost) ?? availablePersons.First().Id;
            _tracker.RecordAssignment(selectedId);
            return selectedId;
        }

        private void SetupPostQueueFromPool(Post post, Role role, int count, Dictionary<Role, List<int>> rolePools)
        {
            if (_postQueues.ContainsKey(post.Id)) return;

            var pool = rolePools[role];

            if (role == Role.MoafAzRazm && post.Name != "ضلع غربی")
                pool = new List<int>();

            if (pool.Count < count)
                throw new InvalidOperationException($"Not enough personnel for {role} on post {post.Name}");

            var ids = pool.Take(count).ToList();
            foreach (var id in ids) pool.Remove(id);

            _postQueues[post.Id] = new RotationQueue<int>(ids);
        }

        private void AddShift(ScheduleDay day, int postId, int startHour, int duration, int personId)
        {
            var slot = new ShiftSlot
            {
                Date = day.Date,
                PostId = postId,
                Start = TimeSpan.FromHours(startHour),
                DurationHours = duration,
                SlotIndex = startHour
            };
            slot.Id = _shiftSlotRepo.Insert(slot);
            day.ShiftSlots.Add(slot);

            var assignment = new Assignment
            {
                ShiftSlotId = slot.Id,
                PersonId = personId,
                AssignedAt = DateTime.UtcNow
            };
            day.Assignments.Add(assignment);
            _assignmentRepo.Insert(assignment);
        }

        private int? GetNextAvailablePerson(int postId, Dictionary<string, HashSet<int>> assignedByPost, string postName = null)
        {
            if (_postQueues.TryGetValue(postId, out var queue))
            {
                int attempts = queue.Count;
                while (attempts-- > 0)
                {
                    var personId = queue.DequeueAndRotate();
                    var person = _personRepo.GetById(personId);
                    if (person == null || !_tracker.CanAssign(person)) continue;

                    if (!assignedByPost.Any(kvp => kvp.Key != postName && kvp.Value.Contains(personId)))
                    {
                        _tracker.RecordAssignment(personId);
                        return personId;
                    }
                }
            }
            return null;
        }
    }
}

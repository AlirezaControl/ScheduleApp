using System;
using System.Collections.Generic;
using System.Linq;
using GuardScheduler.Models;
using GuardScheduler.Data;

namespace GuardScheduler.Services
{
    public class SchedulerService : ISchedulerService
    {
        private readonly IPersonRepository _personRepo;
        private readonly IPostRepository _postRepo;
        private readonly IShiftSlotRepository _shiftSlotRepo;
        private readonly IAssignmentRepository _assignmentRepo;
        private readonly SchedulerOptions _options;

        private readonly Dictionary<Role, RotationQueue<int>> _roleQueues = new Dictionary<Role, RotationQueue<int>>();
        private readonly Dictionary<int, RotationQueue<int>> _postRotationQueues = new Dictionary<int, RotationQueue<int>>();

        public SchedulerService(
            IPersonRepository personRepo,
            IPostRepository postRepo,
            IShiftSlotRepository shiftSlotRepo,
            IAssignmentRepository assignmentRepo,
            SchedulerOptions options)
        {
            _personRepo = personRepo;
            _postRepo = postRepo;
            _shiftSlotRepo = shiftSlotRepo;
            _assignmentRepo = assignmentRepo;
            _options = options;
            InitQueues();
        }

        private void InitQueues()
        {
            var people = _personRepo.GetAll().Where(p => p.Available).ToList();
            foreach (Role role in Enum.GetValues(typeof(Role)))
            {
                var list = people
                    .Where(p => p.PrimaryRole == role)
                    .OrderBy(p => p.RotationOrder)
                    .Select(p => p.Id)
                    .ToList();
                _roleQueues[role] = new RotationQueue<int>(list);
            }
        }

        public List<ScheduleDay> GenerateSchedule(DateTime fromDate, DateTime toDate)
        {
            var days = new List<ScheduleDay>();
            for (var d = fromDate.Date; d <= toDate.Date; d = d.AddDays(1))
            {
                days.Add(GenerateScheduleForDate(d));
            }
            return days;
        }

        public ScheduleDay GenerateScheduleForDate(DateTime date)
        {
            var posts = _postRepo.GetAll();
            var day = new ScheduleDay { Date = date };

            var assignedByPostName = new Dictionary<string, HashSet<int>>();

            var rolePools = Enum.GetValues(typeof(Role))
                .Cast<Role>()
                .ToDictionary(r => r, r => _personRepo.GetAll()
                    .Where(p => p.PrimaryRole == r && p.Available)
                    .OrderBy(p => p.RotationOrder)
                    .Select(p => p.Id)
                    .ToList()
                );

            foreach (var post in posts)
            {
                if (post.SlotsPerDay <= 0) continue;

                if (!assignedByPostName.ContainsKey(post.Name))
                    assignedByPostName[post.Name] = new HashSet<int>();

                // --- Negahban Posts ---
                if (post.AllowedRoles.Contains(Role.Negahban))
                {
                    SetupPostQueueFromPool(post, Role.Negahban, 3, rolePools);
                    var shifts = new List<(int startHour, int duration)>
                    {
                        (0,2),(2,2),(4,2),(6,2),(8,2),(10,2),
                        (12,2),(14,2),(16,2),(18,2),(20,2),(22,2)
                    };

                    foreach (var (startHour, duration) in shifts)
                    {
                        var personId = GetNextAvailableForPost(_postRotationQueues[post.Id], assignedByPostName, post.Name);
                        if (personId.HasValue)
                        {
                            AddShift(day, post.Id, startHour, duration, personId.Value);
                            assignedByPostName[post.Name].Add(personId.Value);
                        }
                    }
                }
                // --- Dezhban Posts ---
                else if (post.AllowedRoles.Contains(Role.Dezhban) && post.Name != "نیروی آماده")
                {
                    SetupPostQueueFromPool(post, Role.Dezhban, 3, rolePools);
                    var shifts = new List<(int startHour, int duration)>
                    {
                        (2,2),(4,2),(6,2),(8,2),(10,2),(12,2),
                        (14,2),(16,2),(18,2),(20,2),(22,2)
                    };

                    foreach (var (startHour, duration) in shifts)
                    {
                        var personId = GetNextAvailableForPost(_postRotationQueues[post.Id], assignedByPostName, post.Name);
                        if (personId.HasValue)
                        {
                            AddShift(day, post.Id, startHour, duration, personId.Value);
                            assignedByPostName[post.Name].Add(personId.Value);
                        }
                    }
                }
                // --- PasBakhsh Posts ---
                else if (post.AllowedRoles.Contains(Role.PasBakhsh) && post.Name != "نیروی آماده")
                {
                    SetupPostQueueFromPool(post, Role.PasBakhsh, 2, rolePools);
                    int[] startHours = { 1, 5, 9, 13, 17, 21 };

                    foreach (var startHour in startHours)
                    {
                        var personId = GetNextAvailableForPost(_postRotationQueues[post.Id], assignedByPostName, post.Name);
                        if (personId.HasValue)
                        {
                            AddShift(day, post.Id, startHour, 4, personId.Value);
                            assignedByPostName[post.Name].Add(personId.Value);
                        }
                    }
                }
                // --- نیروی آماده ---
                else if (post.Name == "نیروی آماده")
                {
                    var personId = AssignFromQueueAvoidingOtherPosts(Role.PasBakhsh, assignedByPostName)
                                   ?? AssignFromQueueAvoidingOtherPosts(Role.Dezhban, assignedByPostName);
                    if (personId.HasValue)
                    {
                        AddShift(day, post.Id, 0, 24, personId.Value);
                        assignedByPostName[post.Name].Add(personId.Value);
                    }
                }
                // --- Default Posts ---
                else
                {
                    for (int idx = 0; idx < post.SlotsPerDay; idx++)
                    {
                        var startHour = (24 / Math.Max(1, post.SlotsPerDay)) * idx;
                        var slot = new ShiftSlot
                        {
                            Date = date,
                            PostId = post.Id,
                            Start = TimeSpan.FromHours(startHour),
                            DurationHours = post.SlotDurationHours,
                            SlotIndex = idx
                        };
                        slot.Id = _shiftSlotRepo.Insert(slot);
                        day.ShiftSlots.Add(slot);

                        var assignedPersonId = AssignPersonToSlotAvoidingOtherPosts(post, assignedByPostName);
                        if (assignedPersonId.HasValue)
                        {
                            var assignment = new Assignment
                            {
                                ShiftSlotId = slot.Id,
                                PersonId = assignedPersonId.Value,
                                AssignedAt = DateTime.UtcNow
                            };
                            day.Assignments.Add(assignment);
                            _assignmentRepo.Insert(assignment);
                            assignedByPostName[post.Name].Add(assignedPersonId.Value);
                        }
                    }
                }
            }

            return day;
        }

        private int? GetNextAvailableForPost(RotationQueue<int> queue, Dictionary<string, HashSet<int>> assignedByPostName, string postName)
        {
            int attempts = queue.Count;
            while (attempts-- > 0)
            {
                var personId = queue.DequeueAndRotate();
                var person = _personRepo.GetById(personId);
                if (person == null || !person.Available) continue;

                if (!assignedByPostName.Any(kvp => kvp.Key != postName && kvp.Value.Contains(personId)))
                    return personId;
            }
            return null;
        }

        private int? AssignFromQueueAvoidingOtherPosts(Role role, Dictionary<string, HashSet<int>> assignedByPostName)
        {
            if (_roleQueues.TryGetValue(role, out var queue))
            {
                int attempts = queue.Count;
                while (attempts-- > 0)
                {
                    var personId = queue.DequeueAndRotate();
                    var person = _personRepo.GetById(personId);
                    if (person == null || !person.Available) continue;

                    if (!assignedByPostName.Any(kvp => kvp.Value.Contains(personId)))
                        return personId;
                }
            }
            return null;
        }

        private int? AssignPersonToSlotAvoidingOtherPosts(Post post, Dictionary<string, HashSet<int>> assignedByPostName)
        {
            var availablePersons = _personRepo.GetAll()
                .Where(p => p.Available)
                .Where(p => post.AllowedRoles.Contains(p.PrimaryRole))
                .Where(p => !assignedByPostName.Any(kvp => kvp.Key != post.Name && kvp.Value.Contains(p.Id)))
                .ToList();

            // Restrict MoafAzRazm to "ضلع غربی"
            if (post.Name != "ضلع غربی")
                availablePersons = availablePersons.Where(p => p.PrimaryRole != Role.MoafAzRazm).ToList();

            if (!availablePersons.Any()) return null;

            var role = availablePersons.First().PrimaryRole;
            return AssignFromQueueAvoidingOtherPosts(role, assignedByPostName) ?? availablePersons.First().Id;
        }

        private void SetupPostQueueFromPool(Post post, Role role, int count, Dictionary<Role, List<int>> rolePools)
        {
            if (_postRotationQueues.ContainsKey(post.Id)) return;

            var pool = rolePools[role];

            // Restrict MoafAzRazm pool to "ضلع غربی"
            if (role == Role.MoafAzRazm && post.Name != "ضلع غربی")
                pool = new List<int>();

            if (pool.Count < count)
                throw new InvalidOperationException($"Not enough available personnel for {role} on post {post.Name}");

            var ids = pool.Take(count).ToList();
            foreach (var id in ids) pool.Remove(id);

            _postRotationQueues[post.Id] = new RotationQueue<int>(ids);
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
    }
}

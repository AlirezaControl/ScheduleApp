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

        private readonly Dictionary<Role, RotationQueue<int>> _roleQueues
            = new Dictionary<Role, RotationQueue<int>>();

        // Per-post rotation queues for Negahban and Dezhban
        private readonly Dictionary<int, RotationQueue<int>> _postRotationQueues
            = new Dictionary<int, RotationQueue<int>>();

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
            var people = _personRepo.GetAll();
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

            // Prepare pools of available personnel per role for this day
            var rolePools = Enum.GetValues(typeof(Role))
                .Cast<Role>()
                .ToDictionary(r => r, r => _personRepo.GetAll()
                    .Where(p => p.PrimaryRole == r)
                    .OrderBy(p => p.RotationOrder)
                    .Select(p => p.Id)
                    .ToList()
                );

            foreach (var post in posts)
            {
                // --- Negahban Posts ---
                if (post.AllowedRoles.Contains(Role.Negahban))
                {
                    SetupPostQueueFromPool(post, Role.Negahban, rolePools);

                    var shifts = new List<(int startHour, int duration)>
                    {
                        (0,2),(2,2),(4,2),(6,2),(8,2),(10,2),
                        (12,2),(14,2),(16,2),(18,2),(20,2),(22,2)
                    };

                    foreach (var (startHour, duration) in shifts)
                    {
                        var personId = _postRotationQueues[post.Id].DequeueAndRotate();
                        AddShift(day, post.Id, startHour, duration, personId);
                    }
                }
                // --- Dezhban Posts ---
                else if (post.AllowedRoles.Contains(Role.Dezhban))
                {
                    SetupPostQueueFromPool(post, Role.Dezhban, rolePools);

                    var shifts = new List<(int startHour, int duration)>
                    {
                        (2,2),(4,2),(6,2),(8,2),(10,2),(12,2),
                        (14,2),(16,2),(18,2),(20,2),(22,2)
                    };

                    foreach (var (startHour, duration) in shifts)
                    {
                        var personId = _postRotationQueues[post.Id].DequeueAndRotate();
                        AddShift(day, post.Id, startHour, duration, personId);
                    }
                }
                // --- PasBakhsh Posts ---
                else if (post.AllowedRoles.Contains(Role.PasBakhsh))
                {
                    int[] startHours = { 1, 5, 9, 13, 17, 21 };
                    foreach (var startHour in startHours)
                    {
                        var personId = AssignFromQueue(Role.PasBakhsh);
                        if (!personId.HasValue) continue;
                        AddShift(day, post.Id, startHour, 4, personId.Value);
                    }
                }
                // --- Default + NiroAmadeh scheduling ---
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

                        var assignedPersonId = AssignPersonToSlot(post);
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
                        }
                    }
                }
            }

            return day;
        }

        // Sets up a 3-person per-post queue, removing them from the role pool
        private void SetupPostQueueFromPool(Post post, Role role, Dictionary<Role, List<int>> rolePools)
        {
            if (_postRotationQueues.ContainsKey(post.Id)) return;

            var pool = rolePools[role];
            if (pool.Count < 3)
                throw new InvalidOperationException($"Not enough personnel for {role} on post {post.Name}");

            var ids = pool.Take(3).ToList();
            // Remove from pool to prevent the same person on another post
            pool.RemoveAll(x => ids.Contains(x));

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

        private int? AssignFromQueue(Role role)
        {
            if (_roleQueues.TryGetValue(role, out var queue) && queue.Count > 0)
            {
                return queue.DequeueAndRotate();
            }
            return null;
        }

        private int? AssignPersonToSlot(Post post)
        {
            var availablePersons = _personRepo.GetAll();
            var matchingPersons = availablePersons
                .Where(p => post.AllowedRoles.Contains(p.PrimaryRole))
                .ToList();

            if (!matchingPersons.Any())
                return null;

            var role = matchingPersons.First().PrimaryRole;
            return AssignFromQueue(role) ?? matchingPersons.First().Id;
        }
    }
}

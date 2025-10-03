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

        // Per-post rotation queues for Negahban, Dezhban, PasBakhsh
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

            // Track assigned persons for this day
            var assignedToday = new HashSet<int>();

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
                if (post.SlotsPerDay <= 0) continue;

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
                        var personId = GetNextAvailableFromPostQueue(post.Id, assignedToday);
                        if (personId.HasValue)
                        {
                            assignedToday.Add(personId.Value);
                            AddShift(day, post.Id, startHour, duration, personId.Value);
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
                        var personId = GetNextAvailableFromPostQueue(post.Id, assignedToday);
                        if (personId.HasValue)
                        {
                            assignedToday.Add(personId.Value);
                            AddShift(day, post.Id, startHour, duration, personId.Value);
                        }
                    }
                }
                // --- PasBakhsh Posts ---
                else if (post.AllowedRoles.Contains(Role.PasBakhsh) && post.Name != "نیروی آماده")
                {
                    SetupPostQueueFromPool(post, Role.PasBakhsh, 2, rolePools);

                    int[] startHours = { 1, 5, 9, 13, 17, 21 }; // 4-hour shifts
                    foreach (var startHour in startHours)
                    {
                        var personId = GetNextAvailableFromPostQueue(post.Id, assignedToday);
                        if (personId.HasValue)
                        {
                            assignedToday.Add(personId.Value);
                            AddShift(day, post.Id, startHour, 4, personId.Value);
                        }
                    }
                }
                // --- نیروی آماده (24-hour single person) ---
                else if (post.Name == "نیروی آماده")
                {
                    var personId = AssignFromQueue(Role.PasBakhsh, assignedToday)
                                   ?? AssignFromQueue(Role.Dezhban, assignedToday);
                    if (personId.HasValue)
                    {
                        assignedToday.Add(personId.Value);
                        AddShift(day, post.Id, 0, 24, personId.Value);
                    }
                }
                // --- Default scheduling for other posts ---
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

                        var assignedPersonId = AssignPersonToSlot(post, assignedToday);
                        if (assignedPersonId.HasValue)
                        {
                            assignedToday.Add(assignedPersonId.Value);
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

        private void SetupPostQueueFromPool(Post post, Role role, int count, Dictionary<Role, List<int>> rolePools)
        {
            if (_postRotationQueues.ContainsKey(post.Id)) return;

            var pool = rolePools[role];
            if (pool.Count < count)
                throw new InvalidOperationException($"Not enough personnel for {role} on post {post.Name}");

            var ids = pool.Take(count).ToList();
            foreach (var id in ids)
                pool.Remove(id);

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

        private int? AssignFromQueue(Role role, HashSet<int> assignedToday)
        {
            if (_roleQueues.TryGetValue(role, out var queue) && queue.Count > 0)
            {
                int attempts = 0;
                int personId;

                do
                {
                    personId = queue.DequeueAndRotate();
                    attempts++;
                    if (attempts > queue.Count) return null; // all already assigned
                } while (assignedToday.Contains(personId));

                return personId;
            }
            return null;
        }

        private int? GetNextAvailableFromPostQueue(int postId, HashSet<int> assignedToday)
        {
            if (!_postRotationQueues.TryGetValue(postId, out var queue) || queue.Count == 0)
                return null;

            int attempts = 0;
            int personId;

            do
            {
                personId = queue.DequeueAndRotate();
                attempts++;
                if (attempts > queue.Count) return null; // all already assigned
            } while (assignedToday.Contains(personId));

            return personId;
        }

        private int? AssignPersonToSlot(Post post, HashSet<int> assignedToday)
        {
            var availablePersons = _personRepo.GetAll();
            var matchingPersons = availablePersons
                .Where(p => post.AllowedRoles.Contains(p.PrimaryRole) && !assignedToday.Contains(p.Id))
                .ToList();

            if (!matchingPersons.Any())
                return null;

            var role = matchingPersons.First().PrimaryRole;
            return AssignFromQueue(role, assignedToday) ?? matchingPersons.First().Id;
        }
    }
}

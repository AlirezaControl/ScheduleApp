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

            foreach (var post in posts)
            {
                // --- Negahban Posts ---
                if (post.AllowedRoles.Contains(Role.Negahban))
                {
                    // Negahbans work in 2-hour alternating shifts (like Pasbakhshs, but 2 hours each)
                    var negahbanShifts = new List<(int startHour, int duration)>
                    {
                        (0,2),(2,2),(4,2),(6,2),(8,2),(10,2),
                        (12,2),(14,2),(16,2),(18,2),(20,2),(22,2)
                    };

                    foreach (var (startHour, duration) in negahbanShifts)
                    {
                        var personId = AssignFromQueue(Role.Negahban);
                        if (!personId.HasValue) continue;

                        var slot = new ShiftSlot
                        {
                            Date = date,
                            PostId = post.Id,
                            Start = TimeSpan.FromHours(startHour),
                            DurationHours = duration,
                            SlotIndex = startHour
                        };
                        slot.Id = _shiftSlotRepo.Insert(slot);
                        day.ShiftSlots.Add(slot);

                        var assignment = new Assignment
                        {
                            ShiftSlotId = slot.Id,
                            PersonId = personId.Value,
                            AssignedAt = DateTime.UtcNow
                        };
                        day.Assignments.Add(assignment);
                        _assignmentRepo.Insert(assignment);
                    }
                }
                // --- PasBakhsh Posts ---
                else if (post.AllowedRoles.Contains(Role.PasBakhsh))
                {
                    int[] startHours = { 1, 5, 9, 13, 17, 21 }; // 6 shifts in day
                    foreach (var startHour in startHours)
                    {
                        var personId = AssignFromQueue(Role.PasBakhsh);
                        if (!personId.HasValue) continue;

                        var slot = new ShiftSlot
                        {
                            Date = date,
                            PostId = post.Id,
                            Start = TimeSpan.FromHours(startHour),
                            DurationHours = 4,
                            SlotIndex = startHour
                        };
                        slot.Id = _shiftSlotRepo.Insert(slot);
                        day.ShiftSlots.Add(slot);

                        var assignment = new Assignment
                        {
                            ShiftSlotId = slot.Id,
                            PersonId = personId.Value,
                            AssignedAt = DateTime.UtcNow
                        };
                        day.Assignments.Add(assignment);
                        _assignmentRepo.Insert(assignment);
                    }
                }
                // --- Dezhban Posts ---
                else if (post.AllowedRoles.Contains(Role.Dezhban))
                {
                    // 2-hour shifts from 2:00 → 24:00
                    var dezhbanShifts = new List<(int startHour, int duration)>
                    {
                        (2,2),(4,2),(6,2),(8,2),(10,2),(12,2),
                        (14,2),(16,2),(18,2),(20,2),(22,2)
                    };

                    foreach (var (startHour, duration) in dezhbanShifts)
                    {
                        var personId = AssignFromQueue(Role.Dezhban);
                        if (!personId.HasValue) continue;

                        var slot = new ShiftSlot
                        {
                            Date = date,
                            PostId = post.Id,
                            Start = TimeSpan.FromHours(startHour),
                            DurationHours = duration,
                            SlotIndex = startHour
                        };
                        slot.Id = _shiftSlotRepo.Insert(slot);
                        day.ShiftSlots.Add(slot);

                        var assignment = new Assignment
                        {
                            ShiftSlotId = slot.Id,
                            PersonId = personId.Value,
                            AssignedAt = DateTime.UtcNow
                        };
                        day.Assignments.Add(assignment);
                        _assignmentRepo.Insert(assignment);
                    }
                }
                // --- Default scheduling ---
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

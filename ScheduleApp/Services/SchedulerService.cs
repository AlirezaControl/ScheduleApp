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
                var list = people.Where(p => p.PrimaryRole == role)
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
                var day = GenerateScheduleForDate(d);
                days.Add(day);
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
                    var negahbanShifts = new List<(int startHour, int duration)>
                    {
                        (0,2),(2,2),(4,2),(6,2),(8,2),(10,2),(12,2),(14,2),(16,2),(18,2),(20,2),(22,2)
                    };

                    int slotsPerShift = 1; // three Negahban per post
                    for (int shiftIdx = 0; shiftIdx < negahbanShifts.Count; shiftIdx++)
                    {
                        var (startHour, duration) = negahbanShifts[shiftIdx];

                        for (int i = 0; i < slotsPerShift; i++)
                        {
                            var slot = new ShiftSlot
                            {
                                Date = date,
                                PostId = post.Id,
                                Start = TimeSpan.FromHours(startHour),
                                DurationHours = duration,
                                SlotIndex = shiftIdx * 10 + i
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
                // --- PasBakhsh Posts ---
                else if (post.AllowedRoles.Contains(Role.PasBakhsh))
                {
                    var matchingPersons = _personRepo.GetAll()
                        .Where(p => p.PrimaryRole == Role.PasBakhsh)
                        .Take(1)
                        .ToList(); // 1 PasBakhsh

                    int[] startHours = { 1,4,9 , 13, 17,21 }; // 4 shifts per person

                    for (int personIdx = 0; personIdx < matchingPersons.Count; personIdx++)
                    {
                        var personId = matchingPersons[personIdx].Id;

                        for (int shiftIdx = 0; shiftIdx < 4; shiftIdx++)
                        {
                            int startHour = startHours[shiftIdx];
                            var slot = new ShiftSlot
                            {
                                Date = date,
                                PostId = post.Id,
                                Start = TimeSpan.FromHours(startHour),
                                DurationHours = 4,
                                SlotIndex = personIdx * 10 + shiftIdx
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

        private int? AssignPersonToSlot(Post post)
        {
            var availablePersons = _personRepo.GetAll();

            var matchingPersons = availablePersons
                .Where(p => post.AllowedRoles.Contains(p.PrimaryRole))
                .ToList();

            if (!matchingPersons.Any())
                return null;

            var role = matchingPersons.First().PrimaryRole;
            if (_roleQueues.TryGetValue(role, out var queue) && queue.Count > 0)
            {
                return queue.DequeueAndRotate(); // Dequeue rotates internally
            }

            return matchingPersons.First().Id;
        }
    }
}
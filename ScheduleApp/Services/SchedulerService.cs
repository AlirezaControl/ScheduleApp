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

                    // Save slot to DB so it gets a valid Id
                    slot.Id = _shiftSlotRepo.Insert(slot);

                    day.ShiftSlots.Add(slot);

                    // Assign a person to the slot
                    var assignedPersonId = AssignPersonToSlot(post);
                    if (assignedPersonId != null)
                    {
                        var assignment = new Assignment
                        {
                            ShiftSlotId = slot.Id,
                            PersonId = assignedPersonId.Value,
                            AssignedAt = DateTime.UtcNow
                        };

                        _assignmentRepo.Insert(assignment);
                    }
                }
            }

            return day;
        }

        private int? AssignPersonToSlot(Post post)
        {
            var availablePersons = _personRepo.GetAll();

            // Filter only those who match allowed roles
            var matchingPersons = availablePersons
                .Where(p => post.AllowedRoles.Contains(p.PrimaryRole))
                .ToList();

            if (!matchingPersons.Any())
                return null;

            // Round-robin by role (instead of always first)
            var role = matchingPersons.First().PrimaryRole;
            if (_roleQueues.TryGetValue(role, out var queue) && queue.Count > 0)
            {
                var personId = queue.DequeueAndRotate();
                queue.Enqueue(personId); // put back for round-robin
                return personId;
            }

            // fallback
            return matchingPersons.First().Id;
        }
    }
}
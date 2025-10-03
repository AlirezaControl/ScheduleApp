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
        private readonly IAssignmentRepository _assignmentRepo;
        private readonly SchedulerOptions _options;

        private readonly Dictionary<Role, RotationQueue<int>> _roleQueues
            = new Dictionary<Role, RotationQueue<int>>();

        public SchedulerService(IPersonRepository personRepo, IPostRepository postRepo,
            IAssignmentRepository assignmentRepo, SchedulerOptions options)
        {
            _personRepo = personRepo;
            _postRepo = postRepo;
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
                    day.ShiftSlots.Add(slot);

                    // Assign a person to the slot based on availability and role
                    var assignedPersonId = AssignPersonToSlot(post);
                    if (assignedPersonId != null)
                    {
                        // Create an assignment object
                        var assignment = new Assignment
                        {
                            ShiftSlotId = slot.Id, // Assuming slot has a unique Id
                            PersonId = assignedPersonId.Value,
                            AssignedAt = DateTime.UtcNow
                        };

                        // Save the assignment to the database
                        _assignmentRepo.Insert(assignment); // Call the insert method
                    }
                }
            }

            return day;
        }

        // This method should return the ID of the assigned person or null if no one is available
        private int? AssignPersonToSlot(Post post)
        {
            var availablePersons = _personRepo.GetAll();
            foreach (Person p in availablePersons)
            {
                if (post.AllowedRoles.Contains(p.PrimaryRole))
                {
                    return p.Id;
                }
            }

            // Implement any additional logic to select a person from availablePersons
            // For example, you might want to round-robin assign or choose based on availability

            // Example: Just return the first available person's ID for simplicity
            return availablePersons.FirstOrDefault()?.Id;
        }
    }
}
// SchedulerService.cs
using System;
using System.Collections.Generic;
using GuardScheduler.Data;
using GuardScheduler.Models;

namespace GuardScheduler.Services
{
    public class SchedulerService : ISchedulerService
    {
        private readonly IPersonRepository _personRepo;
        private readonly IPostRepository _postRepo;
        private readonly IShiftSlotRepository _shiftSlotRepo;
        private readonly IAssignmentRepository _assignmentRepo;
        private readonly SchedulerOptions _options;

        private readonly AssignmentTracker _assignmentTracker;
        private readonly PostScheduler _postScheduler;

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

            _assignmentTracker = new AssignmentTracker(personRepo);
            _postScheduler = new PostScheduler(personRepo, shiftSlotRepo, assignmentRepo, _assignmentTracker);
        }

        public List<ScheduleDay> GenerateSchedule(DateTime fromDate, DateTime toDate)
        {
            _shiftSlotRepo.DeleteAll();

            var days = new List<ScheduleDay>();
            var weekStart = fromDate.Date;

            for (var d = fromDate.Date; d <= toDate.Date; d = d.AddDays(1))
            {
                if ((d - weekStart).TotalDays >= 7)
                {
                    weekStart = d;
                    _assignmentTracker.ResetWeeklyAssignments();
                }

                days.Add(GenerateScheduleForDate(d));
            }

            return days;
        }

        public ScheduleDay GenerateScheduleForDate(DateTime date)
        {
            var posts = _postRepo.GetAll();
            return _postScheduler.SchedulePostsForDate(date, posts);
        }
    }
}

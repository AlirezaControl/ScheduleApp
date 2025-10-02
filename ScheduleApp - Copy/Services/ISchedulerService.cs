using System;
using System.Collections.Generic;
using GuardScheduler.Models;

namespace GuardScheduler.Services
{
    public interface ISchedulerService
    {
        List<ScheduleDay> GenerateSchedule(DateTime fromDate, DateTime toDate);
        ScheduleDay GenerateScheduleForDate(DateTime date);
    }
}
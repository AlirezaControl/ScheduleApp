using System;
using System.Collections.Generic;
using GuardScheduler.Models;

namespace GuardScheduler.Data
{
    public interface IScheduleDayRepository
    {
        void SaveScheduleDays(List<ScheduleDay> scheduleDays);
        List<ScheduleDay> GetScheduleDays(DateTime from, DateTime to);
        void DeleteAll();
    }
}
using System.Collections.Generic;
using GuardScheduler.Models;

namespace GuardScheduler.Services
{
    public interface IExportService
    {
        void ExportToExcel(IEnumerable<ScheduleDay> days, string filePath);
        void ExportToWord(ScheduleDay day, string filePath);
    }
}
using System;
using System.Collections.Generic;
using GuardScheduler.Models;

namespace GuardScheduler.Data
{
    public interface IAssignmentRepository
    {
        List<Assignment> GetAssignmentsForPersonOnDate(int personId, DateTime date);
        List<Assignment> GetAssignmentsForDate(DateTime date);

        // ✅ New alias to match form usage
        List<Assignment> GetAssignmentsByDate(DateTime date);

        int Insert(Assignment a);
        void Delete(int id);
        void DeleteAll();
        List<Assignment> GetAssignmentsForSlot(int shiftSlotId);
    }
}

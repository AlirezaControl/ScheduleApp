using System;
using System.Collections.Generic;
using GuardScheduler.Models;

namespace GuardScheduler.Data
{
    public interface IAssignmentRepository
    {
        List<Assignment> GetAssignmentsForPersonOnDate(int personId, DateTime date);
        List<Assignment> GetAssignmentsForDate(DateTime date);
        int Insert(Assignment a);
        void Delete(int id);
        List<Assignment> GetAssignmentsForSlot(int shiftSlotId);
    }
}

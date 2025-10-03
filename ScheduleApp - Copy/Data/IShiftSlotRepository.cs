using System;
using System.Collections.Generic;
using GuardScheduler.Models;

namespace GuardScheduler.Data
{
    public interface IShiftSlotRepository
    {
        List<ShiftSlot> GetAllForDate(DateTime date);
        ShiftSlot GetById(int id);
        int Insert(ShiftSlot slot);
        void Delete(int id);
    }
}
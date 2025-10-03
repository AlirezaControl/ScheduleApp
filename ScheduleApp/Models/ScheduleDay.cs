using System;
using System.Collections.Generic;
using System.Linq;

namespace GuardScheduler.Models
{
    public class ScheduleDay
    {
        public int Id;
        public DateTime Date { get; set; }
        public List<ShiftSlot> ShiftSlots { get; set; } = new List<ShiftSlot>();
        public List<Assignment> Assignments { get; set; } = new List<Assignment>();

        public Assignment GetAssignment(int slotId)
            => Assignments.FirstOrDefault(a => a.ShiftSlotId == slotId);
    }
}
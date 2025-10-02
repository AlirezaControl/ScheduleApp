using System;

namespace GuardScheduler.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public int ShiftSlotId { get; set; }
        public int PersonId { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}
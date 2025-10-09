using System;

namespace GuardScheduler.Models
{
    public class ShiftSlot
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int PostId { get; set; }
        public TimeSpan Start { get; set; }
        public int DurationHours { get; set; }
        public int SlotIndex { get; set; }
        public TimeSpan Duration => TimeSpan.FromHours(DurationHours);
        public Post Post { get; set; }
    }
}
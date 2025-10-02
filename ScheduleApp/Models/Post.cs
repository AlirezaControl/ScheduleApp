using System.Collections.Generic;

namespace GuardScheduler.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Role> AllowedRoles { get; set; } = new List<Role>();
        public int SlotsPerDay { get; set; } = 1;
        public int SlotDurationHours { get; set; } = 24;
        public bool EnforceRestNextDay { get; set; } = true;
    }
}
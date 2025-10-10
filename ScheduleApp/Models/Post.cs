using System.Collections.Generic;
using System.Linq;

namespace GuardScheduler.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Store as string to match database
        public string AllowedRoles { get; set; }

        // Computed property to get as List<Role>
        public List<Role> AllowedRolesList
        {
            get
            {
                if (string.IsNullOrEmpty(AllowedRoles))
                    return new List<Role>();

                var roles = new List<Role>();
                var roleNames = AllowedRoles.Split(',').Select(r => r.Trim());

                foreach (var roleName in roleNames)
                {
                    if (System.Enum.TryParse<Role>(roleName, out var role))
                    {
                        roles.Add(role);
                    }
                }
                return roles;
            }
            set
            {
                AllowedRoles = value != null ? string.Join(",", value) : "";
            }
        }

        public int SlotsPerDay { get; set; } = 1;
        public int SlotDurationHours { get; set; } = 24;
        public bool EnforceRestNextDay { get; set; } = true;
    }
}
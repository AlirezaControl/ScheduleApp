using System.Collections.Generic;

namespace GuardScheduler.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Role PrimaryRole { get; set; }
        public Role? SecondaryRole { get; set; }
        public int RotationOrder { get; set; }
        public List<string> AllowedPostNames { get; set; } = new List<string>();

        public override string ToString()
        {
            string roles = $"{PrimaryRole}";
            if (SecondaryRole.HasValue)
            {
                roles += $" / {SecondaryRole.Value}";
            }
            return $"{FirstName} {LastName} - Roles: {roles}";
        }
    }
}
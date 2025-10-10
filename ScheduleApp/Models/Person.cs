using GuardScheduler.Models;
using System.Collections.Generic;

public class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool Married { get; set; }
    public Role PrimaryRole { get; set; }
    public Role? SecondaryRole { get; set; }
    public int RotationOrder { get; set; }
    public string AllowedPostNames { get; set; }

    // New property
    public bool Available { get; set; } = true;

    public override string ToString()
    {
        string roles = $"{PrimaryRole}";
        if (SecondaryRole.HasValue)
        {
            roles += $" / {SecondaryRole.Value}";
        }
        return $"{FirstName} {LastName} - Roles: {roles} - Available: {Available}";
    }
}

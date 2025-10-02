using System;
using System.Data;
using GuardScheduler.Data;
using GuardScheduler.Models;

namespace GuardScheduler.Services
{
    public class PersonImporter
    {
        private readonly IPersonRepository _repo;

        public PersonImporter(IPersonRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Imports persons from Excel file. Reads FirstName, LastName, and roles if available.
        /// Excel must have columns: FirstName, LastName, PrimaryRole, SecondaryRole (optional)
        /// </summary>
        public void ImportFromExcel(string filePath)
        {
            DataTable table = ExcelHelper.ReadExcel(filePath);

            foreach (DataRow row in table.Rows)
            {
                var person = new Person
                {
                    FirstName = row["FirstName"].ToString() ?? "",
                    LastName = row["LastName"].ToString() ?? "",
                    PrimaryRole = ParseRole(row["PrimaryRole"].ToString()), // Parse role from Excel
                    SecondaryRole = ParseRole(row["SecondaryRole"].ToString()), // Parse secondary role if any
                    RotationOrder = 0,       // default 0
                    AllowedPostNames = new System.Collections.Generic.List<string>()
                };

                _repo.Insert(person);
            }
        }

        private Role ParseRole(string roleName)
        {
            if (Enum.TryParse<Role>(roleName, true, out var role))
            {
                return role;
            }
            return 0; // or handle as appropriate (e.g., throw an exception, return null, etc.)
        }
    }
}
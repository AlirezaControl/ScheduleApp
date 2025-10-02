using System;
using System.Data;
using GuardScheduler.Data;
using GuardScheduler.Models;

namespace GuardScheduler.Services
{
    public class PersonNameImporter
    {
        private readonly IPersonRepository _repo;

        public PersonNameImporter(IPersonRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Imports persons from Excel file. Only FirstName and LastName are read.
        /// Other fields (roles, rotation, allowed posts) are left empty.
        /// Excel must have columns: FirstName, LastName
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
                    PrimaryRole = 0,         // default placeholder
                    SecondaryRole = null,    // default null
                    RotationOrder = 0,       // default 0
                    AllowedPostNames = new System.Collections.Generic.List<string>()
                };

                _repo.Insert(person);
            }
        }
    }
}
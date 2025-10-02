using GuardScheduler.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GuardScheduler.Services;
using GuardScheduler;

namespace ScheduleApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var repo = new PersonRepository("Data Source=schedule.db");


            var personListForm = new PersonListForm(repo);

            var importer = new PersonImporter(repo);
            importer.ImportFromExcel("C:\\Temp\\persons.xlsx");
            MessageBox.Show("Names imported successfully!");

         
            Application.Run(personListForm);
        }
    }
}

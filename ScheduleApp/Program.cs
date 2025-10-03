using GuardScheduler;
using GuardScheduler.Data;
using GuardScheduler.Models;
using GuardScheduler.Services;
using System;
using System.Windows.Forms;

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
            string connectionString = "Data Source=schedule.db";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initialize repositories
            var personRepository = new PersonRepository(connectionString);
            var postRepository = new PostRepository(connectionString);
            var assignmentRepository = new AssignmentRepository(connectionString);

            // Initialize SchedulerOptions
            var schedulerOptions = new SchedulerOptions();

            // Create the SchedulerService with all required dependencies
            var schedulerService = new SchedulerService(personRepository, postRepository, assignmentRepository, schedulerOptions);

            // Initialize and run the main form (ScheduleForm)
            Application.Run(new ScheduleForm(schedulerService));
        }
    }
}
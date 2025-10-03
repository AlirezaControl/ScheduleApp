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

            // Ensure database & tables are created
            DatabaseInitializer.Initialize(connectionString);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // --- Initialize repositories ---
            var personRepository = new PersonRepository(connectionString);
            var postRepository = new PostRepository(connectionString);
            var shiftSlotRepository = new ShiftSlotRepository(connectionString);
            var assignmentRepository = new AssignmentRepository(connectionString);
            var scheduleRepo = new ScheduleDayRepository(connectionString); // ✅ New ScheduleDay repository

            // --- Scheduler Options ---
            var schedulerOptions = new SchedulerOptions();

            // --- Initialize SchedulerService with ShiftSlotRepository ---
            var schedulerService = new SchedulerService(
                personRepository,
                postRepository,
                shiftSlotRepository,
                assignmentRepository,
                schedulerOptions
            );

            // --- Run Main Form ---
            Application.Run(new ScheduleForm(
                schedulerService,
                assignmentRepository,
                personRepository,
                postRepository,
                scheduleRepo // ✅ Pass repository to the form
            ));
        }
    }
}
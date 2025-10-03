using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GuardScheduler.Models;
using GuardScheduler.Services;
using GuardScheduler.Data;

namespace GuardScheduler
{
    public partial class ScheduleForm : Form
    {
        private readonly ISchedulerService _schedulerService;
        private readonly IAssignmentRepository _assignmentRepo;
        private readonly IPersonRepository _personRepo; // Reference to person repository
        private readonly IPostRepository _postRepo; // Reference to post repository

        public ScheduleForm(ISchedulerService schedulerService, IAssignmentRepository assignmentRepo, IPersonRepository personRepo, IPostRepository postRepo)
        {
            InitializeComponent();
            _schedulerService = schedulerService;
            _assignmentRepo = assignmentRepo;
            _personRepo = personRepo; // Initialize the Person repository
            _postRepo = postRepo; // Initialize the Post repository

            // Set the DateTimePickers to the current Jalali date
            DateTime currentDate = DateTime.UtcNow;
            var jalaliDate = JalaliDateHelper.ToJalali(currentDate);
            dateTimePickerFrom.Value = JalaliDateHelper.FromJalali(jalaliDate.Year, jalaliDate.Month, jalaliDate.Day);
            dateTimePickerTo.Value = dateTimePickerFrom.Value; // Initialize to the same date
        }

        private void btnGenerateSchedule_Click(object sender, EventArgs e)
        {
            // Clear the previous schedule
            listBoxSchedule.Items.Clear();

            // Convert selected Jalali dates back to Miladi
            DateTime fromDate = JalaliDateHelper.FromJalali(
                JalaliDateHelper.ToJalali(dateTimePickerFrom.Value).Year,
                JalaliDateHelper.ToJalali(dateTimePickerFrom.Value).Month,
                JalaliDateHelper.ToJalali(dateTimePickerFrom.Value).Day
            );

            DateTime toDate = JalaliDateHelper.FromJalali(
                JalaliDateHelper.ToJalali(dateTimePickerTo.Value).Year,
                JalaliDateHelper.ToJalali(dateTimePickerTo.Value).Month,
                JalaliDateHelper.ToJalali(dateTimePickerTo.Value).Day
            );

            // Validate date range
            if (fromDate > toDate)
            {
                MessageBox.Show("The From Date must be earlier than the To Date.", "Invalid Date Range", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Generate the schedule
            List<ScheduleDay> scheduleDays = _schedulerService.GenerateSchedule(fromDate, toDate);

            // Display the generated schedule in Jalali format
            foreach (var day in scheduleDays)
            {
                var jalaliDate = JalaliDateHelper.ToJalaliString(day.Date);
                listBoxSchedule.Items.Add($"Schedule for {jalaliDate}:");

                foreach (var slot in day.ShiftSlots)
                {
                    // Fetch assignments for the slot from the assignment repository
                    List<Assignment> assignments = _assignmentRepo.GetAssignmentsForSlot(slot.Id);

                    // Get the post name using the PostId from the slot
                    var post = _postRepo.GetById(slot.PostId);

                    if (assignments.Count > 0)
                    {
                        foreach (var assignment in assignments)
                        {
                            // Get the person's name using the PersonId from the assignment
                            var person = _personRepo.GetById(assignment.PersonId);
                            string personName = person != null ? $"{person.FirstName} {person.LastName}" : "Unknown";

                            listBoxSchedule.Items.Add($"  Post: {post?.Name ?? "Unknown"}, Start: {slot.Start}, Duration: {slot.DurationHours} hours, Assigned to: {personName}");
                        }
                    }
                    else
                    {
                        listBoxSchedule.Items.Add($"  Post: {post?.Name ?? "Unknown"}, Start: {slot.Start}, Duration: {slot.DurationHours} hours, Unassigned");
                    }
                }

                listBoxSchedule.Items.Add(""); // Add a blank line for better readability
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GuardScheduler.Models;
using GuardScheduler.Services;

namespace GuardScheduler
{
    public partial class ScheduleForm : Form
    {
        private readonly ISchedulerService _schedulerService;

        public ScheduleForm(ISchedulerService schedulerService)
        {
            InitializeComponent();
            _schedulerService = schedulerService;

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
            var jalaliFromDate = JalaliDateHelper.ToJalali(dateTimePickerFrom.Value);
            var jalaliToDate = JalaliDateHelper.ToJalali(dateTimePickerTo.Value);
            DateTime fromDate = JalaliDateHelper.FromJalali(jalaliFromDate.Year, jalaliFromDate.Month, jalaliFromDate.Day);
            DateTime toDate = JalaliDateHelper.FromJalali(jalaliToDate.Year, jalaliToDate.Month, jalaliToDate.Day);

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
                    // Assuming you have a method to get assignments for the slot
                    List<Assignment> assignments = GetAssignmentsForSlot(slot.Id); // Placeholder method

                    if (assignments.Count > 0)
                    {
                        foreach (var assignment in assignments)
                        {
                            listBoxSchedule.Items.Add($"  Post ID: {slot.PostId}, Start: {slot.Start}, Duration: {slot.DurationHours} hours, Assigned to Person ID: {assignment.PersonId}");
                        }
                    }
                    else
                    {
                        listBoxSchedule.Items.Add($"  Post ID: {slot.PostId}, Start: {slot.Start}, Duration: {slot.DurationHours} hours, Unassigned");
                    }
                }

                listBoxSchedule.Items.Add(""); // Add a blank line for better readability
            }
        }

        private List<Assignment> GetAssignmentsForSlot(int shiftSlotId)
        {
            // This method should return the list of assignments for a specific shift slot ID
            // You would typically fetch this from your assignment repository or service
            return new List<Assignment>(); // Placeholder return
        }
    }
}
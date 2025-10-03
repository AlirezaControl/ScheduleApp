using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GuardScheduler.Models;
using GuardScheduler.Services;
using GuardScheduler.Data;
using PersianDateTimeControl;
using MD.PersianDateTime;

namespace GuardScheduler
{
    public partial class ScheduleForm : Form
    {
        private readonly ISchedulerService _schedulerService;
        private readonly IAssignmentRepository _assignmentRepo;
        private readonly IPersonRepository _personRepo;
        private readonly IPostRepository _postRepo;

        public ScheduleForm(ISchedulerService schedulerService, IAssignmentRepository assignmentRepo, IPersonRepository personRepo, IPostRepository postRepo)
        {
            InitializeComponent();
            _schedulerService = schedulerService;
            _assignmentRepo = assignmentRepo;
            _personRepo = personRepo;
            _postRepo = postRepo;

            // PersianDatePicker already shows current Jalali date by default
            dateTimePickerFrom.Value = DateTime.Now;
            dateTimePickerTo.Value = DateTime.Now;
        }

        private void btnGenerateSchedule_Click(object sender, EventArgs e)
        {
            // Clear old items
            listBoxSchedule.Items.Clear();

            // PersianDatePicker.Value gives a DateTime directly (internally converted)
            DateTime fromDate = dateTimePickerFrom.Value.Value;
            DateTime toDate = dateTimePickerTo.Value.Value;

            // Validation
            if (fromDate > toDate)
            {
                MessageBox.Show("تاریخ شروع باید قبل از تاریخ پایان باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Generate the schedule
            List<ScheduleDay> scheduleDays = _schedulerService.GenerateSchedule(fromDate, toDate);

            // Display results (convert back to Persian string for UI)
            foreach (var day in scheduleDays)
            {
                string jalaliDate = new PersianDateTime(day.Date).ToString("yyyy/MM/dd");
                listBoxSchedule.Items.Add($"برنامه روز {jalaliDate}:");

                foreach (var slot in day.ShiftSlots)
                {
                    List<Assignment> assignments = _assignmentRepo.GetAssignmentsForSlot(slot.Id);
                    var post = _postRepo.GetById(slot.PostId);

                    if (assignments.Count > 0)
                    {
                        foreach (var assignment in assignments)
                        {
                            var person = _personRepo.GetById(assignment.PersonId);
                            string personName = person != null ? $"{person.FirstName} {person.LastName}" : "ناشناس";

                            listBoxSchedule.Items.Add($"  پست: {post?.Name ?? "ناشناس"}, شروع: {slot.Start}, مدت: {slot.DurationHours} ساعت، نگهبان: {personName}");
                        }
                    }
                    else
                    {
                        listBoxSchedule.Items.Add($"  پست: {post?.Name ?? "ناشناس"}, شروع: {slot.Start}, مدت: {slot.DurationHours} ساعت، بدون نگهبان");
                    }
                }

                listBoxSchedule.Items.Add(""); // blank line
            }
        }
    }
}
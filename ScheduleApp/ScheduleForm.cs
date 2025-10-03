using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GuardScheduler.Models;
using GuardScheduler.Services;
using GuardScheduler.Data;
using MD.PersianDateTime;

namespace GuardScheduler
{
    public partial class ScheduleForm : Form
    {
        private readonly ISchedulerService _schedulerService;
        private readonly IAssignmentRepository _assignmentRepo;
        private readonly IPersonRepository _personRepo;
        private readonly IPostRepository _postRepo;
        private readonly IScheduleDayRepository _scheduleRepo;

        public ScheduleForm(
            ISchedulerService schedulerService,
            IAssignmentRepository assignmentRepo,
            IPersonRepository personRepo,
            IPostRepository postRepo,
            IScheduleDayRepository scheduleRepo)
        {
            InitializeComponent();
            _schedulerService = schedulerService;
            _assignmentRepo = assignmentRepo;
            _personRepo = personRepo;
            _postRepo = postRepo;
            _scheduleRepo = scheduleRepo;

            dateTimePickerFrom.Value = DateTime.Now;
            dateTimePickerTo.Value = DateTime.Now;

            // Setup DataGridView columns
            dgvSchedule.Columns.Clear();
            dgvSchedule.Columns.Add("Date", "تاریخ");
            dgvSchedule.Columns.Add("Post", "پست");
            dgvSchedule.Columns.Add("Start", "شروع");
            dgvSchedule.Columns.Add("Duration", "مدت (ساعت)");
            dgvSchedule.Columns.Add("Guard", "نگهبان");
        }

        private void btnGenerateSchedule_Click(object sender, EventArgs e)
        {
            dgvSchedule.Rows.Clear();

            DateTime fromDate = dateTimePickerFrom.Value.Value.Date;
            DateTime toDate = dateTimePickerTo.Value.Value.Date;

            if (fromDate > toDate)
            {
                MessageBox.Show("تاریخ شروع باید قبل از تاریخ پایان باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Clear old schedule
            _scheduleRepo.DeleteAll();

            // Generate new schedule
            var newSchedule = _schedulerService.GenerateSchedule(fromDate, toDate);
            if (!newSchedule.Any(d => d.ShiftSlots.Any()))
            {
                MessageBox.Show("هیچ برنامه‌ای تولید نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Save to repository
            _scheduleRepo.SaveScheduleDays(newSchedule);

            // --- Display schedule in DataGridView ---
            foreach (var day in newSchedule)
            {
                string jalaliDate = new PersianDateTime(day.Date).ToString("yyyy/MM/dd");

                foreach (var slot in day.ShiftSlots.OrderBy(s => s.Start))
                {
                    var post = _postRepo.GetById(slot.PostId);

                    // Find all assignments for this slot
                    var slotAssignments = day.Assignments
                                             .Where(a => a.ShiftSlotId == slot.Id)
                                             .ToList();

                    if (slotAssignments.Any())
                    {
                        foreach (var assignment in slotAssignments)
                        {
                            var person = _personRepo.GetById(assignment.PersonId);
                            string personName = person != null
                                ? $"{person.FirstName} {person.LastName}"
                                : "بدون نگهبان";

                            dgvSchedule.Rows.Add(jalaliDate, post?.Name ?? "ناشناس", slot.Start, slot.DurationHours, personName);
                        }
                    }
                    else
                    {
                        dgvSchedule.Rows.Add(jalaliDate, post?.Name ?? "ناشناس", slot.Start, slot.DurationHours, "بدون نگهبان");
                    }
                }
            }
        }

    }
}
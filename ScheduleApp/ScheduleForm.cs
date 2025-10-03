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

            // Create separate grids for categories
            SetupGrid(dgv24HourPosts, "پست‌های ۲۴ ساعته");
            SetupGrid(dgvNegahban, "نگهبان");
            SetupGrid(dgvPasbakhsh, "پاس‌بخش");
            SetupGrid(dgvDezhban, "دژبان");
        }

        private void SetupGrid(DataGridView dgv, string title)
        {
            dgv.Columns.Clear();
            dgv.Columns.Add("Post", "پست");

            // Add columns for each hour of the day
            for (int hour = 0; hour < 24; hour++)
            {
                dgv.Columns.Add($"H{hour}", hour.ToString("00") + ":00");
            }

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;

            // Optional: group box title
            dgv.Parent.Text = title;
        }

        private void btnGenerateSchedule_Click(object sender, EventArgs e)
        {
            dgv24HourPosts.Rows.Clear();
            dgvNegahban.Rows.Clear();
            dgvPasbakhsh.Rows.Clear();
            dgvDezhban.Rows.Clear();

            DateTime fromDate = dateTimePickerFrom.Value.Value.Date;
            DateTime toDate = dateTimePickerTo.Value.Value.Date;

            if (fromDate > toDate)
            {
                MessageBox.Show("تاریخ شروع باید قبل از تاریخ پایان باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _scheduleRepo.DeleteAll();
            var newSchedule = _schedulerService.GenerateSchedule(fromDate, toDate);
            if (!newSchedule.Any(d => d.ShiftSlots.Any()))
            {
                MessageBox.Show("هیچ برنامه‌ای تولید نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _scheduleRepo.SaveScheduleDays(newSchedule);

            // --- Display schedule in different timetables ---
            foreach (var day in newSchedule)
            {
                string jalaliDate = new PersianDateTime(day.Date).ToString("yyyy/MM/dd");

                // Group slots by post
                var posts = day.ShiftSlots.GroupBy(s => s.PostId);
                foreach (var postGroup in posts)
                {
                    var post = _postRepo.GetById(postGroup.Key);
                    if (post == null) continue;

                    // Build row for this post
                    var rowCells = new object[25];
                    rowCells[0] = $"{post.Name} ({jalaliDate})"; // first cell = post name

                    foreach (var slot in postGroup)
                    {
                        var assignment = day.Assignments.FirstOrDefault(a => a.ShiftSlotId == slot.Id);
                        if (assignment != null)
                        {
                            var person = _personRepo.GetById(assignment.PersonId);
                            string personName = person != null ? $"{person.FirstName} {person.LastName}" : "بدون نگهبان";

                            int colIndex = slot.Start.Hours; // map hour → column
                            rowCells[colIndex + 1] = personName;
                        }
                    }

                    // Decide which grid to add row to
                    if (post.AllowedRoles.Contains(Role.Negahban))
                        dgvNegahban.Rows.Add(rowCells);
                    else if (post.AllowedRoles.Contains(Role.PasBakhsh))
                        dgvPasbakhsh.Rows.Add(rowCells);
                    else if (post.AllowedRoles.Contains(Role.Dezhban))
                        dgvDezhban.Rows.Add(rowCells);
                    else
                        dgv24HourPosts.Rows.Add(rowCells);
                }
            }
        }
    }
}

using System;
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

            SetupGrid(dgv24HourPosts);
            SetupGrid(dgvNegahban);
            SetupGrid(dgvPasbakhsh);
            SetupGrid(dgvDezhban);
        }

        private void SetupGrid(DataGridView dgv)
        {
            dgv.Columns.Clear();
            dgv.Columns.Add("Post", "پست");
            for (int hour = 0; hour < 24; hour++)
                dgv.Columns.Add($"H{hour}", hour.ToString("00") + ":00");

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
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

            foreach (var day in newSchedule)
            {
                string jalaliDate = new PersianDateTime(day.Date).ToString("yyyy/MM/dd");

                var posts = day.ShiftSlots.GroupBy(s => s.PostId);
                foreach (var postGroup in posts)
                {
                    var post = _postRepo.GetById(postGroup.Key);
                    if (post == null) continue;

                    var rowCells = new object[25];
                    rowCells[0] = $"{post.Name} ({jalaliDate})";

                    foreach (var slot in postGroup)
                    {
                        var assignment = day.Assignments.FirstOrDefault(a => a.ShiftSlotId == slot.Id);
                        string personName = "بدون نگهبان";
                        if (assignment != null)
                        {
                            var person = _personRepo.GetById(assignment.PersonId);
                            personName = person != null ? $"{person.FirstName} {person.LastName}" : personName;
                        }

                        // Fill all hours based on DurationHours
                        int startHour = slot.Start.Hours;
                        int endHour = Math.Min(startHour + slot.DurationHours, 24); // prevent overflow
                        for (int h = startHour; h < endHour; h++)
                        {
                            rowCells[h + 1] = personName;
                        }
                    }

                    if (post.Name == "نیروی آماده" || (!post.AllowedRoles.Contains(Role.Negahban)
                        && !post.AllowedRoles.Contains(Role.PasBakhsh)
                        && !post.AllowedRoles.Contains(Role.Dezhban)))
                    {
                        dgv24HourPosts.Rows.Add(rowCells);
                    }
                    else if (post.AllowedRoles.Contains(Role.Negahban))
                        dgvNegahban.Rows.Add(rowCells);
                    else if (post.AllowedRoles.Contains(Role.PasBakhsh))
                        dgvPasbakhsh.Rows.Add(rowCells);
                    else if (post.AllowedRoles.Contains(Role.Dezhban))
                        dgvDezhban.Rows.Add(rowCells);
                }
            }
        }

        private void btnOpenPersonList_Click(object sender, EventArgs e)
        {
            using (var personListForm = new PersonListForm(_personRepo))
            {
                personListForm.ShowDialog();
            }
        }
    }
}

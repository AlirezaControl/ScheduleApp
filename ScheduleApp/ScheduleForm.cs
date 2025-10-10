using System;
using System.Linq;
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
        private readonly IScheduleDayRepository _scheduleRepo;
        private string TemplatePath = "Template.docx";

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

        private int GetPostDurationHours(Post post)
        {
            if (post.Name.Contains("نیروی آماده")) return 24;
            if (post.AllowedRoles.Contains(Role.Negahban)) return 2;
            if (post.AllowedRoles.Contains(Role.PasBakhsh)) return 4;
            if (post.AllowedRoles.Contains(Role.Dezhban)) return 8;
            return 1;
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

            var persons = _personRepo.GetAll();
            var posts = _postRepo.GetAll();

            foreach (var day in newSchedule)
            {
                string jalaliDate = new PersianDateTime(day.Date).ToString("yyyy/MM/dd");
                var postsGroups = day.ShiftSlots.GroupBy(s => s.PostId);

                foreach (var postGroup in postsGroups)
                {
                    var post = posts.FirstOrDefault(p => p.Id == postGroup.Key);
                    if (post == null) continue;

                    var rowCells = new object[25];
                    rowCells[0] = $"{post.Name} ({jalaliDate})";

                    foreach (var slot in postGroup)
                    {
                        var assignment = day.Assignments.FirstOrDefault(a => a.ShiftSlotId == slot.Id);
                        if (assignment != null)
                        {
                            var person = persons.FirstOrDefault(p => p.Id == assignment.PersonId);
                            string personName = person != null ? $"{person.FirstName} {person.LastName}" : "بدون نگهبان";

                            int duration = GetPostDurationHours(post);
                            for (int h = slot.Start.Hours; h < Math.Min(slot.Start.Hours + duration, 24); h++)
                                rowCells[h + 1] = personName;
                        }
                    }

                    if (post.Name.Contains("نیروی آماده"))
                        dgv24HourPosts.Rows.Add(rowCells);
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
            using var personListForm = new PersonListForm(_personRepo);
            personListForm.ShowDialog();
        }

        private void btnExportWord_Click(object sender, EventArgs e)
        {
            DateTime fromDate = dateTimePickerFrom.Value.Value.Date;
            DateTime toDate = dateTimePickerTo.Value.Value.Date;

            var scheduleDays = _scheduleRepo.GetScheduleDays(fromDate, toDate).ToList();
            var persons = _personRepo.GetAll().ToList();
            var posts = _postRepo.GetAll().ToList();

            if (!scheduleDays.Any())
            {
                MessageBox.Show("هیچ برنامه‌ای برای خروجی یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var dlg = new SaveFileDialog
            {
                Filter = "Word Document|*.docx",
                FileName = $"لوحه نگهبانی_{fromDate:yyyyMMdd}_{toDate:yyyyMMdd}.docx"
            };

            if (dlg.ShowDialog() != DialogResult.OK) return;

            try
            {
                var exporter = new WordTemplateExporter();
                exporter.Export(TemplatePath, dlg.FileName, scheduleDays.First(), posts, persons);

                MessageBox.Show("لوحه نگهبانی با موفقیت ایجاد شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در ایجاد فایل: " + ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

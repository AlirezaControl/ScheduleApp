using System;
using System.Drawing;
using System.Windows.Forms;
using GuardScheduler.Models;
using GuardScheduler.Services;
using GuardScheduler.Data;
using System.Linq;

namespace GuardScheduler.UI
{
    public partial class DetailedScheduleForm : Form
    {
        private readonly ISchedulerService _schedulerService;
        private readonly IAssignmentRepository _assignmentRepo;
        private readonly IPersonRepository _personRepo;
        private readonly IPostRepository _postRepo;
        private readonly IScheduleDayRepository _scheduleRepo;

        private DateTime _currentDateNow;

        public DetailedScheduleForm(
            ISchedulerService schedulerService,
            IAssignmentRepository assignmentRepo,
            IPersonRepository personRepo,
            IPostRepository postRepo,
            IScheduleDayRepository scheduleRepo)
        {
            _schedulerService = schedulerService;
            _assignmentRepo = assignmentRepo;
            _personRepo = personRepo;
            _postRepo = postRepo;
            _scheduleRepo = scheduleRepo;

            InitializeComponent();

            _currentDateNow = DateTime.Now;

            dateTimePickerFrom.Value = _currentDateNow;
            dateTimePickerTo.Value = _currentDateNow;
        }

        private void BtnGenerateSchedule_Click(object sender, EventArgs e)
        {
            DateTime fromDate = dateTimePickerFrom.Value.Value.Date;
            DateTime toDate = dateTimePickerTo.Value.Value.Date;

            if (fromDate > toDate)
            {
                MessageBox.Show("تاریخ شروع باید قبل از تاریخ پایان باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _scheduleRepo.DeleteAll();
            _assignmentRepo.DeleteAll();
            var newSchedule = _schedulerService.GenerateSchedule(fromDate, toDate);

            if (!newSchedule.Any(d => d.ShiftSlots.Any()))
            {
                MessageBox.Show("هیچ برنامه‌ای تولید نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _scheduleRepo.SaveScheduleDays(newSchedule);

            var persons = _personRepo.GetAll();
            var posts = _postRepo.GetAll();

            var day = newSchedule.FirstOrDefault();
            if (day == null) return;

            ClearAllPlaceholders();

            var keyValues = ScheduleMapper.MapAssignmentsToTemplate(day, posts, persons);

            foreach (var kv in keyValues)
            {
                var lbl = GetLabelByTagFromAllTables(kv.Key);
                if (lbl != null)
                    lbl.Text = kv.Value;
            }
        }

        private void ClearAllPlaceholders()
        {
            foreach (var t in new[] { tablePasbakhsh, tableDezhbanMorning, tableDezhbanEvening, tableDezhbanCombined, tableBottom })
            {
                foreach (Control c in t.Controls)
                {
                    if (c is Label lbl && lbl.Tag is string)
                    {
                        lbl.Text = "";
                        lbl.BackColor = Color.White;
                    }
                }
            }
        }

        private Label GetLabelByTagFromAllTables(string tag)
        {
            foreach (var t in new[] { tablePasbakhsh, tableDezhbanMorning, tableDezhbanEvening, tableDezhbanCombined, tableBottom })
            {
                foreach (Control c in t.Controls)
                {
                    if (c is Label lbl && lbl.Tag is string lblTag && lblTag == tag)
                        return lbl;
                }
            }
            return null;
        }

        private void BtnOpenPersonList_Click(object sender, EventArgs e)
        {
            using var personListForm = new PersonListForm(_personRepo);
            personListForm.ShowDialog();
        }

        private void BtnExportWord_Click(object sender, EventArgs e)
        {
            DateTime fromDate = dateTimePickerFrom.Value.Value.Date;
            DateTime toDate = dateTimePickerTo.Value.Value.Date;

            var scheduleDays = _scheduleRepo.GetScheduleDays(fromDate, toDate);
            var persons = _personRepo.GetAll();
            var posts = _postRepo.GetAll();

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
                exporter.Export("Template.docx", dlg.FileName, scheduleDays.First(), posts, persons);

                MessageBox.Show("لوحه نگهبانی با موفقیت ایجاد شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در ایجاد فایل: " + ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
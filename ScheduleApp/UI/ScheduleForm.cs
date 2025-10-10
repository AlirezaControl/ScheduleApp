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

            LoadCurrentAssignments();
        }

        private void LoadCurrentAssignments()
        {
            try
            {
                var assignments = _assignmentRepo.GetAssignmentsByDate(_currentDateNow);
                var persons = _personRepo.GetAll();

                foreach (var assignment in assignments)
                {
                    var person = persons.FirstOrDefault(p => p.Id == assignment.PersonId);
                    if (person != null)
                    {
                        // Find and update the corresponding label
                        // You'll need to implement this mapping based on your shift slot structure
                        UpdateAssignmentLabel(assignment.ShiftSlotId, $"{person.FirstName} {person.LastName}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در بارگذاری انتساب‌ها: {ex.Message}", "خطا",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UpdateAssignmentLabel(int shiftSlotId, string personName)
        {
            // Implement mapping between shiftSlotId and label tags
            // This depends on your data structure
            var labelTag = GetLabelTagFromShiftSlotId(shiftSlotId);
            var label = GetLabelByTagFromAllTables(labelTag);

            if (label != null)
            {
                label.Text = personName;
                label.BackColor = Color.LightGreen;
                label.ForeColor = Color.DarkGreen;
                label.Font = new Font(label.Font, FontStyle.Bold);
            }
        }

        private string GetLabelTagFromShiftSlotId(int shiftSlotId)
        {
            // Implement this mapping based on your shift slot structure
            // This is a simplified example - adjust according to your data
            return $"slot_{shiftSlotId}";
        }

        private void BtnGenerateSchedule_Click(object sender, EventArgs e)
        {
            DateTime fromDate = dateTimePickerFrom.Value.Value.Date;
            DateTime toDate = dateTimePickerTo.Value.Value.Date;

            if (fromDate > toDate)
            {
                MessageBox.Show("تاریخ شروع باید قبل از تاریخ پایان باشد.", "خطا",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _scheduleRepo.DeleteAll();
                _assignmentRepo.DeleteAll();
                var newSchedule = _schedulerService.GenerateSchedule(fromDate, toDate);

                if (!newSchedule.Any(d => d.ShiftSlots.Any()))
                {
                    MessageBox.Show("هیچ برنامه‌ای تولید نشد.", "خطا",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    {
                        lbl.Text = kv.Value;
                        if (!string.IsNullOrEmpty(kv.Value))
                        {
                            lbl.BackColor = Color.LightGreen;
                            lbl.ForeColor = Color.DarkGreen;
                            lbl.Font = new Font(lbl.Font, FontStyle.Bold);
                        }
                    }
                }

                MessageBox.Show("برنامه با موفقیت تولید شد.", "موفقیت",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در تولید برنامه: {ex.Message}", "خطا",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        lbl.ForeColor = SystemColors.ControlText;
                        lbl.Font = new Font(lbl.Font, FontStyle.Regular);
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
            // Refresh assignments after person list changes
            LoadCurrentAssignments();
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
                MessageBox.Show("هیچ برنامه‌ای برای خروجی یافت نشد.", "خطا",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                MessageBox.Show("لوحه نگهبانی با موفقیت ایجاد شد.", "موفقیت",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در ایجاد فایل: " + ex.Message, "خطا",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadCurrentAssignments();
            MessageBox.Show("برنامه با موفقیت بروزرسانی شد.", "بروزرسانی",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
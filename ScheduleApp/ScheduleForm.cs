// File: DetailedScheduleForm.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GuardScheduler.Models;
using GuardScheduler.Services;
using GuardScheduler.Data;
using MD.PersianDateTime;
using PersianDateTimeControl;

namespace GuardScheduler
{
    public partial class DetailedScheduleForm : Form
    {
        private readonly ISchedulerService _schedulerService;
        private readonly IAssignmentRepository _assignmentRepo;
        private readonly IPersonRepository _personRepo;
        private readonly IPostRepository _postRepo;
        private readonly IScheduleDayRepository _scheduleRepo;

        private PersianDatePicker dateTimePickerFrom;
        private PersianDatePicker dateTimePickerTo;

        private Button btnGenerateSchedule;
        private Button btnExportWord;
        private Button btnOpenPersonList;

        private Label lblTitle;

        private TableLayoutPanel tablePasbakhsh;
        private TableLayoutPanel tableDezhbanMorning;
        private TableLayoutPanel tableDezhbanEvening;
        private TableLayoutPanel tableDezhbanCombined;
        private TableLayoutPanel tableBottom;

        private Panel buttonsPanel;
        private Panel tablesPanel;
        private Panel bottomPanel;

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

        private void InitializeComponent()
        {
            this.Text = "لوحه نگهبانی مرکز فاوا - جدول تفصیلی";
            this.ClientSize = new Size(960, 720);
            this.Font = new Font("Tahoma", 10);
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 245);

            var mainLayout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                RowCount = 4,
                ColumnCount = 1,
                Padding = new Padding(15),
                AutoScroll = true,
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 65));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 75));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25));

            this.Controls.Add(mainLayout);

            lblTitle = new Label()
            {
                Text = "لوحه نگهبانی مرکز فاوا",
                Font = new Font("Tahoma", 22, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 144, 255),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            mainLayout.Controls.Add(lblTitle, 0, 0);

            buttonsPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(10),
                Height = 50,
            };
            mainLayout.Controls.Add(buttonsPanel, 0, 1);

            dateTimePickerFrom = new PersianDatePicker()
            {
                Location = new Point(860, 12),
                Width = 110,
                RightToLeft = RightToLeft.Yes,
                Font = new Font("Tahoma", 10),
            };
            buttonsPanel.Controls.Add(dateTimePickerFrom);

            var lblFrom = new Label()
            {
                Text = "از تاریخ:",
                Location = new Point(780, 15),
                AutoSize = true,
                Font = new Font("Tahoma", 11, FontStyle.Regular),
                ForeColor = Color.FromArgb(50, 50, 50),
            };
            buttonsPanel.Controls.Add(lblFrom);

            dateTimePickerTo = new PersianDatePicker()
            {
                Location = new Point(660, 12),
                Width = 110,
                RightToLeft = RightToLeft.Yes,
                Font = new Font("Tahoma", 10),
            };
            buttonsPanel.Controls.Add(dateTimePickerTo);

            var lblTo = new Label()
            {
                Text = "تا تاریخ:",
                Location = new Point(590, 15),
                AutoSize = true,
                Font = new Font("Tahoma", 11, FontStyle.Regular),
                ForeColor = Color.FromArgb(50, 50, 50),
            };
            buttonsPanel.Controls.Add(lblTo);

            btnGenerateSchedule = new Button()
            {
                Text = "تولید برنامه",
                BackColor = Color.FromArgb(30, 144, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(120, 32),
                Location = new Point(470, 10),
                Font = new Font("Tahoma", 11, FontStyle.Bold),
                Cursor = Cursors.Hand,
            };
            btnGenerateSchedule.FlatAppearance.BorderSize = 0;
            btnGenerateSchedule.Click += BtnGenerateSchedule_Click;
            buttonsPanel.Controls.Add(btnGenerateSchedule);

            btnExportWord = new Button()
            {
                Text = "خروجی Word",
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(120, 32),
                Location = new Point(340, 10),
                Font = new Font("Tahoma", 11, FontStyle.Bold),
                Cursor = Cursors.Hand,
            };
            btnExportWord.FlatAppearance.BorderSize = 0;
            btnExportWord.Click += BtnExportWord_Click;
            buttonsPanel.Controls.Add(btnExportWord);

            btnOpenPersonList = new Button()
            {
                Text = "مدیریت نفرات",
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(120, 32),
                Location = new Point(210, 10),
                Font = new Font("Tahoma", 11, FontStyle.Bold),
                Cursor = Cursors.Hand,
            };
            btnOpenPersonList.FlatAppearance.BorderSize = 0;
            btnOpenPersonList.Click += BtnOpenPersonList_Click;
            buttonsPanel.Controls.Add(btnOpenPersonList);

            tablesPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(5),
                BackColor = Color.White,
            };
            mainLayout.Controls.Add(tablesPanel, 0, 2);

            InitializeTables();

            int y = 10;
            foreach (var t in new[] { tablePasbakhsh, tableDezhbanMorning, tableDezhbanEvening, tableDezhbanCombined })
            {
                t.Location = new Point(10, y);
                tablesPanel.Controls.Add(t);
                y += t.Height + 20;
            }

            bottomPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(5),
                BackColor = Color.White,
                AutoScroll = true,
            };
            mainLayout.Controls.Add(bottomPanel, 0, 3);

            InitializeBottomTable();
            bottomPanel.Controls.Add(tableBottom);
        }

        // ------------------------- TABLE INIT -------------------------
        private void InitializeTables()
        {
            Color headerBackColor = Color.FromArgb(30, 144, 255);
            Color headerForeColor = Color.White;
            Font headerFont = new Font("Tahoma", 11, FontStyle.Bold);
            Font cellFont = new Font("Tahoma", 10);

            tablePasbakhsh = CreateTable(6, 2, 920, 70);
            string[] pasbakhshHeaders = { "زمان شیفت", "06-14", "14-18", "18-22", "22-02", "02-06" };
            for (int i = 0; i < 6; i++)
                AddCellToTable(tablePasbakhsh, pasbakhshHeaders[i], i, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tablePasbakhsh, "نام پاسبخش", 0, 1, Color.FromArgb(240, 248, 255), Color.Black, cellFont, false);
            for (int i = 1; i < 6; i++)
                tablePasbakhsh.Controls.Add(CreateSelectableLabel($"p{i}"), i, 1);

            tableDezhbanMorning = CreateTable(6, 2, 920, 70);
            string[] dezhbanMorningHeaders = { "زمان شیفت", "08-10", "10-12", "12-14", "14-16", "16-18" };
            for (int i = 0; i < 6; i++)
                AddCellToTable(tableDezhbanMorning, dezhbanMorningHeaders[i], i, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tableDezhbanMorning, "نام دژبان", 0, 1, Color.FromArgb(240, 248, 255), Color.Black, cellFont, false);
            for (int i = 1; i < 6; i++)
                tableDezhbanMorning.Controls.Add(CreateSelectableLabel($"d{i}"), i, 1);

            tableDezhbanEvening = CreateTable(6, 2, 920, 70);
            string[] dezhbanEveningHeaders = { "زمان شیفت", "20-22", "22-00", "00-02", "02-04", "04-06" };
            for (int i = 0; i < 6; i++)
                AddCellToTable(tableDezhbanEvening, dezhbanEveningHeaders[i], i, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tableDezhbanEvening, "نام دژبان", 0, 1, Color.FromArgb(240, 248, 255), Color.Black, cellFont, false);
            for (int i = 1; i < 6; i++)
                tableDezhbanEvening.Controls.Add(CreateSelectableLabel($"d{i + 6}"), i, 1);

            tableDezhbanCombined = CreateTable(7, 7, 920, 230);
            string[] combinedHeaders = { "ساعت", "ضلع دژبانی (صبح)", "ضلع دژبانی (عصر)", "ضلع شرقی (صبح)", "ضلع شرقی (عصر)", "ضلع غربی (صبح)", "ضلع غربی (عصر)" };
            for (int i = 0; i < 7; i++)
                AddCellToTable(tableDezhbanCombined, combinedHeaders[i], i, 0, headerBackColor, headerForeColor, headerFont, true);

            string[] hours = { "06-08", "08-10", "10-12", "12-02", "02-04", "04-06" };
            for (int row = 1; row <= 6; row++)
            {
                AddCellToTable(tableDezhbanCombined, hours[row - 1], 0, row, Color.FromArgb(240, 248, 255), Color.Black, cellFont, false);

                tableDezhbanCombined.Controls.Add(CreateSelectableLabel($"nd{row}"), 1, row);
                tableDezhbanCombined.Controls.Add(CreateSelectableLabel($"nd{row + 6}"), 2, row);
                tableDezhbanCombined.Controls.Add(CreateSelectableLabel($"sh{row}"), 3, row);
                tableDezhbanCombined.Controls.Add(CreateSelectableLabel($"sh{row + 6}"), 4, row);
                tableDezhbanCombined.Controls.Add(CreateSelectableLabel($"gh{row}"), 5, row);
                tableDezhbanCombined.Controls.Add(CreateSelectableLabel($"gh{row + 6}"), 6, row);
            }
        }

        private TableLayoutPanel CreateTable(int cols, int rows, int width, int height)
        {
            var t = new TableLayoutPanel()
            {
                ColumnCount = cols,
                RowCount = rows,
                Width = width,
                Height = height,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                BackColor = Color.White
            };
            for (int i = 0; i < cols; i++)
                t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / cols));
            for (int i = 0; i < rows; i++)
                t.RowStyles.Add(new RowStyle(SizeType.Absolute, height / rows));
            return t;
        }

        private void InitializeBottomTable()
        {
            tableBottom = CreateTable(6, 6, 920, 180);
            Font headerFont = new Font("Tahoma", 10, FontStyle.Bold);
            Font cellFont = new Font("Tahoma", 10);
            Color headerBackColor = Color.FromArgb(30, 144, 255);
            Color headerForeColor = Color.White;

            AddCellToTable(tableBottom, "مسئول بازداشتگاه", 0, 0, headerBackColor, headerForeColor, headerFont, true);
            tableBottom.Controls.Add(CreateSelectableLabel("na"), 3, 0);

            AddCellToTable(tableBottom, "راننده آماده", 0, 1, headerBackColor, headerForeColor, headerFont, true);
            tableBottom.Controls.Add(CreateSelectableLabel("R"), 1, 1);

            AddCellToTable(tableBottom, "مسئول نظافت: حمام/سلف/سرویس", 0, 2, headerBackColor, headerForeColor, headerFont, true);
            tableBottom.Controls.Add(CreateSelectableLabel("mn"), 1, 2);

            AddCellToTable(tableBottom, "افسر قرارگاه", 2, 3, headerBackColor, headerForeColor, headerFont, true);
            tableBottom.Controls.Add(CreateSelectableLabel("agh"), 3, 3);

            AddCellToTable(tableBottom, "شیفت آشپزخانه", 2, 4, headerBackColor, headerForeColor, headerFont, true);
            tableBottom.Controls.Add(CreateSelectableLabel("a1/a2"), 3, 4);
        }

        private Label CreateSelectableLabel(string tag)
        {
            var lbl = new Label()
            {
                Text = "",
                Tag = tag,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand,
                Font = new Font("Tahoma", 10, FontStyle.Regular),
                Margin = new Padding(1),
            };
            lbl.MouseEnter += (s, e) => { if (lbl.BackColor == Color.White) lbl.BackColor = Color.FromArgb(220, 235, 255); };
            lbl.MouseLeave += (s, e) => { if (lbl.BackColor == Color.FromArgb(220, 235, 255)) lbl.BackColor = Color.White; };
            lbl.DoubleClick += Label_DoubleClick;
            return lbl;
        }

        private void Label_DoubleClick(object sender, EventArgs e)
        {
            if (!(sender is Label lbl)) return;

            var allowedPersons = GetAllowedPersonsForLabel(lbl.Tag?.ToString());

            using var popup = new Form()
            {
                Size = new Size(250, 300),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedToolWindow,
                Text = "انتخاب نفر",
            };
            var listBox = new ListBox()
            {
                Dock = DockStyle.Fill,
                DataSource = allowedPersons,
                DisplayMember = "DisplayName",
                ValueMember = "Id"
            };
            popup.Controls.Add(listBox);

            var btn = new Button()
            {
                Text = "انتخاب",
                Dock = DockStyle.Bottom,
                Height = 35
            };
            btn.Click += (s, args) =>
            {
                if (listBox.SelectedItem is AllowedPerson selected)
                {
                    lbl.Text = selected.DisplayName;
                    popup.Close();
                }
            };
            popup.Controls.Add(btn);

            popup.ShowDialog();
        }

        // ------------------------- ALLOWED PERSONS LOGIC -------------------------
        private List<AllowedPerson> GetAllowedPersonsForLabel(string labelTag)
        {
            if (string.IsNullOrEmpty(labelTag)) return new List<AllowedPerson>();

            var allPersons = _personRepo.GetAll().Where(p => p.Available).ToList();

            // Map labelTag to role(s)
            List<Role> allowedRoles = labelTag switch
            {
                var t when t.StartsWith("p") => new List<Role> { Role.PasBakhsh },
                var t when t.StartsWith("d") => new List<Role> { Role.Dezhban },
                var t when t.StartsWith("nd") => new List<Role> { Role.Negahban },
                var t when t.StartsWith("sh") => new List<Role> { Role.Negahban },
                var t when t.StartsWith("gh") => new List<Role> { Role.Negahban },
                var t when t.StartsWith("R") => new List<Role> { Role.Ranandeh },
                var t when t.StartsWith("mn") => new List<Role> { Role.GoruhB },
                var t when t.StartsWith("agh") => new List<Role> { Role.AfsarGharargah },
                var t when t.StartsWith("a1") || t.StartsWith("a2") => new List<Role> { Role.KomakAshpaz },
                _ => new List<Role>()
            };

            return allPersons
                .Where(p => allowedRoles.Any(r =>
                    p.PrimaryRole == r || p.SecondaryRole == r || (p.AllowedPostNames != null && p.AllowedPostNames.Contains(r.ToString()))
                ))
                .OrderBy(p => p.RotationOrder)
                .Select(p => new AllowedPerson { Id = p.Id, DisplayName = $"{p.FirstName} {p.LastName}" })
                .ToList();
        }

        public class AllowedPerson
        {
            public int Id { get; set; }
            public string DisplayName { get; set; }
        }

        // ------------------------- HELPER -------------------------
        private void AddCellToTable(TableLayoutPanel table, string text, int col, int row, Color backColor, Color foreColor, Font font, bool bold)
        {
            var lbl = new Label()
            {
                Text = text,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                BackColor = backColor,
                ForeColor = foreColor,
                Font = bold ? new Font(font, FontStyle.Bold) : font,
                Margin = new Padding(1),
            };
            table.Controls.Add(lbl, col, row);
        }

        // ------------------------- BUTTON LOGIC -------------------------
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

            var persons = _personRepo.GetAll().ToList();
            var posts = _postRepo.GetAll().ToList();

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

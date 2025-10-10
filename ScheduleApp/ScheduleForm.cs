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

            // Initialize PersianDatePickers with current date
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

            // Main layout panel
            var mainLayout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                RowCount = 4,
                ColumnCount = 1,
                Padding = new Padding(15),
                AutoScroll = true,
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 65)); // Title
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50)); // DatePickers + buttons
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 75));  // Tables
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25));  // Bottom table

            this.Controls.Add(mainLayout);

            // Title Label
            lblTitle = new Label()
            {
                Text = "لوحه نگهبانی مرکز فاوا",
                Font = new Font("Tahoma", 22, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 144, 255),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            mainLayout.Controls.Add(lblTitle, 0, 0);

            // Buttons and DatePickers Panel
            buttonsPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(10),
                Height = 50,
            };
            mainLayout.Controls.Add(buttonsPanel, 0, 1);

            // PersianDatePickers
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

            // Buttons with refined style
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

            // Tables Panel
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

            // Bottom Panel for bottom table without texts at bottom
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

        private void InitializeTables()
        {
            // Common styles
            Color headerBackColor = Color.FromArgb(30, 144, 255);
            Color headerForeColor = Color.White;
            Font headerFont = new Font("Tahoma", 11, FontStyle.Bold);
            Font cellFont = new Font("Tahoma", 10);

            // Pasbakhsh table (6 columns, 2 rows)
            tablePasbakhsh = new TableLayoutPanel()
            {
                ColumnCount = 6,
                RowCount = 2,
                Width = 920,
                Height = 70,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                BackColor = Color.White,
            };
            for (int i = 0; i < 6; i++)
                tablePasbakhsh.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 6));
            tablePasbakhsh.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
            tablePasbakhsh.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));

            AddCellToTable(tablePasbakhsh, "زمان شیفت", 0, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tablePasbakhsh, "06-14", 1, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tablePasbakhsh, "14-18", 2, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tablePasbakhsh, "18-22", 3, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tablePasbakhsh, "22-02", 4, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tablePasbakhsh, "02-06", 5, 0, headerBackColor, headerForeColor, headerFont, true);

            AddCellToTable(tablePasbakhsh, "نام پاسبخش", 0, 1, Color.FromArgb(240, 248, 255), Color.Black, cellFont, false);

            for (int i = 1; i < 6; i++)
            {
                var lbl = CreateSelectableLabel($"p{i}");
                tablePasbakhsh.Controls.Add(lbl, i, 1);
            }

            // Dezhban Morning table (6 columns, 2 rows)
            tableDezhbanMorning = new TableLayoutPanel()
            {
                ColumnCount = 6,
                RowCount = 2,
                Width = 920,
                Height = 70,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                BackColor = Color.White,
            };
            for (int i = 0; i < 6; i++)
                tableDezhbanMorning.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 6));
            tableDezhbanMorning.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
            tableDezhbanMorning.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));

            AddCellToTable(tableDezhbanMorning, "زمان شیفت", 0, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tableDezhbanMorning, "08-10", 1, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tableDezhbanMorning, "10-12", 2, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tableDezhbanMorning, "12-14", 3, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tableDezhbanMorning, "14-16", 4, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tableDezhbanMorning, "16-18", 5, 0, headerBackColor, headerForeColor, headerFont, true);

            AddCellToTable(tableDezhbanMorning, "نام دژبان", 0, 1, Color.FromArgb(240, 248, 255), Color.Black, cellFont, false);

            for (int i = 1; i < 6; i++)
            {
                var lbl = CreateSelectableLabel($"d{i}");
                tableDezhbanMorning.Controls.Add(lbl, i, 1);
            }

            // Dezhban Evening table (6 columns, 2 rows)
            tableDezhbanEvening = new TableLayoutPanel()
            {
                ColumnCount = 6,
                RowCount = 2,
                Width = 920,
                Height = 70,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                BackColor = Color.White,
            };
            for (int i = 0; i < 6; i++)
                tableDezhbanEvening.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 6));
            tableDezhbanEvening.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
            tableDezhbanEvening.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));

            AddCellToTable(tableDezhbanEvening, "زمان شیفت", 0, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tableDezhbanEvening, "20-22", 1, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tableDezhbanEvening, "22-00", 2, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tableDezhbanEvening, "00-02", 3, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tableDezhbanEvening, "02-04", 4, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tableDezhbanEvening, "04-06", 5, 0, headerBackColor, headerForeColor, headerFont, true);

            AddCellToTable(tableDezhbanEvening, "نام دژبان", 0, 1, Color.FromArgb(240, 248, 255), Color.Black, cellFont, false);

            for (int i = 7; i <= 12; i++)
            {
                var lbl = CreateSelectableLabel($"d{i}");
                tableDezhbanEvening.Controls.Add(lbl, i - 6, 1);
            }

            // Combined Dezhban/Shor and Gharb table (7 columns, 7 rows)
            tableDezhbanCombined = new TableLayoutPanel()
            {
                ColumnCount = 7,
                RowCount = 7,
                Width = 920,
                Height = 230,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                BackColor = Color.White,
            };
            for (int i = 0; i < 7; i++)
                tableDezhbanCombined.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 7));
            tableDezhbanCombined.RowStyles.Add(new RowStyle(SizeType.Absolute, 35)); // header
            for (int i = 1; i < 7; i++)
                tableDezhbanCombined.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

            AddCellToTable(tableDezhbanCombined, "ساعت", 0, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tableDezhbanCombined, "ضلع دژبانی (صبح)", 1, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tableDezhbanCombined, "ضلع دژبانی (عصر)", 2, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tableDezhbanCombined, "ضلع شرقی (صبح)", 3, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tableDezhbanCombined, "ضلع شرقی (عصر)", 4, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tableDezhbanCombined, "ضلع غربی (صبح)", 5, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tableDezhbanCombined, "ضلع غربی (عصر)", 6, 0, headerBackColor, headerForeColor, headerFont, true);

            string[] hours = { "06-08", "08-10", "10-12", "12-02", "02-04", "04-06" };
            for (int row = 1; row <= 6; row++)
            {
                AddCellToTable(tableDezhbanCombined, hours[row - 1], 0, row, Color.FromArgb(240, 248, 255), Color.Black, cellFont, false);

                var nd1 = CreateSelectableLabel($"nd{row}");
                tableDezhbanCombined.Controls.Add(nd1, 1, row);

                var nd2 = CreateSelectableLabel($"nd{row + 6}");
                tableDezhbanCombined.Controls.Add(nd2, 2, row);

                var sh1 = CreateSelectableLabel($"sh{row}");
                tableDezhbanCombined.Controls.Add(sh1, 3, row);

                var sh2 = CreateSelectableLabel($"sh{row + 6}");
                tableDezhbanCombined.Controls.Add(sh2, 4, row);

                var gh1 = CreateSelectableLabel($"gh{row}");
                tableDezhbanCombined.Controls.Add(gh1, 5, row);

                var gh2 = CreateSelectableLabel($"gh{row + 6}");
                tableDezhbanCombined.Controls.Add(gh2, 6, row);
            }
        }

        private void InitializeBottomTable()
        {
            // Bottom table with multiple rows and columns for other roles
            tableBottom = new TableLayoutPanel()
            {
                ColumnCount = 6,
                RowCount = 6,
                Width = 920,
                Height = 180,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                BackColor = Color.White,
            };
            for (int i = 0; i < 6; i++)
                tableBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 6));
            for (int i = 0; i < 6; i++)
                tableBottom.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

            Font headerFont = new Font("Tahoma", 10, FontStyle.Bold);
            Font cellFont = new Font("Tahoma", 10);

            Color headerBackColor = Color.FromArgb(30, 144, 255);
            Color headerForeColor = Color.White;

            AddCellToTable(tableBottom, "مسئول بازداشتگاه", 0, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tableBottom, "", 1, 0, Color.White, Color.Black, cellFont, false);
            AddCellToTable(tableBottom, "نیروی آماده", 2, 0, headerBackColor, headerForeColor, headerFont, true);
            var naLabel = CreateSelectableLabel("na");
            tableBottom.Controls.Add(naLabel, 3, 0);
            AddCellToTable(tableBottom, "", 4, 0, Color.White, Color.Black, cellFont, false);
            AddCellToTable(tableBottom, "", 5, 0, Color.White, Color.Black, cellFont, false);

            AddCellToTable(tableBottom, "راننده آماده", 0, 1, headerBackColor, headerForeColor, headerFont, true);
            var rLabel = CreateSelectableLabel("R");
            tableBottom.Controls.Add(rLabel, 1, 1);
            AddCellToTable(tableBottom, "نظافت آسایشگاه", 2, 1, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tableBottom, "", 3, 1, Color.White, Color.Black, cellFont, false);
            AddCellToTable(tableBottom, "", 4, 1, Color.White, Color.Black, cellFont, false);
            AddCellToTable(tableBottom, "", 5, 1, Color.White, Color.Black, cellFont, false);

            AddCellToTable(tableBottom, "مسئول نظافت: حمام/سلف/سرویس", 0, 2, headerBackColor, headerForeColor, headerFont, true);
            var mnLabel = CreateSelectableLabel("mn");
            tableBottom.Controls.Add(mnLabel, 1, 2);
            AddCellToTable(tableBottom, "افراد بازداشتی", 2, 2, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tableBottom, "", 3, 2, Color.White, Color.Black, cellFont, false);
            AddCellToTable(tableBottom, "", 4, 2, Color.White, Color.Black, cellFont, false);
            AddCellToTable(tableBottom, "", 5, 2, Color.White, Color.Black, cellFont, false);

            AddCellToTable(tableBottom, "", 0, 3, Color.White, Color.Black, cellFont, false);
            AddCellToTable(tableBottom, "", 1, 3, Color.White, Color.Black, cellFont, false);
            AddCellToTable(tableBottom, "افسر قرارگاه", 2, 3, headerBackColor, headerForeColor, headerFont, true);
            var aghLabel = CreateSelectableLabel("agh");
            tableBottom.Controls.Add(aghLabel, 3, 3);
            AddCellToTable(tableBottom, "", 4, 3, Color.White, Color.Black, cellFont, false);
            AddCellToTable(tableBottom, "", 5, 3, Color.White, Color.Black, cellFont, false);

            AddCellToTable(tableBottom, "شیفت آتش نشانی", 0, 4, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tableBottom, "", 1, 4, Color.White, Color.Black, cellFont, false);
            AddCellToTable(tableBottom, "شیفت آشپزخانه", 2, 4, headerBackColor, headerForeColor, headerFont, true);
            var a12Label = CreateSelectableLabel("a1/a2");
            tableBottom.Controls.Add(a12Label, 3, 4);
            AddCellToTable(tableBottom, "", 4, 4, Color.White, Color.Black, cellFont, false);
            AddCellToTable(tableBottom, "", 5, 4, Color.White, Color.Black, cellFont, false);

            AddCellToTable(tableBottom, "اتاق افسر جانشین و افسر نگهبانی", 0, 5, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tableBottom, "", 1, 5, Color.White, Color.Black, cellFont, false);
            AddCellToTable(tableBottom, "مسئول پاسدارخانه", 2, 5, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tableBottom, "", 3, 5, Color.White, Color.Black, cellFont, false);
            AddCellToTable(tableBottom, "", 4, 5, Color.White, Color.Black, cellFont, false);
            AddCellToTable(tableBottom, "", 5, 5, Color.White, Color.Black, cellFont, false);
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
            lbl.Click += Label_Click;
            lbl.MouseEnter += (s, e) => { if (lbl.BackColor == Color.White) lbl.BackColor = Color.FromArgb(220, 235, 255); };
            lbl.MouseLeave += (s, e) => { if (lbl.BackColor == Color.FromArgb(220, 235, 255)) lbl.BackColor = Color.White; };
            return lbl;
        }

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

        private void Label_Click(object sender, EventArgs e)
        {
            if (sender is Label lbl)
            {
                // Toggle selection color for demonstration
                if (lbl.BackColor == Color.White)
                    lbl.BackColor = Color.FromArgb(198, 239, 206);
                else
                    lbl.BackColor = Color.White;

                // Here you can implement a dialog or dropdown to select/change the person assigned to this cell
                // For simplicity, just toggling color on click
            }
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

            // Populate labels by keys
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
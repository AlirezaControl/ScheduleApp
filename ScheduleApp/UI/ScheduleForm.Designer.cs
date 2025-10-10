using System.Drawing;
using System.Windows.Forms;
using PersianDateTimeControl;

namespace GuardScheduler.UI
{
    partial class DetailedScheduleForm
    {
        private PersianDatePicker dateTimePickerFrom;
        private PersianDatePicker dateTimePickerTo;
        private Button btnGenerateSchedule;
        private Button btnExportWord;
        private Button btnOpenPersonList;
        private Button btnRefresh;
        private Label lblTitle;
        private TableLayoutPanel tablePasbakhsh;
        private TableLayoutPanel tableDezhbanMorning;
        private TableLayoutPanel tableDezhbanEvening;
        private TableLayoutPanel tableDezhbanCombined;
        private TableLayoutPanel tableBottom;
        private Panel buttonsPanel;
        private Panel tablesPanel;
        private Panel bottomPanel;

        private void InitializeComponent()
        {
            this.Text = "لوحه نگهبانی مرکز فاوا - جدول تفصیلی";
            this.ClientSize = new Size(1000, 800);
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
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 70));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 30));

            this.Controls.Add(mainLayout);

            // Enhanced Title
            lblTitle = new Label()
            {
                Text = "لوحه نگهبانی مرکز فاوا",
                Font = new Font("Tahoma", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 144, 255),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent,
            };
            mainLayout.Controls.Add(lblTitle, 0, 0);

            // Enhanced Buttons Panel
            buttonsPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(248, 249, 250),
                Padding = new Padding(15),
                Height = 60,
            };
            mainLayout.Controls.Add(buttonsPanel, 0, 1);

            // Date controls with better styling
            dateTimePickerFrom = new PersianDatePicker()
            {
                Location = new Point(860, 15),
                Width = 120,
                Height = 32,
                RightToLeft = RightToLeft.Yes,
                Font = new Font("Tahoma", 10),
                BorderStyle = BorderStyle.FixedSingle,
            };
            buttonsPanel.Controls.Add(dateTimePickerFrom);

            var lblFrom = new Label()
            {
                Text = "از تاریخ:",
                Location = new Point(780, 20),
                AutoSize = true,
                Font = new Font("Tahoma", 11, FontStyle.Regular),
                ForeColor = Color.FromArgb(50, 50, 50),
            };
            buttonsPanel.Controls.Add(lblFrom);

            dateTimePickerTo = new PersianDatePicker()
            {
                Location = new Point(650, 15),
                Width = 120,
                Height = 32,
                RightToLeft = RightToLeft.Yes,
                Font = new Font("Tahoma", 10),
                BorderStyle = BorderStyle.FixedSingle,
            };
            buttonsPanel.Controls.Add(dateTimePickerTo);

            var lblTo = new Label()
            {
                Text = "تا تاریخ:",
                Location = new Point(580, 20),
                AutoSize = true,
                Font = new Font("Tahoma", 11, FontStyle.Regular),
                ForeColor = Color.FromArgb(50, 50, 50),
            };
            buttonsPanel.Controls.Add(lblTo);

            // Enhanced Buttons with modern styling
            btnGenerateSchedule = CreateModernButton("تولید برنامه", Color.FromArgb(30, 144, 255), 470);
            btnGenerateSchedule.Click += BtnGenerateSchedule_Click;
            buttonsPanel.Controls.Add(btnGenerateSchedule);

            btnExportWord = CreateModernButton("خروجی Word", Color.FromArgb(46, 204, 113), 340);
            btnExportWord.Click += BtnExportWord_Click;
            buttonsPanel.Controls.Add(btnExportWord);

            btnOpenPersonList = CreateModernButton("مدیریت نفرات", Color.FromArgb(149, 165, 166), 210);
            btnOpenPersonList.Click += BtnOpenPersonList_Click;
            buttonsPanel.Controls.Add(btnOpenPersonList);

            btnRefresh = CreateModernButton("بروزرسانی", Color.FromArgb(155, 89, 182), 80);
            btnRefresh.Click += BtnRefresh_Click;
            buttonsPanel.Controls.Add(btnRefresh);

            tablesPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
            };
            mainLayout.Controls.Add(tablesPanel, 0, 2);

            InitializeTables();

            int y = 10;
            foreach (var t in new[] { tablePasbakhsh, tableDezhbanMorning, tableDezhbanEvening, tableDezhbanCombined })
            {
                t.Location = new Point(10, y);
                tablesPanel.Controls.Add(t);
                y += t.Height + 15;
            }

            bottomPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                BackColor = Color.White,
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle,
            };
            mainLayout.Controls.Add(bottomPanel, 0, 3);

            InitializeBottomTable();
            tableBottom.Location = new Point(10, 10);
            bottomPanel.Controls.Add(tableBottom);
        }

        private Button CreateModernButton(string text, Color backColor, int x)
        {
            return new Button()
            {
                Text = text,
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(120, 36),
                Location = new Point(x, 12),
                Font = new Font("Tahoma", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
            };
        }
    }
}
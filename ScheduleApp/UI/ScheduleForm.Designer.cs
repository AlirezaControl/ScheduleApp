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
    }
}
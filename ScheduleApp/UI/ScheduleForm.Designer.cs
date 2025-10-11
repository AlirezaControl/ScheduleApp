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
        private Panel headerPanel;
        private Panel controlsPanel;
        private Panel tablesPanel;
        private Panel bottomPanel;
        private SplitContainer mainSplitContainer;

        private void InitializeComponent()
        {
            this.Text = "لوحه نگهبانی مرکز فاوا - جدول تفصیلی";
            this.ClientSize = new Size(1400, 900);
            this.Font = new Font("Tahoma", 10);
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 250, 252);
            this.Padding = new Padding(0);

            // Main container with scrolling
            var mainContainer = new Panel()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(240, 242, 245),
            };
            this.Controls.Add(mainContainer);

            // Content panel that holds everything with fixed width
            var contentPanel = new Panel()
            {
                Width = 1350,
                AutoSize = true,
                Padding = new Padding(20),
                BackColor = Color.Transparent,
            };
            mainContainer.Controls.Add(contentPanel);

            // Header Panel - Modern Design
            headerPanel = new Panel()
            {
                Width = contentPanel.Width - 40,
                Height = 140,
                BackColor = Color.FromArgb(30, 144, 255),
                Padding = new Padding(40, 25, 40, 25),
                Location = new Point(0, 0),
            };
            contentPanel.Controls.Add(headerPanel);

            // Modern title with gradient effect simulation
            lblTitle = new Label()
            {
                Text = "لوحه نگهبانی مرکز فاوا",
                Font = new Font("Tahoma", 34, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent,
                Height = 60,
            };
            headerPanel.Controls.Add(lblTitle);

            var lblSubtitle = new Label()
            {
                Text = "سیستم برنامه ریزی و مدیریت نگهبانی",
                Font = new Font("Tahoma", 14, FontStyle.Regular),
                ForeColor = Color.FromArgb(220, 240, 255),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopCenter,
                BackColor = Color.Transparent,
                Height = 30,
                Location = new Point(0, 70),
            };
            headerPanel.Controls.Add(lblSubtitle);

            // Controls Panel - Modern Card Design
            controlsPanel = new Panel()
            {
                Width = contentPanel.Width - 40,
                Height = 160,
                BackColor = Color.White,
                Padding = new Padding(30, 20, 30, 20),
                Location = new Point(0, headerPanel.Bottom + 15),
            };
            contentPanel.Controls.Add(controlsPanel);

            // Add shadow effect to controls panel
            controlsPanel.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, controlsPanel.ClientRectangle,
                    Color.FromArgb(220, 220, 220), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(220, 220, 220), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(220, 220, 220), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(220, 220, 220), 1, ButtonBorderStyle.Solid);
            };

            // Date Controls Section - Top Row
            var dateContainer = new Panel()
            {
                Width = controlsPanel.Width - 60,
                Height = 60,
                BackColor = Color.Transparent,
                Location = new Point(0, 0),
            };
            controlsPanel.Controls.Add(dateContainer);

            // Date labels and pickers with proper spacing
            var lblFrom = new Label()
            {
                Text = "از تاریخ:",
                Font = new Font("Tahoma", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = true,
                Location = new Point(dateContainer.Width - 450, 15),
            };
            dateContainer.Controls.Add(lblFrom);

            dateTimePickerFrom = new PersianDatePicker()
            {
                Width = 200,
                Height = 50,
                RightToLeft = RightToLeft.Yes,
                Font = new Font("Tahoma", 12, FontStyle.Regular),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Location = new Point(dateContainer.Width - 650, 5),
            };
            dateContainer.Controls.Add(dateTimePickerFrom);

            var lblTo = new Label()
            {
                Text = "تا تاریخ:",
                Font = new Font("Tahoma", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = true,
                Location = new Point(dateContainer.Width - 900, 15),
            };
            dateContainer.Controls.Add(lblTo);

            dateTimePickerTo = new PersianDatePicker()
            {
                Width = 200,
                Height = 50,
                RightToLeft = RightToLeft.Yes,
                Font = new Font("Tahoma", 12, FontStyle.Regular),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Location = new Point(dateContainer.Width - 1100, 5),
            };
            dateContainer.Controls.Add(dateTimePickerTo);

            // Buttons Section - Bottom Row
            var buttonsContainer = new Panel()
            {
                Width = controlsPanel.Width - 60,
                Height = 60,
                BackColor = Color.Transparent,
                Location = new Point(0, 70),
            };
            controlsPanel.Controls.Add(buttonsContainer);

            // Modern buttons with equal spacing
            btnGenerateSchedule = CreateModernButton("تولید برنامه", Color.FromArgb(30, 144, 255), 50);
            btnGenerateSchedule.Click += BtnGenerateSchedule_Click;
            btnGenerateSchedule.Location = new Point(buttonsContainer.Width - 300, 5);
            buttonsContainer.Controls.Add(btnGenerateSchedule);

            btnExportWord = CreateModernButton("خروجی Word", Color.FromArgb(46, 204, 113), 50);
            btnExportWord.Click += BtnExportWord_Click;
            btnExportWord.Location = new Point(buttonsContainer.Width - 550, 5);
            buttonsContainer.Controls.Add(btnExportWord);

            btnOpenPersonList = CreateModernButton("مدیریت نفرات", Color.FromArgb(149, 165, 166), 50);
            btnOpenPersonList.Click += BtnOpenPersonList_Click;
            btnOpenPersonList.Location = new Point(buttonsContainer.Width - 800, 5);
            buttonsContainer.Controls.Add(btnOpenPersonList);

            btnRefresh = CreateModernButton("بروزرسانی", Color.FromArgb(155, 89, 182), 50);
            btnRefresh.Click += BtnRefresh_Click;
            btnRefresh.Location = new Point(buttonsContainer.Width - 1050, 5);
            buttonsContainer.Controls.Add(btnRefresh);

            // Main Tables Panel - Scrollable
            tablesPanel = new Panel()
            {
                Width = contentPanel.Width - 40,
                Height = 400,
                BackColor = Color.White,
                Padding = new Padding(25),
                Location = new Point(0, controlsPanel.Bottom + 15),
                AutoScroll = true,
            };
            contentPanel.Controls.Add(tablesPanel);

            // Add border to tables panel
            tablesPanel.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, tablesPanel.ClientRectangle,
                    Color.FromArgb(220, 220, 220), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(220, 220, 220), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(220, 220, 220), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(220, 220, 220), 1, ButtonBorderStyle.Solid);
            };

            InitializeTables();

            // Arrange tables vertically with proper spacing
            int y = 20;
            foreach (var t in new[] { tablePasbakhsh, tableDezhbanMorning, tableDezhbanEvening, tableDezhbanCombined })
            {
                t.Location = new Point((tablesPanel.ClientSize.Width - t.Width) / 2, y);
                tablesPanel.Controls.Add(t);
                y += t.Height + 25;
            }

            // Bottom Panel - Scrollable
            bottomPanel = new Panel()
            {
                Width = contentPanel.Width - 40,
                Height = 300,
                BackColor = Color.White,
                Padding = new Padding(25),
                Location = new Point(0, tablesPanel.Bottom + 15),
                AutoScroll = true,
            };
            contentPanel.Controls.Add(bottomPanel);

            // Add border to bottom panel
            bottomPanel.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, bottomPanel.ClientRectangle,
                    Color.FromArgb(220, 220, 220), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(220, 220, 220), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(220, 220, 220), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(220, 220, 220), 1, ButtonBorderStyle.Solid);
            };

            InitializeBottomTable();
            tableBottom.Location = new Point((bottomPanel.ClientSize.Width - tableBottom.Width) / 2, 20);
            bottomPanel.Controls.Add(tableBottom);

            // Set content panel height based on content
            contentPanel.Height = bottomPanel.Bottom + 20;

            // Center the content panel horizontally
            mainContainer.Resize += (s, e) =>
            {
                contentPanel.Left = (mainContainer.ClientSize.Width - contentPanel.Width) / 2;
            };
            contentPanel.Left = (mainContainer.ClientSize.Width - contentPanel.Width) / 2;
        }

        private Button CreateModernButton(string text, Color backColor, int height)
        {
            var btn = new Button()
            {
                Text = text,
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Tahoma", 11, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Width = 180,
                Height = height,
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Light(backColor, 0.15f);
            btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(backColor, 0.15f);

            // Add modern hover effects
            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = ControlPaint.Light(backColor, 0.1f);
            };
            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = backColor;
            };

            return btn;
        }

        // Helper method to center controls in their parent
        private void CenterControlInParent(Control control, Control parent)
        {
            control.Left = (parent.ClientSize.Width - control.Width) / 2;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GuardScheduler.Models;

namespace GuardScheduler.UI
{
    public partial class ModernSelectionPopup : Form
    {
        public AllowedPerson SelectedPerson { get; private set; }
        public bool ClearAssignment { get; private set; }

        private List<AllowedPerson> _allowedPersons;
        private Assignment _currentAssignment;
        private string _currentText;
        private ListBox _listBox;

        public ModernSelectionPopup(List<AllowedPerson> allowedPersons, Assignment currentAssignment, string currentText)
        {
            _allowedPersons = allowedPersons ?? new List<AllowedPerson>();
            _currentAssignment = currentAssignment;
            _currentText = currentText ?? "";

            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Text = "انتخاب نفر";
            this.BackColor = Color.White;
            this.Padding = new Padding(10);
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 4,
                ColumnCount = 1,
                BackColor = Color.Transparent
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

            // Header
            var headerLabel = new Label
            {
                Text = "لیست افراد مجاز",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Tahoma", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 144, 255),
                BackColor = Color.Transparent
            };
            mainLayout.Controls.Add(headerLabel, 0, 0);

            // List Box - now stored as a field
            _listBox = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Tahoma", 10),
                BorderStyle = BorderStyle.FixedSingle,
                DrawMode = DrawMode.OwnerDrawFixed,
                ItemHeight = 35
            };
            _listBox.DrawItem += ListBox_DrawItem;
            _listBox.DoubleClick += (s, e) => SelectCurrentItem();
            mainLayout.Controls.Add(_listBox, 0, 1);

            // Current Assignment Panel
            var currentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 248, 255),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10)
            };

            var currentLabel = new Label
            {
                Text = $"انتخاب فعلی: {_currentText}",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Tahoma", 10, FontStyle.Regular),
                ForeColor = Color.DarkSlateGray,
                AutoSize = false,
                Height = 30
            };
            currentPanel.Controls.Add(currentLabel);

            var clearButton = new Button
            {
                Text = "حذف انتساب",
                Size = new Size(100, 30),
                Location = new Point(10, 5),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            clearButton.FlatAppearance.BorderSize = 0;
            clearButton.Click += (s, e) =>
            {
                ClearAssignment = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            // Only show clear button if there's a current assignment
            if (!string.IsNullOrEmpty(_currentText))
            {
                currentPanel.Controls.Add(clearButton);
            }

            mainLayout.Controls.Add(currentPanel, 0, 2);

            // Buttons Panel
            var buttonsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            var selectButton = new Button
            {
                Text = "انتخاب",
                Size = new Size(100, 35),
                Location = new Point(150, 5),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Tahoma", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            selectButton.FlatAppearance.BorderSize = 0;
            selectButton.Click += (s, e) => SelectCurrentItem();
            buttonsPanel.Controls.Add(selectButton);

            var cancelButton = new Button
            {
                Text = "انصراف",
                Size = new Size(100, 35),
                Location = new Point(20, 5),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Tahoma", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            cancelButton.FlatAppearance.BorderSize = 0;
            cancelButton.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };
            buttonsPanel.Controls.Add(cancelButton);

            mainLayout.Controls.Add(buttonsPanel, 0, 3);

            this.Controls.Add(mainLayout);
        }

        private void LoadData()
        {
            if (_listBox == null) return;

            try
            {
                // Clear existing items
                _listBox.Items.Clear();

                if (_allowedPersons == null || !_allowedPersons.Any())
                {
                    // Show message when no persons available
                    _listBox.Items.Add("هیچ فرد مجازی برای این پست وجود ندارد");
                    _listBox.Enabled = false;
                    return;
                }

                // Add all allowed persons to the list
                foreach (var person in _allowedPersons)
                {
                    _listBox.Items.Add(person);
                }

                _listBox.Enabled = true;

                // Auto-select the first item if available
                if (_listBox.Items.Count > 0)
                {
                    _listBox.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                _listBox.Items.Add($"خطا در بارگذاری داده‌ها: {ex.Message}");
                _listBox.Enabled = false;
            }
        }

        private void SelectCurrentItem()
        {
            if (_listBox.SelectedItem is AllowedPerson selected)
            {
                SelectedPerson = selected;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("لطفاً یک نفر را انتخاب کنید.", "هشدار",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            var listBox = (ListBox)sender;

            // Handle the case where we show error messages or empty state
            if (listBox.Items[e.Index] is string)
            {
                e.DrawBackground();
                var text = listBox.Items[e.Index].ToString();
                e.Graphics.DrawString(text, new Font("Tahoma", 10, FontStyle.Regular),
                    Brushes.Gray, e.Bounds, StringFormat.GenericDefault);
                e.DrawFocusRectangle();
                return;
            }

            if (!(listBox.Items[e.Index] is AllowedPerson person)) return;

            e.DrawBackground();

            // Alternate background colors for better readability
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(220, 235, 255)), e.Bounds);
            }
            else
            {
                var backColor = e.Index % 2 == 0 ? Color.White : Color.FromArgb(250, 250, 250);
                e.Graphics.FillRectangle(new SolidBrush(backColor), e.Bounds);
            }

            // Draw person name
            var nameRect = new Rectangle(e.Bounds.X + 10, e.Bounds.Y + 5, e.Bounds.Width - 20, 18);
            using (var nameFont = new Font("Tahoma", 10, FontStyle.Regular))
            using (var nameBrush = new SolidBrush(Color.Black))
            {
                e.Graphics.DrawString(person.DisplayName, nameFont, nameBrush, nameRect,
                    new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });
            }

            // Draw role
            var roleRect = new Rectangle(e.Bounds.X + 10, e.Bounds.Y + 23, e.Bounds.Width - 20, 12);
            using (var roleFont = new Font("Tahoma", 8, FontStyle.Italic))
            using (var roleBrush = new SolidBrush(Color.Gray))
            {
                e.Graphics.DrawString(person.Role, roleFont, roleBrush, roleRect,
                    new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });
            }

            e.DrawFocusRectangle();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            // Ensure the listbox is properly focused and visible
            _listBox?.Focus();
        }
    }
}
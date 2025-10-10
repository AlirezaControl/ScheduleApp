using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GuardScheduler.Data;
using GuardScheduler.Models;

namespace GuardScheduler.UI
{
    partial class DetailedScheduleForm
    {
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

        private List<AllowedPerson> GetAllowedPersonsForLabel(string labelTag)
        {
            if (string.IsNullOrEmpty(labelTag)) return new List<AllowedPerson>();

            var allPersons = _personRepo.GetAll().Where(p => p.Available).ToList();

            // Get today's assignments to exclude those already on duty
            var todayAssignments = _assignmentRepo.GetAssignmentsByDate(_currentDateNow)
                ?.Select(a => a.PersonId)
                .Distinct()
                .ToHashSet() ?? new HashSet<int>();

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
                .Where(p =>
                    !todayAssignments.Contains(p.Id) &&
                    allowedRoles.Any(r =>
                        p.PrimaryRole == r || p.SecondaryRole == r ||
                        (p.AllowedPostNames != null && p.AllowedPostNames.Contains(r.ToString()))
                    ))
                .OrderBy(p => p.RotationOrder)
                .Select(p => new AllowedPerson { Id = p.Id, DisplayName = $"{p.FirstName} {p.LastName}" })
                .ToList();
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
    }

    public class AllowedPerson
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
    }
}
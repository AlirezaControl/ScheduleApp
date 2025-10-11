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

            lbl.MouseEnter += (s, e) =>
            {
                if (lbl.BackColor == Color.White || lbl.BackColor == Color.LightGreen)
                    lbl.BackColor = Color.FromArgb(220, 235, 255);
                lbl.BorderStyle = BorderStyle.Fixed3D;
            };

            lbl.MouseLeave += (s, e) =>
            {
                if (lbl.BackColor == Color.FromArgb(220, 235, 255))
                    lbl.BackColor = string.IsNullOrEmpty(lbl.Text) ? Color.White : Color.LightGreen;
                lbl.BorderStyle = BorderStyle.FixedSingle;
            };

            lbl.DoubleClick += Label_DoubleClick;
            return lbl;
        }

        private void Label_DoubleClick(object sender, EventArgs e)
        {
            if (!(sender is Label lbl)) return;

            var labelTag = lbl.Tag?.ToString();
            var allowedPersons = GetAllowedPersonsForLabel(labelTag);
            var currentAssignment = GetCurrentAssignmentForLabel(labelTag);

            using var popup = new ModernSelectionPopup(allowedPersons, currentAssignment, lbl.Text);
            if (popup.ShowDialog() == DialogResult.OK)
            {
                if (popup.SelectedPerson != null)
                {
                    // Assign person
                    UpdateAssignment(labelTag, popup.SelectedPerson.Id, popup.SelectedPerson.DisplayName);
                    lbl.Text = popup.SelectedPerson.DisplayName;
                    lbl.BackColor = Color.LightGreen;
                    lbl.ForeColor = Color.DarkGreen;
                    lbl.Font = new Font(lbl.Font, FontStyle.Bold);
                }
                else if (popup.ClearAssignment)
                {
                    // Clear assignment
                    ClearAssignment(labelTag);
                    lbl.Text = "";
                    lbl.BackColor = Color.White;
                    lbl.ForeColor = SystemColors.ControlText;
                    lbl.Font = new Font(lbl.Font, FontStyle.Regular);
                }
            }
        }

        private void UpdateAssignment(string labelTag, int personId, string displayName)
        {
            try
            {
                var shiftSlotId = GetShiftSlotIdFromTag(labelTag);
                if (shiftSlotId == -1) return;

                // Remove ALL of today's assignments for the selected person first
                var personsTodayAssignments = _assignmentRepo.GetAssignmentsForPersonOnDate(personId, _currentDateNow);
                foreach (var assignment in personsTodayAssignments)
                {
                    _assignmentRepo.Delete(assignment.Id);
                }

                // Remove existing assignment for this slot (in case someone else was assigned)
                var existingAssignmentsForSlot = _assignmentRepo.GetAssignmentsForSlot(shiftSlotId);
                foreach (var assignment in existingAssignmentsForSlot)
                {
                    _assignmentRepo.Delete(assignment.Id);
                }

                // Add new assignment
                var newAssignment = new Assignment
                {
                    ShiftSlotId = shiftSlotId,
                    PersonId = personId,
                    AssignedAt = _currentDateNow
                };

                _assignmentRepo.Insert(newAssignment);

                // Update UI immediately - refresh all labels to reflect the changes
                RefreshAllLabelsForDate(_currentDateNow);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در بروزرسانی انتساب: {ex.Message}", "خطا",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearAssignment(string labelTag)
        {
            try
            {
                var shiftSlotId = GetShiftSlotIdFromTag(labelTag);
                if (shiftSlotId == -1) return;

                var existingAssignments = _assignmentRepo.GetAssignmentsForSlot(shiftSlotId);
                foreach (var assignment in existingAssignments)
                {
                    _assignmentRepo.Delete(assignment.Id);
                }

                RefreshRelatedLabels(labelTag, 0, "");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در حذف انتساب: {ex.Message}", "خطا",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshAllLabelsForDate(DateTime date)
        {
            // Get all assignments for the current date
            var todaysAssignments = _assignmentRepo.GetAssignmentsByDate(date);

            // Create a lookup for quick access: shiftSlotId -> person display name
            var assignmentLookup = new Dictionary<int, string>();
            foreach (var assignment in todaysAssignments)
            {
                var person = _personRepo.GetById(assignment.PersonId);
                if (person != null)
                {
                    assignmentLookup[assignment.ShiftSlotId] = $"{person.FirstName} {person.LastName}";
                }
            }

            // Refresh all tables
            foreach (var table in new[] { tablePasbakhsh, tableDezhbanMorning, tableDezhbanEvening, tableDezhbanCombined, tableBottom })
            {
                foreach (Control control in table.Controls)
                {
                    if (control is Label label && label.Tag != null)
                    {
                        var labelTag = label.Tag.ToString();
                        var shiftSlotId = GetShiftSlotIdFromTag(labelTag);

                        // Only update if we have a valid shift slot ID
                        if (shiftSlotId != -1)
                        {
                            if (assignmentLookup.ContainsKey(shiftSlotId))
                            {
                                // This slot has an assignment
                                label.Text = assignmentLookup[shiftSlotId];
                                label.BackColor = Color.LightGreen;
                                label.ForeColor = Color.DarkGreen;
                                label.Font = new Font(label.Font, FontStyle.Bold);
                            }
                            else
                            {
                                // This slot is empty
                                label.Text = "";
                                label.BackColor = Color.White;
                                label.ForeColor = SystemColors.ControlText;
                                label.Font = new Font(label.Font, FontStyle.Regular);
                            }
                        }
                        // If shiftSlotId is -1, don't modify the label - it might be a static label
                    }
                }
            }
        }

        private void RefreshRelatedLabels(string labelTag, int personId, string displayName)
        {
            // Refresh all labels with the same tag to maintain consistency
            foreach (var table in new[] { tablePasbakhsh, tableDezhbanMorning, tableDezhbanEvening, tableDezhbanCombined, tableBottom })
            {
                foreach (Control control in table.Controls)
                {
                    if (control is Label label && label.Tag?.ToString() == labelTag)
                    {
                        if (personId > 0)
                        {
                            label.Text = displayName;
                            label.BackColor = Color.LightGreen;
                            label.ForeColor = Color.DarkGreen;
                            label.Font = new Font(label.Font, FontStyle.Bold);
                        }
                        else
                        {
                            label.Text = "";
                            label.BackColor = Color.White;
                            label.ForeColor = SystemColors.ControlText;
                            label.Font = new Font(label.Font, FontStyle.Regular);
                        }
                    }
                }
            }
        }

        private int GetShiftSlotIdFromTag(string labelTag)
        {
            if (string.IsNullOrEmpty(labelTag)) return -1;

            var scheduleDay = _scheduleRepo.GetScheduleDays(_currentDateNow, _currentDateNow).FirstOrDefault();
            if (scheduleDay == null) return -1;

            var (postId, slotIndex, startTime) = GetPostInfoFromTag(labelTag);
            if (postId == -1) return -1;

            // Find shift slot that matches post, date, and slot characteristics
            var shiftSlot = scheduleDay.ShiftSlots
                .FirstOrDefault(ss =>
                    ss.PostId == postId &&
                    ss.Date.Date == _currentDateNow.Date &&
                    IsMatchingSlot(ss, slotIndex, startTime));

            return shiftSlot?.Id ?? -1;
        }

        private bool IsMatchingSlot(ShiftSlot slot, int expectedSlotIndex, TimeSpan? expectedStart)
        {
            // If we have an expected slot index, match by that
            if (expectedSlotIndex != -1 && slot.SlotIndex == expectedSlotIndex)
                return true;

            // If we have an expected start time, match by that
            if (expectedStart.HasValue && slot.Start == expectedStart.Value)
                return true;

            // For slots without specific index/start, just match by post
            return expectedSlotIndex == -1 && !expectedStart.HasValue;
        }

        private (int postId, int slotIndex, TimeSpan? startTime) GetPostInfoFromTag(string labelTag)
        {
            if (string.IsNullOrEmpty(labelTag)) return (-1, -1, null);

            // Map label tags to post IDs and slot information
            return labelTag switch
            {
                // Pasbakhsh slots
                "p1" => (56, 1, new TimeSpan(6, 0, 0)),   // 06-14
                "p2" => (56, 2, new TimeSpan(14, 0, 0)),  // 14-18  
                "p3" => (56, 3, new TimeSpan(18, 0, 0)),  // 18-22
                "p4" => (56, 4, new TimeSpan(22, 0, 0)),  // 22-02
                "p5" => (56, 5, new TimeSpan(2, 0, 0)),   // 02-06

                // Dezhban morning slots
                "d1" => (57, 1, new TimeSpan(8, 0, 0)),   // 08-10
                "d2" => (57, 2, new TimeSpan(10, 0, 0)),  // 10-12
                "d3" => (57, 3, new TimeSpan(12, 0, 0)),  // 12-14
                "d4" => (57, 4, new TimeSpan(14, 0, 0)),  // 14-16
                "d5" => (57, 5, new TimeSpan(16, 0, 0)),  // 16-18

                // Dezhban evening slots  
                "d6" => (57, 6, new TimeSpan(20, 0, 0)),  // 20-22
                "d7" => (57, 7, new TimeSpan(22, 0, 0)),  // 22-00
                "d8" => (57, 8, new TimeSpan(0, 0, 0)),   // 00-02
                "d9" => (57, 9, new TimeSpan(2, 0, 0)),   // 02-04
                "d10" => (57, 10, new TimeSpan(4, 0, 0)), // 04-06

                // Negahban Daz (morning)
                "nd1" => (61, 1, new TimeSpan(6, 0, 0)),   // 06-08
                "nd2" => (61, 2, new TimeSpan(8, 0, 0)),   // 08-10
                "nd3" => (61, 3, new TimeSpan(10, 0, 0)),  // 10-12
                "nd4" => (61, 4, new TimeSpan(12, 0, 0)),  // 12-14
                "nd5" => (61, 5, new TimeSpan(14, 0, 0)),  // 14-16
                "nd6" => (61, 6, new TimeSpan(16, 0, 0)),  // 16-18

                // Negahban Daz (evening)
                "nd7" => (61, 7, new TimeSpan(18, 0, 0)),  // 18-20
                "nd8" => (61, 8, new TimeSpan(20, 0, 0)),  // 20-22
                "nd9" => (61, 9, new TimeSpan(22, 0, 0)),  // 22-00
                "nd10" => (61, 10, new TimeSpan(0, 0, 0)), // 00-02
                "nd11" => (61, 11, new TimeSpan(2, 0, 0)), // 02-04
                "nd12" => (61, 12, new TimeSpan(4, 0, 0)), // 04-06

                // Negahban Sharghi (morning)
                "sh1" => (60, 1, new TimeSpan(6, 0, 0)),
                "sh2" => (60, 2, new TimeSpan(8, 0, 0)),
                "sh3" => (60, 3, new TimeSpan(10, 0, 0)),
                "sh4" => (60, 4, new TimeSpan(12, 0, 0)),
                "sh5" => (60, 5, new TimeSpan(14, 0, 0)),
                "sh6" => (60, 6, new TimeSpan(16, 0, 0)),

                // Negahban Sharghi (evening)
                "sh7" => (60, 7, new TimeSpan(18, 0, 0)),
                "sh8" => (60, 8, new TimeSpan(20, 0, 0)),
                "sh9" => (60, 9, new TimeSpan(22, 0, 0)),
                "sh10" => (60, 10, new TimeSpan(0, 0, 0)),
                "sh11" => (60, 11, new TimeSpan(2, 0, 0)),
                "sh12" => (60, 12, new TimeSpan(4, 0, 0)),

                // Negahban Gharbi (morning)
                "gh1" => (62, 1, new TimeSpan(6, 0, 0)),
                "gh2" => (62, 2, new TimeSpan(8, 0, 0)),
                "gh3" => (62, 3, new TimeSpan(10, 0, 0)),
                "gh4" => (62, 4, new TimeSpan(12, 0, 0)),
                "gh5" => (62, 5, new TimeSpan(14, 0, 0)),
                "gh6" => (62, 6, new TimeSpan(16, 0, 0)),

                // Negahban Gharbi (evening)
                "gh7" => (62, 7, new TimeSpan(18, 0, 0)),
                "gh8" => (62, 8, new TimeSpan(20, 0, 0)),
                "gh9" => (62, 9, new TimeSpan(22, 0, 0)),
                "gh10" => (62, 10, new TimeSpan(0, 0, 0)),
                "gh11" => (62, 11, new TimeSpan(2, 0, 0)),
                "gh12" => (62, 12, new TimeSpan(4, 0, 0)),

                // Other posts (single slot per day)
                "R" => (58, 1, new TimeSpan()),      // Ranandeh
                "mn" => (64, 1, new TimeSpan()),     // مسئول نظافت
                "agh" => (65, 1, new TimeSpan()),    // افسر قرارگاه
                "a1" => (59, 1, new TimeSpan()),     // کمک آشپز 1
                "a2" => (59, 2, new TimeSpan()),     // کمک آشپز 2

                _ => (-1, -1, new TimeSpan())
            };
        }

        private Assignment GetCurrentAssignmentForLabel(string labelTag)
        {
            var shiftSlotId = GetShiftSlotIdFromTag(labelTag);
            if (shiftSlotId == -1) return null;

            return _assignmentRepo.GetAssignmentsForSlot(shiftSlotId)
                .FirstOrDefault(a => a.AssignedAt == null || a.AssignedAt == _currentDateNow.Date);
        }

        private List<AllowedPerson> GetAllowedPersonsForLabel(string labelTag)
        {
            if (string.IsNullOrEmpty(labelTag)) return new List<AllowedPerson>();

            var allPersons = _personRepo.GetAll().Where(p => p.Available).ToList();
            var (postId, _, _) = GetPostInfoFromTag(labelTag);

            if (postId == -1) return new List<AllowedPerson>();

            // Get the post to check allowed roles
            var post = _postRepo.GetAll().FirstOrDefault(p => p.Id == postId);
            if (post == null) return new List<AllowedPerson>();

            // Use the computed property to get allowed roles as list
            var allowedRoles = post.AllowedRolesList;

            // Special case: For Western Guard posts (postId 62), also include MoafAzRazm persons
            if (postId == 62) // Negahban Gharbi (Western Guard)
            {
                // Create a new list that includes both Negahban and MoafAzRazm roles
                var expandedRoles = new List<Role>(allowedRoles);
                if (!expandedRoles.Contains(Role.MoafAzRazm))
                {
                    expandedRoles.Add(Role.MoafAzRazm);
                }
                allowedRoles = expandedRoles;
            }

            // Get today's assignments to exclude those already on duty
            var todayAssignments = _assignmentRepo.GetAssignmentsByDate(_currentDateNow)
                ?.Select(a => a.PersonId)
                .Distinct()
                .ToHashSet() ?? new HashSet<int>();

            var selected = allPersons
                .Where(p =>
                    !todayAssignments.Contains(p.Id) &&
                    IsPersonAllowedForPost(p, allowedRoles))
                .OrderBy(p => p.RotationOrder)
                .Select(p => new AllowedPerson
                {
                    Id = p.Id,
                    DisplayName = $"{p.FirstName} {p.LastName}",
                    Role = p.PrimaryRole.ToString()
                })
                .ToList();

            return selected;
        }

        private bool IsPersonAllowedForPost(Person person, List<Role> allowedRoles)
        {
            // First check if person has primary or secondary role that matches allowed roles
            if (allowedRoles.Contains(person.PrimaryRole) ||
                (person.SecondaryRole.HasValue && allowedRoles.Contains(person.SecondaryRole.Value)))
            {
                return true;
            }

            // Then check AllowedPostNames if available
            if (!string.IsNullOrEmpty(person.AllowedPostNames))
            {
                var allowedPostNames = person.AllowedPostNames.Split(',')
                    .Select(name => name.Trim())
                    .ToList();

                foreach (var allowedRole in allowedRoles)
                {
                    if (allowedPostNames.Contains(allowedRole.ToString()))
                        return true;
                }
            }

            return false;
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
        public string Role { get; set; }
    }
}
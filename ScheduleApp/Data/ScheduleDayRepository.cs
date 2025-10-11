using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using GuardScheduler.Models;

namespace GuardScheduler.Data
{
    public class ScheduleDayRepository : SQLiteRepositoryBase, IScheduleDayRepository
    {
        public ScheduleDayRepository(string connectionString) : base(connectionString) { }

        /// <summary>
        /// Save multiple ScheduleDay objects to the Schedules table.
        /// Each ShiftSlot + optional Assignment is a row.
        /// </summary>
        public void SaveScheduleDays(List<ScheduleDay> scheduleDays)
        {
            using (var conn = GetConnection())
            {
                conn.Open();

                foreach (var day in scheduleDays)
                {
                    foreach (var slot in day.ShiftSlots)
                    {
                        // Find assignment for this slot (by reference to same object in memory)
                        var assignment = day.Assignments.FirstOrDefault(a => a.ShiftSlotId == slot.Id || a.ShiftSlotId == 0);

                        var cmd = new SQLiteCommand(@"
                            INSERT INTO Schedules(Date, PostId, PersonId, Start, DurationHours)
                            VALUES(@Date, @PostId, @PersonId, @Start, @DurationHours);
                            SELECT last_insert_rowid();", conn);

                        cmd.Parameters.AddWithValue("@Date", day.Date);
                        cmd.Parameters.AddWithValue("@PostId", slot.PostId);
                        cmd.Parameters.AddWithValue("@PersonId", assignment?.PersonId ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Start", slot.Start.ToString());
                        cmd.Parameters.AddWithValue("@DurationHours", slot.DurationHours);

                        // Set the generated database ID
                        slot.Id = Convert.ToInt32(cmd.ExecuteScalar());

                        // Update assignment to reference correct slot ID
                        if (assignment != null)
                        {
                            assignment.ShiftSlotId = slot.Id;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Update a single assignment in the Schedules table
        /// </summary>
        public void UpdateAssignment(int shiftSlotId, int? personId)
        {
            using (var conn = GetConnection())
            {
                conn.Open();

                var cmd = new SQLiteCommand(@"
                    UPDATE Schedules 
                    SET PersonId = @PersonId 
                    WHERE Id = @ShiftSlotId", conn);

                cmd.Parameters.AddWithValue("@PersonId", personId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ShiftSlotId", shiftSlotId);

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Clear all assignments for a specific person on a specific date
        /// </summary>
        public void ClearPersonAssignmentsForDate(int personId, DateTime date)
        {
            using (var conn = GetConnection())
            {
                conn.Open();

                var cmd = new SQLiteCommand(@"
                    UPDATE Schedules 
                    SET PersonId = NULL 
                    WHERE PersonId = @PersonId AND Date = @Date", conn);

                cmd.Parameters.AddWithValue("@PersonId", personId);
                cmd.Parameters.AddWithValue("@Date", date);

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Clear assignment for a specific shift slot
        /// </summary>
        public void ClearShiftSlotAssignment(int shiftSlotId)
        {
            using (var conn = GetConnection())
            {
                conn.Open();

                var cmd = new SQLiteCommand(@"
                    UPDATE Schedules 
                    SET PersonId = NULL 
                    WHERE Id = @ShiftSlotId", conn);

                cmd.Parameters.AddWithValue("@ShiftSlotId", shiftSlotId);

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Retrieve ScheduleDay objects with their slots and assignments.
        /// </summary>
        public List<ScheduleDay> GetScheduleDays(DateTime from, DateTime to)
        {
            var dayDict = new Dictionary<DateTime, ScheduleDay>();
            from = from.Date;
            to = to.Date;
            using (var conn = GetConnection())
            {
                conn.Open();

                var cmd = new SQLiteCommand(@"
                    SELECT Id, Date, PostId, PersonId, Start, DurationHours
                    FROM Schedules
                    WHERE Date BETWEEN @From AND @To
                    ORDER BY Date, Start", conn);

                cmd.Parameters.AddWithValue("@From", from);
                cmd.Parameters.AddWithValue("@To", to);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int slotId = reader.GetInt32(0);
                        DateTime date = reader.GetDateTime(1);
                        int postId = reader.GetInt32(2);
                        int? personId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3);
                        TimeSpan start = TimeSpan.Parse(reader.GetString(4));
                        int duration = reader.GetInt32(5);

                        if (!dayDict.ContainsKey(date))
                            dayDict[date] = new ScheduleDay { Date = date };

                        var day = dayDict[date];

                        var slot = new ShiftSlot
                        {
                            Id = slotId,
                            Date = date,
                            PostId = postId,
                            Start = start,
                            DurationHours = duration
                        };

                        day.ShiftSlots.Add(slot);

                        if (personId.HasValue)
                        {
                            day.Assignments.Add(new Assignment
                            {
                                ShiftSlotId = slotId,
                                PersonId = personId.Value,
                                AssignedAt = DateTime.Now // could store actual assignment time if needed
                            });
                        }
                    }
                }
            }

            return dayDict.Values.OrderBy(d => d.Date).ToList();
        }

        /// <summary>
        /// Delete all schedule rows.
        /// </summary>
        public void DeleteAll()
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var cmd = new SQLiteCommand("DELETE FROM Schedules;", conn);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Delete all schedules for a specific day.
        /// </summary>
        public void DeleteScheduleDay(DateTime date)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var cmd = new SQLiteCommand("DELETE FROM Schedules WHERE Date = @Date;", conn);
                cmd.Parameters.AddWithValue("@Date", date);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
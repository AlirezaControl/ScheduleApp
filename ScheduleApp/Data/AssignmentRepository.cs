using System;
using System.Collections.Generic;
using System.Data.SQLite;
using GuardScheduler.Models;

namespace GuardScheduler.Data
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly string _connString;

        public AssignmentRepository(string connString)
        {
            _connString = connString;
            DatabaseInitializer.Initialize(connString); // Ensure DB structure exists
        }

        public List<Assignment> GetAssignmentsForPersonOnDate(int personId, DateTime date)
        {
            var assignments = new List<Assignment>();
            using var conn = new SQLiteConnection(_connString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT Id, ShiftSlotId, PersonId, AssignedAt
                FROM Assignment
                WHERE PersonId = @personId AND DATE(AssignedAt) = DATE(@date);";
            cmd.Parameters.AddWithValue("@personId", personId);
            cmd.Parameters.AddWithValue("@date", date);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                assignments.Add(ReadAssignment(reader));
            }
            return assignments;
        }

        public List<Assignment> GetAssignmentsForDate(DateTime date)
        {
            var assignments = new List<Assignment>();
            using var conn = new SQLiteConnection(_connString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT Id, ShiftSlotId, PersonId, AssignedAt
                FROM Assignment
                WHERE DATE(AssignedAt) = DATE(@date);";
            cmd.Parameters.AddWithValue("@date", date);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                assignments.Add(ReadAssignment(reader));
            }
            return assignments;
        }

        // ✅ New alias for convenience (used in your form)
        public List<Assignment> GetAssignmentsByDate(DateTime date)
        {
            return GetAssignmentsForDate(date);
        }

        public int Insert(Assignment assignment)
        {
            using var conn = new SQLiteConnection(_connString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Assignment (ShiftSlotId, PersonId, AssignedAt)
                VALUES (@shiftSlotId, @personId, @assignedAt);
                SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("@shiftSlotId", assignment.ShiftSlotId);
            cmd.Parameters.AddWithValue("@personId", assignment.PersonId);
            cmd.Parameters.AddWithValue("@assignedAt", assignment.AssignedAt);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void Delete(int id)
        {
            using var conn = new SQLiteConnection(_connString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Assignment WHERE Id = @id;";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public void DeleteAll()
        {
            using var conn = new SQLiteConnection(_connString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Assignment;";
            cmd.ExecuteNonQuery();
        }

        public List<Assignment> GetAssignmentsForSlot(int shiftSlotId)
        {
            var assignments = new List<Assignment>();
            using var conn = new SQLiteConnection(_connString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT Id, ShiftSlotId, PersonId, AssignedAt
                FROM Assignment
                WHERE ShiftSlotId = @shiftSlotId;";
            cmd.Parameters.AddWithValue("@shiftSlotId", shiftSlotId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                assignments.Add(ReadAssignment(reader));
            }
            return assignments;
        }

        private Assignment ReadAssignment(SQLiteDataReader reader)
        {
            return new Assignment
            {
                Id = reader.GetInt32(0),
                ShiftSlotId = reader.GetInt32(1),
                PersonId = reader.GetInt32(2),
                AssignedAt = reader.GetDateTime(3)
            };
        }
    }
}

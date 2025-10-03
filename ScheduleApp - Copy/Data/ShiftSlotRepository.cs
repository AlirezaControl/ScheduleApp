using System;
using System.Collections.Generic;
using System.Data.SQLite;
using GuardScheduler.Models;

namespace GuardScheduler.Data
{
    public class ShiftSlotRepository : SQLiteRepositoryBase, IShiftSlotRepository
    {
        public ShiftSlotRepository(string connectionString) : base(connectionString) { }

        public List<ShiftSlot> GetAllForDate(DateTime date)
        {
            var result = new List<ShiftSlot>();
            using var conn = GetConnection();
            conn.Open();
            string sql = "SELECT * FROM ShiftSlot WHERE Date = @date";
            using var cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@date", date.Date);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(Map(reader));
            }
            return result;
        }

        public ShiftSlot GetById(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            string sql = "SELECT * FROM ShiftSlot WHERE Id = @id";
            using var cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? Map(reader) : null;
        }

        public int Insert(ShiftSlot slot)
        {
            using var conn = GetConnection();
            conn.Open();
            string sql = @"INSERT INTO ShiftSlot (Date, PostId, StartHour, DurationHours, SlotIndex)
                           VALUES (@date, @postId, @startHour, @duration, @index);
                           SELECT last_insert_rowid();";
            using var cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@date", slot.Date.Date);
            cmd.Parameters.AddWithValue("@postId", slot.PostId);
            cmd.Parameters.AddWithValue("@startHour", slot.Start.TotalHours);
            cmd.Parameters.AddWithValue("@duration", slot.DurationHours);
            cmd.Parameters.AddWithValue("@index", slot.SlotIndex);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void Delete(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            string sql = "DELETE FROM ShiftSlot WHERE Id = @id";
            using var cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        private ShiftSlot Map(SQLiteDataReader reader)
        {
            return new ShiftSlot
            {
                Id = Convert.ToInt32(reader["Id"]),
                Date = Convert.ToDateTime(reader["Date"]),
                PostId = Convert.ToInt32(reader["PostId"]),
                Start = TimeSpan.FromHours(Convert.ToDouble(reader["StartHour"])),
                DurationHours = Convert.ToInt32(reader["DurationHours"]),
                SlotIndex = Convert.ToInt32(reader["SlotIndex"])
            };
        }
    }
}

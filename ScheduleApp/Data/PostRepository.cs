using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using GuardScheduler.Models;

namespace GuardScheduler.Data
{
    public class PostRepository : IPostRepository
    {
        private readonly string _connString;

        public PostRepository(string connString)
        {
            _connString = connString;
            DatabaseInitializer.Initialize(connString); // Ensure the database is initialized
        }

        public List<Post> GetAll()
        {
            var posts = new List<Post>();
            using var conn = new SQLiteConnection(_connString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Name, AllowedRoles, SlotsPerDay, SlotDurationHours, EnforceRestNextDay FROM Post;";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                posts.Add(ReadPost(reader));
            }
            return posts;
        }

        public Post GetById(int id)
        {
            using var conn = new SQLiteConnection(_connString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Name, AllowedRoles, SlotsPerDay, SlotDurationHours, EnforceRestNextDay FROM Post WHERE Id=@id;";
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
                return ReadPost(reader);
            return null;
        }

        public int Insert(Post post)
        {
            using var conn = new SQLiteConnection(_connString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Post
                (Name, AllowedRoles, SlotsPerDay, SlotDurationHours, EnforceRestNextDay)
                VALUES (@name, @allowedRoles, @slotsPerDay, @slotDurationHours, @enforceRestNextDay);
                SELECT last_insert_rowid();
            ";
            cmd.Parameters.AddWithValue("@name", post.Name);
            cmd.Parameters.AddWithValue("@allowedRoles", string.Join(",", post.AllowedRoles));
            cmd.Parameters.AddWithValue("@slotsPerDay", post.SlotsPerDay);
            cmd.Parameters.AddWithValue("@slotDurationHours", post.SlotDurationHours);
            cmd.Parameters.AddWithValue("@enforceRestNextDay", post.EnforceRestNextDay ? 1 : 0);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void Update(Post post)
        {
            using var conn = new SQLiteConnection(_connString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE Post SET
                    Name=@name,
                    AllowedRoles=@allowedRoles,
                    SlotsPerDay=@slotsPerDay,
                    SlotDurationHours=@slotDurationHours,
                    EnforceRestNextDay=@enforceRestNextDay
                WHERE Id=@id;
            ";
            cmd.Parameters.AddWithValue("@name", post.Name);
            cmd.Parameters.AddWithValue("@allowedRoles", string.Join(",", post.AllowedRoles));
            cmd.Parameters.AddWithValue("@slotsPerDay", post.SlotsPerDay);
            cmd.Parameters.AddWithValue("@slotDurationHours", post.SlotDurationHours);
            cmd.Parameters.AddWithValue("@enforceRestNextDay", post.EnforceRestNextDay ? 1 : 0);
            cmd.Parameters.AddWithValue("@id", post.Id);

            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = new SQLiteConnection(_connString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Post WHERE Id=@id;";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        private Post ReadPost(SQLiteDataReader reader)
        {
            return new Post
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                AllowedRoles = reader.IsDBNull(2) ? new List<Role>() : reader.GetString(2).Split(',').Select(r => (Role)Enum.Parse(typeof(Role), r)).ToList(),
                SlotsPerDay = reader.GetInt32(3),
                SlotDurationHours = reader.GetInt32(4),
                EnforceRestNextDay = Convert.ToBoolean(reader.GetInt32(5))
            };
        }
    }
}
using System.Data.SQLite;
using GuardScheduler.Models;

namespace GuardScheduler.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize(string connString)
        {
            using var conn = new SQLiteConnection(connString);
            conn.Open();
            using var cmd = conn.CreateCommand();

            // --- Roles ---
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Role (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL UNIQUE
                );";
            cmd.ExecuteNonQuery();

            // --- Posts ---
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Post (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL UNIQUE
                );";
            cmd.ExecuteNonQuery();

            // --- Person ---
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Person (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FirstName TEXT NOT NULL,
                    LastName TEXT NOT NULL,
                    PrimaryRoleId INTEGER NOT NULL,
                    SecondaryRoleId INTEGER,
                    RotationOrder INTEGER NOT NULL DEFAULT 0,
                    AllowedPosts TEXT, -- CSV of post names
                    FOREIGN KEY(PrimaryRoleId) REFERENCES Role(Id),
                    FOREIGN KEY(SecondaryRoleId) REFERENCES Role(Id)
                );";
            cmd.ExecuteNonQuery();
        }
    }
}
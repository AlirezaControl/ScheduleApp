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
                    Name TEXT NOT NULL UNIQUE,
                    AllowedRoles TEXT, -- CSV of role names
                    SlotsPerDay INTEGER NOT NULL DEFAULT 1,
                    SlotDurationHours INTEGER NOT NULL DEFAULT 24,
                    EnforceRestNextDay INTEGER NOT NULL DEFAULT 1 -- Using 1 for true and 0 for false
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

            // --- Assignments ---
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Assignment (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ShiftSlotId INTEGER NOT NULL,
                    PersonId INTEGER NOT NULL,
                    AssignedAt DATETIME NOT NULL,
                    FOREIGN KEY (ShiftSlotId) REFERENCES ShiftSlot(Id),
                    FOREIGN KEY (PersonId) REFERENCES Person(Id)
                );";
            cmd.ExecuteNonQuery();
        }
    }
}
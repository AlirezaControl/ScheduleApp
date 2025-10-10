using System;
using System.Collections.Generic;
using System.Data.SQLite;
using GuardScheduler.Models;

namespace GuardScheduler.Data
{
    public class PersonRepository : IPersonRepository
    {
        private readonly string _connString;

        public event EventHandler<PersonChangedEventArgs> PersonChanged;

        public PersonRepository(string connString)
        {
            _connString = connString;
            DatabaseInitializer.Initialize(connString);
        }

        public List<Person> GetAll()
        {
            var list = new List<Person>();
            using var conn = new SQLiteConnection(_connString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT Id, FirstName, LastName, PrimaryRoleId, SecondaryRoleId, RotationOrder, AllowedPosts, Available
                FROM Person;
            ";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(ReadPerson(reader));
            }
            return list;
        }

        public Person GetById(int id)
        {
            using var conn = new SQLiteConnection(_connString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT Id, FirstName, LastName, PrimaryRoleId, SecondaryRoleId, RotationOrder, AllowedPosts, Available
                FROM Person
                WHERE Id=@id;
            ";
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read()) return ReadPerson(reader);
            return null;
        }

        public int Insert(Person p)
        {
            using var conn = new SQLiteConnection(_connString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Person 
                (FirstName, LastName, PrimaryRoleId, SecondaryRoleId, RotationOrder, AllowedPosts, Available) 
                VALUES (@fn, @ln, @primary, @secondary, @order, @posts, @available);
                SELECT last_insert_rowid();
            ";
            cmd.Parameters.AddWithValue("@fn", p.FirstName);
            cmd.Parameters.AddWithValue("@ln", p.LastName);
            cmd.Parameters.AddWithValue("@primary", (int)p.PrimaryRole + 1);
            cmd.Parameters.AddWithValue("@secondary", p.SecondaryRole.HasValue ? (object)((int)p.SecondaryRole + 1) : DBNull.Value);
            cmd.Parameters.AddWithValue("@order", p.RotationOrder);
            cmd.Parameters.AddWithValue("@posts", string.Join(",", p.AllowedPostNames));
            cmd.Parameters.AddWithValue("@available", p.Available ? 1 : 0);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void Update(Person p)
        {
            using var conn = new SQLiteConnection(_connString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE Person SET FirstName=@fn, LastName=@ln, PrimaryRoleId=@primary,
                        SecondaryRoleId=@secondary, RotationOrder=@order, AllowedPosts=@posts, Available=@available
                        WHERE Id=@id;";
            cmd.Parameters.AddWithValue("@fn", p.FirstName);
            cmd.Parameters.AddWithValue("@ln", p.LastName);
            cmd.Parameters.AddWithValue("@primary", (int)p.PrimaryRole + 1);
            cmd.Parameters.AddWithValue("@secondary", p.SecondaryRole.HasValue ? (object)((int)p.SecondaryRole + 1) : DBNull.Value);
            cmd.Parameters.AddWithValue("@order", p.RotationOrder);
            cmd.Parameters.AddWithValue("@posts", string.Join(",", p.AllowedPostNames));
            cmd.Parameters.AddWithValue("@available", p.Available ? 1 : 0);
            cmd.Parameters.AddWithValue("@id", p.Id);
            cmd.ExecuteNonQuery();

            PersonChanged?.Invoke(this, new PersonChangedEventArgs(p)); // Trigger event
        }

        public void Delete(int id)
        {
            using var conn = new SQLiteConnection(_connString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Person WHERE Id=@id;";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        private Person ReadPerson(SQLiteDataReader reader)
        {
            return new Person
            {
                Id = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                PrimaryRole = (Role)(reader.GetInt32(3) - 1),
                SecondaryRole = reader.IsDBNull(4) ? null : (Role?)(reader.GetInt32(4) - 1),
                RotationOrder = reader.GetInt32(5),
                AllowedPostNames = "",
                Available = !reader.IsDBNull(7) && reader.GetInt32(7) == 1
            };
        }
    }
}

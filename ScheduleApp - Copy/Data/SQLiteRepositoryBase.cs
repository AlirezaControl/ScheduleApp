using System.Data.SQLite;

namespace GuardScheduler.Data
{
    public class SQLiteRepositoryBase
    {
        protected readonly string _connString;
        public SQLiteRepositoryBase(string connString) { _connString = connString; }

        protected SQLiteConnection GetConnection() => new SQLiteConnection(_connString);
    }
}
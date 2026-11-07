using Microsoft.Data.Sqlite;

namespace MSL.Data
{
    public static class DatabaseContext
    {
        public const string ConnectionString = "Data Source=ministry.db";

        public static SqliteConnection CreateConnection()
        {
            var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var pragma = connection.CreateCommand();
            pragma.CommandText = "PRAGMA foreign_keys = ON;";
            pragma.ExecuteNonQuery();

            return connection;
        }
    }
}

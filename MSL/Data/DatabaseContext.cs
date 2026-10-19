using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSL.Data
{
    public static class DatabaseContext
    {
        public const string ConnectionString = "Data Source=ministry.db";

        public static SqliteConnection CreateConnection()
        {
            var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            // СИСТЕМНАЯ ОШИБКА:
            // В SQLite по умолчанию отключена поддержка Foreign Keys
            // Для этого надо прописать: PRAGMA foreign_keys = ON;
            return connection;
        }
    }
}

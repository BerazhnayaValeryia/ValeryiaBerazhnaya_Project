using Microsoft.Data.Sqlite;

namespace DataAccess.Database
{
    public class SqliteDbContext
    {
        private readonly string _connectionString;

        public SqliteDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqliteConnection CreateConnection()
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
using Microsoft.Data.Sqlite;

namespace DataAccess.Database
{
    public static class DbInitializer
    {
        public static void Initialize(SqliteConnection connection)
        {
            var createTableCommand = connection.CreateCommand();
            createTableCommand.CommandText =
            """
    CREATE TABLE IF NOT EXISTS Furniture (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Name TEXT NOT NULL,
        Category TEXT NOT NULL,
        Price REAL NOT NULL,
        Quantity INTEGER NOT NULL
    );
    """;
            createTableCommand.ExecuteNonQuery();

            var checkCommand = connection.CreateCommand();
            checkCommand.CommandText = "SELECT COUNT(*) FROM Furniture";

            var count = (long)checkCommand.ExecuteScalar();

            if (count == 0)
            {
                var insertCommand = connection.CreateCommand();
                insertCommand.CommandText =
                """
    INSERT INTO Furniture (Name, Category, Price, Quantity)
    VALUES
        ('Chair Oslo', 'Chair', 120.50, 15),
        ('Table Nord', 'Table', 320.00, 5),
        ('Sofa Luna', 'Sofa', 899.99, 2);
    """;
                insertCommand.ExecuteNonQuery();
            }
        }
    }
}
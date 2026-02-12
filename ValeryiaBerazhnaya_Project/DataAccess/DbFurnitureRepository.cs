using DataAccess.Database;
using FurnitureWarehouse.Domain.Entities;
using FurnitureWarehouse.Domain.Enums;
using Microsoft.Data.Sqlite;

namespace DataAccess.Repositories
{
    public class DbFurnitureRepository : IFurnitureRepository
    {
        private readonly SqliteDbContext _context;

        public DbFurnitureRepository(SqliteDbContext context)
        {
            _context = context;

            using var connection = _context.CreateConnection();
        }

        public IEnumerable<Furniture> GetAll()
        {
            var result = new List<Furniture>();

            using var connection = _context.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Furniture";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(Map(reader));
            }

            return result;
        }

        public IEnumerable<Furniture> SearchByName(string name)
        {
            var result = new List<Furniture>();

            using var connection = _context.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText =
                "SELECT * FROM Furniture WHERE Name LIKE @name";
            command.Parameters.AddWithValue("@name", $"%{name}%");

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(Map(reader));
            }

            return result;
        }

        public IEnumerable<Furniture> SearchByCategory(FurnitureCategory category)
        {
            var result = new List<Furniture>();

            using var connection = _context.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText =
                "SELECT * FROM Furniture WHERE Category = @category";
            command.Parameters.AddWithValue("@category", category.ToString());

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(Map(reader));
            }

            return result;
        }

        public void Add(Furniture furniture)
        {
            using var connection = _context.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText =
            """
            INSERT INTO Furniture (Name, Category, Price, Quantity)
            VALUES (@name, @category, @price, @quantity)
            """;

            command.Parameters.AddWithValue("@name", furniture.Name);
            command.Parameters.AddWithValue("@category", furniture.Category.ToString());
            command.Parameters.AddWithValue("@price", furniture.Price);
            command.Parameters.AddWithValue("@quantity", furniture.Quantity);

            command.ExecuteNonQuery();
        }

        public void Update(int id, decimal price, int quantity)
        {
            using var connection = _context.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText =
            """
            UPDATE Furniture
            SET Price = @price, Quantity = @quantity
            WHERE Id = @id
            """;

            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@price", price);
            command.Parameters.AddWithValue("@quantity", quantity);

            command.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var connection = _context.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText =
                "DELETE FROM Furniture WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
        }

        private Furniture Map(SqliteDataReader reader)
        {
            return new Furniture(
                reader.GetInt32(0),
                reader.GetString(1),
                Enum.Parse<FurnitureCategory>(reader.GetString(2)),
                reader.GetDecimal(3),
                reader.GetInt32(4)
            );
        }
    }
}
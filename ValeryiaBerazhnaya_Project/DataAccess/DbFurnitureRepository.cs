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
            connection.Open();

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

            var rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 0)
                throw new ArgumentException("Item with this ID does not exist.");
        }

        public void Delete(int id)
        {
            using var connection = _context.CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Furniture WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);

            var rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 0)
                throw new ArgumentException("Item with this ID does not exist.");
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

        public Furniture? GetById(int id)
        {
            using var connection = _context.CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
        SELECT Id, Name, Category, Price, Quantity
        FROM Furniture
        WHERE Id = @id";

            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader();

            if (!reader.Read())
                return null;

            return new Furniture(
                reader.GetInt32(0),
                reader.GetString(1),
                Enum.Parse<FurnitureCategory>(reader.GetString(2)),
                reader.GetDecimal(3),
                reader.GetInt32(4)
            );
        }

        public IEnumerable<Furniture> GetByPriceRange(decimal min, decimal max)
        {
            using var connection = _context.CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
        SELECT Id, Name, Category, Price, Quantity
        FROM Furniture
        WHERE Price BETWEEN @min AND @max";

            command.Parameters.AddWithValue("@min", min);
            command.Parameters.AddWithValue("@max", max);

            using var reader = command.ExecuteReader();

            var list = new List<Furniture>();

            while (reader.Read())
            {
                list.Add(new Furniture(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    Enum.Parse<FurnitureCategory>(reader.GetString(2)),
                    reader.GetDecimal(3),
                    reader.GetInt32(4)
                ));
            }

            return list;
        }

        public IEnumerable<Furniture> GetByCategoryAndPrice(
    FurnitureCategory category,
    decimal min,
    decimal max)
        {
            using var connection = _context.CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
        SELECT Id, Name, Category, Price, Quantity
        FROM Furniture
        WHERE Category = @category
        AND Price BETWEEN @min AND @max";

            command.Parameters.AddWithValue("@category", category.ToString());
            command.Parameters.AddWithValue("@min", min);
            command.Parameters.AddWithValue("@max", max);

            using var reader = command.ExecuteReader();

            var list = new List<Furniture>();

            while (reader.Read())
            {
                list.Add(new Furniture(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    Enum.Parse<FurnitureCategory>(reader.GetString(2)),
                    reader.GetDecimal(3),
                    reader.GetInt32(4)
                ));
            }

            return list;
        }
    }
}
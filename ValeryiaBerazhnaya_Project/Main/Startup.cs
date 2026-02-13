using System.IO;
using Application;
using DataAccess.Database;
using DataAccess.Repositories;
using FurnitureWarehouse.Controller;
using FurnitureWarehouse.Presentation;
using FurnitureWarehouse.Service;

namespace FurnitureWarehouse.Main
{
    public class Startup
    {
        public Startup()
        {
        }

        public ConsoleView CreateView()
        {
            var dbPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Data",
                "furniture.db");

            var connectionString = $"Data Source={dbPath}";

            var context = new SqliteDbContext(connectionString);

            using var connection = context.CreateConnection();
            DbInitializer.Initialize(connection);

            var repository = new DbFurnitureRepository(context);
            var service = new InventoryService(repository);

            var userContext = new UserContext();

            var loginService = new LoginService();

            var controller = new InventoryController(
                service,
                userContext,
                loginService);

            return new ConsoleView(controller);
        }
    }
}
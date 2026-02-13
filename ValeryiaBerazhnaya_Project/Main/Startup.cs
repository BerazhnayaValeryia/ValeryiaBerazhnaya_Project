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
            var connectionString = "Data Source=furniture.db";

            var context = new SqliteDbContext(connectionString);

            using var connection = context.CreateConnection();
            DbInitializer.Initialize(connection);

            var repository = new DbFurnitureRepository(context);
            var service = new InventoryService(repository);

            var userContext = new UserContext();

            var controller = new InventoryController(service, userContext);

            return new ConsoleView(controller);
        }
    }
}
using FurnitureWarehouse.Controller;
using FurnitureWarehouse.DataAccess.FileSystem;
using FurnitureWarehouse.Main.Configuration;
using FurnitureWarehouse.Presentation;
using FurnitureWarehouse.Service;

namespace FurnitureWarehouse.Main
{
    public class Startup
    {
        private readonly AppConfiguration _config;

        public Startup(AppConfiguration config)
        {
            _config = config;
        }

        public ConsoleView CreateView()
        {
            var inventoryPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                _config.InventoryFile
            );

            var repository = new FileFurnitureRepository(inventoryPath);
            var service = new InventoryService(repository);
            var controller = new InventoryController(service);

            return new ConsoleView(controller);
        }
    }
}
using FurnitureWarehouse.Controller;
using FurnitureWarehouse.DataAccess.FileSystem;
using FurnitureWarehouse.Service;

class Program
{
    static void Main()
    {
        var repository = new FileFurnitureRepository("inventory.txt");
        var inventoryService = new InventoryService(repository);
        var controller = new InventoryController(inventoryService);

        controller.Run();
    }
}
using FurnitureWarehouse.Main;
using FurnitureWarehouse.Main.Configuration;

var config = new AppConfiguration
{
    InventoryFile = "inventory.txt"
};

var startup = new Startup(config);
var view = startup.CreateView();

view.Run();
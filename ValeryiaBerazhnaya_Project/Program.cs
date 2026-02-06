using FurnitureWarehouse.DataAccess.FileSystem;

Console.WriteLine(Directory.GetCurrentDirectory());

var repository = new FileFurnitureRepository("inventory.txt");

foreach (var item in repository.GetAll())
{
    Console.WriteLine($"{item.Id} - {item.Name}");
}
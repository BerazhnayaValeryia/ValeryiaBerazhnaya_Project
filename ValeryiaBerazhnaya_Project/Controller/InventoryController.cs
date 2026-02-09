using FurnitureWarehouse.Controller.Interfaces;
using FurnitureWarehouse.Service.Interfaces;

namespace FurnitureWarehouse.Controller
{
    public class InventoryController : IInventoryController
    {
        private readonly IInventoryService _service;

        public InventoryController(IInventoryService service)
        {
            _service = service;
        }

        public bool HandleCommand(string input)
        {
            var command = input.Trim().ToLower();

            switch (command)
            {
                case "list":
                    Print(_service.GetAll());
                    break;

                case "search-name":
                    Console.Write("Enter name: ");
                    var name = Console.ReadLine();
                    Print(_service.SearchByName(name ?? ""));
                    break;

                case "search-category":
                    Console.Write("Enter category: ");
                    var category = Console.ReadLine();
                    Print(_service.SearchByCategory(category ?? ""));
                    break;

                case "help":
                    return false;

                case "exit":
                    return true;

                default:
                    Console.WriteLine("Unknown command");
                    break;
            }

            return false;
        }

        private void Print(IEnumerable<Domain.Entities.Furniture> items)
        {
            if (!items.Any())
            {
                Console.WriteLine("No products found.");
                return;
            }

            foreach (var f in items)
            {
                Console.WriteLine(
                    $"ID: {f.Id}, Name: {f.Name}, Category: {f.Category}, " +
                    $"Price: {f.Price}, Quantity: {f.Quantity}"
                );
            }
        }
    }
}
using FurnitureWarehouse.Controller.Interfaces;
using FurnitureWarehouse.Service.Interfaces;

namespace FurnitureWarehouse.Controller
{
    public class InventoryController : IInventoryController
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public void Run()
        {
            PrintHeader();
            PrintMenu();

            while (true)
            {
                Console.Write("\n> ");
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                var command = input.Trim().ToLower();

                switch (command)
                {
                    case "1":
                        ShowAll();
                        break;

                    case "2":
                        SearchByName();
                        break;

                    case "3":
                        SearchByCategory();
                        break;

                    case "4":
                        Console.WriteLine("Goodbye!");
                        return;

                    case "5":
                        PrintMenu();
                        break;

                    default:
                        Console.WriteLine("Unknown command. Type 'help' to see available commands.");
                        break;
                }
            }
        }

        private void PrintHeader()
        {
            Console.WriteLine("===================================");
            Console.WriteLine(" Furniture Warehouse System v1.0");
            Console.WriteLine(" Created: 2026");
            Console.WriteLine(" Developer: Valeria Berezhnaya");
            Console.WriteLine(" Email: valeryia.berazhnaya@stud.esdc.lt");
            Console.WriteLine("===================================");
        }

        private void PrintMenu()
        {
            Console.WriteLine("\nAvailable commands:");
            Console.WriteLine(" 1 - Show all furniture");
            Console.WriteLine(" 2 - Search furniture by name");
            Console.WriteLine(" 3 - Search furniture by category");
            Console.WriteLine(" 4 - Show this menu");
            Console.WriteLine(" 5 - Exit application");
        }

        private void ShowAll()
        {
            var items = _inventoryService.GetAll();

            if (!items.Any())
            {
                Console.WriteLine("Inventory is empty.");
                return;
            }

            foreach (var item in items)
            {
                PrintFurniture(item);
            }
        }

        private void SearchByName()
        {
            Console.Write("Enter name: ");
            var name = Console.ReadLine();

            var results = _inventoryService.SearchByName(name ?? "");

            PrintResults(results);
        }

        private void SearchByCategory()
        {
            Console.Write("Enter category: ");
            var category = Console.ReadLine();

            var results = _inventoryService.SearchByCategory(category ?? "");

            PrintResults(results);
        }

        private void PrintResults(IEnumerable<Domain.Entities.Furniture> results)
        {
            if (!results.Any())
            {
                Console.WriteLine("No products found.");
                return;
            }

            foreach (var item in results)
            {
                PrintFurniture(item);
            }
        }

        private void PrintFurniture(Domain.Entities.Furniture f)
        {
            Console.WriteLine(
                $"ID: {f.Id}, Name: {f.Name}, Category: {f.Category}, " +
                $"Price: {f.Price}, Quantity: {f.Quantity}"
            );
        }
    }
}
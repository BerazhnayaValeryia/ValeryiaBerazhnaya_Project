using FurnitureWarehouse.Controller.Interfaces;
using FurnitureWarehouse.Presentation.Interfaces;

namespace FurnitureWarehouse.Presentation
{
    public class ConsoleView : IView
    {
        private readonly IInventoryController _controller;

        public ConsoleView(IInventoryController controller)
        {
            _controller = controller;
        }

        public void Start()
        {
            PrintHeader();
            PrintMenu();

            while (true)
            {
                Console.Write("\n> ");
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                try
                {
                    var shouldExit = _controller.HandleCommand(input);

                    if (shouldExit)
                        break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            Console.WriteLine("Goodbye!");
        }

        public void Crash(string message)
        {
            Console.WriteLine("Critical error occurred:");
            Console.WriteLine(message);
        }

        private void PrintHeader()
        {
            Console.WriteLine("===================================");
            Console.WriteLine(" Furniture Warehouse System v1.0");
            Console.WriteLine(" Created: 2026");
            Console.WriteLine(" Developer: Valeria Berezhnaya");
            Console.WriteLine("===================================");
        }

        private void PrintMenu()
        {
            Console.WriteLine("\nAvailable commands:");
            Console.WriteLine(" list");
            Console.WriteLine(" search-name");
            Console.WriteLine(" search-category");
            Console.WriteLine(" help");
            Console.WriteLine(" exit");
        }
    }
}
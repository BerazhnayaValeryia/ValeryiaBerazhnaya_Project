//using FurnitureWarehouse.Controller.Interfaces;
//using FurnitureWarehouse.Presentation.Interfaces;

//namespace FurnitureWarehouse.Presentation
//{
//    public class ConsoleView : IView
//    {
//        private readonly IInventoryController _controller;

//        public ConsoleView(IInventoryController controller)
//        {
//            _controller = controller;
//        }

//        public void Start()
//        {
//            PrintHeader();
//            PrintMenu();

//            while (true)
//            {
//                Console.Write("\n> ");
//                var input = Console.ReadLine();

//                if (string.IsNullOrWhiteSpace(input))
//                    continue;

//                try
//                {
//                    var shouldExit = _controller.HandleCommand(input);

//                    if (shouldExit)
//                        break;
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Error: {ex.Message}");
//                }
//            }

//            Console.WriteLine("Goodbye!");
//        }

//        public void Crash(string message)
//        {
//            Console.WriteLine("Critical error occurred:");
//            Console.WriteLine(message);
//        }

//        private void PrintHeader()
//        {
//            Console.WriteLine("===================================");
//            Console.WriteLine(" Furniture Warehouse System v1.0");
//            Console.WriteLine(" Created: 2026");
//            Console.WriteLine(" Developer: Valeria Berezhnaya");
//            Console.WriteLine("===================================");
//        }

//        private void PrintMenu()
//        {
//            Console.WriteLine("\nAvailable commands:");
//            Console.WriteLine(" list");
//            Console.WriteLine(" search-name");
//            Console.WriteLine(" search-category");
//            Console.WriteLine(" help");
//            Console.WriteLine(" exit");
//        }
//    }
//}

using FurnitureWarehouse.Controller.Interfaces;

namespace FurnitureWarehouse.Presentation
{
    public class ConsoleView
    {
        private readonly IInventoryController _controller;

        public ConsoleView(IInventoryController controller)
        {
            _controller = controller;
        }

        public void Run()
        {
            PrintHeader();
            PrintMenu();

            bool exit = false;

            while (!exit)
            {
                Console.Write("> ");
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                exit = _controller.HandleCommand(input);

                if (!exit)
                {
                    Console.WriteLine();
                    PrintMenu();
                }
            }

            Console.WriteLine("Application closed.");
        }

        private void PrintHeader()
        {
            Console.WriteLine("Furniture Warehouse System");
            Console.WriteLine("Version 1.0");
            Console.WriteLine("Created: 2026");
            Console.WriteLine();
        }

        private void PrintMenu()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("list");
            Console.WriteLine("search-name");
            Console.WriteLine("search-category");
            Console.WriteLine("login");
            Console.WriteLine("logout");
            Console.WriteLine("help");
            Console.WriteLine("exit");
            Console.WriteLine();

            if (_controller.IsAdmin())
            {
                Console.WriteLine("add");
                Console.WriteLine("update");
                Console.WriteLine("delete");
            }
        }
    }
}
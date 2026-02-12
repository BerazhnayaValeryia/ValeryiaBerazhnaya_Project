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
            ApplyRoleStyle();
            PrintHeader();
            PrintMenu();

            bool exit = false;

            while (!exit)
            {
                var roleLabel = _controller.IsAdmin() ? "[ADMIN]" : "[USER]";
                Console.Write($"{roleLabel} Enter command: ");

                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                if (input.Trim().ToLower() == "help")
                {
                    Console.WriteLine();
                    PrintMenu();
                    continue;
                }

                var result = _controller.HandleCommand(input);

                switch (result)
                {
                    case CommandResult.Exit:
                        exit = true;
                        break;

                    case CommandResult.RoleChanged:
                        ApplyRoleStyle();
                        PrintHeader();
                        PrintMenu();
                        break;
                }
            }

            Console.ResetColor();
            Console.WriteLine("Application closed.");
        }

        private void PrintHeader()
        {
            var role = _controller.GetCurrentRole();

            Console.WriteLine("Created: 2026");
        }

        private void PrintMenu()
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("      Furniture Warehouse Management System v1.0");
            Console.WriteLine("==================================================");
            Console.WriteLine();

            Console.WriteLine("=============== Available Commands ===============");
            Console.WriteLine();

            Console.WriteLine("Query commands:");
            Console.WriteLine("   list                         - Show all furniture");
            Console.WriteLine("   search-name                  - Search by name");
            Console.WriteLine("   search-category              - Search by category");
            Console.WriteLine();

            if (_controller.IsAdmin())
            {
                Console.WriteLine("Admin commands:");
                Console.WriteLine("   add                          - Add new furniture item");
                Console.WriteLine("   update                       - Update price and quantity");
                Console.WriteLine("   delete                       - Delete item by ID");
                Console.WriteLine();
            }

            Console.WriteLine("Mode commands:");
            Console.WriteLine("   login                        - Switch to admin mode");
            Console.WriteLine("   logout                       - Return to user mode");
            Console.WriteLine();

            Console.WriteLine("Other commands:");
            Console.WriteLine("   help                         - Show this menu");
            Console.WriteLine("   exit                         - Exit application");
            Console.WriteLine();
        }

        private void ApplyRoleStyle()
        {
            if (_controller.IsAdmin())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
            }

            Console.Clear();
        }
    }
}
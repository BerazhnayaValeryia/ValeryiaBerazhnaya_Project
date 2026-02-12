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
                    PrintHelp();
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
            Console.WriteLine("   show                         - Search by id");
            Console.WriteLine("   price                        - Search in price range");
            Console.WriteLine("   price-category               - Search in price range by category");
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

        private void PrintHelp()
        {
            Console.WriteLine();
            Console.WriteLine("   AVAILABLE COMMANDS:");
            Console.WriteLine();

            Console.WriteLine("   Data viewing:");
            Console.WriteLine("   list                         - Show all furniture items");
            Console.WriteLine("   show <id>                    - Search by id");
            Console.WriteLine("   price <price> <price>        - Search in price range");
            Console.WriteLine("   price-category <category> <price> <price>");
            Console.WriteLine("                                - Search in price range");
            Console.WriteLine("   search-name <name>           - Search furniture by name (Example: search-name Chair)");
            Console.WriteLine("   search-category <category>   - Search furniture by category (Example: search-category Table)");
            Console.WriteLine();

            if (_controller.IsAdmin())
            {
                Console.WriteLine("   Admin functions:");
                Console.WriteLine("   add                          - Add new furniture item");
                Console.WriteLine("   update                       - Update price and quantity");
                Console.WriteLine("   delete                       - Delete item by ID");
                Console.WriteLine();
            }

            Console.WriteLine("   System commands:");
            Console.WriteLine("   login                        - Switch to admin mode");
            Console.WriteLine("   logout                       - Return to user mode");
            Console.WriteLine("   exit                         - Exit application");
            Console.WriteLine();

            Console.WriteLine("   To access admin functions (add, update, delete), use command: login");
            Console.WriteLine();
        }
    }
}
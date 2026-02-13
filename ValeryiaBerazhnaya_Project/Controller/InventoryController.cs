using Application;
using FurnitureWarehouse.Controller.Interfaces;
using FurnitureWarehouse.Domain.Enums;
using FurnitureWarehouse.Service.Interfaces;

namespace FurnitureWarehouse.Controller
{
    public class InventoryController : IInventoryController
    {
        private readonly IInventoryService _service;
        private readonly UserContext _userContext;
        private readonly LoginService _loginService;

        public InventoryController(
            IInventoryService service,
            UserContext userContext,
            LoginService loginService)
        {
            _service = service;
            _userContext = userContext;
            _loginService = loginService;
        }

        public CommandResult HandleCommand(string input)
        {
            var parts = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                return CommandResult.None;

            var command = parts[0].ToLower();

            switch (command)
            {
                case "login":
                    {
                        var before = _userContext.IsAdmin;
                        Login();
                        return before != _userContext.IsAdmin
                            ? CommandResult.RoleChanged
                            : CommandResult.None;
                    }

                case "logout":
                    {
                        var before = _userContext.IsAdmin;
                        Logout();
                        return before != _userContext.IsAdmin
                            ? CommandResult.RoleChanged
                            : CommandResult.None;
                    }

                case "add":
                    if (!_userContext.IsAdmin)
                    {
                        Console.WriteLine("Access denied. Admin only.");
                        break;
                    }
                    AddItem();
                    break;

                case "update":
                    if (!_userContext.IsAdmin)
                    {
                        Console.WriteLine("Access denied. Admin only.");
                        break;
                    }
                    UpdateItem();
                    break;

                case "delete":
                    if (!_userContext.IsAdmin)
                    {
                        Console.WriteLine("Access denied. Admin only.");
                        break;
                    }
                    DeleteItem();
                    break;

                case "list":
                    Print(_service.GetAll());
                    break;

                case "search-name":
                    if (parts.Length < 2)
                    {
                        Console.WriteLine("Usage: search-name <name>");
                        break;
                    }

                    var name = string.Join(" ", parts.Skip(1));
                    Print(_service.SearchByName(name));
                    break;

                case "search-category":
                    if (parts.Length < 2)
                    {
                        Console.WriteLine("Usage: search-category <category>");
                        break;
                    }

                    var categoryInput = parts[1];

                    if (!Enum.TryParse<Domain.Enums.FurnitureCategory>(
                        categoryInput, true, out var category))
                    {
                        Console.WriteLine("Invalid category.");
                        break;
                    }

                    Print(_service.SearchByCategory(category.ToString()));
                    break;

                case "exit":
                    return CommandResult.Exit;

                case "show":
                    if (parts.Length != 2 || !int.TryParse(parts[1], out var id))
                    {
                        Console.WriteLine("Usage: show <id>");
                        break;
                    }

                    var item = _service.GetById(id);
                    if (item == null)
                    {
                        Console.WriteLine("Item not found.");
                        break;
                    }

                    Print(new List<Domain.Entities.Furniture> { item });
                    break;

                case "price":
                    {
                        while (true)
                        {
                            if (parts.Length != 3 ||
                                !decimal.TryParse(parts[1], out var min) ||
                                !decimal.TryParse(parts[2], out var max))
                            {
                                WriteError("Usage: price <min> <max>");
                                break;
                            }

                            try
                            {
                                Print(_service.GetByPriceRange(min, max));
                                break;
                            }
                            catch (ArgumentException ex)
                            {
                                WriteError(ex.Message);
                                break;
                            }
                        }

                        break;
                    }

                case "price-category":
                    {
                        if (parts.Length != 4 ||
                            !decimal.TryParse(parts[2], out var minCat) ||
                            !decimal.TryParse(parts[3], out var maxCat))
                        {
                            WriteError("Usage: price-category <category> <min> <max>");
                            break;
                        }

                        if (!Enum.TryParse<FurnitureCategory>(
                            parts[1], true, out var cat))
                        {
                            WriteError("Invalid category.");
                            break;
                        }

                        try
                        {
                            Print(_service.GetByCategoryAndPrice(cat.ToString(), minCat, maxCat));
                        }
                        catch (ArgumentException ex)
                        {
                            WriteError(ex.Message);
                        }

                        break;
                    }

                default:
                    Console.WriteLine("Unknown command");
                    break;
            }

            return CommandResult.None;
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

        private void AddItem()
        {
            Console.WriteLine("=== Add New Furniture ===");

            var categories = Enum.GetValues<FurnitureCategory>();

            Console.WriteLine("Select category:");

            for (int i = 0; i < categories.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {categories[i]}");
            }

            var categoryNumber = ReadInt("Category number");
            if (categoryNumber == null) return;

            if (categoryNumber < 1 || categoryNumber > categories.Length)
            {
                WriteError("Selected category does not exist.");
                return;
            }

            var category = categories[categoryNumber.Value - 1];

            var name = ReadNonEmptyString("Name");
            if (name == null) return;

            var price = ReadDecimal("Price");
            if (price == null) return;

            var quantity = ReadInt("Quantity");
            if (quantity == null) return;

            try
            {
                _service.Add(name, category, price.Value, quantity.Value);
                Console.WriteLine("\nItem added successfully.");
            }
            catch (ArgumentException ex)
            {
                WriteError($"Error: {ex.Message}");
            }
        }

        private void UpdateItem()
        {
            var id = ReadInt("Enter ID");
            if (id == null) return;

            var price = ReadDecimal("New price");
            if (price == null) return;

            var quantity = ReadInt("New quantity");
            if (quantity == null) return;

            try
            {
                _service.Update(id.Value, price.Value, quantity.Value);
                Console.WriteLine("Item updated successfully.");
            }
            catch (ArgumentException ex)
            {
                WriteError($"Error: {ex.Message}");
            }
        }

        private void DeleteItem()
        {
            var id = ReadInt("Enter ID");
            if (id == null) return;

            try
            {
                _service.Delete(id.Value);
                Console.WriteLine("Item deleted successfully.");
            }
            catch (ArgumentException ex)
            {
                WriteError($"Error: {ex.Message}");
            }
        }

        private void Login()
        {
            if (_userContext.IsAdmin)
            {
                Console.WriteLine("Already logged in as admin.");
                return;
            }

            while (true)
            {
                Console.Write("Login (or 'q' to cancel): ");
                var login = Console.ReadLine();

                if (login?.ToLower() == "q")
                    return;

                Console.Write("Password: ");
                var password = ReadPassword();

                if (_loginService.TryLogin(login!, password!, _userContext, out var message))
                {
                    Console.WriteLine(message);
                    return;
                }
                else
                {
                    Console.WriteLine(message);

                    if (message.Contains("locked"))
                        return;
                }
            }
        }

        private void Logout()
        {
            if (!_userContext.IsAdmin)
            {
                Console.WriteLine("You are not in admin mode.");
                return;
            }

            _loginService.Logout(_userContext);
            Console.WriteLine("Logged out. Back to user mode.");
        }

        public bool IsAdmin()
        {
            return _userContext.IsAdmin;
        }

        public UserRole GetCurrentRole()
        {
            return _userContext.Role;
        }

        private string ReadPassword()
        {
            var password = string.Empty;
            ConsoleKey key;

            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[0..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    password += keyInfo.KeyChar;
                    Console.Write("*");
                }

            } while (key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }

        private void WriteError(string message)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = originalColor;
        }

        private string? ReadNonEmptyString(string label)
        {
            while (true)
            {
                Console.Write($"{label} (or 'q' to cancel): ");
                var input = Console.ReadLine();

                if (input?.ToLower() == "q")
                    return null;

                if (string.IsNullOrWhiteSpace(input))
                {
                    WriteError("Value cannot be empty.\n");
                    continue;
                }

                return input;
            }
        }

        private decimal? ReadDecimal(string label)
        {
            while (true)
            {
                Console.Write($"{label} (or 'q' to cancel): ");
                var input = Console.ReadLine();

                if (input?.ToLower() == "q")
                    return null;

                if (!decimal.TryParse(input, out var value))
                {
                    WriteError("Must be a valid number.\n");
                    continue;
                }

                if (value < 0)
                {
                    WriteError("Value cannot be negative.\n");
                    continue;
                }

                return value;
            }
        }

        private int? ReadInt(string label)
        {
            while (true)
            {
                Console.Write($"{label} (or 'q' to cancel): ");
                var input = Console.ReadLine();

                if (input?.ToLower() == "q")
                    return null;

                if (!int.TryParse(input, out var value))
                {
                    WriteError("Must be a whole number.\n");
                    continue;
                }

                if (value < 0)
                {
                    WriteError("Value cannot be negative.\n");
                    continue;
                }

                return value;
            }
        }
    }
}
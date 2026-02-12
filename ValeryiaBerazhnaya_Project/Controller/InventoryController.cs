using Application;
using Domain.Enums;
using FurnitureWarehouse.Controller.Interfaces;
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
    UserContext userContext)
        {
            _service = service;
            _userContext = userContext;
            _loginService = new LoginService();
        }

        public InventoryController(IInventoryService service)
        {
            _service = service;
        }

        public CommandResult HandleCommand(string input)
        {
            var command = input.Trim().ToLower();

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
                    Console.Write("Enter name: ");
                    var name = Console.ReadLine();
                    Print(_service.SearchByName(name ?? ""));
                    break;

                case "search-category":
                    Console.Write("Enter category: ");
                    var category = Console.ReadLine();
                    Print(_service.SearchByCategory(category ?? ""));
                    break;

                case "exit":
                    return CommandResult.Exit;

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
            Console.Write("Name: ");
            var name = Console.ReadLine();

            Console.Write("Category: ");
            var categoryInput = Console.ReadLine();
            var category = Enum.Parse<Domain.Enums.FurnitureCategory>(categoryInput!, true);

            Console.Write("Price: ");
            var price = decimal.Parse(Console.ReadLine()!);

            Console.Write("Quantity: ");
            var quantity = int.Parse(Console.ReadLine()!);

            _service.Add(name!, category, price, quantity);

            Console.WriteLine("Item added successfully.");
        }

        private void UpdateItem()
        {
            Console.Write("Enter ID: ");
            var id = int.Parse(Console.ReadLine()!);

            Console.Write("New price: ");
            var price = decimal.Parse(Console.ReadLine()!);

            Console.Write("New quantity: ");
            var quantity = int.Parse(Console.ReadLine()!);

            _service.Update(id, price, quantity);

            Console.WriteLine("Item updated successfully.");
        }

        private void DeleteItem()
        {
            Console.Write("Enter ID: ");
            var id = int.Parse(Console.ReadLine()!);

            _service.Delete(id);

            Console.WriteLine("Item deleted successfully.");
        }

        private void Login()
        {
            if (_userContext.IsAdmin)
            {
                Console.WriteLine("Already logged in as admin.");
                return;
            }

            Console.Write("Login: ");
            var login = Console.ReadLine();

            Console.Write("Password: ");
            var password = ReadPassword();

            if (_loginService.TryLogin(login!, password!, _userContext))
            {
                Console.WriteLine("Login successful. Admin mode enabled.");
            }
            else
            {
                Console.WriteLine("Invalid credentials.");
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
    }
}
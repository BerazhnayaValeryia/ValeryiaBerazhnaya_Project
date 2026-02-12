using Domain.Enums;

namespace FurnitureWarehouse.Controller.Interfaces
{
    public interface IInventoryController
    {
        CommandResult HandleCommand(string input);

        bool IsAdmin();
        UserRole GetCurrentRole();
    }
}
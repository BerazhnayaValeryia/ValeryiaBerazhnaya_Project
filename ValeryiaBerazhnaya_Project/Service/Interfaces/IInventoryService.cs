using FurnitureWarehouse.Domain.Entities;

namespace FurnitureWarehouse.Service.Interfaces
{
    public interface IInventoryService
    {
        Inventory LoadInventory();
        IEnumerable<Furniture> GetAll();
        IEnumerable<Furniture> SearchByName(string name);
        IEnumerable<Furniture> SearchByCategory(string category);
    }
}
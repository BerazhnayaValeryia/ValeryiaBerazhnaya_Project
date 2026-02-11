using FurnitureWarehouse.Domain.Entities;
using FurnitureWarehouse.Domain.Enums;

namespace FurnitureWarehouse.Service.Interfaces
{
    public interface IInventoryService
    {
        IEnumerable<Furniture> GetAll();
        IEnumerable<Furniture> SearchByName(string name);
        IEnumerable<Furniture> SearchByCategory(string category);
        void Add(string name, FurnitureCategory category, decimal price, int quantity);
        void Update(int id, decimal price, int quantity);
        void Delete(int id);
    }
}
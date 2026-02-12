using FurnitureWarehouse.Domain.Entities;
using FurnitureWarehouse.Domain.Enums;

namespace FurnitureWarehouse.Service.Interfaces
{
    public interface IInventoryService
    {
        IEnumerable<Furniture> GetAll();
        IEnumerable<Furniture> SearchByName(string name);
        IEnumerable<Furniture> SearchByCategory(string category);
        Furniture? GetById(int id);
        IEnumerable<Furniture> GetByPriceRange(decimal min, decimal max);
        IEnumerable<Furniture> GetByCategoryAndPrice(string category, decimal min, decimal max);
        void Add(string name, FurnitureCategory category, decimal price, int quantity);
        void Update(int id, decimal price, int quantity);
        void Delete(int id);
    }
}
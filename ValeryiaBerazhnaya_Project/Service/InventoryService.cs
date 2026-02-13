using FurnitureWarehouse.Domain.Entities;
using FurnitureWarehouse.Domain.Enums;
using FurnitureWarehouse.Service.Interfaces;
using FurnitureWarehouse.Domain.Interfaces;

namespace FurnitureWarehouse.Service
{
    public class InventoryService : IInventoryService
    {
        private readonly IFurnitureRepository _repository;

        public InventoryService(IFurnitureRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Furniture> GetAll()
        {
            return _repository.GetAll();
        }

        public IEnumerable<Furniture> SearchByName(string name)
        {
            return _repository.SearchByName(name);
        }

        public IEnumerable<Furniture> SearchByCategory(string category)
        {
            if (!Enum.TryParse<FurnitureCategory>(category, true, out var parsed))
                return Enumerable.Empty<Furniture>();

            return _repository.SearchByCategory(parsed);
        }

        public void Add(string name, FurnitureCategory category, decimal price, int quantity)
        {
            var furniture = new Furniture(0, name, category, price, quantity);
            _repository.Add(furniture);
        }

        public void Update(int id, decimal price, int quantity)
        {
            _repository.Update(id, price, quantity);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        public Furniture? GetById(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<Furniture> GetByPriceRange(decimal min, decimal max)
        {
            if (min < 0 || max < 0)
                throw new ArgumentException("Price cannot be negative.");

            if (min > max)
                throw new ArgumentException("Min price cannot be greater than max price.");

            return _repository.GetByPriceRange(min, max);
        }

        public IEnumerable<Furniture> GetByCategoryAndPrice(string category, decimal min, decimal max)
        {
            if (!Enum.TryParse<FurnitureCategory>(category, true, out var parsed))
                throw new ArgumentException("Invalid category.");

            if (min < 0 || max < 0)
                throw new ArgumentException("Price cannot be negative.");

            if (min > max)
                throw new ArgumentException("Min price cannot be greater than max price.");

            return _repository.GetByCategoryAndPrice(parsed, min, max);
        }
    }
}
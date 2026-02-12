using FurnitureWarehouse.Domain.Entities;
using FurnitureWarehouse.Domain.Enums;
using FurnitureWarehouse.Service.Interfaces;

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
            return _repository.GetAll().FirstOrDefault(f => f.Id == id);
        }

        public IEnumerable<Furniture> GetByPriceRange(decimal min, decimal max)
        {
            return _repository
                .GetAll()
                .Where(f => f.Price >= min && f.Price <= max);
        }

        public IEnumerable<Furniture> GetByCategoryAndPrice(string category, decimal min, decimal max)
        {
            if (!Enum.TryParse<FurnitureCategory>(category, true, out var parsed))
                return Enumerable.Empty<Furniture>();

            return _repository
                .GetAll()
                .Where(f =>
                    f.Category == parsed &&
                    f.Price >= min &&
                    f.Price <= max);
        }
    }
}
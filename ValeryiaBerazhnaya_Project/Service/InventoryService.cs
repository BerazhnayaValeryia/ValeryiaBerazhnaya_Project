using FurnitureWarehouse.DataAccess.Interfaces;
using FurnitureWarehouse.Domain.Entities;
using FurnitureWarehouse.Domain.Enums;
using FurnitureWarehouse.Domain.Services;
using FurnitureWarehouse.Service.Interfaces;

namespace FurnitureWarehouse.Service
{
    public class InventoryService : IInventoryService
    {
        private readonly Inventory _inventory;
        private readonly FurnitureSearchService _searchService;

        public InventoryService(IFurnitureRepository repository)
        {
            var loader = new InventoryLoader(repository);
            _inventory = loader.Load();

            _searchService = new FurnitureSearchService();
        }

        public Inventory LoadInventory()
        {
            return _inventory;
        }

        public IEnumerable<Furniture> GetAll()
        {
            return _inventory.Furnitures;
        }

        public IEnumerable<Furniture> SearchByName(string name)
        {
            return _searchService.SearchByName(
                _inventory.Furnitures,
                name
            );
        }

        public IEnumerable<Furniture> SearchByCategory(string category)
        {
            if (!Enum.TryParse<FurnitureCategory>(category, true, out var parsed))
                return Enumerable.Empty<Furniture>();

            return _searchService.SearchByCategory(
                _inventory.Furnitures,
                parsed
            );
        }
    }
}
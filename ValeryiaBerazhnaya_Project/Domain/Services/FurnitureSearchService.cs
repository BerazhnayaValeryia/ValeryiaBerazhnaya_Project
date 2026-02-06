using FurnitureWarehouse.Domain.Entities;
using FurnitureWarehouse.Domain.Enums;

namespace FurnitureWarehouse.Domain.Services
{
    public class FurnitureSearchService
    {
        public IEnumerable<Furniture> SearchByName(
            IEnumerable<Furniture> furnitures,
            string name)
        {
            return furnitures
                .Where(f => f.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Furniture> SearchByCategory(
            IEnumerable<Furniture> furnitures,
            FurnitureCategory category)
        {
            return furnitures
                .Where(f => f.Category == category);
        }

        public Furniture? SearchById(
            IEnumerable<Furniture> furnitures,
            int id)
        {
            return furnitures.FirstOrDefault(f => f.Id == id);
        }
    }
}
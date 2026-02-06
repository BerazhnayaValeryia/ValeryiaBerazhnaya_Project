using FurnitureWarehouse.Domain.Exceptions;

namespace FurnitureWarehouse.Domain.Entities
{
    public class Inventory
    {
        private readonly List<Furniture> _furnitures;

        public Inventory()
        {
            _furnitures = new List<Furniture>();
        }

        public IReadOnlyCollection<Furniture> Furnitures => _furnitures.AsReadOnly();

        public void Add(Furniture furniture)
        {
            if (_furnitures.Any(f => f.Id == furniture.Id))
                throw new DuplicateFurnitureException(furniture.Id);

            _furnitures.Add(furniture);
        }
    }
}
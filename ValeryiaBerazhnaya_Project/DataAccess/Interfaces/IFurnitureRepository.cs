using FurnitureWarehouse.Domain.Entities;

namespace FurnitureWarehouse.DataAccess.Interfaces
{
    public interface IFurnitureRepository
    {
        IEnumerable<Furniture> GetAll();
    }
}
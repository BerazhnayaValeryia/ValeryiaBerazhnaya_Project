using FurnitureWarehouse.Domain.Entities;
using FurnitureWarehouse.Domain.Enums;

public interface IFurnitureRepository
{
    IEnumerable<Furniture> GetAll();

    IEnumerable<Furniture> SearchByName(string name);
    IEnumerable<Furniture> SearchByCategory(FurnitureCategory category);

    void Add(Furniture item);
    void Update(int id, decimal price, int quantity);
    void Delete(int id);
}
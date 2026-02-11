using FurnitureWarehouse.Domain.Entities;
using FurnitureWarehouse.DataAccess;

public class InventoryLoader
{
    private readonly IFurnitureRepository _repository;

    public InventoryLoader(IFurnitureRepository repository)
    {
        _repository = repository;
    }

    public Inventory Load()
    {
        var inventory = new Inventory();

        foreach (var furniture in _repository.GetAll())
        {
            inventory.Add(furniture);
        }

        return inventory;
    }
}
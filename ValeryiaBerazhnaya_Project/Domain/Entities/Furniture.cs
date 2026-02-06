using FurnitureWarehouse.Domain.Enums;

namespace FurnitureWarehouse.Domain.Entities
{
    public class Furniture
    {
        public int Id { get; }
        public string Name { get; }
        public FurnitureCategory Category { get; }
        public decimal Price { get; }
        public int Quantity { get; private set; }

        public Furniture(int id, string name, FurnitureCategory category, decimal price, int quantity)
        {
            if (id <= 0)
                throw new ArgumentException("Id must be positive.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");

            if (price < 0)
                throw new ArgumentException("Price cannot be negative.");

            if (quantity < 0)
                throw new ArgumentException("Quantity cannot be negative.");

            Id = id;
            Name = name;
            Category = category;
            Price = price;
            Quantity = quantity;
        }

        public void ChangeQuantity(int newQuantity)
        {
            if (newQuantity < 0)
                throw new ArgumentException("Quantity cannot be negative.");

            Quantity = newQuantity;
        }
    }
}
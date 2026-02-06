namespace FurnitureWarehouse.Domain.Exceptions
{
    public class DuplicateFurnitureException : Exception
    {
        public DuplicateFurnitureException(int id)
            : base($"Furniture with ID {id} already exists.")
        {
        }
    }
}
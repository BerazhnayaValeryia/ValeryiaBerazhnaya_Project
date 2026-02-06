using FurnitureWarehouse.DataAccess.Interfaces;
using FurnitureWarehouse.Domain.Entities;
using FurnitureWarehouse.Domain.Enums;

namespace FurnitureWarehouse.DataAccess.FileSystem
{
    public class FileFurnitureRepository : IFurnitureRepository
    {
        private readonly string _filePath;

        public FileFurnitureRepository(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be empty.");

            _filePath = filePath;
        }

        public IEnumerable<Furniture> GetAll()
        {
            if (!File.Exists(_filePath))
                throw new FileNotFoundException("Inventory file not found.", _filePath);

            var lines = File.ReadAllLines(_filePath);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                yield return ParseLine(line);
            }
        }

        private Furniture ParseLine(string line)
        {
            var parts = line.Split(';');

            if (parts.Length != 5)
                throw new FormatException($"Invalid line format: {line}");

            int id = int.Parse(parts[0]);
            string name = parts[1];
            FurnitureCategory category = Enum.Parse<FurnitureCategory>(parts[2]);
            decimal price = decimal.Parse(parts[3]);
            int quantity = int.Parse(parts[4]);

            return new Furniture(id, name, category, price, quantity);
        }
    }
}
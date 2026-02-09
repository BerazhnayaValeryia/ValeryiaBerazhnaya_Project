namespace FurnitureWarehouse.Main.Configuration
{
    public static class ConfigurationManager
    {
        private static AppConfiguration? _config;

        public static void Initialize()
        {
            _config = new AppConfiguration();

            var path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Resources",
                "app.properties"
            );

            if (!File.Exists(path))
                return;

            foreach (var line in File.ReadAllLines(path))
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;

                var parts = line.Split('=', 2);
                if (parts.Length != 2) continue;

                var key = parts[0].Trim().ToLower();
                var value = parts[1].Trim();

                if (key == "inventory")
                    _config.InventoryFile = value;
            }
        }

        public static AppConfiguration GetConfiguration()
        {
            return _config ??= new AppConfiguration();
        }
    }
}
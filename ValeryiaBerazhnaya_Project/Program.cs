using FurnitureWarehouse.Main;
using FurnitureWarehouse.Main.Configuration;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            ConfigurationManager.Initialize();
            var config = ConfigurationManager.GetConfiguration();

            var startup = new Startup(config);
            var view = startup.CreateView();

            view.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Critical error occurred.");
            Console.WriteLine(ex.Message);
        }
    }
}
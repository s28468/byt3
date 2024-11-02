
namespace ConsoleApp;

public class Program
{
    private static async Task Main(string[] args)
    {
        try
        {
            await Serializer.LoadInstances();
            await Serializer.ClearFile();
            var city1 = new City("a", DateTime.Today, 11, 11);
            var deal1 = new Deal(1, DateTime.Now, DateTime.Today);
            var exported1 = new Exported(1, "ss", true, 1, 0, true, null, "fff", null, null);
            var imported1 = new Imported(1, "ss", true, 1, 0, true, null, "fff", null, null);
            var resource1 = new Resource(1, "ss", "hhh", true, 0, 0, false);
           
            var allDeals = await Deal.GetAllInstances();
            foreach (var deal in allDeals)
            {
                Console.WriteLine(deal.Id);
            }
        }
        finally
        {
            await Serializer.SerializeInstances();
        }
    }
}
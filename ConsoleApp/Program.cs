
using ConsoleApp.Helpers;
using ConsoleApp.Models;

namespace ConsoleApp;

public class Program
{
    private static async Task Main(string[] args)
    {
        Serializer.LoadInstances();
        
        Console.WriteLine(Deal.Instances.Count);
         
         var city1 = new City("New York", DateTime.Today, 500000, 600);
         var deal1 = new Deal(1, DateTime.Now, DateTime.Now.AddMonths(1));
         //var resource1 = new Resource(3, "phone", "A phone", true, 19.99m, 100, true);
         //var exported1 = new Exported(1, "ss", true, 1, 0, true, null, "fff", null, null);
         // var imported1 = new Imported(2, "ss", true, 1, 0, true, null, "fff", null, null);
         
         await Serializer.SerializeInstances();
    }
}
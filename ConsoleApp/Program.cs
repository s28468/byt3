
using System.ComponentModel.DataAnnotations;
using ConsoleApp.Helpers;
using ConsoleApp.Models;

namespace ConsoleApp;

public class Program
{
    private static async Task Main(string[] args)
    {
        Serializer.LoadInstances();
        
        Console.WriteLine(City.Instances.Count);
         
         var city1 = new City("New York", DateTime.Today, 500000, 600);
        // var deal1 = new Deal(1, DateTime.Now, DateTime.Now.AddMonths(1));
         var resource1 = new Resource(3, "phone", "A phone", true, 19.99m, 100, true);
         var exported1 = new Exported(1, "ss", true, 1, 0, true, null, "fff", null, null);
         var imported1 = new Imported(2, "ss", true, 1, 0, true, null, "fff", null, null);
         Console.WriteLine(City.Instances.Count);
         await Serializer.SerializeInstances();
        

        // var vehicle = new PublicVehicle
        // {
        //     Id = 1,
        //     Capacity = 50,
        //     Type = null // This will trigger the validation error for "Type is required."
        // };
        //
        // var validationResults = new List<ValidationResult>();
        // var context = new ValidationContext(vehicle);
        // bool isValid = Validator.TryValidateObject(vehicle, context, validationResults, true);
        //
        // if (!isValid)
        // {
        //     foreach (var validationResult in validationResults)
        //     {
        //         Console.WriteLine(validationResult.ErrorMessage);
        //     }
        // }

    }
}
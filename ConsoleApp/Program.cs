
using System.ComponentModel.DataAnnotations;
using ConsoleApp.Helpers;
using ConsoleApp.Models;

namespace ConsoleApp;

public class Program
{
    private static Task Main(string[] args)
    {
        var w1 = new Workplace("IT Company", IndustryTypeEnum.Technology, 1,
           1000000m,
            0,
            5,
            "123 Tech Lane, Silicon Valley",
           500,
           300
        );
        
        var r1 = new Imported(
            id: 1,
            name: "Electronics Widget",
            availability: true,
            price: 199.99m,
            quantity: 50,
            isExportable: true,
            importer: "Tech Imports Inc.",
            originCity: "Shanghai",
            originCertificate: "ISO9001",
            storageAddress: "Warehouse 42, Tech Park",
            description: "A high-quality electronics widget."
        );
        
        var residential = new Residential(100,  10,
        1,
             1000000m,
             0,
           5,
            "123 Tech Lane, Silicon Valley",
             500,
            300
        );
        
        
        var resident = new Resident(
            id: 1,
            firstName: "John",
            lastName: "Doe",
            passportNum: "A123456789",
            occupationStatus: OccupationStatusType.Employed
        );
        
        var resident2 = new Resident(
            id: 2,
            firstName: "John",
            lastName: "Doe",
            passportNum: "A123456789",
            occupationStatus: OccupationStatusType.Employed
        );
        
        var city = new City("Metropolis",new DateTime(1850, 1, 1),789.12,1000000);
        var resource = new Resource
        {
            Id = 1,
            Name = "Gold",
            Description = "Precious metal used for trading.",
            Availability = true,
            Price = 1500.75m,
            Quantity = 100,
            IsExportable = true
        };
        city.AddConsistsOf(w1);
        city.ModifyConsistsOf(w1, residential);
        Console.WriteLine(city.ConsistsOf.FirstOrDefault());
        
        return Task.CompletedTask;
    }
}

using System.ComponentModel.DataAnnotations;
using ConsoleApp.Helpers;
using ConsoleApp.Models;

namespace ConsoleApp;

public class Program
{
    private static Task Main(string[] args)
    {
        var w1 = new Workplace("IT Company", IndustryTypeEnum.Technology)
        {
            Id = 1,
            Price = 1000000m,
            OpeningLevel = 0,
            CurrLevel = 5,
            Address = "123 Tech Lane, Silicon Valley",
            Capacity = 500,
            Occupied = 300
        };
        
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
        
        w1.AddCreated(r1);
        r1.AddCreatedBy(w1);
        Console.WriteLine(w1.Created.Count);
        return Task.CompletedTask;
    }
}
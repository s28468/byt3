using System.ComponentModel.DataAnnotations;

namespace ConsoleApp.Models;

[Serializable]
public class ManMade: Resource
{
    private static List<ManMade> _instances = [];
    public static IReadOnlyList<ManMade> Instances => _instances.AsReadOnly();
    
    [Required(ErrorMessage = "Manufacturer is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Manufacturer name must be between 2 and 100 characters.")]
    public string Manufacturer { get; set; } 

    [Required(ErrorMessage = "Lifespan is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Lifespan must be a positive number.")]
    public required int Lifespan { get; set; } 
    
    public ManMade(){}
    
    public ManMade(
        int id,
        string name,
        bool availability,
        decimal price,
        int quantity,
        bool isExportable,
        string manufacturer,
        int lifespan,
        string? description = null
    )
        : base(id, name, description, availability, price, quantity, isExportable)
    {
        Manufacturer = manufacturer;
        Lifespan = lifespan;
        _instances.Add(this);
    }
    
    public static void AddInstance(ManMade resource)
    {
        _instances.Add(resource);
    }
}
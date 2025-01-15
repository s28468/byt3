using System.ComponentModel.DataAnnotations;

namespace ConsoleApp.Models;

[Serializable]
public class Exported: Resource
{
    private static List<Exported> _instances = [];
    public static IReadOnlyList<Exported> Instances => _instances.AsReadOnly();
    
    [Required(ErrorMessage = "Exporter is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Exporter name must be between 2 and 100 characters.")]
    public string Exporter { get; set; } 

    [Required(ErrorMessage = "Destination city is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Destination city must be between 2 and 100 characters.")]
    public string DestinationCity { get; set; } 

    [Required(ErrorMessage = "Export license is required.")]
    [StringLength(50, ErrorMessage = "Export license must not exceed 50 characters.")]
    public string ExportLicense { get; set; }
    
    public Exported(){}
    
    public Exported(
        int id,
        string name,
        bool availability,
        decimal price,
        int quantity,
        bool isExportable,
        string exporter,
        string destinationCity,
        string exportLicense,
        string? description = null
    )
        : base(id, name, description, availability, price, quantity, isExportable) 
    {
        Exporter = exporter;
        DestinationCity = destinationCity;
        ExportLicense = exportLicense;
        _instances.Add(this);
    }
    
    public Exported(Resource resource, string exporter, string destinationCity, string exportLicense)
        : base(resource.Id.Value, resource.Name, resource.Description, resource.Availability, resource.Price.Value, resource.Quantity.Value, resource.IsExportable)
    {
        RemoveInstance(resource);
        Exporter = exporter;
        DestinationCity = destinationCity;
        ExportLicense = exportLicense;
        _instances.Add(this);
    }

    public static void AddInstance(Exported resource)
    {
        _instances.Add(resource);
    }
}
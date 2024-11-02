using System.ComponentModel.DataAnnotations;

namespace ConsoleApp;

public class Imported: Resource
{
    private static List<Imported> _instances = [];
    public static IReadOnlyList<Imported> Instances => _instances.AsReadOnly();
    
    [Required(ErrorMessage = "Importer is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Importer name must be between 2 and 100 characters.")]
    public string Importer { get; set; } 

    [Required(ErrorMessage = "Origin city is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Origin city must be between 2 and 100 characters.")]
    public string OriginCity { get; set; } 

    [Required(ErrorMessage = "Origin certificate is required.")]
    [StringLength(50, ErrorMessage = "Origin certificate must not exceed 50 characters.")]
    public string OriginCertificate { get; set; } 

    [Required(ErrorMessage = "Storage address is required.")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Storage address must be between 5 and 200 characters.")]
    public string StorageAddress { get; set; }
    
    public Imported(
        int id,
        string name,
        bool availability,
        decimal price,
        int quantity,
        bool isExportable,
        string importer,
        string originCity,
        string originCertificate,
        string storageAddress,
        string? description = null
    )
        : base(id, name, description, availability, price, quantity, isExportable) 
    {
        Importer = importer;
        OriginCity = originCity;
        OriginCertificate = originCertificate;
        StorageAddress = storageAddress;
        _instances.Add(this);
    }
    
    public new static Task<List<Imported>> GetAllInstances()
    {
        return Task.FromResult(_instances);
    } 
}
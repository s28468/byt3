using System.ComponentModel.DataAnnotations;

public class ManMade: Resource
{
    private static readonly List<ManMade> _instances = [];
    public static IReadOnlyList<ManMade> Instances => _instances.AsReadOnly();
    
    [Required(ErrorMessage = "Manufacturer is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Manufacturer name must be between 2 and 100 characters.")]
    public required string Manufacturer { get; set; } 

    [Required(ErrorMessage = "Lifespan is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Lifespan must be a positive number.")]
    public required int Lifespan { get; set; } 
    
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
        _ = Serializer<ManMade>.SerializeObject(this);
    }
}

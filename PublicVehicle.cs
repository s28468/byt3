using System.ComponentModel.DataAnnotations;

public class PublicVehicle
{
    private static readonly List<PublicVehicle> _instances = [];
    public static IReadOnlyList<PublicVehicle> Instances => _instances.AsReadOnly();
    
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public required int Id { get; set; } 

    [Required(ErrorMessage = "Type is required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Type must be between 3 and 50 characters.")]
    public required string Type { get; set; } // Example: Bus, Tram, Metro, etc.

    [Required(ErrorMessage = "Capacity is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Capacity must be a positive number.")]
    public required int Capacity { get; set; }
    
    public PublicVehicle(int id, string type, int capacity)
    {
        Id = id;
        Type = type;
        Capacity = capacity;
        _instances.Add(this);
        _ = Serializer<PublicVehicle>.SerializeObject(this);
    }
}

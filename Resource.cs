using System.ComponentModel.DataAnnotations;

public abstract class Resource
{
    private static readonly List<Resource> _instances = [];
    public static IReadOnlyList<Resource> Instances => _instances.AsReadOnly(); 
    
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public required int Id { get; set; } 

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
    public required string Name { get; set; } 

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string? Description { get; set; } 

    public bool Availability { get; set; } 

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
    public required decimal Price { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
    public required int Quantity { get; set; } 

    public bool IsExportable { get; set; }

    protected Resource(int id, string name, string description, bool availability, decimal price, int quantity, bool isExportable)
    {
        Id = id;
        Name = name;
        Description = description;
        Availability = availability;
        Price = price;
        Quantity = quantity;
        IsExportable = isExportable;
        _instances.Add(this);
        _ = Serializer<Resource>.SerializeObject(this);
    }
    
    protected Resource(int id, string name, bool availability, decimal price, int quantity, bool isExportable)
    {
        Id = id;
        Name = name;
        Availability = availability;
        Price = price;
        Quantity = quantity;
        IsExportable = isExportable;
        _instances.Add(this);
        _ = Serializer<Resource>.SerializeObject(this);
    }
}

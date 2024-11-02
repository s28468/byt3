using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace ConsoleApp;

public class Resource: SerializableObject<Resource>
{
    public static IReadOnlyList<Resource> Instances => _instances.AsReadOnly(); 
    
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public int Id { get; set; } 

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
    public string Name { get; set; } 

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string? Description { get; set; } 

    public bool Availability { get; set; } 

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
    public int Quantity { get; set; } 

    public bool IsExportable { get; set; }
    
    public Resource() { }
    
    public Resource(int id, string name, string description, bool availability, decimal price, int quantity, bool isExportable)
    {
        Id = id;
        Name = name;
        Description = description;
        Availability = availability;
        Price = price;
        Quantity = quantity;
        IsExportable = isExportable;
        _instances.Add(this);
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
    }
    
     public new static Task LoadAll()
    {
        IEnumerable<Resource> loadedInstances = _instances;
        _instances.Clear();

        foreach (var instance in loadedInstances)
        {
            switch (instance)
            {
                case Exported exported:
                    _instances.Add(exported);
                    break;
                case Imported imported:
                    _instances.Add(imported);
                    break;
                case ManMade manMade:
                    _instances.Add(manMade);
                    break;
                case Natural natural:
                    _instances.Add(natural);
                    break;
                default:
                    _instances.Add(instance);
                    break;
            }
        }

        return Task.CompletedTask;
    }
}
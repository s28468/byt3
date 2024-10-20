using System.ComponentModel.DataAnnotations;

public abstract class Resource(int id, string name, string? description, bool availability, decimal price, int quantity, bool isExportable)
{
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public required int Id { get; set; } = id;

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
    public required string Name { get; set; } = name;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string? Description { get; set; } = description;

    public bool Availability { get; set; } = availability;

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
    public required decimal Price { get; set; } = price;

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
    public required int Quantity { get; set; } = quantity;

    public bool IsExportable { get; set; } = isExportable;

    protected Resource(int id, string name, bool availability, decimal price, int quantity, bool isExportable) : this(id, name, null, availability, price, quantity, isExportable)
    {
    }
}

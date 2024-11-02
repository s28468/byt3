using System.ComponentModel.DataAnnotations;

namespace ConsoleApp;

public class Natural: Resource
{
    private static readonly List<Natural> _instances = [];
   public static IReadOnlyList<Natural> Instances => _instances.AsReadOnly();
    
    [Required(ErrorMessage = "Origin is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Origin must be between 2 and 100 characters.")]
    public required string Origin { get; set; }

    [Required(ErrorMessage = "Producer is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Producer must be between 2 and 100 characters.")]
    public required string Producer { get; set; } 

    [Required(ErrorMessage = "Expiration date is required.")]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(Natural), nameof(ValidateExpirationDate))]
    public required DateTime ExpirationDate { get; set; }
    
    public Natural(){}

    public Natural(
        int id,
        string name,
        bool availability,
        decimal price,
        int quantity,
        bool isExportable,
        string origin,
        string producer,
        DateTime expirationDate,
        string? description = null
    )
        : base(id, name, description, availability, price, quantity, isExportable) 
    {
        Origin = origin;
        Producer = producer;
        ExpirationDate = expirationDate;
        _instances.Add(this);
    }
    public static ValidationResult? ValidateExpirationDate(DateTime expirationDate, ValidationContext context)
    {
        return expirationDate <= DateTime.Now ? new ValidationResult("Expiration date must be in the future.") : ValidationResult.Success;
    }
    
    public new static Task<List<Natural>> GetAllInstances()
    {
        return Task.FromResult(_instances);
    } 
}
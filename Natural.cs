using System.ComponentModel.DataAnnotations;

public class Natural (int id, string name, bool availability, decimal price, int quantity, bool isExportable, string origin, string producer, DateTime expirationDate, string? description = null)
    : Resource(id, name, description, availability, price, quantity, isExportable)
{
    [Required(ErrorMessage = "Origin is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Origin must be between 2 and 100 characters.")]
    public required string Origin { get; set; } = origin;

    [Required(ErrorMessage = "Producer is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Producer must be between 2 and 100 characters.")]
    public required string Producer { get; set; } = producer;

    [Required(ErrorMessage = "Expiration date is required.")]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(Natural), nameof(ValidateExpirationDate))]
    public required DateTime ExpirationDate { get; set; } = expirationDate;

    public static ValidationResult? ValidateExpirationDate(DateTime expirationDate, ValidationContext context)
    {
        return expirationDate <= DateTime.Now ? new ValidationResult("Expiration date must be in the future.") : ValidationResult.Success;
    }
}

using System;
using System.ComponentModel.DataAnnotations;

public class Natural : Resource
{
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

    public static ValidationResult? ValidateExpirationDate(DateTime expirationDate, ValidationContext context)
    {
        if (expirationDate <= DateTime.Now)
        {
            return new ValidationResult("Expiration date must be in the future.");
        }
        return ValidationResult.Success;
    }
}

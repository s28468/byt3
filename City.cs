using System.ComponentModel.DataAnnotations;
using System.Text.Json;

public class City (string name, DateTime dateOfFounding, double area, int population)
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
    public string Name { get; set; } = name;

    [Required(ErrorMessage = "Date of founding is required.")]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(City), nameof(ValidateDateOfFounding))]
    public DateTime DateOfFounding { get; set; } = dateOfFounding;

    [Required(ErrorMessage = "Area is required.")]
    [Range(1, double.MaxValue, ErrorMessage = "Area must be greater than zero.")]
    public double Area { get; set; } = area;

    [Required(ErrorMessage = "Population is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Population must be a positive number.")]
    public int Population { get; set; } = population;

    public static ValidationResult? ValidateDateOfFounding(DateTime dateOfFounding, ValidationContext context)
    {
        return dateOfFounding > DateTime.Now ? new ValidationResult("Date of founding cannot be in the future.") : ValidationResult.Success;
    }
}

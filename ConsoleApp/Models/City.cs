using System.ComponentModel.DataAnnotations;
using ConsoleApp.Helpers;

namespace ConsoleApp.Models;


public class City: SerializableObject<City>
{
    public static IReadOnlyList<City> Instances => _instances.AsReadOnly();
    
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Date of founding is required.")]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(City), nameof(ValidateDateOfFounding))]
    public DateTime DateOfFounding { get; set; }

    [Required(ErrorMessage = "Area is required.")]
    [Range(1, double.MaxValue, ErrorMessage = "Area must be greater than zero.")]
    public double Area { get; set; }

    [Required(ErrorMessage = "Population is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Population must be a positive number.")]
    public int Population { get; set; }
    
    public City() { }
    
    public City(string name, DateTime dateOfFounding, double area, int population)
    {
        Name = name;
        DateOfFounding = dateOfFounding;
        Area = area;
        Population = population;
        _instances.Add(this);
    }

    public static ValidationResult? ValidateDateOfFounding(DateTime dateOfFounding, ValidationContext context)
    {
        return dateOfFounding > DateTime.Now ? new ValidationResult("Date of founding cannot be in the future.") : ValidationResult.Success;
    }
}
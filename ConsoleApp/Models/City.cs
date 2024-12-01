using System.ComponentModel.DataAnnotations;
using ConsoleApp.Helpers;

namespace ConsoleApp.Models;


[Serializable]
public class City : SerializableObject<City>
{
    private static List<City> _instances = new List<City>();
    public static IReadOnlyList<City> Instances => _instances.AsReadOnly();

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Date of founding is required.")]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(City), nameof(ValidateDateOfFounding))]
    public DateTime? DateOfFounding { get; set; }

    [Required(ErrorMessage = "Area is required.")]
    [Range(1, double.MaxValue, ErrorMessage = "Area must be greater than zero.")]
    public double? Area { get; set; }

    [Required(ErrorMessage = "Population is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Population must be a positive number.")]
    public int? Population { get; set; }

    private List<Resource> _traded = new List<Resource>();
    public List<Resource> Traded => new List<Resource>(_traded);

    private List<Building> _consistsOf = new List<Building>();
    public List<Building> ConsistsOf => new List<Building>(_consistsOf);

    private List<Resource> _dealtResources = new List<Resource>();
    public List<Resource> DealtResources => new List<Resource>(_dealtResources);

    private List<Resident> _residents = new List<Resident>(); // Basic association with Resident
    public IReadOnlyList<Resident> Residents => _residents.AsReadOnly();

    public City()
    {
    }

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
        return dateOfFounding > DateTime.Now
            ? new ValidationResult("Date of founding cannot be in the future.")
            : ValidationResult.Success;
    }

    // aggregation
    public void AddTraded(Resource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource), "Resource shouldn't be null.");

        if (_traded.Contains(resource)) return;

        _traded.Add(resource);
        resource.AddTradedBy(this);
    }

    // composition
    public void AddConsistsOf(Building building)
    {
        if (building == null)
            throw new ArgumentNullException(nameof(building), "Building shouldn't be null.");

        if (_consistsOf.Contains(building)) return;

        _consistsOf.Add(building);
        building.AddIsPartOf(this);
    }

    // with attribute/class
    public void AddDealtResources(Resource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource), "Resource shouldn't be null.");

        if (_dealtResources.Contains(resource)) return;

        var d = new Deal(Deal.GetLastId(), DateTime.Now, DateTime.Now.AddDays(3));

        _dealtResources.Add(resource);
        resource.AddTradedCity(this);
    }

    // basic association with Resident
    public void AddResident(Resident resident)
    {
        if (resident == null)
            throw new ArgumentNullException(nameof(resident), "Resident shouldn't be null.");

        if (_residents.Contains(resident)) return;

        _residents.Add(resident);
    }
}
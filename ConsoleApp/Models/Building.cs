using System.ComponentModel.DataAnnotations;
using ConsoleApp.Helpers;

namespace ConsoleApp.Models;

public abstract class Building: SerializableObject<Building>
{
    private static readonly List<Building> _instances = [];
    
    public static IReadOnlyList<Building> Instances => _instances.AsReadOnly();
    
    private const int TotalLevels = 20;

    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Opening level is required.")]
    [Range(0, TotalLevels, ErrorMessage = "Opening level must be between 0 and 20.")]
    public int OpeningLevel { get; set; }

    [Required(ErrorMessage = "Current level is required.")]
    [Range(0, TotalLevels, ErrorMessage = "Current level must be between 0 and 20.")]
    public int CurrLevel { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 200 characters.")]
    public string Address { get; set; }

    [Required(ErrorMessage = "Capacity is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Capacity must be a positive number.")]
    public int Capacity { get; set; }

    [Required(ErrorMessage = "Occupied spaces are required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Occupied spaces cannot be negative.")]
    [CustomValidation(typeof(Building), nameof(ValidateOccupied))]
    public int Occupied { get; set; }

    public int FreePlaces => Capacity - Occupied;
    
    public City? IsPartOf { get; private set; }
    
    protected Building() { }
    
    protected Building(int id, decimal price, int openingLevel, int currLevel, string address, int capacity, int occupied)
    {
        Id = id;
        Price = price;
        OpeningLevel = openingLevel;
        CurrLevel = currLevel;
        Address = address;
        Capacity = capacity;
        Occupied = occupied;
        _instances.Add(this);
    }

    public static ValidationResult? ValidateOccupied(int occupied, ValidationContext context)
    {
        var instance = (Building)context.ObjectInstance;
        return occupied > instance.Capacity ? new ValidationResult("Occupied spaces cannot exceed capacity.") : ValidationResult.Success;
    }
    
    // composition
    public void AddIsPartOf(City city)
    {
        if (city == null)
            throw new ArgumentNullException(nameof(city), "City shouldn't be null.");

        if (IsPartOf != null!) return;

        IsPartOf = city;
        
        city.AddConsistsOf(this);
    }
    
    public void RemoveIsPartOf()
    {
        if (IsPartOf == null) return;
        
        var temp = IsPartOf;
        IsPartOf = null;
        _instances.Remove(this);

        temp.RemoveConsistsOf(this);
    }
    
    public void ModifyIsPartOf(City city)
    {
        if (city == null)
            throw new ArgumentNullException(nameof(city), "City shouldn't be null.");
        RemoveIsPartOf();
        AddIsPartOf(city);
    }
}

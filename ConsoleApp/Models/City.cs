﻿using System.ComponentModel.DataAnnotations;
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
    
    private List<Building> _consistsOf = [];
    public IReadOnlyList<Building> ConsistsOf => _consistsOf.AsReadOnly();

    private List<Deal> _created = [];
    public IReadOnlyList<Deal> Created => _created.AsReadOnly();

    private bool wasCreated = false;
    
    private List<Resident> _residents = []; // Basic association with Resident
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
    public void AddDeal(Resource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource), "Resource shouldn't be null.");
        
        var deal = Deal.ExistsRecent(this, resource);
       
        if (deal == null) //if recent deal doesn't exist
        {
            var newDeal = Deal.CreateDeal(this, resource);
            _created.Add(newDeal); 
            resource.AddTradedIn(this); 
        }
        else  // if exists but not added reverse connection
        {
            _created.Add(deal);
        }
        
        
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

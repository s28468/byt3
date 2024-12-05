using System.ComponentModel.DataAnnotations;
using ConsoleApp.Helpers;
using System.Collections.Generic;

namespace ConsoleApp.Models;

[Serializable]
public class Resident : SerializableObject<Resident>
{
    private static List<Resident> _instances = new List<Resident>();
    public static IReadOnlyList<Resident> Instances => _instances.AsReadOnly();

    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
    public string LastName { get; set; }

    [NoWhitespaces]
    [StringLength(20, ErrorMessage = "Passport number cannot exceed 20 characters.")]
    public string? PassportNum { get; set; }

    [Required(ErrorMessage = "Occupation status is required.")]
    public OccupationStatusType? OccupationStatus { get; set; } // Examples: Unemployed, Student, Employed, Retired

    public Residential LivesIn { get; private set; }

    public Resident Manager { get; private set; } // Reflexive association

    private Dictionary<int, Workplace> _workplaces = new(); // Qualified association

    public PublicVehicle? VehicleUsed { get; set; } // Basic association with PublicVehicle

    private List<RecreationalSpace> _recreationalSpaces = []; // Basic association with RecreationalSpace
    public IReadOnlyList<RecreationalSpace> RecreationalSpaces => _recreationalSpaces.AsReadOnly();

    private City? _city; // Basic association with City
    public City? City => _city;

    public Resident()
    {
    }

    public Resident(int id, string firstName, string lastName, string? passportNum,
        OccupationStatusType occupationStatus)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        PassportNum = passportNum;
        OccupationStatus = occupationStatus;
        _instances.Add(this);
    }

    public Resident(int id, string firstName, string lastName, OccupationStatusType occupationStatus)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        OccupationStatus = occupationStatus;
        _instances.Add(this);
    }
    
    // composition
    public void AddLivesIn(Residential residential)
    {
        if (residential == null)
            throw new ArgumentNullException(nameof(residential), "Residential building shouldn't be null.");

        if (LivesIn != null!) return;

        LivesIn = residential;
        
        residential.AddLivedInBy(this);
    }

    // reflex association
    public void SetManager(Resident manager)
    {
        if (manager == null)
            throw new ArgumentNullException(nameof(manager), "Manager shouldn't be null.");

        Manager = manager;
    }

    // qualified association
    public void AddWorkplace(int personalId, Workplace workplace)
    {
        if (workplace == null)
            throw new ArgumentNullException(nameof(workplace), "Workplace shouldn't be null.");

        if (_workplaces.ContainsKey(personalId)) return;

        _workplaces.Add(personalId, workplace);
    }

    public Workplace? GetWorkplace(int personalId)
    {
        return _workplaces.ContainsKey(personalId) ? _workplaces[personalId] : null;
    }

    // basic association with RecreationalSpace
    public void AddRecreationalSpace(RecreationalSpace recreationalSpace)
    {
        if (recreationalSpace == null)
            throw new ArgumentNullException(nameof(recreationalSpace), "Recreational space shouldn't be null.");

        if (_recreationalSpaces.Contains(recreationalSpace)) return;

        _recreationalSpaces.Add(recreationalSpace);
        recreationalSpace.AddResident(this); // Add reference to the resident in RecreationalSpace
    }

    // basic association with City
    public void SetCity(City city)
    {
        if (city == null)
            throw new ArgumentNullException(nameof(city), "City shouldn't be null.");

        _city = city;
        city.AddResident(this); // Add reference to the resident in City
    }
}
public enum OccupationStatusType
{
    Unemployed,
    Student,
    Employed,
    Retired
}

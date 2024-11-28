using System.ComponentModel.DataAnnotations;
using ConsoleApp.Helpers;

namespace ConsoleApp.Models;

[Serializable]
public class Resident : SerializableObject<Resident>
{
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

    public Residential LivesIn { get; private set; } //get copy?

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
        
        LivesIn = new Residential(residential.UnitCount, residential.FloorCount)
        {
            Id = residential.Id,
            Price = residential.Price,
            OpeningLevel = residential.OpeningLevel,
            CurrLevel = residential.CurrLevel,
            Address = residential.Address,
            Capacity = residential.Capacity,
            Occupied = residential.Occupied,
            UnitCount = residential.UnitCount,
            FloorCount = residential.FloorCount
        };
        
        residential.AddLivedInBy(this);
    }
}

public enum OccupationStatusType
{
    Unemployed,
    Student,
    Employed,
    Retired
}
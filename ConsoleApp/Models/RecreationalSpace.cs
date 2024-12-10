using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ConsoleApp.Models;

[Serializable]
public class RecreationalSpace : Building
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
    public required string? Name { get; set; } 

    [Required(ErrorMessage = "Type is required.")]
    public required RecreationalSpaceType? Type { get; set; }

    [Required(ErrorMessage = "Entry fee is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Entry fee must be a non-negative number.")]
    public required decimal? EntryFee { get; set; }

    [NoWhitespaces]
    [Required(ErrorMessage = "Facilities list is required.")]
    public required List<string>? Facilities { get; set; }

    private List<Resident> _residents = new List<Resident>(); // Basic association with Resident
    public IReadOnlyList<Resident> Residents => _residents.AsReadOnly();
    
    public RecreationalSpace(){}

    [JsonConstructor]
    public RecreationalSpace(string name, RecreationalSpaceType type, decimal entryFee, List<string> facilities,int id, decimal price, int openingLevel, int currLevel, string address, int capacity, int occupied): base(id, price, openingLevel, currLevel, address, capacity, occupied)
    {
        Name = name;
        Type = type;
        EntryFee = entryFee;
        Facilities = facilities;
    }

    // basic association with Resident
    public void AddResident(Resident resident)
    {
        if (resident == null)
            throw new ArgumentNullException(nameof(resident), "Resident shouldn't be null.");

        if (!_residents.Contains(resident))
            _residents.Add(resident);
    }

    public void RemoveResident(Resident resident)
    {
        if (_residents.Contains(resident))
            _residents.Remove(resident);
    }

    public void ModifyResident(Resident oldResident, Resident newResident)
    {
        if (newResident == null)
            throw new ArgumentNullException(nameof(newResident), "New resident shouldn't be null.");

        RemoveResident(oldResident);
        AddResident(newResident);
    }
}

public enum RecreationalSpaceType
{
    Gym,
    ThemePark,
    SwimmingPool,
    Cinema,
    Stadium
}

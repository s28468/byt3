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
    
    public RecreationalSpace(){}

    [JsonConstructor]
    public RecreationalSpace(string name, RecreationalSpaceType type, decimal entryFee, List<string> facilities)
    {
        Name = name;
        Type = type;
        EntryFee = entryFee;
        Facilities = facilities; 
        _instances.Add(this);
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
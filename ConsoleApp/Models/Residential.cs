using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ConsoleApp.Models;

namespace ConsoleApp;

public class Residential: Building
{
    [Required(ErrorMessage = "Unit count is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Unit count must be a positive number.")]
    public int UnitCount { get; set; }

    [Required(ErrorMessage = "Floor count is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Floor count must be a positive number.")]
    public int FloorCount { get; set; }
    
    public Residential() { }
    
    [JsonConstructor]
    public Residential(int unitCount, int floorCount)
    {
        UnitCount = unitCount;
        FloorCount = floorCount;
        _instances.Add(this);
    }
}
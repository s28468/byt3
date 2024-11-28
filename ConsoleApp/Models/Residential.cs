using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ConsoleApp.Models;

[Serializable]
public class Residential: Building
{
    [Required(ErrorMessage = "Unit count is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Unit count must be a positive number.")]
    public int UnitCount { get; set; }

    [Required(ErrorMessage = "Floor count is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Floor count must be a positive number.")]
    public int FloorCount { get; set; }
    
    private List<Resident> _livedInBy = [];
    public List<Resident> LivedInBy => [.._livedInBy];
    
    public Residential() { }
    
    [JsonConstructor]
    public Residential(int unitCount, int floorCount)
    {
        UnitCount = unitCount;
        FloorCount = floorCount;
        _instances.Add(this);
    }
    
    // composition (delete resident if delete this)
    public void AddLivedInBy(Resident resident)
    {
        if (resident == null)
            throw new ArgumentNullException(nameof(resident), "Resident shouldn't be null.");

        if (_livedInBy.Contains(resident)) return;
        
        _livedInBy.Add(resident);
        resident.AddLivesIn(this);
    }
}
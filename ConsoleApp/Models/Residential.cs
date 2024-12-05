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
    public Residential(int unitCount, int floorCount, int id, decimal price, int openingLevel, int currLevel, string address, int capacity, int occupied): base(id, price, openingLevel, currLevel, address, capacity, occupied)
    {
        UnitCount = unitCount;
        FloorCount = floorCount;
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
    
    public void RemoveLivedInBy(Resident resident)
    {
        if (resident == null)
            throw new ArgumentNullException(nameof(resident), "Resident shouldn't be null.");

        if (!_livedInBy.Contains(resident)) return;

        _livedInBy.Remove(resident);
        resident.RemoveLivesIn();
    }
    
    public void ModifyLivedInBy(Resident resident1, Resident resident2)
    { 
        if (resident1 == null || resident2 == null)
            throw new ArgumentNullException(nameof(resident1), "Resident shouldn't be null.");

        if (!_livedInBy.Contains(resident1)) return;

        RemoveLivedInBy(resident1);
        AddLivedInBy(resident2);
    }
}
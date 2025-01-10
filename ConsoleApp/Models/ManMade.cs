using System.ComponentModel.DataAnnotations;
using ConsoleApp.Helpers;

namespace ConsoleApp.Models;

[Serializable]
public class ManMade: SerializableObject<ManMade>, IResource
{
    public static IReadOnlyList<ManMade> Instances => _instances.AsReadOnly();
    
    [Required(ErrorMessage = "Manufacturer is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Manufacturer name must be between 2 and 100 characters.")]
    public string? Manufacturer { get; set; } 

    [Required(ErrorMessage = "Lifespan is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Lifespan must be a positive number.")]
    public int? Lifespan { get; set; } 
    
    public Resource ContainsResource { get; private set; }
    
    private List<Workplace> _createdBy = [];
    public IReadOnlyList<Workplace> CreatedBy => _createdBy.AsReadOnly();
    
    private List<Deal> _tradedIn = [];
    public IReadOnlyList<Deal> TradedIn => _tradedIn.AsReadOnly();
    
    public ManMade(){}
    
    public ManMade(
        int id,
        string name,
        bool availability,
        decimal price,
        int quantity,
        bool isExportable,
        string manufacturer,
        int lifespan,
        string? description = null
    )
    {
        ContainsResource = new Resource(id, name, description, availability, price, quantity, isExportable);

        Manufacturer = manufacturer;
        Lifespan = lifespan;
        _instances.Add(this);
    }
    
    public static void AddInstance(ManMade resource)
    {
        _instances.Add(resource);
    }
    
    // aggregation
    public void AddCreatedBy(Workplace workplace)
    {
        ((IResource)this).WorkplaceExists(workplace);
        if (_createdBy.Contains(workplace)) return;
        
        _createdBy.Add(workplace);
        workplace.AddCreated(this);
    }
    
    public void RemoveCreatedBy(Workplace workplace)
    {
        ((IResource)this).WorkplaceExists(workplace);
        
        if (!_createdBy.Contains(workplace)) return;

        _createdBy.Remove(workplace);
        workplace.RemoveCreated(this);
    }
    
    public void ModifyCreatedBy(Workplace workplace1, Workplace workplace2)
    { 
        RemoveCreatedBy(workplace1);
        AddCreatedBy(workplace2);
    }
    
    // with attribute/class
    public void AddTradedIn(City city) 
    {
        ((IResource)this).CityExists(city);

        var deal = Deal.ExistsRecent(city, this);
        
        if (deal == null) //if recent deal doesn't exist
        {
            var newDeal = Deal.CreateDeal(city, this);
            _tradedIn.Add(newDeal);
            city.AddDeal(this); 
        }
        else // if exists but not added reverse connection
        {
            _tradedIn.Add(deal);
        }
    }
    
    public void RemoveTradedIn(City city, bool isRecursive = false)
    {
        ((IResource)this).CityExists(city);

        if (isRecursive) return; 

        foreach (var deal in _tradedIn.Where(deal => deal.CreatedBy == city).ToList())
        {
            Deal._instances.Remove(deal);
            _tradedIn.Remove(deal);
        }
        
        city.RemoveDeal(this, true);
    }
    
    public void ModifyTradedIn(City city1, City city2) 
    {
        RemoveTradedIn(city1);
        AddTradedIn(city2);
    }
}
using System.ComponentModel.DataAnnotations;
using ConsoleApp.Helpers;

namespace ConsoleApp.Models;

[Serializable]
public class Resource: SerializableObject<Resource>, IResource
{
    public static IReadOnlyList<Resource> Instances => _instances.AsReadOnly(); 
    
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public int? Id { get; set; } 

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
    public string? Name { get; set; } 

    [NoWhitespaces]
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string? Description { get; set; } 

    public bool Availability { get; set; } 

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
    public decimal? Price { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
    public int? Quantity { get; set; } 

    public bool IsExportable { get; set; }
    
    // discriminator
    public ResourceType Type { get; private set; } = ResourceType.None;

    // Natural
    public string? Origin { get; private set; }
    public string? Producer { get; private set; }
    public DateTime? ExpirationDate { get; private set; }

    // ManMade
    public string? Manufacturer { get; private set; }
    public int? Lifespan { get; private set; }
    
    private List<Workplace> _createdBy = [];
    public IReadOnlyList<Workplace> CreatedBy => _createdBy.AsReadOnly();
    
    private List<Deal> _tradedIn = [];
    public IReadOnlyList<Deal> TradedIn => _tradedIn.AsReadOnly();
    
    public Resource() { }
    
    public Resource(int id, string name, string description, bool availability, decimal price, int quantity, bool isExportable)
    {
        Id = id;
        Name = name;
        Description = description;
        Availability = availability;
        Price = price;
        Quantity = quantity;
        IsExportable = isExportable;
        _instances.Add(this);
    }
    
    protected Resource(int id, string name, bool availability, decimal price, int quantity, bool isExportable)
    {
        Id = id;
        Name = name;
        Availability = availability;
        Price = price;
        Quantity = quantity;
        IsExportable = isExportable;
        _instances.Add(this);
    }
    
    public static void SortSubclasses(List<Resource> resources)
    {
        _instances.Clear();
        foreach (var instance in resources)
        {
            switch (instance)
            {
                case Exported exported: 
                    Exported.AddInstance(exported);
                    break;
                case Imported imported:
                    Imported.AddInstance(imported);
                    break;
            }
        }
    }
    
    public void ChangeToNatural(string origin, string producer, DateTime expirationDate)
    {
        Type = ResourceType.Natural;
        Origin = origin;
        Producer = producer;
        ExpirationDate = expirationDate;

        Manufacturer = null;
        Lifespan = null;
    }

    public void ChangeToManMade(string manufacturer, int lifespan)
    {
        Type = ResourceType.ManMade;
        Manufacturer = manufacturer;
        Lifespan = lifespan;

        Origin = null;
        Producer = null;
        ExpirationDate = null;
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

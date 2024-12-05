using System.ComponentModel.DataAnnotations;
using ConsoleApp.Helpers;

namespace ConsoleApp.Models;

[Serializable]
public class Deal: SerializableObject<Deal>
{
    public static IReadOnlyList<Deal> Instances => _instances.AsReadOnly();
    
    [Required(ErrorMessage = "Id is required.")]
    public int? Id { get; set; } 

    [Required(ErrorMessage = "Start date is required.")]
    public DateTime? StartDate { get; set; } 

    [Required(ErrorMessage = "End date is required.")]
    [CustomValidation(typeof(Deal), nameof(ValidateEndDate))]
    public DateTime? EndDate { get; set; }
    
    public City CreatedBy { get; private set; }
    public Resource Traded { get; private set; }
    
    public Deal() { }

    public Deal(int id, DateTime startDate, DateTime endDate, City createdBy, Resource traded)
    {
        Id = id;
        StartDate = startDate;
        EndDate = endDate;
        CreatedBy = createdBy;
        Traded = traded;
        _instances.Add(this);
    }

    public static int GetLastId()
    {
        return Instances.Any() ? (int)Instances[^1].Id! : 1;
    }

    public static ValidationResult? ValidateEndDate(DateTime endDate, ValidationContext context)
    {
        var instance = (Deal)context.ObjectInstance;

        return endDate <= instance.StartDate ? new ValidationResult("End date must be later than the start date.") : ValidationResult.Success;
    }

    // deals will be generated every 30 minutes; no deals for same resources simultaneously may exist
    public static Deal? ExistsRecent(City city, Resource resource)
    {
         return _instances.FirstOrDefault(d => d.CreatedBy == city && d.Traded == resource && d.StartDate >= DateTime.Now.AddMinutes(-30));
    }

    public static Deal CreateDeal(City city, Resource resource)
    {
        return new Deal(GetLastId(), DateTime.Now, DateTime.Now.AddDays(3), city, resource);
    }
    
}
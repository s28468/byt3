using System.ComponentModel.DataAnnotations;

namespace ConsoleApp;

public class Deal: SerializableObject<Deal>
{
    public static IReadOnlyList<Deal> Instances => _instances.AsReadOnly();
    
    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; set; } 

    [Required(ErrorMessage = "Start date is required.")]
    public DateTime StartDate { get; set; } 

    [Required(ErrorMessage = "End date is required.")]
    [CustomValidation(typeof(Deal), nameof(ValidateEndDate))]
    public DateTime EndDate { get; set; }
    
    public Deal() { }

    public Deal(int id, DateTime startDate, DateTime endDate)
    {
        Id = id;
        StartDate = startDate;
        EndDate = endDate;
        _instances.Add(this);
    }

    public static ValidationResult? ValidateEndDate(DateTime endDate, ValidationContext context)
    {
        var instance = (Deal)context.ObjectInstance;

        return endDate <= instance.StartDate ? new ValidationResult("End date must be later than the start date.") : ValidationResult.Success;
    } 
}
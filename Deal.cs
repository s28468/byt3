using System.ComponentModel.DataAnnotations;

public class Deal (int id, DateTime startDate, DateTime endDate)
{
    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; set; } = id;

    [Required(ErrorMessage = "Start date is required.")]
    public DateTime StartDate { get; set; } = startDate;

    [Required(ErrorMessage = "End date is required.")]
    [CustomValidation(typeof(Deal), nameof(ValidateEndDate))]
    public DateTime EndDate { get; set; } = endDate;
    
    public static ValidationResult? ValidateEndDate(DateTime endDate, ValidationContext context)
    {
        var instance = (Deal)context.ObjectInstance;

        return endDate <= instance.StartDate ? new ValidationResult("End date must be later than the start date.") : ValidationResult.Success;
    } 
}

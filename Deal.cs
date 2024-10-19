using System;
using System.ComponentModel.DataAnnotations;

public class Deal
{
    [Required(ErrorMessage = "Id is required.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Start date is required.")]
    public DateTime StartDate { get; set; }

    private DateTime endDate;

    [Required(ErrorMessage = "End date is required.")]
    [CustomValidation(typeof(Deal), nameof(ValidateEndDate))]
    public DateTime EndDate
    {
        get => endDate;
        set => endDate = value;
    }

    public static ValidationResult? ValidateEndDate(DateTime endDate, ValidationContext context)
    {
        var instance = (Deal)context.ObjectInstance;

        if (endDate <= instance.StartDate)
        {
            return new ValidationResult("End date must be later than the start date.");
        }

        return ValidationResult.Success;
    }
}

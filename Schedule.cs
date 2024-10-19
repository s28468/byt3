using System;
using System.ComponentModel.DataAnnotations;

public class Schedule
{
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public required int Id { get; set; }

    [Required(ErrorMessage = "Start time is required.")]
    public required TimeSpan StartTime { get; set; }

    [Required(ErrorMessage = "End time is required.")]
    [CustomValidation(typeof(Schedule), nameof(ValidateEndTime))]
    public required TimeSpan EndTime { get; set; }

    [Required(ErrorMessage = "Frequency is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Frequency must be a positive number.")]
    public required int Frequency { get; set; }

    public static ValidationResult? ValidateEndTime(TimeSpan endTime, ValidationContext context)
    {
        var instance = (Schedule)context.ObjectInstance;
        if (endTime <= instance.StartTime)
        {
            return new ValidationResult("End time must be later than start time.");
        }
        return ValidationResult.Success;
    }
}

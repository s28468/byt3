using System.ComponentModel.DataAnnotations;

public class Schedule (int id, DateTime startTime, DateTime endTime, int frequency)
{
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public required int Id { get; set; } = id;

    [Required(ErrorMessage = "Start time is required.")]
    public required DateTime StartTime { get; set; } = startTime;

    [Required(ErrorMessage = "End time is required.")]
    [CustomValidation(typeof(Schedule), nameof(ValidateEndTime))]
    public required DateTime EndTime { get; set; } = endTime;

    [Required(ErrorMessage = "Frequency is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Frequency must be a positive number.")]
    public required int Frequency { get; set; } = frequency;

    public static ValidationResult? ValidateEndTime(DateTime endTime, ValidationContext context)
    {
        var instance = (Schedule)context.ObjectInstance;
        return endTime <= instance.StartTime ? new ValidationResult("End time must be later than start time.") : ValidationResult.Success;
    }
}

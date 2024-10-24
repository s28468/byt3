using System.ComponentModel.DataAnnotations;

public class Schedule
{
    private static readonly List<Schedule> _instances = [];
    public static IReadOnlyList<Schedule> Instances => _instances.AsReadOnly(); 
    
    
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public required int Id { get; set; } 

    [Required(ErrorMessage = "Start time is required.")]
    public required DateTime StartTime { get; set; }

    [Required(ErrorMessage = "End time is required.")]
    [CustomValidation(typeof(Schedule), nameof(ValidateEndTime))]
    public required DateTime EndTime { get; set; } 

    [Required(ErrorMessage = "Frequency is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Frequency must be a positive number.")]
    public required int Frequency { get; set; } 

    protected Schedule(int id, DateTime startTime, DateTime endTime, int frequency)
    {
        Id = id;
        StartTime = startTime;
        EndTime = endTime;
        Frequency = frequency;
        _instances.Add(this);
        _ = Serializer<Schedule>.SerializeObject(this);
    }
    
    protected static ValidationResult? ValidateEndTime(DateTime endTime, ValidationContext context)
    {
        var instance = (Schedule)context.ObjectInstance;
        return endTime <= instance.StartTime ? new ValidationResult("End time must be later than start time.") : ValidationResult.Success;
    }
}

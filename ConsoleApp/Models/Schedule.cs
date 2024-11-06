using System.ComponentModel.DataAnnotations;
using ConsoleApp.Helpers;

namespace ConsoleApp;

public class Schedule: SerializableObject<Schedule>
{
    public static IReadOnlyList<Schedule> Instances => _instances.AsReadOnly(); 
    
    
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public int Id { get; set; } 

    [Required(ErrorMessage = "Start time is required.")]
    public DateTime StartTime { get; set; }

    [Required(ErrorMessage = "End time is required.")]
    [CustomValidation(typeof(Schedule), nameof(ValidateEndTime))]
    public DateTime EndTime { get; set; } 

    [Required(ErrorMessage = "Frequency is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Frequency must be a positive number.")]
    public int Frequency { get; set; } 
    
    public Schedule(){}

    protected Schedule(int id, DateTime startTime, DateTime endTime, int frequency)
    {
        Id = id;
        StartTime = startTime;
        EndTime = endTime;
        Frequency = frequency;
        _instances.Add(this);
    }
    
    protected static ValidationResult? ValidateEndTime(DateTime endTime, ValidationContext context)
    {
        var instance = (Schedule)context.ObjectInstance;
        return endTime <= instance.StartTime ? new ValidationResult("End time must be later than start time.") : ValidationResult.Success;
    }
}
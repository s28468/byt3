using System.ComponentModel.DataAnnotations;

namespace ConsoleApp;

public class Route: SerializableObject<Route>
{
    public static IReadOnlyList<Route> Instances => _instances.AsReadOnly(); 
    
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Start point is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Start point must be between 2 and 100 characters.")]
    public string StartPoint { get; set; } 

    [Required(ErrorMessage = "End point is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "End point must be between 2 and 100 characters.")]
    public string EndPoint { get; set; }

    [Required(ErrorMessage = "Stop count is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Stop count must be a positive number.")]
    public int StopCount { get; set; } 

    [Required(ErrorMessage = "Duration is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Duration must be a positive number.")]
    public int Duration { get; set; } // Duration in minutes
    
    public Route() { }
    
    protected Route(int id, string startPoint, string endPoint, int stopCount, int duration)
    {
        Id = id;
        StartPoint = startPoint;
        EndPoint = endPoint;
        StopCount = stopCount;
        Duration = duration;
        _instances.Add(this);
    }
}
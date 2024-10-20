using System.ComponentModel.DataAnnotations;

public class Route (int id, string startPoint, string endPoint, int stopCount, int duration)
{
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public required int Id { get; set; } = id;

    [Required(ErrorMessage = "Start point is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Start point must be between 2 and 100 characters.")]
    public required string StartPoint { get; set; } = startPoint;

    [Required(ErrorMessage = "End point is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "End point must be between 2 and 100 characters.")]
    public required string EndPoint { get; set; } = endPoint;

    [Required(ErrorMessage = "Stop count is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Stop count must be a positive number.")]
    public required int StopCount { get; set; } = stopCount;

    [Required(ErrorMessage = "Duration is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Duration must be a positive number.")]
    public required int Duration { get; set; } = duration; // Duration in minutes
}

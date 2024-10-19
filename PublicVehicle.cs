using System;
using System.ComponentModel.DataAnnotations;

public class PublicVehicle
{
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public required int Id { get; set; }

    [Required(ErrorMessage = "Type is required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Type must be between 3 and 50 characters.")]
    public required string Type { get; set; }  // Example: Bus, Tram, Metro, etc.

    [Required(ErrorMessage = "Capacity is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Capacity must be a positive number.")]
    public required int Capacity { get; set; }
}

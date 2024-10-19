using System;
using System.ComponentModel.DataAnnotations;

public class ManMade : Resource
{
    [Required(ErrorMessage = "Manufacturer is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Manufacturer name must be between 2 and 100 characters.")]
    public required string Manufacturer { get; set; }

    [Required(ErrorMessage = "Lifespan is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Lifespan must be a positive number.")]
    public required int Lifespan { get; set; }
}

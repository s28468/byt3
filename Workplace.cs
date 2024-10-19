using System;
using System.ComponentModel.DataAnnotations;

public class Workplace : Building
{
    [Range(1, 24, ErrorMessage = "Work hours must be between 1 and 24.")]
    public int WorkHours { get; set; } = 8;  // Default value is 8 hours

    [Required(ErrorMessage = "Company name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Company name must be between 2 and 100 characters.")]
    public required string CompanyName { get; set; }

    [Required(ErrorMessage = "Industry type is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Industry type must be between 2 and 50 characters.")]
    public required string IndustryType { get; set; }
}

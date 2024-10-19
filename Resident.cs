using System;
using System.ComponentModel.DataAnnotations;

public class Resident
{
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public required int Id { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
    public required string LastName { get; set; }

    [StringLength(20, ErrorMessage = "Passport number cannot exceed 20 characters.")]
    public string? PassportNum { get; set; }

    [Required(ErrorMessage = "Occupation status is required.")]
    [StringLength(30, MinimumLength = 2, ErrorMessage = "Occupation status must be between 2 and 30 characters.")]
    public required string OccupationStatus { get; set; } // Examples: Unemployed, Student, Employed, Retired
}

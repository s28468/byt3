using System;
using System.ComponentModel.DataAnnotations;

public class Residential : Building
{
    [Required(ErrorMessage = "Unit count is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Unit count must be a positive number.")]
    public required int UnitCount { get; set; }

    [Required(ErrorMessage = "Floor count is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Floor count must be a positive number.")]
    public required int FloorCount { get; set; }
}

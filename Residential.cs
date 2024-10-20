using System;
using System.ComponentModel.DataAnnotations;

public class Residential (int unitCount, int floorCount): Building
{
    [Required(ErrorMessage = "Unit count is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Unit count must be a positive number.")]
    public required int UnitCount { get; set; } = unitCount;

    [Required(ErrorMessage = "Floor count is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Floor count must be a positive number.")]
    public required int FloorCount { get; set; } = floorCount;
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class RecreationalSpace : Building
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Type is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Type must be between 2 and 50 characters.")]
    public required string Type { get; set; }

    [Required(ErrorMessage = "Entry fee is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Entry fee must be a non-negative number.")]
    public required decimal EntryFee { get; set; }

    [Required(ErrorMessage = "Facilities list is required.")]
    public required List<string> Facilities { get; set; } = new List<string>();
}

using System;
using System.ComponentModel.DataAnnotations;

public class Exported : Resource
{
    [Required(ErrorMessage = "Exporter is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Exporter name must be between 2 and 100 characters.")]
    public required string Exporter { get; set; }

    [Required(ErrorMessage = "Destination city is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Destination city must be between 2 and 100 characters.")]
    public required string DestinationCity { get; set; }

    [Required(ErrorMessage = "Export license is required.")]
    [StringLength(50, ErrorMessage = "Export license must not exceed 50 characters.")]
    public required string ExportLicense { get; set; }
}

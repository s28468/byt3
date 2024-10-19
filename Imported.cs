using System;
using System.ComponentModel.DataAnnotations;

public class Imported : Resource
{
    [Required(ErrorMessage = "Importer is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Importer name must be between 2 and 100 characters.")]
    public required string Importer { get; set; }

    [Required(ErrorMessage = "Origin city is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Origin city must be between 2 and 100 characters.")]
    public required string OriginCity { get; set; }

    [Required(ErrorMessage = "Origin certificate is required.")]
    [StringLength(50, ErrorMessage = "Origin certificate must not exceed 50 characters.")]
    public required string OriginCertificate { get; set; }

    [Required(ErrorMessage = "Storage address is required.")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Storage address must be between 5 and 200 characters.")]
    public required string StorageAddress { get; set; }
}

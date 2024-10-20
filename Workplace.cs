using System.ComponentModel.DataAnnotations;

public class Workplace (string companyName, string industryType): Building
{
    private const int WorkHours = 8;

    [Required(ErrorMessage = "Company name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Company name must be between 2 and 100 characters.")]
    public required string CompanyName { get; set; } = companyName;

    [Required(ErrorMessage = "Industry type is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Industry type must be between 2 and 50 characters.")]
    public required string IndustryType { get; set; } = industryType;
}

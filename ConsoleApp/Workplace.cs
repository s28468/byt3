using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ConsoleApp;

public class Workplace: Building
{
    private const int WorkHours = 8;

    [Required(ErrorMessage = "Company name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Company name must be between 2 and 100 characters.")]
    public string CompanyName { get; set; } 

    [Required(ErrorMessage = "Industry type is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Industry type must be between 2 and 50 characters.")]
    public string IndustryType { get; set; } 
    
    public Workplace() { }
    
    [JsonConstructor]
    protected Workplace(string companyName, string industryType)
    {
        CompanyName = companyName;
        IndustryType = industryType;
        _instances.Add(this);
    }
    
}
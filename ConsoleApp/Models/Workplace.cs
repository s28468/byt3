using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ConsoleApp.Models;

namespace ConsoleApp;

public class Workplace : Building
{
    private const int WorkHours = 8;

    [Required(ErrorMessage = "Company name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Company name must be between 2 and 100 characters.")]
    public string CompanyName { get; set; } 

    [Required(ErrorMessage = "Industry type is required.")]
    public IndustryTypeEnum IndustryType { get; set; }
    
    public Workplace() { }
    
    [JsonConstructor]
    public Workplace(string companyName, IndustryTypeEnum industryType)
    {
        CompanyName = companyName;
        IndustryType = industryType;
        _instances.Add(this);
    }
}

public enum IndustryTypeEnum
{
    Medical,
    Manufacturing,
    Education,
    Technology,
    Finance
}

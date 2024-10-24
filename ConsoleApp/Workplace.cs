using System.ComponentModel.DataAnnotations;
using ConsoleApp;

public class Workplace: Building
{
    private static readonly List<Workplace> _instances = [];
    public static IReadOnlyList<Workplace> Instances => _instances.AsReadOnly(); 
    
    private const int WorkHours = 8;

    [Required(ErrorMessage = "Company name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Company name must be between 2 and 100 characters.")]
    public required string CompanyName { get; set; } 

    [Required(ErrorMessage = "Industry type is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Industry type must be between 2 and 50 characters.")]
    public required string IndustryType { get; set; } 
    
    protected Workplace(string companyName, string industryType)
    {
        CompanyName = companyName;
        IndustryType = industryType;
        _instances.Add(this);
        _ = Serializer<Workplace>.SerializeObject(this);
    }
    
}

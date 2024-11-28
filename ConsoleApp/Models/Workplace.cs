using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ConsoleApp.Models;

[Serializable]
public class Workplace : Building
{
    private const int WorkHours = 8;

    [Required(ErrorMessage = "Company name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Company name must be between 2 and 100 characters.")]
    public string CompanyName { get; set; } 

    [Required(ErrorMessage = "Industry type is required.")]
    public IndustryTypeEnum IndustryType { get; set; }

    private List<Resource> _created = []; 

    public List<Resource> Created => [.._created];

    public Workplace() { }
   
    public Workplace(string companyName, IndustryTypeEnum industryType)
    {
        CompanyName = companyName;
        IndustryType = industryType;
        _instances.Add(this);
    }
    
    // aggregation
    public void AddCreated(Resource resource)
    {
        if (resource == null) 
            throw new ArgumentNullException(nameof(resource), "Resource shouldn't be null.");

        if (_created.Contains(resource)) return;
        
        _created.Add(resource);
        resource.AddCreatedBy(this);
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

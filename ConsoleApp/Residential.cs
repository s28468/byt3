using System.ComponentModel.DataAnnotations;
using ConsoleApp;

public class Residential: Building
{
    private static readonly List<Residential> _instances = [];
    public static IReadOnlyList<Residential> Instances => _instances.AsReadOnly();
    
    [Required(ErrorMessage = "Unit count is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Unit count must be a positive number.")]
    public required int UnitCount { get; set; }

    [Required(ErrorMessage = "Floor count is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Floor count must be a positive number.")]
    public required int FloorCount { get; set; }
    
    public Residential(int unitCount, int floorCount)
    {
        UnitCount = unitCount;
        FloorCount = floorCount;
        _instances.Add(this);
        _ = Serializer<Residential>.SerializeObject(this);
    }
}

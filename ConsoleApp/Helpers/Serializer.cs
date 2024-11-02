using System.Text.Json;

namespace ConsoleApp;

public abstract class Serializer
{
    private const string Filename = "Instances.xml";
    public static async Task ClearFile()
    {
        if (File.Exists(Filename))
        {
            File.Delete(Filename); 
        }
        await using var stream = File.Create(Filename);
    }

    
    public static async Task SerializeInstances()
    {
        await City.SerializeAll();
        await Deal.SerializeAll();
        await PublicVehicle.SerializeAll();
        await RecreationalSpace.SerializeAll();
        await Resident.SerializeAll();
        await Residential.SerializeAll();
        await Resource.SerializeAll();
        await Route.SerializeAll();
        await Schedule.SerializeAll();
        await Workplace.SerializeAll();
    }

    public static async Task LoadInstances()
    {
        if (File.Exists(Filename))
        {
            await City.LoadAll();
            await Deal.LoadAll();
            await PublicVehicle.LoadAll();
            await RecreationalSpace.LoadAll();
            await Resident.LoadAll();
            await Residential.LoadAll();
            await Resource.LoadAll();
            await Route.LoadAll();
            await Schedule.LoadAll();
            await Workplace.LoadAll();
        }
    }
}
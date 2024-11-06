using System.Text.Json;

namespace ConsoleApp;

public abstract class Serializer
{
    //private const string Filename = "Instances.xml";
    public static async Task ClearFile(string filename)
    {
        if (File.Exists(filename))
        {
            File.Delete(filename); 
        }
        await using var stream = File.Create(filename);
    }

    
    public static async Task SerializeInstances(string filename)
    {
        await City.SerializeAll(filename);
        await Deal.SerializeAll(filename);
        await PublicVehicle.SerializeAll(filename);
        await RecreationalSpace.SerializeAll(filename);
        await Resident.SerializeAll(filename);
        await Residential.SerializeAll(filename);
        await Resource.SerializeAll(filename);
        await Route.SerializeAll(filename);
        await Schedule.SerializeAll(filename);
        await Workplace.SerializeAll(filename);
    }

    public static async Task LoadInstances(string filename)
    {
        if (File.Exists(filename))
        {
            await City.LoadAll(filename);
            await Deal.LoadAll(filename);
            await PublicVehicle.LoadAll(filename);
            await RecreationalSpace.LoadAll(filename);
            await Resident.LoadAll(filename);
            await Residential.LoadAll(filename);
            await Resource.LoadAll(filename);
            await Route.LoadAll(filename);
            await Schedule.LoadAll(filename);
            await Workplace.LoadAll(filename);
        }
    }
}
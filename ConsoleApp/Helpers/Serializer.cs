using ConsoleApp.Models;

namespace ConsoleApp.Helpers
{
    public abstract class Serializer
    {
        public static async Task SerializeInstances()
        {
            await City.SerializeAll();
            await Deal.SerializeAll();
            await PublicVehicle.SerializeAll();
            await RecreationalSpace.SerializeAll();
            await Resident.SerializeAll();
            await Residential.SerializeAll();
            await Resource.SerializeAll();
            await ManMade.SerializeAll();
            await Natural.SerializeAll();
            await Route.SerializeAll();
            await Schedule.SerializeAll();
            await Workplace.SerializeAll();
        }

        public static void LoadInstances()
        {
            City.LoadAll();
            Deal.LoadAll();
            PublicVehicle.LoadAll();
            RecreationalSpace.LoadAll();
            Resident.LoadAll();
            Residential.LoadAll();
            Resource.LoadAll();
            ManMade.LoadAll();
            Natural.LoadAll();
            Route.LoadAll();
            Schedule.LoadAll();
            Workplace.LoadAll();
        }
    }
    
}

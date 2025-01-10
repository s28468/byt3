
using ConsoleApp.Models;

namespace ConsoleApp.Helpers;

public interface IResource
{
    void AddCreatedBy(Workplace workplace);
    void RemoveCreatedBy(Workplace workplace); 
    void ModifyCreatedBy(Workplace workplace1, Workplace workplace2);
    void AddTradedIn(City city); 
    void RemoveTradedIn(City city, bool isRecursive = false); 
    void ModifyTradedIn(City city1, City city2);

    void WorkplaceExists(Workplace workplace)
    {
        if (workplace == null) 
            throw new ArgumentNullException(nameof(workplace), "Workplace shouldn't be null.");
    }
    
    void CityExists(City city)
    {
        if (city == null)
            throw new ArgumentNullException(nameof(city), "City shouldn't be null.");
    }
}

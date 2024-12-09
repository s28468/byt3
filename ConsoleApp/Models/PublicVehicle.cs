using System.ComponentModel.DataAnnotations;
using ConsoleApp.Helpers;
using System.Collections.Generic;
using System.Data.SqlTypes;

namespace ConsoleApp.Models;

[Serializable]
public class PublicVehicle : SerializableObject<PublicVehicle>
{
    private static List<PublicVehicle> _instances = new List<PublicVehicle>();
    public static IReadOnlyList<PublicVehicle> Instances => _instances.AsReadOnly();
    
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public int Id { get; set; } 

    [Required(ErrorMessage = "Type is required.")]
    public VehicleType? Type { get; set; } // Example: Bus, Tram, Metro, etc.

    [Required(ErrorMessage = "Capacity is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Capacity must be a positive number.")]
    public int Capacity { get; set; }
    
    private List<Schedule> _follows = [];
    public IReadOnlyList<Schedule> Follows => _follows.AsReadOnly();

    private List<Resident> _residents = new List<Resident>(); // Basic association with Resident
    public IReadOnlyList<Resident> Residents => _residents.AsReadOnly(); // Read-only access to the list of Residents

    public Route? HasRoute { get; private set; }
    
    public PublicVehicle() { }
    
    public PublicVehicle(int id, VehicleType type, int capacity)
    {
        Id = id;
        Type = type;
        Capacity = capacity;
        _instances.Add(this);
    }
    
    // aggregation
    public void AddFollows(Schedule schedule)
    {
        if (schedule == null)
            throw new ArgumentNullException(nameof(schedule), "Schedule shouldn't be null.");

        // check if schedules overlap
        if (_follows.Any(existing => existing.StartTime < schedule.EndTime && schedule.StartTime < existing.EndTime))
        {
            throw new InvalidOperationException("Schedules shouldn't overlap.");
        }

        if (_follows.Contains(schedule)) return;
        
        _follows.Add(schedule);
        schedule.AddFollowedBy(this);
    }
    
    public void RemoveFollows(Schedule schedule)
    {
        if (schedule == null)
            throw new ArgumentNullException(nameof(schedule), "Schedule shouldn't be null.");

        if (!_follows.Contains(schedule)) return;

        _follows.Remove(schedule);
        schedule.RemoveFollowedBy(this);
    }
    
    public void ModifyFollows(Schedule schedule1, Schedule schedule2)
    {
        if (schedule1 == null || schedule2 == null)
            throw new ArgumentNullException(nameof(schedule1), "Schedule shouldn't be null.");

        if (!_follows.Contains(schedule1)) return;

        RemoveFollows(schedule1);
        AddFollows(schedule2);    
    }
    
    // composition
    public void AddHasRoute(Route route)
    {
        if (route == null)
            throw new ArgumentNullException(nameof(route), "Route shouldn't be null.");

        if (HasRoute != null!) return;

        HasRoute = route;
        
        route.AddFollowedBy(this);
    }
    
    public void RemoveHasRoute()
    {
        if (HasRoute == null) return;
        
        var temp = HasRoute;
        HasRoute = null;
        
        foreach (var schedule in Follows)
        {
            RemoveFollows(schedule);
        }

        _instances.Remove(this);

        temp.RemoveFollowedBy(this);
    }
    
    public void ModifyHasRoute(Route route)
    {
        if (route == null)
            throw new ArgumentNullException(nameof(route), "Route shouldn't be null.");
        
        RemoveHasRoute();
        AddHasRoute(route);
    }

    // Basic association with Resident
    public void AddResident(Resident resident)
    {
        if (resident == null)
            throw new ArgumentNullException(nameof(resident), "Resident shouldn't be null.");

        if (_residents.Contains(resident)) return;

        _residents.Add(resident);
        resident.VehicleUsed = this;
    }

    public void RemoveResident(Resident resident)
    {
        if (resident == null || !_residents.Contains(resident)) return;

        _residents.Remove(resident);
        resident.VehicleUsed = null;
    }

    public void ModifyResident(Resident oldResident, Resident newResident)
    {
        if (newResident == null)
            throw new ArgumentNullException(nameof(newResident), "New resident shouldn't be null.");

        RemoveResident(oldResident);
        AddResident(newResident);
    }
}

public enum VehicleType
{
    Bus,
    Tram,
    Metro
}

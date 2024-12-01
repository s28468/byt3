using System.ComponentModel.DataAnnotations;
using ConsoleApp.Helpers;
using System.Collections.Generic;

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
    
    private List<Schedule> _follows = new List<Schedule>();
    public List<Schedule> Follows => new List<Schedule>(_follows);

    private List<Resident> _residents = new List<Resident>(); // Basic association with Resident
    public IReadOnlyList<Resident> Residents => _residents.AsReadOnly(); // Read-only access to the list of Residents

    public Route HasRoute { get; private set; }
    
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
    
    public void AddHasRoute(Route route)
    {
        if (route == null)
            throw new ArgumentNullException(nameof(route), "Route shouldn't be null.");

        if (HasRoute != null!) return;

        HasRoute = new Route
        {
           Id = route.Id,
           StartPoint = route.StartPoint,
           Duration = route.Duration,
           EndPoint = route.EndPoint,
           StopCount = route.StopCount
        };
        
        route.AddFollowedBy(this);
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
}

public enum VehicleType
{
    Bus,
    Tram,
    Metro
}

﻿using System.ComponentModel.DataAnnotations;
using ConsoleApp.Helpers;

namespace ConsoleApp.Models;

[Serializable]
public class Route: SerializableObject<Route>
{
    public static IReadOnlyList<Route> Instances => _instances.AsReadOnly(); 
    
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Start point is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Start point must be between 2 and 100 characters.")]
    public string StartPoint { get; set; } 

    [Required(ErrorMessage = "End point is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "End point must be between 2 and 100 characters.")]
    public string EndPoint { get; set; }

    [Required(ErrorMessage = "Stop count is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Stop count must be a positive number.")]
    public int StopCount { get; set; } 

    [Required(ErrorMessage = "Duration is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Duration must be a positive number.")]
    public int Duration { get; set; } // Duration in minutes
    
    private List<PublicVehicle> _followedBy = [];
    public List<PublicVehicle> FollowedBy => [.._followedBy];
    
    public Route() { }
    
    protected Route(int id, string startPoint, string endPoint, int stopCount, int duration)
    {
        Id = id;
        StartPoint = startPoint;
        EndPoint = endPoint;
        StopCount = stopCount;
        Duration = duration;
        _instances.Add(this);
    }
    
    // composition
    public void AddFollowedBy (PublicVehicle vehicle)
    {
        if (vehicle == null)
            throw new ArgumentNullException(nameof(vehicle), "Vehicle shouldn't be null.");

        if (_followedBy.Contains(vehicle)) return;
        
        _followedBy.Add(vehicle);
        vehicle.AddHasRoute(this);
    }
    
    public void RemoveFollowedBy (PublicVehicle vehicle)
    {
        if (vehicle == null)
            throw new ArgumentNullException(nameof(vehicle), "Vehicle shouldn't be null.");

        if (!_followedBy.Contains(vehicle)) return;

        _followedBy.Remove(vehicle);
        vehicle.RemoveHasRoute();
    }
    
    public void ModifyFollowedBy (PublicVehicle vehicle1, PublicVehicle vehicle2)
    { 
        if (vehicle1 == null || vehicle2 == null)
            throw new ArgumentNullException(nameof(vehicle1), "Vehicle shouldn't be null.");

        if (!_followedBy.Contains(vehicle1)) return;

        RemoveFollowedBy(vehicle1);
        AddFollowedBy(vehicle2);
    }
}
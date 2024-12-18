﻿using System.ComponentModel.DataAnnotations;
using ConsoleApp.Helpers;

namespace ConsoleApp.Models;

[Serializable]
public class Schedule: SerializableObject<Schedule>
{
    public static IReadOnlyList<Schedule> Instances => _instances.AsReadOnly(); 
    
    
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public int? Id { get; set; } 

    [Required(ErrorMessage = "Start time is required.")]
    public DateTime? StartTime { get; set; }

    [Required(ErrorMessage = "End time is required.")]
    [CustomValidation(typeof(Schedule), nameof(ValidateEndTime))]
    public DateTime? EndTime { get; set; } 

    [Required(ErrorMessage = "Frequency is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Frequency must be a positive number.")]
    public int? Frequency { get; set; } 
    
    private List<PublicVehicle> _followedBy = [];
    public IReadOnlyList<PublicVehicle> FollowedBy => _followedBy.AsReadOnly();
    
    public Schedule(){}

    public Schedule(int id, DateTime startTime, DateTime endTime, int frequency)
    {
        Id = id;
        StartTime = startTime;
        EndTime = endTime;
        Frequency = frequency;
        _instances.Add(this);
    }
    
    protected static ValidationResult? ValidateEndTime(DateTime endTime, ValidationContext context)
    {
        var instance = (Schedule)context.ObjectInstance;
        return endTime <= instance.StartTime ? new ValidationResult("End time must be later than start time.") : ValidationResult.Success;
    }
    
    // aggregation
    public void AddFollowedBy(PublicVehicle vehicle)
    {
        if (vehicle == null)
            throw new ArgumentNullException(nameof(vehicle), "Public vehicle shouldn't be null.");

        if (_followedBy.Contains(vehicle)) return;
        
        _followedBy.Add(vehicle);
        vehicle.AddFollows(this);
    }
    public void RemoveFollowedBy(PublicVehicle vehicle)
    {
        if (vehicle == null)
            throw new ArgumentNullException(nameof(vehicle), "Public vehicle shouldn't be null.");

        if (!_followedBy.Contains(vehicle)) return;

        _followedBy.Remove(vehicle);
        vehicle.RemoveFollows(this);
    }
    
    public void ModifyFollowedBy(PublicVehicle vehicle1, PublicVehicle vehicle2)
    {
        if (vehicle1 == null || vehicle2 == null)
            throw new ArgumentNullException(nameof(vehicle1), "Vehicle shouldn't be null.");

        if (!_followedBy.Contains(vehicle1)) return;

        RemoveFollowedBy(vehicle1);
        AddFollowedBy(vehicle2);
    }
    
}
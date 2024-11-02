﻿using System.ComponentModel.DataAnnotations;

namespace ConsoleApp;

public class PublicVehicle: SerializableObject<PublicVehicle>
{
    public static IReadOnlyList<PublicVehicle> Instances => _instances.AsReadOnly();
    
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public int Id { get; set; } 

    [Required(ErrorMessage = "Type is required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Type must be between 3 and 50 characters.")]
    public string Type { get; set; } // Example: Bus, Tram, Metro, etc.

    [Required(ErrorMessage = "Capacity is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Capacity must be a positive number.")]
    public int Capacity { get; set; }
    
    public PublicVehicle() { }
    
    public PublicVehicle(int id, string type, int capacity)
    {
        Id = id;
        Type = type;
        Capacity = capacity;
        _instances.Add(this);
    }
}

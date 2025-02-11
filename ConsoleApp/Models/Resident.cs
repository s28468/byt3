﻿using System.ComponentModel.DataAnnotations;
using ConsoleApp.Helpers;
using System.Collections.Generic;

namespace ConsoleApp.Models;

[Serializable]
public class Resident : SerializableObject<Resident>
{
    private static List<Resident> _instances = new List<Resident>();
    public static IReadOnlyList<Resident> Instances => _instances.AsReadOnly();

    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
    public string LastName { get; set; }

    [NoWhitespaces]
    [StringLength(20, ErrorMessage = "Passport number cannot exceed 20 characters.")]
    public string? PassportNum { get; set; }

    [Required(ErrorMessage = "Occupation status is required.")]
    public OccupationStatusType? OccupationStatus { get; set; } // Examples: Unemployed, Student, Employed, Retired

    public Residential? LivesIn { get; private set; }

    public Resident Manager { get; private set; } // Reflexive association

    private Dictionary<int, Workplace> _workplaces = new(); // Qualified association
    public Dictionary<int, Workplace> Workplaces => _workplaces;

    private List<PublicVehicle> _vehiclesUsed = []; // Basic association with PublicVehicle
    public IReadOnlyList<PublicVehicle> VehiclesUsed => _vehiclesUsed.AsReadOnly();

    private List<RecreationalSpace> _recreationalSpaces = []; // Basic association with RecreationalSpace
    public IReadOnlyList<RecreationalSpace> RecreationalSpaces => _recreationalSpaces.AsReadOnly();

    private City? _city; // Basic association with City
    public City? City => _city;

    public Resident()
    {
    }

    public Resident(int id, string firstName, string lastName, string? passportNum,
        OccupationStatusType occupationStatus)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        PassportNum = passportNum;
        OccupationStatus = occupationStatus;
        _instances.Add(this);
    }

    public Resident(int id, string firstName, string lastName, OccupationStatusType occupationStatus)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        OccupationStatus = occupationStatus;
        _instances.Add(this);
    }
    
    // composition
    public void AddLivesIn(Residential residential)
    {
        if (residential == null)
            throw new ArgumentNullException(nameof(residential), "Residential building shouldn't be null.");

        if (LivesIn != null!) return;

        LivesIn = residential;
        
        residential.AddLivedInBy(this);
    }
    
    public void RemoveLivesIn()
    {
        if (LivesIn == null) return;
        
        var temp = LivesIn;
        LivesIn = null;
        
      RemoveManager();
      RemoveWorkplace(Id);
       foreach (var recreationalSpace in _recreationalSpaces)
       {
           RemoveRecreationalSpace(recreationalSpace);
       }
       foreach (var vehicle in _vehiclesUsed)
       {
           RemoveVehicleUsed(vehicle);
       }
       RemoveCity();

       _instances.Remove(this);

        temp.RemoveLivedInBy(this);
    }

    public void ModifyLivesIn(Residential residential)
    {
        if (residential == null)
            throw new ArgumentNullException(nameof(residential), "Residential shouldn't be null.");
        
        RemoveLivesIn();
        AddLivesIn(residential);
    }

// reflex association with reverse connection
    public void SetManager(Resident manager)
    {
        if (manager == null)
            throw new ArgumentNullException(nameof(manager), "Manager shouldn't be null.");
        if (manager == this)
            throw new InvalidOperationException("A resident cannot be their own manager.");
        
        Manager?.RemoveSubordinate(this);

        Manager = manager;

        Manager.AddSubordinate(this);
    }

    public void RemoveManager()
    {
        if (Manager == null) return;

        Manager.RemoveSubordinate(this);

        Manager = null;
    }

    public void ModifyManager(Resident newManager)
    {
        if (newManager == null)
            throw new ArgumentNullException(nameof(newManager), "New manager shouldn't be null.");

        RemoveManager();
        SetManager(newManager);
    }

    private List<Resident> _subordinates = new();
    public IReadOnlyList<Resident> Subordinates => _subordinates.AsReadOnly();

    private void AddSubordinate(Resident subordinate)
    {
        if (!_subordinates.Contains(subordinate))
            _subordinates.Add(subordinate);
    }

    private void RemoveSubordinate(Resident subordinate)
    {
        if (_subordinates.Contains(subordinate))
            _subordinates.Remove(subordinate);
    }


// qualified association with reverse connection
    public void AddWorkplace(int personalId, Workplace workplace)
    {
        if (workplace == null)
            throw new ArgumentNullException(nameof(workplace), "Workplace shouldn't be null.");
        if (_workplaces.ContainsKey(personalId)) return;
        _workplaces.Add(personalId, workplace);
        workplace.AddEmployee(this);
    }

    public void RemoveWorkplace(int personalId)
    {
        if (!_workplaces.TryGetValue(personalId, out var workplace)) return;
        workplace.RemoveEmployee(this);
        _workplaces.Remove(personalId);
    }

    public void ModifyWorkplace(int personalId, Workplace newWorkplace)
    {
        if (newWorkplace == null)
            throw new ArgumentNullException(nameof(newWorkplace), "New workplace shouldn't be null.");

        if (_workplaces.TryGetValue(personalId, out var currentWorkplace))
        {
            currentWorkplace.RemoveEmployee(this);
            _workplaces.Remove(personalId);
        }
        _workplaces.Add(personalId, newWorkplace);
        newWorkplace.AddEmployee(this);
    }
    public Workplace? GetWorkplace(int personalId)
    {
        return _workplaces.TryGetValue(personalId, out var workplace) ? workplace : null;
    }


    // basic association with RecreationalSpace
    public void AddRecreationalSpace(RecreationalSpace recreationalSpace)
    {
        if (recreationalSpace == null)
            throw new ArgumentNullException(nameof(recreationalSpace), "Recreational space shouldn't be null.");

        if (_recreationalSpaces.Contains(recreationalSpace)) return;

        _recreationalSpaces.Add(recreationalSpace);
        recreationalSpace.AddResident(this); 
    }

    public void RemoveRecreationalSpace(RecreationalSpace recreationalSpace)
    {
        if (recreationalSpace == null || !_recreationalSpaces.Contains(recreationalSpace)) return;

        _recreationalSpaces.Remove(recreationalSpace);
        recreationalSpace.RemoveResident(this); 
    }

    public void ModifyRecreationalSpace(RecreationalSpace oldSpace, RecreationalSpace newSpace)
    {
        if (newSpace == null)
            throw new ArgumentNullException(nameof(newSpace), "New recreational space shouldn't be null.");

        RemoveRecreationalSpace(oldSpace); 
        AddRecreationalSpace(newSpace);    
    }


    // basic association with City
    public void SetCity(City city)
    {
        if (city == null)
            throw new ArgumentNullException(nameof(city), "City shouldn't be null.");

        if (_city == city) return;

        _city?.RemoveResident(this);

        _city = city;
        _city.AddResident(this);
    }

    public void RemoveCity()
    {
        if (_city == null) return;

        _city.RemoveResident(this);
        _city = null;
    }

    public void ModifyCity(City newCity)
    {
        if (newCity == null)
            throw new ArgumentNullException(nameof(newCity), "New city shouldn't be null.");

        RemoveCity();
        SetCity(newCity);
    }

    
    // Basic association with Public Vehicle
    public void AddVehicleUsed(PublicVehicle publicVehicle)
    {
        if (publicVehicle == null)
            throw new ArgumentNullException(nameof(publicVehicle), "PublicVehicle shouldn't be null.");

        if (_vehiclesUsed.Contains(publicVehicle)) return;

        _vehiclesUsed.Add(publicVehicle);
        publicVehicle.AddResident(this);
    }

    public void RemoveVehicleUsed(PublicVehicle publicVehicle)
    {
        if (publicVehicle == null || !_vehiclesUsed.Contains(publicVehicle)) return;

        _vehiclesUsed.Remove(publicVehicle);
        publicVehicle.RemoveResident(this);
    }

    public void ModifyVehicleUsed (PublicVehicle oldVehicle, PublicVehicle newVehicle)
    {
        if (newVehicle == null || oldVehicle == null)
            throw new ArgumentNullException(nameof(newVehicle), "New vehicle shouldn't be null.");

        RemoveVehicleUsed(oldVehicle);
        AddVehicleUsed(newVehicle);
    }
}
public enum OccupationStatusType
{
    Unemployed,
    Student,
    Employed,
    Retired
}

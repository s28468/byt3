﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ConsoleApp.Models;

[Serializable]
public class Workplace : Building
{
    private const int WorkHours = 8;

    [Required(ErrorMessage = "Company name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Company name must be between 2 and 100 characters.")]
    public string CompanyName { get; set; } 

    [Required(ErrorMessage = "Industry type is required.")]
    public IndustryTypeEnum IndustryType { get; set; }

    private List<Resource> _created = new(); 

    public List<Resource> Created => new List<Resource>(_created);
    
    private Dictionary<int, Resident> _residents = new(); 

    public Workplace() { }
   
    public Workplace(string companyName, IndustryTypeEnum industryType, int id, decimal price, int openingLevel, int currLevel, string address, int capacity, int occupied): base(id, price, openingLevel, currLevel, address, capacity, occupied)
    {
        CompanyName = companyName;
        IndustryType = industryType;
    }

    // Aggregation
    public void AddCreated(Resource resource)
    {
        if (resource == null) 
            throw new ArgumentNullException(nameof(resource), "Resource shouldn't be null.");

        if (_created.Contains(resource)) return;
        
        _created.Add(resource);
        resource.AddCreatedBy(this);
    }
    
    public void RemoveCreated(Resource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource), "Resource shouldn't be null.");

        if (!_created.Contains(resource)) return;

        _created.Remove(resource);
        resource.RemoveCreatedBy(this);
    }
    
    public void ModifyCreated(Resource resource1, Resource resource2)
    { 
        if (resource1 == null || resource2 == null)
            throw new ArgumentNullException(nameof(resource1), "Resource shouldn't be null.");

        if (!_created.Contains(resource1)) return;

        RemoveCreated(resource1);
        AddCreated(resource2);
    }
    
    // Qualified association with reverse connection
    public void AddResident(int personalId, Resident resident)
    {
        if (resident == null)
            throw new ArgumentNullException(nameof(resident), "Resident shouldn't be null.");

        if (_residents.ContainsKey(personalId)) return;

        _residents.Add(personalId, resident);

        resident.AddWorkplace(personalId, this);
    }
    
    public void RemoveResident(int personalId)
    {
        if (!_residents.ContainsKey(personalId)) 
            throw new ArgumentException("PersonalId does not exist.", nameof(personalId));

        var resident = _residents[personalId];
        resident.RemoveWorkplace(personalId);

        _residents.Remove(personalId);
    }

    public Resident? GetResident(int personalId)
    {
        return _residents.ContainsKey(personalId) ? _residents[personalId] : null;
    }

    public void ModifyResident(int personalId, Resident newResident)
    {
        if (newResident == null)
            throw new ArgumentNullException(nameof(newResident), "New resident shouldn't be null.");

        RemoveResident(personalId);
        AddResident(personalId, newResident);
    }

    public IReadOnlyList<Resident> Residents => _residents.Values.ToList();
}

public enum IndustryTypeEnum
{
    Medical,
    Manufacturing,
    Education,
    Technology,
    Finance
}

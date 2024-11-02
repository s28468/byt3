using System.ComponentModel.DataAnnotations;
using ConsoleApp;

namespace UnitTest;

public class Tests
{ 
    private class TestBuilding : Building { }

    private TestBuilding _building;
    
    private string _tempFileName;

    [SetUp]
    public void Setup()
    {
        _building = new TestBuilding
        {
            Id = 1,
            Price = 1000m,
            OpeningLevel = 5,
            CurrLevel = 10,
            Address = "123 Test Street",
            Capacity = 100,
            Occupied = 50
        };
        
        _tempFileName = $"{Guid.NewGuid()}.json";
        
        // typeof(Serializer<Deal>)
        //     .GetField("Filename", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
        //     ?.SetValue(null, _tempFileName);
        // typeof(Deal)
        //     .GetField("_instances", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
        //     ?.SetValue(null, new List<Deal>());
        // // typeof(Serializer<PublicVehicle>)
        // //     .GetField("Filename", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
        // //     ?.SetValue(null, _tempFileName);
        // // typeof(PublicVehicle)
        // //     .GetField("_instances", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
        // //     ?.SetValue(null, new List<PublicVehicle>());
        // typeof(Serializer<Resident>)
        //     .GetField("Filename", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
        //     ?.SetValue(null, _tempFileName);
        // typeof(Resident)
        //     .GetField("_instances", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
        //     ?.SetValue(null, new List<Resident>());
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(_tempFileName))
        {
            File.Delete(_tempFileName);
        }
    }

    #region Workplace

    // [Test]
    // public void WorkplaceValid()
    // {
    //     var workplace = new Workplace("Health Inc.", "Healthcare");
    //     Assert.IsNotNull(workplace);
    //     Assert.That(workplace.CompanyName, Is.EqualTo("Health Inc."));
    //     Assert.That(workplace.IndustryType, Is.EqualTo("Healthcare"));
    // }

    #endregion

    #region Resident

    // [Test]
    // public void ValidResident()
    // {
    //     var resident = new Resident(1, "John", "Doe", "A123456789", "Employed")
    //     {
    //         Id = 1,
    //         FirstName = "John",
    //         LastName = "Doe",
    //         OccupationStatus = "Employed"
    //     };
    //     var results = ValidateModel(resident);
    //     Assert.IsEmpty(results, "Expected no validation errors.");
    // }

    #endregion
    
    #region PublicVehicle

    // [Test]
    // public void PublicVehicleValid()
    // {
    //     var vehicle = new PublicVehicle(1, "Bus", 50);
    //     var results = ValidateModel(vehicle);
    //     Assert.IsEmpty(results, "Expected no validation errors.");
    // }

    #endregion
    
    #region Deal

    [Test]
    public async Task DealSerialized()
    {
        var deal = new Deal(1, DateTime.Now, DateTime.Now.AddDays(1));
        await Serializer<Deal>.SerializeObject(deal);
        Assert.IsTrue(File.Exists(_tempFileName));
        var fileContents = await File.ReadAllTextAsync(_tempFileName);
        Assert.IsNotEmpty(fileContents, "Serialized file is empty.");
        var deserializedDeals = await Serializer<Deal>.DeserializeObjects();
        Assert.That(deserializedDeals.Count, Is.EqualTo(1));
        Assert.That(deserializedDeals[0].Id, Is.EqualTo(deal.Id));
    }
    
    [Test]
    public async Task DealWithMultipleDealsSerialized()
    {
        var deal1 = new Deal(1, DateTime.Now, DateTime.Now.AddDays(1));
        var deal2 = new Deal(2, DateTime.Now, DateTime.Now.AddDays(2));
        await Serializer<Deal>.SerializeObject(deal1);
        await Serializer<Deal>.SerializeObject(deal2);
        var deserializedDeals = await Serializer<Deal>.DeserializeObjects();
        Assert.That(deserializedDeals.Count, Is.EqualTo(2));
        Assert.That(deserializedDeals[0].Id, Is.EqualTo(deal1.Id));
        Assert.That(deserializedDeals[1].Id, Is.EqualTo(deal2.Id));
    }
    
    [Test]
    public void Deal_InvalidEndDateIsStartDate()
    {
        var startDate = DateTime.Now;
        var deal = new Deal(1, startDate, startDate);
        var results = ValidateModel(deal);
        Assert.That(results, Has.Exactly(1).Matches<ValidationResult>(r => r.ErrorMessage != null && r.ErrorMessage.Contains("End date must be later than the start date.")));
    }

    #endregion

    #region City
    
    [Test]
    public void CityValidation()
    {
        var name = "Sample City";
        var dateOfFounding = new DateTime(1800, 1, 1);
        var area = 250.5;
        var population = 50000;
        
        

        Console.WriteLine($"Before creating city, instances count: {City.Instances.Count}");

        var city = new City(name, dateOfFounding, area, population);
        Console.WriteLine($"After creating city, instances count: {City.Instances.Count}");
        var validationResults = ValidateModel(city);
        Assert.IsEmpty(validationResults, "City should be valid.");
        Assert.That(City.Instances.Count, Is.EqualTo(1));
        Assert.That(City.Instances[0], Is.EqualTo(city));
    }
    
    [Test]
    public void CityFailValidation()
    {
        var name = "A";
        var dateOfFounding = new DateTime(1800, 1, 1);
        var area = 250.5;
        var population = 50000;
        var city = new City(name, dateOfFounding, area, population);
        var validationResults = ValidateModel(city);
        Assert.IsTrue(validationResults.Any(v => v.MemberNames.Contains("Name")), "Name validation should fail.");
    }
    
    #endregion

    #region Building

    [Test]
    public void BuildingValid()
    {
        var results = ValidateModel(_building);
        Assert.IsEmpty(results, "No validation errors.");
    }
    
    [Test]
    public void BuildingInvalidPrice()
    {
        _building.Price = 0m;
        var results = ValidateModel(_building);
        Assert.That(results, Has.Exactly(1).Matches<ValidationResult>(r => r.ErrorMessage != null && r.ErrorMessage.Contains("Price must be greater than zero.")));
    }
    
    [Test]
    public void BuildingInvalidNegative()
    {
        _building.Occupied = -1;
        var results = ValidateModel(_building);
        Assert.That(results, Has.Exactly(1).Matches<ValidationResult>(r => r.ErrorMessage != null && r.ErrorMessage.Contains("Occupied spaces cannot be negative.")));
    }
    
    #endregion
    
    #region Validation
    
    private List<ValidationResult> ValidateModel(Resident resident)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(resident, null, null);
        Validator.TryValidateObject(resident, validationContext, validationResults, true);
        return validationResults;
    }
    
    private List<ValidationResult> ValidateModel(PublicVehicle vehicle)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(vehicle, null, null);
        Validator.TryValidateObject(vehicle, validationContext, validationResults, true);
        return validationResults;
    }
    
    private List<ValidationResult> ValidateModel(Building building)
    {
        var context = new ValidationContext(building, null, null);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(building, context, results, true);
        return results;
    }
    
    private IList<ValidationResult> ValidateModel(City city)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(city);
        Validator.TryValidateObject(city, validationContext, validationResults, true);
        return validationResults;
    }
    
    private List<ValidationResult> ValidateModel(Deal deal)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(deal, null, null);
        Validator.TryValidateObject(deal, validationContext, validationResults, true);
        return validationResults;
    }

    #endregion
}
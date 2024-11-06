using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.ComTypes;
using ConsoleApp;

namespace UnitTests;

public class Tests
{ 
    private class TestBuilding : Building { }

    private TestBuilding _building;
    private Workplace _workplace;
    private string Filename = "Instances.xml";

    //private string _tempFileName;

    [SetUp]
    public async Task Setup()
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
        
        //_tempFileName = $"{Guid.NewGuid()}.json";
        
        await Serializer.ClearFile(Filename);
        Deal._instances.Clear();
        PublicVehicle._instances.Clear();
        Building._instances.Clear();
        Resident._instances.Clear();
        Resource._instances.Clear();
        Route._instances.Clear();
        Natural._instances.Clear();
        
        _workplace = new Workplace("TechCorp", IndustryTypeEnum.Technology)
        {
            Id = 1,
            Price = 5000m,
            OpeningLevel = 1,
            CurrLevel = 1,
            Address = "123 Business St",
            Capacity = 50,
            Occupied = 10
        };
        
    }

    #region Serializer

    [Test]
    public async Task SerializeAll_WithInstances_SerializesToFile()
    {
        // Arrange
        var deal1 = new Deal(1, DateTime.Now, DateTime.Now.AddDays(1));
        var deal2 = new Deal(2, DateTime.Now, DateTime.Now.AddDays(2));

        // Act
        await Deal.SerializeAll(Filename);

        // Assert
        Assert.IsTrue(File.Exists(Filename));
        Assert.IsTrue(new FileInfo("Instances.xml").Length > 0);
    }
    
    [Test]
    public async Task LoadAll_WithSerializedData_LoadsInstances()
    {

        var deal1 = new Deal(1, DateTime.Now, DateTime.Now.AddMonths(1));
        
        await Deal.SerializeAll(Filename);
        
        await Deal.LoadAll(Filename);

        // Assert
        Assert.That(Deal._instances.Count, Is.EqualTo(1));
        Console.WriteLine(Deal._instances);
        //Assert.IsTrue(validationResults.Exists(v => v.ErrorMessage.Contains("Error loading instances.")));
        //Assert.That(Deal._instances[0].Id, Is.EqualTo(deal1.Id));
    }
    
    [Test]
    public void GetAllInstances_ReturnsInstancesList()
    {
        // Arrange
        var deal1 = new Deal(1, DateTime.Now, DateTime.Now.AddDays(1));
        var deal2 = new Deal(2, DateTime.Now, DateTime.Now.AddDays(2));

        // Act
        var instances = Deal.GetAllInstances().Result;

        // Assert
        Assert.That(instances.Count, Is.EqualTo(2));
        Assert.Contains(deal1, instances);
        Assert.Contains(deal2, instances);
    }
    
    [Test]
    public async Task SerializeInstances_CreatesSerializedFile()
    {
        // Arrange
        var deal = new Deal(1, DateTime.Now, DateTime.Now.AddDays(1));

        // Act
        await Serializer.SerializeInstances(Filename);

        // Assert
        Assert.IsTrue(File.Exists(Filename));
        Assert.IsTrue(new FileInfo(Filename).Length > 0);
    }
    
    [Test]
    public async Task ClearFile_DeletesFileContent()
    {
        // Arrange
        File.WriteAllText(Filename, "Test content");

        // Act
        await Serializer.ClearFile(Filename);

        // Assert
        Assert.IsTrue(File.Exists(Filename));
        Assert.That(new FileInfo(Filename).Length, Is.EqualTo(0));
    }

    #endregion

    #region Workplace

    [Test]
    public void WorkplaceValid()
    {
        var workplace = new Workplace("Health Inc.", IndustryTypeEnum.Technology);
        Assert.IsNotNull(workplace);
        Assert.That(workplace.CompanyName, Is.EqualTo("Health Inc."));
        Assert.That(workplace.IndustryType, Is.EqualTo(IndustryTypeEnum.Technology));
    }
    
    [Test]
    public void CompanyNameValidation_ValidLength_ReturnsSuccess()
    {
        // Arrange
        _workplace.CompanyName = "Valid Company Name";
        var context = new ValidationContext(_workplace) { MemberName = nameof(Workplace.CompanyName) };

        // Act
        var result = Validator.TryValidateProperty(_workplace.CompanyName, context, null);

        // Assert
        Assert.IsTrue(result);
    }
    
    [Test]
    public void WorkplaceDerivedProperties_InheritedProperties_AreValid()
    {
        // Arrange
        _workplace.Price = 8000m;
        _workplace.OpeningLevel = 3;
        _workplace.CurrLevel = 5;

        // Act
        var context = new ValidationContext(_workplace);
        var validationResults = new System.Collections.Generic.List<ValidationResult>();
        var isValid = Validator.TryValidateObject(_workplace, context, validationResults, true);

        // Assert
        Assert.IsTrue(isValid);
        Assert.That(_workplace.Price, Is.EqualTo(8000m));
        Assert.That(_workplace.OpeningLevel, Is.EqualTo(3));
        Assert.That(_workplace.CurrLevel, Is.EqualTo(5));
    }

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
    
    [Test]
    public void Constructor_WithAllParameters_AddsToInstances()
    {
        // Arrange & Act
        var resident = new Resident(1, "John", "Doe", "A123456789", OccupationStatusType.Employed);

        // Assert
        Assert.Contains(resident, Resident.Instances as ICollection);
        Assert.That(resident.Id, Is.EqualTo(1));
        Assert.That(resident.FirstName, Is.EqualTo("John"));
        Assert.That(resident.LastName, Is.EqualTo("Doe"));
        Assert.That(resident.PassportNum, Is.EqualTo("A123456789"));
        Assert.That(resident.OccupationStatus, Is.EqualTo(OccupationStatusType.Employed));
    }
    
    [Test]
    public void IdValidation_InvalidId_ReturnsValidationError()
    {
        // Arrange
        var resident = new Resident { Id = 0, FirstName = "John", LastName = "Doe", OccupationStatus = OccupationStatusType.Employed };
        var context = new ValidationContext(resident) { MemberName = nameof(Resident.Id) };

        // Act
        var result = Validator.TryValidateProperty(resident.Id, context, null);

        // Assert
        Assert.IsFalse(result);
    }
    
    [Test]
    public void PassportNumValidation_ExceedsMaxLength_ReturnsValidationError()
    {
        // Arrange
        var resident = new Resident { Id = 1, FirstName = "John", LastName = "Doe", PassportNum = new string('A', 21), OccupationStatus = OccupationStatusType.Employed };
        var context = new ValidationContext(resident) { MemberName = nameof(Resident.PassportNum) };

        // Act
        var result = Validator.TryValidateProperty(resident.PassportNum, context, null);

        // Assert
        Assert.IsFalse(result);
    }
    
    [Test]
    public void FirstNameValidation_InvalidLength_ReturnsValidationError()
    {
        // Arrange
        var resident = new Resident { Id = 1, FirstName = "A", LastName = "Doe", OccupationStatus = OccupationStatusType.Student };
        var context = new ValidationContext(resident) { MemberName = nameof(Resident.FirstName) };

        // Act
        var result = Validator.TryValidateProperty(resident.FirstName, context, null);

        // Assert
        Assert.IsFalse(result);
    }
    
    // [Test]
    // public void OccupationStatusValidation_MissingOccupationStatus_ReturnsValidationError()
    // {
    //     // Arrange
    //     var resident = new Resident { Id = 1, FirstName = "John", LastName = "Doe" };
    //     var context = new ValidationContext(resident) { MemberName = nameof(Resident.OccupationStatus) };
    //
    //     // Act
    //     var validationResults = new List<ValidationResult>();
    //     var result = Validator.TryValidateObject(resident, context, validationResults, true);
    //
    //     // Assert
    //     Assert.IsFalse(result);
    //     Assert.IsTrue(validationResults.Exists(v => v.ErrorMessage.Contains("Occupation status is required.")));
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
    
    [Test]
    public void IdVehicleInvalid()
    {
        var vehicle = new PublicVehicle { Id = 0, Type = VehicleType.Tram, Capacity = 30 };
        var context = new ValidationContext(vehicle) { MemberName = nameof(PublicVehicle.Id) };
        var result = Validator.TryValidateProperty(vehicle.Id, context, null);
        Assert.IsFalse(result);
    }
    
    // [Test]
    // public void TypeValidation_MissingType_ReturnsValidationError()
    // {
    //     // Arrange
    //     var vehicle = new PublicVehicle { Id = 1, Capacity = 20 };
    //     var context = new ValidationContext(vehicle) { MemberName = nameof(PublicVehicle.Type) };
    //
    //     // Act
    //     var validationResults = new List<ValidationResult>();
    //     var result = Validator.TryValidateObject(vehicle, context, validationResults, true);
    //
    //     // Assert
    //     Assert.IsFalse(result);
    //     Assert.IsTrue(validationResults.Exists(v => v.ErrorMessage.Contains("Type is required.")));
    // }

    [Test]
    public void CapacityValidation_NegativeCapacity_ReturnsValidationError()
    {
        // Arrange
        var vehicle = new PublicVehicle { Id = 1, Type = VehicleType.Metro, Capacity = -10 };
        var context = new ValidationContext(vehicle) { MemberName = nameof(PublicVehicle.Capacity) };

        // Act
        var result = Validator.TryValidateProperty(vehicle.Capacity, context, null);

        // Assert
        Assert.IsFalse(result);
    }

    #endregion
    
    #region Deal

    [Test]
    public void Constructor_ValidData_AddsToInstances()
    {
        var deal = new Deal(1, DateTime.Now, DateTime.Now.AddDays(1));
        Assert.Contains(deal, Deal._instances);
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

    #region Resource

    [Test]
    public void IdResourceInvalid()
    {
        var resource = new Resource { Id = 0, Name = "Gas", Availability = true, Price = 2.50m, Quantity = 300 };
        var context = new ValidationContext(resource) { MemberName = nameof(Resource.Id) };
        var result = Validator.TryValidateProperty(resource.Id, context, null);
        Assert.IsFalse(result);
    }
    
    [Test]
    public void QuantityValidation_InvalidQuantity_ReturnsValidationError()
    {
        var resource = new Resource { Id = 1, Name = "Gas", Price = 5.00m, Quantity = 0 };
        var context = new ValidationContext(resource) { MemberName = nameof(Resource.Quantity) };
        var result = Validator.TryValidateProperty(resource.Quantity, context, null);
        Assert.IsFalse(result);
    }
    
    [Test]
    public void PriceValidation_NegativePrice_ReturnsValidationError()
    {
        // Arrange
        var resource = new Resource { Id = 1, Name = "Water", Price = -1.00m, Quantity = 10 };
        var context = new ValidationContext(resource) { MemberName = nameof(Resource.Price) };

        // Act
        var result = Validator.TryValidateProperty(resource.Price, context, null);

        // Assert
        Assert.IsFalse(result);
    }

    #endregion

    #region Route

    [Test]
    public void IdRouteInvalid()
    {
        var route = new Route { Id = 0, StartPoint = "Point A", EndPoint = "Point B", StopCount = 3, Duration = 30 };
        var context = new ValidationContext(route) { MemberName = nameof(Route.Id) };
        var result = Validator.TryValidateProperty(route.Id, context, null);
        Assert.IsFalse(result);
    }
    
    [Test]
    public void StopCountValidation_NegativeStopCount_ReturnsValidationError()
    {
        // Arrange
        var route = new Route { Id = 1, StartPoint = "Point A", EndPoint = "Point B", StopCount = -1, Duration = 30 };
        var context = new ValidationContext(route) { MemberName = nameof(Route.StopCount) };

        // Act
        var result = Validator.TryValidateProperty(route.StopCount, context, null);

        // Assert
        Assert.IsFalse(result);
    }
    
    [Test]
    public void DurationValidation_ZeroDuration_ReturnsValidationError()
    {
        // Arrange
        var route = new Route { Id = 1, StartPoint = "Point A", EndPoint = "Point B", StopCount = 2, Duration = 0 };
        var context = new ValidationContext(route) { MemberName = nameof(Route.Duration) };

        // Act
        var result = Validator.TryValidateProperty(route.Duration, context, null);

        // Assert
        Assert.IsFalse(result);
    }
    
    [Test]
    public void StartPointValidation_InvalidLength_ReturnsValidationError()
    {
        // Arrange
        var route = new Route { Id = 1, StartPoint = "A", EndPoint = "Point B", StopCount = 3, Duration = 30 };
        var context = new ValidationContext(route) { MemberName = nameof(Route.StartPoint) };

        // Act
        var result = Validator.TryValidateProperty(route.StartPoint, context, null);

        // Assert
        Assert.IsFalse(result);
    }
    
    [Test]
    public void EndPointValidation_InvalidLength_ReturnsValidationError()
    {
        // Arrange
        var route = new Route { Id = 1, StartPoint = "Point A", EndPoint = "B", StopCount = 3, Duration = 30 };
        var context = new ValidationContext(route) { MemberName = nameof(Route.EndPoint) };

        // Act
        var result = Validator.TryValidateProperty(route.EndPoint, context, null);

        // Assert
        Assert.IsFalse(result);
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

    #region Natural
    
    [Test]
    public void OriginValidation_InvalidLength_ReturnsValidationError()
    {
        // Arrange
        var natural = new Natural
        {
            Id = 1,
            Name = "Natural Resource",
            Availability = true,
            Price = 99.99m,
            Quantity = 10,
            IsExportable = true,
            Origin = "A", // Invalid length
            Producer = "Nature Corp",
            ExpirationDate = DateTime.Now.AddDays(30)
        };
        var context = new ValidationContext(natural) { MemberName = nameof(Natural.Origin) };

        // Act
        var result = Validator.TryValidateProperty(natural.Origin, context, null);

        // Assert
        Assert.IsFalse(result);
    }
    
    [Test]
    public void ExpirationDateValidation_PastDate_ReturnsValidationError()
    {
        // Arrange
        var natural = new Natural
        {
            Id = 1,
            Name = "Natural Resource",
            Availability = true,
            Price = 99.99m,
            Quantity = 10,
            IsExportable = true,
            Origin = "Forest",
            Producer = "Nature Corp",
            ExpirationDate = DateTime.Now.AddDays(-10) // Past date
        };
        var context = new ValidationContext(natural) { MemberName = nameof(Natural.ExpirationDate) };

        // Act
        var result = Validator.TryValidateProperty(natural.ExpirationDate, context, null);

        // Assert
        Assert.IsFalse(result);
    }
    
    [Test]
    public void ExpirationDateValidation_FutureDate_ReturnsSuccess()
    {
        // Arrange
        var natural = new Natural
        {
            Id = 1,
            Name = "Natural Resource",
            Availability = true,
            Price = 99.99m,
            Quantity = 10,
            IsExportable = true,
            Origin = "Forest",
            Producer = "Nature Corp",
            ExpirationDate = DateTime.Now.AddDays(10) // Future date
        };
        var context = new ValidationContext(natural) { MemberName = nameof(Natural.ExpirationDate) };

        // Act
        var result = Validator.TryValidateProperty(natural.ExpirationDate, context, null);

        // Assert
        Assert.IsTrue(result);
    }
    
    #endregion
}
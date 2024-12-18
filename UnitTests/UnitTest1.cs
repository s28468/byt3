using System.Collections;
using System.ComponentModel.DataAnnotations;
using ConsoleApp;
using ConsoleApp.Helpers;
using ConsoleApp.Models;

namespace UnitTests;

public class Tests
{
    #region Basic

    [Test]
    public void AddingBasicAssociation()
    {
        PublicVehicle vehicle = new PublicVehicle(1, VehicleType.Bus, 2);
        Resident resident = new Resident(1, "Lia", "Lia", OccupationStatusType.Employed);

        vehicle.AddResident(resident);

        Assert.That(resident.VehiclesUsed.Contains(vehicle)); //reverse connection
        Assert.That(vehicle.Residents.Contains(resident));
    }

    [Test]
    public void RemovingBasicAssociation()
    {
        PublicVehicle vehicle = new PublicVehicle(1, VehicleType.Bus, 2);
        Resident resident = new Resident(1, "Lia", "Lia", OccupationStatusType.Employed);

        vehicle.AddResident(resident);

        Assert.That(resident.VehiclesUsed.Contains(vehicle)); //reverse connection
        Assert.That(vehicle.Residents.Contains(resident));

        vehicle.RemoveResident(resident);

        Assert.IsEmpty(resident.VehiclesUsed); //reverse connection
        Assert.IsEmpty(vehicle.Residents);
    }

    [Test]
    public void ModifyingBasicAssociation()
    {
        PublicVehicle vehicle = new PublicVehicle(1, VehicleType.Bus, 2);
        Resident resident = new Resident(1, "Lia", "Lia", OccupationStatusType.Employed);

        vehicle.AddResident(resident);

        Assert.That(resident.VehiclesUsed.Contains(vehicle)); //reverse connection
        Assert.That(vehicle.Residents.Contains(resident));

        Resident resident2 = new Resident(2, "Liia", "Liia", OccupationStatusType.Unemployed);
        vehicle.ModifyResident(resident, resident2);

        Assert.IsEmpty(resident.VehiclesUsed); //reverse connection
        Assert.That(resident2.VehiclesUsed.Contains(vehicle)); //reverse connection

        Assert.That(vehicle.Residents.Contains(resident2));
        Assert.That(vehicle.Residents.Contains(resident), Is.False);
    }

    [Test]
    public void ErrorHandlingBasicAssociation()
    {
        PublicVehicle vehicle = new PublicVehicle(1, VehicleType.Bus, 2);

        Assert.Throws<ArgumentNullException>(() => vehicle.AddResident(null!));
    }


    #endregion

    #region Aggregation

    [Test]
    public void AddingAggregationTest()
    {
        Resource resource = new Resource(1, "Steel", "High-quality steel", true, 500m, 100, false);
        Workplace workplace = new Workplace("CDPR", IndustryTypeEnum.Manufacturing, 1, 10, 1, 1, "somewhere", 2, 1);

        resource.AddCreatedBy(workplace);

        Assert.That(resource.CreatedBy.Contains(workplace));
        Assert.That(workplace.Created.Contains(resource)); //reverse connection
    }

    [Test]
    public void RemovingAggregationTest()
    {
        Resource resource = new Resource(1, "Steel", "High-quality steel", true, 500m, 100, false);
        Workplace workplace = new Workplace("CDPR", IndustryTypeEnum.Manufacturing, 1, 10, 1, 1, "somewhere", 2, 1);

        resource.AddCreatedBy(workplace);

        Assert.That(resource.CreatedBy.Contains(workplace));
        Assert.That(workplace.Created.Contains(resource)); //reverse connection

        resource.RemoveCreatedBy(workplace);

        Assert.IsEmpty(resource.CreatedBy);
        Assert.That(resource.CreatedBy.Contains(workplace), Is.False);

        Assert.IsEmpty(workplace.Created); //reverse connection
        Assert.That(workplace.Created.Contains(resource), Is.False); //reverse connection
    }

    [Test]
    public void ModifyingAggregationTest()
    {
        Resource resource = new Resource(1, "Steel", "High-quality steel", true, 500m, 100, false);
        Workplace workplace = new Workplace("CDPR", IndustryTypeEnum.Manufacturing, 1, 10, 1, 1, "somewhere", 2, 1);

        resource.AddCreatedBy(workplace);

        Assert.That(resource.CreatedBy.Contains(workplace));
        Assert.That(workplace.Created.Contains(resource)); //reverse connection

        Workplace workplace2 = new Workplace("CDPR2", IndustryTypeEnum.Manufacturing, 1, 10, 1, 1, "somewhere", 2, 1);
        resource.ModifyCreatedBy(workplace, workplace2);

        Assert.That(resource.CreatedBy.Contains(workplace), Is.False);

        Assert.IsEmpty(workplace.Created); //reverse connection
        Assert.That(workplace.Created.Contains(resource), Is.False); //reverse connection

        Assert.That(resource.CreatedBy.Contains(workplace2));
        Assert.That(workplace2.Created.Contains(resource)); //reverse connection
    }

    [Test]
    public void ErrorHandlingAggregationAssociation()
    {
        Resource resource = new Resource(1, "Steel", "High-quality steel", true, 500m, 100, false);

        Assert.Throws<ArgumentNullException>(() => resource.AddCreatedBy(null!));
    }

    #endregion

    #region Composition

    [Test]
    public void AddingCompositionTest()
    {
        City city = new City();
        Building building = new Workplace();

        city.AddConsistsOf(building);

        Assert.That(city.ConsistsOf.Contains(building));
        Assert.That(building.IsPartOf, Is.EqualTo(city)); //reverse connection
    }

    [Test]
    public void RemovingCompositionTest()
    {
        City city = new City();
        Building building = new Workplace();

        city.AddConsistsOf(building);

        Assert.That(city.ConsistsOf.Contains(building));
        Assert.That(building.IsPartOf, Is.EqualTo(city)); //reverse connection

        city.RemoveConsistsOf(building);

        Assert.IsEmpty(city.ConsistsOf);
        Assert.That(city.ConsistsOf.Contains(building), Is.False);

        Assert.IsNull(building.IsPartOf); //reverse connection
    }

    [Test]
    public void ModifyingCompositionTest()
    {
        City city = new City();
        Building building = new Workplace();

        city.AddConsistsOf(building);

        Assert.That(city.ConsistsOf.Contains(building));
        Assert.That(building.IsPartOf, Is.EqualTo(city)); //reverse connection

        Building building2 = new Workplace();
        city.ModifyConsistsOf(building, building2);

        Assert.IsNotEmpty(city.ConsistsOf);
        Assert.IsNotNull(building2.IsPartOf);

        Assert.That(city.ConsistsOf.Contains(building2));
        Assert.That(building2.IsPartOf, Is.EqualTo(city)); //reverse connection

        Assert.That(city.ConsistsOf.Contains(building), Is.False);
        Assert.IsNull(building.IsPartOf); //reverse connection
    }

    [Test]
    public void ErrorHandlingCompositionAssociation()
    {
        City city = new City();

        Assert.Throws<ArgumentNullException>(() => city.AddConsistsOf(null!));
    }

    #endregion

    #region ReflexAssociation

    [Test]
    public void AddingReflexTest()
    {
        Resident resident = new Resident(1, "Lia", "Lia", OccupationStatusType.Employed);
        Resident supervisor = new Resident(2, "Liia", "Liia", OccupationStatusType.Employed);

        resident.SetManager(supervisor);

        Assert.That(resident.Manager, Is.EqualTo(supervisor));
        Assert.That(supervisor.Subordinates.Contains(resident)); //reverse connection
    }

    [Test]
    public void RemovingReflexTest()
    {
        Resident resident = new Resident(1, "Lia", "Lia", OccupationStatusType.Employed);
        Resident supervisor = new Resident(2, "Liia", "Liia", OccupationStatusType.Employed);

        resident.SetManager(supervisor);

        Assert.That(resident.Manager, Is.EqualTo(supervisor));
        Assert.That(supervisor.Subordinates.Contains(resident)); //reverse connection
        
        resident.RemoveManager();

        Assert.IsNull(resident.Manager);
        Assert.IsEmpty(supervisor.Subordinates); //reverse connection
        //reverse connection
    }

    [Test]
    public void ModifyingReflexTest()
    {
        Resident resident = new Resident(1, "Lia", "Lia", OccupationStatusType.Employed);
        Resident supervisor = new Resident(2, "Liia", "Liia", OccupationStatusType.Employed);

        resident.SetManager(supervisor);

        Assert.That(resident.Manager, Is.EqualTo(supervisor));
        Assert.That(supervisor.Subordinates.Contains(resident)); //reverse connection

        Resident supervisor2 = new Resident(3, "Liiia", "Liiia", OccupationStatusType.Employed);
        resident.ModifyManager(supervisor2);

        Assert.That(resident.Manager, Is.EqualTo(supervisor2));
        Assert.That(supervisor2.Subordinates.Contains(resident)); //reverse connection
        
        Assert.IsEmpty(supervisor.Subordinates); //reverse connection
    }

    [Test]
    public void ErrorHandlingReflexAssociation()
    {
        Resident resident = new Resident(1, "Lia", "Lia", OccupationStatusType.Employed);
        Assert.Throws<ArgumentNullException>(() => resident.SetManager(null!));
    }

    #endregion

    #region Qualified

    [Test]
    public void AddingQualifiedTest()
    {
        Resident resident = new Resident(1, "Lia", "Lia", OccupationStatusType.Employed);
        Workplace workplace = new Workplace();
        
        workplace.AddResident(1, resident);

        Assert.That(workplace.GetResident(1), Is.EqualTo(resident));
        Assert.That(resident.Workplaces.ContainsValue(workplace)); //reverse connection
    }
    
    [Test]
    public void RemovingQualifiedTest()
    {
        Resident resident = new Resident(1, "Lia", "Lia", OccupationStatusType.Employed);
        Workplace workplace = new Workplace();
        
        workplace.AddResident(1, resident);

        Assert.That(workplace.GetResident(1), Is.EqualTo(resident));
        Assert.That(resident.Workplaces.ContainsValue(workplace)); //reverse connection
        
        resident.RemoveWorkplace(1);
        
        Assert.IsEmpty(resident.Workplaces); //reverse connection

    }
    
    [Test]
    public void ModifyingQualifiedTest()
    {
        Resident resident = new Resident(1, "Lia", "Lia", OccupationStatusType.Employed);
        Workplace workplace = new Workplace();
        
        workplace.AddResident(1, resident);

        Assert.That(workplace.GetResident(1), Is.EqualTo(resident));
        Assert.That(resident.Workplaces.ContainsValue(workplace)); //reverse connection
        
        Workplace workplace2 = new Workplace();
        resident.ModifyWorkplace(1, workplace2);
        
        Assert.IsEmpty(workplace.Employees); //reverse connection
        Assert.That(workplace2.Employees.Contains(resident)); //reverse connection
        Assert.That(resident.Workplaces.ContainsValue(workplace2));
    }
    
    [Test]
    public void ErrorHandlingQualifiedAssociation()
    {
        Workplace workplace = new Workplace();
        
        Assert.Throws<ArgumentNullException>(() => workplace.AddResident(1, null!));
    }

    #endregion

    #region AttributeAssocitation

    [Test]
    public void AddingAttributeTest()
    {
        City city = new City();
        Resource resource = new Resource(1, "coin", "none", true, 10, 2, true);
        
        city.AddDeal(resource);
        
        Assert.That(city.Created.Count, Is.EqualTo(1));
        Assert.That(city.Created.First().Traded, Is.EqualTo(resource));
        
        var deal = resource.TradedIn.First(); //reverse connection
        Assert.That(deal.CreatedBy, Is.EqualTo(city)); //reverse connection
        Assert.That(deal.Traded, Is.EqualTo(resource)); //reverse connection
        Assert.Contains(deal, Deal.Instances.ToList()); //reverse connection
    }
    
    [Test]
    public void RemovingAttributeTest()
    {
        City city = new City();
        Resource resource = new Resource(1, "coin", "none", true, 10, 2, true);

        city.AddDeal(resource);

        Assert.That(city.Created.Any(d => d.Traded == resource));
        Assert.That(resource.TradedIn.Any(d => d.CreatedBy == city));

        city.RemoveDeal(resource);

        Assert.That(city.Created.Any(d => d.Traded == resource), Is.False);
        Assert.That(Deal.Instances.Any(d => d.CreatedBy == city && d.Traded == resource), Is.False); //reverse connection
    }
    
    [Test]
    public void ModifyingRemovingAttributeTest()
    {
        City city = new City();
        Resource resource = new Resource(1, "coin", "none", true, 10, 2, true);
        
        city.AddDeal(resource);

        Assert.That(city.Created.Any(d => d.Traded == resource));
        Assert.That(resource.TradedIn.Any(d => d.CreatedBy == city));

        var newResource = new Resource(2, "Copper Ore", "Another raw material", true, 1500.75m, 500, false);
        city.ModifyDeal(resource, newResource);

        Assert.That(city.Created.Any(d => d.Traded == resource), Is.False);
        
        Assert.That(city.Created.Any(d => d.Traded == newResource), Is.True);
        
        Assert.That(newResource.TradedIn.Any(d => d.CreatedBy == city), Is.True);
    }
    
    [Test]
    public void ErrorHandlingDealAssociationTest()
    {
        City city = new City();
        Assert.Throws<ArgumentNullException>(() => city.AddDeal(null!));
        Assert.Throws<ArgumentNullException>(() => city.RemoveDeal(null!));
    }

    #endregion
    
    #region EmptyStrings

    [Test]
    public void ImportedEmptyDesc()
    {
        int id = 1;
        string name = "Sample Product";
        bool availability = true;
        decimal price = 100.0m;
        int quantity = 50;
        bool isExportable = true;
        string importer = "Importer A";
        string originCity = "City A";
        string originCertificate = "Cert123";
        string storageAddress = "123 Warehouse Street";
        var imported = new Imported(id, name, availability, price, quantity, isExportable, importer, originCity, originCertificate, storageAddress, "");
        var validationContext = new ValidationContext(imported);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(imported, validationContext, validationResults, true);
        Assert.That(isValid, Is.False);
        Assert.IsTrue(validationResults.Exists(v => v.ErrorMessage == "Line cannot be empty or whitespace."));
    }
    
    [Test]
    public void ResidentPassNumEmpty()
    {
        var resident = new Resident(1, "John", "Doe", " ", OccupationStatusType.Employed);
        var context = new ValidationContext(resident) { MemberName = nameof(Resident.OccupationStatus) };
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(resident, context, validationResults, true);
        Assert.That(isValid, Is.False);
        Assert.IsTrue(validationResults.Exists(v => v.ErrorMessage != null && v.ErrorMessage.Contains("Line cannot be empty or whitespace.")));
    }
    
    [Test]
    public void RecreationalSpaceFacilitiesListEmpty()
    {
        var invalidFacilities = new List<string> { "Gym", "Pool", " ", "Parking" };
        var recreationalSpace = new RecreationalSpace
        {
            Name = "Community Gym",
            Type = RecreationalSpaceType.Gym,
            EntryFee = 10.0m,
            Facilities = invalidFacilities
        };
        var validationContext = new ValidationContext(recreationalSpace);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(recreationalSpace, validationContext, validationResults, true);
        Assert.That(isValid, Is.False);
        Assert.IsTrue(validationResults.Exists(v => v.ErrorMessage == "List cannot contain an empty or whitespace string."));
    }

    #endregion

    #region Serializer
    
    [Test]
    public async Task SerializeAll_WithInstances_SerializesToFile()
    {
        await Deal.ClearFile();
        Serializer.LoadInstances();
        
        var deal = new Deal(1, DateTime.Now, DateTime.Now.AddDays(1), new City(), new Exported());
        
        await Serializer.SerializeInstances();
        Assert.That(File.Exists("Deal.xml"));
        Assert.That(new FileInfo("Deal.xml").Length > 0);
        
        await Serializer.SerializeInstances();
    }
    
    [Test]
    public async Task LoadAll_WithSerializedData_LoadsInstances()
    {
        await Deal.ClearFile();
        Serializer.LoadInstances();
        var deal = new Deal(1, DateTime.Now, DateTime.Now.AddDays(1), new City(), new Exported());
        
        Assert.That(Deal.Instances.Count, Is.EqualTo(1));
        Assert.That(Deal.Instances[0].Id, Is.EqualTo(deal.Id));
        
        await Serializer.SerializeInstances();
    }
    
    [Test]
    public async Task GetAllInstances_ReturnsInstancesList()
    {
        await Deal.ClearFile();
        Serializer.LoadInstances();
        var deal1 = new Deal(1, DateTime.Now, DateTime.Now.AddDays(1), new City(), new Exported());
        
        var deal2 = new Deal(2, DateTime.Now, DateTime.Now.AddDays(2), new City(), new Imported());
        
        var instances = Deal.GetAllInstances().Result;
        
        Assert.That(instances.Count, Is.EqualTo(2));
        Assert.Contains(deal1, instances);
        Assert.Contains(deal2, instances);
        await Deal.SerializeAll();
    }
    
    #endregion

    #region Resident

    [Test]
    public void ValidResident()
    {
        var resident = new Resident(1, "John", "Doe", "A123456789", OccupationStatusType.Employed);
        var results = ValidateModel(resident);
        Assert.IsEmpty(results, "Expected no validation errors.");
    }
    
    [Test]
    public void Constructor_WithAllParameters_AddsToInstances()
    {
        var resident = new Resident(1, "John", "Doe", "A123456789", OccupationStatusType.Employed);
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
        var resident = new Resident { Id = 0, FirstName = "John", LastName = "Doe", OccupationStatus = OccupationStatusType.Employed };
        var context = new ValidationContext(resident) { MemberName = nameof(Resident.Id) };
        var result = Validator.TryValidateProperty(resident.Id, context, null);
        Assert.IsFalse(result);
    }
    
    [Test]
    public void PassportNumValidation_ExceedsMaxLength_ReturnsValidationError()
    {
        var resident = new Resident { Id = 1, FirstName = "John", LastName = "Doe", PassportNum = new string('A', 21), OccupationStatus = OccupationStatusType.Employed };
        var context = new ValidationContext(resident) { MemberName = nameof(Resident.PassportNum) };
        var result = Validator.TryValidateProperty(resident.PassportNum, context, null);
        Assert.IsFalse(result);
    }
    
    [Test]
    public void FirstNameValidation_InvalidLength_ReturnsValidationError()
    {
        var resident = new Resident { Id = 1, FirstName = "A", LastName = "Doe", OccupationStatus = OccupationStatusType.Student };
        var context = new ValidationContext(resident) { MemberName = nameof(Resident.FirstName) };
        var result = Validator.TryValidateProperty(resident.FirstName, context, null);
        Assert.IsFalse(result);
    }
    
    [Test]
    public void OccupationStatusValidation_MissingOccupationStatus_ReturnsValidationError()
    {
        var resident = new Resident { Id = 1, FirstName = "John", LastName = "Doe" };
        var context = new ValidationContext(resident) { MemberName = nameof(Resident.OccupationStatus) };
        var validationResults = new List<ValidationResult>();
        var result = Validator.TryValidateObject(resident, context, validationResults, true);
        Assert.IsFalse(result);
        Assert.IsTrue(validationResults.Exists(v => v.ErrorMessage != null && v.ErrorMessage.Contains("Occupation status is required.")));
    }

    #endregion
    
    #region PublicVehicle

    [Test]
    public void PublicConstructorVehicleValid()
    {
        var vehicle = new PublicVehicle(1, VehicleType.Bus, 50);
        var results = ValidateModel(vehicle);
        Assert.IsEmpty(results, "Expected no validation errors.");
    }
    
    [Test]
    public void IdVehicleInvalid()
    {
        var vehicle = new PublicVehicle { Id = 0, Type = VehicleType.Tram, Capacity = 30 };
        var context = new ValidationContext(vehicle) { MemberName = nameof(PublicVehicle.Id) };
        var result = Validator.TryValidateProperty(vehicle.Id, context, null);
        Assert.IsFalse(result);
    }
    
    [Test]
    public void TypeValidation_MissingType_ReturnsValidationError()
    {
        var vehicle = new PublicVehicle { Id = 1, Capacity = 20 };
        var context = new ValidationContext(vehicle) { MemberName = nameof(PublicVehicle.Type) };
        var validationResults = new List<ValidationResult>();
        var result = Validator.TryValidateObject(vehicle, context, validationResults, true);
        Assert.IsFalse(result);
        Assert.IsTrue(validationResults.Exists(v => v.ErrorMessage != null && v.ErrorMessage.Contains("Type is required.")));
    }

    [Test]
    public void CapacityValidation_NegativeCapacity_ReturnsValidationError()
    {
        var vehicle = new PublicVehicle { Id = 1, Type = VehicleType.Metro, Capacity = -10 };
        var context = new ValidationContext(vehicle) { MemberName = nameof(PublicVehicle.Capacity) };
        var result = Validator.TryValidateProperty(vehicle.Capacity, context, null);
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
        var route = new Route { Id = 1, StartPoint = "Point A", EndPoint = "Point B", StopCount = -1, Duration = 30 };
        var context = new ValidationContext(route) { MemberName = nameof(Route.StopCount) };
        var result = Validator.TryValidateProperty(route.StopCount, context, null);
        Assert.IsFalse(result);
    }
    
    [Test]
    public void DurationValidation_ZeroDuration_ReturnsValidationError()
    {
        var route = new Route { Id = 1, StartPoint = "Point A", EndPoint = "Point B", StopCount = 2, Duration = 0 };
        var context = new ValidationContext(route) { MemberName = nameof(Route.Duration) };
        var result = Validator.TryValidateProperty(route.Duration, context, null);
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void StartPointValidation_InvalidLength_ReturnsValidationError()
    {
        var route = new Route { Id = 1, StartPoint = "A", EndPoint = "Point B", StopCount = 3, Duration = 30 };
        var context = new ValidationContext(route) { MemberName = nameof(Route.StartPoint) };
        var result = Validator.TryValidateProperty(route.StartPoint, context, null);
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void EndPointValidation_InvalidLength_ReturnsValidationError()
    {
        var route = new Route { Id = 1, StartPoint = "Point A", EndPoint = "B", StopCount = 3, Duration = 30 };
        var context = new ValidationContext(route) { MemberName = nameof(Route.EndPoint) };
        var result = Validator.TryValidateProperty(route.EndPoint, context, null);
        Assert.That(result, Is.False);
    }

    #endregion

    #region Schedule

    [Test]
    public void Schedule_Constructor_SetsPropertiesCorrectly()
    {
        int id = 1;
        DateTime startTime = DateTime.Now;
        DateTime endTime = startTime.AddHours(1);
        int frequency = 15;
        var schedule = new Schedule(id, startTime, endTime, frequency);
        Assert.That(schedule.Id, Is.EqualTo(id));
        Assert.That(schedule.StartTime, Is.EqualTo(startTime));
        Assert.That(schedule.EndTime, Is.EqualTo(endTime));
        Assert.That(schedule.Frequency, Is.EqualTo(frequency));
    }

    [Test]
        public void Schedule_RequiredFieldsValidation_ReturnsErrors_WhenRequiredFieldsMissing()
        {
            var schedule = new Schedule();
            var validationContext = new ValidationContext(schedule);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(schedule, validationContext, validationResults, true);
            Assert.That(isValid, Is.False);
            Assert.IsTrue(validationResults.Exists(v => v.ErrorMessage == "Id is required."));
            Assert.IsTrue(validationResults.Exists(v => v.ErrorMessage == "Start time is required."));
            Assert.IsTrue(validationResults.Exists(v => v.ErrorMessage == "End time is required."));
            Assert.IsTrue(validationResults.Exists(v => v.ErrorMessage == "Frequency is required."));
        }

    #endregion
    
    #region Deal
    
    [Test]
    public void Deal_InvalidEndDateIsStartDate()
    {
        var startDate = DateTime.Now;
        var deal = new Deal(1, startDate, startDate, new City(), new Exported());
        var results = ValidateModel(deal);
        Assert.That(results, Has.Exactly(1).Matches<ValidationResult>(r => r.ErrorMessage != null && r.ErrorMessage.Contains("End date must be later than the start date.")));
    }
    
    [Test]
    public void Deal_RequiredPropertiesValidation_ReturnsError_WhenRequiredFieldsAreMissing()
    {
        var deal = new Deal();
    
        var validationContext = new ValidationContext(deal);
        var validationResults = new List<ValidationResult>();
    
        bool isValid = Validator.TryValidateObject(deal, validationContext, validationResults, true);
    
        Assert.That(isValid, Is.False);
        Assert.That(validationResults.Exists(v => v.ErrorMessage == "Id is required."));
        Assert.That(validationResults.Exists(v => v.ErrorMessage == "Start date is required."));
        Assert.That(validationResults.Exists(v => v.ErrorMessage == "End date is required."));
    }
    
    #endregion

    #region City
    
    [Test]
    public void City_Constructor_SetsPropertiesCorrectly()
    {
        string name = "Sample City";
        DateTime dateOfFounding = DateTime.Now.AddYears(-100);
        double area = 1500.5;
        int population = 500000;

        var city = new City(name, dateOfFounding, area, population);

        Assert.That(city.Name, Is.EqualTo(name));
        Assert.That(city.DateOfFounding, Is.EqualTo(dateOfFounding));
        Assert.That(city.Area, Is.EqualTo(area));
        Assert.That(city.Population, Is.EqualTo(population));
    }
    
    [Test]
    public void CityNameFailValidation()
    {
        var name = "A";
        var dateOfFounding = new DateTime(1800, 1, 1);
        var area = 250.5;
        var population = 50000;
        var city = new City(name, dateOfFounding, area, population);
        var validationResults = ValidateModel(city);
        Assert.That(validationResults.Any(v => v.MemberNames.Contains("Name")), "Name validation should fail.");
    }
    
    [Test]
    public void City_ValidateDateOfFounding_ReturnsError_WhenDateOfFoundingInFuture()
    {
        DateTime futureDate = DateTime.Now.AddYears(10); // Invalid future date
        var city = new City("Future City", futureDate, 2000, 100000);
        var validationContext = new ValidationContext(city);
        if (city.DateOfFounding != null)
        {
            var validationResult = City.ValidateDateOfFounding(city.DateOfFounding.Value, validationContext);
            Assert.IsNotNull(validationResult);
            Assert.That(validationResult?.ErrorMessage, Is.EqualTo("Date of founding cannot be in the future."));
        }
    }
    
    [Test]
    public void City_RequiredFieldsValidation_ReturnsErrors_WhenRequiredFieldsMissing()
    {
        var city = new City();
        var validationContext = new ValidationContext(city);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(city, validationContext, validationResults, true);
        Assert.That(isValid, Is.False);
        Assert.That(validationResults.Exists(v => v.ErrorMessage == "Name is required."));
        Assert.That(validationResults.Exists(v => v.ErrorMessage == "Date of founding is required."));
        Assert.That(validationResults.Exists(v => v.ErrorMessage == "Area is required."));
        Assert.That(validationResults.Exists(v => v.ErrorMessage == "Population is required."));
    }
    
    #endregion

    // #region Building
    //
    // [Test]
    // public void BuildingValid()
    // {
    //     var results = ValidateModel(_building);
    //     Assert.IsEmpty(results, "No validation errors.");
    // }
    //
    // [Test]
    // public void BuildingInvalidPrice()
    // {
    //     _building.Price = 0m;
    //     var results = ValidateModel(_building);
    //     Assert.That(results, Has.Exactly(1).Matches<ValidationResult>(r => r.ErrorMessage != null && r.ErrorMessage.Contains("Price must be greater than zero.")));
    // }
    //
    // [Test]
    // public void BuildingInvalidNegative()
    // {
    //     _building.Occupied = -1;
    //     var results = ValidateModel(_building);
    //     Assert.That(results, Has.Exactly(1).Matches<ValidationResult>(r => r.ErrorMessage != null && r.ErrorMessage.Contains("Occupied spaces cannot be negative.")));
    // }
    //
    // #endregion
    
    // #region RecreationalSpace
    //
    // [Test]
    // public void RecreationalSpace_Constructor_SetsPropertiesCorrectly()
    // {
    //     // Arrange
    //     string name = "Awesome Gym";
    //     RecreationalSpaceType type = RecreationalSpaceType.Gym;
    //     decimal entryFee = 20.0m;
    //     List<string> facilities = new List<string> { "Cardio Machines", "Free Weights", "Swimming Pool" };
    //     var space = new RecreationalSpace(name, type, entryFee, facilities)
    //     {
    //         Name = name,
    //         Type = type,
    //         EntryFee = entryFee,
    //         Facilities = facilities
    //     };
    //     Assert.That(space.Name, Is.EqualTo(name));
    //     Assert.That(space.Type, Is.EqualTo(type));
    //     Assert.That(space.EntryFee, Is.EqualTo(entryFee));
    //     Assert.That(space.Facilities, Is.EqualTo(facilities));
    // }
    //
    // [Test]
    // public void RecreationalSpace_RequiredFieldsValidation_ReturnsErrors_WhenFieldsAreMissing()
    // {
    //     var space = new RecreationalSpace
    //     {
    //         Name = null,
    //         Type = null,
    //         EntryFee = null,
    //         Facilities = null
    //     };
    //     var validationContext = new ValidationContext(space);
    //     var validationResults = new List<ValidationResult>();
    //     bool isValid = Validator.TryValidateObject(space, validationContext, validationResults, true);
    //     Assert.IsFalse(isValid);
    //     Assert.IsTrue(validationResults.Exists(v => v.ErrorMessage == "Name is required."));
    //     Assert.IsTrue(validationResults.Exists(v => v.ErrorMessage == "Type is required."));
    //     Assert.IsTrue(validationResults.Exists(v => v.ErrorMessage == "Entry fee is required."));
    //     Assert.IsTrue(validationResults.Exists(v => v.ErrorMessage == "Facilities list is required."));
    // }
    //
    // [Test]
    // public void RecreationalSpace_EntryFeeValidation_ReturnsError_WhenEntryFeeIsNegative()
    // {
    //     var space = new RecreationalSpace
    //     {
    //         Name = "Negative Fee Gym",
    //         Type = RecreationalSpaceType.Gym,
    //         EntryFee = -5.0m, // Invalid entry fee, negative value
    //         Facilities = new List<string> { "Cardio Machines" }
    //     };
    //     var validationContext = new ValidationContext(space);
    //     var validationResults = new List<ValidationResult>();
    //     bool isValid = Validator.TryValidateObject(space, validationContext, validationResults, true);
    //     Assert.That(isValid, Is.False);
    //     Assert.That(validationResults.Exists(v => v.ErrorMessage == "Entry fee must be a non-negative number."));
    // }
    //
    // #endregion
    
    // #region Workplace
    //
    // private readonly Workplace _workplace = new Workplace("TechCorp", IndustryTypeEnum.Technology)
    // {
    //     Id = 1,
    //     Price = 5000m,
    //     OpeningLevel = 1,
    //     CurrLevel = 1,
    //     Address = "123 Business St",
    //     Capacity = 50,
    //     Occupied = 10
    // };
    //
    // [Test]
    // public void WorkplaceConstructorValid()
    // {
    //     var workplace = new Workplace("Health Inc.", IndustryTypeEnum.Technology);
    //     Assert.IsNotNull(workplace);
    //     Assert.That(workplace.CompanyName, Is.EqualTo("Health Inc."));
    //     Assert.That(workplace.IndustryType, Is.EqualTo(IndustryTypeEnum.Technology));
    // }
    //
    // [Test]
    // public void CompanyNameValidation_ValidLength_ReturnsSuccess()
    // {
    //     _workplace.CompanyName = "Valid Company Name";
    //     var context = new ValidationContext(_workplace) { MemberName = nameof(Workplace.CompanyName) };
    //     var result = Validator.TryValidateProperty(_workplace.CompanyName, context, null);
    //     Assert.That(result);
    // }
    //
    // [Test]
    // public void WorkplaceDerivedProperties_InheritedProperties_AreValid()
    // {
    //     _workplace.Price = 8000m;
    //     _workplace.OpeningLevel = 3;
    //     _workplace.CurrLevel = 5;
    //     var context = new ValidationContext(_workplace);
    //     var validationResults = new System.Collections.Generic.List<ValidationResult>();
    //     var isValid = Validator.TryValidateObject(_workplace, context, validationResults, true);
    //     Assert.That(isValid);
    //     Assert.That(_workplace.Price, Is.EqualTo(8000m));
    //     Assert.That(_workplace.OpeningLevel, Is.EqualTo(3));
    //     Assert.That(_workplace.CurrLevel, Is.EqualTo(5));
    // }
    //
    // #endregion

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
        var resource = new Resource { Id = 1, Name = "Water", Price = -1.00m, Quantity = 10 };
        var context = new ValidationContext(resource) { MemberName = nameof(Resource.Price) };
        var result = Validator.TryValidateProperty(resource.Price, context, null);
        Assert.IsFalse(result);
    }

    #endregion
    
    #region Natural
    
    [Test]
    public void OriginValidation_InvalidLength_ReturnsValidationError()
    {
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
        var result = Validator.TryValidateProperty(natural.Origin, context, null);
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void ExpirationDateValidation_PastDate_ReturnsValidationError()
    {
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
        var result = Validator.TryValidateProperty(natural.ExpirationDate, context, null);
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void ExpirationDateValidation_FutureDate_ReturnsSuccess()
    {
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
        var result = Validator.TryValidateProperty(natural.ExpirationDate, context, null);
        Assert.That(result);
    }
    
    #endregion

    #region ManMade

    [Test]
    public void ManMade_Constructor_SetsPropertiesCorrectly()
    {
        int id = 1;
        string name = "ManMade Product";
        bool availability = true;
        decimal price = 100.50m;
        int quantity = 10;
        bool isExportable = false;
        string manufacturer = "Sample Manufacturer";
        int lifespan = 5;
        string description = "Sample product description";
        var manMade = new ManMade(id, name, availability, price, quantity, isExportable, manufacturer, lifespan, description)
        {
            Lifespan = lifespan
        };
        Assert.That(manMade.Id, Is.EqualTo(id));
        Assert.That(manMade.Name, Is.EqualTo(name));
        Assert.That(manMade.Description, Is.EqualTo(description));
        Assert.That(manMade.Availability, Is.EqualTo(availability));
        Assert.That(manMade.Price, Is.EqualTo(price));
        Assert.That(manMade.Quantity, Is.EqualTo(quantity));
        Assert.That(manMade.IsExportable, Is.EqualTo(isExportable));
        Assert.That(manMade.Manufacturer, Is.EqualTo(manufacturer));
        Assert.That(manMade.Lifespan, Is.EqualTo(lifespan));
    }

    [Test]
    public void ManMade_ManufacturerValidation_ReturnsError_WhenManufacturerTooShort()
    {
        var manMade = new ManMade
        {
            Manufacturer = "A",
            Lifespan = 10
        };
        var validationContext = new ValidationContext(manMade);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(manMade, validationContext, validationResults, true);
        Assert.That(isValid, Is.False);
        Assert.That(validationResults.Exists(v => v.ErrorMessage == "Manufacturer name must be between 2 and 100 characters."));
    }
    
    [Test]
    public void ManMade_LifespanValidation_ReturnsError_WhenLifespanIsNegativeOrZero()
    {
        var manMade = new ManMade
        {
            Manufacturer = "Valid Manufacturer",
            Lifespan = 0 // Invalid (Lifespan must be positive)
        };
        var validationContext = new ValidationContext(manMade);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(manMade, validationContext, validationResults, true);
        Assert.That(isValid, Is.False);
        Assert.That(validationResults.Exists(v => v.ErrorMessage == "Lifespan must be a positive number."));
    }

    #endregion

    #region Exported

    [Test]
    public void Exported_Constructor_SetsPropertiesCorrectly()
    {
        int id = 1;
        string name = "Exported Product";
        bool availability = true;
        decimal price = 200.75m;
        int quantity = 50;
        bool isExportable = true;
        string exporter = "Global Exporter";
        string destinationCity = "New York";
        string exportLicense = "ABC123";
        string description = "Sample exported product";
        var exported = new Exported(id, name, availability, price, quantity, isExportable, exporter, destinationCity, exportLicense, description);
        Assert.That(exported.Id, Is.EqualTo(id));
        Assert.That(exported.Name, Is.EqualTo(name));
        Assert.That(exported.Description, Is.EqualTo(description));
        Assert.That(exported.Availability, Is.EqualTo(availability));
        Assert.That(exported.Price, Is.EqualTo(price));
        Assert.That(exported.Quantity, Is.EqualTo(quantity));
        Assert.That(exported.IsExportable, Is.EqualTo(isExportable));
        Assert.That(exported.Exporter, Is.EqualTo(exporter));
        Assert.That(exported.DestinationCity, Is.EqualTo(destinationCity));
        Assert.That(exported.ExportLicense, Is.EqualTo(exportLicense));
    }
    
    [Test]
    public void Exported_ExporterValidation_ReturnsError_WhenExporterTooShort()
    {
        var exported = new Exported
        {
            Exporter = "A", // Invalid (too short)
            DestinationCity = "Valid City",
            ExportLicense = "ValidLicense"
        };
        var validationContext = new ValidationContext(exported);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(exported, validationContext, validationResults, true);
        Assert.That(isValid, Is.False);
        Assert.That(validationResults.Exists(v => v.ErrorMessage == "Exporter name must be between 2 and 100 characters."));
    }
    
    [Test]
    public void Exported_DestinationCityValidation_ReturnsError_WhenDestinationCityTooShort()
    {
        var exported = new Exported
        {
            Exporter = "Valid Exporter",
            DestinationCity = "A", // Invalid (too short)
            ExportLicense = "ValidLicense"
        };
        var validationContext = new ValidationContext(exported);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(exported, validationContext, validationResults, true);
        Assert.That(isValid, Is.False);
        Assert.That(validationResults.Exists(v => v.ErrorMessage == "Destination city must be between 2 and 100 characters."));
    }
    
    [Test]
    public void Exported_ExportLicenseValidation_ReturnsError_WhenExportLicenseTooLong()
    {
        var exported = new Exported
        {
            Exporter = "Valid Exporter",
            DestinationCity = "Valid City",
            ExportLicense = new string('A', 51) // Invalid (too long)
        };
        var validationContext = new ValidationContext(exported);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(exported, validationContext, validationResults, true);
        Assert.That(isValid, Is.False);
        Assert.That(validationResults.Exists(v => v.ErrorMessage == "Export license must not exceed 50 characters."));
    }

    #endregion

    #region Imported

    [Test]
    public void Imported_Constructor_SetsPropertiesCorrectly()
    {
        int id = 1;
        string name = "Sample Product";
        bool availability = true;
        decimal price = 100.0m;
        int quantity = 50;
        bool isExportable = true;
        string importer = "Importer A";
        string originCity = "City A";
        string originCertificate = "Cert123";
        string storageAddress = "123 Warehouse Street";
        var imported = new Imported(id, name, availability, price, quantity, isExportable, importer, originCity, originCertificate, storageAddress);
        Assert.That(imported.Id, Is.EqualTo(id));
        Assert.That(imported.Name, Is.EqualTo(name));
        Assert.That(imported.Availability, Is.EqualTo(availability));
        Assert.That(imported.Price, Is.EqualTo(price));
        Assert.That(imported.Quantity, Is.EqualTo(quantity));
        Assert.That(imported.IsExportable, Is.EqualTo(isExportable));
        Assert.That(imported.Importer, Is.EqualTo(importer));
        Assert.That(imported.OriginCity, Is.EqualTo(originCity));
        Assert.That(imported.OriginCertificate, Is.EqualTo(originCertificate));
        Assert.That(imported.StorageAddress, Is.EqualTo(storageAddress));
    }
    
    [Test]
    public void Imported_RequiredFieldsValidation_ReturnsErrors_WhenFieldsAreMissing()
    {
        var imported = new Imported();
        var validationContext = new ValidationContext(imported);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(imported, validationContext, validationResults, true);
        Assert.That(isValid, Is.False);
        Assert.IsTrue(validationResults.Exists(v => v.ErrorMessage == "Importer is required."));
        Assert.IsTrue(validationResults.Exists(v => v.ErrorMessage == "Origin city is required."));
        Assert.IsTrue(validationResults.Exists(v => v.ErrorMessage == "Origin certificate is required."));
        Assert.IsTrue(validationResults.Exists(v => v.ErrorMessage == "Storage address is required."));
    }
    
    [Test]
    public void Imported_ImporterValidation_ReturnsError_WhenNameLengthInvalid()
    {
        var imported = new Imported { Importer = "A" }; // Invalid name, too short
        var validationContext = new ValidationContext(imported);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(imported, validationContext, validationResults, true);
        Assert.That(isValid, Is.False);
        Assert.IsTrue(validationResults.Exists(v => v.ErrorMessage == "Importer name must be between 2 and 100 characters."));
    }

    #endregion
    
    #region Validation
    
    private List<ValidationResult> ValidateModel<T>(T obj)
    {
        var validationResults = new List<ValidationResult>();
        if (obj != null)
        {
            var validationContext = new ValidationContext(obj, null, null);
            Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }

        return validationResults;
    }
    

    #endregion

}
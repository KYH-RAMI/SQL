using FailSafeRentals.Core.Models;
using FailSafeRentals.Data;

using Microsoft.EntityFrameworkCore;

using System.Diagnostics;

using var _db = new FailSafeRentalsDbContext();

//upgift 1
//CreateCustomer("Anna Andersson", "anna.andersson@kalleanka.se", "SE1234567");
//CreateNewVehicle();

//uppgift 2
//ListVehicles();

//uppgift 3
//FindVehicleById();

//uppgift 4 Update column vehicle
//UpdateVehicle();

//uppgift 5
//CreateNewRental();
//CreateNewRental2();

//uppgift 6
//ListRentalsDetails();

//uppgift 7
//SlimReport();

//uppgift 8
//var importer = new VehicleImport(_db);
//importer.ImportFromTableFile("vehicles.txt", 1);

//uppgift 9
//await ShowWeatherAndSuggestVehicles();

//uppgift 10
//ShowVehicleExtraFeatures();

//uppgift 11
//SearchVehicleSecurely();

//uppgift 12
//SearchVehiclesWithSafeSql();

//uppgift 13
//GetVehiclesAboveRate();

//uppgift 14
//ShowRentalStatistics();

//uppgift 15
//await TestRentalServiceAsync();

if(Debugger.IsAttached)
{
    Console.Write("\n\nTryck på en tangent för att stänga fönstret...");
    Console.ReadKey();
}

void CreateCustomer(string fullName, string email, string driverLicenseNumber)
{
    var customer = new Customer
    {
        FullName = fullName,
        Email = email,
        DriverLicenseNumber = driverLicenseNumber
    };
    _db.Customers.Add(customer);
    _db.SaveChanges();
    Console.WriteLine($"Kund tillagd: \n\n ID: {customer.Id}, {customer.FullName}");
}

void ListVehicles()
{
    var availableVehicles = _db.Vehicles
        .Where(v => v.IsAvailable)
        .OrderBy(v => v.DailyRate)
        .ToList();
    Console.WriteLine();
    Console.WriteLine("Tillgängliga bilar i systemet: ");
    Console.WriteLine();
    foreach(var vehicle in availableVehicles)
    {
        Console.WriteLine($"{vehicle.RegistrationNumber}, {vehicle.Brand} {vehicle.Model}, {vehicle.DailyRate:C} / dag");
    }
}

void FindVehicleById()
{
    // 1. Hämta via primärnyckel med Find
    var vehicle = _db.Vehicles.Find(1);
    Console.WriteLine($"\n\nFind(1): {vehicle?.Model ?? "hittades inte"} \n");

    // 2. Hämta via registreringsnummer med FirstOrDefault
    var byPlate = _db.Vehicles
        .FirstOrDefault(v => v.RegistrationNumber == "ABC123");
    Console.WriteLine($"\nFirstOrDefault: {byPlate?.Model ?? "hittades inte"} \n");
}

void CreateNewVehicle()
{
    var vehicle = new Vehicle
    {
        RegistrationNumber = "XYZ 789",
        Brand = "Toyota",
        Model = "Camry",
        DailyRate = 150.00m,
        IsAvailable = true
    };
    _db.Vehicles.Add(vehicle);
    _db.SaveChanges();
    Console.WriteLine($"Fordon tillagt: {vehicle.RegistrationNumber}, {vehicle.Brand} {vehicle.Model}");
}

void UpdateVehicle()
{
    var vehicle = _db.Vehicles
        .FirstOrDefault(v => v.RegistrationNumber == "ABC 123");
    if(vehicle is not null)
    {
        vehicle.DailyRate += 200.00m;
        _db.SaveChanges();
        Console.WriteLine($"Fordon uppdaterat: {vehicle.RegistrationNumber}, {vehicle.Brand} {vehicle.Model}, Nytt pris: {vehicle.DailyRate:C}");
        //UPDATE [Vehicles] SET [DailyRate] = @p0 OUTPUT 1 WHERE[Id] = @p1;
    }
    else
    {
        Console.WriteLine("Fordon hittades inte.");
    }
}

    void CreateNewRental()
    {
        var vehicle = _db.Vehicles.First(v => v.IsAvailable);
        var customer = _db.Customers.Find(1);
        var rental = new Rental
        {
            VehicleId = vehicle.Id,
            CustomerId = customer.Id,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(3),
            TotalCost = vehicle.DailyRate * 3
        };
        vehicle.IsAvailable = false;
        _db.Rentals.Add(rental);
        _db.SaveChanges();

        Console.WriteLine();
        Console.WriteLine($"Ny uthyrning skapad: \n\n ID: {rental.Id}, Fordon ID: {rental.VehicleId}, Kund ID: {rental.CustomerId}, Startdatum: {rental.StartDate}, Slutdatum: {rental.EndDate}");
    /*
     *
     INSERT INTO [Rentals] ([CustomerId], [EndDate], [StartDate], [TotalCost], [VehicleId])
      OUTPUT INSERTED.[Id], INSERTED.[CreatedAt]
      VALUES (@p0, @p1, @p2, @p3, @p4);
      UPDATE [Vehicles] SET [IsAvailable] = @p5
      OUTPUT 1
      WHERE [Id] = @p6;
    */
    }

void CreateNewRental2()
{
    var vehicle = _db.Vehicles.Where(v => v.IsAvailable).FirstOrDefault();
    var customer = _db.Customers.Find(1);
    var rental = new Rental
    {
        VehicleId = vehicle.Id,
        CustomerId = customer.Id,
        StartDate = DateTime.UtcNow,
        EndDate = DateTime.UtcNow.AddDays(3),
        TotalCost = vehicle.DailyRate * 3
    };
    vehicle.IsAvailable = false;
    _db.Rentals.Add(rental);
    _db.SaveChanges();
    Console.WriteLine();
    Console.WriteLine($"Ny uthyrning skapad: \n\n ID: {rental.Id}, Fordon ID: {rental.VehicleId}, Kund ID: {rental.CustomerId}, Startdatum: {rental.StartDate}, Slutdatum: {rental.EndDate}, Totalkostnad: {rental.TotalCost:C}");

}

void ListRentalsDetails()
{
    var rentals = _db.Rentals
        .Include(r => r.Vehicle)
        .Include(r => r.Customer)
        .ToList();
    Console.WriteLine();
    Console.WriteLine("RÄTT SÄTT");
    Console.WriteLine("Alla uthyrningar i systemet: ");
    Console.WriteLine();
    foreach(var rental in rentals)
    {
        Console.WriteLine($"ID: {rental.Id}, Fordon: {rental.Vehicle.RegistrationNumber}, Kund: {rental.Customer.FullName}, Startdatum: {rental.StartDate}, Slutdatum: {rental.EndDate}, Totalkostnad: {rental.TotalCost:C}");
    }

    var rentals2 = _db.Rentals.ToList();
    Console.WriteLine("FEL SÄTT");

    foreach(var rental2 in rentals)
    {

        _db.Entry(rental2).Reference(r => r.Vehicle).Load();
        _db.Entry(rental2).Reference(r => r.Customer).Load();
        Console.WriteLine($"\nID: {rental2.Id}, Fordon: {rental2.Vehicle.RegistrationNumber}, \n Kund: {rental2.Customer.FullName}, \n Startdatum: {rental2.StartDate}, \n Slutdatum: {rental2.EndDate}, \n Totalkostnad: {rental2.TotalCost:C}");
    }
}

void SlimReport()
{
    var rentals = _db.Rentals
        .AsNoTracking()
        .Where(r => r.TotalCost > 1500m)
        .Select(r => new
        {
            RentalId = r.Id,
            VehicleReg = r.Vehicle.RegistrationNumber,
            CustomerName = r.Customer.FullName,
            VehicleInfo = $"{r.Vehicle.Brand} {r.Vehicle.Model}",
           Rentaltime = $"{r.StartDate} - {r.EndDate}",
            TotalCost = r.TotalCost
        })
        .ToList();
    foreach(var rental in rentals)
    {
        Console.WriteLine($"\nID: {rental.RentalId}, Fordon: {rental.VehicleReg}, \n Kund: {rental.CustomerName}, \n Fordonsinfo: {rental.VehicleInfo}, \n Uthyrningstid: {rental.Rentaltime}, \n Totalkostnad: {rental.TotalCost:C}");
    }

}
async Task ShowWeatherAndSuggestVehicles()
{
    var weatherService = new WeatherService();

    var weather = await weatherService.GetWeatherAsync();

    Console.WriteLine("Aktuellt väder i Göteborg:");
    Console.WriteLine($"Temperatur: {weather.Current.Temperature} °C");
    Console.WriteLine($"Vindhastighet: {weather.Current.WindSpeed} km/h");

    // Fordonsförslag baserat på vädret
    var availableVehicles = _db.Vehicles
        .AsNoTracking()
        .Where(v => v.IsAvailable);

    if(weather.Current.Temperature < 0)
    {
        availableVehicles = availableVehicles
            .Where(v => v.Brand == "Volvo" || v.Brand == "Audi");
    }

    var suggestedVehicles = availableVehicles
        .OrderBy(v => v.DailyRate)
        .ToList();

    Console.WriteLine("\nRekommenderade fordon:");

    foreach(var vehicle in suggestedVehicles)
    {
        Console.WriteLine(
            $"{vehicle.RegistrationNumber} - {vehicle.Brand} {vehicle.Model} - {vehicle.DailyRate:C}/dag");
    }

    if(weather.Current.WindSpeed > 15)
    {
        Console.WriteLine("\nObservera: Det blåser kraftigt.");
    }

    if(suggestedVehicles.Count == 0)
    {
        Console.WriteLine("Inga tillgängliga fordon matchar vädervillkoren.");
    }
}
void ShowVehicleExtraFeatures()
{
    var vehicle = _db.Vehicles
        .Include(v => v.ExtraFeatures)
        .OrderBy(v => v.Id)
        .FirstOrDefault();

    if(vehicle is null)
    {
        Console.WriteLine("Inget fordon hittades.");
        return;
    }

    Console.WriteLine($"Valt fordon: {vehicle.Brand} {vehicle.Model}");

    var winterTires = new ExtraFeature
    {
        Name = "Vinterdäck Dubb",
        DailyPrice = 150m
    };

    var gps = new ExtraFeature
    {
        Name = "GPS Navigation",
        DailyPrice = 75m
    };

    vehicle.ExtraFeatures.Add(winterTires);
    vehicle.ExtraFeatures.Add(gps);

    _db.SaveChanges();

    Console.WriteLine("\nTillval kopplade till bilen:");

    foreach(var feature in vehicle.ExtraFeatures)
    {
        Console.WriteLine($"{feature.Name} - {feature.DailyPrice:C}/dag");
    }
}

// Uppgift 11 - SQL Injection
// EF Core använder parametriserade SQL-frågor vid LINQ.
// Användarens inmatning skickas som parametern @input
// (motsvarande @p0 i andra genererade frågor).
// SQL Server behandlar parametern som data, inte som SQL-kod.
// Därför kan inmatningen inte köra DROP TABLE Vehicles.
// Testresultat: 0 träffar och ingen SQL Injection.
void SearchVehicleSecurely()
{
    Console.Write("Ange registreringsnummer: ");
    string input = Console.ReadLine() ?? "";

    var vehicles = _db.Vehicles
        .Where(v => v.RegistrationNumber == input)
        .ToList();

    Console.WriteLine($"Antal träffar: {vehicles.Count}");

    foreach(var vehicle in vehicles)
    {
        Console.WriteLine(
            $"{vehicle.RegistrationNumber} - {vehicle.Brand} {vehicle.Model}");
    }
}
void SearchVehiclesWithSafeSql()
{
    Console.Write("Ange bilmärke: ");
    string brandInput = Console.ReadLine() ?? "";

    var vehicles = _db.Vehicles
        .FromSqlInterpolated(
            $"SELECT * FROM Vehicles WHERE Brand = {brandInput}")
        .AsNoTracking()
        .ToList();

    Console.WriteLine($"\nAntal träffar: {vehicles.Count}");

    foreach(var vehicle in vehicles)
    {
        Console.WriteLine(
            $"{vehicle.RegistrationNumber} - {vehicle.Brand} {vehicle.Model}");
    }
}

void GetVehiclesAboveRate()
{
    decimal minDailyRate = 1000m;

    var vehicles = _db.Vehicles
        .FromSqlInterpolated(
            $"EXEC dbo.usp_GetVehiclesAboveRate @MinDailyRate = {minDailyRate}")
        .AsNoTracking()
        .ToList();

    Console.WriteLine($"\nFordon med dygnspris från {minDailyRate:C}:");

    foreach(var vehicle in vehicles)
    {
        Console.WriteLine(
            $"{vehicle.RegistrationNumber} - {vehicle.Brand} {vehicle.Model} - {vehicle.DailyRate:C}/dag");
    }
}

void ShowRentalStatistics()
{
    var statistics = _db.Rentals
        .AsNoTracking()
        .GroupBy(r => r.Vehicle.Brand)
        .Select(g => new
        {
            Brand = g.Key,
            RentalCount = g.Count(),
            TotalRevenue = g.Sum(r => r.TotalCost),
            AverageCost = g.Average(r => r.TotalCost)
        })
        .OrderByDescending(s => s.TotalRevenue)
        .ToList();

    Console.WriteLine("\nUTHYRNINGSSTATISTIK PER BILMÄRKE");
    Console.WriteLine("--------------------------------");

    foreach(var item in statistics)
    {
        Console.WriteLine($"\nBilmärke: {item.Brand}");
        Console.WriteLine($"Antal uthyrningar: {item.RentalCount}");
        Console.WriteLine($"Total intäkt: {item.TotalRevenue:C}");
        Console.WriteLine($"Genomsnittlig bokningskostnad: {item.AverageCost:C}");
    }
}

async Task TestRentalServiceAsync()
{
    var rentalService = new RentalService(_db);

    // Test 1: Hämta tillgängliga fordon
    var vehicles = await rentalService.GetAvailableVehiclesAsync();

    Console.WriteLine("\nTillgängliga fordon:");

    foreach(var vehicle in vehicles)
    {
        Console.WriteLine(
            $"ID: {vehicle.Id} - {vehicle.Brand} {vehicle.Model} - {vehicle.DailyRate:C}/dag");
    }

    // Test 2: Kontrollera att uthyrningsmetoden finns
    Console.WriteLine("\nRentalService är redo att skapa uthyrningar.");

    // Kontrollera testkunden innan vi skapar en uthyrning
    var customer = await _db.Customers.FindAsync(1);

    if(customer is not null)
    {
        Console.WriteLine($"\nTestkund: ID {customer.Id} - {customer.FullName}");
    }
    else
    {
        Console.WriteLine("\nKund med ID 1 finns inte.");
    }
    // Uppgift 15 – Testa CreateRentalAsync()
    if(customer is not null)
    {
        var rental = await rentalService.CreateRentalAsync(
            vehicleId: 11,
            customerId: 1,
            numberOfDays: 2);

        Console.WriteLine("\nTESTUTHYRNING SKAPAD");
        Console.WriteLine($"Uthyrnings-ID: {rental.Id}");
        Console.WriteLine($"Fordons-ID: {rental.VehicleId}");
        Console.WriteLine($"Kund-ID: {rental.CustomerId}");
        Console.WriteLine($"Total kostnad: {rental.TotalCost:C}");
    }
}
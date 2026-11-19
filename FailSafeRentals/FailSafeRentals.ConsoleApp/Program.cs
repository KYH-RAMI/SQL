using FailSafeRentals.ConsoleApp.Data;
using FailSafeRentals.ConsoleApp.Models;

using Microsoft.EntityFrameworkCore;

using System.Diagnostics;

using var _db = new FailSafeRentalsDbContext();

if (!_db.Vehicles.Any())
{
    var gps = _db.ExtraFeatures.FirstOrDefault(f => f.Name == "GPS Navigation")
              ?? new ExtraFeature { Name = "GPS Navigation", DailyPrice = 75m };

    var car = new Vehicle
    {
        RegistrationNumber = "ABC123",
        Brand = "Volvo",
        Model = "XC60",
        DailyRate = 895m,
        ExtraFeatures = [gps]
    };

    var customer = new Customer
    {
        FullName = "Anna Lindqvist",
        Email = "anna@failsafe.se",
        DriverLicenseNumber = "SE-998877"
    };

    _db.Rentals.Add(new Rental
    {
        Customer = customer,
        Vehicle = car,
        StartDate = DateTime.UtcNow,
        TotalCost = 2685m
    });

    _db.SaveChanges();
}

var rental = _db.Rentals
    .Include(r => r.Customer)
    .Include(r => r.Vehicle)
    .First();

Console.WriteLine($"Verifierat: {rental.Customer.FullName} hyr {rental.Vehicle.Brand} ({rental.Vehicle.RegistrationNumber})");

if (Debugger.IsAttached)
{
    Console.Write("\n\nTryck på en tangent för att stänga fönstret...");
    Console.ReadKey();
}
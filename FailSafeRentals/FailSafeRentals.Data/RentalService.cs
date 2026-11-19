using FailSafeRentals.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FailSafeRentals.Data;

public class RentalService
{
    private readonly FailSafeRentalsDbContext _db;

    public RentalService(FailSafeRentalsDbContext db)
    {
        _db = db;
    }

    public async Task<List<Vehicle>> GetAvailableVehiclesAsync()
    {
        return await _db.Vehicles
            .AsNoTracking()
            .Where(v => v.IsAvailable)
            .OrderBy(v => v.DailyRate)
            .ToListAsync();
    }
    public async Task<Rental> CreateRentalAsync(
    int vehicleId,
    int customerId,
    int numberOfDays)
    {
        if(numberOfDays <= 0)
            throw new ArgumentException("Antalet dagar måste vara större än 0.");

        var vehicle = await _db.Vehicles
            .FirstOrDefaultAsync(v => v.Id == vehicleId);

        if(vehicle is null)
            throw new InvalidOperationException("Fordonet finns inte.");

        if(!vehicle.IsAvailable)
            throw new InvalidOperationException("Fordonet är inte tillgängligt.");

        var customerExists = await _db.Customers
            .AnyAsync(c => c.Id == customerId);

        if(!customerExists)
            throw new InvalidOperationException("Kunden finns inte.");

        var rental = new Rental
        {
            VehicleId = vehicleId,
            CustomerId = customerId,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(numberOfDays),
            TotalCost = vehicle.DailyRate * numberOfDays
        };

        vehicle.IsAvailable = false;

        _db.Rentals.Add(rental);

        await _db.SaveChangesAsync();

        return rental;
    }
}
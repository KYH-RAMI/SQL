using FailSafeRentals.Core.Models;
using FailSafeRentals.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FailSafeRentals.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly FailSafeRentalsDbContext _db;

    public VehiclesController(FailSafeRentalsDbContext db)
    {
        _db = db;
    }
    // Uppgift 8
    [HttpGet]
    public async Task<ActionResult<List<Vehicle>>> GetVehicles()
    {
        var vehicles = await _db.Vehicles.ToListAsync();
        return Ok(vehicles);
    }
    // Uppgift 9
    [HttpPost]
    public async Task<ActionResult<Vehicle>> CreateVehicle([FromBody] Vehicle vehicle)
    {
        _db.Vehicles.Add(vehicle);
        await _db.SaveChangesAsync();

        return Created($"/api/Vehicles/{vehicle.Id}", vehicle);
    }
}
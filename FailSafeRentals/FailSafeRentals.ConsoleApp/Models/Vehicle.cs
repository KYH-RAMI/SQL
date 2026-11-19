using System.ComponentModel.DataAnnotations;

namespace FailSafeRentals.ConsoleApp.Models;

public class Vehicle
{
    public int Id { get; set; }

    [Required, MaxLength(10)]
    public string RegistrationNumber { get; set; } = null!;

    [Required, MaxLength(50)]
    public string Brand { get; set; } = null!;

    [Required, MaxLength(50)]
    public string Model { get; set; } = null!;

    public decimal DailyRate { get; set; }
    public bool IsAvailable { get; set; } = true;

    public List<Rental> Rentals { get; set; } = [];
    public List<ExtraFeature> ExtraFeatures { get; set; } = [];
}

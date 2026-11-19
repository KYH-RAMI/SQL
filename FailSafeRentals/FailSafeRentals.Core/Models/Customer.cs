using System.ComponentModel.DataAnnotations;

namespace FailSafeRentals.Core.Models;

public class Customer
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string FullName { get; set; } = null!;

    [Required, MaxLength(200)]
    public string Email { get; set; } = null!;

    [Required, MaxLength(50)]
    public string DriverLicenseNumber { get; set; } = null!;

    public List<Rental> Rentals { get; set; } = [];
}

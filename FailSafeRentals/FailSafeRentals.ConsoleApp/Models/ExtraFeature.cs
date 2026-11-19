using System.ComponentModel.DataAnnotations;

namespace FailSafeRentals.ConsoleApp.Models;

public class ExtraFeature
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; } = null!;

    public decimal DailyPrice { get; set; }

    public List<Vehicle> Vehicles { get; set; } = new();
}

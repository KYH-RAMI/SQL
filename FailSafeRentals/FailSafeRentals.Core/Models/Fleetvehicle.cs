using System.ComponentModel.DataAnnotations;

namespace FailSafeRentals.Core.Models;

public class FleetVehicle
{
    public int Id { get; set; }

    [Required, MaxLength(10)]
    public string RegistrationNumber { get; set; } = null!;

    //[Required, MaxLength(100)]
    //public string MakeAndModel { get; set; } = null!;
    [Required, MaxLength(40)]
    public string Make { get; set; } = string.Empty;

    [Required, MaxLength(40)]
    public string Model { get; set; } = string.Empty;


    [Required]
    public int MileageKm { get; set; }
    public bool NeedsService { get; set; } = false;

    // Explicit främmande nyckel (<Tabellnamn>Id) + navigeringsegenskap
    public int RentalStationId { get; set; }
    public RentalStation RentalStation { get; set; } = null!;

    // Lägg DamageReports explicit
    public int DamageReportId { get; set; }
    public List<DamageReport> DamageReports { get; set; } = null!;

    // Ett fordon kan ha många underhållstaggar
    public List<MaintenanceTag> MaintenanceTags { get; set; } = [];
}

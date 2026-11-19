using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FailSafeRentals.ConsoleApp.Models;

public class DamageReport
{
    public int Id { get; set; }

    [Required, MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Severity { get; set; } = string.Empty;
    public decimal EstimatedCost { get; set; }
    public DateTime ReportedAt { get; set; }
    public DateTime? RepairedAt { get; set; }

    // FK och navigation
    public int FleetVehicleId { get; set; }
    public FleetVehicle FleetVehicle { get; set; } = null!;
}

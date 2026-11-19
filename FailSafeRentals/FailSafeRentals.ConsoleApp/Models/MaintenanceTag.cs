using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FailSafeRentals.ConsoleApp.Models;

public class MaintenanceTag
{
    public int Id { get; set; }

    [Required, MaxLength(40)]
    public string TagName { get; set; } = string.Empty;

    //  En tag kan gälla för många Vehicles
    public List<FleetVehicle> Vehicles { get; set; } = [];
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FailSafeRentals.ConsoleApp.Models;

public class RentalStation
{
    public int Id { get; set; }

    [Required, MaxLength(10)]
    public string StationCode { get; set; } = null!;

    [Required, MaxLength(80)]
    public string City { get; set; } = null!;

    [Required, MaxLength(200)]
    public string StreetAddress { get; set; } = null!;

    // Navigeringsegenskap: En station har många fordon
    public List<FleetVehicle> Vehicles { get; set; } = [];
}

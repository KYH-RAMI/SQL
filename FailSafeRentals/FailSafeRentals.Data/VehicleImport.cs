using System.Globalization;
using FailSafeRentals.Core.Models;

namespace FailSafeRentals.Data;

public class VehicleImport(FailSafeRentalsDbContext db)
{
    public void ImportFromTableFile(string filePath, int stationId)
    {
        if(!File.Exists(filePath))
        {
            Console.WriteLine("Importfilen finns inte.");
            return;
        }

        if(!db.RentalStations.Any(s => s.Id == stationId))
        {
            Console.WriteLine("Uthyrningsstationen finns inte.");
            return;
        }

        var lines = File.ReadAllLines(filePath);

        var existingRegistrations = db.FleetVehicles
            .Select(v => v.RegistrationNumber)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var importedVehicles = new List<FleetVehicle>();
        var errors = new List<string>();

        for(int i = 1; i < lines.Length; i++)
        {
            int lineNumber = i + 1;
            var columns = lines[i].Split('\t');

            // Kontrollera antal kolumner.
            if(columns.Length != 3)
            {
                errors.Add($"Rad {lineNumber}: Fel antal kolumner.");
                continue;
            }

            string registration = columns[0].Trim().ToUpperInvariant();
            string brand = columns[1].Trim();

            // Kontrollera obligatoriska värden.
            if(string.IsNullOrWhiteSpace(registration) ||
                string.IsNullOrWhiteSpace(brand))
            {
                errors.Add($"Rad {lineNumber}: Registreringsnummer eller märke saknas.");
                continue;
            }

            if(registration.Length > 10 || brand.Length > 40)
            {
                errors.Add($"Rad {lineNumber}: Registreringsnummer eller märke är för långt.");
                continue;
            }

            // Kontrollera mätarställning.
            if(!decimal.TryParse(
                    columns[2].Trim(),
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out decimal odometer) ||
                odometer < 0 ||
                odometer > int.MaxValue)
            {
                errors.Add($"Rad {lineNumber}: Ogiltig mätarställning.");
                continue;
            }

            // Förhindra dubbletter.
            if(!existingRegistrations.Add(registration))
            {
                errors.Add($"Rad {lineNumber}: Registreringsnumret finns redan.");
                continue;
            }

            importedVehicles.Add(new FleetVehicle
            {
                RegistrationNumber = registration,
                Make = brand,
                Model = "Okänd",
                MileageKm = (int)Math.Round(
                    odometer,
                    0,
                    MidpointRounding.AwayFromZero),
                RentalStationId = stationId
            });
        }

        // Spara alla godkända fordon.
        db.FleetVehicles.AddRange(importedVehicles);
        db.SaveChanges();

        Console.WriteLine($"\nImporten är klar.");
        Console.WriteLine($"Importerade: {importedVehicles.Count}");
        Console.WriteLine($"Avvisade: {errors.Count}");

        foreach(var error in errors)
        {
            Console.WriteLine(error);
        }
    }
}
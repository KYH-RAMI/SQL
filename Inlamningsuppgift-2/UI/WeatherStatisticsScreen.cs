using FailSafe_WeatherMonitor.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FailSafe_WeatherMonitor.UI;

internal static class WeatherStatisticsScreen
{
    public static async Task ShowAsync()
    {
        await using var db = new WeatherDataContext();

        // Hämta unika mätstationer från databasen.
        var stations = await db.WeatherDataPoints
            .AsNoTracking()
            .Select(x => x.StationName)
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync();

        if(stations.Count == 0)
        {
            AppTheme.ShowNotice(
                "NO WEATHER DATA",
                "No weather measurements are available.",
                NoticeType.Info);

            return;
        }

        while(true)
        {
            // Välj station med det befintliga menysystemet.
            string? selectedStation =
                AppTheme.ShowStationSelector(stations);

            if(selectedStation is null)
                return;

            // Beräkna statistik för vald station.
            var statistics = await db.WeatherDataPoints
                .AsNoTracking()
                .Where(x => x.StationName == selectedStation)
                .GroupBy(x => 1)
                .Select(g => new
                {
                    TotalRecords = g.Count(),
                    AverageTemperature = g.Average(x => x.Temperature),
                    MinTemperature = g.Min(x => x.Temperature),
                    MaxTemperature = g.Max(x => x.Temperature),
                    AverageHumidity = g.Average(x => x.Humidity)
                })
                .SingleOrDefaultAsync();

            if(statistics is null)
            {
                AppTheme.ShowNotice(
                    "NO WEATHER DATA",
                    "No measurements were found for this station.",
                    NoticeType.Info);

                continue;
            }

            // Hämta stationens samtliga mätningar i tidsordning.
            var measurements = await db.WeatherDataPoints
                .AsNoTracking()
                .Where(x => x.StationName == selectedStation)
                .OrderBy(x => x.Timestamp)
                .ThenBy(x => x.Id)
                .ToListAsync();

            // Visa statistik med befintlig design och gradient.
            AppTheme.ShowNotice(
                "WEATHER STATISTICS",
                $"Total measurements: {statistics.TotalRecords}\n" +
                $"Average temperature: {statistics.AverageTemperature.ToString("F1", CultureInfo.InvariantCulture)}°C\n" +
                $"Lowest temperature: {statistics.MinTemperature.ToString("F1", CultureInfo.InvariantCulture)}°C\n" +
                $"Highest temperature: {statistics.MaxTemperature.ToString("F1", CultureInfo.InvariantCulture)}°C\n" +
                $"Average humidity: {statistics.AverageHumidity.ToString("F1", CultureInfo.InvariantCulture)}%",
                NoticeType.Info,
                stationName: selectedStation,
                footerText: "Press any key to view measurements");

            // Visa stationens mätningar i en sidindelad tabell.
            AppTheme.ShowMeasurementsGrid(
                selectedStation,
                measurements);

            // Escape från mätningstabellen återgår till stationsvalet.
        }
    }
}
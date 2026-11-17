using FailSafe_WeatherMonitor.Data;
using FailSafe_WeatherMonitor.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json;

namespace FailSafe_WeatherMonitor.Services;

internal class ImportService
{
    private async Task<JsonDocument> ReadJsonFileAsync(string filePath)
    {
        await using var stream = File.OpenRead(filePath);

        return await JsonDocument.ParseAsync(stream);
    }

    private static bool TryGetDouble(JsonElement element, out double value)
    {
        value = 0;

        if(element.ValueKind == JsonValueKind.Number)
        {
            return element.TryGetDouble(out value) && double.IsFinite(value);
        }

        if(element.ValueKind == JsonValueKind.String)
        {
            string? text = element.GetString()?.Trim().Replace(',', '.');

            return double.TryParse(
                text,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out value) && double.IsFinite(value);
        }

        return false;
    }

    public async Task<ImportResult> ImportAsync(string filePath)
    {
        using JsonDocument document = await ReadJsonFileAsync(filePath);

        if(document.RootElement.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidDataException("The JSON file must contain an array.");
        }

        List<string> badData = new();
        List<WeatherDataPoint> validData = new();
        int duplicates = 0;

        foreach(var jsonElement in document.RootElement.EnumerateArray())
        {
            string? stationName = null;

            if(jsonElement.ValueKind == JsonValueKind.Object &&
                jsonElement.TryGetProperty("StationName", out JsonElement stationNameElement) &&
                stationNameElement.ValueKind == JsonValueKind.String)
            {
                stationName = stationNameElement.GetString()?.Trim();
            }

            if(string.IsNullOrWhiteSpace(stationName))
            {
                badData.Add(JsonSerializer.Serialize(jsonElement));
                continue;
            }

            string? timestampText = null;

            if(jsonElement.TryGetProperty("Timestamp", out JsonElement timestampElement) &&
                timestampElement.ValueKind == JsonValueKind.String)
            {
                timestampText = timestampElement.GetString()?.Trim();
            }

            if(!DateTime.TryParseExact(
                timestampText,
                "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime timestamp))
            {
                badData.Add(JsonSerializer.Serialize(jsonElement));
                continue;
            }

            if(!jsonElement.TryGetProperty("Humidity", out JsonElement humidityElement) ||
                !TryGetDouble(humidityElement, out double humidity))
            {
                badData.Add(JsonSerializer.Serialize(jsonElement));
                continue;
            }

            if(!jsonElement.TryGetProperty("Temperature", out JsonElement temperatureElement) ||
                !TryGetDouble(temperatureElement, out double temperature))
            {
                badData.Add(JsonSerializer.Serialize(jsonElement));
                continue;
            }

            // Validera meteorologisk rimlighet
            if(Math.Abs(temperature) > 60 || humidity < 0 || humidity > 100)
            {
                badData.Add(JsonSerializer.Serialize(jsonElement));
                continue;
            }

            var item = new WeatherDataPoint
            {
                StationName = stationName,
                Timestamp = timestamp,
                Humidity = humidity,
                Temperature = temperature,
                DataSource = "JSON-File"
            };

            // Kontrollera dubbletter inom JSON-filen
            bool exists = validData.Any(x =>
                x.StationName == item.StationName &&
                x.Timestamp == item.Timestamp);

            if(exists)
            {
                duplicates++;
                continue;
            }

            validData.Add(item);
        }

        // Databasintegration
        using var db = new WeatherDataContext();

        var existingRecords = await db.WeatherDataPoints
            .Select(x => new { x.StationName, x.Timestamp })
            .ToListAsync();

        var existingKeys = existingRecords
            .Select(x => (x.StationName, x.Timestamp))
            .ToHashSet();

        List<WeatherDataPoint> newData = new();

        foreach(var item in validData)
        {
            bool exists = !existingKeys.Add((item.StationName, item.Timestamp));

            if(exists)
            {
                duplicates++;
                continue;
            }

            newData.Add(item);
        }

        // Spara nya poster
        if(newData.Count > 0)
        {
            db.WeatherDataPoints.AddRange(newData);
            await db.SaveChangesAsync();
        }

        // Logga felaktiga poster
        string logPath = Path.Combine(
            AppContext.BaseDirectory,
            "bad_data.log");

        await File.WriteAllLinesAsync(logPath, badData);

        return new ImportResult(
            newData.Count,
            badData.Count,
            duplicates);
    }
}

internal sealed record ImportResult(
    int Imported,
    int Rejected,
    int Duplicates);
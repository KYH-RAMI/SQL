using FailSafe_WeatherMonitor.Data;
using FailSafe_WeatherMonitor.Models;
using Microsoft.EntityFrameworkCore;

namespace FailSafe_WeatherMonitor.Services;

internal class WeatherService
{
    private readonly WeatherApiService _weatherApiService = new();

    private static WeatherDataPoint MapToWeatherDataPoint(CurrentWeather current)
    {
        return new WeatherDataPoint
        {
            StationName = "Stockholm",
            Temperature = current.Temperature2m,
            Humidity = current.RelativeHumidity2m,
            DataSource = "API",
            Timestamp = current.Time
        };
    }

    public async Task<WeatherDataPoint?> GetAndSaveWeatherAsync()
    {
        var weatherData = await _weatherApiService.GetWeatherAsync();

        if(weatherData?.Current is null)
        {
            return null;
        }

        var dataPoint = MapToWeatherDataPoint(weatherData.Current);

        await using var db = new WeatherDataContext();

        bool exists = await db.WeatherDataPoints
            .AnyAsync(w =>
                w.StationName == dataPoint.StationName &&
                w.Timestamp == dataPoint.Timestamp);

        if(exists)
        {
            return null;
        }

        db.WeatherDataPoints.Add(dataPoint);
        await db.SaveChangesAsync();

        return dataPoint;
    }
}
using FailSafe_WeatherMonitor.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FailSafe_WeatherMonitor.UI;

internal static class WeatherScreen
{
    private static readonly WeatherService _weatherService = new();

    public static async Task ShowAsync()
    {
        try
        {
            var weatherData = await _weatherService.GetAndSaveWeatherAsync();

            if(weatherData is not null)
            {
                AppTheme.ShowNotice(
                    "WEATHER DATA SAVED",
                    $"Temperature: {weatherData.Temperature.ToString("F1", CultureInfo.InvariantCulture)}°C\n" +
                    $"Humidity: {weatherData.Humidity.ToString("F1", CultureInfo.InvariantCulture)}%\n" +
                    $"Time: {weatherData.Timestamp:yyyy-MM-dd HH:mm:ss}",
                    NoticeType.Success);
            }
            else
            {
                AppTheme.ShowNotice(
                    "NO NEW WEATHER DATA",
                    "The measurement already exists or no data was returned.",
                    NoticeType.Info);
            }
        }
        catch(InvalidOperationException)
        {
            AppTheme.ShowNotice(
                "WEATHER SERVICE ERROR",
                "Unable to retrieve weather data. Please check your connection and try again.",
                NoticeType.Error);
        }
        catch(DbUpdateException)
        {
            AppTheme.ShowNotice(
                "DATABASE ERROR",
                "Weather data could not be saved to the database.",
                NoticeType.Error);
        }
        catch(Exception)
        {
            AppTheme.ShowNotice(
                "UNEXPECTED ERROR",
                "An unexpected error occurred while processing weather data.",
                NoticeType.Error);
        }
    }
}
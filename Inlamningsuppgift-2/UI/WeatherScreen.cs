using FailSafe_WeatherMonitor.Services;

namespace FailSafe_WeatherMonitor.UI
{
    internal static class WeatherScreen
    {
        private static readonly WeatherService _weatherService = new();
        public static async Task ShowAsync()
        {
            var weatherData = await _weatherService.GetAndSaveWeatherAsync();
            if(weatherData is not null)
            {
                AppTheme.ShowNotice("WEATHER DATA SAVED", message:
                    $"Temperature: {weatherData.Temperature}°C\n" +
                    $"Humidity: {weatherData.Humidity}%\n" +
                    $"Time: {weatherData.Timestamp}",
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
    }
}

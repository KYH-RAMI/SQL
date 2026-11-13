using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using FailSafe_WeatherMonitor.Models;

namespace FailSafe_WeatherMonitor.Services
{
    internal class WeatherApiService
    {
        private readonly HttpClient _httpClient = new HttpClient();
      
        public async Task<OpenMeteoResponse?> GetWeatherAsync()
        {
            try
            {
                using var response = await _httpClient.GetAsync("https://api.open-meteo.com/v1/forecast?latitude=59.33&longitude=18.06&current=temperature_2m%2Crelative_humidity_2m&timezone=Europe%2FStockholm");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<OpenMeteoResponse>(content);
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException("Failed to fetch weather data.", ex);
            }
        }
        public async Task SaveWeatherAsync(OpenMeteoResponse weatherData)
        {
            // Här ska du själv skriva logiken för att:
            // 1. Kontrollera att Current finns.
            // 2. Skapa ett WeatherDataPoint-objekt.
            // 3. Spara objektet med EF Core.
            if(weatherData?.Current == null)
            {
                throw new InvalidOperationException("Current weather data is missing.");
            }
             
        }
    }
}


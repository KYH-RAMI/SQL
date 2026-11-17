using System.Net.Http.Json;
using System.Text.Json;
using FailSafe_WeatherMonitor.Models;

namespace FailSafe_WeatherMonitor.Services;

internal class WeatherApiService
{
    private static readonly HttpClient _httpClient = new();

    private const string ApiUrl =
        "https://api.open-meteo.com/v1/forecast?latitude=59.33&longitude=18.06&current=temperature_2m%2Crelative_humidity_2m&timezone=Europe%2FStockholm";

    public async Task<OpenMeteoResponse?> GetWeatherAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<OpenMeteoResponse>(ApiUrl);
        }
        catch(HttpRequestException ex)
        {
            throw new InvalidOperationException(
                "Failed to retrieve weather data from the API.", ex);
        }
        catch(JsonException ex)
        {
            throw new InvalidOperationException(
                "The weather API returned invalid JSON data.", ex);
        }
    }
}
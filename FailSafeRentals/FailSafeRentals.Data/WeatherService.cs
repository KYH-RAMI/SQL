using System.Net.Http;
using System.Net.Http.Json;

namespace FailSafeRentals.Data;

public class WeatherService
{
    private readonly HttpClient _httpClient = new();

    public async Task<WeatherResponse> GetWeatherAsync()
    {
        string url = "https://api.open-meteo.com/v1/forecast" +
                     "?latitude=57.7089&longitude=11.9746" +
                     "&current=temperature_2m,wind_speed_10m";

        var weather = await _httpClient.GetFromJsonAsync<WeatherResponse>(url);

        return weather ?? throw new Exception("Kunde inte läsa väderdatan.");
    }
}
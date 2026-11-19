using System.Text.Json.Serialization;

namespace FailSafeRentals.Data;

public class WeatherResponse
{
    [JsonPropertyName("current")]
    public CurrentWeather Current { get; set; } = new();
}

public class CurrentWeather
{
    [JsonPropertyName("temperature_2m")]
    public double Temperature { get; set; }

    [JsonPropertyName("wind_speed_10m")]
    public double WindSpeed { get; set; }
}
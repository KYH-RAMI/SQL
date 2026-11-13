
using System.Text.Json.Serialization;
using FailSafe_WeatherMonitor.Data;
using FailSafe_WeatherMonitor.Services;
namespace FailSafe_WeatherMonitor.Models
{
    internal class CurrentWeather
    {
        [JsonPropertyName("time")]
        public DateTime Time { get; set; }
        [JsonPropertyName("temperature_2m")]
        public double Temperature2m { get; set; }
        [JsonPropertyName("relative_humidity_2m")]
        public double RelativeHumidity2m { get; set; }


    }
}

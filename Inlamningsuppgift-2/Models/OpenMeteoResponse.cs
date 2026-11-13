
using System.Text.Json.Serialization;

namespace FailSafe_WeatherMonitor.Models
{
    internal class OpenMeteoResponse
    {
        [JsonPropertyName("current")]
        public CurrentWeather? Current { get; set; }
  
    }
}


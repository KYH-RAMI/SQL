using FailSafe_WeatherMonitor.Data;
using FailSafe_WeatherMonitor.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FailSafe_WeatherMonitor.Services
{
    internal class WeatherService
    {
        public async Task<OpenMeteoResponse?> GetWeatherAsync()
        {
            WeatherApiService weatherApiService = new WeatherApiService();
            var weatherData = await weatherApiService.GetWeatherAsync();
            return weatherData;
        }
        private WeatherDataPoint MapToWeatherDataPoint(CurrentWeather current)
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
            var weatherData = await GetWeatherAsync();

            if(weatherData?.Current is null)
            {
                return null;
            }

            var dataPoint = MapToWeatherDataPoint(weatherData.Current);

            using var db = new WeatherDataContext();
            bool exists = await db.WeatherDataPoints
                .AnyAsync(w => w.StationName == dataPoint.StationName &&
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
}
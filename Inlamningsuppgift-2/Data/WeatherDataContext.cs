using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using FailSafe_WeatherMonitor.Models;

namespace FailSafe_WeatherMonitor.Data
{
    public class WeatherDataContext : DbContext
    {
        public DbSet<WeatherDataPoint> WeatherDataPoints { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection saknas.");

            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
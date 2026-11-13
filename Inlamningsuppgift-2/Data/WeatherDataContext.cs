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
            // 1. Läs konfigurationen från appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // 2. Hämta anslutningssträngen
            string connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection saknas.");

            // 3. Konfigurera SQL Server
            optionsBuilder.UseSqlServer(connectionString);
        }

    }
}

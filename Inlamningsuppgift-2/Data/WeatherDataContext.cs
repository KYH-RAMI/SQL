using FailSafe_WeatherMonitor.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FailSafe_WeatherMonitor.Data
{
    public class WeatherDataContext : DbContext
    {
        public DbSet<WeatherDataPoint> WeatherDataPoints { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Development är standard och använder LocalDB.
            string environment =
                Environment.GetEnvironmentVariable("WEATHERMONITOR_ENVIRONMENT")
                ?? "Development";

            bool isProduction = environment.Equals(
                "Production",
                StringComparison.OrdinalIgnoreCase);

            bool isDevelopment = environment.Equals(
                "Development",
                StringComparison.OrdinalIgnoreCase);

            if(!isProduction && !isDevelopment)
            {
                throw new InvalidOperationException(
                    $"Okänd miljö: {environment}");
            }

            string configFile = isProduction
                ? "appsettings.Production.json"
                : "appsettings.json";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(configFile, optional: false)
                .Build();

            string connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    $"DefaultConnection saknas i {configFile}");

            if(isProduction)
            {
                string password =
                    Environment.GetEnvironmentVariable("WEATHERMONITOR_SQL_PASSWORD")
                    ?? throw new InvalidOperationException(
                        "Azure SQL-lösenord saknas.");

                if(string.IsNullOrWhiteSpace(password))
                {
                    throw new InvalidOperationException(
                        "Azure SQL-lösenordet får inte vara tomt.");
                }

                var builder = new SqlConnectionStringBuilder(connectionString);

                // Skydda mot anslutning till fel server.
                if(!builder.DataSource.Contains(
                    "failsafe-rami.database.windows.net",
                    StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException(
                        "Production måste använda Azure SQL-servern failsafe-rami.");
                }

                if(!builder.InitialCatalog.Equals(
                    "FailSafeWeatherDB_Rami",
                    StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException(
                        "Fel Azure-databas har valts.");
                }

                builder.Password = password;
                builder.Encrypt = true;
                builder.TrustServerCertificate = false;

                connectionString = builder.ConnectionString;
            }

            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
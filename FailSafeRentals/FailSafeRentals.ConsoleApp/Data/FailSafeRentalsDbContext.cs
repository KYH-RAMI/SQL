using Microsoft.EntityFrameworkCore;

using FailSafeRentals.ConsoleApp.Models;

namespace FailSafeRentals.ConsoleApp.Data;

public class FailSafeRentalsDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Vehicle> Vehicles { get; set; } = null!;
    public DbSet<Rental> Rentals { get; set; } = null!;
    public DbSet<ExtraFeature> ExtraFeatures { get; set; } = null!;
    public DbSet<RentalStation> RentalStations { get; set; }
    public DbSet<FleetVehicle> FleetVehicles { get; set; }
    public DbSet<DamageReport> DamageReports { get; set; }
    public DbSet<MaintenanceTag> MaintenanceTags { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Server=(localdb)\MSSQLLocalDB;Database=FailSafeRentals;Trusted_Connection=True;TrustServerCertificate=True;");
        //Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FailSafeIT;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1. Unikt index på registreringsnummer
        modelBuilder.Entity<Vehicle>()
            .HasIndex(v => v.RegistrationNumber)
            .IsUnique();

        // 2. Explicit precision för pengar i LocalDB
        modelBuilder.Entity<Vehicle>()
            .Property(v => v.DailyRate)
            .HasPrecision(10, 2);

        // 3. Shadow Property för SecOps-spårbarhet
        modelBuilder.Entity<Rental>()
            .Property<DateTime>("CreatedAt")
            .HasDefaultValueSql("GETUTCDATE()");

        // 4. Skydda uthyrningshistorik mot oavsiktlig radering
        modelBuilder.Entity<Rental>()
            .HasOne(r => r.Customer)
            .WithMany(c => c.Rentals)
            .OnDelete(DeleteBehavior.Restrict);

        // Uppgift 7: Unika index & decimalprecisiion (10,2)
        modelBuilder.Entity<RentalStation>()
            .HasIndex(s => s.StationCode)
            .IsUnique();

        modelBuilder.Entity<FleetVehicle>()
            .HasIndex(v => v.RegistrationNumber)
            .IsUnique();

        // Uppgift 8
        modelBuilder.Entity<DamageReport>()
            .Property(d => d.EstimatedCost)
            .HasPrecision(10, 2);

        modelBuilder.Entity<DamageReport>()
            .Property<DateTime>("AuditLoggedAt")
            .HasDefaultValueSql("GETUTCDATE()");

        // Uppgift 9 Förhindra kaskadradering
        modelBuilder.Entity<FleetVehicle>()
            .HasOne(v => v.RentalStation)
            .WithMany(s => s.Vehicles)
            .HasForeignKey(v => v.RentalStationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DamageReport>()
            .HasOne(d => d.FleetVehicle)
            .WithMany(v => v.DamageReports)
            .HasForeignKey(d => d.FleetVehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        // Lägg till lite data:
        modelBuilder.Entity<RentalStation>()
            .HasData(
                new RentalStation { Id = 1, StationCode = "GOT-01", City = "Göteborg", StreetAddress = "Landvetter flygplats 1"},
                new RentalStation { Id = 2, StationCode = "STO-01", City = "Stockholm", StreetAddress = "Arlanda Terminal 5"},
                new RentalStation { Id = 3, StationCode = "MMX-01", City = "Malmö", StreetAddress = "Sturupsvägen 3"}
                //new RentalStation { Id = 4, StationCode = "OSL-01", City = "Oslo", StreetAddress = "Gardermoen 1"}
            );

    }
}

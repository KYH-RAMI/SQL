using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FailSafeRentals.ConsoleApp.Migrations;

/// <inheritdoc />
public partial class _20261116094431_AddFleetIndexesAndPrecision : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "City",
            table: "RentalStations",
            type: "nvarchar(80)",
            maxLength: 80,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(50)",
            oldMaxLength: 50);

        migrationBuilder.AlterColumn<decimal>(
            name: "EstimatedCost",
            table: "DamageReports",
            type: "decimal(10,2)",
            precision: 10,
            scale: 2,
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "decimal(18,2)");

        migrationBuilder.CreateIndex(
            name: "IX_RentalStations_StationCode",
            table: "RentalStations",
            column: "StationCode",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_FleetVehicles_RegistrationNumber",
            table: "FleetVehicles",
            column: "RegistrationNumber",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_RentalStations_StationCode",
            table: "RentalStations");

        migrationBuilder.DropIndex(
            name: "IX_FleetVehicles_RegistrationNumber",
            table: "FleetVehicles");

        migrationBuilder.AlterColumn<string>(
            name: "City",
            table: "RentalStations",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(80)",
            oldMaxLength: 80);

        migrationBuilder.AlterColumn<decimal>(
            name: "EstimatedCost",
            table: "DamageReports",
            type: "decimal(18,2)",
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "decimal(10,2)",
            oldPrecision: 10,
            oldScale: 2);
    }
}

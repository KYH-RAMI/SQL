using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FailSafeRentals.ConsoleApp.Migrations;

/// <inheritdoc />
public partial class _20261116101316_RestrictDeleteOnFleet : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_DamageReports_FleetVehicles_FleetVehicleId",
            table: "DamageReports");

        migrationBuilder.DropForeignKey(
            name: "FK_FleetVehicles_RentalStations_RentalStationId",
            table: "FleetVehicles");

        migrationBuilder.AddColumn<int>(
            name: "DamageReportId",
            table: "FleetVehicles",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddForeignKey(
            name: "FK_DamageReports_FleetVehicles_FleetVehicleId",
            table: "DamageReports",
            column: "FleetVehicleId",
            principalTable: "FleetVehicles",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_FleetVehicles_RentalStations_RentalStationId",
            table: "FleetVehicles",
            column: "RentalStationId",
            principalTable: "RentalStations",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_DamageReports_FleetVehicles_FleetVehicleId",
            table: "DamageReports");

        migrationBuilder.DropForeignKey(
            name: "FK_FleetVehicles_RentalStations_RentalStationId",
            table: "FleetVehicles");

        migrationBuilder.DropColumn(
            name: "DamageReportId",
            table: "FleetVehicles");

        migrationBuilder.AddForeignKey(
            name: "FK_DamageReports_FleetVehicles_FleetVehicleId",
            table: "DamageReports",
            column: "FleetVehicleId",
            principalTable: "FleetVehicles",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_FleetVehicles_RentalStations_RentalStationId",
            table: "FleetVehicles",
            column: "RentalStationId",
            principalTable: "RentalStations",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}

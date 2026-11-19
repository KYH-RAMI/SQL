using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FailSafeRentals.ConsoleApp.Migrations;

/// <inheritdoc />
public partial class _20261116112836_SplitMakeAndModel : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {

        migrationBuilder.AddColumn<string>(
            name: "Make",
            table: "FleetVehicles",
            type: "nvarchar(40)",
            maxLength: 40,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "Model",
            table: "FleetVehicles",
            type: "nvarchar(40)",
            maxLength: 40,
            nullable: false,
            defaultValue: "");

        // Rädda befintliga data innan kolumnen tas bort!
        migrationBuilder.Sql("UPDATE FleetVehicles SET Make = MakeAndModel;");

        migrationBuilder.DropColumn(
            name: "MakeAndModel",
            table: "FleetVehicles");

    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Make",
            table: "FleetVehicles");

        migrationBuilder.DropColumn(
            name: "Model",
            table: "FleetVehicles");

        migrationBuilder.AddColumn<string>(
            name: "MakeAndModel",
            table: "FleetVehicles",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: false,
            defaultValue: "");
    }
}

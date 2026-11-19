using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FailSafeRentals.ConsoleApp.Migrations;

/// <inheritdoc />
public partial class _20261115112447_InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Customers",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                DriverLicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Customers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ExtraFeatures",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                DailyPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ExtraFeatures", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Vehicles",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                RegistrationNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                Brand = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                DailyRate = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                IsAvailable = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Vehicles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ExtraFeatureVehicle",
            columns: table => new
            {
                ExtraFeaturesId = table.Column<int>(type: "int", nullable: false),
                VehiclesId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ExtraFeatureVehicle", x => new { x.ExtraFeaturesId, x.VehiclesId });
                table.ForeignKey(
                    name: "FK_ExtraFeatureVehicle_ExtraFeatures_ExtraFeaturesId",
                    column: x => x.ExtraFeaturesId,
                    principalTable: "ExtraFeatures",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ExtraFeatureVehicle_Vehicles_VehiclesId",
                    column: x => x.VehiclesId,
                    principalTable: "Vehicles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Rentals",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                CustomerId = table.Column<int>(type: "int", nullable: false),
                VehicleId = table.Column<int>(type: "int", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Rentals", x => x.Id);
                table.ForeignKey(
                    name: "FK_Rentals_Customers_CustomerId",
                    column: x => x.CustomerId,
                    principalTable: "Customers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Rentals_Vehicles_VehicleId",
                    column: x => x.VehicleId,
                    principalTable: "Vehicles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ExtraFeatureVehicle_VehiclesId",
            table: "ExtraFeatureVehicle",
            column: "VehiclesId");

        migrationBuilder.CreateIndex(
            name: "IX_Rentals_CustomerId",
            table: "Rentals",
            column: "CustomerId");

        migrationBuilder.CreateIndex(
            name: "IX_Rentals_VehicleId",
            table: "Rentals",
            column: "VehicleId");

        migrationBuilder.CreateIndex(
            name: "IX_Vehicles_RegistrationNumber",
            table: "Vehicles",
            column: "RegistrationNumber",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ExtraFeatureVehicle");

        migrationBuilder.DropTable(
            name: "Rentals");

        migrationBuilder.DropTable(
            name: "ExtraFeatures");

        migrationBuilder.DropTable(
            name: "Customers");

        migrationBuilder.DropTable(
            name: "Vehicles");
    }
}

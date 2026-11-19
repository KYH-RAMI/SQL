using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FailSafeRentals.ConsoleApp.Migrations;

/// <inheritdoc />
public partial class _20261116094948_AddDamageAuditShadowProperty : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "AuditLoggedAt",
            table: "DamageReports",
            type: "datetime2",
            nullable: false,
            defaultValueSql: "GETUTCDATE()");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "AuditLoggedAt",
            table: "DamageReports");
    }
}

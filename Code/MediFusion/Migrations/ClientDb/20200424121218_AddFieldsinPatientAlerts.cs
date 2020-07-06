using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddFieldsinPatientAlerts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "resolveComments",
                table: "PatientAlerts",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "resolved",
                table: "PatientAlerts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "resolvedBy",
                table: "PatientAlerts",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "resolvedDate",
                table: "PatientAlerts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "resolveComments",
                table: "PatientAlerts");

            migrationBuilder.DropColumn(
                name: "resolved",
                table: "PatientAlerts");

            migrationBuilder.DropColumn(
                name: "resolvedBy",
                table: "PatientAlerts");

            migrationBuilder.DropColumn(
                name: "resolvedDate",
                table: "PatientAlerts");
        }
    }
}

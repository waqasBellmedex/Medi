using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class ChangesInProviderSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.AddColumn<DateTime>(
                name: "breakfrom",
                table: "ProviderSchedule",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "breakto",
                table: "ProviderSchedule",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "day",
                table: "ProviderSchedule",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "breakfrom",
                table: "ProviderSchedule");

            migrationBuilder.DropColumn(
                name: "breakto",
                table: "ProviderSchedule");

            migrationBuilder.DropColumn(
                name: "day",
                table: "ProviderSchedule");

        }
    }
}

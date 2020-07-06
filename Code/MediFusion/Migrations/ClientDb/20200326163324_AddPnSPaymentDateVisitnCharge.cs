using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddPnSPaymentDateVisitnCharge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PrimaryPaymentDate",
                table: "Visit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SecondaryPaymentDate",
                table: "Visit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "OnlinePortals",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PrimaryPaymentDate",
                table: "Charge",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SecondaryPaymentDate",
                table: "Charge",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimaryPaymentDate",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "SecondaryPaymentDate",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "OnlinePortals");

            migrationBuilder.DropColumn(
                name: "PrimaryPaymentDate",
                table: "Charge");

            migrationBuilder.DropColumn(
                name: "SecondaryPaymentDate",
                table: "Charge");
        }
    }
}

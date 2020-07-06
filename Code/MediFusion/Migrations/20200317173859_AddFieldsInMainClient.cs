using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations
{
    public partial class AddFieldsInMainClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeactivateionAdditionalInfo",
                table: "MainClient",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeactivationDate",
                table: "MainClient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeactivationReason",
                table: "MainClient",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeactivateionAdditionalInfo",
                table: "MainClient");

            migrationBuilder.DropColumn(
                name: "DeactivationDate",
                table: "MainClient");

            migrationBuilder.DropColumn(
                name: "DeactivationReason",
                table: "MainClient");
        }
    }
}

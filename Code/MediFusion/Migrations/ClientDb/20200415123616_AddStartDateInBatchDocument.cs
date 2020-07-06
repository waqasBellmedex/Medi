using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddStartDateInBatchDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RererralForService",
                table: "PatientReferral",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "BatchDocument",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "BatchDocument",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RererralForService",
                table: "PatientReferral");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "BatchDocument");
        }
    }
}

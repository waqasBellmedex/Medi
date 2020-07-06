using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddNewFieldsInPractice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CellNumber",
                table: "Practice",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientCategory",
                table: "Practice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPersonName",
                table: "Practice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EHRSoftwareName",
                table: "Practice",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FTEPerDayRate",
                table: "Practice",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FTEPerMonthRate",
                table: "Practice",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FTEPerWeekRate",
                table: "Practice",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IncludePatientCollection",
                table: "Practice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoicePercentage",
                table: "Practice",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumMonthlyAmount",
                table: "Practice",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfFullTimeEmployees",
                table: "Practice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PMSoftwareName",
                table: "Practice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefferedBy",
                table: "Practice",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CellNumber",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "ClientCategory",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "ContactPersonName",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "EHRSoftwareName",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "FTEPerDayRate",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "FTEPerMonthRate",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "FTEPerWeekRate",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "IncludePatientCollection",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "InvoicePercentage",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "MinimumMonthlyAmount",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "NumberOfFullTimeEmployees",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "PMSoftwareName",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "RefferedBy",
                table: "Practice");
        }
    }
}

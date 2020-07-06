using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations
{
    public partial class AddMoreFieldsInMainPractice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CellNumber",
                table: "MainPractice",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientCategory",
                table: "MainPractice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPersonName",
                table: "MainPractice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EHRSoftwareName",
                table: "MainPractice",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FTEPerDayRate",
                table: "MainPractice",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FTEPerMonthRate",
                table: "MainPractice",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FTEPerWeekRate",
                table: "MainPractice",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IncludePatientCollection",
                table: "MainPractice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoicePercentage",
                table: "MainPractice",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumMonthlyAmount",
                table: "MainPractice",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfFullTimeEmployees",
                table: "MainPractice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PMSoftwareName",
                table: "MainPractice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefferedBy",
                table: "MainPractice",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CellNumber",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "ClientCategory",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "ContactPersonName",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "EHRSoftwareName",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "FTEPerDayRate",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "FTEPerMonthRate",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "FTEPerWeekRate",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "IncludePatientCollection",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "InvoicePercentage",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "MinimumMonthlyAmount",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "NumberOfFullTimeEmployees",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "PMSoftwareName",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "RefferedBy",
                table: "MainPractice");
        }
    }
}

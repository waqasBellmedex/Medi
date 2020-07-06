using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddPrescribingMDnExternalCharge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrescribingMD",
                table: "Visit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiagnosisCode",
                table: "ExternalCharge",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NeedDemos",
                table: "ExternalCharge",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NotBilled",
                table: "ExternalCharge",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrescribingMD",
                table: "ExternalCharge",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "ExternalCharge",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportType",
                table: "ExternalCharge",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SheetName",
                table: "ExternalCharge",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrescribingMD",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "DiagnosisCode",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "NeedDemos",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "NotBilled",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "PrescribingMD",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "ReportType",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "SheetName",
                table: "ExternalCharge");
        }
    }
}

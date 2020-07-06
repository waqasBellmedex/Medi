using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddFieldsInExternalPatient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NeedInsuranceCard",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrescribingMD",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryDescription",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryGroup",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryDescription",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryGroup",
                table: "ExternalPatient",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NeedInsuranceCard",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "PrescribingMD",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "PrimaryDescription",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "PrimaryGroup",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "SecondaryDescription",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "SecondaryGroup",
                table: "ExternalPatient");
        }
    }
}

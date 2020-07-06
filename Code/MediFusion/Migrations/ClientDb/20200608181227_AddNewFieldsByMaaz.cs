using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddNewFieldsByMaaz : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResolvedErrorMessage",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResolvedErrorMessage",
                table: "ExternalCharge",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResolvedErrorMessage",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "ResolvedErrorMessage",
                table: "ExternalCharge");
        }
    }
}

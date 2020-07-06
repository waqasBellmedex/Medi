using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddMoreFieldsInExternalPatient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Created",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modified",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "ExternalPatient",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "ExternalPatient");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddFieldsInExternalChargeInModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "ExternalCharge",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "ExternalCharge",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "ExternalCharge");
        }
    }
}

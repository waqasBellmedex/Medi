using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddNewFieldsInExternalCharge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PrimaryInsuredID",
                table: "ExternalCharge",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SecondaryInsuredID",
                table: "ExternalCharge",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimaryInsuredID",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "SecondaryInsuredID",
                table: "ExternalCharge");
        }
    }
}

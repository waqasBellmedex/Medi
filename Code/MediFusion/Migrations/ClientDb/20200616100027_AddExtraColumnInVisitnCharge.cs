using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddExtraColumnInVisitnCharge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExtraColumn",
                table: "Visit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraColumn",
                table: "Charge",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtraColumn",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "ExtraColumn",
                table: "Charge");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddHoldStatementinPatient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HoldStatement",
                table: "Visit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HoldStatement",
                table: "Patient",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoldStatement",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "HoldStatement",
                table: "Patient");
        }
    }
}

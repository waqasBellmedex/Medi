using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddColumnInsuranceNameinExternalCharge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InsuranceName",
                table: "ExternalCharge",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsuranceName",
                table: "ExternalCharge");
        }
    }
}

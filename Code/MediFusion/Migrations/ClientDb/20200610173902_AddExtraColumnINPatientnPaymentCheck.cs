using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddExtraColumnINPatientnPaymentCheck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExtraColumn",
                table: "PaymentCheck",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraColumn",
                table: "Patient",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtraColumn",
                table: "PaymentCheck");

            migrationBuilder.DropColumn(
                name: "ExtraColumn",
                table: "Patient");
        }
    }
}

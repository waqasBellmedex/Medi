using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class PostedByInPaymentCharge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostedBy",
                table: "PaymentVisit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostedBy",
                table: "PaymentCharge",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostedBy",
                table: "PaymentVisit");

            migrationBuilder.DropColumn(
                name: "PostedBy",
                table: "PaymentCharge");
        }
    }
}

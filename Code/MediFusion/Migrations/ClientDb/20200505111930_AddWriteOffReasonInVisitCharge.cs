using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddWriteOffReasonInVisitCharge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WriteOffReason",
                table: "Visit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WriteOffReason",
                table: "Charge",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WriteOffReason",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "WriteOffReason",
                table: "Charge");
        }
    }
}

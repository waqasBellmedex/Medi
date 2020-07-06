using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddPatientPaymentColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "isGoogleCalenderEnable",
                table: "Practice",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AddColumn<string>(
                name: "AdvanceAppliedOnVisits",
                table: "PatientPayment",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AdvancePatientPaymentID",
                table: "PatientPayment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdvanceAppliedOnVisits",
                table: "PatientPayment");

            migrationBuilder.DropColumn(
                name: "AdvancePatientPaymentID",
                table: "PatientPayment");

            migrationBuilder.AlterColumn<bool>(
                name: "isGoogleCalenderEnable",
                table: "Practice",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}

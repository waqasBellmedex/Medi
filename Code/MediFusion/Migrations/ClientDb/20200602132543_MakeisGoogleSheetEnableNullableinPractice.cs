using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class MakeisGoogleSheetEnableNullableinPractice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "isGoogleSheetEnable",
                table: "Practice",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "IsSMSAppointmentReminder",
                table: "Practice",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "IsEmailAppointmentReminder",
                table: "Practice",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AddColumn<bool>(
                name: "priorAuthorization",
                table: "PatientAppointment",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "recurringAppointment",
                table: "PatientAppointment",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "recurringNumber",
                table: "PatientAppointment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "recurringfrequency",
                table: "PatientAppointment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "priorAuthorization",
                table: "PatientAppointment");

            migrationBuilder.DropColumn(
                name: "recurringAppointment",
                table: "PatientAppointment");

            migrationBuilder.DropColumn(
                name: "recurringNumber",
                table: "PatientAppointment");

            migrationBuilder.DropColumn(
                name: "recurringfrequency",
                table: "PatientAppointment");

            migrationBuilder.AlterColumn<bool>(
                name: "isGoogleSheetEnable",
                table: "Practice",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsSMSAppointmentReminder",
                table: "Practice",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsEmailAppointmentReminder",
                table: "Practice",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}

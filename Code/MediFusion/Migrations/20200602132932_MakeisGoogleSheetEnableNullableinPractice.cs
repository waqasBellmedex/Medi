using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations
{
    public partial class MakeisGoogleSheetEnableNullableinPractice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "isGoogleSheetEnable",
                table: "MainPractice",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "IsSMSAppointmentReminder",
                table: "MainPractice",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "IsEmailAppointmentReminder",
                table: "MainPractice",
                nullable: true,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "isGoogleSheetEnable",
                table: "MainPractice",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsSMSAppointmentReminder",
                table: "MainPractice",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsEmailAppointmentReminder",
                table: "MainPractice",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddIsEmailAppointmentReminderinPractice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEmailAppointmentReminder",
                table: "Practice",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSMSAppointmentReminder",
                table: "Practice",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmailAppointmentReminder",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "IsSMSAppointmentReminder",
                table: "Practice");
        }
    }
}

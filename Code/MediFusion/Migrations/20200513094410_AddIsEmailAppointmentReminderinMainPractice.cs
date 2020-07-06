using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations
{
    public partial class AddIsEmailAppointmentReminderinMainPractice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEmailAppointmentReminder",
                table: "MainPractice",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSMSAppointmentReminder",
                table: "MainPractice",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmailAppointmentReminder",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "IsSMSAppointmentReminder",
                table: "MainPractice");
        }
    }
}

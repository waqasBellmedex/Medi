using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddAppointmentPhoneInPractice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppointmentPhoneNumber",
                table: "Practice",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppointmentPhoneNumberExt",
                table: "Practice",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatementFaxNumber",
                table: "Practice",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatementPhoneNumber",
                table: "Practice",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatementPhoneNumberExt",
                table: "Practice",
                maxLength: 4,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentPhoneNumber",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "AppointmentPhoneNumberExt",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "StatementFaxNumber",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "StatementPhoneNumber",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "StatementPhoneNumberExt",
                table: "Practice");
        }
    }
}

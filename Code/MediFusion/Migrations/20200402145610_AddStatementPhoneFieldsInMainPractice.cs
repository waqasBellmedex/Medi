using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations
{
    public partial class AddStatementPhoneFieldsInMainPractice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppointmentPhoneNumber",
                table: "MainPractice",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppointmentPhoneNumberExt",
                table: "MainPractice",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatementFaxNumber",
                table: "MainPractice",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatementPhoneNumber",
                table: "MainPractice",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatementPhoneNumberExt",
                table: "MainPractice",
                maxLength: 4,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentPhoneNumber",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "AppointmentPhoneNumberExt",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "StatementFaxNumber",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "StatementPhoneNumber",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "StatementPhoneNumberExt",
                table: "MainPractice");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations
{
    public partial class AddColumnInMainPractice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "googleCalenderSecret",
                table: "MainPractice",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isGoogleCalenderEnable",
                table: "MainPractice",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "googleCalenderSecret",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "isGoogleCalenderEnable",
                table: "MainPractice");
        }
    }
}

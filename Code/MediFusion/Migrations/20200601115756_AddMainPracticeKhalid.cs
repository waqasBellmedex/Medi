using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations
{
    public partial class AddMainPracticeKhalid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CalenderID",
                table: "MainPractice",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GoogleSheetRows",
                table: "MainPractice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "googleSheetID",
                table: "MainPractice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "googleSheetSecret",
                table: "MainPractice",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isGoogleSheetEnable",
                table: "MainPractice",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CalenderID",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "GoogleSheetRows",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "googleSheetID",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "googleSheetSecret",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "isGoogleSheetEnable",
                table: "MainPractice");
        }
    }
}

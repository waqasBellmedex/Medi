using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations
{
    public partial class AddIsAutoSubmissionINMainPractice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAutoDownloading",
                table: "MainPractice",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAutoSubmission",
                table: "MainPractice",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAutoDownloading",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "IsAutoSubmission",
                table: "MainPractice");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations
{
    public partial class AddFieldsInMainPractice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StatementAgingDays",
                table: "MainPractice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatementExportType",
                table: "MainPractice",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StatementMaxCount",
                table: "MainPractice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatementMessage",
                table: "MainPractice",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatementAgingDays",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "StatementExportType",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "StatementMaxCount",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "StatementMessage",
                table: "MainPractice");
        }
    }
}

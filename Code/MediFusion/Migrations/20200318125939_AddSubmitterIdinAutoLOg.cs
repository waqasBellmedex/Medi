using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations
{
    public partial class AddSubmitterIdinAutoLOg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Exception",
                table: "AutoDownloadingLog",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogMessage",
                table: "AutoDownloadingLog",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SubmitterID",
                table: "AutoDownloadingLog",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Exception",
                table: "AutoDownloadingLog");

            migrationBuilder.DropColumn(
                name: "LogMessage",
                table: "AutoDownloadingLog");

            migrationBuilder.DropColumn(
                name: "SubmitterID",
                table: "AutoDownloadingLog");
        }
    }
}

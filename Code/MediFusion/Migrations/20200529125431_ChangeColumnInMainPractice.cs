using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations
{
    public partial class ChangeColumnInMainPractice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "isGoogleCalenderEnable",
                table: "MainPractice",
                nullable: true,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "isGoogleCalenderEnable",
                table: "MainPractice",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}

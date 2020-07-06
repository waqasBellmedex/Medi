using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations
{
    public partial class MakeNUllableNumberOfFullTimeEmployees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "NumberOfFullTimeEmployees",
                table: "MainPractice",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "NumberOfFullTimeEmployees",
                table: "MainPractice",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}

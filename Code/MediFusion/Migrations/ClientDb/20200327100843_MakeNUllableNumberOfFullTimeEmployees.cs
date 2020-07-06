using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class MakeNUllableNumberOfFullTimeEmployees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "NumberOfFullTimeEmployees",
                table: "Practice",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "NumberOfFullTimeEmployees",
                table: "Practice",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}

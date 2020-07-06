using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class MakeInActiveNUllAbleInProviderSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "InActive",
                table: "ProviderSchedule",
                nullable: true,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "InActive",
                table: "ProviderSchedule",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class ChangeAgainExChargeMissingInfoWithNewColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ICDID",
                table: "ExChargeMissingInfo",
                nullable: true,
                oldClrType: typeof(long));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ICDID",
                table: "ExChargeMissingInfo",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}

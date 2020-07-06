using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class ChangeExChargeMissingInfoColumn2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "PrimaryInsuredID",
                table: "ExChargeMissingInfo",
                nullable: true,
                oldClrType: typeof(long));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "PrimaryInsuredID",
                table: "ExChargeMissingInfo",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);
        }


    }
}

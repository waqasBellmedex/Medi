using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddAsifSabChanges05202020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "reportHeader",
                table: "PatientForms",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "GeneralItems",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<string>(
                name: "appFunction",
                table: "FormsSubHeading",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "customID",
                table: "FormsSubHeading",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "reportHeader",
                table: "PatientForms");

            migrationBuilder.DropColumn(
                name: "appFunction",
                table: "FormsSubHeading");

            migrationBuilder.DropColumn(
                name: "customID",
                table: "FormsSubHeading");

            migrationBuilder.AlterColumn<long>(
                name: "Value",
                table: "GeneralItems",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

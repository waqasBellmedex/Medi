using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddErrorMessageColumnInExternalCharge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalCharge_Cpt_CPTID",
                table: "ExternalCharge");

            migrationBuilder.AlterColumn<long>(
                name: "CPTID",
                table: "ExternalCharge",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "ExternalCharge",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalCharge_Cpt_CPTID",
                table: "ExternalCharge",
                column: "CPTID",
                principalTable: "Cpt",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalCharge_Cpt_CPTID",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "ExternalCharge");

            migrationBuilder.AlterColumn<long>(
                name: "CPTID",
                table: "ExternalCharge",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalCharge_Cpt_CPTID",
                table: "ExternalCharge",
                column: "CPTID",
                principalTable: "Cpt",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

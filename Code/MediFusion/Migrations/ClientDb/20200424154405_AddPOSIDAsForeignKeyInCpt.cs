using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddPOSIDAsForeignKeyInCpt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "POSID",
                table: "Cpt",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cpt_POSID",
                table: "Cpt",
                column: "POSID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cpt_POS_POSID",
                table: "Cpt",
                column: "POSID",
                principalTable: "POS",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cpt_POS_POSID",
                table: "Cpt");

            migrationBuilder.DropIndex(
                name: "IX_Cpt_POSID",
                table: "Cpt");

            migrationBuilder.DropColumn(
                name: "POSID",
                table: "Cpt");
        }
    }
}

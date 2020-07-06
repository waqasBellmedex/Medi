using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddForeignKeysInNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BatchDocumentNoID",
                table: "Notes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_BatchDocumentNoID",
                table: "Notes",
                column: "BatchDocumentNoID");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_BatchDocument_BatchDocumentNoID",
                table: "Notes",
                column: "BatchDocumentNoID",
                principalTable: "BatchDocument",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_BatchDocument_BatchDocumentNoID",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_BatchDocumentNoID",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "BatchDocumentNoID",
                table: "Notes");
        }
    }
}

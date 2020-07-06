using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddVisitIdInNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "VisitID",
                table: "Notes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_VisitID",
                table: "Notes",
                column: "VisitID");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Visit_VisitID",
                table: "Notes",
                column: "VisitID",
                principalTable: "Visit",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Visit_VisitID",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_VisitID",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "VisitID",
                table: "Notes");
        }
    }
}

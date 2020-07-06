using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddpatientIDInNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PatientID",
                table: "Notes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_PatientID",
                table: "Notes",
                column: "PatientID");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Patient_PatientID",
                table: "Notes",
                column: "PatientID",
                principalTable: "Patient",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Patient_PatientID",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_PatientID",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "PatientID",
                table: "Notes");
        }
    }
}

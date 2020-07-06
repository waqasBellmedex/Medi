using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddNotesCollectionInPatientFollowUp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PatientFollowUpID",
                table: "Notes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_PatientFollowUpID",
                table: "Notes",
                column: "PatientFollowUpID");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_PatientFollowUp_PatientFollowUpID",
                table: "Notes",
                column: "PatientFollowUpID",
                principalTable: "PatientFollowUp",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_PatientFollowUp_PatientFollowUpID",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_PatientFollowUpID",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "PatientFollowUpID",
                table: "Notes");
        }
    }
}

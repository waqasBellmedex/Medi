using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class RemovePatAuthInPatPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientPlan_PatientAuthorization_PatientAuthorizationID1",
                table: "PatientPlan");

            migrationBuilder.DropIndex(
                name: "IX_PatientPlan_PatientAuthorizationID1",
                table: "PatientPlan");

            migrationBuilder.DropColumn(
                name: "PatientAuthorizationID",
                table: "PatientPlan");

            migrationBuilder.DropColumn(
                name: "PatientAuthorizationID1",
                table: "PatientPlan");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PatientAuthorizationID",
                table: "PatientPlan",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PatientAuthorizationID1",
                table: "PatientPlan",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientPlan_PatientAuthorizationID1",
                table: "PatientPlan",
                column: "PatientAuthorizationID1");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientPlan_PatientAuthorization_PatientAuthorizationID1",
                table: "PatientPlan",
                column: "PatientAuthorizationID1",
                principalTable: "PatientAuthorization",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

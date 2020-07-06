using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class RemoveForeignPatientReferral : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientReferral_PatientPlan_PatientPlanID",
                table: "PatientReferral");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientReferral_Provider_ProviderID",
                table: "PatientReferral");

            migrationBuilder.DropIndex(
                name: "IX_PatientReferral_PatientPlanID",
                table: "PatientReferral");

            migrationBuilder.DropIndex(
                name: "IX_PatientReferral_ProviderID",
                table: "PatientReferral");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PatientReferral_PatientPlanID",
                table: "PatientReferral",
                column: "PatientPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientReferral_ProviderID",
                table: "PatientReferral",
                column: "ProviderID");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientReferral_PatientPlan_PatientPlanID",
                table: "PatientReferral",
                column: "PatientPlanID",
                principalTable: "PatientPlan",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientReferral_Provider_ProviderID",
                table: "PatientReferral",
                column: "ProviderID",
                principalTable: "Provider",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

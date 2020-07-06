using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddPatientRefferal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientReferral",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientID = table.Column<long>(nullable: true),
                    PCPID = table.Column<long>(nullable: true),
                    ProviderID = table.Column<long>(nullable: true),
                    PatientPlanID = table.Column<long>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    VisitAllowed = table.Column<int>(nullable: true),
                    VisitUsed = table.Column<int>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    ReferralNo = table.Column<string>(nullable: true),
                    FaxStatus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientReferral", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientReferral_Patient_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patient",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientReferral_PatientPlan_PatientPlanID",
                        column: x => x.PatientPlanID,
                        principalTable: "PatientPlan",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientReferral_Provider_ProviderID",
                        column: x => x.ProviderID,
                        principalTable: "Provider",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientReferral_PatientID",
                table: "PatientReferral",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientReferral_PatientPlanID",
                table: "PatientReferral",
                column: "PatientPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientReferral_ProviderID",
                table: "PatientReferral",
                column: "ProviderID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientReferral");
        }
    }
}

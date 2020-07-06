using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddPatientAuthorizationUsed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientAuthorizationUsed",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientAuthID = table.Column<long>(nullable: false),
                    PatientAuthorizationID = table.Column<long>(nullable: true),
                    VisitID = table.Column<long>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAuthorizationUsed", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientAuthorizationUsed_PatientAuthorization_PatientAuthorizationID",
                        column: x => x.PatientAuthorizationID,
                        principalTable: "PatientAuthorization",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientAuthorizationUsed_Visit_VisitID",
                        column: x => x.VisitID,
                        principalTable: "Visit",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientAuthorizationUsed_PatientAuthorizationID",
                table: "PatientAuthorizationUsed",
                column: "PatientAuthorizationID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAuthorizationUsed_VisitID",
                table: "PatientAuthorizationUsed",
                column: "VisitID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientAuthorizationUsed");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class ExternalPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalPayment",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExternalChargeID = table.Column<string>(nullable: true),
                    ExternalPatientID = table.Column<string>(nullable: true),
                    PracticeID = table.Column<long>(nullable: false),
                    LocationID = table.Column<long>(nullable: false),
                    POSCode = table.Column<string>(nullable: true),
                    POSID = table.Column<long>(nullable: true),
                    ProviderName = table.Column<string>(nullable: true),
                    ProviderID = table.Column<long>(nullable: false),
                    CPTID = table.Column<long>(nullable: true),
                    CptCode = table.Column<string>(nullable: true),
                    DaysOrUnits = table.Column<string>(nullable: true),
                    DateOfService = table.Column<DateTime>(type: "Date", nullable: false),
                    Charges = table.Column<decimal>(nullable: false),
                    Balance = table.Column<decimal>(nullable: false),
                    Adj = table.Column<decimal>(nullable: false),
                    InsurancePayment = table.Column<decimal>(nullable: false),
                    PatientPayment = table.Column<decimal>(nullable: false),
                    SubmittetdDate = table.Column<DateTime>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    PatientName = table.Column<string>(nullable: true),
                    MergeStatus = table.Column<string>(nullable: true),
                    InsuranceName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalPayment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExternalPayment_Cpt_CPTID",
                        column: x => x.CPTID,
                        principalTable: "Cpt",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalPayment_Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ExternalPayment_Practice_PracticeID",
                        column: x => x.PracticeID,
                        principalTable: "Practice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ExternalPayment_Provider_ProviderID",
                        column: x => x.ProviderID,
                        principalTable: "Provider",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalPayment_CPTID",
                table: "ExternalPayment",
                column: "CPTID");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalPayment_LocationID",
                table: "ExternalPayment",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalPayment_PracticeID",
                table: "ExternalPayment",
                column: "PracticeID");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalPayment_ProviderID",
                table: "ExternalPayment",
                column: "ProviderID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalPayment");
        }
    }
}

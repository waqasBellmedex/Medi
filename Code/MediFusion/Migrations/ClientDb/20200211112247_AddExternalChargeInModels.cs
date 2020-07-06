using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddExternalChargeInModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalCharge",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExternalPatientID = table.Column<string>(nullable: true),
                    VisitID = table.Column<long>(nullable: true),
                    DOB = table.Column<string>(nullable: true),
                    Insurance = table.Column<string>(nullable: true),
                    PracticeID = table.Column<long>(nullable: false),
                    LocationID = table.Column<long>(nullable: false),
                    POSCode = table.Column<string>(nullable: true),
                    Provider = table.Column<string>(nullable: true),
                    Office = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    CPTCode = table.Column<string>(nullable: true),
                    Modifier1ID = table.Column<long>(nullable: true),
                    Modifier1Amount = table.Column<decimal>(nullable: true),
                    Modifier2ID = table.Column<long>(nullable: true),
                    Modifier2Amount = table.Column<decimal>(nullable: true),
                    Modifier3ID = table.Column<long>(nullable: true),
                    Modifier3Amount = table.Column<decimal>(nullable: true),
                    Modifier4ID = table.Column<long>(nullable: true),
                    Modifier4Amount = table.Column<decimal>(nullable: true),
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
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalCharge", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExternalCharge_Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExternalCharge_Modifier_Modifier1ID",
                        column: x => x.Modifier1ID,
                        principalTable: "Modifier",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalCharge_Modifier_Modifier2ID",
                        column: x => x.Modifier2ID,
                        principalTable: "Modifier",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalCharge_Modifier_Modifier3ID",
                        column: x => x.Modifier3ID,
                        principalTable: "Modifier",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalCharge_Modifier_Modifier4ID",
                        column: x => x.Modifier4ID,
                        principalTable: "Modifier",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalCharge_Practice_PracticeID",
                        column: x => x.PracticeID,
                        principalTable: "Practice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCharge_LocationID",
                table: "ExternalCharge",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCharge_Modifier1ID",
                table: "ExternalCharge",
                column: "Modifier1ID");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCharge_Modifier2ID",
                table: "ExternalCharge",
                column: "Modifier2ID");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCharge_Modifier3ID",
                table: "ExternalCharge",
                column: "Modifier3ID");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCharge_Modifier4ID",
                table: "ExternalCharge",
                column: "Modifier4ID");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCharge_PracticeID",
                table: "ExternalCharge",
                column: "PracticeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalCharge");
        }
    }
}

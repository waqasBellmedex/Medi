using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddInsuranceBillingoptionModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InsuranceBillingoption",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProviderID = table.Column<long>(nullable: false),
                    LocationID = table.Column<long>(nullable: false),
                    InsurancePlanID = table.Column<long>(nullable: false),
                    ReportTaxID = table.Column<bool>(nullable: true),
                    PayToAddress = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceBillingoption", x => x.ID);
                    table.ForeignKey(
                        name: "FK_InsuranceBillingoption_InsurancePlan_InsurancePlanID",
                        column: x => x.InsurancePlanID,
                        principalTable: "InsurancePlan",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InsuranceBillingoption_Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InsuranceBillingoption_Provider_ProviderID",
                        column: x => x.ProviderID,
                        principalTable: "Provider",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceBillingoption_InsurancePlanID",
                table: "InsuranceBillingoption",
                column: "InsurancePlanID");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceBillingoption_LocationID",
                table: "InsuranceBillingoption",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceBillingoption_ProviderID",
                table: "InsuranceBillingoption",
                column: "ProviderID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InsuranceBillingoption");
        }
    }
}

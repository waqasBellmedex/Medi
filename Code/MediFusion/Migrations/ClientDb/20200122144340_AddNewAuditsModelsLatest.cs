using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddNewAuditsModelsLatest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientEligibilityAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientEligibilityID = table.Column<long>(nullable: false),
                    TransactionID = table.Column<long>(nullable: false),
                    ColumnName = table.Column<string>(nullable: true),
                    CurrentValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    CurrentValueID = table.Column<string>(nullable: true),
                    NewValueID = table.Column<string>(nullable: true),
                    HostName = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientEligibilityAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientEligibilityAudit_PatientEligibility_PatientEligibilityID",
                        column: x => x.PatientEligibilityID,
                        principalTable: "PatientEligibility",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientEligibilityDetailAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientEligibilityDetailID = table.Column<long>(nullable: false),
                    TransactionID = table.Column<long>(nullable: false),
                    ColumnName = table.Column<string>(nullable: true),
                    CurrentValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    CurrentValueID = table.Column<string>(nullable: true),
                    NewValueID = table.Column<string>(nullable: true),
                    HostName = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientEligibilityDetailAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientEligibilityDetailAudit_PatientEligibilityDetail_PatientEligibilityDetailID",
                        column: x => x.PatientEligibilityDetailID,
                        principalTable: "PatientEligibilityDetail",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientEligibilityLogAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientEligibilityLogID = table.Column<long>(nullable: false),
                    TransactionID = table.Column<long>(nullable: false),
                    ColumnName = table.Column<string>(nullable: true),
                    CurrentValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    CurrentValueID = table.Column<string>(nullable: true),
                    NewValueID = table.Column<string>(nullable: true),
                    HostName = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientEligibilityLogAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientEligibilityLogAudit_PatientEligibilityLog_PatientEligibilityLogID",
                        column: x => x.PatientEligibilityLogID,
                        principalTable: "PatientEligibilityLog",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientFollowUpChargeAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientFollowUpChargeID = table.Column<long>(nullable: false),
                    TransactionID = table.Column<long>(nullable: false),
                    ColumnName = table.Column<string>(nullable: true),
                    CurrentValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    CurrentValueID = table.Column<string>(nullable: true),
                    NewValueID = table.Column<string>(nullable: true),
                    HostName = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientFollowUpChargeAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientFollowUpChargeAudit_PatientFollowUpCharge_PatientFollowUpChargeID",
                        column: x => x.PatientFollowUpChargeID,
                        principalTable: "PatientFollowUpCharge",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanFollowupChargeAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PlanFollowupChargeID = table.Column<long>(nullable: false),
                    TransactionID = table.Column<long>(nullable: false),
                    ColumnName = table.Column<string>(nullable: true),
                    CurrentValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    CurrentValueID = table.Column<string>(nullable: true),
                    NewValueID = table.Column<string>(nullable: true),
                    HostName = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanFollowupChargeAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PlanFollowupChargeAudit_PlanFollowupCharge_PlanFollowupChargeID",
                        column: x => x.PlanFollowupChargeID,
                        principalTable: "PlanFollowupCharge",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProviderSlotAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProviderSlotID = table.Column<long>(nullable: false),
                    TransactionID = table.Column<long>(nullable: false),
                    ColumnName = table.Column<string>(nullable: true),
                    CurrentValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    CurrentValueID = table.Column<string>(nullable: true),
                    NewValueID = table.Column<string>(nullable: true),
                    HostName = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderSlotAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProviderSlotAudit_ProviderSlot_ProviderSlotID",
                        column: x => x.ProviderSlotID,
                        principalTable: "ProviderSlot",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SettingsAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SettingsID = table.Column<long>(nullable: false),
                    TransactionID = table.Column<long>(nullable: false),
                    ColumnName = table.Column<string>(nullable: true),
                    CurrentValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    CurrentValueID = table.Column<string>(nullable: true),
                    NewValueID = table.Column<string>(nullable: true),
                    HostName = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingsAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SettingsAudit_Settings_SettingsID",
                        column: x => x.SettingsID,
                        principalTable: "Settings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VisitStatusAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VisitStatusID = table.Column<long>(nullable: false),
                    TransactionID = table.Column<long>(nullable: false),
                    ColumnName = table.Column<string>(nullable: true),
                    CurrentValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    CurrentValueID = table.Column<string>(nullable: true),
                    NewValueID = table.Column<string>(nullable: true),
                    HostName = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitStatusAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_VisitStatusAudit_VisitStatus_VisitStatusID",
                        column: x => x.VisitStatusID,
                        principalTable: "VisitStatus",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientEligibilityAudit_PatientEligibilityID",
                table: "PatientEligibilityAudit",
                column: "PatientEligibilityID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientEligibilityDetailAudit_PatientEligibilityDetailID",
                table: "PatientEligibilityDetailAudit",
                column: "PatientEligibilityDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientEligibilityLogAudit_PatientEligibilityLogID",
                table: "PatientEligibilityLogAudit",
                column: "PatientEligibilityLogID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFollowUpChargeAudit_PatientFollowUpChargeID",
                table: "PatientFollowUpChargeAudit",
                column: "PatientFollowUpChargeID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanFollowupChargeAudit_PlanFollowupChargeID",
                table: "PlanFollowupChargeAudit",
                column: "PlanFollowupChargeID");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderSlotAudit_ProviderSlotID",
                table: "ProviderSlotAudit",
                column: "ProviderSlotID");

            migrationBuilder.CreateIndex(
                name: "IX_SettingsAudit_SettingsID",
                table: "SettingsAudit",
                column: "SettingsID");

            migrationBuilder.CreateIndex(
                name: "IX_VisitStatusAudit_VisitStatusID",
                table: "VisitStatusAudit",
                column: "VisitStatusID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientEligibilityAudit");

            migrationBuilder.DropTable(
                name: "PatientEligibilityDetailAudit");

            migrationBuilder.DropTable(
                name: "PatientEligibilityLogAudit");

            migrationBuilder.DropTable(
                name: "PatientFollowUpChargeAudit");

            migrationBuilder.DropTable(
                name: "PlanFollowupChargeAudit");

            migrationBuilder.DropTable(
                name: "ProviderSlotAudit");

            migrationBuilder.DropTable(
                name: "SettingsAudit");

            migrationBuilder.DropTable(
                name: "VisitStatusAudit");
        }
    }
}

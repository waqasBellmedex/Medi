using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddMoreAuditTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActionAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActionID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_ActionAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ActionAudit_Action_ActionID",
                        column: x => x.ActionID,
                        principalTable: "Action",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdjusmentCodeAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdjusmentCodeID = table.Column<long>(nullable: false),
                    AdjustmentCodeID = table.Column<long>(nullable: true),
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
                    table.PrimaryKey("PK_AdjusmentCodeAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AdjusmentCodeAudit_AdjustmentCode_AdjustmentCodeID",
                        column: x => x.AdjustmentCodeID,
                        principalTable: "AdjustmentCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChargeSubmissionHistoryAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChargeSubmissionHistoryID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_ChargeSubmissionHistoryAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ChargeSubmissionHistoryAudit_ChargeSubmissionHistory_ChargeSubmissionHistoryID",
                        column: x => x.ChargeSubmissionHistoryID,
                        principalTable: "ChargeSubmissionHistory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DesignationsAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DesignationsID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_DesignationsAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DesignationsAudit_Designations_DesignationsID",
                        column: x => x.DesignationsID,
                        principalTable: "Designations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GroupID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_GroupAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GroupAudit_Group_GroupID",
                        column: x => x.GroupID,
                        principalTable: "Group",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotesAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NotesID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_NotesAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NotesAudit_Notes_NotesID",
                        column: x => x.NotesID,
                        principalTable: "Notes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientFollowUpAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientFollowUpID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_PatientFollowUpAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientFollowUpAudit_PatientFollowUp_PatientFollowUpID",
                        column: x => x.PatientFollowUpID,
                        principalTable: "PatientFollowUp",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientPaymentAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientPaymentID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_PatientPaymentAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientPaymentAudit_PatientPayment_PatientPaymentID",
                        column: x => x.PatientPaymentID,
                        principalTable: "PatientPayment",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientPaymentChargeAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientPaymentChargeID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_PatientPaymentChargeAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientPaymentChargeAudit_PatientPaymentCharge_PatientPaymentChargeID",
                        column: x => x.PatientPaymentChargeID,
                        principalTable: "PatientPaymentCharge",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentChargeAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PaymentChargeID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_PaymentChargeAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PaymentChargeAudit_PaymentCharge_PaymentChargeID",
                        column: x => x.PaymentChargeID,
                        principalTable: "PaymentCharge",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentCheckAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PaymentCheckID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_PaymentCheckAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PaymentCheckAudit_PaymentCheck_PaymentCheckID",
                        column: x => x.PaymentCheckID,
                        principalTable: "PaymentCheck",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentLedgerAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PaymentLedgerID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_PaymentLedgerAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PaymentLedgerAudit_PaymentLedger_PaymentLedgerID",
                        column: x => x.PaymentLedgerID,
                        principalTable: "PaymentLedger",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentVisitAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PaymentVisitID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_PaymentVisitAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PaymentVisitAudit_PaymentVisit_PaymentVisitID",
                        column: x => x.PaymentVisitID,
                        principalTable: "PaymentVisit",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProviderScheduleAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProviderScheduleID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_ProviderScheduleAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProviderScheduleAudit_ProviderSchedule_ProviderScheduleID",
                        column: x => x.ProviderScheduleID,
                        principalTable: "ProviderSchedule",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReasonAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ReasonID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_ReasonAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ReasonAudit_Reason_ReasonID",
                        column: x => x.ReasonID,
                        principalTable: "Reason",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RemarkCodeAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RemarkCodeID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_RemarkCodeAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RemarkCodeAudit_RemarkCode_RemarkCodeID",
                        column: x => x.RemarkCodeID,
                        principalTable: "RemarkCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportsLogAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ReportsLogID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_ReportsLogAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ReportsLogAudit_ReportsLog_ReportsLogID",
                        column: x => x.ReportsLogID,
                        principalTable: "ReportsLog",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResubmitHistoryAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ResubmitHistoryID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_ResubmitHistoryAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ResubmitHistoryAudit_ResubmitHistory_ResubmitHistoryID",
                        column: x => x.ResubmitHistoryID,
                        principalTable: "ResubmitHistory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RightsAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RightsID = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_RightsAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RightsAudit_Rights_RightsID",
                        column: x => x.RightsID,
                        principalTable: "Rights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionAudit_ActionID",
                table: "ActionAudit",
                column: "ActionID");

            migrationBuilder.CreateIndex(
                name: "IX_AdjusmentCodeAudit_AdjustmentCodeID",
                table: "AdjusmentCodeAudit",
                column: "AdjustmentCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeSubmissionHistoryAudit_ChargeSubmissionHistoryID",
                table: "ChargeSubmissionHistoryAudit",
                column: "ChargeSubmissionHistoryID");

            migrationBuilder.CreateIndex(
                name: "IX_DesignationsAudit_DesignationsID",
                table: "DesignationsAudit",
                column: "DesignationsID");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAudit_GroupID",
                table: "GroupAudit",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_NotesAudit_NotesID",
                table: "NotesAudit",
                column: "NotesID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFollowUpAudit_PatientFollowUpID",
                table: "PatientFollowUpAudit",
                column: "PatientFollowUpID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPaymentAudit_PatientPaymentID",
                table: "PatientPaymentAudit",
                column: "PatientPaymentID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPaymentChargeAudit_PatientPaymentChargeID",
                table: "PatientPaymentChargeAudit",
                column: "PatientPaymentChargeID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentChargeAudit_PaymentChargeID",
                table: "PaymentChargeAudit",
                column: "PaymentChargeID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCheckAudit_PaymentCheckID",
                table: "PaymentCheckAudit",
                column: "PaymentCheckID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentLedgerAudit_PaymentLedgerID",
                table: "PaymentLedgerAudit",
                column: "PaymentLedgerID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVisitAudit_PaymentVisitID",
                table: "PaymentVisitAudit",
                column: "PaymentVisitID");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderScheduleAudit_ProviderScheduleID",
                table: "ProviderScheduleAudit",
                column: "ProviderScheduleID");

            migrationBuilder.CreateIndex(
                name: "IX_ReasonAudit_ReasonID",
                table: "ReasonAudit",
                column: "ReasonID");

            migrationBuilder.CreateIndex(
                name: "IX_RemarkCodeAudit_RemarkCodeID",
                table: "RemarkCodeAudit",
                column: "RemarkCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_ReportsLogAudit_ReportsLogID",
                table: "ReportsLogAudit",
                column: "ReportsLogID");

            migrationBuilder.CreateIndex(
                name: "IX_ResubmitHistoryAudit_ResubmitHistoryID",
                table: "ResubmitHistoryAudit",
                column: "ResubmitHistoryID");

            migrationBuilder.CreateIndex(
                name: "IX_RightsAudit_RightsID",
                table: "RightsAudit",
                column: "RightsID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionAudit");

            migrationBuilder.DropTable(
                name: "AdjusmentCodeAudit");

            migrationBuilder.DropTable(
                name: "ChargeSubmissionHistoryAudit");

            migrationBuilder.DropTable(
                name: "DesignationsAudit");

            migrationBuilder.DropTable(
                name: "GroupAudit");

            migrationBuilder.DropTable(
                name: "NotesAudit");

            migrationBuilder.DropTable(
                name: "PatientFollowUpAudit");

            migrationBuilder.DropTable(
                name: "PatientPaymentAudit");

            migrationBuilder.DropTable(
                name: "PatientPaymentChargeAudit");

            migrationBuilder.DropTable(
                name: "PaymentChargeAudit");

            migrationBuilder.DropTable(
                name: "PaymentCheckAudit");

            migrationBuilder.DropTable(
                name: "PaymentLedgerAudit");

            migrationBuilder.DropTable(
                name: "PaymentVisitAudit");

            migrationBuilder.DropTable(
                name: "ProviderScheduleAudit");

            migrationBuilder.DropTable(
                name: "ReasonAudit");

            migrationBuilder.DropTable(
                name: "RemarkCodeAudit");

            migrationBuilder.DropTable(
                name: "ReportsLogAudit");

            migrationBuilder.DropTable(
                name: "ResubmitHistoryAudit");

            migrationBuilder.DropTable(
                name: "RightsAudit");
        }
    }
}

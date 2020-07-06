using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddAuditModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentValueID",
                table: "VisitAudit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewValueID",
                table: "VisitAudit",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Cpt",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.CreateTable(
                name: "BatchDocumentAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BatchDocumentID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_BatchDocumentAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BatchDocumentAudit_BatchDocument_BatchDocumentID",
                        column: x => x.BatchDocumentID,
                        principalTable: "BatchDocument",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillerAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BillerID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_BillerAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BillerAudit_Biller_BillerID",
                        column: x => x.BillerID,
                        principalTable: "Biller",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypeAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DocumentTypeID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_DocumentTypeAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DocumentTypeAudit_DocumentType_DocumentTypeID",
                        column: x => x.DocumentTypeID,
                        principalTable: "DocumentType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ICDAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ICDID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_ICDAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ICDAudit_ICD_ICDID",
                        column: x => x.ICDID,
                        principalTable: "ICD",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InsurancePlanAddressAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InsurancePlanAddressID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_InsurancePlanAddressAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_InsurancePlanAddressAudit_InsurancePlanAddress_InsurancePlanAddressID",
                        column: x => x.InsurancePlanAddressID,
                        principalTable: "InsurancePlanAddress",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MainUserPracticeAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_MainUserPracticeAudit", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PatientAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_PatientAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientAudit_Patient_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patient",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientPlanAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientPlanID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_PatientPlanAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientPlanAudit_PatientPlan_PatientPlanID",
                        column: x => x.PatientPlanID,
                        principalTable: "PatientPlan",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StatusCodeAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StatusCodeID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_StatusCodeAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_StatusCodeAudit_StatusCode_StatusCodeID",
                        column: x => x.StatusCodeID,
                        principalTable: "StatusCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubmissionLogAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SubmissionLogID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_SubmissionLogAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SubmissionLogAudit_SubmissionLog_SubmissionLogID",
                        column: x => x.SubmissionLogID,
                        principalTable: "SubmissionLog",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VisitReasonAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VisitReasonID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_VisitReasonAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_VisitReasonAudit_VisitReason_VisitReasonID",
                        column: x => x.VisitReasonID,
                        principalTable: "VisitReason",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VisitStatusLogAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VisitStatusLogID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_VisitStatusLogAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_VisitStatusLogAudit_VisitStatusLog_VisitStatusLogID",
                        column: x => x.VisitStatusLogID,
                        principalTable: "VisitStatusLog",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BatchDocumentAudit_BatchDocumentID",
                table: "BatchDocumentAudit",
                column: "BatchDocumentID");

            migrationBuilder.CreateIndex(
                name: "IX_BillerAudit_BillerID",
                table: "BillerAudit",
                column: "BillerID");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTypeAudit_DocumentTypeID",
                table: "DocumentTypeAudit",
                column: "DocumentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_ICDAudit_ICDID",
                table: "ICDAudit",
                column: "ICDID");

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePlanAddressAudit_InsurancePlanAddressID",
                table: "InsurancePlanAddressAudit",
                column: "InsurancePlanAddressID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAudit_PatientID",
                table: "PatientAudit",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPlanAudit_PatientPlanID",
                table: "PatientPlanAudit",
                column: "PatientPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_StatusCodeAudit_StatusCodeID",
                table: "StatusCodeAudit",
                column: "StatusCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionLogAudit_SubmissionLogID",
                table: "SubmissionLogAudit",
                column: "SubmissionLogID");

            migrationBuilder.CreateIndex(
                name: "IX_VisitReasonAudit_VisitReasonID",
                table: "VisitReasonAudit",
                column: "VisitReasonID");

            migrationBuilder.CreateIndex(
                name: "IX_VisitStatusLogAudit_VisitStatusLogID",
                table: "VisitStatusLogAudit",
                column: "VisitStatusLogID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BatchDocumentAudit");

            migrationBuilder.DropTable(
                name: "BillerAudit");

            migrationBuilder.DropTable(
                name: "DocumentTypeAudit");

            migrationBuilder.DropTable(
                name: "ICDAudit");

            migrationBuilder.DropTable(
                name: "InsurancePlanAddressAudit");

            migrationBuilder.DropTable(
                name: "MainUserPracticeAudit");

            migrationBuilder.DropTable(
                name: "PatientAudit");

            migrationBuilder.DropTable(
                name: "PatientPlanAudit");

            migrationBuilder.DropTable(
                name: "StatusCodeAudit");

            migrationBuilder.DropTable(
                name: "SubmissionLogAudit");

            migrationBuilder.DropTable(
                name: "VisitReasonAudit");

            migrationBuilder.DropTable(
                name: "VisitStatusLogAudit");

            migrationBuilder.DropColumn(
                name: "CurrentValueID",
                table: "VisitAudit");

            migrationBuilder.DropColumn(
                name: "NewValueID",
                table: "VisitAudit");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Cpt",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}

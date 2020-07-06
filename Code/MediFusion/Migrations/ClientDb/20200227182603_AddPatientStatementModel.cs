using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddPatientStatementModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutoPlanFollowUpLog_Client_ClientID",
                table: "AutoPlanFollowUpLog");

            migrationBuilder.DropIndex(
                name: "IX_AutoPlanFollowUpLog_ClientID",
                table: "AutoPlanFollowUpLog");

            migrationBuilder.DropColumn(
                name: "ClientID",
                table: "AutoPlanFollowUpLog");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastStatementDate",
                table: "Visit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatementStatus",
                table: "Visit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatementAgingDays",
                table: "Practice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatementExportType",
                table: "Practice",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatementMaxCount",
                table: "Practice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatementMessage",
                table: "Practice",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PatientStatement",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientID = table.Column<long>(nullable: false),
                    PracticeID = table.Column<long>(nullable: false),
                    VisitID = table.Column<long>(nullable: false),
                    pdf_url = table.Column<string>(nullable: true),
                    csv_url = table.Column<string>(nullable: true),
                    statementStatus = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientStatement", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PatientStatementChargeHistory",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientStatementID = table.Column<long>(nullable: false),
                    ChargeID = table.Column<long>(nullable: false),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientStatementChargeHistory", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientStatement");

            migrationBuilder.DropTable(
                name: "PatientStatementChargeHistory");

            migrationBuilder.DropColumn(
                name: "LastStatementDate",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "StatementStatus",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "StatementAgingDays",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "StatementExportType",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "StatementMaxCount",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "StatementMessage",
                table: "Practice");

            migrationBuilder.AddColumn<long>(
                name: "ClientID",
                table: "AutoPlanFollowUpLog",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_AutoPlanFollowUpLog_ClientID",
                table: "AutoPlanFollowUpLog",
                column: "ClientID");

            migrationBuilder.AddForeignKey(
                name: "FK_AutoPlanFollowUpLog_Client_ClientID",
                table: "AutoPlanFollowUpLog",
                column: "ClientID",
                principalTable: "Client",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

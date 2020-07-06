using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class RemoveChangesInProviderModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PayToAddress1",
                table: "Provider",
                maxLength: 55,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayToAddress2",
                table: "Provider",
                maxLength: 55,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayToCity",
                table: "Provider",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayToState",
                table: "Provider",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayToZipCode",
                table: "Provider",
                maxLength: 9,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReportTaxID",
                table: "Provider",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DownloadedFileAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DownloadedFileID = table.Column<long>(nullable: false),
                    DownloadedFilesID = table.Column<long>(nullable: true),
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
                    table.PrimaryKey("PK_DownloadedFileAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DownloadedFileAudit_DownloadedFileAudit_DownloadedFilesID",
                        column: x => x.DownloadedFilesID,
                        principalTable: "DownloadedFileAudit",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanFollowupAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PlanFollowupID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_PlanFollowupAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PlanFollowupAudit_PlanFollowUp_PlanFollowupID",
                        column: x => x.PlanFollowupID,
                        principalTable: "PlanFollowUp",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DownloadedFileAudit_DownloadedFilesID",
                table: "DownloadedFileAudit",
                column: "DownloadedFilesID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanFollowupAudit_PlanFollowupID",
                table: "PlanFollowupAudit",
                column: "PlanFollowupID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DownloadedFileAudit");

            migrationBuilder.DropTable(
                name: "PlanFollowupAudit");

            migrationBuilder.DropColumn(
                name: "PayToAddress1",
                table: "Provider");

            migrationBuilder.DropColumn(
                name: "PayToAddress2",
                table: "Provider");

            migrationBuilder.DropColumn(
                name: "PayToCity",
                table: "Provider");

            migrationBuilder.DropColumn(
                name: "PayToState",
                table: "Provider");

            migrationBuilder.DropColumn(
                name: "PayToZipCode",
                table: "Provider");

            migrationBuilder.DropColumn(
                name: "ReportTaxID",
                table: "Provider");
        }
    }
}

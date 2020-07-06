using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddBatchDocumentChargenPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BatchDocumentCharges",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BatchDocumentNoID = table.Column<long>(nullable: true),
                    DOS = table.Column<DateTime>(type: "Date", nullable: true),
                    NoOfVisits = table.Column<long>(nullable: true),
                    Copay = table.Column<decimal>(nullable: true),
                    OtherPatientAmount = table.Column<decimal>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchDocumentCharges", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BatchDocumentCharges_BatchDocument_BatchDocumentNoID",
                        column: x => x.BatchDocumentNoID,
                        principalTable: "BatchDocument",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BatchDocumentPayment",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BatchDocumentNoID = table.Column<long>(nullable: true),
                    CheckNo = table.Column<string>(nullable: true),
                    CheckDate = table.Column<DateTime>(type: "Date", nullable: true),
                    CheckAmount = table.Column<decimal>(nullable: true),
                    Applied = table.Column<decimal>(nullable: true),
                    UnApplied = table.Column<decimal>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchDocumentPayment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BatchDocumentPayment_BatchDocument_BatchDocumentNoID",
                        column: x => x.BatchDocumentNoID,
                        principalTable: "BatchDocument",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BatchDocumentCharges_BatchDocumentNoID",
                table: "BatchDocumentCharges",
                column: "BatchDocumentNoID");

            migrationBuilder.CreateIndex(
                name: "IX_BatchDocumentPayment_BatchDocumentNoID",
                table: "BatchDocumentPayment",
                column: "BatchDocumentNoID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BatchDocumentCharges");

            migrationBuilder.DropTable(
                name: "BatchDocumentPayment");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class RemoveAdjusmentcodeAudit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdjusmentCodeAudit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdjusmentCodeAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    AdjusmentCodeID = table.Column<long>(nullable: false),
                    AdjustmentCodeID = table.Column<long>(nullable: true),
                    ColumnName = table.Column<string>(nullable: true),
                    CurrentValue = table.Column<string>(nullable: true),
                    CurrentValueID = table.Column<string>(nullable: true),
                    HostName = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    NewValueID = table.Column<string>(nullable: true),
                    TransactionID = table.Column<long>(nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_AdjusmentCodeAudit_AdjustmentCodeID",
                table: "AdjusmentCodeAudit",
                column: "AdjustmentCodeID");
        }
    }
}

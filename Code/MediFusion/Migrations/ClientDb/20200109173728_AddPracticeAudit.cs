using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddPracticeAudit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PracticeAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PracticeID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_PracticeAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PracticeAudit_Practice_PracticeID",
                        column: x => x.PracticeID,
                        principalTable: "Practice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PracticeAudit_PracticeID",
                table: "PracticeAudit",
                column: "PracticeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PracticeAudit");
        }
    }
}

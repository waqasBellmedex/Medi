using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddAutoPlanFollowUpLogInModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutoPlanFollowUpLog",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PracticeID = table.Column<long>(nullable: false),
                    ClientID = table.Column<long>(nullable: false),
                    TotalRecords = table.Column<long>(nullable: false),
                    FollowUpCreated = table.Column<long>(nullable: false),
                    LogMessage = table.Column<string>(nullable: true),
                    Exception = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoPlanFollowUpLog", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AutoPlanFollowUpLog_Client_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Client",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AutoPlanFollowUpLog_Practice_PracticeID",
                        column: x => x.PracticeID,
                        principalTable: "Practice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutoPlanFollowUpLog_ClientID",
                table: "AutoPlanFollowUpLog",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_AutoPlanFollowUpLog_PracticeID",
                table: "AutoPlanFollowUpLog",
                column: "PracticeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoPlanFollowUpLog");
        }
    }
}

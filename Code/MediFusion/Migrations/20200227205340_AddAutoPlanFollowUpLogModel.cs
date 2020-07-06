using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations
{
    public partial class AddAutoPlanFollowUpLogModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProvFirstName",
                table: "MainPractice",
                maxLength: 35,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProvLastName",
                table: "MainPractice",
                maxLength: 35,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProvMiddleInitial",
                table: "MainPractice",
                maxLength: 3,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AutoPlanFollowUpLog",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientID = table.Column<long>(nullable: false),
                    MainClientID = table.Column<long>(nullable: true),
                    PracticeID = table.Column<long>(nullable: false),
                    MainPracticeID = table.Column<long>(nullable: true),
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
                        name: "FK_AutoPlanFollowUpLog_MainClient_MainClientID",
                        column: x => x.MainClientID,
                        principalTable: "MainClient",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AutoPlanFollowUpLog_MainPractice_MainPracticeID",
                        column: x => x.MainPracticeID,
                        principalTable: "MainPractice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutoPlanFollowUpLog_MainClientID",
                table: "AutoPlanFollowUpLog",
                column: "MainClientID");

            migrationBuilder.CreateIndex(
                name: "IX_AutoPlanFollowUpLog_MainPracticeID",
                table: "AutoPlanFollowUpLog",
                column: "MainPracticeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoPlanFollowUpLog");

            migrationBuilder.DropColumn(
                name: "ProvFirstName",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "ProvLastName",
                table: "MainPractice");

            migrationBuilder.DropColumn(
                name: "ProvMiddleInitial",
                table: "MainPractice");
        }
    }
}

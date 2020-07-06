using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class RemoveAutoPlanFollowUpLogModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoPlanFollowUpLog");

            migrationBuilder.AddColumn<string>(
                name: "ProvFirstName",
                table: "Practice",
                maxLength: 35,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProvLastName",
                table: "Practice",
                maxLength: 35,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProvMiddleInitial",
                table: "Practice",
                maxLength: 3,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProvFirstName",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "ProvLastName",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "ProvMiddleInitial",
                table: "Practice");

            migrationBuilder.CreateTable(
                name: "AutoPlanFollowUpLog",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Exception = table.Column<string>(nullable: true),
                    FollowUpCreated = table.Column<long>(nullable: false),
                    LogMessage = table.Column<string>(nullable: true),
                    PracticeID = table.Column<long>(nullable: false),
                    TotalRecords = table.Column<long>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoPlanFollowUpLog", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AutoPlanFollowUpLog_Practice_PracticeID",
                        column: x => x.PracticeID,
                        principalTable: "Practice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutoPlanFollowUpLog_PracticeID",
                table: "AutoPlanFollowUpLog",
                column: "PracticeID");
        }
    }
}

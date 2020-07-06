using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddVisitStatusLogIDINVisitStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "VisitStatusLogID",
                table: "VisitStatus",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VisitStatus_VisitStatusLogID",
                table: "VisitStatus",
                column: "VisitStatusLogID");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitStatus_VisitStatusLog_VisitStatusLogID",
                table: "VisitStatus",
                column: "VisitStatusLogID",
                principalTable: "VisitStatusLog",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitStatus_VisitStatusLog_VisitStatusLogID",
                table: "VisitStatus");

            migrationBuilder.DropIndex(
                name: "IX_VisitStatus_VisitStatusLogID",
                table: "VisitStatus");

            migrationBuilder.DropColumn(
                name: "VisitStatusLogID",
                table: "VisitStatus");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddSubmissionLogIdsInVisit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SubmissionLogID2",
                table: "Visit",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SubmissionLogID3",
                table: "Visit",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SubmissionLogID2",
                table: "Charge",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SubmissionLogID3",
                table: "Charge",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Visit_SubmissionLogID2",
                table: "Visit",
                column: "SubmissionLogID2");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_SubmissionLogID3",
                table: "Visit",
                column: "SubmissionLogID3");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_SubmissionLogID2",
                table: "Charge",
                column: "SubmissionLogID2");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_SubmissionLogID3",
                table: "Charge",
                column: "SubmissionLogID3");

            migrationBuilder.AddForeignKey(
                name: "FK_Charge_SubmissionLog_SubmissionLogID2",
                table: "Charge",
                column: "SubmissionLogID2",
                principalTable: "SubmissionLog",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Charge_SubmissionLog_SubmissionLogID3",
                table: "Charge",
                column: "SubmissionLogID3",
                principalTable: "SubmissionLog",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Visit_SubmissionLog_SubmissionLogID2",
                table: "Visit",
                column: "SubmissionLogID2",
                principalTable: "SubmissionLog",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Visit_SubmissionLog_SubmissionLogID3",
                table: "Visit",
                column: "SubmissionLogID3",
                principalTable: "SubmissionLog",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Charge_SubmissionLog_SubmissionLogID2",
                table: "Charge");

            migrationBuilder.DropForeignKey(
                name: "FK_Charge_SubmissionLog_SubmissionLogID3",
                table: "Charge");

            migrationBuilder.DropForeignKey(
                name: "FK_Visit_SubmissionLog_SubmissionLogID2",
                table: "Visit");

            migrationBuilder.DropForeignKey(
                name: "FK_Visit_SubmissionLog_SubmissionLogID3",
                table: "Visit");

            migrationBuilder.DropIndex(
                name: "IX_Visit_SubmissionLogID2",
                table: "Visit");

            migrationBuilder.DropIndex(
                name: "IX_Visit_SubmissionLogID3",
                table: "Visit");

            migrationBuilder.DropIndex(
                name: "IX_Charge_SubmissionLogID2",
                table: "Charge");

            migrationBuilder.DropIndex(
                name: "IX_Charge_SubmissionLogID3",
                table: "Charge");

            migrationBuilder.DropColumn(
                name: "SubmissionLogID2",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "SubmissionLogID3",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "SubmissionLogID2",
                table: "Charge");

            migrationBuilder.DropColumn(
                name: "SubmissionLogID3",
                table: "Charge");
        }
    }
}

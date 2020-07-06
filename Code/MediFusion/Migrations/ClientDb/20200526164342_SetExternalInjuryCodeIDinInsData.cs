using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class SetExternalInjuryCodeIDinInsData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstitutionalData_ExternalInjuryCode_ExternalInjury1ID",
                table: "InstitutionalData");

            migrationBuilder.DropForeignKey(
                name: "FK_InstitutionalData_ExternalInjuryCode_ExternalInjury2ID",
                table: "InstitutionalData");

            migrationBuilder.DropForeignKey(
                name: "FK_InstitutionalData_ExternalInjuryCode_ExternalInjury3ID",
                table: "InstitutionalData");

            migrationBuilder.DropIndex(
                name: "IX_InstitutionalData_ExternalInjury1ID",
                table: "InstitutionalData");

            migrationBuilder.DropIndex(
                name: "IX_InstitutionalData_ExternalInjury2ID",
                table: "InstitutionalData");

            migrationBuilder.DropIndex(
                name: "IX_InstitutionalData_ExternalInjury3ID",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "ExternalInjury1ID",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "ExternalInjury2ID",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "ExternalInjury3ID",
                table: "InstitutionalData");

            migrationBuilder.RenameColumn(
                name: "ExternalInjuryCode3",
                table: "InstitutionalData",
                newName: "ExternalInjuryCode3ID");

            migrationBuilder.RenameColumn(
                name: "ExternalInjuryCode2",
                table: "InstitutionalData",
                newName: "ExternalInjuryCode2ID");

            migrationBuilder.RenameColumn(
                name: "ExternalInjuryCode1",
                table: "InstitutionalData",
                newName: "ExternalInjuryCode1ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ExternalInjuryCode1ID",
                table: "InstitutionalData",
                column: "ExternalInjuryCode1ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ExternalInjuryCode2ID",
                table: "InstitutionalData",
                column: "ExternalInjuryCode2ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ExternalInjuryCode3ID",
                table: "InstitutionalData",
                column: "ExternalInjuryCode3ID");

            migrationBuilder.AddForeignKey(
                name: "FK_InstitutionalData_ExternalInjuryCode_ExternalInjuryCode1ID",
                table: "InstitutionalData",
                column: "ExternalInjuryCode1ID",
                principalTable: "ExternalInjuryCode",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InstitutionalData_ExternalInjuryCode_ExternalInjuryCode2ID",
                table: "InstitutionalData",
                column: "ExternalInjuryCode2ID",
                principalTable: "ExternalInjuryCode",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InstitutionalData_ExternalInjuryCode_ExternalInjuryCode3ID",
                table: "InstitutionalData",
                column: "ExternalInjuryCode3ID",
                principalTable: "ExternalInjuryCode",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstitutionalData_ExternalInjuryCode_ExternalInjuryCode1ID",
                table: "InstitutionalData");

            migrationBuilder.DropForeignKey(
                name: "FK_InstitutionalData_ExternalInjuryCode_ExternalInjuryCode2ID",
                table: "InstitutionalData");

            migrationBuilder.DropForeignKey(
                name: "FK_InstitutionalData_ExternalInjuryCode_ExternalInjuryCode3ID",
                table: "InstitutionalData");

            migrationBuilder.DropIndex(
                name: "IX_InstitutionalData_ExternalInjuryCode1ID",
                table: "InstitutionalData");

            migrationBuilder.DropIndex(
                name: "IX_InstitutionalData_ExternalInjuryCode2ID",
                table: "InstitutionalData");

            migrationBuilder.DropIndex(
                name: "IX_InstitutionalData_ExternalInjuryCode3ID",
                table: "InstitutionalData");

            migrationBuilder.RenameColumn(
                name: "ExternalInjuryCode3ID",
                table: "InstitutionalData",
                newName: "ExternalInjuryCode3");

            migrationBuilder.RenameColumn(
                name: "ExternalInjuryCode2ID",
                table: "InstitutionalData",
                newName: "ExternalInjuryCode2");

            migrationBuilder.RenameColumn(
                name: "ExternalInjuryCode1ID",
                table: "InstitutionalData",
                newName: "ExternalInjuryCode1");

            migrationBuilder.AddColumn<long>(
                name: "ExternalInjury1ID",
                table: "InstitutionalData",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ExternalInjury2ID",
                table: "InstitutionalData",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ExternalInjury3ID",
                table: "InstitutionalData",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ExternalInjury1ID",
                table: "InstitutionalData",
                column: "ExternalInjury1ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ExternalInjury2ID",
                table: "InstitutionalData",
                column: "ExternalInjury2ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ExternalInjury3ID",
                table: "InstitutionalData",
                column: "ExternalInjury3ID");

            migrationBuilder.AddForeignKey(
                name: "FK_InstitutionalData_ExternalInjuryCode_ExternalInjury1ID",
                table: "InstitutionalData",
                column: "ExternalInjury1ID",
                principalTable: "ExternalInjuryCode",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InstitutionalData_ExternalInjuryCode_ExternalInjury2ID",
                table: "InstitutionalData",
                column: "ExternalInjury2ID",
                principalTable: "ExternalInjuryCode",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InstitutionalData_ExternalInjuryCode_ExternalInjury3ID",
                table: "InstitutionalData",
                column: "ExternalInjury3ID",
                principalTable: "ExternalInjuryCode",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

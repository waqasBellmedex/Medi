using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddChangesVisitIDAuth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientAuthorizationUsed_Visit_VisitID",
                table: "PatientAuthorizationUsed");

            migrationBuilder.AlterColumn<long>(
                name: "VisitID",
                table: "PatientAuthorizationUsed",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAuthorizationUsed_Visit_VisitID",
                table: "PatientAuthorizationUsed",
                column: "VisitID",
                principalTable: "Visit",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientAuthorizationUsed_Visit_VisitID",
                table: "PatientAuthorizationUsed");

            migrationBuilder.AlterColumn<long>(
                name: "VisitID",
                table: "PatientAuthorizationUsed",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAuthorizationUsed_Visit_VisitID",
                table: "PatientAuthorizationUsed",
                column: "VisitID",
                principalTable: "Visit",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

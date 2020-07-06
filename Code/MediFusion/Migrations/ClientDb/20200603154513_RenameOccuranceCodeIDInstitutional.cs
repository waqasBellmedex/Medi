using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class RenameOccuranceCodeIDInstitutional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OccuranceSpanCodeID4",
                table: "InstitutionalData",
                newName: "OccuranceSpanCode4ID");

            migrationBuilder.RenameColumn(
                name: "OccuranceSpanCodeID3",
                table: "InstitutionalData",
                newName: "OccuranceSpanCode3ID");

            migrationBuilder.RenameColumn(
                name: "OccuranceSpanCodeID2",
                table: "InstitutionalData",
                newName: "OccuranceSpanCode2ID");

            migrationBuilder.RenameColumn(
                name: "OccuranceSpanCodeID1",
                table: "InstitutionalData",
                newName: "OccuranceSpanCode1ID");

            migrationBuilder.RenameColumn(
                name: "OccuranceCodeID8",
                table: "InstitutionalData",
                newName: "OccuranceCode8ID");

            migrationBuilder.RenameColumn(
                name: "OccuranceCodeID7",
                table: "InstitutionalData",
                newName: "OccuranceCode7ID");

            migrationBuilder.RenameColumn(
                name: "OccuranceCodeID6",
                table: "InstitutionalData",
                newName: "OccuranceCode6ID");

            migrationBuilder.RenameColumn(
                name: "OccuranceCodeID5",
                table: "InstitutionalData",
                newName: "OccuranceCode5ID");

            migrationBuilder.RenameColumn(
                name: "OccuranceCodeID4",
                table: "InstitutionalData",
                newName: "OccuranceCode4ID");

            migrationBuilder.RenameColumn(
                name: "OccuranceCodeID3",
                table: "InstitutionalData",
                newName: "OccuranceCode3ID");

            migrationBuilder.RenameColumn(
                name: "OccuranceCodeID2",
                table: "InstitutionalData",
                newName: "OccuranceCode2ID");

            migrationBuilder.RenameColumn(
                name: "OccuranceCodeID1",
                table: "InstitutionalData",
                newName: "OccuranceCode1ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OccuranceSpanCode4ID",
                table: "InstitutionalData",
                newName: "OccuranceSpanCodeID4");

            migrationBuilder.RenameColumn(
                name: "OccuranceSpanCode3ID",
                table: "InstitutionalData",
                newName: "OccuranceSpanCodeID3");

            migrationBuilder.RenameColumn(
                name: "OccuranceSpanCode2ID",
                table: "InstitutionalData",
                newName: "OccuranceSpanCodeID2");

            migrationBuilder.RenameColumn(
                name: "OccuranceSpanCode1ID",
                table: "InstitutionalData",
                newName: "OccuranceSpanCodeID1");

            migrationBuilder.RenameColumn(
                name: "OccuranceCode8ID",
                table: "InstitutionalData",
                newName: "OccuranceCodeID8");

            migrationBuilder.RenameColumn(
                name: "OccuranceCode7ID",
                table: "InstitutionalData",
                newName: "OccuranceCodeID7");

            migrationBuilder.RenameColumn(
                name: "OccuranceCode6ID",
                table: "InstitutionalData",
                newName: "OccuranceCodeID6");

            migrationBuilder.RenameColumn(
                name: "OccuranceCode5ID",
                table: "InstitutionalData",
                newName: "OccuranceCodeID5");

            migrationBuilder.RenameColumn(
                name: "OccuranceCode4ID",
                table: "InstitutionalData",
                newName: "OccuranceCodeID4");

            migrationBuilder.RenameColumn(
                name: "OccuranceCode3ID",
                table: "InstitutionalData",
                newName: "OccuranceCodeID3");

            migrationBuilder.RenameColumn(
                name: "OccuranceCode2ID",
                table: "InstitutionalData",
                newName: "OccuranceCodeID2");

            migrationBuilder.RenameColumn(
                name: "OccuranceCode1ID",
                table: "InstitutionalData",
                newName: "OccuranceCodeID1");
        }
    }
}

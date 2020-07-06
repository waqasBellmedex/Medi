using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class RenameALLConditionCodeIDsInInstitutional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConditionCodeID1",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "ConditionCodeID10",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "ConditionCodeID11",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "ConditionCodeID12",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "ConditionCodeID2",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "ConditionCodeID3",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "ConditionCodeID4",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "ConditionCodeID5",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "ConditionCodeID6",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "ConditionCodeID7",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "ConditionCodeID8",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "ConditionCodeID9",
                table: "InstitutionalData");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ConditionCodeID1",
                table: "InstitutionalData",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ConditionCodeID10",
                table: "InstitutionalData",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ConditionCodeID11",
                table: "InstitutionalData",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ConditionCodeID12",
                table: "InstitutionalData",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ConditionCodeID2",
                table: "InstitutionalData",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ConditionCodeID3",
                table: "InstitutionalData",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ConditionCodeID4",
                table: "InstitutionalData",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ConditionCodeID5",
                table: "InstitutionalData",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ConditionCodeID6",
                table: "InstitutionalData",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ConditionCodeID7",
                table: "InstitutionalData",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ConditionCodeID8",
                table: "InstitutionalData",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ConditionCodeID9",
                table: "InstitutionalData",
                nullable: true);
        }
    }
}

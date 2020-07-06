using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class RenameALLValueCode1IDInInstitutional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValueCodeID10",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "ValueCodeID11",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "ValueCodeID12",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "ValueCodeID2",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "ValueCodeID3",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "ValueCodeID4",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "ValueCodeID5",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "ValueCodeID6",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "ValueCodeID7",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "ValueCodeID8",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "ValueCodeID9",
                table: "InstitutionalData");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ValueCodeID10",
                table: "InstitutionalData",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ValueCodeID11",
                table: "InstitutionalData",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ValueCodeID12",
                table: "InstitutionalData",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ValueCodeID2",
                table: "InstitutionalData",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ValueCodeID3",
                table: "InstitutionalData",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ValueCodeID4",
                table: "InstitutionalData",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ValueCodeID5",
                table: "InstitutionalData",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ValueCodeID6",
                table: "InstitutionalData",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ValueCodeID7",
                table: "InstitutionalData",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ValueCodeID8",
                table: "InstitutionalData",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ValueCodeID9",
                table: "InstitutionalData",
                nullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddStatusInPatientAuthorization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResponsibleParty",
                table: "PatientAuthorization",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "PatientAuthorization",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "OnlinePortalsID",
                table: "OnlinePortalCredentialsAudit",
                nullable: true,
                oldClrType: typeof(long));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponsibleParty",
                table: "PatientAuthorization");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "PatientAuthorization");

            migrationBuilder.AlterColumn<long>(
                name: "OnlinePortalsID",
                table: "OnlinePortalCredentialsAudit",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}

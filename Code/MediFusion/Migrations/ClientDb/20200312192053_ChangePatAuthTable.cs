using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class ChangePatAuthTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PatientPlanID",
                table: "PatientAuthorization",
                newName: "InsurancePlanID");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorizationNumber",
                table: "PatientAuthorization",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remaining",
                table: "PatientAuthorization",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remaining",
                table: "PatientAuthorization");

            migrationBuilder.RenameColumn(
                name: "InsurancePlanID",
                table: "PatientAuthorization",
                newName: "PatientPlanID");

            migrationBuilder.AlterColumn<long>(
                name: "AuthorizationNumber",
                table: "PatientAuthorization",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

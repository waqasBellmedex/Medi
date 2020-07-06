using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddNewFieldsInExtentionMethods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AccountNum",
                table: "ExternalPatient",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "EmergencyAddress",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyCity",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContact",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyPhone",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyState",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyZip",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployerAddress",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployerCity",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployerName",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployerPhone",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployerState",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployerZip",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuarantarID",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuarantarName",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationNPI",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PracticeNPI",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryInsurance",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryInsuredID",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryInsuredName",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PrimaryPatientPlanID",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryProvider",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProviderNPI",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryInsurance",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryInsuredID",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryInsuredName",
                table: "ExternalPatient",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SecondaryPatientPlanID",
                table: "ExternalPatient",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmergencyAddress",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "EmergencyCity",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "EmergencyContact",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "EmergencyPhone",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "EmergencyState",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "EmergencyZip",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "EmployerAddress",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "EmployerCity",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "EmployerName",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "EmployerPhone",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "EmployerState",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "EmployerZip",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "GuarantarID",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "GuarantarName",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "LocationNPI",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "PracticeNPI",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "PrimaryInsurance",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "PrimaryInsuredID",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "PrimaryInsuredName",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "PrimaryPatientPlanID",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "PrimaryProvider",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "ProviderNPI",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "SecondaryInsurance",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "SecondaryInsuredID",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "SecondaryInsuredName",
                table: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "SecondaryPatientPlanID",
                table: "ExternalPatient");

            migrationBuilder.AlterColumn<string>(
                name: "AccountNum",
                table: "ExternalPatient",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

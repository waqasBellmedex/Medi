using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddReferralDocumentFileNameInProvider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReferralDocumentFileName",
                table: "Provider",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAutoFollowup",
                table: "Practice",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight_pounds",
                table: "PatientVitals",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight_lbs",
                table: "PatientVitals",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Temperature",
                table: "PatientVitals",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Respiratory_rate",
                table: "PatientVitals",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Pulse",
                table: "PatientVitals",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "Pain",
                table: "PatientVitals",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<decimal>(
                name: "OxygenSaturation",
                table: "PatientVitals",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<bool>(
                name: "Inactive",
                table: "PatientVitals",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<decimal>(
                name: "Height_inch",
                table: "PatientVitals",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Height_foot",
                table: "PatientVitals",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "HeadCircumference",
                table: "PatientVitals",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "BPSystolic",
                table: "PatientVitals",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "BPDiastolic",
                table: "PatientVitals",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "BMI",
                table: "PatientVitals",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<bool>(
                name: "Signed",
                table: "PatientNotes",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferralDocumentFileName",
                table: "Provider");

            migrationBuilder.DropColumn(
                name: "IsAutoFollowup",
                table: "Practice");

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight_pounds",
                table: "PatientVitals",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight_lbs",
                table: "PatientVitals",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Temperature",
                table: "PatientVitals",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Respiratory_rate",
                table: "PatientVitals",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Pulse",
                table: "PatientVitals",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Pain",
                table: "PatientVitals",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "OxygenSaturation",
                table: "PatientVitals",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Inactive",
                table: "PatientVitals",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Height_inch",
                table: "PatientVitals",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Height_foot",
                table: "PatientVitals",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "HeadCircumference",
                table: "PatientVitals",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BPSystolic",
                table: "PatientVitals",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BPDiastolic",
                table: "PatientVitals",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BMI",
                table: "PatientVitals",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Signed",
                table: "PatientNotes",
                nullable: true,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}

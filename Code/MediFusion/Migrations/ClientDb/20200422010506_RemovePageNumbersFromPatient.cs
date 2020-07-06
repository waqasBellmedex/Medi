using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class RemovePageNumbersFromPatient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "selfPay",
               table: "PatientAppointment");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                table: "PatientPayment",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<long>(
                name: "ProviderID",
                table: "ClinicalForms",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "PracticeID",
                table: "ClinicalForms",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<string>(
                name: "url",
                table: "ClinicalForms",
                nullable: true);



            migrationBuilder.DropColumn(
                name: "PageNumbers",
                table: "Patient");

            migrationBuilder.CreateTable(
               name: "PatientAlerts",
               columns: table => new
               {
                   ID = table.Column<long>(nullable: false)
                       .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                   patientID = table.Column<long>(nullable: false),
                   type = table.Column<string>(nullable: true),
                   date = table.Column<DateTime>(nullable: true),
                   assignedTo = table.Column<string>(nullable: true),
                   notes = table.Column<string>(nullable: true),
                   practiceId = table.Column<long>(nullable: true),
                   inactive = table.Column<bool>(nullable: true),
                   AddedDate = table.Column<DateTime>(nullable: true),
                   AddedBy = table.Column<string>(nullable: true),
                   UpdatedDate = table.Column<DateTime>(nullable: true),
                   UpdatedBy = table.Column<string>(nullable: true)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_PatientAlerts", x => x.ID);
               });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.AddColumn<string>(
                name: "PageNumbers",
                table: "Patient",
                nullable: true);
            migrationBuilder.DropTable(
               name: "PatientAlerts");

            migrationBuilder.DropColumn(
                name: "url",
                table: "ClinicalForms");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                table: "PatientPayment",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "selfPay",
                table: "PatientAppointment",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ProviderID",
                table: "ClinicalForms",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PracticeID",
                table: "ClinicalForms",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

        }
    }
}

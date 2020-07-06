using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddPracticeAndOtherKhalid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MissingInfo",
                table: "Patient");

            migrationBuilder.AddColumn<string>(
                name: "CalenderID",
                table: "Practice",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GoogleSheetRows",
                table: "Practice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "googleSheetID",
                table: "Practice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "googleSheetSecret",
                table: "Practice",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isGoogleSheetEnable",
                table: "Practice",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "PatientAppointmentID",
                table: "PatientAppointmentsExternal",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "addedBy",
                table: "PatientAppointmentsExternal",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "appointmentDate",
                table: "PatientAppointmentsExternal",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "appointmentTime",
                table: "PatientAppointmentsExternal",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "canlenderId",
                table: "PatientAppointmentsExternal",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "comments",
                table: "PatientAppointmentsExternal",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "dob",
                table: "PatientAppointmentsExternal",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "emailAddress",
                table: "PatientAppointmentsExternal",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "exception",
                table: "PatientAppointmentsExternal",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "firstName",
                table: "PatientAppointmentsExternal",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "insurancePlanName",
                table: "PatientAppointmentsExternal",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "interval",
                table: "PatientAppointmentsExternal",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isError",
                table: "PatientAppointmentsExternal",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "lastName",
                table: "PatientAppointmentsExternal",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "location",
                table: "PatientAppointmentsExternal",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "phoneNumber",
                table: "PatientAppointmentsExternal",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "policyNumber",
                table: "PatientAppointmentsExternal",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "proivder",
                table: "PatientAppointmentsExternal",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "rowNumber",
                table: "PatientAppointmentsExternal",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updatedBy",
                table: "PatientAppointmentsExternal",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updatedDate",
                table: "PatientAppointmentsExternal",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "sendAppointmentSMS",
                table: "Patient",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CalenderID",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "GoogleSheetRows",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "googleSheetID",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "googleSheetSecret",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "isGoogleSheetEnable",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "PatientAppointmentID",
                table: "PatientAppointmentsExternal");

            migrationBuilder.DropColumn(
                name: "addedBy",
                table: "PatientAppointmentsExternal");

            migrationBuilder.DropColumn(
                name: "appointmentDate",
                table: "PatientAppointmentsExternal");

            migrationBuilder.DropColumn(
                name: "appointmentTime",
                table: "PatientAppointmentsExternal");

            migrationBuilder.DropColumn(
                name: "canlenderId",
                table: "PatientAppointmentsExternal");

            migrationBuilder.DropColumn(
                name: "comments",
                table: "PatientAppointmentsExternal");

            migrationBuilder.DropColumn(
                name: "dob",
                table: "PatientAppointmentsExternal");

            migrationBuilder.DropColumn(
                name: "emailAddress",
                table: "PatientAppointmentsExternal");

            migrationBuilder.DropColumn(
                name: "exception",
                table: "PatientAppointmentsExternal");

            migrationBuilder.DropColumn(
                name: "firstName",
                table: "PatientAppointmentsExternal");

            migrationBuilder.DropColumn(
                name: "insurancePlanName",
                table: "PatientAppointmentsExternal");

            migrationBuilder.DropColumn(
                name: "interval",
                table: "PatientAppointmentsExternal");

            migrationBuilder.DropColumn(
                name: "isError",
                table: "PatientAppointmentsExternal");

            migrationBuilder.DropColumn(
                name: "lastName",
                table: "PatientAppointmentsExternal");

            migrationBuilder.DropColumn(
                name: "location",
                table: "PatientAppointmentsExternal");

            migrationBuilder.DropColumn(
                name: "phoneNumber",
                table: "PatientAppointmentsExternal");

            migrationBuilder.DropColumn(
                name: "policyNumber",
                table: "PatientAppointmentsExternal");

            migrationBuilder.DropColumn(
                name: "proivder",
                table: "PatientAppointmentsExternal");

            migrationBuilder.DropColumn(
                name: "rowNumber",
                table: "PatientAppointmentsExternal");

            migrationBuilder.DropColumn(
                name: "updatedBy",
                table: "PatientAppointmentsExternal");

            migrationBuilder.DropColumn(
                name: "updatedDate",
                table: "PatientAppointmentsExternal");

            migrationBuilder.DropColumn(
                name: "sendAppointmentSMS",
                table: "Patient");

            migrationBuilder.AddColumn<string>(
                name: "MissingInfo",
                table: "Patient",
                nullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddAsifSabChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisitID",
                table: "PatientAppointment");

            migrationBuilder.AddColumn<long>(
                name: "PatientAppointmentID",
                table: "Visit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "InActive",
                table: "PatientPayment",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "patientAppointmentID",
                table: "PatientPayment",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "VisitReasonID",
                table: "ICDMostFavourite",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<bool>(
                name: "Inactive",
                table: "ICDMostFavourite",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AddColumn<int>(
                name: "position",
                table: "GeneralItems",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AppointmentCPTID",
                table: "Charge",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SerialNo",
                table: "AppointmentICD",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "PracticeID",
                table: "AppointmentICD",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<bool>(
                name: "Inactive",
                table: "AppointmentICD",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AddColumn<long>(
                name: "ICDMostFavouriteID",
                table: "AppointmentICD",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Units",
                table: "AppointmentCPT",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                table: "AppointmentCPT",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<long>(
                name: "PracticeID",
                table: "AppointmentCPT",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "AppointmentCPT",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<long>(
                name: "CPTMostFavouriteID",
                table: "AppointmentCPT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ChargeID",
                table: "AppointmentCPT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pointer1",
                table: "AppointmentCPT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pointer2",
                table: "AppointmentCPT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pointer3",
                table: "AppointmentCPT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pointer4",
                table: "AppointmentCPT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Visit_PatientAppointmentID",
                table: "Visit",
                column: "PatientAppointmentID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPayment_patientAppointmentID",
                table: "PatientPayment",
                column: "patientAppointmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientPayment_PatientAppointment_patientAppointmentID",
                table: "PatientPayment",
                column: "patientAppointmentID",
                principalTable: "PatientAppointment",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Visit_PatientAppointment_PatientAppointmentID",
                table: "Visit",
                column: "PatientAppointmentID",
                principalTable: "PatientAppointment",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientPayment_PatientAppointment_patientAppointmentID",
                table: "PatientPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_Visit_PatientAppointment_PatientAppointmentID",
                table: "Visit");

            migrationBuilder.DropIndex(
                name: "IX_Visit_PatientAppointmentID",
                table: "Visit");

            migrationBuilder.DropIndex(
                name: "IX_PatientPayment_patientAppointmentID",
                table: "PatientPayment");

            migrationBuilder.DropColumn(
                name: "PatientAppointmentID",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "InActive",
                table: "PatientPayment");

            migrationBuilder.DropColumn(
                name: "patientAppointmentID",
                table: "PatientPayment");

            migrationBuilder.DropColumn(
                name: "position",
                table: "GeneralItems");

            migrationBuilder.DropColumn(
                name: "AppointmentCPTID",
                table: "Charge");

            migrationBuilder.DropColumn(
                name: "ICDMostFavouriteID",
                table: "AppointmentICD");

            migrationBuilder.DropColumn(
                name: "CPTMostFavouriteID",
                table: "AppointmentCPT");

            migrationBuilder.DropColumn(
                name: "ChargeID",
                table: "AppointmentCPT");

            migrationBuilder.DropColumn(
                name: "Pointer1",
                table: "AppointmentCPT");

            migrationBuilder.DropColumn(
                name: "Pointer2",
                table: "AppointmentCPT");

            migrationBuilder.DropColumn(
                name: "Pointer3",
                table: "AppointmentCPT");

            migrationBuilder.DropColumn(
                name: "Pointer4",
                table: "AppointmentCPT");

            migrationBuilder.AddColumn<long>(
                name: "VisitID",
                table: "PatientAppointment",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "VisitReasonID",
                table: "ICDMostFavourite",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Inactive",
                table: "ICDMostFavourite",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SerialNo",
                table: "AppointmentICD",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PracticeID",
                table: "AppointmentICD",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Inactive",
                table: "AppointmentICD",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Units",
                table: "AppointmentCPT",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                table: "AppointmentCPT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PracticeID",
                table: "AppointmentCPT",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "AppointmentCPT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}

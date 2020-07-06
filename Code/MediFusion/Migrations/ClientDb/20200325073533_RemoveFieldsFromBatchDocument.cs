using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class RemoveFieldsFromBatchDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BatchDocument_Biller_BillerID",
                table: "BatchDocument");

            migrationBuilder.DropIndex(
                name: "IX_BatchDocument_BillerID",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "AdmitEndDate",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "AdmitNotes",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "AdmitNumberOfPatients",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "AdmitNumberOfPatientsEntered",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "AdmitStartDate",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "BillerID",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "ChargeNotes",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "ChargesCopayApplied",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "ChargesEndTime",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "ChargesNumberOfDOS",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "ChargesNumberOfDOSEntered",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "ChargesNumberOfVisits",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "ChargesNumberOfVisitsEntered",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "ChargesStartDate",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "ChargesTotalCopay",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "PaymentAssignedDate",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "PaymentCheckDate",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "PaymentCopay",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "PaymentCopayApplied",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "PaymentEndDate",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "PaymentNotes",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "PaymentPatientAmount",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "PaymentPatientAppliedAmount",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "PaymentPlanAmount",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "PaymentPlanAppliedAmount",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "PaymentStartDate",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "PaymentTotalAmount",
                table: "BatchDocument");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AdmitEndDate",
                table: "BatchDocument",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdmitNotes",
                table: "BatchDocument",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AdmitNumberOfPatients",
                table: "BatchDocument",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "AdmitNumberOfPatientsEntered",
                table: "BatchDocument",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "AdmitStartDate",
                table: "BatchDocument",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BillerID",
                table: "BatchDocument",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "BatchDocument",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChargeNotes",
                table: "BatchDocument",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ChargesCopayApplied",
                table: "BatchDocument",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ChargesEndTime",
                table: "BatchDocument",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ChargesNumberOfDOS",
                table: "BatchDocument",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ChargesNumberOfDOSEntered",
                table: "BatchDocument",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ChargesNumberOfVisits",
                table: "BatchDocument",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ChargesNumberOfVisitsEntered",
                table: "BatchDocument",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "ChargesStartDate",
                table: "BatchDocument",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ChargesTotalCopay",
                table: "BatchDocument",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentAssignedDate",
                table: "BatchDocument",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentCheckDate",
                table: "BatchDocument",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PaymentCopay",
                table: "BatchDocument",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PaymentCopayApplied",
                table: "BatchDocument",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentEndDate",
                table: "BatchDocument",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentNotes",
                table: "BatchDocument",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PaymentPatientAmount",
                table: "BatchDocument",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PaymentPatientAppliedAmount",
                table: "BatchDocument",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PaymentPlanAmount",
                table: "BatchDocument",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PaymentPlanAppliedAmount",
                table: "BatchDocument",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentStartDate",
                table: "BatchDocument",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PaymentTotalAmount",
                table: "BatchDocument",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BatchDocument_BillerID",
                table: "BatchDocument",
                column: "BillerID");

            migrationBuilder.AddForeignKey(
                name: "FK_BatchDocument_Biller_BillerID",
                table: "BatchDocument",
                column: "BillerID",
                principalTable: "Biller",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class UstadJeeChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CopayPaid",
                table: "Visit",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OtherPatResp",
                table: "Visit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PostedDate",
                table: "PaymentVisit",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnAppliedAmount",
                table: "PaymentCheck",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OtherPatResp",
                table: "PaymentCharge",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PostedDate",
                table: "PaymentCharge",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "PatientPayment",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "VisitID",
                table: "PatientPayment",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OtherPatResp",
                table: "Charge",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientPayment_VisitID",
                table: "PatientPayment",
                column: "VisitID");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientPayment_Visit_VisitID",
                table: "PatientPayment",
                column: "VisitID",
                principalTable: "Visit",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientPayment_Visit_VisitID",
                table: "PatientPayment");

            migrationBuilder.DropIndex(
                name: "IX_PatientPayment_VisitID",
                table: "PatientPayment");

            migrationBuilder.DropColumn(
                name: "CopayPaid",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "OtherPatResp",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "PostedDate",
                table: "PaymentVisit");

            migrationBuilder.DropColumn(
                name: "UnAppliedAmount",
                table: "PaymentCheck");

            migrationBuilder.DropColumn(
                name: "OtherPatResp",
                table: "PaymentCharge");

            migrationBuilder.DropColumn(
                name: "PostedDate",
                table: "PaymentCharge");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "PatientPayment");

            migrationBuilder.DropColumn(
                name: "VisitID",
                table: "PatientPayment");

            migrationBuilder.DropColumn(
                name: "OtherPatResp",
                table: "Charge");
        }
    }
}

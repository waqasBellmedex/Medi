using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddIsReversalAppliedInVisitnCharge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReversalApplied",
                table: "Visit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AuthRequired",
                table: "PatientPlan",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AuthorizationDate",
                table: "PatientAuthorization",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RemindBeforeDays",
                table: "PatientAuthorization",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RemindBeforeRemainingVisits",
                table: "PatientAuthorization",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsReversalApplied",
                table: "Charge",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReversalApplied",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "AuthRequired",
                table: "PatientPlan");

            migrationBuilder.DropColumn(
                name: "AuthorizationDate",
                table: "PatientAuthorization");

            migrationBuilder.DropColumn(
                name: "RemindBeforeDays",
                table: "PatientAuthorization");

            migrationBuilder.DropColumn(
                name: "RemindBeforeRemainingVisits",
                table: "PatientAuthorization");

            migrationBuilder.DropColumn(
                name: "IsReversalApplied",
                table: "Charge");
        }
    }
}

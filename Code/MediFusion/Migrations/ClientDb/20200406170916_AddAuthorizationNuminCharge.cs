using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddAuthorizationNuminCharge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Statement1SentDate",
                table: "PatientFollowUpCharge",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Statement2SentDate",
                table: "PatientFollowUpCharge",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Statement3SentDate",
                table: "PatientFollowUpCharge",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorizationNum",
                table: "Charge",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Statement1SentDate",
                table: "PatientFollowUpCharge");

            migrationBuilder.DropColumn(
                name: "Statement2SentDate",
                table: "PatientFollowUpCharge");

            migrationBuilder.DropColumn(
                name: "Statement3SentDate",
                table: "PatientFollowUpCharge");

            migrationBuilder.DropColumn(
                name: "AuthorizationNum",
                table: "Charge");
        }
    }
}

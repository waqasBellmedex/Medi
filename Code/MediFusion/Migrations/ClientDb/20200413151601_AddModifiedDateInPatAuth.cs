using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddModifiedDateInPatAuth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "PatientAuthorizationUsed");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "PatientAuthorization",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "PatientAuthorization");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "PatientAuthorizationUsed",
                nullable: true);
        }
    }
}

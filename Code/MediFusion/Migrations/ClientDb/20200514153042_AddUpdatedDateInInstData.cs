using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddUpdatedDateInInstData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddedBy",
                table: "InstitutionalData",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedDate",
                table: "InstitutionalData",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "InstitutionalData",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "InstitutionalData",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedBy",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "AddedDate",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "InstitutionalData");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "InstitutionalData");
        }
    }
}

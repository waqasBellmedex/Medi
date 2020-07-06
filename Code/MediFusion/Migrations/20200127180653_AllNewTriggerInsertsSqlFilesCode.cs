using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations
{
    public partial class AllNewTriggerInsertsSqlFilesCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastNewInsertsModifiedDate",
                table: "MainClient",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastNewTrigerModifiedDate",
                table: "MainClient",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastNewInsertsModifiedDate",
                table: "MainClient");

            migrationBuilder.DropColumn(
                name: "LastNewTrigerModifiedDate",
                table: "MainClient");
        }
    }
}

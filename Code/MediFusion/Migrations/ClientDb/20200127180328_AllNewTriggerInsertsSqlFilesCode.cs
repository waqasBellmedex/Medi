using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AllNewTriggerInsertsSqlFilesCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastNewInsertsModifiedDate",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastNewTrigerModifiedDate",
                table: "Client",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastNewInsertsModifiedDate",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "LastNewTrigerModifiedDate",
                table: "Client");
        }
    }
}

﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddIsRegularRecordInExternalpATIENT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRegularRecord",
                table: "ExternalPatient",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRegularRecord",
                table: "ExternalPatient");
        }
    }
}

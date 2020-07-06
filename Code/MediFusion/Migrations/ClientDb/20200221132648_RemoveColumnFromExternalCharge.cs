using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class RemoveColumnFromExternalCharge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalCharge_Location_OfficeID",
                table: "ExternalCharge");

            migrationBuilder.DropIndex(
                name: "IX_ExternalCharge_OfficeID",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "OfficeID",
                table: "ExternalCharge");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OfficeID",
                table: "ExternalCharge",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCharge_OfficeID",
                table: "ExternalCharge",
                column: "OfficeID");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalCharge_Location_OfficeID",
                table: "ExternalCharge",
                column: "OfficeID",
                principalTable: "Location",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

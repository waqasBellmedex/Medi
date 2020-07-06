using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddAttentindProviderInCharge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AttendingProviderID",
                table: "Charge",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Charge_AttendingProviderID",
                table: "Charge",
                column: "AttendingProviderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Charge_Provider_AttendingProviderID",
                table: "Charge",
                column: "AttendingProviderID",
                principalTable: "Provider",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Charge_Provider_AttendingProviderID",
                table: "Charge");

            migrationBuilder.DropIndex(
                name: "IX_Charge_AttendingProviderID",
                table: "Charge");

            migrationBuilder.DropColumn(
                name: "AttendingProviderID",
                table: "Charge");
        }
    }
}

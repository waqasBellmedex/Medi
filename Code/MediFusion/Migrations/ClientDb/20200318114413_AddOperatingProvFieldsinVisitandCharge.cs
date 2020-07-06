using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddOperatingProvFieldsinVisitandCharge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AttendingProviderID",
                table: "Visit",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OperatingProviderID",
                table: "Visit",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RevenueCodeID",
                table: "Charge",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Visit_AttendingProviderID",
                table: "Visit",
                column: "AttendingProviderID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_OperatingProviderID",
                table: "Visit",
                column: "OperatingProviderID");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_RevenueCodeID",
                table: "Charge",
                column: "RevenueCodeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Charge_RevenueCode_RevenueCodeID",
                table: "Charge",
                column: "RevenueCodeID",
                principalTable: "RevenueCode",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Visit_Provider_AttendingProviderID",
                table: "Visit",
                column: "AttendingProviderID",
                principalTable: "Provider",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Visit_Provider_OperatingProviderID",
                table: "Visit",
                column: "OperatingProviderID",
                principalTable: "Provider",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Charge_RevenueCode_RevenueCodeID",
                table: "Charge");

            migrationBuilder.DropForeignKey(
                name: "FK_Visit_Provider_AttendingProviderID",
                table: "Visit");

            migrationBuilder.DropForeignKey(
                name: "FK_Visit_Provider_OperatingProviderID",
                table: "Visit");

            migrationBuilder.DropIndex(
                name: "IX_Visit_AttendingProviderID",
                table: "Visit");

            migrationBuilder.DropIndex(
                name: "IX_Visit_OperatingProviderID",
                table: "Visit");

            migrationBuilder.DropIndex(
                name: "IX_Charge_RevenueCodeID",
                table: "Charge");

            migrationBuilder.DropColumn(
                name: "AttendingProviderID",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "OperatingProviderID",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "RevenueCodeID",
                table: "Charge");
        }
    }
}

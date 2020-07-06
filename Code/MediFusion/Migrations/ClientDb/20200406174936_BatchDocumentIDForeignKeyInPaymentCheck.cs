using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class BatchDocumentIDForeignKeyInPaymentCheck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BatchDocumentID",
                table: "PaymentCheck",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AuthorizedAmount",
                table: "PatientAuthorization",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MedicalNecessityRequired",
                table: "PatientAuthorization",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MedicalRecordRequired",
                table: "PatientAuthorization",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCheck_BatchDocumentID",
                table: "PaymentCheck",
                column: "BatchDocumentID");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentCheck_BatchDocument_BatchDocumentID",
                table: "PaymentCheck",
                column: "BatchDocumentID",
                principalTable: "BatchDocument",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentCheck_BatchDocument_BatchDocumentID",
                table: "PaymentCheck");

            migrationBuilder.DropIndex(
                name: "IX_PaymentCheck_BatchDocumentID",
                table: "PaymentCheck");

            migrationBuilder.DropColumn(
                name: "BatchDocumentID",
                table: "PaymentCheck");

            migrationBuilder.DropColumn(
                name: "AuthorizedAmount",
                table: "PatientAuthorization");

            migrationBuilder.DropColumn(
                name: "MedicalNecessityRequired",
                table: "PatientAuthorization");

            migrationBuilder.DropColumn(
                name: "MedicalRecordRequired",
                table: "PatientAuthorization");
        }
    }
}

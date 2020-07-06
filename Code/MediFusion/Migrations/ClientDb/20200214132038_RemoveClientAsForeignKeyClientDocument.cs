using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class RemoveClientAsForeignKeyClientDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientDocument_Client_ClientID",
                table: "ClientDocument");

            migrationBuilder.DropIndex(
                name: "IX_ClientDocument_ClientID",
                table: "ClientDocument");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ClientDocument_ClientID",
                table: "ClientDocument",
                column: "ClientID");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientDocument_Client_ClientID",
                table: "ClientDocument",
                column: "ClientID",
                principalTable: "Client",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

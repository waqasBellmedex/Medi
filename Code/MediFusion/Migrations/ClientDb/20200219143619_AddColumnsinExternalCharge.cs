using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddColumnsinExternalCharge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Modifier1Amount",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "Modifier2Amount",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "Modifier3Amount",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "Modifier4Amount",
                table: "ExternalCharge");

            migrationBuilder.RenameColumn(
                name: "CPTCode",
                table: "ExternalCharge",
                newName: "CptCode");

            migrationBuilder.RenameColumn(
                name: "Provider",
                table: "ExternalCharge",
                newName: "ProviderName");

            migrationBuilder.RenameColumn(
                name: "Office",
                table: "ExternalCharge",
                newName: "OfficeName");

            migrationBuilder.AlterColumn<long>(
                name: "Insurance",
                table: "ExternalCharge",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CPTID",
                table: "ExternalCharge",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "GroupID",
                table: "ExternalCharge",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Modifier1Code",
                table: "ExternalCharge",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier2Code",
                table: "ExternalCharge",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier3Code",
                table: "ExternalCharge",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier4Code",
                table: "ExternalCharge",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OfficeID",
                table: "ExternalCharge",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "POSID",
                table: "ExternalCharge",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProviderID",
                table: "ExternalCharge",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCharge_CPTID",
                table: "ExternalCharge",
                column: "CPTID");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCharge_OfficeID",
                table: "ExternalCharge",
                column: "OfficeID");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCharge_ProviderID",
                table: "ExternalCharge",
                column: "ProviderID");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalCharge_Cpt_CPTID",
                table: "ExternalCharge",
                column: "CPTID",
                principalTable: "Cpt",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalCharge_Location_OfficeID",
                table: "ExternalCharge",
                column: "OfficeID",
                principalTable: "Location",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalCharge_Provider_ProviderID",
                table: "ExternalCharge",
                column: "ProviderID",
                principalTable: "Provider",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalCharge_Cpt_CPTID",
                table: "ExternalCharge");

            migrationBuilder.DropForeignKey(
                name: "FK_ExternalCharge_Location_OfficeID",
                table: "ExternalCharge");

            migrationBuilder.DropForeignKey(
                name: "FK_ExternalCharge_Provider_ProviderID",
                table: "ExternalCharge");

            migrationBuilder.DropIndex(
                name: "IX_ExternalCharge_CPTID",
                table: "ExternalCharge");

            migrationBuilder.DropIndex(
                name: "IX_ExternalCharge_OfficeID",
                table: "ExternalCharge");

            migrationBuilder.DropIndex(
                name: "IX_ExternalCharge_ProviderID",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "CPTID",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "GroupID",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "Modifier1Code",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "Modifier2Code",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "Modifier3Code",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "Modifier4Code",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "OfficeID",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "POSID",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "ProviderID",
                table: "ExternalCharge");

            migrationBuilder.RenameColumn(
                name: "CptCode",
                table: "ExternalCharge",
                newName: "CPTCode");

            migrationBuilder.RenameColumn(
                name: "ProviderName",
                table: "ExternalCharge",
                newName: "Provider");

            migrationBuilder.RenameColumn(
                name: "OfficeName",
                table: "ExternalCharge",
                newName: "Office");

            migrationBuilder.AlterColumn<string>(
                name: "Insurance",
                table: "ExternalCharge",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Modifier1Amount",
                table: "ExternalCharge",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Modifier2Amount",
                table: "ExternalCharge",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Modifier3Amount",
                table: "ExternalCharge",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Modifier4Amount",
                table: "ExternalCharge",
                nullable: true);
        }
    }
}

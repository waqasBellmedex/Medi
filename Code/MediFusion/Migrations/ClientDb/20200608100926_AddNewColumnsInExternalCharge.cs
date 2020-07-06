using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddNewColumnsInExternalCharge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DXCode1",
                table: "ExternalCharge",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DXCode2",
                table: "ExternalCharge",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DXCode3",
                table: "ExternalCharge",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DXCode4",
                table: "ExternalCharge",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DXCode5",
                table: "ExternalCharge",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DXCode6",
                table: "ExternalCharge",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleInitial",
                table: "ExternalCharge",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PrimaryBal",
                table: "ExternalCharge",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryInsuranceName",
                table: "ExternalCharge",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VisitPOSCode",
                table: "ExternalCharge",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DXCode1",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "DXCode2",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "DXCode3",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "DXCode4",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "DXCode5",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "DXCode6",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "MiddleInitial",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "PrimaryBal",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "SecondaryInsuranceName",
                table: "ExternalCharge");

            migrationBuilder.DropColumn(
                name: "VisitPOSCode",
                table: "ExternalCharge");
        }
    }
}

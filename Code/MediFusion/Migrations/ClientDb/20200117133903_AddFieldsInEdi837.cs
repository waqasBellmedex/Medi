using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddFieldsInEdi837 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ERA",
                table: "Edi837Payer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Enrollment",
                table: "Edi837Payer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Secondary",
                table: "Edi837Payer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Edi837Payer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "notes",
                table: "Edi837Payer",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ERA",
                table: "Edi837Payer");

            migrationBuilder.DropColumn(
                name: "Enrollment",
                table: "Edi837Payer");

            migrationBuilder.DropColumn(
                name: "Secondary",
                table: "Edi837Payer");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Edi837Payer");

            migrationBuilder.DropColumn(
                name: "notes",
                table: "Edi837Payer");
        }
    }
}

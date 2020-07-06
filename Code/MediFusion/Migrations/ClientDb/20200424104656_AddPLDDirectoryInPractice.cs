using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddPLDDirectoryInPractice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PLDDirectory",
                table: "Practice",
                type: "varchar(50)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PLDDirectory",
                table: "Practice");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations
{
    public partial class AddIsDeactivatedFieldInMainCLient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeactivated",
                table: "MainClient",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeactivated",
                table: "MainClient");
        }
    }
}

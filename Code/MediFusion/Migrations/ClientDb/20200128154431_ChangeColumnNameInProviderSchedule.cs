using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class ChangeColumnNameInProviderSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "day",
                table: "ProviderSchedule",
                newName: "dayofWeek");

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "PaymentVisit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "PaymentVisit");

            migrationBuilder.RenameColumn(
                name: "dayofWeek",
                table: "ProviderSchedule",
                newName: "day");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddFieldsInBatchDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "NoOfDemographics",
                table: "BatchDocument",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NoOfDemographicsEntered",
                table: "BatchDocument",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponsibleParty",
                table: "BatchDocument",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoOfDemographics",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "NoOfDemographicsEntered",
                table: "BatchDocument");

            migrationBuilder.DropColumn(
                name: "ResponsibleParty",
                table: "BatchDocument");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations
{
    public partial class AddPracticeIDINExInsuranceMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                 name: "PracticeID",
                 table: "ExInsuranceMapping",
                 nullable: true);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                   name: "PracticeID",
                   table: "ExInsuranceMapping");

        }
    }
}

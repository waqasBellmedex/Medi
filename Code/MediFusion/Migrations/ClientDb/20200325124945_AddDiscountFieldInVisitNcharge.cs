using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddDiscountFieldInVisitNcharge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "Visit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrescribingMD",
                table: "Patient",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "Charge",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PracticeResponsibilities",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    PracticeID = table.Column<long>(nullable: true),
                    IsBillingCompanyResponsibility = table.Column<bool>(nullable: true),
                    IsClientCompanyResponsibility = table.Column<bool>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracticeResponsibilities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PracticeResponsibilities_Practice_PracticeID",
                        column: x => x.PracticeID,
                        principalTable: "Practice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PracticeResponsibilities_PracticeID",
                table: "PracticeResponsibilities",
                column: "PracticeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PracticeResponsibilities");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "PrescribingMD",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Charge");
        }
    }
}

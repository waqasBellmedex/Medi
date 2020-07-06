using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddOnlinePortals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OnlinePortals",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InsurancePlanID = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    URL = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlinePortals", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "OnlinePortalCredentials",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OnlinePortalsID = table.Column<long>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    PasswordExpiryDate = table.Column<DateTime>(nullable: false),
                    SercurityQ1 = table.Column<string>(nullable: true),
                    SecurityA1 = table.Column<string>(nullable: true),
                    SecurityQ2 = table.Column<string>(nullable: true),
                    SecurityA2 = table.Column<string>(nullable: true),
                    SecurityQ3 = table.Column<string>(nullable: true),
                    SecurityA3 = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlinePortalCredentials", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OnlinePortalCredentials_OnlinePortals_OnlinePortalsID",
                        column: x => x.OnlinePortalsID,
                        principalTable: "OnlinePortals",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OnlinePortalCredentials_OnlinePortalsID",
                table: "OnlinePortalCredentials",
                column: "OnlinePortalsID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OnlinePortalCredentials");

            migrationBuilder.DropTable(
                name: "OnlinePortals");
        }
    }
}

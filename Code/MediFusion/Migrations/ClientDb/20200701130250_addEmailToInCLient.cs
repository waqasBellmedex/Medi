using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class addEmailToInCLient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "parentAppointmentID",
                table: "PatientAppointment",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "EmailCC",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CC = table.Column<string>(nullable: true),
                    emailhistoryid = table.Column<long>(nullable: false),
                    addedby = table.Column<string>(nullable: true),
                    addeddate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailCC", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EmailHistory",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    sendfrom = table.Column<string>(nullable: true),
                    subject = table.Column<string>(nullable: true),
                    body = table.Column<string>(nullable: true),
                    practiceID = table.Column<long>(nullable: false),
                    addedBy = table.Column<string>(nullable: true),
                    addeddate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailHistory", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EmailAttachments",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    attachmenturl = table.Column<string>(nullable: true),
                    attachmentname = table.Column<string>(nullable: true),
                    emailhistoryid = table.Column<long>(nullable: true),
                    addedby = table.Column<string>(nullable: true),
                    addeddate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailAttachments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EmailAttachments_EmailHistory_emailhistoryid",
                        column: x => x.emailhistoryid,
                        principalTable: "EmailHistory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmailTo",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    sendto = table.Column<string>(nullable: true),
                    emailhistoryid = table.Column<long>(nullable: true),
                    addedby = table.Column<string>(nullable: true),
                    addeddate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTo", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EmailTo_EmailHistory_emailhistoryid",
                        column: x => x.emailhistoryid,
                        principalTable: "EmailHistory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailAttachments_emailhistoryid",
                table: "EmailAttachments",
                column: "emailhistoryid");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTo_emailhistoryid",
                table: "EmailTo",
                column: "emailhistoryid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailAttachments");

            migrationBuilder.DropTable(
                name: "EmailCC");

            migrationBuilder.DropTable(
                name: "EmailTo");

            migrationBuilder.DropTable(
                name: "EmailHistory");

            migrationBuilder.DropColumn(
                name: "parentAppointmentID",
                table: "PatientAppointment");
        }
    }
}

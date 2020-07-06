using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class SMSSerciceChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "googleCalenderSecret",
                table: "Practice",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isGoogleCalenderEnable",
                table: "Practice",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "SMSSentReceived",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    patientID = table.Column<long>(nullable: true),
                    patientAppointmentID = table.Column<long>(nullable: true),
                    clientID = table.Column<long>(nullable: true),
                    practiceID = table.Column<long>(nullable: true),
                    textSent = table.Column<string>(nullable: true),
                    textReceived = table.Column<string>(nullable: true),
                    response = table.Column<string>(nullable: true),
                    sentFromNumber = table.Column<string>(nullable: true),
                    sentToNumber = table.Column<string>(nullable: true),
                    receivedFromNumber = table.Column<string>(nullable: true),
                    receivedToNumber = table.Column<string>(nullable: true),
                    sentBy = table.Column<string>(nullable: true),
                    sentDate = table.Column<DateTime>(nullable: true),
                    ReceivedBy = table.Column<string>(nullable: true),
                    ReceivedDate = table.Column<DateTime>(nullable: true),
                    sentMessageApiReply = table.Column<string>(nullable: true),
                    apiId = table.Column<string>(nullable: true),
                    messageUuid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMSSentReceived", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SMSSentReceived");

            migrationBuilder.DropColumn(
                name: "googleCalenderSecret",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "isGoogleCalenderEnable",
                table: "Practice");
        }
    }
}

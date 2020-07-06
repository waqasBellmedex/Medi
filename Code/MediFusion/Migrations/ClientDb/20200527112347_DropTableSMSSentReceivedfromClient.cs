using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class DropTableSMSSentReceivedfromClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SMSSentReceived");

            migrationBuilder.CreateTable(
                name: "PatientAppointmentsExternal",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    dataReceived = table.Column<string>(nullable: true),
                    addedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAppointmentsExternal", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientAppointmentsExternal");

            migrationBuilder.CreateTable(
                name: "SMSSentReceived",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ReceivedBy = table.Column<string>(nullable: true),
                    ReceivedDate = table.Column<DateTime>(nullable: true),
                    apiId = table.Column<string>(nullable: true),
                    clientID = table.Column<long>(nullable: true),
                    messageUuid = table.Column<string>(nullable: true),
                    patientAppointmentID = table.Column<long>(nullable: true),
                    patientID = table.Column<long>(nullable: true),
                    practiceID = table.Column<long>(nullable: true),
                    receivedFromNumber = table.Column<string>(nullable: true),
                    receivedToNumber = table.Column<string>(nullable: true),
                    response = table.Column<string>(nullable: true),
                    sentBy = table.Column<string>(nullable: true),
                    sentDate = table.Column<DateTime>(nullable: true),
                    sentFromNumber = table.Column<string>(nullable: true),
                    sentMessageApiReply = table.Column<string>(nullable: true),
                    sentToNumber = table.Column<string>(nullable: true),
                    textReceived = table.Column<string>(nullable: true),
                    textSent = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMSSentReceived", x => x.ID);
                });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations
{
    public partial class AddAutoDownloadingLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutoDownloadingLog",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ReceiverID = table.Column<long>(nullable: false),
                    PracticeID = table.Column<long>(nullable: false),
                    TotalDownloaded = table.Column<long>(nullable: false),
                    Files999 = table.Column<long>(nullable: false),
                    Files277 = table.Column<long>(nullable: false),
                    Files835 = table.Column<long>(nullable: false),
                    FilesZip = table.Column<long>(nullable: false),
                    FilesInsideZip = table.Column<long>(nullable: false),
                    FilesOther = table.Column<long>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    Files999Processed = table.Column<long>(nullable: false),
                    Files277Processed = table.Column<long>(nullable: false),
                    Files835Processed = table.Column<long>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoDownloadingLog", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoDownloadingLog");
        }
    }
}

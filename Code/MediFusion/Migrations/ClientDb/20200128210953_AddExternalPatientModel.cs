using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddExternalPatientModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalPatientID",
                table: "Patient",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExternalPatient",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExternalPatientID = table.Column<string>(nullable: true),
                    ProfilePic = table.Column<string>(nullable: true),
                    AccountNum = table.Column<string>(nullable: false),
                    AccountType = table.Column<string>(nullable: true),
                    PreferredLanguage = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    MergeStatus = table.Column<string>(nullable: true),
                    MedicalRecordNumber = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    MiddleInitial = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    SSN = table.Column<string>(maxLength: 100, nullable: true),
                    DOB = table.Column<DateTime>(type: "Date", nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    MaritalStatus = table.Column<string>(nullable: true),
                    Race = table.Column<string>(nullable: true),
                    Ethnicity = table.Column<string>(nullable: true),
                    Address1 = table.Column<string>(nullable: true),
                    Address2 = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    ZipCodeExtension = table.Column<string>(nullable: true),
                    MobileNumber = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PracticeID = table.Column<long>(nullable: true),
                    LocationId = table.Column<long>(nullable: true),
                    ProviderID = table.Column<long>(nullable: true),
                    RefProviderID = table.Column<long>(nullable: true),
                    BatchDocumentID = table.Column<long>(nullable: true),
                    PageNumber = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Notes = table.Column<string>(maxLength: 500, nullable: true),
                    Statement = table.Column<bool>(nullable: true),
                    StatementMessage = table.Column<string>(nullable: true),
                    RemainingDeductible = table.Column<decimal>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalPatient", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExternalPatient_BatchDocument_BatchDocumentID",
                        column: x => x.BatchDocumentID,
                        principalTable: "BatchDocument",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalPatient_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalPatient_Practice_PracticeID",
                        column: x => x.PracticeID,
                        principalTable: "Practice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalPatient_Provider_ProviderID",
                        column: x => x.ProviderID,
                        principalTable: "Provider",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalPatient_RefProvider_RefProviderID",
                        column: x => x.RefProviderID,
                        principalTable: "RefProvider",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalPatient_BatchDocumentID",
                table: "ExternalPatient",
                column: "BatchDocumentID");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalPatient_LocationId",
                table: "ExternalPatient",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalPatient_PracticeID",
                table: "ExternalPatient",
                column: "PracticeID");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalPatient_ProviderID",
                table: "ExternalPatient",
                column: "ProviderID");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalPatient_RefProviderID",
                table: "ExternalPatient",
                column: "RefProviderID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalPatient");

            migrationBuilder.DropColumn(
                name: "ExternalPatientID",
                table: "Patient");
        }
    }
}

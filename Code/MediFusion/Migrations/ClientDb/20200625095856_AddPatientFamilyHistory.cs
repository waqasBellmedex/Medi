using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddPatientFamilyHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentCategory",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    type = table.Column<string>(nullable: true),
                    ParentcategoryID = table.Column<long>(nullable: true),
                    inActive = table.Column<bool>(nullable: true),
                    practiceID = table.Column<long>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    url = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentCategory", x => x.id);
                    table.ForeignKey(
                        name: "FK_DocumentCategory_DocumentCategory_ParentcategoryID",
                        column: x => x.ParentcategoryID,
                        principalTable: "DocumentCategory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientFamilyHistory",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    desc = table.Column<string>(nullable: true),
                    relationship = table.Column<string>(nullable: true),
                    type = table.Column<string>(nullable: true),
                    status = table.Column<bool>(nullable: true),
                    dosage = table.Column<string>(nullable: true),
                    drugName = table.Column<string>(nullable: true),
                    startDate = table.Column<DateTime>(nullable: true),
                    endDate = table.Column<DateTime>(nullable: true),
                    ICDID = table.Column<long>(nullable: false),
                    patientNotesID = table.Column<long>(nullable: false),
                    patientID = table.Column<long>(nullable: false),
                    PracticeID = table.Column<long>(nullable: false),
                    inActive = table.Column<bool>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientFamilyHistory", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientFamilyHistory_ICD_ICDID",
                        column: x => x.ICDID,
                        principalTable: "ICD",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientFamilyHistory_Practice_PracticeID",
                        column: x => x.PracticeID,
                        principalTable: "Practice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientFamilyHistory_Patient_patientID",
                        column: x => x.patientID,
                        principalTable: "Patient",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientFamilyHistory_PatientNotes_patientNotesID",
                        column: x => x.patientNotesID,
                        principalTable: "PatientNotes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProblemList",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    type = table.Column<string>(nullable: true),
                    status = table.Column<bool>(nullable: true),
                    strartdate = table.Column<DateTime>(nullable: true),
                    endDate = table.Column<DateTime>(nullable: true),
                    ICDID = table.Column<long>(nullable: false),
                    patientNotesID = table.Column<long>(nullable: false),
                    patientID = table.Column<long>(nullable: false),
                    desc = table.Column<string>(nullable: true),
                    PracticeID = table.Column<long>(nullable: false),
                    inActive = table.Column<bool>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemList", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProblemList_ICD_ICDID",
                        column: x => x.ICDID,
                        principalTable: "ICD",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProblemList_Practice_PracticeID",
                        column: x => x.PracticeID,
                        principalTable: "Practice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProblemList_Patient_patientID",
                        column: x => x.patientID,
                        principalTable: "Patient",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProblemList_PatientNotes_patientNotesID",
                        column: x => x.patientNotesID,
                        principalTable: "PatientNotes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientDocument",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: true),
                    DocumentCategoryID = table.Column<long>(nullable: true),
                    PatientID = table.Column<long>(nullable: true),
                    size = table.Column<string>(nullable: true),
                    UploadedDate = table.Column<DateTime>(nullable: true),
                    uploadeBy = table.Column<string>(nullable: true),
                    documentDate = table.Column<DateTime>(nullable: true),
                    url = table.Column<string>(nullable: true),
                    notes = table.Column<string>(nullable: true),
                    inActive = table.Column<bool>(nullable: true),
                    practiceID = table.Column<long>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientDocument", x => x.id);
                    table.ForeignKey(
                        name: "FK_PatientDocument_DocumentCategory_DocumentCategoryID",
                        column: x => x.DocumentCategoryID,
                        principalTable: "DocumentCategory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientDocument_Patient_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patient",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentCategory_ParentcategoryID",
                table: "DocumentCategory",
                column: "ParentcategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDocument_DocumentCategoryID",
                table: "PatientDocument",
                column: "DocumentCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDocument_PatientID",
                table: "PatientDocument",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFamilyHistory_ICDID",
                table: "PatientFamilyHistory",
                column: "ICDID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFamilyHistory_PracticeID",
                table: "PatientFamilyHistory",
                column: "PracticeID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFamilyHistory_patientID",
                table: "PatientFamilyHistory",
                column: "patientID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFamilyHistory_patientNotesID",
                table: "PatientFamilyHistory",
                column: "patientNotesID");

            migrationBuilder.CreateIndex(
                name: "IX_ProblemList_ICDID",
                table: "ProblemList",
                column: "ICDID");

            migrationBuilder.CreateIndex(
                name: "IX_ProblemList_PracticeID",
                table: "ProblemList",
                column: "PracticeID");

            migrationBuilder.CreateIndex(
                name: "IX_ProblemList_patientID",
                table: "ProblemList",
                column: "patientID");

            migrationBuilder.CreateIndex(
                name: "IX_ProblemList_patientNotesID",
                table: "ProblemList",
                column: "patientNotesID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientDocument");

            migrationBuilder.DropTable(
                name: "PatientFamilyHistory");

            migrationBuilder.DropTable(
                name: "ProblemList");

            migrationBuilder.DropTable(
                name: "DocumentCategory");
        }
    }
}

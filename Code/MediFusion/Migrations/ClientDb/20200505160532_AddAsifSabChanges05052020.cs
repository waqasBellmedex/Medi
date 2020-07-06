using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddAsifSabChanges05052020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "PatientNotesId",
                table: "PatientVitals",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<decimal>(
                name: "amount",
                table: "PatientStatementChargeHistory",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "amount",
                table: "PatientStatement",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PracticeID",
                table: "PatientForms",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<string>(
                name: "CoSignatureUrl",
                table: "PatientForms",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CoSigned",
                table: "PatientForms",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoSignedBy",
                table: "PatientForms",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CoSignedDate",
                table: "PatientForms",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignatureUrl",
                table: "PatientForms",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Signed",
                table: "PatientForms",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignedBy",
                table: "PatientForms",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SignedDate",
                table: "PatientForms",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FormsSubHeading",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    clinicalFormsID = table.Column<long>(nullable: true),
                    subheading = table.Column<string>(nullable: true),
                    type = table.Column<string>(nullable: true),
                    defaultValue = table.Column<string>(nullable: true),
                    practiceID = table.Column<long>(nullable: true),
                    Inactive = table.Column<bool>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormsSubHeading", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FormsSubHeading_ClinicalForms_clinicalFormsID",
                        column: x => x.clinicalFormsID,
                        principalTable: "ClinicalForms",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormsSubHeading_Practice_practiceID",
                        column: x => x.practiceID,
                        principalTable: "Practice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientAllergy",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientNotesId = table.Column<long>(nullable: false),
                    AllergyType = table.Column<string>(nullable: true),
                    SpecificDrugAllergy = table.Column<string>(nullable: true),
                    Reaction = table.Column<string>(nullable: true),
                    Severity = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    PracticeID = table.Column<decimal>(nullable: false),
                    Inactive = table.Column<bool>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAllergy", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PatientFormValue",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    patientFormID = table.Column<long>(nullable: true),
                    PatientFormsID = table.Column<long>(nullable: true),
                    patientNotesID = table.Column<long>(nullable: true),
                    clinicalFormsID = table.Column<long>(nullable: true),
                    formsSubHeadingID = table.Column<long>(nullable: true),
                    value = table.Column<string>(nullable: true),
                    Inactive = table.Column<bool>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientFormValue", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientFormValue_PatientForms_PatientFormsID",
                        column: x => x.PatientFormsID,
                        principalTable: "PatientForms",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientFormValue_ClinicalForms_clinicalFormsID",
                        column: x => x.clinicalFormsID,
                        principalTable: "ClinicalForms",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientFormValue_FormsSubHeading_formsSubHeadingID",
                        column: x => x.formsSubHeadingID,
                        principalTable: "FormsSubHeading",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientFormValue_PatientNotes_patientNotesID",
                        column: x => x.patientNotesID,
                        principalTable: "PatientNotes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientVitals_PatientNotesId",
                table: "PatientVitals",
                column: "PatientNotesId");

            migrationBuilder.CreateIndex(
                name: "IX_FormsSubHeading_clinicalFormsID",
                table: "FormsSubHeading",
                column: "clinicalFormsID");

            migrationBuilder.CreateIndex(
                name: "IX_FormsSubHeading_practiceID",
                table: "FormsSubHeading",
                column: "practiceID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFormValue_PatientFormsID",
                table: "PatientFormValue",
                column: "PatientFormsID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFormValue_clinicalFormsID",
                table: "PatientFormValue",
                column: "clinicalFormsID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFormValue_formsSubHeadingID",
                table: "PatientFormValue",
                column: "formsSubHeadingID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFormValue_patientNotesID",
                table: "PatientFormValue",
                column: "patientNotesID");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientVitals_PatientNotes_PatientNotesId",
                table: "PatientVitals",
                column: "PatientNotesId",
                principalTable: "PatientNotes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientVitals_PatientNotes_PatientNotesId",
                table: "PatientVitals");

            migrationBuilder.DropTable(
                name: "PatientAllergy");

            migrationBuilder.DropTable(
                name: "PatientFormValue");

            migrationBuilder.DropTable(
                name: "FormsSubHeading");

            migrationBuilder.DropIndex(
                name: "IX_PatientVitals_PatientNotesId",
                table: "PatientVitals");

            migrationBuilder.DropColumn(
                name: "amount",
                table: "PatientStatementChargeHistory");

            migrationBuilder.DropColumn(
                name: "amount",
                table: "PatientStatement");

            migrationBuilder.DropColumn(
                name: "CoSignatureUrl",
                table: "PatientForms");

            migrationBuilder.DropColumn(
                name: "CoSigned",
                table: "PatientForms");

            migrationBuilder.DropColumn(
                name: "CoSignedBy",
                table: "PatientForms");

            migrationBuilder.DropColumn(
                name: "CoSignedDate",
                table: "PatientForms");

            migrationBuilder.DropColumn(
                name: "SignatureUrl",
                table: "PatientForms");

            migrationBuilder.DropColumn(
                name: "Signed",
                table: "PatientForms");

            migrationBuilder.DropColumn(
                name: "SignedBy",
                table: "PatientForms");

            migrationBuilder.DropColumn(
                name: "SignedDate",
                table: "PatientForms");

            migrationBuilder.AlterColumn<long>(
                name: "PatientNotesId",
                table: "PatientVitals",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PracticeID",
                table: "PatientForms",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}

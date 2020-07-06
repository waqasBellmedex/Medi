using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class MergeAsifSabChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientAppointment_Patient_PatientID",
                table: "PatientAppointment");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientAppointment_ProviderSlot_ProviderSlotID",
                table: "PatientAppointment");

            migrationBuilder.DropIndex(
                name: "IX_PatientAppointment_ProviderSlotID",
                table: "PatientAppointment");

            migrationBuilder.RenameColumn(
                name: "ProviderSlotID",
                table: "PatientAppointment",
                newName: "VisitID");

            migrationBuilder.AlterColumn<int>(
                name: "VisitInterval",
                table: "PatientAppointment",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PatientID",
                table: "PatientAppointment",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Inactive",
                table: "PatientAppointment",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RoomID",
                table: "PatientAppointment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "color",
                table: "PatientAppointment",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppointmentCPT",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppointmentID = table.Column<long>(nullable: false),
                    CPTID = table.Column<long>(nullable: false),
                    Modifier1 = table.Column<string>(nullable: true),
                    Modifier2 = table.Column<string>(nullable: true),
                    NdcUnits = table.Column<string>(nullable: true),
                    Units = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    TotalAmount = table.Column<decimal>(nullable: false),
                    PracticeID = table.Column<long>(nullable: false),
                    Inactive = table.Column<bool>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentCPT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentICD",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppointmentID = table.Column<long>(nullable: false),
                    ICDID = table.Column<long>(nullable: false),
                    SerialNo = table.Column<int>(nullable: false),
                    PracticeID = table.Column<long>(nullable: false),
                    Inactive = table.Column<bool>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentICD", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ClinicalForms",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    ProviderID = table.Column<long>(nullable: false),
                    PracticeID = table.Column<long>(nullable: false),
                    Inactive = table.Column<bool>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicalForms", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CPTMostFavourite",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(nullable: true),
                    ProviderID = table.Column<long>(nullable: true),
                    CPTID = table.Column<long>(nullable: false),
                    VisitReasonID = table.Column<long>(nullable: false),
                    PracticeID = table.Column<long>(nullable: true),
                    Inactive = table.Column<bool>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPTMostFavourite", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GeneralItems",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<long>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    Inactive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralItems", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ICDMostFavourite",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(nullable: true),
                    ProviderID = table.Column<long>(nullable: true),
                    ICDID = table.Column<long>(nullable: false),
                    VisitReasonID = table.Column<long>(nullable: false),
                    PracticeID = table.Column<long>(nullable: true),
                    Inactive = table.Column<bool>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ICDMostFavourite", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PatientForms",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientAppointmentID = table.Column<long>(nullable: true),
                    PatientID = table.Column<long>(nullable: true),
                    ClinicalFormID = table.Column<long>(nullable: true),
                    PracticeID = table.Column<long>(nullable: false),
                    Inactive = table.Column<bool>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientForms", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PatientMedicalNotes",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientNotesId = table.Column<long>(nullable: false),
                    note = table.Column<string>(nullable: true),
                    note_html = table.Column<string>(nullable: true),
                    Inactive = table.Column<bool>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientMedicalNotes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PatientNotes",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientID = table.Column<long>(nullable: false),
                    DOS = table.Column<DateTime>(nullable: false),
                    ProviderID = table.Column<long>(nullable: false),
                    LocationID = table.Column<long>(nullable: false),
                    AppointmentID = table.Column<long>(nullable: false),
                    DocumentID = table.Column<long>(nullable: false),
                    DocumentSize = table.Column<int>(nullable: false),
                    Signed = table.Column<string>(nullable: true),
                    SignedBy = table.Column<string>(nullable: true),
                    SignedDate = table.Column<DateTime>(nullable: true),
                    CoSignedBy = table.Column<string>(nullable: true),
                    CoSignedDate = table.Column<DateTime>(nullable: true),
                    PracticeID = table.Column<long>(nullable: false),
                    Inactive = table.Column<bool>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientNotes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PatientVitals",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientNotesId = table.Column<long>(nullable: false),
                    Height_foot = table.Column<decimal>(nullable: false),
                    Height_inch = table.Column<decimal>(nullable: false),
                    Weight_lbs = table.Column<decimal>(nullable: false),
                    Weight_pounds = table.Column<decimal>(nullable: false),
                    BMI = table.Column<decimal>(nullable: false),
                    BPSystolic = table.Column<decimal>(nullable: false),
                    BPDiastolic = table.Column<decimal>(nullable: false),
                    Temperature = table.Column<decimal>(nullable: false),
                    Pulse = table.Column<decimal>(nullable: false),
                    Respiratory_rate = table.Column<decimal>(nullable: false),
                    OxygenSaturation = table.Column<decimal>(nullable: false),
                    Pain = table.Column<int>(nullable: false),
                    HeadCircumference = table.Column<decimal>(nullable: false),
                    PracticeID = table.Column<decimal>(nullable: false),
                    Inactive = table.Column<bool>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientVitals", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PracticeID = table.Column<long>(nullable: true),
                    ProviderID = table.Column<long>(nullable: true),
                    Inactive = table.Column<bool>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ClinicalFormsCPT",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClinicalFormID = table.Column<long>(nullable: false),
                    CPTID = table.Column<long>(nullable: false),
                    Modifier = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    PracticeID = table.Column<long>(nullable: true),
                    Inactive = table.Column<bool>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicalFormsCPT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClinicalFormsCPT_ClinicalForms_ClinicalFormID",
                        column: x => x.ClinicalFormID,
                        principalTable: "ClinicalForms",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClinicalFormsCPT_Practice_PracticeID",
                        column: x => x.PracticeID,
                        principalTable: "Practice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClinicalFormsCPT_ClinicalFormID",
                table: "ClinicalFormsCPT",
                column: "ClinicalFormID");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicalFormsCPT_PracticeID",
                table: "ClinicalFormsCPT",
                column: "PracticeID");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAppointment_Patient_PatientID",
                table: "PatientAppointment",
                column: "PatientID",
                principalTable: "Patient",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientAppointment_Patient_PatientID",
                table: "PatientAppointment");

            migrationBuilder.DropTable(
                name: "AppointmentCPT");

            migrationBuilder.DropTable(
                name: "AppointmentICD");

            migrationBuilder.DropTable(
                name: "ClinicalFormsCPT");

            migrationBuilder.DropTable(
                name: "CPTMostFavourite");

            migrationBuilder.DropTable(
                name: "GeneralItems");

            migrationBuilder.DropTable(
                name: "ICDMostFavourite");

            migrationBuilder.DropTable(
                name: "PatientForms");

            migrationBuilder.DropTable(
                name: "PatientMedicalNotes");

            migrationBuilder.DropTable(
                name: "PatientNotes");

            migrationBuilder.DropTable(
                name: "PatientVitals");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "ClinicalForms");

            migrationBuilder.DropColumn(
                name: "Inactive",
                table: "PatientAppointment");

            migrationBuilder.DropColumn(
                name: "RoomID",
                table: "PatientAppointment");

            migrationBuilder.DropColumn(
                name: "color",
                table: "PatientAppointment");

            migrationBuilder.RenameColumn(
                name: "VisitID",
                table: "PatientAppointment",
                newName: "ProviderSlotID");

            migrationBuilder.AlterColumn<string>(
                name: "VisitInterval",
                table: "PatientAppointment",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PatientID",
                table: "PatientAppointment",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.CreateIndex(
                name: "IX_PatientAppointment_ProviderSlotID",
                table: "PatientAppointment",
                column: "ProviderSlotID");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAppointment_Patient_PatientID",
                table: "PatientAppointment",
                column: "PatientID",
                principalTable: "Patient",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAppointment_ProviderSlot_ProviderSlotID",
                table: "PatientAppointment",
                column: "ProviderSlotID",
                principalTable: "ProviderSlot",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class AddInstitutionalData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InstitutionalData",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false),
                    StatementFromDate = table.Column<DateTime>(nullable: true),
                    StatementToDate = table.Column<DateTime>(nullable: true),
                    PatientStatusCodeID = table.Column<long>(nullable: true),
                    ReasonOfVisitID = table.Column<long>(nullable: true),
                    VisitReasonID = table.Column<long>(nullable: true),
                    AdmissionDate = table.Column<DateTime>(nullable: true),
                    AdmissionHour = table.Column<DateTime>(nullable: true),
                    AdmissionType = table.Column<string>(nullable: true),
                    AdmissionSourceID = table.Column<long>(nullable: true),
                    AdmissionSourceCodeID = table.Column<long>(nullable: true),
                    DischargeDate = table.Column<DateTime>(nullable: true),
                    PrincipalCodeID = table.Column<long>(nullable: true),
                    ICDID = table.Column<long>(nullable: true),
                    AdmittingCodeID = table.Column<long>(nullable: true),
                    ICD1ID = table.Column<long>(nullable: true),
                    ExternalInjuryCode1 = table.Column<long>(nullable: true),
                    ExternalInjury1ID = table.Column<long>(nullable: true),
                    ExternalInjuryCode2 = table.Column<long>(nullable: true),
                    ExternalInjury2ID = table.Column<long>(nullable: true),
                    ExternalInjuryCode3 = table.Column<long>(nullable: true),
                    ExternalInjury3ID = table.Column<long>(nullable: true),
                    PrincipalProcedureCodeID1 = table.Column<long>(nullable: true),
                    CptID = table.Column<long>(nullable: true),
                    PrincipalProcedureDate1 = table.Column<DateTime>(nullable: true),
                    PrincipalProcedureCodeID2 = table.Column<long>(nullable: true),
                    Cpt1ID = table.Column<long>(nullable: true),
                    PrincipalProcedureDate2 = table.Column<DateTime>(nullable: true),
                    PrincipalProcedureCodeID3 = table.Column<long>(nullable: true),
                    Cpt2ID = table.Column<long>(nullable: true),
                    PrincipalProcedureDate3 = table.Column<DateTime>(nullable: true),
                    PrincipalProcedureCodeID4 = table.Column<long>(nullable: true),
                    Cpt3ID = table.Column<long>(nullable: true),
                    PrincipalProcedureDate4 = table.Column<DateTime>(nullable: true),
                    PrincipalProcedureCodeID5 = table.Column<long>(nullable: true),
                    Cpt4ID = table.Column<long>(nullable: true),
                    PrincipalProcedureDate5 = table.Column<DateTime>(nullable: true),
                    PrincipalProcedureCodeID6 = table.Column<long>(nullable: true),
                    Cpt5ID = table.Column<long>(nullable: true),
                    PrincipalProcedureDate6 = table.Column<DateTime>(nullable: true),
                    ValueCodeID1 = table.Column<long>(nullable: true),
                    ValueCode1ID = table.Column<long>(nullable: true),
                    ValueCodeID2 = table.Column<long>(nullable: true),
                    ValueCode2ID = table.Column<long>(nullable: true),
                    ValueCodeID3 = table.Column<long>(nullable: true),
                    ValueCode3ID = table.Column<long>(nullable: true),
                    ValueCodeID4 = table.Column<long>(nullable: true),
                    ValueCode4ID = table.Column<long>(nullable: true),
                    ValueCodeID5 = table.Column<long>(nullable: true),
                    ValueCode5ID = table.Column<long>(nullable: true),
                    ValueCodeID6 = table.Column<long>(nullable: true),
                    ValueCode6ID = table.Column<long>(nullable: true),
                    ValueCodeID7 = table.Column<long>(nullable: true),
                    ValueCode7ID = table.Column<long>(nullable: true),
                    ValueCodeID8 = table.Column<long>(nullable: true),
                    ValueCode8ID = table.Column<long>(nullable: true),
                    ValueCodeID9 = table.Column<long>(nullable: true),
                    ValueCode9ID = table.Column<long>(nullable: true),
                    ValueCodeID10 = table.Column<long>(nullable: true),
                    ValueCode10ID = table.Column<long>(nullable: true),
                    ValueCodeID11 = table.Column<long>(nullable: true),
                    ValueCode11ID = table.Column<long>(nullable: true),
                    ValueCodeID12 = table.Column<long>(nullable: true),
                    ValueCode12ID = table.Column<long>(nullable: true),
                    ConditionCodeID1 = table.Column<long>(nullable: true),
                    ConditionCode1ID = table.Column<long>(nullable: true),
                    ConditionCodeID2 = table.Column<long>(nullable: true),
                    ConditionCode2ID = table.Column<long>(nullable: true),
                    ConditionCodeID3 = table.Column<long>(nullable: true),
                    ConditionCode3ID = table.Column<long>(nullable: true),
                    ConditionCodeID4 = table.Column<long>(nullable: true),
                    ConditionCode4ID = table.Column<long>(nullable: true),
                    ConditionCodeID5 = table.Column<long>(nullable: true),
                    ConditionCode5ID = table.Column<long>(nullable: true),
                    ConditionCodeID6 = table.Column<long>(nullable: true),
                    ConditionCode6ID = table.Column<long>(nullable: true),
                    ConditionCodeID7 = table.Column<long>(nullable: true),
                    ConditionCode7ID = table.Column<long>(nullable: true),
                    ConditionCodeID8 = table.Column<long>(nullable: true),
                    ConditionCode8ID = table.Column<long>(nullable: true),
                    ConditionCodeID9 = table.Column<long>(nullable: true),
                    ConditionCode9ID = table.Column<long>(nullable: true),
                    ConditionCodeID10 = table.Column<long>(nullable: true),
                    ConditionCode10ID = table.Column<long>(nullable: true),
                    ConditionCodeID11 = table.Column<long>(nullable: true),
                    ConditionCode11ID = table.Column<long>(nullable: true),
                    ConditionCodeID12 = table.Column<long>(nullable: true),
                    ConditionCode12ID = table.Column<long>(nullable: true),
                    OccuranceSpanCodeID1 = table.Column<long>(nullable: true),
                    OccurrenceSpanCode1ID = table.Column<long>(nullable: true),
                    SpanCode1FromDate = table.Column<DateTime>(nullable: true),
                    SpanCode1ToDate = table.Column<DateTime>(nullable: true),
                    OccuranceSpanCodeID2 = table.Column<long>(nullable: true),
                    OccurrenceSpanCode2ID = table.Column<long>(nullable: true),
                    SpanCode2FromDate = table.Column<DateTime>(nullable: true),
                    SpanCode2ToDate = table.Column<DateTime>(nullable: true),
                    OccuranceSpanCodeID3 = table.Column<long>(nullable: true),
                    OccurrenceSpanCode3ID = table.Column<long>(nullable: true),
                    SpanCode3FromDate = table.Column<DateTime>(nullable: true),
                    SpanCode3ToDate = table.Column<DateTime>(nullable: true),
                    OccuranceSpanCodeID4 = table.Column<long>(nullable: true),
                    OccurrenceSpanCode4ID = table.Column<long>(nullable: true),
                    SpanCode4FromDate = table.Column<DateTime>(nullable: true),
                    SpanCode4ToDate = table.Column<DateTime>(nullable: true),
                    OccuranceCodeID1 = table.Column<long>(nullable: true),
                    OccurrenceCode1ID = table.Column<long>(nullable: true),
                    OccuranceCode1Date = table.Column<DateTime>(nullable: true),
                    OccuranceCodeID2 = table.Column<long>(nullable: true),
                    OccurrenceCode2ID = table.Column<long>(nullable: true),
                    OccuranceCode2Date = table.Column<DateTime>(nullable: true),
                    OccuranceCodeID3 = table.Column<long>(nullable: true),
                    OccurrenceCode3ID = table.Column<long>(nullable: true),
                    OccuranceCode3Date = table.Column<DateTime>(nullable: true),
                    OccuranceCodeID4 = table.Column<long>(nullable: true),
                    OccurrenceCode4ID = table.Column<long>(nullable: true),
                    OccuranceCode4Date = table.Column<DateTime>(nullable: true),
                    OccuranceCodeID5 = table.Column<long>(nullable: true),
                    OccurrenceCode5ID = table.Column<long>(nullable: true),
                    OccuranceCode5Date = table.Column<DateTime>(nullable: true),
                    OccuranceCodeID6 = table.Column<long>(nullable: true),
                    OccurrenceCode6ID = table.Column<long>(nullable: true),
                    OccuranceCode6Date = table.Column<DateTime>(nullable: true),
                    OccuranceCodeID7 = table.Column<long>(nullable: true),
                    OccurrenceCode7ID = table.Column<long>(nullable: true),
                    OccuranceCode7Date = table.Column<DateTime>(nullable: true),
                    OccuranceCodeID8 = table.Column<long>(nullable: true),
                    OccurrenceCode8ID = table.Column<long>(nullable: true),
                    OccuranceCode8Date = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionalData", x => x.ID);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_AdmissionSourceCode_AdmissionSourceCodeID",
                        column: x => x.AdmissionSourceCodeID,
                        principalTable: "AdmissionSourceCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ConditionCode_ConditionCode10ID",
                        column: x => x.ConditionCode10ID,
                        principalTable: "ConditionCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ConditionCode_ConditionCode11ID",
                        column: x => x.ConditionCode11ID,
                        principalTable: "ConditionCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ConditionCode_ConditionCode12ID",
                        column: x => x.ConditionCode12ID,
                        principalTable: "ConditionCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ConditionCode_ConditionCode1ID",
                        column: x => x.ConditionCode1ID,
                        principalTable: "ConditionCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ConditionCode_ConditionCode2ID",
                        column: x => x.ConditionCode2ID,
                        principalTable: "ConditionCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ConditionCode_ConditionCode3ID",
                        column: x => x.ConditionCode3ID,
                        principalTable: "ConditionCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ConditionCode_ConditionCode4ID",
                        column: x => x.ConditionCode4ID,
                        principalTable: "ConditionCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ConditionCode_ConditionCode5ID",
                        column: x => x.ConditionCode5ID,
                        principalTable: "ConditionCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ConditionCode_ConditionCode6ID",
                        column: x => x.ConditionCode6ID,
                        principalTable: "ConditionCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ConditionCode_ConditionCode7ID",
                        column: x => x.ConditionCode7ID,
                        principalTable: "ConditionCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ConditionCode_ConditionCode8ID",
                        column: x => x.ConditionCode8ID,
                        principalTable: "ConditionCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ConditionCode_ConditionCode9ID",
                        column: x => x.ConditionCode9ID,
                        principalTable: "ConditionCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_Cpt_Cpt1ID",
                        column: x => x.Cpt1ID,
                        principalTable: "Cpt",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_Cpt_Cpt2ID",
                        column: x => x.Cpt2ID,
                        principalTable: "Cpt",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_Cpt_Cpt3ID",
                        column: x => x.Cpt3ID,
                        principalTable: "Cpt",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_Cpt_Cpt4ID",
                        column: x => x.Cpt4ID,
                        principalTable: "Cpt",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_Cpt_Cpt5ID",
                        column: x => x.Cpt5ID,
                        principalTable: "Cpt",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_Cpt_CptID",
                        column: x => x.CptID,
                        principalTable: "Cpt",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ExternalInjuryCode_ExternalInjury1ID",
                        column: x => x.ExternalInjury1ID,
                        principalTable: "ExternalInjuryCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ExternalInjuryCode_ExternalInjury2ID",
                        column: x => x.ExternalInjury2ID,
                        principalTable: "ExternalInjuryCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ExternalInjuryCode_ExternalInjury3ID",
                        column: x => x.ExternalInjury3ID,
                        principalTable: "ExternalInjuryCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ICD_ICD1ID",
                        column: x => x.ICD1ID,
                        principalTable: "ICD",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ICD_ICDID",
                        column: x => x.ICDID,
                        principalTable: "ICD",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_Visit_ID",
                        column: x => x.ID,
                        principalTable: "Visit",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_OccurrenceCode_OccurrenceCode1ID",
                        column: x => x.OccurrenceCode1ID,
                        principalTable: "OccurrenceCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_OccurrenceCode_OccurrenceCode2ID",
                        column: x => x.OccurrenceCode2ID,
                        principalTable: "OccurrenceCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_OccurrenceCode_OccurrenceCode3ID",
                        column: x => x.OccurrenceCode3ID,
                        principalTable: "OccurrenceCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_OccurrenceCode_OccurrenceCode4ID",
                        column: x => x.OccurrenceCode4ID,
                        principalTable: "OccurrenceCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_OccurrenceCode_OccurrenceCode5ID",
                        column: x => x.OccurrenceCode5ID,
                        principalTable: "OccurrenceCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_OccurrenceCode_OccurrenceCode6ID",
                        column: x => x.OccurrenceCode6ID,
                        principalTable: "OccurrenceCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_OccurrenceCode_OccurrenceCode7ID",
                        column: x => x.OccurrenceCode7ID,
                        principalTable: "OccurrenceCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_OccurrenceCode_OccurrenceCode8ID",
                        column: x => x.OccurrenceCode8ID,
                        principalTable: "OccurrenceCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_OccurrenceSpanCode_OccurrenceSpanCode1ID",
                        column: x => x.OccurrenceSpanCode1ID,
                        principalTable: "OccurrenceSpanCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_OccurrenceSpanCode_OccurrenceSpanCode2ID",
                        column: x => x.OccurrenceSpanCode2ID,
                        principalTable: "OccurrenceSpanCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_OccurrenceSpanCode_OccurrenceSpanCode3ID",
                        column: x => x.OccurrenceSpanCode3ID,
                        principalTable: "OccurrenceSpanCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_OccurrenceSpanCode_OccurrenceSpanCode4ID",
                        column: x => x.OccurrenceSpanCode4ID,
                        principalTable: "OccurrenceSpanCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_PatientStatusCode_PatientStatusCodeID",
                        column: x => x.PatientStatusCodeID,
                        principalTable: "PatientStatusCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ValueCode_ValueCode10ID",
                        column: x => x.ValueCode10ID,
                        principalTable: "ValueCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ValueCode_ValueCode11ID",
                        column: x => x.ValueCode11ID,
                        principalTable: "ValueCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ValueCode_ValueCode12ID",
                        column: x => x.ValueCode12ID,
                        principalTable: "ValueCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ValueCode_ValueCode1ID",
                        column: x => x.ValueCode1ID,
                        principalTable: "ValueCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ValueCode_ValueCode2ID",
                        column: x => x.ValueCode2ID,
                        principalTable: "ValueCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ValueCode_ValueCode3ID",
                        column: x => x.ValueCode3ID,
                        principalTable: "ValueCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ValueCode_ValueCode4ID",
                        column: x => x.ValueCode4ID,
                        principalTable: "ValueCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ValueCode_ValueCode5ID",
                        column: x => x.ValueCode5ID,
                        principalTable: "ValueCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ValueCode_ValueCode6ID",
                        column: x => x.ValueCode6ID,
                        principalTable: "ValueCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ValueCode_ValueCode7ID",
                        column: x => x.ValueCode7ID,
                        principalTable: "ValueCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ValueCode_ValueCode8ID",
                        column: x => x.ValueCode8ID,
                        principalTable: "ValueCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_ValueCode_ValueCode9ID",
                        column: x => x.ValueCode9ID,
                        principalTable: "ValueCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstitutionalData_VisitReason_VisitReasonID",
                        column: x => x.VisitReasonID,
                        principalTable: "VisitReason",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_AdmissionSourceCodeID",
                table: "InstitutionalData",
                column: "AdmissionSourceCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ConditionCode10ID",
                table: "InstitutionalData",
                column: "ConditionCode10ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ConditionCode11ID",
                table: "InstitutionalData",
                column: "ConditionCode11ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ConditionCode12ID",
                table: "InstitutionalData",
                column: "ConditionCode12ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ConditionCode1ID",
                table: "InstitutionalData",
                column: "ConditionCode1ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ConditionCode2ID",
                table: "InstitutionalData",
                column: "ConditionCode2ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ConditionCode3ID",
                table: "InstitutionalData",
                column: "ConditionCode3ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ConditionCode4ID",
                table: "InstitutionalData",
                column: "ConditionCode4ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ConditionCode5ID",
                table: "InstitutionalData",
                column: "ConditionCode5ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ConditionCode6ID",
                table: "InstitutionalData",
                column: "ConditionCode6ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ConditionCode7ID",
                table: "InstitutionalData",
                column: "ConditionCode7ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ConditionCode8ID",
                table: "InstitutionalData",
                column: "ConditionCode8ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ConditionCode9ID",
                table: "InstitutionalData",
                column: "ConditionCode9ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_Cpt1ID",
                table: "InstitutionalData",
                column: "Cpt1ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_Cpt2ID",
                table: "InstitutionalData",
                column: "Cpt2ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_Cpt3ID",
                table: "InstitutionalData",
                column: "Cpt3ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_Cpt4ID",
                table: "InstitutionalData",
                column: "Cpt4ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_Cpt5ID",
                table: "InstitutionalData",
                column: "Cpt5ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_CptID",
                table: "InstitutionalData",
                column: "CptID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ExternalInjury1ID",
                table: "InstitutionalData",
                column: "ExternalInjury1ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ExternalInjury2ID",
                table: "InstitutionalData",
                column: "ExternalInjury2ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ExternalInjury3ID",
                table: "InstitutionalData",
                column: "ExternalInjury3ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ICD1ID",
                table: "InstitutionalData",
                column: "ICD1ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ICDID",
                table: "InstitutionalData",
                column: "ICDID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_OccurrenceCode1ID",
                table: "InstitutionalData",
                column: "OccurrenceCode1ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_OccurrenceCode2ID",
                table: "InstitutionalData",
                column: "OccurrenceCode2ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_OccurrenceCode3ID",
                table: "InstitutionalData",
                column: "OccurrenceCode3ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_OccurrenceCode4ID",
                table: "InstitutionalData",
                column: "OccurrenceCode4ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_OccurrenceCode5ID",
                table: "InstitutionalData",
                column: "OccurrenceCode5ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_OccurrenceCode6ID",
                table: "InstitutionalData",
                column: "OccurrenceCode6ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_OccurrenceCode7ID",
                table: "InstitutionalData",
                column: "OccurrenceCode7ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_OccurrenceCode8ID",
                table: "InstitutionalData",
                column: "OccurrenceCode8ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_OccurrenceSpanCode1ID",
                table: "InstitutionalData",
                column: "OccurrenceSpanCode1ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_OccurrenceSpanCode2ID",
                table: "InstitutionalData",
                column: "OccurrenceSpanCode2ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_OccurrenceSpanCode3ID",
                table: "InstitutionalData",
                column: "OccurrenceSpanCode3ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_OccurrenceSpanCode4ID",
                table: "InstitutionalData",
                column: "OccurrenceSpanCode4ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_PatientStatusCodeID",
                table: "InstitutionalData",
                column: "PatientStatusCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ValueCode10ID",
                table: "InstitutionalData",
                column: "ValueCode10ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ValueCode11ID",
                table: "InstitutionalData",
                column: "ValueCode11ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ValueCode12ID",
                table: "InstitutionalData",
                column: "ValueCode12ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ValueCode1ID",
                table: "InstitutionalData",
                column: "ValueCode1ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ValueCode2ID",
                table: "InstitutionalData",
                column: "ValueCode2ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ValueCode3ID",
                table: "InstitutionalData",
                column: "ValueCode3ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ValueCode4ID",
                table: "InstitutionalData",
                column: "ValueCode4ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ValueCode5ID",
                table: "InstitutionalData",
                column: "ValueCode5ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ValueCode6ID",
                table: "InstitutionalData",
                column: "ValueCode6ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ValueCode7ID",
                table: "InstitutionalData",
                column: "ValueCode7ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ValueCode8ID",
                table: "InstitutionalData",
                column: "ValueCode8ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_ValueCode9ID",
                table: "InstitutionalData",
                column: "ValueCode9ID");

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionalData_VisitReasonID",
                table: "InstitutionalData",
                column: "VisitReasonID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstitutionalData");
        }
    }
}

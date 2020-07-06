using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations
{
    public partial class StartUp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MainClient",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    OrganizationName = table.Column<string>(nullable: true),
                    TaxID = table.Column<string>(nullable: true),
                    ServiceLocation = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    OfficeHour = table.Column<string>(nullable: true),
                    FaxNo = table.Column<string>(nullable: true),
                    OfficePhoneNo = table.Column<string>(nullable: true),
                    OfficeEmail = table.Column<string>(nullable: true),
                    ContactPerson = table.Column<string>(nullable: true),
                    ContactNo = table.Column<string>(nullable: true),
                    ContextName = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainClient", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MainDesignations",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainDesignations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MainRights",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SchedulerCreate = table.Column<bool>(nullable: false),
                    SchedulerEdit = table.Column<bool>(nullable: false),
                    SchedulerDelete = table.Column<bool>(nullable: false),
                    SchedulerSearch = table.Column<bool>(nullable: false),
                    SchedulerImport = table.Column<bool>(nullable: false),
                    SchedulerExport = table.Column<bool>(nullable: false),
                    PatientCreate = table.Column<bool>(nullable: false),
                    PatientEdit = table.Column<bool>(nullable: false),
                    PatientDelete = table.Column<bool>(nullable: false),
                    PatientSearch = table.Column<bool>(nullable: false),
                    PatientImport = table.Column<bool>(nullable: false),
                    PatientExport = table.Column<bool>(nullable: false),
                    ChargesCreate = table.Column<bool>(nullable: false),
                    ChargesEdit = table.Column<bool>(nullable: false),
                    ChargesDelete = table.Column<bool>(nullable: false),
                    ChargesSearch = table.Column<bool>(nullable: false),
                    ChargesImport = table.Column<bool>(nullable: false),
                    ChargesExport = table.Column<bool>(nullable: false),
                    DocumentsCreate = table.Column<bool>(nullable: false),
                    DocumentsEdit = table.Column<bool>(nullable: false),
                    DocumentsDelete = table.Column<bool>(nullable: false),
                    DocumentsSearch = table.Column<bool>(nullable: false),
                    DocumentsImport = table.Column<bool>(nullable: false),
                    DocumentsExport = table.Column<bool>(nullable: false),
                    SubmissionsCreate = table.Column<bool>(nullable: false),
                    SubmissionsEdit = table.Column<bool>(nullable: false),
                    SubmissionsDelete = table.Column<bool>(nullable: false),
                    SubmissionsSearch = table.Column<bool>(nullable: false),
                    SubmissionsImport = table.Column<bool>(nullable: false),
                    SubmissionsExport = table.Column<bool>(nullable: false),
                    PaymentsCreate = table.Column<bool>(nullable: false),
                    PaymentsEdit = table.Column<bool>(nullable: false),
                    PaymentsDelete = table.Column<bool>(nullable: false),
                    PaymentsSearch = table.Column<bool>(nullable: false),
                    PaymentsImport = table.Column<bool>(nullable: false),
                    PaymentsExport = table.Column<bool>(nullable: false),
                    FollowupCreate = table.Column<bool>(nullable: false),
                    FollowupEdit = table.Column<bool>(nullable: false),
                    FollowupDelete = table.Column<bool>(nullable: false),
                    FollowupSearch = table.Column<bool>(nullable: false),
                    FollowupImport = table.Column<bool>(nullable: false),
                    FollowupExport = table.Column<bool>(nullable: false),
                    ReportsCreate = table.Column<bool>(nullable: false),
                    ReportsEdit = table.Column<bool>(nullable: false),
                    ReportsDelete = table.Column<bool>(nullable: false),
                    ReportsSearch = table.Column<bool>(nullable: false),
                    ReportsImport = table.Column<bool>(nullable: false),
                    ReportsExport = table.Column<bool>(nullable: false),
                    ClientCreate = table.Column<bool>(nullable: false),
                    ClientEdit = table.Column<bool>(nullable: false),
                    ClientDelete = table.Column<bool>(nullable: false),
                    ClientSearch = table.Column<bool>(nullable: false),
                    ClientImport = table.Column<bool>(nullable: false),
                    ClientExport = table.Column<bool>(nullable: false),
                    UserCreate = table.Column<bool>(nullable: false),
                    UserEdit = table.Column<bool>(nullable: false),
                    UserDelete = table.Column<bool>(nullable: false),
                    UserSearch = table.Column<bool>(nullable: false),
                    UserImport = table.Column<bool>(nullable: false),
                    UserExport = table.Column<bool>(nullable: false),
                    PracticeCreate = table.Column<bool>(nullable: false),
                    PracticeEdit = table.Column<bool>(nullable: false),
                    PracticeDelete = table.Column<bool>(nullable: false),
                    PracticeSearch = table.Column<bool>(nullable: false),
                    PracticeImport = table.Column<bool>(nullable: false),
                    PracticeExport = table.Column<bool>(nullable: false),
                    LocationCreate = table.Column<bool>(nullable: false),
                    LocationEdit = table.Column<bool>(nullable: false),
                    LocationDelete = table.Column<bool>(nullable: false),
                    LocationSearch = table.Column<bool>(nullable: false),
                    LocationImport = table.Column<bool>(nullable: false),
                    LocationExport = table.Column<bool>(nullable: false),
                    ProviderCreate = table.Column<bool>(nullable: false),
                    ProviderEdit = table.Column<bool>(nullable: false),
                    ProviderDelete = table.Column<bool>(nullable: false),
                    ProviderSearch = table.Column<bool>(nullable: false),
                    ProviderImport = table.Column<bool>(nullable: false),
                    ProviderExport = table.Column<bool>(nullable: false),
                    ReferringProviderCreate = table.Column<bool>(nullable: false),
                    ReferringProviderEdit = table.Column<bool>(nullable: false),
                    ReferringProviderDelete = table.Column<bool>(nullable: false),
                    ReferringProviderSearch = table.Column<bool>(nullable: false),
                    ReferringProviderImport = table.Column<bool>(nullable: false),
                    ReferringProviderExport = table.Column<bool>(nullable: false),
                    InsuranceCreate = table.Column<bool>(nullable: false),
                    InsuranceEdit = table.Column<bool>(nullable: false),
                    InsuranceDelete = table.Column<bool>(nullable: false),
                    InsuranceSearch = table.Column<bool>(nullable: false),
                    InsuranceImport = table.Column<bool>(nullable: false),
                    InsuranceExport = table.Column<bool>(nullable: false),
                    InsurancePlanCreate = table.Column<bool>(nullable: false),
                    InsurancePlanEdit = table.Column<bool>(nullable: false),
                    InsurancePlanDelete = table.Column<bool>(nullable: false),
                    InsurancePlanSearch = table.Column<bool>(nullable: false),
                    InsurancePlanImport = table.Column<bool>(nullable: false),
                    InsurancePlanExport = table.Column<bool>(nullable: false),
                    InsurancePlanAddressCreate = table.Column<bool>(nullable: false),
                    InsurancePlanAddressEdit = table.Column<bool>(nullable: false),
                    InsurancePlanAddressDelete = table.Column<bool>(nullable: false),
                    InsurancePlanAddressSearch = table.Column<bool>(nullable: false),
                    InsurancePlanAddressImport = table.Column<bool>(nullable: false),
                    InsurancePlanAddressExport = table.Column<bool>(nullable: false),
                    EDISubmitCreate = table.Column<bool>(nullable: false),
                    EDISubmitEdit = table.Column<bool>(nullable: false),
                    EDISubmitDelete = table.Column<bool>(nullable: false),
                    EDISubmitSearch = table.Column<bool>(nullable: false),
                    EDISubmitImport = table.Column<bool>(nullable: false),
                    EDISubmitExport = table.Column<bool>(nullable: false),
                    EDIEligiBilityCreate = table.Column<bool>(nullable: false),
                    EDIEligiBilityEdit = table.Column<bool>(nullable: false),
                    EDIEligiBilityDelete = table.Column<bool>(nullable: false),
                    EDIEligiBilitySearch = table.Column<bool>(nullable: false),
                    EDIEligiBilityImport = table.Column<bool>(nullable: false),
                    EDIEligiBilityExport = table.Column<bool>(nullable: false),
                    EDIStatusCreate = table.Column<bool>(nullable: false),
                    EDIStatusEdit = table.Column<bool>(nullable: false),
                    EDIStatusDelete = table.Column<bool>(nullable: false),
                    EDIStatusSearch = table.Column<bool>(nullable: false),
                    EDIStatusImport = table.Column<bool>(nullable: false),
                    EDIStatusExport = table.Column<bool>(nullable: false),
                    ICDCreate = table.Column<bool>(nullable: false),
                    ICDEdit = table.Column<bool>(nullable: false),
                    ICDDelete = table.Column<bool>(nullable: false),
                    ICDSearch = table.Column<bool>(nullable: false),
                    ICDImport = table.Column<bool>(nullable: false),
                    ICDExport = table.Column<bool>(nullable: false),
                    CPTCreate = table.Column<bool>(nullable: false),
                    CPTEdit = table.Column<bool>(nullable: false),
                    CPTDelete = table.Column<bool>(nullable: false),
                    CPTSearch = table.Column<bool>(nullable: false),
                    CPTImport = table.Column<bool>(nullable: false),
                    CPTExport = table.Column<bool>(nullable: false),
                    ModifiersCreate = table.Column<bool>(nullable: false),
                    ModifiersEdit = table.Column<bool>(nullable: false),
                    ModifiersDelete = table.Column<bool>(nullable: false),
                    ModifiersSearch = table.Column<bool>(nullable: false),
                    ModifiersImport = table.Column<bool>(nullable: false),
                    ModifiersExport = table.Column<bool>(nullable: false),
                    POSCreate = table.Column<bool>(nullable: false),
                    POSEdit = table.Column<bool>(nullable: false),
                    POSDelete = table.Column<bool>(nullable: false),
                    POSSearch = table.Column<bool>(nullable: false),
                    POSImport = table.Column<bool>(nullable: false),
                    POSExport = table.Column<bool>(nullable: false),
                    ClaimStatusCategoryCodesCreate = table.Column<bool>(nullable: false),
                    ClaimStatusCategoryCodesEdit = table.Column<bool>(nullable: false),
                    ClaimStatusCategoryCodesDelete = table.Column<bool>(nullable: false),
                    ClaimStatusCategoryCodesSearch = table.Column<bool>(nullable: false),
                    ClaimStatusCategoryCodesImport = table.Column<bool>(nullable: false),
                    ClaimStatusCategoryCodesExport = table.Column<bool>(nullable: false),
                    ClaimStatusCodesCreate = table.Column<bool>(nullable: false),
                    ClaimStatusCodesEdit = table.Column<bool>(nullable: false),
                    ClaimStatusCodesDelete = table.Column<bool>(nullable: false),
                    ClaimStatusCodesSearch = table.Column<bool>(nullable: false),
                    ClaimStatusCodesImport = table.Column<bool>(nullable: false),
                    ClaimStatusCodesExport = table.Column<bool>(nullable: false),
                    AdjustmentCodesCreate = table.Column<bool>(nullable: false),
                    AdjustmentCodesEdit = table.Column<bool>(nullable: false),
                    AdjustmentCodesDelete = table.Column<bool>(nullable: false),
                    AdjustmentCodesSearch = table.Column<bool>(nullable: false),
                    AdjustmentCodesImport = table.Column<bool>(nullable: false),
                    AdjustmentCodesExport = table.Column<bool>(nullable: false),
                    RemarkCodesCreate = table.Column<bool>(nullable: false),
                    RemarkCodesEdit = table.Column<bool>(nullable: false),
                    RemarkCodesDelete = table.Column<bool>(nullable: false),
                    RemarkCodesSearch = table.Column<bool>(nullable: false),
                    RemarkCodesImport = table.Column<bool>(nullable: false),
                    RemarkCodesExport = table.Column<bool>(nullable: false),
                    teamCreate = table.Column<bool>(nullable: false),
                    teamupdate = table.Column<bool>(nullable: false),
                    teamDelete = table.Column<bool>(nullable: false),
                    teamSearch = table.Column<bool>(nullable: false),
                    teamExport = table.Column<bool>(nullable: false),
                    teamImport = table.Column<bool>(nullable: false),
                    receiverCreate = table.Column<bool>(nullable: false),
                    receiverupdate = table.Column<bool>(nullable: false),
                    receiverDelete = table.Column<bool>(nullable: false),
                    receiverSearch = table.Column<bool>(nullable: false),
                    receiverExport = table.Column<bool>(nullable: false),
                    receiverImport = table.Column<bool>(nullable: false),
                    submitterCreate = table.Column<bool>(nullable: false),
                    submitterUpdate = table.Column<bool>(nullable: false),
                    submitterDelete = table.Column<bool>(nullable: false),
                    submitterSearch = table.Column<bool>(nullable: false),
                    submitterExport = table.Column<bool>(nullable: false),
                    submitterImport = table.Column<bool>(nullable: false),
                    patientPlanCreate = table.Column<bool>(nullable: false),
                    patientPlanUpdate = table.Column<bool>(nullable: false),
                    patientPlanDelete = table.Column<bool>(nullable: false),
                    patientPlanSearch = table.Column<bool>(nullable: false),
                    patientPlanExport = table.Column<bool>(nullable: false),
                    patientPlanImport = table.Column<bool>(nullable: false),
                    performEligibility = table.Column<bool>(nullable: false),
                    patientPaymentCreate = table.Column<bool>(nullable: false),
                    patientPaymentUpdate = table.Column<bool>(nullable: false),
                    patientPaymentDelete = table.Column<bool>(nullable: false),
                    patientPaymentSearch = table.Column<bool>(nullable: false),
                    patientPaymentExport = table.Column<bool>(nullable: false),
                    patientPaymentImport = table.Column<bool>(nullable: false),
                    resubmitCharges = table.Column<bool>(nullable: false),
                    batchdocumentCreate = table.Column<bool>(nullable: false),
                    batchdocumentUpdate = table.Column<bool>(nullable: false),
                    batchdocumentDelete = table.Column<bool>(nullable: false),
                    batchdocumentSearch = table.Column<bool>(nullable: false),
                    batchdocumentExport = table.Column<bool>(nullable: false),
                    batchdocumentImport = table.Column<bool>(nullable: false),
                    electronicsSubmissionSearch = table.Column<bool>(nullable: false),
                    electronicsSubmissionSubmit = table.Column<bool>(nullable: false),
                    electronicsSubmissionResubmit = table.Column<bool>(nullable: false),
                    paperSubmissionSearch = table.Column<bool>(nullable: false),
                    paperSubmissionSubmit = table.Column<bool>(nullable: false),
                    paperSubmissionResubmit = table.Column<bool>(nullable: false),
                    submissionLogSearch = table.Column<bool>(nullable: false),
                    planFollowupSearch = table.Column<bool>(nullable: false),
                    planFollowupCreate = table.Column<bool>(nullable: false),
                    planFollowupDelete = table.Column<bool>(nullable: false),
                    planFollowupUpdate = table.Column<bool>(nullable: false),
                    planFollowupImport = table.Column<bool>(nullable: false),
                    planFollowupExport = table.Column<bool>(nullable: false),
                    patientFollowupSearch = table.Column<bool>(nullable: false),
                    patientFollowupCreate = table.Column<bool>(nullable: false),
                    patientFollowupDelete = table.Column<bool>(nullable: false),
                    patientFollowupUpdate = table.Column<bool>(nullable: false),
                    patientFollowupImport = table.Column<bool>(nullable: false),
                    patientFollowupExport = table.Column<bool>(nullable: false),
                    groupSearch = table.Column<bool>(nullable: false),
                    groupCreate = table.Column<bool>(nullable: false),
                    groupUpdate = table.Column<bool>(nullable: false),
                    groupDelete = table.Column<bool>(nullable: false),
                    groupExport = table.Column<bool>(nullable: false),
                    groupImport = table.Column<bool>(nullable: false),
                    reasonSearch = table.Column<bool>(nullable: false),
                    reasonCreate = table.Column<bool>(nullable: false),
                    reasonUpdate = table.Column<bool>(nullable: false),
                    reasonDelete = table.Column<bool>(nullable: false),
                    reasonExport = table.Column<bool>(nullable: false),
                    reasonImport = table.Column<bool>(nullable: false),
                    addPaymentVisit = table.Column<bool>(nullable: false),
                    DeleteCheck = table.Column<bool>(nullable: false),
                    ManualPosting = table.Column<bool>(nullable: false),
                    Postcheck = table.Column<bool>(nullable: false),
                    PostExport = table.Column<bool>(nullable: false),
                    PostImport = table.Column<bool>(nullable: false),
                    ManualPostingAdd = table.Column<bool>(nullable: false),
                    ManualPostingUpdate = table.Column<bool>(nullable: false),
                    PostCheckSearch = table.Column<bool>(nullable: false),
                    DeletePaymentVisit = table.Column<bool>(nullable: false),
                    AssignedByUserId = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainRights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MainTeam",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Details = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainTeam", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MainUserPracticeAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<string>(nullable: true),
                    TransactionID = table.Column<long>(nullable: false),
                    ColumnName = table.Column<string>(nullable: true),
                    CurrentValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    CurrentValueID = table.Column<string>(nullable: true),
                    NewValueID = table.Column<string>(nullable: true),
                    HostName = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainUserPracticeAudit", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MainUserPractices",
                columns: table => new
                {
                    UserID = table.Column<string>(nullable: false),
                    PracticeID = table.Column<long>(nullable: false),
                    AssignedByUserId = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    UPALastModified = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainUserPractices", x => new { x.UserID, x.PracticeID });
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MainPractice",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    ClientID = table.Column<long>(nullable: true),
                    OrganizationName = table.Column<string>(maxLength: 50, nullable: false),
                    TaxID = table.Column<string>(maxLength: 9, nullable: true),
                    CLIANumber = table.Column<string>(maxLength: 20, nullable: true),
                    NPI = table.Column<string>(maxLength: 10, nullable: true),
                    SSN = table.Column<string>(maxLength: 9, nullable: true),
                    Type = table.Column<string>(nullable: true),
                    TaxonomyCode = table.Column<string>(maxLength: 10, nullable: true),
                    Address1 = table.Column<string>(maxLength: 55, nullable: true),
                    Address2 = table.Column<string>(maxLength: 55, nullable: true),
                    City = table.Column<string>(maxLength: 20, nullable: true),
                    State = table.Column<string>(maxLength: 2, nullable: true),
                    ZipCode = table.Column<string>(maxLength: 9, nullable: true),
                    OfficePhoneNum = table.Column<string>(maxLength: 10, nullable: true),
                    FaxNumber = table.Column<string>(maxLength: 10, nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    PayToAddress1 = table.Column<string>(maxLength: 55, nullable: true),
                    PayToAddress2 = table.Column<string>(maxLength: 55, nullable: true),
                    PayToCity = table.Column<string>(maxLength: 20, nullable: true),
                    PayToState = table.Column<string>(maxLength: 2, nullable: true),
                    PayToZipCode = table.Column<string>(maxLength: 9, nullable: true),
                    DefaultLocationID = table.Column<long>(nullable: true),
                    WorkingHours = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    MainUserPracticesUserID = table.Column<string>(nullable: true),
                    MainUserPracticesPracticeID = table.Column<long>(nullable: true),
                    MainClientID = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainPractice", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MainPractice_MainClient_MainClientID",
                        column: x => x.MainClientID,
                        principalTable: "MainClient",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MainPractice_MainUserPractices_MainUserPracticesUserID_MainUserPracticesPracticeID",
                        columns: x => new { x.MainUserPracticesUserID, x.MainUserPracticesPracticeID },
                        principalTable: "MainUserPractices",
                        principalColumns: new[] { "UserID", "PracticeID" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    IsUserLogin = table.Column<bool>(nullable: false),
                    LogInAttempts = table.Column<int>(nullable: false),
                    IsUserBlock = table.Column<bool>(nullable: false),
                    IsUserBlockByAdmin = table.Column<bool>(nullable: false),
                    BlockNote = table.Column<string>(nullable: true),
                    ClientID = table.Column<long>(nullable: true),
                    PracticeID = table.Column<long>(nullable: true),
                    MainRightsId = table.Column<string>(nullable: true),
                    TeamID = table.Column<long>(nullable: true),
                    DesignationID = table.Column<long>(nullable: true),
                    ReportingTo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_MainClient_ClientID",
                        column: x => x.ClientID,
                        principalTable: "MainClient",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_MainDesignations_DesignationID",
                        column: x => x.DesignationID,
                        principalTable: "MainDesignations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_MainRights_MainRightsId",
                        column: x => x.MainRightsId,
                        principalTable: "MainRights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_MainPractice_PracticeID",
                        column: x => x.PracticeID,
                        principalTable: "MainPractice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_MainTeam_TeamID",
                        column: x => x.TeamID,
                        principalTable: "MainTeam",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ClientID",
                table: "AspNetUsers",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DesignationID",
                table: "AspNetUsers",
                column: "DesignationID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_MainRightsId",
                table: "AspNetUsers",
                column: "MainRightsId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PracticeID",
                table: "AspNetUsers",
                column: "PracticeID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TeamID",
                table: "AspNetUsers",
                column: "TeamID");

            migrationBuilder.CreateIndex(
                name: "IX_MainPractice_MainClientID",
                table: "MainPractice",
                column: "MainClientID");

            migrationBuilder.CreateIndex(
                name: "IX_MainPractice_MainUserPracticesUserID_MainUserPracticesPracticeID",
                table: "MainPractice",
                columns: new[] { "MainUserPracticesUserID", "MainUserPracticesPracticeID" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "MainUserPracticeAudit");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "MainDesignations");

            migrationBuilder.DropTable(
                name: "MainRights");

            migrationBuilder.DropTable(
                name: "MainPractice");

            migrationBuilder.DropTable(
                name: "MainTeam");

            migrationBuilder.DropTable(
                name: "MainClient");

            migrationBuilder.DropTable(
                name: "MainUserPractices");
        }
    }
}

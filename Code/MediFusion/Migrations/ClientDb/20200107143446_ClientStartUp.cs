using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediFusionPM.Migrations.ClientDb
{
    public partial class ClientStartUp : Migration
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
                name: "Biller",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LastName = table.Column<string>(maxLength: 30, nullable: true),
                    FirstName = table.Column<string>(maxLength: 30, nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biller", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CategoryCodes",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ShortDesc = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    UpdateBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryCodes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ChargeStatus",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VisitStatusID = table.Column<long>(nullable: false),
                    ChargeID = table.Column<long>(nullable: false),
                    CPT = table.Column<string>(nullable: true),
                    DOS = table.Column<DateTime>(nullable: true),
                    Amount = table.Column<decimal>(nullable: true),
                    BilledAmount = table.Column<decimal>(nullable: true),
                    Modifier1 = table.Column<string>(nullable: true),
                    Modifier2 = table.Column<string>(nullable: true),
                    Modifier3 = table.Column<string>(nullable: true),
                    Modifier4 = table.Column<string>(nullable: true),
                    CheckDate = table.Column<DateTime>(nullable: true),
                    PaidAmount = table.Column<decimal>(nullable: true),
                    CheckNumber = table.Column<string>(nullable: true),
                    CategoryCode1 = table.Column<string>(nullable: true),
                    StatusCode1 = table.Column<string>(nullable: true),
                    EntityCode1 = table.Column<string>(nullable: true),
                    RejectionReason1 = table.Column<string>(nullable: true),
                    CategoryCode2 = table.Column<string>(nullable: true),
                    StatusCode2 = table.Column<string>(nullable: true),
                    EntityCode2 = table.Column<string>(nullable: true),
                    RejectionReason2 = table.Column<string>(nullable: true),
                    CategoryCode3 = table.Column<string>(nullable: true),
                    StatusCode3 = table.Column<string>(nullable: true),
                    EntityCode3 = table.Column<string>(nullable: true),
                    RejectionReason3 = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargeStatus", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CityStateZipData",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Lat = table.Column<double>(nullable: false),
                    Lng = table.Column<double>(nullable: false),
                    State_id = table.Column<string>(nullable: true),
                    State_name = table.Column<string>(nullable: true),
                    Zip = table.Column<string>(nullable: true),
                    Population = table.Column<int>(nullable: false),
                    Density = table.Column<double>(nullable: false),
                    city = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityStateZipData", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_Client", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Designations",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Designations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DocumentType",
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
                    table.PrimaryKey("PK_DocumentType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DownloadedFile",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ReportsLogID = table.Column<long>(nullable: true),
                    FilePath = table.Column<string>(nullable: true),
                    FileType = table.Column<string>(nullable: true),
                    Processed = table.Column<bool>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownloadedFile", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ICD",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: false),
                    ICDCode = table.Column<string>(nullable: true),
                    IsValid = table.Column<bool>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ICD", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Insurance",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    OrganizationName = table.Column<string>(maxLength: 50, nullable: true),
                    Address1 = table.Column<string>(maxLength: 55, nullable: true),
                    Address2 = table.Column<string>(maxLength: 55, nullable: true),
                    City = table.Column<string>(maxLength: 20, nullable: true),
                    State = table.Column<string>(maxLength: 2, nullable: true),
                    ZipCode = table.Column<string>(maxLength: 9, nullable: true),
                    OfficePhoneNum = table.Column<string>(maxLength: 10, nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    FaxNumber = table.Column<string>(maxLength: 10, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Notes = table.Column<string>(maxLength: 500, nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Insurance", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Modifier",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    AnesthesiaBaseUnits = table.Column<int>(nullable: true),
                    DefaultFees = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modifier", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PatientEligibility",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EligibilityDate = table.Column<DateTime>(nullable: true),
                    DOS = table.Column<DateTime>(nullable: true),
                    PatientID = table.Column<long>(nullable: false),
                    PracticeID = table.Column<long>(nullable: false),
                    LocationID = table.Column<long>(nullable: true),
                    ProviderID = table.Column<long>(nullable: false),
                    PatientPlanID = table.Column<long>(nullable: false),
                    TRNNumber = table.Column<string>(nullable: true),
                    Relation = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Rejection = table.Column<string>(nullable: true),
                    RejectionCode = table.Column<string>(nullable: true),
                    SubscriberLN = table.Column<string>(nullable: true),
                    SubscriberFN = table.Column<string>(nullable: true),
                    SubscriberMI = table.Column<string>(nullable: true),
                    SubscriberID = table.Column<string>(nullable: true),
                    SubscriberGroupNumber = table.Column<string>(nullable: true),
                    SubscriberAddress = table.Column<string>(nullable: true),
                    SubscriberCity = table.Column<string>(nullable: true),
                    SubscriberState = table.Column<string>(nullable: true),
                    SubscriberZip = table.Column<string>(nullable: true),
                    SubscriberDOB = table.Column<DateTime>(nullable: true),
                    SubscriberGender = table.Column<string>(nullable: true),
                    PatientLN = table.Column<string>(nullable: true),
                    PatientFN = table.Column<string>(nullable: true),
                    PatientMI = table.Column<string>(nullable: true),
                    PatientAddress = table.Column<string>(nullable: true),
                    PatientCity = table.Column<string>(nullable: true),
                    PatientState = table.Column<string>(nullable: true),
                    PatientZip = table.Column<string>(nullable: true),
                    PatientDOB = table.Column<DateTime>(nullable: true),
                    PatientGender = table.Column<string>(nullable: true),
                    ProviderLN = table.Column<string>(nullable: true),
                    ProviderFN = table.Column<string>(nullable: true),
                    ProviderNPI = table.Column<string>(nullable: true),
                    PayerName = table.Column<string>(nullable: true),
                    PayerID = table.Column<string>(nullable: true),
                    ErrorMessage = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientEligibility", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PatientEligibilityDetail",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientEligibilityID = table.Column<long>(nullable: false),
                    Coverage = table.Column<string>(nullable: true),
                    CoverageLevel = table.Column<string>(nullable: true),
                    ServiceTypes = table.Column<string>(nullable: true),
                    PlanName = table.Column<string>(nullable: true),
                    PlanDescription = table.Column<string>(nullable: true),
                    TimePeriod = table.Column<string>(nullable: true),
                    BenefitAmount = table.Column<decimal>(nullable: true),
                    BenefitPercentage = table.Column<string>(nullable: true),
                    Authorization = table.Column<string>(nullable: true),
                    PlanNetwork = table.Column<string>(nullable: true),
                    Messages = table.Column<string>(nullable: true),
                    ReferenceId1 = table.Column<string>(nullable: true),
                    ReferenceValue1 = table.Column<string>(nullable: true),
                    ReferenceId2 = table.Column<string>(nullable: true),
                    ReferenceValue2 = table.Column<string>(nullable: true),
                    ReferenceId3 = table.Column<string>(nullable: true),
                    ReferenceValue3 = table.Column<string>(nullable: true),
                    ReferenceId4 = table.Column<string>(nullable: true),
                    ReferenceValue4 = table.Column<string>(nullable: true),
                    ReferenceId5 = table.Column<string>(nullable: true),
                    ReferenceValue5 = table.Column<string>(nullable: true),
                    DateId1 = table.Column<string>(nullable: true),
                    DateValue1 = table.Column<string>(nullable: true),
                    DateId2 = table.Column<string>(nullable: true),
                    DateValue2 = table.Column<string>(nullable: true),
                    DateId3 = table.Column<string>(nullable: true),
                    DateValue3 = table.Column<string>(nullable: true),
                    DateId4 = table.Column<string>(nullable: true),
                    DateValue4 = table.Column<string>(nullable: true),
                    DateId5 = table.Column<string>(nullable: true),
                    DateValue5 = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientEligibilityDetail", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PatientEligibilityLog",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientEligibilityID = table.Column<long>(nullable: false),
                    Transaction270Path = table.Column<string>(nullable: true),
                    Transaction271Path = table.Column<string>(nullable: true),
                    Transaction999Path = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientEligibilityLog", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PlanType",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "POS",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PosCode = table.Column<string>(maxLength: 8, nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: true),
                    Description = table.Column<string>(maxLength: 300, nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Receiver",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Address1 = table.Column<string>(maxLength: 55, nullable: true),
                    Address2 = table.Column<string>(maxLength: 55, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 10, nullable: true),
                    FaxNumber = table.Column<string>(maxLength: 10, nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    City = table.Column<string>(maxLength: 20, nullable: true),
                    State = table.Column<string>(maxLength: 2, nullable: true),
                    ZipCode = table.Column<string>(maxLength: 9, nullable: true),
                    SubmissionMethod = table.Column<string>(nullable: true),
                    SubmissionURL = table.Column<string>(nullable: true),
                    SubmissionPort = table.Column<string>(nullable: true),
                    SubmissionDirectory = table.Column<string>(nullable: true),
                    ReportsDirectory = table.Column<string>(nullable: true),
                    ErasDirectory = table.Column<string>(nullable: true),
                    X12_837_NM1_40_ReceiverName = table.Column<string>(nullable: true),
                    X12_837_NM1_40_ReceiverID = table.Column<string>(nullable: true),
                    X12_837_ISA_01 = table.Column<string>(nullable: true),
                    X12_837_ISA_03 = table.Column<string>(nullable: true),
                    X12_837_ISA_05 = table.Column<string>(nullable: true),
                    X12_837_ISA_07 = table.Column<string>(nullable: true),
                    X12_837_ISA_08 = table.Column<string>(nullable: true),
                    X12_837_GS_03 = table.Column<string>(nullable: true),
                    X12_270_NM1_40_ReceiverName = table.Column<string>(nullable: true),
                    X12_270_NM1_40_ReceiverID = table.Column<string>(nullable: true),
                    X12_270_ISA_01 = table.Column<string>(nullable: true),
                    X12_270_ISA_03 = table.Column<string>(nullable: true),
                    X12_270_ISA_05 = table.Column<string>(nullable: true),
                    X12_270_ISA_07 = table.Column<string>(nullable: true),
                    X12_270_ISA_08 = table.Column<string>(nullable: true),
                    X12_270_GS_03 = table.Column<string>(nullable: true),
                    X12_276_NM1_40_ReceiverName = table.Column<string>(nullable: true),
                    X12_276_NM1_40_ReceiverID = table.Column<string>(nullable: true),
                    X12_276_ISA_01 = table.Column<string>(nullable: true),
                    X12_276_ISA_03 = table.Column<string>(nullable: true),
                    X12_276_ISA_05 = table.Column<string>(nullable: true),
                    X12_276_ISA_07 = table.Column<string>(nullable: true),
                    X12_276_ISA_08 = table.Column<string>(nullable: true),
                    X12_276_GS_03 = table.Column<string>(nullable: true),
                    ElementSeperator = table.Column<string>(nullable: true),
                    SegmentSeperator = table.Column<string>(nullable: true),
                    SubElementSeperator = table.Column<string>(nullable: true),
                    RepetitionSepeator = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receiver", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RemarkCode",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 10, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RemarkCode", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ReportsLog",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientID = table.Column<long>(nullable: true),
                    ReceiverID = table.Column<long>(nullable: true),
                    SubmitterID = table.Column<long>(nullable: true),
                    ZipFilePath = table.Column<string>(nullable: true),
                    Processed = table.Column<bool>(nullable: true),
                    UserResolved = table.Column<bool>(nullable: true),
                    FilesCount = table.Column<int>(nullable: false),
                    ManualImport = table.Column<bool>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportsLog", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Rights",
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
                    table.PrimaryKey("PK_Rights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientID = table.Column<long>(nullable: false),
                    DocumentServerURL = table.Column<string>(nullable: true),
                    DocumentServerDirectory = table.Column<string>(nullable: true),
                    DocumentServerAuthUser = table.Column<string>(nullable: true),
                    DocumentServerAuthPass = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "StatusCode",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    UpdateBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusCode", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SubmissionLog",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientID = table.Column<long>(nullable: false),
                    ReceiverID = table.Column<long>(nullable: false),
                    SubmitterID = table.Column<long>(nullable: false),
                    SubmitType = table.Column<string>(nullable: true),
                    DownloadedFileID = table.Column<long>(nullable: true),
                    PdfPath = table.Column<string>(nullable: true),
                    FormType = table.Column<string>(nullable: true),
                    ISAControlNumber = table.Column<string>(nullable: true),
                    ClaimCount = table.Column<long>(nullable: false),
                    ClaimAmount = table.Column<decimal>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    Transaction837Path = table.Column<string>(nullable: true),
                    IK5_Status = table.Column<string>(nullable: true),
                    AK9_Status = table.Column<string>(nullable: true),
                    NoOfTotalST = table.Column<int>(nullable: true),
                    NoOfReceivedST = table.Column<int>(nullable: true),
                    NoOfAcceptedST = table.Column<int>(nullable: true),
                    IK5_ErrorCode = table.Column<string>(nullable: true),
                    AK9_ErrorCode = table.Column<string>(nullable: true),
                    Transaction999Path = table.Column<string>(nullable: true),
                    Trasaction277CAPath = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmissionLog", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Taxonomy",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Speciality = table.Column<string>(nullable: true),
                    AMADescription = table.Column<string>(nullable: true),
                    SpecialityType = table.Column<string>(nullable: true),
                    NUCCCode = table.Column<string>(nullable: true),
                    NUCCDescription = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    UpdateBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxonomy", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Details = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TypeOfService",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfService", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserPracticeAudit",
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
                    table.PrimaryKey("PK_UserPracticeAudit", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserPractices",
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
                    table.PrimaryKey("PK_UserPractices", x => new { x.UserID, x.PracticeID });
                });

            migrationBuilder.CreateTable(
                name: "VisitReason",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitReason", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "VisitStatus",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VisitID = table.Column<long>(nullable: true),
                    DOS = table.Column<DateTime>(nullable: true),
                    VisitAmount = table.Column<decimal>(nullable: false),
                    ResponseEntity = table.Column<string>(nullable: true),
                    PracticeID = table.Column<long>(nullable: true),
                    LocationID = table.Column<long>(nullable: true),
                    ProviderID = table.Column<long>(nullable: true),
                    PatientPlanID = table.Column<long>(nullable: true),
                    SubmitterTRN = table.Column<string>(nullable: true),
                    TRNNumber = table.Column<string>(nullable: true),
                    ActionCode = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    SubscriberLN = table.Column<string>(nullable: true),
                    SubscriberFN = table.Column<string>(nullable: true),
                    SubscriberMI = table.Column<string>(nullable: true),
                    SubscriberID = table.Column<string>(nullable: true),
                    SubscriberAddress = table.Column<string>(nullable: true),
                    SubscriberCity = table.Column<string>(nullable: true),
                    SubscriberState = table.Column<string>(nullable: true),
                    SubscriberZip = table.Column<string>(nullable: true),
                    SubscriberDOB = table.Column<DateTime>(nullable: false),
                    SubscriberGender = table.Column<string>(nullable: true),
                    PatientLN = table.Column<string>(nullable: true),
                    PatientFN = table.Column<string>(nullable: true),
                    PatientMI = table.Column<string>(nullable: true),
                    PatientAddress = table.Column<string>(nullable: true),
                    PatientCity = table.Column<string>(nullable: true),
                    PatientState = table.Column<string>(nullable: true),
                    PatientZip = table.Column<string>(nullable: true),
                    PatientDOB = table.Column<DateTime>(nullable: false),
                    PatientGender = table.Column<string>(nullable: true),
                    ProviderLN = table.Column<string>(nullable: true),
                    ProviderFN = table.Column<string>(nullable: true),
                    ProviderNPI = table.Column<string>(nullable: true),
                    PayerName = table.Column<string>(nullable: true),
                    PayerID = table.Column<string>(nullable: true),
                    ErrorMessage = table.Column<string>(nullable: true),
                    CategoryCode1 = table.Column<string>(nullable: true),
                    CategoryCodeDesc1 = table.Column<string>(nullable: true),
                    StatusCode1 = table.Column<string>(nullable: true),
                    StatusCodeDesc1 = table.Column<string>(nullable: true),
                    EntityCode1 = table.Column<string>(nullable: true),
                    EntityCodeDesc1 = table.Column<string>(nullable: true),
                    RejectionReason1 = table.Column<string>(nullable: true),
                    FreeText1 = table.Column<string>(nullable: true),
                    CategoryCode2 = table.Column<string>(nullable: true),
                    CategoryCodeDesc2 = table.Column<string>(nullable: true),
                    StatusCode2 = table.Column<string>(nullable: true),
                    StatusCodeDesc2 = table.Column<string>(nullable: true),
                    EntityCode2 = table.Column<string>(nullable: true),
                    EntityCodeDesc2 = table.Column<string>(nullable: true),
                    RejectionReason2 = table.Column<string>(nullable: true),
                    FreeText2 = table.Column<string>(nullable: true),
                    CategoryCode3 = table.Column<string>(nullable: true),
                    CategoryCodeDesc3 = table.Column<string>(nullable: true),
                    StatusCode3 = table.Column<string>(nullable: true),
                    StatusCodeDesc3 = table.Column<string>(nullable: true),
                    EntityCode3 = table.Column<string>(nullable: true),
                    EntityCodeDesc3 = table.Column<string>(nullable: true),
                    RejectionReason3 = table.Column<string>(nullable: true),
                    FreeText3 = table.Column<string>(nullable: true),
                    PayerControlNumber = table.Column<string>(nullable: true),
                    StatusDate = table.Column<DateTime>(nullable: true),
                    BilledAmount = table.Column<decimal>(nullable: true),
                    PaidAmount = table.Column<decimal>(nullable: true),
                    CheckDate = table.Column<DateTime>(nullable: true),
                    CheckNumber = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitStatus", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "VisitStatusLog",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DownloadedFileID = table.Column<long>(nullable: true),
                    Transaction276Path = table.Column<string>(nullable: true),
                    Transaction277Path = table.Column<string>(nullable: true),
                    Transaction277CAPath = table.Column<string>(nullable: true),
                    Transaction999Path = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitStatusLog", x => x.ID);
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
                name: "InsuranceAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InsuranceID = table.Column<long>(nullable: false),
                    TransactionID = table.Column<long>(nullable: false),
                    ColumnName = table.Column<string>(nullable: true),
                    CurrentValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    CurrentValueID = table.Column<string>(nullable: true),
                    NewValueID = table.Column<string>(nullable: true),
                    HostName = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_InsuranceAudit_Insurance_InsuranceID",
                        column: x => x.InsuranceID,
                        principalTable: "Insurance",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "POSAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    POSID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_POSAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_POSAudit_POS_POSID",
                        column: x => x.POSID,
                        principalTable: "POS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Edi270Payer",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PayerName = table.Column<string>(nullable: true),
                    PayerID = table.Column<string>(nullable: true),
                    ReceiverID = table.Column<long>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Edi270Payer", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Edi270Payer_Receiver_ReceiverID",
                        column: x => x.ReceiverID,
                        principalTable: "Receiver",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Edi276Payer",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PayerName = table.Column<string>(nullable: true),
                    PayerID = table.Column<string>(nullable: true),
                    ReceiverID = table.Column<long>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Edi276Payer", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Edi276Payer_Receiver_ReceiverID",
                        column: x => x.ReceiverID,
                        principalTable: "Receiver",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Edi837Payer",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PayerName = table.Column<string>(nullable: true),
                    PayerID = table.Column<string>(nullable: true),
                    ReceiverID = table.Column<long>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Edi837Payer", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Edi837Payer_Receiver_ReceiverID",
                        column: x => x.ReceiverID,
                        principalTable: "Receiver",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Submitter",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(maxLength: 55, nullable: true),
                    City = table.Column<string>(maxLength: 20, nullable: true),
                    State = table.Column<string>(maxLength: 2, nullable: true),
                    ZipCode = table.Column<string>(maxLength: 9, nullable: true),
                    SubmissionUserName = table.Column<string>(nullable: true),
                    SubmissionPassword = table.Column<string>(nullable: true),
                    ManualSubmission = table.Column<bool>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    X12_837_NM1_41_SubmitterName = table.Column<string>(nullable: true),
                    X12_837_NM1_41_SubmitterID = table.Column<string>(nullable: true),
                    X12_837_ISA_02 = table.Column<string>(nullable: true),
                    X12_837_ISA_04 = table.Column<string>(nullable: true),
                    X12_837_ISA_06 = table.Column<string>(nullable: true),
                    X12_837_GS_02 = table.Column<string>(nullable: true),
                    SubmitterContactPerson = table.Column<string>(nullable: true),
                    SubmitterContactNumber = table.Column<string>(nullable: true),
                    SubmitterEmail = table.Column<string>(nullable: true),
                    SubmitterFaxNumber = table.Column<string>(nullable: true),
                    X12_270_NM1_41_SubmitterName = table.Column<string>(nullable: true),
                    X12_270_NM1_41_SubmitterID = table.Column<string>(nullable: true),
                    X12_270_ISA_02 = table.Column<string>(nullable: true),
                    X12_270_ISA_04 = table.Column<string>(nullable: true),
                    X12_270_ISA_06 = table.Column<string>(nullable: true),
                    X12_270_GS_02 = table.Column<string>(nullable: true),
                    X12_276_NM1_41_SubmitterName = table.Column<string>(nullable: true),
                    X12_276_NM1_41_SubmitterID = table.Column<string>(nullable: true),
                    X12_276_ISA_02 = table.Column<string>(nullable: true),
                    X12_276_ISA_04 = table.Column<string>(nullable: true),
                    X12_276_ISA_06 = table.Column<string>(nullable: true),
                    X12_276_GS_02 = table.Column<string>(nullable: true),
                    ReceiverID = table.Column<long>(nullable: false),
                    ClientID = table.Column<long>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submitter", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Submitter_Client_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Client",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Submitter_Receiver_ReceiverID",
                        column: x => x.ReceiverID,
                        principalTable: "Receiver",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TeamID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_TeamAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TeamAudit_Team_TeamID",
                        column: x => x.TeamID,
                        principalTable: "Team",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cpt",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 100, nullable: false),
                    CPTCode = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    Modifier1ID = table.Column<long>(nullable: true),
                    Modifier2ID = table.Column<long>(nullable: true),
                    Modifier3ID = table.Column<long>(nullable: true),
                    Modifier4ID = table.Column<long>(nullable: true),
                    TypeOfServiceID = table.Column<long>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    DefaultUnits = table.Column<string>(nullable: true),
                    UnitOfMeasurement = table.Column<string>(nullable: true),
                    CLIANumber = table.Column<string>(nullable: true),
                    NDCNumber = table.Column<string>(nullable: true),
                    NDCDescription = table.Column<string>(nullable: true),
                    NDCUnits = table.Column<string>(nullable: true),
                    NDCUnitOfMeasurement = table.Column<string>(nullable: true),
                    IsValid = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    AnesthesiaBaseUnits = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cpt", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Cpt_Modifier_Modifier1ID",
                        column: x => x.Modifier1ID,
                        principalTable: "Modifier",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cpt_Modifier_Modifier2ID",
                        column: x => x.Modifier2ID,
                        principalTable: "Modifier",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cpt_Modifier_Modifier3ID",
                        column: x => x.Modifier3ID,
                        principalTable: "Modifier",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cpt_Modifier_Modifier4ID",
                        column: x => x.Modifier4ID,
                        principalTable: "Modifier",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cpt_TypeOfService_TypeOfServiceID",
                        column: x => x.TypeOfServiceID,
                        principalTable: "TypeOfService",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Practice",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false),
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
                    UserPracticesUserID = table.Column<string>(nullable: true),
                    UserPracticesPracticeID = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Practice", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Practice_Client_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Client",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Practice_UserPractices_UserPracticesUserID_UserPracticesPracticeID",
                        columns: x => new { x.UserPracticesUserID, x.UserPracticesPracticeID },
                        principalTable: "UserPractices",
                        principalColumns: new[] { "UserID", "PracticeID" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InsurancePlan",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PlanName = table.Column<string>(maxLength: 30, nullable: false),
                    Description = table.Column<string>(maxLength: 80, nullable: true),
                    InsuranceID = table.Column<long>(nullable: true),
                    PlanTypeID = table.Column<long>(nullable: true),
                    IsCapitated = table.Column<bool>(nullable: false),
                    SubmissionType = table.Column<string>(nullable: true),
                    PayerID = table.Column<string>(nullable: true),
                    Edi837PayerID = table.Column<long>(nullable: true),
                    Edi270PayerID = table.Column<long>(nullable: true),
                    Edi276PayerID = table.Column<long>(nullable: true),
                    FormType = table.Column<string>(nullable: true),
                    OutstandingDays = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsurancePlan", x => x.ID);
                    table.ForeignKey(
                        name: "FK_InsurancePlan_Edi270Payer_Edi270PayerID",
                        column: x => x.Edi270PayerID,
                        principalTable: "Edi270Payer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InsurancePlan_Edi276Payer_Edi276PayerID",
                        column: x => x.Edi276PayerID,
                        principalTable: "Edi276Payer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InsurancePlan_Edi837Payer_Edi837PayerID",
                        column: x => x.Edi837PayerID,
                        principalTable: "Edi837Payer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InsurancePlan_Insurance_InsuranceID",
                        column: x => x.InsuranceID,
                        principalTable: "Insurance",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InsurancePlan_PlanType_PlanTypeID",
                        column: x => x.PlanTypeID,
                        principalTable: "PlanType",
                        principalColumn: "ID",
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
                    RightsId = table.Column<string>(nullable: true),
                    TeamID = table.Column<long>(nullable: true),
                    DesignationID = table.Column<long>(nullable: true),
                    ReportingTo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Client_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Client",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Designations_DesignationID",
                        column: x => x.DesignationID,
                        principalTable: "Designations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Practice_PracticeID",
                        column: x => x.PracticeID,
                        principalTable: "Practice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Rights_RightsId",
                        column: x => x.RightsId,
                        principalTable: "Rights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Team_TeamID",
                        column: x => x.TeamID,
                        principalTable: "Team",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    OrganizationName = table.Column<string>(maxLength: 50, nullable: false),
                    PracticeID = table.Column<long>(nullable: true),
                    NPI = table.Column<string>(maxLength: 10, nullable: true),
                    POSID = table.Column<long>(nullable: false),
                    Address1 = table.Column<string>(maxLength: 55, nullable: true),
                    Address2 = table.Column<string>(maxLength: 55, nullable: true),
                    City = table.Column<string>(maxLength: 20, nullable: true),
                    State = table.Column<string>(maxLength: 2, nullable: true),
                    ZipCode = table.Column<string>(maxLength: 9, nullable: true),
                    CLIANumber = table.Column<string>(maxLength: 10, nullable: true),
                    Fax = table.Column<string>(maxLength: 10, nullable: true),
                    website = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 10, nullable: true),
                    Notes = table.Column<string>(maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Location_POS_POSID",
                        column: x => x.POSID,
                        principalTable: "POS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Location_Practice_PracticeID",
                        column: x => x.PracticeID,
                        principalTable: "Practice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentCheck",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ReceiverID = table.Column<long>(nullable: true),
                    PracticeID = table.Column<long>(nullable: true),
                    DownloadedFileID = table.Column<long>(nullable: true),
                    CheckNumber = table.Column<string>(nullable: true),
                    CheckDate = table.Column<DateTime>(type: "Date", nullable: true),
                    CheckAmount = table.Column<decimal>(nullable: true),
                    TransactionCode = table.Column<string>(nullable: true),
                    CreditDebitFlag = table.Column<string>(nullable: true),
                    PaymentMethod = table.Column<string>(nullable: true),
                    PayerName = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    AppliedAmount = table.Column<decimal>(nullable: true),
                    PostedAmount = table.Column<decimal>(nullable: true),
                    PostedDate = table.Column<DateTime>(nullable: true),
                    PostedBy = table.Column<string>(nullable: true),
                    PayerID = table.Column<string>(nullable: true),
                    PayerAddress = table.Column<string>(maxLength: 55, nullable: true),
                    PayerCity = table.Column<string>(maxLength: 20, nullable: true),
                    PayerState = table.Column<string>(maxLength: 2, nullable: true),
                    PayerZipCode = table.Column<string>(maxLength: 9, nullable: true),
                    REF_2U_ID = table.Column<string>(nullable: true),
                    PayerContactPerson = table.Column<string>(nullable: true),
                    PayerContactNumber = table.Column<string>(nullable: true),
                    PayeeName = table.Column<string>(nullable: true),
                    PayeeNPI = table.Column<string>(nullable: true),
                    PayeeAddress = table.Column<string>(nullable: true),
                    PayeeCity = table.Column<string>(nullable: true),
                    PayeeState = table.Column<string>(nullable: true),
                    PayeeZipCode = table.Column<string>(nullable: true),
                    PayeeTaxID = table.Column<string>(nullable: true),
                    NumberOfVisits = table.Column<long>(nullable: false),
                    NumberOfPatients = table.Column<long>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentCheck", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PaymentCheck_Practice_PracticeID",
                        column: x => x.PracticeID,
                        principalTable: "Practice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentCheck_Receiver_ReceiverID",
                        column: x => x.ReceiverID,
                        principalTable: "Receiver",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Provider",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    LastName = table.Column<string>(maxLength: 35, nullable: false),
                    FirstName = table.Column<string>(maxLength: 35, nullable: false),
                    MiddleInitial = table.Column<string>(maxLength: 3, nullable: true),
                    Title = table.Column<string>(maxLength: 15, nullable: true),
                    NPI = table.Column<string>(maxLength: 10, nullable: false),
                    SSN = table.Column<string>(maxLength: 9, nullable: false),
                    TaxonomyCode = table.Column<string>(maxLength: 10, nullable: true),
                    Address1 = table.Column<string>(maxLength: 55, nullable: true),
                    Address2 = table.Column<string>(maxLength: 55, nullable: true),
                    City = table.Column<string>(maxLength: 20, nullable: true),
                    State = table.Column<string>(maxLength: 2, nullable: true),
                    ZipCode = table.Column<string>(maxLength: 9, nullable: true),
                    OfficePhoneNum = table.Column<string>(maxLength: 10, nullable: true),
                    FaxNumber = table.Column<string>(maxLength: 10, nullable: true),
                    PracticeID = table.Column<long>(nullable: false),
                    BillUnderProvider = table.Column<bool>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    DEANumber = table.Column<string>(nullable: true),
                    UPINNumber = table.Column<string>(nullable: true),
                    LicenceNumber = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(maxLength: 500, nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provider", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Provider_Practice_PracticeID",
                        column: x => x.PracticeID,
                        principalTable: "Practice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefProvider",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    LastName = table.Column<string>(maxLength: 35, nullable: false),
                    FirstName = table.Column<string>(maxLength: 35, nullable: false),
                    MiddleInitial = table.Column<string>(maxLength: 3, nullable: true),
                    Title = table.Column<string>(nullable: true),
                    NPI = table.Column<string>(maxLength: 10, nullable: true),
                    SSN = table.Column<string>(maxLength: 9, nullable: true),
                    TaxonomyCode = table.Column<string>(maxLength: 10, nullable: true),
                    TaxID = table.Column<string>(maxLength: 9, nullable: true),
                    Address1 = table.Column<string>(maxLength: 55, nullable: true),
                    Address2 = table.Column<string>(maxLength: 55, nullable: true),
                    City = table.Column<string>(maxLength: 20, nullable: true),
                    State = table.Column<string>(maxLength: 2, nullable: true),
                    ZipCode = table.Column<string>(maxLength: 9, nullable: true),
                    OfficePhoneNum = table.Column<string>(maxLength: 10, nullable: true),
                    FaxNumber = table.Column<string>(maxLength: 10, nullable: true),
                    PracticeID = table.Column<long>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Notes = table.Column<string>(maxLength: 500, nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefProvider", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RefProvider_Practice_PracticeID",
                        column: x => x.PracticeID,
                        principalTable: "Practice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InsurancePlanAddress",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InsurancePlanId = table.Column<long>(nullable: false),
                    Address1 = table.Column<string>(maxLength: 55, nullable: true),
                    Address2 = table.Column<string>(maxLength: 55, nullable: true),
                    City = table.Column<string>(maxLength: 20, nullable: true),
                    State = table.Column<string>(maxLength: 2, nullable: true),
                    ZipCode = table.Column<string>(maxLength: 9, nullable: true),
                    ZipCodeExtension = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 10, nullable: true),
                    FaxNumber = table.Column<string>(maxLength: 10, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Notes = table.Column<string>(maxLength: 500, nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsurancePlanAddress", x => x.ID);
                    table.ForeignKey(
                        name: "FK_InsurancePlanAddress_InsurancePlan_InsurancePlanId",
                        column: x => x.InsurancePlanId,
                        principalTable: "InsurancePlan",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Action",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    UserID = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Action", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Action_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
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

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    UserID = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Group_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reason",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    UserID = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reason", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Reason_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LocationAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LocationID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_LocationAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LocationAudit_Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BatchDocument",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    BillerID = table.Column<long>(nullable: true),
                    PracticeID = table.Column<long>(nullable: true),
                    LocationID = table.Column<long>(nullable: true),
                    ProviderID = table.Column<long>(nullable: true),
                    DocumentFilePath = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    NumberOfPages = table.Column<long>(nullable: false),
                    FileSize = table.Column<string>(nullable: true),
                    FileType = table.Column<string>(nullable: true),
                    DocumentTypeID = table.Column<long>(nullable: true),
                    ChargesNumberOfDOS = table.Column<long>(nullable: false),
                    ChargesNumberOfDOSEntered = table.Column<long>(nullable: false),
                    ChargesTotalCopay = table.Column<decimal>(nullable: true),
                    ChargesCopayApplied = table.Column<decimal>(nullable: true),
                    ChargesNumberOfVisits = table.Column<long>(nullable: false),
                    ChargesNumberOfVisitsEntered = table.Column<long>(nullable: false),
                    ChargesStartDate = table.Column<DateTime>(nullable: true),
                    ChargesEndTime = table.Column<DateTime>(nullable: true),
                    ChargeNotes = table.Column<string>(nullable: true),
                    AdmitNumberOfPatients = table.Column<long>(nullable: false),
                    AdmitNumberOfPatientsEntered = table.Column<long>(nullable: false),
                    AdmitStartDate = table.Column<DateTime>(nullable: true),
                    AdmitEndDate = table.Column<DateTime>(nullable: true),
                    AdmitNotes = table.Column<string>(nullable: true),
                    PaymentCheckDate = table.Column<DateTime>(nullable: true),
                    PaymentAssignedDate = table.Column<DateTime>(nullable: true),
                    PaymentTotalAmount = table.Column<decimal>(nullable: true),
                    PaymentPlanAmount = table.Column<decimal>(nullable: true),
                    PaymentPlanAppliedAmount = table.Column<decimal>(nullable: true),
                    PaymentPatientAmount = table.Column<decimal>(nullable: true),
                    PaymentPatientAppliedAmount = table.Column<decimal>(nullable: true),
                    PaymentCopay = table.Column<decimal>(nullable: true),
                    PaymentCopayApplied = table.Column<decimal>(nullable: true),
                    PaymentStartDate = table.Column<DateTime>(nullable: true),
                    PaymentEndDate = table.Column<DateTime>(nullable: true),
                    PaymentNotes = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchDocument", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BatchDocument_Biller_BillerID",
                        column: x => x.BillerID,
                        principalTable: "Biller",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BatchDocument_DocumentType_DocumentTypeID",
                        column: x => x.DocumentTypeID,
                        principalTable: "DocumentType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BatchDocument_Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BatchDocument_Practice_PracticeID",
                        column: x => x.PracticeID,
                        principalTable: "Practice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BatchDocument_Provider_ProviderID",
                        column: x => x.ProviderID,
                        principalTable: "Provider",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProviderSchedule",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProviderID = table.Column<long>(nullable: false),
                    LocationID = table.Column<long>(nullable: true),
                    FromDate = table.Column<DateTime>(type: "Date", nullable: true),
                    ToDate = table.Column<DateTime>(type: "Date", nullable: true),
                    FromTime = table.Column<DateTime>(nullable: true),
                    ToTime = table.Column<DateTime>(nullable: true),
                    TimeInterval = table.Column<int>(nullable: false),
                    OverBookAllowed = table.Column<bool>(nullable: false),
                    Friday = table.Column<bool>(nullable: false),
                    Saturday = table.Column<bool>(nullable: false),
                    Sunday = table.Column<bool>(nullable: false),
                    Monday = table.Column<bool>(nullable: false),
                    Tuesday = table.Column<bool>(nullable: false),
                    Wednesday = table.Column<bool>(nullable: false),
                    Thursday = table.Column<bool>(nullable: false),
                    Notes = table.Column<string>(maxLength: 500, nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderSchedule", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProviderSchedule_Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProviderSchedule_Provider_ProviderID",
                        column: x => x.ProviderID,
                        principalTable: "Provider",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdjustmentCode",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 3, nullable: false),
                    ActionID = table.Column<long>(nullable: true),
                    ReasonID = table.Column<long>(nullable: true),
                    GroupID = table.Column<long>(nullable: true),
                    Description = table.Column<string>(maxLength: 300, nullable: true),
                    Type = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdjustmentCode", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AdjustmentCode_Action_ActionID",
                        column: x => x.ActionID,
                        principalTable: "Action",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdjustmentCode_Group_GroupID",
                        column: x => x.GroupID,
                        principalTable: "Group",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdjustmentCode_Reason_ReasonID",
                        column: x => x.ReasonID,
                        principalTable: "Reason",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProfilePic = table.Column<string>(nullable: true),
                    AccountNum = table.Column<string>(nullable: false),
                    MedicalRecordNumber = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(maxLength: 30, nullable: true),
                    FirstName = table.Column<string>(maxLength: 30, nullable: true),
                    MiddleInitial = table.Column<string>(maxLength: 3, nullable: true),
                    Title = table.Column<string>(nullable: true),
                    SSN = table.Column<string>(maxLength: 9, nullable: true),
                    DOB = table.Column<DateTime>(type: "Date", nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    MaritalStatus = table.Column<string>(nullable: true),
                    Race = table.Column<string>(nullable: true),
                    Ethnicity = table.Column<string>(nullable: true),
                    Address1 = table.Column<string>(maxLength: 55, nullable: true),
                    Address2 = table.Column<string>(maxLength: 55, nullable: true),
                    City = table.Column<string>(maxLength: 20, nullable: true),
                    State = table.Column<string>(maxLength: 2, nullable: true),
                    ZipCode = table.Column<string>(maxLength: 9, nullable: true),
                    ZipCodeExtension = table.Column<string>(nullable: true),
                    MobileNumber = table.Column<string>(maxLength: 10, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 10, nullable: true),
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
                    table.PrimaryKey("PK_Patient", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Patient_BatchDocument_BatchDocumentID",
                        column: x => x.BatchDocumentID,
                        principalTable: "BatchDocument",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patient_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patient_Practice_PracticeID",
                        column: x => x.PracticeID,
                        principalTable: "Practice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patient_Provider_ProviderID",
                        column: x => x.ProviderID,
                        principalTable: "Provider",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patient_RefProvider_RefProviderID",
                        column: x => x.RefProviderID,
                        principalTable: "RefProvider",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProviderSlot",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProviderScheduleID = table.Column<long>(nullable: false),
                    FromDate = table.Column<DateTime>(type: "Date", nullable: true),
                    ToDate = table.Column<DateTime>(type: "Date", nullable: true),
                    FromTime = table.Column<DateTime>(nullable: true),
                    ToTime = table.Column<DateTime>(nullable: true),
                    TimeInterval = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderSlot", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProviderSlot_ProviderSchedule_ProviderScheduleID",
                        column: x => x.ProviderScheduleID,
                        principalTable: "ProviderSchedule",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientFollowUp",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientID = table.Column<long>(nullable: true),
                    ReasonID = table.Column<long>(nullable: true),
                    ActionID = table.Column<long>(nullable: true),
                    GroupID = table.Column<long>(nullable: true),
                    PaymentVisitID = table.Column<long>(nullable: true),
                    AdjustmentCodeID = table.Column<long>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    TickleDate = table.Column<DateTime>(type: "Date", nullable: true),
                    Statement1SentDate = table.Column<DateTime>(nullable: true),
                    Statement2SentDate = table.Column<DateTime>(nullable: true),
                    Statement3SentDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientFollowUp", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientFollowUp_Action_ActionID",
                        column: x => x.ActionID,
                        principalTable: "Action",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientFollowUp_Group_GroupID",
                        column: x => x.GroupID,
                        principalTable: "Group",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientFollowUp_Patient_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patient",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientFollowUp_Reason_ReasonID",
                        column: x => x.ReasonID,
                        principalTable: "Reason",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientPayment",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientID = table.Column<long>(nullable: true),
                    PaymentMethod = table.Column<string>(nullable: true),
                    PaymentDate = table.Column<DateTime>(nullable: true),
                    PaymentAmount = table.Column<decimal>(nullable: true),
                    CheckNumber = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    AllocatedAmount = table.Column<decimal>(nullable: true),
                    RemainingAmount = table.Column<decimal>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientPayment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientPayment_Patient_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patient",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientPlan",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientID = table.Column<long>(nullable: false),
                    InsurancePlanID = table.Column<long>(nullable: true),
                    Coverage = table.Column<string>(nullable: true),
                    RelationShip = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(maxLength: 30, nullable: false),
                    FirstName = table.Column<string>(maxLength: 30, nullable: false),
                    MiddleInitial = table.Column<string>(maxLength: 3, nullable: true),
                    SSN = table.Column<string>(maxLength: 9, nullable: true),
                    DOB = table.Column<DateTime>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Address1 = table.Column<string>(maxLength: 55, nullable: true),
                    Address2 = table.Column<string>(maxLength: 55, nullable: true),
                    City = table.Column<string>(maxLength: 20, nullable: true),
                    State = table.Column<string>(maxLength: 2, nullable: true),
                    ZipCode = table.Column<string>(maxLength: 9, nullable: true),
                    ZipCodeExtension = table.Column<string>(nullable: true),
                    SubscriberId = table.Column<string>(nullable: true),
                    GroupName = table.Column<string>(nullable: true),
                    PlanBeginDate = table.Column<DateTime>(nullable: true),
                    PlanEndDate = table.Column<DateTime>(nullable: true),
                    Copay = table.Column<decimal>(nullable: true),
                    Deductible = table.Column<decimal>(nullable: true),
                    CoInsurance = table.Column<decimal>(nullable: true),
                    EmlpoyerID = table.Column<long>(nullable: true),
                    InsurancePlanAddressID = table.Column<long>(nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 10, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsSelfPay = table.Column<bool>(nullable: true),
                    Notes = table.Column<string>(maxLength: 500, nullable: true),
                    BatchDocumentID = table.Column<long>(nullable: true),
                    PageNumber = table.Column<string>(nullable: true),
                    FrontSideCard = table.Column<string>(nullable: true),
                    backSidecard = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientPlan", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientPlan_BatchDocument_BatchDocumentID",
                        column: x => x.BatchDocumentID,
                        principalTable: "BatchDocument",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientPlan_InsurancePlanAddress_InsurancePlanAddressID",
                        column: x => x.InsurancePlanAddressID,
                        principalTable: "InsurancePlanAddress",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientPlan_InsurancePlan_InsurancePlanID",
                        column: x => x.InsurancePlanID,
                        principalTable: "InsurancePlan",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientPlan_Patient_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patient",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientAppointment",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientID = table.Column<long>(nullable: true),
                    LocationID = table.Column<long>(nullable: true),
                    ProviderID = table.Column<long>(nullable: true),
                    ProviderSlotID = table.Column<long>(nullable: true),
                    PrimarypatientPlanID = table.Column<long>(nullable: true),
                    VisitReasonID = table.Column<long>(nullable: true),
                    AppointmentDate = table.Column<DateTime>(nullable: true),
                    Time = table.Column<DateTime>(nullable: true),
                    VisitInterval = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(maxLength: 500, nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAppointment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientAppointment_Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientAppointment_Patient_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patient",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientAppointment_PatientPlan_PrimarypatientPlanID",
                        column: x => x.PrimarypatientPlanID,
                        principalTable: "PatientPlan",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientAppointment_Provider_ProviderID",
                        column: x => x.ProviderID,
                        principalTable: "Provider",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientAppointment_ProviderSlot_ProviderSlotID",
                        column: x => x.ProviderSlotID,
                        principalTable: "ProviderSlot",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientAppointment_VisitReason_VisitReasonID",
                        column: x => x.VisitReasonID,
                        principalTable: "VisitReason",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Visit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PageNumber = table.Column<string>(nullable: true),
                    BatchDocumentID = table.Column<long>(nullable: true),
                    ClientID = table.Column<long>(nullable: false),
                    PracticeID = table.Column<long>(nullable: false),
                    LocationID = table.Column<long>(nullable: false),
                    POSID = table.Column<long>(nullable: false),
                    ProviderID = table.Column<long>(nullable: false),
                    RefProviderID = table.Column<long>(nullable: true),
                    SupervisingProvID = table.Column<long>(nullable: true),
                    OrderingProvID = table.Column<long>(nullable: true),
                    PatientID = table.Column<long>(nullable: false),
                    PrimaryPatientPlanID = table.Column<long>(nullable: true),
                    SecondaryPatientPlanID = table.Column<long>(nullable: true),
                    TertiaryPatientPlanID = table.Column<long>(nullable: true),
                    SubmissionLogID = table.Column<long>(nullable: true),
                    RejectionReason = table.Column<string>(nullable: true),
                    ICD1ID = table.Column<long>(nullable: false),
                    ICD2ID = table.Column<long>(nullable: true),
                    ICD3ID = table.Column<long>(nullable: true),
                    ICD4ID = table.Column<long>(nullable: true),
                    ICD5ID = table.Column<long>(nullable: true),
                    ICD6ID = table.Column<long>(nullable: true),
                    ICD7ID = table.Column<long>(nullable: true),
                    ICD8ID = table.Column<long>(nullable: true),
                    ICD9ID = table.Column<long>(nullable: true),
                    ICD10ID = table.Column<long>(nullable: true),
                    ICD11ID = table.Column<long>(nullable: true),
                    ICD12ID = table.Column<long>(nullable: true),
                    AuthorizationNum = table.Column<string>(nullable: true),
                    OutsideReferral = table.Column<bool>(nullable: false),
                    IsForcePaper = table.Column<bool>(nullable: true),
                    IsDontPrint = table.Column<bool>(nullable: true),
                    ReferralNum = table.Column<string>(nullable: true),
                    OnsetDateOfIllness = table.Column<DateTime>(type: "Date", nullable: true),
                    FirstDateOfSimiliarIllness = table.Column<DateTime>(type: "Date", nullable: true),
                    IllnessTreatmentDate = table.Column<DateTime>(type: "Date", nullable: true),
                    DateOfPregnancy = table.Column<DateTime>(type: "Date", nullable: true),
                    AdmissionDate = table.Column<DateTime>(type: "Date", nullable: true),
                    DischargeDate = table.Column<DateTime>(type: "Date", nullable: true),
                    LastXrayDate = table.Column<DateTime>(type: "Date", nullable: true),
                    LastXrayType = table.Column<string>(nullable: true),
                    UnableToWorkFromDate = table.Column<DateTime>(type: "Date", nullable: true),
                    UnableToWorkToDate = table.Column<DateTime>(type: "Date", nullable: true),
                    AccidentDate = table.Column<DateTime>(type: "Date", nullable: true),
                    AccidentState = table.Column<string>(maxLength: 2, nullable: true),
                    AccidentType = table.Column<string>(nullable: true),
                    CliaNumber = table.Column<string>(nullable: true),
                    OutsideLab = table.Column<bool>(nullable: true),
                    LabCharges = table.Column<decimal>(nullable: true),
                    ClaimNotes = table.Column<string>(nullable: true),
                    PayerClaimControlNum = table.Column<string>(nullable: true),
                    ClaimFrequencyCode = table.Column<string>(nullable: true),
                    ServiceAuthExcpCode = table.Column<string>(nullable: true),
                    ValidationMessage = table.Column<string>(nullable: true),
                    Emergency = table.Column<bool>(nullable: true),
                    EPSDT = table.Column<bool>(nullable: true),
                    FamilyPlan = table.Column<bool>(nullable: true),
                    TotalAmount = table.Column<decimal>(nullable: true),
                    Copay = table.Column<decimal>(nullable: true),
                    Coinsurance = table.Column<decimal>(nullable: true),
                    Deductible = table.Column<decimal>(nullable: true),
                    PrimaryStatus = table.Column<string>(nullable: true),
                    PrimaryBilledAmount = table.Column<decimal>(nullable: true),
                    PrimaryAllowed = table.Column<decimal>(nullable: true),
                    PrimaryWriteOff = table.Column<decimal>(nullable: true),
                    PrimaryPaid = table.Column<decimal>(nullable: true),
                    PrimaryBal = table.Column<decimal>(nullable: true),
                    PrimaryPatientBal = table.Column<decimal>(nullable: true),
                    PrimaryTransferred = table.Column<decimal>(nullable: true),
                    SecondaryStatus = table.Column<string>(nullable: true),
                    SecondaryBilledAmount = table.Column<decimal>(nullable: true),
                    SecondaryAllowed = table.Column<decimal>(nullable: true),
                    SecondaryWriteOff = table.Column<decimal>(nullable: true),
                    SecondaryPaid = table.Column<decimal>(nullable: true),
                    SecondaryPatResp = table.Column<decimal>(nullable: true),
                    SecondaryBal = table.Column<decimal>(nullable: true),
                    SecondaryPatientBal = table.Column<decimal>(nullable: true),
                    SecondaryTransferred = table.Column<decimal>(nullable: true),
                    TertiaryStatus = table.Column<string>(nullable: true),
                    TertiaryBilledAmount = table.Column<decimal>(nullable: true),
                    TertiaryAllowed = table.Column<decimal>(nullable: true),
                    TertiaryWriteOff = table.Column<decimal>(nullable: true),
                    TertiaryPaid = table.Column<decimal>(nullable: true),
                    TertiaryPatResp = table.Column<decimal>(nullable: true),
                    TertiaryBal = table.Column<decimal>(nullable: true),
                    TertiaryPatientBal = table.Column<decimal>(nullable: true),
                    TertiaryTransferred = table.Column<decimal>(nullable: true),
                    PatientAmount = table.Column<decimal>(nullable: true),
                    PatientPaid = table.Column<decimal>(nullable: true),
                    IsSubmitted = table.Column<bool>(nullable: false),
                    SubmittedDate = table.Column<DateTime>(type: "Date", nullable: true),
                    DateOfServiceFrom = table.Column<DateTime>(type: "Date", nullable: true),
                    DateOfServiceTo = table.Column<DateTime>(type: "Date", nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Visit_BatchDocument_BatchDocumentID",
                        column: x => x.BatchDocumentID,
                        principalTable: "BatchDocument",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visit_Client_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Client",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Visit_ICD_ICD10ID",
                        column: x => x.ICD10ID,
                        principalTable: "ICD",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visit_ICD_ICD11ID",
                        column: x => x.ICD11ID,
                        principalTable: "ICD",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visit_ICD_ICD12ID",
                        column: x => x.ICD12ID,
                        principalTable: "ICD",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visit_ICD_ICD1ID",
                        column: x => x.ICD1ID,
                        principalTable: "ICD",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Visit_ICD_ICD2ID",
                        column: x => x.ICD2ID,
                        principalTable: "ICD",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visit_ICD_ICD3ID",
                        column: x => x.ICD3ID,
                        principalTable: "ICD",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visit_ICD_ICD4ID",
                        column: x => x.ICD4ID,
                        principalTable: "ICD",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visit_ICD_ICD5ID",
                        column: x => x.ICD5ID,
                        principalTable: "ICD",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visit_ICD_ICD6ID",
                        column: x => x.ICD6ID,
                        principalTable: "ICD",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visit_ICD_ICD7ID",
                        column: x => x.ICD7ID,
                        principalTable: "ICD",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visit_ICD_ICD8ID",
                        column: x => x.ICD8ID,
                        principalTable: "ICD",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visit_ICD_ICD9ID",
                        column: x => x.ICD9ID,
                        principalTable: "ICD",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visit_Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Visit_Provider_OrderingProvID",
                        column: x => x.OrderingProvID,
                        principalTable: "Provider",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visit_POS_POSID",
                        column: x => x.POSID,
                        principalTable: "POS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Visit_Patient_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patient",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Visit_Practice_PracticeID",
                        column: x => x.PracticeID,
                        principalTable: "Practice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Visit_PatientPlan_PrimaryPatientPlanID",
                        column: x => x.PrimaryPatientPlanID,
                        principalTable: "PatientPlan",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visit_Provider_ProviderID",
                        column: x => x.ProviderID,
                        principalTable: "Provider",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Visit_RefProvider_RefProviderID",
                        column: x => x.RefProviderID,
                        principalTable: "RefProvider",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visit_PatientPlan_SecondaryPatientPlanID",
                        column: x => x.SecondaryPatientPlanID,
                        principalTable: "PatientPlan",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visit_SubmissionLog_SubmissionLogID",
                        column: x => x.SubmissionLogID,
                        principalTable: "SubmissionLog",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visit_Provider_SupervisingProvID",
                        column: x => x.SupervisingProvID,
                        principalTable: "Provider",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visit_PatientPlan_TertiaryPatientPlanID",
                        column: x => x.TertiaryPatientPlanID,
                        principalTable: "PatientPlan",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Charge",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VisitID = table.Column<long>(nullable: true),
                    ClientID = table.Column<long>(nullable: false),
                    PracticeID = table.Column<long>(nullable: false),
                    LocationID = table.Column<long>(nullable: false),
                    POSID = table.Column<long>(nullable: false),
                    ProviderID = table.Column<long>(nullable: false),
                    RefProviderID = table.Column<long>(nullable: true),
                    SupervisingProvID = table.Column<long>(nullable: true),
                    OrderingProvID = table.Column<long>(nullable: true),
                    PatientID = table.Column<long>(nullable: false),
                    PrimaryPatientPlanID = table.Column<long>(nullable: true),
                    SecondaryPatientPlanID = table.Column<long>(nullable: true),
                    TertiaryPatientPlanID = table.Column<long>(nullable: true),
                    SubmissionLogID = table.Column<long>(nullable: true),
                    CPTID = table.Column<long>(nullable: false),
                    Modifier1ID = table.Column<long>(nullable: true),
                    Modifier1Amount = table.Column<decimal>(nullable: true),
                    Modifier2ID = table.Column<long>(nullable: true),
                    Modifier2Amount = table.Column<decimal>(nullable: true),
                    Modifier3ID = table.Column<long>(nullable: true),
                    Modifier3Amount = table.Column<decimal>(nullable: true),
                    Modifier4ID = table.Column<long>(nullable: true),
                    Modifier4Amount = table.Column<decimal>(nullable: true),
                    Units = table.Column<string>(nullable: true),
                    Minutes = table.Column<string>(nullable: true),
                    NdcUnits = table.Column<int>(nullable: false),
                    NdcMeasurementUnit = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    NdcNumber = table.Column<string>(nullable: true),
                    UnitOfMeasurement = table.Column<string>(nullable: true),
                    DateOfServiceFrom = table.Column<DateTime>(type: "Date", nullable: false),
                    DateOfServiceTo = table.Column<DateTime>(type: "Date", nullable: true),
                    StartTime = table.Column<DateTime>(nullable: true),
                    EndTime = table.Column<DateTime>(nullable: true),
                    TimeUnits = table.Column<int>(nullable: true),
                    BaseUnits = table.Column<int>(nullable: true),
                    ModifierUnits = table.Column<int>(nullable: true),
                    Pointer1 = table.Column<string>(nullable: true),
                    Pointer2 = table.Column<string>(nullable: true),
                    Pointer3 = table.Column<string>(nullable: true),
                    Pointer4 = table.Column<string>(nullable: true),
                    TotalAmount = table.Column<decimal>(nullable: false),
                    Copay = table.Column<decimal>(nullable: true),
                    Coinsurance = table.Column<decimal>(nullable: true),
                    Deductible = table.Column<decimal>(nullable: true),
                    PrimaryBilledAmount = table.Column<decimal>(nullable: true),
                    PrimaryAllowed = table.Column<decimal>(nullable: true),
                    PrimaryWriteOff = table.Column<decimal>(nullable: true),
                    PrimaryPaid = table.Column<decimal>(nullable: true),
                    PrimaryBal = table.Column<decimal>(nullable: true),
                    PrimaryPatientBal = table.Column<decimal>(nullable: true),
                    PrimaryTransferred = table.Column<decimal>(nullable: true),
                    PrimaryStatus = table.Column<string>(nullable: true),
                    SecondaryBilledAmount = table.Column<decimal>(nullable: true),
                    SecondaryAllowed = table.Column<decimal>(nullable: true),
                    SecondaryWriteOff = table.Column<decimal>(nullable: true),
                    SecondaryPaid = table.Column<decimal>(nullable: true),
                    SecondaryPatResp = table.Column<decimal>(nullable: true),
                    SecondaryBal = table.Column<decimal>(nullable: true),
                    SecondaryPatientBal = table.Column<decimal>(nullable: true),
                    SecondaryTransferred = table.Column<decimal>(nullable: true),
                    SecondaryStatus = table.Column<string>(nullable: true),
                    TertiaryBilledAmount = table.Column<decimal>(nullable: true),
                    TertiaryAllowed = table.Column<decimal>(nullable: true),
                    TertiaryWriteOff = table.Column<decimal>(nullable: true),
                    TertiaryPaid = table.Column<decimal>(nullable: true),
                    TertiaryPatResp = table.Column<decimal>(nullable: true),
                    TertiaryBal = table.Column<decimal>(nullable: true),
                    TertiaryPatientBal = table.Column<decimal>(nullable: true),
                    TertiaryTransferred = table.Column<decimal>(nullable: true),
                    TertiaryStatus = table.Column<string>(nullable: true),
                    PatientAmount = table.Column<decimal>(nullable: true),
                    PatientPaid = table.Column<decimal>(nullable: true),
                    IsSubmitted = table.Column<bool>(nullable: false),
                    IsDontPrint = table.Column<bool>(nullable: true),
                    SubmittetdDate = table.Column<DateTime>(type: "Date", nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Charge", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Charge_Cpt_CPTID",
                        column: x => x.CPTID,
                        principalTable: "Cpt",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Charge_Client_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Client",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Charge_Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Charge_Modifier_Modifier1ID",
                        column: x => x.Modifier1ID,
                        principalTable: "Modifier",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Charge_Modifier_Modifier2ID",
                        column: x => x.Modifier2ID,
                        principalTable: "Modifier",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Charge_Modifier_Modifier3ID",
                        column: x => x.Modifier3ID,
                        principalTable: "Modifier",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Charge_Modifier_Modifier4ID",
                        column: x => x.Modifier4ID,
                        principalTable: "Modifier",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Charge_Provider_OrderingProvID",
                        column: x => x.OrderingProvID,
                        principalTable: "Provider",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Charge_POS_POSID",
                        column: x => x.POSID,
                        principalTable: "POS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Charge_Patient_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patient",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Charge_Practice_PracticeID",
                        column: x => x.PracticeID,
                        principalTable: "Practice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Charge_PatientPlan_PrimaryPatientPlanID",
                        column: x => x.PrimaryPatientPlanID,
                        principalTable: "PatientPlan",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Charge_Provider_ProviderID",
                        column: x => x.ProviderID,
                        principalTable: "Provider",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Charge_RefProvider_RefProviderID",
                        column: x => x.RefProviderID,
                        principalTable: "RefProvider",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Charge_PatientPlan_SecondaryPatientPlanID",
                        column: x => x.SecondaryPatientPlanID,
                        principalTable: "PatientPlan",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Charge_SubmissionLog_SubmissionLogID",
                        column: x => x.SubmissionLogID,
                        principalTable: "SubmissionLog",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Charge_Provider_SupervisingProvID",
                        column: x => x.SupervisingProvID,
                        principalTable: "Provider",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Charge_PatientPlan_TertiaryPatientPlanID",
                        column: x => x.TertiaryPatientPlanID,
                        principalTable: "PatientPlan",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Charge_Visit_VisitID",
                        column: x => x.VisitID,
                        principalTable: "Visit",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentVisit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PaymentCheckID = table.Column<long>(nullable: true),
                    VisitID = table.Column<long>(nullable: true),
                    PatientID = table.Column<long>(nullable: true),
                    ClaimNumber = table.Column<string>(nullable: true),
                    BatchDocumentID = table.Column<long>(nullable: true),
                    PageNumber = table.Column<string>(nullable: true),
                    MyProperty = table.Column<int>(nullable: false),
                    ProcessedAs = table.Column<string>(nullable: true),
                    BilledAmount = table.Column<decimal>(nullable: true),
                    PaidAmount = table.Column<decimal>(nullable: true),
                    AllowedAmount = table.Column<decimal>(nullable: true),
                    WriteOffAmount = table.Column<decimal>(nullable: true),
                    PatientAmount = table.Column<decimal>(nullable: true),
                    PayerICN = table.Column<string>(nullable: true),
                    PatientLastName = table.Column<string>(maxLength: 30, nullable: true),
                    PatientFIrstName = table.Column<string>(maxLength: 30, nullable: true),
                    InsuredLastName = table.Column<string>(maxLength: 30, nullable: true),
                    InsuredFirstName = table.Column<string>(maxLength: 30, nullable: true),
                    InsuredID = table.Column<string>(nullable: true),
                    ProvLastName = table.Column<string>(maxLength: 30, nullable: true),
                    ProvFirstName = table.Column<string>(maxLength: 30, nullable: true),
                    ProvNPI = table.Column<string>(nullable: true),
                    PayerContactNumber = table.Column<string>(maxLength: 10, nullable: true),
                    ForwardedPayerName = table.Column<string>(nullable: true),
                    ForwardedPayerID = table.Column<string>(nullable: true),
                    ClaimStatementFromDate = table.Column<DateTime>(nullable: true),
                    ClaimStatementToDate = table.Column<DateTime>(nullable: true),
                    PayerReceivedDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentVisit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PaymentVisit_BatchDocument_BatchDocumentID",
                        column: x => x.BatchDocumentID,
                        principalTable: "BatchDocument",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentVisit_Patient_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patient",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentVisit_PaymentCheck_PaymentCheckID",
                        column: x => x.PaymentCheckID,
                        principalTable: "PaymentCheck",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentVisit_Visit_VisitID",
                        column: x => x.VisitID,
                        principalTable: "Visit",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VisitAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VisitID = table.Column<long>(nullable: false),
                    TransactionID = table.Column<long>(nullable: false),
                    ColumnName = table.Column<string>(nullable: true),
                    CurrentValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    HostName = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_VisitAudit_Visit_VisitID",
                        column: x => x.VisitID,
                        principalTable: "Visit",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChargeAudit",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChargeID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_ChargeAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ChargeAudit_Charge_ChargeID",
                        column: x => x.ChargeID,
                        principalTable: "Charge",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChargeSubmissionHistory",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChargeID = table.Column<long>(nullable: true),
                    ReceiverID = table.Column<long>(nullable: true),
                    SubmissionLogID = table.Column<long>(nullable: true),
                    SubmitType = table.Column<string>(nullable: true),
                    FormType = table.Column<string>(nullable: true),
                    PatientPlanID = table.Column<long>(nullable: true),
                    Amount = table.Column<decimal>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargeSubmissionHistory", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ChargeSubmissionHistory_Charge_ChargeID",
                        column: x => x.ChargeID,
                        principalTable: "Charge",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChargeSubmissionHistory_PatientPlan_PatientPlanID",
                        column: x => x.PatientPlanID,
                        principalTable: "PatientPlan",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChargeSubmissionHistory_Receiver_ReceiverID",
                        column: x => x.ReceiverID,
                        principalTable: "Receiver",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChargeSubmissionHistory_SubmissionLog_SubmissionLogID",
                        column: x => x.SubmissionLogID,
                        principalTable: "SubmissionLog",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientFollowUpCharge",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientFollowUpID = table.Column<long>(nullable: true),
                    ChargeID = table.Column<long>(nullable: true),
                    ReasonID = table.Column<long>(nullable: true),
                    ActionID = table.Column<long>(nullable: true),
                    GroupID = table.Column<long>(nullable: true),
                    AdjustmentCodeID = table.Column<long>(nullable: true),
                    PaymentChargeID = table.Column<long>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientFollowUpCharge", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientFollowUpCharge_Action_ActionID",
                        column: x => x.ActionID,
                        principalTable: "Action",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientFollowUpCharge_Charge_ChargeID",
                        column: x => x.ChargeID,
                        principalTable: "Charge",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientFollowUpCharge_Group_GroupID",
                        column: x => x.GroupID,
                        principalTable: "Group",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientFollowUpCharge_PatientFollowUp_PatientFollowUpID",
                        column: x => x.PatientFollowUpID,
                        principalTable: "PatientFollowUp",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientFollowUpCharge_Reason_ReasonID",
                        column: x => x.ReasonID,
                        principalTable: "Reason",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientPaymentCharge",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientPaymentID = table.Column<long>(nullable: false),
                    VisitID = table.Column<long>(nullable: true),
                    ChargeID = table.Column<long>(nullable: true),
                    AllocatedAmount = table.Column<decimal>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientPaymentCharge", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientPaymentCharge_Charge_ChargeID",
                        column: x => x.ChargeID,
                        principalTable: "Charge",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientPaymentCharge_PatientPayment_PatientPaymentID",
                        column: x => x.PatientPaymentID,
                        principalTable: "PatientPayment",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientPaymentCharge_Visit_VisitID",
                        column: x => x.VisitID,
                        principalTable: "Visit",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResubmitHistory",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChargeID = table.Column<long>(nullable: true),
                    VisitID = table.Column<long>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResubmitHistory", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ResubmitHistory_Charge_ChargeID",
                        column: x => x.ChargeID,
                        principalTable: "Charge",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResubmitHistory_Visit_VisitID",
                        column: x => x.VisitID,
                        principalTable: "Visit",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentCharge",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PaymentVisitID = table.Column<long>(nullable: true),
                    ChargeID = table.Column<long>(nullable: true),
                    AppliedToSec = table.Column<bool>(nullable: false),
                    CPTCode = table.Column<string>(nullable: true),
                    Modifier1 = table.Column<string>(nullable: true),
                    Modifier2 = table.Column<string>(nullable: true),
                    Modifier3 = table.Column<string>(nullable: true),
                    Modifier4 = table.Column<string>(nullable: true),
                    BilledAmount = table.Column<decimal>(nullable: true),
                    PaidAmount = table.Column<decimal>(nullable: true),
                    RevenueCode = table.Column<string>(nullable: true),
                    Units = table.Column<string>(nullable: true),
                    DOSFrom = table.Column<DateTime>(nullable: true),
                    DOSTo = table.Column<DateTime>(nullable: true),
                    ChargeControlNumber = table.Column<string>(nullable: true),
                    Copay = table.Column<decimal>(nullable: true),
                    PatientAmount = table.Column<decimal>(nullable: true),
                    DeductableAmount = table.Column<decimal>(nullable: true),
                    CoinsuranceAmount = table.Column<decimal>(nullable: true),
                    WriteoffAmount = table.Column<decimal>(nullable: true),
                    AllowedAmount = table.Column<decimal>(nullable: true),
                    AdjustmentCodeID1 = table.Column<long>(nullable: true),
                    GroupCode1 = table.Column<string>(nullable: true),
                    AdjustmentAmount1 = table.Column<decimal>(nullable: true),
                    AdjustmentQuantity1 = table.Column<string>(nullable: true),
                    AdjustmentCodeID2 = table.Column<long>(nullable: true),
                    GroupCode2 = table.Column<string>(nullable: true),
                    AdjustmentAmount2 = table.Column<decimal>(nullable: true),
                    AdjustmentQuantity2 = table.Column<string>(nullable: true),
                    AdjustmentCodeID3 = table.Column<long>(nullable: true),
                    GroupCode3 = table.Column<string>(nullable: true),
                    AdjustmentAmount3 = table.Column<decimal>(nullable: true),
                    AdjustmentQuantity3 = table.Column<string>(nullable: true),
                    AdjustmentCodeID4 = table.Column<long>(nullable: true),
                    GroupCode4 = table.Column<string>(nullable: true),
                    AdjustmentAmount4 = table.Column<decimal>(nullable: true),
                    AdjustmentQuantity4 = table.Column<string>(nullable: true),
                    AdjustmentCodeID5 = table.Column<long>(nullable: true),
                    GroupCode5 = table.Column<string>(nullable: true),
                    AdjustmentAmount5 = table.Column<decimal>(nullable: true),
                    AdjustmentQuantity5 = table.Column<string>(nullable: true),
                    RemarkCodeID1 = table.Column<long>(nullable: true),
                    RemarkCodeID2 = table.Column<long>(nullable: true),
                    RemarkCodeID3 = table.Column<long>(nullable: true),
                    RemarkCodeID4 = table.Column<long>(nullable: true),
                    RemarkCodeID5 = table.Column<long>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentCharge", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PaymentCharge_AdjustmentCode_AdjustmentCodeID1",
                        column: x => x.AdjustmentCodeID1,
                        principalTable: "AdjustmentCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentCharge_AdjustmentCode_AdjustmentCodeID2",
                        column: x => x.AdjustmentCodeID2,
                        principalTable: "AdjustmentCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentCharge_AdjustmentCode_AdjustmentCodeID3",
                        column: x => x.AdjustmentCodeID3,
                        principalTable: "AdjustmentCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentCharge_AdjustmentCode_AdjustmentCodeID4",
                        column: x => x.AdjustmentCodeID4,
                        principalTable: "AdjustmentCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentCharge_AdjustmentCode_AdjustmentCodeID5",
                        column: x => x.AdjustmentCodeID5,
                        principalTable: "AdjustmentCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentCharge_Charge_ChargeID",
                        column: x => x.ChargeID,
                        principalTable: "Charge",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentCharge_PaymentVisit_PaymentVisitID",
                        column: x => x.PaymentVisitID,
                        principalTable: "PaymentVisit",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentCharge_RemarkCode_RemarkCodeID1",
                        column: x => x.RemarkCodeID1,
                        principalTable: "RemarkCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentCharge_RemarkCode_RemarkCodeID2",
                        column: x => x.RemarkCodeID2,
                        principalTable: "RemarkCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentCharge_RemarkCode_RemarkCodeID3",
                        column: x => x.RemarkCodeID3,
                        principalTable: "RemarkCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentCharge_RemarkCode_RemarkCodeID4",
                        column: x => x.RemarkCodeID4,
                        principalTable: "RemarkCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentCharge_RemarkCode_RemarkCodeID5",
                        column: x => x.RemarkCodeID5,
                        principalTable: "RemarkCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanFollowUp",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VisitID = table.Column<long>(nullable: true),
                    GroupID = table.Column<long>(nullable: true),
                    ReasonID = table.Column<long>(nullable: true),
                    ActionID = table.Column<long>(nullable: true),
                    AdjustmentCodeID = table.Column<long>(nullable: true),
                    VisitStatusID = table.Column<long>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    PaymentVisitID = table.Column<long>(nullable: true),
                    TickleDate = table.Column<DateTime>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    RemitCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanFollowUp", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PlanFollowUp_Action_ActionID",
                        column: x => x.ActionID,
                        principalTable: "Action",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanFollowUp_AdjustmentCode_AdjustmentCodeID",
                        column: x => x.AdjustmentCodeID,
                        principalTable: "AdjustmentCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanFollowUp_Group_GroupID",
                        column: x => x.GroupID,
                        principalTable: "Group",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanFollowUp_PaymentVisit_PaymentVisitID",
                        column: x => x.PaymentVisitID,
                        principalTable: "PaymentVisit",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanFollowUp_Reason_ReasonID",
                        column: x => x.ReasonID,
                        principalTable: "Reason",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanFollowUp_Visit_VisitID",
                        column: x => x.VisitID,
                        principalTable: "Visit",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanFollowUp_VisitStatus_VisitStatusID",
                        column: x => x.VisitStatusID,
                        principalTable: "VisitStatus",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentLedger",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChargeID = table.Column<long>(nullable: false),
                    VisitID = table.Column<long>(nullable: false),
                    PatientPlanID = table.Column<long>(nullable: true),
                    PatientPaymentChargeID = table.Column<long>(nullable: true),
                    PaymentChargeID = table.Column<long>(nullable: true),
                    AdjustmentCodeID = table.Column<long>(nullable: true),
                    LedgerDate = table.Column<DateTime>(type: "Date", nullable: true),
                    LedgerBy = table.Column<string>(nullable: true),
                    LedgerType = table.Column<string>(nullable: true),
                    LedgerDescription = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: true),
                    Notes = table.Column<string>(maxLength: 500, nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentLedger", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PaymentLedger_AdjustmentCode_AdjustmentCodeID",
                        column: x => x.AdjustmentCodeID,
                        principalTable: "AdjustmentCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentLedger_Charge_ChargeID",
                        column: x => x.ChargeID,
                        principalTable: "Charge",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentLedger_PatientPaymentCharge_PatientPaymentChargeID",
                        column: x => x.PatientPaymentChargeID,
                        principalTable: "PatientPaymentCharge",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentLedger_PatientPlan_PatientPlanID",
                        column: x => x.PatientPlanID,
                        principalTable: "PatientPlan",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentLedger_PaymentCharge_PaymentChargeID",
                        column: x => x.PaymentChargeID,
                        principalTable: "PaymentCharge",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentLedger_Visit_VisitID",
                        column: x => x.VisitID,
                        principalTable: "Visit",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PracticeID = table.Column<long>(nullable: true),
                    PlanFollowupID = table.Column<long>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    NotesDate = table.Column<DateTime>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Notes_PlanFollowUp_PlanFollowupID",
                        column: x => x.PlanFollowupID,
                        principalTable: "PlanFollowUp",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notes_Practice_PracticeID",
                        column: x => x.PracticeID,
                        principalTable: "Practice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanFollowupCharge",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PlanFollowupID = table.Column<long>(nullable: true),
                    ChargeID = table.Column<long>(nullable: true),
                    GroupID = table.Column<long>(nullable: true),
                    ReasonID = table.Column<long>(nullable: true),
                    ActionID = table.Column<long>(nullable: true),
                    AdjustmentCodeID = table.Column<long>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    PaymentChargeID = table.Column<long>(nullable: true),
                    RemarkCode1ID = table.Column<long>(nullable: true),
                    RemarkCodeID = table.Column<long>(nullable: true),
                    RemarkCode2ID = table.Column<long>(nullable: true),
                    RemarkCode3ID = table.Column<long>(nullable: true),
                    RemarkCode4ID = table.Column<long>(nullable: true),
                    TickleDate = table.Column<DateTime>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanFollowupCharge", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PlanFollowupCharge_Action_ActionID",
                        column: x => x.ActionID,
                        principalTable: "Action",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanFollowupCharge_Charge_ChargeID",
                        column: x => x.ChargeID,
                        principalTable: "Charge",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanFollowupCharge_Group_GroupID",
                        column: x => x.GroupID,
                        principalTable: "Group",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanFollowupCharge_PaymentCharge_PaymentChargeID",
                        column: x => x.PaymentChargeID,
                        principalTable: "PaymentCharge",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanFollowupCharge_PlanFollowUp_PlanFollowupID",
                        column: x => x.PlanFollowupID,
                        principalTable: "PlanFollowUp",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanFollowupCharge_Reason_ReasonID",
                        column: x => x.ReasonID,
                        principalTable: "Reason",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanFollowupCharge_RemarkCode_RemarkCode2ID",
                        column: x => x.RemarkCode2ID,
                        principalTable: "RemarkCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanFollowupCharge_RemarkCode_RemarkCode3ID",
                        column: x => x.RemarkCode3ID,
                        principalTable: "RemarkCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanFollowupCharge_RemarkCode_RemarkCode4ID",
                        column: x => x.RemarkCode4ID,
                        principalTable: "RemarkCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanFollowupCharge_RemarkCode_RemarkCodeID",
                        column: x => x.RemarkCodeID,
                        principalTable: "RemarkCode",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Action_UserID",
                table: "Action",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_AdjustmentCode_ActionID",
                table: "AdjustmentCode",
                column: "ActionID");

            migrationBuilder.CreateIndex(
                name: "IX_AdjustmentCode_GroupID",
                table: "AdjustmentCode",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_AdjustmentCode_ReasonID",
                table: "AdjustmentCode",
                column: "ReasonID");

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
                name: "IX_AspNetUsers_RightsId",
                table: "AspNetUsers",
                column: "RightsId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TeamID",
                table: "AspNetUsers",
                column: "TeamID");

            migrationBuilder.CreateIndex(
                name: "IX_BatchDocument_BillerID",
                table: "BatchDocument",
                column: "BillerID");

            migrationBuilder.CreateIndex(
                name: "IX_BatchDocument_DocumentTypeID",
                table: "BatchDocument",
                column: "DocumentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_BatchDocument_LocationID",
                table: "BatchDocument",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_BatchDocument_PracticeID",
                table: "BatchDocument",
                column: "PracticeID");

            migrationBuilder.CreateIndex(
                name: "IX_BatchDocument_ProviderID",
                table: "BatchDocument",
                column: "ProviderID");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_CPTID",
                table: "Charge",
                column: "CPTID");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_ClientID",
                table: "Charge",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_LocationID",
                table: "Charge",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_Modifier1ID",
                table: "Charge",
                column: "Modifier1ID");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_Modifier2ID",
                table: "Charge",
                column: "Modifier2ID");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_Modifier3ID",
                table: "Charge",
                column: "Modifier3ID");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_Modifier4ID",
                table: "Charge",
                column: "Modifier4ID");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_OrderingProvID",
                table: "Charge",
                column: "OrderingProvID");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_POSID",
                table: "Charge",
                column: "POSID");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_PatientID",
                table: "Charge",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_PracticeID",
                table: "Charge",
                column: "PracticeID");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_PrimaryPatientPlanID",
                table: "Charge",
                column: "PrimaryPatientPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_ProviderID",
                table: "Charge",
                column: "ProviderID");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_RefProviderID",
                table: "Charge",
                column: "RefProviderID");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_SecondaryPatientPlanID",
                table: "Charge",
                column: "SecondaryPatientPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_SubmissionLogID",
                table: "Charge",
                column: "SubmissionLogID");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_SupervisingProvID",
                table: "Charge",
                column: "SupervisingProvID");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_TertiaryPatientPlanID",
                table: "Charge",
                column: "TertiaryPatientPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_Charge_VisitID",
                table: "Charge",
                column: "VisitID");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeAudit_ChargeID",
                table: "ChargeAudit",
                column: "ChargeID");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeSubmissionHistory_ChargeID",
                table: "ChargeSubmissionHistory",
                column: "ChargeID");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeSubmissionHistory_PatientPlanID",
                table: "ChargeSubmissionHistory",
                column: "PatientPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeSubmissionHistory_ReceiverID",
                table: "ChargeSubmissionHistory",
                column: "ReceiverID");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeSubmissionHistory_SubmissionLogID",
                table: "ChargeSubmissionHistory",
                column: "SubmissionLogID");

            migrationBuilder.CreateIndex(
                name: "IX_Cpt_Modifier1ID",
                table: "Cpt",
                column: "Modifier1ID");

            migrationBuilder.CreateIndex(
                name: "IX_Cpt_Modifier2ID",
                table: "Cpt",
                column: "Modifier2ID");

            migrationBuilder.CreateIndex(
                name: "IX_Cpt_Modifier3ID",
                table: "Cpt",
                column: "Modifier3ID");

            migrationBuilder.CreateIndex(
                name: "IX_Cpt_Modifier4ID",
                table: "Cpt",
                column: "Modifier4ID");

            migrationBuilder.CreateIndex(
                name: "IX_Cpt_TypeOfServiceID",
                table: "Cpt",
                column: "TypeOfServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_Edi270Payer_ReceiverID",
                table: "Edi270Payer",
                column: "ReceiverID");

            migrationBuilder.CreateIndex(
                name: "IX_Edi276Payer_ReceiverID",
                table: "Edi276Payer",
                column: "ReceiverID");

            migrationBuilder.CreateIndex(
                name: "IX_Edi837Payer_ReceiverID",
                table: "Edi837Payer",
                column: "ReceiverID");

            migrationBuilder.CreateIndex(
                name: "IX_Group_UserID",
                table: "Group",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceAudit_InsuranceID",
                table: "InsuranceAudit",
                column: "InsuranceID");

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePlan_Edi270PayerID",
                table: "InsurancePlan",
                column: "Edi270PayerID");

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePlan_Edi276PayerID",
                table: "InsurancePlan",
                column: "Edi276PayerID");

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePlan_Edi837PayerID",
                table: "InsurancePlan",
                column: "Edi837PayerID");

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePlan_InsuranceID",
                table: "InsurancePlan",
                column: "InsuranceID");

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePlan_PlanTypeID",
                table: "InsurancePlan",
                column: "PlanTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePlanAddress_InsurancePlanId",
                table: "InsurancePlanAddress",
                column: "InsurancePlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_POSID",
                table: "Location",
                column: "POSID");

            migrationBuilder.CreateIndex(
                name: "IX_Location_PracticeID",
                table: "Location",
                column: "PracticeID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationAudit_LocationID",
                table: "LocationAudit",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_PlanFollowupID",
                table: "Notes",
                column: "PlanFollowupID");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_PracticeID",
                table: "Notes",
                column: "PracticeID");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_BatchDocumentID",
                table: "Patient",
                column: "BatchDocumentID");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_LocationId",
                table: "Patient",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_PracticeID",
                table: "Patient",
                column: "PracticeID");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_ProviderID",
                table: "Patient",
                column: "ProviderID");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_RefProviderID",
                table: "Patient",
                column: "RefProviderID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAppointment_LocationID",
                table: "PatientAppointment",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAppointment_PatientID",
                table: "PatientAppointment",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAppointment_PrimarypatientPlanID",
                table: "PatientAppointment",
                column: "PrimarypatientPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAppointment_ProviderID",
                table: "PatientAppointment",
                column: "ProviderID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAppointment_ProviderSlotID",
                table: "PatientAppointment",
                column: "ProviderSlotID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAppointment_VisitReasonID",
                table: "PatientAppointment",
                column: "VisitReasonID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFollowUp_ActionID",
                table: "PatientFollowUp",
                column: "ActionID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFollowUp_GroupID",
                table: "PatientFollowUp",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFollowUp_PatientID",
                table: "PatientFollowUp",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFollowUp_ReasonID",
                table: "PatientFollowUp",
                column: "ReasonID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFollowUpCharge_ActionID",
                table: "PatientFollowUpCharge",
                column: "ActionID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFollowUpCharge_ChargeID",
                table: "PatientFollowUpCharge",
                column: "ChargeID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFollowUpCharge_GroupID",
                table: "PatientFollowUpCharge",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFollowUpCharge_PatientFollowUpID",
                table: "PatientFollowUpCharge",
                column: "PatientFollowUpID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFollowUpCharge_ReasonID",
                table: "PatientFollowUpCharge",
                column: "ReasonID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPayment_PatientID",
                table: "PatientPayment",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPaymentCharge_ChargeID",
                table: "PatientPaymentCharge",
                column: "ChargeID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPaymentCharge_PatientPaymentID",
                table: "PatientPaymentCharge",
                column: "PatientPaymentID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPaymentCharge_VisitID",
                table: "PatientPaymentCharge",
                column: "VisitID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPlan_BatchDocumentID",
                table: "PatientPlan",
                column: "BatchDocumentID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPlan_InsurancePlanAddressID",
                table: "PatientPlan",
                column: "InsurancePlanAddressID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPlan_InsurancePlanID",
                table: "PatientPlan",
                column: "InsurancePlanID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPlan_PatientID",
                table: "PatientPlan",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCharge_AdjustmentCodeID1",
                table: "PaymentCharge",
                column: "AdjustmentCodeID1");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCharge_AdjustmentCodeID2",
                table: "PaymentCharge",
                column: "AdjustmentCodeID2");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCharge_AdjustmentCodeID3",
                table: "PaymentCharge",
                column: "AdjustmentCodeID3");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCharge_AdjustmentCodeID4",
                table: "PaymentCharge",
                column: "AdjustmentCodeID4");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCharge_AdjustmentCodeID5",
                table: "PaymentCharge",
                column: "AdjustmentCodeID5");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCharge_ChargeID",
                table: "PaymentCharge",
                column: "ChargeID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCharge_PaymentVisitID",
                table: "PaymentCharge",
                column: "PaymentVisitID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCharge_RemarkCodeID1",
                table: "PaymentCharge",
                column: "RemarkCodeID1");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCharge_RemarkCodeID2",
                table: "PaymentCharge",
                column: "RemarkCodeID2");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCharge_RemarkCodeID3",
                table: "PaymentCharge",
                column: "RemarkCodeID3");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCharge_RemarkCodeID4",
                table: "PaymentCharge",
                column: "RemarkCodeID4");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCharge_RemarkCodeID5",
                table: "PaymentCharge",
                column: "RemarkCodeID5");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCheck_PracticeID",
                table: "PaymentCheck",
                column: "PracticeID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCheck_ReceiverID",
                table: "PaymentCheck",
                column: "ReceiverID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentLedger_AdjustmentCodeID",
                table: "PaymentLedger",
                column: "AdjustmentCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentLedger_ChargeID",
                table: "PaymentLedger",
                column: "ChargeID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentLedger_PatientPaymentChargeID",
                table: "PaymentLedger",
                column: "PatientPaymentChargeID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentLedger_PatientPlanID",
                table: "PaymentLedger",
                column: "PatientPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentLedger_PaymentChargeID",
                table: "PaymentLedger",
                column: "PaymentChargeID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentLedger_VisitID",
                table: "PaymentLedger",
                column: "VisitID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVisit_BatchDocumentID",
                table: "PaymentVisit",
                column: "BatchDocumentID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVisit_PatientID",
                table: "PaymentVisit",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVisit_PaymentCheckID",
                table: "PaymentVisit",
                column: "PaymentCheckID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVisit_VisitID",
                table: "PaymentVisit",
                column: "VisitID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanFollowUp_ActionID",
                table: "PlanFollowUp",
                column: "ActionID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanFollowUp_AdjustmentCodeID",
                table: "PlanFollowUp",
                column: "AdjustmentCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanFollowUp_GroupID",
                table: "PlanFollowUp",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanFollowUp_PaymentVisitID",
                table: "PlanFollowUp",
                column: "PaymentVisitID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanFollowUp_ReasonID",
                table: "PlanFollowUp",
                column: "ReasonID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanFollowUp_VisitID",
                table: "PlanFollowUp",
                column: "VisitID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanFollowUp_VisitStatusID",
                table: "PlanFollowUp",
                column: "VisitStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanFollowupCharge_ActionID",
                table: "PlanFollowupCharge",
                column: "ActionID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanFollowupCharge_ChargeID",
                table: "PlanFollowupCharge",
                column: "ChargeID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanFollowupCharge_GroupID",
                table: "PlanFollowupCharge",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanFollowupCharge_PaymentChargeID",
                table: "PlanFollowupCharge",
                column: "PaymentChargeID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanFollowupCharge_PlanFollowupID",
                table: "PlanFollowupCharge",
                column: "PlanFollowupID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanFollowupCharge_ReasonID",
                table: "PlanFollowupCharge",
                column: "ReasonID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanFollowupCharge_RemarkCode2ID",
                table: "PlanFollowupCharge",
                column: "RemarkCode2ID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanFollowupCharge_RemarkCode3ID",
                table: "PlanFollowupCharge",
                column: "RemarkCode3ID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanFollowupCharge_RemarkCode4ID",
                table: "PlanFollowupCharge",
                column: "RemarkCode4ID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanFollowupCharge_RemarkCodeID",
                table: "PlanFollowupCharge",
                column: "RemarkCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_POSAudit_POSID",
                table: "POSAudit",
                column: "POSID");

            migrationBuilder.CreateIndex(
                name: "IX_Practice_ClientID",
                table: "Practice",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_Practice_UserPracticesUserID_UserPracticesPracticeID",
                table: "Practice",
                columns: new[] { "UserPracticesUserID", "UserPracticesPracticeID" });

            migrationBuilder.CreateIndex(
                name: "IX_Provider_PracticeID",
                table: "Provider",
                column: "PracticeID");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderSchedule_LocationID",
                table: "ProviderSchedule",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderSchedule_ProviderID",
                table: "ProviderSchedule",
                column: "ProviderID");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderSlot_ProviderScheduleID",
                table: "ProviderSlot",
                column: "ProviderScheduleID");

            migrationBuilder.CreateIndex(
                name: "IX_Reason_UserID",
                table: "Reason",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_RefProvider_PracticeID",
                table: "RefProvider",
                column: "PracticeID");

            migrationBuilder.CreateIndex(
                name: "IX_ResubmitHistory_ChargeID",
                table: "ResubmitHistory",
                column: "ChargeID");

            migrationBuilder.CreateIndex(
                name: "IX_ResubmitHistory_VisitID",
                table: "ResubmitHistory",
                column: "VisitID");

            migrationBuilder.CreateIndex(
                name: "IX_Submitter_ClientID",
                table: "Submitter",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_Submitter_ReceiverID",
                table: "Submitter",
                column: "ReceiverID");

            migrationBuilder.CreateIndex(
                name: "IX_TeamAudit_TeamID",
                table: "TeamAudit",
                column: "TeamID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_BatchDocumentID",
                table: "Visit",
                column: "BatchDocumentID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_ClientID",
                table: "Visit",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_ICD10ID",
                table: "Visit",
                column: "ICD10ID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_ICD11ID",
                table: "Visit",
                column: "ICD11ID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_ICD12ID",
                table: "Visit",
                column: "ICD12ID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_ICD1ID",
                table: "Visit",
                column: "ICD1ID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_ICD2ID",
                table: "Visit",
                column: "ICD2ID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_ICD3ID",
                table: "Visit",
                column: "ICD3ID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_ICD4ID",
                table: "Visit",
                column: "ICD4ID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_ICD5ID",
                table: "Visit",
                column: "ICD5ID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_ICD6ID",
                table: "Visit",
                column: "ICD6ID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_ICD7ID",
                table: "Visit",
                column: "ICD7ID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_ICD8ID",
                table: "Visit",
                column: "ICD8ID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_ICD9ID",
                table: "Visit",
                column: "ICD9ID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_LocationID",
                table: "Visit",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_OrderingProvID",
                table: "Visit",
                column: "OrderingProvID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_POSID",
                table: "Visit",
                column: "POSID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_PatientID",
                table: "Visit",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_PracticeID",
                table: "Visit",
                column: "PracticeID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_PrimaryPatientPlanID",
                table: "Visit",
                column: "PrimaryPatientPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_ProviderID",
                table: "Visit",
                column: "ProviderID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_RefProviderID",
                table: "Visit",
                column: "RefProviderID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_SecondaryPatientPlanID",
                table: "Visit",
                column: "SecondaryPatientPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_SubmissionLogID",
                table: "Visit",
                column: "SubmissionLogID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_SupervisingProvID",
                table: "Visit",
                column: "SupervisingProvID");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_TertiaryPatientPlanID",
                table: "Visit",
                column: "TertiaryPatientPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_VisitAudit_VisitID",
                table: "VisitAudit",
                column: "VisitID");
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
                name: "CategoryCodes");

            migrationBuilder.DropTable(
                name: "ChargeAudit");

            migrationBuilder.DropTable(
                name: "ChargeStatus");

            migrationBuilder.DropTable(
                name: "ChargeSubmissionHistory");

            migrationBuilder.DropTable(
                name: "CityStateZipData");

            migrationBuilder.DropTable(
                name: "DownloadedFile");

            migrationBuilder.DropTable(
                name: "InsuranceAudit");

            migrationBuilder.DropTable(
                name: "LocationAudit");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "PatientAppointment");

            migrationBuilder.DropTable(
                name: "PatientEligibility");

            migrationBuilder.DropTable(
                name: "PatientEligibilityDetail");

            migrationBuilder.DropTable(
                name: "PatientEligibilityLog");

            migrationBuilder.DropTable(
                name: "PatientFollowUpCharge");

            migrationBuilder.DropTable(
                name: "PaymentLedger");

            migrationBuilder.DropTable(
                name: "PlanFollowupCharge");

            migrationBuilder.DropTable(
                name: "POSAudit");

            migrationBuilder.DropTable(
                name: "ReportsLog");

            migrationBuilder.DropTable(
                name: "ResubmitHistory");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "StatusCode");

            migrationBuilder.DropTable(
                name: "Submitter");

            migrationBuilder.DropTable(
                name: "Taxonomy");

            migrationBuilder.DropTable(
                name: "TeamAudit");

            migrationBuilder.DropTable(
                name: "UserPracticeAudit");

            migrationBuilder.DropTable(
                name: "VisitAudit");

            migrationBuilder.DropTable(
                name: "VisitStatusLog");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ProviderSlot");

            migrationBuilder.DropTable(
                name: "VisitReason");

            migrationBuilder.DropTable(
                name: "PatientFollowUp");

            migrationBuilder.DropTable(
                name: "PatientPaymentCharge");

            migrationBuilder.DropTable(
                name: "PaymentCharge");

            migrationBuilder.DropTable(
                name: "PlanFollowUp");

            migrationBuilder.DropTable(
                name: "ProviderSchedule");

            migrationBuilder.DropTable(
                name: "PatientPayment");

            migrationBuilder.DropTable(
                name: "Charge");

            migrationBuilder.DropTable(
                name: "RemarkCode");

            migrationBuilder.DropTable(
                name: "AdjustmentCode");

            migrationBuilder.DropTable(
                name: "PaymentVisit");

            migrationBuilder.DropTable(
                name: "VisitStatus");

            migrationBuilder.DropTable(
                name: "Cpt");

            migrationBuilder.DropTable(
                name: "Action");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropTable(
                name: "Reason");

            migrationBuilder.DropTable(
                name: "PaymentCheck");

            migrationBuilder.DropTable(
                name: "Visit");

            migrationBuilder.DropTable(
                name: "Modifier");

            migrationBuilder.DropTable(
                name: "TypeOfService");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ICD");

            migrationBuilder.DropTable(
                name: "PatientPlan");

            migrationBuilder.DropTable(
                name: "SubmissionLog");

            migrationBuilder.DropTable(
                name: "Designations");

            migrationBuilder.DropTable(
                name: "Rights");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "InsurancePlanAddress");

            migrationBuilder.DropTable(
                name: "Patient");

            migrationBuilder.DropTable(
                name: "InsurancePlan");

            migrationBuilder.DropTable(
                name: "BatchDocument");

            migrationBuilder.DropTable(
                name: "RefProvider");

            migrationBuilder.DropTable(
                name: "Edi270Payer");

            migrationBuilder.DropTable(
                name: "Edi276Payer");

            migrationBuilder.DropTable(
                name: "Edi837Payer");

            migrationBuilder.DropTable(
                name: "Insurance");

            migrationBuilder.DropTable(
                name: "PlanType");

            migrationBuilder.DropTable(
                name: "Biller");

            migrationBuilder.DropTable(
                name: "DocumentType");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Provider");

            migrationBuilder.DropTable(
                name: "Receiver");

            migrationBuilder.DropTable(
                name: "POS");

            migrationBuilder.DropTable(
                name: "Practice");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "UserPractices");
        }
    }
}

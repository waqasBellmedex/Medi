﻿// <auto-generated />
using System;
using MediFusionPM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MediFusionPM.Migrations
{
    [DbContext(typeof(MainContext))]
    [Migration("20200212172226_ChangeNameLengthInMainPractice")]
    partial class ChangeNameLengthInMainPractice
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MediFusionPM.Models.Audit.MainUserPracticeAudit", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AddedBy");

                    b.Property<DateTime>("AddedDate");

                    b.Property<string>("ColumnName");

                    b.Property<string>("CurrentValue");

                    b.Property<string>("CurrentValueID");

                    b.Property<string>("HostName");

                    b.Property<string>("NewValue");

                    b.Property<string>("NewValueID");

                    b.Property<long>("TransactionID");

                    b.Property<string>("UserID");

                    b.HasKey("ID");

                    b.ToTable("MainUserPracticeAudit");
                });

            modelBuilder.Entity("MediFusionPM.Models.Main.MainClient", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AddedBy");

                    b.Property<DateTime>("AddedDate");

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<string>("ContactNo");

                    b.Property<string>("ContactPerson");

                    b.Property<string>("ContextName");

                    b.Property<string>("FaxNo");

                    b.Property<DateTime?>("LastNewInsertsModifiedDate");

                    b.Property<DateTime?>("LastNewTrigerModifiedDate");

                    b.Property<string>("Name");

                    b.Property<string>("OfficeEmail");

                    b.Property<string>("OfficeHour");

                    b.Property<string>("OfficePhoneNo");

                    b.Property<string>("OrganizationName");

                    b.Property<string>("ServiceLocation");

                    b.Property<string>("State");

                    b.Property<string>("TaxID");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedDate");

                    b.Property<string>("ZipCode");

                    b.HasKey("ID");

                    b.ToTable("MainClient");
                });

            modelBuilder.Entity("MediFusionPM.Models.Main.MainDesignations", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AddedBy");

                    b.Property<DateTime>("AddedDate");

                    b.Property<string>("Name");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedDate");

                    b.HasKey("ID");

                    b.ToTable("MainDesignations");
                });

            modelBuilder.Entity("MediFusionPM.Models.Main.MainPractice", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AddedBy");

                    b.Property<DateTime>("AddedDate");

                    b.Property<string>("Address1")
                        .HasMaxLength(55);

                    b.Property<string>("Address2")
                        .HasMaxLength(55);

                    b.Property<string>("CLIANumber")
                        .HasMaxLength(20);

                    b.Property<string>("City")
                        .HasMaxLength(20);

                    b.Property<long?>("ClientID");

                    b.Property<long?>("DefaultLocationID");

                    b.Property<string>("Email");

                    b.Property<string>("FaxNumber")
                        .HasMaxLength(10);

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<long?>("MainClientID");

                    b.Property<long?>("MainUserPracticesPracticeID");

                    b.Property<string>("MainUserPracticesUserID");

                    b.Property<string>("NPI")
                        .HasMaxLength(10);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("Notes")
                        .HasMaxLength(500);

                    b.Property<string>("OfficePhoneNum")
                        .HasMaxLength(10);

                    b.Property<string>("OrganizationName")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("PayToAddress1")
                        .HasMaxLength(55);

                    b.Property<string>("PayToAddress2")
                        .HasMaxLength(55);

                    b.Property<string>("PayToCity")
                        .HasMaxLength(20);

                    b.Property<string>("PayToState")
                        .HasMaxLength(2);

                    b.Property<string>("PayToZipCode")
                        .HasMaxLength(9);

                    b.Property<string>("SSN")
                        .HasMaxLength(9);

                    b.Property<string>("State")
                        .HasMaxLength(2);

                    b.Property<string>("TaxID")
                        .HasMaxLength(9);

                    b.Property<string>("TaxonomyCode")
                        .HasMaxLength(10);

                    b.Property<string>("Type");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedDate");

                    b.Property<string>("Website");

                    b.Property<string>("WorkingHours");

                    b.Property<string>("ZipCode")
                        .HasMaxLength(9);

                    b.HasKey("ID");

                    b.HasIndex("MainClientID");

                    b.HasIndex("MainUserPracticesUserID", "MainUserPracticesPracticeID");

                    b.ToTable("MainPractice");
                });

            modelBuilder.Entity("MediFusionPM.Models.Main.MainRights", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddedBy");

                    b.Property<DateTime>("AddedDate");

                    b.Property<bool>("AdjustmentCodesCreate");

                    b.Property<bool>("AdjustmentCodesDelete");

                    b.Property<bool>("AdjustmentCodesEdit");

                    b.Property<bool>("AdjustmentCodesExport");

                    b.Property<bool>("AdjustmentCodesImport");

                    b.Property<bool>("AdjustmentCodesSearch");

                    b.Property<string>("AssignedByUserId");

                    b.Property<bool>("CPTCreate");

                    b.Property<bool>("CPTDelete");

                    b.Property<bool>("CPTEdit");

                    b.Property<bool>("CPTExport");

                    b.Property<bool>("CPTImport");

                    b.Property<bool>("CPTSearch");

                    b.Property<bool>("ChargesCreate");

                    b.Property<bool>("ChargesDelete");

                    b.Property<bool>("ChargesEdit");

                    b.Property<bool>("ChargesExport");

                    b.Property<bool>("ChargesImport");

                    b.Property<bool>("ChargesSearch");

                    b.Property<bool>("ClaimStatusCategoryCodesCreate");

                    b.Property<bool>("ClaimStatusCategoryCodesDelete");

                    b.Property<bool>("ClaimStatusCategoryCodesEdit");

                    b.Property<bool>("ClaimStatusCategoryCodesExport");

                    b.Property<bool>("ClaimStatusCategoryCodesImport");

                    b.Property<bool>("ClaimStatusCategoryCodesSearch");

                    b.Property<bool>("ClaimStatusCodesCreate");

                    b.Property<bool>("ClaimStatusCodesDelete");

                    b.Property<bool>("ClaimStatusCodesEdit");

                    b.Property<bool>("ClaimStatusCodesExport");

                    b.Property<bool>("ClaimStatusCodesImport");

                    b.Property<bool>("ClaimStatusCodesSearch");

                    b.Property<bool>("ClientCreate");

                    b.Property<bool>("ClientDelete");

                    b.Property<bool>("ClientEdit");

                    b.Property<bool>("ClientExport");

                    b.Property<bool>("ClientImport");

                    b.Property<bool>("ClientSearch");

                    b.Property<bool>("DeleteCheck");

                    b.Property<bool>("DeletePaymentVisit");

                    b.Property<bool>("DocumentsCreate");

                    b.Property<bool>("DocumentsDelete");

                    b.Property<bool>("DocumentsEdit");

                    b.Property<bool>("DocumentsExport");

                    b.Property<bool>("DocumentsImport");

                    b.Property<bool>("DocumentsSearch");

                    b.Property<bool>("EDIEligiBilityCreate");

                    b.Property<bool>("EDIEligiBilityDelete");

                    b.Property<bool>("EDIEligiBilityEdit");

                    b.Property<bool>("EDIEligiBilityExport");

                    b.Property<bool>("EDIEligiBilityImport");

                    b.Property<bool>("EDIEligiBilitySearch");

                    b.Property<bool>("EDIStatusCreate");

                    b.Property<bool>("EDIStatusDelete");

                    b.Property<bool>("EDIStatusEdit");

                    b.Property<bool>("EDIStatusExport");

                    b.Property<bool>("EDIStatusImport");

                    b.Property<bool>("EDIStatusSearch");

                    b.Property<bool>("EDISubmitCreate");

                    b.Property<bool>("EDISubmitDelete");

                    b.Property<bool>("EDISubmitEdit");

                    b.Property<bool>("EDISubmitExport");

                    b.Property<bool>("EDISubmitImport");

                    b.Property<bool>("EDISubmitSearch");

                    b.Property<bool>("FollowupCreate");

                    b.Property<bool>("FollowupDelete");

                    b.Property<bool>("FollowupEdit");

                    b.Property<bool>("FollowupExport");

                    b.Property<bool>("FollowupImport");

                    b.Property<bool>("FollowupSearch");

                    b.Property<bool>("ICDCreate");

                    b.Property<bool>("ICDDelete");

                    b.Property<bool>("ICDEdit");

                    b.Property<bool>("ICDExport");

                    b.Property<bool>("ICDImport");

                    b.Property<bool>("ICDSearch");

                    b.Property<bool>("InsuranceCreate");

                    b.Property<bool>("InsuranceDelete");

                    b.Property<bool>("InsuranceEdit");

                    b.Property<bool>("InsuranceExport");

                    b.Property<bool>("InsuranceImport");

                    b.Property<bool>("InsurancePlanAddressCreate");

                    b.Property<bool>("InsurancePlanAddressDelete");

                    b.Property<bool>("InsurancePlanAddressEdit");

                    b.Property<bool>("InsurancePlanAddressExport");

                    b.Property<bool>("InsurancePlanAddressImport");

                    b.Property<bool>("InsurancePlanAddressSearch");

                    b.Property<bool>("InsurancePlanCreate");

                    b.Property<bool>("InsurancePlanDelete");

                    b.Property<bool>("InsurancePlanEdit");

                    b.Property<bool>("InsurancePlanExport");

                    b.Property<bool>("InsurancePlanImport");

                    b.Property<bool>("InsurancePlanSearch");

                    b.Property<bool>("InsuranceSearch");

                    b.Property<bool>("LocationCreate");

                    b.Property<bool>("LocationDelete");

                    b.Property<bool>("LocationEdit");

                    b.Property<bool>("LocationExport");

                    b.Property<bool>("LocationImport");

                    b.Property<bool>("LocationSearch");

                    b.Property<bool>("ManualPosting");

                    b.Property<bool>("ManualPostingAdd");

                    b.Property<bool>("ManualPostingUpdate");

                    b.Property<bool>("ModifiersCreate");

                    b.Property<bool>("ModifiersDelete");

                    b.Property<bool>("ModifiersEdit");

                    b.Property<bool>("ModifiersExport");

                    b.Property<bool>("ModifiersImport");

                    b.Property<bool>("ModifiersSearch");

                    b.Property<bool>("POSCreate");

                    b.Property<bool>("POSDelete");

                    b.Property<bool>("POSEdit");

                    b.Property<bool>("POSExport");

                    b.Property<bool>("POSImport");

                    b.Property<bool>("POSSearch");

                    b.Property<bool>("PatientCreate");

                    b.Property<bool>("PatientDelete");

                    b.Property<bool>("PatientEdit");

                    b.Property<bool>("PatientExport");

                    b.Property<bool>("PatientImport");

                    b.Property<bool>("PatientSearch");

                    b.Property<bool>("PaymentsCreate");

                    b.Property<bool>("PaymentsDelete");

                    b.Property<bool>("PaymentsEdit");

                    b.Property<bool>("PaymentsExport");

                    b.Property<bool>("PaymentsImport");

                    b.Property<bool>("PaymentsSearch");

                    b.Property<bool>("PostCheckSearch");

                    b.Property<bool>("PostExport");

                    b.Property<bool>("PostImport");

                    b.Property<bool>("Postcheck");

                    b.Property<bool>("PracticeCreate");

                    b.Property<bool>("PracticeDelete");

                    b.Property<bool>("PracticeEdit");

                    b.Property<bool>("PracticeExport");

                    b.Property<bool>("PracticeImport");

                    b.Property<bool>("PracticeSearch");

                    b.Property<bool>("ProviderCreate");

                    b.Property<bool>("ProviderDelete");

                    b.Property<bool>("ProviderEdit");

                    b.Property<bool>("ProviderExport");

                    b.Property<bool>("ProviderImport");

                    b.Property<bool>("ProviderSearch");

                    b.Property<bool>("ReferringProviderCreate");

                    b.Property<bool>("ReferringProviderDelete");

                    b.Property<bool>("ReferringProviderEdit");

                    b.Property<bool>("ReferringProviderExport");

                    b.Property<bool>("ReferringProviderImport");

                    b.Property<bool>("ReferringProviderSearch");

                    b.Property<bool>("RemarkCodesCreate");

                    b.Property<bool>("RemarkCodesDelete");

                    b.Property<bool>("RemarkCodesEdit");

                    b.Property<bool>("RemarkCodesExport");

                    b.Property<bool>("RemarkCodesImport");

                    b.Property<bool>("RemarkCodesSearch");

                    b.Property<bool>("ReportsCreate");

                    b.Property<bool>("ReportsDelete");

                    b.Property<bool>("ReportsEdit");

                    b.Property<bool>("ReportsExport");

                    b.Property<bool>("ReportsImport");

                    b.Property<bool>("ReportsSearch");

                    b.Property<bool>("SchedulerCreate");

                    b.Property<bool>("SchedulerDelete");

                    b.Property<bool>("SchedulerEdit");

                    b.Property<bool>("SchedulerExport");

                    b.Property<bool>("SchedulerImport");

                    b.Property<bool>("SchedulerSearch");

                    b.Property<bool>("SubmissionsCreate");

                    b.Property<bool>("SubmissionsDelete");

                    b.Property<bool>("SubmissionsEdit");

                    b.Property<bool>("SubmissionsExport");

                    b.Property<bool>("SubmissionsImport");

                    b.Property<bool>("SubmissionsSearch");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedDate");

                    b.Property<bool>("UserCreate");

                    b.Property<bool>("UserDelete");

                    b.Property<bool>("UserEdit");

                    b.Property<bool>("UserExport");

                    b.Property<bool>("UserImport");

                    b.Property<bool>("UserSearch");

                    b.Property<bool>("addPaymentVisit");

                    b.Property<bool>("batchdocumentCreate");

                    b.Property<bool>("batchdocumentDelete");

                    b.Property<bool>("batchdocumentExport");

                    b.Property<bool>("batchdocumentImport");

                    b.Property<bool>("batchdocumentSearch");

                    b.Property<bool>("batchdocumentUpdate");

                    b.Property<bool>("electronicsSubmissionResubmit");

                    b.Property<bool>("electronicsSubmissionSearch");

                    b.Property<bool>("electronicsSubmissionSubmit");

                    b.Property<bool>("groupCreate");

                    b.Property<bool>("groupDelete");

                    b.Property<bool>("groupExport");

                    b.Property<bool>("groupImport");

                    b.Property<bool>("groupSearch");

                    b.Property<bool>("groupUpdate");

                    b.Property<bool>("paperSubmissionResubmit");

                    b.Property<bool>("paperSubmissionSearch");

                    b.Property<bool>("paperSubmissionSubmit");

                    b.Property<bool>("patientFollowupCreate");

                    b.Property<bool>("patientFollowupDelete");

                    b.Property<bool>("patientFollowupExport");

                    b.Property<bool>("patientFollowupImport");

                    b.Property<bool>("patientFollowupSearch");

                    b.Property<bool>("patientFollowupUpdate");

                    b.Property<bool>("patientPaymentCreate");

                    b.Property<bool>("patientPaymentDelete");

                    b.Property<bool>("patientPaymentExport");

                    b.Property<bool>("patientPaymentImport");

                    b.Property<bool>("patientPaymentSearch");

                    b.Property<bool>("patientPaymentUpdate");

                    b.Property<bool>("patientPlanCreate");

                    b.Property<bool>("patientPlanDelete");

                    b.Property<bool>("patientPlanExport");

                    b.Property<bool>("patientPlanImport");

                    b.Property<bool>("patientPlanSearch");

                    b.Property<bool>("patientPlanUpdate");

                    b.Property<bool>("performEligibility");

                    b.Property<bool>("planFollowupCreate");

                    b.Property<bool>("planFollowupDelete");

                    b.Property<bool>("planFollowupExport");

                    b.Property<bool>("planFollowupImport");

                    b.Property<bool>("planFollowupSearch");

                    b.Property<bool>("planFollowupUpdate");

                    b.Property<bool>("reasonCreate");

                    b.Property<bool>("reasonDelete");

                    b.Property<bool>("reasonExport");

                    b.Property<bool>("reasonImport");

                    b.Property<bool>("reasonSearch");

                    b.Property<bool>("reasonUpdate");

                    b.Property<bool>("receiverCreate");

                    b.Property<bool>("receiverDelete");

                    b.Property<bool>("receiverExport");

                    b.Property<bool>("receiverImport");

                    b.Property<bool>("receiverSearch");

                    b.Property<bool>("receiverupdate");

                    b.Property<bool>("resubmitCharges");

                    b.Property<bool>("submissionLogSearch");

                    b.Property<bool>("submitterCreate");

                    b.Property<bool>("submitterDelete");

                    b.Property<bool>("submitterExport");

                    b.Property<bool>("submitterImport");

                    b.Property<bool>("submitterSearch");

                    b.Property<bool>("submitterUpdate");

                    b.Property<bool>("teamCreate");

                    b.Property<bool>("teamDelete");

                    b.Property<bool>("teamExport");

                    b.Property<bool>("teamImport");

                    b.Property<bool>("teamSearch");

                    b.Property<bool>("teamupdate");

                    b.HasKey("Id");

                    b.ToTable("MainRights");
                });

            modelBuilder.Entity("MediFusionPM.Models.Main.MainTeam", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AddedBy");

                    b.Property<DateTime>("AddedDate");

                    b.Property<string>("Details");

                    b.Property<string>("Name");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedDate");

                    b.HasKey("ID");

                    b.ToTable("MainTeam");
                });

            modelBuilder.Entity("MediFusionPM.Models.Main.MainUserPractices", b =>
                {
                    b.Property<string>("UserID");

                    b.Property<long>("PracticeID");

                    b.Property<string>("AddedBy");

                    b.Property<DateTime>("AddedDate");

                    b.Property<string>("AssignedByUserId");

                    b.Property<bool>("Status");

                    b.Property<string>("UPALastModified");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedDate");

                    b.HasKey("UserID", "PracticeID");

                    b.ToTable("MainUserPractices");
                });

            modelBuilder.Entity("MediFusionPM.ViewModels.Main.MainAuthIdentityCustom", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("BlockNote");

                    b.Property<long?>("ClientID");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<long?>("DesignationID");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsUserBlock");

                    b.Property<bool>("IsUserBlockByAdmin");

                    b.Property<bool>("IsUserLogin");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<int>("LogInAttempts");

                    b.Property<string>("MainRightsId");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<long?>("PracticeID");

                    b.Property<string>("ReportingTo");

                    b.Property<string>("SecurityStamp");

                    b.Property<long?>("TeamID");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("ClientID");

                    b.HasIndex("DesignationID");

                    b.HasIndex("MainRightsId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("PracticeID");

                    b.HasIndex("TeamID");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("MediFusionPM.Models.Main.MainPractice", b =>
                {
                    b.HasOne("MediFusionPM.Models.Main.MainClient")
                        .WithMany("MainPractice")
                        .HasForeignKey("MainClientID");

                    b.HasOne("MediFusionPM.Models.Main.MainUserPractices", "MainUserPractices")
                        .WithMany("MainPractice")
                        .HasForeignKey("MainUserPracticesUserID", "MainUserPracticesPracticeID");
                });

            modelBuilder.Entity("MediFusionPM.ViewModels.Main.MainAuthIdentityCustom", b =>
                {
                    b.HasOne("MediFusionPM.Models.Main.MainClient", "MainClient")
                        .WithMany()
                        .HasForeignKey("ClientID");

                    b.HasOne("MediFusionPM.Models.Main.MainDesignations", "MainDesignations")
                        .WithMany("MainAuthIdentityCustom")
                        .HasForeignKey("DesignationID");

                    b.HasOne("MediFusionPM.Models.Main.MainRights", "MainRights")
                        .WithMany()
                        .HasForeignKey("MainRightsId");

                    b.HasOne("MediFusionPM.Models.Main.MainPractice", "MainPractice")
                        .WithMany("MainAuthIdentityCustom")
                        .HasForeignKey("PracticeID");

                    b.HasOne("MediFusionPM.Models.Main.MainTeam", "MainTeam")
                        .WithMany("MainAuthIdentityCustom")
                        .HasForeignKey("TeamID");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("MediFusionPM.ViewModels.Main.MainAuthIdentityCustom")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("MediFusionPM.ViewModels.Main.MainAuthIdentityCustom")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MediFusionPM.ViewModels.Main.MainAuthIdentityCustom")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("MediFusionPM.ViewModels.Main.MainAuthIdentityCustom")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

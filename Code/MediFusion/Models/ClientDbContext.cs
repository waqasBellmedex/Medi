using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models.Audit;
using MediFusionPM.Models.TodoApi;
using MediFusionPM.Models.TodoApi.Models;
using MediFusionPM.ViewModel;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using WebApplication1.Models.Audit;
using MediFusionPM.Models.Deleted;

namespace MediFusionPM.Models
{
    public class ClientDbContext : IdentityDbContext<AuthIdentityCustom>
    {
        //public PMContext(DbContextOptions<PMContext> options) : base(options)
        //{

        //}


        public readonly IHostingEnvironment env;
        private string User;
        private IConfiguration _config;
        private HttpContext _httpContext;
        private string DbName;
        private string DB;

        private int counter = 0;
        private int cnt = 0;


        public ClientDbContext(DbContextOptions options, IConfiguration config, IHttpContextAccessor httpContextAccessor, IHostingEnvironment env) : base(options)
        {
            this.env = env;
            _config = config;
            _httpContext = httpContextAccessor.HttpContext;
             DB = "database=";
            //  this.Database.SetCommandTimeout(120);
        }
        public void setDatabaseName(string DbName)
        {
            this.DbName = DbName;
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
            //OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            if (counter == 0)
            {

                base.OnModelCreating(builder);

                builder.Entity<UserPractices>()
                .HasKey(c => new { c.UserID, c.PracticeID });

                // builder.Entity<AuthIdentityCustom>()
                //.HasOne(p => p.ReferenceUserID)
                //.WithMany()
                //.HasForeignKey(b => b.RefUserID);
                counter++;
            }
        }


        public IConfigurationRoot Configuration { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                Configuration = builder.Build();
                string connectionString = Configuration.GetConnectionString("MedifusionLocal");
                string[] splitString = connectionString.Split(';');
                //if (cnt==0)
                //{
                //    if (DbName != null) splitString[1] = splitString[1] + DbName;
                //    else { splitString[1] = splitString[1]; }
                //    cnt++;
                //}
                splitString[1] = splitString[1];

                if (DbName.IsNull())
                    DbName = _httpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("TEMP", StringComparison.InvariantCultureIgnoreCase)).Value;

                connectionString = splitString[0] + "; " + splitString[1] + DbName + "; " + splitString[2] + "; " + splitString[3];
                // Debug.WriteLine("this is the connection string \n\n" + connectionString + "    \n\n");
                optionsBuilder.UseSqlServer(connectionString);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        public string GetNextSequenceValue(string SequenceName)
        {
            var connection = this.Database.GetDbConnection();
            connection.Open();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = string.Format("SELECT NEXT VALUE FOR {0}", SequenceName);
                var obj = cmd.ExecuteScalar();
                connection.Close();
                return obj.ToString();
            }
        }


        //  public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<Practice> Practice { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Provider> Provider { get; set; }
        public DbSet<RefProvider> RefProvider { get; set; }
        public DbSet<Insurance> Insurance { get; set; }
        public DbSet<InsurancePlan> InsurancePlan { get; set; }
        public DbSet<ICD> ICD { get; set; }
        public DbSet<Cpt> Cpt { get; set; }
        public DbSet<ExternalCharge> ExternalCharge { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<POS> POS { get; set; }
        public DbSet<Modifier> Modifier { get; set; }
        public DbSet<TypeOfService> TypeOfService { get; set; }
        public DbSet<PlanType> PlanType { get; set; }
        public DbSet<Edi837Payer> Edi837Payer { get; set; }
        public DbSet<Edi276Payer> Edi276Payer { get; set; }
        public DbSet<Edi270Payer> Edi270Payer { get; set; }
        public DbSet<Receiver> Receiver { get; set; }
        public DbSet<InsurancePlanAddress> InsurancePlanAddress { get; set; }
        public DbSet<PatientPlan> PatientPlan { get; set; }
        public DbSet<Charge> Charge { get; set; }
        public DbSet<Visit> Visit { get; set; }
        public DbSet<Submitter> Submitter { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<SubmissionLog> SubmissionLog { get; set; }
        public DbSet<ChargeSubmissionHistory> ChargeSubmissionHistory { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<PaymentCheck> PaymentCheck { get; set; }
        public DbSet<PaymentVisit> PaymentVisit { get; set; }
        public DbSet<PaymentCharge> PaymentCharge { get; set; }
        public DbSet<ResubmitHistory> ResubmitHistory { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Reason> Reason { get; set; }
        public DbSet<Biller> Biller { get; set; }
        public DbSet<Action> Action { get; set; }
        public DbSet<PlanFollowup> PlanFollowUp { get; set; }
        public DbSet<BatchDocument> BatchDocument { get; set; }
        public DbSet<DocumentType> DocumentType { get; set; }
        public DbSet<RemarkCode> RemarkCode { get; set; }
        public DbSet<AdjustmentCode> AdjustmentCode { get; set; }
        public DbSet<PatientFollowUp> PatientFollowUp { get; set; }
        public DbSet<PatientFollowUpCharge> PatientFollowUpCharge { get; set; }
        public DbSet<PatientPayment> PatientPayment { get; set; }
        public DbSet<PatientPaymentCharge> PatientPaymentCharge { get; set; }
        public DbSet<UserPractices> UserPractices { get; set; }
        public DbSet<Rights> Rights { get; set; }
        public DbSet<PaymentLedger> PaymentLedger { get; set; }
        public DbSet<ProviderSchedule> ProviderSchedule { get; set; }
     
        public DbSet<VisitReason> VisitReason { get; set; }
       
        public DbSet<PatientAppointment> PatientAppointment { get; set; }
        public DbSet<ProviderSlot> ProviderSlot { get; set; }
        public DbSet<PatientEligibility> PatientEligibility { get; set; }
        public DbSet<PatientEligibilityDetail> PatientEligibilityDetail { get; set; }
        public DbSet<PatientEligibilityLog> PatientEligibilityLog { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<Designations> Designations { get; set; }
        public DbSet<PlanFollowupCharge> PlanFollowupCharge { get; set; }
        public DbSet<VisitStatus> VisitStatus { get; set; }
        public DbSet<ChargeStatus> ChargeStatus { get; set; }
        public DbSet<VisitStatusLog> VisitStatusLog { get; set; }
        public DbSet<ReportsLog> ReportsLog { get; set; }
        public DbSet<DownloadedFile> DownloadedFile { get; set; }
        public DbSet<Notes> Notes { get; set; }
        public DbSet<StatusCode> StatusCode { get; set; }
        public DbSet<CategoryCodes> CategoryCodes { get; set; }
        public DbSet<CityStateZipData> CityStateZipData { get; set; }
        public DbSet<Taxonomy> Taxonomy { get; set; }
        public DbSet<InsuranceBillingoption> InsuranceBillingoption { get; set; }
        public DbSet<ExternalPatient> ExternalPatient { get; set; }
        public DbSet<ClientDocument> ClientDocument { get; set; }
        //  public DbSet<AutoPlanFollowUpLog> AutoPlanFollowUpLog { get; set; }
        public DbSet<PatientStatusCode> PatientStatusCode { get; set; }
        public DbSet<BillClassification> BillClassification { get; set; }
        public DbSet<TypeOfFacility> TypeOfFacility { get; set; }
        public DbSet<AdmissionSourceCode> AdmissionSourceCode { get; set; }
        public DbSet<AdmissionTypeofVisit> AdmissionTypeofVisit { get; set; }
        public DbSet<OccurrenceCode> OccurrenceCode { get; set; }
        public DbSet<OccurrenceSpanCode> OccurrenceSpanCode { get; set; }
        public DbSet<ExternalPayment> ExternalPayment { get; set; }
        public DbSet<RevenueCode> RevenueCode { get; set; }
        public DbSet<PatientStatement> PatientStatement { get; set; }
        public DbSet<PatientStatementChargeHistory> PatientStatementChargeHistory { get; set; }
        public DbSet<ExternalInjuryCode> ExternalInjuryCode { get; set; }
        public DbSet<ValueCode> ValueCode { get; set; }
        public DbSet<ConditionCode> ConditionCode { get; set; }
        public DbSet<PatientAuthorization> PatientAuthorization { get; set; }
        public DbSet<PatientAuthorizationUsed> PatientAuthorizationUsed { get; set; }
        public DbSet<AdmissionTypeCode> AdmissionTypeCode { get; set; }
        public DbSet<InstitutionalData> InstitutionalData { get; set; }
        public DbSet<BatchDocumentCharges> BatchDocumentCharges { get; set; }
        public DbSet<BatchDocumentPayment> BatchDocumentPayment { get; set; }
        public DbSet<OnlinePortals> OnlinePortals { get; set; }
        public DbSet<OnlinePortalCredentials> OnlinePortalCredentials { get; set; }
        public DbSet<Amendments> Amendments { get; set; }

        //New Models are added
        public DbSet<PatientNotes> PatientNotes { get; set; }
        public DbSet<GeneralItems> GeneralItems { get; set; }
        public DbSet<Rooms> Rooms { get; set; }
        public DbSet<PatientVitals> PatientVitals { get; set; }
        public DbSet<PatientMedicalNotes> PatientMedicalNotes { get; set; }
        public DbSet<PatientForms> PatientForms { get; set; }
        public DbSet<ClinicalForms> ClinicalForms { get; set; }
        public DbSet<ClinicalFormsCPT> ClinicalFormsCPT { get; set; }
        public DbSet<AppointmentCPT> AppointmentCPT { get; set; }
        public DbSet<AppointmentICD> AppointmentICD { get; set; }
        public DbSet<CPTMostFavourite> CPTMostFavourite { get; set; }
        public DbSet<ICDMostFavourite> ICDMostFavourite { get; set; }
        public DbSet<PatientReferral> PatientReferral { get; set; }
        public DbSet<PatientAlerts> PatientAlerts { get; set; }  
        public DbSet<FormsSubHeading> FormsSubHeading { get; set; }
        public DbSet<PatientAllergy> PatientAllergy { get; set; }
        public DbSet<PatientFormValue> PatientFormValue { get; set; }
        public DbSet<SMSSentReceived> SMSSentReceived { get; set; }
        public DbSet<PatientAppointmentsExternal> PatientAppointmentsExternal { get; set; }
        public DbSet<ExChargeMissingInfo> ExChargeMissingInfo { get; set; }
        public DbSet<PatientDocument> PatientDocument { get; set; }
        public DbSet<ProblemList> ProblemList { get; set; }
        public DbSet<DocumentCategory> DocumentCategory { get; set; }
        public DbSet<PatientFamilyHistory> PatientFamilyHistory { get; set; }
        public DbSet<EmailCC> EmailCC { get; set; }
        public DbSet<EmailHistory> EmailHistory { get; set; }
        public DbSet<EmailAttachments> EmailAttachments { get; set; }
        public DbSet<EmailTo> EmailTo { get; set; }

        #region Audits
        public DbSet<BatchDocumentAudit> BatchDocumentAudit { get; set; }
        public DbSet<BillerAudit> BillerAudit { get; set; }
        public DbSet<ChargeAudit> ChargeAudit { get; set; }
        public DbSet<ClientAudit> ClientAudit { get; set; }
        public DbSet<CptAudit> CptAudit { get; set; }
        public DbSet<DocumentTypeAudit> DocumentTypeAudit { get; set; }
        public DbSet<Edi270PayerAudit> Edi270PayerAudit { get; set; }
        public DbSet<Edi276PayerAudit> Edi276PayerAudit { get; set; }
        public DbSet<Edi837PayerAudit> Edi837PayerAudit { get; set; }
        public DbSet<ICDAudit> ICDAudit { get; set; }
        public DbSet<InsuranceAudit> InsuranceAudit { get; set; }
        public DbSet<InsurancePlanAddressAudit> InsurancePlanAddressAudit { get; set; }
        public DbSet<InsurancePlanAudit> InsurancePlanAudit { get; set; }
        public DbSet<LocationAudit> LocationAudit { get; set; }
        public DbSet<MainUserPracticeAudit> MainUserPracticeAudit { get; set; }
        public DbSet<ModifierAudit> ModifierAudit { get; set; }
        public DbSet<PatientAudit> PatientAudit { get; set; }
        public DbSet<PatientPlanAudit> PatientPlanAudit { get; set; }
        public DbSet<PlanTypeAudit> PlanTypeAudit { get; set; }
        public DbSet<POSAudit> POSAudit { get; set; }
        public DbSet<PracticeAudit> PracticeAudit { get; set; }
        public DbSet<ProviderAudit> ProviderAudit { get; set; }
        public DbSet<ReceiverAudit> ReceiverAudit { get; set; }
        public DbSet<RefProviderAudit> RefProviderAudit { get; set; }
        public DbSet<StatusCodeAudit> StatusCodeAudit { get; set; }
        public DbSet<SubmissionLogAudit> SubmissionLogAudit { get; set; }
        public DbSet<SubmitterAudit> SubmitterAudit { get; set; }
        public DbSet<TaxonomyAudit> TaxonomyAudit { get; set; }
        public DbSet<TeamAudit> TeamAudit { get; set; }
        public DbSet<TypeOfServiceAudit> TypeOfServiceAudit { get; set; }
        public DbSet<UserPracticeAudit> UserPracticeAudit { get; set; }
        public DbSet<VisitAudit> VisitAudit { get; set; }
        public DbSet<VisitReasonAudit> VisitReasonAudit { get; set; }
        public DbSet<VisitStatusLogAudit> VisitStatusLogAudit { get; set; }
        public DbSet<ActionAudit> ActionAudit { get; set; }
        public DbSet<AdjustmentCodeAudit> AdjustmentCodeAudit { get; set; }
        public DbSet<ChargeSubmissionHistoryAudit> ChargeSubmissionHistoryAudit { get; set; }
        public DbSet<DesignationsAudit> DesignationsAudit { get; set; }
        public DbSet<GroupAudit> GroupAudit { get; set; }
        public DbSet<NotesAudit> NotesAudit { get; set; }
        public DbSet<PatientFollowUpAudit> PatientFollowUpAudit { get; set; }
        public DbSet<PatientPaymentAudit> PatientPaymentAudit { get; set; }
        public DbSet<PatientPaymentChargeAudit> PatientPaymentChargeAudit { get; set; }
        public DbSet<PaymentChargeAudit> PaymentChargeAudit { get; set; }
        public DbSet<PaymentCheckAudit> PaymentCheckAudit { get; set; }
        public DbSet<PaymentLedgerAudit> PaymentLedgerAudit { get; set; }
        public DbSet<PaymentVisitAudit> PaymentVisitAudit { get; set; }
        public DbSet<ProviderScheduleAudit> ProviderScheduleAudit { get; set; }
        public DbSet<ReasonAudit> ReasonAudit { get; set; }
        public DbSet<RemarkCodeAudit> RemarkCodeAudit { get; set; }
        public DbSet<ReportsLogAudit> ReportsLogAudit { get; set; }
        public DbSet<ResubmitHistoryAudit> ResubmitHistoryAudit { get; set; }
        public DbSet<RightsAudit> RightsAudit { get; set; }
        public DbSet<DownloadedFileAudit> DownloadedFileAudit { get; set; }
        public DbSet<PlanFollowupAudit> PlanFollowupAudit { get; set; }
        public DbSet<PatientEligibilityAudit> PatientEligibilityAudit { get; set; }
        public DbSet<PatientEligibilityDetailAudit> PatientEligibilityDetailAudit { get; set; }
        public DbSet<PatientEligibilityLogAudit> PatientEligibilityLogAudit { get; set; }
        public DbSet<PatientFollowUpChargeAudit> PatientFollowUpChargeAudit { get; set; }
        public DbSet<PlanFollowupChargeAudit> PlanFollowupChargeAudit { get; set; }
        public DbSet<ProviderSlotAudit> ProviderSlotAudit { get; set; }
        public DbSet<SettingsAudit> SettingsAudit { get; set; }
        public DbSet<VisitStatusAudit> VisitStatusAudit { get; set; }
        public DbSet<AuditException> AuditException { get; set; }
        public DbSet<InsuranceBillingoptionAudit> InsuranceBillingoptionAudit { get; set; }
        public DbSet<OnlinePortalsAudit> OnlinePortalsAudit { get; set; }
        public DbSet<OnlinePortalCredentialsAudit> OnlinePortalCredentialsAudit { get; set; }
        public DbSet<AppointmentCPTAudit> AppointmentCPTAudit { get; set; }
        public DbSet<AppointmentICDAudit> AppointmentICDAudit { get; set; }
        public DbSet<PatientFormsAudit> PatientFormsAudit { get; set; }
        #endregion

        #region DeletedLog
        public DbSet<ActionDeleted> ActionDeleted { get; set; }
        #endregion


    }
}

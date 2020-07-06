//using MediFusion.Models;
//using MediFusion.Models.TodoApi;
//using MediFusion.Models.TodoApi.Models;
//using MediFusion.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using WebApplication1.ViewModel;
//using System.Data.Entity;

namespace WebApplication1.Models
{
    public class PMContext1 : IdentityDbContext<AuthIdentityCustom>
    {
        public PMContext1()
        {

        }
         
        public readonly IHostingEnvironment env;

        public PMContext1(IHostingEnvironment env, DbContextOptions<PMContext1> options):base(options)
        {
            this.env = env;
        }




        ////public DbSet<TodoItem> TodoItems { get; set; }
        //public DbSet<Practice> Practice { get; set; }
        //public DbSet<Location> Location { get; set; }
        //public DbSet<Provider> Provider { get; set; }
        //public DbSet<RefProvider> RefProvider { get; set; }
        //public DbSet<Insurance> Insurance { get; set; }
        //public DbSet<InsurancePlan> InsurancePlan { get; set; }

        //public DbSet<ICD> ICD { get; set; }
        //public DbSet<Cpt> Cpt { get; set; }
        //public DbSet<Patient> Patient { get; set; }
        //public DbSet<POS> POS { get; set; }
        //public DbSet<Modifier> Modifier { get; set; }
        //public DbSet<TypeOfService> TypeOfService { get; set; }
        ////public DbSet<PlanType> PlanType { get; set; }
        //public DbSet<Edi837Payer> Edi837Payer { get; set; }
        //public DbSet<Edi276Payer> Edi276Payer { get; set; }
        //public DbSet<Edi270Payer> Edi270Payer { get; set; }
        //public DbSet<Receiver> Receiver { get; set; }
        //public DbSet<InsurancePlanAddress> InsurancePlanAddress { get; set; }
        //public DbSet<PatientPlan> PatientPlan { get; set; }
        //public DbSet<Charge> Charge { get; set; }
        //public DbSet<Visit> Visit { get; set; }
        //public DbSet<Submitter> Submitter { get; set; }

        //public DbSet<SubmissionLog> SubmissionLog { get; set; }
        //public DbSet<ChargeSubmissionHistory> ChargeSubmissionHistory { get; set; }
        //public DbSet<Settings> Settings { get; set; }
        //public DbSet<PaymentCheck> PaymentCheck { get; set; }
        ////public DbSet<PaymentVisit> PaymentVisit { get; set; }
        ////public DbSet<PaymentCharge> PaymentCharge { get; set; }
        //public DbSet<ResubmitHistory> ResubmitHistory { get; set; }
        //public DbSet<Models.Audit.ChargeAudit> ChargeAudit { get; set; }
        //public DbSet<Models.Audit.VisitAudit> VisitAudit { get; set; }
        //public DbSet<Group> Group { get; set; }
        //public DbSet<Reason> Reason { get; set; }
        //public DbSet<Biller> Biller { get; set; }
        //public DbSet<Action> Action { get; set; }
        ////public DbSet<PlanFollowup> PlanFollowUp { get; set; }
        //public DbSet<BatchDocument> BatchDocument { get; set; }
        //public DbSet<DocumentType> DocumentType { get; set; }
        //public DbSet<RemarkCode> RemarkCode { get; set; }
        ////public DbSet<AdjusmentCode> AdjusmentCode { get; set; }
        ////public DbSet<PatientFollowUp> PatientFollowUp { get; set; }
        //public DbSet<PatientFollowUpCharge> PatientFollowUpCharge { get; set; }
        //public DbSet<PatientPayment> PatientPayment { get; set; }
        //public DbSet<Client> Client { get; set; }
        //public DbSet<UserPractices> UserPractices { get; set; }
        //public DbSet<Rights> Rights { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserPractices>()
        .HasKey(c => new { c.UserID, c.PracticeID });

            builder.Entity<AuthIdentityCustom>()
            .HasOne(p => p.Rights)
            .WithOne(i => i.AuthIdentityCustom)
            .HasForeignKey<Rights>(b => b.Id);


            //builder.Entity<Rights>()
            //.HasKey(c => new { c.Id });
        }
    }


    

}

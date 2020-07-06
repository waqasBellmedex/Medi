using MediFusionPM.Models.Audit;
using MediFusionPM.Models.Main;
using MediFusionPM.ViewModels.Main;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MediFusionPM.Models
{
    public class MainContext : IdentityDbContext<MainAuthIdentityCustom>
    {
        public readonly IHostingEnvironment env;
        public MainContext(IHostingEnvironment env) : base()
        {
            this.env = env;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MainUserPractices>()
            .HasKey(c => new { c.UserID, c.PracticeID });

           // builder.Entity<MainAuthIdentityCustom>()
           //.HasOne(p => p.ReferenceUserID)
           //.WithMany()
           //.HasForeignKey(b => b.RefUserID);
        }

        public IConfigurationRoot Configuration { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("Medifusion"));

            
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


        public DbSet<MainPractice> MainPractice { get; set; }
        public DbSet<MainClient> MainClient { get; set; }
        public DbSet<MainUserPractices> MainUserPractices { get; set; }
        public DbSet<MainRights> MainRights { get; set; }
        public DbSet<MainUserPracticeAudit> MainUserPracticeAudit { get; set; }
        public DbSet<MainTeam> MainTeam { get; set; }
        public DbSet<MainDesignations> MainDesignations { get; set; }
        public DbSet<ExInsuranceMapping> ExInsuranceMapping { get; set; }
        public DbSet<MainUserLoginHistory> MainUserLoginHistory { get; set; }
        public DbSet<AutoPlanFollowUpLog> AutoPlanFollowUpLog { get; set; }

        public DbSet<AutoDownloadingLog> AutoDownloadingLog { get; set; }
        public DbSet<SMSSentReceived> SMSSentReceived { get; set; }
        public DbSet<MainAuthIdentityCustom> MainAuthIdentityCustom { get; set; }
        //  public DbSet<MainPracticeResponsibilities> MainPracticeResponsibilities { get; set; }

    }
}

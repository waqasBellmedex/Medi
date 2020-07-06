using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models.Main;

namespace MediFusionPM.ViewModels.Main
{
    public class MainAuthIdentityCustom : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public bool IsUserLogin { get; set; } // use for only 1 person Login
        public int LogInAttempts { get; set; } // use for counter 1 2 3 after this PersonStatus block
        public bool IsUserBlock { get; set; }
        public bool IsUserBlockByAdmin { get; set; }
        public string BlockNote { get; set; }


        [ForeignKey("MainClient")]
        public long? ClientID { get; set; }
        public virtual MainClient MainClient { get; set; }

        [ForeignKey("MainPractice")]
        public long? PracticeID { get; set; }
        public virtual MainPractice MainPractice { get; set; }

        public MainRights MainRights { get; set; }
        [ForeignKey("MainTeam")]
        public long? TeamID { get; set; }
        public MainTeam MainTeam { get; set; }
        [ForeignKey("MainDesignations")]
        public long? DesignationID { get; set; }
        public MainDesignations MainDesignations { get; set; }
        public string ReportingTo { get; set; }
        public string signatureURL { get; set; }

        public string signatureText { get; set; }
        //public string RefUserID { get; set; }
        //public MainAuthIdentityCustom ReferenceUserID { get; set; }
    }
}

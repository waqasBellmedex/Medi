using MediFusionPM.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ViewModel
{
    public class AuthIdentityCustom : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public bool IsUserLogin { get; set; } // use for only 1 person Login
        public int LogInAttempts { get; set; } // use for counter 1 2 3 after this PersonStatus block
        public bool IsUserBlock { get; set; }
        public bool IsUserBlockByAdmin { get; set; }
        public string BlockNote { get; set; }
        [ForeignKey("Client")]
        public long? ClientID { get; set; }
        public virtual Client Client { get; set; }
        [ForeignKey("Practice")]
        public long? PracticeID { get; set; }
        public virtual Practice Practice { get; set; }

        public Rights Rights { get; set; }
        [ForeignKey("Team")]
        public long? TeamID { get; set; }
        public Team Team { get; set; }
        [ForeignKey("Designations")]
        public long? DesignationID { get; set; }
        public Designations Designations { get; set; }
        public string ReportingTo { get; set; }
       // public string RefUserID { get; set; }
       // public AuthIdentityCustom ReferenceUserID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class PatientReferral
    {
        [Required]
        public long ID { get; set; }
        public long? PatientID { get; set; }
        public long? PCPID { get; set; }
        public long? InsurancePlanID { get; set; }
        [ForeignKey("ID")]
        public long? ProviderID { get; set; }
        //public virtual Provider Provider { get; set; }
        //[ForeignKey("ID")]
        public long? PatientPlanID { get; set; }
        //public virtual PatientPlan PatientPlan { get; set; }
        public DateTime?  StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? VisitAllowed { get; set; }
        public int? VisitUsed { get; set; }
        public string  Status { get; set; }
        public string ReferralNo { get; set; }
        public string FaxStatus { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [NotMapped]
        public string CommonKey { get; set; }
        public string RererralForService { get; set; }

    }
}

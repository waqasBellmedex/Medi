using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class PatientAuthorization
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public long? PatientID { get; set; }
        public long? InsurancePlanID { get; set; }
        public long? PatientPlanID { get; set; }

        public long? ProviderID { get; set; }
        public long? ICDID { get; set; }
        public long? CPTID { get; set; }
        public string AuthorizationNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long? VisitsAllowed { get; set; }
        public long? VisitsUsed { get; set; }
        public string Remarks { get; set; }
        public string Remaining{ get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDate { get; set; }
        [NotMapped]
        public string CommonKey { get; set; }
        public decimal? AuthorizedAmount { get; set; }
        public bool? MedicalRecordRequired { get; set; }
        public bool? MedicalNecessityRequired { get; set; }
        public string Status { get; set; }
        public string ResponsibleParty { get; set; }
        public DateTime? AuthorizationDate { get; set; }
        public int? RemindBeforeDays { get; set; }
        public int? RemindBeforeRemainingVisits { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{

    public class VisitStatus
    {
        [Required]
        public long ID { get; set; }

        public long ?VisitID { get; set; }

        [ForeignKey("ID")]
        public long? VisitStatusLogID { get; set; }
        public virtual VisitStatusLog VisitStatusLog  { get; set; }
        public DateTime? DOS { get; set; }
        public decimal VisitAmount { get; set; }
        public string ResponseEntity { get; set; }

        public long ?PracticeID { get; set; }
        public long ?LocationID { get; set; }
        public long ?ProviderID { get; set; }
        public long ?PatientPlanID { get; set; }
        public string SubmitterTRN { get; set; }
        public string TRNNumber { get; set; }
        public string ActionCode { get; set; }

        public string Status { get; set; }

        public string SubscriberLN { get; set; }
        public string SubscriberFN { get; set; }
        public string SubscriberMI { get; set; }
        public string SubscriberID { get; set; }
        public string SubscriberAddress { get; set; }
        public string SubscriberCity { get; set; }
        public string SubscriberState { get; set; }
        public string SubscriberZip { get; set; }
        public DateTime SubscriberDOB { get; set; }
        public string SubscriberGender { get; set; }

        public string PatientLN { get; set; }
        public string PatientFN { get; set; }
        public string PatientMI { get; set; }
        public string PatientAddress { get; set; }
        public string PatientCity { get; set; }
        public string PatientState { get; set; }
        public string PatientZip { get; set; }
        public DateTime PatientDOB { get; set; }
        public string PatientGender { get; set; }

        public string ProviderLN { get; set; }
        public string ProviderFN { get; set; }
        public string ProviderNPI { get; set; }

        public string PayerName { get; set; }
        public string PayerID { get; set; }
        public string ErrorMessage { get; set; }

        public string CategoryCode1 { get; set; }
        public string CategoryCodeDesc1 { get; set; }
        public string StatusCode1 { get; set; }
        public string StatusCodeDesc1 { get; set; }
        public string EntityCode1 { get; set; }
        public string EntityCodeDesc1 { get; set; }
        public string RejectionReason1 { get; set; }
        public string FreeText1 { get; set; }

        public string CategoryCode2 { get; set; }
        public string CategoryCodeDesc2 { get; set; }
        public string StatusCode2 { get; set; }
        public string StatusCodeDesc2 { get; set; }
        public string EntityCode2 { get; set; }
        public string EntityCodeDesc2 { get; set; }
        public string RejectionReason2 { get; set; }
        public string FreeText2 { get; set; }

        public string CategoryCode3 { get; set; }
        public string CategoryCodeDesc3 { get; set; }
        public string StatusCode3 { get; set; }
        public string StatusCodeDesc3 { get; set; }
        public string EntityCode3 { get; set; }
        public string EntityCodeDesc3 { get; set; }
        public string RejectionReason3 { get; set; }
        public string FreeText3 { get; set; }

        public string PayerControlNumber { get; set; }
        public DateTime? StatusDate { get; set; }
        public decimal? BilledAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public DateTime? CheckDate { get; set; }
        public string CheckNumber { get; set; }

        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }



}

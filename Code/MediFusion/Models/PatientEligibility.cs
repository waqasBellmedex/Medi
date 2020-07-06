using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{

    public class PatientEligibility
    {
        [Required]
        public long ID { get; set; }

        public DateTime ?EligibilityDate { get; set; }
        public DateTime? DOS { get; set; }
        public long PatientID { get; set; }
        public long PracticeID { get; set; }
        public long ?LocationID { get; set; }
        public long ProviderID { get; set; }
        public long PatientPlanID { get; set; }
        public string TRNNumber { get; set; }
        public string Relation { get; set; }
        public string Status { get; set; }
        public string Rejection { get; set; }
        public string RejectionCode { get; set; }

        public string SubscriberLN { get; set; }
        public string SubscriberFN { get; set; }
        public string SubscriberMI { get; set; }
        public string SubscriberID { get; set; }
        public string SubscriberGroupNumber { get; set; }
        public string SubscriberAddress { get; set; }
        public string SubscriberCity { get; set; }
        public string SubscriberState { get; set; }
        public string SubscriberZip { get; set; }
        public DateTime ?SubscriberDOB { get; set; }
        public string SubscriberGender { get; set; }

        public string PatientLN { get; set; }
        public string PatientFN { get; set; }
        public string PatientMI { get; set; }
        public string PatientAddress { get; set; }
        public string PatientCity { get; set; }
        public string PatientState { get; set; }
        public string PatientZip { get; set; }
        public DateTime ?PatientDOB { get; set; }
        public string PatientGender { get; set; }

        public string ProviderLN { get; set; }
        public string ProviderFN { get; set; }
        public string ProviderNPI { get; set; }

        public string PayerName { get; set; }
        public string PayerID { get; set; }
        public string ErrorMessage { get; set; }

        public string AddedBy { get; set; }
        public DateTime ?AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime ?UpdatedDate { get; set; }
    }



}

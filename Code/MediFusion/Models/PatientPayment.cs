using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{
    public class PatientPayment
    {
        [Required]
        public long ID { get; set; }

        [ForeignKey("ID")]
        public long ?PatientID { get; set; }
		public virtual Patient Patient{get;set;}
		[ForeignKey("ID")]
        public long? patientAppointmentID { get; set; }
        public virtual PatientAppointment PatientAppointment{get;set;}
        public bool? InActive { get; set; }
      

        public string PaymentMethod { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? PaymentAmount { get; set; }
        public string CheckNumber { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        public decimal? AllocatedAmount { get; set; }
        public decimal? RemainingAmount { get; set; }

        public long ?VisitID { get; set; }
        public string Type { get; set; }


        [ForeignKey("PatientPaymentID")]
        public ICollection<PatientPaymentCharge> PatientPaymentCharges { get; set; }

        public string AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CCTransactionID { get; set; }

        public long? AdvancePatientPaymentID { get; set; }
        public string AdvanceAppliedOnVisits { get; set; }


    }
}

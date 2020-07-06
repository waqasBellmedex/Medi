using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{
    public class PatientAppointment
    {
        [Required]
        public long ID { get; set; }
        [ForeignKey("ID")]
        public long  PatientID  { get; set; }
        public virtual Patient Patient  { get; set; }
        [ForeignKey("ID")]
        public long ?LocationID { get; set; }
        public virtual Location  Location { get; set; }
        [ForeignKey("ID")]
        public long ?ProviderID { get; set; }
        public virtual Provider Provider { get; set; }

        //[ForeignKey("ID")]
       // public long? ProviderSlotID { get; set; }
        //public virtual ProviderSlot ProviderSlot { get; set; }
        [ForeignKey("ID")]
        public long? PrimarypatientPlanID { get; set; }
       // public bool? selfPay { get; set; }
        public virtual PatientPlan PrimarypatientPlan { get; set; }
        [ForeignKey("ID")]
        public long? VisitReasonID { get; set; }
        public virtual VisitReason VisitReason { get; set; }
        
        public DateTime? AppointmentDate { get; set; }
        public DateTime? Time { get; set; }
        [NotMapped]
        public string AppointmentTime { get; set; }
        public long parentAppointmentID { get; set; }
        public int?  VisitInterval { get; set; }
        public string Status { get; set; }
        [MaxLength(500)]
        public string Notes { get; set; }
        public bool? Inactive { get; set; }
        
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [ForeignKey("ID")]
        public long? RoomID { get; set; }
        public string color { get; set; }
        [NotMapped]

        public List<AppointmentCPT> CPTs;
        [NotMapped]
        public List<Charge> charges;
        [NotMapped]
        public List<AppointmentICD> ICDs;
        [NotMapped]
        public List<PatientForms> forms;
        [NotMapped]

        public List<PatientPayment> patientPayment;
        public bool? priorAuthorization { get; set; }
        public bool? recurringAppointment { get; set; } 
        public int? recurringNumber { get; set; }
        public string recurringfrequency { get; set; }
    }
}

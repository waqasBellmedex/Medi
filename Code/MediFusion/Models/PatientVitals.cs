using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class PatientVitals
    {
        public long ID { get; set; }
        [ForeignKey("ID")]
        public long? PatientNotesId { get; set; }
        public virtual PatientNotes PatientNotes { get; set; }
        [NotMapped] 
        public DateTime? DOS { get; set; }
        [NotMapped]
        public long? appointmentID { get; set; }
        [NotMapped]
        public long? LocationID { get; set; }
        [NotMapped]
        public long? ProviderID { get; set; }
        [NotMapped]
        public long? PatientID { get; set; }

        public decimal? Height_foot { get; set; }
        public decimal? Height_cm { get; set; }
        public decimal? Weight_lbs { get; set; }
        public decimal? Weight_pounds { get; set; }
        public decimal? BMI { get; set; }
        public decimal? BPSystolic { get; set; }
        public decimal? BPDiastolic { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Pulse { get; set; }
        public decimal? Respiratory_rate { get; set; }
        public decimal? OxygenSaturation { get; set; }
        public int? Pain { get; set; }
        public decimal? HeadCircumference { get; set; }
        public decimal PracticeID { get; set; }
        public bool? Inactive { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class PatientForms
    {
        [Required]
        public long ID { get; set; }
        public long? PatientAppointmentID { get; set; }
        public long? PatientID { get; set; }
        public long? ClinicalFormID { get; set; }
        public long? PracticeID { get; set; }
        public bool? Inactive { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? reportHeader { get; set; }
        public bool? Signed { get; set; }
        public string SignedBy { get; set; }
        public string SignatureUrl { get; set; } 
        public DateTime? SignedDate { get; set; } 
        public bool? CoSigned { get; set; }
        public string CoSignedBy { get; set; }
        public string CoSignatureUrl { get; set; }
        public DateTime? CoSignedDate { get; set; }
    }
}

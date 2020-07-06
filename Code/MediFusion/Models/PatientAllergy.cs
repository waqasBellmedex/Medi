using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class PatientAllergy
    {
        public long ID { get; set; }
        public long PatientNotesId { get; set; }
        public string AllergyType { get; set; }
        public string SpecificDrugAllergy { get; set; }
        public string Reaction { get; set; }
        public string Severity { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public decimal PracticeID { get; set; }
        public bool? Inactive { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

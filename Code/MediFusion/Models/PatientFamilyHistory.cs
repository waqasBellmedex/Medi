using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class PatientFamilyHistory
    {

        public long ID { get; set; }
        public string desc { get; set; }
        public string relationship { get; set; }

        public string type { get; set; }
        public bool? status { get; set; }

        public string dosage { get; set; }
        public string drugName { get; set; }

        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        [ForeignKey("ID")]
        public long ICDID { get; set; }
        public virtual ICD ICD { get; set; }
        [ForeignKey("ID")]
        public long patientNotesID { get; set; }
        public virtual PatientNotes patientNotes { get; set; }
        [ForeignKey("ID")]
        public long patientID { get; set; }
        public virtual Patient patient { get; set; }
        [ForeignKey("ID")]
        public long PracticeID { get; set; }
        public virtual Practice Practice { get; set; }
        public bool? inActive { get; set; }

        public string AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    



    }
}

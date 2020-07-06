using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class PatientMedicalNotes
    {
        [Required]
        public long ID { get; set; }
        public long PatientNotesId { get; set; }
        public string note { get; set; }
        public string note_html { get; set; }
        public bool Inactive { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}

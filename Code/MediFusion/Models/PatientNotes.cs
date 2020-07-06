using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class PatientNotes
    {
        public long ID { get; set; }
        public long PatientID { get; set; }
        public DateTime DOS { get; set; }
        public long ProviderID { get; set; }
        public long LocationID { get; set; }
        public long? AppointmentID { get; set; }
        public long? DocumentID { get; set; }
        public int? DocumentSize { get; set; }
        public bool? Signed { get; set; }
        public string SignedBy { get; set; }
        public DateTime? SignedDate { get; set; }
        public string CoSignedBy { get; set; }
        public DateTime? CoSignedDate { get; set; }
        public long PracticeID { get; set; }
        public bool? Inactive { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

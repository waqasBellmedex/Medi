using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{
    public class Notes
    {
        public long ID { get; set; }
        [ForeignKey("ID")]
        public long? PracticeID { get; set; }
        public virtual Practice Practice { get; set; }
        public long? PlanFollowupID { get; set; }
        public long? PatientFollowUpID { get; set; }
        public long? PatientID { get; set; }
        public long? VisitID { get; set; }
        public long? BatchDocumentNoID { get; set; }
        public string Note { get; set; }
        public DateTime? NotesDate { get; set; }
        public string AddedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDate { get; set; }

    }
}

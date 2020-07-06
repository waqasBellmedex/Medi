using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class PatientAlerts
    {
        [Key]
        public long ID { get; set; }
        [ForeignKey("ID")]
        public long patientID { get; set; }
        public string type { get; set; }
        public DateTime? date { get; set; }
        public string assignedTo { get; set; }
        public string notes { get; set; }
        public long? practiceId { get; set; }
        public bool? inactive { get; set; }
        public DateTime? AddedDate { get; set; }
        public string AddedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool? resolved { get; set; }
        public string resolvedBy { get; set; }
        public DateTime? resolvedDate { get; set; }
        public string resolveComments { get; set; }
    }
}

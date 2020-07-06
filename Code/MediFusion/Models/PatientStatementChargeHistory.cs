using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class PatientStatementChargeHistory 
    {
        [Required]
        public long ID { get; set; }

        [ForeignKey("ID")]
        public long PatientStatementID { get; set; }
        [ForeignKey("ID")]
        public long ChargeID { get; set; }
        public decimal? amount { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string AddedBy { get; set; }
        public string UpdatedBy { get; set; }
        
    }
}

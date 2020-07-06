using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class PatientStatement
    {
        [Required]
        public long ID { get; set; }

        [ForeignKey("ID")]
        public long PatientID { get; set; }
        [ForeignKey("ID")]
        public long PracticeID { get; set; }
        [ForeignKey("ID")]
        public long  VisitID { get; set; } 
        public string pdf_url { get; set; }
        public string csv_url { get; set; }
        public decimal? amount { get; set; }
        public int statementStatus { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }  
        public DateTime? AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string AddedBy { get; set; }
        public string UpdatedBy { get; set; }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{
    public class ClinicalFormsCPT
    {
        public long ID { get; set; }

        [ForeignKey("ID")]

        public long ClinicalFormID { get; set; }
        public ClinicalForms ClinicalForm { get; set; }
        [ForeignKey("ID")]

        public  long CPTID { get; set; }
        [NotMapped]
        public string cptCode { get; set; }
        [NotMapped]
        public string description { get; set; }
        public string Modifier { get; set; }  
        public decimal Price { get; set; }
        [ForeignKey("ID")]

        public long? PracticeID { get; set; }
        public virtual Practice Practice { get; set; }
        public bool? Inactive { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}

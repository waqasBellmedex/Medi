using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{
    public class InsuranceBillingoption
    {
        [Required]
        public long ID { get; set; }
        [ForeignKey("ID")]
        public long ProviderID { get; set; }
        public virtual Provider Provider { get; set; }
        [ForeignKey("ID")]
        public long LocationID { get; set; }
        public virtual Location Location { get; set; }
        public long InsurancePlanID { get; set; }
        public bool? ReportTaxID { get; set; }
        public string PayToAddress { get; set; }
        public string AddedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDate { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models.Audit
{
    public class Edi837PayerAudit
    {
        [Required]
        public long ID { get; set; }
        [ForeignKey("ID")]
        public long Edi837PayerID { get; set; }
        public virtual Edi837Payer Edi837Payer { get; set; }
        public long TransactionID { get; set; }
        public string ColumnName { get; set; }
        public string CurrentValue { get; set; }
        public string NewValue { get; set; }
        public string CurrentValueID { get; set; }
        public string NewValueID { get; set; }

        public string HostName { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
   
    }
}

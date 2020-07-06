using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MediFusionPM.Models
{
    public class Edi837Payer
    {
        public long ID { get; set; }
        public string PayerName { get; set; }
        public string PayerID { get; set; }
        [ForeignKey("ID")]
        public long ReceiverID { get; set; }
        public virtual Receiver Receiver { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Enrollment { get; set; }
        public string Type { get; set; }
        public string ERA { get; set; }
        public string Secondary { get; set; }
        public string notes { get; set; }
    }
}

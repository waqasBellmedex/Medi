using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediFusionPM.Models.TodoApi;

namespace MediFusionPM.Models
{
    public class PatientFollowUpCharge
    {
        [Required]
        public long ID { get; set; }
       
        public long? PatientFollowUpID { get; set; }
       
   
        [ForeignKey("ID")]
        public long? ChargeID { get; set; }
        public virtual Charge Charge { get; set; }

        [ForeignKey("ID")]
        public long? ReasonID { get; set; }
        public virtual Reason Reason { get; set; }

        [ForeignKey("ID")]
        public long? ActionID { get; set; }
        public virtual Action Action { get; set; }
        [ForeignKey("ID")]
        public long? GroupID { get; set; }
        public virtual Group Group { get; set; }

        public long? AdjustmentCodeID { get; set; }

        public long? PaymentChargeID { get; set; }

        public DateTime? Statement1SentDate { get; set; }
        public DateTime? Statement2SentDate { get; set; }
        public DateTime? Statement3SentDate { get; set; }
        
        public string Status  { get; set; }
        [DataType(DataType.DateTime)]
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediFusionPM.Models.TodoApi.Models;

namespace MediFusionPM.Models.TodoApi
{
    public class PlanFollowupCharge
    {
        public long ID { get; set; }
        public long? PlanFollowupID { get; set; } //Refrence s Foreign Key From PlanFollowup through ICollection
        [ForeignKey("ID")]
        public long ?ChargeID { get; set; }
        public virtual Charge Charge { get; set; }
        
        [ForeignKey("ID")]
        public long ?GroupID { get; set; }
        public virtual Group Group { get; set; }
        [ForeignKey("ID")]
        public long? ReasonID { get; set; }
        public virtual Reason Reason { get; set; }
        [ForeignKey("ID")]
        public long? ActionID { get; set; }
        public virtual Action Action { get; set; }
        public long? AdjustmentCodeID { get; set; }
   
        public string Notes { get; set; }
        [NotMapped]
        public string Age { get; set; }

        [ForeignKey("ID")]
        public long ?PaymentChargeID { get; set; }
        public virtual PaymentCharge PaymentCharge { get; set; }
    [ForeignKey("ID")]
        public long? RemarkCode1ID { get; set; }
        public virtual RemarkCode RemarkCode { get; set; }
        [ForeignKey("ID")]
        public long? RemarkCode2ID { get; set; }
        public virtual RemarkCode RemarkCode2 { get; set; }
        [ForeignKey("ID")]
        public long? RemarkCode3ID { get; set; }
        public virtual RemarkCode RemarkCode3 { get; set; }
        [ForeignKey("ID")]
        public long? RemarkCode4ID { get; set; }
        public virtual RemarkCode RemarkCode4 { get; set; }

        public DateTime? TickleDate { get; set; }

        public string AddedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ?AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ?UpdatedDate { get; set; }
        
    }
}

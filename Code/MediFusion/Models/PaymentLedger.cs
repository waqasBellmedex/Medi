using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediFusionPM.Models.TodoApi.Models;

namespace MediFusionPM.Models
{
    public class PaymentLedger

    {
        [Required]
        public long ID { get; set; }
        [ForeignKey("ID")]
        public long ChargeID { get; set; }
        public virtual Charge Charge { get; set;}
        [ForeignKey("ID")]
        public long VisitID { get; set; }
        public virtual Visit Visit { get; set; }
        [ForeignKey("ID")]
        public long? PatientPlanID { get; set; }
        public virtual PatientPlan PatientPlan { get; set; }
        [ForeignKey("ID")]
        public long? PatientPaymentChargeID { get; set; }
        public virtual PatientPaymentCharge PatientPaymentCharge { get; set; }

        [ForeignKey("ID")]
        public long? PaymentChargeID { get; set; }
        public virtual PaymentCharge PaymentCharge { get; set; }


        [ForeignKey("ID")]
        public long? AdjustmentCodeID { get; set; }
        public virtual AdjustmentCode AdjustmentCode { get; set; }
        [Column(TypeName= "Date")]
        public DateTime? LedgerDate { get; set; }
        public string LedgerBy { get; set; }
        public string LedgerType { get; set; }
        public string LedgerDescription { get; set; }
        public decimal? Amount { get; set; }
        [MaxLength(500)]
        public string Notes { get; set; }

        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }


    }
}

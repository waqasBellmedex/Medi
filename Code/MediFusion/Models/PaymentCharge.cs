using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{
    namespace TodoApi.Models
    {
        public class PaymentCharge
        {
            [Required]
            public long ID { get; set; }

            //  [ForeignKey("ID")]
            public long? PaymentVisitID { get; set; }
            // public virtual PaymentVisit PaymentVisit { get; set; }

            [ForeignKey("ID")]
            public long? ChargeID { get; set; }
            public virtual Charge Charge { get; set; }

            public bool AppliedToSec { get; set; }
            public string CPTCode { get; set; }
            public string Modifier1 { get; set; }
            public string Modifier2 { get; set; }
            public string Modifier3 { get; set; }
            public string Modifier4 { get; set; }
            public decimal? BilledAmount { get; set; }
            public decimal? PaidAmount { get; set; }
            public string RevenueCode { get; set; }
            public string Units { get; set; }
            public DateTime? DOSFrom { get; set; }
            public DateTime? DOSTo { get; set; }
            public string ChargeControlNumber { get; set; }

           public  decimal? Copay { get; set; }

            public decimal? PatientAmount { get; set; }
            public decimal? DeductableAmount { get; set; }
            public decimal? CoinsuranceAmount { get; set; }
            public decimal? WriteoffAmount { get; set; }
            public decimal? AllowedAmount { get; set; }
            public decimal? OtherPatResp { get; set; }
            [ForeignKey("AdjustmentCode1")]
            public long? AdjustmentCodeID1 { get; set; }
            public virtual AdjustmentCode AdjustmentCode1 { get; set; }
            public string GroupCode1 { get; set; }
            public decimal? AdjustmentAmount1 { get; set; }
            public string AdjustmentQuantity1 { get; set; }
           [ForeignKey("AdjustmentCode2")]
            public long ? AdjustmentCodeID2 { get; set; }
            public virtual AdjustmentCode AdjustmentCode2 { get; set; }
            public string GroupCode2 { get; set; }
            public decimal? AdjustmentAmount2 { get; set; }
            public string AdjustmentQuantity2 { get; set; }
            [ForeignKey("AdjustmentCode3")]
            public long ? AdjustmentCodeID3 { get; set; }
            public virtual AdjustmentCode AdjustmentCode3 { get; set; }
            public string GroupCode3 { get; set; }
            public decimal? AdjustmentAmount3 { get; set; }
            public string AdjustmentQuantity3 { get; set; }
            [ForeignKey("AdjustmentCode4")]
            public long ? AdjustmentCodeID4 { get; set; }
           public virtual AdjustmentCode AdjustmentCode4 { get; set; }
            public string GroupCode4 { get; set; }
            public decimal? AdjustmentAmount4 { get; set; }
            public string AdjustmentQuantity4 { get; set; }

            [ForeignKey("AdjustmentCode5")]
            public long ? AdjustmentCodeID5 { get; set; }
            public virtual AdjustmentCode AdjustmentCode5 { get; set; }
            public string GroupCode5 { get; set; }
            public decimal? AdjustmentAmount5 { get; set; }
            public string AdjustmentQuantity5 { get; set; }

            [ForeignKey("RemarkCode1")]
            public long ?RemarkCodeID1 { get; set; }
            public virtual  RemarkCode RemarkCode1 { get; set; }
            [ForeignKey("RemarkCode2")]
            public long ?RemarkCodeID2 { get; set; }
            public virtual RemarkCode RemarkCode2 { get; set; }
            [ForeignKey("RemarkCode3")]
            public long ?RemarkCodeID3 { get; set; }
            public virtual RemarkCode RemarkCode3 { get; set; }
            [ForeignKey("RemarkCode4")]
            public long ?RemarkCodeID4 { get; set; }
            public virtual RemarkCode RemarkCode4 { get; set; }
            [ForeignKey("RemarkCode5")]
            public long ?RemarkCodeID5 { get; set; }
            public virtual RemarkCode RemarkCode5 { get; set; }


            public string Comments { get; set; }
            public string Status { get; set; }
            public DateTime? PostedDate { get; set; }
            public string PostedBy { get; set; }
            public string AddedBy { get; set; }
            public DateTime AddedDate { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedDate { get; set; }
           
        }


    }
}

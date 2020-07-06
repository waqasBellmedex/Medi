using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{
    public class ChargeStatus
    {
        [Required]
        public long ID { get; set; }

        public long VisitStatusID { get; set; }
        public long ChargeID { get; set; }

        // 2. Charge_Claim_Status
        // Add Following Columns.
        // ID, Claim_Status_ID, Charge_Category_Code, Charge_Status_Code, 
        //CPT, DOS, Charge_Amount, Modifier1, Modifier2, Modifier3, Modifier4, 
        // Paid_Amount, Check_Date, Check_Number

        public string CPT { get; set; }
        public DateTime? DOS { get; set; }
        public decimal? Amount { get; set; }
        public decimal? BilledAmount { get; set; }
        public string Modifier1 { get; set; }
        public string Modifier2 { get; set; }
        public string Modifier3 { get; set; }
        public string Modifier4 { get; set; }
        public DateTime? CheckDate { get; set; }
        public decimal? PaidAmount { get; set; }
        public string CheckNumber { get; set; }

        public string CategoryCode1 { get; set; }
        public string StatusCode1 { get; set; }
        public string EntityCode1 { get; set; }
        public string RejectionReason1 { get; set; }

        public string CategoryCode2 { get; set; }
        public string StatusCode2 { get; set; }
        public string EntityCode2 { get; set; }
        public string RejectionReason2 { get; set; }

        public string CategoryCode3 { get; set; }
        public string StatusCode3 { get; set; }
        public string EntityCode3 { get; set; }
        public string RejectionReason3 { get; set; }

        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}

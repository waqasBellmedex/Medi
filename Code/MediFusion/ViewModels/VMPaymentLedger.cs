using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ViewModels
{
    public class VMPaymentLedger
    {

        public class GPaymentLedger 
        {
            public long ID { get; set; }
            public long ChargeID { get; set; }
            public long VisitID { get; set; }
            public long?  PatientPlanID { get; set; }
            public string Covrage { get; set; }
            public string CheckNumber { get; set; }
            public long? PatientPaymentChargeID { get; set; }
            public long? PaymentChargeID { get; set; }
            public long? AdjustmentCodeID { get; set; }
            public string AdjustmentCode { get; set; }
            public string LedgerBy { get; set; }
            public string LedgerType { get; set; }
            public string LedgerDescription { get; set; }
            public string LedgerDate { get; set; }
            public decimal? Amount { get; set; }
            public string AddedBy { get; set; }


        }

    }
}

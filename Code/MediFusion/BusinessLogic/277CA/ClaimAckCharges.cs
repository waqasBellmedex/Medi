using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediFusionPM.BusinessLogic._277CA
{
    public class ClaimAckCharge
    {
        // Charges

        public string CPT { get; set; }
        public string Modifier1 { get; set; }
        public string Modifier2 { get; set; }
        public string Modifier3 { get; set; }
        public string Modifier4 { get; set; }
        public string CPTDescription { get; set; }
        public decimal ChargeAmt { get; set; }
        public decimal AcceptedAmt { get; set; }

        public string ChargeCategoryCode1 { get; set; }
        public string ChargeStatusCode1 { get; set; }
        public string ChargeEntityCode1 { get; set; }
        public DateTime? ChargeStatusDate1 { get; set; }
        public string ChargeActionCode1 { get; set; }
        public decimal ChargeClaimAmt1 { get; set; }
        public string ChargeRejection1 { get; set; }

        public string ChargeCategoryCode2 { get; set; }
        public string ChargeStatusCode2 { get; set; }
        public string ChargeEntityCode2 { get; set; }
        public DateTime? ChargeStatusDate2 { get; set; }
        public string ChargeActionCode2 { get; set; }
        public decimal ChargeClaimAmt2 { get; set; }
        public string ChargeRejection2 { get; set; }

        public string ChargeCategoryCode3 { get; set; }
        public string ChargeStatusCode3 { get; set; }
        public string ChargeEntityCode3 { get; set; }
        public DateTime? ChargeStatusDate3 { get; set; }
        public string ChargeActionCode3 { get; set; }
        public decimal ChargeClaimAmt3 { get; set; }
        public string ChargeRejection3 { get; set; }

        public string ChargeCategoryCode4 { get; set; }
        public string ChargeStatusCode4 { get; set; }
        public string ChargeEntityCode4 { get; set; }
        public string ChargeStatusDate4 { get; set; }
        public string ChargeActionCode4 { get; set; }
        public decimal ChargeClaimAmt4 { get; set; }
        public string ChargeRejection4 { get; set; }

        public string ChargeCategoryCode5 { get; set; }
        public string ChargeStatusCode5 { get; set; }
        public string ChargeEntityCode5 { get; set; }
        public string ChargeStatusDate5 { get; set; }
        public string ChargeActionCode5 { get; set; }
        public decimal ChargeClaimAmt5 { get; set; }
        public string ChargeRejection5 { get; set; }

        public string ChargeControlNumber { get; set; }
        public DateTime ?ServiceDateFrom { get; set; }
        public DateTime ?ServiceDateTo { get; set; }
    }
}

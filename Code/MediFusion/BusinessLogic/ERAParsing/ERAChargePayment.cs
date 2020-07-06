using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediFusionPM.BusinessLogic.EraParsing
{
    public class ERAChargePayment
    {
        /*
        SVC*HC:99214*230*86.02**1~
        DTM*472*20150921~
        CAS*CO*45*118.6**237*1.67**253*1.76~
        CAS*PR*2*21.95~
        REF*LU*11~
        REF*6R*50305232~
        AMT*B6*109.73~
        LQ*HE*N699~
        */

        /*
        SVC*HC:98941:AT*46.29*9.07**1~
        CAS*CO*45*1.66~
        CAS*OA*23*26.63~
        CAS*PR*2*8.93~
        REF*6R*5772809~
        AMT*B6*44.63~
        */

        public ERAChargePayment()
        {
            RemitCodes = new List<ERARemitCode>();
        }

        public string CPTCode { get; set; }
        public string Modifier1 { get; set; }
        public string Modifier2 { get; set; }
        public string Modifier3 { get; set; }
        public string Modifier4 { get; set; }
        public string CPTDescription { get; set; }
        public decimal? SubmittedAmt { get; set; }
        public decimal? PaidAmt { get; set; }
        public string RevenueCode { get; set; }
        public string UnitsPaid { get; set; }
        public DateTime? ServiceDateFrom { get; set; }
        public DateTime? ServiceDateTo { get; set; }

        public decimal? DeductableAmt { get; set; }
        public decimal? CoInsuranceAmt { get; set; }
        public decimal? CopayAmt { get; set; }
        public decimal? WriteOffAmt { get; set; }

        public string OtherWriteOffCode1 { get; set; }
        public decimal? OtherWriteOffAmt1 { get; set; }
        public string OtherWriteOffCode2 { get; set; }
        public decimal? OtherWriteOffAmt2 { get; set; }
        public string OtherWriteOffCode3 { get; set; }
        public decimal? OtherWriteOffAmt3 { get; set; }
        public string OtherWriteOffCode4 { get; set; }
        public decimal? OtherWriteOffAmt4 { get; set; }
        public string OtherWriteOffCode5 { get; set; }
        public decimal? OtherWriteOffAmt5 { get; set; }

        public decimal? OtherAdjustmentAmt { get; set; }
        public decimal? CorrectionReversalAmt { get; set; }
        public decimal? PayerReductionAmt { get; set; }

        public string ChargeControlNumber { get; set; }
        public string LocationNumber { get; set; }
        public decimal? AllowedAmount { get; set; }

        public List<ERARemitCode> RemitCodes { get; set; }

        public string RemarkCode1 { get; set; }
        public string RemarkCode2 { get; set; }
        public string RemarkCode3 { get; set; }
        public string RemarkCode4 { get; set; }
        public string RemarkCode5 { get; set; }
        public string RemarkCode6 { get; set; }
        public string RemarkCode7 { get; set; }
        public string RemarkCode8 { get; set; }
        public string RemarkCode9 { get; set; }
        public string RemarkCode10 { get; set; }
        public long? ChargeID { get; set; }

    }


}

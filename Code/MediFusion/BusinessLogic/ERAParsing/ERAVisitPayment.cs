using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediFusionPM.BusinessLogic.EraParsing
{
    public class ERAVisitPayment
    {
        public ERAVisitPayment()
        {
            RemitCodes = new List<ERARemitCode>();
        }

        public string PatientControlNumber { get; set; }
        public string ClaimProcessedAs { get; set; }
        public decimal SubmittedAmt { get; set; }
        public decimal PaidAmt { get; set; }
        public decimal? PatResponsibilityAmt { get; set; }
        public decimal PayerTypeValue { get; set; }
        public string PayerControlNumber { get; set; }
        public string FacilityCode { get; set; }
        public string ClaimFrequencyCode { get; set; }
        //CAS
        public string PatientLastName { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientMiddleInitial { get; set; }
        public string SubscriberID { get; set; }
        public string SubscriberHIC { get; set; }
        public string SubscriberLastName { get; set; }
        public string SubscirberFirstName { get; set; }
        public string SubscriberMI { get; set; }
        //Review Later: NM1-74  CORRECTED PATIENT/INSURED INFORMATION

        public string RendPrvEntity { get; set; }
        public string RendPrvLastName { get; set; }
        public string RendPrvFirstName { get; set; }
        public string RendPrvMI { get; set; }
        public string RendPrvNPI { get; set; }

        public string CrossOverPayerName { get; set; }
        public string CrossOverPayerID { get; set; }
        //Review Later: NM1-PR  CORRECTED PRIORITY PAYER NAMEs

        //MIA
        //MOA

        public string SubscriberGroupNum { get; set; }
        public string SubscriberSSN { get; set; }
        public DateTime? ClaimReceivedDate { get; set; }
        public DateTime? ClaimStatementFrom { get; set; }
        public DateTime? ClaimStatementTo { get; set; }
        public string ClaimContactNumber { get; set; }
        public string ClaimTelephone { get; set; }
        public decimal ClaimCoverageAmt { get; set; }
        public decimal ClaimDiscountAmt { get; set; }


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

        public List<ERARemitCode> RemitCodes { get; set; }
        public List<ERAChargePayment> ERAChargePayments { get; set; }
    }
}

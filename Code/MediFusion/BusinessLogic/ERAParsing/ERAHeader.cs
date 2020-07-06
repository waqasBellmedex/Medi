using System;
using System.Collections.Generic;


namespace MediFusionPM.BusinessLogic.EraParsing
{
    /// <summary>
    /// This class contains ISA, GS, ST, Check Details, Payee and Payer Elements.
    /// </summary>
    public class ERAHeader
    {
        public string ISA06SenderID { get; set; }
        public string ISA08ReceiverID { get; set; }
        public DateTime ?ISADateTime { get; set; }
        public string ISAControlNumber { get; set; }
        public string GS02SenderID { get; set; }
        public string GS03ReceiverID { get; set; }
        public string GSControlNumber { get; set; }
        public string STControlNumber { get; set; }
        public string VersionNumber { get; set; }
        public string TransactionCode { get; set; }
        public decimal CheckAmount { get; set; }
        public string CreditDebitFlag { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentFormat { get; set; }
        public DateTime ?CheckDate { get; set; }
        public string CheckNumber { get; set; }
        public string PayerTrn { get; set; }
        public DateTime ?ProductionDate { get; set; }

        public string PayerName { get; set; }
        public string PayerID { get; set; }
        public string PayerAddress { get; set; }
        public string PayerCity { get; set; }
        public string PayerState { get; set; }
        public string PayerZip { get; set; }
        public string REF2U { get; set; }
        public string REFEO { get; set; }
        public string PayerContactName { get; set; }
        public string PayerTelephone { get; set; }
        public string PayerBillingContactName { get; set; }
        public string PayerBillingEmail { get; set; }
        public string PayerBillingTelephone { get; set; }
        public string PayerWebsite { get; set; }

        public string PayeeName { get; set; }
        public string PayeeNPI { get; set; }
        public string PayeeAddress { get; set; }
        public string PayeeCity { get; set; }
        public string PayeeState { get; set; }
        public string PayeeZip { get; set; }
        public string PayeeTaxID { get; set; }

        public List<ERAVisitPayment> ERAVisitPayments { get; set; }
        public List<ERAProviderAdjustment> ERAProviderAdjustments { get; set; }


        
    }

}

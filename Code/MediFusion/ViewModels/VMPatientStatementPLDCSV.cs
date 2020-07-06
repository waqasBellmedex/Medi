using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ViewModels
{
    public class VMPatientStatementPLDCSV
    {
        public string StatementNo { get; set; }
        public string BillingStatement { get; set; }
        public string OfficeName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string StatementDate { get; set; }
        public string PatientName { get; set; }
        public string AccountNo { get; set; }
        public string PatientAddress1 { get; set; }
        public string PatientAddress2 { get; set; }
        public string BillToName { get; set; }
        public string BillToAddress1 { get; set; }
        public string BillToAddress2 { get; set; }
        public string VisitID { get; set; }
        public string DateOfService { get; set; }
        public string CPT { get; set; }
        public string Procedure { get; set; }
        public string Quantity { get; set; }
        public string Charge { get; set; }
        public string InsurancePayment { get; set; }
        public string PatientPayment { get; set; }
        public string Adjustment { get; set; }
        public string CurrentDue { get; set; }
        public string Over30Days { get; set; }
        public string Over60Days { get; set; }
        public string Over90Days { get; set; }
        public string Balance { get; set; }
        public string TotalBalance { get; set; }
        public string Reference { get; set; }
        public string emailAddress { get; set; }
        public string Comments { get; set; }
        public string Attending { get; set; }
        public string ProviderName { get; set; } 
        public string LastPaymentDate { get; set; }
    }
}

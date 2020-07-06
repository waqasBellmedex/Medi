using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ReportViewModels
{
    public class RVMFirstAgingReport
    {
        public class GRAgingDetailReport
        {
            public string PayerName { get; set; }
            public string DOB { get; set; }
            public string SubscriberID { get; set; }
            public string PayerType { get; set; }
            public string AccountNum { get; set; }
            public long VisitNO { get; set; }
            public long ChargeNO { get; set; }
            public string PatientName { get; set; }
            public string DOS { get; set; }
            public string CptCode { get; set; }
            public decimal? Current { get; set; }
            public decimal? IsBetween30And60 { get; set; }
            public decimal? IsBetween61And90 { get; set; }
            public decimal? IsBetween91And120 { get; set; }
            public decimal? IsGreaterThan120 { get; set; }
            public long AgingDays { get; set; }
            public string Status { get; set; }
            public decimal? TotalBalance { get; set; }
            public string EntryDate { get; set; }
            public string lastSubDate { get; set; }
            public string Provider { get; set; }
            public string Location { get; set; }
            public string ReferringProvider { get; set; }
            public string AgingPayer { get; set; }
            public decimal? InsurancePayment { get; set; }
            public decimal? SecInsurancePayment { get; set; }
            public decimal? PatientPayment { get; set; }
            public decimal? Balance { get; set; }
            public decimal? PatientBalance { get; set; }
            public string dx1 { get; set; }
            public string dx2 { get; set; }
            public string dx3 { get; set; }
            public string dx4 { get; set; }

        }



        public class GRAgingReport
        {
            public string PayerName { get; set; }
            public decimal? Current { get; set; }
            public decimal? IsBetween30And60 { get; set; }
            public decimal? IsBetween61And90 { get; set; }
            public decimal? IsBetween91And120 { get; set; }
            public decimal? IsGreaterThan120 { get; set; }
            public decimal? TotalBalance { get; set; }
        }
        public class AgingReportTemplate1
        {
            public int claimAge { get; set; }
            public long VisitNo { get; set; }
            public string DOS { get; set; }
            public string PatientName { get; set; }
            public string PlanName { get; set; }
            public List<int> claimAges { get; set; }
            public List<decimal> charges { get; set; }

        }


        public class AgingReportTemplate2
        {
            public int claimAge { get; set; }
            public string PlanName { get; set; }
            public List<int> claimAges { get; set; }
            public decimal charges { get; set; }
            public string PatientName { get; set; }
            public string DOS { get; set; }
            public string Cpt { get; set; }
            public long VisitNo { get; set; }
            public long ChargeNo { get; set; }

        }
        public class CRAgingReport
        {
            public DateTime? DateOfServiceTo { get; set; }
            public DateTime? DateOfServiceFrom { get; set; }
            public DateTime? EntryDateTo { get; set; }
            public DateTime? EntryDateFrom { get; set; }
            public DateTime? SubmittedDateTo { get; set; }
            public DateTime? SubmittedDateFrom { get; set; }
            //this will prolly be changed to account number or name or w/e related to patient
            public string PatientName { get; set; }
            public string PatientAccountNumber { get; set; }
            public string ProviderName { get; set; }
            public string PracticeName { get; set; }
            public string PayerName { get; set; }
            public string Plan { get; set; }
            public string Location { get; set; }
            public  bool VisitValue { get; set; }
            public bool ChargeValue { get; set; }
            public bool AllValue { get; set; }
            public string SearchType { get; set; }
        }
    }
}

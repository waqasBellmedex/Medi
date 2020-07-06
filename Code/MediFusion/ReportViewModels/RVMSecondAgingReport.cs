using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ReportViewModels
{
    public class RVMSecondAgingReport
    {
        public class GRAgingReport
        {
            public string PayerName { get; set; }
            public string PatientName { get; set; }
            public string AccountNumber { get; set; }
            public long? VisitNumber { get; set; }
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
            public string PayerName { get; set; }
            public List<int> claimAges { get; set; }
            public List<long> charges { get; set; }
            public string PatientName { get; set; }
            public string AccountNumber { get; set; }
            public long? VisitNumber { get; set; }
        }

        public class CRAgingReport
        {
            public DateTime? DateOfServiceTo { get; set; }
            public DateTime? DateOfServiceFrom { get; set; }
            public DateTime? EntryDateTo { get; set; }
            public DateTime? EntryDateFrom { get; set; }
            public DateTime? SubmittedDateTo { get; set; }
            public DateTime? SubmittedDateFrom { get; set; }
            public string PatientName { get; set; }
            public string PatientAccountNumber { get; set; }
            public string ProviderName { get; set; }
            public string PracticeName { get; set; }
            public string PayerName { get; set; }
            public string Location { get; set; }
        }
    }
}

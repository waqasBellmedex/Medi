using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ReportViewModels
{
    public class RVMDetailedCollectionReport
    {
        public class GRDetailedCollectionReport
        {
            public string PayerName { get; set; }
            public string PatientAccountNo { get; set; }
            public long? VisitNo { get; set; }
            public long ChargeId { get; set; }

            public string PatientName { get; set; }
            public string DOS { get; set; }
            public string CPT { get; set; }
            public string CPTUnits { get; set; }
            public string Modifier1 { get; set; }
            public string Modifier2 { get; set; }
            public decimal? ChargeAmount { get; set; }
            public decimal? AllowedAmount { get; set; }
            public decimal? PaidAmount { get; set; }
            public decimal? AdjustmentAmount { get; set; }
            public string CheckNumber { get; set; }
            public string CheckDate { get; set; }
            public string PaymentEntryDate { get; set; }
            public decimal? CheckAmount { get; set; }
            public decimal? AppliedPayments { get; set; }
            public string Provider { get; set; }
            public string Location { get; set; }
            public string ReferringProvider { get; set; }
            public long PatientId { get; set; }

            public long PaymentCheckTempId { get; set; }
            public string dx1 { get; set; }
            public string dx2 { get; set; }
            public string dx3 { get; set; }
            public string dx4 { get; set; }


        }


        public class CRDetailedCollectionReport
        {
            public DateTime? CheckDateTo { get; set; }
            public DateTime? CheckDateFrom { get; set; }
            public DateTime? PostedDateTo { get; set; }
            public DateTime? PostedDateFrom { get; set; }
            public DateTime? DOSDateFrom { get; set; }
            public DateTime? DOSDateTo { get; set; }
            public long? LocationID { get; set; }
            public long? ProviderID { get; set; }
            public string CheckNo { get; set; }
            public string UserPosted { get; set; }
            public long? RefProviderID { get; set; }
            public string CollectionType { get; set; }


        }
    }
}

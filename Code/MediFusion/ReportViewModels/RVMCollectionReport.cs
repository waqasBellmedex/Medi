using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ReportViewModels
{
    public class RVMCollectionReport
    {
        public class GRCollectionReport
        {
            public long SrNo { get; set; }
            public string PostingUserName { get; set; }
            public string PayerName { get; set; }
            public string PracticeName { get; set; }
            public string CheckNumber { get; set; }
            public string CheckDate { get; set; }
            public string PostingDate { get; set; }
            public decimal? CheckAmount { get; set; }
            public decimal? AppliedAmount { get; set; }
            public decimal? UnAppliedAmount { get; set; }
        }

        public class CRCollectionReport
        {
            public string ReportType { get; set; }
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

            //public DateTime? PostedDateTo { get; set; }
            //public DateTime? PostedDateFrom { get; set; }
            //public string UserPosted { get; set; }
            //public string CheckNumber { get; set; }
            //public string ProviderName { get; set; }
            //public string PayerName { get; set; }
            //public string Location { get; set; }
            //public string PracticeName { get; set; }
            //public DateTime? CheckDate { get; set; }
        }

    }
}

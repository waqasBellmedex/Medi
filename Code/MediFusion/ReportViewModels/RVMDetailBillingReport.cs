using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ReportViewModels
{
    public class RVMDetailBillingReport
    {

        public class CRDetailBillingReport
        {
            public long? ProviderID { get; set; }
            public DateTime? DateOfServiceFrom { get; set; }
            public DateTime? DateOfServiceTo { get; set; }
            public DateTime? EntryDateFrom { get; set; }
            public DateTime? EntryDateTo { get; set; }
            public DateTime? SubmittedDateTo { get; set; }
            public DateTime? SubmittedDateFrom { get; set; }
            public string PatientName { get; set; }
            public string CPTCode { get; set; }
        }


        public class GRDetailBillingReport
        {
            public string PrescribingMD { get; set; }
            public decimal? Charges { get; set; }
            public decimal? SumOfCollectedRevenue { get; set; }
            public decimal? AverageRevenue { get; set; }
        

        }


    }
}

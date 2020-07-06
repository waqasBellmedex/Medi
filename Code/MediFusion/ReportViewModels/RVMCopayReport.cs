using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ReportViewModels
{
    public class RVMCopayReport
    {


        public class CCopay {
            public string AccountNum { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public long? ProviderID { get; set; }
            public long? LocationID { get; set; }
            public DateTime? EntryDateFrom { get; set; }
            public DateTime? EntryDateTo { get; set; }
            public DateTime? DosFrom { get; set; }
            public DateTime? DosTo { get; set; }
            public bool? PendingCopay { get; set; }

        }

        public class GCopay
        {
            public string AccountNum { get; set; }
            public long? VisitID { get; set; }
            public long? PatientID { get; set; }
            public string PatientName { get; set; }
            public string DOS { get; set; }
            public decimal Copay { get; set; }
            public decimal CopayCollected { get; set; }
            public long? ProviderID { get; set; }
            public string Provider { get; set; }
            public long? LocationID { get; set; }
            public string Location { get; set; }

        }
    }
}

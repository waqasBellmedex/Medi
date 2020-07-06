using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ChartModel
{
    public class TopSubmissionCO
    {
        public string PayerName { get; set; }
        public long count { get; set; }
    }

    public class CTopSubmissionCO
    {
        public DateTime? DateOfServiceFrom { get; set; }
        public DateTime? DateOfServiceTo { get; set; }
        public DateTime? EntryDateFrom { get; set; }
        public DateTime? EntryDateTo { get; set; }
        public DateTime? SubmittedDateTo { get; set; }
        public DateTime? SubmittedDateFrom { get; set; }
        public string Value { get; set; }

    }
}

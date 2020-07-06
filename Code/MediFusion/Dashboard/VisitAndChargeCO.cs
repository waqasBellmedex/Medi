using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ChartModel
{
    public class VisitAndChargeCO
    {
        public long Count { get; set; }
        public string Month { get; set; }
        public decimal Charges { get; set; }
        public string Year { get; set; }
        public string YearMonth { get; set; }
        public long VisitID { get; set; }
        
    }
    public class CVisitAndChargeCO
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Value { get; set; }
    }
}

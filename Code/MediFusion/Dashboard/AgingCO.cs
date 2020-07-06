using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ChartModel
{
    public class AgingCO
    {
        public string Type { get; set; }
        public decimal Total { get; set; }
        public decimal PlanAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public string Value { get; set; }
    }
    public class CAgingCO
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Value { get; set; }
    }

    public class AgingCOSecond
    {
        public List<long> PaidAmounts { get; set; }
        public List<long> PatientAmounts { get; set; }
        public List<long> Total { get; set; }
    }

    public class AgingCOThird
    {
        public string Name { get; set; }
        public decimal? Range0_30 { get; set; }
        public decimal? Range31_60 { get; set; }
        public decimal? Range61_90 { get; set; }
        public decimal? Range91_120 { get; set; }
        public decimal? Range120plus { get; set; }
        public decimal? Total { get; set; }
    }
}
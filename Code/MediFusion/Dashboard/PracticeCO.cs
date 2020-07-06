using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ChartModel
{
    public class PracticeCO
    {
        public string Month { get; set; }
        public long Charges { get; set; }
        public long Payment { get; set; }
        public long Adjustment { get; set; }
        public long Balance { get; set; }
        public long TotalBalance { get; set; }
    }
}

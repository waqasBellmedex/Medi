using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ChartModel
{
    public class AppointmentCO
    {
        public long Cancelled { get; set; }
        public long NoShow { get; set; }
        public long Recheduled { get; set; }
        public long Seen { get; set; }
        
        
        public long Rescheduled { get; set; }
    }

    public class AppointmentReturnCO
    {
        public long Count { get; set; }
        public string Type { get; set; }
    }
    public class CAppointmentCO
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Value { get; set; }

    }
}

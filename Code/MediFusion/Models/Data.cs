using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class Data
    {
        public long? ID { get; set; }
        public string Value { get; set; }
        public string label { get; set; }
        public string Description { get; set; }
        public string Description1 { get; set; }
        public decimal? Description2 { get; set; }
        public long? POSID { get; set; }

        public int ?AnesthesiaUnits { get; set; }
        public string Category { get; set; }
    }
}

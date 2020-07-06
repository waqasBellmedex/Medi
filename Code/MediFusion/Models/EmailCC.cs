using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class EmailCC
    {
        public long ID { get; set; }
        public string CC { get; set; }
        public long emailhistoryid { get; set; }
        public string addedby { get; set; }
        public DateTime addeddate { get; set; }
    }
}

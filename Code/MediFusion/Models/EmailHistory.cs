using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class EmailHistory
    {
        public long ID { get; set; }
        public string sendfrom { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public long practiceID { get; set; }
        public string addedBy { get; set; }
        public DateTime addeddate { get; set; }
    }
}

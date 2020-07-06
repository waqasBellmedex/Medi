using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class EmailTo
    {

        public long ID { get; set; }
        public string sendto { get; set; }
        [ForeignKey("ID")]
        public long? emailhistoryid { get; set; }
        public virtual EmailHistory EmailHistory { get; set; }
        public string addedby { get; set; }
        public DateTime addeddate { get; set; }
    }
}

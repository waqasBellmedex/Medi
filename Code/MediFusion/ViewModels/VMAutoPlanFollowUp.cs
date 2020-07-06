using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ViewModels
{
    public class VMAutoPlanFollowUp
    {
        public long ID { get; set; }
        public long PracticeID { get; set; }
        public long TotalRecords { get; set; }
        public long InsertedRecords { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

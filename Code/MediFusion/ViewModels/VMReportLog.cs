using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ViewModels
{
    public class VMReportLog
    {
        public class CReportLog
        {
            public long? ReceiverID { get; set; }
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }
            public string User { get; set; }
        }

        public class GReportLog
        {

            public long ID { get; set; }
            public long? RecieverID { get; set; }
            public string    Processed { get; set; }
            public string RecieverName { get; set; }
            public string UserResolved { get; set; }
            public int FilesCount { get; set; }
            public string Date { get; set; }
            public string AddedBy { get; set; }
        }
    }
}

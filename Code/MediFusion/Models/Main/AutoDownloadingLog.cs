using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class AutoDownloadingLog
    {
        public long ID { get; set; }
        public long ReceiverID { get; set; }
        public long PracticeID { get; set; }
        public long TotalDownloaded { get; set; }
        public long Files999 { get; set; }
        public long Files277 { get; set; }
        public long Files835 { get; set; }
        public long FilesZip { get; set; }
        public long FilesInsideZip { get; set; }
        public long FilesOther { get; set; }
        public string Path { get; set; }
        public long Files999Processed { get; set; }
        public long Files277Processed { get; set; }
        public long Files835Processed { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string LogMessage { get; set; }
        public string Exception { get; set; }
        public long? SubmitterID { get; set; }
    }
}

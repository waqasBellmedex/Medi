using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ViewModel
{
    public class VMDownloadFile
    {
        public class CDownloadFiles
        {
            public string FileType { get; set; }
            public long? ID { get; set; }
        }
        public class GDownloadFile
        {
            [Required]
            public long ID { get; set; }

            public long? ReportsLogID { get; set; }

            public string FileName { get; set; }
            public string FileType { get; set; }

            public string Processed { get; set; }

            public string AddedBy { get; set; }
            public string AddedDate { get; set; }
        }
    }
}
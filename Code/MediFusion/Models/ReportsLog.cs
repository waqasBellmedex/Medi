using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{

    public class ReportsLog
    {
        [Required]
        public long ID { get; set; }
        public long ?ClientID { get; set; }
        public long ?ReceiverID { get; set; }
        public long ?SubmitterID { get; set; }
        public string ZipFilePath { get; set; }
        public bool ?Processed { get; set; }
        public bool ?UserResolved { get; set; }
        public int FilesCount { get; set; }
        public bool ManualImport { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool resolve { get; set; }
    }


}


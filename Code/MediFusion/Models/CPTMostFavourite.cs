using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class CPTMostFavourite
    {
        [Required] 
        public long ID { get; set; }
        public string Type { get; set; }
        public long? ProviderID { get; set; }
        public long CPTID { get; set; }
        public long? VisitReasonID { get; set; }
        public long? PracticeID { get; set; }
        public bool? Inactive { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

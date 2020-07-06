using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class AppointmentICD
    {
        [Required]
        public long ID { get; set; }
        [ForeignKey("ID")]
        public long AppointmentID { get; set; }
        [ForeignKey("ID")]
        public long? ICDMostFavouriteID { get; set; }
        [ForeignKey("ID")]
        public long ICDID { get; set; }
        public int? SerialNo { get; set; }
        public long? PracticeID { get; set; }
        public bool? Inactive { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class AppointmentCPT
    {

        [Required]
        public long ID { get; set; } 
        [ForeignKey("ID")]
        public long AppointmentID { get; set; }
        [ForeignKey("ID")]
        public long? CPTMostFavouriteID { get; set; } 
        [ForeignKey("ID")]
        public long CPTID { get; set; }
        public string Modifier1 { get; set; }
        public string Modifier2 { get; set; }
        public long? ChargeID { get; set; }
        public string Pointer1 { get; set; }
        public string Pointer2 { get; set; }
        public string Pointer3 { get; set; }
        public string Pointer4 { get; set; }
        public string NdcUnits { get; set; }
        public int? Units { get; set; }
        public decimal? Amount { get; set; }
        public decimal? TotalAmount { get; set; }
        public long? PracticeID { get; set; }
        public bool? Inactive { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}

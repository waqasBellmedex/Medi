using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{
    public class ProviderSchedule
    {
        
        [Required]
        public long ID { get; set; } 
        [NotMapped]
        public  bool chk { get; set; }
         
        [ForeignKey("ID")]
        public long ProviderID { get; set; }
        public virtual Provider Provider { get; set; }
        [ForeignKey("ID")]
        public long? LocationID { get; set; }
        public virtual Location Location { get; set; }
        public DateTime? FromTime { get; set; }
        public DateTime? ToTime { get; set; }
        public int TimeInterval { get; set; }
        public bool OverBookAllowed { get; set; }
        [MaxLength(500)]
        public string Notes { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string dayofWeek { get; set; }
        public DateTime? breakfrom { get; set; }
        public DateTime? breakto { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public bool? InActive { get; set; }



    }
}

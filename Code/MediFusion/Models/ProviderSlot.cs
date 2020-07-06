using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{
    public class ProviderSlot
    {
        [Required]
        public long ID { get; set; }

        [ForeignKey("ID")]
        public long ProviderScheduleID { get; set; }
        public virtual ProviderSchedule ProviderSchedule { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? FromDate { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? ToDate { get; set; }
        public DateTime? FromTime { get; set; }
        public DateTime? ToTime { get; set; }
        public int TimeInterval { get; set; }
        public string  Status { get; set; }

        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

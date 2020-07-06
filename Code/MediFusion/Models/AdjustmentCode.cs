using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{
    
        public class AdjustmentCode
        {
            [Required]
            public long ID { get; set; }
            [Required]
            [MaxLength(3)]
            public string Code { get; set; }

            [ForeignKey("ID")]
           public long? ActionID { get; set; }
           public virtual Action Action { get; set; }
           [ForeignKey("ID")]
            public long? ReasonID { get; set; }
           public virtual Reason Reason { get; set; }
           [ForeignKey("ID")]
           public long? GroupID { get; set; }
           public virtual Group Group { get; set; }

         // [MaxLength(300)]
            public string Description { get; set; }
            public string Type { get; set; }
           public string AddedBy { get; set; }

            [DataType(DataType.DateTime)]
            public DateTime AddedDate { get; set; }

            public string UpdatedBy { get; set; }
            [DataType(DataType.Date)]
            public DateTime? UpdatedDate { get; set; }
        }
    }



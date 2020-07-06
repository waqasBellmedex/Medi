using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace MediFusionPM.Models
{
    
        public class RemarkCode
        {
            [Required]
            public long ID { get; set; }
            [Required]
            [MaxLength(10)]
            public string Code { get; set; }
             
           // [MaxLength(1000)]
            public string Description { get; set; }
            public string AddedBy { get; set; }

            [DataType(DataType.DateTime)]
            public DateTime AddedDate { get; set; }

            public string UpdatedBy { get; set; }
            [DataType(DataType.Date)]
            public DateTime? UpdatedDate { get; set; }
        }
    }



using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace MediFusionPM.Models
{
    
        public class POS
        {
            [Required]
            public long ID { get; set; }
            [Required]
            [MaxLength(8)]
            public string PosCode { get; set; }
            [MaxLength(70)]
            public string Name { get; set; }  
            [MaxLength(300)]
            public string Description { get; set; }
            public string AddedBy { get; set; }

            [DataType(DataType.DateTime)]
            public DateTime AddedDate { get; set; }

            public string UpdatedBy { get; set; }
            [DataType(DataType.Date)]
            public DateTime? UpdatedDate { get; set; }
        }
    }



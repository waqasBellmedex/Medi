using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;    
namespace MediFusionPM.Models
{
 
        public class ICD
    {
            [Required]
            public long ID { get; set; }
            [Required]
            //[MaxLength(100)]
            public string Description { get; set; }
            public string ICDCode { get; set; }
            public bool ?IsValid { get; set; }
            public bool IsActive { get; set; }
            public bool IsDeleted { get; set; }
            public string AddedBy { get; set; }
            public DateTime AddedDate { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedDate { get; set; }
          

    }
}

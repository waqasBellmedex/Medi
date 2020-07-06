using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class CategoryCodes
    {
        [Required]
        public long ID { get; set; }
        [MaxLength(50)]
        public string ShortDesc { get; set; }

        [MaxLength]
        public string Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime AddedDate { get; set; }
        public string AddedBy { get; set; }
        public string UpdateBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime UpdatedDate { get; set; }


    }
}

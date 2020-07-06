using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class Taxonomy
    {
        [Required]
        public long ID { get; set; }
        [MaxLength]
        public string Speciality { get; set; }
        [MaxLength]
        public string AMADescription { get; set; }
        public string SpecialityType { get; set; }
        public string NUCCCode { get; set; }
        [MaxLength]
        public string NUCCDescription { get; set; }
        [DataType(DataType.Date)]
        public DateTime AddedDate { get; set; }
        public string AddedBy { get; set; }
        public string UpdateBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime UpdatedDate { get; set; }
    }
}

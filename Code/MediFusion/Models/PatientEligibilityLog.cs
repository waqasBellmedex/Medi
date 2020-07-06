using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{

    public class PatientEligibilityLog
    {
        [Required]
        public long ID { get; set; }
        public long PatientEligibilityID { get; set; }
        public string Transaction270Path { get; set; }
        public string Transaction271Path { get; set; }
        public string Transaction999Path { get; set; }


        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

}

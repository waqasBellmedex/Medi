using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{
    public class BatchDetail
    {
        [Required]
        public long ID { get; set; }

        public DateTime DOS { get; set; }
        public long NumberOfVisits { get; set; }

        public long NumOfVisitsEntered { get; set; }
         public decimal? Copay { get; set; }
        public decimal? CopayApplied { get; set; }
        public long AdmitTotalPatients { get; set; }
        public long AdmitTotalPatientEntered { get; set; }

        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}

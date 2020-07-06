using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class BatchDocumentCharges
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public long? BatchDocumentNoID { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? DOS { get; set; }
        public long? NoOfVisits { get; set; }
        public decimal? Copay { get; set; }
        public decimal? OtherPatientAmount { get; set; }

        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? NumberofVisitsEntered { get; set; }

    }
}

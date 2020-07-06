using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class BatchDocumentPayment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public long? BatchDocumentNoID { get; set; }

        public string CheckNo { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? CheckDate { get; set; }
        public decimal? CheckAmount { get; set; }


        public decimal? Applied { get; set; }
        public decimal? UnApplied { get; set; }
        public string Remarks { get; set; }


        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

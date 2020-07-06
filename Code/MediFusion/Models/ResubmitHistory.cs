using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MediFusionPM.Models
{
    public class ResubmitHistory
    {

        public long ID { get; set; }
        [ForeignKey("ID")]
        public long? ChargeID { get; set; }
        public virtual  Charge Charge{get;set;}

        [ForeignKey("ID")]
        public long? VisitID { get; set; }
        public virtual Visit Visit { get; set;}

        public string AddedBy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime AddedDate { get; set; }

        public string UpdatedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDate { get; set; }

    }
}

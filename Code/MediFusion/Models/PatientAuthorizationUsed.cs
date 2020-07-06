using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    
        public class PatientAuthorizationUsed
        {

            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public long ID { get; set; }
            [ForeignKey("ID")]
            public long PatientAuthID { get; set; }
            public virtual PatientAuthorization PatientAuthorization { get; set; }

            [ForeignKey("ID")]
            public long? VisitID { get; set; }
            public virtual Visit Visit { get; set; }
            public string AddedBy { get; set; }
            [DataType(DataType.DateTime)]
            public DateTime? AddedDate { get; set; }
            public string UpdatedBy { get; set; }
            [DataType(DataType.DateTime)]
            public DateTime? UpdatedDate { get; set; }
          


    }
}

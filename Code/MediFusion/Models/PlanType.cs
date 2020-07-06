using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MediFusionPM.Models
{

    namespace TodoApi.Models
    {
        public class PlanType
        {
            public long  ID { get; set; }
            public String Code { get; set; }
            public String Description { get; set; }
            public string AddedBy { get; set; }
            [DataType(DataType.DateTime)]
            public DateTime? AddedDate { get; set; }
            public string UpdatedBy { get; set; }
            [DataType(DataType.DateTime)]
            public DateTime? UpdatedDate { get; set; }


        }





    }
}

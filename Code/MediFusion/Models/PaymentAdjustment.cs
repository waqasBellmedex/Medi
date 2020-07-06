using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{
    namespace TodoApi.Models
    {
        public class PaymentAdjustment
        {
            public string ID { get; set; }
            public long PaymentVisitID { get; set; }
            public long PaymentCheckID { get; set; }

            public string GroupCode { get; set; }
            public string ReasonCode { get; set; }
            public string ReasonAmount { get; set; }
            public int Quantity { get; set; }


            public string AddedBy { get; set; }
            public DateTime AddedDate { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime? UpdatedDate { get; set; }
        }


    }
}

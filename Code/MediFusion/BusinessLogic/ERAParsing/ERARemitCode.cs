using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediFusionPM.BusinessLogic.EraParsing
{
    public class ERARemitCode
    {
        public string GroupCode { get; set; }
        public string ReasonCode { get; set; }
        public decimal? Amount { get; set; }
        public string Quantity { get; set; }
    }
}

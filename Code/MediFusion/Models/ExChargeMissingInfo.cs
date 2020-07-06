using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class ExChargeMissingInfo
    {
        [Key]
        public long ExternalChargeID { get; set; }
        public long? ICDID { get; set; }
        public long? PrimaryInsuredID { get; set; }
        public string InsuranceName { get; set; }
    }

}

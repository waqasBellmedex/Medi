using MediFusionPM.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace MediFusionPM.Models.Main
{
    public class ExInsuranceMapping
    {
        public long ID { get; set; }
        public string ExternalInsuranceName { get; set; }
        public long? InsurancePlanID { get; set; }
        [MaxLength(200)]
        public string PlanName { get; set; }
        public string Status { get; set; }
        public string AddedBy { get; set; }
       [DataType(DataType.DateTime)]
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDate { get; set; }
        public long? PracticeID { get; set; }

    }
}

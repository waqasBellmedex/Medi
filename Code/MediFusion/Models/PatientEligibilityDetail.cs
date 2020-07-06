using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediFusionPM.Models
{

    public class PatientEligibilityDetail
    {
        [Required]
        public long ID { get; set; }
        public long PatientEligibilityID { get; set; }

        public string Coverage { get; set; }
        public string CoverageLevel { get; set; }
        public string ServiceTypes { get; set; }
        public string PlanName { get; set; }
        public string PlanDescription { get; set; }
        public string TimePeriod { get; set; }
        public decimal? BenefitAmount { get; set; }
        public string BenefitPercentage { get; set; }
        public string Authorization { get; set; }
        public string PlanNetwork { get; set; }
        public string Messages { get; set; }

        public string ReferenceId1 { get; set; }
        public string ReferenceValue1 { get; set; }
        public string ReferenceId2 { get; set; }
        public string ReferenceValue2 { get; set; }
        public string ReferenceId3 { get; set; }
        public string ReferenceValue3 { get; set; }
        public string ReferenceId4 { get; set; }
        public string ReferenceValue4 { get; set; }
        public string ReferenceId5 { get; set; }
        public string ReferenceValue5 { get; set; }

        public string DateId1 { get; set; }
        public string DateValue1 { get; set; }
        public string DateId2 { get; set; }
        public string DateValue2 { get; set; }
        public string DateId3 { get; set; }
        public string DateValue3 { get; set; }
        public string DateId4 { get; set; }
        public string DateValue4 { get; set; }
        public string DateId5 { get; set; }
        public string DateValue5 { get; set; }


        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}

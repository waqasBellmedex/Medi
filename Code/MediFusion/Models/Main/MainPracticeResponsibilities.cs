using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.ViewModel;
using MediFusionPM.ViewModels.Main;
namespace MediFusionPM.Models.Main
{
    public class MainPracticeResponsibilities
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public long? MainPracticeID { get; set; }
        public bool? IsBillingCompanyResponsibility { get; set; }
        public bool? IsClientCompanyResponsibility { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ReportViewModels
{
    public class RVMAnesthesiaUnit
    {
        public class GRAnesthesiaUnit
        {
            //public long? PatientID { get; set; }
            public string PatientAccountNumber { get; set; }
            public string PatientName { get; set; }
            public string ProviderName { get; set; }
            public string ClaimCreatedDate { get; set; }
            public string InsuranceName { get; set; }
            public string DateOfService { get; set; }
            public string Pos { get; set; }
            public string Cpt { get; set; }
            public string Description { get; set; }
            public string ShortCptDescription { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }
            public string TotalTime { get; set; }
            public string TotalMin { get; set; }
            public int? TimeUnits { get; set; }
            public int? BaseUnits { get; set; }
            public int? ModifierUnits { get; set; }
            public string TotalUnits { get; set; }
            public string Charges { get; set; }
            public string MOD1 { get; set; }
            public string MOD2 { get; set; }
            public string ChargeAmount { get; set; }

        }
        public class CRAnesthesiaUnit
        {
            //public long? PatientID { get; set; }
            public string PatientAccountNumber { get; set; }
            public string ProviderName { get; set; }
            public DateTime? DateOfServiceFrom { get; set; }
            public DateTime? DateOfServiceTo { get; set; }
            public DateTime? EntryDateFrom { get; set; }
            public DateTime? EntryDateTo { get; set; }
            public DateTime? SubmittedDateTo { get; set; }
            public DateTime? SubmittedDateFrom { get; set; }
            public bool IncludePhysicalStatus { get; set; }

        }
    }
}

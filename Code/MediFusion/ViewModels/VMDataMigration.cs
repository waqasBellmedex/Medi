using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ViewModels
{
    public class VMDataMigration
    {
        public string Type { get; set; }
        public FileUploadViewModel UploadModel { get; set; }
        public long ProviderID { get; set; }
        public long PracticeID { get; set; }
        public long LocationID { get; set; }
    }

    public class CExteralCharges
    {
      public long ? ExternalPatientID { get; set; }
      public string AccountNum { get; set; }
      public string Status { get; set; }
      public string ErrorMessage { get; set; }
      public string PaymentProcessed { get; set; }
      public string ResolvedErrorMessage { get; set; }
      public DateTime? DOSTO { get; set; }
      public DateTime? DOSFROM { get; set; }
      public string FileName { get; set; }
      public DateTime? EntryDateFrom { get; set; }
      public DateTime? EntryDateTo { get; set; }
        public string RecordType { get; set; }

    }

    public class GExteralCharges
    {
        public long ID { get; set; }
	    public string FileName { get; set; }
        public string EntryDate { get; set; }
        public string ExternalPatientName { get; set; }
        public string ExternalPatientID { get; set; }
        public string AccountNum { get; set; }
        public string Provider { get; set; }
        public string InsuranceName { get; set; }
        public long? VisitID { get; set; }
        public string DOS { get; set; }
        public string CPT { get; set; }
        public string ICD { get; set; }
        public long? Modifiers { get; set; }
        public string POS { get; set; }
        public decimal? Charges { get; set; }
        public decimal? InsurancePayment { get; set; }
        public decimal? PatientPayment { get; set; }
        public decimal? Adjustments { get; set; }
        public long? ProviderID { get; set; }
        public string PaymentProcessed { get; set; }
        public string ErrorMessage { get; set; }

        public decimal? BilledAmount { get; set; }
        public long? PatientID { get; set; }
		 public string PrescribingMD { get; set; }
        public long ExternalChargeID { get; internal set; }
        public string RecordType { get; set; }
        public string Status { get; set; }
    }
}

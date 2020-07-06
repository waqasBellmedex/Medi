using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ViewModels
{
    public class VMExternalPatient
    {
        public class GExternalPatient
        {
            public long ID { get; set; }
            public string EntryDate { get; set; }
            public string FileName { get; set; }
            public string ExternalPatientID { get; set; }
            public string AccountNumber { get; set; }
            public string Status { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Dob { get; set; }
            public string PrimarySubscriberID { get; set; }
            public string PrimaryPayer { get; set; }
            public string SecondarySubscriberID { get; set; }
            public string SecondaryPayer { get; set; }
            public string ProviderName { get; set; }
            public long ProviderID { get; set; }   
            public long LocationID { get; set; }
            public string ErrorMessage { get; set; }
            public long PatientID { get; set; }


            public string LocationName { get; set; }
            public long? PrimaryPatientPlanID { get; set; }
            public long? SecondaryPatientPlanID { get; set; }
     
       
        


        }

        public class CExternalPatient
        {
            public string ExternalPatientID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string AccountNumber { get; set; }
            public string Status { get; set; }
            public string FileName { get; set; }
            public DateTime? EntryDateFrom { get; set; }
            public DateTime? EntryDateTo { get; set; }
            public string ResolvedErrorMessage { get; set; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;

namespace MediFusionPM.ViewModels
{
    public class VMPaperSubmission
    {
        public List<DropDown> Receivers { get; set; }

        public void GetProfiles(ClientDbContext pMContext)
        {
            Receivers = (from f in pMContext.Receiver
                         select new DropDown()
                         {
                             ID = f.ID,
                             Description = f.Name
                         }).ToList();
        }


        public class HcfaSubmitModel
        {
            public string FormType { get; set; }
            
            //public long ClientID { get; set; }
            public List<long> Visits { get; set; }

            public List<DropDown> ErrorVisits { get; set; }

            public long? ProcessedClaims { get; set; }
            public bool IsFileSubmitted { get; set; }
            public string FilePath { get; set; }

            public string ErrorMessage { get; set; }
            public string SessionId { get; set; }
            public long ?SubmissionLogID { get; set; }
         

        }

        public class CPaperSubmission
        {
            public string FormType { get; set; }
            public string AccountNum { get; set; }
            public long? VisitID { get; set; }
            public string Practice { get; set; }
            public string Provider { get; set; }
            public string PlanName { get; set; }
            public string PayerID { get; set; }
            public long? LocationID { get; set; }
            public string Location { get; set; }
            public DateTime? EntryDateTo { get; set; }
            public DateTime? EntryDateFrom { get; set; }
            public string InsuranceType { get; set; }

        }


        public class GPaperSubmission
        {
            // We are hiding first column in Grid.
            // So Adding ID column extra, so that VisitID could not be hidden.
            public long ID { get; set; }
            public long VisitID { get; set; }
            public string DOS { get; set; }
            public string AccountNum { get; set; }
            public long PatientID { get; set; }
            public string Patient { get; set; }
            public string PlanName { get; set; }
            public string InsurancePlanID { get; set; }
            public long PracticeID { get; set; }
            public string Practice { get; set; }
            public long LocationID { get; set; }
            public string Location { get; set; }
            public long ProviderID { get; set; }
            public string Provider { get; set; }
            public decimal? TotalAmount { get; set; }
            public string ValidationMessage { get; set; }
            public string SubscriberID { get; set; }
            public string PrimaryPatientPlanID { get; set; }
            public string VisitEntryDate { get; set; }
           //For Testing
            public string PrimaryStatus { get; set; }
            public string SecondaryStatus { get; set; }
        }





    }
}

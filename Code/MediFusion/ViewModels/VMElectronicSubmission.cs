using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;

namespace MediFusionPM.ViewModels
{
    public class VMElectronicSubmission
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
            Receivers.Insert(0, new DropDown() { ID = null, Description = "Please Select" });
        }

        public class SubmitModel
        {
            public long ReceiverID { get; set; }
           // public long ClientID { get; set; }
            public List<long> Visits { get; set; }

            public List<DropDown> ErrorVisits { get; set; }

            public long ProcessedClaims { get; set; }
            public bool IsFileSubmitted { get; set; }
            public string FilePath { get; set; }
            public long ?SubmissionLogID { get; set; }
            public string ErrorMessage { get; set; }
            public string SessionId { get; set; }
        }

        public class CElectronicSubmission
        {
            public long? ReceiverID { get; set; }
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

        public class GElectronicSubmission
        {
            // We are hiding first column in Grid.
            // So Adding ID column extra, so that VisitID could not be hidden.
            public long ID { get; set; }
            public long VisitID { get; set; }
            public string DOS { get; set; }
            public string AccountNum { get; set; }
            public long PatientID { get; set; }
            public string Patient { get; set; }
            public long PracticeID { get; set; }
            public string PlanName { get; set; }
            public string Coverage { get; set; }
            public string PrimaryStatus { get; set; }
           
            public string InsurancePlanID { get; set; }
            public string Practice { get; set; }
            public long LocationID { get; set; }
            public string Location { get; set; }
            public long ProviderID { get; set; }
            public string Provider { get; set; }
            public decimal ?TotalAmount { get; set; }
            public string ValidationMessage { get; set; }
            public bool IsSubmitted { get; set; }
            public string subscriberID { get; set; }
            public string VisitEntryDate { get; set; }
           

            //Secondary Columns
            public string SecondaryStatus { get; set; }
            //public decimal? SecondaryPlanBalance { get; set; }
            //public decimal? SecondaryPatientBalance { get; set; }
          

        }



    }
}

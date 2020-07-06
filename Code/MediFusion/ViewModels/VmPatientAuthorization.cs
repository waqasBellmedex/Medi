using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ViewModels
{
    public class VmPatientAuthorization
    {
        public class CAuthorizationNumber
        {
            public long? CptId { get; set; }
            public long? ProviderId { get; set; }
            public long? PatientId { get; set; }
            public long? PatientPlanId { get; set; }
            public DateTime? DateOfServiceFrom { get; set; }
            public DateTime? DateOfServiceTo { get; set; }

        }
        public class GExpiringAuthorizations
        {

            public long PatiendID { get; set; }
            public long PatientAuthId { get; set; }
            public string AccountNo { get; set; }
            public string AuthorizationNo { get; set; }
            public string VisitsAllowed { get; set; }
            public string VisitsUsed { get; set; }
            public string VisitsRemaining { get; set; }
            public string StartDate { get; set; }
            public string ExpiryDate { get; set; }
        }


        public class CPatientAuthorization
        {
            //NewAdded Fields
            public long? ProviderID { get; set; }
            public long? LocationID { get; set; }
            public DateTime? EntryDateFrom { get; set; }
            public DateTime? EntryDateTo { get; set; }
            public DateTime? AuthorizationDateFrom { get; set; }
            public DateTime? AuthorizationDateTo { get; set; }
            public string Status { get; set; }
            //Existing Fields
            public string AccountNo { get; set; }
            public string AuthorizationNo { get; set; }
            public string CPTCode { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? ExpiryDate { get; set; }
            public string ResponsibleParty { get; set; }
        }
        public class GPatientAuthorization
        {
            public long ID { get; set; }
            public string EntryDate { get; set; }
            public string AuthorizationDate { get; set; }
            public string Status { get; set; }
            public string AccountNo { get; set; }
            public string PatientName { get; set; }
            public string Plan { get; set; }
            public long? ProviderID { get; set; }
            public string ProviderName { get; set; }
            public string AuthorizationNo { get; set; }
            public string CPTCode { get; set; }
            public string StartDate { get; set; }
            public string ExpiryDate { get; set; }
            public long? VisitsAllowed { get; set; }
            public long? VisitsUsed { get; set; }
            public long? PatientID { get; set; }

            public string ResponsibleParty { get; set; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class PatientInfoDropDown
    {
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public string AccountNumber { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public long? PracticeID { get; set; }
        public string PracticeName { get; set; }
        public long? LocationID { get; set; }
        public string LocationName { get; set; }
        public long? POSID { get; set; }
        public string POSName { get; set; }
        public long? providerID { get; set; }
        public string ProviderName { get; set; }
        public long? RefProviderID { get; set; }
        public string RefProviderName { get; set; }
        public long? SupProviderID { get; set; }
        public string SupProviderName { get; set; }
        public long? ReasonID { get; set; }
        public string Reason { get; set; }
        public long? ActionID { get; set; }
        public string Action { get; set; }
        public long? GroupID { get; set; }
        public string Group { get; set; }

        public string PlanName { get; set; }
        public string InsuredName { get; set; }
        public long? InsuredID { get; set; }
        public string PhoneNumber { get; set; }
        public string SubscriberID { get; set; }
        public string Coverage { get; set; }
        public string Value { get; set; }
        public string label { get; set; }
        public string SubscriberName { get; set; }
    

    }
}

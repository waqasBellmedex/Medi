using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ReportViewModels
{
    public class RVMPatient
    {
        public class CRPatientAppointment
        {
            public long? PatientID { get; set; }
            public long? ProviderID { get; set; }
            public long? LocationID { get; set; }
            public DateTime? AppoinmentDateFrom { get; set; }
            public DateTime? AppoinmentDateTo { get; set; }
        }
        public class GRPatientAppointment
        {
            public long? Scheduled { get; set; }
            public long? Seen { get; set; }
            public long? NoShow { get; set; }
            public long? Cancelled { get; set; }
            public long? Rescheduled { get; set; }
        }
        //*************************************

        public class CRPatientPending
        {
            public long? PatientID { get; set; }
            public long? ProviderID { get; set; }
            public long? LocationID { get; set; }
            public DateTime? AppoinmentDateFrom { get; set; }
            public DateTime? AppoinmentDateTo { get; set; }
        }
        public class GRPatientPending
        {
            public DateTime? AppoinmentDate { get; set; }

            public string ProviderName { get; set; }
            public string Location { get; set; }
            public string Patient { get; set; }
            public string AccountNo { get; set; }
            public DateTime? DOB { get; set; }
        }
        //********************************************



        public class CRPatientReferralPhysician
        {
            public long? RefProviderID { get; set; }
            public long? PatientID { get; set; }
            public long? LocationID { get; set; }
            public DateTime? DOSDateFrom { get; set; }
            public DateTime? DOSDateTo { get; set; }
            public DateTime? EntryDateFrom { get; set; }
            public DateTime? EntryDateTo { get; set; }
            public long? CptID { get; set; }
        }
        public class GRPatientReferralPhysician
        {
            public string DOS { get; set; }
            public long? PatientID { get; set; }
            public string RefProvider { get; set; }
            public long VisitId { get; set; }
            public string AccountNo { get; set; }
            public string PatientName { get; set; }
            public string DOB { get; set; }
            public string PhoneNo { get; set; }
        }






    }
}

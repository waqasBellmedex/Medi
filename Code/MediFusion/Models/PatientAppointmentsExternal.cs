using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class PatientAppointmentsExternal
    {

        public long ID { get; set; }
        public string dataReceived { get; set; }
        public DateTime? addedDate { get; set; }        
        public long? PatientAppointmentID { get; set; }       
        
        public string addedBy { get; set; }
        public DateTime? updatedDate { get; set; }
        public string updatedBy { get; set; }
        public DateTime? appointmentDate { get; set; }
        public DateTime? appointmentTime { get; set; }
        public int? interval { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string insurancePlanName { get; set; }
        public string policyNumber { get; set; }
        public string proivder { get; set; }
        public string location { get; set; }

        public string emailAddress { get; set; }
        public string phoneNumber { get; set; }
        public string canlenderId { get; set; }
        public string comments { get; set; }
        public DateTime? dob { get; set; }
        public string exception { get; set; }
        public int? rowNumber { get; set; }

        public bool? isError { get; set; }
    }
}

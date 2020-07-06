using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class SMSSentReceived
    {
        public long ID { get; set; }
        public long? patientID { get; set; }
        public long? patientAppointmentID { get; set; }
        public long? clientID { get; set; }
        public long? practiceID { get; set; }
        public string textSent { get; set; }
        public string textReceived { get; set; }
        public string response { get; set; }
        public string sentFromNumber { get; set; }
        public string sentToNumber { get; set; }
        public string receivedFromNumber { get; set; }
        public string receivedToNumber { get; set; }
        public string sentBy { get; set; }
        public DateTime? sentDate { get; set; }
        public string ReceivedBy { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string sentMessageApiReply { get; set; }
        public string apiId { get; set; }
        public string messageUuid { get; set; }
         
    }
}

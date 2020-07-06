using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediFusionPM.BusinessLogic.EligParser
{
    public class _271Header
    {
        public _271Header()
        {
            ListOfSubscriberData = new List<_271Subscriber>();
        }
        public string ISA06SenderID { get; set; }
        public string ISA08ReceiverID { get; set; }
        public DateTime? ISADateTime { get; set; }
        public string ISAControlNumber { get; set; }
        public string GS02SenderID { get; set; }
        public string GS03ReceiverID { get; set; }
        public string GSControlNumber { get; set; }
        public string STControlNumber { get; set; }
        public string VersionNumber { get; set; }

        // Information Source
        public string PayerOrgName { get; set; }
        public string PayerFirstName { get; set; }
        public string PayerID { get; set; }
        public string PayerAddress { get; set; }
        public string PayerCity { get; set; }
        public string PayerState { get; set; }
        public string PayerZip { get; set; }
        public string PayerContactName { get; set; }
        public string PayerTelephone { get; set; }
        public string PayerBillingContactName { get; set; }
        public string PayerBillingEmail { get; set; }
        public string PayerBillingTelephone { get; set; }
        public string PayerWebsite { get; set; }

        public List<_271Subscriber> ListOfSubscriberData { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class OnlinePortalCredentials
    {
        public long ID { get; set; }
        public long? OnlinePortalsID { get; set; }
        public string Username { get; set; }
        public string  Password { get; set; }   
        public DateTime? PasswordExpiryDate { get; set; }
        public string SercurityQ1 { get; set; }
        public string SecurityA1 { get; set; }
        public string SecurityQ2 { get; set; }
        public string SecurityA2 { get; set; }
        public string SecurityQ3 { get; set; }
        public string SecurityA3 { get; set; }
        public string Notes { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
      
    }

 
    



}

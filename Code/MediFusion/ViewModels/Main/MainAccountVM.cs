using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models.Main;

namespace MediFusionPM.ViewModels.Main
{
    public class MainAccountVM
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        public string UserRole { get; set; }
        public long? ClientID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public MainRights MainRights { get; set; }
        public long PracticeID { get; set; }
        public long? TeamID { get; set; }
        public long? DesignationID { get; set; }
        public string ReportingTo { get; set; }
        public string signatureURL { get; set; }
        public string signatureText { get; set; }
        [NotMapped]
        public string Piccontent { get; set; }
    }
}

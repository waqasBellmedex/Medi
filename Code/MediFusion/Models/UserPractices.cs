using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public class UserPractices
    {
        public string UserID { get; set; }
        public virtual ClientDbContext PMContext { get; set; }
        public long PracticeID { get; set; }
        public ICollection<Practice> Practice { get; set; }

        public string AssignedByUserId { get; set; }
        public bool Status { get; set; }
        public string UPALastModified { get; set; }

        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

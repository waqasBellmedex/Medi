using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models.Main
{
    public class MainUserPractices
    {
        public string UserID { get; set; }
        public virtual MainContext MainContext { get; set; }
        public long PracticeID { get; set; }
        public ICollection<MainPractice> MainPractice { get; set; }

        public string AssignedByUserId { get; set; }
        public bool Status { get; set; }
        public string UPALastModified { get; set; }

        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

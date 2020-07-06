using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediFusionPM.BusinessLogic.ClaimGeneration
{
    /// <summary>
    /// This class contains ISA, GS, ST, Submitter and Receiver Elements.
    /// </summary>
    public class ClaimHeader
    {
        public ClaimHeader()
        {
            this.ISA01AuthQual = "00";
            this.ISA03SecQual = "00";
            this.ISA05SenderQual = "ZZ";
            this.ISA07ReceiverQual = "ZZ";
            this.ISA15UsageIndi = "P";
            this.ISA02AuthInfo = "".PadLeft(10, ' ');
            this.ISA04SecInfo = "".PadLeft(10, ' ');
            this.SubmitterEntity = "2";
            this.SubmitterQual = "46";
            this.RecieverQual = "46";
        }


        public string ISA01AuthQual { get; set; }
        public string ISA02AuthInfo { get; set; }
        public string ISA03SecQual { get; set; }
        public string ISA04SecInfo { get; set; }
        public string ISA05SenderQual { get; set; }
        public string ISA06SenderID { get; set; }
        public string ISA07ReceiverQual { get; set; }
        public string ISA08ReceiverID { get; set; }
        public string ISA13CntrlNumber { get; set; }
        public string ISA15UsageIndi { get; set; }

        public string GS02SenderID { get; set; }
        public string GS03ReceiverID { get; set; }

        public string SubmitterEntity { get; set; }
        public string SubmitterOrgName { get; set; }
        public string SubmitterFirstName { get; set; }
        public string SubmitterQual { get; set; }
        public string SubmitterID { get; set; }

        public string SubmitterContactName { get; set; }
        public string SubmitterTelephone { get; set; }
        public string SubmitterFax { get; set; }
        public string SubmitterEmail { get; set; }

        public string ReceiverOrgName { get; set; }
        public string RecieverQual { get; set; }
        public string ReceiverID { get; set; }
        
        public List<ClaimData> Claims { get; set; }
        public bool RelaxNpiValidation { get; set; }

        public SFTPModel SFTPModel { get; set; }

       

    }

}

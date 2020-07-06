using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediFusionPM.BusinessLogic.EligGenerator
{
    /// <summary>
    /// This class contains ISA, GS, ST, Submitter and Receiver Elements.
    /// </summary>
    public class _270Header
    {
        public _270Header()
        {
            this.ISA01AuthQual = "00";
            this.ISA03SecQual = "00";
            this.ISA05SenderQual = "ZZ";
            this.ISA07ReceiverQual = "ZZ";
            //this.ISA11RepetitionSep = "^";
            //this.ISA12Version = "00501";
            //this.ISA14AckRequested = "1";
            this.ISA15UsageIndi = "P";
            //this.ISA16CompoEleSep = ":";
            this.ISA02AuthInfo = "".PadLeft(10, ' ');
            this.ISA04SecInfo = "".PadLeft(10, ' ');

           
            ListOfSBRData = new List<_270Data>();
        }


        public string ISA01AuthQual { get; set; }
        public string ISA02AuthInfo { get; set; }
        public string ISA03SecQual { get; set; }
        public string ISA04SecInfo { get; set; }
        public string ISA05SenderQual { get; set; }
        public string ISA06SenderID { get; set; }
        public string ISA07ReceiverQual { get; set; }
        public string ISA08ReceiverID { get; set; }
        //public string ISA09Date { get; set; }
        //public string ISA10Time { get; set; }
        //public string ISA11RepetitionSep { get; set; }
        //public string ISA12Version { get; set; }
        public string ISA13CntrlNumber { get; set; }
        //public string ISA14AckRequested { get; set; }
        public string ISA15UsageIndi { get; set; }
        //public string ISA16CompoEleSep { get; set; }

        public string GS02SenderID { get; set; }
        public string GS03ReceiverID { get; set; }

        

        public List<_270Data> ListOfSBRData { get; set; }

    }

}

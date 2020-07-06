using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediFusionPM.BusinessLogic._999
{
    public class _999Header
    {
        public string FilePath { get; set; }
        public string ISA06SenderID { get; set; }
        public string ISA08ReceiverID { get; set; }
        public DateTime ?ISADateTime { get; set; }
        public string ISAControlNumber { get; set; }
        public string GS02SenderID { get; set; }
        public string GS03ReceiverID { get; set; }
        public string GSControlNumber { get; set; }
        public string STControlNumber { get; set; }
        public string VersionNumber { get; set; }

        public List<_999TransactionSet> ListOfTransactions { get; set; }

        public string AK9_01_BatchAcknowledgmentCode { get; set; }
        public string AK9_02_NoOfTotalST { get; set; }
        public string AK9_03_NoOfReceivedST { get; set; }
        public string AK9_04_NoOfAcceptedST { get; set; }

        public string FileStatus { get; set; }
    }

}

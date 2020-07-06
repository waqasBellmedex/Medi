using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediFusionPM.BusinessLogic._999
{
    public class _999TransactionSet
    {
        

        public string AK1_01_Functional_Code { get; set; }
        public string AK1_02_GroupControlNum { get; set; }
        public string Ak1_03_Version { get; set; }

        public string AK2_01_TransactionSetCode { get; set; }
        public string Ak2_02_STControlNum { get; set; }

        public List<_999Error> ListOf999Errors { get; set; }


        public string IK5_01_ST_AcknowledgmentCode { get; set; }
        public string IK5_02_ErrorCode { get; set; }


    }

}

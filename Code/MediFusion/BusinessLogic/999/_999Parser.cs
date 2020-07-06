using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediFusionPM.BusinessLogic._999
{
    public class _999Parser
    {
        int _counter = 0;
        char E = ' ', C = ' ', S = ' ';
        string[] elements = null;
        string[] segments = null;
        List<_999Header> _999Data = null;

        string ISA06, ISA08, ISAControlNum, GS02, GS03, GSConrolNum, Version;
        DateTime? ISADate;
        string filepath = string.Empty;

        public List<_999Header> Parse999File(string FilePath)
        {
            _999Data = new List<_999Header>();
            _counter = 0;
            filepath = FilePath;

            if (!File.Exists(FilePath)) throw new Exception(string.Format("File {0} not found.", FilePath));
            string contents = File.ReadAllText(FilePath);
            if (!contents.StartsWith("ISA") && contents.Length < 200) throw new Exception(string.Format("Invalid File {0}", FilePath));

            E = char.Parse(contents.Substring(3, 1));
            C = char.Parse(contents.Substring(104, 1));
            S = char.Parse(contents.Substring(105, 1));

            segments = contents.Split(S);

            while (segments.Length != _counter + 1)
            {
                elements = segments[_counter].Split(E);
                switch (elements.GetElement(0).Trim())
                {
                    case "ISA":
                        ISA06 = elements.GetElement(6);
                        ISA08 = elements.GetElement(8);
                        ISADate = Utilities.GetDate(elements.GetElement(9), elements.GetElement(10));
                        ISAControlNum = elements.GetElement(13);
                        break;

                    case "GS":
                        GS02 = elements.GetElement(2);
                        GS03 = elements.GetElement(3);
                        if (elements.Length >= 9) Version = elements.GetElement(8);
                        GSConrolNum = elements.GetElement(6);
                        break;

                    case "ST":
                        ParseST();
                        break;

                    case "SE":
                    case "GE":
                    case "IEA":
                        break;

                    default:
                        break;
                }
                _counter += 1;
            }

            return _999Data;
        }

        private void ParseST()
        {
            _999Header header = new _999Header();
            header.ListOfTransactions = new List<_999TransactionSet>();

            header.FilePath = filepath;
            header.ISA06SenderID = this.ISA06;
            header.ISA08ReceiverID = this.ISA08;
            header.ISADateTime = this.ISADate;
            header.ISAControlNumber = this.ISAControlNum;
            header.GS02SenderID = this.GS02;
            header.GS03ReceiverID = this.GS03;
            header.GSControlNumber = this.GSConrolNum;
            header.VersionNumber = this.Version;


            if (elements.GetElement(1) != "999") throw new Exception("Invalid File.");
            header.STControlNumber = elements.GetElement(2);
            _counter += 1;
            bool stCondition = true;

            _999TransactionSet st = new _999TransactionSet();

            while (stCondition)
            {
                elements = segments[_counter].Split(E);
                switch (elements.GetElement(0).Trim())
                {
                    case "AK1":
                        st.AK1_01_Functional_Code = elements.GetElement(1);
                        st.AK1_02_GroupControlNum = elements.GetElement(2);
                        st.Ak1_03_Version = elements.GetElement(3);
                        break;

                    case "AK2":
                        st.AK2_01_TransactionSetCode = elements.GetElement(1);
                        st.Ak2_02_STControlNum = elements.GetElement(2);
                        break;
                    
                    case "IK3":
                        //_999Error error = new _999Error();
                        //error.IK3_01SegmentID = elements.GetElement(1);
                        //error.IK3_02SegmentPositionInST = elements.GetElement(2);
                        throw new NotImplementedException("IK3 Error Parsing Not done yet. Please contact Aziz.");
                        //break;

                    case "IK5":
                        st.IK5_01_ST_AcknowledgmentCode = elements.GetElement(1);
                        st.IK5_02_ErrorCode = elements.GetElement(2);
                        break;

                    case "AK9":
                        header.AK9_01_BatchAcknowledgmentCode = elements.GetElement(1);
                        header.AK9_02_NoOfTotalST = elements.GetElement(2);
                        header.AK9_03_NoOfReceivedST = elements.GetElement(3);
                        header.AK9_04_NoOfAcceptedST = elements.GetElement(4);

                        if (header.AK9_01_BatchAcknowledgmentCode == "A") header.FileStatus = "Accepted";
                        else if (header.AK9_01_BatchAcknowledgmentCode == "E") header.FileStatus = "Accepted with Errors";
                        else if (header.AK9_01_BatchAcknowledgmentCode == "R") header.FileStatus = "Rejected";

                        break;
                    case "SE":
                    case "GE":
                    case "IEA":
                        _counter -= 1; stCondition = false;
                        if (!header.ListOfTransactions.Contains(st)) header.ListOfTransactions.Add(st);
                        if (!_999Data.Contains(header)) _999Data.Add(header);                            
                        break;
                }
                _counter += 1;
            }
        }
    }
}

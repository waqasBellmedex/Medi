using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediFusionPM.BusinessLogic._999
{
    public class _999Error
    {

        public string IK3_01SegmentID { get; set; }
        public long IK3_02SegmentPositionInST { get; set; }
        public string IK03_LoopID { get; set; }
        public string IK04ErrorCode { get; set; }

        public List<SegmentContext> ListOfSegmentContext { get; set; }

        // Business Unit  - Context Reference
        public string CTX01_1_BU_ContextName { get; set; }
        public string CTX01_2_BU_ContextReference { get; set; }

        public string IK401_1ElementPositionInSegment { get; set; }
        public string IK401_2CompositeElementPosition { get; set; }
        public string IK401_3RepeatingElementPosition { get; set; }
        public string IK402_DataElementRefNum { get; set; }
        public string IK403_ErrorCode { get; set; }
        public string IK404_BadData { get; set; }

        public List<SegmentContext> ListOfElementContext { get; set; }


        public class SegmentContext
        {
            public string CTX01_1ContextName { get; set; }
            public string CTX01_2ContextReference { get; set; }
            public string CTX02SegmentID { get; set; }
            public string CTX03SegmentPositionInST { get; set; }
            public string CTX04LoopID { get; set; }
            public string CTX05_1ElementPositionInSegment { get; set; }
            public string CTX05_2CompositeElementPosition { get; set; }
            public string CTX05_3RepeatingElementPosition { get; set; }
            public string CTX06_1DataElementRefNum { get; set; }
        }


        public class ElementContext
        {
            public string CTX01_1ContextName { get; set; }
            public string CTX01_2ContextReference { get; set; }
            public string CTX02SegmentID { get; set; }
            public string CTX03SegmentPositionInST { get; set; }
            public string CTX04LoopID { get; set; }
            public string CTX05_1ElementPositionInSegment { get; set; }
            public string CTX05_2CompositeElementPosition { get; set; }
            public string CTX05_3RepeatingElementPosition { get; set; }
            public string CTX06_1DataElementRefNum { get; set; }
        }



    }

}

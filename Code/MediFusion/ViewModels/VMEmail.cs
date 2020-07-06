using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ViewModels
{
    public class VMEmail
    {
        public string[] sendTo { get; set; }
        public string body { get; set; }

        public string subject { get; set; }
        public string[] CC { get; set; }
       public List<AttachmentRecord> attachmentRecord { get; set; }
    }
    public class AttachmentRecord
    {
       public  string name { get; set; }
        public string attachment { get; set; }

    }

    public class SEmail
    {
        public string sendFrom { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string sendTo { get; set; }
        public string CC { get; set; }
        public string attachment { get; set; }

    }
    public class VMAttachments
    {
        public string attachment { get; set; }
        public long ID { get; set; }
        public string attachmenturl { get; set; }


    }

}

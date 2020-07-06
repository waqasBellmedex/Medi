using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ViewModels
{
    public class FileUploadViewModel
    {
        public string Content { get; set; }
        //public IFormFile File { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public long ClientID { get; set; }
        public string Type { get; set; }
        public long ReceiverID { get; set; }
    }

    public class ListModel
    {
        public long[] Ids { get; set; }
        
        public string Status { get; set; }

    }
}

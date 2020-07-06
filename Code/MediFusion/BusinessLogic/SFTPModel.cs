using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediFusionPM.BusinessLogic
{
    public class SFTPModel
    {

        public string RootDirectory { get; set; }

        public string FTPHost { get; set; }
        public string FTPPort { get; set; }
        public string FTPUserName { get; set; }
        public string FTPPassword { get; set; }

        public bool SubmitToFTP { get; set; }
        public string SubmitDirectory { get; set; }
        public string FileName { get; set; }
        public string ConnectivityType { get; set; }

        public string DownloadDirectory { get; set; }
        public string ArchiveDirectory { get; set; }
        public bool ArchiveFiles { get; set; }

        

    }
}
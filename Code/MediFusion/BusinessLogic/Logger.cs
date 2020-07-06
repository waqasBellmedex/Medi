using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.BusinessLogic
{
    public class Logger
    {
        public string FilePath { get; set; }
        
        public void LogExp(string Text) 
        {
            File.WriteAllText(FilePath, Text);
        }
    }
}

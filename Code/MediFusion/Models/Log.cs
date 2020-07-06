using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public static class Log
    {
        public static void WriteQueryTime(string BasePath, string PracticeName, string Email, string Controller, string Method, DateTime StartTime, int Records)
            {
            try
            {
                string path = Path.Combine(BasePath, "QueriesLog", PracticeName, Email, Controller, Method);
                Directory.CreateDirectory(path);

                string logFile = Path.Combine(path, "Log.txt");
                string logMessage = (DateTime.Now - StartTime).Seconds + "\t" + DateTime.Now + "\t" + Records + "\n";
                File.AppendAllText(logFile, logMessage);
            }
            catch (Exception e)
            {
            }
        }
    }
}

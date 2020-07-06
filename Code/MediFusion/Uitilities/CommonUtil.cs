using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Uitilities
{
    public class CommonUtil
    {
        public static string GetConnectionString(long PracticeId, string Temp)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            configuration = builder.Build();
            string connectionString = configuration.GetConnectionString("MedifusionLocal");
            string[] splitString = connectionString.Split(';');
            splitString[1] = splitString[1];

            connectionString = splitString[0] + "; " + splitString[1] + Temp + "; " + splitString[2] + "; " + splitString[3];

            return connectionString;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
using System.IO;
using System.Data;
using System.Xml.Linq;
using System.Configuration;

namespace MediFusionPM.BusinessLogic
{
    public static class Utilities
    {
        public static string GetElement(this string[] Elements, int index)
        {
            string value = string.Empty;
            if (Elements.Length > index)
            {
                value = Elements[index];
            }
            return value;
        }
        public static bool IsNull2(this string Value)
        {
            if (string.IsNullOrWhiteSpace(Value) || string.IsNullOrEmpty(Value))
                return true;
            else return false;
        }


        public static decimal? GetElementAmount(this string[] Elements, int index)
        {
            decimal? value = null;
            if (Elements.Length > index)
            {
                value = decimal.Parse(Elements[index]);
            }
            return value;
        }

        public static DateTime? GetElementDate(this string[] Elements, int index)
        {
            DateTime? value = null;
            if (Elements.Length > index)
            {
                value = GetDate(Elements[index], string.Empty);
            }
            return value;
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static DateTime? GetDate(string DateYYYYMMDD, string TimeHHMM)
        {
            DateTime ?d = null;
            try
            {
                int year = 0, month = 0, days = 0, hours = 0, minutes = 0, seconds = 0;

                if (DateYYYYMMDD.Length == 6)
                {
                    year = int.Parse("20" + DateYYYYMMDD.Substring(0, 2));
                    month = int.Parse(DateYYYYMMDD.Substring(2, 2));
                    days = int.Parse(DateYYYYMMDD.Substring(4, 2));
                }
                else if (DateYYYYMMDD.Length == 8)
                {
                    year = int.Parse(DateYYYYMMDD.Substring(0, 4));
                    month = int.Parse(DateYYYYMMDD.Substring(4, 2));
                    days = int.Parse(DateYYYYMMDD.Substring(6, 2));
                }
                if (!string.IsNullOrEmpty(TimeHHMM))
                {
                    hours = int.Parse(TimeHHMM.Substring(0, 2));
                    minutes = int.Parse(TimeHHMM.Substring(2, 2));
                }
                
                d =  new DateTime(year, month, days, hours, minutes, seconds);
            }
            catch (Exception)
            {
                
            }
            return d;

        }

        public static bool IsNull(this DateTime Date)
        {
            if (Date <= DateTime.MinValue)
                return true;
            else return false;
        }

        public static bool IsNull(this string Value)
        {
            if (string.IsNullOrWhiteSpace(Value))
                return true;
            else return false;
        }

        public static decimal Val2(this decimal? Value)
        {
            if (Value == null || Value == 0)
                return 0;
            else return Value.Value;
        }

        public static decimal? Val2(this decimal Value)
        {
            if (Value == 0)
                return null;
            else return Value;
        }

        public static string Get(this List<string> List, int Index)
        {
            if (List != null && List.Count > Index)
                return List[Index];
            else return "";
        }

        public static bool IsNull(this Enum Value)
        {
            if (Value != null)
                return true;
            else return false;
        }

       
        public static decimal Amt(this decimal? Amount)
        {
            decimal amount = 0;
            amount = Amount == null ? 0 : Convert.ToDecimal(Amount);
            return amount;
        }

        
        public static string GetSubmissionDirectory(string SFTPHost, string SFTPUser)
        { 
            string rootPath = ConfigurationManager.AppSettings["RootPath"];
            string DirPath = Path.Combine(rootPath, "837P", SFTPUser, SFTPHost, DateTime.Now.ToString("MMddyy"), DateTime.Now.ToString("hhmmss"));
            Directory.CreateDirectory(DirPath);

            return DirPath;
        }

        public static string GetDownloadDirectory(string SFTPHost, string SFTPUser)
        {
            string rootPath = ConfigurationManager.AppSettings["RootPath"];
            string DirPath = Path.Combine(rootPath, "DOWNLOADS", SFTPUser, SFTPHost, DateTime.Now.ToString("MMddyy"), DateTime.Now.ToString("hhmmss"));
            Directory.CreateDirectory(DirPath);

            return DirPath;
        }




        public static string GetFileType(string FilePath)
        {
            string fileType = string.Empty;
            string fileContents = FilePath.StartsWith("ISA") ? FilePath : System.IO.File.ReadAllText(FilePath).Trim();
            if (!fileContents.StartsWith("ISA"))
            {
                fileType = "HR";
            }
            else
            {
                string E = fileContents.Substring(3, 1);

                if (fileContents.Contains(string.Format("ST{0}999{0}", E)))
                    fileType = "999";
                else if (fileContents.Contains(string.Format("ST{0}277{0}", E)) && fileContents.Contains("005010X214"))
                    fileType = "277CA";
                else if (fileContents.Contains(string.Format("ST{0}277{0}", E)))
                    fileType = "277";
                else if (fileContents.Contains(string.Format("ST{0}835{0}", E)))
                    fileType = "835";
                else if (fileContents.Contains(string.Format("ST{0}271{0}", E)))
                    fileType = "271";
                else fileType = "HR";
            }
            return fileType;
        }






    }
}

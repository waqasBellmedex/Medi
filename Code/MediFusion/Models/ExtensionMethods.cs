using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models
{
    public static class ExtensionMethods
    {
        public static bool IsNull(this string Value)
        {
            if (string.IsNullOrWhiteSpace(Value) || string.IsNullOrEmpty(Value))
                return true;
            else return false;
        }
        public static bool IsNull(this bool? Value)
        {
            if ( Value==null)
                return true;
            else return false;
        }
        public static bool IsNull_Bool(this bool? Value)
        {
            if (Value == null)
                return false;
            else return Value.Value;
        }
        public static bool IsNull(DateTime? Value)
        {
            if (Value==null || Value.Value.Year<2019)
                return true;
            else return false;
        }
        public static string GetValueIfNotNull(this string Value, string NewValue)
        {
            if (!string.IsNullOrWhiteSpace(NewValue))
                return NewValue;
            else return Value;
        }


        public static bool IsNull(this long? Value)
        {
            if (Value == null || Value == 0)
                return true;
            else return false;
        }

        public static bool IsNull(this int? Value)
        {
            if (Value == null || Value == 0)
                return true;
            else return false;
        }

        public static bool IsNull(this decimal? Value)
        {
            if (Value == null || Value == 0)
                return true;
            else return false;
        }

        public static bool IsNull(this long Value)
        {
            if (Value == 0)
                return true;
            else return false;
        }
        public static bool IsNull(this DateTime Value)
        {
            if (Value.Equals("0-0-0000") || Value <= DateTime.MinValue)
                return true;
            else return false;
        }


        public static string Format(this DateTime? Value, string Pattern = "MM/dd/yyyy")
        {
            if (Value == null) return "";
            else return ((DateTime)Value).ToString(Pattern);
        }

        public static string Format(this DateTime Value, string Pattern = "MM/dd/yyyy")
        {
            if (Value <= DateTime.MinValue) return "";
            else return ((DateTime)Value).ToString(Pattern);
        }

        public static DateTime Date(this DateTime? Value)
        {
            if (Value <= DateTime.MinValue || Value == null) return DateTime.MinValue;
            else return ((DateTime)Value);
        }
        public static DateTime Date(this DateTime Value,string Pattern = "MM/dd/yyyy")
        {
            if (Value <= DateTime.MinValue || Value == null) return DateTime.MinValue;
            else return ((DateTime)Value);
        }

        public static decimal Val(this decimal? Value)
        {
            if (Value == null || Value == 0)
                return 0;
            else return Value.Value;
        }

        public static decimal? Val(this decimal Value)
        {
            if (Value == 0)
                return null;
            else return Value;
        }

        //public static long? Val(this string Value)
        //{
        //    if (Value.IsNull())
        //        return null;
        //    else return long.Parse(Value);
        //}

        public static int? Val(this string Value)
        {
            if (Value.IsNull())
                return null;
            else return int.Parse(Value);
        }



        public static long? ValZero(this long? Value)
        {
            if (Value.IsNull())
                return 0;
            else return Value;
        }


        public static string ValStr(this string Value)
        {
            if (string.IsNullOrWhiteSpace(Value))
                return string.Empty;
            else return Value;
        }


        public static bool IsBetweenDOS(DateTime? ModelDateTo, DateTime? ModelDateFrom, DateTime? TableDateTo, DateTime? TableDateFrom)
        {
            try
            {

                bool DateToResult = true;
                bool DateFromResult = true;

                if (ModelDateTo != null && ModelDateFrom != null)
                {

                    if (TableDateTo != null)
                    {

                        DateToResult = TableDateTo.Value.Date <= ModelDateTo.Value.Date.AddHours(23).AddMinutes(59);
                        DateFromResult = TableDateFrom.Value >= ModelDateFrom.Value.Date;
                        Debug.WriteLine("2");
                        Debug.WriteLine(ModelDateTo.Value + "  compared with " + TableDateTo.Value);
                        Debug.WriteLine(ModelDateFrom.Value + "  compared with " + TableDateFrom.Value);
                    }



                    else
                    {
                        if (!TableDateFrom.HasValue )
                        {
                            return false;
                        }
                        else
                        {
                            DateFromResult = TableDateFrom.Value >= ModelDateFrom.Value;
                            DateToResult = true;

                            Debug.WriteLine("1");
                            Debug.WriteLine(ModelDateFrom.Value + "  compared with " + TableDateFrom.Value);
                        }
                    }
                }
                else if (ModelDateFrom != null)
                {
                    if (TableDateFrom != null)
                    {
                        DateFromResult = TableDateFrom.Value >= ModelDateFrom.Value;
                        DateToResult = true;
                        Debug.WriteLine("1");
                        Debug.WriteLine(ModelDateFrom.Value + "  compared with " + TableDateFrom.Value);
                    }
                    else
                    {
                        if (!TableDateFrom.HasValue)
                        {
                            return false;
                        }
                        else
                        {
                            DateFromResult = TableDateFrom.Value >= ModelDateFrom.Value;
                            DateToResult = true;

                            //Debug.WriteLine("1");
                            //Debug.WriteLine(ModelDateFrom.Value + "  compared with " + TableDateFrom.Value);
                            //DateFromResult = true;
                            DateToResult = true;
                        }

                        //DateFromResult = true;
                        //DateToResult = true;
                    }
                }
                else
                {
                    DateFromResult = true;
                    DateToResult = true;
                }

                Debug.WriteLine(DateToResult + "      " + DateFromResult);
                //Debug.WriteLine(DateToResult +"   "+ DateFromResult);
                return DateToResult && DateFromResult;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "     isfisjdfijsdfijsdf\n\n\n\n\\n" + ex.StackTrace);
                return true;
            }
        }

        public static double DateDiff(DateTime To, DateTime from)
        {

            return (To - from).TotalDays;
        }
    }
}

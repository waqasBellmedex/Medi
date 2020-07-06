using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediFusionPM.BusinessLogic.EligGenerator
{
    public class _270Generator
    {
        #region GLOBALS

        string E = string.Empty;
        string S = string.Empty;
        string C = string.Empty;
        string R = string.Empty;



        #endregion

        #region PROPERTIES
        //public _270Header Header { get; set; }
        //public _270Data SBRData { get; set; }
        #endregion

        #region CONSTRUCTOR
        public _270Generator()
        {
            //this.gsCtrlNum = "111111111";
            this.E = "*";
            this.S = "~";
            this.C = ":";
            this.R = "^";
        }
        #endregion

        #region EXPOSED METHODS
        public string Generate270Transaction(_270Header Header)
        {
            string eligRequest = string.Empty;
            try
            {
                if (Header == null || Header.ListOfSBRData == null)
                {
                    throw new Exception("Header, Subscriber data cannot be empty.");
                }
                if (string.IsNullOrEmpty(Header.ISA13CntrlNumber))
                {
                    throw new Exception("ISA13CntrlNumber feild cannot be empty");
                }

                string header = string.Empty;
                DateTime D = DateTime.Now;

                Header.ISA01AuthQual = string.IsNullOrEmpty(Header.ISA01AuthQual) ? "00" : Header.ISA01AuthQual.PadLeft(2, '0');
                Header.ISA02AuthInfo = string.IsNullOrEmpty(Header.ISA02AuthInfo) ? string.Empty.PadLeft(10) : Header.ISA02AuthInfo.PadRight(10);
                Header.ISA03SecQual = string.IsNullOrEmpty(Header.ISA03SecQual) ? "00" : Header.ISA03SecQual.PadLeft(2, '0');
                Header.ISA04SecInfo = string.IsNullOrEmpty(Header.ISA04SecInfo) ? string.Empty.PadLeft(10) : Header.ISA04SecInfo.PadRight(10);
                Header.ISA05SenderQual = string.IsNullOrEmpty(Header.ISA05SenderQual) ? "ZZ" : Header.ISA05SenderQual.PadLeft(2, '0');
                Header.ISA07ReceiverQual = string.IsNullOrEmpty(Header.ISA07ReceiverQual) ? "ZZ" : Header.ISA07ReceiverQual.PadLeft(2, '0');

                Header.ISA13CntrlNumber = Header.ISA13CntrlNumber.PadLeft(9, '0').Substring(0, 9);

                eligRequest += "ISA" + E + Header.ISA01AuthQual + E + Header.ISA02AuthInfo + E + Header.ISA03SecQual + E + Header.ISA04SecInfo + E + Header.ISA05SenderQual + E + Header.ISA06SenderID.PadRight(15, ' ')
                                    + E + Header.ISA07ReceiverQual + E + Header.ISA08ReceiverID.PadRight(15, ' ') + E + D.ToString("yyMMdd") + E + D.ToString("hhmm") + E + R + E + "00501" + E + Header.ISA13CntrlNumber
                                    + E + "1" + E + Header.ISA15UsageIndi + E + C + S;
                eligRequest += "GS" + E + "HS" + E + Header.GS02SenderID + E + Header.GS03ReceiverID + E + D.ToString("yyyyMMdd") + E + D.ToString("hhmm") + E + Header.ISA13CntrlNumber + E + "X" + E + "005010X279A1" + S;
                eligRequest += "ST" + E + "270" + E + "0001" + E + "005010X279A1" + S;
                eligRequest += "BHT" + E + "0022" + E + "13" + E + Header.ISA13CntrlNumber + E + D.ToString("yyyyMMdd") + E + D.ToString("hhmm") + S;

                int hlPayer = 0;
                int hlCounter = 0;
                int hlProvider = 0;

                foreach (_270Data SBRData in Header.ListOfSBRData)
                {
                    SBRData.PayerEntity = string.IsNullOrEmpty(SBRData.PayerEntity) ? "2" : SBRData.PayerEntity;
                    SBRData.PayerQual = string.IsNullOrEmpty(SBRData.PayerQual) ? "PI" : SBRData.PayerQual;

                    SBRData.ProvEntity = string.IsNullOrEmpty(SBRData.ProvFirstName) ? "2" : "1";
                    SBRData.ProvQual = string.IsNullOrEmpty(SBRData.ProvQual) ? "XX" : SBRData.ProvQual;
                    SBRData.SBREntityType = "1";

                    if (SBRData.SBRGender.ToUpper() == "MALE") SBRData.SBRGender = "M";
                    else if (SBRData.SBRGender.ToUpper().Replace(" ", "") == "FEMALE") SBRData.SBRGender = "F";

                    if (!SBRData.PATGender.IsNull())
                    {
                        if (SBRData.PATGender.ToUpper() == "MALE") SBRData.PATGender = "M";
                        else if (SBRData.PATGender.ToUpper().Replace(" ", "") == "FEMALE") SBRData.PATGender = "F";
                    }

                    hlPayer = hlCounter + 1;
                    hlCounter = hlPayer + 1;

                    eligRequest += "HL" + E + hlPayer + E + E + "20" + E + "1" + S;
                    eligRequest += "NM1" + E + "PR" + E + SBRData.PayerEntity + E + SBRData.PayerOrgName + E + SBRData.PayerFirstName + E + E + E + E + "PI" + E + SBRData.PayerID + S;

                    eligRequest += "HL" + E + hlCounter + E + hlPayer + E + "21" + E + "1" + S;
                    eligRequest += "NM1" + E + "1P" + E + SBRData.ProvEntity + E + SBRData.ProvLastName + E + SBRData.ProvFirstName + E + SBRData.ProvMI + E + E + E + SBRData.ProvQual + E + SBRData.ProvNPI + S;
                    if (!SBRData.ProvTaxonomyCode.IsNull())
                        eligRequest += "PRV" + E + "BI" + E + "PXC" + E + SBRData.ProvTaxonomyCode + S;

                    hlProvider = hlCounter; hlCounter += 1;


                     eligRequest += "HL" + E + hlCounter + E + hlProvider + E + "22" + E + (SBRData.PatientRelationValue.ToString().Contains("18") ? "0" : "1") + S;
                    if (SBRData.PatientRelationValue.ToString().Contains("18"))
                    {
                        eligRequest += "TRN" + E + "1" + E + SBRData.TRN01 + E + SBRData.TRN02 + S;
                        eligRequest += "NM1" + E + "IL" + E + "1" + E + SBRData.SBRLastName + E + SBRData.SBRFirstName + E + SBRData.SBRMiddleInitial + E + E + E + "MI" + E + SBRData.SBRID + S;
                        if (SBRData.SBRDob != null)
                            eligRequest += "DMG" + E + "D8" + E + string.Format("{0:yyyyMMdd}", SBRData.SBRDob) + E + SBRData.SBRGender + S;
                        if (!string.IsNullOrEmpty(SBRData.SBRSSN))
                            eligRequest += "REF" + E + "SY" + E + SBRData.SBRSSN + S;
                    }
                    else
                    {
                        hlCounter += 1;

                        eligRequest += "HL" + E + hlCounter + E + (hlCounter -1) + E + "23" + "0" + S;
                        eligRequest += "TRN" + E + "1" + E + SBRData.TRN01 + E + SBRData.TRN02;
                        eligRequest += "NM1" + E + "03" + E + "1" + E + SBRData.PATLastName + E + SBRData.ProvFirstName + E + SBRData.ProvMI + S;
                        if (SBRData.SBRDob != null)
                            eligRequest += "DMG" + E + "D8" + E + string.Format("{0:yyyyMMdd}", SBRData.SBRDob) + E + SBRData.SBRGender + S;

                        string[] v = SBRData.PatientRelationValue.ToString().Split('_');
                        eligRequest += "INS" + E + "N" + E + v[v.Length - 1] + S;
                    }

                    eligRequest += "DTP" + E + "291" + E + "D8" + E + string.Format("{0:yyyyMMdd}", SBRData.EligiblityForDate) + S;
                    eligRequest += "EQ" + E + "30" + S;
                }

                int count = eligRequest.Count(x => x == char.Parse(S)) - 2;

                eligRequest += "SE" + E + (count + 1).ToString() + E + "0001" + S;
                eligRequest += "GE" + E + "1" + E + Header.ISA13CntrlNumber + S;
                eligRequest += "IEA" + E + "1" + E + Header.ISA13CntrlNumber + S;


                //_270Data SBRData = Header.ListOfSBRData;

                //SBRData.ProvEntity = string.IsNullOrEmpty(SBRData.ProvFirstName) ? "2" : "1";
                //SBRData.ProvQual = string.IsNullOrEmpty(SBRData.ProvQual) ? "XX" : SBRData.ProvQual;
                //SBRData.SBREntityType = "1";


                //eligRequest += "ISA" + E + Header.ISA01AuthQual + E + Header.ISA02AuthInfo + E + Header.ISA03SecQual + E + Header.ISA04SecInfo + E + Header.ISA05SenderQual + E + Header.ISA06SenderID.PadRight(15, ' ')
                //                + E + Header.ISA07ReceiverQual + E + Header.ISA08ReceiverID.PadRight(15, ' ') + E + D.ToString("yyMMdd") + E + D.ToString("hhmm") + E + R + E + "00501" + E + Header.ISA13CntrlNumber
                //                + E + "1" + E + Header.ISA15UsageIndi + E + C + S;
                //eligRequest += "GS" + E + "HS" + E + Header.GS02SenderID + E + Header.GS03ReceiverID + E + D.ToString("yyyyMMdd") + E + D.ToString("hhmm") + E + Header.ISA13CntrlNumber + E + "X" + E + "005010X279A1" + S;
                //eligRequest += "ST" + E + "270" + E + "0001" + E + "005010X279A1" + S;
                //eligRequest += "BHT" + E + "0022" + E + "13" + E + Header.ISA13CntrlNumber + E + D.ToString("yyyyMMdd") + E + D.ToString("hhmm") + S;


                //eligRequest += "HL" + E + "1" + E + E + "20" + E + "1" + S;
                //eligRequest += "NM1" + E + "PR" + E + Header.PayerEntity + E + Header.PayerOrgName + E + Header.PayerFirstName + E + E + E + E + "PI" + E + Header.PayerID + S;

                //eligRequest += "HL" + E + "2" + E + "1" + E + "21" + E + "1" + S;
                //eligRequest += "NM1" + E + "1P" + E + SBRData.ProvEntity + E + SBRData.ProvLastName + E + SBRData.ProvFirstName + E + SBRData.ProvMI + E + E + E + SBRData.ProvQual + E + SBRData.ProvNPI + S;


                //eligRequest += "HL" + E + "3" + E + "2" + E + "22" + E + (SBRData.PatientRelationValue.ToString().Contains("18") ? "0" : "1") + S;
                //if (SBRData.PatientRelationValue.ToString().Contains("18"))
                //{
                //    eligRequest += "TRN" + E + "1" + E + SBRData.TRN01 + E + SBRData.TRN02 + S;
                //    eligRequest += "NM1" + E + "IL" + E + "1" + E + SBRData.SBRLastName + E + SBRData.SBRFirstName + E + SBRData.SBRMiddleInitial + E + E + E + "MI" + E + SBRData.SBRID + S;
                //    if (SBRData.SBRDob != null)
                //        eligRequest += "DMG" + E + "D8" + E + string.Format("{0:yyyyMMdd}", SBRData.SBRDob) + E + SBRData.SBRGender + S;
                //    if (!string.IsNullOrEmpty(SBRData.SBRSSN))
                //        eligRequest += "REF" + E + "SY" + E + SBRData.SBRSSN + S;
                //}
                //else
                //{
                //    eligRequest += "HL" + E + "4" + E + "3" + E + "23" + "0" + S;
                //    eligRequest += "TRN" + E + "1" + E + SBRData.TRN01 + E + SBRData.TRN02;
                //    eligRequest += "NM1" + E + "03" + E + "1" + E + SBRData.PATLastName + E + SBRData.ProvFirstName + E + SBRData.ProvMI + S;
                //    if (SBRData.SBRDob != null)
                //        eligRequest += "DMG" + E + "D8" + E + string.Format("{0:yyyyMMdd}", SBRData.SBRDob) + E + SBRData.SBRGender + S;

                //    string[] v = SBRData.PatientRelationValue.ToString().Split('_');
                //    eligRequest += "INS" + E + "N" + E + v[v.Length - 1] + S;
                //}

                //eligRequest += "DTP" + E + "291" + E + "D8" + E + string.Format("{0:yyyyMMdd}", SBRData.EligiblityForDate) + S;
                //eligRequest += "EQ" + E + "30" + S;

                //int count = eligRequest.Count(x => x == char.Parse(S)) - 2;

                //eligRequest += "SE" + E + (count + 1).ToString() + E + "0001" + S;
                //eligRequest += "GE" + E + "1" + E + Header.ISA13CntrlNumber + S;
                //eligRequest += "IEA" + E + "1" + E + Header.ISA13CntrlNumber + S;

            }
            catch (Exception exc)
            {
                throw exc;
            }
            return eligRequest;
        }
        #endregion

    }
}

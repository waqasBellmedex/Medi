using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MediFusionPM.BusinessLogic.ClaimGeneration.ClaimGenerator.Output;

namespace MediFusionPM.BusinessLogic.ClaimGeneration
{

    public class ClaimGenerator
    {
        public IConfiguration Configuration { get; }

        #region GLOBALS
        string E = string.Empty;
        string S = string.Empty;
        string C = string.Empty;
        string R = string.Empty;
        int count = 0;
        int hlCounter = 0, hlBPrvCounter = 0, hlSbrCounter = 0, lxCounter = 0;
        string billPrvNPI = string.Empty;
        #endregion

        #region PROPERTIES
        public decimal SubmittedAmount { get; internal set; }
        public DateTime SubmittedDate { get; internal set; }
        public long SubmittedClaims { get; internal set; }
        #endregion

        string testMode = "", fileSequence = "";

        #region CONSTRUCTOR
        public ClaimGenerator(string TestMode = "", string FileSequence= "")
        {
            this.E = "*";
            this.S = "~";
            this.C = ":";
            this.R = "^";

            testMode = TestMode;
        }
        #endregion

        #region EXPOSED METHODS
        public Output Generate837Transaction(ClaimHeader Header)
        {

            //string testMode = ConfigurationManager.AppSettings["TestMode"];
            //string testMode = Configuration["SubmissionSettings:TestMode"]; //Values Comming From appsettings
            //if (testMode.IsNull()) testMode = "Y";
            //string fileSequence = ConfigurationManager.AppSettings["FileSequence"];
            //string fileSequence = Configuration["SubmissionSettings:FileSequence"]; //Values Comming From appsettings
            
            
            
            Output output = new Output();
            if (Header.SFTPModel == null)
            {
                output.ErrorMessage = "SFTP Fields can not be Empty.";
                return output;
            }

            string submitDirectory = Header.SFTPModel.SubmitDirectory;

            // string DirPath = Utilities.GetSubmissionDirectory(Header.SFTPModel.FTPHost, Header.SFTPModel.FTPUserName);
            //string DirPath = Header.SFTPModel.RootDirectory;
             //string DirPath = "E:\\Backups";
            string DirPath = Header.SFTPModel.RootDirectory;
            Directory.CreateDirectory(DirPath);

            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(Header);
            File.WriteAllText(Path.Combine(DirPath, "Input.Json"), jsonString);

            string claim = string.Empty;
            try
            {
                if (Header == null || Header.Claims == null || Header.Claims.Count == 0)
                {
                    output.ErrorMessage = "Header, Claims or Charges can not be Empty.";
                    return output;
                }
                else if (Header.ISA13CntrlNumber.IsNull())
                {
                    output.ErrorMessage = "ISA13 Control Number can not be Empty.";
                    return output;
                }
                else if (Header.ISA15UsageIndi.IsNull())
                {
                    output.ErrorMessage = "ISA15 Usage Indicator can not be Empty.";
                    return output;
                }
                else if (Header.ISA06SenderID.IsNull() || Header.GS02SenderID.IsNull())
                {
                    output.ErrorMessage = "Sender ID can not be Empty";
                    return output;
                }
                else if (Header.ISA08ReceiverID.IsNull() || Header.GS03ReceiverID.IsNull())
                {
                    output.ErrorMessage = "Receiver ID can not be Empty";
                    return output;
                }

                if (Header.SFTPModel.SubmitToFTP &&
                    (Header.SFTPModel.FTPHost.IsNull() || Header.SFTPModel.FTPPassword.IsNull() 
                    || Header.SFTPModel.FTPPort.IsNull() || Header.SFTPModel.FTPUserName.IsNull()))
                {
                    output.ErrorMessage = "FTP Host, User Name, Password, Port can not be Empty.";
                    return output;
                }

                output.Claims = new List<ClaimResult>();

                claim += GenerateHeader(Header);
                foreach (ClaimData CLM in Header.Claims)
                {
                    string msg = Validate(Header, CLM);
                    if (!msg.IsNull())
                    {
                        CLM.ValidationMsg = msg;

                        output.Claims.Add(new ClaimResult()
                        {
                            PatientControlNumber = CLM.PatientControlNumber,
                            VisitID = CLM.VisitID,
                            Submitted = false,
                            ValidationMsg = msg
                        }) ;
                        continue;
                    }

                    if (CLM.BillPrvNPI != billPrvNPI)
                    {
                        claim += GenerateBillingProvider(CLM);
                        billPrvNPI = CLM.BillPrvNPI;
                    }

                    claim += GenerateClaim(CLM);

                    foreach (ChargeData CH in CLM.Charges)
                    {
                        claim += GenerateCharge(CH, CLM);
                    }

                    output.Claims.Add(new ClaimResult()
                    {
                        PatientControlNumber = CLM.PatientControlNumber,
                        Submitted = true,
                        SubmittedDate = CLM.SubmittedDate,
                        VisitID = CLM.VisitID,
                    });
                    output.ClaimAmount += CLM.ClaimAmount;
                }

                count = claim.Count(x => x == char.Parse(S)) - 2;
                claim += GenerateTrailer(Header);

                if (SubmittedClaims > 0)
                {
                    output.ProcessedClaims = SubmittedClaims;
                    output.Transaction837 = claim;

                    string FilePath = string.Empty;
                    string FileName = string.Empty;
                    FileName = !Header.SFTPModel.FileName.IsNull() ? Header.SFTPModel.FileName.Replace("{GS_CONTROL_NUM}", Header.ISA13CntrlNumber) : "";
                    FileName = FileName.IsNull2() ? "837P.txt" : FileName;
                    FilePath = Path.Combine(DirPath, FileName);
                    

                    File.WriteAllText(FilePath, output.Transaction837);

                    if (Header.SFTPModel.SubmitToFTP)
                    {
                        // string FileName = string.Empty;
                        //FileName = Header.SFTPModel.FileName.IsNull() ? "ClaimFile{0}.CLP" : Header.SFTPModel.FileName;
                        //FileName = string.Format(FileName, fileSequence);
                        

                        SFTPSubmission objSubmission = new SFTPSubmission
                            (Header.SFTPModel.FTPHost, Header.SFTPModel.FTPUserName, Header.SFTPModel.FTPPassword,
                            Convert.ToInt16(Header.SFTPModel.FTPPort), Header.SFTPModel.ConnectivityType);
                        if (testMode != "Y")
                        {
                            output.FileSubmittedToFTP = objSubmission.SubmitFile(submitDirectory, FilePath);
                            output.FTPFileName = FileName;

                        }

                        //var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        /*
                        var config = ConfigurationManager.OpenExeConfiguration("~");
                        config.AppSettings.Settings["FileSequence"].Value = (Convert.ToInt64(fileSequence) + 1).ToString();
                        config.Save();
                        */
                        //ConfigurationManager.RefreshSection("appSettings");
                    }

                    output.Transaction837Path = FilePath;
                }
                else
                {
                    output.ErrorMessage = "No Visit(s) Qualified for Submission.";
                }
            }
            catch (Exception exc)
            {
                File.WriteAllText(Path.Combine(DirPath, "Exception.txt"), exc.ToString());
                output.ErrorMessage = "Un Expected Error Occured. Please Contact Support.";
                throw exc;
            }
            finally
            {
                jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(output);
                File.WriteAllText(Path.Combine(DirPath, "Output.Json"), jsonString);
            }
            return output;
        }
        #endregion

        #region PRIVATE METHODS
        
        private string GenerateHeader(ClaimHeader Header)
        {
            Header.ISA01AuthQual = Header.ISA01AuthQual.IsNull() ? "00" : Header.ISA01AuthQual;
            Header.ISA02AuthInfo = Header.ISA02AuthInfo.IsNull() ? string.Empty.PadLeft(10) : Header.ISA02AuthInfo;
            Header.ISA03SecQual = Header.ISA03SecQual.IsNull() ? "00" : Header.ISA03SecQual;
            Header.ISA04SecInfo = Header.ISA04SecInfo.IsNull() ? string.Empty.PadLeft(10) : Header.ISA04SecInfo;
            Header.ISA05SenderQual = Header.ISA05SenderQual.IsNull() ? "ZZ" : Header.ISA05SenderQual;
            Header.ISA07ReceiverQual = Header.ISA07ReceiverQual.IsNull() ? "ZZ" : Header.ISA07ReceiverQual;
            
            string header = string.Empty;
            DateTime D = DateTime.Now;
            this.SubmittedDate = D;

            header += "ISA" + E + Header.ISA01AuthQual + E + Header.ISA02AuthInfo + E + Header.ISA03SecQual + E + Header.ISA04SecInfo + E + Header.ISA05SenderQual + E + Header.ISA06SenderID.PadRight(15, ' ')
                            + E + Header.ISA07ReceiverQual + E + Header.ISA08ReceiverID.PadRight(15, ' ') + E + D.ToString("yyMMdd") + E + D.ToString("hhmm") + E + R + E + "00501" + E + Header.ISA13CntrlNumber
                            + E + "1" + E + Header.ISA15UsageIndi + E + C + S;
            header += "GS" + E + "HC" + E + Header.GS02SenderID + E + Header.GS03ReceiverID + E + D.ToString("yyyyMMdd") + E + D.ToString("hhmm") + E + Header.ISA13CntrlNumber + E + "X" + E + "005010X222A1" + S;
            header += "ST" + E + "837" + E + "0001" + E + "005010X222A1" + S;
            header += "BHT" + E + "0019" + E + "00" + E + Header.ISA13CntrlNumber + E + D.ToString("yyyyMMdd") + E + D.ToString("hhmm") + E + "CH" + S;
            header += "NM1" + E + "41" + E + Header.SubmitterEntity + E + Header.SubmitterOrgName + E + Header.SubmitterFirstName + E + E + E + E + Header.SubmitterQual + E + Header.SubmitterID + S;
            header += "PER" + E + "IC" + E + Header.SubmitterContactName + E + "TE" + E + Header.SubmitterTelephone;
            if (!Header.SubmitterEmail.IsNull())
                header += E + "EM" + E + Header.SubmitterEmail;
            header += S;

            header += "NM1" + E + "40" + E + "2" + E + Header.ReceiverOrgName + E + E + E + E + E + Header.RecieverQual + E + Header.ReceiverID + S;
           
            return header;
        }

        private string GenerateBillingProvider(ClaimData CLM)
        {
            string bPrv = string.Empty;
            hlCounter += 1; hlBPrvCounter = hlCounter;
            
            bPrv += "HL" + E + hlCounter.ToString() +  E + E + "20" + E + "1" + S;
            if (!CLM.BillPrvTaxonomyCode.IsNull())
                bPrv += "PRV" + E + "BI" + E + "PXC" + E + CLM.BillPrvTaxonomyCode + S;

            bPrv += "NM1" + E + "85" + E + CLM.BillPrvEntityType + E + CLM.BillPrvOrgName;

            if (!CLM.BillPrvNPI.IsNull())
                bPrv += E + CLM.BillPrvFirstName + E + CLM.BillPrvMI + E + E + E + "XX" + E + CLM.BillPrvNPI;
            else if (!CLM.BillPrvFirstName.IsNull())
                bPrv += E + CLM.BillPrvFirstName;

            bPrv += S;

            bPrv += "N3" + E + CLM.BillPrvAddress1 + S;
            bPrv += "N4" + E + CLM.BillPrvCity + E + CLM.BillPrvState + E + CLM.BillPrvZipCode + S;
            if (!CLM.BillPrvTaxID.IsNull())
                bPrv += "REF" + E + "EI" + E + CLM.BillPrvTaxID + S;
            else if (!CLM.BillPrvSSN.IsNull())
                bPrv += "REF" + E + "SY" + E + CLM.BillPrvSSN + S;
               
            if (!CLM.BillPrvTelephone.IsNull())
                bPrv += "PER" + E + "IC" + E + CLM.BillPrvContactName + E + "TE" + E + CLM.BillPrvTelephone + S;

            if (!CLM.BillPrvPayToAddr.IsNull())
            {
                bPrv += "NM1" + E + "87" + E + "2" + S;
                bPrv += "N3" + E + CLM.BillPrvAddress1 + S;
                bPrv += "N4" + E + CLM.BillPrvPayToCity + E + CLM.BillPrvPayToState + E + CLM.BillPrvPayToZip + S;
            }
           
            return bPrv;
        }

        private string GenerateClaim(ClaimData CLM)
        {
            string claim = string.Empty;
            lxCounter = 0;
            
            hlCounter += 1; hlSbrCounter = hlCounter;
            claim += "HL" + E + hlCounter + E + hlBPrvCounter + E + "22" + E + (CLM.PatientRelationShip == "18" ? "0" : "1") + S;
            claim += "SBR" + E + CLM.ClaimType + E + CLM.PatientRelationShip + E + CLM.SbrGroupNumber + E + CLM.SbrGroupName
                           + E + CLM.SbrMedicareSecTypeV + E + E + E + E + CLM.PayerType + S;

            claim += "NM1" + E + "IL" + E + "1" + E + CLM.SBRLastName + E + CLM.SBRFirstName + E + CLM.SBRMiddleInitial + E + E + E + "MI" + E + CLM.SBRID + S;
            claim += "N3" + E + CLM.SBRAddress + S;
            claim += "N4" + E + CLM.SBRCity + E + CLM.SBRState + E + CLM.SBRZipCode + S;

            if (CLM.SBRDob != null)
                claim += "DMG" + E +  "D8" + E + string.Format("{0:yyyyMMdd}", CLM.SBRDob) + E + CLM.SBRGender + S;
            if (!CLM.SBRSSN.IsNull())
                claim += "REF" + E + "SY" + E + CLM.SBRSSN + S;

            if (CLM.PayerName.Length > 60) CLM.PayerName = CLM.PayerName.Substring(0, 60);

            claim += "NM1" + E + "PR" + E + "2" + E + CLM.PayerName + E + E + E + E + E + "PI" + E + CLM.PayerID + S;
            if (!CLM.PayerAddress.IsNull())
            {
                claim += "N3" + E + CLM.PayerAddress + S;
                claim += "N4" + E + CLM.PayerCity + E + CLM.PayerState + E + CLM.PayerZipCode + S;
            }
            if (!CLM.BillPrvSecondaryID.IsNull())
                claim += "REF" + E + "G2" + E + CLM.BillPrvSecondaryID + S;

            if (CLM.PatientRelationShip != "18")
            {
                hlCounter += 1;
                claim += "HL" + E + hlCounter + E + hlSbrCounter + E + "23" + E + "0" + S;
                claim += "PAT" + E + "19" + S;              // change 19 to dynamic = patient relationsip
                claim += "NM1" + E + "QC" + E + "1" + E + CLM.PATLastName + E + CLM.PATFirstName;
                if (!CLM.PATMiddleInitial.IsNull())
                    claim += E + CLM.PATMiddleInitial;
                claim += S;
                claim += "N3" + E + CLM.PATAddress + S;
                claim += "N4" + E + CLM.PATCity + E + CLM.PATState + E + CLM.PATZipCode + S;
                claim += "DMG" + E + "D8" + E + string.Format("{0:yyyyMMdd}", CLM.PATDob) + E + CLM.PATGender + S;
            }

            CLM.ClaimAmount = CLM.Charges.Sum(s => s.ChargeAmount.Value);

            claim += "CLM" + E + CLM.PatientControlNumber + E + CLM.ClaimAmount + E + E + E
                           + CLM.POSCode + C + "B" + C + CLM.ClaimFreqCode + E + "Y" + E + "A" + E + "Y" + E + "Y";
            if (!CLM.AccidentType.IsNull())
            {
                claim += E + E + CLM.AccidentType;
                if (!CLM.AccidentState.IsNull())
                    claim += C + C + C + CLM.AccidentState;
            }
            claim += S;

            //00010101

            if (CLM.CurrentIllnessDate != null)
                claim += "DTP" + E + "431" + E + "D8" + E + string.Format("{0:yyyyMMdd}", CLM.CurrentIllnessDate) + S;
            if (CLM.InitialTreatmentDate != null)
                claim += "DTP" + E + "454" + E + "D8" + E + string.Format("{0:yyyyMMdd}", CLM.InitialTreatmentDate) + S;
            if (CLM.LastSeenDate != null)
                claim += "DTP" + E + "304" + E + "D8" + E + string.Format("{0:yyyyMMdd}", CLM.LastSeenDate) + S;
            if (CLM.AcuteManifestationDate != null)
                claim += "DTP" + E + "453" + E + "D8" + E + string.Format("{0:yyyyMMdd}", CLM.AcuteManifestationDate) + S;
            if (CLM.AccidentDate != null)
                claim += "DTP" + E + "439" + E + "D8" + E + string.Format("{0:yyyyMMdd}", CLM.AccidentDate) + S;
            if (CLM.LMPDate != null)
                claim += "DTP" + E + "484" + E + "D8" + E + string.Format("{0:yyyyMMdd}", CLM.LMPDate) + S;
            if (CLM.XrayDate != null)
                claim += "DTP" + E + "455" + E + "D8" + E + string.Format("{0:yyyyMMdd}", CLM.XrayDate) + S;

            if (CLM.DisabilityStartDate != null && CLM.DisabilityEndDate != null)
                claim += "DTP" + E + "314" + E + "RD8" + E + string.Format("{0:yyyyMMdd}", CLM.DisabilityStartDate) + "-" + string.Format("{0:yyyyMMdd}", CLM.DisabilityEndDate) + S;
            else if (CLM.DisabilityStartDate != null)
                claim += "DTP" + E + "360" + E + "D8" + E + string.Format("{0:yyyyMMdd}", CLM.DisabilityStartDate) + S;
            else if (CLM.DisabilityEndDate != null)
                claim += "DTP" + E + "361" + E + "D8" + E + string.Format("{0:yyyyMMdd}", CLM.DisabilityEndDate) + S;

            if (CLM.LastWorkedDate != null)
                claim += "DTP" + E + "297" + E + "D8" + E + string.Format("{0:yyyyMMdd}", CLM.LastWorkedDate) + S;

            if (CLM.AdmissionDate != null)
                claim += "DTP" + E + "435" + E + "D8" + E + string.Format("{0:yyyyMMdd}", CLM.AdmissionDate) + S;
            if (CLM.DischargeDate != null)
                claim += "DTP" + E + "096" + E + "D8" + E + string.Format("{0:yyyyMMdd}", CLM.DischargeDate) + S;

            if (!CLM.ReferralNumber.IsNull())
                claim += "REF" + E + "9F" + E + CLM.ReferralNumber + S;
            if (!CLM.PriorAuthNumber.IsNull())
                claim += "REF" + E + "G1" + E + CLM.PriorAuthNumber + S;
            if (!string.IsNullOrEmpty(CLM.PayerClaimCntrlNum))
                claim += "REF" + E + "F8" + E + CLM.PayerClaimCntrlNum + S;
            if (!CLM.CliaNumber.IsNull())
                claim += "REF" + E + "X4" + E + CLM.CliaNumber + S;

            claim += "REF" + E + "EA" + E + CLM.PatientControlNumber + S;

            if (!CLM.ClaimNotes.IsNull())
                claim += "NTE" + E + "ADD" + E + CLM.ClaimNotes + S;

            claim += "HI" + E + "ABK" + C + CLM.ICD1Code.Replace(".", "");
            if (!CLM.ICD2Code.IsNull()) claim += E + "ABF" + C + CLM.ICD2Code.Replace(".", "");
            if (!CLM.ICD3Code.IsNull()) claim += E + "ABF" + C + CLM.ICD3Code.Replace(".", "");
            if (!CLM.ICD4Code.IsNull()) claim += E + "ABF" + C + CLM.ICD4Code.Replace(".", "");
            if (!CLM.ICD5Code.IsNull()) claim += E + "ABF" + C + CLM.ICD5Code.Replace(".", "");
            if (!CLM.ICD6Code.IsNull()) claim += E + "ABF" + C + CLM.ICD6Code.Replace(".", "");
            if (!CLM.ICD7Code.IsNull()) claim += E + "ABF" + C + CLM.ICD7Code.Replace(".", "");
            if (!CLM.ICD8Code.IsNull()) claim += E + "ABF" + C + CLM.ICD8Code.Replace(".", "");
            if (!CLM.ICD9Code.IsNull()) claim += E + "ABF" + C + CLM.ICD9Code.Replace(".", "");
            if (!CLM.ICD10Code.IsNull()) claim += E + "ABF" + C + CLM.ICD10Code.Replace(".", "");
            if (!CLM.ICD11Code.IsNull()) claim += E + "ABF" + C + CLM.ICD11Code.Replace(".", "");
            if (!CLM.ICD12Code.IsNull()) claim += E + "ABF" + C + CLM.ICD12Code.Replace(".", "");
            
            claim += S;

            //HI    -   Anesthesia related
            //HCP   -   Claim Pricing/Reprincing info   

            if (!CLM.RefPrvLastName.IsNull())
            {
                claim += "NM1" + E + "DN" + E + "1" + E + CLM.RefPrvLastName + E + CLM.RefPrvFirstName + E + CLM.RefPrvMI + E + E + E + "XX" + E + CLM.RefPrvNPI + S;
            }
            if (!CLM.RendPrvLastName.IsNull())
            {

                claim += "NM1" + E + "82" + E + (string.IsNullOrEmpty(CLM.RendPrvFirstName) ? "2" : "1") + E + CLM.RendPrvLastName;
                if (!CLM.RendPrvNPI.IsNull())
                    claim += E + CLM.RendPrvFirstName + E + CLM.RendPrvMI + E + E + E + "XX" + E + CLM.RendPrvNPI;
                else if (!CLM.RendPrvLastName.IsNull())
                    claim += E + CLM.RendPrvFirstName;
                claim += S;
                
                if (!CLM.RendPrvTaxonomy.IsNull()) claim += "PRV" + E + "PE" + E + "PXC" + E + CLM.RendPrvTaxonomy + S;
                if (!CLM.RendPrvSecondaryID.IsNull()) claim += "REF" + E + "G2" + E + CLM.RendPrvSecondaryID + S;
            }
            if (!CLM.LocationOrgName.IsNull())
            {
                claim += "NM1" + E + "77" + E + "2" + E + CLM.LocationOrgName;
                if (!CLM.LocationNPI.IsNull()) claim += E + E + E + E + E + "XX" + E + CLM.LocationNPI;
                claim += S;
                if (!CLM.LocationAddress.IsNull())
                {
                    claim += "N3" + E + CLM.LocationAddress + S;
                    claim += "N4" + E + CLM.LocationCity + E + CLM.LocationState + E + CLM.LocationZip + S;
                }
            }
            if (!CLM.SuperPrvLastName.IsNull())
            {
                claim += "NM1" + E + "DQ" + E + "1" + E + CLM.SuperPrvLastName + E + CLM.SuperPrvFirstName + E + CLM.SuperPrvMI + E + E + E + "XX" + E + CLM.SuperPrvNPI + S;
            }

            //SECONDARY CLAIM
            if (CLM.ClaimType == "S")
            {
                claim += "SBR" + E + "P" + E + CLM.OtherSBRPatRelationV + E + CLM.OtherSBRGroupNumber + E + CLM.OtherSBRGroupName
                           + E + E + E + E + E + CLM.PayerType + S;

                claim += "AMT" + E + "D" + E + CLM.PrimaryPaidAmt + S;
                claim += "OI" + E + E + E + "Y" + E + E + E + "Y" + S;
                claim += "NM1" + E + "IL" + E + "1" + E + CLM.OtherSBRLastName + E + CLM.OtherSBRFirstName + E + CLM.OtherSBRMI + E + E + E + "MI" + E + CLM.OtherSBRId + S;
                if (!CLM.OtherSBRAddress.IsNull())
                {
                    claim += "N3" + E + CLM.OtherSBRAddress + S;
                    claim += "N4" + E + CLM.OtherSBRCity + E + CLM.OtherSBRState + E + CLM.OtherSBRZipCode + S;
                }
                claim += "NM1" + E + "PR" + E + "2" + E + CLM.OtherPayerName + E + E + E + E + E + "PI" + E + CLM.OtherPayerID + S;
                if (!CLM.OtherPayerAddress.IsNull())
                {
                    claim += "N3" + E + CLM.OtherPayerAddress + S;
                    claim += "N4" + E + CLM.OtherPayerCity + E + CLM.OtherPayerState + E + CLM.OtherPayerZipCode + S;
                }
                claim += "REF" + E + "F8" + E + CLM.PayerClaimCntrlNum + S;
            }
            //
            SubmittedClaims += 1;

            return claim;
        }

        private string GenerateCharge(ChargeData CH, ClaimData CLM)
        {
            string charge = string.Empty;
            lxCounter += 1;
            charge += "LX" + E + lxCounter + S;
            charge += "SV1" + E + "HC" + C + CH.CptCode;

            if (!CH.LIDescription.IsNull())
                charge += C + CH.Modifier1 + C + CH.Modifier2 + C + CH.Modifier3 + C + CH.Modifier4 + C + CH.LIDescription;
            else if (!CH.Modifier4.IsNull())
                charge += C + CH.Modifier1 + C + CH.Modifier2 + C + CH.Modifier3 + C + CH.Modifier4;
            else if (!CH.Modifier3.IsNull())
                charge += C + CH.Modifier1 + C + CH.Modifier2 + C + CH.Modifier3;
            else if (!CH.Modifier2.IsNull())
                charge += C + CH.Modifier1 + C + CH.Modifier2;
            else if (!CH.Modifier1.IsNull())
                charge += C + CH.Modifier1;

            //SV1*HC:98941*101*UN*1***1:2:3:4~

            charge += E + CH.ChargeAmount + E;
            if (!CH.Units.IsNull()) charge += "UN" + E + CH.Units;
           else if (!CH.Minutes.IsNull()) charge += "MJ" + E + CH.Minutes;

            charge += E + CH.POS + E + E + CH.Pointer1;
            if (!CH.Pointer2.IsNull()) charge += C + CH.Pointer2;
            if (!CH.Pointer3.IsNull()) charge += C + CH.Pointer3;
            if (!CH.Pointer4.IsNull()) charge += C + CH.Pointer4;

            if(CH.IsEmergency)
                charge += E + E + "Y";

            charge += S;

            if (CH.DateofServiceFrom != null && CH.DateOfServiceTo != null &&  CH.DateofServiceFrom != CH.DateOfServiceTo)
                charge += "DTP" + E + "472" + E + "RD8" + E + string.Format("{0:yyyyMMdd}", CH.DateofServiceFrom) + "-" + string.Format("{0:yyyyMMdd}", CH.DateOfServiceTo) + S;
            else
                charge += "DTP" + E + "472" + E + "D8" + E + string.Format("{0:yyyyMMdd}", CH.DateofServiceFrom) + S;

            charge += "REF" + E + "6R" + E + CH.LineItemControlNum + S;
            if (!CH.ServiceLineNotes.IsNull()) charge += "NTE" + E + "ADD" + E + CH.ServiceLineNotes + S;

            if (!CH.DrugNumber.IsNull()) charge += "LIN" + E + E + "N4" + E + CH.DrugNumber + S;
            if (!CH.DrugCount.IsNull()) charge += "CTP" + E + E + E + E + CH.DrugCount + E + CH.DrugUnit + S;


            if (CLM.ClaimType == "S")
            {
                charge += "SVD" + E + CLM.OtherPayerID  + E + CH.PrimaryPaidAmt + E + "HC" + C + CH.PrimaryCPT;

                if (!CH.PrimaryMod4.IsNull())
                    charge += C + CH.PrimaryMod1 + C + CH.PrimaryMod2 + C + CH.PrimaryMod3 + C + CH.PrimaryMod4;
                else if (!CH.PrimaryMod3.IsNull())
                    charge += C + CH.PrimaryMod1 + C + CH.PrimaryMod2 + C + CH.PrimaryMod3;
                else if (!CH.PrimaryMod2.IsNull())
                    charge += C + CH.PrimaryMod1 + C + CH.PrimaryMod2;
                else if (!CH.PrimaryMod1.IsNull())
                    charge += C + CH.PrimaryMod1;

                charge += E + E + CH.PrimaryUnits + S;

                if (CH.PrimaryWriteOffAmt.Amt() != 0)
                    charge += "CAS" + E + "CO" + E +  "45" + E + CH.PrimaryWriteOffAmt + S;

                if ( CH.PrimaryDeductable.Amt() != 0 && CH.PrimaryCoIns.Amt() != 0)
                    charge += "CAS" + E + "PR" + E + "1" + E + CH.PrimaryDeductable + E + "1" + E + "2" + E + CH.PrimaryCoIns + "1" +  S;
                else if (CH.PrimaryDeductable.Amt() != 0)
                    charge += "CAS" + E + "PR" + E + "1" + E + CH.PrimaryDeductable + E + "1" + S;
                else if (CH.PrimaryCoIns.Amt() != 0)
                    charge += "CAS" + E + "PR" + E + "2" + E + CH.PrimaryCoIns + E + "1" + S;

                if (CH.PrimaryPaidDate != null)
                    charge += "DTP" + E + "573" + E + "D8" + E + string.Format("{0:yyyyMMdd}", CH.PrimaryPaidDate) + S;
            }

            CH.Submitted = true;
            CH.SubmittedDate = SubmittedDate;

            CLM.Submitted = true;
            CLM.SubmittedDate = SubmittedDate;

            return charge;
        }

        private string GenerateTrailer(ClaimHeader Header)
        {
            string trailer = string.Empty;
            trailer += "SE" + E + (count + 1).ToString() + E + "0001" + S;
            trailer += "GE" + E + "1" + E + Header.ISA13CntrlNumber + S;
            trailer += "IEA" + E + "1" + E + Header.ISA13CntrlNumber + S;
            return trailer;
        }


        private string Validate(ClaimHeader Header, ClaimData CLM)
        {
            string msg = string.Empty;

            if (CLM.SBRLastName.IsNull() || CLM.SBRFirstName.IsNull())
                msg = "Subscriber Insured Name is requried";
            else if (CLM.SBRAddress.IsNull() || CLM.SBRCity.IsNull() || CLM.SBRState.IsNull() || CLM.SBRZipCode.IsNull())
                msg = "Subscriber Address, City, State and Zip Code are required";
            else if (CLM.SBRID.IsNull())
                msg = "Subscriber ID is required";
            else if (CLM.SBRGender.IsNull())
                msg = "Subscriber Gender is required";
            else if (CLM.SBRDob == null)
                msg = "Subscriber DOB is required";
            else if (CLM.ClaimType.IsNull())
                msg = "Claim Type is required";
            else if (CLM.PayerType.IsNull())
                msg = "Payer Type is required";
            else if (CLM.PayerType.Length != 2)
                msg = "Payer Type should be on 2 characters";
            else if (CLM.PatientRelationShip.IsNull())
                msg = "Patient Relationship is required";
            else if (CLM.PayerName.IsNull())
                msg = "Payer Name is required";
            else if (CLM.PayerID.IsNull())
                msg = "Payer ID is required";
            else if (CLM.BillPrvOrgName.IsNull())
                msg = "Billing Provider Name is required";
            else if (!Header.RelaxNpiValidation && CLM.BillPrvNPI.IsNull())
                msg = "Billing Provider NPI is required";
            else if (CLM.BillPrvAddress1.IsNull() || CLM.BillPrvCity.IsNull() || CLM.BillPrvState.IsNull() || CLM.BillPrvZipCode.IsNull())
                msg = "Billing Provider Address, City, State and Zip Code are required";
            else if (CLM.BillPrvTaxID.IsNull())
                msg = "Billing Provider Tax ID is required";
            //else if (CLM.BillPrvEntityType == "1" && CLM.BillPrvSSN.IsNull())
            //    msg = "Billing Provider SSN is required";
            else if (CLM.PatientControlNumber.IsNull())
                msg = "Patient Control Number is required";
            //else if (CLM.ClaimAmount.Amt() <= 0)
            //    msg = "Claim Amount is rqeuired";
            else if (CLM.ICD1Code.IsNull())
                msg = "ICD Code 1 is required";
            else if (CLM.POSCode.IsNull())
                msg = "Place of Service is required";
            else if (CLM.POSCode.Length != 2)
                msg = "Payer Type should be of 2 characters";
            else if (CLM.PatientRelationShip != "18")
            {
                if (string.IsNullOrWhiteSpace(CLM.PATLastName) || string.IsNullOrWhiteSpace(CLM.PATFirstName))
                    msg = "Patient Name is requried";
                else if (string.IsNullOrWhiteSpace(CLM.PATAddress) || string.IsNullOrWhiteSpace(CLM.PATCity) || string.IsNullOrWhiteSpace(CLM.SBRState)
                || string.IsNullOrWhiteSpace(CLM.PATZipCode))
                    msg = "Patient Address, City, State and Zip Code are required";
            }
            else if (!CLM.PrescribingMD.IsNull() && CLM.RefPrvNPI.IsNull())
                msg = "Referring Provider is requred when Prescribing MD is available";

            if (CLM.Charges == null || CLM.Charges.Count == 0)
                msg = "Service Lines not found";

            if (!msg.IsNull())
                return msg;


            if (CLM.SecondaryStatus == "N" || CLM.SecondaryStatus == "RS")
            {
                if (CLM.PrimaryPaidAmt == null) msg = "Primary Paid Amount is requrired";
                else if (CLM.OtherSBRLastName.IsNull() || CLM.OtherSBRFirstName.IsNull())
                    msg = "Primary Subscriber Insured Name is requried";
                else if (CLM.OtherSBRId.IsNull())
                    msg = "Primary Subscriber ID is requried";
                else if (CLM.OtherPayerName.IsNull() || CLM.OtherPayerID.IsNull())
                    msg = "Primary Payer Name, Payer ID is requried";
                else if (CLM.PayerClaimCntrlNum.IsNull())
                    msg = "Payer Claim Control Number is required";
            }


            if (!msg.IsNull())
                return msg;

            foreach (ChargeData CH in CLM.Charges)
            {
                if (CH.Units.EndsWith(".0"))
                {
                    CH.Units = CH.Units.Replace(".0", "");
                }
                
                if (CH.CptCode.IsNull())
                    msg = "CPT Code is required";
                else if (CH.ChargeAmount.Amt() <= 0)
                    msg = "Service Line Amount is required";
                else if (CH.Units.IsNull())
                    msg = "Service Line Units are required";
                else if (!CH.Units.All(char.IsNumber))
                    msg = "Service Line Units should be numeric";
                else if (CH.Pointer1.IsNull())
                    msg = "Service Line Pointer 1 is required";
                else if (CH.DateofServiceFrom.IsNull())
                    msg = "Service Line Date Of Service is required";
                else if (CH.LineItemControlNum.IsNull())
                    msg = "Service Line Control Number is required";
                else if( CH.Modifier1 != "QW" && (CH.CptCode.StartsWith("7") || CH.CptCode.StartsWith("8")) && CLM.CliaNumber.IsNull())
                    msg = "CLIA Number is required";


                if (CLM.SecondaryStatus == "N" || CLM.SecondaryStatus == "RS")
                {
                    if (CH.PrimaryCPT.IsNull()) msg = "Primary CPT is requrired";
                    else if (CH.PrimaryPaidAmt == null )
                        msg = "Primary Paid Amount is requried";
                }
            }


            return msg;
        }

        #endregion


        public class Output
        {
            public string Transaction837 { get; set; }
            public string ISAControlNumber { get; set; }
            public string ErrorMessage { get; set; }

            public long ProcessedClaims { get; set; }
            public decimal ClaimAmount { get; set; }
            public List<ClaimResult> Claims { get; set; }

            public bool FileSubmittedToFTP { get; set; }
            public string FTPFileName { get; set; }

            public string Transaction837Path { get; set; }
           

            public class ClaimResult
            {
                public string ValidationMsg { get; set; }
                public string PatientControlNumber { get; set; }
                public long VisitID { get; set; }
                public bool Submitted { get; set; }
                public DateTime SubmittedDate { get; set; }
                
            }
        }

    }
}

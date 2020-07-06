using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MediFusionPM.BusinessLogic._277CA
{
    public class ClaimAckParser
    {
        int _counter = 0;
        char E = ' ', C = ' ', S = ' ';
        string[] elements = null;
        string[] segments = null;
        string filepath = string.Empty;

        List<ClaimAckHeader> ClmAckData = new List<ClaimAckHeader>();

        string ISA06, ISA08, ISAControlNum, GS02, GS03, GSConrolNum, Version;
        DateTime? ISADate;
        public List<ClaimAckHeader> Parse277CAFile(string FilePath)
        {
            filepath = FilePath;

            ClmAckData = new List<ClaimAckHeader>();
            _counter = 0;
            
            if (!File.Exists(FilePath)) throw new Exception(string.Format("File {0} not found.", FilePath));
            string contents = File.ReadAllText(FilePath);
            if (!contents.StartsWith("ISA") && contents.Length < 200) throw new Exception(string.Format("Innvalid File {0}", FilePath));

            E = char.Parse(contents.Substring(3, 1));
            C = char.Parse(contents.Substring(104, 1));
            S = char.Parse(contents.Substring(105, 1));

            segments = contents.Split(S);

            while (segments.Length != _counter + 1)
            {
                elements = segments[_counter].Split(E);
                switch (elements.GetElement(0).Trim())
                {
                    case "ISA":
                        ISA06 = elements.GetElement(6); 
                        ISA08 = elements.GetElement(8);
                        ISADate = Utilities.GetDate(elements.GetElement(9), elements.GetElement(10));
                        ISAControlNum = elements.GetElement(13);
                        break;

                    case "GS":
                        GS02 = elements.GetElement(2);
                        GS03 = elements.GetElement(3);
                        if (elements.Length >= 9) Version = elements.GetElement(8);
                        GSConrolNum = elements.GetElement(6);
                        break;

                    case "ST":
                        ParseST();
                        break;

                    case "SE":
                    case "GE":
                    case "IEA":
                        break;

                    default:
                        break;
                }
                _counter += 1;
            }

            return ClmAckData;
        }

        private void ParseST()
        {

            Dictionary<string, string> lstClaimStatusDesc = GetClaimStatus();
            Dictionary<string, string> lstClaimStatusCodeDesc = GetStatusCodeDesciption();
            Dictionary<string, string> lstClaimCategoryCodeDesc = GetCategoryCodeDesciption();
            Dictionary<string, string> lstEntityCodeDesc = GetEntityDescription();

            string value = string.Empty;


            ClaimAckHeader caHeader = new ClaimAckHeader();
            caHeader.ClaimAckVisits = new List<ClaimAckVisit>();
            caHeader.FilePath = filepath;
            //caHeader.FileContents = File.ReadAllText(filepath);
            
            caHeader.ISA06SenderID = this.ISA06;
            caHeader.ISA08ReceiverID = this.ISA08;
            caHeader.ISADateTime = this.ISADate;
            caHeader.ISAControlNumber = this.ISAControlNum;
            caHeader.GS02SenderID = this.GS02;
            caHeader.GS03ReceiverID = this.GS03;
            caHeader.GSControlNumber = this.GSConrolNum;
            caHeader.VersionNumber = this.Version;


            if (elements.GetElement(1) != "277") throw new Exception("Invalid File.");
            caHeader.STControlNumber = elements.GetElement(2);
            _counter += 1;
            bool stCondition = true;
            while (stCondition)
            {
                elements = segments[_counter].Split(E);
                switch (elements.GetElement(0).Trim())
                {
                    case "HL":

                        if (elements.GetElement(3) == "20")
                        {
                            _counter += 1;
                            bool hlPayer = true;
                            while (hlPayer)
                            {
                                elements = segments[_counter].Split(E);
                                switch (elements.GetElement(0).Trim())
                                {
                                    case "NM1":
                                        caHeader.PayerEntity = elements.GetElement(1);
                                        caHeader.PayerOrgName = elements.GetElement(3);
                                        if (elements.Length > 8) caHeader.PayerID = elements.GetElement(9);
                                        break;

                                    case "N3":
                                        caHeader.PayerAddress = elements.GetElement(1);
                                        break;
                                    case "N4":
                                        caHeader.PayerCity = elements.GetElement(1);
                                        caHeader.PayerState = elements.GetElement(2);
                                        caHeader.PayerZip = elements.GetElement(3);
                                        break;

                                    case "TRN":
                                        caHeader.PayerTRN = elements.GetElement(2);
                                        break;

                                    case "DTP":
                                        if (elements.GetElement(1) == "050")
                                            caHeader.PayerRecepitDate_050 = Utilities.GetDate(elements.GetElement(3), string.Empty);
                                        else if (elements.GetElement(1) == "009")
                                            caHeader.PayerProcessDate_009 = Utilities.GetDate(elements.GetElement(3), string.Empty);
                                        break;

                                    case "HL":
                                        hlPayer = false; _counter -= 1;
                                        break;
                                }
                                if (hlPayer) _counter += 1;
                            }
                        }
                        else if (elements.GetElement(3) == "21")
                        {
                            _counter += 1;
                            bool hlSubmitter = true;
                            while (hlSubmitter)
                            {
                                elements = segments[_counter].Split(E);
                                switch (elements.GetElement(0).Trim())
                                {
                                    case "NM1":
                                        caHeader.SubmitterOrgName = elements.GetElement(3);
                                        caHeader.SubmitterEntityCode = elements.GetElement(2);
                                        if (elements.Length > 8) caHeader.SubmitterEdiID = elements.GetElement(9);
                                        break;

                                    case "TRN":
                                        caHeader.SubmitterTRN = elements.GetElement(2);
                                        break;

                                    case "STC":
                                        string[] stc01 = elements.GetElement(1).Split(C);
                                        if (stc01.Length > 0) caHeader.SubmitterCategoryCode = stc01.GetElement(0);
                                        if (stc01.Length > 1) caHeader.SubmitterStatusCode = stc01.GetElement(1);
                                        if (stc01.Length > 2) caHeader.SubmitterEntityCode = stc01.GetElement(2);

                                        if (!string.IsNullOrEmpty(elements.GetElement(2)))
                                            caHeader.SubmitterStatusDate = Utilities.GetDate(elements.GetElement(2), string.Empty);
                                        if (!string.IsNullOrEmpty(elements.GetElement(3)))
                                            caHeader.SubmitterActionCode = elements.GetElement(3);
                                        if (!string.IsNullOrEmpty(elements.GetElement(4)))
                                            caHeader.SubmitterClaimsAmt = decimal.Parse(elements.GetElement(4));
                                        break;

                                    case "QTY":
                                        if (elements.GetElement(1) == "90") caHeader.SubmitterAcceptedClaims = long.Parse(elements.GetElement(2));
                                        else if (elements.GetElement(1) == "AA") caHeader.SubmitterRejectedClaims = long.Parse(elements.GetElement(2));
                                        break;

                                    case "AMT":
                                        if (elements.GetElement(1) == "YU") caHeader.SubmitterAcceptedAmt = decimal.Parse(elements.GetElement(2));
                                        else if (elements.GetElement(1) == "YY") caHeader.SubmitterRejectedAmt = decimal.Parse(elements.GetElement(2));
                                        break;

                                    case "HL":
                                        hlSubmitter = false; _counter -= 1;
                                        break;
                                }
                                if (hlSubmitter) _counter += 1;
                            }
                        }
                        else if (elements.GetElement(3) == "19")
                        {
                            ClaimAckVisit caProv = new ClaimAckVisit();

                            _counter += 1;
                            bool hlProvider = true;
                            while (hlProvider)
                            {
                                elements = segments[_counter].Split(E);
                                switch (elements.GetElement(0).Trim())
                                {

                                    #region Provider

                                    case "NM1":
                                        caProv.BillProvOrgName = elements.GetElement(3);
                                        caProv.BillProvEntityCode = elements.GetElement(2);
                                        if (elements.Length > 8) caProv.BillProvNPI = elements.GetElement(9);
                                        break;

                                    case "TRN":
                                        caProv.BillProvTRN = elements.GetElement(2);
                                        break;

                                    case "STC":
                                        string[] stc01 = elements.GetElement(1).Split(C);
                                        if (stc01.Length > 0) caProv.BillProvCategoryCode = stc01.GetElement(0);
                                        if (stc01.Length > 1) caProv.BillProvStatusCode = stc01.GetElement(1);
                                        if (stc01.Length > 2) caProv.BillProvEntityCode = stc01.GetElement(2);

                                        if (!string.IsNullOrEmpty(elements.GetElement(2)))
                                            caProv.BillProvStatusDate = Utilities.GetDate(elements.GetElement(2), string.Empty);
                                        if (!string.IsNullOrEmpty(elements.GetElement(3)))
                                            caProv.BillProvActionCode = elements.GetElement(3);
                                        if (!string.IsNullOrEmpty(elements.GetElement(4)))
                                            caProv.BillProvClaimsAmt = decimal.Parse(elements.GetElement(4));
                                        break;

                                    case "QTY":
                                        if (elements.GetElement(1) == "QA") caProv.BillProvAcceptedClaims = long.Parse(elements.GetElement(2));
                                        else if (elements.GetElement(1) == "QC") caProv.BillProvRejectedClaims = long.Parse(elements.GetElement(2));
                                        break;

                                    case "AMT":
                                        if (elements.GetElement(1) == "YU") caProv.BillProvAcceptedAmt = decimal.Parse(elements.GetElement(2));
                                        else if (elements.GetElement(1) == "YY") caProv.BillProvRejectedAmt = decimal.Parse(elements.GetElement(2));
                                        break;

                                    #endregion


                                    case "HL":
                                        if (elements.GetElement(3) == "PT" || elements.GetElement(3) == "23")
                                        {
                                            ClaimAckVisit caVisit = new ClaimAckVisit();
                                            caVisit.ClaimAckCharges = new List<ClaimAckCharge>();

                                            caVisit.BillProvOrgName = caProv.BillProvOrgName;
                                            caVisit.BillProvEntityCode = caProv.BillProvEntityCode;
                                            caVisit.BillProvNPI = caProv.BillProvNPI;
                                            caVisit.BillProvTRN = caProv.BillProvTRN;

                                            caVisit.BillProvCategoryCode = caProv.BillProvCategoryCode;
                                            caVisit.BillProvStatusCode = caProv.BillProvStatusCode;
                                            caVisit.BillProvEntityCode = caProv.BillProvEntityCode;
                                            caVisit.BillProvClaimsAmt = caProv.BillProvClaimsAmt;

                                            caVisit.BillProvAcceptedClaims = caProv.BillProvAcceptedClaims;
                                            caVisit.BillProvRejectedClaims = caProv.BillProvRejectedClaims;

                                            caVisit.BillProvAcceptedAmt = caProv.BillProvAcceptedAmt;
                                            caVisit.BillProvRejectedAmt = caProv.BillProvRejectedAmt;

                                            _counter += 1;
                                            bool hlVisit = true;
                                            int stcCounter = 0;
                                            while (hlVisit)
                                            {
                                                elements = segments[_counter].Split(E);
                                                switch (elements.GetElement(0).Trim())
                                                {
                                                    case "NM1":
                                                        caVisit.PatientLastName = elements.GetElement(3);
                                                        caVisit.PatientFirstName = elements.GetElement(4);
                                                        if (elements.Length > 8) caProv.PatientSubscriberID = elements.GetElement(9);
                                                        break;

                                                    case "TRN":
                                                        caVisit.VisitTRN = elements.GetElement(2);
                                                        break;

                                                    case "STC":
                                                        stcCounter += 1;

                                                        stc01 = elements.GetElement(1).Split(C);

                                                        if (stcCounter == 1)
                                                        {
                                                            if (stc01.Length > 0) caVisit.VisitCategoryCode1 = stc01.GetElement(0);
                                                            if (stc01.Length > 1) caVisit.VisitStatusCode1 = stc01.GetElement(1);
                                                            if (stc01.Length > 2) caVisit.VisitEntityCode1 = stc01.GetElement(2);


                                                            if (!string.IsNullOrWhiteSpace(caVisit.VisitCategoryCode1))
                                                            {
                                                                lstClaimStatusDesc.TryGetValue(caVisit.VisitCategoryCode1, out value);
                                                                caVisit.Status = value;

                                                                lstClaimCategoryCodeDesc.TryGetValue(caVisit.VisitCategoryCode1, out value);
                                                                caVisit.VisitCategoryCodeDesc1 = value;
                                                            }

                                                            if (!string.IsNullOrWhiteSpace(caVisit.VisitStatusCode1))
                                                            {
                                                                lstClaimStatusCodeDesc.TryGetValue(caVisit.VisitStatusCode1, out value);
                                                                caVisit.VisitStatusCodeDesc1 = value;
                                                            }

                                                            if (!string.IsNullOrWhiteSpace(caVisit.VisitEntityCode1))
                                                            {
                                                                lstEntityCodeDesc.TryGetValue(caVisit.VisitEntityCode1, out value);
                                                                caVisit.VisitEntityCodeDesc1 = value;
                                                            }
                                                            
                                                            if (!string.IsNullOrEmpty(elements.GetElement(2)))
                                                                caVisit.VisitStatusDate1 = Utilities.GetDate(elements.GetElement(2), string.Empty);
                                                            if (!string.IsNullOrEmpty(elements.GetElement(3)))
                                                                caVisit.VisitActionCode1 = elements.GetElement(3);
                                                            if (!string.IsNullOrEmpty(elements.GetElement(4)))
                                                                caVisit.VisitClaimAmt1 = decimal.Parse(elements.GetElement(4));

                                                            if (elements.Length > 12)
                                                                caVisit.VisitRejection1 = elements.GetElement(12);
                                                        }
                                                        else if (stcCounter == 2)
                                                        {
                                                            if (stc01.Length > 0) caVisit.VisitCategoryCode2 = stc01.GetElement(0);
                                                            if (stc01.Length > 1) caVisit.VisitStatusCode2 = stc01.GetElement(1);
                                                            if (stc01.Length > 2) caVisit.VisitEntityCode2 = stc01.GetElement(2);

                                                            if (!string.IsNullOrWhiteSpace(caVisit.VisitCategoryCode2))
                                                            {
                                                                lstClaimCategoryCodeDesc.TryGetValue(caVisit.VisitCategoryCode2, out value);
                                                                caVisit.VisitCategoryCodeDesc2 = value;
                                                            }

                                                            if (!string.IsNullOrWhiteSpace(caVisit.VisitStatusCode2))
                                                            {
                                                                lstClaimStatusCodeDesc.TryGetValue(caVisit.VisitStatusCode2, out value);
                                                                caVisit.VisitStatusCodeDesc2 = value;
                                                            }

                                                            if (!string.IsNullOrWhiteSpace(caVisit.VisitEntityCode2))
                                                            {
                                                                lstEntityCodeDesc.TryGetValue(caVisit.VisitEntityCode2, out value);
                                                                caVisit.VisitEntityCodeDesc2 = value;
                                                            }

                                                            if (!string.IsNullOrEmpty(elements.GetElement(2)))
                                                                caVisit.VisitStatusDate2 = Utilities.GetDate(elements.GetElement(2), string.Empty);
                                                            if (!string.IsNullOrEmpty(elements.GetElement(3)))
                                                                caVisit.VisitActionCode2 = elements.GetElement(3);
                                                            if (!string.IsNullOrEmpty(elements.GetElement(4)))
                                                                caVisit.VisitClaimAmt2 = decimal.Parse(elements.GetElement(4));

                                                            if (elements.Length > 12)
                                                                caVisit.VisitRejection2 = elements.GetElement(12);
                                                        }
                                                        else if (stcCounter == 3)
                                                        {
                                                            if (stc01.Length > 0) caVisit.VisitCategoryCode3 = stc01.GetElement(0);
                                                            if (stc01.Length > 1) caVisit.VisitStatusCode3 = stc01.GetElement(1);
                                                            if (stc01.Length > 2) caVisit.VisitEntityCode3 = stc01.GetElement(2);

                                                            if (!string.IsNullOrWhiteSpace(caVisit.VisitCategoryCode3))
                                                            {
                                                                lstClaimCategoryCodeDesc.TryGetValue(caVisit.VisitCategoryCode3, out value);
                                                                caVisit.VisitCategoryCodeDesc3 = value;
                                                            }

                                                            if (!string.IsNullOrWhiteSpace(caVisit.VisitStatusCode3))
                                                            {
                                                                lstClaimStatusCodeDesc.TryGetValue(caVisit.VisitStatusCode3, out value);
                                                                caVisit.VisitStatusCodeDesc3 = value;
                                                            }

                                                            if (!string.IsNullOrWhiteSpace(caVisit.VisitEntityCode3))
                                                            {
                                                                lstEntityCodeDesc.TryGetValue(caVisit.VisitEntityCode3, out value);
                                                                caVisit.VisitEntityCodeDesc3 = value;
                                                            }

                                                            if (!string.IsNullOrEmpty(elements.GetElement(2)))
                                                                caVisit.VisitStatusDate3 = Utilities.GetDate(elements.GetElement(2), string.Empty);
                                                            if (!string.IsNullOrEmpty(elements.GetElement(3)))
                                                                caVisit.VisitActionCode3 = elements.GetElement(3);
                                                            if (!string.IsNullOrEmpty(elements.GetElement(4)))
                                                                caVisit.VisitClaimAmt3 = decimal.Parse(elements.GetElement(4));

                                                            if (elements.Length > 12)
                                                                caVisit.VisitRejection3 = elements.GetElement(12);
                                                        }
                                                        break;

                                                    case "REF":
                                                        if (elements.GetElement(1) == "1K")
                                                            caVisit.PayerClaimControlNum = elements.GetElement(2);
                                                        else if (elements.GetElement(1) == "EA")
                                                            caVisit.VisitMedicalRecordNum_EA = elements.GetElement(2);
                                                        else if (elements.GetElement(1) == "D9")
                                                            caVisit.VisitClearingHouseNum_D9 = elements.GetElement(2);
                                                        break;

                                                    case "DTP":
                                                        if (elements.GetElement(1) == "472")
                                                        {
                                                            string[] dt = elements.GetElement(3).Split('-');
                                                            if (dt.Length > 1)
                                                            {
                                                                caVisit.VisitDateFrom = Utilities.GetDate(dt.GetElement(0), string.Empty);
                                                                caVisit.VisitDateTo = Utilities.GetDate(dt.GetElement(1), string.Empty);
                                                            }
                                                            else caVisit.VisitDateFrom = Utilities.GetDate(dt.GetElement(0), string.Empty);
                                                        }
                                                        break;

                                                    case "SVC":

                                                        #region Charge

                                                        ClaimAckCharge caCharge = new ClaimAckCharge();

                                                        string[] sv1 = elements.GetElement(1).Split(C);
                                                        caCharge.CPT = sv1.GetElement(1);
                                                        if (!string.IsNullOrEmpty(elements.GetElement(2)))
                                                            caCharge.ChargeAmt = decimal.Parse(elements.GetElement(2));

                                                        bool svcCondition = true;
                                                        _counter += 1;
                                                        int chargeStcCounter = 0;
                                                        while (svcCondition)
                                                        {
                                                            elements = segments[_counter].Split(E);
                                                            switch (elements.GetElement(0).Trim())
                                                            {
                                                                case "STC":
                                                                    chargeStcCounter += 1;

                                                                    stc01 = elements.GetElement(1).Split(C);

                                                                    if (chargeStcCounter == 1)
                                                                    {
                                                                        if (stc01.Length > 0) caCharge.ChargeCategoryCode1 = stc01.GetElement(0);
                                                                        if (stc01.Length > 1) caCharge.ChargeStatusCode1 = stc01.GetElement(1);
                                                                        if (stc01.Length > 2) caCharge.ChargeEntityCode1 = stc01.GetElement(2);

                                                                        if (!string.IsNullOrEmpty(elements.GetElement(2)))
                                                                            caCharge.ChargeStatusDate1 = Utilities.GetDate(elements.GetElement(2), string.Empty);
                                                                        if (!string.IsNullOrEmpty(elements.GetElement(3)))
                                                                            caCharge.ChargeActionCode1 = elements.GetElement(3);
                                                                        if (!string.IsNullOrEmpty(elements.GetElement(4)))
                                                                            caCharge.ChargeClaimAmt1 = decimal.Parse(elements.GetElement(4));

                                                                        if (elements.Length > 12)
                                                                            caCharge.ChargeRejection1 = elements.GetElement(12);
                                                                    }
                                                                    else if (chargeStcCounter == 2)
                                                                    {
                                                                        if (stc01.Length > 0) caCharge.ChargeCategoryCode2 = stc01.GetElement(0);
                                                                        if (stc01.Length > 1) caCharge.ChargeStatusCode2 = stc01.GetElement(1);
                                                                        if (stc01.Length > 2) caCharge.ChargeEntityCode2 = stc01.GetElement(2);

                                                                        if (!string.IsNullOrEmpty(elements.GetElement(2)))
                                                                            caCharge.ChargeStatusDate2 = Utilities.GetDate(elements.GetElement(2), string.Empty);
                                                                        if (!string.IsNullOrEmpty(elements.GetElement(3)))
                                                                            caCharge.ChargeActionCode2 = elements.GetElement(3);
                                                                        if (!string.IsNullOrEmpty(elements.GetElement(4)))
                                                                            caCharge.ChargeClaimAmt2 = decimal.Parse(elements.GetElement(4));

                                                                        if (elements.Length > 12)
                                                                            caCharge.ChargeRejection2 = elements.GetElement(12);
                                                                    }
                                                                    else if (chargeStcCounter == 3)
                                                                    {
                                                                        if (stc01.Length > 0) caCharge.ChargeCategoryCode3 = stc01.GetElement(0);
                                                                        if (stc01.Length > 1) caCharge.ChargeStatusCode3 = stc01.GetElement(1);
                                                                        if (stc01.Length > 2) caCharge.ChargeEntityCode3 = stc01.GetElement(2);

                                                                        if (!string.IsNullOrEmpty(elements.GetElement(2)))
                                                                            caCharge.ChargeStatusDate3 = Utilities.GetDate(elements.GetElement(2), string.Empty);
                                                                        if (!string.IsNullOrEmpty(elements.GetElement(3)))
                                                                            caCharge.ChargeActionCode3 = elements.GetElement(3);
                                                                        if (!string.IsNullOrEmpty(elements.GetElement(4)))
                                                                            caCharge.ChargeClaimAmt3 = decimal.Parse(elements.GetElement(4));

                                                                        if (elements.Length > 12)
                                                                            caCharge.ChargeRejection3 = elements.GetElement(12);
                                                                    }
                                                                    break;

                                                                case "REF":
                                                                    if (elements.GetElement(1) == "FJ")
                                                                        caCharge.ChargeControlNumber = elements.GetElement(2);
                                                                    break;

                                                                case "DTP":
                                                                    if (elements.GetElement(1) == "472")
                                                                    {
                                                                        string[] dt = elements.GetElement(3).Split('-');
                                                                        if (dt.Length > 1)
                                                                        {
                                                                            caCharge.ServiceDateFrom = Utilities.GetDate(dt.GetElement(0), string.Empty);
                                                                            caCharge.ServiceDateTo = Utilities.GetDate(dt.GetElement(1), string.Empty);
                                                                        }
                                                                        else
                                                                            caCharge.ServiceDateFrom = Utilities.GetDate(dt.GetElement(0), string.Empty);
                                                                    }
                                                                    break;


                                                                case "SVC":
                                                                case "HL":
                                                                case "SE":
                                                                    if (!caVisit.ClaimAckCharges.Contains(caCharge))
                                                                        caVisit.ClaimAckCharges.Add(caCharge);
                                                                    svcCondition = false;
                                                                    _counter -= 1;
                                                                    break;
                                                            }

                                                            if (svcCondition) _counter += 1;
                                                        }
                                                        break;

                                                        #endregion


                                                    case "HL":
                                                    case "SE":
                                                        if (!caHeader.ClaimAckVisits.Contains(caVisit)) caHeader.ClaimAckVisits.Add(caVisit);
                                                        _counter -= 1;
                                                        hlVisit = false;
                                                        break;
                                                }
                                                if (hlVisit) _counter += 1;
                                            }

                                        }
                                        break;

                                    case "SE":
                                    case "GE":
                                        hlProvider = false; 
                                        _counter -= 1;
                                        break;
                                }
                                if (hlProvider) _counter += 1;
                            }
                            if (hlProvider) _counter += 1;
                        }

                        break;

                    case "SE":
                    case "GE":
                    case "IEA":
                        _counter -= 1; stCondition = false;
                        if (!ClmAckData.Contains(caHeader)) ClmAckData.Add(caHeader);
                        break;
                }
                if (stCondition) _counter += 1;
            }
        }





        private Dictionary<string, string> GetClaimStatus()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            list.Add("A0", "Acknowledged");
            list.Add("A1", "Acknowledged");
            list.Add("A2", "Acknowledged");
            list.Add("A3", "Rejected");
            list.Add("A4", "Not Found");
            list.Add("A5", "Acknowledged");
            list.Add("A6", "Rejected");
            list.Add("A7", "Rejected");
            list.Add("A8", "Rejected");
            list.Add("P0", "Pended");
            list.Add("P1", "Pended");
            list.Add("P2", "Pended");
            list.Add("P3", "Pended");
            list.Add("P4", "Pended");
            list.Add("P5", "Pended");
            list.Add("F0", "Adjudicated");
            list.Add("F1", "Paid");
            list.Add("F2", "Denied");
            list.Add("F3", "Adjudicated");
            list.Add("F3F", "Adjudicated");
            list.Add("F3N", "Adjudicated");
            list.Add("F4", "Adjudicated");
            list.Add("F5", "Adjudicated");
            list.Add("R0", "Additional Info Requested");
            list.Add("R1", "Additional Info Requested");
            list.Add("R3", "Additional Info Requested");
            list.Add("R4", "Additional Info Requested");
            list.Add("R5", "Additional Info Requested");
            list.Add("R6", "Additional Info Requested");
            list.Add("R7", "Additional Info Requested");
            list.Add("R8", "Additional Info Requested");
            list.Add("R9", "Additional Info Requested");
            list.Add("R10", "Additional Info Requested");
            list.Add("R11", "Additional Info Requested");
            list.Add("R12", "Additional Info Requested");
            list.Add("R13", "Additional Info Requested");
            list.Add("R14", "Additional Info Requested");
            list.Add("R15", "Additional Info Requested");
            list.Add("R16", "Additional Info Requested");
            list.Add("R17", "Additional Info Requested");
            list.Add("E0", "Rejected");
            list.Add("E1", "Rejected");
            list.Add("E2", "Rejected");
            list.Add("E3", "Rejected");
            list.Add("E4", "Rejected");
            list.Add("D0", "Rejected");


            return list;
        }

        private Dictionary<string, string> GetCategoryCodeDesciption()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            list.Add("A0", "The claim has been forwarded to another entity");
            list.Add("A1", "The claim has been received");
            list.Add("A2", "The claim has been accepted for adjudication");
            list.Add("A3", "The claim has been rejected and not entered into adjudication");
            list.Add("A4", "The claim can not be found in adjudication system");
            list.Add("A5", "The claim has been split upon acceptance into adjudication system");
            list.Add("A6", "The claim is missing the information specifid and has been rejected");
            list.Add("A7", "The claim has invalid information and has been rejected");
            list.Add("A8", "The claim has relational field in error and has been rejected");
            list.Add("P0", "Pending – A pended claim is one for which no remittance is issued");
            list.Add("P1", "Pending/In Process – The claim is in adjudication system");
            list.Add("P2", "Pending/Payer Review – The claim is suspended and is pending review (E.g. Medical Review, Repricing etc.)");
            list.Add("P3", "Pending/Provider – The claim is waiting for information that has already been requested from Provider");
            list.Add("P4", "The claim is payer administrator/system hold");
            list.Add("P5", "The claim is payer administrator/system hold");
            list.Add("F0", "Finalized – The claim/encounter has completed the adjudication cycle and no more action will be taken");
            list.Add("F1", "Finalized/Payment – The claim/line has been paid");
            list.Add("F2", "Finalized/Denial – the claim/line has been denied");
            list.Add("F3", "Finalized/Revised – Adjudication information has been changed");
            list.Add("F3F", "Finalized/Forwarded – The claim processing has been completed and forwarded to a subsequent payer as identified in this payer's RECORDS");
            list.Add("F3N", "Finalized/Not forwarded – The claim processing has been completed. The claim/encounter has not been forwarded to any payer");
            list.Add("F4", "Finalized/Adjudication complete – No payment forthcoming. The claim has been adjudicated");
            list.Add("F5", "Finalized/Cannot process the claim");
            list.Add("R0", "Requests for additional Information/General Requests – Requests that don't fall into other R – type categories");
            list.Add("R1", "Requests for additional Information/Entity Requests – Requests for information about specific entities (subscribers, patients, various providers)");
            list.Add("R3", "Requests for additional Information/Claim/Line – Requests for information that could normally be submitted on a claim");
            list.Add("R4", "Requests for additional Information/Documentation – Requests for additional supporting documentation. Examples: Certification, X–ray, Notes");
            list.Add("R5", "Request for additional information/more specific detail – Additional information as a follow up to a previous request is needed. The original information was received but is inadequate. More specific/detailed information is requested");
            list.Add("R6", "Requests for additional information – Regulatory requirements");
            list.Add("R7", "Requests for additional information – Confirm care is consistent with Health Plan policy coverage");
            list.Add("R8", "Requests for additional information – Confirm care is consistent with health plan coverage exceptions");
            list.Add("R9", "Requests for additional information – Determination of medical necessity");
            list.Add("R10", "Requests for additional information – Support a filed grievance or appeal");
            list.Add("R11", "Requests for additional information – Pre–payment review of claims");
            list.Add("R12", "Requests for additional information – Clarification or justification of use for specified procedure code");
            list.Add("R13", "Requests for additional information – Original documents submitted are not readable. Used only for subsequent request(s)");
            list.Add("R14", "Requests for additional information – Original documents received are not what was requested. Used only for subsequent request(s)");
            list.Add("R15", "Requests for additional information – Workers Compensation coverage determination");
            list.Add("R16", "Requests for additional information – Eligibility determination");
            list.Add("R17", "Replacement of a Prior Request. Used to indicate that the current attachment request replaces a prior attachment request");
            list.Add("E0", "Response not possible – error on submitted request data");
            list.Add("E1", "Response not possible – System Status");
            list.Add("E2", "Information Holder is not responding; resubmit at a later time");
            list.Add("E3", "Correction required – relational fields in error");
            list.Add("E4", "Trading partner agreement specific requirement not met: Data correction required. (Usage: A status code identifying the type of information requested must be sent)");
            list.Add("D0", "Data Search Unsuccessful – The payer is unable to return status on the requested claim(s) based on the submitted search criteria");

            return list;
        }

        private Dictionary<string, string> GetStatusCodeDesciption()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            list.Add("0", "Cannot Provide Further Status Electronically");
            list.Add("1", "For More Detailed Information, See Remittance Advice");
            list.Add("2", "More Detailed Information In Letter");
            list.Add("3", "Claim Has Been Adjudicated And Is Awaiting Payment Cycle");
            list.Add("4", "This Is A Subsequent Request For Information From The Original Request");
            list.Add("5", "This Is A Final Request For Information");
            list.Add("6", "Balance Due From The Subscriber");
            list.Add("7", "Claim May Be Reconsidered At A Future Date");
            list.Add("8", "No Payment Due To Contract/Plan Provisions");
            list.Add("9", "No Payment Will Be Made For This Claim");
            list.Add("10", "All Originally Submitted Procedure Codes Have Been Combined");
            list.Add("11", "Some Originally Submitted Procedure Codes Have Been Combined");
            list.Add("12", "One Or More Originally Submitted Procedure Codes Have Been Combined");
            list.Add("13", "All Originally Submitted Procedure Codes Have Been Modified");
            list.Add("14", "Some All Originally Submitted Procedure Codes Have Been Modified");
            list.Add("15", "One Or More Originally Submitted Procedure Code Have Been Modified");
            list.Add("16", "Claim/Encounter Has Been Forwarded To Entity");
            list.Add("17", "Claim/Encounter Has Been Forwarded By Third Party Entity To Entity");
            list.Add("18", "Entity Received Claim/Encounter, But Returned Invalid Status");
            list.Add("19", "Claim Has Been Received");
            list.Add("20", "Accepted For Processing");
            list.Add("21", "Missing Or Invalid Information");
            list.Add("23", "Returned To Entity");
            list.Add("24", "Entity Not Approved As An Electronic Submitter");
            list.Add("25", "Entity Not Approved");
            list.Add("26", "Entity Not Found");
            list.Add("27", "Policy Canceled");
            list.Add("28", "Claim Submitted To Wrong Payer");
            list.Add("29", "Subscriber And Policy Number/Contract Number Mismatched");
            list.Add("30", "Subscriber And Subscriber Id Mismatched");
            list.Add("31", "Subscriber And Policyholder Name Mismatched");
            list.Add("32", "Subscriber And Policy Number/Contract Number Not Found");
            list.Add("33", "Subscriber And Subscriber Id Not Found");
            list.Add("34", "Subscriber And Policyholder Name Not Found");
            list.Add("35", "Claim/Encounter Not Found");
            list.Add("37", "Predetermination Is On File, Awaiting Completion Of Services");
            list.Add("38", "Awaiting Next Periodic Adjudication Cycle");
            list.Add("39", "Charges For Pregnancy Deferred Until Delivery");
            list.Add("40", "Waiting For Final Approval");
            list.Add("41", "Special Handling Required At Payer Site");
            list.Add("44", "Charges Pending Provider Audit");
            list.Add("45", "Awaiting Benefit Determination");
            list.Add("46", "Internal Review/Audit");
            list.Add("47", "Internal Review/Audit - Partial Payment Made");
            list.Add("48", "Referral/Authorization");
            list.Add("49", "Pending Provider Accreditation Review");
            list.Add("50", "Claim Waiting For Internal Provider Verification");
            list.Add("51", "Investigating Occupational Illness/Accident");
            list.Add("52", "Investigating Existence Of Other Insurance Coverage");
            list.Add("53", "Claim Being Researched For Insured Id/Group Policy Number Error");
            list.Add("54", "Duplicate Of A Previously Processed Claim/Line");
            list.Add("55", "Claim Assigned To An Approver/Analyst");
            list.Add("56", "Awaiting Eligibility Determination");
            list.Add("57", "Pending Cobra Information Requested");
            list.Add("59", "Information Was Requested By A Non-Electronic Method");
            list.Add("60", "Information Was Requested By An Electronic Method");
            list.Add("61", "Eligibility For Extended Benefits");
            list.Add("64", "Re-Pricing Information");
            list.Add("65", "Claim/Line Has Been Paid");
            list.Add("66", "Payment Reflects Usual And Customary Charges");
            list.Add("67", "Payment Made In Full");
            list.Add("68", "Partial Payment Made For This Claim");
            list.Add("69", "Payment Reflects Plan Provisions");
            list.Add("70", "Payment Reflects Contract Provisions");
            list.Add("71", "Periodic Installment Released");
            list.Add("72", "Claim Contains Split Payment");
            list.Add("73", "Payment Made To Entity, Assignment Of Benefits Not On File");
            list.Add("78", "Duplicate Of An Existing Claim/Line, Awaiting Processing");
            list.Add("81", "Contract/Plan Does Not Cover Pre-Existing Conditions");
            list.Add("83", "No Coverage For Newborns");
            list.Add("84", "Service Not Authorized");
            list.Add("85", "Entity Not Primary");
            list.Add("86", "Diagnosis And Patient Gender Mismatch");
            list.Add("87", "Denied: Entity Not Found");
            list.Add("88", "Entity Not Eligible For Benefits For Submitted Dates Of Service");
            list.Add("89", "Entity Not Eligible For Dental Benefits For Submitted Dates Of Service");
            list.Add("90", "Entity Not Eligible For Medical Benefits For Submitted Dates Of Service");
            list.Add("91", "Entity Not Eligible/Not Approved For Dates Of Service");
            list.Add("92", "Entity Does Not Meet Dependent Or Student Qualification");
            list.Add("93", "Entity Is Not Selected Primary Care Provider");
            list.Add("94", "Entity Not Referred By Selected Primary Care Provider");
            list.Add("95", "Requested Additional Information Not Received");
            list.Add("96", "No Agreement With Entity");
            list.Add("97", "Patient Eligibility Not Found With Entity");
            list.Add("98", "Charges Applied To Deductible");
            list.Add("99", "Pre-Treatment Review");
            list.Add("100", "Pre-Certification Penalty Taken");
            list.Add("101", "Claim Was Processed As Adjustment To Previous Claim");
            list.Add("102", "Newborn's Charges Processed On Mother's Claim");
            list.Add("103", "Claim Combined With Other Claim(S)");
            list.Add("104", "Processed According To Plan Provisions");
            list.Add("105", "Claim/Line Is Capitated");
            list.Add("106", "This Amount Is Not Entity's Responsibility");
            list.Add("107", "Processed According To Contract Provisions");
            list.Add("108", "Coverage Has Been Canceled For This Entity");
            list.Add("109", "Entity Not Eligible");
            list.Add("110", "Claim Requires Pricing Information");
            list.Add("111", "At The Policyholder's Request These Claims Cannot Be Submitted Electronically");
            list.Add("112", "Policyholder Processes Their Own Claims");
            list.Add("113", "Cannot Process Individual Insurance Policy Claims");
            list.Add("114", "Claim/Service Should Be Processed By Entity");
            list.Add("115", "Cannot Process Hmo Claims");
            list.Add("116", "Claim Submitted To Incorrect Payer");
            list.Add("117", "Claim Requires Signature-On-File Indicator");
            list.Add("118", "Tpo R Claim/Line Because Payer Name Is Missing");
            list.Add("119", "Tpo R Claim/Line Because Certification Information Is Missing");
            list.Add("120", "Tpo R Claim/Line Because Claim Does Not Contain Enough Information");
            list.Add("121", "Service Line Number Greater Than Maximum Allowable For Payer");
            list.Add("122", "Missing/Invalid Data Prevents Payer From Processing Claim");
            list.Add("123", "Additional Information Requested From Entity");
            list.Add("124", "Entity's Name, Address, Phone And Id Number");
            list.Add("125", "Entity's Name");
            list.Add("126", "Entity's Address");
            list.Add("127", "Entity's Communication Number");
            list.Add("128", "Entity's Tax Id");
            list.Add("129", "Entity's Blue Cross Provider Id");
            list.Add("130", "Entity's Blue Shield Provider Id");
            list.Add("131", "Entity's Medicare Provider Id");
            list.Add("132", "Entity's Medicaid Provider Id");
            list.Add("133", "Entity's Upin");
            list.Add("134", "Entity's Champus Provider Id");
            list.Add("135", "Entity's Commercial Provider Id");
            list.Add("136", "Entity's Health Industry Id Number");
            list.Add("137", "Entity's Plan Network Id");
            list.Add("138", "Entity's Site Id ");
            list.Add("139", "Entity's Health Maintenance Provider Id (Hmo)");
            list.Add("140", "Entity's Preferred Provider Organization Id (Ppo)");
            list.Add("141", "Entity's Administrative Services Organization Id (Aso)");
            list.Add("142", "Entity's License/Certification Number");
            list.Add("143", "Entity's State License Number");
            list.Add("144", "Entity's Specialty License Number");
            list.Add("145", "Entity's Specialty/Taxonomy Code");
            list.Add("146", "Entity's Anesthesia License Number");
            list.Add("147", "Entity's Qualification Degree/Designation (E.G. Rn,Phd,Md)");
            list.Add("148", "Entity's Social Security Number");
            list.Add("149", "Entity's Employer Id");
            list.Add("150", "Entity's Drug Enforcement Agency (Dea) Number");
            list.Add("152", "Pharmacy Processor Number");
            list.Add("153", "Entity's Id Number");
            list.Add("154", "Relationship Of Surgeon   Surgeon");
            list.Add("155", "Entity's Relationship To Patient");
            list.Add("156", "Patient Relationship To Subscriber");
            list.Add("157", "Entity's Gender");
            list.Add("158", "Entity's Date Of Birth");
            list.Add("159", "Entity's Date Of Death");
            list.Add("160", "Entity's Marital Status");
            list.Add("161", "Entity's Employment Status");
            list.Add("162", "Entity's Health Insurance Claim Number (Hicn)");
            list.Add("163", "Entity's Policy/Group Number");
            list.Add("164", "Entity's Contract/Member Number");
            list.Add("165", "Entity's Employer Name, Address And Phone");
            list.Add("166", "Entity's Employer Name");
            list.Add("167", "Entity's Employer Address");
            list.Add("168", "Entity's Employer Phone Number");
            list.Add("169", "Entity's Employer Id");
            list.Add("170", "Entity's Employee Id");
            list.Add("171", "Other Insurance Coverage Information (Health, Liability, Auto, Etc.)");
            list.Add("172", "Other Employer Name, Address And Telephone Number");
            list.Add("173", "Entity's Name, Address, Phone, Gender, Dob, Marital Status, Employment Status And Relation To Subscriber");
            list.Add("174", "Entity's Student Status");
            list.Add("175", "Entity's School Name");
            list.Add("176", "Entity's School Address");
            list.Add("177", "Transplant Recipient's Name, Date Of Birth, Gender, Relationship To Insured");
            list.Add("178", "Submitted Charges");
            list.Add("179", "Outside Lab Charges");
            list.Add("180", "Hospital S Semi-Private Room Rate");
            list.Add("181", "Hospital S Room Rate");
            list.Add("182", "Allowable/Paid From Other Entities Coverage ");
            list.Add("183", "Amount Entity Has Paid");
            list.Add("184", "Purchase Price For The Rented Durable Medical Equipment");
            list.Add("185", "Rental Price For Durable Medical Equipment");
            list.Add("186", "Purchase And Rental Price Of Durable Medical Equipment");
            list.Add("187", "Date(S) Of Service");
            list.Add("188", "Statement From-Through Dates");
            list.Add("189", "Facility Admission Date");
            list.Add("190", "Facility Discharge Date");
            list.Add("191", "Date Of Last Menstrual Period (Lmp)");
            list.Add("192", "Date Of First Service For Current Series/Symptom/Illness");
            list.Add("193", "First Consultation/Evaluation Date");
            list.Add("194", "Confinement Dates");
            list.Add("195", "Unable To Work Dates/Disability Dates");
            list.Add("196", "Return To Work Dates");
            list.Add("197", "Effective Coverage Date(S)");
            list.Add("198", "Medicare Effective Date");
            list.Add("199", "Date Of Conception And Expected Date Of Delivery");
            list.Add("200", "Date Of Equipment Return");
            list.Add("201", "Date Of Dental Appliance Prior Placement");
            list.Add("202", "Date Of Dental Prior Replacement/Reason For Replacement");
            list.Add("203", "Date Of Dental Appliance Placed");
            list.Add("204", "Date Dental Canal(S) Opened And Date Service Completed");
            list.Add("205", "Date(S) Dental Root Canal Therapy Previously Performed");
            list.Add("206", "Most Recent Date Of Curettage, Root Planing, Or Periodontal Surgery");
            list.Add("207", "Dental Impression And Seating Date");
            list.Add("208", "Most Recent Date Pacemaker Was Implanted");
            list.Add("209", "Most Recent Pacemaker Battery Change Date");
            list.Add("210", "Date Of The Last X-Ray");
            list.Add("211", "Date(S) Of Dialysis Training Provided To Patient");
            list.Add("212", "Date Of Last Routine Dialysis");
            list.Add("213", "Date Of First Routine Dialysis");
            list.Add("214", "Original Date Of Prescription/Orders/Referral");
            list.Add("215", "Date Of Tooth Extraction/Evolution");
            list.Add("216", "Drug Information");
            list.Add("217", "Drug Name, Strength And Dosage Form");
            list.Add("218", "Ndc Number");
            list.Add("219", "Prescription Number");
            list.Add("220", "Drug Product Id Number");
            list.Add("221", "Drug Days Supply And Dosage");
            list.Add("222", "Drug Dispensing Units And Average Wholesale Price (Awp)");
            list.Add("223", "Route Of Drug/Myelogram Administration");
            list.Add("224", "Anatomical Location For Joint Injection");
            list.Add("225", "Anatomical Location");
            list.Add("226", "Joint Injection Site");
            list.Add("227", "Hospital Information");
            list.Add("228", "Type Of Bill For Ub Claim");
            list.Add("229", "Hospital Admission Source");
            list.Add("230", "Hospital Admission Hour");
            list.Add("231", "Hospital Admission Type");
            list.Add("232", "Admitting Diagnosis");
            list.Add("233", "Hospital Discharge Hour");
            list.Add("234", "Patient Discharge Status");
            list.Add("235", "Units Of Blood Furnished");
            list.Add("236", "Units Of Blood Replaced");
            list.Add("237", "Units Of Deductible Blood");
            list.Add("238", "Separate Claim For Mother/Baby Charges");
            list.Add("239", "Dental Information");
            list.Add("240", "Tooth Surface(S) Involved");
            list.Add("241", "List Of All Missing Teeth (Upper And Lower)");
            list.Add("242", "Tooth Numbers, Surfaces, And/Or Quadrants Involved");
            list.Add("243", "Months Of Dental Treatment Remaining");
            list.Add("244", "Tooth Number Or Letter");
            list.Add("245", "Dental Quadrant/Arch");
            list.Add("246", "Total Orthodontic Service Fee, Initial Appliance Fee, Monthly Fee, Length Of Service");
            list.Add("247", "Line Information");
            list.Add("248", "Accident Date, State, Description And Cause");
            list.Add("249", "Place Of Service");
            list.Add("250", "Type Of Service");
            list.Add("251", "Total Anesthesia Minutes");
            list.Add("252", "Entity's Authorization/Certification Number. ");
            list.Add("253", "Procedure/Revenue Code For Service(S) Rendered");
            list.Add("254", "Principal Diagnosis Code");
            list.Add("255", "Diagnosis Code");
            list.Add("256", "Drg Code(S)");
            list.Add("257", "Adsm-Iii-R Code For Services Rendered");
            list.Add("258", "Days/Units For Procedure/Revenue Code");
            list.Add("259", "Frequency Of Service");
            list.Add("260", "Length Of Medical Necessity, Including Begin Date");
            list.Add("261", "Obesity Measurements");
            list.Add("262", "Type Of Surgery/Service For Which Anesthesia Was Administered");
            list.Add("263", "Length Of Time For Services Rendered");
            list.Add("264", "Number Of Liters/Minute   Hours/Day For Respiratory Support");
            list.Add("265", "Number Of Lesions Excised");
            list.Add("266", "Facility Point Of Origin And Destination - Ambulance");
            list.Add("267", "Number Of Miles Patient Was Transported");
            list.Add("268", "Location Of Durable Medical Equipment Use");
            list.Add("269", "Length/Size Of Laceration/Tumor");
            list.Add("270", "Subluxation Location");
            list.Add("271", "Number Of Spine Segments");
            list.Add("272", "Oxygen Contents For Oxygen System Rental");
            list.Add("273", "Weight");
            list.Add("274", "Height");
            list.Add("275", "Claim");
            list.Add("276", "Ub04/Hcfa-1450/1500 Claim Form");
            list.Add("277", "Paper Claim");
            list.Add("278", "Signed Claim Form");
            list.Add("279", "Claim/Service Must Be Itemized");
            list.Add("280", "Itemized Claim By Provider");
            list.Add("281", "Related Confinement Claim");
            list.Add("282", "Copy Of Prescription");
            list.Add("283", "Medicare Entitlement Information Is Required To Determine Primary Coverage");
            list.Add("284", "Copy Of Medicare Id Card");
            list.Add("285", "Vouchers/Explanation Of Benefits (Eob)");
            list.Add("286", "Other Payer's Explanation Of Benefits / Payment Information");
            list.Add("287", "Medical Necessity For Service");
            list.Add("288", "Hospital Late Charges");
            list.Add("289", "Reason For Late Discharge");
            list.Add("290", "Pre-Existing Information");
            list.Add("291", "Reason For Termination Of Pregnancy");
            list.Add("292", "Purpose Of Family Conference/Therapy");
            list.Add("293", "Reason For Physical Therapy");
            list.Add("294", "Supporting Documentation. ");
            list.Add("295", "Attending Physician Report");
            list.Add("296", "Nurse's Notes");
            list.Add("297", "Medical Notes/Report");
            list.Add("298", "Operative Report");
            list.Add("299", "Emergency Room Notes/Report");
            list.Add("300", "Lab/Test Report/Notes/Results");
            list.Add("301", "Mri Report");
            list.Add("302", "Refer To Codes 300 For Lab Notes And 311 For Pathology Notes");
            list.Add("303", "Physical Therapy Notes");
            list.Add("304", "Reports For Service");
            list.Add("305", "Radiology/X-Ray Reports And/Or Interpretation");
            list.Add("306", "Detailed Description Of Service");
            list.Add("307", "Narrative With Pocket Depth Chart");
            list.Add("308", "Discharge Summary");
            list.Add("309", "Code Was Duplicate Of Code 299");
            list.Add("310", "Progress Notes For The Six Months Prior To Statement Date");
            list.Add("311", "Pathology Notes/Report");
            list.Add("312", "Dental Charting");
            list.Add("313", "Bridgework Information");
            list.Add("314", "Dental Records For This Service");
            list.Add("315", "Past Perio Treatment History");
            list.Add("316", "Complete Medical History");
            list.Add("317", "Patient's Medical Records");
            list.Add("318", "X-Rays/Radiology Films");
            list.Add("319", "Pre/Post-Operative X-Rays/Photographs");
            list.Add("320", "Study Models");
            list.Add("321", "Radiographs Or Models");
            list.Add("322", "Recent Full Mouth X-Rays");
            list.Add("323", "Study Models, X-Rays, And/Or Narrative");
            list.Add("324", "Recent X-Ray Of Treatment Area And/Or Narrative");
            list.Add("325", "Recent Fm X-Rays And/Or Narrative");
            list.Add("326", "Copy Of Transplant Acquisition Invoice");
            list.Add("327", "Periodontal Case Type Diagnosis And Recent Pocket Depth Chart With Narrative");
            list.Add("328", "Speech Therapy Notes");
            list.Add("329", "Exercise Notes");
            list.Add("330", "Occupational Notes");
            list.Add("331", "History And Physical");
            list.Add("332", "Authorization/Certification (Include Period Covered)");
            list.Add("333", "Patient Release Of Information Authorization");
            list.Add("334", "Oxygen Certification");
            list.Add("335", "Durable Medical Equipment Certification");
            list.Add("336", "Chiropractic Certification");
            list.Add("337", "Ambulance Certification/Documentation");
            list.Add("338", "Home Health Certification");
            list.Add("339", "Enteral/Parenteral Certification");
            list.Add("340", "Pacemaker Certification");
            list.Add("341", "Private Duty Nursing Certification");
            list.Add("342", "Podiatric Certification");
            list.Add("343", "Documentation That Facility Is State Licensed And Medicare Approved As A Surgical Facility");
            list.Add("344", "Documentation That Provider Of Physical Therapy Is Medicare Part B Approved");
            list.Add("345", "Treatment Plan For Service/Diagnosis");
            list.Add("346", "Proposed Treatment Plan For Next 6 Months");
            list.Add("347", "Refer To Code 345 For Treatment Plan And Code 282 For Prescription");
            list.Add("348", "Chiropractic Treatment Plan");
            list.Add("349", "Psychiatric Treatment Plan");
            list.Add("350", "Speech Pathology Treatment Plan");
            list.Add("351", "Physical/Occupational Therapy Treatment Plan");
            list.Add("352", "Duration Of Treatment Plan");
            list.Add("353", "Orthodontics Treatment Plan");
            list.Add("354", "Treatment Plan For Replacement Of Remaining Missing Teeth");
            list.Add("355", "Has Claim Been Paid?");
            list.Add("356", "Was Blood Furnished?");
            list.Add("357", "Has Or Will Blood Be Replaced?");
            list.Add("358", "Does Provider Accept Assignment Of Benefits?");
            list.Add("359", "Is There A Release Of Information Signature On File?");
            list.Add("360", "Benefits Assignment Certification Indicator");
            list.Add("361", "Is There Other Insurance?");
            list.Add("362", "Is The Dental Patient Covered By Medical Insurance?");
            list.Add("363", "Possible Workers Compensation");
            list.Add("364", "Is Accident/Illness/Condition Employment Related?");
            list.Add("365", "Is Service The Result Of An Accident?");
            list.Add("366", "Is Injury Due To Auto Accident?");
            list.Add("367", "Is Service Performed For A Recurring Condition Or New Condition?");
            list.Add("368", "Is Medical Doctor (Md) Or Doctor Of Osteopath (Do) On Staff Of This Facility?");
            list.Add("369", "Does Patient Condition Preclude Use Of Ordinary Bed?");
            list.Add("370", "Can Patient Operate Controls Of Bed?");
            list.Add("371", "Is Patient Confined To Room?");
            list.Add("372", "Is Patient Confined To Bed?");
            list.Add("373", "Is Patient An Insulin Diabetic?");
            list.Add("374", "Is Prescribed Lenses A Result Of Cataract Surgery?");
            list.Add("375", "Was Refraction Performed?");
            list.Add("376", "Was Charge For Ambulance For A Round-Trip?");
            list.Add("377", "Was Durable Medical Equipment Purchased New Or Used?");
            list.Add("378", "Is Pacemaker Temporary Or Permanent?");
            list.Add("379", "Were Services Performed Supervised By A Physician?");
            list.Add("380", "Crna Supervision/Medical Direction");
            list.Add("381", "Is Drug Generic?");
            list.Add("382", "Did Provider Authorize Generic Or Brand Name Dispensing?");
            list.Add("383", "Nerve Block Use (Surgery Vs. Pain Management)");
            list.Add("384", "Is Prosthesis/Crown/Inlay Placement An Initial Placement Or A Replacement?");
            list.Add("385", "Is Appliance Upper Or Lower Arch  Appliance Fixed Or Removable?");
            list.Add("386", "Orthodontic Treatment/Purpose Indicator");
            list.Add("387", "Date Patient Last Examined By Entity");
            list.Add("388", "Date Post-Operative Care Assumed");
            list.Add("389", "Date Post-Operative Care Relinquished");
            list.Add("390", "Date Of Most Recent Medical Event Necessitating Service(S)");
            list.Add("391", "Date(S) Dialysis Conducted");
            list.Add("392", "Date(S) Of Blood Transfusion(S)");
            list.Add("393", "Date Of Previous Pacemaker Check");
            list.Add("394", "Date(S) Of Most Recent Hospitalization Related To Service");
            list.Add("395", "Date Entity Signed Certification/Recertification");
            list.Add("396", "Date Home Dialysis Began");
            list.Add("397", "Date Of Onset/Exacerbation Of Illness/Condition");
            list.Add("398", "Visual Field Test Results");
            list.Add("399", "Report Of Prior Testing Related To This Service, Including Dates");
            list.Add("400", "Claim Is Out Of Balance");
            list.Add("401", "Source Of Payment Is Not Valid");
            list.Add("402", "Amount Must Be Greater Than Zero");
            list.Add("403", "Entity Referral Notes/Orders/Prescription");
            list.Add("404", "Specific Findings, Complaints, Or Symptoms Necessitating Service");
            list.Add("405", "Summary Of Services");
            list.Add("406", "Brief Medical History As Related To Service(S)");
            list.Add("407", "Complications/Mitigating Circumstances");
            list.Add("408", "Initial Certification");
            list.Add("409", "Medication Logs/Records (Including Medication Therapy)");
            list.Add("410", "Explain Differences Between Treatment Plan And Patient's Condition");
            list.Add("411", "Medical Necessity For Non-Routine Service(S)");
            list.Add("412", "Medical Records To Substantiate Decision Of Non-Coverage");
            list.Add("413", "Explain/Justify Differences Between Treatment Plan And Services Rendered");
            list.Add("414", "Necessity For Concurrent Care (More Than One Physician Treating The Patient)");
            list.Add("415", "Justify Services Outside Composite Rate");
            list.Add("416", "Verification Of Patient's Ability To Retain And Use Information");
            list.Add("417", "Prior Testing, Including Result(S) And Date(S) As Related To Service(S)");
            list.Add("418", "Indicating Why Medications Cannot Be Taken Orally");
            list.Add("419", "Individual Test(S) Comprising The Panel And The Charges For Each Test");
            list.Add("420", "Name, Dosage And Medical Justification Of Contrast Material Used For Radiology Procedure");
            list.Add("421", "Medical Review Attachment/Information For Service(S)");
            list.Add("422", "Homebound Status");
            list.Add("423", "Prognosis");
            list.Add("424", "Statement Of Non-Coverage Including Itemized Bill");
            list.Add("425", "Itemize Non-Covered Services");
            list.Add("426", "All Current Diagnoses");
            list.Add("427", "Emergency Care Provided During Transport");
            list.Add("428", "Reason For Transport By Ambulance");
            list.Add("429", "Loaded Miles And Charges For Transport To Nearest Facility With Appropriate Services");
            list.Add("430", "Nearest Appropriate Facility");
            list.Add("431", "Patient's Condition / Functional Status At Time Of Service");
            list.Add("432", "Date Benefits Exhausted");
            list.Add("433", "Copy Of Patient Revocation Of Hospice Benefits");
            list.Add("434", "Reasons For More Than One Transfer Per Entitlement Period");
            list.Add("435", "Notice Of Admission");
            list.Add("436", "Short Term Goals");
            list.Add("437", "Long Term Goals");
            list.Add("438", "Number Of Patients Attending Session");
            list.Add("439", "Size, Depth, Amount, And Type Of Drainage Wounds");
            list.Add("440", "Why Non-Skilled Caregiver Has Not Been Taught Procedure");
            list.Add("441", "Entity Professional Qualification For Service(S)");
            list.Add("442", "Modalities Of Service");
            list.Add("443", "Initial Evaluation Report");
            list.Add("444", "Method Used To Obtain Test Sample");
            list.Add("445", "Explain Why Hearing Loss Not Correctable By Hearing Aid");
            list.Add("446", "Documentation From Prior Claim(S) Related To Service(S)");
            list.Add("447", "Plan Of Teaching");
            list.Add("448", "Invalid Billing Combination. See Stc12 For Details");
            list.Add("449", "Projected Date To Discontinue Service(S)");
            list.Add("450", "Awaiting Spend Down Determination");
            list.Add("451", "Preoperative And Post-Operative Diagnosis");
            list.Add("452", "Total Visits In Total Number Of Hours/Day And Total Number Of Hours/Week");
            list.Add("453", "Procedure Code Modifier(S) For Service(S) Rendered");
            list.Add("454", "Procedure Code For Services Rendered");
            list.Add("455", "Revenue Code For Services Rendered");
            list.Add("456", "Covered Day(S)");
            list.Add("457", "Non-Covered Day(S)");
            list.Add("458", "Coinsurance Day(S)");
            list.Add("459", "Lifetime Reserve Day(S)");
            list.Add("460", "Nubc Condition Code(S)");
            list.Add("461", "Nubc Occurrence Code(S) And Date(S)");
            list.Add("462", "Nubc Occurrence Span Code(S) And Date(S)");
            list.Add("463", "Nubc Value Code(S) And/Or Amount(S)");
            list.Add("464", "Payer Assigned Claim Control Number");
            list.Add("465", "Principal Procedure Code For Service(S) Rendered");
            list.Add("466", "Entity's Original Signature");
            list.Add("467", "Entity Signature Date");
            list.Add("468", "Patient Signature Source");
            list.Add("469", "Purchase Service Charge");
            list.Add("470", "Was Service Purchased From Another Entity?");
            list.Add("472", "Ambulance Run Sheet");
            list.Add("473", "Missing Or Invalid Lab Indicator");
            list.Add("474", "Procedure Code And Patient Gender Mismatch");
            list.Add("475", "Procedure Code Not Valid For Patient Age");
            list.Add("476", "Missing Or Invalid Units Of Service");
            list.Add("477", "Diagnosis Code Pointer Is Missing Or Invalid");
            list.Add("478", "Claim Submitter'S Identifier");
            list.Add("479", "Other Carrier Payer Id Is Missing Or Invalid");
            list.Add("480", "Entity's Claim Filing Indicator");
            list.Add("481", "Claim/Submission Format Is Invalid");
            list.Add("482", "Date Error, Century Missing");
            list.Add("483", "Maximum Coverage Amount Met Or Exceeded For Benefit Period");
            list.Add("484", "Business Application Currently Not Available");
            list.Add("485", "More Information Available Than Can Be Returned In Real Time Mode");
            list.Add("486", "Principal Procedure Date");
            list.Add("487", "Claim Not Found, Claim Should Have Been Submitted To/Through Entity");
            list.Add("488", "Diagnosis Code(S) For The Services Rendered");
            list.Add("490", "Other Procedure Code For Service(S) Rendered");
            list.Add("491", "Entity Not Eligible For Encounter Submission");
            list.Add("492", "Other Procedure Date");
            list.Add("493", "Version/Release/Industry Id Code Not Currently Supported By Information Holder");
            list.Add("494", "Real-Time Requests Not Supported By The Information Holder, Resubmit As Batch Request");
            list.Add("495", "Requests For Re-Adjudication Must Reference The Newly Assigned Payer Claim Number");
            list.Add("496", "Submitter Not Approved For Electronic Claim Submissions On Behalf Of This Entity");
            list.Add("497", "Sales Tax Not Paid");
            list.Add("498", "Maximum Leave Days Exhausted");
            list.Add("499", "No Rate On File With The Payer For This Service For This Entity");
            list.Add("500", "Entity's Postal/Zip Code");
            list.Add("501", "Entity's State/Province");
            list.Add("502", "Entity's City");
            list.Add("503", "Entity's Street Address");
            list.Add("504", "Entity's Last Name");
            list.Add("505", "Entity's First Name");
            list.Add("506", "Entity Is Changing Processor/Clearinghouse. Submit To The New Processor/Clearinghouse");
            list.Add("507", "Hcpcs");
            list.Add("508", "Icd9 Note: At Least One Other Status Code Is Required");
            list.Add("509", "External Cause Of Injury Code (E-Code)");
            list.Add("510", "Future Date. ");
            list.Add("511", "Invalid Character. ");
            list.Add("512", "Length Invalid For Receiver's Application System. ");
            list.Add("513", "Hipps Rate Code For Services Rendered");
            list.Add("514", "Entity's Middle Name");
            list.Add("515", "Managed Care Review");
            list.Add("516", "Other Entity's Adjudication Or Payment/Remittance Date");
            list.Add("517", "Adjusted Repriced Claim Reference Number");
            list.Add("518", "Adjusted Repriced Line Item Reference Number");
            list.Add("519", "Adjustment Amount");
            list.Add("520", "Adjustment Quantity");
            list.Add("521", "Adjustment Reason Code");
            list.Add("522", "Anesthesia Modifying Units");
            list.Add("523", "Anesthesia Unit Count");
            list.Add("524", "Arterial Blood Gas Quantity");
            list.Add("525", "Begin Therapy Date");
            list.Add("526", "Bundled Or Unbundled Line Number");
            list.Add("527", "Certification Condition Indicator");
            list.Add("528", "Certification Period Projected Visit Count");
            list.Add("529", "Certification Revision Date");
            list.Add("530", "Claim Adjustment Indicator");
            list.Add("531", "Claim Disproportinate Share Amount");
            list.Add("532", "Claim Drg Amount");
            list.Add("533", "Claim Drg Outlier Amount");
            list.Add("534", "Claim Esrd Payment Amount");
            list.Add("535", "Claim Frequency Code");
            list.Add("536", "Claim Indirect Teaching Amount");
            list.Add("537", "Claim Msp Pass-Through Amount");
            list.Add("538", "Claim Or Encounter Identifier");
            list.Add("539", "Claim Pps Capital Amount");
            list.Add("540", "Claim Pps Capital Outlier Amount");
            list.Add("541", "Claim Submission Reason Code");
            list.Add("542", "Claim Total Denied Charge Amount");
            list.Add("543", "Clearinghouse Or Value Added Network Trace");
            list.Add("544", "Clinical Laboratory Improvement Amendment");
            list.Add("545", "Contract Amount");
            list.Add("546", "Contract Code");
            list.Add("547", "Contract Percentage");
            list.Add("548", "Contract Type Code");
            list.Add("549", "Contract Version Identifier");
            list.Add("550", "Coordination Of Benefits Code");
            list.Add("551", "Coordination Of Benefits Total Submitted Charge");
            list.Add("552", "Cost Report Day Count");
            list.Add("553", "Covered Amount");
            list.Add("554", "Date Claim Paid");
            list.Add("555", "Delay Reason Code");
            list.Add("556", "Demonstration Project Identifier");
            list.Add("557", "Diagnosis Date");
            list.Add("558", "Discount Amount");
            list.Add("559", "Document Control Identifier");
            list.Add("560", "Entity's Additional/Secondary Identifier");
            list.Add("561", "Entity's Contact Name");
            list.Add("562", "Entity's National Provider Identifier (Npi)");
            list.Add("563", "Entity's Tax Amount");
            list.Add("564", "Epsdt Indicator");
            list.Add("565", "Estimated Claim Due Amount");
            list.Add("566", "Exception Code");
            list.Add("567", "Facility Code Qualifier");
            list.Add("568", "Family Planning Indicator");
            list.Add("569", "Fixed Format Information");
            list.Add("570", "Free Form Message Text");
            list.Add("571", "Frequency Count");
            list.Add("572", "Frequency Period");
            list.Add("573", "Functional Limitation Code");
            list.Add("574", "Hcpcs Payable Amount Home Health");
            list.Add("575", "Homebound Indicator");
            list.Add("576", "Immunization Batch Number");
            list.Add("577", "Industry Code");
            list.Add("578", "Insurance Type Code");
            list.Add("579", "Investigational Device Exemption Identifier");
            list.Add("580", "Last Certification Date");
            list.Add("581", "Last Worked Date");
            list.Add("582", "Lifetime Psychiatric Days Count");
            list.Add("583", "Line Item Charge Amount");
            list.Add("584", "Line Item Control Number");
            list.Add("585", "Denied Charge Or Non-Covered Charge");
            list.Add("586", "Line Note Text");
            list.Add("587", "Measurement Reference Identification Code");
            list.Add("588", "Medical Record Number");
            list.Add("589", "Provider Accept Assignment Code");
            list.Add("590", "Medicare Coverage Indicator");
            list.Add("591", "Medicare Paid At 100% Amount");
            list.Add("592", "Medicare Paid At 80% Amount");
            list.Add("593", "Medicare Section 4081 Indicator");
            list.Add("594", "Mental Status Code");
            list.Add("595", "Monthly Treatment Count");
            list.Add("596", "Non-Covered Charge Amount");
            list.Add("597", "Non-Payable Professional Component Amount");
            list.Add("598", "Non-Payable Professional Component Billed Amount");
            list.Add("599", "Note Reference Code");
            list.Add("600", "Oxygen Saturation Qty");
            list.Add("601", "Oxygen Test Condition Code");
            list.Add("602", "Oxygen Test Date");
            list.Add("603", "Old Capital Amount");
            list.Add("604", "Originator Application Transaction Identifier");
            list.Add("605", "Orthodontic Treatment Months Count");
            list.Add("606", "Paid From Part A Medicare Trust Fund Amount");
            list.Add("607", "Paid From Part B Medicare Trust Fund Amount");
            list.Add("608", "Paid Service Unit Count");
            list.Add("609", "Participation Agreement");
            list.Add("610", "Patient Discharge Facility Type Code");
            list.Add("611", "Peer Review Authorization Number");
            list.Add("612", "Per Day Limit Amount");
            list.Add("613", "Physician Contact Date");
            list.Add("614", "Physician Order Date");
            list.Add("615", "Policy Compliance Code");
            list.Add("616", "Policy Name");
            list.Add("617", "Postage Claimed Amount");
            list.Add("618", "Pps-Capital Dsh Drg Amount");
            list.Add("619", "Pps-Capital Exception Amount");
            list.Add("620", "Pps-Capital Fsp Drg Amount");
            list.Add("621", "Pps-Capital Hsp Drg Amount");
            list.Add("622", "Pps-Capital Ime Amount");
            list.Add("623", "Pps-Operating Federal Specific Drg Amount");
            list.Add("624", "Pps-Operating Hospital Specific Drg Amount");
            list.Add("625", "Predetermination Of Benefits Identifier");
            list.Add("626", "Pregnancy Indicator");
            list.Add("627", "Pre-Tax Claim Amount");
            list.Add("628", "Pricing Methodology");
            list.Add("629", "Property Casualty Claim Number");
            list.Add("630", "Referring Clia Number");
            list.Add("631", "Reimbursement Rate");
            list.Add("632", "Reject Reason Code");
            list.Add("633", "Related Causes Code (Accident, Auto Accident, Employment)");
            list.Add("634", "Remark Code");
            list.Add("635", "Repriced Ambulatory Patient Group Code");
            list.Add("636", "Repriced Line Item Reference Number");
            list.Add("637", "Repriced Saving Amount");
            list.Add("638", "Repricing Per Diem Or Flat Rate Amount");
            list.Add("639", "Responsibility Amount");
            list.Add("640", "Sales Tax Amount");
            list.Add("641", "Service Adjudication Or Payment Date");
            list.Add("642", "Service Authorization Exception Code");
            list.Add("643", "Service Line Paid Amount");
            list.Add("644", "Service Line Rate");
            list.Add("645", "Service Tax Amount");
            list.Add("646", "Ship, Delivery Or Calendar Pattern Code");
            list.Add("647", "Shipped Date");
            list.Add("648", "Similar Illness Or Symptom Date");
            list.Add("649", "Skilled Nursing Facility Indicator");
            list.Add("650", "Special Program Indicator");
            list.Add("651", "State Industrial Accident Provider Number");
            list.Add("652", "Terms Discount Percentage");
            list.Add("653", "Test Performed Date");
            list.Add("654", "Total Denied Charge Amount");
            list.Add("655", "Total Medicare Paid Amount");
            list.Add("656", "Total Visits Projected This Certification Count");
            list.Add("657", "Total Visits Rendered Count");
            list.Add("658", "Treatment Code");
            list.Add("659", "Unit Or Basis For Measurement Code");
            list.Add("660", "Universal Product Number");
            list.Add("661", "Visits Prior To Recertification Date Count Cr702");
            list.Add("662", "X-Ray Availability Indicator");
            list.Add("663", "Entity's Group Name");
            list.Add("664", "Orthodontic Banding Date");
            list.Add("665", "Surgery Date");
            list.Add("666", "Surgical Procedure Code");
            list.Add("667", "Real-Time Requests Not Supported By The Information Holder, Do Not Resubmit");
            list.Add("668", "Missing Endodontics Treatment History And Prognosis");
            list.Add("669", "Dental Service Narrative Needed");
            list.Add("670", "Funds Applied From A Spending Account; Consumer Directed Cdhp, H S A");
            list.Add("671", "Funds May Be Available From A Spending Account; Consumer Directed Cdhp, H S A");
            list.Add("672", "Other Payer's Payment Information Is Out Of Balance");
            list.Add("673", "Patient Reason For Visit");
            list.Add("674", "Authorization Exceeded");
            list.Add("675", "Facility Admission Through Discharge Dates");
            list.Add("676", "Entity Possibly Compensated By Facility");
            list.Add("677", "Entity Not Affiliated");
            list.Add("678", "Revenue Code And Patient Gender Mismatch");
            list.Add("679", "Submit Newborn Services On Mothers Claim");
            list.Add("680", "Entity's Country");
            list.Add("681", "Claim Currency Not Supported");
            list.Add("682", "Cosmetic Procedure");
            list.Add("683", "Awaiting Associated Hospital Claims");
            list.Add("684", "Rejected. Syntax Error Noted For This Claim/Service/Inquiry");
            list.Add("685", "Claim Could Not Complete Adjudication In Real Time. Processing In A Batch Mode. Do Not Resubmit");
            list.Add("686", "The Claim/ Encounter Has Completed The Adjudication Cycle And The Entire Claim Has Been Voided");
            list.Add("687", "Claim Estimation Can Not Be Completed In Real Time. Do Not Resubmit");
            list.Add("688", "Present On Admission Indicator For Reported Diagnosis Code(S)");
            list.Add("689", "Entity Was Unable To Respond Within The Expected Time Frame");
            list.Add("690", "Multiple Claims Or Estimate Requests Cannot Be Processed In Real Time");
            list.Add("691", "Multiple Claim Status Requests Cannot Be Processed In Real Time");
            list.Add("692", "Contracted Funding Agreement-Subscriber Is Employed By The Provider Of Services");
            list.Add("693", "Amount Must Be Greater Than Or Equal To Zero");
            list.Add("694", "Amount Must Not Be Equal To Zero");
            list.Add("695", "Entity's Country Subdivision Code");
            list.Add("696", "Claim Adjustment Group Code");
            list.Add("697", "Invalid Decimal Precision. ");
            list.Add("698", "Form Type Identification");
            list.Add("699", "Question/Response From Supporting Documentation Form");
            list.Add("700", "Icd10. Note: At Least One Other Status Code Is Required To Identify The Related Procedure Code Or Diagnosis");
            list.Add("701", "Initial Treatment Date");
            list.Add("702", "Repriced Claim Reference Number");
            list.Add("703", "Advanced Billing Concepts (Abc) Code");
            list.Add("704", "Claim Note Text");
            list.Add("705", "Repriced Allowed Amount");
            list.Add("706", "Repriced Approved Amount");
            list.Add("707", "Repriced Approved Ambulatory Patient Group Amount");
            list.Add("708", "Repriced Approved Revenue Code");
            list.Add("709", "Repriced Approved Service Unit Count");
            list.Add("710", "Line Adjudication Information. ");
            list.Add("711", "Stretcher Purpose");
            list.Add("712", "Obstetric Additional Units");
            list.Add("713", "Patient Condition Description");
            list.Add("714", "Care Plan Oversight Number");
            list.Add("715", "Acute Manifestation Date");
            list.Add("716", "Repriced Approved Drg Code");
            list.Add("717", "This Claim Has Been Split For Processing");
            list.Add("718", "Claim/Service Not Submitted Within The Required Timeframe (Timely Filing)");
            list.Add("719", "Nubc Occurrence Code(S)");
            list.Add("720", "Nubc Occurrence Code Date(S)");
            list.Add("721", "Nubc Occurrence Span Code(S)");
            list.Add("722", "Nubc Occurrence Span Code Date(S)");
            list.Add("723", "Drug Days Supply");
            list.Add("724", "Drug Dosage");
            list.Add("725", "Nubc Value Code(S)");
            list.Add("726", "Nubc Value Code Amount(S)");
            list.Add("727", "Accident Date");
            list.Add("728", "Accident State");
            list.Add("729", "Accident Description");
            list.Add("730", "Accident Cause");
            list.Add("731", "Measurement Value/Test Result");
            list.Add("732", "Information Submitted Inconsistent With Billing Guidelines. ");
            list.Add("733", "Prefix For Entity's Contract/Member Number");
            list.Add("734", "Verifying Premium Payment");
            list.Add("735", "This Service/Claim Is Included In The Allowance For Another Service Or Claim");
            list.Add("736", "A Related Or Qualifying Service/Claim Has Not Been Received/Adjudicated");
            list.Add("737", "Current Dental Terminology (Cdt) Code");
            list.Add("738", "Home Infusion Edi Coalition (Heic) Product/Service Code");
            list.Add("739", "Jurisdiction Specific Procedure Or Supply Code");
            list.Add("740", "Drop-Off Location");
            list.Add("741", "Entity Must Be A Person");
            list.Add("742", "Payer Responsibility Sequence Number Code");
            list.Add("743", "Entity’S Credential/Enrollment Information");
            list.Add("744", "Services/Charges Related To The Treatment Of A Hospital-Acquired Condition Or Preventable Medical Error");
            list.Add("745", "Identifier Qualifier ");
            list.Add("746", "Duplicate Submission");
            list.Add("747", "Hospice Employee Indicator");
            list.Add("748", "Corrected Data Note: Requires A Second Status Code To Identify The Corrected Data");
            list.Add("749", "Date Of Injury/Illness");
            list.Add("750", "Auto Accident State Or Province Code");
            list.Add("751", "Ambulance Pick-Up State Or Province Code");
            list.Add("752", "Ambulance Drop-Off State Or Province Code");
            list.Add("753", "Co-Pay Status Code");
            list.Add("754", "Entity Name Suffix. ");
            list.Add("755", "Entity's Primary Identifier. ");
            list.Add("756", "Entity's Received Date. ");
            list.Add("757", "Last Seen Date");
            list.Add("758", "Repriced Approved Hcpcs Code");
            list.Add("759", "Round Trip Purpose Description");
            list.Add("760", "Tooth Status Code");
            list.Add("761", "Entity's Referral Number. ");
            list.Add("762", "Locum Tenens Provider Identifier. Code Must Be Used With Entity Code 82 - Rendering Provider");
            list.Add("763", "Ambulance Pickup Zipcode");
            list.Add("764", "Professional Charges Are Non Covered");
            list.Add("765", "Institutional Charges Are Non Covered");
            list.Add("766", "Services Were Performed During A Health Insurance Exchange (Hix) Premium Payment Grace Period");
            list.Add("767", "Qualifications For Emergent/Urgent Care");
            list.Add("768", "Service Date Outside The Accidental Injury Coverage Period");
            list.Add("769", "Dme Repair Or Maintenance");
            list.Add("771", "Claim Submitted Prematurely. Please Resubmit After Crossover/Payer To Payer Cob Allotted Waiting Period");
            list.Add("772", "The greatest level of diagnosis code specificity is required");
            list.Add("773", "One calendar year per claim");
            list.Add("774", "Experimental/Investigational");
            list.Add("775", "Entity Type Qualifier (Person/Non-Person Entity)");
            list.Add("776", "Pre/Post-operative care");
            list.Add("777", "Processed based on multiple or concurrent procedure rules");
            list.Add("778", "Non-Compensable incident/event");
            list.Add("779", "Service submitted for the same/similar service within a set timeframe");
            list.Add("780", "Lifetime benefit maximum");
            list.Add("781", "Claim has been identified as a readmission");
            list.Add("782", "Second surgical opinion");
            list.Add("783", "Federal sequestration adjustment");

            return list;
        }

        private Dictionary<string, string> GetEntityDescription()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            list.Add("13", "Contracted Service Provider");
            list.Add("17", "Consultant's Office");
            list.Add("AY", "Clearing House");
            list.Add("1E", "Health Maintenance Organization (HMO)");
            list.Add("1G", "Oncology Center");
            list.Add("1H", "Kidney Dialysis Unit");
            list.Add("1I", "Preferred Provider Organization (PPO)");
            list.Add("1O", "Acute Care Hospital");
            list.Add("1P", "Provider");
            list.Add("1Q", "Military Facility");
            list.Add("1R", "University, College or School");
            list.Add("1S", "Outpatient Surgicenter");
            list.Add("1T", "Physician, Clinic or Group Practice");
            list.Add("1U", "Long Term Care Facility");
            list.Add("1V", "Extended Care Facility");
            list.Add("1W", "Psychiatric Health Facility");
            list.Add("1X", "Laboratory");
            list.Add("1Y", "Retail Pharmacy");
            list.Add("1Z", "Home Health Care");
            list.Add("28", "Subcontractor");
            list.Add("2A", "Federal, State, County or City Facility");
            list.Add("2B", "Third-Party Administrator");
            list.Add("2D", "Miscellaneous Health Care Facility");
            list.Add("2E", "Non-Health Care Miscellaneous Facility");
            list.Add("2I", "Church Operated Facility");
            list.Add("2K", "Partnership");
            list.Add("2P", "Public Health Service Facility");
            list.Add("2Q", "Veterans Administration Facility");
            list.Add("2S", "Public Health Service Indian Service Facility");
            list.Add("2Z", "Hospital Unit of an Institution (prison hospital, college infirmary, etc.)");
            list.Add("30", "Service Supplier");
            list.Add("36", "Employer");
            list.Add("3A", "Hospital Unit Within an Institution for the Mentally Retarded");
            list.Add("3C", "Tuberculosis and Other Respiratory Diseases Facility");
            list.Add("3D", "Obstetrics and Gynecology Facility");
            list.Add("3E", "Eye, Ear, Nose and Throat Facility");
            list.Add("3F", "Rehabilitation Facility");
            list.Add("3G", "Orthopedic Facility");
            list.Add("3H", "Chronic Disease Facility");
            list.Add("3I", "Other Specialty Facility");
            list.Add("3J", "Children's General Facility");
            list.Add("3K", "Children's Hospital Unit of an Institution");
            list.Add("3L", "Children's Psychiatric Facility");
            list.Add("3M", "Children's Tuberculosis and Other Respiratory Diseases Facility");
            list.Add("3N", "Children's Eye, Ear, Nose and Throat Facility");
            list.Add("3O", "Children's Rehabilitation Facility");
            list.Add("3P", "Children's Orthopedic Facility");
            list.Add("3Q", "Children's Chronic Disease Facility");
            list.Add("3R", "Children's Other Speciality Facility");
            list.Add("3S", "Institution for Mental Retardation");
            list.Add("3T", "Alcoholism and Other Chemical Dependency Facility");
            list.Add("3U", "General Inpatient Care the AIDS/ARC Facility");
            list.Add("3V", "AIDS/ARC Unit");
            list.Add("3W", "Specialized Outpatient Program for AIDS/ARC");
            list.Add("3X", "Alcohol/Drug Abuse or Dependency Inpatient Unit");
            list.Add("3Y", "Alcohol/Drug Abuse or Dependency Outpatient Services");
            list.Add("3Z", "Arthritis Treatment Center");
            list.Add("40", "Receiver");
            list.Add("41", "Submitter");
            list.Add("44", "Data Processing Service Bureau");
            list.Add("45", "Drop-off Location");
            list.Add("4A", "Birthing Room/LDRP Room");
            list.Add("4B", "Burn Care Unit");
            list.Add("4C", "Cardiac Catheterization Laboratory");
            list.Add("4D", "Open-Heart Surgery Facility");
            list.Add("4E", "Cardiac Intensive Care Unit");
            list.Add("4F", "Angioplasty Facility");
            list.Add("4G", "Chronic Obstructive Pulmonary Disease Service Facility");
            list.Add("4H", "Emergency Department");
            list.Add("4I", "Trauma Center (Certified)");
            list.Add("4J", "Extracorporeal Shock-Wave Lithotripter (ESWL) Unit");
            list.Add("4L", "Genetic Counseling/Screening Services");
            list.Add("4M", "Adult Day Care Program Facility");
            list.Add("4N", "Alzheimer's Diagnostic / Assessment Services");
            list.Add("4O", "Comprehensive Geriatric Assessment Facility");
            list.Add("4P", "Emergency Response (Geriatric) Unit");
            list.Add("4Q", "Geriatric Acute Care Unit");
            list.Add("4R", "Geriatric Clinics");
            list.Add("4S", "Respite Care Facility");
            list.Add("4U", "Patient Education Unit");
            list.Add("4V", "Community Health Promotion Facility");
            list.Add("4W", "Worksite Health Promotion Facility");
            list.Add("4X", "Hemodialysis Facility");
            list.Add("4Y", "Home Health Services");
            list.Add("4Z", "Hospice");
            list.Add("5B", "Histopathology Laboratory");
            list.Add("5C", "Blood Bank");
            list.Add("5D", "Neonatal Intensive Care Unit");
            list.Add("5E", "Obstetrics Unit");
            list.Add("5F", "Occupational Health Services");
            list.Add("5G", "Organized Outpatient Services");
            list.Add("5H", "Pediatric Acute Inpatient Unit");
            list.Add("5I", "Psychiatric Child/Adolescent Services");
            list.Add("5J", "Psychiatric Consultation-Liaison Services");
            list.Add("5K", "Psychiatric Education Services");
            list.Add("5L", "Psychiatric Emergency Services");
            list.Add("5M", "Psychiatric Geriatric Services");
            list.Add("5N", "Psychiatric Inpatient Unit");
            list.Add("5O", "Psychiatric Outpatient Services");
            list.Add("5P", "Psychiatric Partial Hospitalization Program");
            list.Add("5Q", "Megavoltage Radiation Therapy Unit");
            list.Add("5R", "Radioactive Implants Unit");
            list.Add("5S", "Therapeutic Radioisotope Facility");
            list.Add("5T", "X-Ray Radiation Therapy Unit");
            list.Add("5U", "CT Scanner Unit");
            list.Add("5V", "Diagnostic Radioisotope Facility");
            list.Add("5W", "Magnetic Resonance Imaging (MRI) Facility");
            list.Add("5X", "Ultrasound Unit");
            list.Add("5Y", "Rehabilitation Inpatient Unit");
            list.Add("5Z", "Rehabilitation Outpatient Services");
            list.Add("61", "Performed At");
            list.Add("6A", "Reproductive Health Services");
            list.Add("6B", "Skilled Nursing or Other Long-Term Care Unit");
            list.Add("6C", "Single Photon Emission Computerized Tomography (SPECT) Unit");
            list.Add("6D", "Organized Social Work Service Facility");
            list.Add("6E", "Outpatient Social Work Services");
            list.Add("6F", "Emergency Department Social Work Services");
            list.Add("6G", "Sports Medicine Clinic/Services");
            list.Add("6H", "Hospital Auxiliary Unit");
            list.Add("6I", "Patient Representative Services");
            list.Add("6J", "Volunteer Services Department");
            list.Add("6K", "Outpatient Surgery Services");
            list.Add("6L", "Organ/Tissue Transplantation Unit");
            list.Add("6M", "Orthopedic Surgery Facility");
            list.Add("6N", "Occupational Therapy Services");
            list.Add("6O", "Physical Therapy Services");
            list.Add("6P", "Recreational Therapy Services");
            list.Add("6Q", "Respiratory Therapy Services");
            list.Add("6R", "Speech Therapy Services");
            list.Add("6S", "Women's Health Center / Services");
            list.Add("6U", "Cardiac Rehabilitation Program Facility");
            list.Add("6V", "Non-Invasive Cardiac Assessment Services");
            list.Add("6W", "Emergency Medical Technician");
            list.Add("6X", "Disciplinary Contact");
            list.Add("6Y", "Case Manager");
            list.Add("71", "Attending Physician");
            list.Add("72", "Operating Physician");
            list.Add("73", "Other Physician");
            list.Add("74", "Corrected Insured");
            list.Add("77", "Service Location");
            list.Add("7C", "Place of Occurrence");
            list.Add("80", "Hospital");
            list.Add("82", "Rendering Provider");
            list.Add("84", "Subscriber's Employer");
            list.Add("85", "Billing Provider");
            list.Add("87", "Pay-to Provider");
            list.Add("95", "Research Institute");
            list.Add("CK", "Pharmacist");
            list.Add("CZ", "Admitting Surgeon");
            list.Add("D2", "Commercial Insurer");
            list.Add("DD", "Assistant Surgeon");
            list.Add("DJ", "Consulting Physician");
            list.Add("DK", "Ordering Physician");
            list.Add("DN", "Referring Provider");
            list.Add("DO", "Dependent Name");
            list.Add("DQ", "Supervising Physician");
            list.Add("E1", "Person or Other Entity Legally Responsible for a Child");
            list.Add("E2", "Person or Other Entity With Whom a Child Resides");
            list.Add("E7", "Previous Employer");
            list.Add("E9", "Participating Laboratory");
            list.Add("FA", "Facility");
            list.Add("FD", "Physical Address");
            list.Add("FE", "Mail Address");
            list.Add("G3", "Clinic");
            list.Add("GB", "Other Insured");
            list.Add("GD", "Guardian");
            list.Add("GI", "Paramedic");
            list.Add("GJ", "Paramedical Company");
            list.Add("GK", "Previous Insured");
            list.Add("GM", "Spouse Insured");
            list.Add("GY", "Treatment Facility");
            list.Add("HF", "Healthcare Professional Shortage Area (HPSA) Facility");
            list.Add("HH", "Home Health Agency");
            list.Add("I3", "Independent Physicians Association (IPA)");
            list.Add("IJ", "Injection Point");
            list.Add("IL", "Insured or Subscriber");
            list.Add("IN", "Insurer");
            list.Add("LI", "Independent Lab");
            list.Add("LR", "Legal Representative");
            list.Add("MR", "Medical Insurance Carrier");
            list.Add("OB", "Ordered By");
            list.Add("OD", "Doctor of Optometry");
            list.Add("OX", "Oxygen Therapy Facility");
            list.Add("P0", "Patient Facility");
            list.Add("P2", "Primary Insured or Subscriber");
            list.Add("P3", "Primary Care Provider");
            list.Add("P4", "Prior Insurance Carrier");
            list.Add("P6", "Third Party Reviewing Preferred Provider Organization (PPO)");
            list.Add("P7", "Third Party Repricing Preferred Provider Organization (PPO)");
            list.Add("PE", "Payee (subrograted payee)");
            list.Add("PR", "Payer");
            list.Add("PT", "Party to Receive Test Report");
            list.Add("PV", "Party performing certification");
            list.Add("PW", "Pick up Address");
            list.Add("QA", "Pharmacy");
            list.Add("QB", "Purchase Service Provider");
            list.Add("QC", "Patient");
            list.Add("QD", "Responsible Party");
            list.Add("QE", "Policyholder");
            list.Add("QH", "Physician");
            list.Add("QK", "Managed Care");
            list.Add("QL", "Chiropractor");
            list.Add("QN", "Dentist");
            list.Add("QO", "Doctor of Osteopathy");
            list.Add("QS", "Podiatrist");
            list.Add("QV", "Group Practice");
            list.Add("QY", "Medical Doctor");
            list.Add("RC", "Receiving Location");
            list.Add("RW", "Rural Health Clinic");
            list.Add("S4", "Skilled Nursing Facility");
            list.Add("SJ", "Service Provider");
            list.Add("SU", "Supplier/Manufacturer");
            list.Add("T4", "Transfer Point");
            list.Add("TQ", "Third Party Reviewing Organization (TPO)");
            list.Add("TT", "Transfer To");
            list.Add("TU", "Third Party Repricing Organization (TPO)");
            list.Add("UH", "Nursing Home");
            list.Add("X3", "Utilization Management Organization");
            list.Add("X4", "Spouse");
            list.Add("X5", "Durable Medical Equipment Supplier");
            list.Add("ZZ", "Mutually Defined");



            return list;
        }

    }
}

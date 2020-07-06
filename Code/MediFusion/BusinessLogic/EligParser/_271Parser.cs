using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MediFusionPM.BusinessLogic.EligParser
{
    public class _271Parser
    {
        int _counter = 0;
        char E = ' ', C = ' ', S = ' ', R = '^';
        string[] elements = null;
        string[] segments = null;
        List<_271Header> PatientEligiblityData = new List<_271Header>();

        string ISA06, ISA08, ISAControlNum, GS02, GS03, GSConrolNum, Version;
        DateTime? ISADate;
        public List<_271Header> Parse271File(string FilePath)
        {
            PatientEligiblityData = new List<_271Header>();
            _counter = 0;

            if (!File.Exists(FilePath)) throw new Exception(string.Format("File {0} not found.", FilePath));
            string contents = File.ReadAllText(FilePath);
            
            if (!contents.StartsWith("ISA") && contents.Length < 200) throw new Exception(string.Format("Invalid File {0}", FilePath));

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
                        R = char.Parse(elements.GetElement(11));
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

            return PatientEligiblityData;
        }



        private void ParseST()
        {
            Dictionary<string, string> lstEB01Benefit = GetEB_01_Benefit();
            Dictionary<string, string> lstEB02CoverageLevel = GetEB_02_CoverageLevel();
            Dictionary<string, string> lstEB03ServiceType = GetEB_03_ServiceType();
            Dictionary<string, string> lstEB04InsuranceType = GetEB_04_InsuranceType();
            Dictionary<string, string> lstEB06TimePeriod = GetEB_06_TimePeriod();
            Dictionary<string, string> lstEB09QuanityQual = GetEB_09_QuantityQualifier();
            Dictionary<string, string> lstEB11QuanityQual = GetEB_11_Authorization();
            Dictionary<string, string> lstEB12PlanNetwork = GetEB_12_PlanNetwork();
            Dictionary<string, string> lstRef = GetREF();
            Dictionary<string, string> lstDates = GetDate();
            Dictionary<string, string> lstAAA = GetAAA();

            string value = string.Empty;

            _271Header header = new _271Header();
            _271Subscriber subscriber = new _271Subscriber();
            _271SBREligibilityInfo eligInfo = new _271SBREligibilityInfo();

            string provLastName = string.Empty; string provFirstName = string.Empty; string provNPI = string.Empty;
            
            //header.SubscriberData = new _271Subscriber();

            header.ISA06SenderID = this.ISA06;
            header.ISA08ReceiverID = this.ISA08;
            header.ISADateTime = this.ISADate;
            header.ISAControlNumber = this.ISAControlNum;
            header.GS02SenderID = this.GS02;
            header.GS03ReceiverID = this.GS03;
            header.GSControlNumber = this.GSConrolNum;
            header.VersionNumber = this.Version;


            if (elements.GetElement(1) != "271") throw new Exception("Invalid File.");
            header.STControlNumber = elements.GetElement(2);
            _counter += 1;
            bool stCondition = true;
            while (stCondition)
            {
                elements = segments[_counter].Split(E);
                switch (elements.GetElement(0).Trim())
                {
                    case "HL":

                        #region HL Segment

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
                                        header.PayerOrgName = elements.GetElement(3);
                                        if (elements.Length > 8) header.PayerID = elements.GetElement(9);
                                        break;

                                    case "N3":
                                        header.PayerAddress = elements.GetElement(1);
                                        break;
                                    case "N4":
                                        header.PayerCity = elements.GetElement(1);
                                        header.PayerState = elements.GetElement(2);
                                        header.PayerZip = elements.GetElement(3);
                                        break;

                                    case "PER":
                                        if (elements.GetElement(1) == "CX")
                                        {
                                            header.PayerContactName = elements.GetElement(2);
                                            if (elements.GetElement(3) == "TE") header.PayerTelephone = elements.GetElement(4);
                                        }
                                        else if (elements.GetElement(1) == "BL")
                                        {
                                            header.PayerBillingContactName = elements.GetElement(2);
                                            if (elements.GetElement(3) == "TE") header.PayerBillingTelephone = elements.GetElement(4);
                                            else if (elements.GetElement(3) == "EM") header.PayerBillingEmail = elements.GetElement(4);

                                            if (elements.Length > 5)
                                            {
                                                if (elements.GetElement(5) == "TE") header.PayerBillingContactName = elements.GetElement(5);
                                                else if (elements.GetElement(5) == "EM") header.PayerBillingEmail = elements.GetElement(5);
                                            }
                                        }
                                        else if (elements.GetElement(1) == "IC")
                                        {
                                            if (elements.GetElement(3) == "UR") header.PayerWebsite = elements.GetElement(4);

                                            header.PayerContactName = elements.GetElement(2);
                                            if (elements.GetElement(3) == "TE") header.PayerTelephone = elements.GetElement(4);

                                        }
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
                                        provLastName = elements.GetElement(3);
                                        provFirstName = elements.GetElement(4);
                                        if (elements.Length > 8) provNPI = elements.GetElement(9);
                                        break;

                                    case "HL":
                                        hlSubmitter = false; _counter -= 1;
                                        break;
                                }
                                if (hlSubmitter) _counter += 1;
                            }
                        }
                        else if (elements.GetElement(3) == "22")
                        {
                            subscriber = new _271Subscriber() { ProvLastName = provLastName, ProvFirstName = provFirstName, ProvNPI = provNPI };
                            
                            _counter += 1;
                            bool hlSubscriber = true;
                            while (hlSubscriber)
                            {
                                elements = segments[_counter].Split(E);
                                switch (elements.GetElement(0).Trim())
                                {
                                    case "TRN":
                                        subscriber.TRN = elements.GetElement(2);
                                        break;

                                    case "NM1":
                                        subscriber.SBRLastName = elements.GetElement(3);
                                        subscriber.SBRFirstName = elements.GetElement(4);
                                        subscriber.SBRMiddleInitial = elements.GetElement(5);
                                        if (elements.Length > 8) subscriber.SBRID = elements.GetElement(9);
                                        break;

                                    case "REF":
                                        if (elements.GetElement(1) == "6P")
                                            subscriber.PolicyNumber = elements.GetElement(2);
                                        else if (elements.GetElement(1) == "SY")
                                            subscriber.SSN = elements.GetElement(2);
                                        break;

                                    case "N3":
                                        subscriber.SBRAddress = elements.GetElement(1);
                                        break;

                                    case "N4":
                                        subscriber.SBRCity = elements.GetElement(1);
                                        subscriber.SBRState = elements.GetElement(2);
                                        subscriber.SBRZipCode = elements.GetElement(3);
                                        break;

                                    case "DMG":
                                        subscriber.SBRDob = Utilities.GetDate(elements.GetElement(2), "");
                                        subscriber.SBRGender = elements.GetElement(3);
                                        break;

                                    case "DTP":
                                        if (elements.GetElement(1) == "346")
                                        {
                                            string[] dt = elements.GetElement(3).Split('-');
                                            if (dt.Length > 1)
                                            {
                                                subscriber.PlanBeginDate = Utilities.GetDate(dt[0], string.Empty);
                                                subscriber.PlanBeginDate = Utilities.GetDate(dt[1], string.Empty);
                                            }
                                            else subscriber.PlanBeginDate = Utilities.GetDate(dt[0], string.Empty);
                                        }
                                        else if (elements.GetElement(1) == "356")
                                        {
                                            string[] dt = elements.GetElement(3).Split('-');
                                            if (dt.Length > 1)
                                            {
                                                subscriber.EligibilityBeginDate = Utilities.GetDate(dt[0], string.Empty);
                                                subscriber.EligiblityEndDate = Utilities.GetDate(dt[1], string.Empty);
                                            }
                                            else subscriber.EligibilityBeginDate = Utilities.GetDate(dt[0], string.Empty);
                                        }
                                        break;

                                    case "AAA":
                                        subscriber.AAA01 = elements.GetElement(1);
                                        subscriber.AAA03 = elements.GetElement(3);
                                        subscriber.AAA04 = elements.GetElement(4);

                                        lstAAA.TryGetValue(subscriber.AAA03, out value);
                                        
                                        subscriber.AAAErrorMsg += "," + value;
                                        subscriber.AAAErrorMsg = subscriber.AAAErrorMsg.Trim(',');
                                        break;

                                    case "HL":
                                        if (!header.ListOfSubscriberData.Contains(subscriber))
                                            header.ListOfSubscriberData.Add(subscriber);

                                        hlSubscriber = false;
                                        _counter -= 1;

                                        break;
                                       

                                    case "EB":
                                    case "SE":
                                    case "GE":
                                        if(elements.GetElement(0) == "SE")
                                        {
                                            if (!header.ListOfSubscriberData.Contains(subscriber))
                                                header.ListOfSubscriberData.Add(subscriber);
                                        }
                                        hlSubscriber = false;
                                        _counter -= 1;
                                        break;
                                }
                                if (hlSubscriber) _counter += 1;
                            }
                            if (hlSubscriber) _counter += 1;
                        }
                        break;

                    #endregion

                    case "EB":

                        #region EB Segment

                        int msgCounter = 0;

                        //Adding Previous EB
                        if (!string.IsNullOrEmpty(eligInfo.EB01CoverageType) && !subscriber.EligibilityData.Contains(eligInfo))
                            subscriber.EligibilityData.Add(eligInfo);
                        //

                        eligInfo = new _271SBREligibilityInfo();
                        eligInfo.ListOfReferenceIds = new Dictionary<string, string>();
                        eligInfo.ListOfDates = new Dictionary<string, DateTime>();
                        eligInfo.Messages = new List<string>();

                        eligInfo.EB01CoverageType = elements.GetElement(1);

                        lstEB01Benefit.TryGetValue(eligInfo.EB01CoverageType, out value);
                        eligInfo.EB01CoverageTypeV = value;

                        eligInfo.EB02CoverageLevel = elements.GetElement(2);
                        lstEB02CoverageLevel.TryGetValue(eligInfo.EB02CoverageLevel, out value);
                        eligInfo.EB02CoverageLevelV = value;

                        eligInfo.EB03ServiceTypeCode = elements.GetElement(3);
                        string[] eb03 = eligInfo.EB03ServiceTypeCode.Split(R);
                        foreach (string s in eb03)
                        {
                            lstEB03ServiceType.TryGetValue(s, out value);
                            eligInfo.EB03ServiceTypeCodeV += value + ";; ";
                            value = "";
                        }
                        //eligInfo.EB03ServiceTypeCodeV = eligInfo.EB03ServiceTypeCodeV.Trim().TrimEnd(new char[] { ',' });

                        eligInfo.EB04InsuranceTypeCode = elements.GetElement(4);
                        lstEB04InsuranceType.TryGetValue(eligInfo.EB04InsuranceTypeCode, out value);
                        eligInfo.EB04InsuranceTypeCodeV = value;

                        if (eligInfo.EB04InsuranceTypeCode.ToString().Contains(C))
                        {
                            eligInfo.EB04InsuranceTypeCodeList = eligInfo.EB04InsuranceTypeCode.Split(C).ToList<string>();
                        }

                        eligInfo.EB05PlanCoverageDesc = elements.GetElement(5);
                        eligInfo.EB06TimePeriod = elements.GetElement(6);

                        lstEB06TimePeriod.TryGetValue(eligInfo.EB06TimePeriod, out value);
                        eligInfo.EB06TimePeriodV = value;

                        eligInfo.EB07MonetoryAmount = elements.GetElement(7);
                        eligInfo.EB08BenefitPercent = elements.GetElement(8);

                        eligInfo.EB09QuanityQualifier = elements.GetElement(9);
                        lstEB09QuanityQual.TryGetValue(eligInfo.EB09QuanityQualifier, out value);
                        eligInfo.EB09QuanityQualifierV = value;

                        eligInfo.EB10BenenfitQuantity = elements.GetElement(10);

                        eligInfo.EB11AuthorizationIndicator = elements.GetElement(11);
                        lstEB11QuanityQual.TryGetValue(eligInfo.EB11AuthorizationIndicator, out value);
                        eligInfo.EB11AuthorizationIndicatorV = value;

                        eligInfo.EB12PlanNetworkIndicator = elements.GetElement(12);
                        lstEB12PlanNetwork.TryGetValue(eligInfo.EB12PlanNetworkIndicator, out value);
                        eligInfo.EB12PlanNetworkIndicatorV = value;

                        if (elements.Length > 13)
                        {
                            string[] eb13 = elements.GetElement(13).Split(C);
                            eligInfo.EB13_01_ServiceQual = eb13.GetElement(0);
                            eligInfo.EB13_02_CPT = eb13.GetElement(1);
                            eligInfo.EB13_03_Modifier1 = eb13.GetElement(2);
                            eligInfo.EB13_04_Modifier2 = eb13.GetElement(3);
                            eligInfo.EB13_05_Modifier3 = eb13.GetElement(4);
                            eligInfo.EB13_06_Modifier4 = eb13.GetElement(5);
                            eligInfo.EB13_07_Description = eb13.GetElement(6);
                            eligInfo.EB13_08_ServiceQual = eb13.GetElement(7);
                        }

                        if (elements.Length > 14)
                        {
                            string[] eb14 = elements.GetElement(14).Split(C);
                            eligInfo.EB14_01_DiagPointer1 = eb14.GetElement(0);
                            eligInfo.EB14_02_DiagPointer2 = eb14.GetElement(1);
                            eligInfo.EB14_03_DiagPointer3 = eb14.GetElement(2);
                            eligInfo.EB14_04_DiagPointer4 = eb14.GetElement(3);
                        }

                        _counter += 1;

                        bool ebLoop = true;
                        while (ebLoop)
                        {
                            elements = segments[_counter].Split(E);
                            switch (elements.GetElement(0).Trim())
                            {
                                case "HSD":
                                    break;

                                case "REF":
                                    lstRef.TryGetValue(elements.GetElement(1), out value);
                                    if (!string.IsNullOrEmpty(value) && !eligInfo.ListOfReferenceIds.ContainsKey(value))
                                        eligInfo.ListOfReferenceIds.Add(value, elements.GetElement(1));
                                    break;

                                case "DTP":
                                    if (elements.GetElement(1) == "096") eligInfo.DisChargeDate = Utilities.GetDate(elements.GetElement(3), string.Empty);
                                    else if (elements.GetElement(1) == "193") eligInfo.PeriodStartDate = Utilities.GetDate(elements.GetElement(3), string.Empty);
                                    else if (elements.GetElement(1) == "194") eligInfo.PeriodEndDate = Utilities.GetDate(elements.GetElement(3), string.Empty);
                                    else if (elements.GetElement(1) == "198") eligInfo.CompletionDate = Utilities.GetDate(elements.GetElement(3), string.Empty);
                                    else if (elements.GetElement(1) == "290") eligInfo.COBDate = Utilities.GetDate(elements.GetElement(3), string.Empty);
                                    else if (elements.GetElement(1) == "291") eligInfo.PlanDate = Utilities.GetDate(elements.GetElement(3), string.Empty);
                                    else if (elements.GetElement(1) == "292") eligInfo.BenefitBegin = Utilities.GetDate(elements.GetElement(3), string.Empty);
                                    else if (elements.GetElement(1) == "295") { }
                                    else if (elements.GetElement(1) == "304") { }
                                    else if (elements.GetElement(1) == "307") eligInfo.EligiblityDate = Utilities.GetDate(elements.GetElement(3), string.Empty);
                                    else if (elements.GetElement(1) == "318") { }
                                    else if (elements.GetElement(1) == "346") eligInfo.PlanBeginDate = Utilities.GetDate(elements.GetElement(3), string.Empty);
                                    else if (elements.GetElement(1) == "348") eligInfo.BenefitBegin = Utilities.GetDate(elements.GetElement(3), string.Empty);
                                    else if (elements.GetElement(1) == "349") eligInfo.BenefitEnd = Utilities.GetDate(elements.GetElement(3), string.Empty);
                                    else if (elements.GetElement(1) == "356") eligInfo.EligibilityBeginDate = Utilities.GetDate(elements.GetElement(3), string.Empty);
                                    else if (elements.GetElement(1) == "357") eligInfo.EligibilityEndDate = Utilities.GetDate(elements.GetElement(3), string.Empty);
                                    else if (elements.GetElement(1) == "435") { }
                                    else if (elements.GetElement(1) == "472") eligInfo.ServiceDate = Utilities.GetDate(elements.GetElement(3), string.Empty);
                                    else if (elements.GetElement(1) == "636") { }
                                    else if (elements.GetElement(1) == "771") { }

                                    lstDates.TryGetValue(elements.GetElement(1), out value);
                                    if (!string.IsNullOrEmpty(value) && !eligInfo.ListOfDates.ContainsKey(value))
                                        eligInfo.ListOfDates.Add(value, (DateTime)Utilities.GetDate(elements.GetElement(3), string.Empty));
                                    
                                    break;

                                case "AAA":
                                    eligInfo.AAA01 = elements.GetElement(1);
                                    eligInfo.AAA03 = elements.GetElement(3);
                                    eligInfo.AAA04 = elements.GetElement(4);
                                    break;

                                case "MSG":
                                    msgCounter += 1;
                                    if (msgCounter == 1) eligInfo.MessageText1 = elements.GetElement(1);
                                    else if (msgCounter == 2) eligInfo.MessageText2 = elements.GetElement(1);
                                    else if (msgCounter == 3) eligInfo.MessageText3 = elements.GetElement(1);
                                    else if (msgCounter == 4) eligInfo.MessageText4 = elements.GetElement(1);
                                    else if (msgCounter == 5) eligInfo.MessageText5 = elements.GetElement(1);
                                    else if (msgCounter == 6) eligInfo.MessageText6 = elements.GetElement(1);
                                    else if (msgCounter == 7) eligInfo.MessageText7 = elements.GetElement(1);
                                    else if (msgCounter == 8) eligInfo.MessageText8 = elements.GetElement(1);
                                    else if (msgCounter == 9) eligInfo.MessageText9 = elements.GetElement(1);
                                    else if (msgCounter == 10) eligInfo.MessageText10 = elements.GetElement(1);

                                    eligInfo.Messages.Add(elements.GetElement(1));

                                    break;

                                case "III":
                                    break;

                                case "LS":

                                    _counter += 1;
                                    bool ebLsLoop = true;
                                    while (ebLsLoop)
                                    {
                                        elements = segments[_counter].Split(E);
                                        switch (elements.GetElement(0).Trim())
                                        {
                                            case "NM1":
                                                eligInfo.EBEntity = elements.GetElement(2);
                                                eligInfo.EBLastName = elements.GetElement(3);
                                                eligInfo.EBFirstName = elements.GetElement(4);
                                                eligInfo.EBMI = elements.GetElement(5);
                                                if (elements.Length > 9) eligInfo.EBNPI = elements.GetElement(9);
                                                break;

                                            case "N3":
                                                eligInfo.EBAddress = elements.GetElement(1);
                                                break;

                                            case "N4":
                                                eligInfo.EBCity = elements.GetElement(1);
                                                eligInfo.EBState = elements.GetElement(2);
                                                eligInfo.EBZipCode = elements.GetElement(3);
                                                break;

                                            case "PER":
                                                if (elements.GetElement(1) == "CX")
                                                {
                                                    eligInfo.EBContactPerson = elements.GetElement(2);
                                                    if (elements.GetElement(3) == "TE") eligInfo.EBTelephoneNUm = elements.GetElement(4);
                                                }
                                                else if (elements.GetElement(1) == "BL")
                                                {
                                                    eligInfo.EBBillingContactPerson = elements.GetElement(2);
                                                    if (elements.GetElement(3) == "TE") eligInfo.EBBillingTelephoneNum = elements.GetElement(4);
                                                    else if (elements.GetElement(3) == "EM") eligInfo.EBBillingEmail = elements.GetElement(4);


                                                    if (elements.Length > 5)
                                                    {
                                                        if (elements.GetElement(5) == "TE") eligInfo.EBBillingContactPerson = elements.GetElement(5);
                                                        else if (elements.GetElement(5) == "EM") eligInfo.EBBillingEmail = elements.GetElement(5);
                                                    }
                                                }
                                                else if (elements.GetElement(1) == "IC")
                                                {
                                                    if (elements.GetElement(3) == "UR") eligInfo.EBWebsite = elements.GetElement(4);
                                                }
                                                break;

                                            case "PRV":
                                                eligInfo.EBProviderType = elements.GetElement(1);
                                                eligInfo.EBTaxonomyCode = elements.GetElement(2);
                                                break;

                                            case "LE":
                                                ebLsLoop = false;
                                                ebLoop = false;

                                                _counter -= 1;

                                                subscriber.EligibilityData.Add(eligInfo);

                                                break;
                                        }

                                        if (ebLoop) _counter += 1;
                                    }
                                    break;

                                case "HL":
                                    subscriber.EligibilityData.Add(eligInfo);
                                    if (!header.ListOfSubscriberData.Contains(subscriber))
                                        header.ListOfSubscriberData.Add(subscriber);
                                    
                                    ebLoop = false;
                                    _counter -= 1;
                                    break;


                                case "EB":
                                case "SE":
                                    subscriber.EligibilityData.Add(eligInfo);
                                    
                                    ebLoop = false;
                                    _counter -= 1;
                                    break;
                            }
                            if (ebLoop) _counter += 1;
                        }

                        if (ebLoop) _counter += 1;

                        break;

                    #endregion

                    
                    case "SE":
                    case "GE":
                    case "IEA":
                        //Adding Last EB
                        if (!subscriber.EligibilityData.Contains(eligInfo))
                            subscriber.EligibilityData.Add(eligInfo);
                        //
                        _counter -= 1; stCondition = false;
                        if (!PatientEligiblityData.Contains(header)) PatientEligiblityData.Add(header);
                        break;
                }
                if (stCondition) _counter += 1;
            }

            if(!header.ListOfSubscriberData.Contains(subscriber))
                header.ListOfSubscriberData.Add(subscriber);
        }


        private Dictionary<string, string> GetEB_01_Benefit()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            list.Add("1", "Active");
            list.Add("2", "Active - Full Risk Capitation");
            list.Add("3", "Active - Services Capitated");
            list.Add("4", "Active - Services Capitated to Primary Care Physician");
            list.Add("5", "Active - Pending Investigation");
            list.Add("6", "Inactive");
            list.Add("7", "Inactive - Pending Eligibility Update");
            list.Add("8", "Inactive - Pending Investigation");
            list.Add("A", "Co-Insurance ");
            list.Add("B", "Co-Payment ");
            list.Add("C", "Deductible ");
            list.Add("CB", "Coverage Basis");
            list.Add("D", "Benefit Description");
            list.Add("E", "Exclusions");
            list.Add("F", "Limitations");
            list.Add("G", "Out of Pocket (Stop Loss)");
            list.Add("H", "Unlimited");
            list.Add("I", "Non-Covered");
            list.Add("J", "Cost Containment ");
            list.Add("K", "Reserve");
            list.Add("L", "Primary Care Provider");
            list.Add("M", "Pre-existing Condition");
            list.Add("MC", "Managed Care Coordinator");
            list.Add("N", "Services Restricted to Following Provider");
            list.Add("O", "Not Deemed a Medical Necessity");
            list.Add("P", "Benefit Disclaimer ");
            list.Add("Q", "Second Surgical Opinion Required");
            list.Add("R", "Other or Additional Payor");
            list.Add("S", "Prior Year(s) History");
            list.Add("T", "Card(s) Reported Lost/Stolen ");
            list.Add("U", "Contact Following Entity for Eligibility or Benefit Information");
            list.Add("V", "Cannot Process");
            list.Add("W", "Other Source of Data");
            list.Add("X", "Health Care Facility");
            list.Add("Y", "Spend Down ");

            return list;
        }

        private Dictionary<string, string> GetEB_02_CoverageLevel()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            list.Add("CHD", "Children Only");
            list.Add("DEP", "Dependents Only");
            list.Add("ECH", "Employee and Children");
            list.Add("EMP", "Employee Only");
            list.Add("ESP", "Employee and Spouse");
            list.Add("FAM", "Family");
            list.Add("IND", "Individual");
            list.Add("SPC", "Spouse and Children");
            list.Add("SPO", "Spouse Only");

            return list;
        }

        private Dictionary<string, string> GetEB_03_ServiceType()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            list.Add("1", "Medical Care");
            list.Add("2", "Surgical");
            list.Add("3", "Consultation");
            list.Add("4", "Diagnostic X-Ray");
            list.Add("5", "Diagnostic Lab");
            list.Add("6", "Radiation Therapy");
            list.Add("7", "Anesthesia");
            list.Add("8", "Surgical Assistance");
            list.Add("9", "Other Medical");
            list.Add("10", " Blood Charges");
            list.Add("11", " Used Durable Medical Equipment");
            list.Add("12", " Durable Medical Equipment Purchase");
            list.Add("13", " Ambulatory Service Center Facility");
            list.Add("14", " Renal Supplies in the Home");
            list.Add("15", " Alternate Method Dialysis");
            list.Add("16", " Chronic Renal Disease (CRD) Equipment");
            list.Add("17", " Pre-Admission Testing");
            list.Add("18", " Durable Medical Equipment Rental");
            list.Add("19", " Pneumonia Vaccine");
            list.Add("20", " Second Surgical Opinion");
            list.Add("21", " Third Surgical Opinion");
            list.Add("22", " Social Work");
            list.Add("23", " Diagnostic Dental");
            list.Add("24", " Periodontics");
            list.Add("25", " Restorative");
            list.Add("26", " Endodontics");
            list.Add("27", " Maxillofacial Prosthetics");
            list.Add("28", " Adjunctive Dental Services");
            list.Add("30", " Health Benefit Plan Coverage ");
            list.Add("32", " Plan Waiting Period");
            list.Add("33", " Chiropractic");
            list.Add("34", " Chiropractic Office Visits");
            list.Add("35", " Dental Care");
            list.Add("36", " Dental Crowns");
            list.Add("37", " Dental Accident");
            list.Add("38", " Orthodontics");
            list.Add("39", " Prosthodontics");
            list.Add("40", " Oral Surgery");
            list.Add("41", " Routine (Preventive) Dental");
            list.Add("42", " Home Health Care");
            list.Add("43", " Home Health Prescriptions");
            list.Add("44", " Home Health Visits");
            list.Add("45", " Hospice");
            list.Add("46", " Respite Care");
            list.Add("47", " Hospital");
            list.Add("48", " Hospital - Inpatient");
            list.Add("49", " Hospital - Room and Board");
            list.Add("50", " Hospital - Outpatient");
            list.Add("51", " Hospital - Emergency Accident");
            list.Add("52", " Hospital - Emergency Medical");
            list.Add("53", " Hospital - Ambulatory Surgical");
            list.Add("54", " Long Term Care");
            list.Add("55", " Major Medical");
            list.Add("56", " Medically Related Transportation");
            list.Add("57", " Air Transportation");
            list.Add("58", " Cabulance");
            list.Add("59", " Licensed Ambulance");
            list.Add("60", " General Benefits");
            list.Add("61", " In-vitro Fertilization");
            list.Add("62", " MRI/CAT Scan");
            list.Add("63", " Donor Procedures");
            list.Add("64", " Acupuncture");
            list.Add("65", " Newborn Care");
            list.Add("66", " Pathology");
            list.Add("67", " Smoking Cessation");
            list.Add("68", " Well Baby Care");
            list.Add("69", " Maternity");
            list.Add("70", " Transplants");
            list.Add("71", " Audiology Exam");
            list.Add("72", " Inhalation Therapy");
            list.Add("73", " Diagnostic Medical");
            list.Add("74", " Private Duty Nursing");
            list.Add("75", " Prosthetic Device");
            list.Add("76", " Dialysis");
            list.Add("77", " Otological Exam");
            list.Add("78", " Chemotherapy");
            list.Add("79", " Allergy Testing");
            list.Add("80", " Immunizations");
            list.Add("81", " Routine Physical");
            list.Add("82", " Family Planning");
            list.Add("83", " Infertility");
            list.Add("84", " Abortion");
            list.Add("85", " AIDS");
            list.Add("86", " Emergency Services");
            list.Add("87", " Cancer");
            list.Add("88", " Pharmacy");
            list.Add("89", " Free Standing Prescription Drug");
            list.Add("90", " Mail Order Prescription Drug");
            list.Add("91", " Brand Name Prescription Drug");
            list.Add("92", " Generic Prescription Drug");
            list.Add("93", " Podiatry");
            list.Add("94", " Podiatry - Office Visits");
            list.Add("95", " Podiatry - Nursing Home Visits");
            list.Add("96", " Professional (Physician)");
            list.Add("97", " Anesthesiologist");
            list.Add("98", " Professional (Physician) Visit - Office");
            list.Add("99", " Professional (Physician) Visit - Inpatient");
            list.Add("A0", " Professional (Physician) Visit - Outpatient");
            list.Add("A1", " Professional (Physician) Visit - Nursing Home");
            list.Add("A2", " Professional (Physician) Visit - Skilled Nursing Facility");
            list.Add("A3", " Professional (Physician) Visit - Home");
            list.Add("A4", " Psychiatric");
            list.Add("A5", " Psychiatric - Room and Board");
            list.Add("A6", " Psychotherapy");
            list.Add("A7", " Psychiatric - Inpatient");
            list.Add("A8", " Psychiatric - Outpatient");
            list.Add("A9", " Rehabilitation");
            list.Add("AA", " Rehabilitation - Room and Board");
            list.Add("AB", " Rehabilitation - Inpatient");
            list.Add("AC", " Rehabilitation - Outpatient");
            list.Add("AD", " Occupational Therapy");
            list.Add("AE", " Physical Medicine");
            list.Add("AF", " Speech Therapy");
            list.Add("AG", " Skilled Nursing Care");
            list.Add("AH", " Skilled Nursing Care - Room and Board");
            list.Add("AI", " Substance Abuse");
            list.Add("AJ", " Alcoholism");
            list.Add("AK", " Drug Addiction");
            list.Add("AL", " Vision (Optometry)");
            list.Add("AM", " Frames");
            list.Add("AN", " Routine Exam ");
            list.Add("AO", " Lenses");
            list.Add("AQ", " Nonmedically Necessary Physical");
            list.Add("AR", " Experimental Drug Therapy");
            list.Add("B1", " Burn Care");
            list.Add("B2", " Brand Name Prescription Drug - Formulary");
            list.Add("B3", " Brand Name Prescription Drug - Non-Formulary");
            list.Add("BA", " Independent Medical Evaluation");
            list.Add("BB", " Partial Hospitalization (Psychiatric)");
            list.Add("BC", " Day Care (Psychiatric)");
            list.Add("BD", " Cognitive Therapy");
            list.Add("BE", " Massage Therapy");
            list.Add("BF", " Pulmonary Rehabilitation");
            list.Add("BG", " Cardiac Rehabilitation");
            list.Add("BH", " Pediatric");
            list.Add("BI", " Nursery");
            list.Add("BJ", " Skin");
            list.Add("BK", " Orthopedic");
            list.Add("BL", " Cardiac");
            list.Add("BM", " Lymphatic");
            list.Add("BN", " Gastrointestinal");
            list.Add("BP", " Endocrine");
            list.Add("BQ", " Neurology");
            list.Add("BR", " Eye");
            list.Add("BS", " Invasive Procedures");
            list.Add("BT", " Gynecological");
            list.Add("BU", " Obstetrical");
            list.Add("BV", " Obstetrical/Gynecological");
            list.Add("BW", " Mail Order Prescription Drug: Brand Name");
            list.Add("BX", " Mail Order Prescription Drug: Generic");
            list.Add("BY", " Physician Visit - Office: Sick");
            list.Add("BZ", " Physician Visit - Office: Well");
            list.Add("C1", " Coronary Care");
            list.Add("CA", " Private Duty Nursing - Inpatient");
            list.Add("CB", " Private Duty Nursing - Home");
            list.Add("CC", " Surgical Benefits - Professional (Physician)");
            list.Add("CD", " Surgical Benefits - Facility");
            list.Add("CE", " Mental Health Provider - Inpatient");
            list.Add("CF", " Mental Health Provider - Outpatient");
            list.Add("CG", " Mental Health Facility - Inpatient");
            list.Add("CH", " Mental Health Facility - Outpatient");
            list.Add("CI", " Substance Abuse Facility - Inpatient");
            list.Add("CJ", " Substance Abuse Facility - Outpatient");
            list.Add("CK", " Screening X-ray");
            list.Add("CL", " Screening laboratory");
            list.Add("CM", " Mammogram, High Risk Patient");
            list.Add("CN", " Mammogram, Low Risk Patient");
            list.Add("CO", " Flu Vaccination");
            list.Add("CP", " Eyewear and Eyewear Accessories");
            list.Add("CQ", " Case Management");
            list.Add("DG", " Dermatology");
            list.Add("DM", " Durable Medical Equipment");
            list.Add("DS", " Diabetic Supplies");
            list.Add("GF", " Generic Prescription Drug - Formulary");
            list.Add("GN", " Generic Prescription Drug - Non-Formulary");
            list.Add("GY", " Allergy");
            list.Add("IC", " Intensive Care");
            list.Add("MH", " Mental Health");
            list.Add("NI", " Neonatal Intensive Care");
            list.Add("ON", " Oncology");
            list.Add("PT", " Physical Therapy");
            list.Add("PU", " Pulmonary");
            list.Add("RN", " Renal");
            list.Add("RT", " Residential Psychiatric Treatment");
            list.Add("TC", " Transitional Care");
            list.Add("TN", " Transitional Nursery Care");
            list.Add("UC", " Urgent Care ");


            return list;
        }

        private Dictionary<string, string> GetEB_04_InsuranceType()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            list.Add("12", "Medicare Secondary Working Aged Beneficiary or Spouse with Employer Group Health Plan");
            list.Add("13", "Medicare Secondary End-Stage Renal Disease Beneficiary in the Mandated Coordination Period with an Employer’s Group Health Plan ");
            list.Add("14", "Medicare Secondary, No-fault Insurance including Auto is Primary");
            list.Add("15", "Medicare Secondary Worker’s Compensation");
            list.Add("16", "Medicare Secondary Public Health Service (PHS)or Other Federal Agency");
            list.Add("41", "Medicare Secondary Black Lung");
            list.Add("42", "Medicare Secondary Veteran’s Administration");
            list.Add("43", "Medicare Secondary Disabled Beneficiary Under Age 65 with Large Group Health Plan (LGHP)");
            list.Add("47", "Medicare Secondary, Other Liability Insurance is Primary");
            list.Add("AP", "Auto Insurance Policy");
            list.Add("C1", "Commercial");
            list.Add("CO", "Consolidated Omnibus Budget Reconciliation Act (COBRA)");
            list.Add("CP", "Medicare Conditionally Primary");
            list.Add("D ", "Disability");
            list.Add("DB", "Disability Benefits");
            list.Add("EP", "Exclusive Provider Organization");
            list.Add("FF", "Family or Friends");
            list.Add("GP", "Group Policy");
            list.Add("HM", "Health Maintenance Organization (HMO)");
            list.Add("HN", "Health Maintenance Organization (HMO) - Medicare Risk");
            list.Add("HS", "Special Low Income Medicare Beneficiary");
            list.Add("IN", "Indemnity");
            list.Add("IP", "Individual Policy");
            list.Add("LC", "Long Term Care");
            list.Add("LD", "Long Term Policy");
            list.Add("LI", "Life Insurance");
            list.Add("LT", "Litigation");
            list.Add("MA", "Medicare Part A");
            list.Add("MB", "Medicare Part B");
            list.Add("MC", "Medicaid");
            list.Add("MH", "Medigap Part A");
            list.Add("MI", "Medigap Part B");
            list.Add("MP", "Medicare Primary");
            list.Add("OT", "Other");
            list.Add("PE", "Property Insurance - Personal");
            list.Add("PL", "Personal");
            list.Add("PP", "Personal Payment (Cash - No Insurance)");
            list.Add("PR", "Preferred Provider Organization (PPO)");
            list.Add("PS", "Point of Service (POS)");
            list.Add("QM", "Qualified Medicare Beneficiary");
            list.Add("RP", "Property Insurance - Real");
            list.Add("SP", "Supplemental Policy");
            list.Add("TF", "Tax Equity Fiscal Responsibility Act (TEFRA)");
            list.Add("WC", "Workers Compensation");
            list.Add("WU", "Wrap Up Policy ");

            return list;
        }

        private Dictionary<string, string> GetEB_06_TimePeriod()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            list.Add("6", "Hour");
            list.Add("7", "Day");
            list.Add("13", "24 Hours");
            list.Add("21", "Years");
            list.Add("22", "Service Year");
            list.Add("23", "Calendar Year");
            list.Add("24", "Year to Date");
            list.Add("25", "Contract");
            list.Add("26", "Episode");
            list.Add("27", "Visit");
            list.Add("28", "Outlier");
            list.Add("29", "Remaining");
            list.Add("30", "Exceeded");
            list.Add("31", "Not Exceeded");
            list.Add("32", "Lifetime");
            list.Add("33", "Lifetime Remaining");
            list.Add("34", "Month");
            list.Add("35", "Week");
            list.Add("36", "Admission ");

            return list;
        }

        private Dictionary<string, string> GetEB_09_QuantityQualifier()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            list.Add("8H", "Minimum");
            list.Add("99", "Quantity Used");
            list.Add("CA", "Covered - Actual");
            list.Add("CE", "Covered - Estimated");
            list.Add("D3", "Number of Co-insurance Days");
            list.Add("DB", "Deductible Blood Units");
            list.Add("DY", "Days");
            list.Add("HS", "Hours");
            list.Add("LA", "Life-time Reserve - Actual");
            list.Add("LE", "Life-time Reserve - Estimated");
            list.Add("M2", "Maximum");
            list.Add("MN", "Month");
            list.Add("P6", "Number of Services or Procedures");
            list.Add("QA", "Quantity Approved");
            list.Add("S7", "Age, High Value ");
            list.Add("S8", "Age, Low Value ");
            list.Add("VS", "Visits");
            list.Add("YY", "Years");

            return list;
        }

        private Dictionary<string, string> GetEB_11_Authorization()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            list.Add("N", "No");
            list.Add("U", "Unknown");
            list.Add("Y", "Yes");

            return list;
        }

        private Dictionary<string, string> GetEB_12_PlanNetwork()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            list.Add("N", "No");
            list.Add("U", "Unknown");
            list.Add("W", "Plan Network Does not apply");
            list.Add("Y", "Yes");

            return list;
        }

        private Dictionary<string, string> GetREF()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            list.Add("18", "Plan Number");
            list.Add("1L", "Group or Policy Number");
            list.Add("1W", "Member Identification Number");
            list.Add("49", "Family Unit Number");
            list.Add("6P", "Group Number");
            list.Add("9F", "Referral Number");
            list.Add("A6", "Employee Identification Number");
            list.Add("F6", "Health Insurance Claim Number");
            list.Add("G1", "Prior Authorization Number");
            list.Add("1G", "Insurance Policy Number");
            list.Add("N6", "Plan Network Identification Number");
            list.Add("NQ", "Medicaid Recipient Identification Number");

            return list;
        }

        private Dictionary<string, string> GetDate()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            list.Add("096", "Discharge");
            list.Add("193", "Period Start");
            list.Add("194", "Period End");
            list.Add("198", "Completion");
            list.Add("290", "Coordination of Benefits");
            list.Add("291", "Plan");
            list.Add("292", "Benefit");
            list.Add("295", "Primary Care Provider");
            list.Add("304", "Latest Visit or Consultation");
            list.Add("307", "Eligibility");
            list.Add("318", "Added");
            list.Add("346", "Plan Begin");
            list.Add("347", "Plan End");
            list.Add("348", "Benefit Begin");
            list.Add("349", "Benefit End");
            list.Add("356", "Eligibility Begin");
            list.Add("357", "Eligibility End");
            list.Add("435", "Admission");
            list.Add("472", "Service");
            list.Add("636", "Date of Last Update");
            list.Add("771", "Status");


            return list;
        }

        private Dictionary<string, string> GetAAA()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            list.Add("71", "Patient Birth Date Does Not Match That for the Patient On the Database");
            list.Add("72", "Invalid/Missing Subscriber/Insured ID");
            list.Add("73", "Invalid/Missing Subscriber/Insured Name");
            
            return list;
        }
    }


    public class Output271
    {
        public string Transaction270 { get; set; }
        public string Transaction271 { get; set; }
        public string Transaction999 { get; set; }
        public string ErrorMessage { get; set; }
        public List<_271Header> EligibilityData { get; set; }
    }

}

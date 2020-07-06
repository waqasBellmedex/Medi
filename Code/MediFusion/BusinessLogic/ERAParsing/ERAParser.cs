using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MediFusionPM.BusinessLogic.EraParsing
{
    public class ERAParser
    {
        int _counter = 0;
        char E = ' ', C = ' ', S = ' ';
        string[] elements = null;
        string[] segments = null;
        List<ERAHeader> ERAData = new List<ERAHeader>();

        string ISA06, ISA08, ISAControlNum, GS02, GS03, GSConrolNum, Version;
        DateTime? ISADate;
        public List<ERAHeader> ParseERAFile(string FilePath)
        {
            ERAData = new List<ERAHeader>();
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
                elements = segments[_counter].Trim().Split(E);
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

            return ERAData;
        }

        private void ParseST()
        {
            ERAHeader eraHeader = new ERAHeader();
            eraHeader.ERAVisitPayments = new List<ERAVisitPayment>();

            eraHeader.ISA06SenderID = this.ISA06;
            eraHeader.ISA08ReceiverID = this.ISA08;
            eraHeader.ISADateTime = this.ISADate;
            eraHeader.ISAControlNumber = this.ISAControlNum;
            eraHeader.GS02SenderID = this.GS02;
            eraHeader.GS03ReceiverID = this.GS03;
            eraHeader.GSControlNumber = this.GSConrolNum;
            eraHeader.VersionNumber = this.Version;


            if (elements.GetElement(1) != "835") throw new Exception("Invalid File.");
            eraHeader.STControlNumber = elements.GetElement(2);
            _counter += 1;
            bool stCondition = true;
            while (stCondition)
            {
                elements = segments[_counter].Split(E);
                switch (elements.GetElement(0).Trim())
                {
                    case "BPR":
                        eraHeader.TransactionCode = elements.GetElement(1);
                        eraHeader.CheckAmount = decimal.Parse(elements.GetElement(2));
                        eraHeader.CreditDebitFlag = elements.GetElement(3);
                        eraHeader.PaymentMethod = elements.GetElement(4);
                        eraHeader.PaymentFormat = elements.GetElement(5);
                        eraHeader.CheckDate = Convert.ToDateTime(Utilities.GetDate(elements.GetElement(16), string.Empty));
                        break;

                    case "TRN":
                        eraHeader.CheckNumber = elements.GetElement(2);
                        eraHeader.PayerTrn = elements.GetElement(3);
                        break;

                    case "REF":
                        break;

                    case "DTM":
                        if (elements.GetElement(1) == "405")
                            eraHeader.ProductionDate = Utilities.GetDate(elements.GetElement(2), string.Empty);
                        break;

                    case "N1":
                        if (elements.GetElement(1) == "PR")
                        {
                            eraHeader.PayerName = elements.GetElement(2);
                            if (elements.Length >= 4) eraHeader.PayerID = elements.GetElement(4);
                            _counter += 1;
                            bool n1PRCondtion = true;
                            while (n1PRCondtion)
                            {
                                elements = segments[_counter].Split(E);
                                switch (elements.GetElement(0).Trim())
                                {
                                    case "N3":
                                        eraHeader.PayerAddress = elements.GetElement(1);
                                        break;
                                    case "N4":
                                        eraHeader.PayerCity = elements.GetElement(1);
                                        eraHeader.PayerState = elements.GetElement(2);
                                        eraHeader.PayerZip = elements.GetElement(3);
                                        break;
                                    case "REF":
                                        if (elements.GetElement(1) == "2U") eraHeader.REF2U = elements.GetElement(2);
                                        if (elements.GetElement(1) == "EO") eraHeader.REFEO = elements.GetElement(2);
                                        break;
                                    case "PER":
                                        if (elements.GetElement(1) == "CX")
                                        {
                                            eraHeader.PayerContactName = elements.GetElement(2);
                                            if (elements.GetElement(3) == "TE") eraHeader.PayerTelephone = elements.GetElement(4);
                                        }
                                        else if (elements.GetElement(1) == "BL")
                                        {
                                            eraHeader.PayerBillingContactName = elements.GetElement(2);
                                            if (elements.GetElement(3) == "TE") eraHeader.PayerBillingTelephone = elements.GetElement(4);
                                            else if (elements.GetElement(3) == "EM") eraHeader.PayerBillingEmail = elements.GetElement(4);

                                            if (elements.Length > 5)
                                            {
                                                if (elements.GetElement(5) == "TE") eraHeader.PayerBillingContactName = elements.GetElement(5);
                                                else if (elements.GetElement(5) == "EM") eraHeader.PayerBillingEmail = elements.GetElement(5);
                                            }
                                        }
                                        else if (elements.GetElement(1) == "IC")
                                        {
                                            if (elements.GetElement(3) == "UR") eraHeader.PayerWebsite = elements.GetElement(4);
                                        }
                                        break;

                                    case "N1":
                                        _counter -= 1; n1PRCondtion = false;
                                        break;
                                }
                                if (n1PRCondtion) _counter += 1;
                            }
                        }
                        else if (elements.GetElement(1) == "PE")
                        {
                            eraHeader.PayeeName = elements.GetElement(2);
                            if (elements.Length >= 4) eraHeader.PayeeNPI = elements.GetElement(4);
                            _counter += 1;
                            bool n1PECondtion = true;
                            while (n1PECondtion)
                            {
                                elements = segments[_counter].Split(E);
                                switch (elements.GetElement(0).Trim())
                                {
                                    case "N3":
                                        eraHeader.PayeeAddress = elements.GetElement(1);
                                        break;
                                    case "N4":
                                        eraHeader.PayeeCity = elements.GetElement(1);
                                        eraHeader.PayeeState = elements.GetElement(2);
                                        eraHeader.PayeeZip = elements.GetElement(3);
                                        break;
                                    case "REF":
                                        if (elements.GetElement(1) == "TJ") eraHeader.PayeeTaxID = elements.GetElement(2);
                                        break;
                                    case "LX":
                                    case "CLP":
                                    case "PLB":
                                    case "SE":
                                        _counter -= 1; n1PECondtion = false;
                                        break;
                                }
                                if (n1PECondtion) _counter += 1;
                            }
                        }
                        break;

                    case "LX":
                        break;
                    case "TS3":
                        break;

                    case "CLP":

                        ERAVisitPayment eraVisit = new ERAVisitPayment();
                        eraVisit.ERAChargePayments = new List<ERAChargePayment>();

                        eraVisit.PatientControlNumber = elements.GetElement(1);
                        eraVisit.ClaimProcessedAs = elements.GetElement(2);
                        eraVisit.SubmittedAmt = decimal.Parse(elements.GetElement(3));
                        eraVisit.PaidAmt = decimal.Parse(elements.GetElement(4));
                        eraVisit.PatResponsibilityAmt = string.IsNullOrEmpty(elements.GetElement(5)) ? default(decimal?) : decimal.Parse(elements.GetElement(5));
                        eraVisit.PayerControlNumber = elements.GetElement(7);
                        if (elements.Length > 8)
                        {
                            eraVisit.FacilityCode = elements.GetElement(8);
                            eraVisit.ClaimFrequencyCode = elements.GetElement(9);
                        }
                        _counter += 1;
                        bool clpCondtion = true;

                        while (clpCondtion)
                        {
                            elements = segments[_counter].Split(E);
                            switch (elements.GetElement(0).Trim())
                            {
                                case "CAS":
                                    if (elements.GetElement(1) == "CO")
                                    {

                                        for (int cc = 3; cc <= elements.Length; cc += 3)
                                        {
                                            if ((elements.GetElement(cc - 2) == "PR" &&
                                                       (elements.GetElement(cc - 1) == "1" || elements.GetElement(cc - 1) == "2" ||
                                                       elements.GetElement(cc - 1) == "3")) ||
                                                        cc > 3 && elements.GetElement(cc - 2 - 3) == "PR" && (elements.GetElement(cc - 1) == "1" || elements.GetElement(cc - 1) == "2" ||
                                                       elements.GetElement(cc - 1) == "3"))
                                            {
                                            }
                                            else
                                            {
                                                // CAS*CO*45*10.59**253*.08~
                                                string groupCode = elements.GetElement(cc - 2);
                                                if (groupCode.IsNull2())
                                                {
                                                    groupCode = elements.GetElement(cc - 2 - 3);
                                                    //groupCode = eraVisit.RemitCodes[eraVisit.RemitCodes.Count - 1].GroupCode;
                                                }

                                                eraVisit.RemitCodes.Add(new ERARemitCode()
                                                {
                                                    GroupCode = groupCode,
                                                    Amount = elements.GetElementAmount(cc),
                                                    ReasonCode = elements.GetElement(cc - 1)
                                                });
                                            }

                                            if (elements.GetElement(cc - 1) == "45" || elements.GetElement(cc - 1) == "253")
                                            {
                                                eraVisit.WriteOffAmt = eraVisit.WriteOffAmt.Val2() + decimal.Parse(elements.GetElement(cc));
                                            }
                                            else if (string.IsNullOrEmpty(eraVisit.OtherWriteOffCode1))
                                            {
                                                eraVisit.OtherWriteOffCode1 = elements.GetElement(cc - 1);
                                                eraVisit.OtherWriteOffAmt1 = decimal.Parse(elements.GetElement(cc));
                                            }
                                            else if (string.IsNullOrEmpty(eraVisit.OtherWriteOffCode2))
                                            {
                                                eraVisit.OtherWriteOffCode2 = elements.GetElement(cc - 1);
                                                eraVisit.OtherWriteOffAmt2 = decimal.Parse(elements.GetElement(cc));
                                            }
                                            else if (!string.IsNullOrEmpty(eraVisit.OtherWriteOffCode3))
                                            {
                                                eraVisit.OtherWriteOffCode3 = elements.GetElement(cc - 1);
                                                eraVisit.OtherWriteOffAmt3 = decimal.Parse(elements.GetElement(cc));
                                            }
                                            else if (!string.IsNullOrEmpty(eraVisit.OtherWriteOffCode4))
                                            {
                                                eraVisit.OtherWriteOffCode4 = elements.GetElement(cc - 1);
                                                eraVisit.OtherWriteOffAmt4 = decimal.Parse(elements.GetElement(cc));
                                            }
                                            else if (!string.IsNullOrEmpty(eraVisit.OtherWriteOffCode5))
                                            {
                                                eraVisit.OtherWriteOffCode5 = elements.GetElement(cc - 1);
                                                eraVisit.OtherWriteOffAmt5 = decimal.Parse(elements.GetElement(cc));
                                            }
                                            else
                                            {

                                            }
                                        }
                                    }
                                    else if (elements.GetElement(1) == "OA")
                                    {
                                        if (elements.GetElement(2) == "23") eraVisit.OtherAdjustmentAmt = decimal.Parse(elements.GetElement(3));
                                    }
                                    else if (elements.GetElement(1) == "PR")
                                    {
                                        if (elements.GetElement(2) == "1") eraVisit.DeductableAmt = decimal.Parse(elements.GetElement(3));
                                        else if (elements.GetElement(2) == "2") eraVisit.CoInsuranceAmt = decimal.Parse(elements.GetElement(3));
                                        else if (elements.GetElement(2) == "3") eraVisit.CopayAmt = decimal.Parse(elements.GetElement(3));

                                        if (elements.Length > 5)
                                        {
                                            if (elements.GetElement(5) == "1") eraVisit.DeductableAmt = decimal.Parse(elements.GetElement(6));
                                            else if (elements.GetElement(5) == "2") eraVisit.CoInsuranceAmt = decimal.Parse(elements.GetElement(6));
                                            else if (elements.GetElement(5) == "3") eraVisit.CopayAmt = decimal.Parse(elements.GetElement(6));
                                        }
                                    }

                                    break;
                                case "NM1":
                                    if (elements.GetElement(1) == "QC")
                                    {
                                        eraVisit.SubscriberLastName = elements.GetElement(3);
                                        eraVisit.SubscirberFirstName = elements.GetElement(4);
                                        if (elements.Length < 6) break;
                                        eraVisit.SubscriberMI = elements.GetElement(5);
                                        eraVisit.SubscriberID = elements.GetElement(9);
                                    }
                                    else if (elements.GetElement(1) == "74") { }
                                    else if (elements.GetElement(1) == "82")
                                    {
                                        eraVisit.RendPrvLastName = elements.GetElement(3);
                                        eraVisit.RendPrvFirstName = elements.GetElement(4);
                                        eraVisit.RendPrvMI = elements.GetElement(5);
                                        eraVisit.RendPrvNPI = elements.GetElement(9);
                                    }
                                    else if (elements.GetElement(1) == "TT")
                                    {
                                        eraVisit.CrossOverPayerName = elements.GetElement(3);
                                        eraVisit.CrossOverPayerID = elements.GetElement(9);
                                    }
                                    else if (elements.GetElement(1) == "IL")
                                    {
                                        eraVisit.SubscriberLastName = elements.GetElement(3);
                                        eraVisit.SubscirberFirstName = elements.GetElement(4);
                                        if (elements.Length < 6) break;
                                        eraVisit.SubscriberMI = elements.GetElement(5);
                                        eraVisit.SubscriberID = elements.GetElement(9);
                                    }
                                    break;

                                case "MIA":
                                    break;
                                case "MOA":
                                    break;

                                case "REF":
                                    if (elements.GetElement(1) == "1L") { }
                                    else if (elements.GetElement(1) == "CE") { }
                                    else if (elements.GetElement(1) == "EA") { }
                                    break;

                                case "DTM":
                                    if (elements.GetElement(1) == "232") eraVisit.ClaimStatementFrom = Utilities.GetDate(elements.GetElement(2), string.Empty);
                                    else if (elements.GetElement(1) == "233") eraVisit.ClaimStatementTo = Utilities.GetDate(elements.GetElement(2), string.Empty);
                                    else if (elements.GetElement(1) == "050") eraVisit.ClaimReceivedDate = Utilities.GetDate(elements.GetElement(2), string.Empty);
                                    break;

                                case "PER":
                                    eraVisit.ClaimContactNumber = elements.GetElement(2);
                                    if (elements.GetElement(3) == "TE") eraVisit.ClaimTelephone = elements.GetElement(4);
                                    break;

                                case "AMT":
                                    if (elements.GetElement(1) == "AU") eraVisit.ClaimCoverageAmt = decimal.Parse(elements.GetElement(2));
                                    break;

                                case "SVC":
                                    ERAChargePayment eraCharge = new ERAChargePayment();

                                    string[] svc01 = elements.GetElement(1).Split(C);
                                    eraCharge.CPTCode = elements.GetElement(1).Split(C)[1];
                                    if (svc01.Length > 2) eraCharge.Modifier1 = svc01[2];
                                    if (svc01.Length > 3) eraCharge.Modifier2 = svc01[3];
                                    if (svc01.Length > 4) eraCharge.Modifier3 = svc01[4];
                                    if (svc01.Length > 5) eraCharge.Modifier4 = svc01[5];
                                    if (svc01.Length > 6) eraCharge.CPTDescription = svc01[6];
                                    eraCharge.SubmittedAmt = decimal.Parse(elements.GetElement(2));
                                    eraCharge.PaidAmt = decimal.Parse(elements.GetElement(3));
                                    eraCharge.UnitsPaid = elements.GetElement(5);

                                    _counter += 1;
                                    bool svcCondtion = true;
                                    int lqCounter = 0;
                                    while (svcCondtion)
                                    {
                                        elements = segments[_counter].Split(E);
                                        switch (elements.GetElement(0).Trim())
                                        {
                                            case "DTM":
                                                if (elements.GetElement(1) == "150") eraCharge.ServiceDateFrom = Utilities.GetDate(elements.GetElement(2), string.Empty);
                                                else if (elements.GetElement(1) == "151") eraCharge.ServiceDateTo = Utilities.GetDate(elements.GetElement(2), string.Empty);
                                                else if (elements.GetElement(1) == "472") eraCharge.ServiceDateFrom = Utilities.GetDate(elements.GetElement(2), string.Empty);
                                                break;

                                            case "CAS":

                                                for (int cc = 3; cc <= elements.Length; cc += 3)
                                                {
                                                    if ((elements.GetElement(cc - 2) == "PR" &&
                                                        (elements.GetElement(cc - 1) == "1" || elements.GetElement(cc - 1) == "2" ||
                                                        elements.GetElement(cc - 1) == "3")) ||
                                                         cc > 3 && elements.GetElement(cc - 2 - 3) == "PR" && (elements.GetElement(cc - 1) == "1" || elements.GetElement(cc - 1) == "2" ||
                                                        elements.GetElement(cc - 1) == "3"))
                                                    {
                                                    }
                                                    else
                                                    {
                                                        // CAS*CO*45*10.59**253*.08~
                                                        string groupCode = elements.GetElement(cc - 2);
                                                        if (groupCode.IsNull2())
                                                        {
                                                            groupCode = elements.GetElement(cc - 2 - 3);
                                                            //groupCode = eraCharge.RemitCodes[eraCharge.RemitCodes.Count - 1].GroupCode;
                                                        }

                                                        eraCharge.RemitCodes.Add(new ERARemitCode()
                                                        {
                                                            GroupCode = groupCode,
                                                            Amount = elements.GetElementAmount(cc),
                                                            ReasonCode = elements.GetElement(cc - 1)
                                                        });
                                                    }
                                                }


                                                if (elements.GetElement(1) == "CO")
                                                {
                                                    for (int cc = 3; cc <= elements.Length; cc += 3)
                                                    {
                                                        if (elements.GetElement(cc - 1) == "45" || elements.GetElement(cc - 1) == "253")
                                                        {
                                                            eraCharge.WriteOffAmt = eraCharge.WriteOffAmt.Val2() +  decimal.Parse(elements.GetElement(cc));
                                                        }
                                                        else if (string.IsNullOrEmpty(eraCharge.OtherWriteOffCode1))
                                                        {
                                                            eraCharge.OtherWriteOffCode1 = elements.GetElement(cc - 1);
                                                            eraCharge.OtherWriteOffAmt1 = decimal.Parse(elements.GetElement(cc));
                                                        }
                                                        else if (string.IsNullOrEmpty(eraCharge.OtherWriteOffCode2))
                                                        {
                                                            eraCharge.OtherWriteOffCode2 = elements.GetElement(cc - 1);
                                                            eraCharge.OtherWriteOffAmt2 = decimal.Parse(elements.GetElement(cc));
                                                        }
                                                        else if (!string.IsNullOrEmpty(eraCharge.OtherWriteOffCode3))
                                                        {
                                                            eraCharge.OtherWriteOffCode3 = elements.GetElement(cc - 1);
                                                            eraCharge.OtherWriteOffAmt3 = decimal.Parse(elements.GetElement(cc));
                                                        }
                                                        else if (!string.IsNullOrEmpty(eraCharge.OtherWriteOffCode4))
                                                        {
                                                            eraCharge.OtherWriteOffCode4 = elements.GetElement(cc - 1);
                                                            eraCharge.OtherWriteOffAmt4 = decimal.Parse(elements.GetElement(cc));
                                                        }
                                                        else if (!string.IsNullOrEmpty(eraCharge.OtherWriteOffCode5))
                                                        {
                                                            eraCharge.OtherWriteOffCode5 = elements.GetElement(cc - 1);
                                                            eraCharge.OtherWriteOffAmt5 = decimal.Parse(elements.GetElement(cc));
                                                        }
                                                        else
                                                        {

                                                        }
                                                    }
                                                }
                                                else if (elements.GetElement(1) == "OA")
                                                {
                                                    if (elements.GetElement(2) == "23") eraCharge.OtherAdjustmentAmt = decimal.Parse(elements.GetElement(3));
                                                }
                                                else if (elements.GetElement(1) == "PR")
                                                {
                                                    if (elements.GetElement(2) == "1") eraCharge.DeductableAmt = decimal.Parse(elements.GetElement(3));
                                                    else if (elements.GetElement(2) == "2") eraCharge.CoInsuranceAmt = decimal.Parse(elements.GetElement(3));
                                                    else if (elements.GetElement(2) == "3") eraCharge.CopayAmt = decimal.Parse(elements.GetElement(3));

                                                    if (elements.Length > 5)
                                                    {
                                                        if (elements.GetElement(5) == "1") eraCharge.DeductableAmt = decimal.Parse(elements.GetElement(6));
                                                        else if (elements.GetElement(5) == "2") eraCharge.CoInsuranceAmt = decimal.Parse(elements.GetElement(6));
                                                        else if (elements.GetElement(5) == "3") eraCharge.CopayAmt = decimal.Parse(elements.GetElement(6));
                                                    }
                                                }
                                                break;
                                            case "REF":
                                                if (elements.GetElement(1) == "6R") eraCharge.ChargeControlNumber = elements.GetElement(2);
                                                break;
                                            case "AMT":
                                                if (elements.GetElement(1) == "B6") eraCharge.AllowedAmount = decimal.Parse(elements.GetElement(2));
                                                break;
                                            case "LQ":
                                                if (elements.GetElement(1) == "HE")
                                                {
                                                    lqCounter += 1;
                                                    if (lqCounter == 1) eraCharge.RemarkCode1 = elements.GetElement(2);
                                                    if (lqCounter == 2) eraCharge.RemarkCode2 = elements.GetElement(2);
                                                    if (lqCounter == 3) eraCharge.RemarkCode3 = elements.GetElement(2);
                                                    if (lqCounter == 4) eraCharge.RemarkCode4 = elements.GetElement(2);
                                                    if (lqCounter == 5) eraCharge.RemarkCode5 = elements.GetElement(2);
                                                    if (lqCounter == 6) eraCharge.RemarkCode6 = elements.GetElement(2);
                                                    if (lqCounter == 7) eraCharge.RemarkCode7 = elements.GetElement(2);
                                                    if (lqCounter == 7) eraCharge.RemarkCode8 = elements.GetElement(2);
                                                    if (lqCounter == 9) eraCharge.RemarkCode9 = elements.GetElement(2);
                                                    if (lqCounter == 10) eraCharge.RemarkCode10 = elements.GetElement(2);
                                                }
                                                break;
                                            case "SVC":
                                            case "CLP":
                                            case "SE":
                                            case "PLB":
                                                _counter -= 1; svcCondtion = false;
                                                if (!eraVisit.ERAChargePayments.Contains(eraCharge)) eraVisit.ERAChargePayments.Add(eraCharge);
                                                break;
                                        }
                                        if (svcCondtion) _counter += 1;
                                    }
                                    break;

                                case "SE":
                                case "PLB":
                                case "CLP":
                                    _counter -= 1; clpCondtion = false;
                                    if (!eraHeader.ERAVisitPayments.Contains(eraVisit)) eraHeader.ERAVisitPayments.Add(eraVisit);
                                    break;
                            }
                            if (clpCondtion) _counter += 1;
                        }
                        break;
                    case "SE":
                    case "GE":
                    case "IEA":
                        _counter -= 1; stCondition = false;
                        if (!ERAData.Contains(eraHeader)) ERAData.Add(eraHeader);
                        break;
                }
                _counter += 1;
            }
        }
    }
}

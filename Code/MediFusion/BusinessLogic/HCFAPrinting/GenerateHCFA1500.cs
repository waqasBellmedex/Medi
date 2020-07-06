using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.BusinessLogic.ClaimGeneration;
using static MediFusionPM.BusinessLogic.ClaimGeneration.ClaimGenerator.Output;
using System.Diagnostics;
using System.util;

namespace MediFusionPM.BusinessLogic.HCFAPrinting
{
    public class GenerateHCFA1500
    {
        private bool _validateClaims = true;
        private string _networkPath = string.Empty;

        public GenerateHCFA1500(bool ValidateClaims, string NetworkPath)
        {
            _validateClaims = ValidateClaims;
            _networkPath = NetworkPath;
        }
        public HcfaOutput GenerateHcfa(ClaimHeader Header, string ResourceFile, string OutputDirectory)
        {
            var ListofAllFiles = new List<string>();
            HcfaOutput output = new HcfaOutput() { Claims = new List<ClaimResult>() };
            Directory.CreateDirectory(OutputDirectory);

            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(Header);
            File.WriteAllText(Path.Combine(OutputDirectory, "Input.Json"), jsonString);

            try
            {
                PdfReader.unethicalreading = true;

                string cms1500Template = ResourceFile;

                foreach (ClaimData CLM in Header.Claims)
                {
                    if (_validateClaims)
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
                        });
                        continue;
                        }
                    }

                    CLM.ClaimAmount = CLM.Charges.Sum(c => c.ChargeAmount.Value);

                    string generatedHcfa = Path.Combine(OutputDirectory, string.Format("{0}.pdf", CLM.PatientControlNumber));
                    var pdfReader = new PdfReader(cms1500Template);
                    var pdfStamper = new PdfStamper(pdfReader, new FileStream(generatedHcfa, FileMode.Create));

                    AcroFields pdfFormFields = pdfStamper.AcroFields;

                    if (pdfFormFields.Fields == null || pdfFormFields.Fields.Count == 0)
                    {
                        //cms1500PDF = "Error:Unable to create CMS 1500 Form";
                        //return cms1500PDF;
                    }

                    foreach (var entry in pdfFormFields.Fields)
                    {
                        string fieldName = entry.Key.ToString();
                        string pdob = string.Empty;
                        string pointers = string.Empty;
                        string DateOfCIIP = string.Empty, DateOfCIIPQual = string.Empty, OtherDate = string.Empty, OtherDateQual = string.Empty, tempDate = string.Empty;

                        try
                        {
                            if (CLM.CurrentIllnessDate != null && CLM.CurrentIllnessDate.HasValue)
                            {
                                DateOfCIIPQual = "431";
                                DateOfCIIP = string.Format("{0:MM/dd/yyyy}", CLM.CurrentIllnessDate.Value);
                            }
                            else if (CLM.LMPDate != null && CLM.LMPDate.HasValue)
                            {
                                DateOfCIIPQual = "484";
                                DateOfCIIP = string.Format("{0:MM/dd/yyyy}", CLM.LMPDate.Value);
                            }

                            //--------------------------------------------------------------------------------------------

                            if (CLM.InitialTreatmentDate != null && CLM.InitialTreatmentDate.HasValue)
                            {
                                OtherDateQual = "454";
                                OtherDate = string.Format("{0:MM/dd/yyyy}", CLM.InitialTreatmentDate.Value);
                            }
                            else if (CLM.LastSeenDate != null && CLM.LastSeenDate.HasValue)
                            {
                                OtherDateQual = "304";
                                OtherDate = string.Format("{0:MM/dd/yyyy}", CLM.LastSeenDate.Value);
                            }
                            else if (CLM.AcuteManifestationDate != null && CLM.AcuteManifestationDate.HasValue)
                            {
                                OtherDateQual = "453";
                                OtherDate = string.Format("{0:MM/dd/yyyy}", CLM.AcuteManifestationDate.Value);
                            }
                            else if (CLM.AccidentDate != null && CLM.AccidentDate.HasValue)
                            {
                                OtherDateQual = "439";
                                OtherDate = string.Format("{0:MM/dd/yyyy}", CLM.AccidentDate.Value);
                            }
                            else if (CLM.XrayDate != null && CLM.XrayDate.HasValue)
                            {
                                OtherDateQual = "455";
                                OtherDate = string.Format("{0:MM/dd/yyyy}", CLM.XrayDate.Value);
                            }

                            //--------------------------------------------------------------------------------------------

                            #region "Fields"
                            switch (fieldName)
                            {
                                #region Payer Info
                                // PAYER NAME
                                case "insurance_name":
                                    pdfFormFields.SetField(fieldName, CLM.PayerName.Trim().ToUpper());
                                    break;
                                // PAYER ADDRESS LINE 1
                                case "insurance_address":
                                    if(CLM.PayerAddress!=null)
                                        pdfFormFields.SetField(fieldName, CLM.PayerAddress.Trim().ToUpper());
                                    break;
                                // PAYER ADDRESS LINE 2
                                case "insurance_address2":
                                    //pdfFormFields.SetField(fieldName, CLM.Payer.Trim().ToUpper());
                                    break;
                                // PAYER ADDRESS LINE 3
                                case "insurance_city_state_zip":
                                    string cityStateZip = CLM.PayerCity + " " + CLM.PayerState + " " + CLM.PayerZipCode;
                                    pdfFormFields.SetField(fieldName, cityStateZip.Trim().ToUpper());
                                    break;
                                #endregion

                                #region Claim Type
                                // 1. MEDICARE FLAG 2. MEDICAID FLAG  3. TRICARE  4. CHAPMVA 5. GROUP HEALTH PLAN 6. FECA 7. OTHER 
                                case "insurance_type":
                                    // We have only Medicaid at this time.
                                    string insType = "";
                                    if (CLM.PayerType == "MA" || CLM.PayerType == "MB") insType = "Medicare";
                                    else if (CLM.PayerType == "MC") insType = "Medicaid";
                                    else if (CLM.PayerType == "BL") insType = "Tricare";
                                    else if (CLM.PayerType == "CH") insType = "Champva";
                                    else insType = "Other";

                                    pdfFormFields.SetField(fieldName, insType);
                                    break;

                                #endregion

                                #region Insured Info

                                // INSURED'S ID NUMBER
                                case "insurance_id":
                                    pdfFormFields.SetField(fieldName, CLM.SBRID.Trim().ToUpper());
                                    break;

                                // INSURED'S NAME (LAST NAME, FIRST NAME, MI)
                                case "ins_name":
                                    string name = CLM.SBRLastName + ", " + CLM.SBRFirstName + (CLM.SBRMiddleInitial.IsNull() ? "" : ", " + CLM.SBRMiddleInitial);
                                    pdfFormFields.SetField(fieldName, name.ToUpper());
                                    break;

                                // INSURED ADDRESS
                                case "ins_street":
                                    pdfFormFields.SetField(fieldName, CLM.SBRAddress.ToUpper());
                                    break;
                                // INSURED'S CITY 
                                case "ins_city":
                                    pdfFormFields.SetField(fieldName, CLM.SBRCity.ToUpper());
                                    break;
                                // INSURED'S STATE
                                case "ins_state":
                                    pdfFormFields.SetField(fieldName, CLM.SBRState.ToUpper());
                                    break;
                                // INSURED'S ZIP
                                case "ins_zip":
                                    pdfFormFields.SetField(fieldName, CLM.SBRZipCode);
                                    break;
                                // INSURED'S PHONE (333)
                                case "ins_phone area":
                                    //pdfFormFields.SetField(fieldName, CLM.Sbr)
                                    break;
                                // INSURED'S PHONE (2854078)
                                case "ins_phone": break;

                                // INSURED'S POLICY GROUP OR FECA NUMBER
                                case "ins_policy":
                                    pdfFormFields.SetField(fieldName, CLM.SbrGroupNumber.ToUpper());
                                    break;

                                // INSURED DOB'S YEAR
                                case "ins_dob_yy":
                                    pdob = string.Format("{0:MM/dd/yyyy}", CLM.SBRDob);
                                    pdfFormFields.SetField(fieldName, pdob.Substring(6, 4));
                                    break;

                                // INSURED DOB'S DAY
                                case "ins_dob_dd":
                                    pdob = string.Format("{0:MM/dd/yyyy}", CLM.SBRDob);
                                    pdfFormFields.SetField(fieldName, pdob.Substring(3, 2));
                                    break;

                                // INSURED DOB'S MONTH
                                case "ins_dob_mm":
                                    pdob = string.Format("{0:MM/dd/yyyy}", CLM.SBRDob);
                                    pdfFormFields.SetField(fieldName, pdob.Substring(0, 2));
                                    break;


                                // INSURED'S GENDER: 1 - MALE,  2 - FEMALE
                                case "ins_sex":
                                    if (CLM.SBRGender == "M")
                                        pdfFormFields.SetField(fieldName, "MALE");
                                    else if (CLM.SBRGender == "F")
                                        pdfFormFields.SetField(fieldName, "FEMALE");
                                    break;


                                // INSURANCE PLAN NAME OR PROGRAM NAME
                                case "ins_plan_name":
                                    if (CLM.SbrGroupName != null)
                                        pdfFormFields.SetField(fieldName, CLM.SbrGroupName.ToUpper());
                                    break;

                                // IS THERE ANOTHER HEALTH BENEFIT PLAN: 1 - YES,  2 - NO
                                case "ins_benefit_plan":
                                    pdfFormFields.SetField(fieldName, "NO");
                                    break;

                                // INSURED'S OR AUTHORIZED PERSON'S SIGNATURE
                                case "ins_signature":
                                    pdob = string.Format("{0:MM/dd/yyyy}", DateTime.Now);
                                    pdfFormFields.SetField(fieldName, pdob.Substring(6, 4) + pdob.Substring(0, 2) + pdob.Substring(3, 2));
                                    break;

                                #endregion

                                #region Patient
                                // No patient info available. We have only self subscribed claims for now. 

                                // PATIENT NAME 
                                case "pt_name":
                                    name = CLM.PATLastName + ", " + CLM.PATFirstName + (CLM.PATMiddleInitial.IsNull2() ? "" : ", " + CLM.PATMiddleInitial);
                                    pdfFormFields.SetField(fieldName, name.ToUpper());
                                    break;

                                // PATIENT'S DOB'S YEAR
                                case "birth_yy":
                                    pdob = string.Format("{0:MM/dd/yyyy}", CLM.PATDob);
                                    pdfFormFields.SetField(fieldName, pdob.Substring(6, 4));
                                    break;
                                // PATIENT'S DOB'S DAY
                                case "birth_dd":
                                    pdob = string.Format("{0:MM/dd/yyyy}", CLM.PATDob);
                                    pdfFormFields.SetField(fieldName, pdob.Substring(3, 2));
                                    break;
                                // PATIENT'S DOB'S MONTH
                                case "birth_mm":
                                    pdob = string.Format("{0:MM/dd/yyyy}", CLM.PATDob);
                                    pdfFormFields.SetField(fieldName, pdob.Substring(0, 2));
                                    break;

                                // PATIENT ADDRESS
                                case "pt_street":
                                    //address = ClientDT.Rows[0]["Address1"].ToString().Trim() + " " + ClientDT.Rows[0]["Address2"].ToString().Trim();
                                    pdfFormFields.SetField(fieldName, CLM.PATAddress.ToUpper());
                                    break;

                                // PATIENT CITY
                                case "pt_city":
                                    pdfFormFields.SetField(fieldName, CLM.PATCity.ToUpper());
                                    break;
                                // PATIENT STATE
                                case "pt_state":
                                    pdfFormFields.SetField(fieldName, CLM.PATState.ToUpper());
                                    break;
                                // PATIENT ZIP
                                case "pt_zip":
                                    pdfFormFields.SetField(fieldName, CLM.PATZipCode.ToUpper());
                                    break;
                                // PATIENT PHONE (333)
                                case "pt_AreaCode": break;
                                // PATIENT PHONE (2854078)
                                case "pt_phone": break;
                                // PATIENT RELATIONSHIP TO INSURED
                                case "rel_to_ins":
                                    string relationCode = "";
                                    if (CLM.PatientRelationShip == "18") relationCode = "S";
                                    pdfFormFields.SetField(fieldName, relationCode);
                                    break;

                                // PATIENT'S GENDER   
                                case "sex":
                                    if (CLM.PATGender == "M")
                                        pdfFormFields.SetField(fieldName, "MALE");
                                    else if (CLM.PATGender == "F")
                                        pdfFormFields.SetField(fieldName, "FEMALE");
                                    break;


                                // PATIENT'S OR AUTHORIZED PERSON'S SIGNATURE DATE
                                case "pt_signature":
                                    break;
                                // PATIENTS'S OR AUTHORIZED PERSON'S SIGNATURE
                                case "pt_date":
                                    break;

                                #endregion

                                #region Patient Condition
                                // EMPLOYMENT
                                case "employment":
                                    pdfFormFields.SetField(fieldName, CLM.AccidentType == "EM" ? "YES" : "NO");
                                    break;
                                // AUTO ACCIDENT
                                case "pt_auto_accident":
                                    pdfFormFields.SetField(fieldName, CLM.AccidentType == "AA" ? "YES" : "NO");
                                    break;
                                // OTHER ACCIDENT 
                                case "other_accident":
                                    pdfFormFields.SetField(fieldName, CLM.AccidentType == "OA" ? "YES" : "NO");
                                    break;
                                // ACCIDENT STATE
                                case "accident_place":
                                    pdfFormFields.SetField(fieldName, CLM.AccidentState);
                                    break;

                                #endregion

                                #region OtherInsured Info
                                // OTHER INSURED NAME (LAST NAME, FIRST NAME, MI)
                                case "other_ins_name":
                                    string OtherSbrName = CLM.OtherSBRLastName + ", " + CLM.OtherSBRFirstName + (CLM.OtherSBRMI.IsNull() ? "" : ", " + CLM.OtherSBRMI);
                                    pdfFormFields.SetField(fieldName, OtherSbrName.ToUpper());
                                    break;
                                // OTHER INSURED'S POLICY OR GROUP NUMBER
                                case "other_ins_policy":
                                    if (CLM.OtherSBRGroupNumber != null)
                                        pdfFormFields.SetField(fieldName, CLM.OtherSBRGroupNumber.ToUpper());
                                    break;

                                // OTHER INSURANCE PLAN NAME OR PROGRAM NAME
                                case "other_ins_plan_name":
                                    pdfFormFields.SetField(fieldName, CLM.OtherSBRGroupName);
                                    break;
                                #endregion



                                #region Dates

                                // DATE OF CURRENT ILLNESS, INJURY OR PREGNANCY(LMP) QUAL
                                case "73":
                                    if(DateOfCIIPQual!=null && DateOfCIIPQual!= "")
                                        pdfFormFields.SetField(fieldName, DateOfCIIPQual);
                                    break;
                                // DATE OF CURRENT ILLNESS, INURY, PREGNANCY(LMP) - YEAR
                                case "cur_ill_yy":
                                    if (DateOfCIIP != null && DateOfCIIP != "")
                                        pdfFormFields.SetField(fieldName, DateOfCIIP.Substring(6, 4));
                                    break;
                                // DATE OF CURRENT ILLNESS, INURY, PREGNANCY(LMP) - DAY
                                case "cur_ill_dd":
                                    if (DateOfCIIP != null && DateOfCIIP != "")
                                        pdfFormFields.SetField(fieldName, DateOfCIIP.Substring(3, 2));
                                    break;
                                // DATE OF CURRENT ILLNESS, INURY, PREGNANCY(LMP) - MONTH
                                case "cur_ill_mm":
                                    if (DateOfCIIP != null && DateOfCIIP != "")
                                        pdfFormFields.SetField(fieldName, DateOfCIIP.Substring(0, 2));
                                    break;

                                // OTHER DATE QUAL 
                                case "74":
                                    if (OtherDateQual != null && OtherDateQual != "")
                                        pdfFormFields.SetField(fieldName, OtherDateQual);
                                    break;
                                // OTHER DATE YEAR 
                                case "sim_ill_yy":
                                    if (OtherDate != null && OtherDate != "")
                                        pdfFormFields.SetField(fieldName, OtherDate.Substring(6, 4));
                                    break;
                                // OTHER DATE DAY
                                case "sim_ill_dd":
                                    if (OtherDate != null && OtherDate != "")
                                        pdfFormFields.SetField(fieldName, OtherDate.Substring(3, 2));
                                    break;
                                // OTHER DATE MONTH
                                case "sim_ill_mm":
                                    if (OtherDate != null && OtherDate != "")
                                        pdfFormFields.SetField(fieldName, OtherDate.Substring(0, 2));
                                    break;


                                // DATES PATIENT UNABLE TO  WORK IN CURRENT OCCUPATION - FROM YEAR
                                case "work_yy_from":
                                    pdfFormFields.SetField(fieldName, GetDatePart(CLM.LastWorkedDate, "yy"));
                                    break;
                                // DATES PATIENT UNABLE TO  WORK IN CURRENT OCCUPATION - FROM DAY
                                case "work_dd_from":
                                    pdfFormFields.SetField(fieldName, GetDatePart(CLM.LastWorkedDate, "dd"));
                                    break;
                                // DATES PATIENT UNABLE TO  WORK IN CURRENT OCCUPATION - FROM MONTH
                                case "work_mm_from":
                                    pdfFormFields.SetField(fieldName, GetDatePart(CLM.LastWorkedDate, "MM"));
                                    break;

                                // DATES PATIENT UNABLE TO  WORK IN CURRENT OCCUPATION - TO YEAR
                                case "work_yy_end":
                                    break;
                                // DATES PATIENT UNABLE TO  WORK IN CURRENT OCCUPATION - TO MONTH
                                case "work_mm_end":
                                    break;
                                // DATES PATIENT UNABLE TO  WORK IN CURRENT OCCUPATION - TO DAY
                                case "work_dd_end":
                                    break;

                                // HOSPITALIZATION DATES RELATED TO CURRENT SERVICES - FROM MONTH
                                case "hosp_mm_from":
                                    pdfFormFields.SetField(fieldName, GetDatePart(CLM.AdmissionDate, "MM"));
                                    break;
                                // HOSPITALIZATION DATES RELATED TO CURRENT SERVICES - FROM DAY
                                case "hosp_dd_from":
                                    pdfFormFields.SetField(fieldName, GetDatePart(CLM.AdmissionDate, "dd"));
                                    break;
                                // HOSPITALIZATION DATES RELATED TO CURRENT SERVICES - FROM YEAR
                                case "hosp_yy_from":
                                    pdfFormFields.SetField(fieldName, GetDatePart(CLM.AdmissionDate, "yy"));
                                    break;

                                // HOSPITALIZATION DATES RELATED TO CURRENT SERVICES - TO YEAR
                                case "hosp_yy_end":
                                    pdfFormFields.SetField(fieldName, GetDatePart(CLM.DischargeDate, "yy"));
                                    break;
                                // HOSPITALIZATION DATES RELATED TO CURRENT SERVICES - TO MONTH
                                case "hosp_mm_end":
                                    pdfFormFields.SetField(fieldName, GetDatePart(CLM.DischargeDate, "MM"));
                                    break;
                                // HOSPITALIZATION DATES RELATED TO CURRENT SERVICES - TO DAY
                                case "hosp_dd_end":
                                    pdfFormFields.SetField(fieldName, GetDatePart(CLM.DischargeDate, "dd"));
                                    break;


                                #endregion

                                #region Refering/Ordering/Supervising Provider
                                // NAME OF REFERING PROVIDER OR OTHER SOURCE QUALIFIER
                                case "85":
                                    string qual = string.Empty;
                                    if (!CLM.RefPrvNPI.IsNull()) qual = "DN";
                                    else if (!CLM.SuperPrvNPI.IsNull()) qual = "DQ";
                                    pdfFormFields.SetField(fieldName, qual);
                                    break;


                                // NAME OF REFERING PROVIDER OR OTHER SOURCE
                                case "ref_physician":
                                    string prvName = string.Empty;
                                    if (!CLM.RefPrvLastName.IsNull())
                                        prvName = CLM.RefPrvLastName + ", " + CLM.RefPrvFirstName;
                                    else if (!CLM.SuperPrvLastName.IsNull())
                                        prvName = CLM.SuperPrvLastName + ", " + CLM.SuperPrvFirstName;

                                    pdfFormFields.SetField(fieldName, prvName);
                                    break;

                                // NAME OF REFERING PROVIDER OR OTHER SOURCE - QUALIFER (17a)
                                case "physician number 17a1":
                                    break;
                                // NAME OF REFERING PROVIDER OR OTHER SOURCE - VALUE (17a)
                                case "physician number 17a": break;
                                // NAME OF REFERING PROVIDER OR OTHER SOURCE - NPI
                                case "id_physician":
                                    string npi = string.Empty;
                                    if (!CLM.RefPrvNPI.IsNull()) npi = CLM.RefPrvNPI;
                                    else if (!CLM.SuperPrvNPI.IsNull()) npi = CLM.SuperPrvNPI;

                                    pdfFormFields.SetField(fieldName, npi);
                                    break;

                                #endregion

                                #region Service Lines

                                    #region  SERVICE LINE # 1

                                    // DETAIL 
                                    case "Suppl":
                                        break;

                                    // DOS FROM MONTH
                                    case "sv1_mm_from":
                                        if (CLM.Charges.Count > 0)
                                            pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[0].DateofServiceFrom, "MM"));
                                        break;

                                    // DOS FROM DAY
                                    case "sv1_dd_from":
                                        if (CLM.Charges.Count > 0)
                                            pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[0].DateofServiceFrom, "dd"));
                                        break;

                                    // DOS FROM YEAR
                                    case "sv1_yy_from":
                                        //Debug.WriteLine(CLM.Charges.Count+"   " + CLM.Charges[0].DateofServiceFrom.ToString("MM/dd/yy").Substring(6, 4));
                                        //Debug.WriteLine(CLM.Charges.Count+"   " + GetDatePart(CLM.Charges[0].DateofServiceFrom, "yy"));
                                        if (CLM.Charges.Count > 0)
                                            //pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[0].DateofServiceFrom, "yy"));
                                            Debug.WriteLine(GetDatePart(CLM.Charges[0].DateofServiceFrom, "yy"));
                                        pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[0].DateofServiceFrom, "yy"));

                                    break;
                                // DOS TO MONTH
                                case "sv1_mm_end":
                                    if (CLM.Charges.Count > 0)
                                        pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[0].DateOfServiceTo, "MM"));
                                    break;

                                // DOS TO DAY
                                case "sv1_dd_end":
                                    if (CLM.Charges.Count > 0)
                                        pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[0].DateOfServiceTo, "dd"));
                                    break;

                                // DOS TO YEAR
                                case "sv1_yy_end":
                                    if (CLM.Charges.Count > 0)
                                        pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[0].DateOfServiceTo, "yy"));
                                    break;


                                //POS
                                case "place1":
                                    if (CLM.Charges.Count > 0)
                                        pdfFormFields.SetField(fieldName, CLM.Charges[0].POS);
                                    break;

                                // EMG
                                case "type1":
                                    break;

                                // CPT/HCPCS
                                case "cpt1":
                                    if (CLM.Charges.Count > 0)
                                        pdfFormFields.SetField(fieldName, CLM.Charges[0].CptCode.Replace("-", "").Trim());
                                    break;
                                // MODIFIER 1
                                case "mod1":
                                    if (CLM.Charges.Count > 0)
                                        pdfFormFields.SetField(fieldName, CLM.Charges[0].Modifier1);
                                    break;
                                // MODIFIER 2
                                case "mod1a":
                                    if (CLM.Charges.Count > 0)
                                        pdfFormFields.SetField(fieldName, CLM.Charges[0].Modifier2);
                                    break;
                                // MODIFIER 3
                                case "mod1b":
                                    if (CLM.Charges.Count > 0)
                                        pdfFormFields.SetField(fieldName, CLM.Charges[0].Modifier3);
                                    break;
                                //MODIFIER 4
                                case "mod1c":
                                    if (CLM.Charges.Count > 0)
                                        pdfFormFields.SetField(fieldName, CLM.Charges[0].Modifier4);
                                    break;
                                // DIAGNOSIS POINTER
                                case "diag1":
                                    //pointers = CLM.Charges[0].Pointer1
                                    if (CLM.Charges.Count > 0)
                                    {
                                        pointers = FormatPointer(CLM.Charges[0].Pointer1) + FormatPointer(CLM.Charges[0].Pointer2) +
                                            FormatPointer(CLM.Charges[0].Pointer3) + FormatPointer(CLM.Charges[0].Pointer4);
                                        pdfFormFields.SetField(fieldName, pointers);
                                    }

                                    break;
                                // CHARGE AMOUNT 1
                                case "135":
                                    //if (Claim.Charges.Count > 0)
                                    //{
                                    //    s = Claim.Charges[0].VisitCharge.ToString("0.00", CultureInfo.InvariantCulture);
                                    //    parts = s.Split('.');
                                    //    pdfFormFields.SetField(fieldName, parts != null & parts.Length > 0 ? parts[0] : string.Empty);
                                    //} 
                                    break;
                                // CHARGE AMOUNT 2
                                case "ch1":
                                    if (CLM.Charges.Count > 0)
                                    {
                                        string s = ((decimal)(CLM.Charges[0].ChargeAmount)).ToString("0.00", CultureInfo.InvariantCulture);
                                        //parts = s.Split('.');
                                        //pdfFormFields.SetField(fieldName, parts != null & parts.Length > 1 ? parts[1] : "00");
                                        pdfFormFields.SetField(fieldName, s);
                                    }
                                    break;
                                // DAYS OR UNITS
                                case "day1":
                                    if (CLM.Charges.Count > 0)
                                        pdfFormFields.SetField(fieldName, CLM.Charges[0].Units.ToString());
                                    break;
                                // FAMILY PLAN
                                case "plan1": break;
                                // RENDERING NPI
                                case "local1":
                                    if (CLM.Charges.Count > 0)
                                        pdfFormFields.SetField(fieldName, CLM.RendPrvNPI);
                                    break;
                                // REDNERING PROVIDER ID QUALIFER
                                    case "emg1":
                                        if (CLM.Charges.Count > 0 && CLM.Charges[0].IsEmergency)
                                            pdfFormFields.SetField(fieldName, "Y");

                                        break;
                                    // RENDIERNG PROVIDER ID
                                    case "local1a": break;
                                    // EPSDT
                                    case "epsdt1": break;

                                    #endregion

                                    #region SERVICE LINE # 2

                                    // DETAIL 
                                    case "Suppla": break;

                                    // EPSDT
                                    case "epsdt2": break;
                                    // RENDIERNG PROVIDER ID QUALIFER
                                    case "emg2":
                                        if (CLM.Charges.Count > 1 && CLM.Charges[1].IsEmergency)
                                            pdfFormFields.SetField(fieldName, "Y");
                                        break;
                                    // RENDIERNG PROVIDER ID
                                    case "local2a": break;
                                    // RENDERING NPI
                                    case "local2":
                                        if (CLM.Charges.Count > 1)
                                            pdfFormFields.SetField(fieldName, CLM.RendPrvNPI);
                                        break;
                                    // FAMILY PLAN
                                    case "plan2": break;
                                    // DAYS OR UNITS
                                    case "day2":
                                        if (CLM.Charges.Count > 1)
                                            pdfFormFields.SetField(fieldName, CLM.Charges[1].Units.ToString());
                                        break;
                                    // CHARGE AMOUNT 2
                                    case "ch2":
                                        if (CLM.Charges.Count > 1)
                                        {
                                            string s = ((decimal)(CLM.Charges[1].ChargeAmount)).ToString("0.00", CultureInfo.InvariantCulture);
                                            //parts = s.Split('.');
                                            //pdfFormFields.SetField(fieldName, parts != null & parts.Length > 1 ? parts[1] : "00");
                                            pdfFormFields.SetField(fieldName, s);
                                        }
                                        break;
                                    // CHARGE AMOUNT 1
                                    case "157":
                                        //if (Claim.Charges.Count > 1)
                                        //{
                                        //    s = Claim.Charges[1].VisitCharge.ToString("0.00", CultureInfo.InvariantCulture);
                                        //    parts = s.Split('.');
                                        //    pdfFormFields.SetField(fieldName, parts != null & parts.Length > 0 ? parts[0] : string.Empty);
                                        //}
                                        break;
                                    // DIAGNOSIS POINTER
                                    case "diag2":
                                        if (CLM.Charges.Count > 1)
                                        {
                                            pointers = FormatPointer(CLM.Charges[1].Pointer1) + FormatPointer(CLM.Charges[1].Pointer2) +
                                               FormatPointer(CLM.Charges[1].Pointer3) + FormatPointer(CLM.Charges[1].Pointer4);
                                            pdfFormFields.SetField(fieldName, pointers);
                                        }
                                        break;
                                    //MODIFIER 4
                                    case "mod2c":
                                        if (CLM.Charges.Count > 1)
                                            pdfFormFields.SetField(fieldName, CLM.Charges[1].Modifier4);
                                        break;
                                    //MODIFIER 3
                                    case "mod2b":
                                        if (CLM.Charges.Count > 1)
                                            pdfFormFields.SetField(fieldName, CLM.Charges[1].Modifier3);
                                        break;
                                    //MODIFIER 2
                                    case "mod2a":
                                        if (CLM.Charges.Count > 1)
                                            pdfFormFields.SetField(fieldName, CLM.Charges[1].Modifier2);
                                        break;
                                    //MODIFIER 1
                                    case "mod2":
                                        if (CLM.Charges.Count > 1)
                                            pdfFormFields.SetField(fieldName, CLM.Charges[1].Modifier1);
                                        break;

                                // CPT/HCPCS
                                case "cpt2":
                                    if (CLM.Charges.Count > 1)
                                        pdfFormFields.SetField(fieldName, CLM.Charges[1].CptCode.Replace("-", "").Trim());
                                    break;

                                // EMG
                                case "type2": break;
                                //POS
                                case "place2":
                                    if (CLM.Charges.Count > 1)
                                        pdfFormFields.SetField(fieldName, CLM.Charges[1].POS);
                                    break;
                                // DOS TO YEAR
                                case "sv2_yy_end":
                                    if (CLM.Charges.Count > 1)
                                        pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[1].DateofServiceFrom, "yy"));
                                    break;
                                // DOS TO DAY
                                case "sv2_dd_end":
                                    if (CLM.Charges.Count > 1)
                                        pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[1].DateofServiceFrom, "dd"));
                                    break;
                                // DOS TO MONTH
                                case "sv2_mm_end":
                                    if (CLM.Charges.Count > 1)
                                        pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[1].DateofServiceFrom, "MM"));
                                    break;

                                // DOS FROM YEAR
                                case "sv2_yy_from":
                                    if (CLM.Charges.Count > 1)
                                        pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[1].DateOfServiceTo, "yy"));
                                    break;
                                // DOS FROM DAY
                                case "sv2_dd_from":
                                    if (CLM.Charges.Count > 1)
                                        pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[1].DateOfServiceTo, "dd"));
                                    break;
                                // DOS FROM MONTH
                                case "sv2_mm_from":
                                    if (CLM.Charges.Count > 1)
                                        pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[1].DateOfServiceTo, "MM"));
                                    break;

                                    #endregion


                                    #region SERVICE LINE # 3

                                    // DETAIL 
                                    case "Supplb": break;

                                    // EPSDT
                                    case "epsdt3": break;
                                    // RENDIERNG PROVIDER ID QUALIFER
                                    case "emg3":
                                        if (CLM.Charges.Count > 2 && CLM.Charges[2].IsEmergency)
                                            pdfFormFields.SetField(fieldName, "Y");
                                        break;
                                    // RENDIERNG PROVIDER ID
                                    case "local3a": break;
                                    // RENDERING NPI
                                    case "local3":
                                        if (CLM.Charges.Count > 2)
                                            pdfFormFields.SetField(fieldName, CLM.RendPrvNPI);
                                        break;
                                    // FAMILY PLAN
                                    case "plan3": break;
                                    // DAYS OR UNITS
                                    case "day3":
                                        if (CLM.Charges.Count > 2)
                                            pdfFormFields.SetField(fieldName, CLM.Charges[2].Units.ToString());
                                        break;
                                    // CHARGE AMOUNT 2
                                    case "ch3":
                                        if (CLM.Charges.Count > 2)
                                        {
                                            string s = ((decimal)(CLM.Charges[2].ChargeAmount)).ToString("0.00", CultureInfo.InvariantCulture);

                                        //parts = s.Split('.');
                                        //pdfFormFields.SetField(fieldName, parts != null & parts.Length > 1 ? parts[1] : "00");
                                        pdfFormFields.SetField(fieldName, s);
                                    }
                                    break;
                                // CHARGE AMOUNT 1
                                case "179":
                                    //if (Claim.Charges.Count > 2)
                                    //{
                                    //    s = Claim.Charges[2].VisitCharge.ToString("0.00", CultureInfo.InvariantCulture);
                                    //    parts = s.Split('.');
                                    //    pdfFormFields.SetField(fieldName, parts != null & parts.Length > 0 ? parts[0] : string.Empty);
                                    //}
                                    break;
                                // DIAGNOSIS POINTER
                                case "diag3":
                                    if (CLM.Charges.Count > 2)
                                    {
                                        pointers = FormatPointer(CLM.Charges[2].Pointer1) + FormatPointer(CLM.Charges[2].Pointer2) +
                                           FormatPointer(CLM.Charges[2].Pointer3) + FormatPointer(CLM.Charges[2].Pointer4);
                                        pdfFormFields.SetField(fieldName, pointers);
                                    }
                                    break;
                                //MODIFIER 4
                                case "mod3c":
                                    if (CLM.Charges.Count > 2)
                                        pdfFormFields.SetField(fieldName, CLM.Charges[2].Modifier4);
                                    break;
                                //MODIFIER 3
                                case "mod3b":
                                    if (CLM.Charges.Count > 2)
                                        pdfFormFields.SetField(fieldName, CLM.Charges[2].Modifier3);
                                    break;
                                //MODIFIER 2
                                case "mod3a":
                                    if (CLM.Charges.Count > 2)
                                        pdfFormFields.SetField(fieldName, CLM.Charges[2].Modifier2);
                                    break;
                                //MODIFIER 1
                                case "mod3":
                                    if (CLM.Charges.Count > 2)
                                        pdfFormFields.SetField(fieldName, CLM.Charges[2].Modifier1);
                                    break;
                                // CPT/HCPCS
                                case "cpt3":
                                    if (CLM.Charges.Count > 2)
                                        pdfFormFields.SetField(fieldName, CLM.Charges[2].CptCode.Replace("-", "").Trim());
                                    break;
                                //EMG 
                                    case "type3":
                                        if (CLM.Charges.Count > 2 && CLM.Charges[2].IsEmergency)
                                            pdfFormFields.SetField(fieldName, "Y");
                                        break;
                                    //POS
                                    case "place3":
                                        if (CLM.Charges.Count > 2)
                                            pdfFormFields.SetField(fieldName, CLM.Charges[2].POS);
                                        break;
                                    // DOS TO YEAR
                                    case "sv3_yy_end":
                                        if (CLM.Charges.Count > 2)
                                            pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[2].DateofServiceFrom, "yy"));
                                        break;
                                    // DOS TO DAY
                                    case "sv3_dd_end":
                                        if (CLM.Charges.Count > 2)
                                            pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[2].DateofServiceFrom, "dd"));
                                        break;
                                    // DOS TO MONTH
                                    case "sv3_mm_end":
                                        if (CLM.Charges.Count > 2)
                                            pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[2].DateofServiceFrom, "MM"));
                                        break;

                                    // DOS FROM YEAR
                                    case "sv3_yy_from":
                                        if (CLM.Charges.Count > 2)
                                            pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[2].DateOfServiceTo, "yy"));
                                        break;
                                    // DOS FROM DAY
                                    case "sv3_dd_from":
                                        if (CLM.Charges.Count > 2)
                                            pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[2].DateOfServiceTo, "dd"));
                                        break;
                                    // DOS FROM MONTH
                                    case "sv3_mm_from":
                                        if (CLM.Charges.Count > 2)
                                            pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[2].DateOfServiceTo, "MM"));
                                        break;

                                    #endregion

                                    #region SERVICE LINE # 4

                                // DETAIL 
                                case "Supplc": break;

                                    // EPSDT
                                    case "epsdt4": break;
                                    // RENDIERNG PROVIDER ID QUALIFER
                                    case "emg4":
                                        if (CLM.Charges.Count > 3 && CLM.Charges[3].IsEmergency)
                                            pdfFormFields.SetField(fieldName, "Y");
                                        break;
                                    // RENDIERNG PROVIDER ID
                                    case "local4a": break;
                                    // RENDERING NPI
                                    case "local4":
                                        if (CLM.Charges.Count > 3)
                                            pdfFormFields.SetField(fieldName, CLM.RendPrvNPI);
                                        break;
                                    // FAMILY PLAN
                                    case "plan4": break;
                                    // DAYS OR UNITS
                                    case "day4":
                                        if (CLM.Charges.Count > 3)
                                            pdfFormFields.SetField(fieldName, CLM.Charges[3].Units.ToString());
                                        break;
                                    // CHARGE AMOUNT 2
                                    case "ch4":
                                        if (CLM.Charges.Count > 3)
                                        {
                                            string s = ((decimal)(CLM.Charges[3].ChargeAmount)).ToString("0.00", CultureInfo.InvariantCulture);
                                            //parts = s.Split('.');
                                            //pdfFormFields.SetField(fieldName, parts != null & parts.Length > 1 ? parts[1] : "00");
                                            pdfFormFields.SetField(fieldName, s);
                                        }
                                        break;
                                    // CHARGE AMOUNT 1
                                    case "201":
                                        //if (Claim.Charges.Count > 3)
                                        //{
                                        //    s = Claim.Charges[3].VisitCharge.ToString("0.00", CultureInfo.InvariantCulture);
                                        //    parts = s.Split('.');
                                        //    pdfFormFields.SetField(fieldName, parts != null & parts.Length > 0 ? parts[0] : string.Empty);
                                        //}
                                        break;
                                    // DIAGNOSIS POINTER
                                    case "diag4":
                                        if (CLM.Charges.Count > 3)
                                        {
                                            pointers = FormatPointer(CLM.Charges[3].Pointer1) + FormatPointer(CLM.Charges[3].Pointer2) +
                                               FormatPointer(CLM.Charges[3].Pointer3) + FormatPointer(CLM.Charges[3].Pointer4);
                                            pdfFormFields.SetField(fieldName, pointers);
                                        }
                                        break;
                                    //MODIFIER 4
                                    case "mod4c":
                                        if (CLM.Charges.Count > 3)
                                            pdfFormFields.SetField(fieldName, CLM.Charges[3].Modifier4);
                                        break;
                                    //MODIFIER 3
                                    case "mod4b":
                                        if (CLM.Charges.Count > 3)
                                            pdfFormFields.SetField(fieldName, CLM.Charges[3].Modifier3);
                                        break;
                                    //MODIFIER 2
                                    case "mod4a":
                                        if (CLM.Charges.Count > 3)
                                            pdfFormFields.SetField(fieldName, CLM.Charges[3].Modifier2);
                                        break;
                                    //MODIFIER 1
                                    case "mod4":
                                        if (CLM.Charges.Count > 3)
                                            pdfFormFields.SetField(fieldName, CLM.Charges[3].Modifier1);
                                        break;
                                    // CPT/HCPCS
                                    case "cpt4":
                                        if (CLM.Charges.Count > 3)
                                            pdfFormFields.SetField(fieldName, CLM.Charges[3].CptCode.Replace("-", "").Trim());
                                        break;
                                    //EMG 
                                    case "type4": break;
                                    //POS
                                    case "place4":
                                        if (CLM.Charges.Count > 3)
                                            pdfFormFields.SetField(fieldName, CLM.Charges[3].POS);
                                        break;
                                    // DOS TO YEAR
                                    case "sv4_yy_end":
                                        if (CLM.Charges.Count > 3)
                                            pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[3].DateofServiceFrom, "yy"));
                                        break;
                                    // DOS TO DAY
                                    case "sv4_dd_end":
                                        if (CLM.Charges.Count > 3)
                                            pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[3].DateofServiceFrom, "dd"));
                                        break;
                                    // DOS TO MONTH
                                    case "sv4_mm_end":
                                        if (CLM.Charges.Count > 3)
                                            pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[3].DateofServiceFrom, "MM"));
                                        break;

                                // DOS FROM YEAR
                                case "sv4_yy_from":
                                    if (CLM.Charges.Count > 3)
                                        pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[3].DateOfServiceTo, "yy"));
                                    break;
                                // DOS FROM DAY
                                case "sv4_dd_from":
                                    if (CLM.Charges.Count > 3)
                                        pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[3].DateOfServiceTo, "dd"));
                                    break;
                                // DOS FROM MONTH
                                case "sv4_mm_from":
                                    if (CLM.Charges.Count > 3)
                                        pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[3].DateOfServiceTo, "MM"));
                                    break;

                                    #endregion

                                    #region SERVICE LINE # 5

                                    // DETAIL 
                                    case "Suppld": break;

                                    // EPSDT
                                    case "epsdt5": break;
                                    // RENDIERNG PROVIDER ID QUALIFER
                                    case "emg5":
                                        if (CLM.Charges.Count > 4 && CLM.Charges[4].IsEmergency)
                                            pdfFormFields.SetField(fieldName, "Y");
                                        break;
                                    // RENDIERNG PROVIDER ID
                                    case "local5a": break;
                                    // RENDERING NPI
                                    case "local5":
                                        if (CLM.Charges.Count > 4)
                                            pdfFormFields.SetField(fieldName, CLM.RendPrvNPI);
                                        break;
                                    // FAMILY PLAN
                                    case "plan5": break;
                                    // DAYS OR UNITS
                                    case "day5":
                                        if (CLM.Charges.Count > 4)
                                            pdfFormFields.SetField(fieldName, CLM.Charges[4].Units.ToString());
                                        break;
                                    // CHARGE AMOUNT 2
                                    case "ch5":
                                        if (CLM.Charges.Count > 4)
                                        {
                                            string s = ((decimal)(CLM.Charges[4].ChargeAmount)).ToString("0.00", CultureInfo.InvariantCulture);
                                            //parts = s.Split('.');
                                            //pdfFormFields.SetField(fieldName, parts != null & parts.Length > 1 ? parts[1] : "00");
                                            pdfFormFields.SetField(fieldName, s);
                                        }
                                        break;
                                    // CHARGE AMOUNT 1
                                    case "223":
                                        //if (Claim.Charges.Count > 4)
                                        //{
                                        //    s = Claim.Charges[4].VisitCharge.ToString("0.00", CultureInfo.InvariantCulture);
                                        //    parts = s.Split('.');
                                        //    pdfFormFields.SetField(fieldName, parts != null & parts.Length > 0 ? parts[0] : string.Empty);
                                        //}
                                        break;
                                    // DIAGNOSIS POINTER
                                    case "diag5":
                                        if (CLM.Charges.Count > 4)
                                        {
                                            pointers = FormatPointer(CLM.Charges[4].Pointer1) + FormatPointer(CLM.Charges[4].Pointer2) +
                                               FormatPointer(CLM.Charges[4].Pointer3) + FormatPointer(CLM.Charges[4].Pointer4);
                                            pdfFormFields.SetField(fieldName, pointers);
                                        }
                                        break;
                                    //MODIFIER 4
                                    case "mod5c":
                                        if (CLM.Charges.Count > 4)
                                            pdfFormFields.SetField(fieldName, CLM.Charges[4].Modifier4);
                                        break;
                                    //MODIFIER 3
                                    case "mod5b":
                                        if (CLM.Charges.Count > 4)
                                            pdfFormFields.SetField(fieldName, CLM.Charges[4].Modifier3);
                                        break;
                                    //MODIFIER 2
                                    case "mod5a":
                                        if (CLM.Charges.Count > 4)
                                            pdfFormFields.SetField(fieldName, CLM.Charges[4].Modifier2);
                                        break;
                                    //MODIFIER 1
                                    case "mod5":
                                        if (CLM.Charges.Count > 4)
                                            pdfFormFields.SetField(fieldName, CLM.Charges[4].Modifier1);
                                        break;
                                    // CPT/HCPCS
                                    case "cpt5":
                                        if (CLM.Charges.Count > 4)
                                            pdfFormFields.SetField(fieldName, CLM.Charges[4].CptCode.Replace("-", "").Trim());
                                        break;
                                    //EMG 
                                    case "type5": break;
                                    //POS
                                    case "place5":
                                        if (CLM.Charges.Count > 4)
                                            pdfFormFields.SetField(fieldName, CLM.Charges[4].POS);
                                        break;
                                    // DOS TO YEAR
                                    case "sv5_yy_end":
                                        if (CLM.Charges.Count > 4)
                                            pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[4].DateofServiceFrom, "yy"));
                                        break;
                                    // DOS TO DAY
                                    case "sv5_dd_end":
                                        if (CLM.Charges.Count > 4)
                                            pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[4].DateofServiceFrom, "dd"));
                                        break;
                                    // DOS TO MONTH
                                    case "sv5_mm_end":
                                        if (CLM.Charges.Count > 4)
                                            pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[4].DateofServiceFrom, "MM"));
                                        break;

                                    // DOS FROM YEAR
                                    case "sv5_yy_from":
                                        if (CLM.Charges.Count > 4)
                                            pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[4].DateOfServiceTo, "yy"));
                                        break;
                                    // DOS FROM DAY
                                    case "sv5_dd_from":
                                        if (CLM.Charges.Count > 4)
                                            pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[4].DateOfServiceTo, "dd"));
                                        break;
                                    // DOS FROM MONTH
                                    case "sv5_mm_from":
                                        if (CLM.Charges.Count > 4)
                                            pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[4].DateOfServiceTo, "MM"));
                                        break;

                                    #endregion

                                    #region SERVICE LINE # 6

                                    // DETAIL 
                                    case "Supple": break;

                                    // EPSDT
                                    case "epsdt6": break;
                                    // RENDIERNG PROVIDER ID QUALIFER
                                    case "emg6":
                                        if (CLM.Charges.Count > 5 && CLM.Charges[5].IsEmergency)
                                            pdfFormFields.SetField(fieldName, "Y");
                                        break;
                                    // RENDIERNG PROVIDER ID
                                    case "local6a": break;
                                    // RENDERING NPI
                                    case "local6":
                                        if (CLM.Charges.Count > 5)
                                            pdfFormFields.SetField(fieldName, CLM.RendPrvNPI);
                                        break;
                                    // FAMILY PLAN
                                    case "plan6": break;
                                    // DAYS OR UNITS
                                    case "day6":
                                        if (CLM.Charges.Count > 5)
                                            pdfFormFields.SetField(fieldName, CLM.Charges[5].Units.ToString());
                                        break;
                                    // CHARGE AMOUNT 2
                                    case "ch6":
                                        if (CLM.Charges.Count > 5)
                                        {
                                            string s = ((decimal)(CLM.Charges[5].ChargeAmount)).ToString("0.00", CultureInfo.InvariantCulture);

                                        //parts = s.Split('.');
                                        //pdfFormFields.SetField(fieldName, parts != null & parts.Length > 1 ? parts[1] : "00");
                                        pdfFormFields.SetField(fieldName, s);
                                    }
                                    break;
                                // CHARGE AMOUNT 1
                                case "245":
                                    //if (Claim.Charges.Count > 5)
                                    //{
                                    //    s = Claim.Charges[5].VisitCharge.ToString("0.00", CultureInfo.InvariantCulture);
                                    //    parts = s.Split('.');
                                    //    pdfFormFields.SetField(fieldName, parts != null & parts.Length > 0 ? parts[0] : string.Empty);
                                    //}
                                    break;
                                // DIAGNOSIS POINTER
                                case "diag6":
                                    if (CLM.Charges.Count > 5)
                                    {
                                        pointers = FormatPointer(CLM.Charges[5].Pointer1) + FormatPointer(CLM.Charges[5].Pointer2) +
                                            FormatPointer(CLM.Charges[5].Pointer3) + FormatPointer(CLM.Charges[5].Pointer4);
                                        pdfFormFields.SetField(fieldName, pointers);
                                    }
                                    break;
                                //MODIFIER 4
                                case "mod6c":
                                    if (CLM.Charges.Count > 5)
                                        pdfFormFields.SetField(fieldName, CLM.Charges[5].Modifier4);
                                    break;
                                //MODIFIER 3
                                case "mod6b":
                                    if (CLM.Charges.Count > 5)
                                        pdfFormFields.SetField(fieldName, CLM.Charges[5].Modifier3); break;
                                //MODIFIER 2
                                case "mod6a":
                                    if (CLM.Charges.Count > 5)
                                        pdfFormFields.SetField(fieldName, CLM.Charges[5].Modifier2);
                                    break;
                                //MODIFIER 1
                                case "mod6":
                                    if (CLM.Charges.Count > 5)
                                        pdfFormFields.SetField(fieldName, CLM.Charges[5].Modifier1);
                                    break;
                                // CPT/HCPCS
                                case "cpt6":
                                    if (CLM.Charges.Count > 5)
                                        pdfFormFields.SetField(fieldName, CLM.Charges[5].CptCode.Replace("-", "").Trim());
                                    break;
                                //EMG
                                case "type6": break;
                                //POS
                                case "place6":
                                    if (CLM.Charges.Count > 5)
                                        pdfFormFields.SetField(fieldName, CLM.Charges[5].POS);
                                    break;
                                // DOS TO YEAR
                                case "sv6_yy_end":
                                    if (CLM.Charges.Count > 5)
                                        pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[5].DateofServiceFrom, "yy"));
                                    break;
                                // DOS TO DAY
                                case "sv6_dd_end":
                                    if (CLM.Charges.Count > 5)
                                        pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[5].DateofServiceFrom, "dd"));
                                    break;
                                // DOS TO MONTH
                                case "sv6_mm_end":
                                    if (CLM.Charges.Count > 5)
                                        pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[5].DateofServiceFrom, "MM"));
                                    break;

                                // DOS FROM YEAR
                                case "sv6_yy_from":
                                    if (CLM.Charges.Count > 5)
                                        pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[5].DateOfServiceTo, "yy"));
                                    break;
                                // DOS FROM DAY
                                case "sv6_dd_from":
                                    if (CLM.Charges.Count > 5)
                                        pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[5].DateOfServiceTo, "dd"));
                                    break;
                                // DOS FROM MONTH
                                case "sv6_mm_from":
                                    if (CLM.Charges.Count > 5)
                                        pdfFormFields.SetField(fieldName, GetDatePart(CLM.Charges[5].DateOfServiceTo, "MM"));
                                    break;
                                    #endregion

                                #endregion

                                #region Amounts
                                // PATIENT'S ACCOUNT NO.
                                case "pt_account":
                                    pdfFormFields.SetField(fieldName, CLM.PatientControlNumber.ToString());
                                    break;

                                case "t_charge":
                                    pdfFormFields.SetField(fieldName, CLM.ClaimAmount.ToString());
                                    break;

                                case "amt_paid":
                                    break;

                                #endregion

                                #region Service Practice Location Information
                                // SERVICE PRACTICE LOCATION INFORMATION
                                case "fac_name":
                                    pdfFormFields.SetField(fieldName, CLM.LocationOrgName.ToUpper());
                                    break;
                                // SERVICE PRACTICE LOCATION ADDRESS
                                case "fac_street":
                                    pdfFormFields.SetField(fieldName, CLM.LocationAddress.ToUpper());
                                    break;
                                // SERVICE PRACTICE LOCATION CITY, STATE, ZIP
                                case "fac_location":
                                    cityStateZip = CLM.BillPrvCity + ", " + CLM.BillPrvState + ", " + CLM.BillPrvZipCode;
                                    pdfFormFields.SetField(fieldName, cityStateZip.ToUpper());
                                    break;
                                // SERVICE PRACTICE LOCATION INFORMATION a
                                case "pin1":
                                    pdfFormFields.SetField(fieldName, CLM.LocationNPI.ToUpper());
                                    break;
                                // SERVICE PRACTICE LOCATION INFORMATION b
                                case "grp1": break;
                                #endregion

                                #region Billing Provider
                                // BILLING PROVIDER INFO - PHONE 2 (2854078)
                                case "doc_phone": break;

                                // BILLING PROVIDER INFO - PHONE 1 (333)
                                case "doc_phone area": break;

                                // BILLING PROVIDER NAME
                                case "doc_name":
                                    string billingProvider = string.Empty;
                                    pdfFormFields.SetField(fieldName, CLM.BillPrvOrgName.ToUpper());
                                    break;
                                // BILLING PROVIDER ADDRESS
                                case "doc_street":
                                    pdfFormFields.SetField(fieldName, CLM.BillPrvAddress1.ToUpper());
                                    break;
                                // BILLING PROVIDER CITY/STATE/ZIP
                                case "doc_location":
                                    cityStateZip = CLM.BillPrvCity + ", " + CLM.BillPrvState + ", " + CLM.BillPrvZipCode;
                                    pdfFormFields.SetField(fieldName, cityStateZip.ToUpper());
                                    break;
                                // BILLING PROVIDER INFO a
                                case "pin":
                                    pdfFormFields.SetField(fieldName, CLM.BillPrvNPI);
                                    break;

                                // BILLING PROVIDER INFO b
                                case "grp":
                                    pdfFormFields.SetField(fieldName, "ZZ" + CLM.BillPrvTaxonomyCode);
                                    break;

                                // FEDERAL TAX ID - : 1 - SSN,  2 - EIN
                                case "ssn":
                                    // We are billing as company only, hece Tax id is always reported
                                    pdfFormFields.SetField(fieldName, "EIN");
                                    break;

                                // FEDERAL TAX ID
                                case "tax_id":
                                    pdfFormFields.SetField(fieldName, CLM.BillPrvTaxID);
                                    break;

                                #endregion

                                #region diagnosis
                                case "99icd":
                                    // 0 For ICD-10
                                    // 9 For ICD-9
                                    pdfFormFields.SetField(fieldName, "0");
                                    break;

                                // DIAGNOSES A
                                case "diagnosis1":
                                    pdfFormFields.SetField(fieldName, CLM.ICD1Code);
                                    break;
                                // DIAGNOSES B
                                case "diagnosis2":
                                    pdfFormFields.SetField(fieldName, CLM.ICD2Code);
                                    break;
                                // DIAGNOSES C
                                case "diagnosis3":
                                    pdfFormFields.SetField(fieldName, CLM.ICD3Code);
                                    break;
                                // DIAGNOSES D
                                case "diagnosis4":
                                    pdfFormFields.SetField(fieldName, CLM.ICD4Code);
                                    break;
                                // DIAGNOSES E
                                case "diagnosis5":
                                    pdfFormFields.SetField(fieldName, CLM.ICD5Code);
                                    break;
                                // DIAGNOSES F
                                case "diagnosis6":
                                    pdfFormFields.SetField(fieldName, CLM.ICD6Code);
                                    break;
                                // DIAGNOSES G
                                case "diagnosis7":
                                    pdfFormFields.SetField(fieldName, CLM.ICD7Code);
                                    break;
                                // DIAGNOSES H
                                case "diagnosis8":
                                    pdfFormFields.SetField(fieldName, CLM.ICD8Code);
                                    break;
                                // DIAGNOSES I
                                case "diagnosis9":
                                    pdfFormFields.SetField(fieldName, CLM.ICD9Code);
                                    break;
                                // DIAGNOSES J
                                case "diagnosis10":
                                    pdfFormFields.SetField(fieldName, CLM.ICD10Code);
                                    break;
                                // DIAGNOSES K
                                case "diagnosis11":
                                    pdfFormFields.SetField(fieldName, CLM.ICD11Code);
                                    break;
                                // DIAGNOSES L
                                case "diagnosis12":
                                    pdfFormFields.SetField(fieldName, CLM.ICD12Code);
                                    break;
                                #endregion

                                #region Other

                                // SIGNATURE OF PHYSICIAN - SIGN
                                case "physician_signature":
                                    pdfFormFields.SetField(fieldName, "SIGNATURE ON FILE");
                                    break;

                                // SIGNATURE OF PHYISICIAN DATE
                                case "physician_date":
                                    pdob = string.Format("{0:MM/dd/yyyy}", DateTime.Now);
                                    pdfFormFields.SetField(fieldName, pdob.Substring(6, 4) + pdob.Substring(0, 2) + pdob.Substring(3, 2));
                                    break;
                                //


                                // ACCEPT ASSIGNMENT: 1 - YES,  2 - NO (27a)
                                case "assignment":
                                    pdfFormFields.SetField(fieldName, "YES");
                                    break;

                                // OUT SIDE LAB.  1 - YES,  2 - NO
                                case "lab":
                                    pdfFormFields.SetField(fieldName, "NO");
                                    break;

                                // LAB CHARGES
                                case "charge":
                                    break;

                                // RESUBMISSION CODE
                                case "medicaid_resub":
                                    break;
                                // ORIGINAL REF. NO. - Payer claim control number
                                case "original_ref":
                                    break;
                                // PRIOR AUTHORIZATION NUMBER
                                case "prior_auth":

                                    break;

                                    #endregion
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.StackTrace);
                        }
                    }

                    pdfStamper.FormFlattening = true;
                    pdfStamper.Close();

                    output.Claims.Add(new ClaimResult()
                    {
                        PatientControlNumber = CLM.PatientControlNumber,
                        Submitted = true,
                        SubmittedDate = CLM.SubmittedDate,
                        VisitID = CLM.VisitID
                    });
                    output.ClaimAmount += CLM.ClaimAmount;
                    output.ProcessedClaims += 1;

                    ListofAllFiles.Add(generatedHcfa);
                }

                //string deploymentPath = congi
                // string NetworkDirectory = Path.Combine("http:\\\\96.69.218.154:8020", "accessible-files");
                // string NetworkDirectory = Path.Combine("https:\\\\service.medifusion.com", "accessible-files");
                //string NetworkDirectory = Path.Combine(_networkPath, "accessible-files");
                string mergedFile = Path.Combine(OutputDirectory, "HCFA1500.pdf");

                if (ListofAllFiles != null && ListofAllFiles.Count > 0)
                    MergePDFs(ListofAllFiles, mergedFile);
                if (!mergedFile.IsNull()) output.PDFFilePath = mergedFile;

                //if (!mergedFile.IsNull()) output.PDFFilePath = Path.Combine(mergedFile);
                //PdfReader reader = new PdfReader(output.PDFFilePath);

                //// 1 inch = 72 units of the rectangle -- multiply inches with 72 to get the required number of units
                ////Document doc = new Document(new Rectangle(895.78f, 1275.04f), 0, 0, 0, 0);
                //Document doc = new Document(new Rectangle(895.78f, 1000.04f), 0, 0, -250, 0);
                //PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(Path.Combine(OutputDirectory, "HCFA1500.pdf"), FileMode.Create));
                //doc.Open();
                //PdfContentByte cb = writer.DirectContent;
                //int pages = reader.NumberOfPages;
                //for (int i = 0; i < pages; i++)
                //{
                //    PdfImportedPage page = writer.GetImportedPage(reader, i+1); //page #1
                //    float Scale = 1.4f;
                //    cb.AddTemplate(page, Scale, 0, 0, Scale-0.2f, 0, 0);
                //}
                //doc.Close();
                //if (!mergedFile.IsNull()) output.PDFFilePath = Path.Combine(OutputDirectory, "HCFA1500.pdf");


            }
            catch (Exception exc)
            {
                File.WriteAllText(Path.Combine(OutputDirectory, "Exception.txt"), exc.ToString());
                output.ErrorMessage = "Un Expected Error Occured. Please Contact Support.";
                throw exc;
            }
            finally
            {
                jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(output);
                File.WriteAllText(Path.Combine(OutputDirectory, "Output.Json"), jsonString);
            }
            return output;
        }

        public static bool MergePDFs(List<string> ListofAllFiles, string OutputFile)
        {
            bool merged = true;
            using (FileStream stream = new FileStream(OutputFile, FileMode.Create))
            {
                Document document = new Document();
                PdfCopy pdf = new PdfCopy(document, stream);
                PdfReader reader = null;
                try
                {
                    document.Open();
                    foreach (string file in ListofAllFiles)
                    {
                        reader = new PdfReader(file);
                        pdf.AddDocument(reader);
                        reader.Close();
                    }
                }
                catch (Exception)
                {
                    merged = false;
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
                finally
                {
                    if (document != null)
                    {
                        document.Close();
                    }
                }
            }
            return merged;
        }

        private string GetDatePart(DateTime? Date, string Part)
        {
            if (Date != null && Date > DateTime.MinValue)
            {
                string temp = ((DateTime)Date).ToString("MM/dd/yy");
                return GetDatePart(temp, Part);
            }
            else return string.Empty;
        }

        private string FormatPointer(string Pointer)
        {
            if (Pointer.IsNull()) return string.Empty;

            if (Pointer == "1") Pointer = " A";
            else if (Pointer == "2") Pointer = " B";
            else if (Pointer == "3") Pointer = " C";
            else if (Pointer == "4") Pointer = " D";
            else if (Pointer == "5") Pointer = " E";
            else if (Pointer == "6") Pointer = " F";
            else if (Pointer == "7") Pointer = " G";
            else if (Pointer == "8") Pointer = " H";
            else if (Pointer == "9") Pointer = " I";
            else if (Pointer == "10") Pointer = " J";
            else if (Pointer == "11") Pointer = " K";
            else if (Pointer == "12") Pointer = " L";

            return Pointer;
        }
        private string GetDatePart(String Date, string Part)
        {
            string output = string.Empty;

            if (Date.Length < 10)
            {
                if (Date.Substring(1, 1) == "/")
                    Date = "0" + Date;
            }
            if (Date.Substring(4, 1) == "/")
            {
                Date = Date.Substring(0, 3) + "0" + Date.Substring(3, 6);
            }

            if (Part == "YYYY")
            {
                output = Date.Substring(6, 4);
            }
            else if (Part == "yy")
            {
                output = Date.Substring(6, 2);
            }
            else if (Part == "MM")
            {
                output = Date.Substring(0, 2);
            }
            else if (Part == "dd")
            {
                output = Date.Substring(3, 2);
            }

            return output;
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
            else if (CLM.PayerAddress.IsNull() || CLM.PayerCity.IsNull() || CLM.PayerState.IsNull() || CLM.PayerZipCode.IsNull())
                msg = "Payer Address, City, State and Zip Code are required";
            else if (CLM.BillPrvOrgName.IsNull())
                msg = "Billing Provider Name is required";
            else if (!Header.RelaxNpiValidation && CLM.BillPrvNPI.IsNull())
                msg = "Billing Provider NPI is required";
            else if (CLM.BillPrvAddress1.IsNull() || CLM.BillPrvCity.IsNull() || CLM.BillPrvState.IsNull() || CLM.BillPrvZipCode.IsNull())
                msg = "Billing Provider Address, City, State and Zip Code are required";
            else if (CLM.BillPrvTaxID.IsNull())
                msg = "Billing Provider Tax ID is required";
            else if (CLM.PatientControlNumber.IsNull())
                msg = "Patient Control Number is required";
            //else if (CLM.ClaimAmount.Amt() <= 0)
            //    msg = "Claim Amount is rqeuired";
            else if (CLM.ICD1Code.IsNull())
                msg = "ICD Code 1 is required";
            else if (CLM.POSCode.IsNull())
                msg = "Place of Service is required";
            else if (CLM.POSCode.Length != 2)
                msg = "POS should be of 2 characters";
            else if (CLM.PatientRelationShip != "18")
            {
                if (CLM.PATLastName.IsNull() || CLM.PATFirstName.IsNull())
                    msg = "Patient Name is requried";
                else if (CLM.PATAddress.IsNull() || CLM.PATCity.IsNull() || CLM.SBRState.IsNull() || CLM.PATZipCode.IsNull())
                    msg = "Patient Address, City, State and Zip Code are required";
            }

            if (!msg.IsNull())
                return msg;

            foreach (ChargeData CH in CLM.Charges)
            {

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
            }


            return msg;
        }
    }

    public class HcfaOutput
    {
        public string ErrorMessage { get; set; }

        public long ProcessedClaims { get; set; }
        public decimal ClaimAmount { get; set; }
        public List<ClaimResult> Claims { get; set; }

        public string PDFFilePath { get; set; }
    }
}

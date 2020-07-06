using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediFusionPM.BusinessLogic.EligParser
{
    public class _271Subscriber
    {
        // Billing Provider

        public _271Subscriber()
        {
            EligibilityData = new List<_271SBREligibilityInfo>();
        }
        public string ProvEntity { get; set; }
        public string ProvLastName { get; set; }
        public string ProvFirstName { get; set; }
        public string ProvMI { get; set; }
        public string ProvQual { get; set; }
        public string ProvNPI { get; set; }

        // NM1-IL   Subscriber name
        public string SBREntityType { get; set; }
        public string SBRLastName { get; set; }
        public string SBRFirstName { get; set; }
        public string SBRMiddleInitial { get; set; }
        public string SBRID { get; set; }

        //N3, N4    Subscriber
        public string SBRAddress { get; set; }
        public string SBRCity { get; set; }
        public string SBRState { get; set; }
        public string SBRZipCode { get; set; }

        //DMG       Subscriber
        public DateTime? SBRDob { get; set; }
        public string SBRGender { get; set; }
        public string SBRSSN { get; set; }

        public string PlanNumber { get; set; }
        public string PolicyNumber { get; set; }
        public string MemberID { get; set; }
        public string CaseNumer { get; set; }
        public string FamilyUnitNumber { get; set; }
        public string SSN { get; set; }

        public string AAA01 { get; set; }
        public string AAA03 { get; set; }
        public string AAA04 { get; set; }

        public string AAAErrorMsg { get; set; }


        public DateTime? DisChargeDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? PlanDate { get; set; }
        public DateTime? EligiblityDate { get; set; }

        public DateTime? PlanBeginDate { get; set; }
        public DateTime? PlanEndDate { get; set; }
        public DateTime? EligibilityBeginDate { get; set; }
        public DateTime? EligiblityEndDate { get; set; }
        public DateTime? ServiceDate { get; set; }


        public List<_271SBREligibilityInfo> EligibilityData { get; set; }




//        096 Discharge
//102 Issue
//152 Effective Date of Change
//291 Plan

//307 Eligibility
//318 Added 
//340 Consolidated Omnibus Budget Reconciliation Act (COBRA) Begin
//341 Consolidated Omnibus Budget Reconciliation Act (COBRA) End
//342 Premium Paid to Date Begin
//343 Premium Paid to Date End
//346 Plan Begin
//347 Plan End
//356 Eligibility Begin
//357 Eligibility End
//382 Enrollment
//435 Admission
//442 Date of Death
//458 Certification
//472 Service
//539 Policy Effective
//540 Policy Expiration
//636 Date of Last Update
//771 Status 



   
//        18 Plan Number
//1L Group or Policy Number 
//    1W Member Identification 
//        3H Case Number
//49 Family Unit Number 
//    6P Group Number
//    CT Contract Number
//        EA Medical Record Identification Number
//EJ Patient Account Number
//F6 Health Insurance Claim (HIC) Number 
//    GH Identification Card Serial Number
//        HJ Identity Card Number 
//            IF Issue Number
//IG Insurance Policy Number
//N6 Plan Network Identification Number
//NQ Medicaid Recipient Identification Number 

//    Q4 Prior Identifier Number
//        SY Social Security Number 
//            Y4 Agency Claim Number 





        public string PatientLastName { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientMI { get; set; }

        public string TRN { get; set; }



        public decimal? CoInsuranceAmount { get; set; }

        public decimal? CopayAmount { get; set; }

        public decimal? DeductibleAmount { get; set; }
    }
}

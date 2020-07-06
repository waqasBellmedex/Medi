using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediFusionPM.BusinessLogic.EligParser
{
    public class _271SBREligibilityInfo
    {


        public string EB01CoverageType { get; set; }
        public string EB02CoverageLevel { get; set; }
        public string EB03ServiceTypeCode { get; set; }
        public string EB04InsuranceTypeCode { get; set; }
        public List<string> EB04InsuranceTypeCodeList { get; set; }
        public string EB05PlanCoverageDesc { get; set; }
        public string EB06TimePeriod { get; set; }
        public string EB07MonetoryAmount { get; set; }
        public string EB08BenefitPercent { get; set; }
        public string EB09QuanityQualifier { get; set; }
        public string EB10BenenfitQuantity { get; set; }
        public string EB11AuthorizationIndicator { get; set; }
        public string EB12PlanNetworkIndicator { get; set; }

        public string EB13_01_ServiceQual { get; set; }
        public string EB13_02_CPT { get; set; }
        public string EB13_03_Modifier1 { get; set; }
        public string EB13_04_Modifier2 { get; set; }
        public string EB13_05_Modifier3 { get; set; }
        public string EB13_06_Modifier4 { get; set; }
        public string EB13_07_Description { get; set; }
        public string EB13_08_ServiceQual { get; set; }

        public string EB14_01_DiagPointer1 { get; set; }
        public string EB14_02_DiagPointer2 { get; set; }
        public string EB14_03_DiagPointer3 { get; set; }
        public string EB14_04_DiagPointer4 { get; set; }


        public string PlanNumber { get; set; }
        public string GroupNumber { get; set; }
        public string ReferalNumber { get; set; }



        public DateTime? DisChargeDate { get; set; }
        public DateTime? PeriodStartDate { get; set; }
        public DateTime? PeriodEndDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime? COBDate { get; set; }


        public DateTime? PlanDate { get; set; }
        public DateTime? EligiblityDate { get; set; }
        public DateTime? PlanBeginDate { get; set; }
        public DateTime? PlanEndDate { get; set; }
        public DateTime? EligibilityBeginDate { get; set; }
        public DateTime? EligibilityEndDate { get; set; }
        public DateTime? ServiceDate { get; set; }
        public DateTime? BenefitBegin { get; set; }
        public DateTime? BenefitEnd { get; set; }


        public string AAA01 { get; set; }
        public string AAA03 { get; set; }
        public string AAA04 { get; set; }


        public string MessageText1 { get; set; }
        public string MessageText2 { get; set; }
        public string MessageText3 { get; set; }
        public string MessageText4 { get; set; }
        public string MessageText5 { get; set; }
        public string MessageText6 { get; set; }
        public string MessageText7 { get; set; }
        public string MessageText8 { get; set; }
        public string MessageText9 { get; set; }
        public string MessageText10 { get; set; }

        //III


        public string EBEntity { get; set; }
        public string EBLastName { get; set; }
        public string EBFirstName { get; set; }
        public string EBMI { get; set; }
        public string EBQual { get; set; }
        public string EBNPI { get; set; }
        public string EBRelationCode { get; set; }

        public string EBAddress { get; set; }
        public string EBCity { get; set; }
        public string EBState { get; set; }
        public string EBZipCode { get; set; }

        public string EBContactPerson { get; set; }
        public string EBContactNum { get; set; }
        public string EBTelephoneNUm { get; set; }

        public string EBBillingContactPerson { get; set; }
        public string EBBillingContactNum { get; set; }
        public string EBBillingTelephoneNum { get; set; }
        public string EBBillingEmail { get; set; }

        public string EBWebsite { get; set; }

        public string EBProviderType { get; set; }
        public string EBTaxonomyCode { get; set; }





        public string EB01CoverageTypeV { get; set; }
        public string EB02CoverageLevelV { get; set; }
        public string EB03ServiceTypeCodeV { get; set; }
        public string EB04InsuranceTypeCodeV { get; set; }

        public string EB05PlanCoverageDescV { get; set; }
        public string EB06TimePeriodV { get; set; }

        public string EB09QuanityQualifierV { get; set; }

        public string EB11AuthorizationIndicatorV { get; set; }
        public string EB12PlanNetworkIndicatorV { get; set; }

        public string EB13_01_ServiceQualV { get; set; }
        public string EB13_08_ServiceQualV { get; set; }

        public List<string> Messages { get; set; }
        public Dictionary<string, string> ListOfReferenceIds { get; set; }
        public Dictionary<string, DateTime> ListOfDates { get; set; }
    }
}

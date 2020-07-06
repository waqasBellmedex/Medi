using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediFusionPM.BusinessLogic._277CA
{
    public class ClaimAckVisit
    {
        // Billing Provider

        public string BillProvOrgName { get; set; }
        public string BillProvFirstName { get; set; }
        public string BillProvNPI { get; set; }
        public string BillProvTaxID { get; set; }

        public string BillProvTRN { get; set; }
        public string BillProvCategoryCode { get; set; }
        public string BillProvStatusCode { get; set; }
        public string BillProvEntityCode { get; set; }
        public DateTime ?BillProvStatusDate { get; set; }
        public string BillProvActionCode { get; set; }
        public decimal BillProvClaimsAmt { get; set; }

        public long BillProvAcceptedClaims { get; set; }
        public long BillProvRejectedClaims { get; set; }
        public decimal BillProvAcceptedAmt { get; set; }
        public decimal BillProvRejectedAmt { get; set; }

        // Visits

        public string PatientLastName { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientSubscriberID { get; set; }

        public string VisitTRN { get; set; }
        public string VisitContorlNumber { get; set; }


        public string Status { get; set; }



        public string VisitCategoryCode1 { get; set; }
        public string VisitCategoryCodeDesc1 { get; set; }
        public string VisitStatusCode1 { get; set; }
        public string VisitStatusCodeDesc1 { get; set; }
        public string VisitEntityCode1 { get; set; }
        public string VisitEntityCodeDesc1 { get; set; }
        public DateTime ?VisitStatusDate1 { get; set; }
        public string VisitActionCode1 { get; set; }
        public decimal VisitClaimAmt1 { get; set; }
        public string VisitRejection1 { get; set; }

        public string VisitCategoryCode2 { get; set; }
        public string VisitCategoryCodeDesc2 { get; set; }
        public string VisitStatusCode2 { get; set; }
        public string VisitStatusCodeDesc2 { get; set; }
        public string VisitEntityCode2 { get; set; }
        public string VisitEntityCodeDesc2 { get; set; }
        public DateTime? VisitStatusDate2 { get; set; }
        public string VisitActionCode2 { get; set; }
        public decimal VisitClaimAmt2 { get; set; }
        public string VisitRejection2 { get; set; }

        public string VisitCategoryCode3 { get; set; }
        public string VisitCategoryCodeDesc3 { get; set; }
        public string VisitStatusCode3 { get; set; }
        public string VisitStatusCodeDesc3 { get; set; }
        public string VisitEntityCode3 { get; set; }
        public string VisitEntityCodeDesc3 { get; set; }
        public DateTime? VisitStatusDate3 { get; set; }
        public string VisitActionCode3 { get; set; }
        public decimal VisitClaimAmt3 { get; set; }
        public string VisitRejection3 { get; set; }

        public string PayerClaimControlNum { get; set; }
        public string VisitClearingHouseNum_D9 { get; set; }
        public string VisitMedicalRecordNum_EA { get; set; }

        public DateTime ?VisitDateFrom { get; set; }
        public DateTime ?VisitDateTo { get; set; }

        public List<ClaimAckCharge> ClaimAckCharges { get; set; }
    }
}

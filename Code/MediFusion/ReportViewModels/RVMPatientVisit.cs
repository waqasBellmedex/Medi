using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ReportViewModels
{
    public class RVMPatientVisit
    {
        public class GRPatientVisit
        {
            public long? VisitNo { get; set; }
            public string EntryDate { get; set; }
            public string SubmissionDate { get; set; }
            public string PatientLastName { get; set; }
            public string PatientFirstName { get; set; }
            public string MiddleInitial { get; set; }
            public string DateOfBirth { get; set; }
            public string PatientGender { get; set; }
            public string SSN { get; set; }
            public string PrimaryInsurance { get; set; }
            public string PrimarySubscriberId /*PrimaryPolicyNumber*/ { get; set; }
            public string SecondaryInsurance { get; set; }
            public string SecondarySubscriberId/*SecondaryPolicyNumber*/ { get; set; }
            public string OtherInsurance { get; set; }
            public string OtherSubscriberId/*OtherPolicyNumber*/ { get; set; }
            public string DateOfService { get; set; }
            public string ProviderName { get; set; }
            public string IndividualNPI { get; set; }
            public string AttendingProvider { get; set; }
            public string ReferringPhysicianName { get; set; }
            public string FacilityName { get; set; }
            public string Cpt { get; set; }
            public string POS { get; set; }
            public string CptDescription { get; set; }
            public string MOD1 { get; set; }
            public string MOD2 { get; set; }
            public string dx1 { get; set; }
            public string dx2 { get; set; }
            public string dx3 { get; set; }
            public string dx4 { get; set; }
            public decimal? Charges { get; set; }
            public decimal? AllowedAmount { get; set; }
            public decimal? PaidAmount { get; set; }
            public decimal? AdjustmentAmount { get; set; }
            public decimal? Balance { get; set; }
            public string PrimaryCheckDate { get; set; }
            public string SecondaryCheckDate { get; set; }
            public decimal? PatientBalance { get; set; }
            public long? PatientID { get; set; }

            public string ShortCptDescription { get; set; }
         
            //public string MOD3 { get; set; }
            //public string MOD4 { get; set; }
        
            //public string dx5 { get; set; }
            //public string dx6 { get; set; }
            //public string dx7 { get; set; }
            //public string dx8 { get; set; }
            //public string dx9 { get; set; }
            //public string dx10 { get; set; }
                           
            public decimal? PlanBalance { get; set; }
            public string PrimaryStatus { get; set; }

            public string SecondaryStatus { get; set; }
        }

        public class CRPatientVisit
        {
            public long? ProviderID { get; set; }
            public DateTime? DateOfServiceFrom { get; set; }
            public DateTime? DateOfServiceTo { get; set; }
            public DateTime? EntryDateFrom { get; set; }
            public DateTime? EntryDateTo { get; set; }
            public DateTime? SubmittedDateTo { get; set; }
            public DateTime? SubmittedDateFrom { get; set; }
            public string PatientName { get; set; }
            public string CPTCode { get; set; }
            public string VisitType { get; set; }
            public string PaymentCriteria { get; set; }
            public string PrescribingMD { get; set; }
            public int pageNo { get; set; }
            public int PerPage { get; set; }


        }


        public class GRPatientVisitSimple
        {
            public string PatientName { get; set; }
            public string InsuranceName { get; set; }
            public string DateOfService { get; set; }
            public string Cpt { get; set; }
            public string POS { get; set; }
            public decimal? Charges { get; set; }
            public string SubmissionDate { get; set; }
            public string PaymentDate { get; set; }
            public string SecondaryPaymentDate { get; set; }
            
            public string PrescribingMD { get; set; }
            public string Provider { get; set; }
        }

        public class GRPatientVisitCountPaid
        {
            public string PatientName { get; set; }
            public string InsuranceName { get; set; }
            public string DateOfService { get; set; }
            public string Cpt { get; set; }
            public decimal? BilledAmount { get; set; }
            public decimal? PaidAmount { get; set; }
            public decimal? PlanBalance { get; set; }
            public decimal? PatientBalance { get; set; }
            public string POS { get; set; }

            public string SubmissionDate { get; set; }
            public string PaymentDate { get; set; }
            public string SecondaryPaymentDate { get; set; }


        }



        public class GRPatientVisitTotalRevenue
        {
            public string PatientName { get; set; }
            public string PayerName { get; set; }
            public string PrescribingMD { get; set; }
            public string Provider { get; set; }
            public string DateOfService { get; set; }
            public string Cpt { get; set; }
            public string POS { get; set; }
            public decimal? Charges { get; set; }
            public decimal? Payment { get; set; }
            public decimal? CollectedRevenue { get; set; }
            public decimal? Balance { get; set; }
            public decimal? AverageRevenue { get; set; }
            public string SubmissionDate { get; set; }
            public string PaymentDate { get; set; }
            public string SecondaryPaymentDate { get; set; }



        }

    }
}


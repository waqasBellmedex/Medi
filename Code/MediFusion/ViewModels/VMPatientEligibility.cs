using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;
namespace MediFusionPM.ViewModels
{
    public class VMPatientEligibility
    {

        public List<PatientEligibilityDetail> EligibilityData { get; set; }
        public SubscriberData  SBRData { get; set; }

        public void GetEligibilityDetail(ClientDbContext pMContext, long id)
        {
            EligibilityData = pMContext.PatientEligibilityDetail.Where(p => p.PatientEligibilityID == id).ToList();
            SBRData = (
                        from p in pMContext.Patient
                        join pp in pMContext.PatientPlan
                        on p.ID equals pp.PatientID
                        join pElig in pMContext.PatientEligibility
                        on pp.ID equals pElig.PatientPlanID
                        join insPlan in pMContext.InsurancePlan
                        on pp.InsurancePlanID equals insPlan.ID
                        where pElig.ID == id
                        select new SubscriberData()
                        {
                            PatientName = p.LastName + ", " + p.FirstName,
                            DOS = pElig.DOS.Format("MM/dd/yyyy"),
                            EligibilityDate = pElig.EligibilityDate.Format("MM/dd/yyyy"),
                            SubscriberGroupNumber = pElig.SubscriberGroupNumber,
                            PayerName = pElig.PayerName,
                            PayerID = pElig.PayerID,
                            PlanName = insPlan.PlanName,
                            ProviderName = pElig.ProviderLN + ", " + pElig.ProviderFN,
                            ProviderNPI = pElig.ProviderNPI,
                            ErrorMessage = pElig.Rejection,
                            Status = pElig.Status,
                            SubscriberID = pElig.SubscriberID,
                            Relation = pElig.Relation,
                            PatientAddress = p.Address1.GetValueIfNotNull(pElig.PatientAddress),
                            PatientCity = p.City.GetValueIfNotNull(pElig.PatientCity),
                            PatientDOB = p.DOB.HasValue ? p.DOB.Format("MM/dd/yyyy") : pElig.SubscriberDOB.Format("MM/dd/yyyy"),
                            PatientGender = p.Gender.GetValueIfNotNull(pElig.PatientGender),
                            PatientState = p.State.GetValueIfNotNull(pElig.PatientState),
                            PatientZip = p.ZipCode.GetValueIfNotNull(pElig.PatientZip),
                            SubscriberAddress = pp.Address1.GetValueIfNotNull(pElig.SubscriberAddress),
                            SubscriberCity = pp.City.GetValueIfNotNull(pElig.SubscriberCity),
                            SubscriberDOB = pp.DOB.HasValue ? pp.DOB.Format("MM/dd/yyyy") : pElig.SubscriberDOB.Format("MM/dd/yyyy"),
                            SubscriberGender = pp.Gender.GetValueIfNotNull(pElig.SubscriberGender),
                            SubscriberName = pp.LastName.GetValueIfNotNull(pElig.SubscriberLN) + ", " + pp.FirstName.GetValueIfNotNull(pElig.SubscriberFN),
                            SubscriberState = pp.State.GetValueIfNotNull(pElig.SubscriberState),
                            SubscriberZip = pp.ZipCode.GetValueIfNotNull(pElig.SubscriberZip)
                        }).SingleOrDefault();
        }


        public class SubscriberData
        {
            public string EligibilityDate { get; set; }
            public string DOS { get; set; }
            public string Status { get; set; }
            public string ErrorMessage { get; set; }
            public string Relation { get; set; }

            public string SubscriberName { get; set; }
            
            public string SubscriberID { get; set; }
            public string SubscriberGroupNumber { get; set; }
            public string SubscriberAddress { get; set; }
            public string SubscriberCity { get; set; }
            public string SubscriberState { get; set; }
            public string SubscriberZip { get; set; }
            public string SubscriberDOB { get; set; }
            public string SubscriberGender { get; set; }

            public string PatientName { get; set; }
            
            public string PatientAddress { get; set; }
            public string PatientCity { get; set; }
            public string PatientState { get; set; }
            public string PatientZip { get; set; }
            public string PatientDOB { get; set; }
            public string PatientGender { get; set; }

            public string ProviderName { get; set; }
            public string ProviderNPI { get; set; }

            public string PlanName { get; set; }

            public string PayerName { get; set; }
            public string PayerID { get; set; }
           
        }


        public class EligibilitySubmitModel
        {
            public long PatientPlanID { get; set; }
            public string ServiceTypeCode { get; set; }
            public DateTime? DOS { get; set; }
            public long ProviderID { get; set; }
        }

        public class GPatientEligibility
        {
            public long ID { get; set; }
            public long? PatientID { get; set; }
            public long? ProviderID { get; set; }
            public string EligibilityDate { get; set; }
            public string DOS { get; set; }
            public string Patient { get; set; }
            public string Plan { get; set; }
            public string Payer { get; set; }
            public string Status { get; set; }
            public string SubscriberID { get; set; }
            public string GroupNumber { get; set; }
            public string Provider { get; set; }
            public string Remarks { get; set; }
        }



    }
}

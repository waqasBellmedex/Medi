using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace MediFusionPM.BusinessLogic.ClaimGeneration
{
    public class ClaimData
    {
        public ClaimData()
        {
            this.BillPrvEntityType = "2";
            this.ClaimFreqCode = "1";
            
        }

        //NM1-85
        public string BillPrvTaxonomyCode { get; set; }
        public string BillPrvEntityType { get; set; }
        public string BillPrvOrgName { get; set; }
        public string BillPrvFirstName { get; set; }
        public string BillPrvMI { get; set; }
        public string BillPrvNPI { get; set; }
        //N3, N4
        public string BillPrvAddress1 { get; set; }
        public string BillPrvCity { get; set; }
        public string BillPrvState { get; set; }
        public string BillPrvZipCode { get; set; }
        //REF-EI, REF-SY
        public string BillPrvTaxID { get; set; }
        public string BillPrvSSN { get; set; }

        public string BillPrvSecondaryID { get; set; }

        //PER
        public string BillPrvContactName { get; set; }
        public string BillPrvTelephone { get; set; }
        public string BillPrvFax { get; set; }
        public string BillPrvEmail { get; set; }

        //NM1-87
        public string BillPrvPayToAddr { get; set; }
        public string BillPrvPayToCity { get; set; }
        public string BillPrvPayToState { get; set; }
        public string BillPrvPayToZip { get; set; }
      
        // SBR
        public string ClaimType { get; set; }
        public bool IsSelfSubscribed { get; set; }
        public string SbrGroupNumber { get; set; }
        public string SbrGroupName { get; set; }
        public string SbrMedicareSecTypeV { get; set; }
        public string PayerType { get; set; }
      
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
        public DateTime ?SBRDob { get; set; }
        public string SBRGender { get; set; }
        public string SBRSSN { get; set; }

        // NM1-PR    Payer Name
        public string PayerName { get; set; }
        public string PayerID { get; set; }

        //N3, N4      Payer Address
        public string PayerAddress { get; set; }
        public string PayerCity { get; set; }
        public string PayerState { get; set; }
        public string PayerZipCode { get; set; }

        // PAT        Patient
        public string PatientRelationShip { get; set; }

        // NM1-QC   Patient name
        public string PATEntityType { get; set; }
        public string PATLastName { get; set; }
        public string PATFirstName { get; set; }
        public string PATMiddleInitial { get; set; }
       

        //N3, N4    Patient Address, City, State, Zip
        public string PATAddress { get; set; }
        public string PATCity { get; set; }
        public string PATState { get; set; }
        public string PATZipCode { get; set; }

        //DMG       Patient DOB, Gender
        public DateTime ?PATDob { get; set; }
        public string PATGender { get; set; }

        //CLM       Claim Information
        public string PatientControlNumber { get; set; }
        public decimal ClaimAmount { get; set; }
        public string POSCode { get; set; }
        public string ClaimFreqCode { get; set; }
        public string ProvSignature { get; set; }
        public string PrvPayerAssignment { get; set; }
        public string BenefitsAssignment { get; set; }
        public string ReleaseOfInfo { get; set; }

        public string AccidentType { get; set; }
        public string AccidentState { get; set; }
        public string SpecialProgValue { get; set; }

        public string DelayReasonValue { get; set; }

        // Claim Dates
        public DateTime ?CurrentIllnessDate { get; set; }
        public DateTime ?InitialTreatmentDate { get; set; }
        public DateTime ?LastSeenDate { get; set; }
        public DateTime ?AcuteManifestationDate { get; set; }
        public DateTime ?AccidentDate { get; set; }
        public DateTime ?LMPDate { get; set; }
        public DateTime ?XrayDate { get; set; }
        public DateTime ?DisabilityStartDate { get; set; }
        public DateTime ?DisabilityEndDate { get; set; }
        public DateTime ?LastWorkedDate { get; set; }
        public DateTime ?AdmissionDate { get; set; }
        public DateTime ?DischargeDate { get; set; }
      
        //PWK   CLAIM SUPPLEMENTAL INFORMATION 
        //CN1   CONTRACT INFORMATION
        //AMT   PATIENT AMOUND PAID

        public string ServiceAuthExcV { get; set; }

        public bool IsMedicareMedigap { get; set; }
        public string MammographyNumber { get; set; }
        public string ReferralNumber { get; set; }
        public string PriorAuthNumber { get; set; }
        public string PayerClaimCntrlNum { get; set; }
        public string CliaNumber { get; set; }
      
        public string MedicarRecordNum { get; set; }
        public string ClaimNotes { get; set; }

        //CR1
        //CR2
        //CRC

        public string ICD1Code { get; set; }
        public string ICD2Code { get; set; }
        public string ICD3Code { get; set; }
        public string ICD4Code { get; set; }
        public string ICD5Code { get; set; }
        public string ICD6Code { get; set; }
        public string ICD7Code { get; set; }
        public string ICD8Code { get; set; }
        public string ICD9Code { get; set; }
        public string ICD10Code { get; set; }
        public string ICD11Code { get; set; }
        public string ICD12Code { get; set; }

        //HI    Anesthesia related procedure
        //HI    Condition Information
        //HCP   Claim Pricing Information

        public string RefPrvLastName { get; set; }
        public string RefPrvFirstName { get; set; }
        public string RefPrvMI { get; set; }
        public string RefPrvNPI { get; set; }

        public string RendPrvLastName { get; set; }
        public string RendPrvFirstName { get; set; }
        public string RendPrvMI { get; set; }
        public string RendPrvNPI { get; set; }
        public string RendPrvTaxonomy { get; set; }

        public string RendPrvSecondaryID { get; set; }

        public string LocationOrgName { get; set; }
        public string LocationNPI { get; set; }
        public string LocationAddress { get; set; }
        public string LocationCity { get; set; }
        public string LocationState { get; set; }
        public string LocationZip { get; set; }
       
        public string SuperPrvLastName { get; set; }
        public string SuperPrvFirstName { get; set; }
        public string SuperPrvMI { get; set; }
        public string SuperPrvNPI { get; set; }
       
        //NM1   Ambulance pick-up location
        //NM1   AMBULANCE DROP-OFF LOCATION

        public string OtherSBRPatRelationV { get; set; }

        public string OtherSBRGroupNumber { get; set; }
        public string OtherSBRGroupName { get; set; }
        public string OtherPayerTypeValue { get; set; }

        public decimal ?PrimaryPaidAmt { get; set; }
        public string OtherSBRLastName { get; set; }
        public string OtherSBRFirstName { get; set; }
        public string OtherSBRMI { get; set; }
        public string OtherSBRId { get; set; }
        public string OtherSBRAddress { get; set; }
        public string OtherSBRCity { get; set; }
        public string OtherSBRState { get; set; }
        public string OtherSBRZipCode { get; set; }

        public string OtherPayerName { get; set; }
        public string OtherPayerID { get; set; }

        public string OtherPayerAddress { get; set; }
        public string OtherPayerCity { get; set; }
        public string OtherPayerState { get; set; }
        public string OtherPayerZipCode { get; set; }

        public List<ChargeData> Charges { get; set; }

        public DateTime SubmittedDate { get; internal set; }
        public bool Submitted { get; internal set; }

        public string ValidationMsg { get; internal set; }
        public long VisitID { get; set; }
        public long ?PrimaryVisitID { get; set; }

        // Add New Fields 
        public string SecondaryStatus { get; set; }
        public long? SecondaryPatientPlanID { get; set; }
        public long PatientID { get; set; }


        public long ProviderID { get; set; }
        public long InsruancePlanID { get; set; }
        public long LocationID { get; set; }
        public long PracticeID { get; set; }
        public bool ?BillUnderProvider { get; set; }

        public string PrescribingMD { get; set; }

        // Instead OF ICDCode  Fields
        public long? ICD7CodeID { get; set; }
        public long? ICD8CodeID { get; set; }
        public long? ICD9CodeID { get; set; }
        public long? ICD10CodeID { get; set; }
        public long? ICD11CodeID { get; set; }
        public long? ICD12CodeID { get; set; }
        //Instead of SupervisingProviderDetails
        public long? SupervisingProvID { get; set; }
        public long? InsurancePlanAddressID { get; set; }



    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.Models.Main
{
    public class MainRights
    {
        [Key, ForeignKey("MainAuthIdentityCustom")]
        public string Id { get; set; }


        //Scheduler Rights
        public bool SchedulerCreate { get; set; }
        public bool SchedulerEdit { get; set; }
        public bool SchedulerDelete { get; set; }
        public bool SchedulerSearch { get; set; }
        public bool SchedulerImport { get; set; }
        public bool SchedulerExport { get; set; }



        //Patient Rights
        public bool PatientCreate { get; set; }
        public bool PatientEdit { get; set; }
        public bool PatientDelete { get; set; }
        public bool PatientSearch { get; set; }
        public bool PatientImport { get; set; }
        public bool PatientExport { get; set; }

        //Charges Rights
        public bool ChargesCreate { get; set; }
        public bool ChargesEdit { get; set; }
        public bool ChargesDelete { get; set; }
        public bool ChargesSearch { get; set; }
        public bool ChargesImport { get; set; }
        public bool ChargesExport { get; set; }

        //Documents Rights
        public bool DocumentsCreate { get; set; }
        public bool DocumentsEdit { get; set; }
        public bool DocumentsDelete { get; set; }
        public bool DocumentsSearch { get; set; }
        public bool DocumentsImport { get; set; }
        public bool DocumentsExport { get; set; }

        //Submissions Rights
        public bool SubmissionsCreate { get; set; }
        public bool SubmissionsEdit { get; set; }
        public bool SubmissionsDelete { get; set; }
        public bool SubmissionsSearch { get; set; }
        public bool SubmissionsImport { get; set; }
        public bool SubmissionsExport { get; set; }

        //Payments Rights
        public bool PaymentsCreate { get; set; }
        public bool PaymentsEdit { get; set; }
        public bool PaymentsDelete { get; set; }
        public bool PaymentsSearch { get; set; }
        public bool PaymentsImport { get; set; }
        public bool PaymentsExport { get; set; }

        //Followup Rights
        public bool FollowupCreate { get; set; }
        public bool FollowupEdit { get; set; }
        public bool FollowupDelete { get; set; }
        public bool FollowupSearch { get; set; }
        public bool FollowupImport { get; set; }
        public bool FollowupExport { get; set; }

        //Reports Rights
        public bool ReportsCreate { get; set; }
        public bool ReportsEdit { get; set; }
        public bool ReportsDelete { get; set; }
        public bool ReportsSearch { get; set; }
        public bool ReportsImport { get; set; }
        public bool ReportsExport { get; set; }


        ////SetUp Client 
        //Client Rights
        public bool ClientCreate { get; set; }
        public bool ClientEdit { get; set; }
        public bool ClientDelete { get; set; }
        public bool ClientSearch { get; set; }
        public bool ClientImport { get; set; }
        public bool ClientExport { get; set; }

        public bool UserCreate { get; set; }
        public bool UserEdit { get; set; }
        public bool UserDelete { get; set; }
        public bool UserSearch { get; set; }
        public bool UserImport { get; set; }
        public bool UserExport { get; set; }

        ////SetUp Admin 
        //Practice
        public bool PracticeCreate { get; set; }
        public bool PracticeEdit { get; set; }
        public bool PracticeDelete { get; set; }
        public bool PracticeSearch { get; set; }
        public bool PracticeImport { get; set; }
        public bool PracticeExport { get; set; }


        //Location
        public bool LocationCreate { get; set; }
        public bool LocationEdit { get; set; }
        public bool LocationDelete { get; set; }
        public bool LocationSearch { get; set; }
        public bool LocationImport { get; set; }
        public bool LocationExport { get; set; }

        //Provider
        public bool ProviderCreate { get; set; }
        public bool ProviderEdit { get; set; }
        public bool ProviderDelete { get; set; }
        public bool ProviderSearch { get; set; }
        public bool ProviderImport { get; set; }
        public bool ProviderExport { get; set; }


        //Referring Provider
        public bool ReferringProviderCreate { get; set; }
        public bool ReferringProviderEdit { get; set; }
        public bool ReferringProviderDelete { get; set; }
        public bool ReferringProviderSearch { get; set; }
        public bool ReferringProviderImport { get; set; }
        public bool ReferringProviderExport { get; set; }

        //Setup Insurance
        //Insurance

        public bool InsuranceCreate { get; set; }
        public bool InsuranceEdit { get; set; }
        public bool InsuranceDelete { get; set; }
        public bool InsuranceSearch { get; set; }
        public bool InsuranceImport { get; set; }
        public bool InsuranceExport { get; set; }

        //Insurance Plan 
        public bool InsurancePlanCreate { get; set; }
        public bool InsurancePlanEdit { get; set; }
        public bool InsurancePlanDelete { get; set; }
        public bool InsurancePlanSearch { get; set; }
        public bool InsurancePlanImport { get; set; }
        public bool InsurancePlanExport { get; set; }

        //Insurance Plan Address 
        public bool InsurancePlanAddressCreate { get; set; }
        public bool InsurancePlanAddressEdit { get; set; }
        public bool InsurancePlanAddressDelete { get; set; }
        public bool InsurancePlanAddressSearch { get; set; }
        public bool InsurancePlanAddressImport { get; set; }
        public bool InsurancePlanAddressExport { get; set; }

        //EDI Submit
        public bool EDISubmitCreate { get; set; }
        public bool EDISubmitEdit { get; set; }
        public bool EDISubmitDelete { get; set; }
        public bool EDISubmitSearch { get; set; }
        public bool EDISubmitImport { get; set; }
        public bool EDISubmitExport { get; set; }

        //EDI EligiBility
        public bool EDIEligiBilityCreate { get; set; }
        public bool EDIEligiBilityEdit { get; set; }
        public bool EDIEligiBilityDelete { get; set; }
        public bool EDIEligiBilitySearch { get; set; }
        public bool EDIEligiBilityImport { get; set; }
        public bool EDIEligiBilityExport { get; set; }

        //EDI Status
        public bool EDIStatusCreate { get; set; }
        public bool EDIStatusEdit { get; set; }
        public bool EDIStatusDelete { get; set; }
        public bool EDIStatusSearch { get; set; }
        public bool EDIStatusImport { get; set; }
        public bool EDIStatusExport { get; set; }

        //Coding
        //ICD
        public bool ICDCreate { get; set; }
        public bool ICDEdit { get; set; }
        public bool ICDDelete { get; set; }
        public bool ICDSearch { get; set; }
        public bool ICDImport { get; set; }
        public bool ICDExport { get; set; }

        //CPT
        public bool CPTCreate { get; set; }
        public bool CPTEdit { get; set; }
        public bool CPTDelete { get; set; }
        public bool CPTSearch { get; set; }
        public bool CPTImport { get; set; }
        public bool CPTExport { get; set; }

        //Modifiers
        public bool ModifiersCreate { get; set; }
        public bool ModifiersEdit { get; set; }
        public bool ModifiersDelete { get; set; }
        public bool ModifiersSearch { get; set; }
        public bool ModifiersImport { get; set; }
        public bool ModifiersExport { get; set; }

        //POS
        public bool POSCreate { get; set; }
        public bool POSEdit { get; set; }
        public bool POSDelete { get; set; }
        public bool POSSearch { get; set; }
        public bool POSImport { get; set; }
        public bool POSExport { get; set; }

        //EDI Codes
        //Claim Status Category Codes
        public bool ClaimStatusCategoryCodesCreate { get; set; }
        public bool ClaimStatusCategoryCodesEdit { get; set; }
        public bool ClaimStatusCategoryCodesDelete { get; set; }
        public bool ClaimStatusCategoryCodesSearch { get; set; }
        public bool ClaimStatusCategoryCodesImport { get; set; }
        public bool ClaimStatusCategoryCodesExport { get; set; }

        //Claim Status Codes
        public bool ClaimStatusCodesCreate { get; set; }
        public bool ClaimStatusCodesEdit { get; set; }
        public bool ClaimStatusCodesDelete { get; set; }
        public bool ClaimStatusCodesSearch { get; set; }
        public bool ClaimStatusCodesImport { get; set; }
        public bool ClaimStatusCodesExport { get; set; }

        //Adjustment Codes
        public bool AdjustmentCodesCreate { get; set; }
        public bool AdjustmentCodesEdit { get; set; }
        public bool AdjustmentCodesDelete { get; set; }
        public bool AdjustmentCodesSearch { get; set; }
        public bool AdjustmentCodesImport { get; set; }
        public bool AdjustmentCodesExport { get; set; }

        //Remark Codes
        public bool RemarkCodesCreate { get; set; }
        public bool RemarkCodesEdit { get; set; }
        public bool RemarkCodesDelete { get; set; }
        public bool RemarkCodesSearch { get; set; }
        public bool RemarkCodesImport { get; set; }
        public bool RemarkCodesExport { get; set; }



        //Team Rights
        public bool teamCreate { get; set; }
        public bool teamupdate { get; set; }
        public bool teamDelete { get; set; }
        public bool teamSearch { get; set; }
        public bool teamExport { get; set; }
        public bool teamImport { get; set; }

        //Receiver Rights
        public bool receiverCreate { get; set; }
        public bool receiverupdate { get; set; }
        public bool receiverDelete { get; set; }
        public bool receiverSearch { get; set; }
        public bool receiverExport { get; set; }
        public bool receiverImport { get; set; }

        //Submitter Rights
        public bool submitterCreate { get; set; }
        public bool submitterUpdate { get; set; }
        public bool submitterDelete { get; set; }
        public bool submitterSearch { get; set; }
        public bool submitterExport { get; set; }
        public bool submitterImport { get; set; }




        //PatientPlan Rights
        public bool patientPlanCreate { get; set; }
        public bool patientPlanUpdate { get; set; }
        public bool patientPlanDelete { get; set; }
        public bool patientPlanSearch { get; set; }
        public bool patientPlanExport { get; set; }
        public bool patientPlanImport { get; set; }
        public bool performEligibility { get; set; }


        //PatientPayment Rights
        public bool patientPaymentCreate { get; set; }
        public bool patientPaymentUpdate { get; set; }
        public bool patientPaymentDelete { get; set; }
        public bool patientPaymentSearch { get; set; }
        public bool patientPaymentExport { get; set; }
        public bool patientPaymentImport { get; set; }

        // Charges Rights
        public bool resubmitCharges { get; set; }
        // BatchDocument Rights
        public bool batchdocumentCreate { get; set; }
        public bool batchdocumentUpdate { get; set; }
        public bool batchdocumentDelete { get; set; }
        public bool batchdocumentSearch { get; set; }
        public bool batchdocumentExport { get; set; }
        public bool batchdocumentImport { get; set; }





        // ElectronicSubmission Rights
        public bool electronicsSubmissionSearch { get; set; }
        public bool electronicsSubmissionSubmit { get; set; }
        public bool electronicsSubmissionResubmit { get; set; }

        // PaperSubmission Rights
        public bool paperSubmissionSearch { get; set; }
        public bool paperSubmissionSubmit { get; set; }
        public bool paperSubmissionResubmit { get; set; }
        // SubmissionLog Rights	
        public bool submissionLogSearch { get; set; }
        // PlanFollowup Rights	
        public bool planFollowupSearch { get; set; }
        public bool planFollowupCreate { get; set; }
        public bool planFollowupDelete { get; set; }
        public bool planFollowupUpdate { get; set; }
        public bool planFollowupImport { get; set; }
        public bool planFollowupExport { get; set; }

        // PatientFollowup Rights	
        public bool patientFollowupSearch { get; set; }
        public bool patientFollowupCreate { get; set; }
        public bool patientFollowupDelete { get; set; }
        public bool patientFollowupUpdate { get; set; }
        public bool patientFollowupImport { get; set; }
        public bool patientFollowupExport { get; set; }

        // Group Rights	
        public bool groupSearch { get; set; }
        public bool groupCreate { get; set; }
        public bool groupUpdate { get; set; }
        public bool groupDelete { get; set; }
        public bool groupExport { get; set; }
        public bool groupImport { get; set; }
        // Reason Rights	
        public bool reasonSearch { get; set; }
        public bool reasonCreate { get; set; }
        public bool reasonUpdate { get; set; }
        public bool reasonDelete { get; set; }
        public bool reasonExport { get; set; }
        public bool reasonImport { get; set; }

        public bool addPaymentVisit { get; set; }
        public bool DeleteCheck { get; set; }
        public bool ManualPosting { get; set; }
        public bool Postcheck { get; set; }
        public bool PostExport { get; set; }
        public bool PostImport { get; set; }


        public bool ManualPostingAdd { get; set; }
        public bool ManualPostingUpdate { get; set; }
        public bool PostCheckSearch { get; set; }
        public bool DeletePaymentVisit { get; set; }



        //public bool Create { get; set; }
        //public bool Edit { get; set; }
        //public bool Delete { get; set; }
        //public bool Search { get; set; }
        //public bool Import { get; set; }
        //public bool Export { get; set; }

        public string AssignedByUserId { get; set; }
        public string AddedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDate { get; set; }
        // public AuthIdentityCustom AuthIdentityCustom { get; set; }
    }
}

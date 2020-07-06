using MediFusionPM.DashboardController;
using MediFusionPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MediFusionPM.ViewModels.VMElectronicSubmission;
using static MediFusionPM.ViewModels.VMPaperSubmission;
using static MediFusionPM.ViewModels.VMPatientFollowup;
using static MediFusionPM.ViewModels.VMPaymentCheck;
using static MediFusionPM.ViewModels.VMPlanFollowUp;
using static MediFusionPM.ViewModels.VMSubmissionLog;
using static MediFusionPM.ViewModels.VMVisit;

namespace MediFusionPM.Dashboard
{
    public class BillingDashboard
    {

        public BillingDashboard()
        {
        }
        public class ReasonVisitAmmount
        {
            public string ReasonName { get; set; }
            public int VisitCount { get; set; }
            public long TotalAmmount { get; set; }

        }

        public class ReasonVisitAmmountGraph
        {
            public string ReasonName { get; set; }
            public int VisitCount { get; set; }

        }

        public class SearchBillingDoard
        {
            public string Value { get; set; }
        }

        public class GetChargePayment
        {
            public int Year { get; set; }
            public string Month { get; set; }
            public decimal Charge { get; set; }
            public decimal PlanPayment { get; set; }
            public decimal PatientPaid { get; set; }
            public decimal Adjustments { get; set; }
            public decimal PlanBal { get; set; }
            public decimal PatBal { get; set; }
            public decimal Payment { get; set; }
            public string YearMonth { get; set; }
            public decimal TotalBal { get; set; }

            }
        public class GetChargePaymentGraph
        {
            public int Year { get; set; }
            public string Month { get; set; }
            public long Charge { get; set; }
            public long PlanPayment { get; set; }
            public long PatientPaid { get; set; }
            public long Adjustments { get; set; }
            public long PlanBal { get; set; }
            public long PatBal { get; set; }

        }


        public int PaperSubmission { get; set; }
        public int ElectronicSubmission { get; set; }
        public int SystemReject { get; set; }
        public int ClameReject { get; set; }
        public int ClameDenied { get; set; }
        public int ERANeedPosting { get; set; }
        public int ERAFailed { get; set; }
        public int PlaneFollowUp { get; set; }
        public int PatientFollowUp { get; set; }
        public long ClaimSubmitedDaily { get; set; }
        public int paymentReceived { get; set; }
        public int CheckPosted { get; set; }
        public int ClaimEntered { get; set; }
        public int PlaneFollowUpDaily { get; set; }
        public int PatientFollowUpDaily { get; set; }
        public int ERAFailedDaily { get; set; }
        public void OverallSummary(ClientDbContext contextClient, MainContext mainContext, long PracticeID, long ClientID, string Email, string Role, string UserID,string contextName)
        {

            BillingDashboardController Bdc = new BillingDashboardController(contextClient, mainContext);

            //PaperSubmission
            // PaperSubmissionController psc = new PaperSubmissionController(contextClient, mainContext);
            CPaperSubmission PaperSubmissionModel = new CPaperSubmission();
            var PaperSubmissionList = Bdc.FindVisitsPaperSubmission(PaperSubmissionModel, PracticeID, ClientID, Email, Role, UserID);
            PaperSubmission = PaperSubmissionList.Count();


            //ElectronicSubmission
            //  ElectronicSubmissionController esc = new ElectronicSubmissionController(contextClient, null, mainContext);
            CElectronicSubmission ElectronicSubmissionModel = new CElectronicSubmission();
            ElectronicSubmissionModel.ReceiverID = 1;
            var ElectronicSubmissionList = Bdc.FindVisits(ElectronicSubmissionModel, PracticeID, ClientID, Email, Role, UserID, contextName);
            ElectronicSubmission = ElectronicSubmissionList.Count();


            //SystemReject
            //VisitController srvis = new VisitController(contextClient, mainContext);
            CVisit SystemRejectVisitModel = new CVisit();
            var SystemRejectVisitList = Bdc.FindVisitsVMess(SystemRejectVisitModel, PracticeID, ClientID, Email, Role, UserID);
            SystemReject = SystemRejectVisitList.Count();

            //ClameReject 
            CVisit VisitModel = new CVisit();
            VisitModel.Status = "R";
            var VisitList = Bdc.FindVisits(VisitModel, PracticeID, ClientID, Email, Role, UserID, contextName);
            ClameReject = VisitList.Count();

            //ClameDenied
            VisitModel.Status = "DN";
            var CdVisitList = Bdc.FindVisits(VisitModel, PracticeID, ClientID, Email, Role, UserID, contextName);
            ClameDenied = CdVisitList.Count();


            //ERANeedPosting
            //  PaymentCheckController pch = new PaymentCheckController(contextClient, mainContext);
            CPaymentCheck ERANeedPostingModel = new CPaymentCheck();
            ERANeedPostingModel.Status = "NP";
            var ERANeedPostingList = Bdc.FindPaymentChecks(ERANeedPostingModel, PracticeID, ClientID, Email, Role, UserID);
            ERANeedPosting = ERANeedPostingList.Count();

            //ERAFailed
            CPaymentCheck ERAFailedPostingModel = new CPaymentCheck();
            ERAFailedPostingModel.Status = "F";
            var ERAFailedPostingList = Bdc.FindPaymentChecks(ERAFailedPostingModel, PracticeID, ClientID, Email, Role, UserID);
            ERAFailed = ERAFailedPostingList.Count();



            //PlaneFollowUp
            //PlanFollowupController pfc = new PlanFollowupController(contextClient, mainContext);
            CPlanFollowup PlanFollowupModel = new CPlanFollowup();
            PlanFollowupModel.Resolved = false;
            var PlanFollowUpList = Bdc.FindPlanFollowUp(PlanFollowupModel, PracticeID, ClientID, Email, Role, UserID);
            PlaneFollowUp = PlanFollowUpList.Count();


            //PatientFollowUp
            //  PatientFollowUpController patfc = new PatientFollowUpController(contextClient, mainContext);
            CPatientFollowup PatientFollowUpModel = new CPatientFollowup();
            PatientFollowUpModel.Resolved = false;
            var PatientFollowUpList = Bdc.FindPatientFollowUp(PatientFollowUpModel, PracticeID, ClientID, Email, Role, UserID);
            PatientFollowUp = PatientFollowUpList.Count();


            //*********************************************************************

            //Daily Summary

            // SubmissionLogController slc = new SubmissionLogController(contextClient, mainContext);
            CSubmissionLog SubmissionLogModel = new CSubmissionLog();
            //SubmissionLogModel.SubmitType = "E";
            // SubmissionLogModel.SubmitDate = DateTime.Now.Date.AddDays(-1);

            SubmissionLogModel.SubmitDate = DateTime.Now.Date;
            long SubmissionLogList = Bdc.FindSubmissionLog(SubmissionLogModel, PracticeID, ClientID, Email, Role, UserID);
            ClaimSubmitedDaily = SubmissionLogList;


            //paymentReceivedDaily

            CPaymentCheck paymentReceivedModel = new CPaymentCheck();
            paymentReceivedModel.EntryDateFrom = DateTime.Now.Date;
            var paymentReceivedList = Bdc.FindPaymentChecks(paymentReceivedModel, PracticeID, ClientID, Email, Role, UserID);
            paymentReceived = paymentReceivedList.Count();


            //CheckPostedDaily
            CPaymentCheck CheckPostedModel = new CPaymentCheck();
            CheckPostedModel.EntryDateFrom = DateTime.Now.Date;
            CheckPostedModel.Status = "P";
            var CheckPostedList = Bdc.FindPaymentChecks(CheckPostedModel, PracticeID, ClientID, Email, Role, UserID);
            CheckPosted = CheckPostedList.Count();

            //ClaimEnteredDaily

            CVisit ClaimEnteredVisitModel = new CVisit();
            ClaimEnteredVisitModel.EntryDateFrom = DateTime.Now.Date;
            var ClaimEnteredVisitList = Bdc.FindVisits(ClaimEnteredVisitModel, PracticeID, ClientID, Email, Role, UserID, contextName);
            ClaimEntered = ClaimEnteredVisitList.Count();

            //PlanFollowUpDaily
            //  PlanFollowupController pfcd = new PlanFollowupController(contextClient, mainContext);
            CPlanFollowup PlaneFollowUpDailyModel = new CPlanFollowup();
            PlaneFollowUpDailyModel.SubmitDate = DateTime.Now.Date;
            var PlaneFollowUpDailyList = Bdc.FindPlanFollowUp(PlaneFollowUpDailyModel, PracticeID, ClientID, Email, Role, UserID);
            PlaneFollowUpDaily = PlaneFollowUpDailyList.Count();

            //PatientFollowUpDaily
            // PatientFollowUpController patfcd = new PatientFollowUpController(contextClient, mainContext);
            CPatientFollowup PatientFollowUpDailyModel = new CPatientFollowup();
            PatientFollowUpDailyModel.FollowUpDate = DateTime.Now.Date;
            var PatientFollowUpDailyList = Bdc.FindPatientFollowUp(PatientFollowUpDailyModel, PracticeID, ClientID, Email, Role, UserID);
            PatientFollowUpDaily = PatientFollowUpDailyList.Count();

            //ERAFailedDaily
            CPaymentCheck ERAFailedDailyModel = new CPaymentCheck();
            ERAFailedDailyModel.EntryDateFrom = DateTime.Now.Date;
            ERAFailedDailyModel.Status = "F";
            var ERAFailedDailyList = Bdc.FindPaymentChecks(ERAFailedDailyModel, PracticeID, ClientID, Email, Role, UserID);
            ERAFailedDaily = ERAFailedDailyList.Count();


        }






        public void BillingClaimAndERASummary(ClientDbContext contextClient, MainContext mainContext, long PracticeID, long ClientID, string Email, string Role, string UserID, string contextName)
        {

            BillingDashboardController Bdc = new BillingDashboardController(contextClient, mainContext);

            //PaperSubmission
            // PaperSubmissionController psc = new PaperSubmissionController(contextClient, mainContext);
            CPaperSubmission PaperSubmissionModel = new CPaperSubmission();
            var PaperSubmissionList = Bdc.FindVisitsPaperSubmission(PaperSubmissionModel, PracticeID, ClientID, Email, Role, UserID);
            PaperSubmission = PaperSubmissionList.Count();


            //ElectronicSubmission
            //  ElectronicSubmissionController esc = new ElectronicSubmissionController(contextClient, null, mainContext);
            CElectronicSubmission ElectronicSubmissionModel = new CElectronicSubmission();
            ElectronicSubmissionModel.ReceiverID = 1;
            var ElectronicSubmissionList = Bdc.FindVisits(ElectronicSubmissionModel, PracticeID, ClientID, Email, Role, UserID, contextName);
            ElectronicSubmission = ElectronicSubmissionList.Count();


            //SystemReject
            //VisitController srvis = new VisitController(contextClient, mainContext);
            CVisit SystemRejectVisitModel = new CVisit();
            var SystemRejectVisitList = Bdc.FindVisitsVMess(SystemRejectVisitModel, PracticeID, ClientID, Email, Role, UserID);
            SystemReject = SystemRejectVisitList.Count();

            //ClameReject 
            CVisit VisitModel = new CVisit();
            VisitModel.Status = "R";
            var VisitList = Bdc.FindVisits(VisitModel, PracticeID, ClientID, Email, Role, UserID, contextName);
            ClameReject = VisitList.Count();

            //ClameDenied
            VisitModel.Status = "DN";
            var CdVisitList = Bdc.FindVisits(VisitModel, PracticeID, ClientID, Email, Role, UserID, contextName);
            ClameDenied = CdVisitList.Count();


            //ERANeedPosting
            //  PaymentCheckController pch = new PaymentCheckController(contextClient, mainContext);
            CPaymentCheck ERANeedPostingModel = new CPaymentCheck();
            ERANeedPostingModel.Status = "NP";
            var ERANeedPostingList = Bdc.FindPaymentChecks(ERANeedPostingModel, PracticeID, ClientID, Email, Role, UserID);
            ERANeedPosting = ERANeedPostingList.Count();

            //ERAFailed
            CPaymentCheck ERAFailedPostingModel = new CPaymentCheck();
            ERAFailedPostingModel.Status = "F";
            var ERAFailedPostingList = Bdc.FindPaymentChecks(ERAFailedPostingModel, PracticeID, ClientID, Email, Role, UserID);
            ERAFailed = ERAFailedPostingList.Count();
        }


        public void BillingFollowUpAndOtherSummary(ClientDbContext contextClient, MainContext mainContext, long PracticeID, long ClientID, string Email, string Role, string UserID, string contextName)
        {

            BillingDashboardController Bdc = new BillingDashboardController(contextClient, mainContext);


            //PlaneFollowUp
            //PlanFollowupController pfc = new PlanFollowupController(contextClient, mainContext);
            CPlanFollowup PlanFollowupModel = new CPlanFollowup();
            PlanFollowupModel.Resolved = false;
            var PlanFollowUpList = Bdc.FindPlanFollowUp(PlanFollowupModel, PracticeID, ClientID, Email, Role, UserID);
            PlaneFollowUp = PlanFollowUpList.Count();


            //PatientFollowUp
            //  PatientFollowUpController patfc = new PatientFollowUpController(contextClient, mainContext);
            CPatientFollowup PatientFollowUpModel = new CPatientFollowup();
            PatientFollowUpModel.Resolved = false;
            var PatientFollowUpList = Bdc.FindPatientFollowUp(PatientFollowUpModel, PracticeID, ClientID, Email, Role, UserID);
            PatientFollowUp = PatientFollowUpList.Count();

        }




        public void BillingDailySummary(ClientDbContext contextClient, MainContext mainContext, long PracticeID, long ClientID, string Email, string Role, string UserID, string contextName)
        {

            BillingDashboardController Bdc = new BillingDashboardController(contextClient, mainContext);

            //Daily Summary

            // SubmissionLogController slc = new SubmissionLogController(contextClient, mainContext);
            CSubmissionLog SubmissionLogModel = new CSubmissionLog();
            //SubmissionLogModel.SubmitType = "E";
            // SubmissionLogModel.SubmitDate = DateTime.Now.Date.AddDays(-1);

            SubmissionLogModel.SubmitDate = DateTime.Now.Date;
            long SubmissionLogList = Bdc.FindSubmissionLog(SubmissionLogModel, PracticeID, ClientID, Email, Role, UserID);
            ClaimSubmitedDaily = SubmissionLogList;


            //paymentReceivedDaily

            CPaymentCheck paymentReceivedModel = new CPaymentCheck();
            paymentReceivedModel.EntryDateFrom = DateTime.Now.Date;
            var paymentReceivedList = Bdc.FindPaymentChecks(paymentReceivedModel, PracticeID, ClientID, Email, Role, UserID);
            paymentReceived = paymentReceivedList.Count();


            //CheckPostedDaily
            CPaymentCheck CheckPostedModel = new CPaymentCheck();
            CheckPostedModel.EntryDateFrom = DateTime.Now.Date;
            CheckPostedModel.Status = "P";
            var CheckPostedList = Bdc.FindPaymentChecks(CheckPostedModel, PracticeID, ClientID, Email, Role, UserID);
            CheckPosted = CheckPostedList.Count();

            //ClaimEnteredDaily

            CVisit ClaimEnteredVisitModel = new CVisit();
            ClaimEnteredVisitModel.EntryDateFrom = DateTime.Now.Date;
            var ClaimEnteredVisitList = Bdc.FindVisits(ClaimEnteredVisitModel, PracticeID, ClientID, Email, Role, UserID, contextName);
            ClaimEntered = ClaimEnteredVisitList.Count();

            //PlanFollowUpDaily
            //  PlanFollowupController pfcd = new PlanFollowupController(contextClient, mainContext);
            CPlanFollowup PlaneFollowUpDailyModel = new CPlanFollowup();
            PlaneFollowUpDailyModel.SubmitDate = DateTime.Now.Date;
            var PlaneFollowUpDailyList = Bdc.FindPlanFollowUp(PlaneFollowUpDailyModel, PracticeID, ClientID, Email, Role, UserID);
            PlaneFollowUpDaily = PlaneFollowUpDailyList.Count();

            //PatientFollowUpDaily
            // PatientFollowUpController patfcd = new PatientFollowUpController(contextClient, mainContext);
            CPatientFollowup PatientFollowUpDailyModel = new CPatientFollowup();
            PatientFollowUpDailyModel.FollowUpDate = DateTime.Now.Date;
            var PatientFollowUpDailyList = Bdc.FindPatientFollowUp(PatientFollowUpDailyModel, PracticeID, ClientID, Email, Role, UserID);
            PatientFollowUpDaily = PatientFollowUpDailyList.Count();

            //ERAFailedDaily
            CPaymentCheck ERAFailedDailyModel = new CPaymentCheck();
            ERAFailedDailyModel.EntryDateFrom = DateTime.Now.Date;
            ERAFailedDailyModel.Status = "F";
            var ERAFailedDailyList = Bdc.FindPaymentChecks(ERAFailedDailyModel, PracticeID, ClientID, Email, Role, UserID);
            ERAFailedDaily = ERAFailedDailyList.Count();


        }






    }
}
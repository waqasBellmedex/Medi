using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using MediFusionPM.BusinessLogic;
using MediFusionPM.BusinessLogic.ClaimGeneration;
using MediFusionPM.Models;
using MediFusionPM.Models.TodoApi.Models;
using MediFusionPM.ViewModels;
using static MediFusionPM.BusinessLogic.ClaimGeneration.ClaimGenerator;
using static MediFusionPM.BusinessLogic.ClaimGeneration.ClaimGenerator.Output;
using static MediFusionPM.ViewModels.VMCommon;
using static MediFusionPM.ViewModels.VMElectronicSubmission;



namespace MediFusionPM.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class ElectronicSubmissionController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;

        public IConfiguration Configuration { get; }

        public ElectronicSubmissionController(ClientDbContext context, IConfiguration configuration, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            Configuration = configuration;
            _startTime = DateTime.Now;

        }


        [Route("GetProfiles")]
        [HttpGet()]
        public async Task<ActionResult<VMElectronicSubmission>> GetProfiles()
        {
           
            ViewModels.VMElectronicSubmission obj = new ViewModels.VMElectronicSubmission();
            obj.GetProfiles(_context);
            return obj;
        }

        [HttpPost]
        [Route("FindVisits")]
        public async Task<ActionResult<IEnumerable<GElectronicSubmission>>> FindVisits(CElectronicSubmission CModel)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.planFollowupSearch == false)
            //return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            return FindVisits(CModel, UD);

        }

        
        private List <GElectronicSubmission> FindVisits(CElectronicSubmission CModel, UserInfoData UD)
        {
            Submitter submitter = _context.Submitter.Where(m => m.ReceiverID == CModel.ReceiverID && m.ClientID == UD.ClientID).SingleOrDefault();
            if (submitter == null)
            {
               // return BadRequest("Submitter is not Setup");

            }
            List<GElectronicSubmission> PrimaryVisits
            = (from v in _context.Visit
               join pat in _context.Patient
                            on v.PatientID equals pat.ID
               join prac in _context.Practice
                            on pat.PracticeID equals prac.ID
               join loc in _context.Location
                            on v.LocationID equals loc.ID
               join prov in _context.Provider
                            on v.ProviderID equals prov.ID
               join pPlan in _context.PatientPlan
                            on pat.ID equals pPlan.PatientID
               join insPlan in _context.InsurancePlan
                            on pPlan.InsurancePlanID equals insPlan.ID
               join x12_837 in _context.Edi837Payer
                            on insPlan.Edi837PayerID equals x12_837.ID
               join rec in _context.Receiver
                            on x12_837.ReceiverID equals rec.ID
               join sub in _context.Submitter
                            on rec.ID equals sub.ReceiverID
               //join up in _context.UserPractices on prac.ID equals up.PracticeID
               //join u in _context.Users on up.UserID equals u.Id
               where      v.PrimaryPatientPlanID == pPlan.ID &&
                           (insPlan.SubmissionType.Equals("E")) &&
                            prac.ID == UD.PracticeID &&
                            sub.ClientID == UD.ClientID &&
                            (v.IsSubmitted == false) && (v.PrimaryStatus == "N" || v.PrimaryStatus.IsNull2() || v.PrimaryStatus == "RS") &&
                             //(v.IsDontPrint == null ? false : v.IsDontPrint == false) &&
                             //(v.IsForcePaper == null ? false : v.IsForcePaper == false) &&
                             v.IsForcePaper == false &&
                             v.IsDontPrint == false &&
                           (sub.ReceiverID.Equals(CModel.ReceiverID)) &&
                            (string.IsNullOrEmpty(CModel.ReceiverID.ToString()) ? true : rec.ID.Equals(CModel.ReceiverID)) &&
                            (string.IsNullOrEmpty(CModel.AccountNum) ? true : pat.AccountNum.Equals(CModel.AccountNum)) &&
                            (string.IsNullOrEmpty(CModel.Practice) ? true : prac.Name.Contains(CModel.Practice)) &&
                            (string.IsNullOrEmpty(CModel.Location) ? true : loc.Name.Contains(CModel.Location)) &&
                            (CModel.Location.IsNull2() ? true : loc.Name.Contains(CModel.Location)) &&
                            (string.IsNullOrEmpty(CModel.Provider) ? true : prov.Name.Contains(CModel.Provider)) &&
                            (string.IsNullOrEmpty(CModel.PayerID) ? true : x12_837.PayerID.Equals(CModel.PayerID)) &&
                            (string.IsNullOrEmpty(CModel.PlanName) ? true : insPlan.PlanName.Contains(CModel.PlanName)) &&
                            (CModel.VisitID.IsNull() ? true : v.ID.Equals(CModel.VisitID))&&
                            (CModel.InsuranceType == "P" ? v.PrimaryPatientPlanID.Value > 0 && v.SecondaryPatientPlanID == null && v.TertiaryPatientPlanID == null :
                            CModel.InsuranceType == "S" ? v.PrimaryPatientPlanID.Value > 0 && v.SecondaryPatientPlanID > 0 && v.TertiaryPatientPlanID == null :
                            CModel.InsuranceType == "T" ? v.PrimaryPatientPlanID.Value > 0 && v.SecondaryPatientPlanID > 0 && v.TertiaryPatientPlanID > 0 :
                            v.PrimaryPatientPlanID.Value > 0)

                         && (ExtensionMethods.IsBetweenDOS(CModel.EntryDateTo, CModel.EntryDateFrom, v.AddedDate, v.AddedDate))
               select new GElectronicSubmission()
               {
                   AccountNum = pat.AccountNum,
                   DOS = v.DateOfServiceFrom.Format("MM/dd/yyyy"),
                   PracticeID = prac.ID,
                   Practice = prac.Name,
                   PatientID = pat.ID,
                   PlanName = insPlan.PlanName,
                   Coverage = pPlan.Coverage,
                   PrimaryStatus = v.PrimaryStatus,
                   SecondaryStatus = v.SecondaryStatus,
                   InsurancePlanID = insPlan.ID.ToString(),
                   Patient = pat.LastName + ", " + pat.FirstName,
                   ProviderID = prov.ID,
                   Provider = prov.Name,
                   VisitID = v.ID,
                   ID = 0,
                   LocationID = loc.ID,
                   Location = loc.Name,
                   TotalAmount = v.TotalAmount,
                   ValidationMessage = v.ValidationMessage,
                   IsSubmitted = v.IsSubmitted,
                   subscriberID = pPlan.SubscriberId,
                   VisitEntryDate = v.AddedDate.ToString("MM/dd/yyyy")
               }).ToList<GElectronicSubmission>();

            // Find Those Visits which are submitted to Secondary Insurance
            List<GElectronicSubmission> SecondaryVisits
                        = (from v in _context.Visit
                           join pat in _context.Patient
                            on v.PatientID equals pat.ID
                           join prac in _context.Practice
                            on pat.PracticeID equals prac.ID
                           join loc in _context.Location
                            on v.LocationID equals loc.ID
                           join prov in _context.Provider
                            on v.ProviderID equals prov.ID
                           join pPlan in _context.PatientPlan
                            on pat.ID equals pPlan.PatientID
                           join insPlan in _context.InsurancePlan
                            on pPlan.InsurancePlanID equals insPlan.ID
                           join x12_837 in _context.Edi837Payer
                            on insPlan.Edi837PayerID equals x12_837.ID
                           join rec in _context.Receiver
                            on x12_837.ReceiverID equals rec.ID
                           join sub in _context.Submitter
                            on rec.ID equals sub.ReceiverID
                           //join up in _context.UserPractices on prac.ID equals up.PracticeID
                           //join u in _context.Users on up.UserID equals u.Id

                           where v.SecondaryPatientPlanID == pPlan.ID && v.SecondaryBilledAmount.Val() > 0 && v.PrimaryBal.Val() == 0 &&
                            v.SecondaryBal.Val() > 0 && (v.SecondaryStatus.IsNull2() || v.SecondaryStatus == "N" || v.SecondaryStatus == "RS") &&
                            (insPlan.SubmissionType.Equals("E")) && (v.IsSubmitted == true) && // Issubmitted set as True fo Secondary
                            prac.ID == UD.PracticeID &&
                            sub.ClientID == UD.ClientID &&
                            v.IsForcePaper == false &&
                            v.IsDontPrint == false &&
                            (sub.ReceiverID.Equals(CModel.ReceiverID)) && //IsChargesAvailable(v.ID) &&
                            (string.IsNullOrEmpty(CModel.ReceiverID.ToString()) ? true : rec.ID.Equals(CModel.ReceiverID)) &&
                            (string.IsNullOrEmpty(CModel.AccountNum) ? true : pat.AccountNum.Equals(CModel.AccountNum)) &&
                            (string.IsNullOrEmpty(CModel.Practice) ? true : prac.Name.Contains(CModel.Practice)) &&
                            (string.IsNullOrEmpty(CModel.Provider) ? true : prov.Name.Contains(CModel.Provider)) &&
                            (CModel.Location.IsNull2() ? true : loc.Name.Contains(CModel.Location)) &&
                            (string.IsNullOrEmpty(CModel.PayerID) ? true : x12_837.PayerID.Equals(CModel.PayerID)) &&
                            (string.IsNullOrEmpty(CModel.PlanName) ? true : insPlan.PlanName.Contains(CModel.PlanName)) &&
                            (CModel.VisitID.IsNull() ? true : v.ID.Equals(CModel.VisitID))
                            && (ExtensionMethods.IsBetweenDOS(CModel.EntryDateTo, CModel.EntryDateFrom, v.AddedDate, v.AddedDate))&&
                            (CModel.InsuranceType == "P" ? v.PrimaryPatientPlanID.Value > 0 && v.SecondaryPatientPlanID == null && v.TertiaryPatientPlanID == null :
                            CModel.InsuranceType == "S" ? v.PrimaryPatientPlanID.Value > 0 && v.SecondaryPatientPlanID > 0 && v.TertiaryPatientPlanID == null :
                            CModel.InsuranceType == "T" ? v.PrimaryPatientPlanID.Value > 0 && v.SecondaryPatientPlanID > 0 && v.TertiaryPatientPlanID > 0 :
                            v.PrimaryPatientPlanID.Value > 0)
                           select new GElectronicSubmission()
                           {
                               AccountNum = pat.AccountNum,
                               DOS = v.DateOfServiceFrom.Format("MM/dd/yyyy"),
                               PracticeID = prac.ID,
                               Practice = prac.Name,
                               PatientID = pat.ID,
                               PlanName = insPlan.PlanName,
                               Coverage = pPlan.Coverage,
                               InsurancePlanID = insPlan.ID.ToString(),
                               Patient = pat.LastName + ", " + pat.FirstName,
                               ProviderID = prov.ID,
                               Provider = prov.Name,
                               VisitID = v.ID,
                               ID = 0,
                               LocationID = loc.ID,
                               Location = loc.Name,
                               TotalAmount = v.TotalAmount,
                               ValidationMessage = v.ValidationMessage,
                               IsSubmitted = v.IsSubmitted,
                               subscriberID = pPlan.SubscriberId,
                               VisitEntryDate = v.AddedDate.ToString("MM/dd/yyyy")
                           }).ToList<GElectronicSubmission>();

            List<GElectronicSubmission> newList = new List<GElectronicSubmission>();

            if (PrimaryVisits != null) newList.AddRange(PrimaryVisits);
            if (SecondaryVisits != null) newList.AddRange(SecondaryVisits);
            return newList;

        }

        //private bool IsChargesAvailable(long VisitID)
        //{
        //    return _context.Charge.Where(c => c.VisitID == VisitID && c.IsSubmitted == false && c.IsDontPrint == false).Count() > 0 ? true: false;
        //}


        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CElectronicSubmission CModel)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GElectronicSubmission> data = FindVisits(CModel, UD);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD,CModel,"Model Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CElectronicSubmission CModel)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GElectronicSubmission> data = FindVisits(CModel, UD);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);
        }

        [Route("SubmitVisits")]
        [HttpPost]
        public async Task<ActionResult<SubmitModel>> SubmitVisits(SubmitModel submitModel)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
              User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
              );
            string Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            //var user = (from usr in _context.Users
            //            where usr.Email == Email
            //            select new
            //            {
            //                usr.PracticeID,
            //                usr.ClientID
            //            }).FirstOrDefault();
            try
            {
                if (submitModel.ReceiverID == 0)
                {
                    submitModel.ErrorMessage = "Receiver cannot be empty";
                    return submitModel;
                }

                if (submitModel.Visits == null || submitModel.Visits.Count == 0)
                {
                    submitModel.ErrorMessage = "No Visits Found";
                    return submitModel;
                }



                //Settings settings = await _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefaultAsync();

                //Optimized Code

                var settings = (from set in _context.Settings where set.ClientID == UD.ClientID select new {  set.DocumentServerURL ,set.DocumentServerDirectory}).FirstOrDefault();
              
                
                if (settings == null)
                {

                    submitModel.ErrorMessage = "Document Server Settings Not Found";
                    return submitModel;

                }
                Submitter submitter = _context.Submitter.Where(m => m.ReceiverID == submitModel.ReceiverID && m.ClientID == UD.ClientID).SingleOrDefault();
                if (submitter == null)
                {
                    submitModel.ErrorMessage = "Submitter is not Setup";
                    return submitModel;
                }

                submitModel.ErrorVisits = new List<DropDown>();

                #region ClaimHeader
                ClaimHeader headerData = await
                    (from rec in _context.Receiver
                     join sub in _context.Submitter
                     on rec.ID equals sub.ReceiverID
                     join cl in _context.Client
                     on sub.ClientID equals cl.ID
                     where
                     (cl.ID.Equals(UD.ClientID)) &&
                     (rec.ID.Equals(submitModel.ReceiverID))
                     select new ClaimHeader()
                     {
                         Claims = null,
                         GS02SenderID = sub.X12_837_GS_02,
                         GS03ReceiverID = rec.X12_837_GS_03,
                         ISA01AuthQual = rec.X12_837_ISA_01,
                         ISA02AuthInfo = sub.X12_837_ISA_02,
                         ISA03SecQual = rec.X12_837_ISA_03,
                         ISA04SecInfo = sub.X12_837_ISA_04,
                         ISA05SenderQual = rec.X12_837_ISA_05,
                         ISA06SenderID = sub.X12_837_ISA_06,
                         ISA07ReceiverQual = rec.X12_837_ISA_07,
                         ISA08ReceiverID = rec.X12_837_ISA_08,
                         ISA13CntrlNumber = "111111111",
                         ISA15UsageIndi = "T",
                         ReceiverID = rec.X12_837_NM1_40_ReceiverID,
                         ReceiverOrgName = rec.X12_837_NM1_40_ReceiverName,
                         RecieverQual = "46",
                         RelaxNpiValidation = false,
                         SFTPModel = new SFTPModel()
                         {
                             FTPHost = rec.SubmissionURL,
                             FTPPort = rec.SubmissionPort,
                             FTPUserName = sub.SubmissionUserName,
                             FTPPassword = sub.SubmissionPassword,
                             SubmitToFTP = !sub.ManualSubmission,
                             ConnectivityType = rec.SubmissionMethod,
                             SubmitDirectory = rec.SubmissionDirectory,
                             FileName = sub.FileName
                         },
                         SubmitterContactName = sub.SubmitterContactNumber,
                         SubmitterEmail = sub.SubmitterEmail,
                         SubmitterEntity = "2",
                         SubmitterFax = sub.SubmitterFaxNumber,
                         SubmitterFirstName = "",
                         SubmitterID = sub.X12_837_NM1_41_SubmitterID,
                         SubmitterOrgName = sub.X12_837_NM1_41_SubmitterName,
                         SubmitterQual = "46",
                         SubmitterTelephone = sub.SubmitterContactNumber,
                     }).SingleOrDefaultAsync();
                #endregion


                if (headerData == null || headerData.ISA06SenderID.IsNull2() || headerData.ISA08ReceiverID.IsNull2() ||
                    headerData.GS02SenderID.IsNull2() || headerData.GS03ReceiverID.IsNull2() ||
                    headerData.ISA15UsageIndi.IsNull2() | headerData.SubmitterOrgName.IsNull2() ||
                    headerData.SubmitterID.IsNull2() || headerData.SubmitterContactName.IsNull2() ||
                    headerData.SubmitterTelephone.IsNull2())
                {
                    submitModel.ErrorMessage = "Electronic Setup is not Complete";
                    return submitModel;
                }

                // Get Sequence# for ISA
                headerData.ISA13CntrlNumber = _context.GetNextSequenceValue("S_GSControlNumber");
                //

                Practice prac = _context.Practice.Find(UD.PracticeID);
                long totalInsBillOptions = _context.InsuranceBillingoption.Count();

                #region Claims
                headerData.Claims = await
                    (
                    from v in _context.Visit
                    join pat in _context.Patient
                    on v.PatientID equals pat.ID
                    join pract in _context.Practice
                    on pat.PracticeID equals pract.ID
                    join loc in _context.Location
                    on pat.LocationId equals loc.ID
                    join prov in _context.Provider
                    on v.ProviderID equals prov.ID
                    join pPlan in _context.PatientPlan
                    on v.PrimaryPatientPlanID equals pPlan.ID
                    join insPlan in _context.InsurancePlan
                    on pPlan.InsurancePlanID equals insPlan.ID
                    join planType in _context.PlanType
                    on insPlan.PlanTypeID equals planType.ID
                    join ediPayer in _context.Edi837Payer
                    on insPlan.Edi837PayerID equals ediPayer.ID
                    join rec in _context.Receiver
                    on ediPayer.ReceiverID equals rec.ID
                    join icd1 in _context.ICD
                    on v.ICD1ID equals icd1.ID
                    join i2 in _context.ICD
                    on v.ICD2ID equals i2.ID into i21
                    from icd2 in i21.DefaultIfEmpty()
                    join i3 in _context.ICD
                    on v.ICD3ID equals i3.ID into i31
                    from icd3 in i31.DefaultIfEmpty()
                    join i4 in _context.ICD
                    on v.ICD4ID equals i4.ID into i41
                    from icd4 in i41.DefaultIfEmpty()
                    join i5 in _context.ICD
                    on v.ICD5ID equals i5.ID into i51
                    from icd5 in i51.DefaultIfEmpty()
                    join i6 in _context.ICD
                    on v.ICD6ID equals i6.ID into i61
                    from icd6 in i61.DefaultIfEmpty()
                 
                    //Optimizing Query Performance

                    //join i7 in _context.ICD
                    //on v.ICD7ID equals i7.ID into i71
                    //from icd7 in i71.DefaultIfEmpty()
                    //join i8 in _context.ICD
                    //on v.ICD8ID equals i8.ID into i81
                    //from icd8 in i81.DefaultIfEmpty()
                    //join i9 in _context.ICD
                    //on v.ICD9ID equals i9.ID into i91
                    //from icd9 in i91.DefaultIfEmpty()
                    //join i10 in _context.ICD
                    //on v.ICD10ID equals i10.ID into i10_1
                    //from icd10 in i10_1.DefaultIfEmpty()
                    //join i11 in _context.ICD
                    //on v.ICD11ID equals i11.ID into i11_1
                    //from icd11 in i11_1.DefaultIfEmpty()
                    //join i12 in _context.ICD
                    //on v.ICD12ID equals i12.ID into i12_1
                    //from icd12 in i12_1.DefaultIfEmpty()
                    join pos in _context.POS
                    on v.POSID equals pos.ID
                    //join ipa in _context.InsurancePlanAddress
                    //on pPlan.InsurancePlanAddressID equals ipa.ID into ipa2
                    //from insPlanAdd in ipa2.DefaultIfEmpty()
                    join rp in _context.RefProvider
                    on v.RefProviderID equals rp.ID into rp1
                    from refProv in rp1.DefaultIfEmpty()
                    //join sp in _context.Provider
                    //on v.SupervisingProvID equals sp.ID into sp1
                    //from supProv in sp1.DefaultIfEmpty()
                    where submitModel.Visits.Contains(v.ID, new VisitComparer()) && 
                    (v.IsSubmitted == false || v.SecondaryStatus == "N" || v.PrimaryStatus == "RS" || v.SecondaryStatus == "RS") &&
                    v.IsForcePaper == false &&
                    rec.ID == submitModel.ReceiverID //&& IsChargesAvailable(v.ID)
                    select new ClaimData()
                    {
                        AccidentDate = v.AccidentDate,
                        AccidentState = v.AccidentState,
                        AccidentType = v.AccidentType,
                        AcuteManifestationDate = null,
                        AdmissionDate = v.AdmissionDate,
                        BenefitsAssignment = "",
                        BillPrvEntityType = prov.BillUnderProvider == true ? "1" : "2",
                        BillPrvOrgName = prov.BillUnderProvider == true ? prov.LastName : pract.OrganizationName,
                        BillPrvFirstName = prov.BillUnderProvider == true ? prov.FirstName : "",
                        BillPrvMI = prov.BillUnderProvider == true ? prov.MiddleInitial : "",
                        BillPrvNPI = prov.BillUnderProvider == true ? prov.NPI : pract.NPI,
                        BillPrvTaxID = prov.BillUnderProvider == true ? (prov.ReportTaxID == true ? pract.TaxID : "") : pract.TaxID,
                        BillPrvSSN = prov.BillUnderProvider == true ? (prov.ReportTaxID == false ? prov.SSN : "") : prov.SSN,
                        BillPrvTaxonomyCode = pract.TaxonomyCode,
                        BillPrvAddress1 = prov.BillUnderProvider == true ? prov.Address1 : pract.Address1,
                        BillPrvCity = prov.BillUnderProvider == true ? prov.City : pract.City,
                        BillPrvState = prov.BillUnderProvider == true ? prov.State : pract.State,
                        BillPrvZipCode = prov.BillUnderProvider == true ? prov.ZipCode : pract.ZipCode,
                        BillPrvTelephone = prov.BillUnderProvider == true ? prov.OfficePhoneNum : pract.OfficePhoneNum,
                        BillPrvContactName = "",
                        BillPrvEmail = prov.BillUnderProvider == true ? prov.Email : pract.Email,
                        BillPrvFax = prov.BillUnderProvider == true ? prov.FaxNumber : pract.FaxNumber,
                        BillPrvPayToAddr = prov.BillUnderProvider == true ? prov.PayToAddress1 : pract.PayToAddress1,
                        BillPrvPayToCity = prov.BillUnderProvider == true ? prov.PayToCity : pract.PayToCity,
                        BillPrvPayToState = prov.BillUnderProvider == true ? prov.PayToState : pract.PayToState,
                        BillPrvPayToZip = prov.BillUnderProvider == true ? prov.PayToZipCode : pract.ZipCode,
                        BillPrvSecondaryID = "",
                        Charges = null,
                        ClaimAmount = v.TotalAmount.Amt(),
                        ClaimFreqCode = v.ClaimFrequencyCode.IsNull2() ? "1" : v.ClaimFrequencyCode,
                        ClaimNotes = v.ClaimNotes,
                        ClaimType = pPlan.Coverage,
                        CliaNumber = !loc.CLIANumber.IsNull2() ? loc.CLIANumber : v.CliaNumber,
                        CurrentIllnessDate = null,
                        DelayReasonValue = "",
                        DisabilityStartDate = v.UnableToWorkFromDate,
                        DisabilityEndDate = v.UnableToWorkToDate,
                        DischargeDate = v.DischargeDate,
                        XrayDate = v.LastXrayDate,
                        ICD1Code = icd1.ICDCode,
                        ICD2Code = icd2.ICDCode,
                        ICD3Code = icd3.ICDCode,
                        ICD4Code = icd4.ICDCode,
                        ICD5Code = icd5.ICDCode,
                        ICD6Code = icd6.ICDCode,
                        //ICD7Code = icd7.ICDCode,
                        //ICD8Code = icd8.ICDCode,
                        //ICD9Code = icd9.ICDCode,
                        //ICD10Code = icd10.ICDCode,
                        //ICD11Code = icd11.ICDCode,
                        //ICD12Code = icd12.ICDCode,
                        ICD7CodeID = v.ICD7ID,
                        ICD8CodeID = v.ICD8ID,
                        ICD9CodeID = v.ICD9ID,
                        ICD10CodeID = v.ICD10ID,
                        ICD11CodeID = v.ICD11ID,
                        ICD12CodeID = v.ICD12ID,
                        InitialTreatmentDate = null,
                        IsMedicareMedigap = false,
                        IsSelfSubscribed = pPlan.RelationShip == "18" ? true : false,
                        LastSeenDate = v.LastSeenDate,
                        LastWorkedDate = null,
                        LMPDate = v.DateOfPregnancy,
                        LocationAddress = loc.Address1,
                        LocationCity = loc.City,
                        LocationNPI = prov.BillUnderProvider == true ? prov.NPI : loc.NPI,
                        LocationOrgName = loc.OrganizationName,
                        LocationState = loc.State,
                        LocationZip = loc.ZipCode,
                        MammographyNumber = "",
                        MedicarRecordNum = "",
                        PATAddress = pat.Address1,
                        PATCity = pat.City,
                        PATState = pat.State,
                        PATZipCode = pat.ZipCode,
                        PATDob = pat.DOB,
                        PATEntityType = "1",
                        PATFirstName = pat.FirstName,
                        PATLastName = pat.LastName,
                        PATGender = pat.Gender,
                        PatientRelationShip = pPlan.RelationShip,
                        PATMiddleInitial = pat.MiddleInitial,
                        PayerType = planType.Code,
                        PayerID = ediPayer.PayerID,
                        PayerName = ediPayer.PayerName,
                        //PayerAddress = insPlanAdd.Address1,
                        //PayerCity = insPlanAdd.City,
                        //PayerState = insPlanAdd.State,
                        //PayerZipCode = insPlanAdd.ZipCode,
                        InsurancePlanAddressID = pPlan.InsurancePlanAddressID,
                        //PatientControlNumber = UD.PracticeID == 8 ? v.ID + " " + pat.AccountNum : v.ID + "_" + pat.AccountNum,
                        PatientControlNumber = v.ID + " " + pat.AccountNum,
                        RendPrvFirstName = prov.FirstName,
                        RendPrvLastName = prov.LastName,
                        RendPrvMI = prov.MiddleInitial,
                        RendPrvNPI = prov.NPI,
                        RendPrvSecondaryID = "",
                        RendPrvTaxonomy = prov.TaxonomyCode,
                        RefPrvFirstName = refProv.FirstName,
                        RefPrvLastName = refProv.LastName,
                        RefPrvMI = refProv.MiddleInitial,
                        RefPrvNPI = refProv.NPI,
                        POSCode = pos.PosCode,
                        SBREntityType = "1",
                        SBRLastName = pPlan.LastName,
                        SBRFirstName = pPlan.FirstName,
                        SBRMiddleInitial = pPlan.MiddleInitial,
                        SBRID = pPlan.SubscriberId,
                        SbrGroupName = pPlan.GroupName,
                        SbrGroupNumber = pPlan.GroupNumber,
                        SBRGender = pPlan.Gender,
                        SBRDob = pPlan.DOB,
                        SBRSSN = pPlan.SSN,
                        SBRAddress = pPlan.Address1,
                        SBRCity = pPlan.City,
                        SBRState = pPlan.State,
                        SBRZipCode = pPlan.ZipCode,
                        SbrMedicareSecTypeV = "",
                        //SuperPrvFirstName = supProv.FirstName,
                        //SuperPrvLastName = supProv.LastName,
                        //SuperPrvMI = supProv.MiddleInitial,
                        //SuperPrvNPI = supProv.NPI,
                        SupervisingProvID = v.SupervisingProvID,
                        VisitID = v.ID,
                        SecondaryStatus = v.SecondaryStatus,
                        SecondaryPatientPlanID = v.SecondaryPatientPlanID,
                        PatientID = v.PatientID,
                        InsruancePlanID = insPlan.ID,
                        ProviderID = prov.ID,
                        LocationID = loc.ID,
                        PracticeID = pract.ID,
                        BillUnderProvider = prov.BillUnderProvider,
                        PayerClaimCntrlNum = v.PayerClaimControlNum,
                        PriorAuthNumber = v.AuthorizationNum,
                        PrescribingMD = v.PrescribingMD,
                    }).ToListAsync<ClaimData>();

              
                #endregion


                if (headerData.Claims == null || headerData.Claims.Count == 0)
                {
                    submitModel.ErrorMessage = "No Visit(s) Qualified for Submission";
                    return submitModel;
                }


                foreach (ClaimData CLM in headerData.Claims)
                {
                    // Updating ValidationMessage as Empty
                    Visit visit = await _context.Visit.FindAsync(CLM.VisitID);
                    if (visit != null)
                    {
                        visit.ValidationMessage = string.Empty;
                        _context.Visit.Update(visit);
                        await _context.SaveChangesAsync();
                    }


                    #region GettingICDCodes

                    if (!CLM.ICD7CodeID.IsNull())
                    {
                        CLM.ICD7Code = _context.ICD.Find(CLM.ICD7CodeID)?.ICDCode;
                    }

                    if (!CLM.ICD8CodeID.IsNull())
                    {
                        CLM.ICD8Code = _context.ICD.Find(CLM.ICD8CodeID)?.ICDCode;
                    }
                    if (!CLM.ICD9CodeID.IsNull())
                    {
                        CLM.ICD9Code = _context.ICD.Find(CLM.ICD9CodeID)?.ICDCode;
                    }
                    if (!CLM.ICD10CodeID.IsNull())
                    {
                        CLM.ICD10Code = _context.ICD.Find(CLM.ICD10CodeID)?.ICDCode;
                    }
                    if (!CLM.ICD11CodeID.IsNull())
                    {
                        CLM.ICD11Code = _context.ICD.Find(CLM.ICD11CodeID)?.ICDCode;
                    }
                    if (!CLM.ICD12CodeID.IsNull())
                    {
                        CLM.ICD12Code = _context.ICD.Find(CLM.ICD12CodeID)?.ICDCode;
                    }
                    if (!CLM.SupervisingProvID.IsNull())
                    {
                        var Provider = await _context.Provider.FindAsync(CLM.SupervisingProvID);
                        CLM.SuperPrvFirstName = Provider.FirstName;
                        CLM.SuperPrvLastName = Provider.LastName;
                        CLM.SuperPrvMI = Provider.MiddleInitial;
                        CLM.SuperPrvNPI = Provider.NPI;


                    }
                    if (!CLM.InsurancePlanAddressID.IsNull())
                    {
                        var ipAddress = await _context.InsurancePlanAddress.FindAsync(CLM.InsurancePlanAddressID);
                        CLM.PayerAddress = ipAddress.Address1;
                        CLM.PayerCity = ipAddress.City;
                        CLM.PayerState = ipAddress.State;
                        CLM.PayerZipCode = ipAddress.ZipCode;
                    }
                    #endregion

                    #region InsuranceBillingOptions

                    if (CLM.BillUnderProvider != true && totalInsBillOptions > 0)
                    {
                        InsuranceBillingoption insuranceBillingoption = _context.InsuranceBillingoption.Where(m => m.InsurancePlanID == CLM.InsruancePlanID && m.LocationID == CLM.LocationID
                        && m.ProviderID == CLM.ProviderID).SingleOrDefault();

                        Provider provProf = _context.Provider.Find(CLM.ProviderID);
                        

                        if (insuranceBillingoption != null)
                        {
                            CLM.BillPrvEntityType = "1";
                            CLM.BillPrvOrgName = provProf.LastName;
                            CLM.BillPrvFirstName = provProf.FirstName;
                            CLM.BillPrvMI = provProf.MiddleInitial;
                            CLM.BillPrvNPI = provProf.NPI;
                            CLM.BillPrvTaxID = insuranceBillingoption.ReportTaxID == true ? CLM.BillPrvTaxID : "";
                            CLM.BillPrvSSN = insuranceBillingoption.ReportTaxID == false ? provProf.SSN : "";
                            CLM.BillPrvTaxonomyCode = provProf.TaxonomyCode;

                            CLM.BillPrvAddress1 = provProf.Address1;
                            CLM.BillPrvCity = provProf.City;
                            CLM.BillPrvState = provProf.State;
                            CLM.BillPrvZipCode = provProf.ZipCode;
                            CLM.BillPrvTelephone = "";
                            CLM.BillPrvContactName = "";

                            CLM.LocationNPI = provProf.NPI;

                            if (insuranceBillingoption.PayToAddress == "None")
                            {
                                CLM.BillPrvPayToAddr = "";
                                CLM.BillPrvPayToCity = "";
                                CLM.BillPrvPayToState = "";
                                CLM.BillPrvPayToZip = "";
                            }
                            else if (insuranceBillingoption.PayToAddress == "Provider")
                            {
                                CLM.BillPrvPayToAddr = provProf.PayToAddress1;
                                CLM.BillPrvPayToCity = provProf.PayToCity;
                                CLM.BillPrvPayToState = provProf.PayToState;
                                CLM.BillPrvPayToZip = provProf.PayToZipCode;
                            }
                            else if (insuranceBillingoption.PayToAddress == "Practice")
                            {
                                CLM.BillPrvPayToAddr = prac.PayToAddress1;
                                CLM.BillPrvPayToCity = prac.PayToCity;
                                CLM.BillPrvPayToState = prac.PayToState;
                                CLM.BillPrvPayToZip = prac.PayToZipCode;
                            }
                        }
                    }
                    #endregion

                    if (CLM.SecondaryStatus == "N" || CLM.SecondaryStatus == "RS")
                    {
                        if (visit.SecondaryBilledAmount.Val() > 0)
                        {
                            string pID = CLM.PayerID;
                            string pType = CLM.PayerType;
                            string pName = CLM.PayerName;
                            string pAddress = CLM.PayerAddress;
                            string pCity = CLM.PayerCity;
                            string pState = CLM.PayerState;
                            string pZipCode = CLM.PayerZipCode;
                            string sbrLName = CLM.SBRLastName;
                            string sbrFName = CLM.SBRFirstName;
                            string sbrMInitial = CLM.SBRMiddleInitial;
                            string sbrID = CLM.SBRID;
                            string sbrGName = CLM.SbrGroupName;
                            string sbrGnumber = CLM.SbrGroupNumber;
                            string sbrGender = CLM.SBRGender;
                            DateTime? sbrDOB = CLM.SBRDob;
                            string sbrSSN = CLM.SBRSSN;
                            string sbrAddress = CLM.SBRAddress;
                            string sbrCity = CLM.SBRCity;
                            string sbrRelation = CLM.PatientRelationShip;

                            #region SecondaryClaims
                            var secondaryData =
                            (from v in _context.Visit
                             join pat in _context.Patient
                         on v.PatientID equals pat.ID
                             join fac in _context.Practice
                         on pat.PracticeID equals fac.ID
                             join loc in _context.Location
                         on pat.LocationId equals loc.ID
                             join prov in _context.Provider
                         on pat.ProviderID equals prov.ID
                             join pPlan in _context.PatientPlan
                         on v.SecondaryPatientPlanID equals pPlan.ID
                             join insPlan in _context.InsurancePlan
                         on pPlan.InsurancePlanID equals insPlan.ID
                             join ipa in _context.InsurancePlanAddress
                             on pPlan.InsurancePlanAddressID equals ipa.ID into ipa2
                             from insPlanAdd in ipa2.DefaultIfEmpty()
                             join planType in _context.PlanType
                            on insPlan.PlanTypeID equals planType.ID
                             join ediPayer in _context.Edi837Payer
                         on insPlan.Edi837PayerID equals ediPayer.ID
                             join rec in _context.Receiver
                         on ediPayer.ReceiverID equals rec.ID
                             where v.ID == CLM.VisitID && v.SecondaryPatientPlanID == CLM.SecondaryPatientPlanID && v.PatientID == CLM.PatientID
                             select new
                             {
                                 PayerType = planType.Code,
                                 PayerID = ediPayer.PayerID,
                                 PayerName = ediPayer.PayerName,
                                 PayerAddress = insPlanAdd.Address1,
                                 PayerCity = insPlanAdd.City,
                                 PayerState = insPlanAdd.State,
                                 PayerZipCode = insPlanAdd.ZipCode,
                                 SBRLastName = pPlan.LastName,
                                 SBRFirstName = pPlan.FirstName,
                                 SBRMiddleInitial = pPlan.MiddleInitial,
                                 SBRID = pPlan.SubscriberId,
                                 SbrGroupName = pPlan.GroupName,
                                 SbrGroupNumber = "",
                                 SBRGender = pPlan.Gender,
                                 SBRDob = pPlan.DOB,
                                 SBRSSN = pPlan.SSN,
                                 SBRAddress = pPlan.Address1,
                                 SBRCity = pPlan.City,
                                 SBRState = pPlan.State,
                                 SBRZipCode = pPlan.ZipCode,
                                 ClaimType = pPlan.Coverage,
                                 PatientRelationShip = pPlan.RelationShip

                             }).SingleOrDefault();
                            CLM.PayerType = secondaryData.PayerType;
                            CLM.PayerID = secondaryData.PayerID;
                            CLM.PayerName = secondaryData.PayerName;
                            CLM.PayerAddress = secondaryData.PayerAddress;
                            CLM.PayerCity = secondaryData.PayerCity;
                            CLM.PayerState = secondaryData.PayerState;
                            CLM.PayerZipCode = secondaryData.PayerZipCode;
                            CLM.SBRLastName = secondaryData.SBRLastName;
                            CLM.SBRFirstName = secondaryData.SBRFirstName;
                            CLM.SBRMiddleInitial = secondaryData.SBRMiddleInitial;
                            CLM.SBRID = secondaryData.SBRID;
                            CLM.SbrGroupName = secondaryData.SbrGroupName;
                            CLM.SbrGroupNumber = "";
                            CLM.SBRGender = secondaryData.SBRGender;
                            CLM.SBRDob = secondaryData.SBRDob;
                            CLM.SBRSSN = secondaryData.SBRSSN;
                            CLM.SBRAddress = secondaryData.SBRAddress;
                            CLM.SBRCity = secondaryData.SBRCity;
                            CLM.SBRState = secondaryData.SBRState;
                            CLM.SBRZipCode = secondaryData.SBRZipCode;
                            CLM.ClaimType = secondaryData.ClaimType;
                            CLM.PatientRelationShip = secondaryData.PatientRelationShip;
                            CLM.OtherPayerName = pName;
                            CLM.OtherPayerID = pID;
                            CLM.OtherSBRLastName = sbrLName;
                            CLM.OtherSBRFirstName = sbrFName;
                            CLM.OtherSBRId = sbrID;
                            CLM.OtherPayerTypeValue = pType;
                            CLM.OtherSBRPatRelationV = sbrRelation;

                            #endregion
                        }
                        else
                        {
                            CLM.SecondaryStatus = null;
                            CLM.ClaimType = "P";
                        }
                    }


                    #region Charges
                    CLM.Charges = await (
                from c in _context.Charge
                join v in _context.Visit
                on c.VisitID equals v.ID
                join cpt in _context.Cpt
                on c.CPTID equals cpt.ID
                join m1 in _context.Modifier
                on c.Modifier1ID equals m1.ID into m11
                from mod1 in m11.DefaultIfEmpty()
                join m2 in _context.Modifier
                on c.Modifier2ID equals m2.ID into m22
                from mod2 in m22.DefaultIfEmpty()
                join m3 in _context.Modifier
                on c.Modifier3ID equals m3.ID into m33
                from mod3 in m33.DefaultIfEmpty()
                join m4 in _context.Modifier
                on c.Modifier4ID equals m4.ID into m44
                from mod4 in m44.DefaultIfEmpty()
                where c.VisitID == CLM.VisitID && (c.IsSubmitted == false || c.SecondaryStatus == "N" || c.SecondaryStatus == "RS") 
                &&  (c.IsDontPrint == null || c.IsDontPrint == false)
                select new ChargeData()
                {
                    ChargeAmount = c.TotalAmount,
                    ChargeID = c.ID,
                    CliaNumber = "",
                    CptCode = cpt.CPTCode,
                    DateofServiceFrom = c.DateOfServiceFrom,
                    DateOfServiceTo = c.DateOfServiceTo,
                    DrugCount = "",
                    DrugNumber = "",
                    DrugUnit = "",
                    ICD1 = "",
                    ICD2 = "",
                    ICD3 = "",
                    ICD4 = "",
                    IsEmergency = v.Emergency != null? v.Emergency.Value : false,
                    LIDescription = "",
                    LineItemControlNum = c.ID.ToString(),
                    Minutes = c.Minutes,
                    Units = c.Units.IsNull2() ? "1" : c.Units,
                    Modifier1 = mod1.Code,
                    Modifier2 = mod2.Code,
                    Modifier3 = mod3.Code,
                    Modifier4 = mod4.Code,
                    Pointer1 = c.Pointer1,
                    Pointer2 = c.Pointer2,
                    Pointer3 = c.Pointer3,
                    Pointer4 = c.Pointer4,
                    POS = v.POSID != c.POSID ? c.POS.PosCode : "",

                }).ToListAsync();
                    if (CLM.SecondaryStatus == "N" || CLM.SecondaryStatus == "RS")
                    {
                        foreach (ChargeData CH in CLM.Charges)
                        {
                            var secondaryChargeData = (from payCharge in _context.PaymentCharge
                                                       join payVisit in _context.PaymentVisit
                                                       on payCharge.PaymentVisitID equals payVisit.ID
                                                       join payCheck in _context.PaymentCheck 
                                                       on payVisit.PaymentCheckID equals payCheck.ID
                                                       join vst in _context.Visit
                                                       on payVisit.VisitID equals vst.ID
                                                       where vst.ID == CLM.VisitID && payCharge.ChargeID == CH.ChargeID
                                                       select new
                                                       {
                                                           payCheck.CheckDate,
                                                           payCharge.PaidAmount,
                                                           payCharge.WriteoffAmount,
                                                           payCharge.Copay,
                                                           payCharge.DeductableAmount,
                                                           payCharge.CoinsuranceAmount,
                                                           payCharge.OtherPatResp
                                                       }).SingleOrDefault();
                            CH.PrimaryCPT = CH.CptCode;
                            CH.PrimaryMod1 = CH.Modifier1;
                            CH.PrimaryMod2 = CH.Modifier2;
                            CH.PrimaryMod3 = CH.Modifier3;
                            CH.PrimaryMod4 = CH.Modifier4;
                            CH.PrimaryPaidAmt = secondaryChargeData.PaidAmount;
                            CH.PrimaryWriteOffAmt = secondaryChargeData.WriteoffAmount;
                            CH.PrimaryCoIns = secondaryChargeData.CoinsuranceAmount;
                            CH.PrimaryDeductable = secondaryChargeData.DeductableAmount;
                            CH.PrimaryPaidDate = secondaryChargeData.CheckDate;
                            CH.PrimaryUnits = CH.Units;
                        }
                        CLM.PrimaryPaidAmt = CLM.Charges.Sum(c => c.PrimaryPaidAmt.Val());
                        CLM.ClaimType = "S";
                    }

                    #endregion
                }
              
                string directoryPath = Path.Combine("\\\\", settings.DocumentServerURL,
                        settings.DocumentServerDirectory, UD.ClientID.ToString(), submitModel.ReceiverID.ToString(),
                        DateTime.Now.ToString("MMddyyyy"), DateTime.Now.ToString("hhmmssff"));
                headerData.SFTPModel.RootDirectory = directoryPath;

                string testMode = Configuration["SubmissionSettings:TestMode"]; //Values Comming From appsettings
                if (testMode.IsNull2()) testMode = "Y";
                //string fileSequence = ConfigurationManager.AppSettings["FileSequence"];
                string fileSequence = Configuration["SubmissionSettings:FileSequence"]; //Values Comming From appsettings


                ClaimGenerator claimGenerator = new ClaimGenerator(testMode, fileSequence);
                Output output = claimGenerator.Generate837Transaction(headerData);
                SubmissionLog submissionLog = null;

                var transactionOption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
                using (var objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption, TransactionScopeAsyncFlowOption.Enabled))
                {
                    #region Processed Claims
                    if (output.ProcessedClaims > 0)
                    {
                        // SubmissionLog
                        submissionLog = new SubmissionLog()
                        {
                            AddedBy = Email,
                            AddedDate = DateTime.Now,
                            ClaimCount = output.ProcessedClaims,
                            ClaimAmount = output.ClaimAmount,
                            ClientID = UD.ClientID,
                            ReceiverID = submitModel.ReceiverID,
                            Transaction837Path = output.Transaction837Path,
                            ISAControlNumber = headerData.ISA13CntrlNumber,
                            SubmitType = "E",
                            Resolve = false
                        };

                        await _context.SubmissionLog.AddAsync(submissionLog);
                        //await _context.SaveChangesAsync();

                        if (headerData.SFTPModel.SubmitToFTP)
                        {
                            if (output.FileSubmittedToFTP)
                            {
                                submitModel.IsFileSubmitted = true;

                                #region Updating Claims
                                foreach (ClaimResult claim in output.Claims)
                                {
                                    Visit visit = await _context.Visit.FindAsync(claim.VisitID);
                                    if (visit != null)
                                    {
                                        if (claim.Submitted)
                                        {
                                            if (visit.PrimaryStatus.IsNull2() || visit.PrimaryStatus == "N" || visit.PrimaryStatus == "RS")
                                            {
                                            visit.IsSubmitted = claim.Submitted;
                                            visit.SubmittedDate = claim.SubmittedDate;
                                            visit.SubmissionLogID = submissionLog.ID;
                                            visit.PrimaryStatus = "S";
                                            visit.RejectionReason = string.Empty;
                                            }
                                            else if (visit.SecondaryStatus == "N" || visit.SecondaryStatus == "RS")
                                            {
                                                visit.SubmittedDate = claim.SubmittedDate;
                                                visit.SecondaryStatus = "S";
                                                visit.SubmissionLogID2 = submissionLog.ID;
                                                visit.RejectionReason = string.Empty;
                                            }
                                            else if (visit.TertiaryStatus == "N" || visit.TertiaryStatus == "RS")
                                            {
                                                visit.SubmittedDate = claim.SubmittedDate;
                                                visit.TertiaryStatus = "S";
                                                visit.SubmissionLogID3 = submissionLog.ID;
                                                visit.RejectionReason = string.Empty;
                                            }
                                        }

                                        _context.Visit.Update(visit);
                                        //await _context.SaveChangesAsync();

                                        if (visit.IsSubmitted)
                                        {
                                            foreach (ChargeData C in headerData.Claims.Find(m => m.VisitID == visit.ID).Charges)
                                            {
                                                Charge charge = await _context.Charge.FindAsync(C.ChargeID);
                                                if (charge != null)
                                                {
                                                    long ?PatientPlanID = null;
                                                    decimal? Amount = null;
                                                    if (charge.PrimaryStatus.IsNull2() || charge.PrimaryStatus == "N" || charge.PrimaryStatus == "RS")
                                                    {
                                                        charge.IsSubmitted = C.Submitted;
                                                        charge.SubmittetdDate = C.SubmittedDate;
                                                        charge.SubmissionLogID = submissionLog.ID;
                                                        charge.PrimaryStatus = "S";
                                                        PatientPlanID = charge.PrimaryPatientPlanID;
                                                        Amount = charge.PrimaryBilledAmount;
                                                        charge.RejectionReason = string.Empty;
                                                    }
                                                    else if (charge.SecondaryStatus == "N" || charge.SecondaryStatus == "RS")
                                                    {
                                                        charge.SubmittetdDate = claim.SubmittedDate;
                                                        charge.SecondaryStatus = "S";
                                                        charge.SubmissionLogID2 = submissionLog.ID;
                                                        PatientPlanID = charge.SecondaryPatientPlanID;
                                                        Amount = charge.SecondaryBilledAmount;
                                                        charge.RejectionReason = string.Empty;
                                                    }
                                                    else if (charge.TertiaryStatus == "N" || charge.TertiaryStatus == "RS")
                                                    {
                                                        charge.SubmittetdDate = claim.SubmittedDate;
                                                        charge.TertiaryStatus = "S";
                                                        charge.SubmissionLogID3 = submissionLog.ID;
                                                        PatientPlanID = charge.TertiaryPatientPlanID;
                                                        Amount = charge.TertiaryBilledAmount;
                                                        charge.RejectionReason = string.Empty;
                                                    }
                                                    _context.Charge.Update(charge);
                                                    //await _context.SaveChangesAsync();

                                                    ChargeSubmissionHistory chargeHistory = new ChargeSubmissionHistory()
                                                    {
                                                        AddedBy = Email,
                                                        AddedDate = DateTime.Now,
                                                        ChargeID = charge.ID,
                                                        ReceiverID = submitModel.ReceiverID,
                                                        SubmissionLogID = submissionLog.ID,
                                                        PatientPlanID = PatientPlanID,
                                                        SubmitType = "E",
                                                        Amount = Amount

                                                    };

                                                    await _context.ChargeSubmissionHistory.AddAsync(chargeHistory);
                                                    //await _context.SaveChangesAsync();
                                                }
                                                else
                                                {
                                                    throw new System.Exception("Charge Not Found");
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        throw new System.Exception("Visit Not Found");
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                submitModel.ErrorMessage = "";
                            }
                        }
                        // Manual Submission
                        else
                        {
                            #region Updating Claims
                            foreach (ClaimResult claim in output.Claims)
                            {
                                Visit visit = await _context.Visit.FindAsync(claim.VisitID);
                                if (visit != null)
                                {
                                    if (claim.Submitted)
                                    {
                                        if (visit.PrimaryStatus.IsNull2() || visit.PrimaryStatus == "N" || visit.PrimaryStatus == "RS")
                                        {
                                        visit.IsSubmitted = claim.Submitted;
                                        visit.SubmittedDate = claim.SubmittedDate;
                                        visit.SubmissionLogID = submissionLog.ID;
                                        visit.PrimaryStatus = "S";
                                        visit.RejectionReason = string.Empty;
                                        }
                                        else if (visit.SecondaryStatus == "N" || visit.SecondaryStatus == "RS" )
                                        {
                                            visit.SubmittedDate = claim.SubmittedDate;
                                            visit.SecondaryStatus = "S";
                                            visit.SubmissionLogID2 = submissionLog.ID;
                                            visit.RejectionReason = string.Empty;
                                        }
                                        else if (visit.TertiaryStatus == "N" || visit.TertiaryStatus == "RS" )
                                        {
                                            visit.SubmittedDate = claim.SubmittedDate;
                                            visit.TertiaryStatus = "S";
                                            visit.SubmissionLogID3 = submissionLog.ID;
                                            visit.RejectionReason = string.Empty;
                                        }

                                    _context.Visit.Update(visit);
                                    }
                                    //await _context.SaveChangesAsync();

                                    if (visit.IsSubmitted)
                                    {
                                        foreach (ChargeData C in headerData.Claims.Find(m => m.VisitID == visit.ID).Charges)
                                        {
                                            Charge charge = await _context.Charge.FindAsync(C.ChargeID);
                                            if (charge != null)
                                            {
                                                long? PatientPlanID = null;
                                                decimal? Amount = null;
                                                if (charge.PrimaryStatus.IsNull2() || charge.PrimaryStatus == "N" || charge.PrimaryStatus == "RS")
                                                {
                                                charge.IsSubmitted = C.Submitted;
                                                charge.SubmittetdDate = C.SubmittedDate;
                                                charge.SubmissionLogID = submissionLog.ID;
                                                    charge.PrimaryStatus = "S";
                                                    PatientPlanID = charge.PrimaryPatientPlanID;
                                                    Amount = charge.PrimaryBilledAmount;
                                                    charge.RejectionReason = string.Empty;
                                                }
                                                else if (charge.SecondaryStatus == "N" || charge.SecondaryStatus == "RS")
                                                {
                                                    charge.SubmittetdDate = claim.SubmittedDate;
                                                    charge.SecondaryStatus = "S";
                                                    charge.SubmissionLogID2 = submissionLog.ID;
                                                    PatientPlanID = charge.SecondaryPatientPlanID;
                                                    Amount = charge.SecondaryBilledAmount;
                                                    charge.RejectionReason = string.Empty;
                                                }
                                                else if (charge.TertiaryStatus == "N" || charge.TertiaryStatus == "RS")
                                                {
                                                    charge.SubmittetdDate = claim.SubmittedDate;
                                                    charge.TertiaryStatus = "S";
                                                    charge.SubmissionLogID3 = submissionLog.ID;
                                                    PatientPlanID = charge.TertiaryPatientPlanID;
                                                    Amount = charge.TertiaryBilledAmount;
                                                    charge.RejectionReason = string.Empty;
                                                }
                                                _context.Charge.Update(charge);
                                                //await _context.SaveChangesAsync();

                                                ChargeSubmissionHistory chargeHistory = new ChargeSubmissionHistory()
                                                {
                                                    AddedBy = Email,
                                                    AddedDate = DateTime.Now,
                                                    ChargeID = charge.ID,
                                                    ReceiverID = submitModel.ReceiverID,
                                                    SubmissionLogID = submissionLog.ID,
                                                    PatientPlanID = PatientPlanID,
                                                    SubmitType = "E",
                                                    Amount = Amount
                                                };

                                                await _context.ChargeSubmissionHistory.AddAsync(chargeHistory);
                                                //await _context.SaveChangesAsync();
                                            }
                                            else
                                            {
                                                return BadRequest("Charge Not Found");
                                            }
                                        }
                                    }
                                }

                                else
                                {
                                    return BadRequest("Visit Not Found");
                                }
                            }
                            #endregion

                            submitModel.FilePath = output.Transaction837Path;
                        }

                        // objTrnScope.Complete();
                        //  objTrnScope.Dispose();

                        submitModel.ProcessedClaims = output.ProcessedClaims;

                    }
                    #endregion

                    #region Updating Claims

                    if (output.Claims != null && output.Claims.Count > 0)
                    {
                        foreach (ClaimResult claim in output.Claims)
                        {
                            if (!claim.ValidationMsg.IsNull2())
                            {
                                Visit visit = await _context.Visit.FindAsync(claim.VisitID);
                                if (visit != null)
                                {
                                    visit.ValidationMessage = claim.ValidationMsg;
                                    submitModel.ErrorVisits.Add(new DropDown() { ID = visit.ID, Description = claim.ValidationMsg });
                                    _context.Visit.Update(visit);
                                    //await _context.SaveChangesAsync();
                                }
                                else
                                {
                                    return BadRequest("Visit Not Found");
                                }
                            }

                        }
                    }

                    #endregion


                    if (submitModel.ErrorVisits == null || submitModel.ErrorVisits.Count == 0)
                        submitModel.ErrorMessage = output.ErrorMessage;

                    await _context.SaveChangesAsync();
                    objTrnScope.Complete();
                    objTrnScope.Dispose();

                    submitModel.SubmissionLogID = submissionLog?.ID;
                }
            }

            catch (Exception ex)
            {
                throw;
            }
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, submitModel.Visits.Count);
            return submitModel;
        }



        [Route("DownloadEDIFile/{SubmissionLogID}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadEDIFile(long SubmissionLogID)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            SubmissionLog submission = await _context.SubmissionLog.FindAsync(SubmissionLogID);
            if (submission != null)
            {
                if (!System.IO.File.Exists(submission.Transaction837Path))
                {
                    return NotFound();
                }

                Byte[] fileBytes = System.IO.File.ReadAllBytes(submission.Transaction837Path);
                //this returns a byte array of the PDF file content
                if (fileBytes == null)
                    return NotFound();
                var stream = new MemoryStream(fileBytes); //saves it into a stream
                stream.Position = 0;
                Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
                this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);
                return File(stream, "application/octec-stream", "837p.txt");
            }

            return NotFound();
        }



        [Route("DeleteVisit/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteVisit(long id)
        {
            // Delete Charges First.
            //foreach (Charge charge in item.Charges)
            //{

            //}

            var visit = await _context.Visit.FindAsync(id);

            if (visit == null)
            {
                return NotFound();
            }

            _context.Visit.Remove(visit);
            await _context.SaveChangesAsync();

            return Ok();
        }

        public class VisitComparer : IEqualityComparer<long>
        {
            public bool Equals(long x, long y)
            {
                return (x.Equals(y));
            }

            public int GetHashCode(long x)
            {
                return 0;
            }
        }

    }
}
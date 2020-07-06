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
using MediFusionPM.BusinessLogic;
using MediFusionPM.BusinessLogic.ClaimGeneration;
using MediFusionPM.BusinessLogic.HCFAPrinting;
using MediFusionPM.Models;
using MediFusionPM.Models.TodoApi.Models;
using MediFusionPM.ViewModels;
using static MediFusionPM.BusinessLogic.ClaimGeneration.ClaimGenerator;
using static MediFusionPM.BusinessLogic.ClaimGeneration.ClaimGenerator.Output;
using static MediFusionPM.ViewModels.VMCommon;
using static MediFusionPM.ViewModels.VMPaperSubmission;
using Microsoft.Extensions.Configuration;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class PaperSubmissionController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;

        public IConfiguration Configuration { get; }

        public PaperSubmissionController(ClientDbContext context, MainContext contextMain, IConfiguration configuration)
        {
            _context = context;
            _contextMain = contextMain;
            Configuration = configuration;
            _startTime = DateTime.Now;

        }

        [Route("GetProfiles")]
        [HttpGet()]
        public async Task<ActionResult<VMPaperSubmission>> GetProfiles()
        {
            ViewModels.VMPaperSubmission obj = new ViewModels.VMPaperSubmission();
            obj.GetProfiles(_context);
            return obj;
        }

        [HttpPost]
        [Route("FindVisits")]
        public async Task<ActionResult<IEnumerable<GPaperSubmission>>> FindVisits(CPaperSubmission CModel)
        {
            //  UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            // User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.planFollowupSearch == false)
            //return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindVisits(CModel, PracticeId);
        }

        private List<GPaperSubmission> FindVisits(CPaperSubmission CModel, long PracticeId)
            {
            List<GPaperSubmission> PrimaryVisits = (from v in _context.Visit
                                                    join pat in _context.Patient
                                                    on v.PatientID equals pat.ID
                                                    join prac in _context.Practice
                                                    on v.PracticeID equals prac.ID
                                                    join loc in _context.Location
                                                    on v.LocationID equals loc.ID
                                                    join prov in _context.Provider
                                                    on v.ProviderID equals prov.ID
                                                    join pPlan in _context.PatientPlan
                                                    on v.PrimaryPatientPlanID equals pPlan.ID
                                                    join insPlan in _context.InsurancePlan
                                                    on pPlan.InsurancePlanID equals insPlan.ID
                                                    //join up in _context.UserPractices on fac.ID equals up.PracticeID
                                                    //join u in _context.Users on up.UserID equals u.Id
                                                    where
                                                    v.PracticeID == PracticeId &&
                                                    v.IsDontPrint == false &&
                                                    (insPlan.SubmissionType.Equals("P")) &&
                                                    (string.IsNullOrEmpty(CModel.AccountNum) ? true : pat.AccountNum.Equals(CModel.AccountNum)) &&
                                                    (string.IsNullOrEmpty(CModel.Practice) ? true : prac.Name.Contains(CModel.Practice)) &&
                                                    (string.IsNullOrEmpty(CModel.Provider) ? true : prov.Name.Contains(CModel.Provider)) &&
                                                    (CModel.Location.IsNull2() ? true : loc.Name.Contains(CModel.Location)) &&
                                                    (string.IsNullOrEmpty(CModel.PlanName) ? true : insPlan.PlanName.Contains(CModel.PlanName)) &&
                                                    (string.IsNullOrEmpty(CModel.FormType) ? true : insPlan.FormType.Contains(CModel.FormType)) &&
                                                    (CModel.VisitID.IsNull() ? true : v.ID.Equals(CModel.VisitID)) &&
                                                    //(ExtensionMethods.IsBetweenDOS(CModel.EntryDateTo, CModel.EntryDateFrom, v.AddedDate, v.AddedDate))&&
                                                   //  (ExtensionMethods.IsBetweenDOS(CModel.EntryDateTo, CModel.EntryDateFrom, v.AddedDate, v.AddedDate))&&
                                                    (CModel.EntryDateFrom == null && CModel.EntryDateTo == null ? true : CModel.EntryDateFrom != null && CModel.EntryDateTo != null ?  v.AddedDate >= CModel.EntryDateFrom && v.AddedDate <= CModel.EntryDateTo : CModel.EntryDateFrom != null ? v.AddedDate >= CModel.EntryDateFrom : false )&&
                                                    (CModel.InsuranceType == "P" ? v.PrimaryPatientPlanID.Value > 0 && v.SecondaryPatientPlanID == null && v.TertiaryPatientPlanID == null :
                                                      CModel.InsuranceType == "S" ? v.PrimaryPatientPlanID.Value > 0 && v.SecondaryPatientPlanID > 0 && v.TertiaryPatientPlanID == null :
                                                      CModel.InsuranceType == "T" ? v.PrimaryPatientPlanID.Value > 0 && v.SecondaryPatientPlanID > 0 && v.TertiaryPatientPlanID > 0 :
                                                      v.PrimaryPatientPlanID.Value > 0)
                                                    && (v.PrimaryStatus == "N" || v.PrimaryStatus == null || v.PrimaryStatus == "") &&
                                                   (v.IsSubmitted == false) //&& IsChargesAvailable(_context, v.ID)
                                                    select new GPaperSubmission()
                                                    {
                                                        AccountNum = pat.AccountNum,
                                                        DOS = v.DateOfServiceFrom.Format("MM/dd/yyyy"),
                                                        PracticeID = prac.ID,
                                                        Practice = prac.Name,
                                                        PatientID = pat.ID,
                                                        Patient = pat.LastName + ", " + pat.FirstName,
                                                        ProviderID = prov.ID,
                                                        Provider = prov.Name,
                                                        VisitID = v.ID,
                                                        PlanName = insPlan.PlanName,
                                                        InsurancePlanID = insPlan.ID.ToString(),
                                                        ID = 0,
                                                        LocationID = loc.ID,
                                                        Location = loc.Name,
                                                        TotalAmount = v.TotalAmount.Val(),
                                                        ValidationMessage = v.ValidationMessage,
                                                        SubscriberID = pPlan.SubscriberId,
                                                        PrimaryPatientPlanID = v.PrimaryPatientPlanID.ToString(),
                                                        VisitEntryDate = v.AddedDate.ToString("MM/dd/yyyy"),
                                                        //For Testing
                                                        PrimaryStatus = v.PrimaryStatus,
                                                        SecondaryStatus = v.SecondaryStatus,
                                                        //}).Distinct() .ToListAsync();
                                                    }).ToList<GPaperSubmission>();

         
            //For Seconday Paper Submissions
            List<GPaperSubmission> SecondaryVisits = (from v in _context.Visit
                                                      join pat in _context.Patient
                                                      on v.PatientID equals pat.ID
                                                      join prac in _context.Practice
                                                      on v.PracticeID equals prac.ID
                                                      join loc in _context.Location
                                                      on v.LocationID equals loc.ID
                                                      join prov in _context.Provider
                                                      on v.ProviderID equals prov.ID
                                                      join pPlan in _context.PatientPlan
                                                      on v.SecondaryPatientPlanID equals pPlan.ID
                                                      join insPlan in _context.InsurancePlan
                                                      on pPlan.InsurancePlanID equals insPlan.ID
                                                      //join up in _context.UserPractices on fac.ID equals up.PracticeID
                                                      //join u in _context.Users on up.UserID equals u.Id
                                                      where v.PracticeID == PracticeId &&
                                                      (v.SecondaryPatientPlanID == pPlan.ID) &&
                                                      (v.SecondaryBilledAmount.Val() > 0) && 
                                                      (v.SecondaryBal.Val() > 0) && 
                                                      (v.SecondaryStatus.IsNull2() || v.SecondaryStatus == "N") &&
                                                      //(v.SecondaryStatus.Equals("N")) && 
                                                      (insPlan.SubmissionType.Equals("P")) && (v.IsSubmitted == true) && // Issubmitted set as True fo Secondary
                                                      v.IsDontPrint == false &&
                                                      (string.IsNullOrEmpty(CModel.AccountNum) ? true : pat.AccountNum.Equals(CModel.AccountNum)) &&
                                                      (string.IsNullOrEmpty(CModel.Practice) ? true : prac.Name.Contains(CModel.Practice)) &&
                                                      (string.IsNullOrEmpty(CModel.Provider) ? true : prov.Name.Contains(CModel.Provider)) &&
                                                      (CModel.Location.IsNull2() ? true : loc.Name.Contains(CModel.Location)) &&
                                                      (string.IsNullOrEmpty(CModel.PlanName) ? true : insPlan.PlanName.Contains(CModel.PlanName)) &&
                                                      (string.IsNullOrEmpty(CModel.FormType) ? true : insPlan.FormType.Contains(CModel.FormType)) &&
                                                      (CModel.VisitID.IsNull() ? true : v.ID.Equals(CModel.VisitID))&&
                                                     // (ExtensionMethods.IsBetweenDOS(CModel.EntryDateTo, CModel.EntryDateFrom, v.AddedDate, v.AddedDate))
                                                     (CModel.EntryDateFrom == null && CModel.EntryDateTo == null ?true :  CModel.EntryDateFrom != null && CModel.EntryDateTo != null ? v.AddedDate >= CModel.EntryDateFrom && v.AddedDate <= CModel.EntryDateTo : CModel.EntryDateFrom != null ? v.AddedDate >= CModel.EntryDateFrom : false) &&
                                                     (CModel.InsuranceType == "P" ? v.PrimaryPatientPlanID.Value > 0 && v.SecondaryPatientPlanID == null && v.TertiaryPatientPlanID == null :
                                                      CModel.InsuranceType == "S" ? v.PrimaryPatientPlanID.Value > 0 && v.SecondaryPatientPlanID > 0 && v.TertiaryPatientPlanID == null :
                                                      CModel.InsuranceType == "T" ? v.PrimaryPatientPlanID.Value > 0 && v.SecondaryPatientPlanID > 0 && v.TertiaryPatientPlanID > 0 :
                                                      v.PrimaryPatientPlanID.Value > 0)
                                                      select new GPaperSubmission()
                                                      {
                                                          AccountNum = pat.AccountNum,
                                                          DOS = v.DateOfServiceFrom.Format("MM/dd/yyyy"),
                                                          PracticeID = prac.ID,
                                                          Practice = prac.Name,
                                                          PatientID = pat.ID,
                                                          Patient = pat.LastName + ", " + pat.FirstName,
                                                          ProviderID = prov.ID,
                                                          Provider = prov.Name,
                                                          VisitID = v.ID,
                                                          PlanName = insPlan.PlanName,
                                                          InsurancePlanID = insPlan.ID.ToString(),
                                                          ID = 0,
                                                          LocationID = loc.ID,
                                                          Location = loc.Name,
                                                          TotalAmount = v.TotalAmount.Val(),
                                                          ValidationMessage = v.ValidationMessage,
                                                          SubscriberID = pPlan.SubscriberId,
                                                          PrimaryPatientPlanID = v.PrimaryPatientPlanID.ToString(),
                                                          VisitEntryDate = v.AddedDate.ToString("MM/dd/yyyy"),
                                                          //For Testing
                                                          PrimaryStatus = v.PrimaryStatus,
                                                          SecondaryStatus = v.SecondaryStatus
                                                          //}).Distinct() .ToListAsync();
                                                      }).ToList<GPaperSubmission>();

            List<GPaperSubmission> newList = new List<GPaperSubmission>();

            if (PrimaryVisits != null) newList.AddRange(PrimaryVisits);
            if (SecondaryVisits != null) newList.AddRange(SecondaryVisits);
            return newList;

        }

        private bool IsChargesAvailable(ClientDbContext Context, long VisitID)
        {
            return Context.Charge.Where(c => c.VisitID == VisitID && c.IsSubmitted == false && c.IsDontPrint == false).Count() > 0 ? true : false;
        }

        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CPaperSubmission CModel)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GPaperSubmission> data = FindVisits(CModel, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CModel, "Model Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CPaperSubmission CModel)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GPaperSubmission> data = FindVisits(CModel, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);
        }
        [Route("SubmitVisits")]
        [HttpPost]
        public async Task<ActionResult<HcfaSubmitModel>> SubmitVisits(HcfaSubmitModel submitModel)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

            try
            {
                if (submitModel.FormType.IsNull2())
                {
                    submitModel.ErrorMessage = "Form Type cannot be empty";
                    return submitModel;
                    //return BadRequest("Form Type cannot be empty");
                }

                if (submitModel.Visits == null || submitModel.Visits.Count == 0)
                {
                    submitModel.ErrorMessage = "No Visits Found";
                    return submitModel;
                    //return BadRequest("No Visits Found");
                }

                Settings settings = await _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefaultAsync();
                if (settings == null)
                {
                    submitModel.ErrorMessage = "Document Server Settings Not Found";
                    return submitModel;
                    //return BadRequest("Document Server Settings Not Found");
                }

                submitModel.ErrorVisits = new List<DropDown>();

                #region ClaimHeader
                //ClaimHeader headerData = await
                //    (from rec in _context.Receiver
                //     join sub in _context.Submitter
                //     on rec.ID equals sub.ReceiverID
                //     join cl in _context.Client
                //     on sub.ClientID equals cl.ID
                //     where
                //     (submitModel.ClientID == 0 ? true : cl.ID.Equals(submitModel.ClientID)) &&
                //     (submitModel.ReceiverID == 0 ? true : rec.ID.Equals(submitModel.ReceiverID))
                //     select new ClaimHeader()
                //     {
                //         Claims = null,
                //         GS02SenderID = sub.X12_837_GS_02,
                //         GS03ReceiverID = rec.X12_837_GS_03,
                //         ISA01AuthQual = rec.X12_837_ISA_01,
                //         ISA02AuthInfo = sub.X12_837_ISA_02,
                //         ISA03SecQual = rec.X12_837_ISA_03,
                //         ISA04SecInfo = sub.X12_837_ISA_04,
                //         ISA05SenderQual = rec.X12_837_ISA_05,
                //         ISA06SenderID = sub.X12_837_ISA_06,
                //         ISA07ReceiverQual = rec.X12_837_ISA_07,
                //         ISA08ReceiverID = rec.X12_837_ISA_08,
                //         ISA13CntrlNumber = "111111111",
                //         ISA15UsageIndi = "T",
                //         ReceiverID = rec.X12_837_NM1_40_ReceiverID,
                //         ReceiverOrgName = rec.X12_837_NM1_40_ReceiverName,
                //         RecieverQual = "46",
                //         RelaxNpiValidation = false,
                //         SFTPModel = new SFTPModel()
                //         {
                //             FTPHost = rec.SubmissionURL,
                //             FTPPort = rec.SubmissionPort,
                //             FTPUserName = sub.SubmissionUserName,
                //             FTPPassword = sub.SubmissionPassword,
                //             SubmitToFTP = sub.ManualSubmission,
                //             ConnectivityType = rec.SubmissionMethod,
                //             SubmitDirectory = rec.SubmissionDirectory,
                //             //FileName = string.Format(sub.FileName, DateTime.Now.ToString("hhmmss"))
                //             FileName = sub.FileName
                //         },
                //         SubmitterContactName = sub.SubmitterContactNumber,
                //         SubmitterEmail = sub.SubmitterEmail,
                //         SubmitterEntity = "2",
                //         SubmitterFax = sub.SubmitterFaxNumber,
                //         SubmitterFirstName = "",
                //         SubmitterID = sub.X12_837_NM1_41_SubmitterID,
                //         SubmitterOrgName = sub.X12_837_NM1_41_SubmitterName,
                //         SubmitterQual = "46",
                //         SubmitterTelephone = sub.SubmitterContactNumber,
                //     }).SingleOrDefaultAsync();
                #endregion

                ClaimHeader headerData = new ClaimHeader() { SFTPModel = new SFTPModel() };

                #region Claims
                headerData.Claims = await
                    (
                    from v in _context.Visit
                    join pat in _context.Patient
                    on v.PatientID equals pat.ID
                    join fac in _context.Practice
                    on pat.PracticeID equals fac.ID
                    join loc in _context.Location
                    on pat.LocationId equals loc.ID
                    join prov in _context.Provider
                    on pat.ProviderID equals prov.ID
                    join pPlan in _context.PatientPlan
                    on v.PrimaryPatientPlanID equals pPlan.ID
                    join insPlan in _context.InsurancePlan
                    on pPlan.InsurancePlanID equals insPlan.ID
                    join planType in _context.PlanType
                    on insPlan.PlanTypeID equals planType.ID
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
                    where submitModel.Visits.Contains(v.ID, new VisitComparer()) && v.IsSubmitted == false &&
                    insPlan.FormType == submitModel.FormType
                    select new ClaimData()
                    {
                        AccidentDate = v.AccidentDate,
                        AccidentState = v.AccidentState,
                        AccidentType = v.AccidentType,
                        AcuteManifestationDate = null,
                        AdmissionDate = v.AdmissionDate,
                        BenefitsAssignment = "",
                        BillPrvEntityType = "2",
                        BillPrvOrgName = fac.OrganizationName,
                        BillPrvFirstName = "",
                        BillPrvMI = "",
                        BillPrvNPI = fac.NPI,
                        BillPrvTaxID = fac.TaxID,
                        BillPrvTaxonomyCode = fac.TaxonomyCode,
                        BillPrvAddress1 = fac.Address1,
                        BillPrvCity = fac.City,
                        BillPrvState = fac.State,
                        BillPrvZipCode = fac.ZipCode,
                        BillPrvTelephone = fac.OfficePhoneNum,
                        BillPrvContactName = "",
                        BillPrvEmail = fac.Email,
                        BillPrvFax = fac.FaxNumber,
                        BillPrvPayToAddr = fac.PayToAddress1,
                        BillPrvPayToCity = fac.PayToCity,
                        BillPrvPayToState = fac.PayToState,
                        BillPrvPayToZip = fac.ZipCode,
                        BillPrvSecondaryID = "",
                        BillPrvSSN = "",
                        Charges = null,
                        ClaimAmount = v.TotalAmount.Amt(),
                        ClaimFreqCode = v.ClaimFrequencyCode,
                        ClaimNotes = v.ClaimNotes,
                        ClaimType = pPlan.Coverage,
                        CliaNumber = v.CliaNumber,
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
                        LastSeenDate = null,
                        LastWorkedDate = null,
                        LMPDate = v.DateOfPregnancy,
                        LocationAddress = loc.Address1,
                        LocationCity = loc.City,
                        LocationNPI = loc.NPI,
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
                        PayerID = "",
                        PayerName = insPlan.Description,
                        //PayerAddress = insPlanAdd.Address1,
                        //PayerCity = insPlanAdd.City,
                        //PayerState = insPlanAdd.State,
                        //PayerZipCode = insPlanAdd.ZipCode,
                        PatientControlNumber = v.ID + "_" + pat.AccountNum,
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
                        SbrGroupNumber = "",
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
                        VisitID = v.ID
                    }).ToListAsync<ClaimData>();
                #endregion

                
                if (headerData.Claims == null || headerData.Claims.Count == 0)
                {

                    submitModel.ErrorMessage = "No Visit(s) Qualified for Submission";
                    return submitModel;
                    //return BadRequest("No Visit(s) Qualified for Submission");
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
                where c.VisitID == CLM.VisitID
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
                    IsEmergency = v.Emergency != null ? v.Emergency.Value : false,
                    LIDescription = "",
                    LineItemControlNum = c.ID.ToString(),
                    Minutes = c.Minutes,
                    Units = c.Units,
                    Modifier1 = mod1.Code,
                    Modifier2 = mod2.Code,
                    Modifier3 = mod3.Code,
                    Modifier4 = mod4.Code,
                    Pointer1 = c.Pointer1,
                    Pointer2 = c.Pointer2,
                    Pointer3 = c.Pointer3,
                    Pointer4 = c.Pointer4,
                    POS = ""

                }).ToListAsync();

                    #endregion
                }

                string directoryPath = Path.Combine("\\\\", settings.DocumentServerURL,
                        settings.DocumentServerDirectory, UD.ClientID.ToString(), "Paper", submitModel.FormType.ToString(),
                        DateTime.Now.ToString("MMddyyyy"), DateTime.Now.ToString("hhmmssff"));
                headerData.SFTPModel.RootDirectory = directoryPath;

                string networkURL = Configuration["ClientSettings:DeploymentURL"]; ;

                GenerateHCFA1500 hCFA1500 = new GenerateHCFA1500(true,networkURL);
                HcfaOutput output = hCFA1500.GenerateHcfa(headerData,
                    Path.Combine(_context.env.ContentRootPath, "Resources", "CMS_1500_NUCC.pdf"),
                    directoryPath);

                SubmissionLog submissionLog = null;
                var transactionOption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
                using (var objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption, TransactionScopeAsyncFlowOption.Enabled))
                {

                    #region Processed Claims
                    if (output.ProcessedClaims > 0)
                    {
                        // Submission Log 
                        submissionLog = new SubmissionLog()
                        {
                            AddedBy = UD.Email,
                            AddedDate = DateTime.Now,
                            ClaimCount = output.ProcessedClaims,
                            ClaimAmount = output.ClaimAmount,
                            ClientID = UD.ClientID,
                            SubmitType = "P",
                            PdfPath = output.PDFFilePath,
                            FormType = submitModel.FormType
                        };

                        await _context.SubmissionLog.AddAsync(submissionLog);
                        //await _context.SaveChangesAsync();


                        #region Updating Claims
                        foreach (ClaimResult claim in output.Claims)
                        {
                            Visit visit = await _context.Visit.FindAsync(claim.VisitID);
                            if (visit != null)
                            {
                                if (claim.Submitted)
                                {
                                    visit.IsSubmitted = claim.Submitted;
                                    visit.SubmittedDate = DateTime.Now; // Need To change and make it according Server Time
                                    visit.SubmissionLogID = submissionLog.ID;
                                    visit.PrimaryStatus = "S";
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
                                            charge.IsSubmitted =true;
                                            charge.SubmittetdDate = DateTime.Now;
                                            //charge.IsSubmitted = C.Submitted;
                                            //charge.SubmittetdDate = C.SubmittedDate;
                                            visit.SubmissionLogID = submissionLog.ID;
                                            visit.PrimaryStatus = "S";
                                            _context.Charge.Update(charge);
                                            //await _context.SaveChangesAsync();

                                            ChargeSubmissionHistory chargeHistory = new ChargeSubmissionHistory()
                                            {
                                                AddedBy = UD.Email,
                                                AddedDate = DateTime.Now,
                                                ChargeID = charge.ID,
                                                FormType = submitModel.FormType,
                                                SubmissionLogID = submissionLog.ID,
                                                SubmitType = "P",
                                                PatientPlanID = charge.PrimaryPatientPlanID,
                                                Amount = charge.PrimaryBilledAmount

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


                        submitModel.FilePath = output.PDFFilePath;
                        submitModel.ProcessedClaims = output.ProcessedClaims;
                        
                    }

                    #endregion

                    #region Updating Error Claims

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
                                    throw new System.Exception("Visit Not Found");
                                }
                            }

                        }
                    }

                    #endregion

                    await _context.SaveChangesAsync();
                    objTrnScope.Complete();
                    objTrnScope.Dispose();
                }

                if (submitModel.ErrorVisits == null || submitModel.ErrorVisits.Count == 0)
                    submitModel.ErrorMessage = output.ErrorMessage;

                submitModel.SubmissionLogID = submissionLog?.ID;
            }

            catch (Exception ex)
            {
                throw;
            }
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, submitModel.Visits.Count);
            return submitModel;
        }


        [Route("DownloadHCFAFile/{SubmissionLogID}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadHCFAFile(long SubmissionLogID)
        {
            SubmissionLog submission = await _context.SubmissionLog.FindAsync(SubmissionLogID);
            if (submission != null)
            {

                if (!System.IO.File.Exists(submission.PdfPath))
                {
                    return NotFound();
                }
                Byte[] fileBytes = System.IO.File.ReadAllBytes(submission.PdfPath);
                //this returns a byte array of the PDF file content
                if (fileBytes == null)
                    return NotFound();
                var stream = new MemoryStream(fileBytes); //saves it into a stream
                stream.Position = 0;
                return File(stream, "application/octec-stream", "837p.pdf");
            }

            return NotFound();
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



        [Route("GetHCFAFilePath/{SubmissionLogID}")]
        [HttpGet("{id}")]
        public string GetHCFAFilePath(long SubmissionLogID)
        {
            SubmissionLog submission = _context.SubmissionLog.Find(SubmissionLogID);
            if (submission != null)
            {
                if (!System.IO.File.Exists(submission.PdfPath))
                {
                    return "File Not Found";
                }
                return submission.PdfPath;
            }

            return "File Not Found";
        }

        [Route("ViewHcfaFile")]
        [HttpPost]
        public string ViewHcfaFile(ListModel model)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
          );

            Settings settings = _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefault();
            if (settings == null)
            {
                return "Document Server Settings Not Found";
            }
            string outputFilePath = string.Empty;

            try
            {
                ClaimHeader headerData = new ClaimHeader() { SFTPModel = new SFTPModel() };

                #region Claims
                headerData.Claims =
                    (
                    from v in _context.Visit
                    join pat in _context.Patient
                    on v.PatientID equals pat.ID
                    join fac in _context.Practice
                    on pat.PracticeID equals fac.ID
                    join loc in _context.Location
                    on pat.LocationId equals loc.ID
                    join prov in _context.Provider
                    on pat.ProviderID equals prov.ID
                    join pPlan in _context.PatientPlan
                    on v.PrimaryPatientPlanID equals pPlan.ID
                    join insPlan in _context.InsurancePlan
                    on pPlan.InsurancePlanID equals insPlan.ID
                    join planType in _context.PlanType
                    on insPlan.PlanTypeID equals planType.ID
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
                    join i7 in _context.ICD
                    on v.ICD7ID equals i7.ID into i71
                    from icd7 in i71.DefaultIfEmpty()
                    join i8 in _context.ICD
                    on v.ICD8ID equals i8.ID into i81
                    from icd8 in i81.DefaultIfEmpty()
                    join i9 in _context.ICD
                    on v.ICD9ID equals i9.ID into i91
                    from icd9 in i91.DefaultIfEmpty()
                    join i10 in _context.ICD
                    on v.ICD10ID equals i10.ID into i10_1
                    from icd10 in i10_1.DefaultIfEmpty()
                    join i11 in _context.ICD
                    on v.ICD11ID equals i11.ID into i11_1
                    from icd11 in i11_1.DefaultIfEmpty()
                    join i12 in _context.ICD
                    on v.ICD12ID equals i12.ID into i12_1
                    from icd12 in i12_1.DefaultIfEmpty()
                    join pos in _context.POS
                    on v.POSID equals pos.ID
                    join ipa in _context.InsurancePlanAddress
                    on pPlan.InsurancePlanAddressID equals ipa.ID into ipa2
                    from insPlanAdd in ipa2.DefaultIfEmpty()
                    join rp in _context.RefProvider
                    on v.RefProviderID equals rp.ID into rp1
                    from refProv in rp1.DefaultIfEmpty()
                    join sp in _context.Provider
                    on v.SupervisingProvID equals sp.ID into sp1
                    from supProv in sp1.DefaultIfEmpty()
                    where model.Ids.Contains(v.ID, new VisitComparer())
                    select new ClaimData()
                    {
                        AccidentDate = v.AccidentDate,
                        AccidentState = v.AccidentState,
                        AccidentType = v.AccidentType,
                        AcuteManifestationDate = null,
                        AdmissionDate = v.AdmissionDate,
                        BenefitsAssignment = "",
                        BillPrvEntityType = "2",
                        BillPrvOrgName = fac.OrganizationName,
                        BillPrvFirstName = "",
                        BillPrvMI = "",
                        BillPrvNPI = fac.NPI,
                        BillPrvTaxID = fac.TaxID,
                        BillPrvTaxonomyCode = fac.TaxonomyCode,
                        BillPrvAddress1 = fac.Address1,
                        BillPrvCity = fac.City,
                        BillPrvState = fac.State,
                        BillPrvZipCode = fac.ZipCode,
                        BillPrvTelephone = fac.OfficePhoneNum,
                        BillPrvContactName = "",
                        BillPrvEmail = fac.Email,
                        BillPrvFax = fac.FaxNumber,
                        BillPrvPayToAddr = fac.PayToAddress1,
                        BillPrvPayToCity = fac.PayToCity,
                        BillPrvPayToState = fac.PayToState,
                        BillPrvPayToZip = fac.ZipCode,
                        BillPrvSecondaryID = "",
                        BillPrvSSN = "",
                        Charges = null,
                        ClaimAmount = v.TotalAmount.Amt(),
                        ClaimFreqCode = v.ClaimFrequencyCode,
                        ClaimNotes = v.ClaimNotes,
                        ClaimType = pPlan.Coverage,
                        CliaNumber = v.CliaNumber,
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
                        ICD7Code = icd7.ICDCode,
                        ICD8Code = icd8.ICDCode,
                        ICD9Code = icd9.ICDCode,
                        ICD10Code = icd10.ICDCode,
                        ICD11Code = icd11.ICDCode,
                        ICD12Code = icd12.ICDCode,
                        InitialTreatmentDate = null,
                        IsMedicareMedigap = false,
                        IsSelfSubscribed = pPlan.RelationShip == "18" ? true : false,
                        LastSeenDate = null,
                        LastWorkedDate = null,
                        LMPDate = v.DateOfPregnancy,
                        LocationAddress = loc.Address1,
                        LocationCity = loc.City,
                        LocationNPI = loc.NPI,
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
                        PayerID = "",
                        PayerName = insPlan.Description,
                        PayerAddress = insPlanAdd.Address1,
                        PayerCity = insPlanAdd.City,
                        PayerState = insPlanAdd.State,
                        PayerZipCode = insPlanAdd.ZipCode,
                        PatientControlNumber = v.ID + "_" + pat.AccountNum,
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
                        SbrGroupNumber = "",
                        SBRGender = pPlan.Gender,
                        SBRDob = pPlan.DOB,
                        SBRSSN = pPlan.SSN,
                        SBRAddress = pPlan.Address1,
                        SBRCity = pPlan.City,
                        SBRState = pPlan.State,
                        SBRZipCode = pPlan.ZipCode,
                        SbrMedicareSecTypeV = "",
                        SuperPrvFirstName = supProv.FirstName,
                        SuperPrvLastName = supProv.LastName,
                        SuperPrvMI = supProv.MiddleInitial,
                        SuperPrvNPI = supProv.NPI,
                        VisitID = v.ID
                    }).ToList<ClaimData>();
                #endregion

                if (headerData.Claims == null || headerData.Claims.Count == 0)
                {
                    return "Data Not Found";
                }

                foreach (ClaimData CLM in headerData.Claims)
                {
                    #region Charges
                    CLM.Charges = (
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
                where c.VisitID == CLM.VisitID
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
                    IsEmergency = false,
                    LIDescription = "",
                    LineItemControlNum = c.ID.ToString(),
                    Minutes = c.Minutes,
                    Units = c.Units,
                    Modifier1 = mod1.Code,
                    Modifier2 = mod2.Code,
                    Modifier3 = mod3.Code,
                    Modifier4 = mod4.Code,
                    Pointer1 = c.Pointer1,
                    Pointer2 = c.Pointer2,
                    Pointer3 = c.Pointer3,
                    Pointer4 = c.Pointer4,
                    POS = ""

                }).ToList();

                    # endregion
                }
                string directoryPath = Path.Combine(_context.env.ContentRootPath, "wwwroot", "accessible-files");
                
                //string directoryPath = Path.Combine("https://service.medifusion.com", "wwwroot", "accessible-files");
               
                //string directoryPath = Path.Combine("\\\\", settings.DocumentServerURL,
                //        settings.DocumentServerDirectory, "TEMP", "ViewPaper", "HCFA 1500",
                //        DateTime.Now.ToString("MMddyyyy"), DateTime.Now.ToString("hhmmssff"));
                headerData.SFTPModel.RootDirectory = directoryPath;
                string networkURL = Configuration["ClientSettings:DeploymentURL"]; ;
                GenerateHCFA1500 hCFA1500 = new GenerateHCFA1500(false,networkURL);
                HcfaOutput output = hCFA1500.GenerateHcfa(headerData,
                 Path.Combine(_context.env.ContentRootPath, "Resources", "CMS_1500_NUCC.pdf"),
                 directoryPath);

                string SystemSpecificStaticFilePath = output.PDFFilePath;

                string networkPath = Path.Combine(networkURL,"accessible-files",Path.GetFileName(SystemSpecificStaticFilePath));

                outputFilePath = networkPath;
            }

            catch (Exception ex)
            {
                throw;
            }
            return outputFilePath;
        }

        }
}
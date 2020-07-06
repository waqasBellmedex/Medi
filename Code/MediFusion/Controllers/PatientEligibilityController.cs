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
using MediFusionPM.BusinessLogic.EligGenerator;
using MediFusionPM.BusinessLogic.EligParser;
using MediFusionPM.Models;
using MediFusionPM.Models.TodoApi.Models;
using MediFusionPM.ViewModels;
using static MediFusionPM.ViewModels.VMCommon;
using static MediFusionPM.ViewModels.VMPatient;
using static MediFusionPM.ViewModels.VMPatientEligibility;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class PatientEligibilityController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;

        public PatientEligibilityController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PerformEligibility")]

        public async Task<ActionResult<VMPatientEligibility>> PerformEligibility(EligibilitySubmitModel SubmitModel)
        {

            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //long clientID = 1;
            long ClientID = UD.ClientID;
            //if (SubmitModel.ProviderID.IsNull())
            //{
            //    return BadRequest("Provider Should not be empty");
            //}

            Settings settings = await _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefaultAsync();
            if (settings == null)
            {
                return BadRequest("Document Server Settings Not Found");
            }


            Edi270Payer obj = (from pp in _context.PatientPlan
                             join ip in _context.InsurancePlan
                             on pp.InsurancePlanID equals ip.ID
                             join edi270 in _context.Edi270Payer
                             on ip.Edi270PayerID equals edi270.ID
                             where pp.ID == SubmitModel.PatientPlanID
                       select edi270
                             ).SingleOrDefault();

            if (obj == null)
                return BadRequest("Plan is not Setup for Eligibiltiy");

            long ReceiverID = obj.ReceiverID;

            #region ClaimHeader
            _270Header Header = await
                (
                from pp in _context.PatientPlan
                             join ip in _context.InsurancePlan
                             on pp.InsurancePlanID equals ip.ID
                             join edi270 in _context.Edi270Payer
                             on ip.Edi270PayerID equals edi270.ID
                join rec in _context.Receiver on
                edi270.ReceiverID equals rec.ID
                 join sub in _context.Submitter
                 on rec.ID equals sub.ReceiverID
                 join cl in _context.Client
                 on sub.ClientID equals cl.ID
                 where
                 cl.ID == UD.ClientID && pp.ID == SubmitModel.PatientPlanID
                select new _270Header()
                 {
                     GS02SenderID = sub.X12_270_GS_02,
                     GS03ReceiverID = rec.X12_270_GS_03,
                     ISA01AuthQual = rec.X12_270_ISA_01,
                     ISA02AuthInfo = sub.X12_270_ISA_02,
                     ISA03SecQual = rec.X12_270_ISA_03,
                     ISA04SecInfo = sub.X12_270_ISA_04,
                     ISA05SenderQual = rec.X12_270_ISA_05,
                     ISA06SenderID = sub.X12_270_ISA_06,
                     ISA07ReceiverQual = rec.X12_270_ISA_07,
                     ISA08ReceiverID = rec.X12_270_ISA_08,
                     ISA13CntrlNumber = "111111111",
                     ISA15UsageIndi = "T"
                 }).SingleOrDefaultAsync();
            #endregion


            if (Header == null || Header.ISA06SenderID.IsNull() || Header.ISA08ReceiverID.IsNull() ||
                   Header.GS02SenderID.IsNull() || Header.GS03ReceiverID.IsNull())
            {
                return BadRequest("Electronic Setup is not Complete");
            }


            #region Patient Data
            Header.ListOfSBRData = await
                    (
                    from pat in _context.Patient
                    join pp in _context.PatientPlan
                    on pat.ID equals pp.PatientID
                    join ip in _context.InsurancePlan
                    on pp.InsurancePlanID equals ip.ID
                    join edi270 in _context.Edi270Payer
                    on ip.Edi270PayerID equals edi270.ID
                    join rec in _context.Receiver on
                    edi270.ReceiverID equals rec.ID
                    join sub in _context.Submitter
                    on rec.ID equals sub.ReceiverID
                    join cl in _context.Client
                    on sub.ClientID equals cl.ID
                    where cl.ID == UD.ClientID && pp.ID == SubmitModel.PatientPlanID
                    select new _270Data()
                    {
                        PATAddress = pat.Address1,
                        PATCity = pat.City,
                        PATState = pat.State,
                        PATZipCode = pat.ZipCode,
                        PATDob = pat.DOB.Date(),
                        PATEntityType = "1",
                        PATFirstName = pat.FirstName,
                        PATLastName = pat.LastName,
                        PATGender = pat.Gender,
                        PATMiddleInitial = pat.MiddleInitial,
                        PayerID = edi270.PayerID,
                        PayerOrgName = edi270.PayerName,
                        PayerEntity = "2",
                        PayerFirstName = "",
                        PayerQual = "PI",
                        SBREntityType = "1",
                        SBRLastName = pp.LastName,
                        SBRFirstName = pp.FirstName,
                        SBRMiddleInitial = pp.MiddleInitial,
                        SBRID = pp.SubscriberId,
                        SBRGender = pp.Gender,
                        SBRDob = pp.DOB,
                        SBRSSN = pp.SSN,
                        SBRAddress = pp.Address1,
                        SBRCity = pp.City,
                        SBRState = pp.State,
                        SBRZipCode = pp.ZipCode,
                        EligiblityForDate = SubmitModel.DOS.Date().IsNull() ? DateTime.Now: SubmitModel.DOS.Date(),
                        PatientID = pat.ID,
                        LocationID = pat.LocationId,
                        PracticeID = pat.PracticeID
                    }).ToListAsync<_270Data>();
            #endregion


            Provider provider = null;
            if (SubmitModel.ProviderID.IsNull())
            {
                provider = (from pat in _context.Patient
                            join p in _context.Provider
                            on pat.ProviderID equals p.ID
                            select p).SingleOrDefault();
            }
            else
            {
                provider = await _context.Provider.FindAsync(SubmitModel.ProviderID);
            }

            Header.ListOfSBRData[0].ProvEntity = "1";
            Header.ListOfSBRData[0].ProvLastName = provider.LastName;
            Header.ListOfSBRData[0].ProvFirstName = provider.FirstName;
            Header.ListOfSBRData[0].ProvMI = provider.MiddleInitial;
            Header.ListOfSBRData[0].ProvNPI = provider.NPI;
            Header.ListOfSBRData[0].ProvTaxonomyCode = provider.TaxonomyCode;

            Header.ListOfSBRData[0].TRN01 = _context.GetNextSequenceValue("S_TRN_270");
            Header.ListOfSBRData[0].TRN02 = Header.ISA06SenderID;


            string directoryPath = Path.Combine("\\\\", settings.DocumentServerURL,
                     settings.DocumentServerDirectory, UD.ClientID.ToString(), "EDI_ELIG", ReceiverID.ToString(),
                     DateTime.Now.ToString("MMddyyyy"), DateTime.Now.ToString("hhmmssff"));
            Directory.CreateDirectory(directoryPath);

            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(Header);
            System.IO.File.WriteAllText(Path.Combine(directoryPath, "Input.Json"), jsonString);


            _270Generator generator = new _270Generator();
            string transation270 = generator.Generate270Transaction(Header);

            PatientEligibility patElig = null;

            var transactionOption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (var objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption, TransactionScopeAsyncFlowOption.Enabled))
            {
                _270Data CLM = Header.ListOfSBRData[0];
                patElig = new PatientEligibility()
                {
                    AddedBy = "MEMON",
                    AddedDate = DateTime.Now,
                    DOS = CLM.EligiblityForDate,
                    EligibilityDate = DateTime.Now,
                    ErrorMessage = "",
                    LocationID = CLM.LocationID,
                    PatientAddress = CLM.PATAddress,
                    PatientCity = CLM.PATCity,
                    PatientDOB = CLM.PATDob,
                    PatientFN = CLM.PATFirstName,
                    PatientGender = CLM.PATGender,
                    PatientID = CLM.PatientID.Value,
                    PatientLN = CLM.PATLastName,
                    PatientMI = CLM.PATMiddleInitial,
                    PatientPlanID = SubmitModel.PatientPlanID,
                    PatientState = CLM.PATState,
                    PatientZip = CLM.PATZipCode,
                    PayerID = CLM.PayerID,
                    PayerName = CLM.PayerOrgName,
                    PracticeID = CLM.PracticeID.Value,
                    ProviderFN = CLM.ProvFirstName,
                    ProviderID = provider.ID,
                    ProviderLN = CLM.ProvLastName,
                    ProviderNPI = CLM.ProvNPI,
                    Rejection = "",
                    RejectionCode = "",
                    Status = "W",
                    SubscriberAddress = CLM.SBRAddress,
                    SubscriberCity = CLM.SBRCity,
                    SubscriberDOB = CLM.SBRDob,
                    SubscriberFN = CLM.SBRFirstName,
                    SubscriberGender = CLM.SBRGender,
                    SubscriberGroupNumber = "",
                    SubscriberID = CLM.SBRID,
                    SubscriberLN = CLM.SBRLastName,
                    SubscriberMI = CLM.SBRMiddleInitial,
                    SubscriberState = CLM.SBRState,
                    SubscriberZip = CLM.SBRZipCode,
                    TRNNumber = CLM.TRN01
                };
                //_context.PatientEligibility.Add(patElig);

                // Integration of Clearing Houses
                string transaction271 = "ISA*00*          *00*          *ZZ*841162764      *ZZ*GATE0308       *190909*1117*^*00501*252111745*0*P*:~GS*HB*841162764*GATE0308*20190909*1117*1*X*005010X279A1~ST*271*000000001*005010X279A1~BHT*0022*11*115182297*20190909*1117~HL*1**20*1~NM1*PR*2*CGLIC*****FI*06-0303370~PER*IC**TE*8002446224*UR*cignaforhcp.cigna.com~HL*2*1*21*1~NM1*1P*1*ORLANDO-WEBER*TIFFANY****XX*1568413193~HL*3*2*22*0~TRN*2*1115176558*GATE0308  ~NM1*IL*1*HERRERA*ELIAS****MI*U5653594301~REF*6P*3327596*REALOGY GROUP, LLC~N3*2503 DRISCOLL ST~N4*HOUSTON*TX*770196707~DMG*D8*19841002*M~INS*Y*18~DTP*356*D8*20170101~EB*1**30**Choice Fund HSA Open Access Plus~MSG*Healthcare professional is in network based on NPI ID provided in request.~MSG*Complete Care Management~EB*C*FAM*30***23*1500.00*****Y~MSG*Amounts apply to both in-network and out-of-network~MSG*Benefit does apply to member's out-of-pocket maximum~MSG*Accumulators are shared between Medical AND Mental Health~EB*G*FAM*30***23*4000.00*****W~MSG*Accumulators are shared between Medical AND Mental Health~EB*A*FAM*30*****.10****Y~DTP*348*D8*20190101~MSG*This benefit does apply to member's out-of-pocket maximum~EB*A*IND*30*****.10****Y~DTP*348*D8*20190101~MSG*This benefit does apply to member's out-of-pocket maximum~EB*C*FAM*30***23*3000.00*****N~MSG*Benefit does apply to member's out-of-pocket maximum~MSG*Amounts apply to both in-network and out-of-network~MSG*Accumulators are shared between Medical AND Mental Health~EB*A*FAM*30*****.40****N~DTP*348*D8*20190101~MSG*This benefit does apply to member's out-of-pocket maximum~EB*A*IND*30*****.40****N~DTP*348*D8*20190101~MSG*This benefit does apply to member's out-of-pocket maximum~EB*1**81^3^69^80^62^BQ^7^2^82^BR^86^84^73^98^50^70^BH^9^68^4^5^19^83^97^BG^33^BN^76^74^52^48^75^79^A1^71^A9^51^67^A3^56^78^BF^12^AB^53^64^6^AG^8^18^BD^99^UC^A0^AE^AF^17^61^13^AD^BL^BK^47^1^96*********W~EB*A*IND*81*****.00***N*Y~DTP*348*D8*20190101~MSG*Specialist~MSG*PCP~III*ZZ*11~EB*C*IND*81***23*0.00****N*Y~MSG*Specialist~MSG*PCP~III*ZZ*11~EB*C*IND*69^12^18***23*0.00****N*Y~MSG*Breast-Feeding Equipment and Supplies~III*ZZ*12~EB*A*IND*69^12^18*****.00***N*Y~DTP*348*D8*20190101~MSG*Breast-Feeding Equipment and Supplies~III*ZZ*12~EB*A*IND*80*****.00***N*W~DTP*348*D8*20190101~MSG*H1N1 A Vaccine Administration~EB*C*IND*80***23*0.00****N*W~MSG*H1N1 A Vaccine Administration~EB*C*IND*80***23*0.00****N*Y~MSG*Immunizations~MSG*PPACA Preventive Immunizations - PCP~MSG*PPACA Preventive Immunizations - Specialist~III*ZZ*11~EB*A*IND*80*****.00***N*Y~DTP*348*D8*20190101~MSG*Immunizations~MSG*PPACA Preventive Immunizations - PCP~MSG*PPACA Preventive Immunizations - Specialist~III*ZZ*11~EB*A*FAM*62*****.10***N*N~DTP*348*D8*20190101~MSG*MRI~MSG*This benefit does apply to member's out-of-pocket maximum~MSG*CAT~III*ZZ*23~EB*C*FAM*62***23*1500.00****N*N~MSG*MRI~MSG*Benefit does apply to member's out-of-pocket maximum~MSG*Combined with In Network Plan Level~MSG*CAT~III*ZZ*23~EB*C*IND*62***23*0.00****N*Y~MSG*CAT - Preventive Colonoscopy~III*ZZ*22~EB*A*IND*62*****.00***N*Y~DTP*348*D8*20190101~MSG*CAT - Preventive Colonoscopy~III*ZZ*22~EB*A*IND*62*****.10***Y*N~DTP*348*D8*20190101~MSG*CAT~MSG*This benefit does apply to member's out-of-pocket maximum~MSG*MRI~III*ZZ*20~EB*CB**62^73********Y*N~EB*A*IND*62*****.10***N*N~DTP*348*D8*20190101~MSG*MRI~MSG*This benefit does apply to member's out-of-pocket maximum~MSG*CAT~III*ZZ*23~EB*A*FAM*62*****.10***Y*N~DTP*348*D8*20190101~MSG*MRI~MSG*This benefit does apply to member's out-of-pocket maximum~MSG*CAT~III*ZZ*20~EB*C*FAM*62***23*1500.00****Y*N~MSG*MRI~MSG*Benefit does apply to member's out-of-pocket maximum~MSG*Combined with In Network Plan Level~MSG*CAT~III*ZZ*20~EB*C*IND*62***23*0.00****N*Y~MSG*CAT - PCP Preventive Colonoscopy~MSG*CAT - Specialist Preventive Colonoscopy~III*ZZ*11~EB*A*IND*62*****.00***N*Y~DTP*348*D8*20190101~MSG*CAT - PCP Preventive Colonoscopy~MSG*CAT - Specialist Preventive Colonoscopy~III*ZZ*11~EB*C*IND*82***23*0.00****N*Y~MSG*Hormone Patch - Specialist~MSG*Including Womens Specialist~MSG*Hormone Patch - PCP~MSG*Including Womens PCP~MSG*Gynecological~MSG*Contraceptive injection PCP~MSG*Contraceptive injection Specialist~MSG*Insertion Of IUD~III*ZZ*11~EB*A*IND*82*****.00***N*Y~DTP*348*D8*20190101~MSG*Hormone Patch - Specialist~MSG*Including Womens Specialist~MSG*Hormone Patch - PCP~MSG*Including Womens PCP~MSG*Gynecological~MSG*Contraceptive injection PCP~MSG*Contraceptive injection Specialist~MSG*Insertion Of IUD~III*ZZ*11~EB*C*IND*82***23*0.00****Y*Y~MSG*Insertion Of IUD - SPC~MSG*Insertion Of IUD - PCP~III*ZZ*11~EB*CB**82********Y*Y~EB*A*IND*82*****.00***Y*Y~DTP*348*D8*20190101~MSG*Insertion Of IUD - SPC~MSG*Insertion Of IUD - PCP~III*ZZ*11~EB*A*IND*82*****.00***Y*Y~DTP*348*D8*20190101~MSG*Tubal Ligation~III*ZZ*22~EB*C*IND*82***23*0.00****Y*Y~MSG*Tubal Ligation~III*ZZ*22~EB*A*IND*82*****.40***N*N~DTP*348*D8*20190101~MSG*Including Womens Specialist~MSG*This benefit does apply to member's out-of-pocket maximum~MSG*Including Womens PCP~III*ZZ*11~EB*A*FAM*86*****.10***N*N~DTP*348*D8*20190101~MSG*This benefit does apply to member's out-of-pocket maximum~III*ZZ*23~EB*C*FAM*86***23*1500.00****N*N~MSG*Benefit does apply to member's out-of-pocket maximum~MSG*Combined with In Network Plan Level~III*ZZ*23~EB*A*IND*86*****.10***N*N~DTP*348*D8*20190101~MSG*This benefit does apply to member's out-of-pocket maximum~III*ZZ*23~EB*A*FAM*73*****.10***N*N~DTP*348*D8*20190101~MSG*PET~MSG*This benefit does apply to member's out-of-pocket maximum~III*ZZ*23~EB*C*FAM*73***23*1500.00****N*N~MSG*PET~MSG*Benefit does apply to member's out-of-pocket maximum~MSG*Combined with In Network Plan Level~III*ZZ*23~EB*C*FAM*73***23*1500.00****Y*N~MSG*PET~MSG*Benefit does apply to member's out-of-pocket maximum~MSG*Combined with In Network Plan Level~III*ZZ*20~EB*A*FAM*73*****.10***Y*N~DTP*348*D8*20190101~MSG*PET~MSG*This benefit does apply to member's out-of-pocket maximum~III*ZZ*20~EB*A*IND*73*****.10***N*N~DTP*348*D8*20190101~MSG*PET~MSG*This benefit does apply to member's out-of-pocket maximum~III*ZZ*23~EB*A*IND*73*****.10***Y*N~DTP*348*D8*20190101~MSG*PET~MSG*This benefit does apply to member's out-of-pocket maximum~III*ZZ*20~EB*A*IND*98*****.10***N*Y~DTP*348*D8*20190101~MSG*Telehealth through contracted vendor~MSG*This benefit does apply to member's out-of-pocket maximum~III*ZZ*99~EB*A*IND*70*****.00***N*W~DTP*348*D8*20190101~EB*C*IND*70***23*0.00****N*W~EB*F*IND*70***26*10000.00****N*W~DTP*348*D8*20190101~MSG*Per Cause~EB*A*IND*70*****.00***Y*W~DTP*348*D8*20190101~EB*CB**70^74^A9^AF^75^56********Y*W~EB*C*IND*70***23*0.00****Y*W~EB*C*IND*BH***23*0.00****N*Y~MSG*Lab~MSG*Immunizations~MSG*Exam~III*ZZ*11~EB*A*IND*BH*****.00***N*Y~DTP*348*D8*20190101~MSG*Lab~MSG*Immunizations~MSG*Exam~III*ZZ*11~EB*C*IND*68^19***23*0.00****N*Y~III*ZZ*11~EB*A*IND*68^19*****.00***N*Y~DTP*348*D8*20190101~III*ZZ*11~EB*C*IND*4***23*0.00****N*Y~MSG*Preventive Mammogram~MSG*X-Ray~III*ZZ*22~EB*C*IND*4***23*0.00****N*Y~MSG*Preventive Mammogram~MSG*X-Ray~III*ZZ*11~EB*A*IND*4*****.00***N*Y~DTP*348*D8*20190101~MSG*Preventive Mammogram~MSG*X-Ray~III*ZZ*22~EB*A*IND*4*****.00***N*Y~DTP*348*D8*20190101~MSG*Preventive Mammogram~MSG*X-Ray~III*ZZ*11~EB*C*IND*5***23*0.00****N*Y~MSG*Gynecological~MSG*Obstetrical~III*ZZ*11~EB*A*IND*5*****.00***N*Y~DTP*348*D8*20190101~MSG*Gynecological~MSG*Obstetrical~III*ZZ*11~EB*F*IND*BG***23***VS*60*N*Y~MSG*Cardiac Rehabilitation~MSG*Combined Chiropractic and Occupational Therapy and Cognitive Therapy and Pulmonary Rehabilitation and Cardiac Rehabilitation~EB*F*IND*BG***23***VS*60*N*N~MSG*Cardiac Rehabilitation~MSG*Combined Chiropractic and Occupational Therapy and Cognitive Therapy and Pulmonary Rehabilitation and Cardiac Rehabilitation~EB*F*IND*33***23***VS*60*N*Y~MSG*Specialist~MSG*Combined Chiropractic and Occupational Therapy and Cognitive Therapy and Pulmonary Rehabilitation and Cardiac Rehabilitation~MSG*PCP~EB*F*IND*33***23***VS*60*N*N~MSG*Specialist~MSG*Combined Chiropractic and Occupational Therapy and Cognitive Therapy and Pulmonary Rehabilitation and Cardiac Rehabilitation~MSG*PCP~EB*F*IND*74***23***VS*70*Y*Y~EB*F*IND*74***23***VS*70*Y*N~EB*F*IND*74***23***VS*70*N*N~EB*F*IND*74***23***VS*70*N*Y~EB*A*FAM*75*****.10***N*N~DTP*348*D8*20190101~MSG*Including Wigs~MSG*This benefit does apply to member's out-of-pocket maximum~III*ZZ*12~EB*C*FAM*75***23*1500.00****N*N~MSG*Including Wigs~MSG*Benefit does apply to member's out-of-pocket maximum~MSG*Combined with In Network Plan Level~III*ZZ*12~EB*F*IND*75***23***VS*1*N*N~MSG*Including Wigs~EB*A*IND*75*****.10***N*N~DTP*348*D8*20190101~MSG*Including Wigs~MSG*This benefit does apply to member's out-of-pocket maximum~III*ZZ*12~EB*F*IND*75***23***VS*1*N*Y~MSG*Including Wigs~EB*A*IND*75*****.10***N*Y~DTP*348*D8*20190101~MSG*Including Mastectomy Bras~MSG*This benefit does apply to member's out-of-pocket maximum~III*ZZ*12~EB*A*FAM*75*****.40***N*N~DTP*348*D8*20190101~MSG*Including Mastectomy Bras~MSG*This benefit does apply to member's out-of-pocket maximum~III*ZZ*12~EB*A*IND*75*****.40***N*N~DTP*348*D8*20190101~MSG*Including Mastectomy Bras~MSG*This benefit does apply to member's out-of-pocket maximum~III*ZZ*12~EB*A*FAM*75*****.10***N*Y~DTP*348*D8*20190101~MSG*Including Mastectomy Bras~MSG*This benefit does apply to member's out-of-pocket maximum~III*ZZ*12~EB*F*IND*A9***23***DY*60*Y*Y~MSG*Semi Private Room~MSG*Private Room~EB*F*IND*A9***23***DY*60*Y*N~MSG*Semi Private Room~MSG*Private Room~EB*A*IND*67*****.00***N*Y~DTP*348*D8*20190101~MSG*Counseling~III*ZZ*11~EB*C*IND*67***23*0.00****N*Y~MSG*Counseling~III*ZZ*11~EB*A*IND*56*****.10***N*N~DTP*348*D8*20190101~MSG*This benefit does apply to member's out-of-pocket maximum~EB*C*FAM*56***23*1500.00****N*N~MSG*Benefit does apply to member's out-of-pocket maximum~MSG*Combined with In Network Plan Level~EB*A*FAM*56*****.10***N*N~DTP*348*D8*20190101~MSG*This benefit does apply to member's out-of-pocket maximum~EB*F*IND*BF***23***VS*60*N*N~MSG*Pulmonary Rehabilitation~MSG*Combined Chiropractic and Occupational Therapy and Cognitive Therapy and Pulmonary Rehabilitation and Cardiac Rehabilitation~EB*F*IND*BF***23***VS*60*N*Y~MSG*Pulmonary Rehabilitation~MSG*Combined Chiropractic and Occupational Therapy and Cognitive Therapy and Pulmonary Rehabilitation and Cardiac Rehabilitation~EB*F*IND*BD***23***VS*60*N*N~MSG*Combined Chiropractic and Occupational Therapy and Cognitive Therapy and Pulmonary Rehabilitation and Cardiac Rehabilitation~EB*F*IND*BD***23***VS*60*N*Y~MSG*Combined Chiropractic and Occupational Therapy and Cognitive Therapy and Pulmonary Rehabilitation and Cardiac Rehabilitation~EB*C*FAM*UC***23*1500.00****N*N~MSG*Illness~MSG*Benefit does apply to member's out-of-pocket maximum~MSG*Combined with In Network Plan Level~MSG*Injury~EB*A*FAM*UC*****.10***N*N~DTP*348*D8*20190101~MSG*Illness~MSG*This benefit does apply to member's out-of-pocket maximum~MSG*Injury~EB*A*IND*UC*****.10***N*N~DTP*348*D8*20190101~MSG*Injury~MSG*This benefit does apply to member's out-of-pocket maximum~MSG*Illness~EB*F*IND*AF***23***VS*60*Y*N~MSG*Speech Therapy~MSG*Combined Chiropractic and Occupational Therapy and Cognitive Therapy and Pulmonary Rehabilitation and Cardiac Rehabilitation~EB*F*IND*AF***23***VS*60*Y*Y~MSG*Speech Therapy~MSG*Combined Chiropractic and Occupational Therapy and Cognitive Therapy and Pulmonary Rehabilitation and Cardiac Rehabilitation~EB*F*IND*AD***23***VS*60*N*Y~MSG*Occupational Therapy~MSG*Combined Chiropractic and Occupational Therapy and Cognitive Therapy and Pulmonary Rehabilitation and Cardiac Rehabilitation~EB*F*IND*AD***23***VS*60*N*N~MSG*Occupational Therapy~MSG*Combined Chiropractic and Occupational Therapy and Cognitive Therapy and Pulmonary Rehabilitation and Cardiac Rehabilitation~EB*1**MH~DTP*356*D8*20180101~EB*1**35*HM*Dental HMO~DTP*356*D8*20190101~EB*C*FAM*30***29*0.00*****Y~MSG*Amounts apply to both in-network and out-of-network~MSG*Benefit does apply to member's out-of-pocket maximum~MSG*Accumulators are shared between Medical AND Mental Health~EB*G*FAM*30***29*2351.02*****W~MSG*Accumulators are shared between Medical AND Mental Health~EB*C*FAM*30***29*1500.00*****N~MSG*Benefit does apply to member's out-of-pocket maximum~MSG*Amounts apply to both in-network and out-of-network~MSG*Accumulators are shared between Medical AND Mental Health~EB*F*IND*BG***29***VS*60**Y~MSG*Cardiac Rehabilitation~MSG*Combined Chiropractic and Occupational Therapy and Cognitive Therapy and Pulmonary Rehabilitation and Cardiac Rehabilitation~EB*F*IND*BG***29***VS*60**N~MSG*Cardiac Rehabilitation~MSG*Combined Chiropractic and Occupational Therapy and Cognitive Therapy and Pulmonary Rehabilitation and Cardiac Rehabilitation~EB*F*IND*33***29***VS*60**Y~MSG*Specialist~MSG*Combined Chiropractic and Occupational Therapy and Cognitive Therapy and Pulmonary Rehabilitation and Cardiac Rehabilitation~MSG*PCP~EB*F*IND*33***29***VS*60**N~MSG*Specialist~MSG*Combined Chiropractic and Occupational Therapy and Cognitive Therapy and Pulmonary Rehabilitation and Cardiac Rehabilitation~MSG*PCP~EB*F*IND*74***29***VS*70**Y~EB*F*IND*74***29***VS*70**N~EB*F*IND*75***29***VS*1**N~MSG*Including Wigs~EB*F*IND*75***29***VS*1**Y~MSG*Including Wigs~EB*F*IND*A9***29***DY*60**Y~MSG*Semi Private Room~MSG*Private Room~EB*F*IND*A9***29***DY*60**N~MSG*Semi Private Room~MSG*Private Room~EB*F*IND*BF***29***VS*60**N~MSG*Pulmonary Rehabilitation~MSG*Combined Chiropractic and Occupational Therapy and Cognitive Therapy and Pulmonary Rehabilitation and Cardiac Rehabilitation~EB*F*IND*BF***29***VS*60**Y~MSG*Pulmonary Rehabilitation~MSG*Combined Chiropractic and Occupational Therapy and Cognitive Therapy and Pulmonary Rehabilitation and Cardiac Rehabilitation~EB*F*IND*BD***29***VS*60**N~MSG*Combined Chiropractic and Occupational Therapy and Cognitive Therapy and Pulmonary Rehabilitation and Cardiac Rehabilitation~EB*F*IND*BD***29***VS*60**Y~MSG*Combined Chiropractic and Occupational Therapy and Cognitive Therapy and Pulmonary Rehabilitation and Cardiac Rehabilitation~EB*F*IND*AF***29***VS*60**N~MSG*Speech Therapy~MSG*Combined Chiropractic and Occupational Therapy and Cognitive Therapy and Pulmonary Rehabilitation and Cardiac Rehabilitation~EB*F*IND*AF***29***VS*60**Y~MSG*Speech Therapy~MSG*Combined Chiropractic and Occupational Therapy and Cognitive Therapy and Pulmonary Rehabilitation and Cardiac Rehabilitation~EB*F*IND*AD***29***VS*60**Y~MSG*Occupational Therapy~MSG*Combined Chiropractic and Occupational Therapy and Cognitive Therapy and Pulmonary Rehabilitation and Cardiac Rehabilitation~EB*F*IND*AD***29***VS*60**N~MSG*Occupational Therapy~MSG*Combined Chiropractic and Occupational Therapy and Cognitive Therapy and Pulmonary Rehabilitation and Cardiac Rehabilitation~EB*U**88**CVS~LS*2120~NM1*PR*2*CVS~PER*IC**TE*8555591386~LE*2120~SE*455*000000001~GE*1*1~IEA*1*252111745~";
                // 

                _context.PatientEligibility.Add(patElig);

                patElig.Status = "P";
                //_context.PatientEligibility.Update(patElig);

                string filePath270 = string.Empty, filePath271 = string.Empty, filePath999 = string.Empty;

                filePath270 = Path.Combine(directoryPath, "Request270.txt");
                filePath271 = Path.Combine(directoryPath, "Response271.txt");
                filePath999 = Path.Combine(directoryPath, "Failed999.txt");

                System.IO.File.WriteAllText(filePath270, transation270);
                if (transaction271.Contains("271"))
                    System.IO.File.WriteAllText(filePath271, transaction271);
                else if(transaction271.Contains("999"))
                    System.IO.File.WriteAllText(filePath999, transaction271);

                _271Parser objParser = new _271Parser();
                List<_271Header> Header271Data = objParser.Parse271File(filePath271);

                foreach (_271Header H in Header271Data)
                {
                    patElig.PayerName = H.PayerOrgName;
                    patElig.PayerID = H.PayerID;

                    foreach (_271Subscriber SBR in H.ListOfSubscriberData)
                    {
                        patElig.SubscriberAddress = patElig.SubscriberAddress.GetValueIfNotNull(SBR.SBRAddress);
                        patElig.SubscriberCity = patElig.SubscriberCity.GetValueIfNotNull(SBR.SBRCity);
                        patElig.SubscriberState = patElig.SubscriberState.GetValueIfNotNull(SBR.SBRState);
                        patElig.SubscriberZip = patElig.SubscriberZip.GetValueIfNotNull(SBR.SBRZipCode);
                        //patElig.SubscriberDOB = patElig.SubscriberDOB
                        patElig.SubscriberLN = patElig.SubscriberLN.GetValueIfNotNull(SBR.SBRLastName);
                        patElig.SubscriberFN = patElig.SubscriberFN.GetValueIfNotNull(SBR.SBRFirstName);
                        patElig.SubscriberMI = patElig.SubscriberMI.GetValueIfNotNull(SBR.SBRMiddleInitial);
                        patElig.SubscriberID = patElig.SubscriberID.GetValueIfNotNull(SBR.SBRID);
                        patElig.SubscriberGender = patElig.SubscriberGender.GetValueIfNotNull(SBR.SBRGender);

                        patElig.RejectionCode = SBR.AAA03;
                        patElig.Rejection = SBR.AAAErrorMsg;
                        patElig.ErrorMessage = SBR.AAAErrorMsg;

                        foreach (_271SBREligibilityInfo info in SBR.EligibilityData)
                        {
                            PatientEligibilityDetail detail = new PatientEligibilityDetail()
                            {
                                AddedBy = "MEMON",
                                AddedDate = System.DateTime.Now,
                                PatientEligibilityID = patElig.ID,
                                Authorization = info.EB11AuthorizationIndicatorV,
                                BenefitPercentage = info.EB08BenefitPercent,
                                Coverage = info.EB01CoverageTypeV,
                                CoverageLevel = info.EB02CoverageLevelV,
                                PlanDescription = info.EB05PlanCoverageDesc,
                                PlanName = "",
                                BenefitAmount = 0,
                                PlanNetwork = info.EB12PlanNetworkIndicatorV,
                                TimePeriod = info.EB06TimePeriodV,
                                ServiceTypes = info.EB03ServiceTypeCodeV,
                                Messages = string.Join(";;" , info.Messages)
                        };

                            int counter = 1;
                            #region Dates
                            foreach (KeyValuePair<string, DateTime> kvp in info.ListOfDates)
                            {
                                if (counter == 1)
                                {
                                    detail.DateId1 = kvp.Key;
                                    detail.DateValue1 = kvp.Value.Format();
                                }
                                else if (counter == 2)
                                {
                                    detail.DateId2 = kvp.Key;
                                    detail.DateValue2 = kvp.Value.Format();
                                }
                                else if (counter == 3)
                                {
                                    detail.DateId3 = kvp.Key;
                                    detail.DateValue3 = kvp.Value.Format();
                                }
                                else if (counter == 4)
                                {
                                    detail.DateId4 = kvp.Key;
                                    detail.DateValue4 = kvp.Value.Format();
                                }
                                else if (counter == 5)
                                {
                                    detail.DateId5 = kvp.Key;
                                    detail.DateValue5 = kvp.Value.Format();
                                }
                                counter += 1;
                            }
                            #endregion

                            counter = 1;
                            #region Reference Ids
                            foreach (KeyValuePair<string, string> kvp in info.ListOfReferenceIds)
                            {
                                if (counter == 1)
                                {
                                    detail.ReferenceId1 = kvp.Key;
                                    detail.ReferenceValue1 = kvp.Value;
                                }
                                else if (counter == 2)
                                {
                                    detail.ReferenceId2 = kvp.Key;
                                    detail.ReferenceValue2 = kvp.Value;
                                }
                                else if (counter == 3)
                                {
                                    detail.ReferenceId3 = kvp.Key;
                                    detail.ReferenceValue3 = kvp.Value;
                                }
                                else if (counter == 4)
                                {
                                    detail.ReferenceId4 = kvp.Key;
                                    detail.ReferenceValue4 = kvp.Value;
                                }
                                else if (counter == 5)
                                {
                                    detail.ReferenceId5 = kvp.Key;
                                    detail.ReferenceValue5 = kvp.Value;
                                }
                                counter += 1;
                            }
                            #endregion

                            _context.PatientEligibilityDetail.Add(detail);
                        }
                    }
                    
                }


                PatientEligibilityLog log = new PatientEligibilityLog()
                {
                    AddedBy = "MEMON",
                    AddedDate = System.DateTime.Now,
                    PatientEligibilityID = patElig.ID,
                    Transaction270Path = filePath270,
                    Transaction271Path = filePath271,
                    Transaction999Path = filePath999
                };
                _context.PatientEligibilityLog.Add(log);

                await _context.SaveChangesAsync();
                objTrnScope.Complete();
                objTrnScope.Dispose();
            }


            VMPatientEligibility vmObj = new VMPatientEligibility();
            vmObj.GetEligibilityDetail(_context, patElig.ID);
          
            return vmObj;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Patient ID</param>
        /// <returns></returns>
        [Route("FindPatientEligibilityRecords/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<GPatientEligibility>>> FindPatientEligibilityRecords(long id)
        {
            
            return await (from pElig in _context.PatientEligibility
                          join p in _context.Patient
                          on pElig.PatientID equals p.ID
                          join pp in _context.PatientPlan
                          on p.ID equals pp.PatientID
                          join iPlan in _context.InsurancePlan
                          on pp.InsurancePlanID equals iPlan.ID
                          join prov in _context.Provider
                          on p.ProviderID equals prov.ID
                          where p.ID == id
                          select new GPatientEligibility()
                          {
                              ID = pp.ID,
                              PatientID = p.ID,
                              Patient = p.LastName + ", " + p.FirstName,
                              DOS = pElig.DOS.Format("MM/dd/yyyy"),
                              EligibilityDate = pElig.EligibilityDate.Format("MM/dd/yyyy"),
                              GroupNumber = pElig.SubscriberGroupNumber,
                              Payer = pElig.PayerName,
                              Plan = iPlan.PlanName,
                              Provider = prov.Name,
                              ProviderID = prov.ID,
                              Remarks = pElig.Rejection,
                              Status = pElig.Status,
                              SubscriberID = pElig.SubscriberID
                          }).ToListAsync();
        }



        [Route("FindPatientEligibilityDetail/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<VMPatientEligibility>> FindPatientEligibilityDetail(long id)
        {
            VMPatientEligibility vmObj = new VMPatientEligibility();
            vmObj.GetEligibilityDetail(_context, id);
            return vmObj;
        }

        [Route("DeletePatientEligibility/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeletePatientEligibility(long id)
        {
            var patientElig = await _context.PatientEligibility.FindAsync(id);

            if (patientElig == null)
            {
                return NotFound();
            }
            _context.PatientEligibility.Remove(patientElig);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
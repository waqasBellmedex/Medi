//using iText.Html2pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
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
using MediFusionPM.Models;
using MediFusionPM.Models.TodoApi;
using MediFusionPM.Models.TodoApi.Models;
using MediFusionPM.ViewModels;
using static MediFusionPM.ViewModels.VMCommon;
using static MediFusionPM.ViewModels.VMPlanFollowUp;
using static MediFusionPM.ViewModels.VMPatientPayment;
using MediFusionPM.Models.Audit;
using System.Xml;
using System.Xml.Xsl;
using MediFusionPM.ViewModel;
using iText.Html2pdf;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class PlanFollowupController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;

        public PlanFollowupController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;


        }

        [Route("GetEOB/{PaymentVisitID}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentCheck>> GetEOB(long PaymentVisitID)
        {
            PaymentVisit paymentVisit = await _context.PaymentVisit.FindAsync(PaymentVisitID);
            if (paymentVisit == null)
            {
                return NotFound();
            }

            var PaymentCheck = await _context.PaymentCheck.FindAsync(paymentVisit.PaymentCheckID);
            if (PaymentCheck == null)
            {
                return NotFound();
            }
            else
            {
                PaymentCheck.PaymentVisit = _context.PaymentVisit.Where(m => m.ID == PaymentVisitID).ToList<PaymentVisit>();
                foreach (PaymentVisit payment in PaymentCheck.PaymentVisit)
                {
                    payment.PaymentCharge = _context.PaymentCharge.Where(m => m.PaymentVisitID == payment.ID).ToList<PaymentCharge>();
                }
            }
            return PaymentCheck;
        }

        [Route("GetProfiles/{PlanFollowUpID}")]
        [HttpGet("{PlanFollowUpID}")]
        public async Task<ActionResult<VMPlanFollowUp>> GetProfiles(long id, long PlanFollowUpID)
        {
            ViewModels.VMPlanFollowUp obj = new ViewModels.VMPlanFollowUp();
            obj.GetProfiles(_context, PlanFollowUpID);

            return obj;
        }
        [Route("FindPlanFollowUp/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<PlanFollowup>> FindPlanFollowUp(long id)
        {
            var PlanFollowUp = await _context.PlanFollowUp.FindAsync(id);
            if (PlanFollowUp == null)
            {
                return NotFound();
            }
            //NotMapped Column Age use to Display Age of followup
            PlanFollowUp.Age = System.DateTime.Now.Subtract(PlanFollowUp.AddedDate.Date()).Days.ToString();

            // GFollowupCharge Notmapped List used to return PlanFollowUpCharge Values
            List<GFollowupCharge> Charges = (from pfc in _context.PlanFollowupCharge
                                             join c in _context.Charge on pfc.ChargeID equals c.ID
                                             join cpt in _context.Cpt on c.CPTID equals cpt.ID
                                             join pPlan in _context.PatientPlan on c.PrimaryPatientPlanID equals pPlan.ID
                                             join pChrg in _context.PaymentCharge on pfc.PaymentChargeID equals pChrg.ID into pc from paymentCharge in pc.DefaultIfEmpty()
                                             join ad in _context.AdjustmentCode on pfc.AdjustmentCodeID equals ad.ID into Table1
                                             from t1 in Table1.DefaultIfEmpty()
                                             where pfc.PlanFollowupID == id &&
                                             (c.PrimaryBal.Val() + c.SecondaryBal.Val() + c.TertiaryBal.Val()) > 0
                                             select new GFollowupCharge
                                             {
                                                 ChargeID = pfc.ChargeID,
                                                 DOS = c.DateOfServiceFrom.Format("MM/dd/yyyy"),
                                                 CPT = cpt.CPTCode,
                                                 SubmitDate = c.SubmittetdDate.Format("MM/dd/yyyy"),
                                                 PlanBalance = c.PrimaryBal.Val() + c.SecondaryBal.Val() + c.TertiaryBal.Val(),
                                                 AdjustmentCodeID = pfc.AdjustmentCodeID,
                                                 AdjustmentCode = t1.Code,
                                                 BilledAmount = c.PrimaryBilledAmount.Val(),
                                                 Coverage = pPlan.Coverage,
                                                 RemarkCode = paymentCharge.RemarkCodeID1,
                                             }).ToList<GFollowupCharge>();
            PlanFollowUp.GFollowupCharge = Charges;

            //PlanFollowUp.PlanFollowupCharge = _context.PlanFollowupCharge.Where(m => m.PlanFollowupID == id).ToList<PlanFollowupCharge>();
            PlanFollowUp.Note = _context.Notes.Where(m => m.PlanFollowupID == id).ToList<Notes>();

            return PlanFollowUp;
        }

        [Route("FindPlanFollowUpByFollowUpID/{ID}")]
        [HttpGet("{ID}")]
        public List<PatientPaymentGrid> FindPatientPaymentCharges(long ID)
        {
            List<PatientPaymentGrid> data = ( from pf in _context.PlanFollowUp
                                              join  v in _context.Visit on pf.VisitID equals v.ID
                                             join c in _context.Charge on v.ID equals c.VisitID
                                             join cpt in _context.Cpt on c.CPTID equals cpt.ID
                                             join pPlan in _context.PatientPlan on v.PrimaryPatientPlanID equals pPlan.ID
                                             join iPlan in _context.InsurancePlan on pPlan.InsurancePlanID equals iPlan.ID
                                             where pf.ID == ID
                                              select new PatientPaymentGrid()
                                             {
                                                 ID = pf.ID,
                                                 VisitID = v.ID,
                                                 ChargeID = c.ID,
                                                 DOS = c.DateOfServiceFrom.Format("MM/dd/yyyy"),
                                                 SubmitDate = c.SubmittetdDate.Format("MM/dd/yyyy"),
                                                 Plan = iPlan.PlanName,
                                                 CPT = cpt.CPTCode,
                                                 BilledAmount = c.PrimaryBilledAmount,
                                                 WriteOff = c.PrimaryWriteOff,
                                                 AllowedAmount = c.PrimaryAllowed,
                                                 PaidAmount = c.PrimaryPaid,
                                                 Copay = c.Copay,
                                                 CoInsurance = c.Coinsurance,
                                                 Deductible = c.Deductible,
                                                 PatientBalance = c.PrimaryPatientBal,
                                                 AllocatedAmount = null,
                                                 Status = null
                                             }).ToList<PatientPaymentGrid>();
            return data;

        }


        [HttpPost]
        [Route("FindPlanFollowUp")]
        public async Task<ActionResult<IEnumerable<GPlanFollowup>>> FindPlanFollowUp(CPlanFollowup CPlanFollowup)
        {

            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (UD == null || UD.Rights == null || UD.Rights.planFollowupSearch == false)
            return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            return FindPlanFollowUp(CPlanFollowup, UD);
        }
        [HttpPost]
        [Route("PatientFollowUpVisits")]
        public ActionResult PatientFollowUpVisits(ListModel patientFollowUpIds)
        {
           UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.patientStatements == false)
            //    return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            if (patientFollowUpIds == null || patientFollowUpIds.Ids.Length == 0)
            {
                return BadRequest("Please select Patient.");
            }
            var data = (from p in _context.PatientFollowUpCharge
                                                 join pf in _context.PatientFollowUp on p.PatientFollowUpID equals pf.ID
                                                 join pCharge in _context.PaymentCharge on p.PaymentChargeID equals pCharge.ID
                                                 join pVisit in _context.PaymentVisit on pCharge.PaymentVisitID equals pVisit.ID
                                                 join v in _context.Visit on pVisit.VisitID equals v.ID
                                                 join pat in _context.Patient on v.PatientID equals pat.ID
                                                 join c in _context.Charge on v.ID equals c.VisitID
                                                 join pPlan in _context.PatientPlan on v.PrimaryPatientPlanID equals pPlan.ID
                                                 join iPlan in _context.InsurancePlan on pPlan.InsurancePlanID equals iPlan.ID
                                                 join cpt in _context.Cpt on c.CPTID equals cpt.ID
                                                 where patientFollowUpIds.Ids.Contains(pf.ID)    && p.ChargeID == c.ID
                                                 select new
                                                 {
                                                     id= pf.ID,
                                                     PatientFollowUpID = pf.ID,
                                                     PatientID = pf.PatientID,
                                                     Patient  = pat.LastName+", "+pat.FirstName ,
                                                     VisitID = pVisit.VisitID,
                                                     Plan = iPlan.PlanName,
                                                     DOS = v.DateOfServiceFrom.Format("MM/dd/yyyy"),
                                                     BilledAmount = v.PrimaryBilledAmount,
                                                     PaidAmount = v.PrimaryPaid,
                                                     AllowedAmount = v.PrimaryAllowed,
                                                     Copay = v.Copay,
                                                     Deductible = v.Deductible,
                                                     CoInsurance = v.Coinsurance,
                                                     PatientPaid = v.PatientPaid,
                                                     PatientAmount = (v.PrimaryPatientBal.Val()
                                  + v.SecondaryPatientBal.Val()
                                  + v.TertiaryPatientBal.Val()),
                                                     AddedDate = p.AddedDate.Format("MM/dd/yyyy"),
                                                     AddedBy = p.AddedBy,
                                                 }).Distinct().ToList();
            Practice practice = _context.Practice.Find(UD.PracticeID);
            string reportMsg = practice.StatementMessage??"";
            string reportType = practice.StatementExportType??"";
            var ret = new { data = data, reportMessage = reportMsg, statementType = reportType };
            return Json(ret);
        }
        [Route("DownloadStatementFile/{patStatementID}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadStatementFile(long patStatementID)
        {
            PatientStatement patStatement = await _context.PatientStatement.FindAsync(patStatementID);
            if (patStatement != null)
            {
                if (!System.IO.File.Exists(patStatement.pdf_url))
                {
                    return NotFound();
                }
                Byte[] fileBytes = System.IO.File.ReadAllBytes(patStatement.pdf_url);
                if (fileBytes == null)
                    return NotFound();
                var stream = new MemoryStream(fileBytes); //saves it into a stream
                stream.Position = 0;
                int index=patStatement.pdf_url.LastIndexOf("\\");
                string filename = patStatement.pdf_url.Substring(index + 1, patStatement.pdf_url.Length - index - 1);
                return File(stream, "application/octec-stream", filename);
            }
            return NotFound();
        }
        [Route("DownloadPLDFile/{patStatementID}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadPLDFile(long patStatementID)
        {
            PatientStatement patStatement = await _context.PatientStatement.FindAsync(patStatementID);
            if (patStatement != null)
            {
                if (!System.IO.File.Exists(patStatement.csv_url))
                {
                    return NotFound();
                }
                Byte[] fileBytes = System.IO.File.ReadAllBytes(patStatement.csv_url);
                if (fileBytes == null)
                    return NotFound();
                var stream = new MemoryStream(fileBytes); //saves it into a stream
                stream.Position = 0;
                int index = patStatement.csv_url.LastIndexOf("\\");
                string filename = patStatement.csv_url.Substring(index + 1, patStatement.csv_url.Length - index - 1);
                return File(stream, "application/octec-stream", filename);
            }
            return NotFound();
        }
        [HttpPost]
        [Route("GeneratePatientStatement")]
        public ActionResult GeneratePatientStatement(ListModel claimId, string patientId, string reportType,string statementMessage)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.patientStatements == false)
            //    return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            if (patientId == null || patientId.Length == 0)
            {
                return BadRequest("Please select Patient(s).");
            }
            if (claimId == null || claimId.Ids == null || claimId.Ids.Length == 0)
            {
                return BadRequest("Please select Visit(s).");
            }
            Settings settings = _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefault();
            string resourcesPath = System.IO.Path.Combine(_context.env.ContentRootPath, "Resources/statement/");
            string DirectoryPath = System.IO.Path.Combine("\\\\", settings.DocumentServerURL,
                        settings.DocumentServerDirectory, UD.ClientID.ToString(), "PatientStatement");
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }

            var datetime = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

            //System.IO.FileStream fs = new FileStream(FileFullname, FileMode.Create);
            string PDFfileName="", HTMLfileName = "", CSVfileName = "" ;
            List<VMPatientStatementPLDCSV> lstVMPatientStatementPLDCSV = new List<VMPatientStatementPLDCSV>();
            VMPatientStatementPLDCSV vmPatientStatementPLDCSV = null;
            string[] arrPatientIds = patientId.Split(",");
            List<long> PatientVisitsIds = new List<long>();
            List<VMPatientStatementFile> lstFiles = new List<VMPatientStatementFile>();
            Practice practice = _context.Practice.Find(UD.PracticeID);
            List<long> PatientIdsConsumed = new List<long>();
            PatientStatement patStatement = null;
            CSVfileName = System.IO.Path.Combine(DirectoryPath , string.Join(",", claimId.Ids) + "_" + datetime + ".csv");

            for (int i = 0; i < arrPatientIds.Length; i++)
            {
                if (!PatientIdsConsumed.Contains(long.Parse(arrPatientIds[i])))
                    PatientIdsConsumed.Add(long.Parse(arrPatientIds[i]));
                else
                    continue; // patientID duplicate
                long patId = long.Parse(arrPatientIds[i]);
                var header = (from p in _context.Practice
                              join pt in _context.Patient on p.ID equals pt.PracticeID
                              where p.ID == UD.PracticeID && pt.ID == patId
                              select new
                              {
                                  practiceName = p.Name,
                                  practiceAddress = p.Address1,
                                  practiceCity = p.City,
                                  practiceState = p.State,
                                  practiceZipCode = p.ZipCode,
                                  patientID = pt.ID,
                                  patientAccountNum = pt.AccountNum,
                                  patient = pt.LastName+", " + pt.FirstName,
                                  patientAddress = pt.Address1,
                                  patientAddress2 = pt.Address2,
                                  patientCity = pt.City,
                                  patientState = pt.State,
                                  patientZipCode = pt.ZipCode,
                              }
                            ).ToList();
                string data = "";
                decimal? total = 0;
                if (reportType.ToLower() == "details")
                {
                    var details = (from v in _context.Visit
                               join vC in _context.Charge on v.ID equals vC.VisitID
                               join cpt in _context.Cpt on vC.CPTID equals cpt.ID
                               where v.PracticeID == UD.PracticeID && v.PatientID == patId
                               && claimId.Ids.Contains(v.ID)
                               select new
                               {
                                   DateOfServiceFrom = vC.DateOfServiceFrom.ToString("MM/dd/yyyy"),
                                   DateOfServiceTo = vC.DateOfServiceTo.HasValue ? vC.DateOfServiceTo.Value.ToString("MM/dd/yyyy") : "",
                                   cpt = cpt.CPTCode,
                                   visitID = v.ID,
                                   description = cpt.ShortDescription,
                                   chargeID = vC.ID,
                                   totalAmount = vC.TotalAmount, 
                                   PaidAmount = (vC.PrimaryPaid.Val()  + vC.SecondaryPaid.Val() + vC.TertiaryPaid.Val()),
                                   units = vC.Units,
                                   PatientPaid = vC.PatientPaid,
                                   patientBalance = (vC.PrimaryPatientBal.Val()   
                                  +  vC.SecondaryPatientBal.Val()  
                                  +  vC.TertiaryPatientBal.Val() )
                               }
                                        ).ToList();
                        if (details != null)
                    {
                        PatientVisitsIds.Clear();
                        for (int d = 0; d < details.Count; d++)
                        {
                            if (!PatientVisitsIds.Contains(details[d].visitID))
                                PatientVisitsIds.Add(details[d].visitID);
                        }
                        PDFfileName = System.IO.Path.Combine(DirectoryPath  , arrPatientIds[i] + "-" + string.Join(",", PatientVisitsIds) + "_" + datetime + ".pdf");
                        HTMLfileName = System.IO.Path.Combine(DirectoryPath  , arrPatientIds[i] + "-" + string.Join(",", PatientVisitsIds) + "_" + datetime + ".html");
                        //Practice practice = _context.Practice.Find(UD.PracticeID);
                        for (int c = 0; c < PatientVisitsIds.Count; c++)
                        {
                            Visit visit = _context.Visit.Find(PatientVisitsIds[c]);
                            visit.StatementStatus = visit.StatementStatus==null ?  0: (visit.StatementStatus+1);
                            visit.LastStatementDate = DateTime.Now;
                            //PatientFollowUpController patientFollowUpC = new PatientFollowUpController(_context, _contextMain);
                            //PatientFollowUpC.SavePatientFollowUp(PatientFollowUp);
                            patStatement= SavePatientStatementLog(  visit,   PDFfileName,   CSVfileName, statementMessage,   reportType, UD);
                            var detailsFiltered = details.FindAll(e => e.visitID == visit.ID);
                            for (int k = 0; k < detailsFiltered.Count; k++)
                            { 
                                PatientStatementChargeHistory patStatementChargeHistory = new PatientStatementChargeHistory();
                                patStatementChargeHistory.PatientStatementID = patStatement.ID;
                                patStatementChargeHistory.ChargeID = detailsFiltered[k].chargeID;
                                patStatementChargeHistory.AddedBy = UD.Email;
                                patStatementChargeHistory.AddedDate = DateTime.Now;
                                patStatementChargeHistory.UpdatedBy = UD.Email;
                                patStatementChargeHistory.UpdatedDate = DateTime.Now;
                                _context.PatientStatementChargeHistory.Add(patStatementChargeHistory);
                                if (practice.StatementExportType == "PLD")
                                {   
                                    vmPatientStatementPLDCSV = new VMPatientStatementPLDCSV();
                                    vmPatientStatementPLDCSV.StatementNo = patStatement.ID.ToString();
                                    vmPatientStatementPLDCSV.BillingStatement = "1";
                                    vmPatientStatementPLDCSV.OfficeName = practice.OrganizationName;
                                    vmPatientStatementPLDCSV.Address = practice.Address1;
                                    vmPatientStatementPLDCSV.City = practice.City;
                                    vmPatientStatementPLDCSV.State = practice.State;
                                    vmPatientStatementPLDCSV.ZipCode = practice.ZipCode;
                                    vmPatientStatementPLDCSV.Phone = practice.OfficePhoneNum;
                                    vmPatientStatementPLDCSV.StatementDate = DateTime.Now.ToString("MM/dd/yyyy");
                                    vmPatientStatementPLDCSV.PatientName = header[0].patient;
                                    vmPatientStatementPLDCSV.AccountNo = header[0].patientAccountNum;
                                    vmPatientStatementPLDCSV.PatientAddress1 = header[0].patientAddress;
                                    vmPatientStatementPLDCSV.PatientAddress2 = header[0].patientAddress2;
                                    vmPatientStatementPLDCSV.BillToName = header[0].patient;
                                    vmPatientStatementPLDCSV.BillToAddress1 = header[0].patientAddress;
                                    vmPatientStatementPLDCSV.BillToAddress2 = header[0].patientAddress2;
                                    vmPatientStatementPLDCSV.VisitID = visit.ID.ToString();
                                    vmPatientStatementPLDCSV.DateOfService = visit.DateOfServiceFrom.Value.ToString("MM/dd/yyyy");
                                    vmPatientStatementPLDCSV.CPT = detailsFiltered[k].cpt;
                                    vmPatientStatementPLDCSV.Procedure = detailsFiltered[k].description;
                                    vmPatientStatementPLDCSV.Quantity = detailsFiltered[k].units;
                                    vmPatientStatementPLDCSV.Charge = detailsFiltered[k].totalAmount.ToString();
                                    vmPatientStatementPLDCSV.InsurancePayment = "";
                                    vmPatientStatementPLDCSV.PatientPayment = "";
                                    vmPatientStatementPLDCSV.Adjustment = "";
                                    vmPatientStatementPLDCSV.CurrentDue = "";
                                    vmPatientStatementPLDCSV.Over30Days = "";
                                    vmPatientStatementPLDCSV.Over60Days = "";
                                    vmPatientStatementPLDCSV.Over90Days = "";
                                    vmPatientStatementPLDCSV.Balance = "";
                                    vmPatientStatementPLDCSV.TotalBalance = "";
                                    vmPatientStatementPLDCSV.Reference = "";
                                    vmPatientStatementPLDCSV.emailAddress = "";
                                    vmPatientStatementPLDCSV.Comments = "";
                                    lstVMPatientStatementPLDCSV.Add(vmPatientStatementPLDCSV);


                                }
                            }
                        }
                        data = @" <table style='width:95%; margin-top: 10px;border:1px solid #000;border-collapse: collapse;'>
                <thead>
                    <tr>
                        <th  style='background-color: #adadad;border:1px solid #000;border-collapse: collapse;width:30px;'>
                            DATE
                        </th>
                         <th style='background-color: #adadad;border:1px solid #000;border-collapse: collapse;width:66px'>
                            CODE
                        </th>
                        <th style='background-color: #adadad;border:1px solid #000;border-collapse: collapse;padding:8px; '>
                            DESCRIPTION
                        </th>
                        <th colspan='2' style='background-color: #adadad;border:1px solid #000;border-collapse: collapse;padding: 8px; width: 93px;'>
                            AMOUNT 
                        </th>
                         
                    </tr>
                </thead>
                <tbody>";
                        string dos = "";
                        total = 0;
                        for (int k = 0; k < details.Count; k++)
                        {
                            dos = details[k].DateOfServiceFrom;
                            total  = details[k].patientBalance+ total;
                            if (!ExtensionMethods.IsNull(details[k].DateOfServiceTo) &&
                                details[k].DateOfServiceTo != details[k].DateOfServiceFrom)
                                dos += "-" + details[k].DateOfServiceTo;
                            data += @"<tr>
                        <td class='tableDetails'  style='width:30px; border:1px solid #000; border-collapse: collapse; padding: 8px; font-size: 16px; '>
                            " + dos + @"
                        </td>
                        <td  class='tableDetails'  style='width:66px ;border:1px solid #000; border-collapse: collapse; padding: 8px; font-size: 16px; '>
                            " + details[k].cpt + @"
                        </td>
                        <td  class='tableDetails' >
                            " + details[k].description + @"
                        </td>
                        <td  class='tableDetails'  style='width:93px ;border:1px solid #000; border-collapse: collapse; padding: 8px; font-size: 16px;' >
                               " + details[k].patientBalance  + @"
                        </td> 
                    </tr>";
                        }
                        data += @" </tbody></table>";
                    }
                }
                else if (reportType.ToLower() == "summary")
                {
                    var details = (from p in _context.Practice
                                   join pt in _context.Patient on p.ID equals pt.PracticeID
                                   join v in _context.Visit on pt.ID equals v.PatientID
                                   where p.ID == UD.PracticeID && pt.ID == patId
                                   && claimId.Ids.Contains(v.ID)
                                   select new
                                   {
                                       DateOfServiceFrom = v.DateOfServiceFrom.Value.ToString("MM/dd/yyyy"),
                                       DateOfServiceTo = v.DateOfServiceTo.HasValue ? v.DateOfServiceTo.Value.ToString("MM/dd/yyyy") : "",
                                       v.TotalAmount,
                                       visitID= v.ID,
                                       allowedAmount = v.PrimaryAllowed,
                                       v.PrimaryPaid,
                                       v.SecondaryPaid,
                                       v.PatientPaid,
                                       patientBalance = v.PrimaryPatientBal.Val()
                                      + v.SecondaryPatientBal.Val()
                                      + v.TertiaryPatientBal.Val()
                                   }
                                           ).ToList();
                    if (details != null)
                    {
                        PatientVisitsIds.Clear();
                        for (int d = 0; d < details.Count; d++)
                        {
                            if (!PatientVisitsIds.Contains(details[d].visitID))
                                PatientVisitsIds.Add(details[d].visitID);
                        }
                        PDFfileName = System.IO.Path.Combine(DirectoryPath , arrPatientIds[i] + "-" + string.Join(",", PatientVisitsIds) + "_" + datetime + ".pdf");
                        HTMLfileName = System.IO.Path.Combine(DirectoryPath  , arrPatientIds[i] + "-" + string.Join(",", PatientVisitsIds) + "_" + datetime + ".html");
                        data = @" <table style='width:95%; margin-top: 10px;border:1px solid #000;border-collapse: collapse;'>
                <thead>
                    <tr>
                        <th  style='background-color: #adadad;border:1px solid #000;border-collapse: collapse;width:30px;'>
                            DATE
                        </th> 
                        <th colspan='2' style='background-color: #adadad;border:1px solid #000;border-collapse: collapse;padding: 8px; width: 93px;'>
                            AMOUNT 
                        </th>
                         
                    </tr>
                </thead>
                <tbody>";
                        string dos = "";
                        total = 0;
                        for (int k = 0; k < details.Count; k++)
                        {
                            dos = details[k].DateOfServiceFrom;
                            total += details[k].patientBalance.Val();
                            if (!ExtensionMethods.IsNull(details[k].DateOfServiceTo) &&
                                details[k].DateOfServiceTo != details[k].DateOfServiceFrom)
                                dos += "-" + details[k].DateOfServiceTo;
                            Visit visit = _context.Visit.Find(details[k].visitID);
                            visit.StatementStatus = visit.StatementStatus == null ? 0 : (visit.StatementStatus + 1);
                            visit.LastStatementDate = DateTime.Now; 
                            patStatement = SavePatientStatementLog( visit, PDFfileName, CSVfileName, statementMessage, reportType, UD);
                            data += @"<tr>
                        <td class='tableDetails'  style='width:30px; border:1px solid #000; border-collapse: collapse; padding: 8px; font-size: 16px; '>
                            " + dos + @"
                        </td>
                        <td  class='tableDetails'  style='width:93px ;border:1px solid #000; border-collapse: collapse; padding: 8px; font-size: 16px;' >
                               " + details[k].patientBalance.Val() + @"
                        </td> 
                    </tr>";
                        }
                        data += @" </tbody></table>";
                    }
                }
                else
                {
                    return BadRequest("Please select Report Type.");
                }
                string statementHTML = System.IO.File.ReadAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Resources/statement", "statement.html"));
                statementHTML = statementHTML.Replace("$PRACTICE_NAME", header[0].practiceName);
                statementHTML = statementHTML.Replace("$PRACTICE_Address", header[0].practiceAddress);
                statementHTML = statementHTML.Replace("$PRACTICE_City", header[0].practiceCity);
                statementHTML = statementHTML.Replace("$PRACTICE_State", header[0].practiceState);
                statementHTML = statementHTML.Replace("$PRACTICE_Zip", header[0].practiceZipCode);
                statementHTML = statementHTML.Replace("$PATIENT_Name", header[0].patient);
                statementHTML = statementHTML.Replace("$PATIENT_Address", header[0].patientAddress);
                statementHTML = statementHTML.Replace("$PATIENT_City", header[0].patientCity);
                statementHTML = statementHTML.Replace("$PATIENT_State", header[0].patientState);
                statementHTML = statementHTML.Replace("$PATIENT_Zip", header[0].patientZipCode);
                statementHTML = statementHTML.Replace("$PATIENT_Id", header[0].patientAccountNum);
                statementHTML = statementHTML.Replace("$STATEMENT_Date", DateTime.Now.Date.ToString("MM/dd/yyyy"));
                statementHTML = statementHTML.Replace("$STATEMENT_DATA", data);
                statementHTML = statementHTML.Replace("$AMOUNT", "$"+total.ToString());
                statementHTML = statementHTML.Replace("$URL", resourcesPath);
                statementHTML = statementHTML.Replace("$MESSAGE", statementMessage);
                System.IO.File.WriteAllText(HTMLfileName, statementHTML);
                PdfWriter writer = new PdfWriter(PDFfileName);
                PdfDocument pdfDocument = new PdfDocument(writer);
                pdfDocument.SetDefaultPageSize(PageSize.A4);
                HtmlConverter.ConvertToPdf(statementHTML, pdfDocument, null);
                PageSize pg = pdfDocument.GetDefaultPageSize();
                 VMPatientStatementFile file = new VMPatientStatementFile();
                file.ID = i; 
                file.patient = header[0].patient;
                file.claimIds = string.Join(",", PatientVisitsIds);
                file.FileName = arrPatientIds[i]+ "_" + datetime + ".pdf";
                file.patientId = header[0].patientID.ToString();
                file.pdf_id = patStatement.ID.ToString();
                file.pdf_url = PDFfileName;
                lstFiles.Add(file);
            }
            _context.SaveChangesAsync();
            System.IO.File.WriteAllText(CSVfileName, string.Empty);
            string line = @"Statement No. ,Billing Statement,Office Name,Address,City,State,ZipCode,Phone,Statement Date,Patient Name,Account #,Patient Address1,Patient Address2,Bill To Name,Bill To Address1,Bill To Address2,Visit ID,Date Of Service,CPT,Procedure,Quantity,Charge,Insurance Payment,Patient Payment,Adjustment,Current Due,Over 30 Days,Over 60 Days,Over 90 Days,Balance,Total Balance,Reference,email Address,Comments";
            System.IO.File.AppendAllText(CSVfileName, line);
            var a = new {  data = lstFiles, csv = CSVfileName };
            return Json(a);
        }
  
    private PatientStatement SavePatientStatementLog(  Visit visit,string PDFfileName,string CSVfileName,string StatementMessage,string reportType,UserInfoData UD)
        {
  
            PatientStatement patStatement = new PatientStatement();
            patStatement.PatientID = visit.PatientID;
            patStatement.VisitID = visit.ID;
            patStatement.pdf_url = PDFfileName;
            patStatement.csv_url = CSVfileName;
            patStatement.statementStatus = visit.StatementStatus == null ? 0 : (visit.StatementStatus.Value);
            patStatement.Message =  StatementMessage ?? "";
            patStatement.Type = reportType;
            patStatement.PracticeID = UD.PracticeID;
            patStatement.AddedBy = UD.Email;
            patStatement.AddedDate = DateTime.Now;
            patStatement.UpdatedBy = UD.Email;
            patStatement.UpdatedDate = DateTime.Now;
            _context.PatientStatement.Add(patStatement);
            _context.SaveChanges();
            VisitController visitC = new VisitController(_context, _contextMain);
             visitC.SaveVisit(visit);
            return patStatement;
        }
        public byte[] CreateHtml(string BASEURI)
        {
            MemoryStream ms = new MemoryStream();
            StreamWriter writer = new StreamWriter(ms);
            XslCompiledTransform transformer = new XslCompiledTransform();
            XmlReader xmlReader = XmlReader.Create(new System.IO.StringReader(BASEURI)); 
            transformer.Transform(xmlReader, null, writer);
            xmlReader.Close();
            ms.Close();
            writer.Close();
            return ms.ToArray();
        }
        [HttpPost]
        [Route("PatientStatementDetails")]
        public async Task<ActionResult<IEnumerable<GPlanFollowup>>> PatientStatementDetails(CPlanFollowup CPlanFollowup)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.patientStatements == false)
            //    return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            return FindPlanFollowUp(CPlanFollowup, UD);
        }
        private List<GPlanFollowup> FindPlanFollowUp(CPlanFollowup CPlanFollowup, UserInfoData UD)
        {
            List<GPlanFollowup> data = (from pf in _context.PlanFollowUp
                          join v in _context.Visit on pf.VisitID equals v.ID
                          join r1 in _context.Reason on pf.ReasonID equals r1.ID into r11 from r in r11.DefaultIfEmpty()
                          join g1 in _context.Group on pf.GroupID equals g1.ID into g11 from g in g11.DefaultIfEmpty()
                          join a1 in _context.Action on pf.ActionID equals a1.ID into a11 from a in a11.DefaultIfEmpty()
                          join aCode in _context.AdjustmentCode on pf.AdjustmentCodeID equals aCode.ID into Table1 from t1 in Table1.DefaultIfEmpty()
                          join f in _context.Practice on v.PracticeID equals f.ID
                          //join up in _context.UserPractices on f.ID equals up.PracticeID
                          //join u in _context.Users on up.UserID equals u.Id
                          join l in _context.Location on v.LocationID equals l.ID
                          join pp in _context.PatientPlan on v.PrimaryPatientPlanID equals pp.ID
                          join ip in _context.InsurancePlan on pp.InsurancePlanID equals ip.ID
                          join p in _context.Patient on v.PatientID equals p.ID
                          join pro in _context.Provider on v.ProviderID equals pro.ID
                          orderby  pf.ID
                          where f.ID == UD.PracticeID && //u.Id.ToString() == UD.UserID &&
                          (v.PrimaryBal.Val() + v.SecondaryBal.Val() + v.TertiaryBal.Val() ) > 0 &&
                          (pf.TickleDate == null ? true : DateTime.Now > pf.TickleDate)&&
                         // u.IsUserBlock == false &&
                         (CPlanFollowup.ReasonID.IsNull() ? true : r.ID.Equals(CPlanFollowup.ReasonID)) &&
                         (CPlanFollowup.GroupID.IsNull() ? true : g.ID.Equals(CPlanFollowup.GroupID)) &&
                         (CPlanFollowup.ActionID.IsNull() ? true : a.ID.Equals(CPlanFollowup.ActionID)) &&
                         (CPlanFollowup.Practice.IsNull() ? true : f.Name.ToUpper().Contains(CPlanFollowup.Practice)) &&
                         (CPlanFollowup.Location.IsNull() ? true : l.Name.ToUpper().Contains(CPlanFollowup.Location)) &&
                         (CPlanFollowup.PlanName.IsNull() ? true : ip.PlanName.ToUpper().Contains(CPlanFollowup.PlanName)) &&
                         (CPlanFollowup.VisitID.IsNull() ? true : v.ID.Equals(CPlanFollowup.VisitID)) &&
                         (CPlanFollowup.AccountNum.IsNull() ? true : p.AccountNum.Equals(CPlanFollowup.AccountNum)) &&
                         (CPlanFollowup.DOS == null ? true : object.Equals(CPlanFollowup.DOS, v.DateOfServiceFrom)) &&
                         (CPlanFollowup.SubmitDate == null ? true : object.Equals(CPlanFollowup.SubmitDate, v.SubmittedDate))&&
                         (CPlanFollowup.TickleDate == null ? true : object.Equals(CPlanFollowup.TickleDate, pf.TickleDate))&&
                         (ExtensionMethods.IsBetweenDOS(CPlanFollowup.ToDate, CPlanFollowup.FromDate, pf.AddedDate, pf.AddedDate)) 


                          select new GPlanFollowup()
                          {
                              ID = pf.ID,
                              PlanFollowUpID = pf.ID,
                              VisitID = pf.VisitID,
                              DOS = v.DateOfServiceFrom.Format("MM/dd/yyyy"),
                              AccountNum = p.AccountNum,
                              Patient = p.LastName + ", " + p.FirstName,
                              PatientID = p.ID,
                              Practice = f.Name,
                              PracticeID = f.ID,
                              Location = l.Name,
                              LocationID = l.ID,
                              Provider = pro.LastName + ", " + pro.FirstName,
                              ProviderID = pro.ID,
                              PlanBalance = v.PrimaryBal,
                              AdjustmentCodeID = pf.AdjustmentCodeID,
                              AdjustmentCode = t1.Code,
                              Group = g.Name,
                              GroupID = g.ID,
                              Reason = r.Name,
                              ReasonID = r.ID,
                              Action = a.Name,
                              ActionID = a.ID,
                              RemitCode = pf.RemitCode,
                              SubmitDate = v.SubmittedDate.Format("MM/dd/yyyy"),
                              EntryDate = pf.AddedDate.Date().ToString("MM/dd/yyyy"),
                              TickleDate = pf.TickleDate.Format("MM/dd/yyyy"),
                              FollowupAge = (pf.AddedDate.Date().IsNull() ? "" : System.DateTime.Now.Subtract(pf.AddedDate.Date()).Days.ToString()),
                              PlanName = ip.PlanName,
                              InsurancePlanID = ip.ID,
                              IsSubmitted = v.IsSubmitted==true ? "Yes" : "No", // Need To put Yes For true
                              BilledAmount = v.PrimaryBilledAmount.Val()
                          }).ToList();
            return data;
        }
        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CPlanFollowup CPlanFollowup)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GPlanFollowup> data = FindPlanFollowUp(CPlanFollowup, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CPlanFollowup, "Plan Follow up Report");
        }


        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CPlanFollowup CPlanFollowup)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GPlanFollowup> data = FindPlanFollowUp(CPlanFollowup, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);
        }

        [Route("SavePlanFollowUp")]
        [HttpPost]
        public async Task<ActionResult<PlanFollowup>> SavePlanFollowUp(PlanFollowup item)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
           if (UD == null || UD.Rights == null || UD.Rights.planFollowupCreate == false)
           return BadRequest("You Don't Have Rights. Please Contact BellMedex.");

            bool succ = TryValidateModel(item);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            var transactionOption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (var objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (item.ID == 0)
                    {

                        item.AddedBy = UD.Email;
                        item.AddedDate = DateTime.Now;
                        _context.PlanFollowUp.Add(item);
                        // Adding Notes
                        foreach (Notes notes in item.Note)
                        {
                            if (notes.ID <= 0)
                            {
                                notes.PlanFollowupID = item.ID;
                                notes.AddedBy = UD.Email;
                                notes.AddedDate = DateTime.Now;
                                notes.NotesDate = DateTime.Now;
                                _context.Notes.Add(notes);
                            }
                        }
                    }

                    else if (UD.Rights.planFollowupUpdate == true)
                    {
                        item.UpdatedBy = UD.Email;
                        item.UpdatedDate = DateTime.Now;
                        _context.PlanFollowUp.Update(item);
                        // Adding Notes
                        foreach (Notes notes in item.Note)
                        {
                            if (notes.ID <= 0)
                            {
                                notes.PlanFollowupID = item.ID;
                                notes.AddedBy = UD.Email;
                                notes.AddedDate = DateTime.Now;
                                notes.NotesDate = DateTime.Now;
                                _context.Notes.Add(notes);
                            }

                            else
                            {
                                notes.PlanFollowupID = item.ID;
                                notes.UpdatedBy = UD.Email;
                                notes.UpdatedDate = DateTime.Now;
                                _context.Notes.Update(notes);

                            }
                        } // Ending of  foreach (Notes notes in item.Note)

                    }
                    else return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
                    await _context.SaveChangesAsync();
                    objTrnScope.Complete();
                    objTrnScope.Dispose();
                }
                catch (Exception ex)
                {
                    System.IO.File.WriteAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Logs", "PlanFollowUp.txt"), ex.ToString());
                    throw ex;

                }
                finally
                {

                }
            }


            return Ok(item);
        }

    
        [Route("DeletePlanFollowUp/{id}")]
        [HttpDelete]

        public async Task<ActionResult> DeletePlanFollowUp(long id)
        {
            var PlanFollowUp = await _context.PlanFollowUp.FindAsync(id);

            if (PlanFollowUp == null)
            {
                return NotFound();
            }

            _context.PlanFollowUp.Remove(PlanFollowUp);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [Route("FindAudit/{PlanFollowUpID}")]
        [HttpGet("{PlanFollowUpID}")]
        public List<PlanFollowupAudit> FindAudit(long PlanFollowUpID)
        {
            List<PlanFollowupAudit> data = (from pAudit in _context.PlanFollowupAudit
                                            where pAudit.PlanFollowupID == PlanFollowUpID
                                            orderby pAudit.AddedDate  descending 
                                            select new PlanFollowupAudit()
                                            {
                                                ID = pAudit.ID,
                                                PlanFollowupID = pAudit.PlanFollowupID,
                                                TransactionID = pAudit.TransactionID,
                                                ColumnName = pAudit.ColumnName,
                                                CurrentValue = pAudit.CurrentValue,
                                                NewValue = pAudit.NewValue,
                                                CurrentValueID = pAudit.CurrentValueID,
                                                NewValueID = pAudit.NewValueID,
                                                HostName = pAudit.HostName,
                                                AddedBy = pAudit.AddedBy,
                                                AddedDate = pAudit.AddedDate,
                                            }).ToList<PlanFollowupAudit>();
            return data;

        }
        
    }
}
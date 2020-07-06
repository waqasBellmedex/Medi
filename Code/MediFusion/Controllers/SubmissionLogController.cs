using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MediFusionPM.Models;
using MediFusionPM.Models.TodoApi.Models;
using MediFusionPM.ViewModels;
using static MediFusionPM.ViewModels.VMCommon;
using static MediFusionPM.ViewModels.VMSubmissionLog;
using static MediFusionPM.ViewModels.VMVisit;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class SubmissionLogController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;

        public SubmissionLogController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
        }


        [HttpGet]
        [Route("GetSubmissionLogs")]
        public async Task<ActionResult<IEnumerable<SubmissionLog>>> GetSubmissionLogs()
        {
            return await _context.SubmissionLog.ToListAsync();
        }

        [Route("FindSubmissionLog/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<SubmissionLog>> FindSubmissionLog(long id)
        {
            var SubmissionLog = await _context.SubmissionLog.FindAsync(id);

            if (SubmissionLog == null)
            {
                return NotFound();
            }

            return SubmissionLog;
        }



        [HttpPost]
        [Route("FindSubmissionLog")]
        public async Task<ActionResult<IEnumerable<GSubmissionLog>>> FindSubmissionLog(CSubmissionLog CSubmissionLog)
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.submissionLogSearch == false)
            //return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindSubmissionLog(CSubmissionLog, PracticeId);
        }
        private List<GSubmissionLog> FindSubmissionLog(CSubmissionLog CSubmissionLog, long PracticeId)
        {
            List<GSubmissionLog> data = (from sLog in _context.SubmissionLog
                                         join rec in _context.Receiver on sLog.ReceiverID equals rec.ID into r2
                                         from rec in r2.DefaultIfEmpty()
                                         join prac in _context.Practice on sLog.ClientID equals prac.ClientID
                                         //join up in _context.UserPractices on prac.ID equals up.PracticeID
                                         //join u in _context.Users on up.UserID equals u.Id
                                         orderby sLog.ID descending
                                         where
                          prac.ID == PracticeId &&/* u.Id.ToString() == UD.UserID &&*/
                                                  //u.IsUserBlock == false &&
                         (CSubmissionLog.SubmitBatchNumber.IsNull() ? true : sLog.ID.ToString().Equals(CSubmissionLog.SubmitBatchNumber)) &&
                         (CSubmissionLog.Receiver.IsNull() ? true : rec.Name.ToUpper().Contains(CSubmissionLog.Receiver)) &&
                          (CSubmissionLog.FormType.IsNull() ? true : sLog.FormType.ToUpper().Contains(CSubmissionLog.FormType)) &&
                          (CSubmissionLog.SubmitDate == null ? true : object.Equals(CSubmissionLog.SubmitDate, sLog.AddedDate.Date)) &&
                         (CSubmissionLog.SubmitType.IsNull() ? true : sLog.SubmitType.Contains(CSubmissionLog.SubmitType)) &&
                         (CSubmissionLog.ISAControllNumber.IsNull() ? true : sLog.ISAControlNumber.Equals(CSubmissionLog.ISAControllNumber))&&
                         (CSubmissionLog.ResolvedErrorMessage.IsNull() ? true : CSubmissionLog.ResolvedErrorMessage == "Y" ? sLog.Resolve == true : CSubmissionLog.ResolvedErrorMessage == "N" ? sLog.Resolve == false : true)


                                         select new GSubmissionLog()
                                         {
                                             PracticeID = prac.ID,
                                             SubmitBatchNumber = sLog.ID,
                                             Receiver = rec.Name,
                                             FormType = sLog.FormType,
                                             SubmitDate = sLog.AddedDate.ToString("MM/dd/yyyy h:mm:ss tt"),
                                             Status = GetStatus(sLog),
                                             ISAControllNumber = sLog.ISAControlNumber,
                                             SubmitType = (sLog.SubmitType == "P" ? "Paper" : (sLog.SubmitType == "E" ? "EDI" : "")),
                                             Notes = sLog.Notes,
                                             NumOfVisits = sLog.ClaimCount,
                                             VisitCount = sLog.ClaimCount,
                                             VisitAmount = sLog.ClaimAmount.Val(),
                                             Resolve = sLog.Resolve == true ? "Yes" : "No"
                                         }).ToList();

            if (data != null)
            {
                {
                    data = (from d in data
                            join prac in _context.Practice on d.PracticeID equals prac.ID

                            where prac.ID.Equals(PracticeId)
                            select d).Distinct().ToList();
                }
            }

            return data;
        }


        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CSubmissionLog CSubmissionLog)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GSubmissionLog> data = FindSubmissionLog(CSubmissionLog, PracticeId);
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CSubmissionLog, "Submission Log Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CSubmissionLog CSubmissionLog)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GSubmissionLog> data = FindSubmissionLog(CSubmissionLog, PracticeId);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }

        private string GetStatus(SubmissionLog log)
        {
            string status = string.Empty;
            if (log.IK5_Status == "A" && log.AK9_Status == "A") status = "Batch Accepted";
            else if (log.IK5_Status == "R" && log.AK9_Status == "R") status = "Batch Rejected";
            else if (log.IK5_Status == "E" && log.AK9_Status == "E") status = "Partially Accepted";

            return status;
        }


        [Route("GetSubmitedVisits/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<List<GVisit>>> GetSubmitedVisits(long id)
        {
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GVisit> visit = (from v in _context.Visit
                                  join sLog in _context.SubmissionLog on v.SubmissionLogID equals sLog.ID
                                  join pat in _context.Patient on v.PatientID equals pat.ID
                                  join prac in _context.Practice on v.PracticeID equals prac.ID
                                  join pPlan in _context.PatientPlan on pat.ID equals pPlan.PatientID
                                  join IPlan in _context.InsurancePlan on pPlan.InsurancePlanID equals IPlan.ID
                                  //join up in _context.UserPractices on prac.ID equals up.PracticeID
                                  //join u in _context.Users on up.UserID equals u.Id
                                  join loc in _context.Location on v.LocationID equals loc.ID
                                  join pro in _context.Provider on v.ProviderID equals pro.ID
                                  where prac.ID == PracticeId && sLog.ID == id && v.PrimaryPatientPlanID == pPlan.ID
                                  //u.Id.ToString() == UD.UserID
                                  // && v.AddedBy == Email 
                                  // && u.IsUserBlock == false &&

                                  select new GVisit
                                  {
                                      VisitID = v.ID,
                                      ProviderID = pro.ID,
                                      PracticeID = prac.ID,
                                      LocationID = loc.ID,
                                      DOS = v.DateOfServiceFrom.Format("MM/dd/yyyy"),
                                      AccountNum = pat.AccountNum,
                                      patientID = pat.ID,
                                      Patient = pat.LastName + ", " + pat.FirstName,
                                      Practice = prac.Name,
                                      Location = loc.Name,
                                      Provider = pro.LastName + ", " + pro.FirstName,
                                      Amount = v.TotalAmount,
                                      Status = TranslateStatus(v.PrimaryStatus),
                                      RejectionReason = v.RejectionReason,
                                      InsurancePlanName = IPlan.PlanName,
                                      InsurancePlanID = IPlan.ID

                                  }
                       ).ToList();


            return visit;
        }

        public string TranslateStatus(string Status)
        {


            string desc = string.Empty;
            if (Status == "N")
            {
                desc = "New Charge";
            }
            if (Status == "S")
            {
                desc = "Submitted";
            }
            if (Status == "K")
            {
                desc = "999 Accepted";
            }
            if (Status == "D")
            {
                desc = "999 Denied";
            }
            if (Status == "E")
            {
                desc = "999 has Errors";
            }
            if (Status == "P")
            {
                desc = "Paid";
            }
            if (Status == "DN")
            {
                desc = "Denied";
            }
            if (Status == "PT_P")
            {
                desc = "Patient Paid";
            }
            if (Status == "PPTS")
            {
                desc = "Transefered to Sec.";
            }
            if (Status == "PPTT")
            {
                desc = "Paid-Transfered To Ter";
            }
            if (Status == "PPTP")
            {
                desc = "Transfered to Patient";
            }
            if (Status == "SPTP")
            {
                desc = "Paid-Transfered To Patient";
            }
            if (Status == "SPTT")
            {
                desc = "Paid-Transfered To Ter";
            }
            if (Status == "PR_TP")
            {
                desc = "Pat. Resp. Transferred to Pat";
            }
            if (Status == "PPTM")
            {
                desc = "Paid - Medigaped";
            }
            if (Status == "M")
            {
                desc = "Medigaped";
            }
            if (Status == "R")
            {
                desc = "Rejected";
            }
            if (Status == "A1AY")
            {
                desc = "Received By Clearing House";
            }
            if (Status == "A0PR")
            {
                desc = "Forwarded  to Payer";
            }
            if (Status == "A1PR")
            {
                desc = "Received By Payer";
            }
            if (Status == "A2PR")
            {
                desc = "Accepted By Payer";
            }
            if (Status == "TS")
            {
                desc = "Transferred to Secondary";
            }
            if (Status == "TT")
            {
                desc = "Transferred to Tertiary";
            }
            if (Status == "PTPT")
            {
                desc = "Plan to Patient Transfer";
            }

            return desc;
        }




        [Route("GetSubmitedCharges/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<GVisit>>> GetSubmitedCharges(long id)
        {
            //string Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            //var CurrentLoginUser = (from u in _context.Users
            //                        where u.Email == Email
            //                        select u
            //                    ).FirstOrDefault();
            //string Userid = CurrentLoginUser.Id;
            //long PracId = CurrentLoginUser.PracticeID.Value;
            //long Clientid = CurrentLoginUser.ClientID.Value;
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GVisit> visit = (from v in _context.Visit
                                  join c in _context.Charge on v.ID equals c.VisitID 
                                  join sLog in _context.SubmissionLog on v.SubmissionLogID equals sLog.ID
                                  join pat in _context.Patient on v.PatientID equals pat.ID
                                  join prac in _context.Practice on v.PracticeID equals prac.ID
                                  join pPlan in _context.PatientPlan on pat.ID equals pPlan.PatientID
                                  join IPlan in _context.InsurancePlan on pPlan.InsurancePlanID equals IPlan.ID
                                  //join up in _context.UserPractices on prac.ID equals up.PracticeID
                                  //join u in _context.Users on up.UserID equals u.Id
                                  join loc in _context.Location on v.LocationID equals loc.ID
                                  join pro in _context.Provider on v.ProviderID equals pro.ID
                                  //where prac.ID == PracId && u.Id.ToString() == Userid
                                  // && v.AddedBy == Email && u.IsUserBlock == false &&
                                  where sLog.ID == id && c.PrimaryPatientPlanID == pPlan.ID && v.PrimaryPatientPlanID == pPlan.ID


                                  select new GVisit
                                  {
                                      VisitID = v.ID,
                                      ChargeID = c.ID,
                                      ProviderID = pro.ID,
                                      PracticeID = prac.ID,
                                      LocationID = loc.ID,
                                      DOS = v.DateOfServiceFrom.Format("MM/dd/yyyy"),
                                      AccountNum = pat.AccountNum,
                                      patientID = pat.ID,
                                      Patient = pat.LastName + ", " + pat.FirstName,
                                      Practice = prac.Name,
                                      Location = loc.Name,
                                      Provider = pro.LastName + ", " + pro.FirstName,
                                      Amount = v.TotalAmount,
                                      //Status = c.PrimaryStatus
                                      Status = TranslateStatus(v.PrimaryStatus),
                                      InsurancePlanID = IPlan.ID,
                                      InsurancePlanName = IPlan.PlanName
                                  }
                       ).ToList();


            return visit;
        }

        [Route("ResubmitVisit/{SubmissionLogId}")]
        [HttpPost("{SubmissionLogId}")]
        public async Task<ActionResult<VMVisit>> ResubmitVisit(long SubmissionLogId)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value);


            List<Visit> visits = _context.Visit.Where(v => v.SubmissionLogID == SubmissionLogId).ToList<Visit>();

            foreach (Visit visit in visits)
            {

                if (visit != null)
                {
                    if (!visit.TertiaryStatus.IsNull())
                    {
                        visit.TertiaryStatus = "RS";
                        visit.IsResubmitted = true;
                    }

                    else if (!visit.SecondaryStatus.IsNull())
                    {
                        visit.SecondaryStatus = "RS";
                        visit.IsResubmitted = true;
                    }

                    else

                    {
                        visit.PrimaryStatus = "RS";
                        visit.IsSubmitted = false;
                        visit.IsResubmitted = true;
                    }
                    _context.Visit.Update(visit);

                    List<Charge> charges = _context.Charge.Where(c => c.SubmissionLogID == SubmissionLogId && c.VisitID == visit.ID).ToList<Charge>();

                    foreach (Charge charge in charges)
                    {
                        if (!charge.TertiaryStatus.IsNull())
                        {
                            charge.TertiaryStatus = "RS";
                            charge.IsResubmitted = true;
                        }
                        else if (!charge.SecondaryStatus.IsNull())
                        {
                            charge.SecondaryStatus = "RS";
                            charge.IsResubmitted = true;
                        }
                        else
                        {
                            charge.IsSubmitted = false;
                            charge.PrimaryStatus = "RS";
                            charge.IsResubmitted = true;
                        }

                        _context.Charge.Update(charge);

                        ResubmitHistory resHistory = new ResubmitHistory();
                        resHistory.ChargeID = charge.ID;
                        resHistory.VisitID = visit.ID;
                        resHistory.AddedBy = UD.Email;
                        resHistory.AddedDate = DateTime.Now;
                        resHistory.UpdatedBy = UD.Email;
                        resHistory.UpdatedDate = DateTime.Now;
                        _context.ResubmitHistory.Add(resHistory);

                    }
                    _context.SaveChanges();
                }

            }


            return Ok("Sucessfully Resubmit");

        }
        [Route("FindAudit/{SubmissionLogID}")]
        [HttpGet("{SubmissionLogID}")]
        public List<SubmissionLogAudit> FindAudit(long SubmissionLogID)
        {
            List<SubmissionLogAudit> data = (from pAudit in _context.SubmissionLogAudit
                                             where pAudit.SubmissionLogID == SubmissionLogID
                                             orderby pAudit.AddedDate descending
                                             select new SubmissionLogAudit()
                                             {
                                                 ID = pAudit.ID,
                                                 SubmissionLogID = pAudit.SubmissionLogID,
                                                 TransactionID = pAudit.TransactionID,
                                                 ColumnName = pAudit.ColumnName,
                                                 CurrentValue = pAudit.CurrentValue,
                                                 NewValue = pAudit.NewValue,
                                                 CurrentValueID = pAudit.CurrentValueID,
                                                 NewValueID = pAudit.NewValueID,
                                                 HostName = pAudit.HostName,
                                                 AddedBy = pAudit.AddedBy,
                                                 AddedDate = pAudit.AddedDate,
                                             }).ToList<SubmissionLogAudit>();
            return data;
        }


        [Route("ResolveSubmission/{id}/{value}")]
        [HttpGet("{id}/{value}")]
        public async Task<ActionResult<SubmissionLog>> ResolveSubmission(long id , bool value)
        {
            var submission = await _context.SubmissionLog.FindAsync(id);
           
            if(submission != null)
            {
                submission.Resolve = value;
            }
            _context.SaveChanges();
            
            return submission;
        }

     [Route("EditNotes")]
[HttpPost]
public ActionResult<SubmissionLog> EditNotes(ListSubmission list)//long ID,string Note)
{
    var data = (from N in _context.SubmissionLog
                where N.ID == list.Id
                select N).FirstOrDefault();
    if (data == null)
    {
        BadRequest("Cannot Find The Values");
    }
    data.Notes = list.notes;
    _context.SubmissionLog.Update(data);
    _context.SaveChanges();
    return data;
}

        [HttpGet("{id}")]
        [Route("GetNotes/{id}")]
        public async Task<IActionResult> GetNotes(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var note = await _context.SubmissionLog.FindAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
        }


        [Route("HoldPrint/{visitID}/{HoldValue}")]
        [HttpPost("{visitID}/{HoldValue}")]

        public async Task<ActionResult<VMVisit>> HoldPrint(long VisitID, bool HoldValue)
        {

            var visits = _context.Visit.Where(v => v.ID == VisitID);
            foreach (Visit visit in visits)
            {
                if (HoldValue == true)
                {
                    visit.PrimaryStatus = "H";
                    visit.IsDontPrint = true;
                }
                else
                {
                    visit.PrimaryStatus = "N";
                    visit.IsDontPrint = false;
                }

                _context.Visit.Update(visit);

                List<Charge> charge = _context.Charge.Where(c => c.ID == VisitID).ToList<Charge>();

                foreach (Charge charg in charge)
                {

                    if (HoldValue == true)
                    {
                        charg.PrimaryStatus = "H";
                        charg.IsDontPrint = true;

                    }
                    else
                    {
                        charg.PrimaryStatus = "N";
                        charg.IsDontPrint = false;
                    }
                    _context.Charge.Update(charg);
                }
            }
            _context.SaveChanges();


            return Ok("Successfully PrintHold !");
        }

    }
}
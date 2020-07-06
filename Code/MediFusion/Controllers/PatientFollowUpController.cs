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
using MediFusionPM.Models.TodoApi;
using MediFusionPM.Models.TodoApi.Models;
using MediFusionPM.ViewModels;
using static MediFusionPM.ViewModels.VMCommon;
using static MediFusionPM.ViewModels.VMPatientFollowup;
using System.Transactions;
using System.IO;
using MediFusionPM.Models.Audit;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using MediFusionPM.ViewModel;
using iText.Html2pdf;
using iText.IO.Source;
using System.Text.RegularExpressions;
using MediFusionPM.Models.Main;
using System.IO.Compression;
using iTextSharp.text.html.simpleparser;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Events;
using iText.Layout;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class PatientFollowUpController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;

        private DateTime _startTime = DateTime.MinValue;


        public PatientFollowUpController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;
        }
        [Route("GetProfiles")]
        [HttpGet()]
        public async Task<ActionResult<VMPatientFollowup>> GetProfiles(long id)
        {
            ViewModels.VMPatientFollowup obj = new ViewModels.VMPatientFollowup();
            obj.GetProfiles(_context);



            return obj;
        }

        [Route("FindPatientFollowUp/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientFollowUp>> FindPatientFollowUp(long id)
        {
            var PatientFollowUp = await _context.PatientFollowUp.FindAsync(id);

            if (PatientFollowUp == null)
            {
                return NotFound();
            }
            PatientFollowUp.Age = System.DateTime.Now.Subtract(PatientFollowUp.AddedDate.Date()).Days.ToString();
            // GPatientFollowupCharge Notmapped List used to return  Values
            List<GPatientFollowupCharge> data = (
                                                 from pf in _context.PatientFollowUp
                                                 join pCharge in _context.PatientFollowUpCharge on pf.ID equals pCharge.PatientFollowUpID
                                                 join c in _context.Charge on pCharge.ChargeID equals c.ID
                                                 join v in _context.Visit on c.VisitID equals v.ID
                                                 join pPlan in _context.PatientPlan on v.PrimaryPatientPlanID equals pPlan.ID
                                                 join iPlan in _context.InsurancePlan on pPlan.InsurancePlanID equals iPlan.ID
                                                 join cpt in _context.Cpt on c.CPTID equals cpt.ID
                                                 where pf.ID == id && pCharge.ChargeID == c.ID
                                                 && (c.PrimaryPatientBal.Val() + c.SecondaryPatientBal.Val() + c.TertiaryPatientBal.Val()) > 0
                                                 //&& (pf.TickleDate.Date().IsNull() ? true : pf.TickleDate > DateTime.Now)
                                                 select new GPatientFollowupCharge()
                                                 {
                                                     ID = pCharge.ID,
                                                     PatientFollowUpID = pf.ID,
                                                     PatientID = pf.PatientID,
                                                     VisitID = v.ID,
                                                     ChargeID = pCharge.ChargeID,
                                                     InsurancePlanID = iPlan.ID,
                                                     Plan = iPlan.PlanName,
                                                     DOS = c.DateOfServiceFrom.Format("MM/dd/yyyy"),
                                                     CPT = cpt.CPTCode,
                                                     BilledAmount = c.PrimaryBilledAmount,
                                                     PaidAmount = c.PrimaryPaid,
                                                     AllowedAmount = c.PrimaryAllowed,
                                                     Copay = c.Copay,
                                                     Deductible = c.Deductible,
                                                     CoInsurance = c.Coinsurance,
                                                     PatientPaid = c.PatientPaid,
                                                     PatientAmount = c.PrimaryPatientBal,
                                                     PatientBal = c.PrimaryPatientBal.Val() + c.SecondaryPatientBal.Val() + c.TertiaryPatientBal.Val(),
                                                     AddedDate = pCharge.AddedDate.Format("MM/dd/yyyy"),
                                                     AddedBy = pCharge.AddedBy,
                                                     TickleDate = pf.TickleDate.Format("MM/dd/yyyy"),
                                                     Status = (pCharge.Status.IsNull() ? "New" : pCharge.Status)

                                                 }).ToList<GPatientFollowupCharge>();

            PatientFollowUp.GPatientFollowupCharge = data;
            PatientFollowUp.Note = _context.Notes.Where(m => m.PatientFollowUpID == id).ToList<Notes>();
            return PatientFollowUp;
        }



        //[Route("FindPatientFollowUpCharge/{PatientID}")]
        //[HttpGet("{PatientID}")]
        //public List<GPatientFollowupCharge> FindPatientFollowUpCharge(long PatientID)
        //{
        //        List<GPatientFollowupCharge> data = (from p in _context.PatientFollowUpCharge
        //                                             join pf in _context.PatientFollowUp on p.PatientFollowUpID equals pf.ID
        //                                             join pVisit in _context.PaymentVisit on pf.PaymentVisitID equals pVisit.ID
        //                                             join v in _context.Visit on pVisit.VisitID equals v.ID
        //                                             join c in _context.Charge on v.ID equals c.VisitID
        //                                             join pPlan in _context.PatientPlan on v.PrimaryPatientPlanID equals pPlan.ID
        //                                             join iPlan in _context.InsurancePlan on pPlan.InsurancePlanID equals iPlan.ID
        //                                             join cpt in _context.Cpt on c.CPTID equals cpt.ID
        //                                             where pf.PatientID == PatientID && p.ChargeID == c.ID
        //                                             select new GPatientFollowupCharge()
        //                                             {
        //                                                 ID = p.ID,
        //                                                 PatientFollowUpID = pf.ID,
        //                                                 PatientID = pf.PatientID,
        //                                                 VisitID = pVisit.VisitID,
        //                                                 ChargeID = p.ChargeID,
        //                                                 Plan = iPlan.PlanName,
        //                                                 DOS = c.DateOfServiceFrom.Format("MM/dd/yyyy"),
        //                                                 CPT = cpt.CPTCode,
        //                                                 BilledAmount = c.PrimaryBilledAmount,
        //                                                 PaidAmount = c.PrimaryPaid,
        //                                                 AllowedAmount = c.PrimaryAllowed,
        //                                                 Copay = c.Copay,
        //                                                 Deductible = c.Deductible,
        //                                                 CoInsurance = c.Coinsurance,
        //                                                 PatientPaid = c.PatientPaid,
        //                                                 PatientAmount = c.PrimaryPatientBal,
        //                                                 AddedDate = p.AddedDate.Format("MM/dd/yyyy"),
        //                                                 AddedBy = p.AddedBy,


        //                                             }).ToList<GPatientFollowupCharge>();
        //        return data;

        //}


        [Route("FindPatientInfo/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientInfoDropDown>> FindPatientInfo(long id)
        {
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            PatientInfoDropDown data = (from pf in _context.PatientFollowUp
                                        join p in _context.Patient
                                         on pf.PatientID equals p.ID
                                        //join pp in _context.PatientPlan on p.ID equals pp.PatientID
                                        //join ip in _context.InsurancePlan on pp.InsurancePlanID equals ip.ID
                                        join prac in _context.Practice on p.PracticeID equals prac.ID
                                        //join up in _context.UserPractices on prac.ID equals up.PracticeID
                                        //join u in _context.Users on up.UserID equals u.Id
                                        join loc in _context.Location on p.LocationId equals loc.ID
                                        join r in _context.Reason on pf.ReasonID equals r.ID into Table1
                                        from t1 in Table1.DefaultIfEmpty()
                                        join a in _context.Action on pf.ActionID equals a.ID into Table2
                                        from t2 in Table2.DefaultIfEmpty()
                                        join g in _context.Group on pf.GroupID equals g.ID into Table3
                                        from t3 in Table3.DefaultIfEmpty()
                                        join prov in _context.Provider on p.ProviderID equals prov.ID into table4
                                        from t4 in table4.DefaultIfEmpty()
                                        join refprov in _context.RefProvider on p.RefProviderID equals refprov.ID into table5
                                        from t5 in table5.DefaultIfEmpty()
                                        join supProv in _context.Provider on p.ProviderID equals supProv.ID into table6
                                        from t6 in table6.DefaultIfEmpty()
                                        where pf.ID == id &&
                                        prac.ID == PracticeId
                                        //&& u.Id.ToString() == UD.UserID
                                        //&& pf.AddedBy == Email 
                                        //&& u.IsUserBlock == false 
                                        select new PatientInfoDropDown()
                                        {
                                            PatientID = pf.PatientID.ToString(),
                                            PatientName = p.LastName + ", " + p.FirstName,
                                            AccountNumber = p.AccountNum,
                                            DOB = p.DOB.Format("MM/dd/yyyy"),
                                            Gender = p.Gender,
                                            //PlanName = ip.PlanName,
                                            //InsuredName = ip.Description,
                                            // InsuredID = pf.InsurancePlanID,
                                            ReasonID = pf.ReasonID,
                                            Reason = t1.Description,
                                            ActionID = pf.ActionID,
                                            Action = t2.Description,
                                            GroupID = pf.GroupID,
                                            Group = t3.Description,
                                            PracticeID = prac.ID,
                                            PracticeName = prac.Name,
                                            LocationID = loc.ID,
                                            LocationName = loc.Name,
                                            providerID = t4.ID,
                                            ProviderName = t4.Name,
                                            RefProviderID = t5.ID,
                                            RefProviderName = t5.Name,
                                            SupProviderID = t6.ID,
                                            SupProviderName = t6.Name,

                                        }).SingleOrDefault();
            return data;

        }


        [HttpPost]
        [Route("FindPatientFollowUp")]
        public async Task<ActionResult<IEnumerable<GPatientFollowup>>> FindPatientFollowUp(CPatientFollowup CPatientFollowup)
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.patientFollowupSearch == false)
            //return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindPatientFollowUp(CPatientFollowup, PracticeId);
        }
        private List<GPatientFollowup> FindPatientFollowUp(CPatientFollowup CPatientFollowup, long PracticeId)
        {
            List<GPatientFollowup> data = (from pf in _context.PatientFollowUp
                                           join pCharge in _context.PatientFollowUpCharge on pf.ID equals pCharge.PatientFollowUpID
                                           join c in _context.Charge on pCharge.ChargeID equals c.ID
                                           join pat in _context.Patient on pf.PatientID equals pat.ID
                                           join prac in _context.Practice on pat.PracticeID equals prac.ID
                                           join r in _context.Reason on pf.ReasonID equals r.ID into Table1
                                           from t1 in Table1.DefaultIfEmpty()
                                           join g in _context.Group on pf.GroupID equals g.ID into Table2
                                           from t2 in Table2.DefaultIfEmpty()
                                           join a in _context.Action on pf.ActionID equals a.ID into Table3
                                           from t3 in Table3.DefaultIfEmpty()
                                           where prac.ID == PracticeId
                                           && (c.PrimaryPatientBal.Val() + c.SecondaryPatientBal.Val() + c.TertiaryPatientBal.Val()) > 0
                                           && (CPatientFollowup.PatientID.IsNull() ? true : pf.PatientID.Equals(CPatientFollowup.PatientID))
                                           && (CPatientFollowup.FollowUpDate == null ? true : object.Equals(CPatientFollowup.FollowUpDate, pf.AddedDate))
                                           && (CPatientFollowup.PatientAccount.IsNull() ? true : pat.AccountNum.Equals(CPatientFollowup.PatientAccount))
                                           && (CPatientFollowup.ReasonID.IsNull() ? true : t1.ID.Equals(CPatientFollowup.ReasonID))
                                           && (CPatientFollowup.GroupID.IsNull() ? true : t2.ID.Equals(CPatientFollowup.GroupID))
                                           && (CPatientFollowup.ActionID.IsNull() ? true : t3.ID.Equals(CPatientFollowup.ActionID))
                                           && (ExtensionMethods.IsBetweenDOS(CPatientFollowup.ToDate, CPatientFollowup.FromDate, pf.AddedDate, pf.AddedDate))
                                           && (CPatientFollowup.TickleDate == null ? true : object.Equals(CPatientFollowup.TickleDate, pf.TickleDate))
                                          && (CPatientFollowup.Status.Equals("All")? pCharge.Status =="" || pCharge.Status.IsNull() || pCharge.Status.Equals("1st Statement Sent") || pCharge.Status.Equals("2nd Statement Sent") || pCharge.Status.Equals("3rd Statement Sent") || pCharge.Status.Equals("Collection Agency")  : CPatientFollowup.Status.Equals("New") ? pCharge.Status == "" || pCharge.Status.IsNull() : pCharge.Status.IsNull() ? false : pCharge.Status.Equals(CPatientFollowup.Status))
                                          && (pat.HoldStatement == null || pat.HoldStatement == false)

                                           group new
                                           {
                                               ID = pf.ID,
                                               PatientID = pf.PatientID,
                                               PatientFollowUpID = pf.ID,
                                               PatientAccount = pat.AccountNum,
                                               PatientName = pat.LastName + ", " + pat.FirstName,
                                               FollowUpDate = pf.AddedDate.Format("MM/dd/yyyy"),
                                               TickleDate = pf.TickleDate.Format("MM/dd/yyyy"),
                                               Reason = t1.Name,
                                               Group = t2.Name,
                                               Action = t3.Name,
                                               Status = (pCharge.Status.IsNull() ? "New" : pCharge.Status)
                                           } by new { PatientID = pf.PatientID } into gp

                                           select new GPatientFollowup
                                           {
                                               ID = gp.Select(a => a.ID).FirstOrDefault(),
                                               PatientID = gp.Key.PatientID,
                                               PatientFollowUpID = gp.Select(a => a.PatientFollowUpID).FirstOrDefault(),
                                               PatientAccount = gp.Select(a => a.PatientAccount).FirstOrDefault(),
                                               PatientName = gp.Select(a => a.PatientName).FirstOrDefault(),
                                               FollowUpDate = gp.Select(a => a.FollowUpDate).FirstOrDefault(),
                                               TickleDate = gp.Select(a => a.TickleDate).FirstOrDefault(),
                                               PatientAmount = (
                                                                  (from patcharge in _context.PatientFollowUpCharge
                                                                   join C in _context.Charge on patcharge.ChargeID equals C.ID
                                                                   where gp.Key.PatientID == C.PatientID
                                                                   select C.PrimaryPatientBal.Val() + C.SecondaryPatientBal.Val() + C.TertiaryPatientBal.Val() > 0 ?
                                                                   C.PrimaryPatientBal.Val() + C.SecondaryPatientBal.Val() + C.TertiaryPatientBal.Val() : 0).Sum()
                                                                  ),
                                               Reason = gp.Select(a => a.Reason).FirstOrDefault(),
                                               Group = gp.Select(a => a.Group).FirstOrDefault(),
                                               Action = gp.Select(a => a.Action).FirstOrDefault(),
                                               Status = gp.Select(a => a.Status).FirstOrDefault()
                                           }).ToList();
            return data;
        }

        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CPatientFollowup CPatientFollowup)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GPatientFollowup> data = FindPatientFollowUp(CPatientFollowup, PracticeId);
            ExportController controller = new ExportController(_context);
            
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CPatientFollowup, "Patient FollowUp Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CPatientFollowup CPatientFollowup)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GPatientFollowup> data = FindPatientFollowUp(CPatientFollowup, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }

        //[Route("SavePatientFollowUp")]
        //[HttpPost]
        //public async Task<ActionResult<PatientFollowUp>> SavePatientFollowUp(PatientFollowUp item)
        //{
        //    UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
        //   User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
        //   User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
        //   if (UD == null || UD.Rights == null || UD.Rights.patientFollowupCreate == false)
        //   return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
        //    bool succ = TryValidateModel(item);
        //    if (!ModelState.IsValid)
        //    {
        //        string messages = string.Join("; ", ModelState.Values
        //                                .SelectMany(x => x.Errors)
        //                                .Select(x => x.ErrorMessage));
        //        return BadRequest(messages);
        //    }
        //    if (item.ID == 0)
        //    {
        //        item.AddedBy = UD.Email;
        //        item.AddedDate = DateTime.Now;
        //        _context.PatientFollowUp.Add(item);
        //        await _context.SaveChangesAsync();
        //    }
        //    else if (UD.Rights.patientFollowupUpdate == true)
        //    {
        //        item.UpdatedBy = UD.Email;
        //        item.UpdatedDate = DateTime.Now;
        //        _context.PatientFollowUp.Update(item);
        //        await _context.SaveChangesAsync();
        //    }
        //    else return BadRequest("You Don't Have Rights. Please Contact BellMedex.");                                                                                                                              
        //    return Ok(item);
        //}

        [Route("SavePatientFollowUp")]
        [HttpPost]
        public async Task<ActionResult<PatientFollowUp>> SavePatientFollowUp(PatientFollowUp item)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (UD == null || UD.Rights == null || UD.Rights.patientFollowupCreate == false)
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
                        _context.PatientFollowUp.Add(item);
                        // Adding Notes
                        foreach (Notes notes in item.Note)
                        {
                            if (notes.ID <= 0)
                            {
                                notes.PatientFollowUpID = item.ID;
                                notes.AddedBy = UD.Email;
                                notes.AddedDate = DateTime.Now;
                                notes.NotesDate = DateTime.Now;
                                _context.Notes.Add(notes);
                            }
                        }
                    }

                    else if (UD.Rights.patientFollowupUpdate == true)
                    {
                        item.UpdatedBy = UD.Email;
                        item.UpdatedDate = DateTime.Now;
                        _context.PatientFollowUp.Update(item);
                        // Adding Notes
                        foreach (Notes notes in item.Note)
                        {
                            if (notes.ID <= 0)
                            {
                                notes.PatientFollowUpID = item.ID;
                                notes.AddedBy = UD.Email;
                                notes.AddedDate = DateTime.Now;
                                notes.NotesDate = DateTime.Now;
                                _context.Notes.Add(notes);
                            }

                            else
                            {
                                notes.PatientFollowUpID = item.ID;
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
                    System.IO.File.WriteAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Logs", "PatientFollowUp.txt"), ex.ToString());
                    throw ex;

                }
                finally
                {

                }
            }
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);
            return Ok(item);
        }


        [Route("DeletepatientFollowUp/{id}")]
        [HttpDelete]

        public async Task<ActionResult> DeletepatientFollowUp(long id)
        {
            var PatientFollowUp = await _context.PatientFollowUp.FindAsync(id);

            if (PatientFollowUp == null)
            {
                return NotFound();
            }

            _context.PatientFollowUp.Remove(PatientFollowUp);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [Route("FindAudit/{PatientFollowUpID}")]
        [HttpGet("{PatientFollowUpID}")]
        public List<PatientFollowUpAudit> FindAudit(long PatientFollowUpID)
        {
            List<PatientFollowUpAudit> data = (from pAudit in _context.PatientFollowUpAudit
                                               where pAudit.PatientFollowUpID == PatientFollowUpID
                                               orderby pAudit.AddedDate descending
                                               select new PatientFollowUpAudit()
                                               {
                                                   ID = pAudit.ID,
                                                   PatientFollowUpID = pAudit.PatientFollowUpID,
                                                   TransactionID = pAudit.TransactionID,
                                                   ColumnName = pAudit.ColumnName,
                                                   CurrentValue = pAudit.CurrentValue,
                                                   NewValue = pAudit.NewValue,
                                                   CurrentValueID = pAudit.CurrentValueID,
                                                   NewValueID = pAudit.NewValueID,
                                                   HostName = pAudit.HostName,
                                                   AddedBy = pAudit.AddedBy,
                                                   AddedDate = pAudit.AddedDate,
                                               }).ToList<PatientFollowUpAudit>();
            return data;
        }

        [HttpPost]
        [Route("PatientFollowUpVisits")]
        public ActionResult PatientFollowUpVisits(ListModel patientFollowUpIds)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (patientFollowUpIds == null || patientFollowUpIds.Ids.Length == 0)
            {
                return BadRequest("Please select Patient.");
            }
            var data = (from p in _context.PatientFollowUpCharge
                        join pf in _context.PatientFollowUp on p.PatientFollowUpID equals pf.ID
                        join c in _context.Charge on p.ChargeID equals c.ID
                        join v in _context.Visit on c.VisitID equals v.ID
                        join pat in _context.Patient on v.PatientID equals pat.ID
                        join pCharge in _context.PaymentCharge on p.PaymentChargeID equals pCharge.ID into TempPCharge
                        from finalPCharge in TempPCharge.DefaultIfEmpty()
                        join pVisit in _context.PaymentVisit on finalPCharge.PaymentVisitID equals pVisit.ID into TempPVisit
                        from finalPVisit in TempPVisit.DefaultIfEmpty()
                        join pPlan in _context.PatientPlan on v.PrimaryPatientPlanID equals pPlan.ID
                        join iPlan in _context.InsurancePlan on pPlan.InsurancePlanID equals iPlan.ID
                        join cpt in _context.Cpt on c.CPTID equals cpt.ID
                        where patientFollowUpIds.Ids.Contains(pf.ID) && p.ChargeID == c.ID
                        && (v.PrimaryPatientBal.Val() + v.SecondaryPatientBal.Val() + v.TertiaryPatientBal.Val()) > 0
                       // && patientFollowUpIds.Status == p.Status
                       && ((patientFollowUpIds.Status.IsNull() || patientFollowUpIds.Status.Equals("")) ? true : patientFollowUpIds.Status.Equals(patientFollowUpIds.Status.Trim()))
                       && (pat.HoldStatement == null || pat.HoldStatement == false)
                        select new
                        {
                            id = pf.ID,
                            PatientFollowUpID = pf.ID,
                            PatientID = pf.PatientID,
                            Patient = pat.LastName + ", " + pat.FirstName,
                            VisitID = v.ID,
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
                            Status = (p.Status.IsNull() || p.Status.Equals("") ? "New" : p.Status),
                        }).Distinct().Where(w => w.PatientAmount > 0).ToList();
            Practice practice = _context.Practice.Find(UD.PracticeID);
            string reportMsg = practice.StatementMessage ?? "";
            string reportType = practice.StatementExportType ?? "";
            var ret = new { data = data, reportMessage = reportMsg, statementType = reportType };
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);
            return Json(ret);
        }

        [HttpPost]
        [Route("DownloadStatementFile")]
     
        public async Task<IActionResult> DownloadStatementFile(DPdfAll pdf)
        {

            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            Settings settings = _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefault();
            //string resourcesPath = System.IO.Path.Combine(_context.env.ContentRootPath, "Resources/statement/");
           
            string DirectoryPath = System.IO.Path.Combine("\\\\", settings.DocumentServerURL,
               settings.DocumentServerDirectory, UD.ClientID.ToString(), "PatientStatement");
            //string DirectoryPath = System.IO.Path.Combine("\\\\", "C:",
             //settings.DocumentServerDirectory, UD.ClientID.ToString(), "PatientStatement");


         
            if (pdf.pdf_address != null && pdf.pdf_address.Length==1)
            {
                string pdf_url = System.IO.Path.Combine(DirectoryPath, pdf. pdf_address[0].Replace("**", "\\"));
                if (!System.IO.File.Exists(pdf_url))
                {
                    return NotFound();
                }
                Byte[] fileBytes = System.IO.File.ReadAllBytes(pdf_url);
                if (fileBytes == null)
                    return NotFound();
                var stream = new MemoryStream(fileBytes); //saves it into a stream
                stream.Position = 0;
                int index = pdf_url.LastIndexOf("\\");
                string filename = pdf_url.Substring(index + 1, pdf_url.Length - index - 1);
                Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
                this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);

                return File(stream, "application/octec-stream", filename);
            }
            if (pdf.pdf_address != null && pdf.pdf_address.Length  >1)
            {
                
                var datetime = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
               
                string sourcePath = DirectoryPath;
                string targetPath = System.IO.Path.Combine(DirectoryPath, "Allzipdf\\"+ datetime);
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }
                string finalTargetPath = System.IO.Path.Combine(targetPath, datetime);
                if (!Directory.Exists(finalTargetPath))
                {
                    Directory.CreateDirectory(finalTargetPath);
                }
                 foreach (var file in pdf.pdf_address)
                {
                    string sourceFile = System.IO.Path.Combine(sourcePath, file.Replace("**", "\\"));
                    string destFile = System.IO.Path.Combine(finalTargetPath, file.Replace("View**",""));
                    System.IO.File.Copy(sourceFile, destFile, true);
                }
                string zipFilePath = System.IO.Path.Combine(DirectoryPath, "Allzipdf\\zipfiles"+datetime+".zip");

                ZipFile.CreateFromDirectory(targetPath, zipFilePath);
                Byte[] fileBytes = System.IO.File.ReadAllBytes(zipFilePath);
                if (fileBytes == null)

                    return NotFound();
                var stream = new MemoryStream(fileBytes); //saves it into a stream
                stream.Position = 0;
                int index = zipFilePath.LastIndexOf("\\");
                string filename = zipFilePath.Substring(index + 1, zipFilePath.Length - index - 1);
                Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
                this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);
                return File(stream, "application/octec-stream", filename);

            }
            return NotFound();
        }
        [Route("DownloadPLDFile/{csv_address}")]
        [HttpGet("{csv_address}")]
        public async Task<IActionResult> DownloadPLDFile(string csv_address)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

            Settings settings = _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefault();            
             string DirectoryPath = System.IO.Path.Combine("\\\\", settings.DocumentServerURL,
                settings.DocumentServerDirectory, UD.ClientID.ToString(), "PatientStatement");
            //string DirectoryPath = System.IO.Path.Combine("\\\\", "C:",
                    // settings.DocumentServerDirectory, UD.ClientID.ToString(), "PatientStatement");
                     

           
            if (csv_address != "")
            {
                string csv_url = System.IO.Path.Combine(DirectoryPath, csv_address.Replace("**", "\\"));
                if (!System.IO.File.Exists(csv_url))
                {
                    return NotFound();
                }
                Byte[] fileBytes = System.IO.File.ReadAllBytes(csv_url);
                if (fileBytes == null)
                    return NotFound();
                var stream = new MemoryStream(fileBytes); //saves it into a stream
                stream.Position = 0;
                int index = csv_url.LastIndexOf("\\");
                string filename = csv_url.Substring(index + 1, csv_url.Length - index - 1);

                Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
                this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);
                return File(stream, "application/octec-stream", filename);
            }

            
            return NotFound();
        }
        [HttpPost]
        [Route("GeneratePatientStatement")]
        [Obsolete]
        public async Task<ActionResult> GeneratePatientStatement(GPatientStatement GPatientStatement)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            string FinalSatatment = "";
            if (GPatientStatement.patientId == null || GPatientStatement.patientId.Length == 0)
            {
                return BadRequest("Please select Patient(s).");
            }
            if (GPatientStatement.claimId == null || GPatientStatement. claimId.Length == 0)
            {
                return BadRequest("Please select Visit(s).");
            }

            var arrPatientIds = GPatientStatement.patientId;

            //var patientIds = data.Select(x => x.PatientID).Distinct();
            var patWithAdvPayments = new List<string>();

            foreach (var patID in arrPatientIds)
            {
                PatientPayment patPayment = _context.PatientPayment.Where(pp => pp.Type == "ADVANCE PAYMENT" && pp.PatientID.ToString() == patID.ToString()
                          && pp.PaymentAmount.Val() - pp.AllocatedAmount.Val() > 0).FirstOrDefault();

                if (patPayment != null) patWithAdvPayments.Add(patID.ToString());
            }
            ////Comment for now as Aziz Sab Said
            //if (patWithAdvPayments.Count > 0 && advPaymentProcMode.IsNull())
            //{
            //    var aa = new { data = "", csv = "", patWithAdvPayments = patWithAdvPayments.Count };
            //    return Json(aa);
            //}

            if (GPatientStatement.advPaymentProcMode == "PROCESS ADVANCE PAYMENTS FIRST")
            {
                //foreach (var patID in arrPatientIds)
                //{
                //    PatientPayment patPayment = _context.PatientPayment.Where(pp => pp.Type == "ADVANCE PAYMENT" && pp.PatientID.ToString() == patID
                //          && pp.PaymentAmount.Val() - pp.AllocatedAmount.Val() > 0).FirstOrDefault();

                //    List<Visit> patVisits = _context.Visit.Where(v => v.PatientID.ToString() == patID).ToList();

                //    foreach (Visit v in patVisits)
                //    {


                //        VisitController VisitController = new VisitController(_context, _contextMain);
                //        VisitController.ControllerContext = this.ControllerContext;
                //        await VisitController.SaveVisit(v);
                //    }

                //}
            }
            else if (GPatientStatement.advPaymentProcMode == "DISCARD ADVANCE PAYMENTS")
            {
            }
            else if (GPatientStatement.advPaymentProcMode == "DISCARD ADVANCE PAYMENT VISITS")
            {
            }


            Settings settings = _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefault();
            string resourcesPath = System.IO.Path.Combine(_context.env.ContentRootPath, "Resources/statement/");
            string DirectoryPath = System.IO.Path.Combine("\\\\", settings.DocumentServerURL,
                         settings.DocumentServerDirectory, UD.ClientID.ToString(), "PatientStatement");
           // string DirectoryPath = System.IO.Path.Combine("\\\\", "C:",
                 //     settings.DocumentServerDirectory, UD.PracticeID.ToString(), "PatientStatement");
            string ViewDirectoryPath;
            if (GPatientStatement.viewOnly == true)



            { ViewDirectoryPath = System.IO.Path.Combine(DirectoryPath, "View");

                if (!Directory.Exists(ViewDirectoryPath))
                {
                    Directory.CreateDirectory(ViewDirectoryPath);
                }

            }


            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }
            var datetime = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
            string PDFfileName = "", HTMLfileName = "", CSVfileName = "", savePDFfileName="", saveCSVfileName;
            List<VMPatientStatementPLDCSV> lstVMPatientStatementPLDCSV = new List<VMPatientStatementPLDCSV>();
            VMPatientStatementPLDCSV vmPatientStatementPLDCSV = null;

            List<long> PatientVisitsIds = new List<long>();
            List<VMPatientStatementFile> lstFiles = new List<VMPatientStatementFile>();
            Practice practice = _context.Practice.Find(UD.PracticeID);
            List<long> PatientIdsConsumed = new List<long>();
            PatientStatement patStatement = null;
            if (GPatientStatement.viewOnly == true)
                saveCSVfileName = System.IO.Path.Combine("View", datetime + ".csv");
            else
            saveCSVfileName = datetime + ".csv";
            CSVfileName = System.IO.Path.Combine(DirectoryPath, saveCSVfileName);
            
            DateTime statementDate = DateTime.Now;

            for (int i = 0; i < arrPatientIds.Length; i++)
            {
                if (!PatientIdsConsumed.Contains(arrPatientIds[i]))
                    PatientIdsConsumed.Add(arrPatientIds[i]);
                else
                    continue; // patientID duplicate
                FinalSatatment = ""; 
                long patId = arrPatientIds[i];
                DateTime? _lastPaymentDate = (from p in _context.PatientPayment
                                             where p.PatientID == patId
                                             select p
                                             ).OrderByDescending(o=>o.PaymentDate).Select(s => s.PaymentDate).FirstOrDefault();
                string lastPaymentDate = _lastPaymentDate != null ? _lastPaymentDate.Value.Format("MM/dd/yyyy") : "";
                var header = (from p in _context.Practice
                              join pt in _context.Patient on p.ID equals pt.PracticeID
                              where p.ID == UD.PracticeID && pt.ID == patId
                              select new
                              {
                                  practiceName = p.OrganizationName,
                                  practiceAddress = p.Address1,
                                  practiceCity = p.City,
                                  practiceState = p.State,
                                  practiceZipCode = p.ZipCode,
                                  practiceEmail = p.Email,

                                  StatementPhoneNumber = p.StatementPhoneNumber,
                                  StatementFaxNumber = p.StatementFaxNumber,
                                  AppointmentPhoneNumber = p.AppointmentPhoneNumber,
                                  patientID = pt.ID,
                                  patientAccountNum = pt.AccountNum,
                                  patient = pt.FirstName + " " + pt.LastName,
                                  patientAddress = pt.Address1,
                                  patientAddress2 = pt.City+" "+pt.State+" "+pt.ZipCode,
                                  patientCity = pt.City,
                                  patientState = pt.State,
                                  patientZipCode = pt.ZipCode,
                                  patientEmail = pt.Email,
                              }
                            ).ToList();
                string data = "";
                decimal? total = 0;
                if (GPatientStatement.reportType.ToLower() == "details")
                {
                    string reference;
                     var details = (from v in _context.Visit
                                   join vC in _context.Charge on v.ID equals vC.VisitID
                                   join pr in _context.Provider on vC.ProviderID equals pr.ID into table1
                                   from prT in table1.DefaultIfEmpty()
                                   join cpt in _context.Cpt on vC.CPTID equals cpt.ID
                                   join payVisit in _context.PaymentVisit on v.ID equals payVisit.VisitID into tpayVisit
                                   from payVisitT in tpayVisit.DefaultIfEmpty()
                                   join payCheck in _context.PaymentCheck on payVisitT.PaymentCheckID equals payCheck.ID into tpayCheck
                                   from payCheckT in tpayCheck.DefaultIfEmpty()
                                   where v.PracticeID == UD.PracticeID && v.PatientID == patId // && payVisitT.ProcessedAs == "1"
                                   && GPatientStatement.claimId.Contains(v.ID) 
                                   // Added By Aziz Sab
                                  // && v.HoldStatement != true && ExtensionMethods.IsNull_Bool(v.HoldStatement) == false
                                    select new
                                   {
                                       DateOfServiceFrom = vC.DateOfServiceFrom.ToString("MM/dd/yyyy"),
                                       DateOfServiceTo = vC.DateOfServiceTo.HasValue ? vC.DateOfServiceTo.Value.ToString("MM/dd/yyyy") : "",
                                       cpt = cpt.CPTCode,
                                       Aging = ExtensionMethods.DateDiff(DateTime.Now.Date, payCheckT.CheckDate ?? DateTime.Now),
                                       visitID = v.ID,
                                       PatientID= v.PatientID,
                                       description = cpt.ShortDescription,
                                       chargeID = vC.ID,
                                       totalAmount = vC.TotalAmount,
                                       PaidAmount = (vC.PrimaryPaid.Val() + vC.SecondaryPaid.Val() + vC.TertiaryPaid.Val()),
                                       units = vC.Units,
                                       AttendingProviderName = prT!=null?prT.LastName+", "+prT.FirstName:"",
                                       //LastPaymentDate = vC.SecondaryPaymentDate != null ? vC.SecondaryPaymentDate : vC.PrimaryPaymentDate, 
                                       Deductible =vC.Deductible.Val(),
                                       Coinsurance = vC.Coinsurance.Val(),
                                       Copay = vC.Copay.Val(),
                                       Balance = (vC.PrimaryPatientBal.Val()
                                      + vC.SecondaryPatientBal.Val()
                                      + vC.TertiaryPatientBal.Val()),
                                       TotalBalance = vC.PrimaryPatientBal.Val(),
                                      // TotalBalance = (v.PrimaryPatientBal.Val()
                                      //+ v.SecondaryPatientBal.Val()
                                      //+ v.TertiaryPatientBal.Val()),
                                       PatientPaid = vC.PatientPaid,
                                       WriteOff = (vC.PrimaryWriteOff.Val()
                                      + vC.SecondaryWriteOff.Val()
                                      + vC.TertiaryWriteOff.Val()),
                                       patientBalance = (vC.PrimaryPatientBal.Val()
                                      + vC.SecondaryPatientBal.Val()
                                      + vC.TertiaryPatientBal.Val())
                                   }
                                        ).ToList().Where(w => w.patientBalance > 0).ToList();
                    if(details==null || details.Count==0)
                    {
                        return BadRequest("Data not found.");
                    }
                    if (details != null)
                    {
                        PatientVisitsIds.Clear();
                        for (int d = 0; d < details.Count; d++)
                        {
                            if (!PatientVisitsIds.Contains(details[d].visitID))
                                PatientVisitsIds.Add(details[d].visitID);
                        }
                        if (GPatientStatement.viewOnly == true)
                            savePDFfileName = System.IO.Path.Combine("View", arrPatientIds[i] + "_" + datetime + ".pdf");
                        else
                            savePDFfileName = arrPatientIds[i] + "_" + datetime + ".pdf";
                        PDFfileName = System.IO.Path.Combine(DirectoryPath, savePDFfileName);
                        
                        HTMLfileName = System.IO.Path.Combine(DirectoryPath, arrPatientIds[i] + "_" + datetime + ".html");
                        
                        int detailcount = details.Count;
                        int checkcount = 0;
                        int start=0;
                        int end = 11;
                       
                        while (checkcount< detailcount)  
                        {
                            PatientVisitsIds.Clear();

                            data = @" <table style='width:100%; border:0px; solid #000; border-collapse: collapse;  font-size: 13.2px;text-align: center;'>
                <thead>
                    <tr style='height: 10px;'>
                <th  style='background-color: #adadad;border-collapse: collapse;width:70px;'>
                            Visit#  </th>

                        <th  style='background-color: #adadad;border-collapse: collapse;width:40px;'>
                            Date
                        </th>
                         <th style='background-color: #adadad;border-collapse: collapse;width:66px'>
                            CPT
                        </th>
                        <th style='background-color: #adadad;border-collapse: collapse; '>
                            Description
                        </th>
            <th style='background-color: #adadad;border-collapse: collapse; '>
                            Units
                        </th>
                        
           <th style='background-color: #adadad;border-collapse: collapse; '>
                            Charges
                        </th>
                        
        <th style='background-color: #adadad;border-collapse: collapse;     line-height: 90%; '>
                            Insurance<br>Payment
                        </th>
                        
           <th style='background-color: #adadad;border-collapse: collapse;     line-height: 90%;'>
                         Patient<br>Payment
                        </th>
                        
         <th style='background-color: #adadad;border-collapse: collapse; '>
                            Adjustment
                        </th>
                        <th style='background-color: #adadad;border-collapse: collapse; '>
                            Balance
                        </th>
                         
                    </tr>
                </thead>
                <tbody>";
                            string dos = "";
                            total = 0;
                            if (end >= detailcount)
                                end = detailcount;
                            for (int k = start; k < end; k++) // details.Count
                                                                  //for (int j = 0; j <8 ; j++) // details.Count
                                {
                                if (!PatientVisitsIds.Contains(details[k].visitID))
                                    PatientVisitsIds.Add(details[k].visitID);

                                reference = string.Empty;
                                if (details[k].Coinsurance.Val() > 0 && details[k].Deductible.Val() > 0)
                                    reference = "Co-Insurance-Deductible";
                                else if (details[k].Coinsurance.Val() > 0)
                                    reference = "Co-Insurance";
                                else if (details[k].Deductible.Val() > 0)
                                    reference = "Deductible";
                                else if (details[k].Copay.Val() > 0)
                                    reference = "Copay";
                                else reference = "Other";
                               
                                dos = details[k].DateOfServiceFrom;
                                total = details[k].patientBalance + total;
                                    string description = details[k].description !=null 
                                    ?(details[k].description).ToString().Trim('\'').Replace("\n", "").Replace("'", "").Replace("\"", "").Replace("\r\n", "").Replace(Environment.NewLine, "")
                                    :"";
                                description = Regex.Replace(description, "(?<=')(.*?)'(?=.*')", "$1");
                                if (description.Length > 34)
                                    description = description.Substring(0, 34);
                                if (!ExtensionMethods.IsNull(details[k].DateOfServiceTo) &&
                                    details[k].DateOfServiceTo != details[k].DateOfServiceFrom)
                                {
                                dos += "-" + details[k].DateOfServiceTo;
                                end = end - 1;
                                }
                                   
                                data += @"<tr style='height: 5px; '>
                  <td  class='tableDetails'  style='width:35px;  border-collapse: collapse; padding: 0px; font-size: 12px;text-align: center; '>

                            " + details[k].visitID + @"
                        </td>
  <td  class='tableDetails'  style='width:35px;  border-collapse: collapse; padding: 0px; font-size: 12px;text-align: center; '>
                            " + dos + @"
                        </td>
                        <td   class='tableDetails'  style='width:200px ; border-collapse: collapse; padding: 0px; font-size: 12px; text-align: center;'>
                            " + details[k].cpt + @"
                        </td>
                        <td   class='tableDetails'style='width:381px;border-collapse: collapse;padding: 0px;font-size:8px;text-align:left;overflow: hidden;white-space: nowrap;' >
                           '" + description + @"

                        </td>
                <td   class='tableDetails'  style='width:93px ; border-collapse: collapse; padding: 0px; font-size: 12px;text-align: center;' >
                               " + details[k].units + @"
                                      </td> 
               <td   class='tableDetails'  style='width:93px ; border-collapse: collapse; padding: 0px; font-size: 12px;text-align: center;' >
                               " + details[k].totalAmount + @"
                                      </td>
               <td   class='tableDetails'  style='width:93px ; border-collapse: collapse; padding: 0px; font-size: 12px;text-align: center;' >
                               " + details[k].PaidAmount + @"
                                      </td>
                <td   class='tableDetails'  style='width:93px ; border-collapse: collapse; padding: 0px; font-size: 12px;text-align: center;' >
                               " + details[k].PatientPaid + @"
                                      </td>
                <td   class='tableDetails'  style='width:93px ; border-collapse: collapse; padding: 0px; font-size: 12px;text-align: center;' >
                               " + details[k].WriteOff + @"
                                      </td>
                      
                <td   class='tableDetails'  style='width:110px ;   border-collapse: collapse; padding: 0px; font-size: 12px;text-align: center;' >
                               " + details[k].patientBalance + @"
                                      </td>
                    </tr>
                     <tr>
                     <td colspan='5' style='font-size:10px; text-align:left; padding-left: 12px;'>
                       <strong>Attending</strong> :&nbsp; " + details[k].AttendingProviderName + @"
                               </td>
                     <td colspan='5'style='font-size:10px;text-align:left;'>
                        <strong> Reference</strong> :&nbsp;  " + reference + @"
                                </td></tr>
                       ";

                                    checkcount++;
                            }
                            data += @" </tbody></table>";
                           
                         
                            {
                                
                                
                                for (int c = 0; c < PatientVisitsIds.Count; c++)
{
                                                                    Visit visit = _context.Visit.Find(PatientVisitsIds[c]);
                                    visit.StatementStatus = visit.StatementStatus == null ? 1 : (visit.StatementStatus + 1);
                                    visit.LastStatementDate = DateTime.Now;

                                   

                                    decimal totalBalance = details.Sum(s => s.Balance);
                                    if (GPatientStatement.viewOnly == false)
                                        patStatement = SavePatientStatementLog(visit, Convert.ToDecimal(total), savePDFfileName, saveCSVfileName, GPatientStatement.statementMessage, GPatientStatement.reportType, UD);

                                    var detailsFiltered = details.FindAll(e => e.visitID == visit.ID);
                                    for (int z = 0; z < detailsFiltered.Count; z++)
                                    {
                                        if (GPatientStatement.viewOnly == false)
                                        {
                                            PatientStatementChargeHistory patStatementChargeHistory = new PatientStatementChargeHistory();
                                            patStatementChargeHistory.PatientStatementID = patStatement.ID;
                                            patStatementChargeHistory.amount = total;
                                            patStatementChargeHistory.ChargeID = detailsFiltered[z].chargeID;
                                            patStatementChargeHistory.ChargeID = detailsFiltered[z].chargeID;
                                            patStatementChargeHistory.AddedBy = UD.Email;
                                            patStatementChargeHistory.AddedDate = DateTime.Now;
                                            patStatementChargeHistory.UpdatedBy = UD.Email;
                                            patStatementChargeHistory.UpdatedDate = DateTime.Now;
                                            _context.PatientStatementChargeHistory.Add(patStatementChargeHistory);
                                        }


                                        PatientFollowUpCharge followUpCharge =
                                            (from patFol in _context.PatientFollowUp
                                             join patFolChar in _context.PatientFollowUpCharge
                                             on patFol.ID equals patFolChar.PatientFollowUpID
                                             where patFol.PatientID.ToString() == arrPatientIds[i].ToString() && patFolChar.ChargeID == detailsFiltered[z].chargeID
                                             select patFolChar).FirstOrDefault();
                                        if (followUpCharge != null)
                                        {
                                            if (followUpCharge.Statement1SentDate == null)
                                            {
                                                followUpCharge.Statement1SentDate = statementDate;
                                                followUpCharge.Status = "1st Statement Sent";
                                            }
                                            else if (followUpCharge.Statement1SentDate != null && followUpCharge.Statement2SentDate == null)
                                            {
                                                followUpCharge.Statement2SentDate = statementDate;
                                                followUpCharge.Status = "2nd Statement Sent";
                                            }
                                            else if (followUpCharge.Statement2SentDate != null && followUpCharge.Statement3SentDate == null)
                                            {
                                                followUpCharge.Statement3SentDate = statementDate;
                                                followUpCharge.Status = "3rd Statement Sent";
                                            }
                                            if (followUpCharge.Statement1SentDate != null && followUpCharge.Statement2SentDate != null
                                                && followUpCharge.Statement3SentDate != null)
                                            {
                                                followUpCharge.Status = "Collection Agency";
                                            }

                                        }



                                        if (practice.StatementExportType == "PLD")
                                        {
                                            vmPatientStatementPLDCSV = new VMPatientStatementPLDCSV();
                                            vmPatientStatementPLDCSV.StatementNo = (patStatement==null?"":patStatement.ID.ToString());
                                            vmPatientStatementPLDCSV.BillingStatement = "1";
                                            vmPatientStatementPLDCSV.OfficeName = practice.OrganizationName;
                                            vmPatientStatementPLDCSV.Address = practice.Address1;
                                            vmPatientStatementPLDCSV.City = practice.City;
                                            vmPatientStatementPLDCSV.State = practice.State;
                                            vmPatientStatementPLDCSV.ZipCode = practice.ZipCode;
                                            vmPatientStatementPLDCSV.Phone = practice.OfficePhoneNum;
                                            vmPatientStatementPLDCSV.StatementDate = DateTime.Now.ToString("MM/dd/yyyy").Trim();
                                            vmPatientStatementPLDCSV.PatientName = header[0].patient;
                                            vmPatientStatementPLDCSV.AccountNo = header[0].patientAccountNum;
                                            vmPatientStatementPLDCSV.PatientAddress1 = header[0].patientAddress;
                                            vmPatientStatementPLDCSV.PatientAddress2 = header[0].patientAddress2;
                                            vmPatientStatementPLDCSV.BillToName = header[0].patient;
                                            vmPatientStatementPLDCSV.BillToAddress1 = header[0].patientAddress;
                                            vmPatientStatementPLDCSV.BillToAddress2 = header[0].patientAddress2;
                                            vmPatientStatementPLDCSV.VisitID = visit.ID.ToString();
                                            vmPatientStatementPLDCSV.DateOfService = visit.DateOfServiceFrom.Value.Format("MM/dd/yyyy");//.ToString("MM/dd/yyyy").Trim();
                                            vmPatientStatementPLDCSV.CPT = detailsFiltered[z].cpt;
                                            vmPatientStatementPLDCSV.Procedure = detailsFiltered[z].description;
                                            vmPatientStatementPLDCSV.Quantity = detailsFiltered[z].units;
                                            vmPatientStatementPLDCSV.Charge = detailsFiltered[z].totalAmount.ToString();
                                            vmPatientStatementPLDCSV.InsurancePayment = detailsFiltered[z].PaidAmount.ToString();
                                            vmPatientStatementPLDCSV.PatientPayment = detailsFiltered[z].PatientPaid.ToString();
                                            vmPatientStatementPLDCSV.Adjustment = detailsFiltered[z].WriteOff.ToString();
                                            vmPatientStatementPLDCSV.CurrentDue = "";
                                            if (detailsFiltered[z].Aging > 0 && detailsFiltered[z].Aging < 31)
                                                vmPatientStatementPLDCSV.Over30Days = detailsFiltered[z].patientBalance.ToString();
                                            else
                                                vmPatientStatementPLDCSV.Over30Days = "";
                                            if (detailsFiltered[z].Aging > 30 && detailsFiltered[z].Aging < 61)
                                                vmPatientStatementPLDCSV.Over60Days = detailsFiltered[z].patientBalance.ToString();
                                            else
                                                vmPatientStatementPLDCSV.Over60Days = "";
                                            if (detailsFiltered[z].Aging > 60)
                                                vmPatientStatementPLDCSV.Over90Days = detailsFiltered[z].patientBalance.ToString();
                                            else
                                                vmPatientStatementPLDCSV.Over90Days = "";
                                            vmPatientStatementPLDCSV.Balance = detailsFiltered[z].Balance.ToString();
                                            vmPatientStatementPLDCSV.TotalBalance = totalBalance.ToString();// (totalBalance!=null && totalBalance>0) totalBalance.ToString();// detailsFiltered[z].TotalBalance.ToString();
                                            reference = string.Empty;
                                            if (detailsFiltered[z].Coinsurance.Val() > 0 && detailsFiltered[z].Deductible.Val() > 0)
                                                reference = "Co-Insurance-Deductible";
                                            else if (detailsFiltered[z].Coinsurance.Val() > 0)
                                                reference = "Co-Insurance";
                                            else if (detailsFiltered[z].Deductible.Val() > 0)
                                                reference = "Deductible";
                                            else if (detailsFiltered[z].Copay.Val() > 0)
                                                reference = "Copay";
                                            else reference = "Other";
                                            vmPatientStatementPLDCSV.Reference = reference;
                                             vmPatientStatementPLDCSV.LastPaymentDate = lastPaymentDate;// detailsFiltered[z].LastPaymentDate!=null? detailsFiltered[z].LastPaymentDate.Value.Format("MM/dd/yyyy"):"";
                                             vmPatientStatementPLDCSV.Attending = detailsFiltered[z].AttendingProviderName;
                                            vmPatientStatementPLDCSV.emailAddress = header[0].patientEmail;
                                            vmPatientStatementPLDCSV.Comments = "";
                                            lstVMPatientStatementPLDCSV.Add(vmPatientStatementPLDCSV);
                                        }
                                    } 
                                } 
                            }
                           
                            string statementHTML = System.IO.File.ReadAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Resources/statement", "statement.html"));
                            statementHTML = statementHTML.Replace("$PRACTICE_NAME", header[0].practiceName.Trim());
                            statementHTML = statementHTML.Replace("$PRACTICE_Address", header[0].practiceAddress);
                            statementHTML = statementHTML.Replace("$PRACTICE_City", header[0].practiceCity);
                            statementHTML = statementHTML.Replace("$PRACTICE_State", header[0].practiceState);
                            statementHTML = statementHTML.Replace("$PRACTICE_Zip", header[0].practiceZipCode);
                            statementHTML = statementHTML.Replace("$PRACTICE_Email", header[0].practiceEmail);
                            statementHTML = statementHTML.Replace("$Billing_Phone", header[0].StatementPhoneNumber);
                            statementHTML = statementHTML.Replace("$Billing_Fax", header[0].StatementFaxNumber);
                            statementHTML = statementHTML.Replace("$Appointment_Phone", header[0].AppointmentPhoneNumber);
                            statementHTML = statementHTML.Replace("$PATIENT_Name", header[0].patient.Trim());
                            statementHTML = statementHTML.Replace("$PATIENT_Address", header[0].patientAddress.Trim());
                            statementHTML = statementHTML.Replace("$PATIENT_City", header[0].patientCity.Trim());
                            statementHTML = statementHTML.Replace("$PATIENT_State", header[0].patientState);
                            statementHTML = statementHTML.Replace("$PATIENT_Zip", header[0].patientZipCode);
                            statementHTML = statementHTML.Replace("$PATIENT_Id", header[0].patientAccountNum);
                            statementHTML = statementHTML.Replace("$STATEMENT_Date", DateTime.Now.Date.ToString("MM/dd/yyyy"));
                            statementHTML = statementHTML.Replace("$L_Pay_Date", lastPaymentDate);

                            statementHTML = statementHTML.Replace("$STATEMENT_DATA", data);
                            statementHTML = statementHTML.Replace("$AMOUNT", "$" + total.ToString());
                            statementHTML = statementHTML.Replace("$URL", resourcesPath);
							if(GPatientStatement.statementMessage.Length>772)
                            statementHTML = statementHTML.Replace("$MESSAGE", GPatientStatement.statementMessage.Substring(0,772));
						else
							statementHTML = statementHTML.Replace("$MESSAGE", GPatientStatement.statementMessage);
                            FinalSatatment += statementHTML;
                            start = end;
                            end += 11; 
                        } 
                    }
                }
                else if (GPatientStatement.reportType.ToLower() == "summary")
                {
                    var details = (from p in _context.Practice
                                   join pt in _context.Patient on p.ID equals pt.PracticeID
                                   join v in _context.Visit on pt.ID equals v.PatientID
                                   where p.ID == UD.PracticeID && pt.ID == patId
                                   && GPatientStatement.claimId.Contains(v.ID)
                                   select new
                                   {
                                       DateOfServiceFrom = v.DateOfServiceFrom.Value.ToString("MM/dd/yyyy"),
                                       DateOfServiceTo = v.DateOfServiceTo.HasValue ? v.DateOfServiceTo.Value.ToString("MM/dd/yyyy") : "",
                                       v.TotalAmount,
                                       visitID = v.ID,
                                       allowedAmount = v.PrimaryAllowed,
                                       v.PrimaryPaid,
                                       v.SecondaryPaid,
                                       v.PatientPaid,
                                       patientBalance = v.PrimaryPatientBal.Val()
                                      + v.SecondaryPatientBal.Val()
                                      + v.TertiaryPatientBal.Val()
                                   }
                                           ).ToList();
                    if (details != null && details.Count>0)
                    {
                        PatientVisitsIds.Clear();
                        for (int d = 0; d < details.Count; d++)
                        {
                            if (!PatientVisitsIds.Contains(details[d].visitID))
                                PatientVisitsIds.Add(details[d].visitID);
                        }
                        if (GPatientStatement.viewOnly == true)
                            savePDFfileName = System.IO.Path.Combine("View", arrPatientIds[i] + "_" + datetime + ".pdf");
                        else
                            savePDFfileName = arrPatientIds[i] + "_" + datetime + ".pdf";
                        PDFfileName = System.IO.Path.Combine(DirectoryPath, savePDFfileName);
                        PDFfileName = System.IO.Path.Combine(DirectoryPath, savePDFfileName);
                        savePDFfileName = arrPatientIds[i] + "_" + datetime + ".pdf";
                        HTMLfileName = System.IO.Path.Combine(DirectoryPath, arrPatientIds[i] + "_" + datetime + ".html");

                        int detailcount = details.Count;
                        int checkcount = 0;
                        int start = 0;
                        int end = 12;

                        while (checkcount < detailcount)
                           
                        {
                            data = @" <table style='width:95%; margin-top: 10px;border-collapse: collapse;'>
                <thead>
                    <tr>
                        <th  style='background-color: #adadad;border:1px solid #000;border-collapse: collapse;width:30px;text-align: center;'>
                            DATE
                        </th> 
                        <th colspan='2' style='background-color: #adadad;border:1px solid #000;border-collapse: collapse;padding: 8px; width: 93px;text-align: center;'>
                            AMOUNT 
                        </th>
                         
                    </tr>
                </thead>
                <tbody>";
                            string dos = "";
                            total = 0;
                            if (end >= detailcount)
                                end = detailcount;
                               for (int k = start; k < end; k++) 
                                {
                                dos = details[k].DateOfServiceFrom;
                                total += details[k].patientBalance.Val();
                                if (!ExtensionMethods.IsNull(details[k].DateOfServiceTo) &&
                                    details[k].DateOfServiceTo != details[k].DateOfServiceFrom)
                                    dos += "-" + details[k].DateOfServiceTo;
                                Visit visit = _context.Visit.Find(details[k].visitID);
                                visit.StatementStatus = visit.StatementStatus == null ? 0 : (visit.StatementStatus + 1);
                                visit.LastStatementDate = DateTime.Now;
                                if (GPatientStatement.viewOnly == false)
                                    patStatement = SavePatientStatementLog(visit,Convert.ToDecimal(total), savePDFfileName, saveCSVfileName, GPatientStatement. statementMessage, GPatientStatement.reportType, UD);
                                data += @"<tr>
                        <td class='tableDetails'  style='width:30px;  border-collapse: collapse; padding: 8px; font-size: 16px; text-align: center;'>
                            " + dos + @"
                        </td>
                        <td  class='tableDetails'  style='width:93px ; border-collapse: collapse; padding: 8px; font-size: 16px;text-align: center;' >
                               " + details[k].patientBalance.Val() + @"
                        </td> 
                    </tr>";
                                checkcount++;
                            }
                            data += @" </tbody></table>";
                            string statementHTML = System.IO.File.ReadAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Resources/statement", "statement.html"));
                            statementHTML = statementHTML.Replace("$PRACTICE_NAME", header[0].practiceName);
                            statementHTML = statementHTML.Replace("$PRACTICE_Address", header[0].practiceAddress);
                            statementHTML = statementHTML.Replace("$PRACTICE_City", header[0].practiceCity);
                            statementHTML = statementHTML.Replace("$PRACTICE_State", header[0].practiceState);
                            statementHTML = statementHTML.Replace("$PRACTICE_Zip", header[0].practiceZipCode);
                            statementHTML = statementHTML.Replace("$PRACTICE_Email", header[0].practiceEmail);
                            statementHTML = statementHTML.Replace("$Billing_Phone", header[0].StatementPhoneNumber);
                            statementHTML = statementHTML.Replace("$Billing_Fax", header[0].StatementFaxNumber);
                            statementHTML = statementHTML.Replace("$Appointment_Phone", header[0].AppointmentPhoneNumber);
                            statementHTML = statementHTML.Replace("$PATIENT_Name", header[0].patient);
                            statementHTML = statementHTML.Replace("$PATIENT_Address", header[0].patientAddress);
                            statementHTML = statementHTML.Replace("$PATIENT_City", header[0].patientCity);
                            statementHTML = statementHTML.Replace("$PATIENT_State", header[0].patientState);
                            statementHTML = statementHTML.Replace("$PATIENT_Zip", header[0].patientZipCode);
                            statementHTML = statementHTML.Replace("$PATIENT_Id", header[0].patientAccountNum);
                            statementHTML = statementHTML.Replace("$STATEMENT_Date", DateTime.Now.Date.ToString("MM/dd/yyyy"));
                            statementHTML = statementHTML.Replace("$STATEMENT_DATA", data);
                            statementHTML = statementHTML.Replace("$AMOUNT", "$" + total.ToString());
                            statementHTML = statementHTML.Replace("$URL", resourcesPath);
                            statementHTML = statementHTML.Replace("$MESSAGE", GPatientStatement.statementMessage);
                            FinalSatatment += statementHTML;
                            start = end;
                            end += 12; 
                        }
                        }
                    }
                else
                {
                    return BadRequest("Please select Report Type.");
                }
                if (FinalSatatment != "")
                {
                    PdfWriter writer = new PdfWriter(PDFfileName);
                    PdfDocument pdfDocument = new PdfDocument(writer);
                    Document doc = new Document(pdfDocument);
                    pdfDocument.SetDefaultPageSize(PageSize.A4);
                   
                    PageSize pg = pdfDocument.GetDefaultPageSize();
                    string send = header[0].practiceAddress + " " + header[0].practiceCity + " " + header[0].practiceState + " " + header[0].practiceZipCode;
                    pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, new TextFooterEventHandler(doc, header[0].practiceName, send, _context));
                    HtmlConverter.ConvertToPdf(FinalSatatment, pdfDocument, null);
                   
                    doc.Close();




                    // writer.PageEvent = new Footer();

                    //Paragraph welcomeParagraph = new Paragraph("Hello, World");

                    //document.Add(welcomeParagraph);
                    //iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);
                    //hw.Parse(new StringReader(FinalSatatment));

                    // document.Close();









                    System.IO.File.WriteAllText(HTMLfileName, FinalSatatment);
                }
                VMPatientStatementFile file = new VMPatientStatementFile();
                file.ID = i;
                file.patient = header[0].patient;
                file.claimIds = string.Join(",", PatientVisitsIds);
                file.FileName = arrPatientIds[i] + "_" + datetime + ".pdf";
                file.patientId = header[0].patientID.ToString();
                file.pdf_id = (patStatement==null?"":patStatement.ID.ToString());
                file.pdf_url = savePDFfileName.Replace("\\","**");
                lstFiles.Add(file);
                PatientFollowUp planFollowup = _context.PatientFollowUp.Where(w => w.PatientID == patId).FirstOrDefault();
                if (planFollowup.Statement1SentDate == null)
                {
                    planFollowup.Statement1SentDate = DateTime.Now;
                    planFollowup.Status = "1st Statement Sent";
                }
                else if (planFollowup.Statement1SentDate != null && planFollowup.Statement2SentDate == null)
                {
                    planFollowup.Statement2SentDate = DateTime.Now;
                    planFollowup.Status = "2nd Statement Sent";
                }
                else if (planFollowup.Statement2SentDate != null && planFollowup.Statement3SentDate == null)
                {
                    planFollowup.Statement3SentDate = DateTime.Now;
                    planFollowup.Status = "3rd Statement Sent";
                }
                if (planFollowup.Statement1SentDate != null && planFollowup.Statement2SentDate != null
                    && planFollowup.Statement3SentDate != null)
                {
                    planFollowup.Status = "Collection Agency";
                }

            }
            if (GPatientStatement.viewOnly == false)
                await _context.SaveChangesAsync();
            if (Directory.Exists(CSVfileName))
                System.IO.File.WriteAllText(CSVfileName, string.Empty);
            //string line = @"Statement No. ,Billing Statement,Office Name,Address,City,State,ZipCode,Phone,Statement Date,Patient Name,Account #,Patient Address1,Patient Address2,Bill To Name,Bill To Address1,Bill To Address2,Visit ID,Date Of Service,CPT,Procedure,Quantity,Charge,Insurance Payment,Patient Payment,Adjustment,Current Due,Over 30 Days,Over 60 Days,Over 90 Days,Balance,Total Balance,Reference,email Address,Comments";
            string line = @"Statement No. ,Billing Statement,Office Name,Address,City,State,ZipCode,Phone,Statement Date,Patient Name,Account #,Patient Address1,CityStateZip,Bill To Name,Bill To Address1,CityStateZip,Visit ID,Date Of Service,CPT,Procedure,Quantity,Charge,Insurance Payment,Patient Payment,Adjustment,Current Due,Over 30 Days,Over 60 Days,Over 90 Days,Balance,Total Balance,Reference,email Address,Comments,Attending,ProviderName,LastPaymentDate";
             System.IO.File.AppendAllText(CSVfileName, line); 
            foreach (var VMPatientStatementPLDCSV in lstVMPatientStatementPLDCSV)
            {
                line = Environment.NewLine + @"" + VMPatientStatementPLDCSV.StatementNo + ",1," + VMPatientStatementPLDCSV.OfficeName + "," +
                    VMPatientStatementPLDCSV.Address + "," + VMPatientStatementPLDCSV.City + "," + VMPatientStatementPLDCSV.State +
                    "," + VMPatientStatementPLDCSV.ZipCode + "," + VMPatientStatementPLDCSV.Phone + "," + VMPatientStatementPLDCSV.StatementDate + "," + VMPatientStatementPLDCSV.PatientName + "," + VMPatientStatementPLDCSV.AccountNo
                    + "," + VMPatientStatementPLDCSV.PatientAddress1 + "," + VMPatientStatementPLDCSV.PatientAddress2 + "," + VMPatientStatementPLDCSV.BillToName + "," + VMPatientStatementPLDCSV.BillToAddress1
                    + "," + VMPatientStatementPLDCSV.BillToAddress2 + "," + VMPatientStatementPLDCSV.VisitID + "," + VMPatientStatementPLDCSV.DateOfService + "," + VMPatientStatementPLDCSV.CPT + ","+ ((VMPatientStatementPLDCSV.Procedure!=null && VMPatientStatementPLDCSV.Procedure.Length>0 ) ? VMPatientStatementPLDCSV.Procedure.Replace(",", " ") :"")  
                   + "," + VMPatientStatementPLDCSV.Quantity + "," + VMPatientStatementPLDCSV.Charge + "," + VMPatientStatementPLDCSV.InsurancePayment
                    + "," + VMPatientStatementPLDCSV.PatientPayment + "," + VMPatientStatementPLDCSV.Adjustment + "," + VMPatientStatementPLDCSV.CurrentDue + "," + VMPatientStatementPLDCSV.Over30Days
                    + "," + VMPatientStatementPLDCSV.Over60Days + "," + VMPatientStatementPLDCSV.Over90Days + "," + VMPatientStatementPLDCSV.Balance
                    + "," + VMPatientStatementPLDCSV.TotalBalance + "," + VMPatientStatementPLDCSV.Reference + "," + VMPatientStatementPLDCSV.emailAddress + "," + VMPatientStatementPLDCSV.Comments + "," + "\"" + VMPatientStatementPLDCSV.Attending + "\"" + "," + "\"" + VMPatientStatementPLDCSV.ProviderName + "\"" + "," + VMPatientStatementPLDCSV.LastPaymentDate + "";
                System.IO.File.AppendAllText(CSVfileName, line);
            }

          // Code By Aziz Sab 05122020
            string patientStatementID = lstVMPatientStatementPLDCSV != null && lstVMPatientStatementPLDCSV.Count > 0 ? lstVMPatientStatementPLDCSV[0].StatementNo : "";
            var a = new { data = lstFiles, csv = saveCSVfileName.Replace("\\", "**"), StatementID = patientStatementID };


            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);
            return Json(a);
        }

        private PatientStatement SavePatientStatementLog(Visit visit,decimal totalBalance, string PDFfileName, string CSVfileName, string StatementMessage, string reportType, UserInfoData UD)
        {


            PatientStatement patStatement = new PatientStatement();
            patStatement.PatientID = visit.PatientID;
            patStatement.VisitID = visit.ID;
            patStatement.pdf_url = PDFfileName;
            patStatement.csv_url = CSVfileName;
           // patStatement.csv_url = CSVfileName;
            patStatement.amount = totalBalance;
            patStatement.statementStatus = visit.StatementStatus == null ? 0 : (visit.StatementStatus.Value);
            patStatement.Message = StatementMessage ?? "";
            patStatement.Type = reportType;
            patStatement.PracticeID = UD.PracticeID;
            patStatement.AddedBy = UD.Email;
            patStatement.AddedDate = DateTime.Now;
            patStatement.UpdatedBy = UD.Email;
            patStatement.UpdatedDate = DateTime.Now;
            _context.PatientStatement.Add(patStatement);
            _context.SaveChanges();
            VisitController visitC = new VisitController(_context, _contextMain);
            visitC.ControllerContext = this.ControllerContext;
            visitC.SaveVisit(visit).GetAwaiter().GetResult();

            return patStatement;
        }


        [Route("UploadPLDFile/{patStatementID}")]
        [HttpGet("{id}")]
        public async Task<ActionResult> UploadPLDFile(long patStatementID)
        {
            //if (ViewOnly == true)
            //{

            //}
            string filePath = string.Empty;
            string temp = User.Claims.FirstOrDefault(x => x.Type.Equals("TEMP", StringComparison.InvariantCultureIgnoreCase)).Value;

            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
            );

            Settings settings = await _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefaultAsync();
            if (settings == null)
            {
                return BadRequest("Document Server not found");
            }

            MainPractice prac = _contextMain.MainPractice.Find(UD.PracticeID);

            if (settings == null || prac.PLDDirectory.IsNull())
            {
                return BadRequest("PLD Directory is not setup");
            }

            PatientStatement patStatement = await _context.PatientStatement.FindAsync(patStatementID);
            string remotecSVPath = string.Empty;
            if (patStatement != null)
            {
                string remotePath = System.IO.Path.Combine("\\\\", settings.DocumentServerURL,
                        settings.DocumentServerDirectory, UD.ClientID.ToString(), "PatientStatement");

                remotecSVPath = System.IO.Path.Combine(remotePath, patStatement.csv_url);
                if (!System.IO.File.Exists(remotecSVPath))
                {
                    return BadRequest("File not found on Server");
                }
            }


            string directoryPath = System.IO.Path.Combine("\\\\", settings.DocumentServerURL,
            settings.DocumentServerDirectory, temp, "PLD",
            DateTime.Now.ToString("MMddyyyy"), DateTime.Now.ToString("hhmmssff"));
            Directory.CreateDirectory(directoryPath);
            // filePath = System.IO.Path.Combine(directoryPath, System.IO.Path.GetFileName(remotecSVPath));
            // BellMedEx- BEYOND THE BRAINS_04302020_010000.csv
            string filename = "BellMedEx-" + UD.PracticeName + "_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("hhmmss") + ".csv";
            filePath = System.IO.Path.Combine(directoryPath, filename);
            System.IO.File.Copy(remotecSVPath, filePath);

            MediFusionPM.BusinessLogic.SFTPSubmission submission = new BusinessLogic.SFTPSubmission("ftp3.thepldgroupinc.com", "MediFusion", "DdY7m1sV", 22, "SFTP");
            submission.SubmitFile(prac.PLDDirectory, filePath);

            return Ok("File Uploaded Successfully");
        }

    }
}
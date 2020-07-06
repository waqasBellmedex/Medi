using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;
using MediFusionPM.Models.TodoApi;
using MediFusionPM.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static MediFusionPM.ViewModels.VMCommon;
using static MediFusionPM.ViewModels.VMJobs;
using MediFusionPM.Models.Main;
using MediFusionPM.Uitilities;

namespace MediFusionPM.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AutoPlanFollowUpController : ControllerBase
    {

        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;

        public AutoPlanFollowUpController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;
        }

        [Route("SaveAutoFollowUp")]
        [HttpPost]
        public async Task<ActionResult<VMAutoPlanFollowUp>> SaveAutoFollowUp()
     
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            List<CreateFollowUpTable> AutoPlan = (from v in _context.Visit
                                                  join Prac in _context.Practice on v.PracticeID equals Prac.ID
                                                  join pPlan in _context.PatientPlan on v.PrimaryPatientPlanID equals pPlan.ID
                                                  join iPlan in _context.InsurancePlan on pPlan.InsurancePlanID equals iPlan.ID
                                                  where
                                                  Prac.ID == PracticeId &&
                                                  v.IsSubmitted == true &&
                                                 ((DateTime.Now - v.SubmittedDate.Date()).Days >= (iPlan.OutstandingDays.IsNull() ? 3 : 1)) &&
                                                 (v.PrimaryPaid.IsNull() || v.PrimaryPaid.Val() == 0)
                                                  let ces = from Vi in _context.PlanFollowUp
                                                  select Vi.ID
                                                  where (!ces.Contains(v.ID))
                                                  select new CreateFollowUpTable()
                                                  {
                                                      VisitID = v.ID,
                                                      LocationID = v.LocationID,
                                                      ProviderID = v.ID,
                                                      PrimaryPatientPlanID = v.ID,
                                                      PatientID = v.ID,
                                                      SubmittedDateID = v.ID,
                                                      PrimaryPaidID = v.ID,
                                                      PracticeID = v.ID,
                                                      PlanID = pPlan.ID,
                                                      InsuranceplanID = pPlan.ID,
                                                  }).ToList();

            var reason = (from r in _context.Reason
                          where r.Name == "System"
                          select r.ID).FirstOrDefault();
            var action = (from r in _context.Action
                          where r.Name == "New"
                          select r.ID).FirstOrDefault();

            var group = (from r in _context.Group
                         where r.Name == "System"
                         select r.ID).FirstOrDefault();

            int inserted = 0;

            for (int i = 0; i < AutoPlan.Count; i++)
            {
                PlanFollowup Obj = new PlanFollowup();
                Obj.ActionID = action;
                Obj.ReasonID = reason;
                Obj.ActionID = action;
                Obj.GroupID = group;
                Obj.AddedBy = Email;
                Obj.AddedDate = DateTime.Now;


                //lstInsertData.Add(Obj);

               _context.PlanFollowUp.Add(Obj);
                
                inserted++;

            }
            _context.SaveChanges();
            VMAutoPlanFollowUp VmAutoPlanFollowUp = new VMAutoPlanFollowUp();
            VmAutoPlanFollowUp.PracticeID = PracticeId;
            VmAutoPlanFollowUp.AddedBy = Email;
            VmAutoPlanFollowUp.AddedDate = DateTime.Now;
            VmAutoPlanFollowUp.UpdatedDate = null;
            VmAutoPlanFollowUp.TotalRecords = AutoPlan.Count;
            VmAutoPlanFollowUp.InsertedRecords = inserted;
            return VmAutoPlanFollowUp;
        }

        [Route("SaveFollowUpLog")]
        [HttpPost]
        public async Task<ActionResult<AutoPlanFollowUpLog>> SaveFollowUpLog(AutoPlanFollowUpLog Autoplanfollowup)
        {
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            bool succ = TryValidateModel(Autoplanfollowup);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }

            if (Autoplanfollowup.ID == 0)
            {
                Autoplanfollowup.AddedBy = Email;
                Autoplanfollowup.AddedDate = DateTime.Now;
                //  Autoplanfollowup.ServiceStartTime = DateTime.Now;
                Autoplanfollowup.LogMessage = Autoplanfollowup.LogMessage;
                _contextMain.Add(Autoplanfollowup);
                await _contextMain.SaveChangesAsync();
            }
            else
            {
                Autoplanfollowup.UpdatedBy = Email;
                Autoplanfollowup.UpdatedDate = DateTime.Now;
                //Autoplanfollowup.ServiceStartTime = DateTime.Now;
                _contextMain.Update(Autoplanfollowup);
                await _contextMain.SaveChangesAsync();
            }
            List<Models.Main.AutoPlanFollowUpLog> logs = _contextMain.AutoPlanFollowUpLog.Where(p => p.AddedDate.Date == DateTime.Now.Date).ToList();
            var table = logs.ToHtmlTable();
            return Ok(Autoplanfollowup);
        }

        [Route("GetTodaysLog")]
        [HttpGet()]
        public string  GetTodaysLog()
        {
            //List<Models.Main.AutoPlanFollowUpLog> logs = _contextMain.AutoPlanFollowUpLog.Where(p => p.AddedDate.Date == DateTime.Now.Date).ToList();
            var logs = (from log in _contextMain.AutoPlanFollowUpLog
                        join practice in _contextMain.MainPractice on log.PracticeID equals practice.ID
                        where log.AddedDate.Date == DateTime.Now.Date
                        select new
                        {
                            ID = log.ID,
                            AddedDate = log.AddedDate,
                            PracticeID = log.PracticeID,
                            PracticeName = practice.Name,
                            TotalRecords = log.TotalRecords,
                            FollowUpCreated = log.FollowUpCreated,
                            Message = log.LogMessage
                        }).ToList();
            var table = logs.ToHtmlTable();
            return table; 
        }


    }
}


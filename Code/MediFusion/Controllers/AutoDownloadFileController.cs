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
    public class AutoDownloadFileController : ControllerBase

    {

        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
       
        public AutoDownloadFileController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
        }

        [Route("SaveDownloadLog")]
        [HttpPost]
        public async Task<ActionResult<AutoDownloadingLog>> SaveDownloadLog(AutoDownloadingLog AutoDownloadLog)
        {
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            bool succ = TryValidateModel(AutoDownloadLog);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }

            if (AutoDownloadLog.ID == 0)
            {
                AutoDownloadLog.AddedBy = Email;
                AutoDownloadLog.AddedDate = DateTime.Now;
                //  Autoplanfollowup.ServiceStartTime = DateTime.Now;
                AutoDownloadLog.LogMessage = AutoDownloadLog.LogMessage;
                _contextMain.Add(AutoDownloadLog);
                await _contextMain.SaveChangesAsync();
            }
            else
            {
                AutoDownloadLog.UpdatedBy = Email;
                AutoDownloadLog.UpdatedDate = DateTime.Now;
                //Autoplanfollowup.ServiceStartTime = DateTime.Now;
                _contextMain.Update(AutoDownloadLog);
                await _contextMain.SaveChangesAsync();
            }
            List<Models.Main.AutoPlanFollowUpLog> logs = _contextMain.AutoPlanFollowUpLog.Where(p => p.AddedDate.Date == DateTime.Now.Date).ToList();
            var table = logs.ToHtmlTable();
            return Ok(AutoDownloadLog);
        }

        [Route("GetTodaysLog")]
        [HttpGet()]
        public string GetTodaysLog()
        {
            //List<Models.Main.AutoPlanFollowUpLog> logs = _contextMain.AutoPlanFollowUpLog.Where(p => p.AddedDate.Date == DateTime.Now.Date).ToList();
            var logs = (from log in _contextMain.AutoDownloadingLog
                        join practice in _contextMain.MainPractice on log.PracticeID equals practice.ID
                        where log.AddedDate.Date == DateTime.Now.Date
                        select new
                        {
                            ID = log.ID,
                            AddedDate = log.AddedDate,
                            PracticeID = log.PracticeID,
                            PracticeName = practice.Name,
                            Message = log.LogMessage,
                            ExceptionMessage = log.Exception
                        }).ToList();
            var table = logs.ToHtmlTable();
            return table;
        }


    }
}
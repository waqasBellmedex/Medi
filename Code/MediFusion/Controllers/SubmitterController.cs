using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediFusionPM.Models;
using MediFusionPM.ViewModels;
using static MediFusionPM.ViewModels.VMCommon;
using static MediFusionPM.ViewModels.VMSubmitter;
using MediFusionPM.Models.Audit;
using MediFusionPM.Models.Main;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmitterController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;

        public SubmitterController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
        }

        // GET: api/Submitters
        [HttpGet]
        [Route("GetSubmitters")]
        public async Task<ActionResult<IEnumerable<Submitter>>> GetSubmitters()
        {
            return await _context.Submitter.ToListAsync();
        }
        [Route("GetProfiles")]
        [HttpGet()]
        public async Task<ActionResult<VMSubmitter>> GetProfiles(long id)
        {
            ViewModels.VMSubmitter obj = new ViewModels.VMSubmitter();
            obj.GetProfiles(_context);

            return obj;
        }
        [Route("FindSubmitter/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Submitter>> FindSubmitter(long id)
        {
            var po = await _context.Submitter.FindAsync(id);

            if (po == null)
            {
                return NotFound();
            }

            return po;
        }

        [HttpPost]
        [Route("FindSubmitters")]
        public async Task<ActionResult<IEnumerable<GSubmitter>>> FindSubmitters(CSubmitter CSubmit)
        {
            //  UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.submitterSearch == false)
            //return BadRequest("You Don't Have Rights. Please Contact BellMedex.");

            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindSubmitters(CSubmit, PracticeId);
        }
        private List<GSubmitter> FindSubmitters(CSubmitter CSubmit, long PracticeId)
        {
            List<GSubmitter> data = (from s in _context.Submitter
                          join r in _context.Receiver on s.ReceiverID equals r.ID
                          where
                          (CSubmit.Name == null ? true : s.Name.Contains(CSubmit.Name)) 
                          //(CSubmit.Address == null ? true : s.Address.Contains(CSubmit.Address))
                          select new GSubmitter()
                          {
                              ID = s.ID,
                              Name = s.Name,
                              Address = s.Address,
                              ReceiverID = r.ID,
                              ReceiverName = r.Name

                          }).ToList();
            return data;
        }
        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CSubmitter CSubmit)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GSubmitter> data = FindSubmitters(CSubmit, PracticeId);
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CSubmit, "Submit Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CSubmitter CSubmit)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GSubmitter> data = FindSubmitters(CSubmit, PracticeId);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }



        // POST: api/Submitters
        [HttpPost]
        [Route("SaveSubmitter")]
        public async Task<ActionResult<Submitter>> SaveSubmitters(Submitter item)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (UD == null || UD.Rights == null || UD.Rights.submitterCreate == false)
            return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            bool SubmitterExists = _context.Submitter.Count(p => p.ClientID == UD.ClientID && p.ReceiverID == item.ReceiverID && item.ID == 0) > 0;
            if (SubmitterExists)
            {
                  return BadRequest("Submitter  With  ClientID # :" + UD.ClientID + " And " + "ReceiverID :"+item.ReceiverID + " Already Exists In Submitter please Enter unique Reciever & Client");
            }
            bool succ = TryValidateModel(item);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }

            if (item.ID == 0)
            {
                item.ClientID = UD.ClientID;
                item.AddedBy = UD.Email;
                item.AddedDate = DateTime.Now;
                _context.Submitter.Add(item);
                await _context.SaveChangesAsync();
            }
            else if (UD.Rights.submitterUpdate == true)
            {
                item.UpdatedBy = UD.Email;
                item.ClientID = UD.ClientID;
                item.UpdatedDate = DateTime.Now;
                _context.Submitter.Update(item);
                await _context.SaveChangesAsync();
            }
            else return BadRequest("You Don't Have Rights. Please Contact BellMedex.");


            Practice practice = _context.Practice.Find(UD.PracticeID);
            practice.IsAutoDownloading = item.AutoDownloading == null ? false : item.AutoDownloading.Value;
            _context.Practice.Update(practice);
            _context.SaveChanges();

            MainPractice main = _contextMain.MainPractice.Find(UD.PracticeID);
            main.IsAutoDownloading = item.AutoDownloading == null ? false : item.AutoDownloading.Value;
            _contextMain.MainPractice.Update(main);
            _contextMain.SaveChanges();

            return Ok(item);
        }

        // DELETE: api/Submitters/5
        [Route("DeleteSubmitter/{id}")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Submitter>> DeleteSubmitter(long id)
        {
            var pr = await _context.Submitter.FindAsync(id);

            if (pr == null)
            {
                return NotFound();
            }

            _context.Submitter.Remove(pr);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [Route("FindAudit/{SubmitterID}")]
        [HttpGet("{SubmitterID}")]
        public List<SubmitterAudit> FindAudit(long SubmitterID)
        {
            List<SubmitterAudit> data = (from pAudit in _context.SubmitterAudit
                                           where pAudit.SubmitterID == SubmitterID
                                           select new SubmitterAudit()
                                           {
                                               ID = pAudit.ID,
                                               SubmitterID = pAudit.SubmitterID,
                                               TransactionID = pAudit.TransactionID,
                                               ColumnName = pAudit.ColumnName,
                                               CurrentValue = pAudit.CurrentValue,
                                               NewValue = pAudit.NewValue,
                                               CurrentValueID = pAudit.CurrentValueID,
                                               NewValueID = pAudit.NewValueID,
                                               HostName = pAudit.HostName,
                                               AddedBy = pAudit.AddedBy,
                                               AddedDate = pAudit.AddedDate,
                                           }).ToList<SubmitterAudit>();
            return data;
        }

    }
}
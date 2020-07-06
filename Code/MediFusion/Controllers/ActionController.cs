using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MediFusionPM.Models;
using MediFusionPM.Models.TodoApi.Models;
using MediFusionPM.ViewModels;
using static MediFusionPM.ViewModels.VMAction;
using Action = MediFusionPM.Models.Action;
using static MediFusionPM.ViewModels.VMCommon;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class ActionController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;
        public ActionController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;
        }

        [HttpGet]
        [Route("GetActions")]
        public async Task<ActionResult<IEnumerable<Action>>> GetActions()
        {
            
            try
            {

                return await _context.Action.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("GetProfiles")]
        [HttpGet()]
        public async Task<ActionResult<VMAction>> GetProfiles(long id)
        {

            ViewModels.VMAction obj = new ViewModels.VMAction();
            obj.GetProfiles(_context);
            return obj;
        }

        [Route("FindAction/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Action>> FindAction(long id)
        {
            var Action = await _context.Action.FindAsync(id);

            if (Action == null)
            {

                return NotFound();
            }
            return Action;
        }
        [HttpPost]
        [Route("FindActions")]
        public async Task<ActionResult<IEnumerable<GAction>>> FindActions(CAction CAction)
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            // if (UD == null || UD.Rights == null || UD.Rights.Action == false)
            //   return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindActions(CAction, PracticeId);
        }
        private List<GAction> FindActions(CAction CAction, long PracticeId)
        {
            List<GAction> data = (from a in _context.Action
                                  join u in _context.Users on a.UserID equals u.Id into Table1
                                  from t1 in Table1.DefaultIfEmpty()

                                  where
                                 (CAction.Name.IsNull() ? true : a.Name.Contains(CAction.Name)) &&
                                 (CAction.Description.IsNull() ? true : a.Description.Contains(CAction.Description)) &&
                                 (CAction.User.IsNull() ? true : t1.LastName.Contains(CAction.User))
                                  select new GAction()
                                  {
                                      ID = a.ID,
                                      Name = a.Name,
                                      Description = a.Description,
                                      User = t1.LastName + ", " + t1.FirstName,
                                      UserID = t1.Id,
                                  }).ToList();
            return data;

        }
        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CAction CAction)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GAction> data = FindActions(CAction, PracticeId);
            ExportController controller = new ExportController(_context);

            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD,CAction,"Action Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CAction CAction)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GAction> data = FindActions(CAction, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }
        [Route("SaveAction")]
        [HttpPost]
        public async Task<ActionResult<Action>> SaveAction(Action item)
        {
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
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
                item.AddedBy = Email;
                item.AddedDate = DateTime.Now;
                _context.Action.Add(item);
                await _context.SaveChangesAsync();
            }
            else
            {
                item.UpdatedBy = Email;
                item.UpdatedDate = DateTime.Now;
                _context.Action.Update(item);
                await _context.SaveChangesAsync();
            }
            return Ok(item);
        }

        [Route("DeleteAction/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteAction(long id)
        {
           
            var Action = await _context.Action.FindAsync(id);

            if (Action == null)
            {
                return NotFound();
            }

            _context.Action.Remove(Action);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Route("FindAudit/{ActionID}")]
        [HttpGet("{ActionID}")]
        public List<ActionAudit> FindAudit(long ActionID)
        {
            List<ActionAudit> data = (from pAudit in _context.ActionAudit
                                      where pAudit.ActionID == ActionID
                                      orderby pAudit.AddedDate descending
                                      select new ActionAudit()
                                      {
                                          ID = pAudit.ID,
                                          ActionID = pAudit.ActionID,
                                          TransactionID = pAudit.TransactionID,
                                          ColumnName = pAudit.ColumnName,
                                          CurrentValue = pAudit.CurrentValue,
                                          NewValue = pAudit.NewValue,
                                          CurrentValueID = pAudit.CurrentValueID,
                                          NewValueID = pAudit.NewValueID,
                                          HostName = pAudit.HostName,
                                          AddedBy = pAudit.AddedBy,
                                          AddedDate = pAudit.AddedDate,
                                      }).ToList<ActionAudit>();
            return data;
        }


    }
}
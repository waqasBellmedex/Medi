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
using static MediFusionPM.ViewModels.VMGroup;
using static MediFusionPM.ViewModels.VMCommon;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class GroupController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;


        public GroupController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;

        }


        [HttpGet]
        [Route("GetGroups")]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroups()
        {
            try
            {
                return await _context.Group.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetProfiles")]
        [HttpGet()]
        public async Task<ActionResult<VMGroup>> GetProfiles(long id)
        {
            ViewModels.VMGroup obj = new ViewModels.VMGroup();
            obj.GetProfiles(_context);
            return obj;
        }

        [Route("FindGroup/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> FindGroup(long id)
        {
            var Group = await _context.Group.FindAsync(id);

            if (Group == null)
            {
                return NotFound();
            }
            return Group;
        }


        [HttpPost]
        [Route("FindGroups")]
        public async Task<ActionResult<IEnumerable<GGroup>>> FindGroups(CGroup CGroup)
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.groupSearch == false)
            //return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindGroups(CGroup, PracticeId);
        }
        private List<GGroup> FindGroups(CGroup CGroup, long PracticeId)
        {
            List<GGroup> data = (from g in _context.Group
                          join u in _context.Users on g.UserID equals u.Id into Table1
                          from t1 in Table1.DefaultIfEmpty()
                          where
                         (CGroup.Name.IsNull() ? true : g.Name.Contains(CGroup.Name)) &&
                         (CGroup.Description.IsNull() ? true : g.Description.Contains(CGroup.Description))&&
                         (CGroup.User.IsNull() ? true : t1.LastName.Contains(CGroup.User)) 
                          select new GGroup()
                          {
                              ID = g.ID,
                              Name = g.Name,
                              Description = g.Description,
                              User = t1.LastName + ", " + t1.FirstName,
                              UserID = t1.Id,
                          }).ToList();
            return data;
        }

        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CGroup CGroup)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GGroup> data = FindGroups(CGroup, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CGroup, "Group Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CGroup CGroup)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GGroup> data = FindGroups(CGroup, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }


        [Route("SaveGroup")]
        [HttpPost]
        public async Task<ActionResult<Group>> SaveGroup(Group item)
        {
           UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
           if (UD == null || UD.Rights == null || UD.Rights.groupCreate == false)
           return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
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
                item.AddedBy = UD.Email;
                item.AddedDate = DateTime.Now;
                _context.Group.Add(item);
                await _context.SaveChangesAsync();
            }
            else if (UD.Rights.groupUpdate == true)
            {
                item.UpdatedBy = UD.Email;
                item.UpdatedDate = DateTime.Now;
                _context.Group.Update(item);
                await _context.SaveChangesAsync();
            }
            else return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);
            return Ok(item);
        }

        [Route("DeleteGroup/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteGroup(long id)
        {
            var Group = await _context.Group.FindAsync(id);

            if (Group == null)
            {
                return NotFound();
            }

            _context.Group.Remove(Group);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [Route("FindAudit/{GroupID}")]
        [HttpGet("{GroupID}")]
        public List<GroupAudit> FindAudit(long GroupID)
        {
            List<GroupAudit> data = (from pAudit in _context.GroupAudit
                                     where pAudit.GroupID == GroupID
                                     orderby pAudit.AddedDate descending
                                     select new GroupAudit()
                                     {
                                         ID = pAudit.ID,
                                         GroupID = pAudit.GroupID,
                                         TransactionID = pAudit.TransactionID,
                                         ColumnName = pAudit.ColumnName,
                                         CurrentValue = pAudit.CurrentValue,
                                         NewValue = pAudit.NewValue,
                                         CurrentValueID = pAudit.CurrentValueID,
                                         NewValueID = pAudit.NewValueID,
                                         HostName = pAudit.HostName,
                                         AddedBy = pAudit.AddedBy,
                                         AddedDate = pAudit.AddedDate,
                                     }).ToList<GroupAudit>();
            return data;
        }


    }
}
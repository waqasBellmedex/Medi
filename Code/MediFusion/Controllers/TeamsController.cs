using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediFusionPM.Models;
using static MediFusionPM.ViewModels.VMTeam;
using MediFusionPM.Models.Main;
using static MediFusionPM.ViewModels.VMCommon;
using MediFusionPM.ViewModels;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeamsController : ControllerBase
    {
     //   private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private readonly ClientDbContext _context;
        public TeamsController(ClientDbContext contextClient, MainContext contextMain)
        {
            _context = contextClient;
            _contextMain = contextMain;
        }

        // GET: api/Teams
        [HttpGet]
        [Route("GetTeams")]
        public IEnumerable<MainTeam> GetTeams()
        {
            return _contextMain.MainTeam;
        }

        // GET: api/Teams/5
        [HttpGet("{id}")]
        [Route("GetTeam/{id}")]
        public async Task<IActionResult> GetTeam(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var team = await _contextMain.MainTeam.FindAsync(id);

            if (team == null)
            {
                return NotFound();
            }

            return Ok(team);
        }

        [HttpPost]
        [Route("FindTeams")]
        public async Task<ActionResult<IEnumerable<GTeam>>> FindTeams(CTeam CTeam)
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.LocationSearch == false)
            //return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindTeams(CTeam, PracticeId);
        }
        private List<GTeam> FindTeams(CTeam CTeam, long PracticeId)
        {
            List<GTeam> data = (from t in _contextMain.MainTeam
                          where
                          (CTeam.Name.IsNull() ? true : t.Name.Contains(CTeam.Name))
                          select new GTeam()
                          {
                              ID = t.ID,
                              Name = t.Name,
                              Details = t.Details,
                          })
                .ToList();
            return data;
        }
        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CTeam CTeam)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GTeam> data = FindTeams(CTeam, PracticeId);
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CTeam, "Team Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CTeam CTeam)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GTeam> data = FindTeams(CTeam, PracticeId);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }
        // POST: api/Teams
        [HttpPost]
        [Route("SaveTeam")]
        public async Task<IActionResult> SaveTeam(MainTeam team)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (UD == null || UD.Rights == null || UD.Rights.teamCreate == false)
            return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (team.ID.IsNull())
            {
                team.AddedBy = UD.Email;
                team.AddedDate = DateTime.Now;
                _contextMain.MainTeam.Add(team);
                await _contextMain.SaveChangesAsync();

                Team Cliteam = new Team();
                Cliteam.ID = team.ID;
                Cliteam.AddedBy = UD.Email;
                Cliteam.Name = team.Name;
                Cliteam.Details = team.Details;
                Cliteam.AddedDate = DateTime.Now;
                _context.Team.Add(Cliteam);
                await _context.SaveChangesAsync();
                return Ok(team);
            }
            else if (UD.Rights.teamupdate == true)
            {
                team.UpdatedBy = UD.Email;
                team.UpdatedDate = DateTime.Now;
                _contextMain.MainTeam.Update(team);
                await _contextMain.SaveChangesAsync();

               // var t = (from u in _context.Team where u.ID == team.ID select u).FirstOrDefault();
             //   t.UpdatedBy = Email;
              //  t.UpdatedDate = DateTime.Now;
                //_context.Team.Update(t);
                //await _context.SaveChangesAsync();
                //return Ok(team);
            }
            else return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            return Ok(team);
        }

        // DELETE: api/Teams/5
        [HttpDelete("{id}")]
        [Route("DeleteTeam/{id}")]
        public async Task<IActionResult> DeleteTeam([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var team = await _contextMain.MainTeam.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            _contextMain.MainTeam.Remove(team);
            await _contextMain.SaveChangesAsync();

            return Ok(team);
        }

        private bool TeamExists(long id)
        {
            return _contextMain.MainTeam.Any(e => e.ID == id);
        }

        [HttpGet("{id}")]
        [Route("FindTeamUsers/{id}")]
        public async Task<ActionResult<IEnumerable<GTeam>>> FindTeamUsers(long id)
        {
            return await (from usr in _contextMain.Users
                          join t in _contextMain.MainTeam
                          on usr.TeamID equals t.ID
                          join uRol in _contextMain.UserRoles
                          on usr.Id equals uRol.UserId
                          join rol in _contextMain.Roles
                          on uRol.RoleId equals rol.Id
                          where usr.TeamID == id
                          select new GTeam()
                          {
                              Name = usr.LastName + ", " + usr.FirstName,
                              Email = usr.Email,
                              ReportTo = usr.ReportingTo,
                              Role = rol.Name
                          })
                .ToListAsync();
        }
        [Route("FindAudit/{TeamID}")]
        [HttpGet("{TeamID}")]
        public List<TeamAudit> FindAudit(long TeamID)
        {
            List<TeamAudit> data = (from pAudit in _context.TeamAudit
                                        where pAudit.TeamID == TeamID
                                        select new TeamAudit()
                                        {
                                            ID = pAudit.ID,
                                            TeamID = pAudit.TeamID,
                                            TransactionID = pAudit.TransactionID,
                                            ColumnName = pAudit.ColumnName,
                                            CurrentValue = pAudit.CurrentValue,
                                            NewValue = pAudit.NewValue,
                                            CurrentValueID = pAudit.CurrentValueID,
                                            NewValueID = pAudit.NewValueID,
                                            HostName = pAudit.HostName,
                                            AddedBy = pAudit.AddedBy,
                                            AddedDate = pAudit.AddedDate,
                                        }).ToList<TeamAudit>();
            return data;
        }

    }

}


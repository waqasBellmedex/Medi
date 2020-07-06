using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using static MediFusionPM.ViewModels.VMLocation;
using MediFusionPM.Models.Audit;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class LocationController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;


        public LocationController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;


            // Only For Testing
            //   if (_context.Location.Count() == 0)
            //  {
            // _context.Locations.Add(new Location { Name = "", OrganizationName = "" });
            // _context.SaveChanges();
            //  }
        }

        [Route("GetProfiles")]
        [HttpGet()]
        public async Task<ActionResult<VMLocation>> GetProfiles(long id)
        {
            ViewModels.VMLocation obj = new ViewModels.VMLocation();
            obj.GetProfiles(_context);
            return obj;
        }

        [HttpGet]
        [Route("GetLocations")]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
        {
            return await _context.Location.ToListAsync();
        }

        [Route("FindLocation/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> FindLocation(long id)
        {
            var Location = await _context.Location.FindAsync(id);

            if (Location == null)
            {
                return NotFound();
            }
            return Location;
        }


        [HttpPost]
        [Route("FindLocations")]
        public async Task<ActionResult<IEnumerable<GLocation>>> FindLocations(CLocation CLocation)
        {

            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.LocationSearch == false)
            //return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindLocations(CLocation, PracticeId);
        }

        private List<GLocation> FindLocations(CLocation CLocation, long PracticeId)
        {
            List<GLocation> data = (from loc in _context.Location
                                    join prac in _context.Practice on loc.PracticeID equals prac.ID
                                    join pos in _context.POS on loc.POSID equals pos.ID
                                    //join up in _context.UserPractices on prac.ID equals up.PracticeID
                                    //join u in _context.Users on up.UserID equals u.Id

                                    where prac.ID == PracticeId && //&& u.Id.ToString() == UD.UserID &&
                                    //u.IsUserBlock == false &&
                                    (CLocation.Name.IsNull() ? true : loc.Name.Contains(CLocation.Name)) &&
                                    (CLocation.OrganizationName.IsNull() ? true : loc.OrganizationName.Contains(CLocation.OrganizationName)) &&
                                    (CLocation.PosCode.IsNull() ? true : pos.PosCode.Contains(CLocation.PosCode) || pos.Name.Contains(CLocation.PosCode)) &&
                                    (CLocation.NPI.IsNull() ? true : loc.NPI.Equals(CLocation.NPI))
                                    select new GLocation()
                                    {
                                        ID = loc.ID,
                                        Name = loc.Name,
                                        OrganizationName = loc.OrganizationName,
                                        PracticeID = prac.ID,
                                        Practice = prac.Name,
                                        NPI = loc.NPI,
                                        PosCode = pos.PosCode + " - " + pos.Name,
                                        Address = loc.Address1 + ", " + loc.City + ", " + loc.State + ", " + loc.ZipCode
                                    }).ToList();
            return data;
        }

        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CLocation CLocation)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GLocation> data = FindLocations(CLocation, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CLocation, "Location Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CLocation CLocation)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GLocation> data = FindLocations(CLocation, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }

        [Route("SaveLocation")]
        [HttpPost]
        public async Task<ActionResult<Location>> SaveLocation(Location item)
        {

            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (UD == null || UD.Rights == null || UD.Rights.LocationCreate == false)
            return BadRequest("You Don't Have Rights. Please Contact BellMedex.");

            bool succ = TryValidateModel(item);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            bool LocationExists = _context.Location.Count(p => p.Name == item.Name && item.ID == 0) > 0;
            if (LocationExists)
            {
                return BadRequest("Location With Same Name Already Exists");
            }
            if (item.ID == 0)
            {
                item.AddedBy = UD.Email;
                item.AddedDate = DateTime.Now;
                _context.Location.Add(item);
                await _context.SaveChangesAsync();
            }
            else if (UD.Rights.LocationEdit == true)
            {
                bool LocationProviderExistsUpdate = _context.Location.Any(p => p.Name == item.Name && item.ID != p.ID);

                if (LocationProviderExistsUpdate == true)
                {
                    return BadRequest("Location With Same Name Already Exists");
                }
                item.UpdatedBy = UD.Email;
                item.UpdatedDate = DateTime.Now;
                _context.Location.Update(item);
                await _context.SaveChangesAsync();
            }
            else return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);
            return Ok(item);
        }

        [Route("DeleteLocation/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteLocation(long id)
        {
            var Location = await _context.Location.FindAsync(id);

            if (Location == null)
            {
                return NotFound();
            }

            _context.Location.Remove(Location);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [Route("FindAudit/{LocationID}")]
        [HttpGet("{LocationID}")]
        public List<LocationAudit> FindAudit(long LocationID)
        {
            List<LocationAudit> data = (from pAudit in _context.LocationAudit
                                         where pAudit.LocationID == LocationID
                                        orderby pAudit.AddedDate descending
                                        select new LocationAudit()
                                         {
                                             ID = pAudit.ID,
                                             LocationID = pAudit.LocationID,
                                             TransactionID = pAudit.TransactionID,
                                             ColumnName = pAudit.ColumnName,
                                             CurrentValue = pAudit.CurrentValue,
                                             NewValue = pAudit.NewValue,
                                             CurrentValueID = pAudit.CurrentValueID,
                                             NewValueID = pAudit.NewValueID,
                                             HostName = pAudit.HostName,
                                             AddedBy = pAudit.AddedBy,
                                             AddedDate = pAudit.AddedDate,
                                         }).ToList<LocationAudit>();

            return data;
        }

    }
}

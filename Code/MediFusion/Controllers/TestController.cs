using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediFusionPM.Models;
using MediFusionPM.ViewModels;
using static MediFusionPM.ViewModels.VMCommon;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {


        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;

        public TestController( ClientDbContext context, MainContext contextMain)
        {
           
            _context = context;
            _contextMain = contextMain;
        }

        [Route("SetSession")]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> SetSession()
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

            HttpContext.Session.SetString("AuthenticationToken", Convert.ToString(UD.UserID));
            //HttpContext.Session.SetInt32("UserID", Convert.ToInt32(090078601));
            //// Getting New Guid
            //string guid = Convert.ToString(Guid.NewGuid());
            ////Storing new Guid in Session
            //HttpContext.Session.SetString("AuthenticationToken", Convert.ToString(guid));

            ////Adding Cookie in Browser
            //CookieOptions option = new CookieOptions { Expires = DateTime.Now.AddHours(24), HttpOnly = true };
            //Response.Cookies.Append("AuthenticationToken", guid, option);
            var Sess = HttpContext.Session.GetString("AuthenticationToken");
            return Ok(Sess);
        }

      
            [Route("GetSession")]
            [HttpGet]
            [Authorize]
            public async Task<ActionResult> GetSession()
            {

            var Sess = HttpContext.Session.GetString("AuthenticationToken");

            return Ok(Sess);
            }
        }
}
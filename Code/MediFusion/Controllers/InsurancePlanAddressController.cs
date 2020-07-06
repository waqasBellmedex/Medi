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
using static MediFusionPM.ViewModels.VMInsurancePlanAddress;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class InsurancePlanAddressController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;

        public InsurancePlanAddressController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;

        }

        [HttpGet]
        [Route("GetInsurancePlanAddresses")]
        public async Task<ActionResult<IEnumerable<InsurancePlanAddress>>> GetInsurancePlanAddresses()
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            var InsurancePlanAddress =  await _context.InsurancePlanAddress.ToListAsync();
            
            InsurancePlanAddress.Insert(0, new InsurancePlanAddress() { ID = 0, Address1 = "Please Select" });
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, InsurancePlanAddress.Count);

            return InsurancePlanAddress;

        }


        //[Route("GetInsurancePlanAddresses/{InsurancePlanId}")]
        //[HttpGet("{id}")]
        //public async Task<ActionResult<IEnumerable<InsurancePlanAddress>>> GetInsurancePlanAddresses(long InsurancePlanId)
        //{
        //    List<InsurancePlanAddress> Addresses = (from ipa in _context.InsurancePlanAddress
        //                                          where ipa.InsurancePlanId == InsurancePlanId
        //                                          select ipa ).ToList();
        //    if (Addresses == null)
        //    {
        //        NotFound();
        //    }

        //    Addresses.Insert(0, new InsurancePlanAddress() { ID = 0, Address1 = "Please Select" });
        //    return Addresses;
        //}

        [Route("GetInsurancePlanAddressesByInsurancePlanID/{InsurancePlanId}")]
        [HttpGet("{id}")]
        public  List<InsurancePlanAddress> GetInsurancePlanAddressesByInsuranceID(long InsurancePlanId)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

            List<InsurancePlanAddress> Addresses = (from ipa in _context.InsurancePlanAddress
                                                    where ipa.InsurancePlanId == InsurancePlanId
                                                    select new InsurancePlanAddress
                                                    {
                                                    ID = ipa.ID,
                                                    InsurancePlanId = ipa.InsurancePlanId,
                                                   Address1 = ipa.Address1,
                                                   Address2 = ipa.Address2,
                                                   City = ipa.City,
                                                   State = ipa.City,
                                                   ZipCode = ipa.ZipCode,
                                                   PhoneNumber = ipa.PhoneNumber,
                                                   FaxNumber = ipa.FaxNumber,

                                                                     }).ToList();
            Addresses.Insert(0, new InsurancePlanAddress() { ID = 0, Address1 = "Please Select" });
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, Addresses.Count);
            return Addresses;
        }
        [Route("FindInsurancePlanAddress/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<InsurancePlanAddress>> FindInsurancePlanAddress(long id)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

            var insPlanAddress = await _context.InsurancePlanAddress.FindAsync(id);

            if (insPlanAddress == null)
            {
                return NotFound();
            }
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);
            return insPlanAddress;
        }

        [Route("GetProfiles")]
        [HttpGet()]
        public async Task<ActionResult<VMInsurancePlanAddress>> GetProfiles(long id)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

            ViewModels.VMInsurancePlanAddress obj = new ViewModels.VMInsurancePlanAddress();
            obj.GetProfiles(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);
            return obj;
        }


        [HttpPost]
        [Route("FindInsurancePlanAddress")]
        public async Task<ActionResult<IEnumerable<GInsurancePlanAddress>>> FindInsurancePlanAddress(CInsurancePlanAddress CInsurancePlanAddress)
        {
        UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
        User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
        User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
        if (UD == null || UD.Rights == null || UD.Rights.InsurancePlanAddressSearch == false)
        return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
        return FindInsurancePlanAddress(CInsurancePlanAddress, UD);
        }

        private List<GInsurancePlanAddress> FindInsurancePlanAddress(CInsurancePlanAddress CInsurancePlanAddress, UserInfoData UD)
        {
            List<GInsurancePlanAddress> data = (from ip in _context.InsurancePlan
                          join ipa in _context.InsurancePlanAddress
                          on ip.ID equals ipa.InsurancePlanId
                         
                          where
                          (CInsurancePlanAddress.InsurancePlan.IsNull() ? true : ip.PlanName .ToUpper().Contains(CInsurancePlanAddress.InsurancePlan)) &&
                          (CInsurancePlanAddress.Address.IsNull() ? true : ipa.Address1.ToUpper().Contains(CInsurancePlanAddress.Address)) &&
                          (CInsurancePlanAddress.PhoneNumber.IsNull() ? true : ipa.PhoneNumber.Equals(CInsurancePlanAddress.PhoneNumber)) 
                         
                          select new GInsurancePlanAddress()
                          {
                            Id = ipa.ID,
                            InsurancePlanID =ip.ID,
                            InsurancePlan = ip.PlanName,
                            Address = ipa.Address1 + ", " + ipa.City + ", " + ipa.State + ", " + ipa.ZipCode,
                            PhoneNumber = ipa.PhoneNumber,
                            FaxNumber = ipa.FaxNumber,
                              
                          }).ToList();
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return data;
        }

        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CInsurancePlanAddress CInsurancePlanAddress)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GInsurancePlanAddress> data = FindInsurancePlanAddress(CInsurancePlanAddress, UD);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CInsurancePlanAddress, "Insurance Plan Address Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CInsurancePlanAddress CInsurancePlanAddress)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GInsurancePlanAddress> data = FindInsurancePlanAddress(CInsurancePlanAddress, UD);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }


        [Route("SaveInsurancePlanAddress")]
        [HttpPost]
        public async Task<ActionResult<InsurancePlanAddress>> SaveInsurancePlanAddress(InsurancePlanAddress item)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (UD == null || UD.Rights == null || UD.Rights.InsurancePlanAddressCreate == false)
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
                _context.InsurancePlanAddress.Add(item);
                await _context.SaveChangesAsync();
            }
            else if (UD.Rights.InsurancePlanAddressEdit == true)
            {
                item.UpdatedBy = UD.Email;
                item.UpdatedDate = DateTime.Now;
                _context.InsurancePlanAddress.Update(item);
                await _context.SaveChangesAsync();
            }
            else return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);
            return Ok(item);
        }

        [Route("DeleteInsurancePlanAddress/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteInsurancePlanAddress(long id)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

            var InsurancePlanAddress = await _context.InsurancePlanAddress.FindAsync(id);

            if (InsurancePlanAddress == null)
            {
                return NotFound();
            }

            _context.InsurancePlanAddress.Remove(InsurancePlanAddress);
            await _context.SaveChangesAsync();
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);
            return Ok();
        }

        [Route("FindAudit/{InsurancePlanAddressID}")]
        [HttpGet("{InsurancePlanAddressID}")]
        public List<InsurancePlanAddressAudit> FindAudit(long InsurancePlanAddressID)
        {

            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

            List<InsurancePlanAddressAudit> data = (from pAudit in _context.InsurancePlanAddressAudit
                                                    where pAudit.InsurancePlanAddressID == InsurancePlanAddressID
                                                    orderby pAudit.AddedDate descending
                                                    select new InsurancePlanAddressAudit()
                                                    {
                                                        ID = pAudit.ID,
                                                        InsurancePlanAddressID = pAudit.InsurancePlanAddressID,
                                                        TransactionID = pAudit.TransactionID,
                                                        ColumnName = pAudit.ColumnName,
                                                        CurrentValue = pAudit.CurrentValue,
                                                        NewValue = pAudit.NewValue,
                                                        CurrentValueID = pAudit.CurrentValueID,
                                                        NewValueID = pAudit.NewValueID,
                                                        HostName = pAudit.HostName,
                                                        AddedBy = pAudit.AddedBy,
                                                        AddedDate = pAudit.AddedDate,
                                                    }).ToList<InsurancePlanAddressAudit>();
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return data;
        }







    }
}
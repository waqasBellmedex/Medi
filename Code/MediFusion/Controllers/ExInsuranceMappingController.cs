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
using static MediFusionPM.ViewModels.VMCommon;
using MediFusionPM.Models.Main;
using static MediFusionPM.ViewModels.Main.VMExInsuranceMapping;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class ExInsuranceMappingController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;

        public ExInsuranceMappingController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;
        }

        [HttpGet]
        [Route("GetExInsuranceMapping")]
        public async Task<ActionResult<IEnumerable<ExInsuranceMapping>>> GetExInsuranceMapping()
        {
            try
            {
                return await _contextMain.ExInsuranceMapping.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("FindExInsuranceMapping/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ExInsuranceMapping>> FindExInsuranceMapping(long id)
        {
            var Action = await _contextMain.ExInsuranceMapping.FindAsync(id);

            if (Action == null)
            {
                return NotFound();
            }
            return Action;
        }
        [HttpPost]
        [Route("FindExInsuranceMapping")]
        public List <GExInsuranceMapping> FindExInsuranceMapping(CExInsuranceMapping CExInsuranceMapping)
        {
            List<GExInsuranceMapping> data = (from ins in _contextMain.ExInsuranceMapping
                                              //join iPlan in _context.InsurancePlan on ins.InsurancePlanID equals iPlan.ID
                                             where
                                              (CExInsuranceMapping.ExternalInsuranceName.IsNull() ? true : ins.ExternalInsuranceName.Contains(CExInsuranceMapping.ExternalInsuranceName)) &&
                                              (CExInsuranceMapping.InsurancePlanID.IsNull() ? true : ins.InsurancePlanID.Equals(CExInsuranceMapping.InsurancePlanID)) &&
                                              (ExtensionMethods.IsBetweenDOS(CExInsuranceMapping.ToDate , CExInsuranceMapping.FromDate,ins.AddedDate,ins.AddedDate)) &&
                                               (CExInsuranceMapping.Status.IsNull() ?  true : CExInsuranceMapping.Status == "F" ? CExInsuranceMapping.Status.Equals(ins.Status) : CExInsuranceMapping.Status == "A" ? CExInsuranceMapping.Status.Equals(ins.Status) || ins.Status.IsNull() : false)

                              select new GExInsuranceMapping()
                              {
                                  ID = ins.ID,
                                  ExternalInsuranceName = ins.ExternalInsuranceName,
                                  InsurancePlanID = ins.InsurancePlanID.ToString(),
                                  PlanName = ins.PlanName,
                                  AddedBy = ins.AddedBy,
                                  AddedDate = ins.AddedDate.Format("MM/dd/yyyy"),
                                 // AddedDate = ins.AddedDate.Format("mm/dd/yyyy"),
                                  Status = ins.Status == "A" ? "Added"  : ins.Status == "F" ? "Failed" : ""

                              }).ToList();
            return data;

        }

        [Route("SaveExInsuranceMapping")]
        [HttpPost]
        public async Task<ActionResult<ExInsuranceMapping>> SaveExInsuranceMapping(ExInsuranceMapping item)
        {
            // string Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            bool succ = TryValidateModel(item);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            if (!item.ExternalInsuranceName.IsNull() && !item.PlanName.IsNull() && !item.InsurancePlanID.IsNull())
            {
                item.Status = "A";
            }

            if (item.ID == 0)
            {
                item.AddedBy = Email;
                item.AddedDate = DateTime.Now;
                item.PracticeID = PracticeId;
                _contextMain.ExInsuranceMapping.Add(item);
                await _contextMain.SaveChangesAsync();
            }
            else
            {
                item.UpdatedBy = Email;
                item.UpdatedDate = DateTime.Now;
                item.PracticeID = PracticeId;
                _contextMain.ExInsuranceMapping.Update(item);
                await _contextMain.SaveChangesAsync();
            }
            return Ok(item);
        }



        [Route("DeleteExInsuranceMapping/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteExInsuranceMapping(long id)
        {
            var exInsuranceMapping = await _contextMain.ExInsuranceMapping.FindAsync(id);
            if (exInsuranceMapping == null)
            {
                return BadRequest("Record Not Found.");
            }
            _contextMain.ExInsuranceMapping.Remove(exInsuranceMapping);
            await _contextMain.SaveChangesAsync();
            return Ok();
        }


        //[Route("DeleteExInsuranceMapping/{id}")]
        //[HttpDelete]
        //public async Task<ActionResult> DeleteExInsuranceMapping(long id)
        //{

        //    var exInsuranceMapping = await _contextMain.ExInsuranceMapping.FindAsync(id);

        //    if (exInsuranceMapping == null)
        //    {
        //        return NotFound();
        //    }


        //    // Code for to Delete Multiple ID's from ExInsuranceMapping
        //    List<ExInsuranceMapping> data = (from op in _contextMain.ExInsuranceMapping where op.ID == id select op).ToList();
        //    if (data.Count > 0)
        //    {
        //        foreach (ExInsuranceMapping creadentail in data)
        //        {
        //            _contextMain.ExInsuranceMapping.Remove(creadentail);
        //        }
        //    }
        //    _contextMain.ExInsuranceMapping.Remove(exInsuranceMapping);
        //    await _context.SaveChangesAsync();


        //    return Ok();
        //}
    }
}
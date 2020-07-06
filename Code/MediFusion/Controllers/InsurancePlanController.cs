using System;
using System.Collections.Generic;
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
using static MediFusionPM.ViewModels.VMInsurancePlan;
using static MediFusionPM.ViewModels.VMCommon;
using MediFusionPM.Models.Audit;
using System.Transactions;
using System.IO;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]

    public class InsurancePlanController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;

        public InsurancePlanController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;

        }

        [Route("GetProfiles")]
        [HttpGet()]
        public async Task<ActionResult<VMInsurancePlan>> GetProfiles(long id)
        {
            ViewModels.VMInsurancePlan obj = new ViewModels.VMInsurancePlan();
            obj.GetProfiles(_context);
            return obj;
        }


        [HttpGet]
        [Route("GetInsurancePlans")]
        public async Task<ActionResult<IEnumerable<InsurancePlan>>> GetInsurancePlans()
        {
            return await _context.InsurancePlan.ToListAsync();
        }

        [Route("FindInsurancePlan/{id}")]
        [HttpGet("{id}")]

        public async Task<ActionResult<InsurancePlan>> FindInsurancePlan(long id)
        {
            var InsurancePlan = await _context.InsurancePlan.FindAsync(id);
            if (InsurancePlan == null)
            {
                return NotFound();
            }
            InsurancePlan.InsuranceBillingoption = _context.InsuranceBillingoption.Where(m => m.InsurancePlanID == id).ToList<InsuranceBillingoption>();
            return InsurancePlan;
        }

        [Route("FindAudit/{InsurancePlanID}")]
        [HttpGet("{InsurancePlanID}")]
        public List<InsurancePlanAudit> FindAudit(long InsurancePlanID)
        {
            List<InsurancePlanAudit> data = (from iPlanA in _context.InsurancePlanAudit
                                            where iPlanA.InsurancePlanID == InsurancePlanID
                                             orderby iPlanA.AddedDate descending
                                             select new InsurancePlanAudit()
                                      {
                                          ID = iPlanA.ID,
                                          InsurancePlanID = iPlanA.InsurancePlanID,
                                          TransactionID = iPlanA.TransactionID,
                                          ColumnName = iPlanA.ColumnName,
                                          CurrentValue = iPlanA.CurrentValue,
                                          NewValue = iPlanA.NewValue,
                                          CurrentValueID = iPlanA.CurrentValueID,
                                          NewValueID = iPlanA.NewValueID,
                                          HostName = iPlanA.HostName,
                                          AddedBy = iPlanA.AddedBy,
                                          AddedDate = iPlanA.AddedDate,
                                      }).ToList<InsurancePlanAudit>();
           
            return data;
        }

        [Route("FindSelfPayPlan")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DropDown>>> FindSelfPayPlan()
        {

            List<DropDown> SelfPay = (from iPlan in _context.InsurancePlan
                                      where iPlan.PlanName == "SelfPay" && iPlan.Description == "SelfPay"
                                      select new DropDown()
                                     {
                                         ID = iPlan.ID,
                                         Description = iPlan.PlanName,
                                         Description2 = iPlan.Description
                                     }
                       ).ToList();

            SelfPay.Insert(0, new DropDown() { ID = null, Description = "Please Select" });
            return SelfPay;
        }

        [HttpPost]
        [Route("FindInsurancePlan")]
        public async Task<ActionResult<IEnumerable<GInsurancePlan>>> FindInsurancePlan(CInsurancePlan CInsurancePlan)
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.InsurancePlanSearch == false)
            //return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindInsurancePlan(CInsurancePlan, PracticeId);
            
        }
        private List<GInsurancePlan> FindInsurancePlan(CInsurancePlan CInsurancePlan, long PracticeId)
        {
            List < GInsurancePlan > data = (from iPlan in _context.InsurancePlan
                                           join ins in _context.Insurance on iPlan.InsuranceID equals ins.ID
                                           join pType in _context.PlanType on iPlan.PlanTypeID equals pType.ID
                                           join edi837 in _context.Edi837Payer on iPlan.Edi837PayerID equals edi837.ID into Table1 from t1 in Table1.DefaultIfEmpty()
                                            where
                            (CInsurancePlan.PlanName.IsNull() ? true : iPlan.PlanName.Contains(CInsurancePlan.PlanName)) &&
                            (CInsurancePlan.Description.IsNull() ? true : iPlan.Description.Contains(CInsurancePlan.Description)) &&
                            (CInsurancePlan.PayerID.IsNull() ? true : iPlan.Edi837PayerID.Equals(CInsurancePlan.PayerID))&&
                            (CInsurancePlan.Insurance.IsNull() ? true : ins.Name.Contains(CInsurancePlan.Insurance)) &&
                            (CInsurancePlan.PlanType.IsNull() ? true : pType.Description.Contains(CInsurancePlan.PlanType)) &&
                            (CInsurancePlan.PayerName.IsNull() ? true : t1.PayerName.Contains(CInsurancePlan.PayerName))

                                           select new GInsurancePlan()
                                           {
                                               ID = iPlan.ID,
                                               PlanName = iPlan.PlanName,
                                               Description = iPlan.Description,
                                               PayerID = t1.PayerID,
                                               InsuranceID = iPlan.InsuranceID,
                                               Insurance = ins.Name,
                                               PlanType = pType.Description,
                                               PayerName = t1.PayerName
                                           }).ToList();
            return data;
        }
        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CInsurancePlan CInsurancePlan)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GInsurancePlan> data = FindInsurancePlan(CInsurancePlan, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CInsurancePlan, "Insurance Plan Report");
        }


        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CInsurancePlan CInsurancePlan)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GInsurancePlan> data = FindInsurancePlan(CInsurancePlan, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }
        [Route("SaveInsurancePlan")]
        [HttpPost]
        public async Task<ActionResult<InsurancePlan>> SaveInsurancePlan(InsurancePlan item)
        {
           UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
           if (UD == null || UD.Rights == null || UD.Rights.InsurancePlanCreate == false)
           return BadRequest("You Don't Have Rights. Please Contact BellMedex.");

            bool succ = TryValidateModel(item);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            var transactionOption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (var objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                { 
                if (item.ID == 0)
                {
                    item.AddedBy = UD.Email;
                    item.AddedDate = DateTime.Now;
                    _context.InsurancePlan.Add(item);
                    //  await _context.SaveChangesAsync();
                    if (item.InsuranceBillingoption != null)
                    {
                        foreach (InsuranceBillingoption insBilling in item.InsuranceBillingoption)
                        {
                            insBilling.AddedBy = UD.Email;
                            insBilling.AddedDate = DateTime.Now;
                            insBilling.InsurancePlanID = item.ID;
                            _context.Add(insBilling);
                        }
                    }
                }
                else if (UD.Rights.InsurancePlanEdit == true)
                {
                    item.UpdatedBy = UD.Email;
                    item.UpdatedDate = DateTime.Now;
                    _context.InsurancePlan.Update(item);
                    // await _context.SaveChangesAsync();
                    if (item.InsuranceBillingoption != null)
                    {
                        foreach (InsuranceBillingoption insBilling in item.InsuranceBillingoption)
                        {
                            if (insBilling.ID <= 0)
                            {
                                insBilling.AddedBy = UD.Email;
                                insBilling.AddedDate = DateTime.Now;
                                insBilling.InsurancePlanID = item.ID;
                                _context.Add(insBilling);

                            }
                            else
                            {
                                insBilling.InsurancePlanID = item.ID;
                                insBilling.UpdatedBy = UD.Email;
                                insBilling.UpdatedDate = DateTime.Now;
                                _context.Update(insBilling);

                            }
                        }
                    }
                }
                  else return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
                   await _context.SaveChangesAsync();
                    objTrnScope.Complete();
                    objTrnScope.Dispose();
                }
                catch (Exception ex)
                {
                    System.IO.File.WriteAllText(Path.Combine(_context.env.ContentRootPath, "Logs", "InsurancePlan.txt"), ex.ToString());
                    throw ex;

                }
                finally
                {

                }
                return Ok(item);
            }
            }
        
        [Route("DeleteInsurancePlan/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteInsurance(long id)
        {
            var InsurancePlan = await _context.InsurancePlan.FindAsync(id);

            if (InsurancePlan == null)
            {
                return NotFound();
            }

            _context.InsurancePlan.Remove(InsurancePlan);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
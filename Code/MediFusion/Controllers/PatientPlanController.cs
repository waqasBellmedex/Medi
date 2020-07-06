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
using static MediFusionPM.ViewModels.VMPatientPlan;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class PatientPlanController : ControllerBase
    {
        private readonly ClientDbContext _context;


       public PatientPlanController(ClientDbContext context)
       {
            _context = context;

             //Only For Testing
            // if (_context.PatientPlan.Count() == 0)
            //  {
             //  _context.Patient.Add(new Patient { AccountNum = "123456" });
           //  _context.SaveChanges();
            // }
        }


        [HttpGet]
        [Route("GetPatientPlans")]
        public async Task<ActionResult<IEnumerable<PatientPlan>>> GetPatientPlans()
        {
            return await _context.PatientPlan.ToListAsync();
        }

        [Route("FindPatientPlan/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientPlan>> FindPatientPlan(long id)
        {
            var PatientPlan = await _context.PatientPlan.FindAsync(id);

            if (PatientPlan == null)
            {
                return NotFound();
            }

            return PatientPlan;
        }

        [Route("FindInsurancePlanAddress/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<InsurancePlanAddress>> FindInsurancePlanAddress(long id)
        {
          var planAddress = await _context.InsurancePlanAddress.FindAsync(id);

           if (planAddress == null)
            {
                return NotFound();
            }

            return planAddress;


      
        }

        [Route("GetpatientPlansByPatientID/{PatientId}")]
        [HttpGet("{PatientId}")]
        public List<DropDown> GetpatientPlansByPatientID(long PatientId)
        {
            List<DropDown> data = (from pp in _context.PatientPlan
                                   join ip in _context.InsurancePlan
                                    on pp.InsurancePlanID equals ip.ID
                                   where pp.PatientID == PatientId orderby pp.Coverage ascending
                                   
                                   select new DropDown()
                                   {
                                       ID = pp.ID,
                                       Description = pp.Coverage,
                                       Description2 = ip.PlanName,
                                       Description3 = pp.Copay.ToString()
                                       
                                   }).ToList();
           // data.Insert(0, new DropDown() { ID = null, Description = "Please Select" });
            return data;
           
        }

        [Route("GetActivePatientPlansByPatientID/{PatientId}")]
        [HttpGet("{PatientId}")]
        public List<DropDown> GetActivePatientPlansByPatientID(long PatientId)
        {
            List<DropDown> data = (from pp in _context.PatientPlan
                                   join ip in _context.InsurancePlan
                                    on pp.InsurancePlanID equals ip.ID
                                   where pp.IsActive == true
                                   where pp.PatientID == PatientId
                                   orderby pp.Coverage ascending

                                   select new DropDown()
                                   {
                                       ID = pp.ID,
                                       Description = pp.Coverage,
                                       Description2 = ip.PlanName,
                                       Description3 = pp.Copay.ToString(),
                                       SubscriberID = pp.SubscriberId

                                   }).ToList();
            // data.Insert(0, new DropDown() { ID = null, Description = "Please Select" });
            return data;
        }


        //Get Patient Active Plans
        [Route("GetpatientActivePlansByPatientID/{PatientId}")]
        [HttpGet("{PatientId}")]
        public List<DropDown> GetpatientActivePlansByPatientID(long PatientId)
        {
            List<DropDown> data = (from pp in _context.PatientPlan
                                   join ip in _context.InsurancePlan
                                    on pp.InsurancePlanID equals ip.ID
                                   where pp.PatientID == PatientId && pp.IsActive == true
                                   orderby pp.Coverage ascending

                                   select new DropDown()
                                   {
                                       ID = pp.ID,
                                       Description = pp.Coverage,
                                       Description2 = ip.PlanName,
                                   }).ToList();
            // data.Insert(0, new DropDown() { ID = null, Description = "Please Select" });
            return data;

        }


        [Route("GetProfiles")]
        [HttpGet()]
        public async Task<ActionResult<VMPatientPlan>> GetProfiles(long id)
        {
            ViewModels.VMPatientPlan obj = new ViewModels.VMPatientPlan();
            obj.GetProfiles(_context);

            return obj;
        }

        //[Route("SavePatientPlan")]
        //[HttpPost]
        //public async Task<ActionResult<PatientPlan>> SavePatientPlan(PatientPlan item)
        //{
        //    string Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
        //    bool succ = TryValidateModel(item);
        //    if (!ModelState.IsValid)
        //    {
        //        string messages = string.Join("; ", ModelState.Values
        //                                .SelectMany(x => x.Errors)
        //                                .Select(x => x.ErrorMessage));
        //        return BadRequest(messages);
        //    }

        //    if (item.ID == 0)
        //    {

        //        item.AddedBy = Email;
        //        item.AddedDate = DateTime.Now;
        //        _context.PatientPlan.Add(item);
        //        await _context.SaveChangesAsync();
        //    }
        //    else
        //    {
        //        item.UpdatedBy = Email;
        //        item.UpdatedDate = DateTime.Now;
        //        _context.PatientPlan.Update(item);
        //        await _context.SaveChangesAsync();
        //    }
        //    return Ok(item);
        //}
        [Route("SavePatientPlan")]
        [HttpPost]
        public async Task<ActionResult<PatientPlan>> SavePatientPlan(PatientPlan item)
            {
            string Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            bool succ = TryValidateModel(item);
            bool PlanExists = _context.PatientPlan.Count(
            p => p.Coverage == item.Coverage && p.PatientID == item.PatientID && p.IsActive == true && item.ID == 0) > 0;
              
            if (PlanExists)
            {
                return BadRequest("Patient With  Covrage  -" + item.Coverage + " Already Exists In PatientPlan ");
            }

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
                _context.PatientPlan.Add(item);
                await _context.SaveChangesAsync();
            }
            else
            {
                bool PlanExistsUpdate = _context.PatientPlan.Any(
                p => p.Coverage == item.Coverage && p.PatientID == item.PatientID && p.IsActive == true && item.ID != p.ID);

                 if (PlanExistsUpdate == true)
                {
                    return BadRequest("Patient With  Covrage - " + item.Coverage + " Already Exists In PatientPlan ");
                }
                item.UpdatedBy = Email;
                item.UpdatedDate = DateTime.Now;
                _context.PatientPlan.Update(item);
                await _context.SaveChangesAsync();
            }
            return Ok(item);
        }
        [Route("DeletePatientPlan/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeletePatientPlan(long id)
        {
            var PatientPlan = await _context.PatientPlan.FindAsync(id);

            if (PatientPlan == null)
            {
                return NotFound();
            }

            _context.PatientPlan.Remove(PatientPlan);
            await _context.SaveChangesAsync();

            return Ok();
        }
        [Route("FindAudit/{PatientPlanID}")]
        [HttpGet("{PatientPlanID}")]
        public List<PatientPlanAudit> FindAudit(long PatientPlanID)
        {
            List<PatientPlanAudit> data = (from pAudit in _context.PatientPlanAudit
                                           where pAudit.PatientPlanID == PatientPlanID
                                           orderby pAudit.AddedDate descending
                                           select new PatientPlanAudit()
                                           {
                                               ID = pAudit.ID,
                                               PatientPlanID = pAudit.PatientPlanID,
                                               TransactionID = pAudit.TransactionID,
                                               ColumnName = pAudit.ColumnName,
                                               CurrentValue = pAudit.CurrentValue,
                                               NewValue = pAudit.NewValue,
                                               CurrentValueID = pAudit.CurrentValueID,
                                               NewValueID = pAudit.NewValueID,
                                               HostName = pAudit.HostName,
                                               AddedBy = pAudit.AddedBy,
                                               AddedDate = pAudit.AddedDate,
                                           }).ToList<PatientPlanAudit>();
            return data;
        }



    }
}
using System;
using System.Collections.Generic;
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
using static MediFusionPM.ViewModels.VMChargeSubmissionHistory;
using MediFusionPM.Models.Audit;
using System.IdentityModel.Tokens.Jwt;
using static MediFusionPM.ViewModels.VMCommon;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class ChargeSubmissionHistoryController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;

        public ChargeSubmissionHistoryController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;

        }

        [Route("FindChargeSubmissionHistory/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ChargeSubmissionHistory>> FindChargeSubmissionHistory(long id)
        {
            var ChargeSubmissionHistory = await _context.ChargeSubmissionHistory.FindAsync(id);

            if (ChargeSubmissionHistory == null)
            {
                return NotFound();
            }
           
            return ChargeSubmissionHistory;
        }

        [Route("ChargeSubmissionHistory/{ID}")]
        [HttpGet("{ID}")]
        public List<FindChargeSubmissionHistory> ChargeSubmissionHistory(long ID)
        {
            
            List<FindChargeSubmissionHistory> data = (from csh in _context.ChargeSubmissionHistory
                                                  join pPlan in _context.PatientPlan on csh.PatientPlanID equals pPlan.ID
                                                  join iPlan in _context.InsurancePlan on pPlan.InsurancePlanID equals iPlan.ID
                                                 // join pType in _context.PlanType on iPlan.PlanTypeID equals pType.ID
                                                  join r in _context.Receiver on csh.ReceiverID equals r.ID into Table1 from t1 in Table1.DefaultIfEmpty()

                                                  where csh.ID == ID
                                                  select new FindChargeSubmissionHistory()
                                                  {
                                                      ID = csh.ID,
                                                      ChargeID = csh.ChargeID,
                                                      Receiver = t1.Name,
                                                      SubmitType = (csh.SubmitType == "P" ? "Paper" : (csh.SubmitType == "E" ? "EDI" : "")),
                                                      FormType = csh.FormType,
                                                      PatientPlanID = csh.PatientPlanID,
                                                      Plan = iPlan.PlanName,
                                                      Amount = csh.Amount,
                                                      AddedBy = csh.AddedBy,
                                                      AddedDate = csh.AddedDate.Format("MM/dd/yyyy"),
                                                      UpdatedBy = csh.UpdatedBy,
                                                      UpdatedDate = csh.UpdatedDate.Format("MM/dd/yyyy"),
                                                  }).ToList();
         
            return data;

        }


        [Route("FindChargeSubmission/{ChargeID}")]
        [HttpGet("{ChargeID}")]
        public List<GChargeSubmissionHistory> GetpatientPlansByPatientID(long ChargeID)
        {
           
            List<GChargeSubmissionHistory> data = (from csh in _context.ChargeSubmissionHistory
                                                   join r in _context.Receiver on csh.ReceiverID equals r.ID into r1
                                                   from rec in r1.DefaultIfEmpty()
                                                   join pPlan in _context.PatientPlan on csh.PatientPlanID equals pPlan.ID
                                                   join iPlan in _context.InsurancePlan on pPlan.InsurancePlanID equals iPlan.ID
                                                   where csh.ChargeID == ChargeID orderby csh.AddedDate descending
                                                   select new GChargeSubmissionHistory()
                                                   {
                                                       ID = csh.ID,
                                                       ChargeID = csh.ChargeID,
                                                       SubmitType = (csh.SubmitType == "P" ? "Paper" : 
                                                       (csh.SubmitType == "E" ? "EDI"  : "")) , 
                                                       Receiver = rec !=null ? rec.Name : csh.FormType ,
                                                       AddedDate = csh.AddedDate.ToString("MM/dd/yyyy"),
                                                       User = csh.AddedBy,
                                                       FormType = csh.FormType,
                                                       Coverage = (pPlan.Coverage == "P" ? "PRIMARY" : (pPlan.Coverage == "S" ? "SECONDARY" : (pPlan.Coverage == "T" ? "TERITARY" : ""))),
                                                       Plan = iPlan.PlanName
                                                   }) .ToList();

            return data;

        }

        [Route("FindAudit/{ChargeSubmissionHistoryID}")]
        [HttpGet("{ChargeSubmissionHistoryID}")]
        public List<ChargeSubmissionHistoryAudit> FindAudit(long ChargeSubmissionHistoryID)
        {
           
            List<ChargeSubmissionHistoryAudit> data = (from pAudit in _context.ChargeSubmissionHistoryAudit
                                                       where pAudit.ChargeSubmissionHistoryID == ChargeSubmissionHistoryID orderby pAudit.AddedDate descending
                                                       select new ChargeSubmissionHistoryAudit()
                                                       {
                                                           ID = pAudit.ID,
                                                           ChargeSubmissionHistoryID = pAudit.ChargeSubmissionHistoryID,
                                                           TransactionID = pAudit.TransactionID,
                                                           ColumnName = pAudit.ColumnName,
                                                           CurrentValue = pAudit.CurrentValue,
                                                           NewValue = pAudit.NewValue,
                                                           CurrentValueID = pAudit.CurrentValueID,
                                                           NewValueID = pAudit.NewValueID,
                                                           HostName = pAudit.HostName,
                                                           AddedBy = pAudit.AddedBy,
                                                           AddedDate = pAudit.AddedDate,
                                                       }).ToList<ChargeSubmissionHistoryAudit>();
            return data;
        }



    }
}
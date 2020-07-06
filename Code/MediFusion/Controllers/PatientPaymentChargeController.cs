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
using MediFusionPM.Models.TodoApi;
using MediFusionPM.Models.TodoApi.Models;
using MediFusionPM.ViewModels;
using static MediFusionPM.ViewModels.VMPatientPaymentCharge;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class PatientPaymentChargeController : Controller
    {
        private readonly ClientDbContext _context;
        public PatientPaymentChargeController(ClientDbContext context)
        {
            _context = context;
        }

        [Route("FindPatientPaymentCharge/{ID)}")]
        [HttpGet("{ID}")]
        public List<GPatientPaymentCharge> FindPatientPaymentCharge(long ID)
        {
            List<GPatientPaymentCharge> data = (from pc in _context.PatientPaymentCharge
                                                join pp in _context.PatientPayment on pc.PatientPaymentID equals pp.ID
                                                join v in _context.Visit on pc.VisitID equals v.ID
                                                
                                                join c in _context.Charge on v.ID equals c.VisitID
                                                join cpt in _context.Cpt on c.CPTID equals cpt.ID
                                                join pPlan in _context.PatientPlan on v.PrimaryPatientPlanID equals pPlan.ID
                                                join iPlan in _context.InsurancePlan on pPlan.InsurancePlanID equals iPlan.ID

                                                where pc.PatientPaymentID == ID && c.PrimaryPatientBal > 0
                                                select new GPatientPaymentCharge()
                                             {
                                                 VisitID = v.ID,
                                                 ChargeID = c.ID,
                                                 DOS = c.DateOfServiceFrom.Format("MM/dd/yyyy"),
                                                 SubmitDate = c.SubmittetdDate.Format("MM/dd/yyyy"),
                                                 Plan = iPlan.PlanName,
                                                 CPT = cpt.CPTCode,
                                                 BilledAmount = c.PrimaryBilledAmount,
                                                 WriteOff = c.PrimaryWriteOff,
                                                 AllowedAmount = c.PrimaryAllowed,
                                                 PaidAmount = c.PrimaryPaid,
                                                 Copay = c.Copay,
                                                 CoInsurance = c.Coinsurance,
                                                 Deductible = c.Deductible,
                                                 PatientBalance = c.PrimaryPatientBal,
                                                 AllocationAmount = null,
                                             }).ToList <GPatientPaymentCharge>();
            return data;

        }

        [Route("FindAudit/{PatientPaymentChargeID}")]
        [HttpGet("{PatientPaymentChargeID}")]
        public List<PatientPaymentChargeAudit> FindAudit(long PatientPaymentChargeID)
        {
            List<PatientPaymentChargeAudit> data = (from pAudit in _context.PatientPaymentChargeAudit
                                                    where pAudit.PatientPaymentChargeID == PatientPaymentChargeID
                                                    orderby pAudit.AddedDate descending
                                                    select new PatientPaymentChargeAudit()
                                               {
                                                   ID = pAudit.ID,
                                                   PatientPaymentChargeID = pAudit.PatientPaymentChargeID,
                                                   TransactionID = pAudit.TransactionID,
                                                   ColumnName = pAudit.ColumnName,
                                                   CurrentValue = pAudit.CurrentValue,
                                                   NewValue = pAudit.NewValue,
                                                   CurrentValueID = pAudit.CurrentValueID,
                                                   NewValueID = pAudit.NewValueID,
                                                   HostName = pAudit.HostName,
                                                   AddedBy = pAudit.AddedBy,
                                                   AddedDate = pAudit.AddedDate,
                                               }).ToList<PatientPaymentChargeAudit>();
            return data;
        }
       
    }
}
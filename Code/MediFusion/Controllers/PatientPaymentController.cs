using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MediFusionPM.Models;
using MediFusionPM.Models.TodoApi;
using MediFusionPM.Models.TodoApi.Models;
using MediFusionPM.ViewModels;
using static MediFusionPM.ViewModels.VMPatientPayment;
using static MediFusionPM.ViewModels.VMCommon;

namespace MediFusionPM.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class PatientPaymentController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;

        public PatientPaymentController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;
        }


        [HttpGet]
        [Route("GetPatientPayments")]
        public async Task<ActionResult<IEnumerable<PatientPayment>>> GetPatientPayments()
        {
            return await _context.PatientPayment.ToListAsync();
        }


        [Route("FindPatientPayment/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientPayment>> FindPatientPayment(long id)
        {
            var PatientPayment = await _context.PatientPayment.FindAsync(id);

            if (PatientPayment == null)
            {
                return NotFound();
            }
            return PatientPayment;
        }

        [Route("GetAdvancePayment/{PatientID}")]
        [HttpGet("{PatientID}")]
        public  List <PatientPayment> GetAdvancePayment(long PatientID)
        {
            var PatientPayment = (from pPayment in _context.PatientPayment
                                  where pPayment.PatientID == PatientID && pPayment.Type == "ADVANCE PAYMENT"
                                             select new PatientPayment
                                             {
                                                 ID = pPayment.ID,
                                                 PatientID = pPayment.PatientID,
                                                 PaymentMethod = pPayment.PaymentMethod,
                                                 PaymentDate = pPayment.PaymentDate,
                                                 PaymentAmount = pPayment.PaymentAmount.Val(),
                                                 CheckNumber = pPayment.CheckNumber,
                                                 Description = pPayment.Description,
                                                 Status = pPayment.Status,
                                                 AllocatedAmount = pPayment.AllocatedAmount.Val(),
                                                 RemainingAmount = pPayment.RemainingAmount.Val(),
                                                 VisitID = pPayment.VisitID,
                                                 Type = pPayment.Type,
                                                 AddedBy  = pPayment.AddedBy,
                                                 AddedDate = pPayment.AddedDate,
                                             }).ToList();

            if (PatientPayment == null)
            {
                NotFound();
            }
       

            return PatientPayment;

        }

        [Route("FindPatientPaymentCharges/{PatientID}")]
        [HttpGet("{PatientID}")]
        public List<PatientPaymentGrid> FindPatientPaymentCharges(long PatientID)
        {
            List<PatientPaymentGrid> data = (from v in _context.Visit
                                             join c in _context.Charge on v.ID equals c.VisitID
                                             join cpt in _context.Cpt on c.CPTID equals cpt.ID
                                             join pPlan in _context.PatientPlan on v.PrimaryPatientPlanID equals pPlan.ID
                                             join iPlan in _context.InsurancePlan on pPlan.InsurancePlanID equals iPlan.ID
                                             where v.PatientID == PatientID && c.PrimaryPatientBal > 0
                                             select new PatientPaymentGrid()
                                             {
                                                 ID = 0,
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
                                                 AllocatedAmount = null,
                                                 Status = null
                                             }).ToList<PatientPaymentGrid>();
            return data;

            }

        [Route("FindPatientChargesByPaymentID/{PaymentID}")]
        [HttpGet("{PaymentID}")]
        public List<PatientPaymentGrid> FindPatientChargesByPaymentID(long PaymentID)
        {
            List<PatientPaymentGrid> data = (from v in _context.Visit
                                             join c in _context.Charge on v.ID equals c.VisitID
                                             join cpt in _context.Cpt on c.CPTID equals cpt.ID
                                             join pPlan in _context.PatientPlan on v.PrimaryPatientPlanID equals pPlan.ID
                                             join iPlan in _context.InsurancePlan on pPlan.InsurancePlanID equals iPlan.ID
                                             join ppc in _context.PatientPaymentCharge on c.ID equals ppc.ChargeID
                                             join pp in _context.PatientPayment on ppc.PatientPaymentID equals pp.ID
                                             where pp.ID == PaymentID
                                             select new PatientPaymentGrid()
                                             {
                                                 ID = ppc.ID,
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
                                                 AllocatedAmount = ppc.AllocatedAmount,
                                                 Status = ppc.Status,
                                             }).ToList<PatientPaymentGrid>();
            return data;
        }


        [HttpPost]
        [Route("FindPatientPayment")]
        public async Task<ActionResult<IEnumerable<GPatientPayment>>> FindPatientPayment(CPatientPayment CPatientPayment)
        {
            return await (from pPayment in _context.PatientPayment
                          where
                         (CPatientPayment.PaymentDate == null ? true : object.Equals(CPatientPayment.PaymentDate, pPayment.PaymentDate)) &&
                         (CPatientPayment.CheckNumber.IsNull() ? true : pPayment.CheckNumber.Equals(CPatientPayment.CheckNumber)) &&
                         (CPatientPayment.Status.IsNull() ? true : pPayment.Status.Equals(CPatientPayment.Status)) &&
                          pPayment.PatientID == CPatientPayment.PatientID
                          select new GPatientPayment()
                          {
                              ID = pPayment.ID,
                              paymentDate = pPayment.PaymentDate.Format("MM/dd/yyyy"),
                              PaymentMethod = pPayment.PaymentMethod,
                              PaymentAmount = pPayment.PaymentAmount,
                              AllocatedAmount =  pPayment.AllocatedAmount,
                              RemainingAmount = pPayment.RemainingAmount,
                              Status = pPayment.Status,
                          }).ToListAsync();
        }

        [Route("SavePatientPayment")]
        [HttpPost]
        public async Task<ActionResult<PatientPayment>> SavePatientPayment(PatientPayment item)
        {
            string Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            bool succ = TryValidateModel(item);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }

            if (item.PatientID.IsNull())
            {
                return BadRequest("Patient Parameter should not be null");
            }

            List<PatientPaymentCharge> patPayCharges = item.PatientPaymentCharges.ToList();

            var transactionOption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (var objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption, TransactionScopeAsyncFlowOption.Enabled))
            {
                if (item.ID == 0)
                {
                    item.AddedBy = Email;
                    item.AddedDate = DateTime.Now;

                    _context.PatientPayment.Add(item);

                    foreach (PatientPaymentCharge pc in patPayCharges)
                    {
                        if (!pc.VisitID.IsNull() && !pc.ChargeID.IsNull() && pc.AllocatedAmount > 0)
                        {
                            pc.PatientPaymentID = item.ID;
                            pc.AddedBy = Email;
                            pc.AddedDate = DateTime.Now;

                            if (pc.Status != "C")
                            {
                                Charge charge = _context.Charge.Find(pc.ChargeID);
                                if (charge.Copay.Val() + charge.Deductible.Val() + charge.Coinsurance.Val() == pc.AllocatedAmount.Val())
                                {
                                    pc.Status = "C";    //Closed
                                    _context.PaymentLedger.Add(AddLedger(Email, charge, pc.ID, pc.AllocatedAmount));

                                    charge.PrimaryPatientBal = charge.PrimaryPatientBal.Val() - pc.AllocatedAmount;
                                    charge.PatientPaid = charge.PatientPaid.Val() + pc.AllocatedAmount.Val();
                                    charge.PrimaryStatus = "PT_P";

                                    _context.Charge.Update(charge);
                                }

                                Visit visit = _context.Visit.Find(charge.VisitID);
                                visit.PatientPaid = _context.Charge.Where(c=> c.VisitID == visit.ID).Sum(c => c.PatientPaid.IsNull() ? 0 : c.PatientPaid.Val());
                                _context.Visit.Update(visit);
                            }
                            _context.PatientPaymentCharge.Add(pc);
                        }
                    }
                }
                else
                {
                    item.UpdatedBy = Email;
                    item.UpdatedDate = DateTime.Now;
                    _context.PatientPayment.Update(item);
                    //  
                    foreach (PatientPaymentCharge pc in patPayCharges)
                    {
                        if (!pc.VisitID.IsNull() && !pc.ChargeID.IsNull() && pc.AllocatedAmount > 0)
                        {
                            // Patient Ledger
                            if (pc.Status != "C")
                            {
                                Charge charge = _context.Charge.Find(pc.ChargeID);
                                if (charge.Copay.Val() + charge.Deductible.Val() + charge.Coinsurance.Val() == pc.AllocatedAmount.Val())
                                {
                                    pc.Status = "C";    //Closed
                                    _context.PaymentLedger.Add(AddLedger(Email, charge, pc.ID, pc.AllocatedAmount));

                                    charge.PrimaryPatientBal = charge.PrimaryPatientBal.Val() - pc.AllocatedAmount;
                                    charge.PrimaryStatus = "PT_P";
                                    charge.PatientPaid = charge.PatientPaid.Val() + pc.AllocatedAmount.Val();
                                }

                                Visit visit = _context.Visit.Find(charge.VisitID);
                                visit.PatientPaid = _context.Charge.Where(c => c.VisitID == visit.ID).Sum(c => c.PatientPaid.IsNull() ? 0 : c.PatientPaid.Val());
                                _context.Visit.Update(visit);
                            }

                            if (pc.ID <= 0)
                            {
                                pc.PatientPaymentID = item.ID;
                                pc.AddedBy = Email;
                                pc.AddedDate = DateTime.Now;
                                _context.PatientPaymentCharge.Add(pc);
                            }
                            else
                            {
                                pc.AddedBy = Email;
                                pc.AddedDate = DateTime.Now;
                                _context.PatientPaymentCharge.Update(pc);
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
                objTrnScope.Complete();
                objTrnScope.Dispose();
            }

            return Ok(item);
        }

        private PaymentLedger AddLedger(string Email, Charge Charge, long PatientPaymentChargeID, decimal? AllocatedAmount)
        {
            PaymentLedger ledger = new PaymentLedger()
            {
                AddedBy = Email,
                AddedDate = DateTime.Now,
                AdjustmentCodeID = null,
                ChargeID = Charge.ID,
                LedgerBy = "PATIENT",
                LedgerDate = DateTime.Now,
                LedgerType = "PATIENT PAID",
                LedgerDescription = "",
                PaymentChargeID = null,
                PatientPaymentChargeID = PatientPaymentChargeID,
                PatientPlanID = Charge.PrimaryPatientPlanID.Value,
                VisitID = Charge.VisitID.Value,
                Amount = AllocatedAmount
            };
            return ledger;
        }

        [Route("DeletePatientPayment/{id}")]
        [HttpDelete]

        public async Task<ActionResult> DeletePatientPayment(long id)
        {
            var PatientPayment = await _context.PatientPayment.FindAsync(id);

            if (PatientPayment == null)
            {
                return NotFound();
            }
            //if(PatientPayment.Status=="C") 
            _context.PatientPayment.Remove(PatientPayment);
            await _context.SaveChangesAsync();

            return Ok();
        }



    }
}
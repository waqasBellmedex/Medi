using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MediFusionPM.Models;
using MediFusionPM.Models.TodoApi.Models;
using MediFusionPM.ViewModels;
using static MediFusionPM.ViewModels.VMPaymentCheck;
using static MediFusionPM.ViewModels.VMCheckInfo;
using MediFusionPM.BusinessLogic.EraParsing;
using MediFusionPM.Models.TodoApi;
using System.Text;
using System.IO;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using static MediFusionPM.ViewModels.VMCommon;
using System.IO.Compression;
using MediFusionPM.BusinessLogic;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class PaymentCheckController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;

        public PaymentCheckController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
        }

        [HttpGet]
        [Route("GetPaymentChecks")]
        public async Task<ActionResult<IEnumerable<PaymentCheck>>> GetPaymentChecks()
        {
            return await _context.PaymentCheck.ToListAsync();
        }


        [Route("FindPaymentCheck/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentCheck>> FindPaymentCheck(long id)
        {
            var PaymentCheck = await _context.PaymentCheck.FindAsync(id);
            if (PaymentCheck == null)
            {
                return NotFound();
            }
            else
            {
                // PaymentCheck.PaymentVisit = _context.PaymentVisit.Where(m => m.PaymentCheckID == id).ToList<PaymentVisit>();
                var PaymentVisit = (from pVisit in _context.PaymentVisit
                                    join pat in _context.Patient on pVisit.PatientID equals pat.ID into p
                                    from pat in p.DefaultIfEmpty()
                                    where pVisit.PaymentCheckID == id
                                    select new PaymentVisit()
                                    {
                                        ID = pVisit.ID,
                                        PaymentCheckID = pVisit.PaymentCheckID,
                                        VisitID = pVisit.VisitID,
                                        ClaimNumber = pVisit.ClaimNumber,
                                        BatchDocumentID = pVisit.BatchDocumentID,
                                        PageNumber = pVisit.PageNumber,
                                        MyProperty = pVisit.MyProperty,
                                        ProcessedAs = pVisit.ProcessedAs,
                                        BilledAmount = pVisit.BilledAmount.Val(),
                                        PaidAmount = pVisit.PaidAmount.Val(),
                                        AllowedAmount = pVisit.AllowedAmount.Val(),
                                        WriteOffAmount = pVisit.WriteOffAmount.Val(),
                                        PatientAmount = pVisit.PatientAmount.Val(),
                                        PayerICN = pVisit.PayerICN,
                                        InsuredID = pVisit.InsuredID,
                                        ProvLastName = pVisit.ProvLastName,
                                        ProvFirstName = pVisit.ProvFirstName,
                                        ProvNPI = pVisit.ProvNPI,
                                        PayerContactNumber = pVisit.PayerContactNumber,
                                        ForwardedPayerName = pVisit.ForwardedPayerName,
                                        ForwardedPayerID = pVisit.ForwardedPayerID,
                                        ClaimStatementFromDate = pVisit.ClaimStatementFromDate,
                                        ClaimStatementToDate = pVisit.ClaimStatementToDate,
                                        PayerReceivedDate = pVisit.PayerReceivedDate,
                                        Status = pVisit.Status,
                                        AddedDate = pVisit.AddedDate,
                                        AddedBy = pVisit.AddedBy,
                                        UpdatedBy = pVisit.UpdatedBy,
                                        UpdatedDate = pVisit.UpdatedDate,
                                        PostedDate = pVisit.PostedDate,
                                        Comments = pVisit.Comments,
                                        PostedBy = pVisit.PostedBy,
                                        PatientID = pVisit.PatientID,
                                        PatientName = pat.LastName + ", " + pat.FirstName,
                                        InsuredFirstName = pVisit.InsuredFirstName,
                                        InsuredLastName = pVisit.InsuredLastName,
                                        PatientFIrstName = pVisit.PatientFIrstName,
                                        PatientLastName = pVisit.PatientLastName
                                    }).ToList();
                PaymentCheck.PaymentVisit = PaymentVisit;
                foreach (PaymentVisit payment in PaymentCheck.PaymentVisit)
                {
                    payment.PaymentCharge = _context.PaymentCharge.OrderBy(m => m.ChargeID).Where(m => m.PaymentVisitID == payment.ID).ToList<PaymentCharge>();
                }
            }
            return PaymentCheck;
        }


        [Route("GetProfiles")]
        [HttpGet()]
        public async Task<ActionResult<VMPaymentCheck>> GetProfiles(long id)
        {
            ViewModels.VMPaymentCheck obj = new ViewModels.VMPaymentCheck();
            obj.GetProfiles(_context);

            return obj;
        }

        [Route("GetCheckPaymentInfo/{PaymentCheckId}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentCheck>> GetCheckPaymentInfo(long PaymentCheckId)
        {
            PaymentCheck check = (from p in _context.PaymentCheck
                                  where p.ID == PaymentCheckId
                                  select p).FirstOrDefault();
            if (check == null)
            {
                NotFound();
            }
            check.PaymentVisit = (from p in _context.PaymentVisit where p.PaymentCheckID == check.ID select p)
                                .ToList<PaymentVisit>();
            return check;
        }

        [Route("GetVisitPaymentInfo/{PaymentVisitId}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentVisit>> GetVisitPaymentInfo(long PaymentVisitId)
        {
            PaymentVisit Visit = (from p in _context.PaymentVisit
                                  where p.ID == PaymentVisitId
                                  select p
                       ).FirstOrDefault();

            if (Visit == null)
            {
                NotFound();
            }
            Visit.PaymentCharge = (from p in _context.PaymentCharge where p.PaymentVisitID == Visit.ID select p)
                                .ToList<PaymentCharge>();
            return Visit;
        }
        [Route("FindChargeSubmissionHistory/{ID}")]
        [HttpGet("{ID}")]
        public List<ChargeSubmissionHistory> ChargeSubmissionHistory(long ID)
        {
            List<ChargeSubmissionHistory> data = (from csh in _context.ChargeSubmissionHistory
                                                  join pPlan in _context.PatientPlan on csh.PatientPlanID equals pPlan.ID
                                                  join iPlan in _context.InsurancePlan on pPlan.InsurancePlanID equals iPlan.ID
                                                  join pType in _context.PlanType on iPlan.PlanTypeID equals pType.ID

                                                  where csh.ID == ID
                                                  select new ChargeSubmissionHistory()
                                                  {
                                                      ID = csh.ID,
                                                      ChargeID = csh.ChargeID,
                                                      ReceiverID = csh.ReceiverID,
                                                      SubmissionLogID = csh.SubmissionLogID,
                                                      SubmitType = csh.SubmitType,
                                                      // PatientPlanID = pType.Description,
                                                      Amount = csh.Amount,
                                                      AddedBy = csh.AddedBy,
                                                      AddedDate = csh.AddedDate,
                                                      UpdatedBy = csh.UpdatedBy,
                                                      UpdatedDate = csh.UpdatedDate,
                                                  })

                                                   .ToList();
            return data;
        }

        [HttpPost]
        [Route("FindPaymentChecks")]
        public async Task<ActionResult<IEnumerable<GPaymentCheck>>> FindPaymentChecks(CPaymentCheck CPaymentCheck)
        {
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindPaymentChecks(CPaymentCheck, PracticeId);

        }
        private List<GPaymentCheck> FindPaymentChecks(CPaymentCheck CPaymentCheck, long PracticeId)
        {

            if (CPaymentCheck.Status == "F")
            {
                List<GPaymentCheck> lst = (from pc in _context.PaymentCheck

                                           join rec in _context.Receiver on pc.ReceiverID equals rec.ID into Table1
                                           from t1 in Table1.DefaultIfEmpty()
                                           join df in _context.DownloadedFile on pc.DownloadedFileID equals df.ID into Table2
                                           from downloadedFile in Table2.DefaultIfEmpty()
                                           where 
                                           (ExtensionMethods.IsBetweenDOS(CPaymentCheck.EntryDateTo, CPaymentCheck.EntryDateFrom, pc.AddedDate, pc.AddedDate)) &&
                                           (ExtensionMethods.IsBetweenDOS(CPaymentCheck.CheckDateTo, CPaymentCheck.CheckDateFrom, pc.CheckDate, pc.CheckDate)) &&
                                           (ExtensionMethods.IsBetweenDOS(CPaymentCheck.PostedToDate, CPaymentCheck.PostedFromDate, pc.PostedDate, pc.PostedDate)) &&

                                          (CPaymentCheck.CheckNumber.IsNull2() ? true : pc.CheckNumber.Contains(CPaymentCheck.CheckNumber)) &&
                                          // (CPaymentCheck.Practice.IsNull2() ? true : prac.ID.Equals(CPaymentCheck.Practice)) &&
                                          (CPaymentCheck.Payer.IsNull2() ? true : pc.PayerID.Equals(CPaymentCheck.Payer)) &&
                                          (CPaymentCheck.ReceiverID.IsNull() ? true : pc.ReceiverID.Equals(CPaymentCheck.ReceiverID)) &&
                                          //StatusCheck(CPaymentCheck.Status, pc.Status)
                                          (CPaymentCheck.Status.Equals("A") ? true : pc.Status.Equals(CPaymentCheck.Status)) &&
                                          (CPaymentCheck.TypeID.IsNull2() ? true : CPaymentCheck.TypeID.Equals("M") ? pc.PaymentMethod == null : CPaymentCheck.TypeID.Equals("ERA") ? pc.PaymentMethod != null : false)

                                           orderby pc.AddedDate descending
                                           select new GPaymentCheck()
                                           {
                                               Id = pc.ID,
                                               CheckNumber = pc.CheckNumber,
                                               PaymentMethod = pc.PaymentMethod.IsNull2() ? "Manual Posting" : "ERA",
                                               CheckDate = pc.CheckDate.Format("MM/dd/yyyy"),
                                               CheckAmount = pc.CheckAmount,
                                               Appliedamount = pc.AppliedAmount,
                                               PostedAmount = pc.PostedAmount,
                                               NumberOfVisits = pc.NumberOfVisits,
                                               NumberOfPatients = pc.NumberOfPatients,
                                               Status = TranslateStatus(pc.Status),
                                               Payer = pc.PayerName,
                                               //     Practice = prac.Name,
                                               EntryDate = pc.AddedDate.Format("MM/dd/yyyy hh:mm tt"),
                                               EnteredBy = pc.AddedBy,
                                               ReceiverID = pc.ReceiverID,
                                               Receiver = t1.Name,
                                               PostedDate = pc.PostedDate.Format("MM/dd/yyyy"),
                                               PayeeName = pc.PayeeName,
                                               PayeeNPI = pc.PayeeNPI,
                                               FileName = Path.GetFileName(downloadedFile.FilePath)
                                           }).ToList();
                // return lst;
                // Location 
                if (!CPaymentCheck.Location.IsNull2())
                {
                    lst = (from l in lst
                           join p in _context.Practice
                           on l.PracticeID equals p.ID
                           join loc in _context.Location
                           on p.ID equals loc.PracticeID
                           where loc.OrganizationName.Contains(CPaymentCheck.Location)
                           select l).ToList();
                }

                if (!CPaymentCheck.Provider.IsNull2())
                {
                    lst = (from l in lst
                           join p in _context.Practice
                           on l.PracticeID equals p.ID
                           join pro in _context.Provider
                           on p.ID equals pro.PracticeID
                           where pro.Name.Contains(CPaymentCheck.Provider)
                           select l).ToList();

                }

                return lst;

            }
        

            else
            {
                List<GPaymentCheck> lst = (from pc in _context.PaymentCheck

                                           join rec in _context.Receiver on pc.ReceiverID equals rec.ID into Table1
                                           from t1 in Table1.DefaultIfEmpty()
                                           join df in _context.DownloadedFile on pc.DownloadedFileID equals df.ID into Table2
                                           from downloadedFile in Table2.DefaultIfEmpty()
                                           where pc.PracticeID == PracticeId &&
                                           (ExtensionMethods.IsBetweenDOS(CPaymentCheck.EntryDateTo, CPaymentCheck.EntryDateFrom, pc.AddedDate, pc.AddedDate)) &&
                                           (ExtensionMethods.IsBetweenDOS(CPaymentCheck.CheckDateTo, CPaymentCheck.CheckDateFrom, pc.CheckDate, pc.CheckDate)) &&
                                           (ExtensionMethods.IsBetweenDOS(CPaymentCheck.PostedToDate, CPaymentCheck.PostedFromDate, pc.PostedDate, pc.PostedDate)) &&

                                          (CPaymentCheck.CheckNumber.IsNull2() ? true : pc.CheckNumber.Equals(CPaymentCheck.CheckNumber)) &&
                                          // (CPaymentCheck.Practice.IsNull2() ? true : prac.ID.Equals(CPaymentCheck.Practice)) &&
                                          (CPaymentCheck.Payer.IsNull2() ? true : pc.PayerID.Equals(CPaymentCheck.Payer)) &&
                                          (CPaymentCheck.ReceiverID.IsNull() ? true : pc.ReceiverID.Equals(CPaymentCheck.ReceiverID)) &&
                                          //StatusCheck(CPaymentCheck.Status, pc.Status)
                                          (CPaymentCheck.Status.Equals("A") ? true : pc.Status.Equals(CPaymentCheck.Status)) &&
                                          (CPaymentCheck.TypeID.IsNull2() ? true : CPaymentCheck.TypeID.Equals("M") ? pc.PaymentMethod == null : CPaymentCheck.TypeID.Equals("ERA") ? pc.PaymentMethod != null : false)

                                           orderby pc.AddedDate descending
                                           select new GPaymentCheck()
                                           {
                                               Id = pc.ID,
                                               CheckNumber = pc.CheckNumber,
                                               PaymentMethod = pc.PaymentMethod.IsNull2() ? "Manual Posting" : "ERA",
                                               CheckDate = pc.CheckDate.Format("MM/dd/yyyy"),
                                               CheckAmount = pc.CheckAmount,
                                               Appliedamount = pc.AppliedAmount,
                                               PostedAmount = pc.PostedAmount,
                                               NumberOfVisits = pc.NumberOfVisits,
                                               NumberOfPatients = pc.NumberOfPatients,
                                               Status = TranslateStatus(pc.Status),
                                               Payer = pc.PayerName,
                                               //     Practice = prac.Name,
                                               EntryDate = pc.AddedDate.Format("MM/dd/yyyy hh:mm tt"),
                                               EnteredBy = pc.AddedBy,
                                               ReceiverID = pc.ReceiverID,
                                               Receiver = t1.Name,
                                               PostedDate = pc.PostedDate.Format("MM/dd/yyyy"),
                                               PayeeName = pc.PayeeName,
                                               PayeeNPI = pc.PayeeNPI,
                                               FileName = Path.GetFileName(downloadedFile.FilePath)
                                           }).ToList();
                // return lst;
                // Location 
                if (!CPaymentCheck.Location.IsNull2())
                {
                    lst = (from l in lst
                           join p in _context.Practice
                           on l.PracticeID equals p.ID
                           join loc in _context.Location
                           on p.ID equals loc.PracticeID
                           where loc.OrganizationName.Contains(CPaymentCheck.Location)
                           select l).ToList();
                }

                if (!CPaymentCheck.Provider.IsNull2())
                {
                    lst = (from l in lst
                           join p in _context.Practice
                           on l.PracticeID equals p.ID
                           join pro in _context.Provider
                           on p.ID equals pro.PracticeID
                           where pro.Name.Contains(CPaymentCheck.Provider)
                           select l).ToList();

                }

                return lst;

            }

        }




        //public string TranslatePaymentStatus(string Status)
        //{
        //    string desc = string.Empty;

        //    if (Status == "" || Status==null)
        //    {
        //        desc = "Manual Posting";
        //    }
        //    if (Status == "M")
        //    {
        //        desc = "Manual Posting";
        //    }
        //    if (Status == "E")
        //    {
        //        desc = "ERA";
        //    }
        //   // else { desc = Status; }
        //    return desc;
        //}


        public string TranslateStatus(string Status)
        {
            string desc = string.Empty;
            if (Status == "C")
            {
                desc = "CLOSE";
            }
            else if (Status == "NP")
            {
                desc = "NEED POSTING";
            }
            else if (Status == "P")
            {
                desc = "POSTED";
            }
            else if (Status == "F")
            {
                desc = "FAILED";
            }
            else if (Status == "NR")
            {
                desc = "NOT RELATED";
            }

            return desc;
        }
        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CPaymentCheck CPaymentCheck)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GPaymentCheck> data = FindPaymentChecks(CPaymentCheck, PracticeId);
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CPaymentCheck, "Payment Check Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CPaymentCheck CPaymentCheck)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GPaymentCheck> data = FindPaymentChecks(CPaymentCheck, PracticeId);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }



        public bool StatusCheck(string Cstatus, string DStatus)
        {
            bool result = false;
            if (Cstatus.Equals("A"))
            {
                result = true;
            }
            else if (Cstatus.Equals(DStatus))
            {
                result = true;
            }
            return result;
        }

        [Route("SavePaymentCheck")]
        [HttpPost]
        public async Task<ActionResult<PaymentCheck>> SavePaymentCheck(PaymentCheck item)
        {
            string Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            bool succ = TryValidateModel(item);
            bool CheckExists = _context.PaymentCheck.Count(p => p.CheckNumber == item.CheckNumber && item.ID == 0 && (p.Status != "NR" || p.Status != "F")) > 0;

            if (CheckExists)
            {
                //return BadRequest("CheckNumber : " + item.CheckNumber + "  Already Exists Please Enter unique CheckNumber");
                return BadRequest("Already Exists Please Enter unique CheckNumber");
            }
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }

            long patientCount = item.PaymentVisit.Select(o => o.PatientID).Distinct().Count();

            long? defaultActionID = _context.Action.Where(a => a.Name == "EOB").FirstOrDefault()?.ID;
            long? defaultGroupID = _context.Group.Where(a => a.Name == "EOB").FirstOrDefault()?.ID;
            long? defaultReasonID = _context.Reason.Where(a => a.Name == "EOB").FirstOrDefault()?.ID;

            var transactionOption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (var objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption, TransactionScopeAsyncFlowOption.Enabled))
            {
                #region Batch Linking
                PaymentCheck OldPaymentCheck = _context.PaymentCheck.Where(p => p.ID == item.ID).AsNoTracking().FirstOrDefault();
                long BatchDocumentIDToUse = 0;
                BatchDocument Batch = null;
                if (item.BatchDocument != null)
                {
                    Batch = item.BatchDocument;
                }
                if (Batch == null)
                {
                    if (item.BatchDocumentID.GetValueOrDefault() != 0)
                    {
                        Batch = _context.BatchDocument.Where(bd => bd.ID == item.BatchDocumentID.GetValueOrDefault()).FirstOrDefault();
                    }
                    else
                    {
                        if (OldPaymentCheck != null)
                        {
                            if (OldPaymentCheck.BatchDocumentID.GetValueOrDefault() != 0)
                            {
                                Batch = _context.BatchDocument.Where(bd => bd.ID == OldPaymentCheck.BatchDocumentID.GetValueOrDefault()).FirstOrDefault();
                            }
                        }
                    }
                }
                if (Batch != null)
                {
                    if (OldPaymentCheck != null)
                    {
                        bool CounterChangedOnce = false;
                        if ((OldPaymentCheck.PageNumber != null && OldPaymentCheck.PageNumber != "") && (item.PageNumber == null && item.PageNumber == ""))
                        {
                            Batch.NoOfDemographicsEntered = Batch.NoOfDemographicsEntered.ValZero() - 1;
                            if (Batch.StartDate == null) Batch.StartDate = DateTime.Now;
                            CounterChangedOnce = true;
                        }
                        else if ((OldPaymentCheck.PageNumber == null && OldPaymentCheck.PageNumber == "") && (item.PageNumber != null && item.PageNumber != ""))
                        {
                            Batch.NoOfDemographicsEntered = Batch.NoOfDemographicsEntered.ValZero() + 1;
                            if (Batch.StartDate == null) Batch.StartDate = DateTime.Now;
                            CounterChangedOnce = true;
                        }
                        if (OldPaymentCheck.BatchDocumentID.GetValueOrDefault() != 0 && item.BatchDocumentID.GetValueOrDefault() == 0)
                        {
                            if (CounterChangedOnce == false)
                                Batch.NoOfDemographicsEntered = Batch.NoOfDemographicsEntered.ValZero() - 1;
                            if (Batch.StartDate == null) Batch.StartDate = DateTime.Now;
                            item.BatchDocument = null;
                        }
                        else if ((OldPaymentCheck.BatchDocumentID.GetValueOrDefault() == 0 && item.BatchDocumentID.GetValueOrDefault() != 0))
                        {
                            if (CounterChangedOnce == false)
                                Batch.NoOfDemographicsEntered = Batch.NoOfDemographicsEntered.ValZero() + 1;
                            if (Batch.StartDate == null) Batch.StartDate = DateTime.Now;
                        }
                    }
                }
                if (item.ID == 0)
                {
                    if (item.BatchDocumentID.GetValueOrDefault() != 0)
                    {
                        Batch.NoOfDemographicsEntered = Batch.NoOfDemographicsEntered.ValZero() + 1;
                        if (Batch.StartDate == null) Batch.StartDate = DateTime.Now;
                        item.DocumentBatchApplied = true;
                    }
                }
                if (item.BatchDocumentID != null && !item.PageNumber.IsNull2() && Batch != null)
                {
                    BatchDocumentPayment batPayment = _context.BatchDocumentPayment.Where(m => m.BatchDocumentNoID == Batch.ID && m.CheckDate.Date() == item.CheckDate.Date()).FirstOrDefault();
                    if (batPayment == null)
                    {
                        batPayment = new BatchDocumentPayment();

                        batPayment.BatchDocumentNoID = Batch.ID;
                        batPayment.AddedBy = Email;
                        batPayment.AddedDate = DateTime.Now;
                        batPayment.CheckDate = item.CheckDate;
                        batPayment.CheckAmount = item.CheckAmount;
                        batPayment.CheckNo = item.CheckNumber;

                        if (Batch.StartDate == null)
                        {
                            Batch.StartDate = DateTime.Now;
                            _context.BatchDocument.Update(Batch);
                        }
                        _context.BatchDocumentPayment.Update(batPayment);
                    }
                    else
                    {
                        batPayment.UpdatedBy = Email;
                        batPayment.UpdatedDate = DateTime.Now;
                        batPayment.CheckDate = item.CheckDate;
                        batPayment.CheckAmount = item.CheckAmount;

                        if (Batch.StartDate == null)
                        {
                            Batch.StartDate = DateTime.Now;
                            _context.BatchDocument.Update(Batch);
                        }
                        _context.BatchDocumentPayment.Update(batPayment);
                    }
                }
                if (Batch != null)
                {
                    _context.BatchDocument.Update(Batch);
                }
                //if (item.BatchDocumentID != null && !item.PageNumber.IsNull2())
                //{
                //    BatchDocument batch = _context.BatchDocument.Find(item.BatchDocumentID);
                //    if (batch != null && item.DocumentBatchApplied != true)
                //    {
                //        BatchDocumentPayment batPayment = _context.BatchDocumentPayment.Where(m => m.BatchDocumentNoID == batch.ID && m.CheckDate.Date() == item.CheckDate.Date()).FirstOrDefault();
                //        if (batPayment == null)
                //        {
                //            batPayment = new BatchDocumentPayment();

                //            batPayment.BatchDocumentNoID = batch.ID;
                //            batPayment.AddedBy = Email;
                //            batPayment.AddedDate = DateTime.Now;
                //            batPayment.CheckDate = item.CheckDate;
                //            batPayment.CheckAmount = item.CheckAmount;
                //            batPayment.CheckNo = item.CheckNumber;

                //            if (batch.StartDate == null)
                //            {
                //                batch.StartDate = DateTime.Now;
                //                _context.BatchDocument.Update(batch);
                //            }
                //            _context.BatchDocumentPayment.Update(batPayment);
                //        }
                //        else
                //        {
                //            batPayment.UpdatedBy = Email;
                //            batPayment.UpdatedDate = DateTime.Now;
                //            batPayment.CheckDate = item.CheckDate;
                //            batPayment.CheckAmount = item.CheckAmount;

                //            if (batch.StartDate == null)
                //            {
                //                batch.StartDate = DateTime.Now;
                //                _context.BatchDocument.Update(batch);
                //            }
                //            _context.BatchDocumentPayment.Update(batPayment);
                //        }
                //    }
                //}
                //else if (item.ID > 0 && item.BatchDocumentID == null && item.PageNumber.IsNull2() && item.DocumentBatchApplied == true)
                //{
                //    PaymentCheck pc = _context.PaymentCheck.Find(item.ID);
                //    BatchDocument batch = _context.BatchDocument.Find(pc.BatchDocumentID);

                //    if (batch != null && batch.NoOfDemographicsEntered.ValZero() > 0)
                //    {
                //        BatchDocumentPayment batCharge = _context.BatchDocumentPayment.Where(m => m.BatchDocumentNoID == batch.ID && m.CheckDate.Date() == item.CheckDate.Date()).FirstOrDefault();

                //    }
                //}
                #endregion
                if (item.ID <= 0)
                {
                    //item.AppliedAmount = item.CheckAmount;
                    item.NumberOfVisits = item.PaymentVisit.Count;
                    item.NumberOfPatients = patientCount;
                    item.AddedBy = Email;
                    item.AddedDate = DateTime.Now;
                    _context.PaymentCheck.Add(item);

                    foreach (PaymentVisit payVisit in item.PaymentVisit)
                    {
                        if (payVisit.ID <= 0)
                        {
                            payVisit.AddedBy = Email;
                            payVisit.AddedDate = DateTime.Now;
                            _context.PaymentVisit.Add(payVisit);
                        }
                        else
                        {
                            payVisit.UpdatedBy = Email;
                            payVisit.UpdatedDate = DateTime.Now;
                            _context.PaymentVisit.Update(payVisit);
                            //await _context.SaveChangesAsync();
                        }

                        //long? adjustmentCodeID = null;
                        foreach (PaymentCharge payCharge in payVisit.PaymentCharge)
                        {
                            if (payCharge.AllowedAmount.IsNull() && payCharge.WriteoffAmount.IsNull() && payCharge.AdjustmentCodeID1.IsNull() && payCharge.AdjustmentCodeID2.IsNull())
                            {
                                continue;
                            }

                            if (payCharge.ID <= 0)
                            {
                                payCharge.AddedBy = Email;
                                payCharge.AddedDate = DateTime.Now;
                                _context.PaymentCharge.Add(payCharge);
                                //await _context.SaveChangesAsync();
                            }
                            else
                            {
                                payCharge.UpdatedBy = Email;
                                payCharge.UpdatedDate = DateTime.Now;
                                _context.PaymentCharge.Update(payCharge);
                                //await _context.SaveChangesAsync();
                            }
                        }

                        this.CreateFollowup(payVisit, defaultActionID, defaultReasonID, defaultGroupID, Email);
                    }
                }
                else
                {
                    bool CheckNumberExistsUpdate = _context.PaymentCheck.Any(p => p.CheckNumber == item.CheckNumber && item.ID != p.ID);

                    if (CheckNumberExistsUpdate == true)
                    {
                        // return BadRequest("CheckNumber : " + item.CheckNumber + " Already Exists Please Enter unique CheckNumber");
                        return BadRequest("Already Exists Please Enter unique CheckNumber");
                    }

                    //item.AppliedAmount = item.CheckAmount;
                    item.NumberOfVisits = item.PaymentVisit.Count;
                    item.NumberOfPatients = patientCount;
                    item.UpdatedBy = Email;
                    item.UpdatedDate = DateTime.Now;

                    _context.PaymentCheck.Update(item);

                    foreach (PaymentVisit payVisit in item.PaymentVisit)
                    {
                        if (payVisit.ID <= 0)
                        {
                            payVisit.AddedBy = Email;
                            payVisit.AddedDate = DateTime.Now;
                            _context.PaymentVisit.Add(payVisit);
                        }
                        else
                        {
                            payVisit.UpdatedBy = Email;
                            payVisit.UpdatedDate = DateTime.Now;
                            _context.PaymentVisit.Update(payVisit);
                        }

                        this.CreateFollowup(payVisit, defaultActionID, defaultReasonID, defaultGroupID, Email);
                    }


                    int totalPayments = _context.PaymentVisit.Where(v => v.PaymentCheckID == item.ID).Count();
                    int postedPayments = _context.PaymentVisit.Where(v => v.PaymentCheckID == item.ID && v.Status == "P").Count();

                    if (totalPayments == postedPayments)
                        item.Status = "P";
                    else item.Status = "NP";


                    _context.PaymentCheck.Update(item);
                }

                await _context.SaveChangesAsync();
                objTrnScope.Complete();
            }

            return Ok(item);
        }


        private void CreateFollowup(PaymentVisit PayVisit, long? DefaultActionID, long? DefaultReasonID,
                                       long? DefaultGroupID, string Email, ICollection<PaymentCharge> PaymentCharges = null)
        {
            PlanFollowup followup = null;
            long? adjustmentCodeID = null;

            if (PayVisit.PaymentCharge != null) PaymentCharges = PayVisit.PaymentCharge;

            foreach (PaymentCharge payCharge in PaymentCharges)
            {
                if (payCharge.AllowedAmount.IsNull() && payCharge.WriteoffAmount.IsNull() && payCharge.AdjustmentCodeID1.IsNull() && payCharge.AdjustmentCodeID2.IsNull())
                {
                    continue;
                }
                if (payCharge.PaidAmount.IsNull() && payCharge.PatientAmount.IsNull() && payCharge.WriteoffAmount.IsNull())
                {
                    adjustmentCodeID = payCharge.AdjustmentCodeID1;

                    long? actionID = null, reasonID = null, groupID = null;
                    AdjustmentCode adjCode = _context.AdjustmentCode.Find(adjustmentCodeID);

                    if (adjCode != null)
                    {
                        actionID = adjCode.ActionID.IsNull() ? DefaultActionID : adjCode.ActionID;
                        reasonID = adjCode.ReasonID.IsNull() ? DefaultReasonID : adjCode.ReasonID;
                        groupID = adjCode.GroupID.IsNull() ? DefaultGroupID : adjCode.GroupID;
                    }

                    if (followup == null)
                    {
                        followup = _context.PlanFollowUp.Where(v => v.VisitID == PayVisit.VisitID).FirstOrDefault();
                        if (followup == null)
                        {
                            followup = new PlanFollowup()
                            {
                                ActionID = actionID,
                                AddedBy = Email,
                                AddedDate = System.DateTime.Now,
                                GroupID = groupID,
                                Notes = "",
                                PaymentVisitID = PayVisit.ID,
                                ReasonID = reasonID,
                                AdjustmentCodeID = adjustmentCodeID,
                                TickleDate = null,
                                VisitID = PayVisit.VisitID,
                                VisitStatusID = null
                            };
                            _context.PlanFollowUp.Add(followup);
                        }
                        else
                        {
                            followup.ActionID = actionID;
                            followup.UpdatedBy = Email;
                            followup.UpdatedDate = DateTime.Now;
                            followup.GroupID = groupID;
                            followup.PaymentVisitID = PayVisit.ID;
                            followup.ReasonID = reasonID;
                            followup.AdjustmentCodeID = adjustmentCodeID;

                            _context.PlanFollowUp.Update(followup);
                        }
                    }

                    PlanFollowupCharge followupCharge = _context.PlanFollowupCharge.Where(p => p.PlanFollowupID == followup.ID && p.ChargeID == payCharge.ChargeID).FirstOrDefault();
                    if (followupCharge == null)
                    {
                        followupCharge = new PlanFollowupCharge()
                        {
                            PlanFollowupID = followup.ID,
                            ChargeID = payCharge.ChargeID,
                            GroupID = groupID,
                            ActionID = actionID,
                            AdjustmentCodeID = adjustmentCodeID,
                            PaymentChargeID = payCharge.ID,
                            AddedBy = Email,
                            AddedDate = DateTime.Now,
                            ReasonID = reasonID,
                            RemarkCode1ID = payCharge.RemarkCodeID1,
                            RemarkCode2ID = payCharge.RemarkCodeID2,
                            RemarkCode3ID = payCharge.RemarkCodeID3,
                            RemarkCode4ID = payCharge.RemarkCodeID4
                        };
                        _context.PlanFollowupCharge.Add(followupCharge);
                    }
                    else
                    {
                        followupCharge.GroupID = groupID;
                        followupCharge.ActionID = actionID;
                        followupCharge.ChargeID = payCharge.ChargeID;
                        followupCharge.AdjustmentCodeID = adjustmentCodeID;
                        followupCharge.PaymentChargeID = payCharge.ID;
                        followupCharge.UpdatedBy = Email;
                        followupCharge.UpdatedDate = DateTime.Now;
                        followupCharge.ReasonID = reasonID;
                        followupCharge.RemarkCode1ID = payCharge.RemarkCodeID1;
                        followupCharge.RemarkCode2ID = payCharge.RemarkCodeID2;
                        followupCharge.RemarkCode3ID = payCharge.RemarkCodeID3;
                        followupCharge.RemarkCode4ID = payCharge.RemarkCodeID4;
                        _context.PlanFollowupCharge.Update(followupCharge);
                    }
                }
            }
        }



        private FollowUpData CreatePatientFollowup(long PatientID, long? DefaultActionID, long? DefaultReasonID,
                                       long? DefaultGroupID, string Email, PaymentCharge PaymentCharge, List<PatientFollowUp> FollowUpList, List<PatientFollowUpCharge> FollowUpChargeList)
        {
            PatientFollowUp followup = null;
            PatientFollowUpCharge followupCharge = null;
            long? adjustmentCodeID = null;

            adjustmentCodeID = PaymentCharge.AdjustmentCodeID1;
            AdjustmentCode adjCode = null;

            long? actionID = null, reasonID = null, groupID = null;
            if (adjustmentCodeID != null)
                adjCode = _context.AdjustmentCode.Find(adjustmentCodeID);

            if (adjCode != null)
            {
                actionID = adjCode.ActionID.IsNull() ? DefaultActionID : adjCode.ActionID;
                reasonID = adjCode.ReasonID.IsNull() ? DefaultReasonID : adjCode.ReasonID;
                groupID = adjCode.GroupID.IsNull() ? DefaultGroupID : adjCode.GroupID;
            }
            bool UpdateFollowup = false;
            bool UpdateFollowupCharge = false;
            int FollowUpIndex, FollowUpChargeIndex;
            if (followup == null)
            {



                if (FollowUpList.Count == 0)
                {
                    followup = _context.PatientFollowUp.Where(v => v.PatientID == PatientID).FirstOrDefault();

                }
                else if (FollowUpList.Count > 0 && FollowUpList.Where(p => p.PatientID.Equals(PatientID)).FirstOrDefault() == null)
                {
                    followup = _context.PatientFollowUp.Where(v => v.PatientID == PatientID).FirstOrDefault();
                }
                else
                {
                    followup = FollowUpList.Where(p => p.PatientID.Equals(PatientID)).FirstOrDefault();
                    UpdateFollowup = true;
                    FollowUpIndex = FollowUpList.IndexOf(followup);
                }

                //}

                if (followup == null)
                {
                    followup = new PatientFollowUp()
                    {
                        ActionID = actionID,
                        AddedBy = Email,
                        AddedDate = System.DateTime.Now,
                        GroupID = groupID,
                        Notes = "",
                        PaymentVisitID = PaymentCharge.PaymentVisitID,
                        ReasonID = reasonID,
                        //AdjustmentCodeID = adjustmentCodeID,
                        TickleDate = null,
                        PatientID = PatientID,
                        Statement1SentDate = null,
                        Statement2SentDate = null,
                        Statement3SentDate = null,
                        Status = ""
                    };
                    FollowUpList.Add(followup);
                    //_context.PatientFollowUp.Add(followup);
                }
                else
                {
                    if (UpdateFollowup)
                    {
                        FollowUpList.Remove(followup);
                        followup.ActionID = actionID;
                        followup.UpdatedBy = Email;
                        followup.UpdatedDate = DateTime.Now;
                        followup.GroupID = groupID;
                        followup.PaymentVisitID = PaymentCharge.PaymentVisitID;
                        followup.ReasonID = reasonID;
                        FollowUpList.Add(followup);
                    }
                    else
                    {
                        followup.ActionID = actionID;
                        followup.UpdatedBy = Email;
                        followup.UpdatedDate = DateTime.Now;
                        followup.GroupID = groupID;
                        followup.PaymentVisitID = PaymentCharge.PaymentVisitID;
                        followup.ReasonID = reasonID;
                        //followup.AdjustmentCodeID = adjustmentCodeID;
                        _context.PatientFollowUp.Update(followup);
                    }
                }
            }


            if (FollowUpChargeList.Count == 0)
            {
                followupCharge = _context.PatientFollowUpCharge.Where(p => p.PatientFollowUpID == followup.ID && p.ChargeID == PaymentCharge.ChargeID).FirstOrDefault();
            }
            else if (FollowUpChargeList.Count > 0 && FollowUpChargeList.Where(p => p.PatientFollowUpID.Equals(followup.ID) && p.ChargeID.Equals(PaymentCharge.ChargeID)).FirstOrDefault() == null)
            {
                followupCharge = _context.PatientFollowUpCharge.Where(p => p.PatientFollowUpID == followup.ID && p.ChargeID == PaymentCharge.ChargeID).FirstOrDefault();
            }
            else
            {
                followupCharge = FollowUpChargeList.Where(p => p.PatientFollowUpID.Equals(followup.ID) && p.ChargeID.Equals(PaymentCharge.ChargeID)).FirstOrDefault();
                UpdateFollowupCharge = true;
                FollowUpChargeIndex = FollowUpChargeList.IndexOf(followupCharge);
            }

            if (followupCharge == null)
            {
                followupCharge = new PatientFollowUpCharge()
                {
                    PatientFollowUpID = followup.ID,
                    ChargeID = PaymentCharge.ChargeID,
                    GroupID = groupID,
                    ActionID = actionID,
                    //AdjustmentCodeID = adjustmentCodeID,
                    PaymentChargeID = PaymentCharge.ID,
                    AddedBy = Email,
                    AddedDate = DateTime.Now,
                    ReasonID = reasonID,
                    Status = ""
                };
                FollowUpChargeList.Add(followupCharge);
            }
            else

            {
                if (UpdateFollowupCharge)
                {
                    FollowUpChargeList.Remove(followupCharge);
                    followupCharge.GroupID = groupID;
                    followupCharge.ActionID = actionID;
                    //followupCharge.AdjustmentCodeID = adjustmentCodeID;
                    followupCharge.PaymentChargeID = PaymentCharge.ID;
                    followupCharge.UpdatedBy = Email;
                    followupCharge.UpdatedDate = DateTime.Now;
                    followupCharge.ReasonID = reasonID;
                    FollowUpChargeList.Add(followupCharge);
                }
                else
                {
                    followupCharge.GroupID = groupID;
                    followupCharge.ActionID = actionID;
                    //followupCharge.AdjustmentCodeID = adjustmentCodeID;
                    followupCharge.PaymentChargeID = PaymentCharge.ID;
                    followupCharge.UpdatedBy = Email;
                    followupCharge.UpdatedDate = DateTime.Now;
                    followupCharge.ReasonID = reasonID;
                    _context.PatientFollowUpCharge.Update(followupCharge);
                }
            }

            FollowUpData data = new FollowUpData();
            data.FollowUp = FollowUpList;
            data.FollowUpCharge = FollowUpChargeList;
            return data;
        }

        private T CreateWithValues<T>(EntityEntry values)
    where T : new()
        {
            T entity = new T();
            Type type = typeof(T);

            foreach (var name in values.CurrentValues.Properties)
            {
                var property = type.GetProperty(name.Name);
                Debug.WriteLine(property.PropertyType);
                string[] ExactType = property.PropertyType.ToString().Split('.');
                if (ExactType[ExactType.Length - 1].Equals("Int64"))
                    property.SetValue(entity, values.CurrentValues.GetValue<long>(name.Name));
                else if (ExactType[ExactType.Length - 1].Equals("Int32"))
                    property.SetValue(entity, values.CurrentValues.GetValue<int>(name.Name));
                else if (ExactType[ExactType.Length - 1].Equals("Patient"))
                    property.SetValue(entity, values.CurrentValues.GetValue<Patient>(name.Name));
                else if (ExactType[ExactType.Length - 1].Equals("Reason"))
                    property.SetValue(entity, values.CurrentValues.GetValue<Reason>(name.Name));
                else if (ExactType[ExactType.Length - 1].Equals("Action"))
                    property.SetValue(entity, values.CurrentValues.GetValue<MediFusionPM.Models.Action>(name.Name));
                else if (ExactType[ExactType.Length - 1].Equals("Group"))
                    property.SetValue(entity, values.CurrentValues.GetValue<Group>(name.Name));
                else if (ExactType[ExactType.Length - 1].Equals("string"))
                    property.SetValue(entity, values.CurrentValues.GetValue<string>(name.Name));
                else if (ExactType[ExactType.Length - 1].Equals("DateTime"))
                    property.SetValue(entity, values.CurrentValues.GetValue<DateTime>(name.Name));
            }

            return entity;
        }

        [Route("DeletePaymentCheck/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeletePaymentCheck(long? id)
        {
            var transactionOption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (var objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption, TransactionScopeAsyncFlowOption.Enabled))
            {

                var PaymentCheck = _context.PaymentCheck.Where(p => p.ID == id).FirstOrDefault();
                if (PaymentCheck != null)
                {
                    List<PaymentVisit> PaymentVisit = _context.PaymentVisit.Where(p => p.PaymentCheckID == id).ToList<PaymentVisit>();
                    foreach (PaymentVisit pv in PaymentVisit)
                    {
                        // set paymentvisit field ID in variable id for further use
                        id = pv.ID;

                        var pfUp = _context.PlanFollowUp.Where(p => p.PaymentVisitID == pv.ID).ToList();
                        foreach (PlanFollowup pf in pfUp)
                        {
                            var pfCharge = _context.PlanFollowupCharge.Where(p => p.PlanFollowupID == pf.ID).ToList();

                            foreach (PlanFollowupCharge pfc in pfCharge)
                            {
                                _context.PlanFollowupCharge.Remove(pfc);
                            }
                            _context.PlanFollowUp.Remove(pf);
                        }


                        var PaymentCharge = _context.PaymentCharge.Where(p => p.PaymentVisitID == id).ToList();

                        foreach (PaymentCharge pc in PaymentCharge)
                        {

                            _context.PaymentCharge.Remove(pc);
                        }

                        _context.PaymentVisit.Remove(pv);

                        _context.PaymentCheck.Remove(PaymentCheck);

                    }

                }
                await _context.SaveChangesAsync();
                objTrnScope.Complete();
            }
            return Ok();
        }


        //[Route("DeletePaymentCheck/{id}")]
        //[HttpDelete]
        //public async Task<ActionResult> DeletePaymentCheck(long? id)
        //{
        //    var transactionOption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
        //    using (var objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption, TransactionScopeAsyncFlowOption.Enabled))
        //    {

        //        var PaymentCheck = _context.PaymentCheck.Where(p => p.ID == id).FirstOrDefault();
        //        if (PaymentCheck != null)
        //        {
        //            List<PaymentVisit> PaymentVisit = _context.PaymentVisit.Where(p => p.PaymentCheckID == id).ToList<PaymentVisit>();
        //            foreach (PaymentVisit pv in PaymentVisit)
        //            {
        //                id = pv.ID;
        //                var PlanFollowup = _context.PlanFollowUp.Where(pf => pf.PaymentVisitID == id).ToList();
        //                if (PlanFollowup.Count > 0)  //Neet to change with count
        //                {

        //                    foreach (PlanFollowup pf in PlanFollowup)
        //                    {

        //                        var PlanFollowUpCharge = _context.PlanFollowupCharge.Where(pfc => pfc.PlanFollowupID == pf.ID).ToList();
        //                        if (PlanFollowUpCharge.Count > 0)
        //                        {
        //                            foreach (PlanFollowupCharge pfc in PlanFollowUpCharge)
        //                            {
        //                                _context.PlanFollowupCharge.Remove(pfc);
        //                            }
        //                        }


        //                        _context.PlanFollowUp.Remove(pf);
        //                    }
        //                }

        //            }
        //            if (PaymentVisit.Count > 0)
        //            {



        //                var PaymentCharge = _context.PaymentCharge.Where(p => p.PaymentVisitID == id).ToList();

        //                foreach (PaymentCharge pc in PaymentCharge)
        //                {
        //                    var PaymentLedger = _context.PaymentLedger.Where(plg => plg.PaymentChargeID == pc.ID).ToList();
        //                    if (PaymentLedger.Count > 0)
        //                    {
        //                        foreach (PaymentLedger pm in PaymentLedger)
        //                        {
        //                            _context.PaymentLedger.Remove(pm);
        //                        }
        //                    }

        //                    _context.PaymentCharge.Remove(pc);
        //                }
        //                foreach (PaymentVisit pv in PaymentVisit)
        //                {
        //                    _context.PaymentVisit.Remove(pv);
        //                }
        //                _context.PaymentCheck.Remove(PaymentCheck);
        //            }
        //        }
        //        await _context.SaveChangesAsync();
        //        objTrnScope.Complete();
        //    }
        //    return Ok();
        //}

        [Route("ImportEraFile")]
        [HttpPost()]
        public async Task<IActionResult> ImportEraFile(FileUploadViewModel File)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value);

            Settings settings = await _context.Settings.Where(s => s.ClientID == UD.ClientID).FirstOrDefaultAsync();
            if (settings == null)
            {
                return BadRequest("Document Server Settings Not Found");
            }

            string DownloadDirectory = Path.Combine("\\\\", settings.DocumentServerURL,
                       settings.DocumentServerDirectory,
                       "ManualImport",
                       DateTime.Now.ToString("MMddyyyy"), DateTime.Now.ToString("hhmmssff"));

            string AllDownloads = Path.Combine(DownloadDirectory, "DOWNLOADS");
            Directory.CreateDirectory(AllDownloads);

            string fileType = string.Empty, FilePath = "";
            if (File.Type.Replace(".", "") == "zip" || File.Type.Replace(".", "") == "rar")
            {
                return BadRequest("Zip File is not supported. Please contaxt Bell MedEx.");
            }
            else
            {
                byte[] data = Convert.FromBase64String(File.Content.Substring(File.Content.IndexOf("base64,") + 7));
                string decodedString = Encoding.UTF8.GetString(data);

                fileType = Utilities.GetFileType(decodedString);
                string dir = Path.Combine(AllDownloads, fileType);
                Directory.CreateDirectory(dir);
                FilePath = Path.Combine(dir, File.Name + fileType);
                await System.IO.File.WriteAllTextAsync(FilePath, decodedString);
            }

            string zipFilePath = Path.Combine(DownloadDirectory, "alldownloads.zip");
            ZipFile.CreateFromDirectory(AllDownloads, zipFilePath);

            //ReportsLog log = new ReportsLog()
            //{
            //    AddedBy = UD.Email,
            //    AddedDate = DateTime.Now,
            //    ClientID = UD.ClientID,
            //    ReceiverID = null,
            //    UserResolved = false,
            //    ManualImport = true,
            //    SubmitterID = null,
            //    ZipFilePath = zipFilePath
            //};
            //_context.ReportsLog.Add(log);
            //await _context.SaveChangesAsync();

            //DownloadedFile downloadedFile = new DownloadedFile()
            //{
            //    AddedBy = UD.Email,
            //    AddedDate = DateTime.Now,
            //    FileType = fileType,
            //    FilePath = FilePath,
            //    Processed = false,
            //    ReportsLogID = log.ID
            //};
            //_context.DownloadedFile.Add(downloadedFile);
            //_context.SaveChanges();

            //ImportERA(zipFilePath, FilePath, UD.Email, UD.ClientID);

            string Email = UD.Email;
            long ClientID = UD.ClientID;

            int duplicateCheckCount = 0;
            List<PaymentCheck> importedChecks = new List<PaymentCheck>();

            long? ReceiverID = null;
            var transactionOption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (var objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption, TransactionScopeAsyncFlowOption.Enabled))
            {
                ReportsLog log = new ReportsLog()
                {
                    AddedBy = Email,
                    AddedDate = DateTime.Now,
                    ClientID = ClientID,
                    ReceiverID = null,
                    UserResolved = false,
                    ManualImport = true,
                    SubmitterID = null,
                    ZipFilePath = zipFilePath
                };
                _context.ReportsLog.Add(log);
                //await _context.SaveChangesAsync();

                DownloadedFile downloadedFile = new DownloadedFile()
                {
                    AddedBy = Email,
                    AddedDate = DateTime.Now,
                    FileType = fileType,
                    FilePath = FilePath,
                    Processed = false,
                    ReportsLogID = log.ID
                };
                _context.DownloadedFile.Add(downloadedFile);
                //_context.SaveChanges();

                ERAParser eraParser = new ERAParser();
                List<ERAHeader> EraData = eraParser.ParseERAFile(FilePath);

                long? defaultActionID = _context.Action.Where(a => a.Name == "EOB").FirstOrDefault()?.ID;
                long? defaultGroupID = _context.Group.Where(a => a.Name == "EOB").FirstOrDefault()?.ID;
                long? defaultReasonID = _context.Reason.Where(a => a.Name == "EOB").FirstOrDefault()?.ID;



                foreach (ERAHeader H in EraData)
                {
                    bool CheckExists = _context.PaymentCheck.Count(p => p.CheckNumber == H.CheckNumber && p.PracticeID == UD.PracticeID) > 0;
                    if (CheckExists)
                    {
                        duplicateCheckCount += 1;
                        continue;
                    }

                    Practice practice = _context.Practice.Where(f => f.NPI == H.PayeeNPI).FirstOrDefault();
                    if (practice == null)
                        practice = _context.Practice.Where(f => f.TaxID == H.PayeeTaxID).FirstOrDefault();

                    long? PracticeID = practice?.ID;

                    if (PracticeID == null)
                    {
                        foreach (ERAVisitPayment V in H.ERAVisitPayments)
                        {
                            string[] temp = null;
                            if (V.PatientControlNumber.Contains(" "))
                                temp = V.PatientControlNumber.Split(' ');
                            else
                                temp = V.PatientControlNumber.Split('_');
                            if (temp != null && temp.Length >= 2)
                            {
                                PracticeID = _context.Visit.Where(v => v.ID == long.Parse(temp[0])).FirstOrDefault()?.PracticeID;
                                if (PracticeID > 0) break;
                            }
                        }
                    }

                    string comments = "";
                    if (PracticeID.IsNull()) comments = "Practice Not found";

                    PaymentCheck paymentCheck = new PaymentCheck()
                    {
                        AddedBy = Email,
                        AddedDate = DateTime.Now,
                        CheckAmount = H.CheckAmount,
                        CheckDate = H.CheckDate,
                        CheckNumber = H.CheckNumber,
                        CreditDebitFlag = H.CreditDebitFlag,
                        PracticeID = PracticeID,
                        PayeeName = H.PayeeName,
                        PayeeTaxID = H.PayeeTaxID,
                        PayeeNPI = H.PayeeNPI,
                        PayeeAddress = H.PayeeAddress,
                        PayeeCity = H.PayeeCity,
                        PayeeState = H.PayeeState,
                        PayeeZipCode = H.PayeeZip,
                        PayerName = H.PayerName,
                        PayerID = H.PayerID,
                        PayerAddress = H.PayerAddress,
                        PayerCity = H.PayerCity,
                        PayerState = H.PayerState,
                        PayerZipCode = H.PayerZip,
                        PayerContactNumber = H.PayerTelephone,
                        PayerContactPerson = H.PayerContactName,
                        REF_2U_ID = H.REF2U,
                        TransactionCode = H.TransactionCode,
                        PaymentMethod = H.PaymentMethod,
                        ReceiverID = ReceiverID,
                        DownloadedFileID = downloadedFile.ID,
                        NumberOfVisits = H.ERAVisitPayments.Count,
                        NumberOfPatients = H.ERAVisitPayments.Select(o => o.SubscriberLastName + o.SubscirberFirstName).Distinct().Count(),
                        Status = PracticeID.IsNull() ? "F" : "NP",
                        Comments = comments
                    };
                    _context.PaymentCheck.Add(paymentCheck);

                    foreach (ERAVisitPayment V in H.ERAVisitPayments)
                    {
                        Visit visit = null;
                        Patient patient = null;
                        string[] temp = null;
                        if (V.PatientControlNumber.Contains(" "))
                            temp = V.PatientControlNumber.Split(' ');
                        else
                            temp = V.PatientControlNumber.Split('_');
                        if (temp != null && temp.Length >= 2)
                        {
                            visit = _context.Visit.Where(v => v.ID == long.Parse(temp[0]) && v.PracticeID == PracticeID).FirstOrDefault();
                            patient = _context.Patient.Where(v => v.AccountNum == temp[1]).FirstOrDefault();
                        }

                        long? VisitID = visit?.ID;
                        long? PatientID = patient?.ID;

                        PaymentVisit payVisit = new PaymentVisit()
                        {
                            AddedBy = Email,
                            AddedDate = DateTime.Now,
                            PaymentCheckID = paymentCheck.ID,
                            BilledAmount = V.SubmittedAmt,
                            PaidAmount = V.PaidAmt,
                            PatientAmount = V.PatResponsibilityAmt,
                            ClaimNumber = V.PatientControlNumber,
                            ClaimStatementFromDate = V.ClaimStatementFrom,
                            ClaimStatementToDate = V.ClaimStatementTo,
                            ForwardedPayerID = V.CrossOverPayerID,
                            ForwardedPayerName = V.CrossOverPayerName,
                            InsuredFirstName = V.SubscirberFirstName,
                            InsuredLastName = V.SubscriberLastName,
                            InsuredID = V.SubscriberID,
                            PatientFIrstName = V.PatientFirstName,
                            PatientLastName = V.PatientLastName,
                            ProvFirstName = V.RendPrvFirstName,
                            ProvLastName = V.RendPrvLastName,
                            ProvNPI = V.RendPrvNPI,
                            PayerICN = V.PayerControlNumber,
                            PayerReceivedDate = V.ClaimReceivedDate,
                            ProcessedAs = V.ClaimProcessedAs,
                            PatientID = PatientID,
                            VisitID = VisitID,
                            Status = "N"
                        };

                        decimal visitAllowedAmt = 0, mVisitAllowedAmt = 0;
                        decimal visitWriteOffAmt = 0;
                        decimal visitPatResp = 0;

                        foreach (ERAChargePayment C in V.ERAChargePayments)
                        {
                            // Finding ChargeID
                            Charge charge = null;
                            if (!C.ChargeControlNumber.IsNull2() && C.ChargeControlNumber.All(char.IsDigit)
                                && visit != null)
                            {
                                charge = _context.Charge.Where(c => c.PatientID == PatientID && c.ID == long.Parse(C.ChargeControlNumber) && c.VisitID == visit.ID).FirstOrDefault();
                            }

                            if (charge == null && visit != null)
                            {
                                charge = _context.Charge.Where(c => c.PatientID == PatientID && c.VisitID == visit.ID && c.Cpt.CPTCode == C.CPTCode && c.DateOfServiceFrom.Date == C.ServiceDateFrom.Date() && c.TotalAmount == C.SubmittedAmt).FirstOrDefault();
                            }
                            else if (charge == null)
                            {
                                if (C.ServiceDateFrom.Date() == null)
                                {
                                    charge = _context.Charge.Where(c => c.PatientID == PatientID && c.Cpt.CPTCode == C.CPTCode && c.DateOfServiceFrom.Date == V.ClaimStatementFrom.Date() && c.TotalAmount == C.SubmittedAmt).FirstOrDefault();
                                }
                                else
                                {
                                    charge = _context.Charge.Where(c => c.PatientID == PatientID && c.Cpt.CPTCode == C.CPTCode && c.DateOfServiceFrom.Date == C.ServiceDateFrom.Date() && c.TotalAmount == C.SubmittedAmt).FirstOrDefault();
                                }
                            }
                            long? ChargeID = charge?.ID;

                            C.ChargeID = ChargeID;

                            if (visit == null && charge != null)
                            {
                                visit = _context.Visit.Find(charge.VisitID);
                                VisitID = visit?.ID;
                            }
                            //

                            visitAllowedAmt += C.AllowedAmount.Val();
                            mVisitAllowedAmt += C.PaidAmt.Val() + C.CopayAmt.Val() + C.DeductableAmt.Val() + C.CoInsuranceAmt.Val();

                            visitWriteOffAmt += C.WriteOffAmt.Val();
                            visitPatResp += C.CopayAmt.Val() + C.CoInsuranceAmt.Val() + C.DeductableAmt.Val();

                            ERARemitCode E = null;
                            if (C.RemitCodes != null && C.RemitCodes.Count > 0)
                            {
                                if (C.RemitCodes.Count > 0)
                                {
                                    E = C.RemitCodes[0];
                                    if (E.ReasonCode != "45" || E.ReasonCode != "253")
                                    {
                                    }
                                    else
                                    {
                                        AdjustmentCode adjustmentCode = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault();
                                        if (adjustmentCode.Type == "W")
                                            visitWriteOffAmt += E.Amount.Val();
                                    }
                                }
                                if (C.RemitCodes.Count > 1)
                                {
                                    E = C.RemitCodes[1];
                                    if (E.ReasonCode != "45" || E.ReasonCode != "253")
                                    {
                                    }
                                    else
                                    {
                                        AdjustmentCode adjustmentCode = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault();
                                        if (adjustmentCode.Type == "W")
                                            visitWriteOffAmt += E.Amount.Val();
                                    }
                                }
                                if (C.RemitCodes.Count > 2)
                                {
                                    E = C.RemitCodes[2];
                                    if (E.ReasonCode != "45" || E.ReasonCode != "253")
                                    {
                                    }
                                    else
                                    {
                                        AdjustmentCode adjustmentCode = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault();
                                        if (adjustmentCode.Type == "W")
                                            visitWriteOffAmt += E.Amount.Val();
                                    }
                                }
                                if (C.RemitCodes.Count > 3)
                                {
                                    E = C.RemitCodes[3];
                                    if (E.ReasonCode != "45" || E.ReasonCode != "253")
                                    {
                                    }
                                    else
                                    {
                                        AdjustmentCode adjustmentCode = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault();
                                        if (adjustmentCode.Type == "W")
                                            visitWriteOffAmt += E.Amount.Val();
                                    }
                                }
                                if (C.RemitCodes.Count > 4)
                                {
                                    E = C.RemitCodes[4];
                                    if (E.ReasonCode != "45" || E.ReasonCode != "253")
                                    {
                                    }
                                    else
                                    {
                                        AdjustmentCode adjustmentCode = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault();
                                        if (adjustmentCode.Type == "W")
                                            visitWriteOffAmt += E.Amount.Val();
                                    }
                                }
                            }
                        }

                        if (visitAllowedAmt != mVisitAllowedAmt)
                            visitAllowedAmt = mVisitAllowedAmt;

                        payVisit.AllowedAmount = visitAllowedAmt.Val();
                        payVisit.WriteOffAmount = visitWriteOffAmt.Val();
                        payVisit.PatientAmount = visitPatResp.Val();
                        payVisit.VisitID = VisitID;

                        _context.PaymentVisit.Add(payVisit);

                        long? FirstAdjustmentID = null;
                        ICollection<PaymentCharge> paymentCharges = new List<PaymentCharge>();

                        foreach (ERAChargePayment C in V.ERAChargePayments)
                        {
                            // This code has been moved upwords

                            //Charge charge = null;
                            //if (!C.ChargeControlNumber.IsNull2() && C.ChargeControlNumber.All(char.IsDigit)
                            //    && visit != null)
                            //{
                            //    charge = _context.Charge.Where(c => c.PatientID == PatientID &&  c.ID == long.Parse(C.ChargeControlNumber) && c.VisitID == visit.ID).FirstOrDefault();
                            //}

                            //if (charge == null && visit != null)
                            //{
                            //    charge = _context.Charge.Where(c => c.PatientID == PatientID && c.VisitID == visit.ID && c.Cpt.CPTCode == C.CPTCode && c.DateOfServiceFrom.Date == C.ServiceDateFrom.Date() && c.TotalAmount == C.SubmittedAmt).FirstOrDefault();
                            //}
                            //else if(charge == null)
                            //{
                            //    charge = _context.Charge.Where(c => c.PatientID == PatientID && c.Cpt.CPTCode == C.CPTCode && c.DateOfServiceFrom.Date == C.ServiceDateFrom.Date() && c.TotalAmount == C.SubmittedAmt).FirstOrDefault();
                            //}
                            //long? ChargeID = charge?.ID;

                            decimal? mAllowedAmount = C.PaidAmt.Val() + C.CopayAmt.Val() + C.DeductableAmt.Val() + C.CoInsuranceAmt.Val();
                            if (mAllowedAmount != C.AllowedAmount.Val())
                                C.AllowedAmount = mAllowedAmount;

                            long? ChargeID = C.ChargeID;
                            
                            PaymentCharge paymentCharge = new PaymentCharge()
                            {
                                AddedBy = Email,
                                AddedDate = DateTime.Now,
                                PaymentVisitID = payVisit.ID,
                                ChargeID = ChargeID,
                                AllowedAmount = C.AllowedAmount.IsNull() ? C.PaidAmt.Val() + C.CopayAmt.Val() + C.DeductableAmt.Val() + C.CoInsuranceAmt.Val() : C.AllowedAmount,
                                BilledAmount = C.SubmittedAmt,
                                ChargeControlNumber = C.ChargeControlNumber,
                                Copay = C.CopayAmt,
                                CoinsuranceAmount = C.CoInsuranceAmt,
                                DeductableAmount = C.DeductableAmt,
                                CPTCode = C.CPTCode,
                                DOSFrom = C.ServiceDateFrom,
                                DOSTo = C.ServiceDateTo,
                                Modifier1 = C.Modifier1,
                                Modifier2 = C.Modifier2,
                                Modifier3 = C.Modifier3,
                                Modifier4 = C.Modifier4,
                                PaidAmount = C.PaidAmt,
                                PatientAmount = C.CopayAmt.Val() + C.DeductableAmt.Val() + C.CoInsuranceAmt.Val(),
                                RevenueCode = C.RevenueCode,
                                RemarkCodeID1 = C.RemarkCode1.IsNull2() ? null : _context.RemarkCode.Where(r => r.Code == C.RemarkCode1).FirstOrDefault()?.ID,
                                RemarkCodeID2 = C.RemarkCode2.IsNull2() ? null : _context.RemarkCode.Where(r => r.Code == C.RemarkCode2).FirstOrDefault()?.ID,
                                RemarkCodeID3 = C.RemarkCode3.IsNull2() ? null : _context.RemarkCode.Where(r => r.Code == C.RemarkCode3).FirstOrDefault()?.ID,
                                RemarkCodeID4 = C.RemarkCode4.IsNull2() ? null : _context.RemarkCode.Where(r => r.Code == C.RemarkCode4).FirstOrDefault()?.ID,
                                RemarkCodeID5 = C.RemarkCode5.IsNull2() ? null : _context.RemarkCode.Where(r => r.Code == C.RemarkCode5).FirstOrDefault()?.ID,
                                Units = C.UnitsPaid,
                                WriteoffAmount = C.WriteOffAmt,
                                Status = "N",
                                AppliedToSec = true
                            };


                            ERARemitCode E = null;
                            if (C.RemitCodes != null && C.RemitCodes.Count > 0)
                            {
                                if (C.RemitCodes.Count > 0)
                                {
                                    E = C.RemitCodes[0];
                                    AdjustmentCode adjustmentCode = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault();
                                    paymentCharge.AdjustmentCodeID1 = adjustmentCode?.ID;
                                    paymentCharge.AdjustmentAmount1 = E.Amount;
                                    paymentCharge.GroupCode1 = E.GroupCode;
                                    paymentCharge.AdjustmentQuantity1 = E.Quantity;
                                    FirstAdjustmentID = paymentCharge.AdjustmentCodeID1;
                                    if (adjustmentCode.Type == "W")
                                    {
                                        if (E.ReasonCode != "45" || E.ReasonCode != "253")
                                        {
                                        }
                                        else
                                        {
                                            paymentCharge.WriteoffAmount = paymentCharge.WriteoffAmount.Val() + E.Amount.Val();
                                        }
                                    }
                                }
                                if (C.RemitCodes.Count > 1)
                                {
                                    E = C.RemitCodes[1];
                                    AdjustmentCode adjustmentCode = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault();
                                    paymentCharge.AdjustmentCodeID2 = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault()?.ID;
                                    paymentCharge.AdjustmentAmount2 = E.Amount;
                                    paymentCharge.GroupCode2 = E.GroupCode;
                                    paymentCharge.AdjustmentQuantity2 = E.Quantity;
                                    if (adjustmentCode.Type == "W")
                                    {
                                        if (E.ReasonCode != "45" || E.ReasonCode != "253")
                                        {
                                        }
                                        else
                                        {
                                            paymentCharge.WriteoffAmount = paymentCharge.WriteoffAmount.Val() + E.Amount.Val();
                                        }
                                    }
                                }
                                if (C.RemitCodes.Count > 2)
                                {
                                    E = C.RemitCodes[2];
                                    AdjustmentCode adjustmentCode = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault();
                                    paymentCharge.AdjustmentCodeID3 = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault()?.ID;
                                    paymentCharge.AdjustmentAmount3 = E.Amount;
                                    paymentCharge.GroupCode3 = E.GroupCode;
                                    paymentCharge.AdjustmentQuantity3 = E.Quantity;
                                    if (adjustmentCode.Type == "W")
                                    {
                                        if (E.ReasonCode != "45" || E.ReasonCode != "253")
                                        {
                                        }
                                        else
                                        {
                                            paymentCharge.WriteoffAmount = paymentCharge.WriteoffAmount.Val() + E.Amount.Val();
                                        }
                                    }
                                }
                                if (C.RemitCodes.Count > 3)
                                {
                                    E = C.RemitCodes[3];
                                    AdjustmentCode adjustmentCode = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault();
                                    paymentCharge.AdjustmentCodeID4 = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault()?.ID;
                                    paymentCharge.AdjustmentAmount4 = E.Amount;
                                    paymentCharge.GroupCode4 = E.GroupCode;
                                    paymentCharge.AdjustmentQuantity4 = E.Quantity;
                                    if (adjustmentCode.Type == "W")
                                    {
                                        if (E.ReasonCode != "45" || E.ReasonCode != "253")
                                        {
                                        }
                                        else
                                        {
                                            paymentCharge.WriteoffAmount = paymentCharge.WriteoffAmount.Val() + E.Amount.Val();
                                        }
                                    }
                                }
                                if (C.RemitCodes.Count > 4)
                                {
                                    E = C.RemitCodes[5];
                                    AdjustmentCode adjustmentCode = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault();
                                    paymentCharge.AdjustmentCodeID5 = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault()?.ID;
                                    paymentCharge.AdjustmentAmount5 = E.Amount;
                                    paymentCharge.GroupCode5 = E.GroupCode;
                                    paymentCharge.AdjustmentQuantity5 = E.Quantity;
                                    if (adjustmentCode.Type == "W")
                                    {
                                        if (E.ReasonCode != "45" || E.ReasonCode != "253")
                                        {
                                        }
                                        else
                                        {
                                            paymentCharge.WriteoffAmount = paymentCharge.WriteoffAmount.Val() + E.Amount.Val();
                                        }
                                    }
                                }
                            }

                            _context.PaymentCharge.Add(paymentCharge);
                            paymentCharges.Add(paymentCharge);
                        }



                        CreateFollowup(payVisit, defaultActionID, defaultReasonID, defaultGroupID, Email, paymentCharges);
                    }

                    importedChecks.Add(paymentCheck);
                }

                await _context.SaveChangesAsync();
                objTrnScope.Complete();

                if (duplicateCheckCount == EraData.Count)
                    return BadRequest("Check(s) already exists");
            }

            foreach (PaymentCheck PC in importedChecks)
            {
                int total = _context.PaymentVisit.Where(v => v.PaymentCheckID == PC.ID).Count();
                int notLinked = _context.PaymentVisit.Where(v => v.PaymentCheckID == PC.ID && v.VisitID == null).Count();
                if (total == notLinked && PC.Status != "F")
                {
                    PC.Status = "F";
                    _context.PaymentCheck.Update(PC);
                    await _context.SaveChangesAsync();
                }
            }

            return Ok();
        }

        //public async void ImportERA(string ZipFile, string FilePath, string Email, long ClientID)
        //{

        //    long? ReceiverID = null;
        //    var transactionOption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
        //    using (var objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption, TransactionScopeAsyncFlowOption.Enabled))
        //    {
        //        ReportsLog log = new ReportsLog()
        //        {
        //            AddedBy = Email,
        //            AddedDate = DateTime.Now,
        //            ClientID = ClientID,
        //            ReceiverID = null,
        //            UserResolved = false,
        //            ManualImport = true,
        //            SubmitterID = null,
        //            ZipFilePath = ZipFile
        //        };
        //        _context.ReportsLog.Add(log);
        //        //await _context.SaveChangesAsync();

        //        DownloadedFile downloadedFile = new DownloadedFile()
        //        {
        //            AddedBy = Email,
        //            AddedDate = DateTime.Now,
        //            FileType = "835",
        //            FilePath = FilePath,
        //            Processed = false,
        //            ReportsLogID = log.ID
        //        };
        //        _context.DownloadedFile.Add(downloadedFile);
        //        //_context.SaveChanges();

        //        ERAParser eraParser = new ERAParser();
        //        List<ERAHeader> EraData = eraParser.ParseERAFile(FilePath);

        //        long? defaultActionID = _context.Action.Where(a => a.Name == "EOB").FirstOrDefault()?.ID;
        //        long? defaultGroupID = _context.Group.Where(a => a.Name == "EOB").FirstOrDefault()?.ID;
        //        long? defaultReasonID = _context.Reason.Where(a => a.Name == "EOB").FirstOrDefault()?.ID;

        //        foreach (ERAHeader H in EraData)
        //        {
        //            Practice practice = _context.Practice.Where(f => f.NPI == H.PayeeNPI).FirstOrDefault();
        //            if (practice == null)
        //                practice = _context.Practice.Where(f => f.TaxID == H.PayeeTaxID).FirstOrDefault();

        //            long? PracticeID = practice?.ID;

        //            if (PracticeID == null)
        //            {
        //                foreach (ERAVisitPayment V in H.ERAVisitPayments)
        //                {
        //                    string[] temp = V.PatientControlNumber.Split('_');
        //                    if (temp != null && temp.Length >= 2)
        //                    {
        //                        PracticeID = _context.Visit.Where(v => v.ID == long.Parse(temp[0])).FirstOrDefault()?.PracticeID;
        //                        if (PracticeID > 0) break;
        //                    }
        //                }
        //            }

        //            PaymentCheck paymentCheck = new PaymentCheck()
        //            {
        //                AddedBy = Email,
        //                AddedDate = DateTime.Now,
        //                CheckAmount = H.CheckAmount,
        //                CheckDate = H.CheckDate,
        //                CheckNumber = H.CheckNumber,
        //                CreditDebitFlag = H.CreditDebitFlag,
        //                PracticeID = PracticeID,
        //                PayeeName = H.PayeeName,
        //                PayeeTaxID = H.PayeeTaxID,
        //                PayeeNPI = H.PayeeNPI,
        //                PayeeAddress = H.PayeeAddress,
        //                PayeeCity = H.PayeeCity,
        //                PayeeState = H.PayeeState,
        //                PayeeZipCode = H.PayeeZip,
        //                PayerName = H.PayerName,
        //                PayerID = H.PayerID,
        //                PayerAddress = H.PayerAddress,
        //                PayerCity = H.PayerCity,
        //                PayerState = H.PayerState,
        //                PayerZipCode = H.PayerZip,
        //                PayerContactNumber = H.PayerTelephone,
        //                PayerContactPerson = H.PayerContactName,
        //                REF_2U_ID = H.REF2U,
        //                TransactionCode = H.TransactionCode,
        //                PaymentMethod = H.PaymentMethod,
        //                ReceiverID = ReceiverID,
        //                DownloadedFileID = downloadedFile.ID
        //            };
        //            _context.PaymentCheck.Add(paymentCheck);

        //            foreach (ERAVisitPayment V in H.ERAVisitPayments)
        //            {
        //                Visit visit = null;
        //                Patient patient = null;
        //                string[] temp = V.PatientControlNumber.Split('_');
        //                if (temp != null && temp.Length >= 2)
        //                {
        //                    visit = _context.Visit.Where(v => v.ID == long.Parse(temp[0])).FirstOrDefault();
        //                    patient = _context.Patient.Where(v => v.ID == long.Parse(temp[1])).FirstOrDefault();
        //                }

        //                long? VisitID = visit?.ID;
        //                long? PatientID = patient?.ID;

        //                PaymentVisit payVisit = new PaymentVisit()
        //                {
        //                    AddedBy = Email,
        //                    AddedDate = DateTime.Now,
        //                    PaymentCheckID = paymentCheck.ID,
        //                    BilledAmount = V.SubmittedAmt,
        //                    PaidAmount = V.PaidAmt,
        //                    PatientAmount = V.PatResponsibilityAmt,
        //                    ClaimNumber = V.PatientControlNumber,
        //                    ClaimStatementFromDate = V.ClaimStatementFrom,
        //                    ClaimStatementToDate = V.ClaimStatementTo,
        //                    ForwardedPayerID = V.CrossOverPayerID,
        //                    ForwardedPayerName = V.CrossOverPayerName,
        //                    InsuredFirstName = V.SubscirberFirstName,
        //                    InsuredLastName = V.SubscriberLastName,
        //                    InsuredID = V.SubscriberID,
        //                    PatientFIrstName = V.PatientFirstName,
        //                    PatientLastName = V.PatientLastName,
        //                    ProvFirstName = V.RendPrvFirstName,
        //                    ProvLastName = V.RendPrvLastName,
        //                    ProvNPI = V.RendPrvNPI,
        //                    PayerICN = V.PayerControlNumber,
        //                    PayerReceivedDate = V.ClaimReceivedDate,
        //                    ProcessedAs = V.ClaimProcessedAs,
        //                    PatientID = PatientID,
        //                    VisitID = VisitID
        //                };

        //                decimal? visitAllowedAmt = null;
        //                decimal? visitWriteOffAmt = null;
        //                decimal? visitPatResp = null;

        //                foreach (ERAChargePayment C in V.ERAChargePayments)
        //                {
        //                    visitAllowedAmt += C.AllowedAmount.Val();
        //                    visitWriteOffAmt += C.WriteOffAmt.Val();
        //                    visitPatResp += C.CopayAmt.Val() + C.CoInsuranceAmt.Val() + C.DeductableAmt.Val();
        //                }

        //                payVisit.AllowedAmount = visitAllowedAmt;
        //                payVisit.WriteOffAmount = visitWriteOffAmt;
        //                payVisit.PatientAmount = visitPatResp;

        //                _context.PaymentVisit.Add(payVisit);

        //                long? FirstAdjustmentID = null;
        //                ICollection<PaymentCharge> paymentCharges = new List<PaymentCharge>();

        //                foreach (ERAChargePayment C in V.ERAChargePayments)
        //                {
        //                    Charge charge = null;
        //                    if (!C.ChargeControlNumber.IsNull2() && C.ChargeControlNumber.All(char.IsDigit))
        //                    {
        //                        charge = _context.Charge.Where(c => c.ID == long.Parse(C.ChargeControlNumber)).FirstOrDefault();
        //                    }

        //                    long? ChargeID = charge?.ID;

        //                    PaymentCharge paymentCharge = new PaymentCharge()
        //                    {
        //                        AddedBy = Email,
        //                        AddedDate = DateTime.Now,
        //                        PaymentVisitID = payVisit.ID,
        //                        ChargeID = ChargeID,
        //                        AllowedAmount = C.AllowedAmount.IsNull() ? C.PaidAmt.Val() + C.CopayAmt.Val() + C.DeductableAmt.Val() + C.CoInsuranceAmt.Val() : C.AllowedAmount,
        //                        BilledAmount = C.SubmittedAmt,
        //                        ChargeControlNumber = C.ChargeControlNumber,
        //                        Copay = C.CopayAmt,
        //                        CoinsuranceAmount = C.CoInsuranceAmt,
        //                        DeductableAmount = C.DeductableAmt,
        //                        CPTCode = C.CPTCode,
        //                        DOSFrom = C.ServiceDateFrom,
        //                        DOSTo = C.ServiceDateTo,
        //                        Modifier1 = C.Modifier1,
        //                        Modifier2 = C.Modifier2,
        //                        Modifier3 = C.Modifier3,
        //                        Modifier4 = C.Modifier4,
        //                        PaidAmount = C.PaidAmt,
        //                        PatientAmount = null,
        //                        RevenueCode = C.RevenueCode,
        //                        RemarkCodeID1 = C.RemarkCode1.IsNull2() ? null : _context.RemarkCode.Where(r => r.Code == C.RemarkCode1).FirstOrDefault()?.ID,
        //                        RemarkCodeID2 = C.RemarkCode2.IsNull2() ? null : _context.RemarkCode.Where(r => r.Code == C.RemarkCode2).FirstOrDefault()?.ID,
        //                        RemarkCodeID3 = C.RemarkCode3.IsNull2() ? null : _context.RemarkCode.Where(r => r.Code == C.RemarkCode3).FirstOrDefault()?.ID,
        //                        RemarkCodeID4 = C.RemarkCode4.IsNull2() ? null : _context.RemarkCode.Where(r => r.Code == C.RemarkCode4).FirstOrDefault()?.ID,
        //                        RemarkCodeID5 = C.RemarkCode5.IsNull2() ? null : _context.RemarkCode.Where(r => r.Code == C.RemarkCode5).FirstOrDefault()?.ID,
        //                        Units = C.UnitsPaid,
        //                        WriteoffAmount = C.WriteOffAmt
        //                    };


        //                    ERARemitCode E = null;
        //                    if (C.RemitCodes != null && C.RemitCodes.Count > 0)
        //                    {
        //                        if (C.RemitCodes.Count > 0)
        //                        {
        //                            E = C.RemitCodes[0];
        //                            paymentCharge.AdjustmentCodeID1 = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault()?.ID;
        //                            paymentCharge.AdjustmentAmount1 = E.Amount;
        //                            paymentCharge.GroupCode1 = E.GroupCode;
        //                            paymentCharge.AdjustmentQuantity1 = E.Quantity;
        //                            FirstAdjustmentID = paymentCharge.AdjustmentCodeID1;
        //                        }
        //                        if (C.RemitCodes.Count > 1)
        //                        {
        //                            E = C.RemitCodes[1];
        //                            paymentCharge.AdjustmentCodeID2 = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault()?.ID;
        //                            paymentCharge.AdjustmentAmount2 = E.Amount;
        //                            paymentCharge.GroupCode2 = E.GroupCode;
        //                            paymentCharge.AdjustmentQuantity2 = E.Quantity;
        //                        }
        //                        if (C.RemitCodes.Count > 2)
        //                        {
        //                            E = C.RemitCodes[2];
        //                            paymentCharge.AdjustmentCodeID3 = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault()?.ID;
        //                            paymentCharge.AdjustmentAmount3 = E.Amount;
        //                            paymentCharge.GroupCode3 = E.GroupCode;
        //                            paymentCharge.AdjustmentQuantity3 = E.Quantity;
        //                        }
        //                        if (C.RemitCodes.Count > 3)
        //                        {
        //                            E = C.RemitCodes[3];
        //                            paymentCharge.AdjustmentCodeID4 = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault()?.ID;
        //                            paymentCharge.AdjustmentAmount4 = E.Amount;
        //                            paymentCharge.GroupCode4 = E.GroupCode;
        //                            paymentCharge.AdjustmentQuantity4 = E.Quantity;
        //                        }
        //                        if (C.RemitCodes.Count > 4)
        //                        {
        //                            E = C.RemitCodes[5];
        //                            paymentCharge.AdjustmentCodeID5 = _context.AdjustmentCode.Where(c => c.Code == E.ReasonCode).FirstOrDefault()?.ID;
        //                            paymentCharge.AdjustmentAmount5 = E.Amount;
        //                            paymentCharge.GroupCode5 = E.GroupCode;
        //                            paymentCharge.AdjustmentQuantity5 = E.Quantity;
        //                        }
        //                    }

        //                    _context.PaymentCharge.Add(paymentCharge);
        //                    paymentCharges.Add(paymentCharge);

        //                    //visitAllowedAmt += C.AllowedAmount.Val();
        //                    //visitWriteOffAmt += C.WriteOffAmt.Val();
        //                    //visitPatResp += C.CopayAmt.Val() + C.CoInsuranceAmt.Val() + C.DeductableAmt.Val();
        //                }

        //                //payVisit.AllowedAmount = visitAllowedAmt;
        //                //payVisit.WriteOffAmount = visitWriteOffAmt;
        //                //payVisit.PatientAmount = visitPatResp;
        //                //_context.PaymentVisit.Update(payVisit);

        //                CreateFollowup(payVisit, defaultActionID, defaultReasonID, defaultGroupID, Email, paymentCharges);
        //            }
        //        }

        //        await _context.SaveChangesAsync();
        //        objTrnScope.Complete();
        //    }
        //}

        [Route("PostEra/{id}")]
        [HttpGet()]
        public async Task<ActionResult<PaymentCheck>> PostEra(long id)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value);

            PaymentCheck check = await _context.PaymentCheck.Where(p => p.ID == id && p.Status != "P").FirstOrDefaultAsync();
            if (check == null)
                return BadRequest("Check is Already Posted.");

            List<PaymentVisit> visitPayments = await _context.PaymentVisit.Where(v => v.PaymentCheckID == check.ID && v.Status != "P").ToListAsync();
            if (visitPayments == null || visitPayments.Count == 0)
            {
                visitPayments = await _context.PaymentVisit.Where(v => v.PaymentCheckID == check.ID).ToListAsync();
                check.Status = "P";
                check.PostedDate = visitPayments[0].PostedDate;
                check.PostedBy = UD.Email;
                _context.PaymentCheck.Update(check);
                _context.SaveChanges();
                return BadRequest("Invalid Check - No Visit Payments Found");
            }

            decimal postedAmount = 0;

            long? defaultActionID = _context.Action.Where(a => a.Name == "SYSTEM").FirstOrDefault()?.ID;
            long? defaultGroupID = _context.Group.Where(a => a.Name == "NEW").FirstOrDefault()?.ID;
            long? defaultReasonID = _context.Reason.Where(a => a.Name == "SYSTEM").FirstOrDefault()?.ID;


            var transactionOption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (var objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption, TransactionScopeAsyncFlowOption.Enabled))
            {
                List<PatientFollowUp> AddedPatientFollowUp = new List<PatientFollowUp>();

                List<ChargeWithID> AddedPatientFollowUpChargeToBeAdded = new List<ChargeWithID>();
                FollowUpData data = new FollowUpData();
                PatientPlan TempSecondaryPatientPlan = null;
                foreach (var vp in visitPayments)
                {
                    if (vp.VisitID.IsNull()) continue;

                    Visit visit = await _context.Visit.FindAsync(vp.VisitID);

                    if (vp.PatientID.IsNull())
                    {
                        if (!vp.VisitID.IsNull())
                        {
                            vp.PatientID = visit.PatientID;
                            _context.PaymentVisit.Update(vp);
                        }
                    }

                    List<PaymentCharge> chargePayments = await _context.PaymentCharge.OrderBy(c => c.ChargeID).Where(c => c.PaymentVisitID == vp.ID && c.Status != "P").ToListAsync();
                    if (chargePayments == null || chargePayments.Count == 0)
                        return BadRequest("Invalid Check - No Charge Payments Found");

                    long? SecondaryPatientPlanID = null;
                    List<PatientPlan> plans = _context.PatientPlan.
                                    Where(p => p.PatientID == vp.PatientID && p.Coverage == "S" && p.IsActive == true).ToList();

                    if (plans != null && plans.Count > 0)
                        SecondaryPatientPlanID = plans[0]?.ID;

                    if (vp.ProcessedAs == "19" && SecondaryPatientPlanID.IsNull())
                    {
                        Patient Patient = _context.Patient.Where(p => p.ID == vp.PatientID).FirstOrDefault();

                        TempSecondaryPatientPlan = new PatientPlan();
                        TempSecondaryPatientPlan.FirstName = Patient.FirstName;
                        TempSecondaryPatientPlan.LastName = Patient.LastName;
                        TempSecondaryPatientPlan.PatientID = Patient.ID;
                        TempSecondaryPatientPlan.DOB = Patient.DOB;
                        TempSecondaryPatientPlan.Gender = Patient.Gender;
                        TempSecondaryPatientPlan.Email = Patient.Email;
                        TempSecondaryPatientPlan.Address1 = Patient.Address1;
                        TempSecondaryPatientPlan.City = Patient.City;
                        TempSecondaryPatientPlan.State = Patient.State;
                        TempSecondaryPatientPlan.ZipCode = Patient.ZipCode;
                        TempSecondaryPatientPlan.PhoneNumber = Patient.PhoneNumber;
                        TempSecondaryPatientPlan.Coverage = "S";
                        TempSecondaryPatientPlan.IsActive = true;
                        TempSecondaryPatientPlan.IsDeleted = false;
                        TempSecondaryPatientPlan.AddedDate = DateTime.Today.Date;
                        TempSecondaryPatientPlan.AddedBy = UD == null ? "" : UD.Email.ToUpper();
                        TempSecondaryPatientPlan.RelationShip = 18 + "";

                        _context.PatientPlan.Add(TempSecondaryPatientPlan);


                        Notes note = new Notes();
                        note.PracticeID = UD.PracticeID;
                        note.Note = "SECONDARY PLAN WAS ADDED BY AUTO PROCESS WHILE POSTING CROSS OVER CHECK.";
                        note.AddedBy = UD.Email;
                        note.AddedDate = DateTime.Now;
                        note.NotesDate = DateTime.Now;
                        note.PatientID = Patient.ID;
                        _context.Notes.Add(note);
                        //_context.SaveChanges();
                        SecondaryPatientPlanID = TempSecondaryPatientPlan.ID;
                        //vp.Comments = "Medigap - Secondary Plan is required for Posting";
                        //_context.PaymentVisit.Update(vp);
                        //continue;
                    }
                    else if (!SecondaryPatientPlanID.IsNull())
                    {
                        TempSecondaryPatientPlan = plans[0];
                    }

                    // Getting Tertiary 
                    long? TertiaryPatientPlanID = null;
                    if (vp.ProcessedAs == "2" || vp.ProcessedAs == "20")
                    {
                        List<PatientPlan> terPlans = _context.PatientPlan.
                                    Where(p => p.PatientID == vp.PatientID && p.Coverage == "T" && p.IsActive == true).ToList();

                        if (terPlans != null && terPlans.Count > 0)
                            TertiaryPatientPlanID = plans[0]?.ID;
                    }
                    List<PatientFollowUpCharge> AddedPatientFollowUpCharge = new List<PatientFollowUpCharge>();

                    
                    decimal visitCopay = 0;
                    decimal? extraCopayPaid = 0, extraCopayRemaining = 0;

                    // Getting Extra Copay Paid
                    decimal? totalCopayReceived = chargePayments.Sum(c => c.Copay.Val());
                    if (visit.CopayPaid.Val() > totalCopayReceived)
                    {
                        extraCopayPaid = visit.CopayPaid.Val() - totalCopayReceived.Val();
                        extraCopayRemaining = extraCopayPaid;
                    }
                    Charge extraCopayCharge = null;

                    bool isAnyChargeDenied = chargePayments.Count(c => c.AllowedAmount.Val() == 0) > 0 ? true : false;
                    //

                    foreach (var ch in chargePayments)
                    {
                        Charge charge = null;
                        if (ch.ChargeID.IsNull())
                        {
                            charge = _context.Charge.Where(c => c.VisitID == vp.VisitID && c.PatientID == vp.PatientID && c.Cpt.CPTCode == ch.CPTCode && c.DateOfServiceFrom.Date == ch.DOSFrom.Date() && c.TotalAmount == ch.BilledAmount).FirstOrDefault();
                            ch.ChargeID = charge?.ID;
                        }
                        else
                            charge = _context.Charge.Find(ch.ChargeID);

                        if (charge == null) continue;

                        // 1 - Primary
                        // 4 - Denied
                        // 19 - Primary - Forwarded to another
                        if (vp.ProcessedAs == "1" || vp.ProcessedAs.IsNull2() || vp.ProcessedAs == "4"
                            || vp.ProcessedAs == "19")
                        {

                            // PatResp = Copay + Deductible + Co-Insurance
                            decimal? patResp = ch.Copay.Val() + ch.DeductableAmount.Val() + ch.CoinsuranceAmount.Val();
                            decimal? WriteOff = ch.WriteoffAmount;
                            decimal? allowed = !ch.AllowedAmount.IsNull() ? ch.AllowedAmount : ch.PaidAmount.Val() + patResp.Val();

                            PatientPlan patientPlan = _context.PatientPlan.Find(charge.PrimaryPatientPlanID);
                            if (patientPlan.InsurancePlanID != 1)
                            {
                                if (charge.PrimaryPaid.Val() == ch.PaidAmount && charge.PrimaryAllowed == allowed && charge.PrimaryWriteOff == WriteOff)
                                {
                                    ch.UpdatedDate = DateTime.Now;
                                    ch.Comments = "Similar payment is already posted";
                                    _context.PaymentCharge.Update(ch);
                                    continue;
                                }
                            }
                            // Allowed = Paid + Pat Resp
                            charge.PrimaryAllowed = allowed;
                            charge.PrimaryWriteOff = charge.PrimaryWriteOff.Val() + WriteOff;
                            charge.PrimaryPaid = charge.PrimaryPaid.Val() + ch.PaidAmount;

                            // Bal = Paid - Write Off (pat resp for now, that will later be transferred to secondary\pat and will be deducted from balance)
                            charge.PrimaryBal = charge.PrimaryBal.Val() - ch?.PaidAmount.Val() - ch?.WriteoffAmount.Val();


                            // Applying CopayPaid if Copay is available in EOB
                            if (ch.Copay.Val() > 0)
                            {
                                // calculating visit copay received.
                                visitCopay = visitCopay + ch.Copay.Val();

                                // if Copay is paid, then deducting from primary bal
                                if (visit.CopayPaid.Val() > 0)
                                {
                                    charge.PrimaryBal = charge.PrimaryBal.Val() - visit.CopayPaid.Val();
                                    charge.PatientPaid = charge.PatientPaid.Val() + visit.CopayPaid.Val();
                                    patResp = patResp - visit.CopayPaid.Val();

                                }
                            }

                            // Setting AppliedToSec to false, if Secondary plan not exits in system.
                            if (ch.AppliedToSec && TempSecondaryPatientPlan == null)
                            {
                                ch.AppliedToSec = false;
                            }
                            //


                            //decimal? otherPR = ch.OtherPatResp.Val();
                            //if (otherPR.Val() > 0)
                            //{
                            //    if(ch.AppliedToSec == false)
                            //        patResp = patResp.Val() + ch.OtherPatResp.Val();
                            //}

                            patResp = patResp.Val() + ch.OtherPatResp.Val();


                            // Applying Extra Copay Paid Amount When Copay in EOB is 0
                            if (patResp > 0 && (totalCopayReceived == 0 || totalCopayReceived < visit.CopayPaid.Val()) && extraCopayPaid > 0 && extraCopayRemaining > 0)
                            {
                                if (isAnyChargeDenied == true && extraCopayPaid > 0 && totalCopayReceived == 0)
                                {
                                }
                                else if (patResp >= extraCopayRemaining)
                                {
                                    patResp = patResp - extraCopayRemaining;
                                    charge.PrimaryBal = charge.PrimaryBal.Val() - extraCopayRemaining;
                                    charge.PatientPaid = charge.PatientPaid.Val() + extraCopayRemaining;
                                    //charge.PrimaryTransferred = patResp;

                                    if (extraCopayCharge != null)
                                    {
                                        extraCopayCharge.PrimaryPatientBal = extraCopayCharge.PrimaryPatientBal.Val() - (-1 * extraCopayRemaining);
                                        extraCopayCharge.PatientPaid = extraCopayCharge.PatientPaid.Val() - extraCopayRemaining;
                                    }
                                    extraCopayRemaining = 0;
                                }
                                else
                                {
                                    decimal? tempPTResp = patResp;
                                    //patResp = patResp - extraCopayRemaining;
                                    extraCopayRemaining = extraCopayRemaining - tempPTResp;
                                    charge.PrimaryBal = charge.PrimaryBal.Val() - tempPTResp;
                                    patResp = patResp - tempPTResp;

                                    charge.PatientPaid = charge.PatientPaid.Val() + tempPTResp;
                                    //charge.PrimaryTransferred = tempPTResp;

                                    if (extraCopayCharge != null)
                                    {
                                        extraCopayCharge.PrimaryPatientBal = extraCopayCharge.PrimaryPatientBal.Val() - (-1 * tempPTResp);
                                        extraCopayCharge.PatientPaid = extraCopayCharge.PatientPaid.Val() - tempPTResp;
                                    }
                                }

                                if (extraCopayCharge != null)
                                    _context.Charge.Update(extraCopayCharge);
                            }
                            //

                            charge.Copay = ch.Copay.Val();
                            charge.Deductible = ch.DeductableAmount;
                            charge.Coinsurance = ch.CoinsuranceAmount;
                            charge.OtherPatResp = ch.OtherPatResp;



                            if (patResp < 0)
                            {
                                //patResp = patResp.Val() + ch.OtherPatResp.Val();

                                decimal? amtToTransfer = patResp;
                                charge.PrimaryPatientBal = charge.PrimaryPatientBal.Val() + amtToTransfer.Val();
                                //charge.PrimaryTransferred = charge.PrimaryTransferred.Val() + amtToTransfer.Val();

                                if (-1 * patResp - charge.PrimaryBal.Val() > 0)
                                {
                                    charge.PrimaryBal = 0;
                                    extraCopayCharge = charge;
                                }
                                else
                                {
                                    charge.PrimaryBal = charge.PrimaryBal.Val() - amtToTransfer.Val();
                                }
                                charge.PrimaryStatus = "P";
                            }
                            else if (patResp > 0)
                            {
                                if (ch.AppliedToSec)
                                {
                                    charge.SecondaryPatientPlanID = TempSecondaryPatientPlan.ID;
                                    decimal? amtToTransfer = patResp;
                                    charge.SecondaryBilledAmount = amtToTransfer;
                                    charge.SecondaryBal = amtToTransfer;
                                    charge.SecondaryStatus = "N";

                                    charge.PrimaryTransferred = amtToTransfer;
                                    charge.PrimaryBal = charge.PrimaryBal.Val() - amtToTransfer.Val();
                                    charge.PrimaryStatus = "PPTS";

                                    if (vp.ProcessedAs == "19")
                                    {
                                        charge.PrimaryStatus = "PPTM";
                                        charge.SecondaryStatus = "M";   // Madigaped
                                    }
                                }
                                else
                                {
                                    //patResp = patResp.Val() + ch.OtherPatResp.Val();

                                    decimal? amtToTransfer = patResp;
                                    charge.PrimaryPatientBal = charge.PrimaryPatientBal.Val() + amtToTransfer.Val();
                                    charge.PrimaryTransferred = charge.PrimaryTransferred.Val() + amtToTransfer.Val();
                                    charge.PrimaryBal = charge.PrimaryBal.Val() - amtToTransfer.Val();
                                    charge.PrimaryStatus = "PPTP";

                                    data = CreatePatientFollowup(charge.PatientID, defaultActionID, defaultReasonID, defaultGroupID, UD.Email, ch, AddedPatientFollowUp, AddedPatientFollowUpCharge);
                                    AddedPatientFollowUp = data.FollowUp;
                                    AddedPatientFollowUpCharge = data.FollowUpCharge;
                                    foreach (PatientFollowUpCharge FUPCharge in AddedPatientFollowUpCharge)
                                    {
                                        ChargeWithID chargeWithID = new ChargeWithID();
                                        chargeWithID.PatientFollowUpCharge = FUPCharge;
                                        chargeWithID.PatientID = vp.PatientID.Value;
                                        if (!AddedPatientFollowUpChargeToBeAdded.Contains(chargeWithID))
                                            AddedPatientFollowUpChargeToBeAdded.Add(chargeWithID);

                                    }
                                }
                            }
                            else if (charge.PrimaryPaid.Val() > 0)
                                charge.PrimaryStatus = "P";
                            else if (charge.PrimaryWriteOff.Val() == charge.PrimaryBilledAmount.Val())
                                charge.PrimaryStatus = "W";
                            else if (vp.PatientAmount != null && vp.PatientAmount.Val() > 0)
                                charge.PrimaryStatus = "P";
                            else if (charge.PrimaryPaid.IsNull())
                                charge.PrimaryStatus = "DN";

                            PaymentLedger ledger = null;

                            if (charge.PrimaryAllowed > 0)
                            {
                                #region PAID AMOUNT LEDGER
                                if (ch.PaidAmount > 0)
                                {
                                    postedAmount += charge.PrimaryPaid.Val();

                                    ledger = new PaymentLedger()
                                    {
                                        AddedBy = UD.Email,
                                        AddedDate = DateTime.Now,
                                        AdjustmentCodeID = null,
                                        ChargeID = charge.ID,
                                        LedgerBy = "INSURANCE",
                                        LedgerDate = DateTime.Now,
                                        LedgerType = "PRIMARY PAYMENT",
                                        LedgerDescription = "",
                                        PaymentChargeID = ch.ID,
                                        PatientPaymentChargeID = null,
                                        PatientPlanID = charge.PrimaryPatientPlanID.Value,
                                        VisitID = charge.VisitID.Value,
                                        Amount = ch.PaidAmount
                                    };
                                    _context.PaymentLedger.Add(ledger);
                                }
                                #endregion

                                #region Patient Copay
                                if (ch.Copay > 0)
                                {
                                    ledger = AddLedger(UD.Email, charge, ch.ID, "INSURANCE", "COPAY", "", ch.Copay, null, charge.PrimaryPatientPlanID);
                                    _context.PaymentLedger.Add(ledger);
                                }
                                #endregion

                                #region Patient Deductible
                                if (ch.DeductableAmount > 0)
                                {
                                    ledger = AddLedger(UD.Email, charge, ch.ID, "INSURANCE", "DEDUCTIBLE", "", ch.DeductableAmount, null, charge.PrimaryPatientPlanID);
                                    _context.PaymentLedger.Add(ledger);
                                }
                                #endregion

                                #region Patient CoInsurance
                                if (ch.CoinsuranceAmount > 0)
                                {
                                    ledger = AddLedger(UD.Email, charge, ch.ID, "INSURANCE", "CO-INSURANCE", "", ch.CoinsuranceAmount, null, charge.PrimaryPatientPlanID);
                                    _context.PaymentLedger.Add(ledger);
                                }
                                #endregion
                            }

                            #region Adjustment Ledgers

                            if (!ch.AdjustmentCodeID1.IsNull() && ch.WriteoffAmount > 0)
                            {
                                AdjustmentCode adjustmentCode = _context.AdjustmentCode.Find(ch.AdjustmentCodeID1);
                                ledger = AddLedger(UD.Email, charge, ch.ID, "INSURANCE", adjustmentCode.Description, "",
                                    ch.AdjustmentAmount1, ch.AdjustmentCodeID1, charge.PrimaryPatientPlanID);

                                _context.PaymentLedger.Add(ledger);
                            }

                            if (!ch.AdjustmentCodeID2.IsNull() && ch.WriteoffAmount > 0)
                            {
                                AdjustmentCode adjustmentCode = _context.AdjustmentCode.Find(ch.AdjustmentCodeID2);
                                ledger = AddLedger(UD.Email, charge, ch.ID, "INSURANCE", adjustmentCode.Description, "",
                                    ch.AdjustmentAmount2, ch.AdjustmentCodeID2, charge.PrimaryPatientPlanID);

                                _context.PaymentLedger.Add(ledger);
                            }

                            if (!ch.AdjustmentCodeID3.IsNull() && ch.WriteoffAmount > 0)
                            {
                                AdjustmentCode adjustmentCode = _context.AdjustmentCode.Find(ch.AdjustmentCodeID3);
                                ledger = AddLedger(UD.Email, charge, ch.ID, "INSURANCE", adjustmentCode.Description, "",
                                    ch.AdjustmentAmount3, ch.AdjustmentCodeID3, charge.PrimaryPatientPlanID);

                                _context.PaymentLedger.Add(ledger);
                            }

                            #endregion

                            charge.PrimaryPaymentDate = check.CheckDate;
                            _context.Charge.Update(charge);
                        }
                        // 2 - Secondary
                        // 20 - Secondary - Forwarded to Additional Payer
                        else if (vp.ProcessedAs == "2" || vp.ProcessedAs == "20")
                        {

                            decimal? patResp = ch.Copay + ch.DeductableAmount.Val() + ch.CoinsuranceAmount.Val();
                            decimal? WriteOff = ch.WriteoffAmount;

                            charge.SecondaryAllowed = !ch.AllowedAmount.IsNull() ? ch.AllowedAmount : ch.PaidAmount.Val() + patResp.Val();
                            charge.SecondaryWriteOff = WriteOff;
                            charge.SecondaryPaid = ch.PaidAmount;
                            //charge.SecondaryBal = charge.SecondaryBilledAmount - ch?.PaidAmount.Val() - ch?.WriteoffAmount.Val();
                            charge.SecondaryBal = charge.SecondaryBal.Val() - ch?.PaidAmount.Val() - ch?.WriteoffAmount.Val();


                            charge.Copay = ch.Copay;
                            charge.Deductible = ch.DeductableAmount;
                            charge.Coinsurance = ch.CoinsuranceAmount;
                            charge.OtherPatResp = ch.OtherPatResp;

                            if (ch.AppliedToSec && TertiaryPatientPlanID.IsNull())
                            {
                                ch.AppliedToSec = false;
                                //ch.Comments = "";   
                            }

                            if (patResp > 0)
                            {
                                if (ch.AppliedToSec)
                                {
                                    charge.TertiaryPatientPlanID = TempSecondaryPatientPlan.ID;
                                    decimal? amtToTransfer = patResp;
                                    charge.TertiaryBilledAmount = amtToTransfer;
                                    charge.TertiaryBal = amtToTransfer;
                                    charge.TertiaryStatus = "N";

                                    charge.SecondaryTransferred = amtToTransfer;
                                    charge.SecondaryBal = charge.SecondaryBal.Val() - amtToTransfer.Val();
                                    charge.SecondaryStatus = "PPTT";

                                    if (vp.ProcessedAs == "20")
                                    {
                                        charge.SecondaryStatus = "PPTM";
                                        charge.TertiaryStatus = "M";   // Madigaped
                                    }

                                }
                                else
                                {
                                    patResp = patResp.Val() + ch.OtherPatResp.Val();
                                    decimal? amtToTransfer = patResp;
                                    charge.SecondaryPatientBal = amtToTransfer.Val();
                                    charge.SecondaryTransferred = amtToTransfer.Val();
                                    charge.SecondaryBal = charge.SecondaryBal.Val() - amtToTransfer.Val();
                                    charge.SecondaryStatus = "PPTP";
                                }
                            }
                            else if (charge.SecondaryPaid.Val() > 0)
                                charge.SecondaryStatus = "P";
                            else if (charge.SecondaryWriteOff.Val() == charge.SecondaryBilledAmount.Val())
                                charge.PrimaryStatus = "W";
                            else if (charge.SecondaryPaid.IsNull())
                                charge.SecondaryStatus = "DN";

                            PaymentLedger ledger = null;

                            if (charge.SecondaryAllowed > 0)
                            {
                                #region PAID AMOUNT LEDGER
                                if (ch.PaidAmount > 0)
                                {
                                    postedAmount += charge.SecondaryPaid.Val();
                                    ledger = AddLedger(UD.Email, charge, ch.ID, "INSURANCE", "SECONDARY PAYMENT", "", ch.PaidAmount, null, charge.SecondaryPatientPlanID);
                                    _context.PaymentLedger.Add(ledger);
                                }
                                #endregion

                                #region Patient Copay
                                if (ch.Copay > 0)
                                {
                                    ledger = AddLedger(UD.Email, charge, ch.ID, "INSURANCE", "COPAY", "", ch.Copay, null, charge.SecondaryPatientPlanID);
                                    _context.PaymentLedger.Add(ledger);
                                }
                                #endregion

                                #region Patient Deductible
                                if (ch.DeductableAmount > 0)
                                {
                                    ledger = AddLedger(UD.Email, charge, ch.ID, "INSURANCE", "DEDUCTIBLE", "", ch.DeductableAmount, null, charge.SecondaryPatientPlanID);
                                    _context.PaymentLedger.Add(ledger);
                                }
                                #endregion

                                #region Patient CoInsurance
                                if (ch.CoinsuranceAmount > 0)
                                {
                                    ledger = AddLedger(UD.Email, charge, ch.ID, "INSURANCE", "CO-INSURANCE", "", ch.CoinsuranceAmount, null, charge.SecondaryPatientPlanID);
                                    _context.PaymentLedger.Add(ledger);
                                }
                                #endregion
                            }

                            #region Adjustment Ledgers

                            if (!ch.AdjustmentCodeID1.IsNull() && ch.WriteoffAmount > 0)
                            {
                                AdjustmentCode adjustmentCode = _context.AdjustmentCode.Find(ch.AdjustmentCodeID1);
                                ledger = AddLedger(UD.Email, charge, ch.ID, "INSURANCE", adjustmentCode.Description, "",
                                    ch.AdjustmentAmount1, ch.AdjustmentCodeID1, charge.SecondaryPatientPlanID);

                                _context.PaymentLedger.Add(ledger);
                            }

                            if (!ch.AdjustmentCodeID2.IsNull() && ch.WriteoffAmount > 0)
                            {
                                AdjustmentCode adjustmentCode = _context.AdjustmentCode.Find(ch.AdjustmentCodeID2);
                                ledger = AddLedger(UD.Email, charge, ch.ID, "INSURANCE", adjustmentCode.Description, "",
                                    ch.AdjustmentAmount2, ch.AdjustmentCodeID2, charge.SecondaryPatientPlanID);

                                _context.PaymentLedger.Add(ledger);
                            }

                            if (!ch.AdjustmentCodeID3.IsNull() && ch.WriteoffAmount > 0)
                            {
                                AdjustmentCode adjustmentCode = _context.AdjustmentCode.Find(ch.AdjustmentCodeID3);
                                ledger = AddLedger(UD.Email, charge, ch.ID, "INSURANCE", adjustmentCode.Description, "",
                                    ch.AdjustmentAmount3, ch.AdjustmentCodeID3, charge.SecondaryPatientPlanID);

                                _context.PaymentLedger.Add(ledger);
                            }

                            #endregion

                            charge.SecondaryPaymentDate = check.CheckDate;
                            _context.Charge.Update(charge);
                        }
                        else if (vp.ProcessedAs == "22")
                        {
                            decimal? patResp = ch.Copay.Val() + ch.DeductableAmount.Val() + ch.CoinsuranceAmount.Val();
                            decimal? WriteOff = ch.WriteoffAmount;
                            decimal? allowed = !ch.AllowedAmount.IsNull() ? ch.AllowedAmount : ch.PaidAmount.Val() + patResp.Val();

                            PatientPlan patientPlan = _context.PatientPlan.Find(charge.PrimaryPatientPlanID);
                            if (patientPlan.InsurancePlanID != 1)
                            {
                                if (charge.PrimaryPaid.Val() == ch.PaidAmount && charge.PrimaryAllowed == allowed && charge.PrimaryWriteOff == WriteOff)
                                {
                                    ch.UpdatedDate = DateTime.Now;
                                    ch.Comments = "Similiar payment is already posted";
                                    _context.PaymentCharge.Update(ch);
                                    continue;
                                }
                            }
                            // Allowed = Paid + Pat Resp
                            charge.PrimaryAllowed = allowed;
                            charge.PrimaryWriteOff = charge.PrimaryWriteOff.Val() + WriteOff;
                            charge.PrimaryPaid = charge.PrimaryPaid.Val() + ch.PaidAmount;

                            // Bal = Paid - Write Off (pat resp for now, that will later be transferred to secondary\pat and will be deducted from balance)
                            charge.PrimaryBal = charge.PrimaryBal.Val() - ch?.PaidAmount.Val() - ch?.WriteoffAmount.Val();


                            // Applying CopayPaid if Copay is available in EOB
                            if (ch.Copay.Val() > 0)
                            {
                                // calculating visit copay received.
                                visitCopay = visitCopay + ch.Copay.Val();

                                // if Copay is paid, then deducting from primary bal
                                if (visit.CopayPaid.Val() > 0)
                                {
                                    charge.PrimaryBal = charge.PrimaryBal.Val() - visit.CopayPaid.Val();
                                    charge.PatientPaid = charge.PatientPaid.Val() + visit.CopayPaid.Val();
                                    patResp = patResp - visit.CopayPaid.Val();

                                }
                            }

                            // Setting AppliedToSec to false, if Secondary plan not exits in system.
                            if (ch.AppliedToSec && TempSecondaryPatientPlan == null)
                            {
                                ch.AppliedToSec = false;
                            }
                            //


                            //decimal? otherPR = ch.OtherPatResp.Val();
                            //if (otherPR.Val() > 0)
                            //{
                            //    if(ch.AppliedToSec == false)
                            //        patResp = patResp.Val() + ch.OtherPatResp.Val();
                            //}

                            patResp = patResp.Val() + ch.OtherPatResp.Val();


                            // Applying Extra Copay Paid Amount When Copay in EOB is 0
                            if (patResp > 0 && (totalCopayReceived == 0 || totalCopayReceived < visit.CopayPaid.Val()) && extraCopayPaid > 0 && extraCopayRemaining > 0)
                            {
                                if (isAnyChargeDenied == true && extraCopayPaid > 0 && totalCopayReceived == 0)
                                {
                                }
                                else if (patResp >= extraCopayRemaining)
                                {
                                    patResp = patResp - extraCopayRemaining;
                                    charge.PrimaryBal = charge.PrimaryBal.Val() - extraCopayRemaining;
                                    charge.PatientPaid = charge.PatientPaid.Val() + extraCopayRemaining;
                                    //charge.PrimaryTransferred = patResp;

                                    if (extraCopayCharge != null)
                                    {
                                        extraCopayCharge.PrimaryPatientBal = extraCopayCharge.PrimaryPatientBal.Val() - (-1 * extraCopayRemaining);
                                        extraCopayCharge.PatientPaid = extraCopayCharge.PatientPaid.Val() - extraCopayRemaining;
                                    }
                                    extraCopayRemaining = 0;
                                }
                                else
                                {
                                    decimal? tempPTResp = patResp;
                                    //patResp = patResp - extraCopayRemaining;
                                    extraCopayRemaining = extraCopayRemaining - tempPTResp;
                                    charge.PrimaryBal = charge.PrimaryBal.Val() - tempPTResp;
                                    patResp = patResp - tempPTResp;

                                    charge.PatientPaid = charge.PatientPaid.Val() + tempPTResp;
                                    //charge.PrimaryTransferred = tempPTResp;

                                    if (extraCopayCharge != null)
                                    {
                                        extraCopayCharge.PrimaryPatientBal = extraCopayCharge.PrimaryPatientBal.Val() - (-1 * tempPTResp);
                                        extraCopayCharge.PatientPaid = extraCopayCharge.PatientPaid.Val() - tempPTResp;
                                    }
                                }

                                if (extraCopayCharge != null)
                                    _context.Charge.Update(extraCopayCharge);
                            }
                            //

                            charge.Copay = ch.Copay.Val();
                            charge.Deductible = ch.DeductableAmount;
                            charge.Coinsurance = ch.CoinsuranceAmount;
                            charge.OtherPatResp = ch.OtherPatResp;



                            if (patResp < 0)
                            {
                                //patResp = patResp.Val() + ch.OtherPatResp.Val();

                                decimal? amtToTransfer = patResp;
                                charge.PrimaryPatientBal = charge.PrimaryPatientBal.Val() + amtToTransfer.Val();
                                //charge.PrimaryTransferred = charge.PrimaryTransferred.Val() + amtToTransfer.Val();

                                if (-1 * patResp - charge.PrimaryBal.Val() > 0)
                                {
                                    charge.PrimaryBal = 0;
                                    extraCopayCharge = charge;
                                }
                                else
                                {
                                    charge.PrimaryBal = charge.PrimaryBal.Val() - amtToTransfer.Val();
                                }
                                charge.PrimaryStatus = "P";
                            }
                            else if (patResp > 0)
                            {
                                if (ch.AppliedToSec)
                                {
                                    charge.SecondaryPatientPlanID = TempSecondaryPatientPlan.ID;
                                    decimal? amtToTransfer = patResp;
                                    charge.SecondaryBilledAmount = amtToTransfer;
                                    charge.SecondaryBal = amtToTransfer;
                                    charge.SecondaryStatus = "N";

                                    charge.PrimaryTransferred = amtToTransfer;
                                    charge.PrimaryBal = charge.PrimaryBal.Val() - amtToTransfer.Val();
                                    charge.PrimaryStatus = "PPTS";

                                    if (vp.ProcessedAs == "19")
                                    {
                                        charge.PrimaryStatus = "PPTM";
                                        charge.SecondaryStatus = "M";   // Madigaped
                                    }
                                }
                                else
                                {
                                    //patResp = patResp.Val() + ch.OtherPatResp.Val();

                                    decimal? amtToTransfer = patResp;
                                    charge.PrimaryPatientBal = charge.PrimaryPatientBal.Val() + amtToTransfer.Val();
                                    charge.PrimaryTransferred = charge.PrimaryTransferred.Val() + amtToTransfer.Val();
                                    charge.PrimaryBal = charge.PrimaryBal.Val() - amtToTransfer.Val();
                                    charge.PrimaryStatus = "PPTP";

                                    data = CreatePatientFollowup(charge.PatientID, defaultActionID, defaultReasonID, defaultGroupID, UD.Email, ch, AddedPatientFollowUp, AddedPatientFollowUpCharge);
                                    AddedPatientFollowUp = data.FollowUp;
                                    AddedPatientFollowUpCharge = data.FollowUpCharge;
                                    foreach (PatientFollowUpCharge FUPCharge in AddedPatientFollowUpCharge)
                                    {
                                        ChargeWithID chargeWithID = new ChargeWithID();
                                        chargeWithID.PatientFollowUpCharge = FUPCharge;
                                        chargeWithID.PatientID = vp.PatientID.Value;
                                        if (!AddedPatientFollowUpChargeToBeAdded.Contains(chargeWithID))
                                            AddedPatientFollowUpChargeToBeAdded.Add(chargeWithID);

                                    }
                                }
                            }
                            else if (charge.PrimaryPaid.Val() > 0)
                                charge.PrimaryStatus = "P";
                            else if (charge.PrimaryWriteOff.Val() == charge.PrimaryBilledAmount.Val())
                                charge.PrimaryStatus = "W";
                            else if (vp.PatientAmount != null && vp.PatientAmount.Val() > 0)
                                charge.PrimaryStatus = "P";
                            else if (charge.PrimaryPaid.IsNull())
                                charge.PrimaryStatus = "DN";

                            PaymentLedger ledger = null;

                            if (charge.PrimaryAllowed > 0)
                            {
                                #region PAID AMOUNT LEDGER
                                if (ch.PaidAmount <= 0)
                                {
                                    postedAmount += charge.PrimaryPaid.Val();

                                    ledger = new PaymentLedger()
                                    {
                                        AddedBy = UD.Email,
                                        AddedDate = DateTime.Now,
                                        AdjustmentCodeID = null,
                                        ChargeID = charge.ID,
                                        LedgerBy = "INSURANCE",
                                        LedgerDate = DateTime.Now,
                                        LedgerType = "PRIMARY PAYMENT",
                                        LedgerDescription = "",
                                        PaymentChargeID = ch.ID,
                                        PatientPaymentChargeID = null,
                                        PatientPlanID = charge.PrimaryPatientPlanID.Value,
                                        VisitID = charge.VisitID.Value,
                                        Amount = ch.PaidAmount
                                    };
                                    _context.PaymentLedger.Add(ledger);
                                }
                                #endregion

                                #region Patient Copay
                                if (ch.Copay <= 0)
                                {
                                    ledger = AddLedger(UD.Email, charge, ch.ID, "INSURANCE", "COPAY", "", ch.Copay, null, charge.PrimaryPatientPlanID);
                                    _context.PaymentLedger.Add(ledger);
                                }
                                #endregion

                                #region Patient Deductible
                                if (ch.DeductableAmount <= 0)
                                {
                                    ledger = AddLedger(UD.Email, charge, ch.ID, "INSURANCE", "DEDUCTIBLE", "", ch.DeductableAmount, null, charge.PrimaryPatientPlanID);
                                    _context.PaymentLedger.Add(ledger);
                                }
                                #endregion

                                #region Patient CoInsurance
                                if (ch.CoinsuranceAmount <= 0)
                                {
                                    ledger = AddLedger(UD.Email, charge, ch.ID, "INSURANCE", "CO-INSURANCE", "", ch.CoinsuranceAmount, null, charge.PrimaryPatientPlanID);
                                    _context.PaymentLedger.Add(ledger);
                                }
                                #endregion
                            }

                            #region Adjustment Ledgers

                            if (!ch.AdjustmentCodeID1.IsNull() && ch.WriteoffAmount <= 0)
                            {
                                AdjustmentCode adjustmentCode = _context.AdjustmentCode.Find(ch.AdjustmentCodeID1);
                                ledger = AddLedger(UD.Email, charge, ch.ID, "INSURANCE", adjustmentCode.Description, "",
                                    ch.AdjustmentAmount1, ch.AdjustmentCodeID1, charge.PrimaryPatientPlanID);

                                _context.PaymentLedger.Add(ledger);
                            }

                            if (!ch.AdjustmentCodeID2.IsNull() && ch.WriteoffAmount <= 0)
                            {
                                AdjustmentCode adjustmentCode = _context.AdjustmentCode.Find(ch.AdjustmentCodeID2);
                                ledger = AddLedger(UD.Email, charge, ch.ID, "INSURANCE", adjustmentCode.Description, "",
                                    ch.AdjustmentAmount2, ch.AdjustmentCodeID2, charge.PrimaryPatientPlanID);

                                _context.PaymentLedger.Add(ledger);
                            }

                            if (!ch.AdjustmentCodeID3.IsNull() && ch.WriteoffAmount <= 0)
                            {
                                AdjustmentCode adjustmentCode = _context.AdjustmentCode.Find(ch.AdjustmentCodeID3);
                                ledger = AddLedger(UD.Email, charge, ch.ID, "INSURANCE", adjustmentCode.Description, "",
                                    ch.AdjustmentAmount3, ch.AdjustmentCodeID3, charge.PrimaryPatientPlanID);

                                _context.PaymentLedger.Add(ledger);
                            }

                            #endregion

                            _context.Charge.Update(charge);
                        }



                        //previousPatientId = currentPatientId;
                        ch.Status = "P";
                        ch.PostedDate = DateTime.Now;
                        ch.PostedBy = UD.Email;
                        _context.PaymentCharge.Update(ch);
                    }

                    // 
                    //List<Charge> chargesWithNegativePatBal = _context.Charge.Where(c => c.VisitID == vp.VisitID && c.PrimaryPatientBal.Val() < 0).ToList();
                    //foreach (Charge cNegBal in chargesWithNegativePatBal)
                    //{
                    //    List<Charge> chargesWithPositivePatBal = _context.Charge.Where(c => c.VisitID == vp.VisitID && c.PrimaryPatientBal.Val() > 0).ToList();

                    //    foreach (Charge cPosBal in chargesWithPositivePatBal)
                    //    {
                    //        if (cPosBal.PrimaryPatientBal.Val() >= -1 * cNegBal.PrimaryPatientBal.Val())
                    //        {
                    //            cPosBal.PrimaryPatientBal = cPosBal.PrimaryPatientBal.Val() - (-1 * cNegBal.PrimaryPatientBal.Val());
                    //            cPosBal.PrimaryTransferred = cPosBal.PrimaryTransferred.Val() - (-1 * cNegBal.PrimaryPatientBal.Val());

                    //            cNegBal.PrimaryPatientBal = 0;
                    //        }
                    //        else
                    //        {
                    //            cPosBal.PrimaryPatientBal = cPosBal.PrimaryPatientBal.Val() - (-1 * cNegBal.PrimaryPatientBal.Val());
                    //            cPosBal.PrimaryTransferred = cPosBal.PrimaryTransferred.Val() - (-1 * cNegBal.PrimaryPatientBal.Val());

                    //            cNegBal.PrimaryPatientBal = cNegBal.PrimaryPatientBal.Val() + cPosBal.PrimaryPatientBal.Val();
                    //        }
                    //        _context.Charge.Update(cPosBal);
                    //    }
                    //    _context.Charge.Update(cNegBal);
                    //}



                    var visitSum = (from c in _context.Charge
                                    where c.VisitID == vp.VisitID
                                    group c by c.VisitID into v
                                    select new
                                    {
                                        PrimaryAllowed = v.Sum(x => x.PrimaryAllowed.Val()),
                                        PrimaryWriteOff = v.Sum(x => x.PrimaryWriteOff.Val()),
                                        PrimaryPaid = v.Sum(x => x.PrimaryPaid.Val()),
                                        PrimaryPatBal = v.Sum(x => x.PrimaryPatientBal.Val()),
                                        PrimaryBal = v.Sum(x => x.PrimaryBal.Val()),
                                        Copay = v.Sum(x => x.Copay.Val()),
                                        Deductible = v.Sum(x => x.Deductible.Val()),
                                        CoInsurance = v.Sum(x => x.Coinsurance.Val()),
                                        OtherPatResp = v.Sum(x => x.OtherPatResp.Val()),
                                        SecondaryBilledAmount = v.Sum(x => x.SecondaryBilledAmount.Val()),
                                        SecondaryBal = v.Sum(x => x.SecondaryBal.Val()),
                                        PrimaryTransferred = v.Sum(x => x.PrimaryTransferred.Val()),
                                        //PrimaryTransferred = v.Sum(x => x.PrimaryPatientBal.Val() < 0 ? (x.PrimaryPatientBal.Val() + x.PrimaryTransferred.Val()) : x.PrimaryTransferred.Val()),
                                        SecondaryTransferred = v.Sum(x => x.SecondaryTransferred.Val()),
                                        SecondaryAllowed = v.Sum(x => x.SecondaryAllowed.Val()),
                                        SecondaryWriteOff = v.Sum(x => x.SecondaryWriteOff.Val()),
                                        SecondaryPaid = v.Sum(x => x.SecondaryPaid.Val()),
                                        TertiaryBilledAmount = v.Sum(x => x.TertiaryBilledAmount.Val()),
                                        TertiaryBal = v.Sum(x => x.TertiaryBal.Val()),
                                        SecondaryPatientBal = v.Sum(x => x.SecondaryPatientBal.Val()),
                                    }).FirstOrDefault();




                    if (vp.ProcessedAs == "1" || vp.ProcessedAs.IsNull2() || vp.ProcessedAs == "19")
                    {
                        visit.PrimaryAllowed = visitSum.PrimaryAllowed.Val();
                        visit.PrimaryWriteOff = visitSum.PrimaryWriteOff.Val();
                        visit.PrimaryPaid = visitSum.PrimaryPaid.Val();
                        visit.PrimaryBal = visitSum.PrimaryBal;
                        visit.Copay = visitSum.Copay.Val() > 0 ? visitSum.Copay.Val() : visit.Copay;
                        visit.Deductible = visitSum.Deductible.Val();
                        visit.Coinsurance = visitSum.CoInsurance.Val();
                        visit.OtherPatResp = visitSum.OtherPatResp.Val();
                        visit.SecondaryBilledAmount = visitSum.SecondaryBilledAmount.Val();
                        visit.SecondaryBal = visitSum.SecondaryBal.Val();
                        visit.PrimaryTransferred = visitSum.PrimaryTransferred.Val();
                        visit.PrimaryPatientBal = visitSum.PrimaryPatBal.Val();

                        if ((extraCopayCharge == null && isAnyChargeDenied != true))
                            visit.PrimaryPatientBal = visit.PrimaryPatientBal.Val() - extraCopayRemaining;

                        visit.PrimaryPaymentDate = check.CheckDate;

                        // If Patient Bal < 0, move the amount to Advance Payment
                        if (visit.PrimaryPatientBal < 0 && isAnyChargeDenied != true)
                        {
                            PatientPayment patientPayment = _context.PatientPayment.Where(pp => pp.Type == "ADVANCE PAYMENT" && pp.PatientID == visit.PatientID).FirstOrDefault();
                            if (patientPayment == null)
                            {
                                patientPayment = new PatientPayment()
                                {
                                    AddedBy = UD.Email,
                                    AddedDate = DateTime.Now,
                                    Type = "ADVANCE PAYMENT",
                                    PaymentMethod = "",
                                    PatientID = visit.PatientID,
                                    VisitID = null,
                                    Description = "ADVANCE PAYMENT",
                                    PaymentDate = DateTime.Now,
                                    PaymentAmount = (-1 * visit.PrimaryPatientBal),
                                    CheckNumber = check.CheckNumber,
                                    RemainingAmount = (-1 * visit.PrimaryPatientBal)
                                };
                                _context.PatientPayment.Add(patientPayment);
                            }
                            else
                            {
                                patientPayment.UpdatedBy = UD.Email;
                                patientPayment.AddedDate = DateTime.Now;
                                patientPayment.PaymentAmount = patientPayment.PaymentAmount + (-1 * visit.PrimaryPatientBal);
                                patientPayment.RemainingAmount = patientPayment.PaymentAmount + (-1 * visit.PrimaryPatientBal);
                                _context.PatientPayment.Update(patientPayment);
                            }

                            visit.MovedToAdvancePayment = (-1 * visit.PrimaryPatientBal);
                        }

                        if (visit.PrimaryPaid.Val() > 0 && visit.SecondaryBilledAmount.Val() > 0)
                        {
                            visit.PrimaryStatus = "PPTS";
                            visit.SecondaryPatientPlanID = TempSecondaryPatientPlan.ID;
                            visit.SecondaryStatus = "N";

                            if (vp.ProcessedAs == "19")
                            {
                                visit.PrimaryStatus = "PPTM";
                                visit.SecondaryStatus = "M";   // Madigaped
                            }
                        }
                        else if (visit.SecondaryBilledAmount.Val() > 0)
                        {
                            visit.PrimaryStatus = "TS";
                            visit.SecondaryPatientPlanID = TempSecondaryPatientPlan.ID;
                            visit.SecondaryStatus = "N";

                            if (vp.ProcessedAs == "19")
                            {
                                visit.PrimaryStatus = "M";
                                visit.SecondaryStatus = "M";   // Madigaped
                            }
                        }
                        else if (visit.PrimaryPaid.Val() > 0 && visit.PrimaryPatientBal.Val() > 0)
                            visit.PrimaryStatus = "PPTP";
                        else if (visit.PrimaryPaid.Val() > 0)
                            visit.PrimaryStatus = "P";
                        else if (visit.PrimaryPatientBal.Val() < 0)
                            visit.PrimaryStatus = "PR_TP";
                        else if (visit.PrimaryPatientBal.Val() > 0)
                            visit.PrimaryStatus = "PR_TP";
                        else if (visit.PrimaryWriteOff.Val() == visit.PrimaryBilledAmount.Val())
                            visit.PrimaryStatus = "W";
                        else if (visit.PrimaryPaid.IsNull())
                            visit.PrimaryStatus = "DN";
                    }
                    else if (vp.ProcessedAs == "2" || vp.ProcessedAs == "20")
                    {
                        visit.SecondaryAllowed = visitSum.SecondaryAllowed.Val();
                        visit.SecondaryWriteOff = visitSum.SecondaryWriteOff.Val();
                        visit.SecondaryPaid = visitSum.SecondaryPaid.Val();
                        visit.SecondaryBal = visitSum.SecondaryBal;
                        visit.Copay = visitSum.Copay.Val();
                        visit.Deductible = visitSum.Deductible.Val();
                        visit.Coinsurance = visitSum.CoInsurance.Val();
                        visit.OtherPatResp = visitSum.OtherPatResp.Val();

                        visit.TertiaryBilledAmount = visitSum.TertiaryBilledAmount.Val();
                        visit.TertiaryBal = visitSum.TertiaryBal.Val();
                        visit.SecondaryTransferred = visitSum.SecondaryTransferred.Val();
                        visit.SecondaryPatientBal = visitSum.SecondaryPatientBal.Val();

                        visit.SecondaryPaymentDate = check.CheckDate;

                        if (visit.SecondaryPaid.Val() > 0 && visit.TertiaryBilledAmount.Val() > 0)
                        {
                            visit.SecondaryStatus = "PPTT";
                            visit.TertiaryPatientPlanID = TertiaryPatientPlanID;
                            visit.TertiaryStatus = "N";

                            if (vp.ProcessedAs == "20")
                            {
                                visit.SecondaryStatus = "PPTM";
                                visit.TertiaryStatus = "M";   // Madigaped
                            }
                        }

                        else if (visit.SecondaryPaid.Val() > 0 && visit.SecondaryPatientBal.Val() > 0)
                            visit.SecondaryStatus = "PPTP";
                        else if (visit.SecondaryPaid.Val() > 0)
                            visit.SecondaryStatus = "P";
                        if (visit.TertiaryBilledAmount.Val() > 0)
                        {
                            visit.SecondaryStatus = "TT";
                            visit.TertiaryPatientPlanID = TertiaryPatientPlanID;
                            visit.TertiaryStatus = "N";
                        }
                        else if (visit.SecondaryPatientBal.Val() > 0)
                            visit.SecondaryStatus = "PR_TP";
                        else if (visit.SecondaryWriteOff.Val() == visit.SecondaryBilledAmount.Val())
                            visit.SecondaryStatus = "W";
                        else if (visit.SecondaryPaid.IsNull())
                            visit.SecondaryStatus = "DN";
                    }
                    else if (vp.ProcessedAs == "22")
                    {
                        visit.PrimaryAllowed = visit.PrimaryAllowed - visitSum.PrimaryAllowed.Val();
                        visit.PrimaryWriteOff = visitSum.PrimaryWriteOff.Val();
                        visit.PrimaryPaid = visitSum.PrimaryPaid.Val();
                        visit.PrimaryBal = visitSum.PrimaryBal;
                        visit.Copay = visitSum.Copay.Val() > 0 ? visitSum.Copay.Val() : visit.Copay;
                        visit.Deductible = visitSum.Deductible.Val();
                        visit.Coinsurance = visitSum.CoInsurance.Val();
                        visit.OtherPatResp = visitSum.OtherPatResp.Val();
                        visit.SecondaryBilledAmount = visitSum.SecondaryBilledAmount.Val();
                        visit.SecondaryBal = visitSum.SecondaryBal.Val();
                        visit.PrimaryTransferred = visitSum.PrimaryTransferred.Val();
                        visit.PrimaryPatientBal = visitSum.PrimaryPatBal.Val();

                        if ((extraCopayCharge == null && isAnyChargeDenied != true))
                            visit.PrimaryPatientBal = visit.PrimaryPatientBal.Val() + extraCopayRemaining;

                        // If Patient Bal < 0, move the amount to Advance Payment
                        if (visit.PrimaryPatientBal < 0 && isAnyChargeDenied != true)
                        {
                            PatientPayment patientPayment = _context.PatientPayment.Where(pp => pp.Type == "ADVANCE PAYMENT" && pp.PatientID == visit.PatientID).FirstOrDefault();
                            if (patientPayment == null)
                            {
                                patientPayment = new PatientPayment()
                                {
                                    AddedBy = UD.Email,
                                    AddedDate = DateTime.Now,
                                    Type = "ADVANCE PAYMENT",
                                    PaymentMethod = "",
                                    PatientID = visit.PatientID,
                                    VisitID = null,
                                    Description = "ADVANCE PAYMENT",
                                    PaymentDate = DateTime.Now,
                                    PaymentAmount = (-1 * visit.PrimaryPatientBal),
                                    //RemainingAmount = 
                                };
                                _context.PatientPayment.Add(patientPayment);
                            }
                            else
                            {
                                patientPayment.UpdatedBy = UD.Email;
                                patientPayment.AddedDate = DateTime.Now;
                                patientPayment.PaymentAmount = patientPayment.PaymentAmount + (-1 * visit.PrimaryPatientBal);
                                _context.PatientPayment.Update(patientPayment);
                            }

                            visit.MovedToAdvancePayment = (-1 * visit.PrimaryPatientBal);
                        }

                        visit.PrimaryStatus = "S";
                        if (visit.SecondaryBal.Val() > 0)
                        {
                            visit.SecondaryStatus = "S";
                        }

                        //if (visit.PrimaryPaid.Val() > 0 && visit.SecondaryBilledAmount.Val() > 0)
                        //{
                        //    visit.PrimaryStatus = "PPTS";
                        //    visit.SecondaryPatientPlanID = SecondaryPatientPlanID;
                        //    visit.SecondaryStatus = "N";

                        //    if (vp.ProcessedAs == "19")
                        //    {
                        //        visit.PrimaryStatus = "PPTM";
                        //        visit.SecondaryStatus = "M";   // Madigaped
                        //    }
                        //}
                        //else if (visit.SecondaryBilledAmount.Val() > 0)
                        //{
                        //    visit.PrimaryStatus = "TS";
                        //    visit.SecondaryPatientPlanID = SecondaryPatientPlanID;
                        //    visit.SecondaryStatus = "N";

                        //    if (vp.ProcessedAs == "19")
                        //    {
                        //        visit.PrimaryStatus = "M";
                        //        visit.SecondaryStatus = "M";   // Madigaped
                        //    }
                        //}
                        //else if (visit.PrimaryPaid.Val() > 0 && visit.PrimaryPatientBal.Val() > 0)
                        //    visit.PrimaryStatus = "PPTP";
                        //else if (visit.PrimaryPaid.Val() > 0)
                        //    visit.PrimaryStatus = "P";
                        //else if (visit.PrimaryPatientBal.Val() < 0)
                        //    visit.PrimaryStatus = "PR_TP";
                        //else if (visit.PrimaryPatientBal.Val() > 0)
                        //    visit.PrimaryStatus = "PR_TP";
                        //else if (visit.PrimaryWriteOff.Val() == visit.PrimaryBilledAmount.Val())
                        //    visit.PrimaryStatus = "W";
                        //else if (visit.PrimaryPaid.IsNull())
                        //    visit.PrimaryStatus = "DN";
                    }


                    _context.Visit.Update(visit);

                    // Patient Follow up already created but new charge received for patient follow up charge
                    if (AddedPatientFollowUp.Count == 0 && AddedPatientFollowUpChargeToBeAdded.Count > 0)
                    {
                        foreach (ChargeWithID fupc in AddedPatientFollowUpChargeToBeAdded)
                        {
                            PatientFollowUp followUp = _context.PatientFollowUp.Where(f => f.PatientID == fupc.PatientID).FirstOrDefault();
                            fupc.PatientFollowUpCharge.PatientFollowUpID = followUp.ID;
                            _context.PatientFollowUpCharge.Add(fupc.PatientFollowUpCharge);
                        }
                    }
                    else
                    {
                        foreach (PatientFollowUp fup in AddedPatientFollowUp)
                        {
                            _context.PatientFollowUp.Add(fup);
                            foreach (ChargeWithID fupc in AddedPatientFollowUpChargeToBeAdded)
                            {
                                if (fup.PatientID == fupc.PatientID)
                                {
                                    fupc.PatientFollowUpCharge.PatientFollowUpID = fup.ID;
                                    _context.PatientFollowUpCharge.Add(fupc.PatientFollowUpCharge);
                                }
                            }
                        }
                    }


                    //int totalCharges = _context.PaymentCharge.Where(c => c.PaymentVisitID == vp.ID).Count();
                    //int totalPostedCharges = _context.PaymentCharge.Where(c => c.PaymentVisitID == vp.ID && c.Status == "P").Count();

                    //if (totalCharges == totalPostedCharges)
                    //{
                    //    vp.Status = "P";
                    //    vp.PostedDate = DateTime.Now;
                    //    vp.PostedBy = UD.Email;
                    //    _context.PaymentVisit.Update(vp);
                    //}
                }

                //int totalPayments = _context.PaymentVisit.Where(v => v.PaymentCheckID == check.ID && v.Status != "P").Count();
                //int postedPayments = _context.PaymentVisit.Where(v => v.PaymentCheckID == check.ID && v.Status == "P").Count();

                //if (totalPayments == postedPayments)
                //    check.Status = "P";

                check.PostedAmount = check.PostedAmount.Val() + postedAmount;
                check.AppliedAmount = _context.PaymentVisit.Where(f => f.PaymentCheckID == check.ID).Sum(v => v.PaidAmount.Val());
                check.UnAppliedAmount = check.CheckAmount.Val() - check.AppliedAmount.Val();
                check.PostedDate = DateTime.Now;
                check.PostedBy = UD.Email;

                _context.PaymentCheck.Update(check);
                await _context.SaveChangesAsync();
                objTrnScope.Complete();
            }



            int total = 0, posted = 0, similiarPayment = 0;
            foreach (PaymentVisit payment in _context.PaymentVisit.Where(f => f.PaymentCheckID == check.ID))
            {
                total = _context.PaymentCharge.Where(v => v.PaymentVisitID == payment.ID).Count();
                posted = _context.PaymentCharge.Where(v => v.PaymentVisitID == payment.ID && v.Status == "P").Count();
                similiarPayment = _context.PaymentCharge.Where(v => v.PaymentVisitID == payment.ID && v.Comments == "Similar payment is already posted").Count();
                if (total == posted + similiarPayment)
                {
                    if (payment.Status != "P")
                    {
                        payment.Status = "P";
                        payment.PostedDate = DateTime.Now;
                        payment.PostedBy = UD.Email;
                        _context.PaymentVisit.Update(payment);
                    }
                }
            }
            await _context.SaveChangesAsync();

            total = _context.PaymentVisit.Where(v => v.PaymentCheckID == check.ID).Count();
            posted = _context.PaymentVisit.Where(v => v.PaymentCheckID == check.ID && v.Status == "P").Count();
            if (total == posted)
            {
                check.Status = "P";
                check.PostedDate = DateTime.Now;
                check.PostedBy = UD.Email;
                _context.PaymentCheck.Update(check);
                await _context.SaveChangesAsync();
            }
            List<ExternalCharge> ReferenceCharge = _context.ExternalCharge.Where(p => p.PaymentCheckID == check.ID && p.PatientPayment.Val() > 0).ToList();
            List<Visit> VisitsWithPayments = new List<Visit>();
            foreach (ExternalCharge TempEC in ReferenceCharge)
            {
                Visit TempVisit = _context.Visit.Where(lamb => lamb.ID == TempEC.VisitID).FirstOrDefault();
                if (TempVisit != null)
                {
                    if (TempVisit.PatientPayments == null) TempVisit.PatientPayments = new List<PatientPayment>();
                    if (TempVisit.Note == null) TempVisit.Note = new List<Notes>();
                    TempVisit.PatientPayments.Add(new PatientPayment()
                    {
                        AddedBy = UD.Email,
                        AddedDate = DateTime.Now,
                        AllocatedAmount = null,
                        CCTransactionID = null,
                        CheckNumber = check.CheckNumber,
                        Description = null,
                        PatientID = TempVisit.PatientID,
                        PaymentAmount = TempEC.PatientPayment,
                        PaymentDate = check.CheckDate,
                        PaymentMethod = "Cash",
                        Status = "O",
                        Type = "O"
                    });
                    if (!VisitsWithPayments.Contains(TempVisit))
                    {
                        VisitsWithPayments.Add(TempVisit);
                    }

                }
            }
            foreach (Visit Temp in VisitsWithPayments)
            {
                if (Temp.SecondaryStatus == "N" && !Temp.SecondaryPatientPlanID.IsNull() && Temp.PrimaryTransferred.Val() > 0)
                {
                    Temp.SecondaryStatus = "S";

                    List<Charge> secondaryCharges = _context.Charge.Where(c => c.VisitID == Temp.ID && c.PrimaryTransferred.Val() > 0).ToList();
                    foreach (var SC in secondaryCharges)
                    {
                        SC.SecondaryStatus = "S";
                        _context.Charge.Update(SC);
                    }
                    _context.SaveChanges();

                    Temp.Note.Add(new Notes()
                    {
                        AddedBy = UD.Email,
                        AddedDate = DateTime.Now,
                        NotesDate = DateTime.Now,
                        VisitID = Temp.ID,
                        Note = "Secondary Claim Marked As Submitted Because Posting is Done Via External Data."
                    });
                }
                VisitController VisitController = new VisitController(_context, _contextMain);
                VisitController.ControllerContext = this.ControllerContext;
                await VisitController.SaveVisit(Temp);
            }




            // Patient DONT PAID

            List<long?> notPaidVisits = _context.ExternalCharge.Where(p => p.PaymentCheckID == check.ID).Select(s => s.VisitID).Distinct().ToList<long?>();
            foreach (long vId in notPaidVisits)
            {
                Visit Temp = _context.Visit.Find(vId);

                if (Temp == null) continue;
                Temp.Note = new List<Notes>();

                if (Temp.SecondaryStatus == "N" && !Temp.SecondaryPatientPlanID.IsNull() && Temp.PrimaryTransferred.Val() > 0)
                {
                    Temp.SecondaryStatus = "S";

                    List<Charge> secondaryCharges = _context.Charge.Where(c => c.VisitID == Temp.ID && c.PrimaryTransferred.Val() > 0).ToList();
                    foreach (var SC in secondaryCharges)
                    {
                        SC.SecondaryStatus = "S";
                        _context.Charge.Update(SC);
                    }


                    _context.Notes.Add(new Notes()
                    {
                        AddedBy = UD.Email,
                        AddedDate = DateTime.Now,
                        NotesDate = DateTime.Now,
                        VisitID = Temp.ID,
                        Note = "Secondary Claim Marked As Submitted Because Posting is Done Via External Data."
                    });
                    _context.Visit.Update(Temp);
                }
            }
            if (notPaidVisits != null && notPaidVisits.Count() > 0)
                _context.SaveChanges();

            return Ok(check);
        }

        public class ChargeWithID
        {
            public PatientFollowUpCharge PatientFollowUpCharge { get; set; }
            public long PatientID { get; set; }
        }


        [Route("PostEra")]
        [HttpPost()]
        public async Task<IActionResult> PostEra(ListModel model)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value);

            foreach (long id in model.Ids)
            {
                try
                {
                    await PostEra(id);
                }
                catch (Exception)
                {
                    if (model.Ids.Count() == 1)
                        return BadRequest("Please contaxt Bell MedEx");
                }
            }
            return Ok();
        }


        [Route("MarkAsNotRelated")]
        [HttpPost()]
        public async Task<IActionResult> MarkAsNotRelated(ListModel listobject)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value);

            //List<PaymentCheck> checks = _context.PaymentCheck.Where(m => listobject.Ids.Equals(m.ID) && m.Status != "NR").ToList();
            //if (checks != null && checks.Count > 0)
            //{
            //    checks.ForEach(x => x.Status = "NR");
            //}
            //_context.PaymentCheck.UpdateRange(checks);
            //await _context.SaveChangesAsync();

            //foreach (long id in listobject.Ids)
            if (listobject == null)
            {
                return BadRequest("RecordNotFound");
            }
            if (listobject != null)
            {
                foreach (var listid in listobject.Ids)
                {
                    var paymentcheck = _context.PaymentCheck.Where(m => (m.Status == "NP" || m.Status == "F") && m.ID == listid).FirstOrDefault();
                    if (paymentcheck != null)
                    {
                        paymentcheck.Status = "NR";
                        _context.PaymentCheck.Update(paymentcheck);
                    }
                }
            }
            await _context.SaveChangesAsync();
            return Ok();
        }







        private PaymentLedger AddLedger(string Email, Charge Charge, long PaymentChargeID,
            string LedgerBy, string LedgerType, string LedgerDesc, decimal? Amount, long? AdjustmentCodeID, long? PatientPlanID)
        {
            PaymentLedger ledger = new PaymentLedger()
            {
                AddedBy = Email,
                AddedDate = DateTime.Now,
                AdjustmentCodeID = AdjustmentCodeID,
                ChargeID = Charge.ID,
                LedgerBy = LedgerBy,
                LedgerDate = DateTime.Now,
                LedgerType = LedgerType,
                LedgerDescription = LedgerDesc,
                PaymentChargeID = PaymentChargeID,
                PatientPaymentChargeID = null,
                PatientPlanID = PatientPlanID,
                VisitID = Charge.VisitID.Value,
                Amount = Amount
            };
            return ledger;
        }
        [Route("FindAudit/{PaymentCheckID}")]
        [HttpGet("{PaymentCheckID}")]
        public List<PaymentCheckAudit> FindAudit(long PaymentCheckID)
        {
            List<PaymentCheckAudit> data = (from pAudit in _context.PaymentCheckAudit
                                            where pAudit.PaymentCheckID == PaymentCheckID
                                            orderby pAudit.AddedDate descending
                                            select new PaymentCheckAudit()
                                            {
                                                ID = pAudit.ID,
                                                PaymentCheckID = pAudit.PaymentCheckID,
                                                TransactionID = pAudit.TransactionID,
                                                ColumnName = pAudit.ColumnName,
                                                CurrentValue = pAudit.CurrentValue,
                                                NewValue = pAudit.NewValue,
                                                CurrentValueID = pAudit.CurrentValueID,
                                                NewValueID = pAudit.NewValueID,
                                                HostName = pAudit.HostName,
                                                AddedBy = pAudit.AddedBy,
                                                AddedDate = pAudit.AddedDate,
                                            }).ToList<PaymentCheckAudit>();
            return data;
        }


        [Route("DownloadERAFile/{paymentcheckID}")]        [HttpGet("{paymentcheckID}")]        public async Task<IActionResult> DownloadERAFile(long paymentcheckID)        {            PaymentCheck pay = await _context.PaymentCheck.FindAsync(paymentcheckID);            if (pay != null)            {                DownloadedFile download = await _context.DownloadedFile.FindAsync(pay.DownloadedFileID);                if (!System.IO.File.Exists(download.FilePath))                {                    return BadRequest("File Not Found");                }
                //Byte[] fileBytes = System.IO.File.ReadAllBytes(download.FilePath);
                Byte[] fileBytes = System.IO.File.ReadAllBytes(download.FilePath);
                //this returns a byte array of the PDF file content
                if (fileBytes == null)                    return BadRequest("File Not Found");                var stream = new MemoryStream(fileBytes); //saves it into a stream
                stream.Position = 0;
                //return File(stream, "application/octec-stream", "DownloadedEra.zip");
                return File(stream, "application/octec-stream", System.IO.Path.GetFileName(download.FilePath));            }            else
            {
                return BadRequest("File Not Found");
            }
        }


    }

 
    public class FollowUpData
    {
        public List<PatientFollowUp> FollowUp { get; set; }
        public List<PatientFollowUpCharge> FollowUpCharge { get; set; }
    }
}






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
using static MediFusionPM.ViewModels.VMCommon;
using static MediFusionPM.ViewModels.VMPatient;
using static MediFusionPM.ViewModels.VMVisit;
using MediFusionPM.Models.Audit;
using System.Diagnostics;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Html2pdf;
using System.IO.Compression;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using MediFusionPM.Uitilities;
using static MediFusionPM.ViewModels.VMClaimStatus;
using Microsoft.AspNetCore.Http;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class VisitController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;
        public VisitController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;
        }



        [HttpGet]
        [Route("GetVisits")]
        public async Task<ActionResult<IEnumerable<Visit>>> GetVisits()
        {
            return await _context.Visit.ToListAsync();

        }


        [Route("GetProfiles")]
        [HttpGet()]
        public async Task<ActionResult<VMVisit>> GetProfiles()
        {
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            ViewModels.VMVisit obj = new ViewModels.VMVisit();
            List<PatientInfoDropDown> patInfo = GetProfilesNew();
            obj.PatientInfo = patInfo;
            //obj.GetProfiles(_context, PracticeId);
            return obj;
        }


        private List<PatientInfoDropDown> GetProfilesNew()
        {
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            string temp = User.Claims.FirstOrDefault(x => x.Type.Equals("TEMP", StringComparison.InvariantCultureIgnoreCase)).Value;
            string connectionString = CommonUtil.GetConnectionString(PracticeId, temp);

            List<PatientInfoDropDown> data = new List<PatientInfoDropDown>();
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                string oString = "select p.ID PatientID, (p.LastName + ', ' + p.FirstName) PatientName, p.AccountNum AccountNumber, convert(varchar,p.dob,101) DOB, p.Gender Gender, p.PracticeID PracticeID, p.LocationId LocationID, l.POSID POSID, p.ProviderID providerID, p.RefProviderID RefProviderID, (p.LastName + ' ' + p.FirstName + ' ' + p.AccountNum) label, (p.LastName + ' ' + p.FirstName + ' ' + p.AccountNum) Value  from Patient p join Location l on p.LocationId = l.ID where p.practiceid = {0}";
                oString = string.Format(oString, PracticeId);

                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                myConnection.Open();

                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        long? practiceId = null;
                        long? providerID = null;
                        long? refProvID = null;
                        long? locationID = null;
                        long? posID = null;

                        if (!oReader["PracticeID"].ToString().IsNull())
                            practiceId = long.Parse(oReader["PracticeID"].ToString());
                        if (!oReader["LocationID"].ToString().IsNull())
                            locationID = long.Parse(oReader["LocationID"].ToString());
                        if (!oReader["POSID"].ToString().IsNull())
                            posID = long.Parse(oReader["POSID"].ToString());
                        if (!oReader["ProviderID"].ToString().IsNull())
                            providerID = long.Parse(oReader["ProviderID"].ToString());
                        if (!oReader["RefProviderID"].ToString().IsNull())
                            refProvID = long.Parse(oReader["RefProviderID"].ToString());

                        data.Add(new PatientInfoDropDown()
                        {
                            PatientID = oReader["PatientID"].ToString(),
                            //PatientName = oReader["PatientName"].ToString(),
                            //AccountNumber = oReader["AccountNumber"].ToString(),
                            DOB = oReader["DOB"].ToString(),
                            Gender = oReader["Gender"].ToString(),
                            PracticeID = practiceId,
                            LocationID = locationID,
                            POSID = posID,
                            providerID = providerID,
                            RefProviderID = refProvID,
                            label = oReader["label"].ToString(),
                            //Value = oReader["Value"].ToString(),
                        });
                    }
                    myConnection.Close();
                }
            }
            return data;
        }


        [Route("ResubmitVisit/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Visit>> ResubmitVisit(long id)
        {
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            Visit visit = _context.Visit.Where(v => v.ID == id && v.IsSubmitted == true).SingleOrDefault();
            if (visit != null)
            {
                if (!visit.TertiaryStatus.IsNull())
                {
                    visit.TertiaryStatus = "RS";
                    visit.IsResubmitted = true;
                }
                else if (!visit.SecondaryStatus.IsNull())
                {
                    visit.SecondaryStatus = "RS";
                    visit.IsResubmitted = true;
                }
                else
                {
                    visit.IsSubmitted = false;
                    visit.PrimaryStatus = "RS";
                    visit.IsResubmitted = true;
                }

                _context.Visit.Update(visit);


                List<Charge> charges = _context.Charge.Where(c => c.VisitID == id && c.IsSubmitted == true).ToList<Charge>();

                foreach (Charge charge in charges)
                {
                    if (!charge.TertiaryStatus.IsNull())
                    {
                        charge.TertiaryStatus = "RS";
                    }
                    else if (!charge.SecondaryStatus.IsNull())
                    {
                        charge.SecondaryStatus = "RS";
                    }
                    else
                    {
                        charge.IsSubmitted = false;
                        charge.PrimaryStatus = "RS";
                    }

                    _context.Charge.Update(charge);

                    ResubmitHistory resHistory = new ResubmitHistory();

                    resHistory.ChargeID = charge.ID;
                    resHistory.VisitID = visit.ID;
                    resHistory.AddedBy = Email;
                    resHistory.AddedDate = DateTime.Now;
                    _context.ResubmitHistory.Add(resHistory);
                }

                _context.SaveChanges();

            }
            else
            {

                return NotFound("Visit Id is found or Visit is already Submitted ");

            }

            var updatedVisit = await _context.Visit.FindAsync(id);
            visit.Charges = _context.Charge.Where(m => m.VisitID == id).ToList<Charge>();
            return updatedVisit;

        }


        [Route("ResubmitCharge/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Charge>> ResubmitCharge(long id)
        {
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            Charge charge = _context.Charge.Where(c => c.ID == id && c.IsSubmitted == true).SingleOrDefault();
            if (charge != null)
            {
                charge.IsSubmitted = false;
                charge.IsResubmitted = true;
                _context.Charge.Update(charge);
                ResubmitHistory resHistory = new ResubmitHistory();
                _context.ResubmitHistory.Add(resHistory);
                resHistory.ChargeID = charge.ID;
                resHistory.AddedBy = Email;
                resHistory.AddedDate = DateTime.Now;
                _context.SaveChanges();
                var AllCharges = _context.Charge.Where(c => c.VisitID == charge.VisitID);
                var SubmittedFalse = _context.Charge.Where(c => c.VisitID == charge.VisitID && c.IsSubmitted == false);
                if (AllCharges.Count() == SubmittedFalse.Count())
                {
                    List<Visit> visit = _context.Visit.Where(v => v.ID == charge.VisitID).ToList<Visit>();
                    foreach (Visit v in visit)
                    {
                        v.PrimaryStatus = "RS";
                        _context.SaveChanges();
                    }
                }
            }
            else
            {
                return NotFound("No Charge Id is found or Charge is already Submitted ");
            }
            var updatedCharge = await _context.Charge.FindAsync(id);
            return updatedCharge;
        }


        [Route("FindVisit/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Visit>> FindVisit(long id)
        {
            var visit = await _context.Visit.FindAsync(id);
            if (visit == null)
            {
                return NotFound();
            }
            else
            {
                visit.Charges = _context.Charge.Where(m => m.VisitID == id).ToList<Charge>();
                visit.PatientPayments = _context.PatientPayment.Where(m => m.VisitID == id).ToList<PatientPayment>();
                visit.Note = _context.Notes.Where(m => m.VisitID == id).ToList<Notes>();
                visit.InstitutionalData = _context.InstitutionalData.Where(m => m.ID == id).FirstOrDefault<InstitutionalData>();
            }
            return visit;
        }

        [Route("FindCharge/{VisitId}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Charge>>> FindCharge(long VisitId)
        {
            List<Charge> charge = (from c in _context.Charge
                                   where c.VisitID == VisitId
                                   select c
                       ).ToList();

            if (charge == null)
            {
                NotFound();
            }

            return charge;
        }

        [Route("FindSubmittedCharge")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<object>>> FindSubmittedCharge(SubmittedCharge SubmittedCharge)
        {
            if (SubmittedCharge.ProcessedAs == "1" || SubmittedCharge.ProcessedAs == "" || SubmittedCharge.ProcessedAs == "19")
            {
                //var visit = (from v in _context.Visit where v.ID == SubmittedCharge.VisitID && v.IsSubmitted == true select v).SingleOrDefault();
                //if (visit == null) return BadRequest("Payment Can Not Be Applied Because Primary  Visit Is Not Submitted Yet ");

                if (SubmittedCharge.PaymentMethod.IsNull())
                {
                    var charge = await (from c in _context.Charge
                                        join p in _context.Patient on c.PatientID equals p.ID
                                        join cpt in _context.Cpt on c.CPTID equals cpt.ID
                                        join v in _context.Visit on c.VisitID equals v.ID
                                        where c.VisitID == SubmittedCharge.VisitID
                                        // && c.IsSubmitted == true 
                                        && c.PrimaryBal.Val() > 0 && c.PrimaryBilledAmount.Val() > 0
                                        select new
                                        {
                                            ChargeID = c.ID,
                                            DosFrom = c.DateOfServiceFrom.Format("MM/dd/yyyy"),
                                            CptID = c.CPTID,
                                            CptCode = cpt.CPTCode,
                                            BilledAmount = c.PrimaryBilledAmount,
                                            PatientID = c.PatientID,
                                            PatientName = p.LastName + ", " + p.FirstName + ", " + p.AccountNum,
                                            v.IsSubmitted
                                        }).ToListAsync();

                    if (charge == null || charge.Count == 0) return BadRequest("Primary Charges Not Found");
                    return charge;
                }
                else
                {
                    var charge = await (from c in _context.Charge
                                        join p in _context.Patient on c.PatientID equals p.ID
                                        join cpt in _context.Cpt on c.CPTID equals cpt.ID
                                        join v in _context.Visit on c.VisitID equals v.ID
                                        where c.VisitID == SubmittedCharge.VisitID
                                        //&& c.IsSubmitted == true
                                        //&& c.PrimaryBal.Val() > 0 && c.PrimaryBilledAmount.Val() > 0
                                        select new
                                        {
                                            ChargeID = c.ID,
                                            DosFrom = c.DateOfServiceFrom.Format("MM/dd/yyyy"),
                                            CptID = c.CPTID,
                                            CptCode = cpt.CPTCode,
                                            BilledAmount = c.PrimaryBilledAmount,
                                            PatientID = c.PatientID,
                                            PatientName = p.LastName + ", " + p.FirstName + ", " + p.AccountNum,
                                            v.IsSubmitted
                                        }).ToListAsync();

                    if (charge == null || charge.Count == 0) return BadRequest("Primary Charges Not Found");
                    return charge;
                }


            }
            else if (SubmittedCharge.ProcessedAs == "2" || SubmittedCharge.ProcessedAs == "20")
            {
                //var visit = (from v in _context.Visit where v.ID == SubmittedCharge.VisitID && v.IsSubmitted == true select v).SingleOrDefault();
                //if (visit == null) return BadRequest("Payment Can Not Be Applied Because Secondary Visit Is Not Submitted Yet ");

                var visit = _context.Visit.Find(SubmittedCharge.VisitID);

                if (visit.SecondaryPatientPlanID.IsNull() || visit.SecondaryBilledAmount.Val() == 0)
                    return BadRequest("Secondary Visit Not found");

                var charge = await (from c in _context.Charge
                                    join p in _context.Patient on c.PatientID equals p.ID
                                    join cpt in _context.Cpt on c.CPTID equals cpt.ID
                                    join v in _context.Visit on c.VisitID equals v.ID
                                    where c.VisitID == SubmittedCharge.VisitID //&& c.IsSubmitted == true
                                    //&& (c.SecondaryStatus == "S" || c.SecondaryStatus == "M") 
                                    && c.SecondaryBal.Val() > 0 && c.SecondaryBilledAmount.Val() > 0
                                    select new
                                    {
                                        ChargeID = c.ID,
                                        DosFrom = c.DateOfServiceFrom.Format("MM/dd/yyyy"),
                                        CptID = c.CPTID,
                                        CptCode = cpt.CPTCode,
                                        BilledAmount = !SubmittedCharge.PaymentMethod.IsNull() ? c.PrimaryBilledAmount : c.SecondaryBilledAmount,
                                        c.PrimaryBilledAmount,
                                        PatientID = c.PatientID,
                                        PatientName = p.LastName + ", " + p.FirstName + ", " + p.AccountNum,
                                        IsSubmitted = (c.SecondaryStatus == "S" || c.SecondaryStatus == "M") ? true : false
                                    }).ToListAsync();

                if (charge == null || charge.Count == 0) return BadRequest("Secondary Charges Not Found");

                return charge;
            }
            else if (SubmittedCharge.ProcessedAs == "22")
            {
                //var visit = (from v in _context.Visit where v.ID == SubmittedCharge.VisitID && v.IsSubmitted == true select v).SingleOrDefault();
                //if (visit == null) return BadRequest("Payment Can Not Be Applied Because Primary  Visit Is Not Submitted Yet ");

                var charge = await (from c in _context.Charge
                                    join p in _context.Patient on c.PatientID equals p.ID
                                    join cpt in _context.Cpt on c.CPTID equals cpt.ID
                                    join v in _context.Visit on c.VisitID equals v.ID
                                    where c.VisitID == SubmittedCharge.VisitID
                                    //&& c.IsSubmitted == true 
                                    && c.PrimaryBilledAmount.Val() > 0
                                    // c.PrimaryBal.Val() > 0 

                                    select new
                                    {
                                        ChargeID = c.ID,
                                        DosFrom = c.DateOfServiceFrom.Format("MM/dd/yyyy"),
                                        CptID = c.CPTID,
                                        CptCode = cpt.CPTCode,
                                        BilledAmount = c.PrimaryBilledAmount,
                                        PatientID = c.PatientID,
                                        PatientName = p.LastName + ", " + p.FirstName + ", " + p.AccountNum,
                                        v.IsSubmitted
                                    }).ToListAsync();
                if (charge == null) return BadRequest("Primary Charges Not Found");


                return charge;


            }
            else if (SubmittedCharge.ProcessedAs == "4")
            {
                //var visit = (from v in _context.Visit where v.ID == SubmittedCharge.VisitID && v.IsSubmitted == true select v).SingleOrDefault();
                //if (visit == null) return BadRequest("Payment Can Not Be Applied Because Primary  Visit Is Not Submitted Yet ");


                var charge = await (from c in _context.Charge
                                    join p in _context.Patient on c.PatientID equals p.ID
                                    join cpt in _context.Cpt on c.CPTID equals cpt.ID
                                    join v in _context.Visit on c.VisitID equals v.ID
                                    where c.VisitID == SubmittedCharge.VisitID
                                    //&& c.IsSubmitted == true 
                                    && c.PrimaryBal.Val() > 0 && c.PrimaryBilledAmount.Val() > 0
                                    select new
                                    {
                                        ChargeID = c.ID,
                                        DosFrom = c.DateOfServiceFrom.Format("MM/dd/yyyy"),
                                        CptID = c.CPTID,
                                        CptCode = cpt.CPTCode,
                                        BilledAmount = c.PrimaryBilledAmount,
                                        PatientID = c.PatientID,
                                        PatientName = p.LastName + ", " + p.FirstName + ", " + p.AccountNum,
                                        v.IsSubmitted
                                    }).ToListAsync();

                if (charge == null || charge.Count == 0) return BadRequest("Primary Charges Not Found");
                return charge;
            }
            return BadRequest("Submitted Charge Not Found");
        }


        //[HttpPost]
        //[Route("FindVisits")]
        //public async Task<ActionResult<IEnumerable<GVisit>>> FindVisits(CVisit CVisit)
        //{
        //    // UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
        //    //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
        //    //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
        //    //  if (UD == null || UD.Rights == null || UD.Rights.AdjustmentCodesCreate == false)
        //    //    return BadRequest("You Don't Have Rights. Please Contact BellMedex.");   // Need To Check VisitSearchRights

        //    long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
        //    return FindVisits(CVisit, PracticeId);
        //}
        //private List<GVisit> FindVisits(CVisit CVisit, long PracticeId)
        //{
        //    DateTime StartingTime = DateTime.Now;
        //    List<GVisit> data = (from v in _context.Visit
        //                         join pat in _context.Patient on v.PatientID equals pat.ID
        //                         //join prac in _context.Practice on pat.PracticeID equals prac.ID
        //                         //join up in _context.UserPractices on fac.ID equals up.PracticeID
        //                         //join u in _context.Users on up.UserID equals u.Id
        //                         join loc in _context.Location on v.LocationID equals loc.ID
        //                         join prov in _context.Provider on v.ProviderID equals prov.ID
        //                         join pPlan in _context.PatientPlan on v.PrimaryPatientPlanID equals pPlan.ID
        //                         join iPlan in _context.InsurancePlan on pPlan.InsurancePlanID equals iPlan.ID
        //                         orderby v.ID descending
        //                         where v.PracticeID == PracticeId &&
        //                         //u.Id.ToString() == UD.UserID &&
        //                         // && u.IsUserBlock == false &&
        //                         (CVisit.LastName.IsNull() ? true : pat.LastName.ToUpper().Contains(CVisit.LastName))
        //                         && (CVisit.FirstName.IsNull() ? true : pat.FirstName.ToUpper().Contains(CVisit.FirstName))
        //                         && (CVisit.AccountNum.IsNull() ? true : pat.AccountNum.ToUpper().Equals(CVisit.AccountNum))
        //                         //     && (CVisit.Practice.IsNull() ? true : prac.Name.ToUpper().Contains(CVisit.Practice))
        //                         && (CVisit.Location.IsNull() ? true : loc.Name.ToUpper().Contains(CVisit.Location))
        //                         && (CVisit.Provider.IsNull() ? true : prov.Name.ToUpper().Contains(CVisit.Provider))
        //                         && (CVisit.SubscriberID.IsNull() ? true : pPlan.SubscriberId.Equals(CVisit.SubscriberID))
        //                         // (CVisit.PayerID.IsNull() ? true : Edi837.PayerID.Equals(CVisit.PayerID))&&
        //                         && (CVisit.Plan.IsNull() ? true : iPlan.PlanName.ToUpper().Contains(CVisit.Plan))
        //                         && (CVisit.VisitID.IsNull() ? true : v.ID.Equals(CVisit.VisitID))
        //                         && (CVisit.BatchID.IsNull() ? true : v.BatchDocumentID.Equals(CVisit.BatchID))
        //                         // && (ExtensionMethods.IsBetweenDOS(CVisit.DosTo, CVisit.DosFrom, v.DateOfServiceFrom, v.DateOfServiceFrom))
        //                         //&& ((CVisit.EntryDateFrom != null && CVisit.EntryDateTo != null) ? (v.SubmittedDate != null ? v.SubmittedDate.GetValueOrDefault().Date <= CVisit.EntryDateTo.GetValueOrDefault().Date && v.SubmittedDate.GetValueOrDefault().Date >= CVisit.EntryDateFrom.GetValueOrDefault().Date : v.AddedDate != null ? v.SubmittedDate.GetValueOrDefault().Date >= CVisit.EntryDateFrom.GetValueOrDefault() : false) : (CVisit.EntryDateFrom != null ? (v.SubmittedDate != null && CVisit.EntryDateFrom.HasValue ? v.SubmittedDate.GetValueOrDefault().Date >= CVisit.EntryDateFrom.GetValueOrDefault() : true) : true))
        //                         //&& (ExtensionMethods.IsBetweenDOS(CVisit.EntryDateTo, CVisit.EntryDateFrom, v.AddedDate, v.AddedDate))
        //                         && ((CVisit.EntryDateFrom != null && CVisit.EntryDateTo != null) ? (v.AddedDate != null ? v.AddedDate.Date <= CVisit.EntryDateTo.GetValueOrDefault().Date && v.AddedDate.Date >= CVisit.EntryDateFrom.GetValueOrDefault().Date : v.AddedDate != null ? v.AddedDate.Date >= CVisit.EntryDateFrom.GetValueOrDefault() : false) : (CVisit.EntryDateFrom != null ? (v.AddedDate != null && CVisit.EntryDateFrom.HasValue ? v.AddedDate.Date >= CVisit.EntryDateFrom.GetValueOrDefault() : true) : true))
        //                         //&& (ExtensionMethods.IsBetweenDOS(CVisit.SubmittedToDate, CVisit.SubmittedFromDate, v.SubmittedDate, v.SubmittedDate))
        //                         && ((CVisit.SubmittedFromDate != null && CVisit.SubmittedToDate != null) ? (v.SubmittedDate != null ? v.SubmittedDate.GetValueOrDefault().Date <= CVisit.SubmittedToDate.GetValueOrDefault().Date && v.SubmittedDate.GetValueOrDefault().Date >= CVisit.SubmittedFromDate.GetValueOrDefault().Date : v.AddedDate != null ? v.SubmittedDate.GetValueOrDefault().Date >= CVisit.SubmittedFromDate.GetValueOrDefault() : false) : (CVisit.SubmittedFromDate != null ? (v.SubmittedDate != null && CVisit.EntryDateFrom.HasValue ? v.SubmittedDate.GetValueOrDefault().Date >= CVisit.SubmittedFromDate.GetValueOrDefault() : true) : true))
        //                         && (CVisit.InsuranceType == "P" ? v.PrimaryPatientPlanID.Value > 0 && v.SecondaryPatientPlanID == null && v.TertiaryPatientPlanID == null :
        //                         CVisit.InsuranceType == "S" ? v.PrimaryPatientPlanID.Value > 0 && v.SecondaryPatientPlanID > 0 && v.TertiaryPatientPlanID == null :
        //                         CVisit.InsuranceType == "T" ? v.PrimaryPatientPlanID.Value > 0 && v.SecondaryPatientPlanID > 0 && v.TertiaryPatientPlanID > 0 :
        //                         v.PrimaryPatientPlanID.Value > 0)
        //                         && (CVisit.SubmissionType.IsNull() ? true : iPlan.SubmissionType.Equals(CVisit.SubmissionType))
        //                         //  && (CVisit.Status.IsNull() ? true :   v.PrimaryStatus.StartsWith(CVisit.Status) ||  v.SecondaryStatus.StartsWith(CVisit.Status))
        //                         && (CVisit.Status.IsNull() ? true : v.PrimaryStatus.IsNull() || v.PrimaryStatus == "" ? false : v.PrimaryStatus.Equals(CVisit.Status) || (v.SecondaryStatus.IsNull() || v.SecondaryStatus == "" ? false : v.SecondaryStatus.Equals(CVisit.Status)))
        //                         && (CVisit.IsSubmitted == "Y" ? v.IsSubmitted == true : CVisit.IsSubmitted == "N" ? v.IsSubmitted == false : v.IsSubmitted == true || v.IsSubmitted == false)
        //                         && (CVisit.IsPaid.IsNull() ? true : CVisit.IsPaid.Equals("Y") ? v.PrimaryPaid.Val() > 0 && v.PrimaryBal.Val() + v.SecondaryBal.Val() + v.TertiaryBal.Val() == 0 :
        //                         CVisit.IsPaid.Equals("P") ? v.PrimaryPaid.Val() > 0 && v.PrimaryBal.Val() + v.SecondaryBal.Val() + v.TertiaryBal.Val() > 0 : v.PrimaryPaid.Val() == 0)
        //                         select new GVisit()
        //                         {
        //                             patientID = pat.ID,
        //                             AccountNum = pat.AccountNum,
        //                             Patient = pat.LastName + ", " + pat.FirstName,
        //                             DOS = v.DateOfServiceFrom.Format("MM/dd/yyyy"),
        //                             EntryDate = v.AddedDate.Format("MM/dd/yyyy"),
        //                             PracticeID = v.ID,
        //                             //Practice = prac.Name,
        //                             LocationID = loc.ID,
        //                             Location = loc.Name,
        //                             SubmittedDate = v.SubmittedDate.Format("MM/dd/yyyy"),
        //                             ClaimAge = (CVisit.AgeType == "S") ? (v.SubmittedDate.Date().IsNull() ? "" : System.DateTime.Now.Subtract(v.SubmittedDate.Date()).Days.ToString()) :
        //                             (CVisit.AgeType == "D") ? (v.DateOfServiceFrom.Date().IsNull() ? "" : System.DateTime.Now.Subtract(v.DateOfServiceFrom.Date()).Days.ToString()) :
        //                             (CVisit.AgeType == "E") ? (v.AddedDate.IsNull() ? "" : System.DateTime.Now.Subtract(v.AddedDate).Days.ToString()) : "",
        //                             //ClaimAge = (v.SubmittedDate.Date().IsNull() ?  "" : System.DateTime.Now.Subtract(v.SubmittedDate.Date()).Days.ToString() ),
        //                             BilledAmount = v.PrimaryBilledAmount,
        //                             AllowedAmount = v.PrimaryAllowed,
        //                             PaidAmount = v.PrimaryPaid,
        //                             //PrimaryStatus = v.PrimaryStatus == "N" ? "Regular" : (v.PrimaryStatus == "S" ? "Submitted" : (v.PrimaryStatus == "PPTS" ? "Transefered to Sec." : (v.PrimaryStatus == "PPTP" ? "Transfered to Pat" : ""))),
        //                             PrimaryStatus = TranslateStatus(v.PrimaryStatus),
        //                             AdjustmentAmount = v.PrimaryWriteOff,
        //                             PrimaryPlanBalance = v.PrimaryBal,
        //                             PrimaryPatientBalance = v.PrimaryPatientBal,
        //                             Rejection = v.RejectionReason,
        //                             ProviderID = prov.ID,
        //                             Provider = prov.Name,
        //                             VisitID = v.ID,
        //                             InsurancePlanID = iPlan.ID,
        //                             InsurancePlanName = iPlan.PlanName,
        //                             SubscriberID = pPlan.SubscriberId,
        //                             PrimaryPatientPlanID = v.PrimaryPatientPlanID.ToString(),
        //                             SecondaryStatus = TranslateStatus(v.SecondaryStatus),
        //                             SecondaryPlanBalance = v.SecondaryBal,
        //                             SecondaryPatientBalance = v.SecondaryPatientBal,
        //                             Edi837PayerID = iPlan.Edi837PayerID,
        //                             VisitType = v.VisitType
        //                         }).ToList();

        //    if (!CVisit.CPTCode.IsNull())
        //    {
        //        data = (from d in data
        //                join c in _context.Charge on d.VisitID equals c.VisitID
        //                join cpt in _context.Cpt on c.CPTID equals cpt.ID
        //                where cpt.CPTCode.Equals(CVisit.CPTCode)
        //                select d).Distinct().ToList();
        //    }
        //    if (!CVisit.ChargeID.IsNull())
        //    {
        //        data = (from d in data
        //                join c in _context.Charge on d.VisitID equals c.VisitID
        //                where c.ID.Equals(CVisit.ChargeID)
        //                select d).Distinct().ToList();
        //    }
        //    if (!CVisit.PayerID.IsNull())
        //    {
        //        data = (from d in data
        //                join edi in _context.Edi837Payer on d.Edi837PayerID equals edi.ID
        //                where (CVisit.PayerID.IsNull() ? true : edi.ID.Equals(CVisit.PayerID))

        //                select d).ToList();
        //    }
        //    DateTime EndTime = DateTime.Now;
        //    var temp = EndTime - StartingTime;
        //    Debug.WriteLine(temp.TotalSeconds + ":" + temp.TotalMilliseconds);
        //    return data;
        //}

        //[HttpPost]
        //[Route("Export")]
        //public async Task<IActionResult> Export(CVisit CVisit)
        //{
        //    UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
        //     User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
        //     User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
        //     );
        //    long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
        //    List<GVisit> data = FindVisits(CVisit, PracticeId);
        //    ExportController controller = new ExportController(_context);

        //    Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
        //    this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);

        //    return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CVisit, "Visit Report");

        //}

        //[HttpPost]
        //[Route("ExportPdf")]
        //public async Task<IActionResult> ExportPdf(CVisit CVisit)
        //{
        //    UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
        //     User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
        //     User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
        //     );
        //    long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
        //    List<GVisit> data = FindVisits(CVisit, PracticeId);
        //    ExportController controller = new ExportController(_context);

        //    Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
        //    this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);

        //    return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);
        //}

        [HttpPost]
        [Route("ApplyWriteOff")]
        public void ApplyWriteOff(ListModel list)
        {
            // Charge chg = new Charge();

            if (list == null || list.Ids.Count() == 0)
            {
                BadRequest("Please Select Visit IDs");
            }
            string Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            Visit V = new Visit();
            foreach (long visitID in list.Ids)
            {
                V = _context.Visit.Find(visitID);
                if (V == null) continue;

                List<Charge> chg = _context.Charge.Where(e => e.VisitID == V.ID &&
                                                        ((e.PrimaryBal.Val() + e.SecondaryBal.Val() + e.TertiaryBal.Val() > 0) ||
                                                        (e.PrimaryPatientBal.Val() + e.SecondaryPatientBal.Val() + e.TertiaryPatientBal.Val() > 0))).ToList();

                foreach (Charge chrg in chg)
                {
                    decimal? patbal = chrg.PrimaryPatientBal.Val() + chrg.SecondaryPatientBal.Val() + chrg.TertiaryPatientBal.Val();
                    decimal? planBal = chrg.PrimaryBal.Val() + chrg.SecondaryBal.Val() + chrg.TertiaryBal.Val();

                    if (chrg.PrimaryBal.Val() > 0)
                    {
                        chrg.PrimaryWriteOff = chrg.PrimaryWriteOff.Val() + chrg.PrimaryBal.Val();
                        _context.PaymentLedger.Add(AddLedger(Email, chrg, null, chrg.PrimaryBal, "MANUAL PLAN WRITE OFF", "USER", chrg.PrimaryPatientPlanID));
                        chrg.PrimaryBal = null;
                        chrg.PrimaryStatus = "W";
                        chrg.WriteOffReason = list.Status;
                    }
                    if (chrg.SecondaryBal.Val() > 0)
                    {
                        chrg.SecondaryWriteOff = chrg.SecondaryWriteOff.Val() + chrg.SecondaryBal.Val();
                        _context.PaymentLedger.Add(AddLedger(Email, chrg, null, chrg.SecondaryBal, "MANUAL PLAN WRITE OFF", "USER", chrg.SecondaryPatientPlanID));
                        chrg.SecondaryBal = null;
                        chrg.SecondaryStatus = "W";
                        chrg.WriteOffReason = list.Status;
                    }
                    if (chrg.TertiaryBal.Val() > 0)
                    {
                        chrg.TertiaryWriteOff = chrg.TertiaryWriteOff.Val() + chrg.TertiaryBal.Val();
                        _context.PaymentLedger.Add(AddLedger(Email, chrg, null, chrg.TertiaryBal, "MANUAL PLAN WRITE OFF", "USER", chrg.TertiaryPatientPlanID));
                        chrg.TertiaryBal = null;
                        chrg.TertiaryStatus = "W";
                        chrg.WriteOffReason = list.Status;
                    }
                    if (chrg.PrimaryPatientBal.Val() > 0)
                    {
                        chrg.PrimaryWriteOff = chrg.PrimaryWriteOff.Val() + chrg.PrimaryPatientBal.Val();
                        _context.PaymentLedger.Add(AddLedger(Email, chrg, null, chrg.PrimaryPatientBal, "MANUAL PAT WRITE OFF", "USER", chrg.PrimaryPatientPlanID));
                        chrg.PrimaryPatientBal = null;
                        chrg.PrimaryStatus = "W";
                        chrg.WriteOffReason = list.Status;
                    }
                    if (chrg.SecondaryPatientBal.Val() > 0)
                    {
                        chrg.SecondaryWriteOff = chrg.SecondaryWriteOff.Val() + chrg.SecondaryPatientBal.Val();
                        _context.PaymentLedger.Add(AddLedger(Email, chrg, null, chrg.SecondaryPatientBal, "MANUAL PAT WRITE OFF", "USER", chrg.SecondaryPatientPlanID));
                        chrg.SecondaryPatientBal = null;
                        chrg.SecondaryStatus = "W";
                        chrg.WriteOffReason = list.Status;
                    }
                    if (chrg.TertiaryPatientBal.Val() > 0)
                    {
                        chrg.TertiaryWriteOff = chrg.TertiaryWriteOff.Val() + chrg.TertiaryPatientBal.Val();
                        _context.PaymentLedger.Add(AddLedger(Email, chrg, null, chrg.TertiaryPatientBal, "MANUAL PAT WRITE OFF", "USER", chrg.TertiaryPatientPlanID));
                        chrg.TertiaryPatientBal = null;
                        chrg.TertiaryStatus = "W";
                        chrg.WriteOffReason = list.Status;
                    }
                    _context.Charge.Update(chrg);
                    _context.SaveChanges();
                }
                V.PrimaryWriteOff = _context.Charge.Where(c => c.VisitID == V.ID).Sum(c => c.PrimaryWriteOff.Val());
                V.SecondaryWriteOff = _context.Charge.Where(c => c.VisitID == V.ID).Sum(c => c.SecondaryWriteOff.Val());
                V.TertiaryWriteOff = _context.Charge.Where(c => c.VisitID == V.ID).Sum(c => c.TertiaryWriteOff.Val());
                V.PrimaryBal = _context.Charge.Where(c => c.VisitID == V.ID).Sum(c => c.PrimaryBal.Val());
                V.SecondaryBal = _context.Charge.Where(c => c.VisitID == V.ID).Sum(c => c.SecondaryBal.Val());
                V.TertiaryBal = _context.Charge.Where(c => c.VisitID == V.ID).Sum(c => c.TertiaryBal.Val());
                V.PrimaryPatientBal = _context.Charge.Where(c => c.VisitID == V.ID).Sum(c => c.PrimaryPatientBal.Val());
                V.SecondaryPatientBal = _context.Charge.Where(c => c.VisitID == V.ID).Sum(c => c.SecondaryPatientBal.Val());
                V.TertiaryPatientBal = _context.Charge.Where(c => c.VisitID == V.ID).Sum(c => c.TertiaryPatientBal.Val());
                V.PrimaryStatus = _context.Charge.Where(c => c.VisitID == V.ID).Select(c => c.PrimaryStatus).FirstOrDefault();
                V.SecondaryStatus = _context.Charge.Where(c => c.VisitID == V.ID).Select(c => c.SecondaryStatus).FirstOrDefault();
                V.TertiaryStatus = _context.Charge.Where(c => c.VisitID == V.ID).Select(c => c.TertiaryStatus).FirstOrDefault();
                V.WriteOffReason = list.Status;
                _context.Visit.Update(V);
            }


            _context.SaveChanges();

        }

        public string TranslateStatus(string Status)
        {


            string desc = string.Empty;
            if (Status == "N")
            {
                desc = "New Charge";
            }
            if (Status == "S")
            {
                desc = "Submitted";
            }
            if (Status == "K")
            {
                desc = "999 Accepted";
            }
            if (Status == "D")
            {
                desc = "999 Denied";
            }
            if (Status == "E")
            {
                desc = "999 has Errors";
            }
            if (Status == "P")
            {
                desc = "Paid";
            }
            if (Status == "DN")
            {
                desc = "Denied";
            }
            if (Status == "PT_P")
            {
                desc = "Patient Paid";
            }
            if (Status == "PPTS")
            {
                desc = "Transefered to Sec.";
            }
            if (Status == "PPTT")
            {
                desc = "Paid-Transfered To Ter";
            }
            if (Status == "PPTP")
            {
                desc = "Transfered to Patient";
            }
            if (Status == "SPTP")
            {
                desc = "Paid-Transfered To Patient";
            }
            if (Status == "SPTT")
            {
                desc = "Paid-Transfered To Ter";
            }
            if (Status == "PR_TP")
            {
                desc = "Pat. Resp. Transferred to Pat";
            }
            if (Status == "PPTM")
            {
                desc = "Paid - Medigaped";
            }
            if (Status == "M")
            {
                desc = "Medigaped";
            }
            if (Status == "R")
            {
                desc = "Rejected";
            }
            if (Status == "A1AY")
            {
                desc = "Received By Clearing House";
            }
            if (Status == "A0PR")
            {
                desc = "Forwarded  to Payer";
            }
            if (Status == "A1PR")
            {
                desc = "Received By Payer";
            }
            if (Status == "A2PR")
            {
                desc = "Accepted By Payer";
            }
            if (Status == "TS")
            {
                desc = "Transferred to Secondary";
            }
            if (Status == "TT")
            {
                desc = "Transferred to Tertiary";
            }
            if (Status == "PTPT")
            {
                desc = "Plan to Patient Transfer";
            }

            if (Status == "RS")
            {
                desc = "Re-Submitted";
            }


            return desc;
        }


        [Route("SaveVisit")]
        [HttpPost]
        public async Task<ActionResult<Visit>> SaveVisit(Visit item)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
            );

            if (item.ClientID != UD.ClientID)
                return BadRequest("Data can't be saved as Practice has been switched.");

            try
            {
                // object o = null;
                // string oo = o.ToString();
                if (item.Charges == null || item.Charges.Count == 0)
                {
                    return BadRequest("Charges Not Found");
                }


                bool succ = TryValidateModel(item);
                if (!ModelState.IsValid)
                {
                    string messages = string.Join("; ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
                    return BadRequest(messages);
                }

                long CheckVisitId = 0;

                Visit OldVisit = null;
                DateTime OldDOS = DateTime.MinValue;


                var transactionOption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted };
                using (var objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption, TransactionScopeAsyncFlowOption.Enabled))
                {
                    //DOS
                    item.DateOfServiceFrom = item.Charges.ToArray()[0].DateOfServiceFrom;
                    if (item.ID > 0)
                    {
                        OldVisit = _context.Visit.Where(v => v.ID == item.ID && v.PracticeID == UD.PracticeID).AsNoTracking().FirstOrDefault();
                        if (OldVisit != null)
                        {
                            if (OldVisit.DateOfServiceFrom.HasValue)
                            {
                                OldDOS = OldVisit.DateOfServiceFrom.Value;
                            }
                        }

                    }
                    //
                    #region Batch Linking
                    bool NewBatchCharge = false;
                    long BatchDocumentIdToUse = 0;

                    BatchDocument Batch = null;
                    BatchDocumentCharges BatCharge = null;
                    if (item.BatchDocument != null)
                    {
                        Batch = item.BatchDocument;
                    }

                    if (Batch == null)
                    {
                        if (OldVisit != null)
                        {
                            Batch = _context.BatchDocument.Find(OldVisit.BatchDocumentID.GetValueOrDefault());
                            if (Batch != null)
                            {
                                if (item.BatchDocument != null && item.BatchDocument.BatchDocumentCharges != null)
                                    BatCharge = item.BatchDocument.BatchDocumentCharges.Where(m => m.BatchDocumentNoID == Batch.ID && OldDOS == m.DOS).FirstOrDefault();
                            }
                        }
                        else
                        {
                            if (item.BatchDocumentID.GetValueOrDefault() != 0)
                            {
                                Batch = _context.BatchDocument.Find(item.BatchDocumentID.GetValueOrDefault());
                                if (Batch != null)
                                {
                                    if (item.BatchDocument != null && item.BatchDocument.BatchDocumentCharges != null)
                                        BatCharge = item.BatchDocument.BatchDocumentCharges.Where(m => m.BatchDocumentNoID == Batch.ID && m.DOS == item.DateOfServiceFrom.Value).FirstOrDefault();
                                }
                            }
                        }
                    }
                    if (Batch != null)
                    {
                        if (OldDOS != DateTime.MinValue)
                        {
                            if (BatCharge == null)
                                BatCharge = _context.BatchDocumentCharges.Where(m => m.BatchDocumentNoID == Batch.ID && OldDOS == m.DOS).FirstOrDefault();
                        }
                        else if (item.DateOfServiceFrom.HasValue)
                        {
                            if (BatCharge == null)
                                BatCharge = _context.BatchDocumentCharges.Where(m => m.BatchDocumentNoID == Batch.ID && m.DOS == item.DateOfServiceFrom.Value).FirstOrDefault();
                        }
                        else
                        {
                            BatCharge = new BatchDocumentCharges();

                            BatCharge.BatchDocumentNoID = Batch.ID;
                            BatCharge.AddedBy = UD.Email;
                            BatCharge.AddedDate = DateTime.Now;
                            BatCharge.DOS = item.DateOfServiceFrom;
                            if (Batch.StartDate == null)
                            {
                                Batch.StartDate = DateTime.Now;
                                Batch.BatchDocumentCharges.Add(BatCharge);
                                _context.BatchDocument.Update(Batch);
                            }
                            NewBatchCharge = true;
                        }
                        if (BatCharge == null)
                        {
                            if (Batch.BatchDocumentCharges == null)
                                Batch.BatchDocumentCharges = new List<BatchDocumentCharges>();
                            BatCharge = new BatchDocumentCharges();

                            BatCharge.BatchDocumentNoID = Batch.ID;
                            BatCharge.AddedBy = UD.Email;
                            BatCharge.AddedDate = DateTime.Now;
                            BatCharge.DOS = item.DateOfServiceFrom;
                            if (Batch.StartDate == null)
                            {
                                Batch.StartDate = DateTime.Now;
                                Batch.BatchDocumentCharges.Add(BatCharge);
                                _context.BatchDocument.Update(Batch);
                            }
                            else
                            {
                                Batch.BatchDocumentCharges.Add(BatCharge);
                            }

                            NewBatchCharge = true;
                        }

                        bool CounterChangedOnce = false;
                        if (OldVisit != null)
                        {
                            if ((item.PageNumber != null && item.PageNumber != "") && (OldVisit.PageNumber == null && OldVisit.PageNumber == ""))
                            {
                                if (BatCharge != null)
                                {
                                    BatCharge.NoOfVisits -= 1;
                                    CounterChangedOnce = true;
                                }
                            }
                            else if ((item.PageNumber == null && item.PageNumber == "") && (OldVisit.PageNumber != null && OldVisit.PageNumber != ""))
                            {
                                if (BatCharge != null)
                                {
                                    BatCharge.NoOfVisits += 1;
                                    CounterChangedOnce = true;
                                }
                            }
                            if (item.BatchDocumentID.GetValueOrDefault() != 0 && OldVisit.BatchDocumentID.GetValueOrDefault() == 0)
                            {
                                if (BatCharge != null && CounterChangedOnce == false)
                                {
                                    BatCharge.NoOfVisits -= 1;
                                }
                                item.BatchDocumentID = null;
                            }
                            if (item.BatchDocumentID.GetValueOrDefault() == 0 && OldVisit.BatchDocumentID.GetValueOrDefault() != 0)
                            {
                                if (BatCharge != null && CounterChangedOnce == false)
                                {
                                    BatCharge.NoOfVisits += 1;
                                }
                            }
                        }
                        else
                        {
                            if (BatCharge == null)
                                BatCharge = _context.BatchDocumentCharges.Where(m => m.BatchDocumentNoID == Batch.ID && m.DOS == item.DateOfServiceFrom.Value).FirstOrDefault();
                            if (BatCharge != null)
                            {
                                if (BatCharge.NoOfVisits == null)
                                    BatCharge.NoOfVisits = 0;
                                BatCharge.NoOfVisits += 1;
                            }
                        }
                    }

                    //if(item.ID == 0 && item.BatchDocumentID != null)
                    //{
                    //    if(BatCharge == null)
                    //        BatCharge = _context.BatchDocumentCharges.Where(m => m.BatchDocumentNoID == Batch.ID && m.DOS == item.DateOfServiceFrom.Value).FirstOrDefault();
                    //    if(BatCharge != null)
                    //    {
                    //        BatCharge.NoOfVisits += 1;
                    //    }
                    //}
                    if (NewBatchCharge)
                    {
                        if (Batch != null)
                        {
                            _context.BatchDocumentCharges.Add(BatCharge);
                        }
                    }
                    else
                    {
                        if (Batch != null)
                        {
                            _context.BatchDocumentCharges.Update(BatCharge);
                        }
                    }



                    #endregion

                    try
                    {
                        List<PatientAuthorization> list = new List<PatientAuthorization>();
                        List<GVMVisit> ChargeList = new List<GVMVisit>();
                        foreach (var charge in item.Charges)
                        {

                            //Checking if any authorization is available against this patient
                            var PatAuthList = (from pAuth in _context.PatientAuthorization
                                               join ip in _context.InsurancePlan on pAuth.InsurancePlanID equals ip.ID
                                               join pp in _context.PatientPlan on ip.ID equals pp.InsurancePlanID
                                               where (pp.ID == item.PrimaryPatientPlanID)
                                               && pAuth.AuthorizationNumber == item.AuthorizationNum
                                               && pAuth.ProviderID == item.ProviderID
                                               && pAuth.PatientID == item.PatientID
                                               select pAuth
                                        ).ToList();
                            if (PatAuthList.Count > 0)
                            {

                                var pAuthList = (from pAuth in PatAuthList
                                                 join cpt in _context.Cpt
                                                 on pAuth.CPTID equals cpt.ID
                                                 where pAuth.CPTID == charge.CPTID
                                                 && pAuth.PatientID == item.PatientID
                                                 select pAuth
                                    ).ToList();
                                if (pAuthList.Count > 0)
                                {
                                    var authorization = (from pAuth in pAuthList
                                                         join pro in _context.Provider
                                                         on pAuth.ProviderID equals pro.ID
                                                         join ip in _context.InsurancePlan on pAuth.InsurancePlanID equals ip.ID
                                                         join pp in _context.PatientPlan on ip.ID equals pp.InsurancePlanID
                                                         where (pp.ID == item.PrimaryPatientPlanID)
                                                         && pAuth.PatientID == item.PatientID
                                                         && pAuth.CPTID == charge.CPTID
                                            && pAuth.AuthorizationNumber == item.AuthorizationNum
                                                        ////&& (ExtensionMethods.IsBetweenDOS(charge.DateOfServiceTo, charge.DateOfServiceFrom, pAuth.EndDate, pAuth.StartDate))
                                                        //&& charge.DateOfServiceFrom.Date() >= pAuth.StartDate.Date()
                                                        //&& charge.DateOfServiceFrom.Date() <= pAuth.EndDate.Date()
                                                        //&& charge.DateOfServiceTo.Date() >= pAuth.StartDate.Date()
                                                        //&& charge.DateOfServiceTo.Date() <= pAuth.EndDate.Date()
                                                        && ((pAuth.VisitsAllowed.ValZero() - pAuth.VisitsUsed.ValZero()) > 0)
                                                         select pAuth
                                  ).FirstOrDefault();

                                    if (authorization == null)
                                    {
                                        if (item.ID == 0)
                                        {
                                            return BadRequest("Authorization is expired against CPT");
                                        }
                                        else
                                        {
                                            //Do Nothing
                                        }

                                    }
                                    else
                                    {
                                        list.Add(authorization);
                                    }
                                }




                            }
                            var oldCharge = (from c in _context.Charge
                                             where c.ID == charge.ID
                                             select new GVMVisit
                                             {
                                                 ChargeID = c.ID,
                                                 patientID = c.PatientID,
                                                 CPTID = c.CPTID,
                                                 PrimaryPatientPlanID = c.PrimaryPatientPlanID,
                                                 AuthorizationNum = c.AuthorizationNum,
                                                 ProviderID = c.ProviderID
                                             }).FirstOrDefault();
                            if (oldCharge != null)
                            {
                                ChargeList.Add(oldCharge);
                            }
                        }

                        long? defaultActionID = _context.Action.Where(a => a.Name == "SYSTEM").FirstOrDefault()?.ID;
                        long? defaultGroupID = _context.Group.Where(a => a.Name == "NEW").FirstOrDefault()?.ID;
                        long? defaultReasonID = _context.Reason.Where(a => a.Name == "SYSTEM").FirstOrDefault()?.ID;

                        // DOS
                        item.DateOfServiceFrom = item.Charges.ToArray()[0].DateOfServiceFrom;
                        //

                        long? insurancePlanID = _context.PatientPlan.Where(m => m.ID == item.PrimaryPatientPlanID).SingleOrDefault()?.InsurancePlanID;
                        if (item.ID == 0)
                        {
                            if (insurancePlanID == 1)
                            {
                                item.PrimaryPatientBal = item.PrimaryBilledAmount.Val();
                                item.PrimaryBal = 0;
                                item.IsSubmitted = true;
                                item.PrimaryStatus = "S";
                                //CreatePatientFollowup(item.PatientID, defaultActionID, defaultReasonID, defaultGroupID, UD.Email, item.Charges.ToList()[0].ID);
                            }
                            else
                            {
                                item.PrimaryBal = item.PrimaryBilledAmount.Val() - item.Copay.Val();
                                item.PrimaryPatientBal = item.Copay.Val() - item.CopayPaid.Val();
                            }
                            item.AddedBy = UD.Email;
                            item.AddedDate = DateTime.Now;
                            _context.Visit.Add(item);

                            // Adding Charges
                            if (item.Note != null && item.Note.Count > 0)
                            {
                                foreach (Notes notes in item.Note)
                                {
                                    if (notes.ID <= 0)
                                    {

                                        notes.VisitID = item.ID;
                                        notes.AddedBy = UD.Email;
                                        notes.AddedDate = DateTime.Now;
                                        notes.NotesDate = DateTime.Now;
                                        _context.Notes.Add(notes);
                                    }
                                }
                            }

                            foreach (Charge charge in item.Charges)
                            {
                                //bool ChargeExists = _context.Charge.Count(c => c.DateOfServiceFrom == charge.DateOfServiceFrom.Date && c.CPTID == charge.CPTID && c.PatientID == item.PatientID && charge.ID <= 0) > 0;
                                //if (ChargeExists)
                                //{
                                //    return BadRequest("Dos With Same Procedure Already Exists.");
                                //}

                                if (insurancePlanID == 1)
                                {
                                    charge.PrimaryPatientBal = charge.PrimaryBilledAmount.Val();
                                    charge.IsSubmitted = true;
                                    charge.PrimaryStatus = "S";
                                    charge.PrimaryBal = 0;
                                }
                                else
                                {
                                    charge.PrimaryBal = charge.PrimaryBilledAmount;
                                }
                                charge.VisitID = item.ID;
                                charge.ClientID = item.ClientID;
                                charge.PracticeID = item.PracticeID;
                                charge.LocationID = item.LocationID;
                                charge.ProviderID = item.ProviderID;
                                //  charge.POSID = item.POSID;           //Comment POSID bec it is Comming From FrontEnd
                                charge.RefProviderID = item.RefProviderID;
                                charge.SupervisingProvID = item.SupervisingProvID;
                                charge.OrderingProvID = item.OrderingProvID;
                                charge.PatientID = item.PatientID;
                                charge.PrimaryPatientPlanID = item.PrimaryPatientPlanID;
                                charge.SecondaryPatientPlanID = item.SecondaryPatientPlanID;
                                charge.TertiaryPatientPlanID = item.TertiaryPatientPlanID;
                                charge.AddedBy = UD.Email;
                                charge.AddedDate = DateTime.Now;
                                charge.IsReversalApplied = item.IsReversalApplied;
                                charge.WriteOffReason = item.WriteOffReason;
                                _context.Charge.Add(charge);

                            }

                        }
                        else
                        {
                            item.UpdatedBy = UD.Email;
                            item.UpdatedDate = DateTime.Now;
                            item.PrimaryBilledAmount = item.Charges.Sum(c => c.PrimaryBilledAmount.Val());

                            _context.Visit.Update(item);




                            if (item.Note != null && item.Note.Count > 0)
                            {
                                foreach (Notes notes in item.Note)
                                {
                                    if (notes.ID <= 0)
                                    {

                                        notes.VisitID = item.ID;
                                        notes.AddedBy = UD.Email;
                                        notes.AddedDate = DateTime.Now;
                                        notes.NotesDate = DateTime.Now;
                                        _context.Notes.Add(notes);
                                    }
                                    else
                                    {

                                        notes.VisitID = item.ID;
                                        notes.UpdatedBy = UD.Email;
                                        notes.UpdatedDate = DateTime.Now;
                                        notes.NotesDate = DateTime.Now;
                                        _context.Notes.Update(notes);

                                    }
                                }
                            }


                            // Updating Charges
                            foreach (Charge charge in item.Charges)
                            {


                                if (insurancePlanID != 1)
                                    if (charge.IsSubmitted == true) continue;
                                if (insurancePlanID == 1)
                                {
                                    charge.PrimaryPatientBal = charge.PrimaryBilledAmount.Val() - charge.PatientPaid.Val() - charge.Discount.Val();
                                    charge.IsSubmitted = true;
                                    charge.PrimaryStatus = "S";
                                    charge.PrimaryBal = 0;
                                }
                                else
                                {
                                    charge.PrimaryBal = charge.PrimaryBilledAmount;
                                    charge.PrimaryPatientBal -= charge.Discount.Val();
                                }
                                if (charge.ID <= 0)
                                {
                                    //bool ChargeExists = _context.Charge.Count(c => c.DateOfServiceFrom == charge.DateOfServiceFrom.Date && c.CPTID == charge.CPTID && c.PatientID == item.PatientID && charge.ID <= 0) > 0;
                                    //if (ChargeExists)
                                    //{
                                    //    return BadRequest("Dos With Same Procedure Already Exists.");
                                    //}
                                    charge.VisitID = item.ID;
                                    charge.ClientID = item.ClientID;
                                    charge.PracticeID = item.PracticeID;
                                    charge.LocationID = item.LocationID;
                                    charge.ProviderID = item.ProviderID;
                                    //  charge.POSID = item.POSID;           //Comment POSID bec it is Comming From FrontEnd
                                    charge.RefProviderID = item.RefProviderID;
                                    charge.SupervisingProvID = item.SupervisingProvID;
                                    charge.OrderingProvID = item.OrderingProvID;
                                    charge.PatientID = item.PatientID;
                                    charge.PrimaryPatientPlanID = item.PrimaryPatientPlanID;
                                    charge.SecondaryPatientPlanID = item.SecondaryPatientPlanID;
                                    charge.TertiaryPatientPlanID = item.TertiaryPatientPlanID;
                                    //charge.PrimaryBal = charge.PrimaryBilledAmount;
                                    charge.AddedBy = UD.Email;
                                    charge.AddedDate = DateTime.Now;
                                    charge.IsReversalApplied = item.IsReversalApplied;
                                    charge.WriteOffReason = item.WriteOffReason;
                                    _context.Charge.Add(charge);

                                }
                                else
                                {
                                    //bool ChargeExistsUpdate = _context.Charge.Any(c => c.DateOfServiceFrom == charge.DateOfServiceFrom.Date && c.CPTID == charge.CPTID && c.PatientID == item.PatientID && charge.ID != c.ID);

                                    //if (ChargeExistsUpdate == true)
                                    //{
                                    //    return BadRequest("Dos With Same Procedure Already Exists.");
                                    //}
                                    charge.UpdatedBy = UD.Email;
                                    charge.ClientID = item.ClientID;
                                    charge.PracticeID = item.PracticeID;
                                    charge.LocationID = item.LocationID;
                                    charge.ProviderID = item.ProviderID;
                                    //  charge.POSID = item.POSID;           //Comment POSID bec it is Comming From FrontEnd
                                    charge.RefProviderID = item.RefProviderID;
                                    charge.SupervisingProvID = item.SupervisingProvID;
                                    charge.OrderingProvID = item.OrderingProvID;
                                    charge.PatientID = item.PatientID;
                                    charge.PrimaryPatientPlanID = item.PrimaryPatientPlanID;
                                    charge.SecondaryPatientPlanID = item.SecondaryPatientPlanID;
                                    charge.TertiaryPatientPlanID = item.TertiaryPatientPlanID;
                                    //charge.PrimaryBal = charge.PrimaryBilledAmount;
                                    charge.UpdatedDate = DateTime.Now;
                                    charge.IsReversalApplied = item.IsReversalApplied;
                                    charge.WriteOffReason = item.WriteOffReason;
                                    _context.Charge.Update(charge);

                                }
                            }

                            if (item.IsSubmitted == false)
                            {
                                //if (item.Copay.Val() == item.CopayPaid.Val())
                                if (insurancePlanID == 1)
                                {
                                    item.PrimaryBal = 0;
                                    item.PrimaryPatientBal = item.Charges.Sum(c => c.PrimaryPatientBal.Val());
                                    item.IsSubmitted = true;
                                    item.PrimaryStatus = "S";
                                }
                                else
                                {
                                    if (item.Copay.Val() > 0)
                                    {
                                        item.PrimaryBal = item.Charges.Sum(c => c.PrimaryBal.Val()) - item.Copay.Val();

                                        //if (item.PrimaryPatientBal.Val() == 0 && item.Copay.Val() > 0)
                                        item.PrimaryPatientBal = item.Copay.Val() - item.CopayPaid.Val();
                                    }
                                    else
                                    {
                                        item.PrimaryBal = item.Charges.Sum(c => c.PrimaryBal.Val());
                                        item.PrimaryPatientBal = 0;
                                    }
                                }
                            }
                        }
                        if (item.PatientPayments != null && item.PatientPayments.Count > 0)
                        {
                            item.PatientPayments.ToList().ForEach(x => x.AddedDate = (x.AddedDate != null ? x.AddedDate.Value.IsNull() : true) ? DateTime.Now : x.AddedDate.Value);
                            item.PatientPayments.ToList().ForEach(x => x.AddedBy = (!x.AddedBy.IsNull() ? x.AddedBy : UD.Email));
                            item.PatientPayments.ToList().ForEach(x => x.VisitID = x.VisitID.IsNull() ? item.ID : x.VisitID);
                        }


                        await _context.SaveChangesAsync();

                        //////Institutional Data In Case Of Save n Update
                        if (item.InstitutionalData != null)
                        {
                            InstitutionalData data = _context.InstitutionalData.Where(m => m.ID == item.ID).SingleOrDefault();
                            if (data == null)
                            {
                                InstitutionalData dropdown = Newtonsoft.Json.JsonConvert.DeserializeObject<InstitutionalData>(item.InstitutionalData.ToString());
                                dropdown.ID = item.ID;
                                dropdown.AddedBy = UD.Email;
                                dropdown.AddedDate = DateTime.Now;

                                _context.InstitutionalData.Add(dropdown);

                                item.VisitType = "I";
                                _context.Visit.Update(item);
                            }
                            else
                            {
                                InstitutionalData dropdown = Newtonsoft.Json.JsonConvert.DeserializeObject<InstitutionalData>(item.InstitutionalData.ToString());
                                data.StatementFromDate = dropdown.StatementFromDate;
                                data.StatementToDate = dropdown.StatementToDate;
                                data.PatientStatusCodeID = dropdown.PatientStatusCodeID;
                                data.ReasonOfVisitID = dropdown.ReasonOfVisitID;
                                data.AdmissionDate = dropdown.AdmissionDate;
                                data.AdmissionHour = dropdown.AdmissionHour;
                                data.AdmissionType = dropdown.AdmissionType;
                                data.AdmissionSourceID = dropdown.AdmissionSourceID;
                                data.DischargeDate = dropdown.DischargeDate;
                                data.PrincipalCodeID = dropdown.PrincipalCodeID;
                                data.AdmittingCodeID = dropdown.AdmittingCodeID;
                                data.ExternalInjuryCode1ID = dropdown.ExternalInjuryCode1ID;
                                data.ExternalInjuryCode2ID = dropdown.ExternalInjuryCode2ID;
                                data.ExternalInjuryCode3ID = dropdown.ExternalInjuryCode3ID;
                                data.PrincipalProcedureCodeID1 = dropdown.PrincipalProcedureCodeID1;
                                data.PrincipalProcedureDate1 = dropdown.PrincipalProcedureDate1;
                                data.PrincipalProcedureCodeID2 = dropdown.PrincipalProcedureCodeID2;
                                data.PrincipalProcedureDate2 = dropdown.PrincipalProcedureDate2;
                                data.PrincipalProcedureCodeID3 = dropdown.PrincipalProcedureCodeID3;
                                data.PrincipalProcedureDate3 = dropdown.PrincipalProcedureDate3;
                                data.PrincipalProcedureCodeID4 = dropdown.PrincipalProcedureCodeID4;
                                data.PrincipalProcedureDate4 = dropdown.PrincipalProcedureDate4;
                                data.PrincipalProcedureCodeID5 = dropdown.PrincipalProcedureCodeID5;
                                data.PrincipalProcedureDate5 = dropdown.PrincipalProcedureDate5;
                                data.PrincipalProcedureCodeID6 = dropdown.PrincipalProcedureCodeID6;
                                data.PrincipalProcedureDate6 = dropdown.PrincipalProcedureDate6;
                                //data.ValueCodeID1 = dropdown.ValueCodeID1;
                                data.ValueCode1ID = dropdown.ValueCode1ID;
                                data.ValueCode2ID = dropdown.ValueCode2ID;
                                data.ValueCode3ID = dropdown.ValueCode3ID;
                                data.ValueCode4ID = dropdown.ValueCode4ID;
                                data.ValueCode5ID = dropdown.ValueCode5ID;
                                data.ValueCode6ID = dropdown.ValueCode6ID;
                                data.ValueCode7ID = dropdown.ValueCode7ID;
                                data.ValueCode8ID = dropdown.ValueCode8ID;
                                data.ValueCode9ID = dropdown.ValueCode9ID;
                                data.ValueCode10ID = dropdown.ValueCode10ID;
                                data.ValueCode11ID = dropdown.ValueCode11ID;
                                data.ValueCode12ID = dropdown.ValueCode12ID;
                                data.ConditionCode1ID = dropdown.ConditionCode1ID;
                                data.ConditionCode2ID = dropdown.ConditionCode2ID;
                                data.ConditionCode3ID = dropdown.ConditionCode3ID;
                                data.ConditionCode4ID = dropdown.ConditionCode4ID;
                                data.ConditionCode5ID = dropdown.ConditionCode5ID;
                                data.ConditionCode6ID = dropdown.ConditionCode6ID;
                                data.ConditionCode7ID = dropdown.ConditionCode7ID;
                                data.ConditionCode8ID = dropdown.ConditionCode8ID;
                                data.ConditionCode9ID = dropdown.ConditionCode9ID;
                                data.ConditionCode10ID = dropdown.ConditionCode10ID;
                                data.ConditionCode11ID = dropdown.ConditionCode11ID;
                                data.ConditionCode12ID = dropdown.ConditionCode12ID;
                                data.OccuranceSpanCode1ID = dropdown.OccuranceSpanCode1ID;
                                data.SpanCode1FromDate = dropdown.SpanCode1FromDate;
                                data.SpanCode1ToDate = dropdown.SpanCode1ToDate;
                                data.OccuranceSpanCode2ID = dropdown.OccuranceSpanCode2ID;
                                data.SpanCode2FromDate = dropdown.SpanCode2FromDate;
                                data.OccuranceSpanCode3ID = dropdown.OccuranceSpanCode3ID;
                                data.SpanCode3FromDate = dropdown.SpanCode3FromDate;
                                data.OccuranceSpanCode4ID = dropdown.OccuranceSpanCode4ID;
                                data.SpanCode4FromDate = dropdown.SpanCode4FromDate;
                                data.SpanCode4ToDate = dropdown.SpanCode4ToDate;
                                data.OccuranceCode1ID = dropdown.OccuranceCode1ID;
                                data.OccuranceCode1Date = dropdown.OccuranceCode1Date;
                                data.OccuranceCode2ID = dropdown.OccuranceCode2ID;
                                data.OccuranceCode2Date = dropdown.OccuranceCode2Date;
                                data.OccuranceCode3ID = dropdown.OccuranceCode3ID;
                                data.OccuranceCode3Date = dropdown.OccuranceCode3Date;
                                data.OccuranceCode4ID = dropdown.OccuranceCode4ID;
                                data.OccuranceCode4Date = dropdown.OccuranceCode4Date;
                                data.OccuranceCode5ID = dropdown.OccuranceCode5ID;
                                data.OccuranceCode5Date = dropdown.OccuranceCode5Date;
                                data.OccuranceCode6ID = dropdown.OccuranceCode6ID;
                                data.OccuranceCode6Date = dropdown.OccuranceCode6Date;
                                data.OccuranceCode7ID = dropdown.OccuranceCode7ID;
                                data.OccuranceCode7Date = dropdown.OccuranceCode7Date;
                                data.OccuranceCode8ID = dropdown.OccuranceCode8ID;
                                data.OccuranceCode8Date = dropdown.OccuranceCode8Date;


                                data.UpdatedBy = UD.Email;
                                data.UpdatedDate = DateTime.Now;
                                _context.InstitutionalData.Update(data);

                            }

                        }



                        //#region Batch Linking
                        //if (item.BatchDocumentID != null && !item.PageNumber.IsNull())
                        //{

                        //    BatchDocument batch = _context.BatchDocument.Find(item.BatchDocumentID);
                        //    if (batch != null && item.DocumentBatchApplied != true)
                        //    {
                        //        BatchDocumentCharges batCharge = _context.BatchDocumentCharges.Where(m => m.BatchDocumentNoID == batch.ID && item.Charges.Any(a => a.ID == m.ChargeID)).FirstOrDefault();
                        //        if (batCharge == null)
                        //        {
                        //            batCharge = new BatchDocumentCharges();

                        //            batCharge.BatchDocumentNoID = batch.ID;
                        //            batCharge.AddedBy = UD.Email;
                        //            batCharge.AddedDate = DateTime.Now;
                        //            batCharge.DOS = item.DateOfServiceFrom;
                        //            batCharge.NoOfVisits = batCharge.NoOfVisits.ValZero() + 1;
                        //            Charge ch = new Charge();
                        //            batCharge.ChargeID = ch.ID;


                        //            if (batch.StartDate == null)
                        //            {
                        //                batch.StartDate = DateTime.Now;
                        //                _context.BatchDocument.Update(batch);
                        //            }
                        //            _context.BatchDocumentCharges.Update(batCharge);
                        //        }
                        //        else
                        //        {
                        //            batCharge.UpdatedBy = UD.Email;
                        //            batCharge.UpdatedDate = DateTime.Now;
                        //            batCharge.NoOfVisits = batCharge.NoOfVisits.ValZero() + 1;

                        //            if (batch.StartDate == null)
                        //            {
                        //                batch.StartDate = DateTime.Now;
                        //                _context.BatchDocument.Update(batch);
                        //            }
                        //            _context.BatchDocumentCharges.Update(batCharge);
                        //        }
                        //    }
                        //}
                        //else if (item.ID > 0 && item.BatchDocumentID == null && item.PageNumber.IsNull() && item.DocumentBatchApplied == true)
                        //{
                        //    Visit vst = _context.Visit.Find(item.ID);
                        //    BatchDocument batch = _context.BatchDocument.Find(vst.BatchDocumentID);

                        //    if (batch != null && batch.NoOfDemographicsEntered.ValZero() > 0)
                        //    {
                        //        BatchDocumentCharges batCharge = _context.BatchDocumentCharges.Where(m => m.BatchDocumentNoID == batch.ID && m.DOS.Date() == item.DateOfServiceFrom.Date()).FirstOrDefault();
                        //        if (batCharge.NoOfVisits.ValZero() > 0)
                        //        {
                        //            batCharge.NoOfVisits = batCharge.NoOfVisits.ValZero() - 1;
                        //            _context.BatchDocumentCharges.Update(batCharge);
                        //            item.DocumentBatchApplied = false;
                        //        }
                        //    }
                        //}
                        //#endregion


                        if (insurancePlanID == 1)
                            foreach (Charge TempCharge in item.Charges)
                            {
                                CreatePatientFollowup(item.PatientID, defaultActionID, defaultReasonID, defaultGroupID, UD.Email, TempCharge.ID);
                            }

                        objTrnScope.Complete();
                        objTrnScope.Dispose();

                        bool paymentApplied = false;
                        if (item.PatientPayments != null && item.PatientPayments.Count > 0)
                            foreach (PatientPayment patPayment in item.PatientPayments)
                            {
                                if (patPayment.Status == "C")
                                    continue;

                                if (patPayment.AllocatedAmount.Val() == 0)
                                    patPayment.RemainingAmount = patPayment.PaymentAmount;

                                if (patPayment.Type == "Copay" || patPayment.Type == "C")
                                {
                                    paymentApplied = true;

                                    item.CopayPaid = item.CopayPaid.Val() + patPayment.PaymentAmount;
                                    item.PrimaryPatientBal = item.PrimaryPatientBal.Val() - patPayment.PaymentAmount;
                                    item.PatientPaid = item.PatientPaid.Val() + patPayment.PaymentAmount;

                                    patPayment.AllocatedAmount = patPayment.PaymentAmount;
                                    patPayment.RemainingAmount = 0;

                                    if (patPayment.PaymentMethod.ToUpper() == "ADVANCE PAYMENT")
                                    {
                                        PatientPayment advPayment = _context.PatientPayment.Where(pp => pp.PatientID == item.PatientID && pp.Type == "ADVANCE PAYMENT").SingleOrDefault();
                                        advPayment.RemainingAmount = advPayment.RemainingAmount.IsNull() ? advPayment.PaymentAmount.Val() - patPayment.AllocatedAmount.Val() : advPayment.RemainingAmount.Val() - patPayment.AllocatedAmount.Val();
                                        advPayment.AdvanceAppliedOnVisits = advPayment.AdvanceAppliedOnVisits + "," + item.ID;
                                        _context.PatientPayment.Update(advPayment);

                                        patPayment.AdvancePatientPaymentID = advPayment.ID;
                                    }

                                    List<Charge> copayCharges = item.Charges.Where(c => c.Copay.Val() > 0).ToList();
                                    if (copayCharges.Count > 0)
                                    {
                                        copayCharges[0].PrimaryPatientBal = copayCharges[0].PrimaryPatientBal.Val() - patPayment.PaymentAmount;
                                        patPayment.AllocatedAmount = patPayment.PaymentAmount;
                                        patPayment.RemainingAmount = 0;

                                        copayCharges[0].PatientPaid = copayCharges[0].PatientPaid.Val() + patPayment.PaymentAmount;

                                        _context.Charge.Update(copayCharges[0]);

                                        //patBalCh.PrimaryStatus

                                        PatientPaymentCharge ppc = new PatientPaymentCharge()
                                        {
                                            AddedBy = UD.Email,
                                            AddedDate = DateTime.Now,
                                            AllocatedAmount = patPayment.PaymentAmount,
                                            ChargeID = item.Charges.ToList()[0].ID,
                                            PatientPaymentID = patPayment.ID,
                                            Status = "",
                                            VisitID = item.ID
                                        };
                                        _context.PatientPaymentCharge.Add(ppc);

                                        if (patPayment.PaymentMethod.ToUpper() == "ADVANCE PAYMENT")
                                            _context.PaymentLedger.Add(AddLedger(UD.Email, item.Charges.ToList()[0], ppc.ID, ppc.AllocatedAmount, "PAID FROM ADVANCE PAYMENT", null, null));
                                        else
                                            _context.PaymentLedger.Add(AddLedger(UD.Email, item.Charges.ToList()[0], ppc.ID, ppc.AllocatedAmount, "PATIENT PAID", null, null));
                                    }



                                }
                                else if (patPayment.Type == "O" || patPayment.Type == "Other")
                                {
                                    List<Charge> patBalCharges = item.Charges.Where(c => c.PrimaryPatientBal.Val() > 0).ToList();
                                    if (patBalCharges.Count == 0)
                                    {

                                        paymentApplied = true;

                                        item.Charges.ToList()[0].PrimaryPatientBal = item.Charges.ToList()[0].PrimaryPatientBal.Val() - patPayment.PaymentAmount;
                                        item.Charges.ToList()[0].PatientPaid = item.Charges.ToList()[0].PatientPaid.Val() + patPayment.PaymentAmount;

                                        patPayment.AllocatedAmount = patPayment.PaymentAmount;
                                        patPayment.RemainingAmount = 0;

                                        item.Charges.ToList()[0].PatientPaid = item.Charges.ToList()[0].PatientPaid.Val() + patPayment.PaymentAmount;
                                        //patBalCh.PrimaryStatus

                                        PatientPaymentCharge ppc = new PatientPaymentCharge()
                                        {
                                            AddedBy = UD.Email,
                                            AddedDate = DateTime.Now,
                                            AllocatedAmount = patPayment.PaymentAmount,
                                            ChargeID = item.Charges.ToList()[0].ID,
                                            PatientPaymentID = patPayment.ID,
                                            Status = "",
                                            VisitID = item.ID
                                        };
                                        _context.PatientPaymentCharge.Add(ppc);

                                        if (patPayment.Type.ToUpper() == "ADVANCE PAYMENT")
                                        {
                                            PatientPayment advPayment = _context.PatientPayment.Where(pp => pp.PatientID == item.PatientID && pp.Type == "ADVANCE PAYMENT").SingleOrDefault();
                                            advPayment.RemainingAmount = advPayment.RemainingAmount.IsNull() ? advPayment.PaymentAmount.Val() - patPayment.AllocatedAmount.Val() : advPayment.RemainingAmount.Val() - patPayment.AllocatedAmount.Val();
                                            advPayment.AdvanceAppliedOnVisits = advPayment.AdvanceAppliedOnVisits + "," + item.ID;
                                            _context.PatientPayment.Update(advPayment);

                                            patPayment.AdvancePatientPaymentID = advPayment.ID;
                                        }

                                        if (patPayment.PaymentMethod.ToUpper() == "ADVANCE PAYMENT")
                                            _context.PaymentLedger.Add(AddLedger(UD.Email, item.Charges.ToList()[0], ppc.ID, ppc.AllocatedAmount, "PAID FROM ADVANCE PAYMENT", null, null));
                                        else
                                            _context.PaymentLedger.Add(AddLedger(UD.Email, item.Charges.ToList()[0], ppc.ID, ppc.AllocatedAmount, "PATIENT PAID", null, null));

                                    }
                                    else
                                    {

                                        foreach (Charge patBalCh in patBalCharges)
                                        {
                                            if (patPayment.RemainingAmount.Val() == 0) continue;
                                            if (patPayment.RemainingAmount >= patBalCh.PrimaryPatientBal.Val())
                                            {
                                                paymentApplied = true;

                                                decimal primaryBal = patBalCh.PrimaryPatientBal.Val();
                                                //patPayment.PaymentAmount = patPayment.PaymentAmount.Val() - primaryBal;
                                                patPayment.AllocatedAmount = patPayment.AllocatedAmount.Val() + primaryBal;
                                                patPayment.RemainingAmount = patPayment.RemainingAmount.Val() - primaryBal;

                                                patBalCh.PrimaryPatientBal = 0;
                                                patBalCh.PatientPaid = patBalCh.PatientPaid.Val() + primaryBal;
                                                //patBalCh.PrimaryStatus

                                                PatientPaymentCharge ppc = new PatientPaymentCharge()
                                                {
                                                    AddedBy = UD.Email,
                                                    AddedDate = DateTime.Now,
                                                    AllocatedAmount = primaryBal,
                                                    ChargeID = patBalCh.ID,
                                                    PatientPaymentID = patPayment.ID,
                                                    Status = "C",
                                                    VisitID = item.ID
                                                };
                                                _context.PatientPaymentCharge.Add(ppc);

                                                if (patPayment.Type.ToUpper() == "ADVANCE PAYMENT")
                                                {
                                                    PatientPayment advPayment = _context.PatientPayment.Where(pp => pp.PatientID == item.PatientID && pp.Type == "ADVANCE PAYMENT").SingleOrDefault();
                                                    advPayment.RemainingAmount = advPayment.RemainingAmount.IsNull() ? advPayment.PaymentAmount.Val() - patPayment.AllocatedAmount.Val() : advPayment.RemainingAmount.Val() - patPayment.AllocatedAmount.Val();
                                                    advPayment.AdvanceAppliedOnVisits = advPayment.AdvanceAppliedOnVisits + "," + item.ID;
                                                    _context.PatientPayment.Update(advPayment);

                                                    patPayment.AdvancePatientPaymentID = advPayment.ID;
                                                }

                                                if (patPayment.PaymentMethod.ToUpper() == "ADVANCE PAYMENT")
                                                    _context.PaymentLedger.Add(AddLedger(UD.Email, patBalCh, ppc.ID, ppc.AllocatedAmount, "PAID FROM ADVANCE PAYMENT", null, null));
                                                else
                                                    _context.PaymentLedger.Add(AddLedger(UD.Email, patBalCh, ppc.ID, ppc.AllocatedAmount, "PATIENT PAID", null, null));
                                            }
                                            else if (patPayment.RemainingAmount < patBalCh.PrimaryPatientBal.Val())
                                            {
                                                paymentApplied = true;

                                                decimal amountToPay = patPayment.RemainingAmount.Val();
                                                patPayment.AllocatedAmount = patPayment.AllocatedAmount.Val() + amountToPay;
                                                patPayment.RemainingAmount = patPayment.RemainingAmount.Val() - amountToPay;

                                                patBalCh.PrimaryPatientBal = patBalCh.PrimaryPatientBal.Val() - amountToPay;
                                                patBalCh.PatientPaid = patBalCh.PatientPaid.Val() + amountToPay;
                                                //patBalCh.PrimaryStatus

                                                PatientPaymentCharge ppc = new PatientPaymentCharge()
                                                {
                                                    AddedBy = UD.Email,
                                                    AddedDate = DateTime.Now,
                                                    AllocatedAmount = amountToPay,
                                                    ChargeID = patBalCh.ID,
                                                    PatientPaymentID = patPayment.ID,
                                                    Status = "",
                                                    VisitID = item.ID
                                                };
                                                _context.PatientPaymentCharge.Add(ppc);

                                                if (patPayment.Type.ToUpper() == "ADVANCE PAYMENT")
                                                {
                                                    PatientPayment advPayment = _context.PatientPayment.Where(pp => pp.PatientID == item.PatientID && pp.Type == "ADVANCE PAYMENT").SingleOrDefault();
                                                    advPayment.RemainingAmount = advPayment.RemainingAmount.IsNull() ? advPayment.PaymentAmount.Val() - patPayment.AllocatedAmount.Val() : advPayment.RemainingAmount.Val() - patPayment.AllocatedAmount.Val();
                                                    advPayment.AdvanceAppliedOnVisits = advPayment.AdvanceAppliedOnVisits + "," + item.ID;
                                                    _context.PatientPayment.Update(advPayment);

                                                    patPayment.AdvancePatientPaymentID = advPayment.ID;
                                                }

                                                if (patPayment.PaymentMethod.ToUpper() == "ADVANCE PAYMENT")
                                                    _context.PaymentLedger.Add(AddLedger(UD.Email, patBalCh, ppc.ID, ppc.AllocatedAmount, "PAID FROM ADVANCE PAYMENT", null, null));
                                                else

                                                    _context.PaymentLedger.Add(AddLedger(UD.Email, patBalCh, ppc.ID, ppc.AllocatedAmount, "PATIENT PAID", null, null));
                                            }

                                            _context.Charge.Update(patBalCh);
                                        }
                                    }

                                    // if copay is applied after payment, then copay is also applied on charges
                                    if (item.Copay.Val() > 0)
                                    {
                                        item.PrimaryPatientBal = item.Charges.Sum(c => c.PrimaryPatientBal.Val());
                                        item.PatientPaid = item.Charges.Sum(c => c.PatientPaid.Val());
                                    }
                                    // if copay is not applied after payment, it means copay is not applied on charges.
                                    else
                                    {
                                        item.PrimaryPatientBal = item.Charges.Sum(c => c.PrimaryPatientBal.Val());
                                        item.PatientPaid = item.Charges.Sum(c => c.PatientPaid.Val()) + item.MovedToAdvancePayment.Val();
                                    }


                                }
                                else if (patPayment.Type == "D")
                                {
                                    List<Charge> patBalCharges = item.Charges.Where(c => c.PrimaryPatientBal.Val() > 0).ToList();
                                    if (patBalCharges.Count == 0)
                                    {
                                        paymentApplied = true;

                                        item.Charges.ToList()[0].PrimaryPatientBal = item.Charges.ToList()[0].PrimaryPatientBal.Val() - patPayment.PaymentAmount;
                                        item.Charges.ToList()[0].Discount = item.Charges.ToList()[0].Discount.Val() + patPayment.PaymentAmount;

                                        patPayment.AllocatedAmount = patPayment.PaymentAmount;
                                        patPayment.RemainingAmount = 0;

                                        //item.Charges.ToList()[0].Discount = item.Charges.ToList()[0].Discount.Val() + patPayment.PaymentAmount;
                                        //patBalCh.PrimaryStatus

                                        PatientPaymentCharge ppc = new PatientPaymentCharge()
                                        {
                                            AddedBy = UD.Email,
                                            AddedDate = DateTime.Now,
                                            AllocatedAmount = patPayment.PaymentAmount,
                                            ChargeID = item.Charges.ToList()[0].ID,
                                            PatientPaymentID = patPayment.ID,
                                            Status = "",
                                            VisitID = item.ID
                                        };
                                        _context.PatientPaymentCharge.Add(ppc);

                                        _context.PaymentLedger.Add(AddLedger(UD.Email, item.Charges.ToList()[0], ppc.ID, ppc.AllocatedAmount, "PATIENT DISCOUNT", null, null));
                                    }
                                    else
                                    {

                                        foreach (Charge patBalCh in patBalCharges)
                                        {
                                            if (patPayment.RemainingAmount.Val() == 0) continue;
                                            if (patPayment.RemainingAmount >= patBalCh.PrimaryPatientBal.Val())
                                            {
                                                paymentApplied = true;

                                                decimal primaryBal = patBalCh.PrimaryPatientBal.Val();
                                                //patPayment.PaymentAmount = patPayment.PaymentAmount.Val() - primaryBal;
                                                patPayment.AllocatedAmount = patPayment.AllocatedAmount.Val() + primaryBal;
                                                patPayment.RemainingAmount = patPayment.RemainingAmount.Val() - primaryBal;

                                                patBalCh.PrimaryPatientBal = 0;
                                                patBalCh.Discount = patBalCh.Discount.Val() + primaryBal;
                                                //patBalCh.PrimaryStatus

                                                PatientPaymentCharge ppc = new PatientPaymentCharge()
                                                {
                                                    AddedBy = UD.Email,
                                                    AddedDate = DateTime.Now,
                                                    AllocatedAmount = primaryBal,
                                                    ChargeID = patBalCh.ID,
                                                    PatientPaymentID = patPayment.ID,
                                                    Status = "C",
                                                    VisitID = item.ID
                                                };
                                                _context.PatientPaymentCharge.Add(ppc);

                                                _context.PaymentLedger.Add(AddLedger(UD.Email, patBalCh, ppc.ID, ppc.AllocatedAmount, "PATIENT DISCOUNT", null, null));
                                            }
                                            else if (patPayment.RemainingAmount < patBalCh.PrimaryPatientBal.Val())
                                            {
                                                paymentApplied = true;

                                                decimal amountToPay = patPayment.RemainingAmount.Val();
                                                patPayment.AllocatedAmount = patPayment.AllocatedAmount.Val() + amountToPay;
                                                patPayment.RemainingAmount = patPayment.RemainingAmount.Val() - amountToPay;

                                                patBalCh.PrimaryPatientBal = patBalCh.PrimaryPatientBal.Val() - amountToPay;
                                                patBalCh.Discount = patBalCh.Discount.Val() + amountToPay;
                                                //patBalCh.PrimaryStatus

                                                PatientPaymentCharge ppc = new PatientPaymentCharge()
                                                {
                                                    AddedBy = UD.Email,
                                                    AddedDate = DateTime.Now,
                                                    AllocatedAmount = amountToPay,
                                                    ChargeID = patBalCh.ID,
                                                    PatientPaymentID = patPayment.ID,
                                                    Status = "",
                                                    VisitID = item.ID
                                                };
                                                _context.PatientPaymentCharge.Add(ppc);

                                                _context.PaymentLedger.Add(AddLedger(UD.Email, patBalCh, ppc.ID, ppc.AllocatedAmount, "PATIENT DISCOUNT", null, null));

                                            }

                                            _context.Charge.Update(patBalCh);
                                        }
                                    }
                                    if (item.Copay.Val() > 0)
                                    {
                                        item.PrimaryPatientBal = item.Charges.Sum(c => c.PrimaryPatientBal.Val());
                                        item.PatientPaid = item.Charges.Sum(c => c.PatientPaid.Val());
                                    }
                                    // if copay is not applied after payment, it means copay is not applied on charges.
                                    else
                                    {
                                        item.PrimaryPatientBal = item.Charges.Sum(c => c.PrimaryPatientBal.Val());
                                        item.PatientPaid = item.Charges.Sum(c => c.PatientPaid.Val()) + item.MovedToAdvancePayment.Val();
                                    }

                                    item.Discount = item.Charges.Sum(c => c.Discount.Val());
                                }
                                patPayment.Status = "C";
                                _context.PatientPayment.Update(patPayment);
                            }


                        //if (paymentApplied == true)
                        //{
                        //    item.PrimaryPatientBal = item.Charges.Sum(c => c.PrimaryPatientBal.Val());
                        //    if (item.PatientPaid.Val() == 0)
                        //        item.PatientPaid = item.CopayPaid.Val();
                        //    else
                        //        item.PatientPaid = item.Charges.Sum(c => c.PatientPaid.Val());
                        //}

                        _context.Visit.Update(item);
                        _context.SaveChanges();



                        foreach (var charge in item.Charges)
                        {
                            var authorizationNew = (from pAuth in _context.PatientAuthorization
                                                    join cpt in _context.Cpt
                                                    on pAuth.CPTID equals cpt.ID
                                                    join pro in _context.Provider
                                                    on pAuth.ProviderID equals pro.ID
                                                    join ip in _context.InsurancePlan on pAuth.InsurancePlanID equals ip.ID
                                                    join pp in _context.PatientPlan on ip.ID equals pp.InsurancePlanID
                                                    where pp.ID == item.PrimaryPatientPlanID
                                                    && pAuth.PatientID == item.PatientID
                                                    && pAuth.AuthorizationNumber == item.AuthorizationNum
                                                    && pAuth.ProviderID == item.ProviderID
                                                    && pAuth.CPTID == charge.CPTID
                                                    &&
                                                    ((pAuth.VisitsAllowed.ValZero() - pAuth.VisitsUsed.ValZero()) > 0)
                                                    select pAuth
                            ).FirstOrDefault();

                            //get old charge data from ChargeList in which we store old chage record . for inc or dec
                            var oldVisitCharge = (from c in ChargeList
                                                  where c.ChargeID == charge.ID
                                                  select c).FirstOrDefault();
                            if (oldVisitCharge != null)
                            {
                                //Old Charge ha
                                // Compare Old Charge data with PatientAuthorization table . kia pehly wala charge ka data auth ma ha ya nahi
                                var oldVisitChargeDataAuth = (from pAuth in _context.PatientAuthorization
                                                              join pro in _context.Provider
                                                              on pAuth.ProviderID equals pro.ID
                                                              join cpt in _context.Cpt
                                                              on pAuth.CPTID equals cpt.ID

                                                              join pPlan in _context.PatientPlan
                                                              on pAuth.PatientPlanID equals pPlan.InsurancePlanID

                                                              where oldVisitCharge.patientID == pAuth.PatientID
                                                              && pAuth.PatientID == item.PatientID
                                                              && pAuth.CPTID == oldVisitCharge.CPTID
                                                              && (pPlan.ID == oldVisitCharge.PrimaryPatientPlanID)
                                                              && pAuth.CPTID == oldVisitCharge.CPTID
                                                              && pAuth.AuthorizationNumber == oldVisitCharge.AuthorizationNum
                                                              //&& pAuth.PatientPlanID == oldVisitCharge.PrimaryPatientPlanID
                                                              && pAuth.ProviderID == oldVisitCharge.ProviderID
                                                              select pAuth).FirstOrDefault();
                                //ab jo chage ma new data data aya ha us ko hmne dekha k PatientAuthorization ma check kia ha upr. agr data nahi ha to if ma jao nahi to else
                                if (authorizationNew == null)
                                {
                                    // agr ha to matlab pehly increment hoa tha .  pir ye check krna ha k 

                                    if (oldVisitChargeDataAuth == null)
                                    {
                                        //Nothing
                                    }
                                    else
                                    {
                                        // -1
                                        oldVisitChargeDataAuth.VisitsUsed = oldVisitChargeDataAuth.VisitsUsed.ValZero() - 1;
                                        oldVisitChargeDataAuth.UpdatedBy = UD.Email;
                                        oldVisitChargeDataAuth.UpdatedDate = DateTime.Now;
                                        _context.PatientAuthorization.Update(oldVisitChargeDataAuth);

                                        var q = (from u in _context.PatientAuthorizationUsed where oldVisitChargeDataAuth.ID == u.PatientAuthID && item.ID == u.VisitID select u).FirstOrDefault();
                                        if (q == null)
                                        {
                                            PatientAuthorizationUsed pAuthUsed = new PatientAuthorizationUsed
                                            {
                                                PatientAuthID = oldVisitChargeDataAuth.ID,
                                                VisitID = item.ID,
                                                AddedBy = UD.Email,
                                                AddedDate = DateTime.Now
                                            };
                                            _context.PatientAuthorizationUsed.Add(pAuthUsed);
                                        }

                                    }
                                }
                                else
                                {
                                    // According to new charge data , Record is available in PatientAuth but not in Old Record
                                    if (oldVisitChargeDataAuth == null)
                                    {
                                        //+1
                                        authorizationNew.VisitsUsed = authorizationNew.VisitsUsed.ValZero() + 1;
                                        authorizationNew.UpdatedBy = UD.Email;
                                        authorizationNew.UpdatedDate = DateTime.Now;
                                        _context.PatientAuthorization.Update(authorizationNew);

                                        var checkInPatienAuthUsed = (from u in _context.PatientAuthorizationUsed where u.VisitID == item.ID select u).FirstOrDefault();
                                        if (checkInPatienAuthUsed == null)
                                        {
                                            PatientAuthorizationUsed pAuthUsed = new PatientAuthorizationUsed
                                            {
                                                PatientAuthID = authorizationNew.ID,
                                                VisitID = item.ID,
                                                AddedBy = UD.Email,
                                                AddedDate = DateTime.Now
                                            };
                                            _context.PatientAuthorizationUsed.Add(pAuthUsed);
                                        }

                                    }
                                    else
                                    {
                                        //Nothing
                                    }
                                }
                            }
                            else
                            {
                                //New Charge ha or  Auth ma b ha to increment kr do
                                if (authorizationNew != null)
                                {
                                    //+1
                                    authorizationNew.VisitsUsed = authorizationNew.VisitsUsed.ValZero() + 1;
                                    authorizationNew.UpdatedBy = UD.Email;
                                    authorizationNew.UpdatedDate = DateTime.Now;
                                    _context.PatientAuthorization.Update(authorizationNew);

                                    var q = (from u in _context.PatientAuthorizationUsed where authorizationNew.ID == u.PatientAuthID && item.ID == u.VisitID select u).FirstOrDefault();
                                    if (q == null)
                                    {
                                        PatientAuthorizationUsed pAuthUsed = new PatientAuthorizationUsed
                                        {
                                            PatientAuthID = authorizationNew.ID,
                                            VisitID = item.ID,
                                            AddedBy = UD.Email,
                                            AddedDate = DateTime.Now
                                        };
                                        _context.PatientAuthorizationUsed.Add(pAuthUsed);
                                    }
                                }
                                //New Charge ha or  Auth ma b nahi ha to kuch b nahi krna.
                                else
                                {
                                    //Nothing
                                }
                            }

                        }
                        await _context.SaveChangesAsync();
                        ////Institutional Data New Visit New Institutional Data Case



                    }
                    catch (Exception ex)
                    {
                        System.IO.File.WriteAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Logs", "Visit.txt"), ex.ToString());
                        throw ex;

                    }
                    finally
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
             this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);

            return Ok(item);
        }
        [HttpPost]
        [Route("GenerateItemizedStatement")]
        public async Task<IActionResult> GenerateItemizedStatement(ListModel visitIds, string patientId)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (visitIds == null || visitIds.Ids.Length == 0)
            {
                return BadRequest("Please select Visit(s).");
            }
            Settings settings = _context.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefault();
            string resourcesPath = System.IO.Path.Combine(_context.env.ContentRootPath, "Resources/statement/");
            string DirectoryPath = System.IO.Path.Combine("\\\\", settings.DocumentServerURL,
                        settings.DocumentServerDirectory, UD.ClientID.ToString(), "ItemizedStatement");
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }
            var datetime = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
            string PDFfileName = "", HTMLfileName = "";
            Practice practice = _context.Practice.Find(UD.PracticeID);
            string[] arrPatientIds = patientId.Split(",");
            List<long> PatientIdsConsumed = new List<long>();
            string[] files = new string[arrPatientIds.Length];
            for (int i = 0; i < arrPatientIds.Length; i++)
            {
                if (!PatientIdsConsumed.Contains(long.Parse(arrPatientIds[i])))
                    PatientIdsConsumed.Add(long.Parse(arrPatientIds[i]));
                else
                    continue; // patientID duplicate
                long patId = long.Parse(arrPatientIds[i]);
                PDFfileName = System.IO.Path.Combine(DirectoryPath, ("P-" + patId + "_" + datetime + ".pdf"));
                HTMLfileName = System.IO.Path.Combine(DirectoryPath, "P-" + patId + "_" + datetime + ".html");
                var header = (from p in _context.Practice
                              join pt in _context.Patient on p.ID equals pt.PracticeID
                              where p.ID == UD.PracticeID && pt.ID == patId
                              select new
                              {
                                  practiceName = p.Name,
                                  practiceAddress = p.Address1,
                                  practiceCity = p.City,
                                  practiceState = p.State,
                                  practiceZipCode = p.ZipCode,
                                  TaxID = p.TaxID,
                                  practicePhoneNum = p.OfficePhoneNum,//+ p.PhoneNumExt!=null ? " Ext: " +p.PhoneNumExt :"" ,
                                  patientID = pt.ID,
                                  patientAccountNum = pt.AccountNum,
                                  patient = pt.LastName + ", " + pt.FirstName,
                                  patientAddress = pt.Address1,
                                  patientAddress2 = pt.Address2,
                                  patientCity = pt.City,
                                  patientState = pt.State,
                                  patientZipCode = pt.ZipCode,
                                  PhoneNumber = pt.PhoneNumber
                              }
                            ).ToList();
                var InsurancesData = (from pt in _context.Patient
                                      join pp in _context.PatientPlan on pt.ID equals pp.PatientID into Table4
                                      from t4 in Table4.DefaultIfEmpty()
                                      join ip in _context.InsurancePlan on t4.InsurancePlanID equals ip.ID into Table5
                                      from t5 in Table5.DefaultIfEmpty()
                                      join ins in _context.Insurance on t5.InsuranceID equals ins.ID into Table6
                                      from insT in Table6.DefaultIfEmpty()
                                      where pt.PracticeID == UD.PracticeID && pt.ID == patId
                                      select new
                                      {
                                          PlanName = t5.PlanName,
                                          InsuranceAddress1 = insT.Address1,
                                          InsuranceCity = insT.City,
                                          InsuranceType = t4.Coverage,
                                          InsuranceState = insT.State,
                                          InsuranceZipCode = insT.ZipCode,
                                          InsurancePhoneNum = insT.OfficePhoneNum,
                                          Adjuster = t5.PayerID,
                                          GroupName = t4.GroupName,
                                          SubscriberId = t4.SubscriberId
                                      }
                          ).ToList();
                string data = "";
                decimal? total = 0;
                var details = (from v in _context.Visit
                               join vC in _context.Charge on v.ID equals vC.VisitID
                               join cpt in _context.Cpt on vC.CPTID equals cpt.ID
                               join pr in _context.Provider on v.ProviderID equals pr.ID into Table4
                               from prT in Table4.DefaultIfEmpty()
                               where v.PracticeID == UD.PracticeID && v.PatientID == patId
                               && visitIds.Ids.Contains(v.ID)
                               select new
                               {
                                   DateOfServiceFrom = vC.DateOfServiceFrom.ToString("MM/dd/yyyy"),
                                   DateOfServiceTo = vC.DateOfServiceTo.HasValue ? vC.DateOfServiceTo.Value.ToString("MM/dd/yyyy") : "",
                                   cpt = cpt.CPTCode,
                                   visitID = v.ID,
                                   providerName = prT.LastName + ", " + prT.FirstName,
                                   cptDescription = cpt.ShortDescription,
                                   chargeID = vC.ID,
                                   totalAmount = vC.TotalAmount,
                                   WriteOff = (vC.PrimaryWriteOff.Val()
                                  + vC.SecondaryWriteOff.Val()
                                  + vC.TertiaryWriteOff.Val()),
                                   PaidAmount = (vC.PrimaryPaid.Val() + vC.SecondaryPaid.Val() + vC.TertiaryPaid.Val() + vC.PatientPaid.Val()),
                                   units = vC.Units,
                                   PatientPaid = vC.PatientPaid,
                                   balance = (vC.PrimaryBal.Val()
                                  + vC.SecondaryBal.Val()
                                  + vC.TertiaryBal.Val() + vC.PrimaryPatientBal.Val() + vC.SecondaryPatientBal.Val() + vC.TertiaryPatientBal.Val()),
                               }
                                    ).ToList();
                if (details != null)
                {
                    data = @" <table style='width:95%; margin-top: 10px;border:1px solid #000;border-collapse: collapse;'>
                <thead>
                    <tr> 
                        <td class='thick-line-b'><strong>Visit #</strong></td>
        				<td class='text-center thick-line-b'><strong>Service Date</strong></td>
        				<td class='text-center thick-line-b'><strong>Provider Name</strong></td>
        				<td class='text-right thick-line-b'><strong>Procedure Description</strong></td>
        				<td class='text-right thick-line-b'><strong>Code</strong></td>
        				<td class='text-right thick-line-b'><strong>Charges</strong></td>
        				<td class='text-right thick-line-b'><strong>Adjust</strong></td>
        				<td class='text-right thick-line-b'><strong>Payments</strong></td>
        				<td class='text-right thick-line-b'><strong>Balance</strong></td>
                         
                    </tr>
                </thead>
                <tbody>";
                    string dos = "";
                    total = 0;
                    for (int k = 0; k < details.Count; k++)
                    {
                        dos = details[k].DateOfServiceFrom;
                        total = details[k].balance + total;
                        if (!ExtensionMethods.IsNull(details[k].DateOfServiceTo) &&
                            details[k].DateOfServiceTo != details[k].DateOfServiceFrom)
                            dos += "-" + details[k].DateOfServiceTo;
                        data += @"<tr> 
                                    <td>" + details[k].visitID + @"</td>
    								<td class='text-center'>" + dos + @"</td>
    								<td class='text-center'>" + details[k].providerName + @"</td>
    								  <td class='text-right'>" + details[k].cptDescription + @"</td>
    								<td class='text-right'>" + details[k].cpt + @"</td>
    								<td   style='text - align: center!important;'>" + details[k].totalAmount + @"</td>
    								<td   style='text - align: center!important;'>" + details[k].WriteOff + @"</td>
    								<td  style='text - align: center!important;'>" + details[k].PaidAmount + @"</td>
    								<td   style='text - align: center!important;'>" + details[k].balance + @"</td>
                    </tr>";
                    }
                    data += @"<tr> 
                                    <tr>
	<td class='thick-line'></td>
	<td class='thick-line'></td>
	<td class='thick-line'></td>
	<td class='thick-line'></td>
	<td class='thick-line'></td>
	<td class='thick-line'></td>
	<td class='thick-line'></td>
	<td class='no-line text-center'><strong>Balance Due</strong></td>
	<td class='no-line text-right'>" + total + @"</td>  
                    </tr>";
                    data += @" </tbody></table>";
                }
                string statementHTML = System.IO.File.ReadAllText(System.IO.Path.Combine(_context.env.ContentRootPath, "Resources/statement", "ItemizedStatement.html"));
                statementHTML = statementHTML.Replace("$PRACTICE_NAME", header[0].practiceName);
                statementHTML = statementHTML.Replace("$PRACTICE_Address", header[0].practiceAddress);
                statementHTML = statementHTML.Replace("$PRACTICE_City", header[0].practiceCity);
                statementHTML = statementHTML.Replace("$PRACTICE_State", header[0].practiceState);
                statementHTML = statementHTML.Replace("$PRACTICE_Zip", header[0].practiceZipCode);
                statementHTML = statementHTML.Replace("$PRACTICE_tax", header[0].TaxID);
                statementHTML = statementHTML.Replace("$PATIENT_Name", header[0].patient);
                statementHTML = statementHTML.Replace("$PATIENT_Address", header[0].patientAddress);
                statementHTML = statementHTML.Replace("$PATIENT_City", header[0].patientCity);
                statementHTML = statementHTML.Replace("$PATIENT_State", header[0].patientState);
                statementHTML = statementHTML.Replace("$PATIENT_Zip", header[0].patientZipCode);
                statementHTML = statementHTML.Replace("$PATIENT_Phone", header[0].PhoneNumber);
                statementHTML = statementHTML.Replace("$PATIENT_Id", header[0].patientAccountNum);
                statementHTML = statementHTML.Replace("$STATEMENT_Date", DateTime.Now.Date.ToString("MM/dd/yyyy"));
                statementHTML = statementHTML.Replace("$STATEMENT_DATA", data);
                var InsurancesDataPrimary = InsurancesData.Where(w => w.InsuranceType.Trim().Equals("P")).FirstOrDefault();
                if (InsurancesDataPrimary != null && InsurancesDataPrimary.PlanName != "")
                {
                    statementHTML = statementHTML.Replace("$InsuranceP_Name", InsurancesDataPrimary.PlanName);
                    statementHTML = statementHTML.Replace("$InsuranceP_Adjuster", InsurancesDataPrimary.Adjuster);
                    statementHTML = statementHTML.Replace("$InsuranceP_GroupNumber", InsurancesDataPrimary.GroupName);
                    statementHTML = statementHTML.Replace("$InsuranceP_Address", InsurancesDataPrimary.InsuranceAddress1);
                    statementHTML = statementHTML.Replace("$InsuranceP_City", InsurancesDataPrimary.InsuranceCity);
                    statementHTML = statementHTML.Replace("$InsuranceP_State", InsurancesDataPrimary.InsuranceState);
                    statementHTML = statementHTML.Replace("$InsuranceP_Zip", InsurancesDataPrimary.InsuranceZipCode);
                    statementHTML = statementHTML.Replace("$InsuranceP_SubscriberId", InsurancesDataPrimary.SubscriberId);
                }
                else
                {
                    statementHTML = statementHTML.Replace("$InsuranceP_Name", "");
                    statementHTML = statementHTML.Replace("$InsuranceP_Adjuster", "");
                    statementHTML = statementHTML.Replace("$InsuranceP_GroupNumber", "");
                    statementHTML = statementHTML.Replace("$InsuranceP_Address", "");
                    statementHTML = statementHTML.Replace("$InsuranceP_City", "");
                    statementHTML = statementHTML.Replace("$InsuranceP_State", "");
                    statementHTML = statementHTML.Replace("$InsuranceP_Zip", "");
                    statementHTML = statementHTML.Replace("$InsuranceP_SubscriberId", "");
                }
                var InsurancesDataSecondary = InsurancesData.Where(w => w.InsuranceType.Trim().Equals("S")).FirstOrDefault();
                if (InsurancesDataSecondary != null && InsurancesDataSecondary.PlanName != "")
                {
                    statementHTML = statementHTML.Replace("$InsuranceS_Name", InsurancesDataSecondary.PlanName);
                    statementHTML = statementHTML.Replace("$InsuranceS_Adjuster", InsurancesDataSecondary.Adjuster);
                    statementHTML = statementHTML.Replace("$InsuranceS_GroupNumber", InsurancesDataSecondary.GroupName);
                    statementHTML = statementHTML.Replace("$InsuranceS_Address", InsurancesDataSecondary.InsuranceAddress1);
                    statementHTML = statementHTML.Replace("$InsuranceS_City", InsurancesDataSecondary.InsuranceCity);
                    statementHTML = statementHTML.Replace("$InsuranceS_State", InsurancesDataSecondary.InsuranceState);
                    statementHTML = statementHTML.Replace("$InsuranceS_Zip", InsurancesDataSecondary.InsuranceZipCode);
                    statementHTML = statementHTML.Replace("$InsuranceS_SubscriberId", InsurancesDataSecondary.SubscriberId);
                }
                else
                {
                    statementHTML = statementHTML.Replace("$InsuranceS_Name", "");
                    statementHTML = statementHTML.Replace("$InsuranceS_Adjuster", "");
                    statementHTML = statementHTML.Replace("$InsuranceS_GroupNumber", "");
                    statementHTML = statementHTML.Replace("$InsuranceS_Address", "");
                    statementHTML = statementHTML.Replace("$InsuranceS_City", "");
                    statementHTML = statementHTML.Replace("$InsuranceS_State", "");
                    statementHTML = statementHTML.Replace("$InsuranceS_Zip", "");
                    statementHTML = statementHTML.Replace("$InsuranceS_SubscriberId", "");
                }
                System.IO.File.WriteAllText(HTMLfileName, statementHTML);
                PdfWriter writer = new PdfWriter(PDFfileName);
                PdfDocument pdfDocument = new PdfDocument(writer);
                pdfDocument.SetDefaultPageSize(PageSize.A4);
                string cssPath = System.IO.Path.Combine(_context.env.ContentRootPath, "Resources/statement", "bootstrap.min.css");
                ConverterProperties converterProperties = new ConverterProperties();
                converterProperties.SetBaseUri(cssPath);
                HtmlConverter.ConvertToPdf(statementHTML, pdfDocument, converterProperties);
                PageSize pg = pdfDocument.GetDefaultPageSize();
                files[i] = PDFfileName;
            }
            string ZIPfileName = System.IO.Path.Combine(DirectoryPath, UD.Email + "_" + datetime + ".zip");
            var zip = ZipFile.Open(ZIPfileName, ZipArchiveMode.Create);
            foreach (var file in files)
            {
                if (file != null)
                    zip.CreateEntryFromFile(file, System.IO.Path.GetFileName(file), CompressionLevel.Optimal);
            }
            zip.Dispose();
            Byte[] fileBytes = System.IO.File.ReadAllBytes(ZIPfileName);
            if (fileBytes == null)
                return NotFound();
            var stream = new MemoryStream(fileBytes); //saves it into a stream
            stream.Position = 0;
            int index = ZIPfileName.LastIndexOf("\\");
            string filename = ZIPfileName.Substring(index + 1, ZIPfileName.Length - index - 1);
            return File(stream, "application/octec-stream", filename);
        }

        private PaymentLedger AddLedger(string Email, Charge Charge, long? PatientPaymentChargeID, decimal? AllocatedAmount, string LedgerType, string LedgerBy, long? PatientPlanID)
        {
            PaymentLedger ledger = new PaymentLedger()
            {
                AddedBy = Email,
                AddedDate = DateTime.Now,
                AdjustmentCodeID = null,
                ChargeID = Charge.ID,
                LedgerBy = LedgerBy,
                LedgerDate = DateTime.Now,
                LedgerType = LedgerType,
                //LedgerType = "PATIENT PAID",
                LedgerDescription = "",
                PaymentChargeID = null,
                PatientPaymentChargeID = PatientPaymentChargeID,
                // PatientPlanID = Charge.PrimaryPatientPlanID.Value,
                PatientPlanID = PatientPlanID,
                VisitID = Charge.VisitID.Value,
                Amount = AllocatedAmount
            };
            return ledger;

        }
        [Route("DeleteVisit/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteVisit(long id)
        {
            var transactionOption = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted };
            using (var objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption, TransactionScopeAsyncFlowOption.Enabled))
            {
                var visit = _context.Visit.Where(v => v.ID == id).FirstOrDefault();
                if (visit == null)
                {
                    return NotFound();
                }

                List<Notes> notes = _context.Notes.Where(p => p.VisitID == id).ToList<Notes>();
                foreach (Notes n in notes)
                {
                    _context.Notes.Remove(n);
                }
                List<VisitAudit> vAudit = _context.VisitAudit.Where(p => p.VisitID == id).ToList<VisitAudit>();
                foreach (VisitAudit va in vAudit)
                {
                    _context.VisitAudit.Remove(va);
                }
                List<Charge> charges = _context.Charge.Where(c => c.VisitID == id).ToList<Charge>();
                foreach (Charge ch in charges)
                {
                    List<ChargeAudit> cAudit = _context.ChargeAudit.Where(c => c.ChargeID == ch.ID).ToList<ChargeAudit>();
                    foreach (ChargeAudit ca in cAudit)
                    {
                        _context.ChargeAudit.Remove(ca);
                    }
                    _context.Charge.Remove(ch);
                }

                _context.Visit.Remove(visit);
                await _context.SaveChangesAsync();
                objTrnScope.Complete();
            }
            return Ok();


        }

    





        //List<PlanFollowup> FollowUp = (from pf in _context.PlanFollowUp
        //                               where pf.VisitID == VisitID
        //                               select //Inner Values For Select
        //new PlanFollowup
        //{
        //    ID = pf.ID,
        //    VisitID = pf.VisitID,
        //    GroupID = pf.GroupID,
        //    ReasonID = pf.ReasonID,
        //    ActionID = pf.ActionID,
        //    RemitCode = pf.RemitCode,
        //    AdjustmentCodeID = pf.AdjustmentCodeID,
        //    VisitStatusID = pf.VisitStatusID,
        //    Notes = pf.Notes,
        //    Age = System.DateTime.Now.Subtract(pf.AddedDate.Date()).Days.ToString(),
        //    PaymentVisitID = pf.PaymentVisitID,
        //    TickleDate = pf.TickleDate,
        //    AddedBy = pf.AddedBy,
        //    AddedDate = pf.AddedDate,
        //    UpdatedBy = pf.UpdatedBy,
        //    UpdatedDate = pf.UpdatedDate,
        //}).ToList<PlanFollowup>();
        //return FollowUp;




        [Route("FindFollowUpByVisitID/{VisitID}")]

        [HttpGet("{VisitID}")]
        public async Task<ActionResult<PlanFollowup>> FindFollowupByVisitID(long VisitID)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
           );


            PlanFollowup FollowUp = _context.PlanFollowUp.Where(c => c.VisitID == VisitID).SingleOrDefault();

            if (FollowUp == null)
            {
                return BadRequest("FollowUp for Visit ID : " + VisitID + " didnt Exists");
            }
            else
            {
                Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
                this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);
                return FollowUp;
            }

        }
        [Route("FindAudit/{VisitID}")]
        [HttpGet("{VisitID}")]
        public List<VisitAudit> FindAudit(long VisitID)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
           );

            List<VisitAudit> data = (from pAudit in _context.VisitAudit
                                     where pAudit.VisitID == VisitID
                                     orderby pAudit.AddedDate descending
                                     select new VisitAudit()
                                     {
                                         ID = pAudit.ID,
                                         VisitID = pAudit.VisitID,
                                         TransactionID = pAudit.TransactionID,
                                         ColumnName = pAudit.ColumnName,
                                         CurrentValue = pAudit.CurrentValue,
                                         NewValue = pAudit.NewValue,
                                         CurrentValueID = pAudit.CurrentValueID,
                                         NewValueID = pAudit.NewValueID,
                                         HostName = pAudit.HostName,
                                         AddedBy = pAudit.AddedBy,
                                         AddedDate = pAudit.AddedDate,
                                     }).ToList<VisitAudit>();
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);

            return data;
        }

        //private void CreatePatientFollowup(long PatientID, long? DefaultActionID, long? DefaultReasonID,
        //long? DefaultGroupID, string Email)
        //{
        //    PatientFollowUp followup = null;
        //    PatientFollowUpCharge followupcharge = null;
        //    followup = _context.PatientFollowUp.Where(v => v.PatientID == PatientID).FirstOrDefault();

        //    if (followup == null)
        //    {
        //        followup.PatientID = PatientID;
        //        followup.ReasonID = DefaultReasonID;
        //        followup.ActionID = DefaultGroupID;
        //        followup.GroupID = DefaultGroupID;
        //        followup.AddedBy = Email;
        //        followup.AddedDate = DateTime.Now;
        //        followup.Resolved = false;
        //        _context.PatientFollowUp.Add(followup);

        //        followupcharge = _context.PatientFollowUpCharge.Where(v => v.PatientFollowUpID == followup.ID).FirstOrDefault();

        //        if (followupcharge == null)
        //        {
        //            followupcharge.PatientFollowUpID = followup.ID;
        //            followupcharge.ReasonID = followup.ReasonID;
        //            followupcharge.ActionID = followup.ActionID;
        //            followupcharge.GroupID = followup.GroupID;

        //            followupcharge.AddedBy = followup.AddedBy;
        //            followupcharge.AddedDate = DateTime.Now;
        //            _context.PatientFollowUpCharge.Add(followupcharge);
        //        }




        //    }
        //}


        [Route("MarkAsSubmitted/{VisitID}")]
        [HttpGet("{VisitID}")]
        public async Task<ActionResult<Visit>> MarkAsSubmitted(long VisitID)
        {
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            string temp = User.Claims.FirstOrDefault(x => x.Type.Equals("Temp", StringComparison.InvariantCultureIgnoreCase)).Value;
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);

            string newConnection = CommonUtil.GetConnectionString(PracticeId, temp);

            Visit visit = new Visit();
            visit = _context.Visit.Where(v => v.ID == VisitID && v.IsSubmitted == false || v.IsSubmitted.Equals("")).FirstOrDefault();
            if (visit == null)
            {
                // return BadRequest("VisitNot Found."); 

                visit = _context.Visit.Where(v => v.ID == VisitID && v.IsSubmitted == true).FirstOrDefault();
                if ((visit.SecondaryStatus == "N" || visit.SecondaryStatus == "RS") && visit.SecondaryPatientPlanID != null && visit.SecondaryBilledAmount.Val() > 0)
                {
                    visit.SecondaryStatus = "S";
                    visit.UpdatedBy = Email;
                    visit.RejectionReason = ""; 
                    visit.UpdatedDate = DateTime.Now;
                    visit.SubmittedDate = DateTime.Now;
                    _context.Visit.Update(visit);


                    List<Charge> charges = _context.Charge.Where(c => c.VisitID == VisitID && c.IsSubmitted == true && (c.SecondaryStatus == "N" || c.SecondaryStatus == "RS") && c.SecondaryPatientPlanID != null && c.SecondaryBilledAmount.Val() > 0).ToList<Charge>();

                    if (charges != null)
                    {
                        foreach (Charge cha in charges)
                        {
                            cha.SecondaryStatus = "S";
                            cha.IsSubmitted = true;
                            cha.UpdatedBy = Email;
                            cha.UpdatedDate = DateTime.Now;
                            cha.SubmittetdDate = DateTime.Now;
                            _context.Charge.Update(cha);
                        }
                    }

                    Notes notes = new Notes();
                    notes.VisitID = visit.ID;
                    notes.Note = "Mark As Submitted Feature was used for Secondary Submission.";
                    notes.AddedBy = Email;
                    notes.AddedDate = DateTime.Now;
                    notes.NotesDate = DateTime.Now;
                    _context.Notes.Add(notes);
                    await _context.SaveChangesAsync();

                }
            }

            else
            {
                visit.IsSubmitted = true;
                visit.PrimaryStatus = "S";
                visit.UpdatedBy = Email;
                visit.UpdatedDate = DateTime.Now;
                visit.SubmittedDate = DateTime.Now;
                _context.Visit.Update(visit);
                List<Charge> charges = _context.Charge.Where(c => c.VisitID == VisitID && c.IsSubmitted == false || c.IsSubmitted.Equals("")).ToList<Charge>();

                if (charges != null)
                {
                    foreach (Charge cha in charges)
                    {
                        cha.PrimaryStatus = "S";
                        cha.IsSubmitted = true;
                        cha.UpdatedBy = Email;
                        cha.UpdatedDate = DateTime.Now;
                        cha.SubmittetdDate = DateTime.Now;
                        _context.Charge.Update(cha);
                    }
                }

                Notes notes = new Notes();
                notes.VisitID = visit.ID;
                notes.Note = "Mark As Submitted Feature was used for Submission.";
                notes.AddedBy = Email;
                notes.AddedDate = DateTime.Now;
                notes.NotesDate = DateTime.Now;
                _context.Notes.Add(notes);
                await _context.SaveChangesAsync();
            }




            visit.Charges = _context.Charge.Where(m => m.VisitID == visit.ID).ToList<Charge>();
            return visit;
        }


        [Route("TransferToPatient/{VisitID}")]
        [HttpGet("{VisitID}")]
        public async Task<ActionResult<Visit>> TransferToPatient(long VisitID)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
           );

            Visit visit = await _context.Visit.FindAsync(VisitID);
            if (visit == null)
                return BadRequest("Visit Not Found");
            if (visit.PrimaryBal.Val() + visit.SecondaryBal.Val() + visit.TertiaryBal.Val() == 0)
                return BadRequest("Plan Balance is already zero");
            List<Charge> charges = _context.Charge.Where(c => c.VisitID == VisitID).ToList();
            foreach (Charge c in charges)
            {
                decimal? planBal = null;
                if (c.PrimaryBal.Val() > 0)
                {
                    planBal = c.PrimaryBal.Val();
                    c.PrimaryPatientBal = c.PrimaryPatientBal.Val() + planBal;
                    c.OtherPatResp = c.OtherPatResp.Val() + planBal;
                    c.PrimaryBal = c.PrimaryBal.Val() - planBal;
                    c.PrimaryStatus = "PTPT";           // plan to patient transfer
                    _context.Charge.Update(c);
                }
                else if (c.SecondaryBal.Val() > 0)
                {
                    planBal = c.SecondaryBal.Val();
                    c.PrimaryPatientBal = c.PrimaryPatientBal.Val() + planBal;
                    c.OtherPatResp = c.OtherPatResp.Val() + planBal;
                    c.SecondaryBal = c.SecondaryBal.Val() - planBal;
                    c.SecondaryBilledAmount = c.SecondaryBilledAmount.Val() - planBal.Val();
                    c.PrimaryStatus = "PTPT";           // plan to patient transfer
                    _context.Charge.Update(c);
                }
                PaymentLedger ledger = new PaymentLedger()
                {
                    AddedBy = UD.Email,
                    AddedDate = DateTime.Now,
                    AdjustmentCodeID = null,
                    ChargeID = c.ID,
                    LedgerBy = "",
                    LedgerDate = DateTime.Now,
                    LedgerType = "PLAN TO PATIENT TRANSFER",
                    LedgerDescription = "PLAN TO PATIENT TRANSFER",
                    PaymentChargeID = null,
                    PatientPaymentChargeID = null,
                    PatientPlanID = c.PrimaryPatientPlanID,
                    VisitID = c.VisitID.Value,
                    Amount = planBal
                };
                _context.PaymentLedger.Add(ledger);
                long? defaultActionID = _context.Action.Where(a => a.Name == "SYSTEM").FirstOrDefault()?.ID;
                long? defaultGroupID = _context.Group.Where(a => a.Name == "NEW").FirstOrDefault()?.ID;
                long? defaultReasonID = _context.Reason.Where(a => a.Name == "SYSTEM").FirstOrDefault()?.ID;
                CreatePatientFollowup(c.PatientID, defaultActionID, defaultReasonID, defaultGroupID, UD.Email, c.ID);
            }
            var visitSum = (from c in _context.Charge
                            where c.VisitID == visit.ID
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
                                SecondaryTransferred = v.Sum(x => x.SecondaryTransferred.Val()),
                                SecondaryAllowed = v.Sum(x => x.SecondaryAllowed.Val()),
                                SecondaryWriteOff = v.Sum(x => x.SecondaryWriteOff.Val()),
                                SecondaryPaid = v.Sum(x => x.SecondaryPaid.Val()),
                                TertiaryBilledAmount = v.Sum(x => x.TertiaryBilledAmount.Val()),
                                TertiaryBal = v.Sum(x => x.TertiaryBal.Val()),
                                SecondaryPatientBal = v.Sum(x => x.SecondaryPatientBal.Val()),
                            }).FirstOrDefault();
            visit.PrimaryBal = visitSum.PrimaryBal;
            visit.PrimaryPatientBal = visitSum.PrimaryPatBal;
            visit.OtherPatResp = visitSum.OtherPatResp;
            visit.PrimaryStatus = "PTPT";
            visit.SecondaryBal = visitSum.SecondaryBal;
            visit.SecondaryBilledAmount = visitSum.SecondaryBilledAmount;
            _context.Visit.Update(visit);
            _context.SaveChanges();
            visit.Charges = _context.Charge.Where(m => m.VisitID == visit.ID).ToList<Charge>();
            visit.PatientPayments = _context.PatientPayment.Where(m => m.VisitID == visit.ID).ToList<PatientPayment>();
            visit.Note = _context.Notes.Where(m => m.VisitID == visit.ID).ToList<Notes>();

            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);



            return visit;
        }
        private void CreatePatientFollowup(long PatientID, long? DefaultActionID, long? DefaultReasonID,
                long? DefaultGroupID, string Email, long ChargeID)
        {
            PatientFollowUp followup = null;
            PatientFollowUpCharge followupcharge = null;
            followup = _context.PatientFollowUp.Where(v => v.PatientID == PatientID).FirstOrDefault();

            if (followup == null)
            {
                followup = new PatientFollowUp();
                followup.PatientID = PatientID;
                followup.ReasonID = DefaultReasonID;
                followup.ActionID = DefaultGroupID;
                followup.GroupID = DefaultGroupID;
                followup.AddedBy = Email;
                followup.AddedDate = DateTime.Now;
                followup.Resolved = false;

                _context.PatientFollowUp.Add(followup);

                followupcharge = new PatientFollowUpCharge();

                followupcharge.PatientFollowUpID = followup.ID;
                followupcharge.ReasonID = followup.ReasonID;
                followupcharge.ActionID = followup.ActionID;
                followupcharge.GroupID = followup.GroupID;
                followupcharge.ChargeID = ChargeID;
                followupcharge.AddedBy = followup.AddedBy;
                followupcharge.AddedDate = DateTime.Now;

                _context.PatientFollowUpCharge.Add(followupcharge);

            }
            //else
            //{
            //    followupcharge = _context.PatientFollowUpCharge.Where(v => v.PatientFollowUpID == followup.ID && v.ChargeID == ChargeID).FirstOrDefault();
            //    if (followupcharge == null) followupcharge = new PatientFollowUpCharge();
            //    followupcharge.PatientFollowUpID = followup.ID;
            //    followupcharge.ReasonID = followup.ReasonID;
            //    followupcharge.ActionID = followup.ActionID;
            //    followupcharge.GroupID = followup.GroupID;
            //    followupcharge.ChargeID = ChargeID;
            //    followupcharge.AddedBy = followup.AddedBy;
            //    followupcharge.AddedDate = DateTime.Now;

            //    _context.PatientFollowUpCharge.Add(followupcharge);
            //}
            else  // Code by Aziz sab 3-30-2020 
            {
                followupcharge = _context.PatientFollowUpCharge.Where(v => v.PatientFollowUpID == followup.ID && v.ChargeID == ChargeID).FirstOrDefault();
                if (followupcharge == null)
                {
                    followupcharge = new PatientFollowUpCharge();
                    followupcharge.PatientFollowUpID = followup.ID;
                    followupcharge.ReasonID = followup.ReasonID;
                    followupcharge.ActionID = followup.ActionID;
                    followupcharge.GroupID = followup.GroupID;
                    followupcharge.ChargeID = ChargeID;
                    followupcharge.AddedBy = followup.AddedBy;
                    followupcharge.AddedDate = DateTime.Now;

                    _context.PatientFollowUpCharge.Add(followupcharge);
                }
                else
                {
                    followupcharge.ReasonID = followup.ReasonID;
                    followupcharge.ActionID = followup.ActionID;
                    followupcharge.GroupID = followup.GroupID;
                    followupcharge.ChargeID = ChargeID;
                    followupcharge.AddedBy = followup.AddedBy;
                    followupcharge.AddedDate = DateTime.Now;

                    _context.PatientFollowUpCharge.Update(followupcharge);
                }
            }
            _context.SaveChanges();
        }

        [Route("TransfertoPlan/{VisitID}")]
        [HttpGet("{VisitID}")]
        public async Task<ActionResult<Visit>> TransfertoPlan(long VisitID)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
           );

            Visit visit = await _context.Visit.FindAsync(VisitID);
            if (visit == null)
                return BadRequest("Visit Not Found");

            if (visit.PrimaryPatientBal.Val() + visit.SecondaryPatientBal.Val() + visit.TertiaryPatientBal.Val() == 0)
                return BadRequest("Patient Balance is already zero");

            List<Charge> charges = _context.Charge.Where(c => c.VisitID == VisitID).ToList();
            long? PatientPlanId = null;

            foreach (Charge c in charges)
            {

                decimal? patientBal = null;

                // If Primary has allowed amount and patient has balance, Transfer to plan will be -> Transfer to Secondary Plan
                if (c.PrimaryAllowed.Val() > 0 && c.PrimaryPatientBal.Val() > 0)
                {

                    var checkPlan = (from pp in _context.PatientPlan where pp.PatientID == visit.PatientID && pp.Coverage == "S" && pp.IsActive == true select pp).FirstOrDefault();

                    if (checkPlan == null) return BadRequest("Secondary plan not found");

                    c.SecondaryBilledAmount = c.PrimaryPatientBal.Val();
                    c.SecondaryBal = c.PrimaryPatientBal.Val();

                    c.SecondaryPatientPlanID = checkPlan.ID;
                    visit.SecondaryPatientPlanID = checkPlan.ID;

                    c.PrimaryStatus = "PPTS";
                    c.SecondaryStatus = "N";
                    c.PrimaryPatientBal = null;
                    _context.Charge.Update(c);
                    PatientPlanId = checkPlan.ID;
                    visit.PrimaryStatus = "PPTS";
                    visit.SecondaryStatus = "N";
                }
                // if it has primary patient bal (SElF PAY/Transfer to patient), transfer to plan will be -> Transfer to primary 
                else if (c.PrimaryPatientBal.Val() > 0)
                {
                    c.PrimaryBal = c.PrimaryPatientBal.Val();
                    c.PrimaryPatientBal = null;
                    c.PrimaryStatus = "PAT_T_PT";           // patient to primary plan transfer
                    visit.PrimaryStatus = "PAT_T_PT";
                    _context.Charge.Update(c);
                    PatientPlanId = c.PrimaryPatientPlanID;
                }



                PaymentLedger ledger = new PaymentLedger()
                {
                    AddedBy = UD.Email,
                    AddedDate = DateTime.Now,
                    AdjustmentCodeID = null,
                    ChargeID = c.ID,
                    LedgerBy = "USER",
                    LedgerDate = DateTime.Now,
                    LedgerType = "PATIENT TO PLAN TRANSFER",
                    LedgerDescription = "PATIENT TO PLAN TRANSFER",
                    PaymentChargeID = null,
                    PatientPaymentChargeID = null,
                    PatientPlanID = PatientPlanId,
                    VisitID = c.VisitID.Value,
                    Amount = patientBal
                };
                _context.PaymentLedger.Add(ledger);
            }

            var visitSum = (from c in _context.Charge
                            where c.VisitID == visit.ID
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
                                SecondaryTransferred = v.Sum(x => x.SecondaryTransferred.Val()),
                                SecondaryAllowed = v.Sum(x => x.SecondaryAllowed.Val()),
                                SecondaryWriteOff = v.Sum(x => x.SecondaryWriteOff.Val()),
                                SecondaryPaid = v.Sum(x => x.SecondaryPaid.Val()),
                                TertiaryBilledAmount = v.Sum(x => x.TertiaryBilledAmount.Val()),
                                TertiaryBal = v.Sum(x => x.TertiaryBal.Val()),
                                SecondaryPatientBal = v.Sum(x => x.SecondaryPatientBal.Val()),
                            }).FirstOrDefault();

            visit.PrimaryBal = visitSum.PrimaryBal;
            visit.PrimaryPatientBal = visitSum.PrimaryPatBal;
            visit.OtherPatResp = visitSum.OtherPatResp;

            visit.SecondaryBal = visitSum.SecondaryBal;
            visit.SecondaryBilledAmount = visitSum.SecondaryBilledAmount;
            _context.Visit.Update(visit);
            _context.SaveChanges();

            visit.Charges = _context.Charge.Where(m => m.VisitID == visit.ID).ToList<Charge>();
            visit.PatientPayments = _context.PatientPayment.Where(m => m.VisitID == visit.ID).ToList<PatientPayment>();
            visit.Note = _context.Notes.Where(m => m.VisitID == visit.ID).ToList<Notes>();

            return visit;
        }
        [HttpPost]
        [Route("FindVisits")]
        public async Task<ActionResult<IEnumerable<GVisit>>> FindVisits(CVisit CVisit)
        {
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            string temp = User.Claims.FirstOrDefault(x => x.Type.Equals("TEMP", StringComparison.InvariantCultureIgnoreCase)).Value;
            return FindVisits(CVisit, PracticeId, temp);
        }
        private List<GVisit> FindVisits(CVisit CVisit, long PracticeId, string contextName)
        {
            string connectionstring = CommonUtil.GetConnectionString(PracticeId, contextName);
            List<GVisit> data = new List<GVisit>();
            using (SqlConnection myconnection = new SqlConnection(connectionstring))
            {
                string selection = "";
                if (CVisit.AgeType == "S")
                {
                    selection += (" (ISNULL(DATEDIFF(day,  v.SubmittedDate,GETDATE()), 0 ))  ClaimAge,");
                }
                else if (CVisit.AgeType == "D")
                {
                    selection += (" (ISNULL(DATEDIFF(day,  v.DateOfServiceFrom,GETDATE()), 0 ))  ClaimAge,");
                }
                else if (CVisit.AgeType == "E")
                {
                    selection += (" (ISNULL(DATEDIFF(day,  v.AddedDate,GETDATE()), 0 ))  ClaimAge,");
                }

                string ostring = "select pat.ID patientID, " + selection + "pat.AccountNum AccountNum, (pat.LastName + ', ' + pat.FirstName) Patient,convert(varchar, v.DateOfServiceFrom, 101)   DOS,  'PrimaryStatus' = CASE when v.PrimaryStatus = 'N' then 'New Charge'    when v.PrimaryStatus = 'S' then 'Submitted'  when v.PrimaryStatus = 'K' then '999 Accepted' When v.PrimaryStatus = 'D' then '999 Denied'  when v.PrimaryStatus = 'E' then '999 has Errors' when v.PrimaryStatus = 'P' then 'Paid'  when v.PrimaryStatus = 'DN' then 'Denied' when v.PrimaryStatus = 'PT_P' then 'Patient Paid' when v.PrimaryStatus = 'PPTS' then 'Transefered to Sec.' when v.PrimaryStatus = 'PPTT' then 'Paid-Transfered To Ter' when v.PrimaryStatus = 'PPTP' then 'Transfered to Patient' when v.PrimaryStatus = 'SPTP' then 'Paid-Transfered To Patient' when v.PrimaryStatus = 'SPTT' then 'Paid-Transfered To Ter' when v.PrimaryStatus = 'PR_TP' then 'Pat. Resp. Transferred to Pat' when v.PrimaryStatus = 'PPTM' then 'Paid - Medigaped' when v.PrimaryStatus = 'M' then 'Medigaped' when v.PrimaryStatus = 'R' then 'Rejected' when v.PrimaryStatus = 'A1AY' then 'Received By Clearing House' when v.PrimaryStatus = 'A0PR' then 'Forwarded  to Payer' when v.PrimaryStatus = 'A1PR' then 'Received By Payer' when v.PrimaryStatus = 'A2PR' then 'Accepted By Payer' when v.PrimaryStatus = 'TS' then 'Transferred to Secondary' when v.PrimaryStatus = 'TT' then 'Transferred to Tertiary' when v.PrimaryStatus = 'PTPT' then 'Plan to Patient Transfer' when v.PrimaryStatus = 'RS' then 'Re-Submitted' END ,  'SecondaryStatus' = CASE when v.SecondaryStatus = 'N' then 'New Charge'    when v.SecondaryStatus = 'S' then 'Submitted'  when v.SecondaryStatus = 'K' then '999 Accepted' When v.SecondaryStatus = 'D' then '999 Denied'  when v.SecondaryStatus = 'E' then '999 has Errors' when v.SecondaryStatus = 'P' then 'Paid'  when v.SecondaryStatus = 'DN' then 'Denied' when v.SecondaryStatus = 'PT_P' then 'Patient Paid' when v.SecondaryStatus = 'PPTS' then 'Transefered to Sec.' when v.SecondaryStatus = 'PPTT' then 'Paid-Transfered To Ter' when v.SecondaryStatus = 'PPTP' then 'Transfered to Patient' when v.SecondaryStatus = 'SPTP' then 'Paid-Transfered To Patient' when v.SecondaryStatus = 'SPTT' then 'Paid-Transfered To Ter' when v.SecondaryStatus = 'PR_TP' then 'Pat. Resp. Transferred to Pat' when v.SecondaryStatus = 'PPTM' then 'Paid - Medigaped' when v.SecondaryStatus = 'M' then 'Medigaped' when v.SecondaryStatus = 'R' then 'Rejected' when v.SecondaryStatus = 'A1AY' then 'Received By Clearing House' when v.SecondaryStatus = 'A0PR' then 'Forwarded  to Payer' when v.SecondaryStatus = 'A1PR' then 'Received By Payer' when v.SecondaryStatus = 'A2PR' then 'Accepted By Payer' when v.SecondaryStatus = 'TS' then 'Transferred to Secondary' when v.SecondaryStatus = 'TT' then 'Transferred to Tertiary' when v.SecondaryStatus = 'PTPT' then 'Plan to Patient Transfer' when v.SecondaryStatus = 'RS' then 'Re-Submitted' END,convert(varchar, v.AddedDate, 101)  EntryDate, v.ID PracticeID, 'VisitType'= Case when v.VisitType='I' Then 'Institutional' when v.VisitType='P' Then 'P' when v.VisitType='D' Then 'D' end , loc.ID LocationID, loc.[Name] [Location],convert(varchar, v.SubmittedDate, 101)  SubmittedDate, v.PrimaryBilledAmount BilledAmount,v.PrimaryAllowed AllowedAmount, v.PrimaryPaid PaidAmount, v.PrimaryWriteOff AdjustmentAmount,v.PrimaryBal PrimaryPlanBalance, v.PrimaryPatientBal PrimaryPatientBalance, v.RejectionReason Rejection, prov.ID ProviderID, v.ID VisitID, prov.[Name] [Provider]  , iPlan.ID InsurancePlanID,iPlan.PlanName  InsurancePlanName, pPlan.SubscriberId SubscriberID, v.PrimaryPatientPlanID PrimaryPatientPlanID, v.SecondaryBal SecondaryPlanBalance, v.SecondaryPatientBal SecondaryPatientBalance, iPlan.Edi837PayerID Edi837PayerID from Visit v join Patient pat on v.PatientID = pat.ID join [Location] loc on v.LocationID = loc.ID join[Provider] prov on v.ProviderID = prov.ID join PatientPlan pPlan on v.PrimaryPatientPlanID = pPlan.ID join InsurancePlan iPlan on pPlan.InsurancePlanID = iPlan.ID";


                if (!CVisit.ChargeID.IsNull())
                {
                    ostring += " join  Charge c on v.ID = c.VisitID ";
                }
                if (!CVisit.CPTCode.IsNull())
                {
                    if (!CVisit.ChargeID.IsNull())
                    {
                        ostring += "  join Cpt cpt on c.CPTID = cpt.ID ";
                    }
                    else
                    {
                        ostring += " join Charge c on v.ID = c.VisitID join Cpt cpt on c.CPTID = cpt.ID ";
                    }
                }


                ostring += " where v.practiceid = {0} ";
                ostring = string.Format(ostring, PracticeId);


                if (!CVisit.LastName.IsNull())
                {
                    if (CVisit.LastName.Contains("'"))
                    {
                        //Modify ContextName
                        string RLastName = CVisit.LastName.Trim();
                        RLastName = RLastName.Replace("'", "''");
                        ostring += string.Format(" and pat.LastName like '%{0}%'", RLastName);
                    }
                    else
                    {
                        ostring += string.Format(" and pat.LastName like '%{0}%'", CVisit.LastName);
                    }

                }


                if (!CVisit.FirstName.IsNull())
                {
                    if (CVisit.FirstName.Contains("'"))
                    {
                        //Modify ContextName
                        string RFirstName = CVisit.FirstName.Trim();
                        RFirstName = RFirstName.Replace("'", "''");
                        ostring += string.Format(" and pat.FirstName like '%{0}%'", RFirstName);
                    }
                    else
                    {
                        ostring += string.Format(" and pat.FirstName like '%{0}%'", CVisit.FirstName);
                    }

                }

                if (!CVisit.AccountNum.IsNull())
                    ostring += string.Format(" and pat.AccountNum ='{0}'", CVisit.AccountNum);
                if (!CVisit.Location.IsNull())
                    ostring += string.Format(" and  loc.Name like '%{0}%'", CVisit.Location);
                if (!CVisit.Provider.IsNull())
                    ostring += string.Format(" and prov.Name like '%{0}%'", CVisit.Provider);
                if (!CVisit.SubscriberID.IsNull())
                    ostring += string.Format(" and pPlan.SubscriberId ='{0}'", CVisit.SubscriberID);
                if (!CVisit.Plan.IsNull())
                    ostring += string.Format(" and iPlan.PlanName like '%{0}%'", CVisit.Plan);
                if (!CVisit.VisitID.IsNull())
                    ostring += string.Format(" and v.ID ='{0}'", CVisit.VisitID);
                if (!CVisit.BatchID.IsNull())
                    ostring += string.Format(" and v.BatchDocumentID ='{0}'", CVisit.BatchID);

                if (!CVisit.CPTCode.IsNull())
                {
                    ostring += string.Format(" and cpt.CPTCode ='{0}'", CVisit.CPTCode);
                }
                if (!CVisit.ChargeID.IsNull())
                {
                    ostring += string.Format(" and c.ID ='{0}'", CVisit.ChargeID);
                }
                if (!CVisit.PayerID.IsNull())
                {
                    ostring += string.Format(" and iPlan.Edi837PayerID ='{0}'", CVisit.PayerID);
                }



                if (!CVisit.VisitType.IsNull())
                    ostring += string.Format(" and v.VisitType like '%{0}%'", CVisit.VisitType);


                if (CVisit.EntryDateFrom != null && CVisit.EntryDateTo != null)
                {
                    ostring += (" and (v.AddedDate  >= '" + CVisit.EntryDateFrom.GetValueOrDefault().Date + "' and v.AddedDate  < '" + CVisit.EntryDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                }
                else if (CVisit.EntryDateFrom != null && CVisit.EntryDateTo == null)
                {
                    ostring += (" and ( v.AddedDate  >= '" + CVisit.EntryDateFrom.GetValueOrDefault().Date + "')");

                }
                else if (CVisit.EntryDateFrom == null && CVisit.EntryDateTo != null)
                {
                    ostring += (" and (v.AddedDate  <= '" + CVisit.EntryDateTo.GetValueOrDefault().Date + "')");
                }


                if (CVisit.SubmittedFromDate != null && CVisit.SubmittedToDate != null)
                {
                    ostring += (" and (v.SubmittedDate  >= '" + CVisit.SubmittedFromDate.GetValueOrDefault().Date + "' and v.SubmittedDate  < '" + CVisit.SubmittedToDate.GetValueOrDefault().Date.AddDays(1) + "')");
                }
                else if (CVisit.SubmittedFromDate != null && CVisit.SubmittedToDate == null)
                {
                    ostring += (" and ( v.SubmittedDate  >= '" + CVisit.SubmittedFromDate.GetValueOrDefault().Date + "')");

                }
                else if (CVisit.SubmittedFromDate == null && CVisit.SubmittedToDate != null)
                {
                    ostring += (" and (v.SubmittedDate  <= '" + CVisit.SubmittedToDate.GetValueOrDefault().Date + "')");
                }




                if (CVisit.DosFrom != null && CVisit.DosTo != null)
                {
                    ostring += (" and (v.DateOfServiceFrom  >= '" + CVisit.DosFrom.GetValueOrDefault().Date + "' and v.DateOfServiceFrom  < '" + CVisit.DosTo.GetValueOrDefault().Date.AddDays(1) + "')");
                }
                else if (CVisit.DosFrom != null && CVisit.DosTo == null)
                {
                    ostring += (" and ( v.DateOfServiceFrom  >= '" + CVisit.DosFrom.GetValueOrDefault().Date + "')");

                }
                else if (CVisit.DosFrom == null && CVisit.DosTo != null)
                {
                    ostring += (" and (v.DateOfServiceFrom  <= '" + CVisit.DosTo.GetValueOrDefault().Date + "')");
                }



                if (CVisit.InsuranceType == "P")
                {
                    ostring += (" and ( ISNULL(v.PrimaryPatientPlanID, 0 )  > 0 and  v.SecondaryPatientPlanID is null and v.TertiaryPatientPlanID is null)");
                }
                else if (CVisit.InsuranceType == "S")
                {
                    ostring += (" and ( ISNULL(v.PrimaryPatientPlanID, 0 ) > 0 and  ISNULL(v.SecondaryPatientPlanID, 0 )  > 0 and v.TertiaryPatientPlanID is null )");
                }
                else if (CVisit.InsuranceType == "T")
                {
                    ostring += (" and (  ISNULL(v.PrimaryPatientPlanID, 0 ) > 0 and  ISNULL(v.SecondaryPatientPlanID, 0 ) > 0  and  ISNULL(v.TertiaryPatientPlanID, 0 ) > 0 )");
                }
                else
                {
                    ostring += (" and (ISNULL(v.PrimaryPatientPlanID, 0 ) > 0)");
                }


                if (!CVisit.SubmissionType.IsNull())
                {
                    ostring += string.Format(" and iPlan.SubmissionType ='{0}'", CVisit.SubmissionType);
                }

                if (!CVisit.Status.IsNull())
                {
                    if (CVisit.Status == "SystemRejected")
                    {
                        ostring += string.Format(" and ( v.ValidationMessage is not  null AND v.ValidationMessage !='' ) ");
                    }
                    else
                    {
                        ostring += string.Format(" and ( v.PrimaryStatus = '" + CVisit.Status + "' OR v.SecondaryStatus = '" + CVisit.Status + "' ) ");
                    }
                }
                if (CVisit.IsSubmitted == "Y")
                {
                    ostring += string.Format(" and v.IsSubmitted = 'true' ");
                }
                else if (CVisit.IsSubmitted == "N")
                    ostring += string.Format(" and v.IsSubmitted = 'false'   ");

                if (!CVisit.IsPaid.IsNull())
                {
                    if (CVisit.InsuranceType == "Y")
                    {
                        ostring += string.Format(" and (ISNULL(v.PrimaryPaid, 0 ) > 0 and ISNULL(v.PrimaryBal, 0 ) + ISNULL(v.SecondaryBal, 0 ) + ISNULL(v.TertiaryBal, 0 ) = 0 )");
                    }
                    else
                    {
                        if (CVisit.InsuranceType == "P")
                        {
                            ostring += string.Format(" and ( ISNULL(v.PrimaryPaid, 0 ) > 0 and ISNULL(v.PrimaryBal, 0 ) + ISNULL(v.SecondaryBal, 0 ) + ISNULL(v.TertiaryBal, 0 ) > 0  )");
                        }
                        else
                        {
                            ostring += string.Format(" and ISNULL(v.PrimaryPaid, 0 ) = 0");

                        }
                    }
                }


                ostring += string.Format("  ORDER BY  v.ID DESC;");

                SqlCommand ocmd = new SqlCommand(ostring, myconnection);
                myconnection.Open();

                using (SqlDataReader oreader = ocmd.ExecuteReader())
                {
                    while (oreader.Read())
                    {

                        data.Add(new GVisit()
                        {
                            patientID = oreader["patientID"].ToString() != "" ? long.Parse(oreader["patientID"].ToString()) : 0,
                            AccountNum = oreader["accountnum"].ToString(),
                            Patient = oreader["patient"].ToString(),
                            DOS = oreader["dos"].ToString(),
                            EntryDate = oreader["entrydate"].ToString(),
                            PracticeID = oreader["PracticeID"].ToString() != "" ? long.Parse(oreader["PracticeID"].ToString()) : 0,
                            LocationID = oreader["LocationID"].ToString() != "" ? long.Parse(oreader["LocationID"].ToString()) : 0,
                            Location = oreader["Location"].ToString(),
                            SubmittedDate = oreader["SubmittedDate"].ToString(),
                            ClaimAge = oreader["ClaimAge"].ToString(),
                            BilledAmount = oreader["BilledAmount"].ToString() != "" ? decimal.Parse(oreader["BilledAmount"].ToString()) : 0,
                            AllowedAmount = oreader["AllowedAmount"].ToString() != "" ? decimal.Parse(oreader["AllowedAmount"].ToString()) : 0,
                            PaidAmount = oreader["PaidAmount"].ToString() != "" ? decimal.Parse(oreader["PaidAmount"].ToString()) : 0,
                            PrimaryStatus = oreader["PrimaryStatus"].ToString(),
                            AdjustmentAmount = oreader["AdjustmentAmount"].ToString() != "" ? decimal.Parse(oreader["AdjustmentAmount"].ToString()) : 0,
                            PrimaryPlanBalance = oreader["PrimaryPlanBalance"].ToString() != "" ? decimal.Parse(oreader["PrimaryPlanBalance"].ToString()) : 0,
                            PrimaryPatientBalance = oreader["PrimaryPatientBalance"].ToString() != "" ? decimal.Parse(oreader["PrimaryPatientBalance"].ToString()) : 0,
                            Rejection = oreader["Rejection"].ToString(),
                            ProviderID = oreader["ProviderID"].ToString() != "" ? long.Parse(oreader["ProviderID"].ToString()) : 0,
                            Provider = oreader["Provider"].ToString(),
                            VisitID = oreader["VisitID"].ToString() != "" ? long.Parse(oreader["VisitID"].ToString()) : 0,
                            InsurancePlanID = oreader["InsurancePlanID"].ToString() != "" ? long.Parse(oreader["InsurancePlanID"].ToString()) : 0,
                            InsurancePlanName = oreader["InsurancePlanName"].ToString(),
                            SubscriberID = oreader["SubscriberID"].ToString(),
                            PrimaryPatientPlanID = oreader["PrimaryPatientPlanID"].ToString(),
                            SecondaryStatus = oreader["SecondaryStatus"].ToString(),
                            SecondaryPlanBalance = oreader["SecondaryPlanBalance"].ToString() != "" ? decimal.Parse(oreader["SecondaryPlanBalance"].ToString()) : 0,
                            SecondaryPatientBalance = oreader["SecondaryPatientBalance"].ToString() != "" ? decimal.Parse(oreader["SecondaryPatientBalance"].ToString()) : 0,
                            Edi837PayerID = oreader["Edi837PayerID"].ToString() != "" ? long.Parse(oreader["Edi837PayerID"].ToString()) : 0,
                            VisitType = oreader["VisitType"].ToString(),


                        });
                    }
                    myconnection.Close();
                }
            }
            return data;

        }


        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CVisit CVisit)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            string temp = User.Claims.FirstOrDefault(x => x.Type.Equals("TEMP", StringComparison.InvariantCultureIgnoreCase)).Value;
            List<GVisit> data = FindVisits(CVisit, PracticeId, temp);
            ExportController controller = new ExportController(_context);

            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);

            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CVisit, "Visit Report");

        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CVisit CVisit)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            string temp = User.Claims.FirstOrDefault(x => x.Type.Equals("TEMP", StringComparison.InvariantCultureIgnoreCase)).Value;
            List<GVisit> data = FindVisits(CVisit, PracticeId, temp);
            ExportController controller = new ExportController(_context);

            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);

            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);
        }

        [Route("FindClaimStatus/{VisitID}")]
        [HttpGet("{VisitID}")]
        public List<GClaimStatus> FindClaimStatus(long VisitID)
        {
            Visit visit = _context.Visit.Where(e => e.ID.Equals(VisitID)).FirstOrDefault();//.FindAsync(VisitID);
            List<GClaimStatus> Claim = (from e in _context.VisitStatus
                                        join VL in _context.VisitStatusLog on e.VisitStatusLogID equals VL.ID
                                        where e.VisitID == visit.ID
                                        orderby e.StatusDate descending
                                        select new GClaimStatus
                                        {
                                            DownloadedFile = VL.DownloadedFileID,
                                            VisitID = e.VisitID,
                                            ResponseEntity = TranslateStatus(e.ResponseEntity),//e.CategoryCode1 + e.ResponseEntity,
                                            TRNNumber = e.TRNNumber,
                                            ActionCode = e.ActionCode,
                                            PayerName = e.ActionCode,
                                            CategoryCode1 = e.CategoryCode1,
                                            StatusCode1 = e.StatusCode1,
                                            FreeText1 = e.FreeText1,
                                            FreeText2 = e.FreeText2,
                                            StatusDate = e.StatusDate,
                                            PayerControlNumber = e.PayerControlNumber,
                                            AddedDate = e.AddedDate,
                                        }).ToList();
            return Claim;
        }

        [Route("FindVisits2")]
        [HttpPost]
        public async Task<ActionResult> FindVisits2(CVisit CVisit)
        {

            List<GVisit> objPatient = new List<GVisit>();

            int skipPage = (CVisit.pageNo - 1) * CVisit.PerPage;
            int totalCount = 0, totalPages = 0, perPage = 0, currentPage = 0;

            try
            {
                if (skipPage >= 0)
                {


                    long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
                    var Client = (from w in _contextMain.MainPractice
                                  where w.ID == PracticeId
                                  select w
                                ).FirstOrDefault();
                    string contextName = _contextMain.MainClient.Find(Client.ClientID)?.ContextName;
                    string newConnection = GetConnectionStringManager(contextName);
                    List<GVisit> data3 = new List<GVisit>();
                    List<GVisit> dataCount = new List<GVisit>();


                    string selection = "";
                    if (CVisit.AgeType == "S")
                    {
                        selection += (" (ISNULL(DATEDIFF(day,  v.SubmittedDate,GETDATE()), 0 ))  ClaimAge,");
                    }
                    else if (CVisit.AgeType == "D")
                    {
                        selection += (" (ISNULL(DATEDIFF(day,  v.DateOfServiceFrom,GETDATE()), 0 ))  ClaimAge,");
                    }
                    else if (CVisit.AgeType == "E")
                    {
                        selection += (" (ISNULL(DATEDIFF(day,  v.AddedDate,GETDATE()), 0 ))  ClaimAge,");
                    }

                    string selectStatement = "";
                    string WhereStatement = "";
                    string ostring = "";
                    string fromStatement = "";


                    if (!CVisit.ChargeID.IsNull())
                    {
                        WhereStatement += " join  Charge c on v.ID = c.VisitID ";
                    }
                    if (!CVisit.CPTCode.IsNull())
                    {
                        if (!CVisit.ChargeID.IsNull())
                        {
                            WhereStatement += "  join Cpt cpt on c.CPTID = cpt.ID ";
                        }
                        else
                        {
                            WhereStatement += " join Charge c on v.ID = c.VisitID join Cpt cpt on c.CPTID = cpt.ID ";
                        }
                    }

                    WhereStatement = " where v.practiceid = {0} ";
                    WhereStatement = string.Format(WhereStatement, PracticeId);


                    if (!CVisit.LastName.IsNull())
                    {
                        if (CVisit.LastName.Contains("'"))
                        {
                            //Modify ContextName
                            string RLastName = CVisit.LastName.Trim();
                            RLastName = RLastName.Replace("'", "''");
                            WhereStatement += string.Format(" and pat.LastName like '%{0}%'", RLastName);
                        }
                        else
                        {
                            WhereStatement += string.Format(" and pat.LastName like '%{0}%'", CVisit.LastName);
                        }

                    }


                    if (!CVisit.FirstName.IsNull())
                    {
                        if (CVisit.FirstName.Contains("'"))
                        {
                            //Modify ContextName
                            string RFirstName = CVisit.FirstName.Trim();
                            RFirstName = RFirstName.Replace("'", "''");
                            WhereStatement += string.Format(" and pat.FirstName like '%{0}%'", RFirstName);
                        }
                        else
                        {
                            WhereStatement += string.Format(" and pat.FirstName like '%{0}%'", CVisit.FirstName);
                        }

                    }

                    if (!CVisit.AccountNum.IsNull())
                        WhereStatement += string.Format(" and pat.AccountNum ='{0}'", CVisit.AccountNum);
                    if (!CVisit.Location.IsNull())
                        WhereStatement += string.Format(" and  loc.Name like '%{0}%'", CVisit.Location);
                    if (!CVisit.Provider.IsNull())
                        WhereStatement += string.Format(" and prov.Name like '%{0}%'", CVisit.Provider);
                    if (!CVisit.SubscriberID.IsNull())
                        WhereStatement += string.Format(" and pPlan.SubscriberId ='{0}'", CVisit.SubscriberID);
                    if (!CVisit.Plan.IsNull())
                        WhereStatement += string.Format(" and iPlan.PlanName like '%{0}%'", CVisit.Plan);
                    if (!CVisit.VisitID.IsNull())
                        WhereStatement += string.Format(" and v.ID ='{0}'", CVisit.VisitID);
                    if (!CVisit.BatchID.IsNull())
                        WhereStatement += string.Format(" and v.BatchDocumentID ='{0}'", CVisit.BatchID);

                    if (!CVisit.CPTCode.IsNull())
                    {
                        WhereStatement += string.Format(" and cpt.CPTCode ='{0}'", CVisit.CPTCode);
                    }
                    if (!CVisit.ChargeID.IsNull())
                    {
                        WhereStatement += string.Format(" and c.ID ='{0}'", CVisit.ChargeID);
                    }
                    if (!CVisit.PayerID.IsNull())
                    {
                        WhereStatement += string.Format(" and iPlan.Edi837PayerID ='{0}'", CVisit.PayerID);
                    }



                    if (!CVisit.VisitType.IsNull())
                        WhereStatement += string.Format(" and v.VisitType like '%{0}%'", CVisit.VisitType);


                    if (CVisit.EntryDateFrom != null && CVisit.EntryDateTo != null)
                    {
                        WhereStatement += (" and (v.AddedDate  >= '" + CVisit.EntryDateFrom.GetValueOrDefault().Date + "' and v.AddedDate  < '" + CVisit.EntryDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (CVisit.EntryDateFrom != null && CVisit.EntryDateTo == null)
                    {
                        WhereStatement += (" and ( v.AddedDate  >= '" + CVisit.EntryDateFrom.GetValueOrDefault().Date + "')");

                    }
                    else if (CVisit.EntryDateFrom == null && CVisit.EntryDateTo != null)
                    {
                        WhereStatement += (" and (v.AddedDate  <= '" + CVisit.EntryDateTo.GetValueOrDefault().Date + "')");
                    }


                    if (CVisit.SubmittedFromDate != null && CVisit.SubmittedToDate != null)
                    {
                        WhereStatement += (" and (v.SubmittedDate  >= '" + CVisit.SubmittedFromDate.GetValueOrDefault().Date + "' and v.SubmittedDate  < '" + CVisit.SubmittedToDate.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (CVisit.SubmittedFromDate != null && CVisit.SubmittedToDate == null)
                    {
                        WhereStatement += (" and ( v.SubmittedDate  >= '" + CVisit.SubmittedFromDate.GetValueOrDefault().Date + "')");

                    }
                    else if (CVisit.SubmittedFromDate == null && CVisit.SubmittedToDate != null)
                    {
                        WhereStatement += (" and (v.SubmittedDate  <= '" + CVisit.SubmittedToDate.GetValueOrDefault().Date + "')");
                    }




                    if (CVisit.DosFrom != null && CVisit.DosTo != null)
                    {
                        WhereStatement += (" and (v.DateOfServiceFrom  >= '" + CVisit.DosFrom.GetValueOrDefault().Date + "' and v.DateOfServiceFrom  < '" + CVisit.DosTo.GetValueOrDefault().Date.AddDays(1) + "')");
                    }
                    else if (CVisit.DosFrom != null && CVisit.DosTo == null)
                    {
                        WhereStatement += (" and ( v.DateOfServiceFrom  >= '" + CVisit.DosFrom.GetValueOrDefault().Date + "')");

                    }
                    else if (CVisit.DosFrom == null && CVisit.DosTo != null)
                    {
                        WhereStatement += (" and (v.DateOfServiceFrom  <= '" + CVisit.DosTo.GetValueOrDefault().Date + "')");
                    }



                    if (CVisit.InsuranceType == "P")
                    {
                        WhereStatement += (" and ( ISNULL(v.PrimaryPatientPlanID, 0 )  > 0 and  v.SecondaryPatientPlanID is null and v.TertiaryPatientPlanID is null)");
                    }
                    else if (CVisit.InsuranceType == "S")
                    {
                        WhereStatement += (" and ( ISNULL(v.PrimaryPatientPlanID, 0 ) > 0 and  ISNULL(v.SecondaryPatientPlanID, 0 )  > 0 and v.TertiaryPatientPlanID is null )");
                    }
                    else if (CVisit.InsuranceType == "T")
                    {
                        WhereStatement += (" and (  ISNULL(v.PrimaryPatientPlanID, 0 ) > 0 and  ISNULL(v.SecondaryPatientPlanID, 0 ) > 0  and  ISNULL(v.TertiaryPatientPlanID, 0 ) > 0 )");
                    }
                    else
                    {
                        WhereStatement += (" and (ISNULL(v.PrimaryPatientPlanID, 0 ) > 0)");
                    }


                    if (!CVisit.SubmissionType.IsNull())
                    {
                        WhereStatement += string.Format(" and iPlan.SubmissionType ='{0}'", CVisit.SubmissionType);
                    }

                    if (!CVisit.Status.IsNull())
                    {
                        if (CVisit.Status == "SystemRejected")
                        {
                            ostring += string.Format(" and ( v.ValidationMessage is not  null AND v.ValidationMessage !='' ) ");
                        }
                        else
                        {
                            ostring += string.Format(" and ( v.PrimaryStatus = '" + CVisit.Status + "' OR v.SecondaryStatus = '" + CVisit.Status + "' ) ");
                        }
                    }
                    if (CVisit.IsSubmitted == "Y")
                    {
                        WhereStatement += string.Format(" and v.IsSubmitted = 'true' ");
                    }
                    else if (CVisit.IsSubmitted == "N")
                        WhereStatement += string.Format(" and v.IsSubmitted = 'false'   ");

                    if (!CVisit.IsPaid.IsNull())
                    {
                        if (CVisit.InsuranceType == "Y")
                        {
                            WhereStatement += string.Format(" and (ISNULL(v.PrimaryPaid, 0 ) > 0 and ISNULL(v.PrimaryBal, 0 ) + ISNULL(v.SecondaryBal, 0 ) + ISNULL(v.TertiaryBal, 0 ) = 0 )");
                        }
                        else
                        {
                            if (CVisit.InsuranceType == "P")
                            {
                                WhereStatement += string.Format(" and ( ISNULL(v.PrimaryPaid, 0 ) > 0 and ISNULL(v.PrimaryBal, 0 ) + ISNULL(v.SecondaryBal, 0 ) + ISNULL(v.TertiaryBal, 0 ) > 0  )");
                            }
                            else
                            {
                                WhereStatement += string.Format(" and ISNULL(v.PrimaryPaid, 0 ) = 0");

                            }
                        }
                    }

                    #region From

                    fromStatement += string.Format("  from Visit v join Patient pat on v.PatientID = pat.ID join [Location] loc on v.LocationID = loc.ID join[Provider] prov on v.ProviderID = prov.ID join PatientPlan pPlan on v.PrimaryPatientPlanID = pPlan.ID join InsurancePlan iPlan on pPlan.InsurancePlanID = iPlan.ID  ");
                    #endregion

                    if (CVisit.pageNo == 1)
                    {
                        selectStatement = "select count(1) as TotalCount ";
                        ostring = selectStatement + fromStatement + WhereStatement;

                        using (SqlConnection myconnection = new SqlConnection(newConnection))
                        {
                            SqlCommand ocmd = new SqlCommand(ostring, myconnection);
                            myconnection.Open();

                            using (SqlDataReader oreader = ocmd.ExecuteReader())
                            {
                                while (oreader.Read())
                                {
                                    totalCount = oreader["TotalCount"].ToString() != "" ? int.Parse(oreader["TotalCount"].ToString()) : 0;
                                }
                                myconnection.Close();
                            }
                        }

                        totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalCount) / CVisit.PerPage));
                    }

                    List<GVisit> data2 = new List<GVisit>();
                    using (SqlConnection myconnection = new SqlConnection(newConnection))
                    {


                        selectStatement = "select pat.ID patientID, " + selection + "pat.AccountNum AccountNum, (pat.LastName + ', ' + pat.FirstName) Patient,convert(varchar, v.DateOfServiceFrom, 101)   DOS,  'PrimaryStatus' = CASE when v.PrimaryStatus = 'N' then 'New Charge'    when v.PrimaryStatus = 'S' then 'Submitted'  when v.PrimaryStatus = 'K' then '999 Accepted' When v.PrimaryStatus = 'D' then '999 Denied'  when v.PrimaryStatus = 'E' then '999 has Errors' when v.PrimaryStatus = 'P' then 'Paid'  when v.PrimaryStatus = 'DN' then 'Denied' when v.PrimaryStatus = 'PT_P' then 'Patient Paid' when v.PrimaryStatus = 'PPTS' then 'Transefered to Sec.' when v.PrimaryStatus = 'PPTT' then 'Paid-Transfered To Ter' when v.PrimaryStatus = 'PPTP' then 'Transfered to Patient' when v.PrimaryStatus = 'SPTP' then 'Paid-Transfered To Patient' when v.PrimaryStatus = 'SPTT' then 'Paid-Transfered To Ter' when v.PrimaryStatus = 'PR_TP' then 'Pat. Resp. Transferred to Pat' when v.PrimaryStatus = 'PPTM' then 'Paid - Medigaped' when v.PrimaryStatus = 'M' then 'Medigaped' when v.PrimaryStatus = 'R' then 'Rejected' when v.PrimaryStatus = 'A1AY' then 'Received By Clearing House' when v.PrimaryStatus = 'A0PR' then 'Forwarded  to Payer' when v.PrimaryStatus = 'A1PR' then 'Received By Payer' when v.PrimaryStatus = 'A2PR' then 'Accepted By Payer' when v.PrimaryStatus = 'TS' then 'Transferred to Secondary' when v.PrimaryStatus = 'TT' then 'Transferred to Tertiary' when v.PrimaryStatus = 'PTPT' then 'Plan to Patient Transfer' when v.PrimaryStatus = 'RS' then 'Re-Submitted' END ,  'SecondaryStatus' = CASE when v.SecondaryStatus = 'N' then 'New Charge'    when v.SecondaryStatus = 'S' then 'Submitted'  when v.SecondaryStatus = 'K' then '999 Accepted' When v.SecondaryStatus = 'D' then '999 Denied'  when v.SecondaryStatus = 'E' then '999 has Errors' when v.SecondaryStatus = 'P' then 'Paid'  when v.SecondaryStatus = 'DN' then 'Denied' when v.SecondaryStatus = 'PT_P' then 'Patient Paid' when v.SecondaryStatus = 'PPTS' then 'Transefered to Sec.' when v.SecondaryStatus = 'PPTT' then 'Paid-Transfered To Ter' when v.SecondaryStatus = 'PPTP' then 'Transfered to Patient' when v.SecondaryStatus = 'SPTP' then 'Paid-Transfered To Patient' when v.SecondaryStatus = 'SPTT' then 'Paid-Transfered To Ter' when v.SecondaryStatus = 'PR_TP' then 'Pat. Resp. Transferred to Pat' when v.SecondaryStatus = 'PPTM' then 'Paid - Medigaped' when v.SecondaryStatus = 'M' then 'Medigaped' when v.SecondaryStatus = 'R' then 'Rejected' when v.SecondaryStatus = 'A1AY' then 'Received By Clearing House' when v.SecondaryStatus = 'A0PR' then 'Forwarded  to Payer' when v.SecondaryStatus = 'A1PR' then 'Received By Payer' when v.SecondaryStatus = 'A2PR' then 'Accepted By Payer' when v.SecondaryStatus = 'TS' then 'Transferred to Secondary' when v.SecondaryStatus = 'TT' then 'Transferred to Tertiary' when v.SecondaryStatus = 'PTPT' then 'Plan to Patient Transfer' when v.SecondaryStatus = 'RS' then 'Re-Submitted' END,convert(varchar, v.AddedDate, 101)  EntryDate, v.ID PracticeID, 'VisitType'= Case when v.VisitType='I' Then 'Institutional' when v.VisitType='P' Then 'P' when v.VisitType='D' Then 'D' end , loc.ID LocationID, loc.[Name] [Location],convert(varchar, v.SubmittedDate, 101)  SubmittedDate, v.PrimaryBilledAmount BilledAmount,v.PrimaryAllowed AllowedAmount, v.PrimaryPaid PaidAmount, v.PrimaryWriteOff AdjustmentAmount,v.PrimaryBal PrimaryPlanBalance, v.PrimaryPatientBal PrimaryPatientBalance, v.RejectionReason Rejection, prov.ID ProviderID, v.ID VisitID, prov.[Name] [Provider]  , iPlan.ID InsurancePlanID,iPlan.PlanName  InsurancePlanName, pPlan.SubscriberId SubscriberID, v.PrimaryPatientPlanID PrimaryPatientPlanID, v.SecondaryBal SecondaryPlanBalance, v.SecondaryPatientBal SecondaryPatientBalance, iPlan.Edi837PayerID Edi837PayerID from Visit v join Patient pat on v.PatientID = pat.ID join [Location] loc on v.LocationID = loc.ID join[Provider] prov on v.ProviderID = prov.ID join PatientPlan pPlan on v.PrimaryPatientPlanID = pPlan.ID join InsurancePlan iPlan on pPlan.InsurancePlanID = iPlan.ID ";



                        string orderBy = string.Format(" ORDER BY v.ID OFFSET " + skipPage + " ROWS FETCH NEXT " + CVisit.PerPage + " ROWS ONLY ;      ");

                        ostring = selectStatement + WhereStatement + orderBy;

                        SqlCommand ocmd = new SqlCommand(ostring, myconnection);
                        myconnection.Open();

                        using (SqlDataReader oreader = ocmd.ExecuteReader())
                        {
                            while (oreader.Read())
                            {

                                data2.Add(new GVisit()
                                {
                                    patientID = oreader["patientID"].ToString() != "" ? long.Parse(oreader["patientID"].ToString()) : 0,
                                    AccountNum = oreader["accountnum"].ToString(),
                                    Patient = oreader["patient"].ToString(),
                                    DOS = oreader["dos"].ToString(),
                                    EntryDate = oreader["entrydate"].ToString(),
                                    PracticeID = oreader["PracticeID"].ToString() != "" ? long.Parse(oreader["PracticeID"].ToString()) : 0,
                                    LocationID = oreader["LocationID"].ToString() != "" ? long.Parse(oreader["LocationID"].ToString()) : 0,
                                    Location = oreader["Location"].ToString(),
                                    SubmittedDate = oreader["SubmittedDate"].ToString(),
                                    ClaimAge = oreader["ClaimAge"].ToString(),
                                    BilledAmount = oreader["BilledAmount"].ToString() != "" ? decimal.Parse(oreader["BilledAmount"].ToString()) : 0,
                                    AllowedAmount = oreader["AllowedAmount"].ToString() != "" ? decimal.Parse(oreader["AllowedAmount"].ToString()) : 0,
                                    PaidAmount = oreader["PaidAmount"].ToString() != "" ? decimal.Parse(oreader["PaidAmount"].ToString()) : 0,
                                    PrimaryStatus = oreader["PrimaryStatus"].ToString(),
                                    AdjustmentAmount = oreader["AdjustmentAmount"].ToString() != "" ? decimal.Parse(oreader["AdjustmentAmount"].ToString()) : 0,
                                    PrimaryPlanBalance = oreader["PrimaryPlanBalance"].ToString() != "" ? decimal.Parse(oreader["PrimaryPlanBalance"].ToString()) : 0,
                                    PrimaryPatientBalance = oreader["PrimaryPatientBalance"].ToString() != "" ? decimal.Parse(oreader["PrimaryPatientBalance"].ToString()) : 0,
                                    Rejection = oreader["Rejection"].ToString(),
                                    ProviderID = oreader["ProviderID"].ToString() != "" ? long.Parse(oreader["ProviderID"].ToString()) : 0,
                                    Provider = oreader["Provider"].ToString(),
                                    VisitID = oreader["VisitID"].ToString() != "" ? long.Parse(oreader["VisitID"].ToString()) : 0,
                                    InsurancePlanID = oreader["InsurancePlanID"].ToString() != "" ? long.Parse(oreader["InsurancePlanID"].ToString()) : 0,
                                    InsurancePlanName = oreader["InsurancePlanName"].ToString(),
                                    SubscriberID = oreader["SubscriberID"].ToString(),
                                    PrimaryPatientPlanID = oreader["PrimaryPatientPlanID"].ToString(),
                                    SecondaryStatus = oreader["SecondaryStatus"].ToString(),
                                    SecondaryPlanBalance = oreader["SecondaryPlanBalance"].ToString() != "" ? decimal.Parse(oreader["SecondaryPlanBalance"].ToString()) : 0,
                                    SecondaryPatientBalance = oreader["SecondaryPatientBalance"].ToString() != "" ? decimal.Parse(oreader["SecondaryPatientBalance"].ToString()) : 0,
                                    Edi837PayerID = oreader["Edi837PayerID"].ToString() != "" ? long.Parse(oreader["Edi837PayerID"].ToString()) : 0,
                                    VisitType = oreader["VisitType"].ToString(),


                                });
                            }
                            myconnection.Close();
                        }
                    }
                    //List<GVisit> data2 = new List<GVisit>();
                    //using (SqlConnection myconnection = new SqlConnection(newConnection))
                    //{
                    //    selectStatement = "select pat.ID patientID, " + selection + "pat.AccountNum AccountNum, (pat.LastName + ', ' + pat.FirstName) Patient,convert(varchar, v.DateOfServiceFrom, 101)   DOS,  'PrimaryStatus' = CASE when v.PrimaryStatus = 'N' then 'New Charge'    when v.PrimaryStatus = 'S' then 'Submitted'  when v.PrimaryStatus = 'K' then '999 Accepted' When v.PrimaryStatus = 'D' then '999 Denied'  when v.PrimaryStatus = 'E' then '999 has Errors' when v.PrimaryStatus = 'P' then 'Paid'  when v.PrimaryStatus = 'DN' then 'Denied' when v.PrimaryStatus = 'PT_P' then 'Patient Paid' when v.PrimaryStatus = 'PPTS' then 'Transefered to Sec.' when v.PrimaryStatus = 'PPTT' then 'Paid-Transfered To Ter' when v.PrimaryStatus = 'PPTP' then 'Transfered to Patient' when v.PrimaryStatus = 'SPTP' then 'Paid-Transfered To Patient' when v.PrimaryStatus = 'SPTT' then 'Paid-Transfered To Ter' when v.PrimaryStatus = 'PR_TP' then 'Pat. Resp. Transferred to Pat' when v.PrimaryStatus = 'PPTM' then 'Paid - Medigaped' when v.PrimaryStatus = 'M' then 'Medigaped' when v.PrimaryStatus = 'R' then 'Rejected' when v.PrimaryStatus = 'A1AY' then 'Received By Clearing House' when v.PrimaryStatus = 'A0PR' then 'Forwarded  to Payer' when v.PrimaryStatus = 'A1PR' then 'Received By Payer' when v.PrimaryStatus = 'A2PR' then 'Accepted By Payer' when v.PrimaryStatus = 'TS' then 'Transferred to Secondary' when v.PrimaryStatus = 'TT' then 'Transferred to Tertiary' when v.PrimaryStatus = 'PTPT' then 'Plan to Patient Transfer' when v.PrimaryStatus = 'RS' then 'Re-Submitted' END ,  'SecondaryStatus' = CASE when v.SecondaryStatus = 'N' then 'New Charge'    when v.SecondaryStatus = 'S' then 'Submitted'  when v.SecondaryStatus = 'K' then '999 Accepted' When v.SecondaryStatus = 'D' then '999 Denied'  when v.SecondaryStatus = 'E' then '999 has Errors' when v.SecondaryStatus = 'P' then 'Paid'  when v.SecondaryStatus = 'DN' then 'Denied' when v.SecondaryStatus = 'PT_P' then 'Patient Paid' when v.SecondaryStatus = 'PPTS' then 'Transefered to Sec.' when v.SecondaryStatus = 'PPTT' then 'Paid-Transfered To Ter' when v.SecondaryStatus = 'PPTP' then 'Transfered to Patient' when v.SecondaryStatus = 'SPTP' then 'Paid-Transfered To Patient' when v.SecondaryStatus = 'SPTT' then 'Paid-Transfered To Ter' when v.SecondaryStatus = 'PR_TP' then 'Pat. Resp. Transferred to Pat' when v.SecondaryStatus = 'PPTM' then 'Paid - Medigaped' when v.SecondaryStatus = 'M' then 'Medigaped' when v.SecondaryStatus = 'R' then 'Rejected' when v.SecondaryStatus = 'A1AY' then 'Received By Clearing House' when v.SecondaryStatus = 'A0PR' then 'Forwarded  to Payer' when v.SecondaryStatus = 'A1PR' then 'Received By Payer' when v.SecondaryStatus = 'A2PR' then 'Accepted By Payer' when v.SecondaryStatus = 'TS' then 'Transferred to Secondary' when v.SecondaryStatus = 'TT' then 'Transferred to Tertiary' when v.SecondaryStatus = 'PTPT' then 'Plan to Patient Transfer' when v.SecondaryStatus = 'RS' then 'Re-Submitted' END,convert(varchar, v.AddedDate, 101)  EntryDate, v.ID PracticeID, 'VisitType'= Case when v.VisitType='I' Then 'Institutional' when v.VisitType='P' Then 'P' when v.VisitType='D' Then 'D' end , loc.ID LocationID, loc.[Name] [Location],convert(varchar, v.SubmittedDate, 101)  SubmittedDate, v.PrimaryBilledAmount BilledAmount,v.PrimaryAllowed AllowedAmount, v.PrimaryPaid PaidAmount, v.PrimaryWriteOff AdjustmentAmount,v.PrimaryBal PrimaryPlanBalance, v.PrimaryPatientBal PrimaryPatientBalance, v.RejectionReason Rejection, prov.ID ProviderID, v.ID VisitID, prov.[Name] [Provider]  , iPlan.ID InsurancePlanID,iPlan.PlanName  InsurancePlanName, pPlan.SubscriberId SubscriberID, v.PrimaryPatientPlanID PrimaryPatientPlanID, v.SecondaryBal SecondaryPlanBalance, v.SecondaryPatientBal SecondaryPatientBalance, iPlan.Edi837PayerID Edi837PayerID from Visit v join Patient pat on v.PatientID = pat.ID join [Location] loc on v.LocationID = loc.ID join[Provider] prov on v.ProviderID = prov.ID join PatientPlan pPlan on v.PrimaryPatientPlanID = pPlan.ID join InsurancePlan iPlan on pPlan.InsurancePlanID = iPlan.ID";

                    //    ostring = selectStatement + " " + ostring;

                    //   // ostring += string.Format(" ORDER BY patientID OFFSET " + skipPage + " ROWS FETCH NEXT " + CVisit.PerPage + " ROWS ONLY ;      ");

                    //    SqlCommand ocmd = new SqlCommand(ostring, myconnection);
                    //    myconnection.Open();

                    //    using (SqlDataReader oreader = ocmd.ExecuteReader())
                    //    {
                    //        while (oreader.Read())
                    //        {

                    //            data2.Add(new GVisit()
                    //            {
                    //                patientID = oreader["patientID"].ToString() != "" ? long.Parse(oreader["patientID"].ToString()) : 0,
                    //                AccountNum = oreader["accountnum"].ToString(),
                    //                Patient = oreader["patient"].ToString(),
                    //                DOS = oreader["dos"].ToString(),
                    //                EntryDate = oreader["entrydate"].ToString(),
                    //                PracticeID = oreader["PracticeID"].ToString() != "" ? long.Parse(oreader["PracticeID"].ToString()) : 0,
                    //                LocationID = oreader["LocationID"].ToString() != "" ? long.Parse(oreader["LocationID"].ToString()) : 0,
                    //                Location = oreader["Location"].ToString(),
                    //                SubmittedDate = oreader["SubmittedDate"].ToString(),
                    //                ClaimAge = oreader["ClaimAge"].ToString(),
                    //                BilledAmount = oreader["BilledAmount"].ToString() != "" ? decimal.Parse(oreader["BilledAmount"].ToString()) : 0,
                    //                AllowedAmount = oreader["AllowedAmount"].ToString() != "" ? decimal.Parse(oreader["AllowedAmount"].ToString()) : 0,
                    //                PaidAmount = oreader["PaidAmount"].ToString() != "" ? decimal.Parse(oreader["PaidAmount"].ToString()) : 0,
                    //                PrimaryStatus = oreader["PrimaryStatus"].ToString(),
                    //                AdjustmentAmount = oreader["AdjustmentAmount"].ToString() != "" ? decimal.Parse(oreader["AdjustmentAmount"].ToString()) : 0,
                    //                PrimaryPlanBalance = oreader["PrimaryPlanBalance"].ToString() != "" ? decimal.Parse(oreader["PrimaryPlanBalance"].ToString()) : 0,
                    //                PrimaryPatientBalance = oreader["PrimaryPatientBalance"].ToString() != "" ? decimal.Parse(oreader["PrimaryPatientBalance"].ToString()) : 0,
                    //                Rejection = oreader["Rejection"].ToString(),
                    //                ProviderID = oreader["ProviderID"].ToString() != "" ? long.Parse(oreader["ProviderID"].ToString()) : 0,
                    //                Provider = oreader["Provider"].ToString(),
                    //                VisitID = oreader["VisitID"].ToString() != "" ? long.Parse(oreader["VisitID"].ToString()) : 0,
                    //                InsurancePlanID = oreader["InsurancePlanID"].ToString() != "" ? long.Parse(oreader["InsurancePlanID"].ToString()) : 0,
                    //                InsurancePlanName = oreader["InsurancePlanName"].ToString(),
                    //                SubscriberID = oreader["SubscriberID"].ToString(),
                    //                PrimaryPatientPlanID = oreader["PrimaryPatientPlanID"].ToString(),
                    //                SecondaryStatus = oreader["SecondaryStatus"].ToString(),
                    //                SecondaryPlanBalance = oreader["SecondaryPlanBalance"].ToString() != "" ? decimal.Parse(oreader["SecondaryPlanBalance"].ToString()) : 0,
                    //                SecondaryPatientBalance = oreader["SecondaryPatientBalance"].ToString() != "" ? decimal.Parse(oreader["SecondaryPatientBalance"].ToString()) : 0,
                    //                Edi837PayerID = oreader["Edi837PayerID"].ToString() != "" ? long.Parse(oreader["Edi837PayerID"].ToString()) : 0,
                    //                VisitType = oreader["VisitType"].ToString(),


                    //            });
                    //        }
                    //        myconnection.Close();
                    //    }
                    //}

                    if (CVisit.pageNo != 1)
                    {
                        totalCount = -1;
                    }
                    perPage = CVisit.PerPage;
                    currentPage = CVisit.pageNo;
                    objPatient = data2;

                }

                var z = new { data = objPatient, TotalCount = totalCount, totalPages = totalPages, PerPage = perPage, CurrentPage = currentPage };
                return Ok(z);

            }
            catch (Exception)
            {
                throw;
            }



        }





        [Route("FindVisits3")]
        [HttpPost]
        public async Task<ActionResult> FindVisits3(CVisit CVisit)
        {
            int skipPage = (CVisit.pageNo - 1) * CVisit.PerPage;
            int totalCount = 0, totalPages = 0, perPage = 0, currentPage = 0;
            List<GVisit> objPatient = new List<GVisit>();
            try
            {
                if (skipPage >= 0)
                {
                    long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
                    var Client = (from w in _contextMain.MainPractice
                                  where w.ID == PracticeId
                                  select w
                                ).FirstOrDefault();
                    string contextName = _contextMain.MainClient.Find(Client.ClientID)?.ContextName;
                    string newConnection = GetConnectionStringManager(contextName);
                    List<GVisit> data3 = new List<GVisit>();
                    List<GVisit> dataCount = new List<GVisit>();




                    List<GVisit> data2 = new List<GVisit>();
                    using (SqlConnection myconnection = new SqlConnection(newConnection))
                    {
                        string selection = "";
                        if (CVisit.AgeType == "S")
                        {
                            selection += (" (ISNULL(DATEDIFF(day,  v.SubmittedDate,GETDATE()), 0 ))  ClaimAge,");
                        }
                        else if (CVisit.AgeType == "D")
                        {
                            selection += (" (ISNULL(DATEDIFF(day,  v.DateOfServiceFrom,GETDATE()), 0 ))  ClaimAge,");
                        }
                        else if (CVisit.AgeType == "E")
                        {
                            selection += (" (ISNULL(DATEDIFF(day,  v.AddedDate,GETDATE()), 0 ))  ClaimAge,");
                        }

                        string ostring = "select pat.ID patientID, " + selection + "pat.AccountNum AccountNum, (pat.LastName + ', ' + pat.FirstName) Patient,convert(varchar, v.DateOfServiceFrom, 101)   DOS,  'PrimaryStatus' = CASE when v.PrimaryStatus = 'N' then 'New Charge'    when v.PrimaryStatus = 'S' then 'Submitted'  when v.PrimaryStatus = 'K' then '999 Accepted' When v.PrimaryStatus = 'D' then '999 Denied'  when v.PrimaryStatus = 'E' then '999 has Errors' when v.PrimaryStatus = 'P' then 'Paid'  when v.PrimaryStatus = 'DN' then 'Denied' when v.PrimaryStatus = 'PT_P' then 'Patient Paid' when v.PrimaryStatus = 'PPTS' then 'Transefered to Sec.' when v.PrimaryStatus = 'PPTT' then 'Paid-Transfered To Ter' when v.PrimaryStatus = 'PPTP' then 'Transfered to Patient' when v.PrimaryStatus = 'SPTP' then 'Paid-Transfered To Patient' when v.PrimaryStatus = 'SPTT' then 'Paid-Transfered To Ter' when v.PrimaryStatus = 'PR_TP' then 'Pat. Resp. Transferred to Pat' when v.PrimaryStatus = 'PPTM' then 'Paid - Medigaped' when v.PrimaryStatus = 'M' then 'Medigaped' when v.PrimaryStatus = 'R' then 'Rejected' when v.PrimaryStatus = 'A1AY' then 'Received By Clearing House' when v.PrimaryStatus = 'A0PR' then 'Forwarded  to Payer' when v.PrimaryStatus = 'A1PR' then 'Received By Payer' when v.PrimaryStatus = 'A2PR' then 'Accepted By Payer' when v.PrimaryStatus = 'TS' then 'Transferred to Secondary' when v.PrimaryStatus = 'TT' then 'Transferred to Tertiary' when v.PrimaryStatus = 'PTPT' then 'Plan to Patient Transfer' when v.PrimaryStatus = 'RS' then 'Re-Submitted' END ,  'SecondaryStatus' = CASE when v.SecondaryStatus = 'N' then 'New Charge'    when v.SecondaryStatus = 'S' then 'Submitted'  when v.SecondaryStatus = 'K' then '999 Accepted' When v.SecondaryStatus = 'D' then '999 Denied'  when v.SecondaryStatus = 'E' then '999 has Errors' when v.SecondaryStatus = 'P' then 'Paid'  when v.SecondaryStatus = 'DN' then 'Denied' when v.SecondaryStatus = 'PT_P' then 'Patient Paid' when v.SecondaryStatus = 'PPTS' then 'Transefered to Sec.' when v.SecondaryStatus = 'PPTT' then 'Paid-Transfered To Ter' when v.SecondaryStatus = 'PPTP' then 'Transfered to Patient' when v.SecondaryStatus = 'SPTP' then 'Paid-Transfered To Patient' when v.SecondaryStatus = 'SPTT' then 'Paid-Transfered To Ter' when v.SecondaryStatus = 'PR_TP' then 'Pat. Resp. Transferred to Pat' when v.SecondaryStatus = 'PPTM' then 'Paid - Medigaped' when v.SecondaryStatus = 'M' then 'Medigaped' when v.SecondaryStatus = 'R' then 'Rejected' when v.SecondaryStatus = 'A1AY' then 'Received By Clearing House' when v.SecondaryStatus = 'A0PR' then 'Forwarded  to Payer' when v.SecondaryStatus = 'A1PR' then 'Received By Payer' when v.SecondaryStatus = 'A2PR' then 'Accepted By Payer' when v.SecondaryStatus = 'TS' then 'Transferred to Secondary' when v.SecondaryStatus = 'TT' then 'Transferred to Tertiary' when v.SecondaryStatus = 'PTPT' then 'Plan to Patient Transfer' when v.SecondaryStatus = 'RS' then 'Re-Submitted' END,convert(varchar, v.AddedDate, 101)  EntryDate, v.ID PracticeID, 'VisitType'= Case when v.VisitType='I' Then 'Institutional' when v.VisitType='P' Then 'P' when v.VisitType='D' Then 'D' end , loc.ID LocationID, loc.[Name] [Location],convert(varchar, v.SubmittedDate, 101)  SubmittedDate, v.PrimaryBilledAmount BilledAmount,v.PrimaryAllowed AllowedAmount, v.PrimaryPaid PaidAmount, v.PrimaryWriteOff AdjustmentAmount,v.PrimaryBal PrimaryPlanBalance, v.PrimaryPatientBal PrimaryPatientBalance, v.RejectionReason Rejection, prov.ID ProviderID, v.ID VisitID, prov.[Name] [Provider]  , iPlan.ID InsurancePlanID,iPlan.PlanName  InsurancePlanName, pPlan.SubscriberId SubscriberID, v.PrimaryPatientPlanID PrimaryPatientPlanID, v.SecondaryBal SecondaryPlanBalance, v.SecondaryPatientBal SecondaryPatientBalance, iPlan.Edi837PayerID Edi837PayerID from Visit v join Patient pat on v.PatientID = pat.ID join [Location] loc on v.LocationID = loc.ID join[Provider] prov on v.ProviderID = prov.ID join PatientPlan pPlan on v.PrimaryPatientPlanID = pPlan.ID join InsurancePlan iPlan on pPlan.InsurancePlanID = iPlan.ID";


                        if (!CVisit.ChargeID.IsNull())
                        {
                            ostring += " join  Charge c on v.ID = c.VisitID ";
                        }
                        if (!CVisit.CPTCode.IsNull())
                        {
                            if (!CVisit.ChargeID.IsNull())
                            {
                                ostring += "  join Cpt cpt on c.CPTID = cpt.ID ";
                            }
                            else
                            {
                                ostring += " join Charge c on v.ID = c.VisitID join Cpt cpt on c.CPTID = cpt.ID ";
                            }
                        }


                        ostring += " where v.practiceid = {0} ";
                        ostring = string.Format(ostring, PracticeId);


                        if (!CVisit.LastName.IsNull())
                        {
                            if (CVisit.LastName.Contains("'"))
                            {
                                //Modify ContextName
                                string RLastName = CVisit.LastName.Trim();
                                RLastName = RLastName.Replace("'", "''");
                                ostring += string.Format(" and pat.LastName like '%{0}%'", RLastName);
                            }
                            else
                            {
                                ostring += string.Format(" and pat.LastName like '%{0}%'", CVisit.LastName);
                            }

                        }


                        if (!CVisit.FirstName.IsNull())
                        {
                            if (CVisit.FirstName.Contains("'"))
                            {
                                //Modify ContextName
                                string RFirstName = CVisit.FirstName.Trim();
                                RFirstName = RFirstName.Replace("'", "''");
                                ostring += string.Format(" and pat.FirstName like '%{0}%'", RFirstName);
                            }
                            else
                            {
                                ostring += string.Format(" and pat.FirstName like '%{0}%'", CVisit.FirstName);
                            }

                        }

                        if (!CVisit.AccountNum.IsNull())
                            ostring += string.Format(" and pat.AccountNum ='{0}'", CVisit.AccountNum);
                        if (!CVisit.Location.IsNull())
                            ostring += string.Format(" and  loc.Name like '%{0}%'", CVisit.Location);
                        if (!CVisit.Provider.IsNull())
                            ostring += string.Format(" and prov.Name like '%{0}%'", CVisit.Provider);
                        if (!CVisit.SubscriberID.IsNull())
                            ostring += string.Format(" and pPlan.SubscriberId ='{0}'", CVisit.SubscriberID);
                        if (!CVisit.Plan.IsNull())
                            ostring += string.Format(" and iPlan.PlanName like '%{0}%'", CVisit.Plan);
                        if (!CVisit.VisitID.IsNull())
                            ostring += string.Format(" and v.ID ='{0}'", CVisit.VisitID);
                        if (!CVisit.BatchID.IsNull())
                            ostring += string.Format(" and v.BatchDocumentID ='{0}'", CVisit.BatchID);

                        if (!CVisit.CPTCode.IsNull())
                        {
                            ostring += string.Format(" and cpt.CPTCode ='{0}'", CVisit.CPTCode);
                        }
                        if (!CVisit.ChargeID.IsNull())
                        {
                            ostring += string.Format(" and c.ID ='{0}'", CVisit.ChargeID);
                        }
                        if (!CVisit.PayerID.IsNull())
                        {
                            ostring += string.Format(" and iPlan.Edi837PayerID ='{0}'", CVisit.PayerID);
                        }



                        if (!CVisit.VisitType.IsNull())
                            ostring += string.Format(" and v.VisitType like '%{0}%'", CVisit.VisitType);


                        if (CVisit.EntryDateFrom != null && CVisit.EntryDateTo != null)
                        {
                            ostring += (" and (v.AddedDate  >= '" + CVisit.EntryDateFrom.GetValueOrDefault().Date + "' and v.AddedDate  < '" + CVisit.EntryDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                        }
                        else if (CVisit.EntryDateFrom != null && CVisit.EntryDateTo == null)
                        {
                            ostring += (" and ( v.AddedDate  >= '" + CVisit.EntryDateFrom.GetValueOrDefault().Date + "')");

                        }
                        else if (CVisit.EntryDateFrom == null && CVisit.EntryDateTo != null)
                        {
                            ostring += (" and (v.AddedDate  <= '" + CVisit.EntryDateTo.GetValueOrDefault().Date + "')");
                        }


                        if (CVisit.SubmittedFromDate != null && CVisit.SubmittedToDate != null)
                        {
                            ostring += (" and (v.SubmittedDate  >= '" + CVisit.SubmittedFromDate.GetValueOrDefault().Date + "' and v.SubmittedDate  < '" + CVisit.SubmittedToDate.GetValueOrDefault().Date.AddDays(1) + "')");
                        }
                        else if (CVisit.SubmittedFromDate != null && CVisit.SubmittedToDate == null)
                        {
                            ostring += (" and ( v.SubmittedDate  >= '" + CVisit.SubmittedFromDate.GetValueOrDefault().Date + "')");

                        }
                        else if (CVisit.SubmittedFromDate == null && CVisit.SubmittedToDate != null)
                        {
                            ostring += (" and (v.SubmittedDate  <= '" + CVisit.SubmittedToDate.GetValueOrDefault().Date + "')");
                        }




                        if (CVisit.DosFrom != null && CVisit.DosTo != null)
                        {
                            ostring += (" and (v.DateOfServiceFrom  >= '" + CVisit.DosFrom.GetValueOrDefault().Date + "' and v.DateOfServiceFrom  < '" + CVisit.DosTo.GetValueOrDefault().Date.AddDays(1) + "')");
                        }
                        else if (CVisit.DosFrom != null && CVisit.DosTo == null)
                        {
                            ostring += (" and ( v.DateOfServiceFrom  >= '" + CVisit.DosFrom.GetValueOrDefault().Date + "')");

                        }
                        else if (CVisit.DosFrom == null && CVisit.DosTo != null)
                        {
                            ostring += (" and (v.DateOfServiceFrom  <= '" + CVisit.DosTo.GetValueOrDefault().Date + "')");
                        }



                        if (CVisit.InsuranceType == "P")
                        {
                            ostring += (" and ( ISNULL(v.PrimaryPatientPlanID, 0 )  > 0 and  v.SecondaryPatientPlanID is null and v.TertiaryPatientPlanID is null)");
                        }
                        else if (CVisit.InsuranceType == "S")
                        {
                            ostring += (" and ( ISNULL(v.PrimaryPatientPlanID, 0 ) > 0 and  ISNULL(v.SecondaryPatientPlanID, 0 )  > 0 and v.TertiaryPatientPlanID is null )");
                        }
                        else if (CVisit.InsuranceType == "T")
                        {
                            ostring += (" and (  ISNULL(v.PrimaryPatientPlanID, 0 ) > 0 and  ISNULL(v.SecondaryPatientPlanID, 0 ) > 0  and  ISNULL(v.TertiaryPatientPlanID, 0 ) > 0 )");
                        }
                        else
                        {
                            ostring += (" and (ISNULL(v.PrimaryPatientPlanID, 0 ) > 0)");
                        }


                        if (!CVisit.SubmissionType.IsNull())
                        {
                            ostring += string.Format(" and iPlan.SubmissionType ='{0}'", CVisit.SubmissionType);
                        }

                        if (!CVisit.Status.IsNull())
                        {
                            if (CVisit.Status == "SystemRejected")
                            {
                                ostring += string.Format(" and ( v.ValidationMessage is not  null AND v.ValidationMessage !='' ) ");
                            }
                            else
                            {
                                ostring += string.Format(" and ( v.PrimaryStatus = '" + CVisit.Status + "' OR v.SecondaryStatus = '" + CVisit.Status + "' ) ");
                            }
                        }
                        if (CVisit.IsSubmitted == "Y")
                        {
                            ostring += string.Format(" and v.IsSubmitted = 'true' ");
                        }
                        else if (CVisit.IsSubmitted == "N")
                            ostring += string.Format(" and v.IsSubmitted = 'false'   ");

                        if (!CVisit.IsPaid.IsNull())
                        {
                            if (CVisit.InsuranceType == "Y")
                            {
                                ostring += string.Format(" and (ISNULL(v.PrimaryPaid, 0 ) > 0 and ISNULL(v.PrimaryBal, 0 ) + ISNULL(v.SecondaryBal, 0 ) + ISNULL(v.TertiaryBal, 0 ) = 0 )");
                            }
                            else
                            {
                                if (CVisit.InsuranceType == "P")
                                {
                                    ostring += string.Format(" and ( ISNULL(v.PrimaryPaid, 0 ) > 0 and ISNULL(v.PrimaryBal, 0 ) + ISNULL(v.SecondaryBal, 0 ) + ISNULL(v.TertiaryBal, 0 ) > 0  )");
                                }
                                else
                                {
                                    ostring += string.Format(" and ISNULL(v.PrimaryPaid, 0 ) = 0");

                                }
                            }
                        }


                        // ostring += string.Format(" ORDER BY v.ID OFFSET " + skipPage + " ROWS FETCH NEXT " + CVisit.PerPage + " ROWS ONLY ;      ");

                        SqlCommand ocmd = new SqlCommand(ostring, myconnection);
                        myconnection.Open();

                        using (SqlDataReader oreader = ocmd.ExecuteReader())
                        {
                            while (oreader.Read())
                            {

                                data2.Add(new GVisit()
                                {
                                    patientID = oreader["patientID"].ToString() != "" ? long.Parse(oreader["patientID"].ToString()) : 0,
                                    AccountNum = oreader["accountnum"].ToString(),
                                    Patient = oreader["patient"].ToString(),
                                    DOS = oreader["dos"].ToString(),
                                    EntryDate = oreader["entrydate"].ToString(),
                                    PracticeID = oreader["PracticeID"].ToString() != "" ? long.Parse(oreader["PracticeID"].ToString()) : 0,
                                    LocationID = oreader["LocationID"].ToString() != "" ? long.Parse(oreader["LocationID"].ToString()) : 0,
                                    Location = oreader["Location"].ToString(),
                                    SubmittedDate = oreader["SubmittedDate"].ToString(),
                                    ClaimAge = oreader["ClaimAge"].ToString(),
                                    BilledAmount = oreader["BilledAmount"].ToString() != "" ? decimal.Parse(oreader["BilledAmount"].ToString()) : 0,
                                    AllowedAmount = oreader["AllowedAmount"].ToString() != "" ? decimal.Parse(oreader["AllowedAmount"].ToString()) : 0,
                                    PaidAmount = oreader["PaidAmount"].ToString() != "" ? decimal.Parse(oreader["PaidAmount"].ToString()) : 0,
                                    PrimaryStatus = oreader["PrimaryStatus"].ToString(),
                                    AdjustmentAmount = oreader["AdjustmentAmount"].ToString() != "" ? decimal.Parse(oreader["AdjustmentAmount"].ToString()) : 0,
                                    PrimaryPlanBalance = oreader["PrimaryPlanBalance"].ToString() != "" ? decimal.Parse(oreader["PrimaryPlanBalance"].ToString()) : 0,
                                    PrimaryPatientBalance = oreader["PrimaryPatientBalance"].ToString() != "" ? decimal.Parse(oreader["PrimaryPatientBalance"].ToString()) : 0,
                                    Rejection = oreader["Rejection"].ToString(),
                                    ProviderID = oreader["ProviderID"].ToString() != "" ? long.Parse(oreader["ProviderID"].ToString()) : 0,
                                    Provider = oreader["Provider"].ToString(),
                                    VisitID = oreader["VisitID"].ToString() != "" ? long.Parse(oreader["VisitID"].ToString()) : 0,
                                    InsurancePlanID = oreader["InsurancePlanID"].ToString() != "" ? long.Parse(oreader["InsurancePlanID"].ToString()) : 0,
                                    InsurancePlanName = oreader["InsurancePlanName"].ToString(),
                                    SubscriberID = oreader["SubscriberID"].ToString(),
                                    PrimaryPatientPlanID = oreader["PrimaryPatientPlanID"].ToString(),
                                    SecondaryStatus = oreader["SecondaryStatus"].ToString(),
                                    SecondaryPlanBalance = oreader["SecondaryPlanBalance"].ToString() != "" ? decimal.Parse(oreader["SecondaryPlanBalance"].ToString()) : 0,
                                    SecondaryPatientBalance = oreader["SecondaryPatientBalance"].ToString() != "" ? decimal.Parse(oreader["SecondaryPatientBalance"].ToString()) : 0,
                                    Edi837PayerID = oreader["Edi837PayerID"].ToString() != "" ? long.Parse(oreader["Edi837PayerID"].ToString()) : 0,
                                    VisitType = oreader["VisitType"].ToString(),


                                });
                            }
                            myconnection.Close();
                        }
                    }



                    totalCount = data2.Count();
                    totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalCount) / CVisit.PerPage));
                    perPage = CVisit.PerPage;
                    currentPage = CVisit.pageNo;
                    objPatient = data2.OrderBy(a => a.VisitID).Skip(skipPage).Take(CVisit.PerPage).ToList();

                }

                var z = new { data = objPatient, TotalCount = totalCount, totalPages = totalPages, PerPage = perPage, CurrentPage = currentPage };
                return Ok(z);

            }
            catch (Exception)
            {
                throw;
            }
        }

        private IConfiguration configuration;

        public string GetConnectionStringManager(string contextName)
        {
            HttpContextAccessor httpContextAccessor = new HttpContextAccessor();

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            configuration = builder.Build();
            string connectionString = configuration.GetConnectionString("MedifusionLocal");
            string[] splitString = connectionString.Split(';');
            splitString[1] = splitString[1];

            if (contextName.IsNull())
                contextName = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("TEMP", StringComparison.InvariantCultureIgnoreCase)).Value;

            connectionString = splitString[0] + "; " + splitString[1] + contextName + "; " + splitString[2] + "; " + splitString[3];
            return connectionString;
            //var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            //return builder.Build().GetSection("ConnectionStrings").GetSection("MedifusionLocal").Value;
        }

    }
}
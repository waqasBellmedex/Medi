using System;
using System.IO;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Dashboard;
using MediFusionPM.Models;
using MediFusionPM.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApplication1.ChartModel;
using static MediFusionPM.Dashboard.BillingDashboard;
using static MediFusionPM.ViewModels.VMCommon;
using static MediFusionPM.ViewModels.VMElectronicSubmission;
using static MediFusionPM.ViewModels.VMPaperSubmission;
using static MediFusionPM.ViewModels.VMPatientFollowup;
using static MediFusionPM.ViewModels.VMPaymentCheck;
using static MediFusionPM.ViewModels.VMPlanFollowUp;
using static MediFusionPM.ViewModels.VMSubmissionLog;
using static MediFusionPM.ViewModels.VMVisit;
using System.Data.SqlClient;
using MediFusionPM.Uitilities;

namespace MediFusionPM.DashboardController
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingDashboardController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private IConfiguration configuration;

        public BillingDashboardController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
        }

        [Route("BillingSummary")]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<BillingDashboard>> BillingSummary()
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
            );

            string contextName = User.Claims.FirstOrDefault(x => x.Type.Equals("TEMP", StringComparison.InvariantCultureIgnoreCase)).Value;

            BillingDashboard obj = new BillingDashboard();
            obj.OverallSummary(_context, _contextMain, UD.PracticeID, UD.ClientID, UD.Email, UD.Role, UD.UserID, contextName);

            return obj;
        }



        [Route("BillingClaimAndERASummary")]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<BillingDashboard>> BillingClaimAndERASummary()
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
            );

            string contextName = User.Claims.FirstOrDefault(x => x.Type.Equals("TEMP", StringComparison.InvariantCultureIgnoreCase)).Value;

            BillingDashboard obj = new BillingDashboard();
            obj.BillingClaimAndERASummary(_context, _contextMain, UD.PracticeID, UD.ClientID, UD.Email, UD.Role, UD.UserID, contextName);

            return obj;
        }

        [Route("BillingFollowUpAndOtherSummary")]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<BillingDashboard>> BillingFollowUpAndOtherSummary()
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
            );

            string contextName = User.Claims.FirstOrDefault(x => x.Type.Equals("TEMP", StringComparison.InvariantCultureIgnoreCase)).Value;

            BillingDashboard obj = new BillingDashboard();
            obj.BillingFollowUpAndOtherSummary(_context, _contextMain, UD.PracticeID, UD.ClientID, UD.Email, UD.Role, UD.UserID, contextName);

            return obj;
        }

        [Route("BillingDailySummary")]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<BillingDashboard>> BillingDailySummary()
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
            );

            string contextName = User.Claims.FirstOrDefault(x => x.Type.Equals("TEMP", StringComparison.InvariantCultureIgnoreCase)).Value;

            BillingDashboard obj = new BillingDashboard();
            obj.BillingDailySummary(_context, _contextMain, UD.PracticeID, UD.ClientID, UD.Email, UD.Role, UD.UserID, contextName);

            return obj;
        }













        [Route("GetPlanFollowUpData")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ReasonVisitAmmount>>> GetPlanFollowUpData(SearchBillingDoard searchBillingDoard)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
           );

            if (searchBillingDoard.Value == "DOS")
            {
                return await (from pfu in _context.PlanFollowUp
                              join r in _context.Reason
                              on pfu.ReasonID equals r.ID
                              join visittable in _context.Visit on pfu.VisitID equals visittable.ID
                              join practice in _context.Practice
                              on visittable.PracticeID equals practice.ID
                              where (visittable.DateOfServiceFrom.HasValue ? true : false) && (visittable.PracticeID == UD.PracticeID)
                              group new { amount = visittable.PatientAmount.HasValue ? visittable.PatientAmount : 0 }
                              by new { name = r.Name, r.ID } into gp
                              select new ReasonVisitAmmount
                              {
                                  VisitCount = gp.Count(),
                                  ReasonName = gp.Key.name,
                                  TotalAmmount = (long)gp.Select(a => a.amount).Sum()
                              }
                            ).ToAsyncEnumerable().ToList();
            }
            else if (searchBillingDoard.Value == "AD")
            {
                return await (from pfu in _context.PlanFollowUp
                              join r in _context.Reason
                              on pfu.ReasonID equals r.ID
                              join visittable in _context.Visit on pfu.VisitID equals visittable.ID
                              join practice in _context.Practice
                              on visittable.PracticeID equals practice.ID
                              where (visittable.AddedDate.Date.IsNull()) && (visittable.PracticeID == UD.PracticeID)
                              group new { amount = visittable.PatientAmount.HasValue ? visittable.PatientAmount : 0 } by new { name = r.Name, r.ID } into gp
                              select new ReasonVisitAmmount
                              {
                                  VisitCount = gp.Count(),
                                  ReasonName = gp.Key.name,
                                  TotalAmmount = (long)gp.Select(a => a.amount).Sum()
                              }
                            ).ToAsyncEnumerable().ToList();
            }
            else
            {
                return await (from pfu in _context.PlanFollowUp
                              join r in _context.Reason
                              on pfu.ReasonID equals r.ID
                              join visittable in _context.Visit on pfu.VisitID equals visittable.ID
                              join practice in _context.Practice
                              on visittable.PracticeID equals practice.ID
                              where (visittable.SubmittedDate.HasValue ? true : false) && (visittable.PracticeID == UD.PracticeID)
                              group new { amount = visittable.PatientAmount.HasValue ? visittable.PatientAmount : 0 } by new { name = r.Name, r.ID } into gp
                              select new ReasonVisitAmmount
                              {
                                  VisitCount = gp.Count(),
                                  ReasonName = gp.Key.name,
                                  TotalAmmount = (long)gp.Select(a => a.amount).Sum()
                              }
                            ).ToAsyncEnumerable().ToList();
            }

        }

        //Done
        [Route("GetPlanFollowUpGraph")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ReasonVisitAmmountGraph>>> GetPlanFollowUpGraph(SearchBillingDoard searchBillingDoard)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
            );

            if (searchBillingDoard.Value == "DOS")
            {
                return await (from pfu in _context.PlanFollowUp
                              join r in _context.Reason
                              on pfu.ReasonID equals r.ID
                              join visittable in _context.Visit on pfu.VisitID equals visittable.ID
                              join practice in _context.Practice
                              on visittable.PracticeID equals practice.ID
                              where (visittable.DateOfServiceFrom.HasValue ? true : false) && (visittable.PracticeID == UD.PracticeID)
                              group new { amount = visittable.PatientAmount.HasValue ? visittable.PatientAmount : 0 }
                              by new { name = r.Name, r.ID } into gp
                              select new ReasonVisitAmmountGraph
                              {
                                  VisitCount = gp.Count(),
                                  ReasonName = gp.Key.name
                              }
                     ).ToAsyncEnumerable().ToList();
            }
            else if (searchBillingDoard.Value == "AD")
            {
                return await (from pfu in _context.PlanFollowUp
                              join r in _context.Reason
                              on pfu.ReasonID equals r.ID
                              join visittable in _context.Visit on pfu.VisitID equals visittable.ID
                              join practice in _context.Practice
                             on visittable.PracticeID equals practice.ID
                              where (visittable.AddedDate.Date.IsNull()) && (visittable.PracticeID == UD.PracticeID)
                              group new { amount = visittable.PatientAmount.HasValue ? visittable.PatientAmount : 0 }
                              by new { name = r.Name, r.ID } into gp
                              select new ReasonVisitAmmountGraph
                              {
                                  VisitCount = gp.Count(),
                                  ReasonName = gp.Key.name
                              }
                    ).ToAsyncEnumerable().ToList();
            }
            else
            {
                return await (from pfu in _context.PlanFollowUp
                              join r in _context.Reason
                              on pfu.ReasonID equals r.ID
                              join visittable in _context.Visit on pfu.VisitID equals visittable.ID
                              join practice in _context.Practice
                             on visittable.PracticeID equals practice.ID
                              where (visittable.SubmittedDate.HasValue ? true : false) && (visittable.PracticeID == UD.PracticeID)
                              group new { amount = visittable.PatientAmount.HasValue ? visittable.PatientAmount : 0 }
                              by new { name = r.Name, r.ID } into gp
                              select new ReasonVisitAmmountGraph
                              {
                                  VisitCount = gp.Count(),
                                  ReasonName = gp.Key.name
                              }
                ).ToAsyncEnumerable().ToList();
            }



        }

        [Route("GetChargePaymentData")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<GetChargePayment>>> GetChargePaymentData(CVisitAndChargeCO cVisitAndCharge)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
            );

            DateTime date = DateTime.Now.AddMonths(-5);
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);

            if (cVisitAndCharge.Value == "DOS")
            {

                return await (from v in _context.Visit
                              join pat in _context.Patient on v.PatientID equals pat.ID
                              join fac in _context.Practice
                              on pat.PracticeID equals fac.ID

                              join loc in _context.Location
                              on pat.LocationId equals loc.ID
                              join prov in _context.Provider
                              on pat.ProviderID equals prov.ID
                              join pPlan in _context.PatientPlan
                              on v.PrimaryPatientPlanID equals pPlan.ID
                              join iPlan in _context.InsurancePlan
                              on pPlan.InsurancePlanID equals iPlan.ID
                              where (v.DateOfServiceFrom.Value <= DateTime.Now) && (v.DateOfServiceFrom.Value >= firstDayOfMonth)
                              && (v.PracticeID == UD.PracticeID)
                              group new
                              {
                                  PrimaryBilled = (v.PrimaryBilledAmount.Val()) + (v.SecondaryBilledAmount.Val()) + (v.TertiaryBilledAmount.Val()),
                                  PrimaryPaid = (v.PrimaryPaid.Val()) + (v.SecondaryPaid.Val()) + (v.TertiaryPaid.Val()),
                                  PatientPaid = (v.PatientPaid.Val()),
                                  PrimaryWrtOff = (v.PrimaryWriteOff.Val()) + (v.SecondaryWriteOff.Val()) + (v.TertiaryWriteOff.Val()),
                                  PrimaryBalance = (v.PrimaryBal.Val()) + (v.SecondaryPaid.Val()) + (v.TertiaryPaid.Val()),
                                  PrimaryPatientBal = (v.PrimaryPatientBal.Val()) + (v.SecondaryPatientBal.Val()) + (v.TertiaryPatientBal.Val()),
                                  Payment = (v.PrimaryPaid.Val()) + (v.SecondaryPaid.Val()) + (v.TertiaryPaid.Val()) + (v.PatientPaid.Val())
                              } by new { Month = v.DateOfServiceFrom.Value.Month, Year = v.DateOfServiceFrom.Value.Year } into gp
                              select new GetChargePayment
                              {
                                  Year = gp.Key.Year,
                                  Month = ParseMonth(gp.Key.Month),
                                  Charge = (long)gp.Select(m => m.PrimaryBilled).Sum(),
                                  PlanPayment = (long)gp.Select(m => m.PrimaryPaid).Sum(),
                                  PatientPaid = (long)gp.Select(m => m.PatientPaid).Sum(),
                                  Adjustments = (long)gp.Select(m => m.PrimaryWrtOff).Sum(),
                                  PlanBal = (long)gp.Select(m => m.PrimaryBalance).Sum(),
                                  PatBal = (long)gp.Select(m => m.PrimaryPatientBal).Sum(),
                                  Payment = (long)gp.Select(m => m.Payment).Sum()
                              }
           ).ToAsyncEnumerable().OrderByDescending(m => m.Year).ThenByDescending(m => ParseMonthToInt(m.Month)).ToList();

            }
            else if (cVisitAndCharge.Value == "AD")
            {

                return await (from v in _context.Visit
                              join pat in _context.Patient on v.PatientID equals pat.ID
                              join fac in _context.Practice
                              on pat.PracticeID equals fac.ID
                              join loc in _context.Location
                              on pat.LocationId equals loc.ID
                              join prov in _context.Provider
                              on pat.ProviderID equals prov.ID
                              join pPlan in _context.PatientPlan
                              on v.PrimaryPatientPlanID equals pPlan.ID
                              join iPlan in _context.InsurancePlan
                              on pPlan.InsurancePlanID equals iPlan.ID
                              where (v.AddedDate) <= DateTime.Now && (v.AddedDate) >= (firstDayOfMonth)
                              && (v.PracticeID == UD.PracticeID)
                              group new
                              {
                                  PrimaryBilled = (v.PrimaryBilledAmount.Val()) + (v.SecondaryBilledAmount.Val()) + (v.TertiaryBilledAmount.Val()),
                                  PrimaryPaid = (v.PrimaryPaid.Val()) + (v.SecondaryPaid.Val()) + (v.TertiaryPaid.Val()),
                                  PatientPaid = (v.PatientPaid.Val()),
                                  PrimaryWrtOff = (v.PrimaryWriteOff.Val()) + (v.SecondaryWriteOff.Val()) + (v.TertiaryWriteOff.Val()),
                                  PrimaryBalance = (v.PrimaryBal.Val()) + (v.SecondaryPaid.Val()) + (v.TertiaryPaid.Val()),
                                  PrimaryPatientBal = (v.PrimaryPatientBal.Val()) + (v.SecondaryPatientBal.Val()) + (v.TertiaryPatientBal.Val()),
                                  Payment = (v.PrimaryPaid.Val()) + (v.SecondaryPaid.Val()) + (v.TertiaryPaid.Val()) + (v.PatientPaid.Val())
                              } by new { Month = v.AddedDate.Month, Year = v.AddedDate.Year } into gp
                              select new GetChargePayment
                              {
                                  Year = gp.Key.Year,
                                  Month = ParseMonth(gp.Key.Month),
                                  Charge = (long)gp.Select(m => m.PrimaryBilled).Sum(),
                                  PlanPayment = (long)gp.Select(m => m.PrimaryPaid).Sum(),
                                  PatientPaid = (long)gp.Select(m => m.PatientPaid).Sum(),
                                  Adjustments = (long)gp.Select(m => m.PrimaryWrtOff).Sum(),
                                  PlanBal = (long)gp.Select(m => m.PrimaryBalance).Sum(),
                                  PatBal = (long)gp.Select(m => m.PrimaryPatientBal).Sum(),
                                  Payment = (long)gp.Select(m => m.Payment).Sum()
                              }
                            ).ToAsyncEnumerable().OrderByDescending(m => m.Year).ThenByDescending(m => ParseMonthToInt(m.Month)).ToList();
            }
            else
            {
                return await (from v in _context.Visit
                              join pat in _context.Patient on v.PatientID equals pat.ID
                              join fac in _context.Practice
                              on pat.PracticeID equals fac.ID
                              join loc in _context.Location
                              on pat.LocationId equals loc.ID
                              join prov in _context.Provider
                              on pat.ProviderID equals prov.ID
                              join pPlan in _context.PatientPlan
                              on v.PrimaryPatientPlanID equals pPlan.ID
                              join iPlan in _context.InsurancePlan
                              on pPlan.InsurancePlanID equals iPlan.ID
                              where (v.SubmittedDate.Value) <= DateTime.Now && (v.SubmittedDate.Value) >= (firstDayOfMonth) && (v.PracticeID == UD.PracticeID)
                              group new
                              {
                                  PrimaryBilled = (v.PrimaryBilledAmount.Val()) + (v.SecondaryBilledAmount.Val()) + (v.TertiaryBilledAmount.Val()),
                                  PrimaryPaid = (v.PrimaryPaid.Val()) + (v.SecondaryPaid.Val()) + (v.TertiaryPaid.Val()),
                                  PatientPaid = (v.PatientPaid.Val()),
                                  PrimaryWrtOff = (v.PrimaryWriteOff.Val()) + (v.SecondaryWriteOff.Val()) + (v.TertiaryWriteOff.Val()),
                                  PrimaryBalance = (v.PrimaryBal.Val()) + (v.SecondaryPaid.Val()) + (v.TertiaryPaid.Val()),
                                  PrimaryPatientBal = (v.PrimaryPatientBal.Val()) + (v.SecondaryPatientBal.Val()) + (v.TertiaryPatientBal.Val()),
                                  Payment = (v.PrimaryPaid.Val()) + (v.SecondaryPaid.Val()) + (v.TertiaryPaid.Val()) + (v.PatientPaid.Val())
                              } by new { Month = v.SubmittedDate.Value.Month, Year = v.SubmittedDate.Value.Year } into gp
                              select new GetChargePayment
                              {
                                  Year = gp.Key.Year,
                                  Month = ParseMonth(gp.Key.Month),
                                  Charge = (long)gp.Select(m => m.PrimaryBilled).Sum(),
                                  PlanPayment = (long)gp.Select(m => m.PrimaryPaid).Sum(),
                                  PatientPaid = (long)gp.Select(m => m.PatientPaid).Sum(),
                                  Adjustments = (long)gp.Select(m => m.PrimaryWrtOff).Sum(),
                                  PlanBal = (long)gp.Select(m => m.PrimaryBalance).Sum(),
                                  PatBal = (long)gp.Select(m => m.PrimaryPatientBal).Sum(),
                                  Payment = (long)gp.Select(m => m.Payment).Sum()
                              }
           ).ToAsyncEnumerable().OrderByDescending(m => m.Year).ThenByDescending(m => ParseMonthToInt(m.Month)).ToList();
            }

        }


        [Route("GetChargePaymentGraph")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<GetChargePayment>>> GetChargePaymentGraph(SearchBillingDoard searchBillingDoard)
        {

            //var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            //var RoleClaim = User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase));
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            var Client = (from w in _contextMain.MainPractice
                          where w.ID == PracticeId
                          select w
                        ).FirstOrDefault();
            string contextName = _contextMain.MainClient.Find(Client.ClientID)?.ContextName;
            string newConnection = GetConnectionStringManager(contextName);

            if (searchBillingDoard.Value == "DOS")
            {
                List<GetChargePayment> chargePayment = new List<GetChargePayment>();
                using (SqlConnection myConnection = new SqlConnection(newConnection))
                {

                    string oString = "select  MONTH(v.DateOfServiceFrom) as m, Year(v.DateOfServiceFrom) as y,SUM(ISNULL(v.PrimaryBilledAmount, 0) + ISNULL(v.SecondaryBilledAmount, 0) + ISNULL(v.TertiaryBilledAmount, 0)) as PrimaryBilled,SUM(ISNULL(v.PrimaryPaid, 0) + ISNULL(v.SecondaryPaid, 0) + ISNULL(v.TertiaryPaid, 0)) as PrimaryPaid,SUM(ISNULL(v.PatientPaid, 0)) as PatientPaid,SUM(ISNULL(v.PrimaryWriteOff, 0) + ISNULL(v.SecondaryWriteOff, 0) + ISNULL(v.TertiaryWriteOff, 0)) as PrimaryWrtOff ,SUM(ISNULL(v.PrimaryBal, 0) + ISNULL(v.SecondaryBal, 0) + ISNULL(v.TertiaryBal, 0)) as PrimaryBalance ,SUM(ISNULL(v.PrimaryPatientBal, 0) + ISNULL(v.SecondaryPatientBal, 0) + ISNULL(v.TertiaryPatientBal, 0)) as PrimaryPatientBal ,SUM(ISNULL(v.PrimaryPaid, 0) + ISNULL(v.SecondaryPaid, 0) + ISNULL(v.TertiaryPaid, 0) + ISNULL(v.PatientPaid, 0)) as Payment from Visit v join Patient on v.PatientID = Patient.ID join Practice on Patient.PracticeID = Practice.ID join [Location] on Patient.LocationId = [Location].ID join [Provider] on Patient.ProviderID = [Provider].ID join PatientPlan on v.PrimaryPatientPlanID = PatientPlan.ID join InsurancePlan on PatientPlan.InsurancePlanID = InsurancePlan.ID where (v.DateOfServiceFrom <= CURRENT_TIMESTAMP) AND(v.DateOfServiceFrom >= (SELECT DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - 5, 0))) AND v.PracticeID = " + PracticeId + " GROUP BY MONTH(v.DateOfServiceFrom), Year(v.DateOfServiceFrom) ORDER BY y DESC, m DESC";
                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            var chaPayment = new GetChargePayment();
                            chaPayment.YearMonth = ParseMonth(Convert.ToInt32(oReader["m"].ToString())) + " " + oReader["y"].ToString();
                            chaPayment.Year = Convert.ToInt32(oReader["y"].ToString());
                            chaPayment.Month = ParseMonth(Convert.ToInt32(oReader["m"].ToString()));
                            chaPayment.Charge = Convert.ToDecimal(oReader["PrimaryBilled"].ToString());
                            chaPayment.PlanPayment = Convert.ToDecimal(oReader["PrimaryPaid"].ToString());
                            chaPayment.PatientPaid = Convert.ToDecimal(oReader["PatientPaid"].ToString());
                            chaPayment.Adjustments = Convert.ToDecimal(oReader["PrimaryWrtOff"].ToString());
                            chaPayment.PlanBal = Convert.ToDecimal(oReader["PrimaryBalance"].ToString());
                            chaPayment.PatBal = Convert.ToDecimal(oReader["PrimaryPatientBal"].ToString());
                            chaPayment.Payment = Convert.ToDecimal(oReader["Payment"].ToString());
                            chaPayment.TotalBal = chaPayment.Payment = Convert.ToDecimal(oReader["PrimaryBilled"].ToString()) - Convert.ToDecimal(oReader["PrimaryPaid"].ToString()) - Convert.ToDecimal(oReader["PatientPaid"].ToString()) - Convert.ToDecimal(oReader["PrimaryWrtOff"].ToString());

                            chargePayment.Add(chaPayment);
                        }
                        myConnection.Close();
                    }
                }

                return chargePayment;



            }
            else if (searchBillingDoard.Value == "AD")
            {

                List<GetChargePayment> chargePayment = new List<GetChargePayment>();
                using (SqlConnection myConnection = new SqlConnection(newConnection))
                {

                    string oString = "select  MONTH(v.AddedDate) as m, Year(v.AddedDate) as y,SUM(ISNULL(v.PrimaryBilledAmount, 0) + ISNULL(v.SecondaryBilledAmount, 0) + ISNULL(v.TertiaryBilledAmount, 0)) as PrimaryBilled,SUM(ISNULL(v.PrimaryPaid, 0) + ISNULL(v.SecondaryPaid, 0) + ISNULL(v.TertiaryPaid, 0)) as PrimaryPaid,SUM(ISNULL(v.PatientPaid, 0)) as PatientPaid,SUM(ISNULL(v.PrimaryWriteOff, 0) + ISNULL(v.SecondaryWriteOff, 0) + ISNULL(v.TertiaryWriteOff, 0)) as PrimaryWrtOff ,SUM(ISNULL(v.PrimaryBal, 0) + ISNULL(v.SecondaryBal, 0) + ISNULL(v.TertiaryBal, 0)) as PrimaryBalance ,SUM(ISNULL(v.PrimaryPatientBal, 0) + ISNULL(v.SecondaryPatientBal, 0) + ISNULL(v.TertiaryPatientBal, 0)) as PrimaryPatientBal ,SUM(ISNULL(v.PrimaryPaid, 0) + ISNULL(v.SecondaryPaid, 0) + ISNULL(v.TertiaryPaid, 0) + ISNULL(v.PatientPaid, 0)) as Payment from Visit v join Patient on v.PatientID = Patient.ID join Practice on Patient.PracticeID = Practice.ID join [Location] on Patient.LocationId = [Location].ID join [Provider] on Patient.ProviderID = [Provider].ID join PatientPlan on v.PrimaryPatientPlanID = PatientPlan.ID join InsurancePlan on PatientPlan.InsurancePlanID = InsurancePlan.ID where (v.AddedDate <= CURRENT_TIMESTAMP) AND(v.AddedDate >= (SELECT DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - 5, 0))) AND v.PracticeID = " + PracticeId + " GROUP BY MONTH(v.AddedDate), Year(v.AddedDate) ORDER BY y DESC, m DESC";
                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            var chaPayment = new GetChargePayment();
                            chaPayment.YearMonth = ParseMonth(Convert.ToInt32(oReader["m"].ToString())) + " " + oReader["y"].ToString();
                            chaPayment.Year = Convert.ToInt32(oReader["y"].ToString());
                            chaPayment.Month = ParseMonth(Convert.ToInt32(oReader["m"].ToString()));
                            chaPayment.Charge = Convert.ToDecimal(oReader["PrimaryBilled"].ToString());
                            chaPayment.PlanPayment = Convert.ToDecimal(oReader["PrimaryPaid"].ToString());
                            chaPayment.PatientPaid = Convert.ToDecimal(oReader["PatientPaid"].ToString());
                            chaPayment.Adjustments = Convert.ToDecimal(oReader["PrimaryWrtOff"].ToString());
                            chaPayment.PlanBal = Convert.ToDecimal(oReader["PrimaryBalance"].ToString());
                            chaPayment.PatBal = Convert.ToDecimal(oReader["PrimaryPatientBal"].ToString());
                            chaPayment.Payment = Convert.ToDecimal(oReader["Payment"].ToString());
                            chaPayment.TotalBal = chaPayment.Payment = Convert.ToDecimal(oReader["PrimaryBilled"].ToString()) - Convert.ToDecimal(oReader["PrimaryPaid"].ToString()) - Convert.ToDecimal(oReader["PatientPaid"].ToString()) - Convert.ToDecimal(oReader["PrimaryWrtOff"].ToString());

                            chargePayment.Add(chaPayment);
                        }
                        myConnection.Close();
                    }
                }

                return chargePayment;


            }
            else if (searchBillingDoard.Value == "PD")
            {
                List<GetChargePayment> chargePayment = new List<GetChargePayment>();
                using (SqlConnection myConnection = new SqlConnection(newConnection))
                {

                    string oString = "select  MONTH(v.AddedDate) as m, Year(v.AddedDate) as y,SUM(ISNULL(v.PrimaryBilledAmount, 0) + ISNULL(v.SecondaryBilledAmount, 0) + ISNULL(v.TertiaryBilledAmount, 0)) as PrimaryBilled,SUM(ISNULL(v.PrimaryPaid, 0) + ISNULL(v.SecondaryPaid, 0) + ISNULL(v.TertiaryPaid, 0)) as PrimaryPaid,SUM(ISNULL(v.PatientPaid, 0)) as PatientPaid,SUM(ISNULL(v.PrimaryWriteOff, 0) + ISNULL(v.SecondaryWriteOff, 0) + ISNULL(v.TertiaryWriteOff, 0)) as PrimaryWrtOff ,SUM(ISNULL(v.PrimaryBal, 0) + ISNULL(v.SecondaryBal, 0) + ISNULL(v.TertiaryBal, 0)) as PrimaryBalance ,SUM(ISNULL(v.PrimaryPatientBal, 0) + ISNULL(v.SecondaryPatientBal, 0) + ISNULL(v.TertiaryPatientBal, 0)) as PrimaryPatientBal ,SUM(ISNULL(v.PrimaryPaid, 0) + ISNULL(v.SecondaryPaid, 0) + ISNULL(v.TertiaryPaid, 0) + ISNULL(v.PatientPaid, 0)) as Payment from Visit v join Patient on v.PatientID = Patient.ID join Practice on Patient.PracticeID = Practice.ID join [Location] on Patient.LocationId = [Location].ID join [Provider] on Patient.ProviderID = [Provider].ID join PatientPlan on v.PrimaryPatientPlanID = PatientPlan.ID join InsurancePlan on PatientPlan.InsurancePlanID = InsurancePlan.ID where (v.AddedDate <= CURRENT_TIMESTAMP) AND(v.AddedDate >= (SELECT DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - 5, 0))) AND v.PracticeID = " + PracticeId + " GROUP BY MONTH(v.AddedDate), Year(v.AddedDate) ORDER BY y DESC, m DESC";
                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            var chaPayment = new GetChargePayment();
                            chaPayment.YearMonth = ParseMonth(Convert.ToInt32(oReader["m"].ToString())) + " " + oReader["y"].ToString();
                            chaPayment.Year = Convert.ToInt32(oReader["y"].ToString());
                            chaPayment.Month = ParseMonth(Convert.ToInt32(oReader["m"].ToString()));
                            chaPayment.Charge = Convert.ToDecimal(oReader["PrimaryBilled"].ToString());
                            //chaPayment.PlanPayment = Convert.ToDecimal(oReader["PrimaryPaid"].ToString());
                            chaPayment.PatientPaid = Convert.ToDecimal(oReader["PatientPaid"].ToString());
                            //chaPayment.Adjustments = Convert.ToDecimal(oReader["PrimaryWrtOff"].ToString());
                            chaPayment.PlanBal = Convert.ToDecimal(oReader["PrimaryBalance"].ToString());
                            chaPayment.PatBal = Convert.ToDecimal(oReader["PrimaryPatientBal"].ToString());
                            chaPayment.Payment = Convert.ToDecimal(oReader["Payment"].ToString());
                            chaPayment.TotalBal = chaPayment.Payment = Convert.ToDecimal(oReader["PrimaryBilled"].ToString()) - Convert.ToDecimal(oReader["PrimaryPaid"].ToString()) - Convert.ToDecimal(oReader["PatientPaid"].ToString()) - Convert.ToDecimal(oReader["PrimaryWrtOff"].ToString());

                            chargePayment.Add(chaPayment);
                        }
                        myConnection.Close();
                    }
                }




                List<GetChargePayment> chargePayment2 = new List<GetChargePayment>();
                using (SqlConnection myConnection = new SqlConnection(newConnection))
                {

                    string oString = "select MONTH(pc.posteddate) as m, Year(pc.posteddate) as y, sum(paidamount) as paidamount,sum(WriteoffAmount) as Adjustment from paymentcharge pc  where status = 'P' and chargeid is not null  GROUP BY MONTH(pc.posteddate), Year(pc.posteddate) ORDER BY y DESC, m DESC"; SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            var chaPayment = new GetChargePayment();
                            chaPayment.YearMonth = ParseMonth(Convert.ToInt32(oReader["m"].ToString())) + " " + oReader["y"].ToString();
                            chaPayment.Year = Convert.ToInt32(oReader["y"].ToString());
                            chaPayment.Month = ParseMonth(Convert.ToInt32(oReader["m"].ToString()));
                            chaPayment.PlanPayment = Convert.ToDecimal(oReader["paidamount"].ToString());
                            chaPayment.Adjustments = Convert.ToDecimal(oReader["Adjustment"].ToString());

                            chargePayment2.Add(chaPayment);
                        }
                        myConnection.Close();
                    }
                }

                List<GetChargePayment> chargePayment3 = new List<GetChargePayment>();

                foreach (var item in chargePayment)
                {
                    foreach (var item2 in chargePayment2)
                    {
                        if (item.Month == item2.Month && item.Year == item2.Year)
                        {
                            item.PlanPayment = item2.PlanPayment;
                            item.Adjustments = item2.Adjustments;
                        }

                    }
                    chargePayment3.Add(item);
                }


                return chargePayment3;

            }


            else
            {

                List<GetChargePayment> chargePayment = new List<GetChargePayment>();
                using (SqlConnection myConnection = new SqlConnection(newConnection))
                {

                    string oString = "select  MONTH(v.SubmittedDate) as m, Year(v.SubmittedDate) as y,SUM(ISNULL(v.PrimaryBilledAmount, 0) + ISNULL(v.SecondaryBilledAmount, 0) + ISNULL(v.TertiaryBilledAmount, 0)) as PrimaryBilled,SUM(ISNULL(v.PrimaryPaid, 0) + ISNULL(v.SecondaryPaid, 0) + ISNULL(v.TertiaryPaid, 0)) as PrimaryPaid,SUM(ISNULL(v.PatientPaid, 0)) as PatientPaid,SUM(ISNULL(v.PrimaryWriteOff, 0) + ISNULL(v.SecondaryWriteOff, 0) + ISNULL(v.TertiaryWriteOff, 0)) as PrimaryWrtOff ,SUM(ISNULL(v.PrimaryBal, 0) + ISNULL(v.SecondaryBal, 0) + ISNULL(v.TertiaryBal, 0)) as PrimaryBalance ,SUM(ISNULL(v.PrimaryPatientBal, 0) + ISNULL(v.SecondaryPatientBal, 0) + ISNULL(v.TertiaryPatientBal, 0)) as PrimaryPatientBal ,SUM(ISNULL(v.PrimaryPaid, 0) + ISNULL(v.SecondaryPaid, 0) + ISNULL(v.TertiaryPaid, 0) + ISNULL(v.PatientPaid, 0)) as Payment from Visit v join Patient on v.PatientID = Patient.ID join Practice on Patient.PracticeID = Practice.ID join [Location] on Patient.LocationId = [Location].ID join [Provider] on Patient.ProviderID = [Provider].ID join PatientPlan on v.PrimaryPatientPlanID = PatientPlan.ID join InsurancePlan on PatientPlan.InsurancePlanID = InsurancePlan.ID where (v.SubmittedDate <= CURRENT_TIMESTAMP) AND(v.SubmittedDate >= (SELECT DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - 5, 0))) AND v.PracticeID = " + PracticeId + " GROUP BY MONTH(v.SubmittedDate), Year(v.SubmittedDate) ORDER BY y DESC, m DESC";
                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            var chaPayment = new GetChargePayment();
                            chaPayment.YearMonth = ParseMonth(Convert.ToInt32(oReader["m"].ToString())) + " " + oReader["y"].ToString();
                            chaPayment.Year = Convert.ToInt32(oReader["y"].ToString());
                            chaPayment.Month = ParseMonth(Convert.ToInt32(oReader["m"].ToString()));
                            chaPayment.Charge = Convert.ToDecimal(oReader["PrimaryBilled"].ToString());
                            chaPayment.PlanPayment = Convert.ToDecimal(oReader["PrimaryPaid"].ToString());
                            chaPayment.PatientPaid = Convert.ToDecimal(oReader["PatientPaid"].ToString());
                            chaPayment.Adjustments = Convert.ToDecimal(oReader["PrimaryWrtOff"].ToString());
                            chaPayment.PlanBal = Convert.ToDecimal(oReader["PrimaryBalance"].ToString());
                            chaPayment.PatBal = Convert.ToDecimal(oReader["PrimaryPatientBal"].ToString());
                            chaPayment.Payment = Convert.ToDecimal(oReader["Payment"].ToString());
                            chaPayment.TotalBal = chaPayment.Payment = Convert.ToDecimal(oReader["PrimaryBilled"].ToString()) - Convert.ToDecimal(oReader["PrimaryPaid"].ToString()) - Convert.ToDecimal(oReader["PatientPaid"].ToString()) - Convert.ToDecimal(oReader["PrimaryWrtOff"].ToString());

                            chargePayment.Add(chaPayment);
                        }
                        myConnection.Close();
                    }
                }

                return chargePayment;

            }

        }   

        public List<GVisit> FindVisits(CVisit CVisit, long PracticeID, long ClientID, string Email, string Role, string UserID, string contextName)
        {


            string connectionstring = CommonUtil.GetConnectionString(PracticeID, contextName);
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


                string ostring = "select pat.ID patientID from Visit v join Patient pat on v.PatientID = pat.ID join [Location] loc on v.LocationID = loc.ID join[Provider] prov on v.ProviderID = prov.ID join PatientPlan pPlan on v.PrimaryPatientPlanID = pPlan.ID join InsurancePlan iPlan on pPlan.InsurancePlanID = iPlan.ID";


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
                ostring = string.Format(ostring, PracticeID);


                if (!CVisit.LastName.IsNull())
                    ostring += string.Format(" and pat.LastName like '%{0}%'", CVisit.LastName);
                if (!CVisit.FirstName.IsNull())
                    ostring += string.Format(" and pat.FirstName like '%{0}%'", CVisit.FirstName);
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
                    ostring += string.Format(" and ( v.PrimaryStatus = '" + CVisit.Status + "' OR v.SecondaryStatus = '" + CVisit.Status + "' ) ");
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
                            //  AccountNum = oreader["accountnum"].ToString(),
                        });
                    }
                    myconnection.Close();
                }
            }
            return data;
        }


        public List<GPlanFollowup> FindPlanFollowUp(CPlanFollowup CPlanFollowup, long PracticeID, long ClientID, string Email, string Role, string UserID)
        {

            List<GPlanFollowup> data = (from pf in _context.PlanFollowUp
                                        join v in _context.Visit on pf.VisitID equals v.ID
                                        join r1 in _context.Reason on pf.ReasonID equals r1.ID into r11
                                        from r in r11.DefaultIfEmpty()
                                        join g1 in _context.Group on pf.GroupID equals g1.ID into g11
                                        from g in g11.DefaultIfEmpty()
                                        join a1 in _context.Action on pf.ActionID equals a1.ID into a11
                                        from a in a11.DefaultIfEmpty()
                                        join aCode in _context.AdjustmentCode on pf.AdjustmentCodeID equals aCode.ID into Table1
                                        from t1 in Table1.DefaultIfEmpty()
                                        join f in _context.Practice on v.PracticeID equals f.ID
                                        //join up in _context.UserPractices on f.ID equals up.PracticeID
                                        //join u in _context.Users on up.UserID equals u.Id
                                        join l in _context.Location on v.LocationID equals l.ID
                                        join pp in _context.PatientPlan on v.PrimaryPatientPlanID equals pp.ID
                                        join ip in _context.InsurancePlan on pp.InsurancePlanID equals ip.ID
                                        join p in _context.Patient on v.PatientID equals p.ID
                                        join pro in _context.Provider on v.ProviderID equals pro.ID

                                        where v.PracticeID == PracticeID &&
                                        //u.Id.ToString() == UserID &&
                                        (v.PrimaryBal.Val() + v.SecondaryBal.Val() + v.TertiaryBal.Val()) > 0 &&
                                        (pf.TickleDate == null ? true : DateTime.Now > pf.TickleDate) &&

                                       (CPlanFollowup.Resolved == null ? true : pf.Resolved.Equals(CPlanFollowup.Resolved)) &&
                                       (CPlanFollowup.ReasonID.IsNull() ? true : r.ID.Equals(CPlanFollowup.ReasonID)) &&
                                       (CPlanFollowup.GroupID.IsNull() ? true : g.ID.Equals(CPlanFollowup.GroupID)) &&
                                       (CPlanFollowup.ActionID.IsNull() ? true : a.ID.Equals(CPlanFollowup.ActionID)) &&
                                       (CPlanFollowup.Practice.IsNull() ? true : f.Name.ToUpper().Contains(CPlanFollowup.Practice)) &&
                                       (CPlanFollowup.Location.IsNull() ? true : l.Name.ToUpper().Contains(CPlanFollowup.Location)) &&
                                       (CPlanFollowup.PlanName.IsNull() ? true : ip.PlanName.ToUpper().Contains(CPlanFollowup.PlanName)) &&
                                       (CPlanFollowup.VisitID.IsNull() ? true : v.ID.Equals(CPlanFollowup.VisitID)) &&
                                       (CPlanFollowup.AccountNum.IsNull() ? true : p.AccountNum.Equals(CPlanFollowup.AccountNum)) &&
                                       (CPlanFollowup.DOS == null ? true : object.Equals(CPlanFollowup.DOS, v.DateOfServiceFrom)) &&
                                       (CPlanFollowup.SubmitDate == null ? true : CPlanFollowup.SubmitDate == v.SubmittedDate) &&
                                       //(CPlanFollowup.SubmitDate == null ? true : object.Equals(CPlanFollowup.SubmitDate, v.SubmittedDate)) &&
                                       (CPlanFollowup.TickleDate == null ? true : object.Equals(CPlanFollowup.TickleDate, pf.TickleDate))


                                        select new GPlanFollowup()
                                        {
                                            ID = pf.ID,
                                            PlanFollowUpID = pf.ID,
                                            VisitID = pf.VisitID,
                                            DOS = v.DateOfServiceFrom.Format("MM/dd/yyyy"),
                                            AccountNum = p.AccountNum,
                                            Patient = p.LastName + ", " + p.FirstName,
                                            PatientID = p.ID,
                                            Practice = f.Name,
                                            PracticeID = f.ID,
                                            Location = l.Name,
                                            LocationID = l.ID,
                                            Provider = pro.LastName + ", " + pro.FirstName,
                                            ProviderID = pro.ID,
                                            PlanBalance = v.PrimaryBal,
                                            AdjustmentCodeID = pf.AdjustmentCodeID,
                                            AdjustmentCode = t1.Code,
                                            Group = g.Name,
                                            GroupID = g.ID,
                                            Reason = r.Name,
                                            ReasonID = r.ID,
                                            Action = a.Name,
                                            ActionID = a.ID,
                                            RemitCode = pf.RemitCode,
                                            SubmitDate = v.SubmittedDate.Format("MM/dd/yyyy"),
                                            EntryDate = pf.AddedDate.Date().ToString("MM/dd/yyyy"),
                                            TickleDate = pf.TickleDate.Format("MM/dd/yyyy"),
                                            FollowupAge = (pf.AddedDate.Date().IsNull() ? "" : System.DateTime.Now.Subtract(pf.AddedDate.Date()).Days.ToString()),
                                            PlanName = ip.PlanName,
                                            InsurancePlanID = ip.ID,
                                            IsSubmitted = v.IsSubmitted == true ? "Yes" : "No", // Need To put Yes For true
                                            BilledAmount = v.PrimaryBilledAmount.Val()
                                        }).ToList();
            return data;
        }

        public long FindSubmissionLog(CSubmissionLog CSubmissionLog, long PracticeID, long ClientID, string Email, string Role, string UserID)
        {
            List<GSubmissionLog> data = (from sLog in _context.SubmissionLog
                                         join rec in _context.Receiver on sLog.ReceiverID equals rec.ID into r2
                                         from rec in r2.DefaultIfEmpty()
                                         join prac in _context.Practice on sLog.ClientID equals prac.ClientID
                                         //join up in _context.UserPractices on prac.ID equals up.PracticeID
                                         //join u in _context.Users on up.UserID equals u.Id
                                         where prac.ID == PracticeID &&
                                         //&& u.Id.ToString() == UserID &&
                                         // u.IsUserBlock == false &&
                                         (CSubmissionLog.SubmitBatchNumber.IsNull() ? true : sLog.ID.ToString().Equals(CSubmissionLog.SubmitBatchNumber)) &&
                                         (CSubmissionLog.Receiver.IsNull() ? true : rec.Name.ToUpper().Contains(CSubmissionLog.Receiver)) &&
                                         (CSubmissionLog.FormType.IsNull() ? true : sLog.FormType.ToUpper().Contains(CSubmissionLog.FormType)) &&
                                         (CSubmissionLog.SubmitDate == null ? true : CSubmissionLog.SubmitDate == sLog.AddedDate.Date) &&
                                         // (CSubmissionLog.SubmitDate == null ? true :
                                         // (CSubmissionLog.SubmitDate <= sLog.AddedDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59)
                                         // && CSubmissionLog.SubmitDate >= sLog.AddedDate.Date)
                                         //) &&
                                         (CSubmissionLog.SubmitType.IsNull() ? true : sLog.SubmitType.Contains(CSubmissionLog.SubmitType)) &&
                                         (CSubmissionLog.ISAControllNumber.IsNull() ? true : sLog.ISAControlNumber.Equals(CSubmissionLog.ISAControllNumber))&&
                                         (CSubmissionLog.ResolvedErrorMessage.IsNull() ? true : CSubmissionLog.ResolvedErrorMessage == "Y" ? sLog.Resolve == true : CSubmissionLog.ResolvedErrorMessage == "N" ? sLog.Resolve == false : true) 

                                         select new GSubmissionLog()
                                         {
                                             SubmitBatchNumber = sLog.ID,
                                             Receiver = rec.Name,
                                             FormType = sLog.FormType,
                                             SubmitDate = sLog.AddedDate.ToString("MM/dd/yyyy h:mm:ss tt"),
                                             Status = GetStatus(sLog),
                                             ISAControllNumber = sLog.ISAControlNumber,
                                             SubmitType = (sLog.SubmitType == "P" ? "Paper" : (sLog.SubmitType == "E" ? "EDI" : "")),
                                             Notes = sLog.Notes,
                                             NumOfVisits = sLog.ClaimCount,
                                             VisitCount = sLog.ClaimCount,
                                             Resolve = sLog.Resolve == true ? "Yes" : "No"
                                         }).ToList();

            var sum = data.Select(x => x.VisitCount).Sum();
            return sum;

        }



        public List<GVisit> FindVisitsVMess(CVisit CVisit, long PracticeID, long ClientID, string Email, string Role, string UserID)
        {

            List<GVisit> data = (from v in _context.Visit
                                 join pat in _context.Patient on v.PatientID equals pat.ID
                                 join prac in _context.Practice
                                 on pat.PracticeID equals prac.ID
                                 //join up in _context.UserPractices
                                 //on fac.ID equals up.PracticeID
                                 //join u in _context.Users
                                 //on up.UserID equals u.Id
                                 join loc in _context.Location
                                 on pat.LocationId equals loc.ID
                                 join prov in _context.Provider
                                 on pat.ProviderID equals prov.ID
                                 join pPlan in _context.PatientPlan
                                 on v.PrimaryPatientPlanID equals pPlan.ID
                                 join iPlan in _context.InsurancePlan
                                 on pPlan.InsurancePlanID equals iPlan.ID
                                 where prac.ID == PracticeID
                                 //&& u.Id.ToString() == UserID
                                 && (v.ValidationMessage != null && v.ValidationMessage != "") &&
                                 // && u.IsUserBlock == false &&
                                 (CVisit.LastName.IsNull() ? true : pat.LastName.ToUpper().Contains(CVisit.LastName)) &&
                                 (CVisit.FirstName.IsNull() ? true : pat.FirstName.ToUpper().Contains(CVisit.FirstName)) &&
                                 (CVisit.AccountNum.IsNull() ? true : pat.AccountNum.ToUpper().Equals(CVisit.AccountNum)) &&
                                 (CVisit.Practice.IsNull() ? true : prac.Name.ToUpper().Contains(CVisit.Practice)) &&
                                 (CVisit.Location.IsNull() ? true : loc.Name.ToUpper().Contains(CVisit.Location)) &&
                                 (CVisit.Provider.IsNull() ? true : prov.Name.ToUpper().Contains(CVisit.Provider)) &&
                                  // (CVisit.InsuredID.IsNull() ? true : pPlan.InsurancePlanID.Equals(CVisit.InsuredID)) &&
                                  // (CVisit.ChargeID.IsNull() ? true : c.ID.Equals(CVisit.ChargeID))&&   
                                  (CVisit.PayerID.IsNull() ? true : iPlan.PayerID.Equals(CVisit.PayerID)) &&
                                 (CVisit.Plan.IsNull() ? true : iPlan.PlanName.ToUpper().Contains(CVisit.Plan)) &&
                                 (CVisit.VisitID.IsNull() ? true : v.ID.Equals(CVisit.VisitID)) &&
                                 (CVisit.BatchID.IsNull() ? true : v.BatchDocumentID.Equals(CVisit.BatchID)) &&
                                 (CVisit.DosFrom != null && CVisit.DosTo != null ?
                                 ((DateTime)v.DateOfServiceFrom).Date >= CVisit.DosFrom && ((DateTime)v.DateOfServiceTo).Date <= CVisit.DosTo
                                 : (CVisit.DosFrom != null ? ((DateTime)v.DateOfServiceFrom).Date >= CVisit.DosFrom : true)) &&

                                 (CVisit.EntryDateFrom != null && CVisit.EntryDateTo != null ?
                                 ((DateTime)v.AddedDate).Date >= CVisit.EntryDateFrom && ((DateTime)v.AddedDate).Date <= CVisit.EntryDateTo
                                 : (CVisit.EntryDateFrom != null ? ((DateTime)v.AddedDate).Date >= CVisit.EntryDateFrom : true)) &&
                                 (CVisit.InsuranceType.IsNull() ? true : pPlan.Coverage.Equals(CVisit.InsuranceType)) &&
                                 (CVisit.SubmissionType.IsNull() ? true : iPlan.SubmissionType.Equals(CVisit.SubmissionType)) &&
                                 //(CVisit.Status.IsNull() ? true : CVisit.Status == v.PrimaryStatus || CVisit.Status == v.SecondaryStatus)
                                 (CVisit.Status.IsNull() ? true : v.PrimaryStatus.StartsWith(CVisit.Status) || v.SecondaryStatus.StartsWith(CVisit.Status))


                                 //(CVisit.IsSubmitted == null ? true : v.IsSubmitted.Equals(CVisit.IsSubmitted))
                                 // (CVisit.IsSubmitted == false ? true : v.IsSubmitted.Equals(CVisit.IsSubmitted)) 

                                 select new GVisit()
                                 {
                                     patientID = pat.ID,
                                     AccountNum = pat.AccountNum,
                                     Patient = pat.LastName + ", " + pat.FirstName,
                                     DOS = v.DateOfServiceFrom.Format("MM/dd/yyyy"),
                                     EntryDate = v.AddedDate.Format("MM/dd/yyyy"),
                                     PracticeID = prac.ID,
                                     Practice = prac.Name,
                                     LocationID = loc.ID,
                                     Location = loc.Name,
                                     SubmittedDate = v.SubmittedDate.Format("MM/dd/yyyy"),
                                     ClaimAge = (v.SubmittedDate.Date().IsNull() ? "" : System.DateTime.Now.Subtract(v.SubmittedDate.Date()).Days.ToString()),
                                     BilledAmount = v.PrimaryBilledAmount,
                                     AllowedAmount = v.PrimaryAllowed,
                                     PaidAmount = v.PrimaryPaid,
                                     //PrimaryStatus = v.PrimaryStatus == "N" ? "Regular" : (v.PrimaryStatus == "S" ? "Submitted" : (v.PrimaryStatus == "PPTS" ? "Transefered to Sec." : (v.PrimaryStatus == "PPTP" ? "Transfered to Pat" : ""))),
                                     PrimaryStatus = TranslateStatus(v.PrimaryStatus),
                                     AdjustmentAmount = v.PrimaryWriteOff,
                                     PrimaryPlanBalance = v.PrimaryBal,
                                     PrimaryPatientBalance = v.PrimaryPatientBal,
                                     Rejection = v.RejectionReason,
                                     ProviderID = prov.ID,
                                     Provider = prov.Name,
                                     VisitID = v.ID,
                                     InsurancePlanID = iPlan.ID,
                                     InsurancePlanName = iPlan.PlanName,
                                     SubscriberID = pPlan.SubscriberId,
                                     PrimaryPatientPlanID = v.PrimaryPatientPlanID.ToString(),
                                     SecondaryStatus = TranslateStatus(v.SecondaryStatus),
                                     SecondaryPlanBalance = v.SecondaryBal,
                                     SecondaryPatientBalance = v.SecondaryPatientBal

                                 }).ToList();

            if (!CVisit.CPTCode.IsNull())
            {
                data = (from d in data
                        join c in _context.Charge on d.VisitID equals c.VisitID
                        join cpt in _context.Cpt on c.CPTID equals cpt.ID
                        where cpt.CPTCode.Equals(CVisit.CPTCode)
                        select d).Distinct().ToList();
            }
            if (!CVisit.ChargeID.IsNull())
            {
                data = (from d in data
                        join c in _context.Charge on d.VisitID equals c.VisitID
                        where c.ID.Equals(CVisit.ChargeID)
                        select d).Distinct().ToList();
            }
            return data;
        }


        public List<GElectronicSubmission> FindVisits(CElectronicSubmission CModel, long PracticeID, long ClientID, string Email, string Role, string UserID, string contextName)
        {
            Submitter submitter = _context.Submitter.Where(m => m.ReceiverID == CModel.ReceiverID && m.ClientID == ClientID).SingleOrDefault();
            if (submitter == null)
            {
                // return BadRequest("Submitter is not Setup");

            }
            List<GElectronicSubmission> PrimaryVisits
            = (from v in _context.Visit
               join pat in _context.Patient
                            on v.PatientID equals pat.ID
               join prac in _context.Practice
                            on pat.PracticeID equals prac.ID
               join loc in _context.Location
                            on v.LocationID equals loc.ID
               join prov in _context.Provider
                            on v.ProviderID equals prov.ID
               join pPlan in _context.PatientPlan
                            on pat.ID equals pPlan.PatientID
               join insPlan in _context.InsurancePlan
                            on pPlan.InsurancePlanID equals insPlan.ID
               join x12_837 in _context.Edi837Payer
                            on insPlan.Edi837PayerID equals x12_837.ID
               join rec in _context.Receiver
                            on x12_837.ReceiverID equals rec.ID
               join sub in _context.Submitter
                            on rec.ID equals sub.ReceiverID
               //join up in _context.UserPractices on prac.ID equals up.PracticeID
               //join u in _context.Users on up.UserID equals u.Id
               where v.PrimaryPatientPlanID == pPlan.ID &&
                           (insPlan.SubmissionType.Equals("E")) &&
                            prac.ID == PracticeID &&
                            sub.ClientID == ClientID &&
                            (v.IsSubmitted == false) && (v.PrimaryStatus == "N" || v.PrimaryStatus == null || v.PrimaryStatus == "") &&
                             //(v.IsDontPrint == null ? false : v.IsDontPrint == false) &&
                             //(v.IsForcePaper == null ? false : v.IsForcePaper == false) &&
                             v.IsForcePaper == false &&
                             v.IsDontPrint == false &&
                           (sub.ReceiverID.Equals(CModel.ReceiverID)) &&
                            (string.IsNullOrEmpty(CModel.ReceiverID.ToString()) ? true : rec.ID.Equals(CModel.ReceiverID)) &&
                            (string.IsNullOrEmpty(CModel.AccountNum) ? true : pat.AccountNum.Equals(CModel.AccountNum)) &&
                            (string.IsNullOrEmpty(CModel.Practice) ? true : prac.Name.Contains(CModel.Practice)) &&
                            (string.IsNullOrEmpty(CModel.Location) ? true : loc.Name.Contains(CModel.Location)) &&
                            (CModel.Location.IsNull() ? true : loc.Name.Contains(CModel.Location)) &&
                            (string.IsNullOrEmpty(CModel.Provider) ? true : prov.Name.Contains(CModel.Provider)) &&
                            (string.IsNullOrEmpty(CModel.PayerID) ? true : x12_837.PayerID.Equals(CModel.PayerID)) &&
                            (string.IsNullOrEmpty(CModel.PlanName) ? true : insPlan.PlanName.Contains(CModel.PlanName)) &&
                            (CModel.VisitID.IsNull() ? true : v.ID.Equals(CModel.VisitID))
                         && (ExtensionMethods.IsBetweenDOS(CModel.EntryDateTo, CModel.EntryDateFrom, v.AddedDate, v.AddedDate))
               select new GElectronicSubmission()
               {
                   AccountNum = pat.AccountNum,
                   DOS = v.DateOfServiceFrom.Format("MM/dd/yyyy"),
                   PracticeID = prac.ID,
                   Practice = prac.Name,
                   PatientID = pat.ID,
                   PlanName = insPlan.PlanName,
                   Coverage = pPlan.Coverage,
                   PrimaryStatus = v.PrimaryStatus,
                   SecondaryStatus = v.SecondaryStatus,
                   InsurancePlanID = insPlan.ID.ToString(),
                   Patient = pat.LastName + ", " + pat.FirstName,
                   ProviderID = prov.ID,
                   Provider = prov.Name,
                   VisitID = v.ID,
                   ID = 0,
                   LocationID = loc.ID,
                   Location = loc.Name,
                   TotalAmount = v.TotalAmount,
                   ValidationMessage = v.ValidationMessage,
                   IsSubmitted = v.IsSubmitted,
                   subscriberID = pPlan.SubscriberId,
                   VisitEntryDate = v.AddedDate.ToString("MM/dd/yyyy")
               }).ToList<GElectronicSubmission>();

            // Find Those Visits which are submitted to Secondary Insurance
            List<GElectronicSubmission> SecondaryVisits
                        = (from v in _context.Visit
                           join pat in _context.Patient
                            on v.PatientID equals pat.ID
                           join prac in _context.Practice
                            on pat.PracticeID equals prac.ID
                           join loc in _context.Location
                            on v.LocationID equals loc.ID
                           join prov in _context.Provider
                            on v.ProviderID equals prov.ID
                           join pPlan in _context.PatientPlan
                            on pat.ID equals pPlan.PatientID
                           join insPlan in _context.InsurancePlan
                            on pPlan.InsurancePlanID equals insPlan.ID
                           join x12_837 in _context.Edi837Payer
                            on insPlan.Edi837PayerID equals x12_837.ID
                           join rec in _context.Receiver
                            on x12_837.ReceiverID equals rec.ID
                           join sub in _context.Submitter
                            on rec.ID equals sub.ReceiverID
                           //join up in _context.UserPractices on prac.ID equals up.PracticeID
                           //join u in _context.Users on up.UserID equals u.Id

                           where v.SecondaryPatientPlanID == pPlan.ID && v.SecondaryBilledAmount.Val() > 0 &&
                            v.SecondaryBal.Val() > 0 && (v.SecondaryStatus.IsNull() || v.SecondaryStatus == "N") &&
                            (insPlan.SubmissionType.Equals("E")) && (v.IsSubmitted == true) && // Issubmitted set as True fo Secondary
                            prac.ID == PracticeID &&
                            sub.ClientID == ClientID &&
                            v.IsForcePaper == false &&
                            v.IsDontPrint == false &&
                            (sub.ReceiverID.Equals(CModel.ReceiverID)) && //IsChargesAvailable(v.ID) &&
                            (string.IsNullOrEmpty(CModel.ReceiverID.ToString()) ? true : rec.ID.Equals(CModel.ReceiverID)) &&
                            (string.IsNullOrEmpty(CModel.AccountNum) ? true : pat.AccountNum.Equals(CModel.AccountNum)) &&
                            (string.IsNullOrEmpty(CModel.Practice) ? true : prac.Name.Contains(CModel.Practice)) &&
                            (string.IsNullOrEmpty(CModel.Provider) ? true : prov.Name.Contains(CModel.Provider)) &&
                            (CModel.Location.IsNull() ? true : loc.Name.Contains(CModel.Location)) &&
                            (string.IsNullOrEmpty(CModel.PayerID) ? true : x12_837.PayerID.Equals(CModel.PayerID)) &&
                            (string.IsNullOrEmpty(CModel.PlanName) ? true : insPlan.PlanName.Contains(CModel.PlanName)) &&
                            (CModel.VisitID.IsNull() ? true : v.ID.Equals(CModel.VisitID))
                            && (ExtensionMethods.IsBetweenDOS(CModel.EntryDateTo, CModel.EntryDateFrom, v.AddedDate, v.AddedDate))
                           select new GElectronicSubmission()
                           {
                               AccountNum = pat.AccountNum,
                               DOS = v.DateOfServiceFrom.Format("MM/dd/yyyy"),
                               PracticeID = prac.ID,
                               Practice = prac.Name,
                               PatientID = pat.ID,
                               PlanName = insPlan.PlanName,
                               Coverage = pPlan.Coverage,
                               InsurancePlanID = insPlan.ID.ToString(),
                               Patient = pat.LastName + ", " + pat.FirstName,
                               ProviderID = prov.ID,
                               Provider = prov.Name,
                               VisitID = v.ID,
                               ID = 0,
                               LocationID = loc.ID,
                               Location = loc.Name,
                               TotalAmount = v.TotalAmount,
                               ValidationMessage = v.ValidationMessage,
                               IsSubmitted = v.IsSubmitted,
                               subscriberID = pPlan.SubscriberId,
                               VisitEntryDate = v.AddedDate.ToString("MM/dd/yyyy")
                           }).ToList<GElectronicSubmission>();

            List<GElectronicSubmission> newList = new List<GElectronicSubmission>();

            if (PrimaryVisits != null) newList.AddRange(PrimaryVisits);
            if (SecondaryVisits != null) newList.AddRange(SecondaryVisits);
            return newList;

        }


        public List<GPaperSubmission> FindVisitsPaperSubmission(CPaperSubmission CModel, long PracticeID, long ClientID, string Email, string Role, string UserID)
        {
            List<GPaperSubmission> PrimaryVisits = (from v in _context.Visit
                                                    join pat in _context.Patient
                                                    on v.PatientID equals pat.ID
                                                    join prac in _context.Practice
                                                    on v.PracticeID equals prac.ID
                                                    join loc in _context.Location
                                                    on v.LocationID equals loc.ID
                                                    join prov in _context.Provider
                                                    on v.ProviderID equals prov.ID
                                                    join pPlan in _context.PatientPlan
                                                    on v.PrimaryPatientPlanID equals pPlan.ID
                                                    join insPlan in _context.InsurancePlan
                                                    on pPlan.InsurancePlanID equals insPlan.ID
                                                    //join up in _context.UserPractices on fac.ID equals up.PracticeID
                                                    //join u in _context.Users on up.UserID equals u.Id
                                                    where
                                                    v.PracticeID == PracticeID &&
                                                    v.IsDontPrint == false &&
                                                    (insPlan.SubmissionType.Equals("P")) &&
                                                    (string.IsNullOrEmpty(CModel.AccountNum) ? true : pat.AccountNum.Equals(CModel.AccountNum)) &&
                                                    (string.IsNullOrEmpty(CModel.Practice) ? true : prac.Name.Contains(CModel.Practice)) &&
                                                    (string.IsNullOrEmpty(CModel.Provider) ? true : prov.Name.Contains(CModel.Provider)) &&
                                                    (CModel.Location.IsNull() ? true : loc.Name.Contains(CModel.Location)) &&
                                                    (string.IsNullOrEmpty(CModel.PlanName) ? true : insPlan.PlanName.Contains(CModel.PlanName)) &&
                                                    (string.IsNullOrEmpty(CModel.FormType) ? true : insPlan.FormType.Contains(CModel.FormType)) &&
                                                    (CModel.VisitID.IsNull() ? true : v.ID.Equals(CModel.VisitID)) &&
                                                    //(ExtensionMethods.IsBetweenDOS(CModel.EntryDateTo, CModel.EntryDateFrom, v.AddedDate, v.AddedDate))&&
                                                    //  (ExtensionMethods.IsBetweenDOS(CModel.EntryDateTo, CModel.EntryDateFrom, v.AddedDate, v.AddedDate))&&
                                                    (CModel.EntryDateFrom == null && CModel.EntryDateTo == null ? true : CModel.EntryDateFrom != null && CModel.EntryDateTo != null ? v.AddedDate >= CModel.EntryDateFrom && v.AddedDate <= CModel.EntryDateTo : CModel.EntryDateFrom != null ? v.AddedDate >= CModel.EntryDateFrom : false)
                                                    && (v.PrimaryStatus == "N" || v.PrimaryStatus == null || v.PrimaryStatus == "") &&
                                                   (v.IsSubmitted == false) //&& IsChargesAvailable(_context, v.ID)
                                                    select new GPaperSubmission()
                                                    {
                                                        AccountNum = pat.AccountNum,
                                                        DOS = v.DateOfServiceFrom.Format("MM/dd/yyyy"),
                                                        PracticeID = prac.ID,
                                                        Practice = prac.Name,
                                                        PatientID = pat.ID,
                                                        Patient = pat.LastName + ", " + pat.FirstName,
                                                        ProviderID = prov.ID,
                                                        Provider = prov.Name,
                                                        VisitID = v.ID,
                                                        PlanName = insPlan.PlanName,
                                                        InsurancePlanID = insPlan.ID.ToString(),
                                                        ID = 0,
                                                        LocationID = loc.ID,
                                                        Location = loc.Name,
                                                        TotalAmount = v.TotalAmount.Val(),
                                                        ValidationMessage = v.ValidationMessage,
                                                        SubscriberID = pPlan.SubscriberId,
                                                        PrimaryPatientPlanID = v.PrimaryPatientPlanID.ToString(),
                                                        VisitEntryDate = v.AddedDate.ToString("MM/dd/yyyy"),
                                                        //For Testing
                                                        PrimaryStatus = v.PrimaryStatus,
                                                        SecondaryStatus = v.SecondaryStatus,
                                                        //}).Distinct() .ToListAsync();
                                                    }).ToList<GPaperSubmission>();


            //For Seconday Paper Submissions
            List<GPaperSubmission> SecondaryVisits = (from v in _context.Visit
                                                      join pat in _context.Patient
                                                      on v.PatientID equals pat.ID
                                                      join prac in _context.Practice
                                                      on v.PracticeID equals prac.ID
                                                      join loc in _context.Location
                                                      on v.LocationID equals loc.ID
                                                      join prov in _context.Provider
                                                      on v.ProviderID equals prov.ID
                                                      join pPlan in _context.PatientPlan
                                                      on v.SecondaryPatientPlanID equals pPlan.ID
                                                      join insPlan in _context.InsurancePlan
                                                      on pPlan.InsurancePlanID equals insPlan.ID
                                                      //join up in _context.UserPractices on fac.ID equals up.PracticeID
                                                      //join u in _context.Users on up.UserID equals u.Id
                                                      where v.PracticeID == PracticeID &&
                                                      (v.SecondaryPatientPlanID == pPlan.ID) &&
                                                      (v.SecondaryBilledAmount.Val() > 0) &&
                                                      (v.SecondaryBal.Val() > 0) &&
                                                      (v.SecondaryStatus.IsNull() || v.SecondaryStatus == "N") &&
                                                      //(v.SecondaryStatus.Equals("N")) && 
                                                      (insPlan.SubmissionType.Equals("P")) && (v.IsSubmitted == true) && // Issubmitted set as True fo Secondary
                                                      v.IsDontPrint == false &&
                                                      (string.IsNullOrEmpty(CModel.AccountNum) ? true : pat.AccountNum.Equals(CModel.AccountNum)) &&
                                                      (string.IsNullOrEmpty(CModel.Practice) ? true : prac.Name.Contains(CModel.Practice)) &&
                                                      (string.IsNullOrEmpty(CModel.Provider) ? true : prov.Name.Contains(CModel.Provider)) &&
                                                      (CModel.Location.IsNull() ? true : loc.Name.Contains(CModel.Location)) &&
                                                      (string.IsNullOrEmpty(CModel.PlanName) ? true : insPlan.PlanName.Contains(CModel.PlanName)) &&
                                                      (string.IsNullOrEmpty(CModel.FormType) ? true : insPlan.FormType.Contains(CModel.FormType)) &&
                                                      (CModel.VisitID.IsNull() ? true : v.ID.Equals(CModel.VisitID)) &&
                                                     // (ExtensionMethods.IsBetweenDOS(CModel.EntryDateTo, CModel.EntryDateFrom, v.AddedDate, v.AddedDate))
                                                     (CModel.EntryDateFrom == null && CModel.EntryDateTo == null ? true : CModel.EntryDateFrom != null && CModel.EntryDateTo != null ? v.AddedDate >= CModel.EntryDateFrom && v.AddedDate <= CModel.EntryDateTo : CModel.EntryDateFrom != null ? v.AddedDate >= CModel.EntryDateFrom : false)
                                                      select new GPaperSubmission()
                                                      {
                                                          AccountNum = pat.AccountNum,
                                                          DOS = v.DateOfServiceFrom.Format("MM/dd/yyyy"),
                                                          PracticeID = prac.ID,
                                                          Practice = prac.Name,
                                                          PatientID = pat.ID,
                                                          Patient = pat.LastName + ", " + pat.FirstName,
                                                          ProviderID = prov.ID,
                                                          Provider = prov.Name,
                                                          VisitID = v.ID,
                                                          PlanName = insPlan.PlanName,
                                                          InsurancePlanID = insPlan.ID.ToString(),
                                                          ID = 0,
                                                          LocationID = loc.ID,
                                                          Location = loc.Name,
                                                          TotalAmount = v.TotalAmount.Val(),
                                                          ValidationMessage = v.ValidationMessage,
                                                          SubscriberID = pPlan.SubscriberId,
                                                          PrimaryPatientPlanID = v.PrimaryPatientPlanID.ToString(),
                                                          VisitEntryDate = v.AddedDate.ToString("MM/dd/yyyy"),
                                                          //For Testing
                                                          PrimaryStatus = v.PrimaryStatus,
                                                          SecondaryStatus = v.SecondaryStatus
                                                          //}).Distinct() .ToListAsync();
                                                      }).ToList<GPaperSubmission>();

            List<GPaperSubmission> newList = new List<GPaperSubmission>();

            if (PrimaryVisits != null) newList.AddRange(PrimaryVisits);
            if (SecondaryVisits != null) newList.AddRange(SecondaryVisits);
            return newList;
        }



        public List<GPaymentCheck> FindPaymentChecks(CPaymentCheck CPaymentCheck, long PracticeID, long ClientID, string Email, string Role, string UserID)
        {

            List<GPaymentCheck> lst = (from pc in _context.PaymentCheck
                                       join prac in _context.Practice on pc.PracticeID equals prac.ID
                                       //join up in _context.UserPractices on prac.ID equals up.PracticeID
                                       //join u in _context.Users on up.UserID equals u.Id
                                       join rec in _context.Receiver on pc.ReceiverID equals rec.ID into Table1
                                       from t1 in Table1.DefaultIfEmpty()
                                       where prac.ID == PracticeID &&
                                       //&& u.Id == UserID
                                       //&& u.IsUserBlock == false 
                                      (CPaymentCheck.EntryDateFrom == null ? true : CPaymentCheck.EntryDateFrom == pc.AddedDate.Date) &&
                                      //(CPaymentCheck.EntryDateFrom != null && CPaymentCheck.EntryDateTo != null ?
                                      // pc.AddedDate.Date >= CPaymentCheck.EntryDateFrom && pc.AddedDate.Date <= CPaymentCheck.EntryDateTo
                                      //: (CPaymentCheck.EntryDateFrom != null ? pc.AddedDate.Date >= CPaymentCheck.EntryDateFrom : true)) &&
                                      (CPaymentCheck.CheckDateFrom != null && CPaymentCheck.CheckDateTo != null ?
                                      ((DateTime)pc.CheckDate).Date >= CPaymentCheck.CheckDateFrom && ((DateTime)pc.CheckDate).Date <= CPaymentCheck.CheckDateTo
                                      : (CPaymentCheck.CheckDateFrom != null ? ((DateTime)pc.CheckDate).Date >= CPaymentCheck.CheckDateFrom : true)) &&
                                      (CPaymentCheck.CheckNumber.IsNull() ? true : pc.CheckNumber.Equals(CPaymentCheck.CheckNumber)) &&
                                      (CPaymentCheck.Practice.IsNull() ? true : prac.ID.Equals(CPaymentCheck.Practice)) &&
                                      (CPaymentCheck.Payer.IsNull() ? true : pc.PayerID.Equals(CPaymentCheck.Payer)) &&
                                      (CPaymentCheck.ReceiverID.IsNull() ? true : pc.ReceiverID.Equals(CPaymentCheck.ReceiverID))
                                      &&
                                      (CPaymentCheck.Status.IsNull() ? true : StatusCheck(CPaymentCheck.Status, pc.Status))

                                       //  (CPaymentCheck.Status.IsNull2() ? true : pc.Status.Equals(CPaymentCheck.Status))
                                       orderby pc.AddedDate descending
                                       select new GPaymentCheck()
                                       {
                                           Id = pc.ID,
                                           CheckNumber = pc.CheckNumber,
                                           PaymentMethod = pc.PaymentMethod,
                                           CheckDate = pc.CheckDate.Format("MM/dd/yyyy"),
                                           CheckAmount = pc.CheckAmount,
                                           Appliedamount = pc.AppliedAmount,
                                           PostedAmount = pc.PostedAmount,
                                           NumberOfVisits = pc.NumberOfVisits,
                                           NumberOfPatients = pc.NumberOfPatients,
                                           Status = TranslateStatus(pc.Status),
                                           Payer = pc.PayerName,
                                           Practice = prac.Name,
                                           EntryDate = pc.AddedDate.Format("MM/dd/yyyy hh:mm tt"),
                                           EnteredBy = pc.AddedBy,
                                           ReceiverID = pc.ReceiverID,
                                           Receiver = t1.Name,
                                       }).ToList();
            // return lst;
            // Location 
            if (!CPaymentCheck.Location.IsNull())
            {
                lst = (from l in lst
                       join p in _context.Practice
                       on l.PracticeID equals p.ID
                       join loc in _context.Location
                       on p.ID equals loc.PracticeID
                       where loc.OrganizationName.Contains(CPaymentCheck.Location)
                       select l).ToList();
            }

            if (!CPaymentCheck.Provider.IsNull())
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




        public List<GPatientFollowup> FindPatientFollowUp(CPatientFollowup CPatientFollowup, long PracticeID, long ClientID, string Email, string Role, string UserID)
        {
            List<GPatientFollowup> data = (from pf in _context.PatientFollowUp
                                           join pCharge in _context.PatientFollowUpCharge on pf.ID equals pCharge.PatientFollowUpID
                                           join c in _context.Charge on pCharge.ChargeID equals c.ID
                                           join pat in _context.Patient on pf.PatientID equals pat.ID
                                           join prac in _context.Practice on pat.PracticeID equals prac.ID
                                           join r in _context.Reason on pf.ReasonID equals r.ID into Table1
                                           from t1 in Table1.DefaultIfEmpty()
                                           join g in _context.Group on pf.GroupID equals g.ID into Table2
                                           from t2 in Table2.DefaultIfEmpty()
                                           join a in _context.Action on pf.ActionID equals a.ID into Table3
                                           from t3 in Table3.DefaultIfEmpty()
                                           where prac.ID == PracticeID
                                           && (c.PrimaryPatientBal.Val() + c.SecondaryPatientBal.Val() + c.TertiaryPatientBal.Val()) > 0
                                           && (CPatientFollowup.PatientID.IsNull() ? true : pf.PatientID.Equals(CPatientFollowup.PatientID))
                                           && (CPatientFollowup.FollowUpDate == null ? true : object.Equals(CPatientFollowup.FollowUpDate, pf.AddedDate))
                                           && (CPatientFollowup.PatientAccount.IsNull() ? true : pat.AccountNum.Equals(CPatientFollowup.PatientAccount))
                                           && (CPatientFollowup.ReasonID.IsNull() ? true : t1.ID.Equals(CPatientFollowup.ReasonID))
                                           && (CPatientFollowup.GroupID.IsNull() ? true : t2.ID.Equals(CPatientFollowup.GroupID))
                                           && (CPatientFollowup.ActionID.IsNull() ? true : t3.ID.Equals(CPatientFollowup.ActionID))
                                           && (ExtensionMethods.IsBetweenDOS(CPatientFollowup.ToDate, CPatientFollowup.FromDate, pf.AddedDate, pf.AddedDate))
                                           && (CPatientFollowup.TickleDate == null ? true : object.Equals(CPatientFollowup.TickleDate, pf.TickleDate))
                                          //&& (CPatientFollowup.Status.Equals("All") ? pCharge.Status == "" || pCharge.Status.IsNull() || pCharge.Status.Equals("1st Statement Sent") || pCharge.Status.Equals("2nd Statement Sent") || pCharge.Status.Equals("3rd Statement Sent") || pCharge.Status.Equals("Collection Agency") : CPatientFollowup.Status.Equals("New") ? pCharge.Status == "" || pCharge.Status.IsNull() : pCharge.Status.IsNull() ? false : pCharge.Status.Equals(CPatientFollowup.Status))
                                          && (pat.HoldStatement == null || pat.HoldStatement == false)

                                           group new
                                           {
                                               ID = pf.ID,
                                               PatientID = pf.PatientID,
                                               PatientFollowUpID = pf.ID,
                                               PatientAccount = pat.AccountNum,
                                               PatientName = pat.LastName + ", " + pat.FirstName,
                                               FollowUpDate = pf.AddedDate.Format("MM/dd/yyyy"),
                                               TickleDate = pf.TickleDate.Format("MM/dd/yyyy"),
                                               Reason = t1.Name,
                                               Group = t2.Name,
                                               Action = t3.Name,
                                               Status = (pCharge.Status.IsNull() ? "New" : pCharge.Status)
                                           } by new { PatientID = pf.PatientID } into gp

                                           select new GPatientFollowup
                                           {
                                               ID = gp.Select(a => a.ID).FirstOrDefault(),
                                               PatientID = gp.Key.PatientID,
                                               PatientFollowUpID = gp.Select(a => a.PatientFollowUpID).FirstOrDefault(),
                                               PatientAccount = gp.Select(a => a.PatientAccount).FirstOrDefault(),
                                               PatientName = gp.Select(a => a.PatientName).FirstOrDefault(),
                                               FollowUpDate = gp.Select(a => a.FollowUpDate).FirstOrDefault(),
                                               TickleDate = gp.Select(a => a.TickleDate).FirstOrDefault(),
                                               PatientAmount = (
                                                                  (from patcharge in _context.PatientFollowUpCharge
                                                                   join C in _context.Charge on patcharge.ChargeID equals C.ID
                                                                   where gp.Key.PatientID == C.PatientID
                                                                   select C.PrimaryPatientBal.Val() + C.SecondaryPatientBal.Val() + C.TertiaryPatientBal.Val() > 0 ?
                                                                   C.PrimaryPatientBal.Val() + C.SecondaryPatientBal.Val() + C.TertiaryPatientBal.Val() : 0).Sum()
                                                                  ),
                                               Reason = gp.Select(a => a.Reason).FirstOrDefault(),
                                               Group = gp.Select(a => a.Group).FirstOrDefault(),
                                               Action = gp.Select(a => a.Action).FirstOrDefault(),
                                               Status = gp.Select(a => a.Status).FirstOrDefault()
                                           }).ToList();
            return data;
        }





        public string ParseMonth(int monthNumber)
        {
            string[] monthsInEnglish = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            return monthsInEnglish[monthNumber - 1];
        }

        public int ParseMonthToInt(string monthNumber)
        {
            string[] monthsInEnglish = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov",
            "Dec" };
            //int[] monthsInEnglish = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

            return monthsInEnglish.ToList().IndexOf(monthNumber);
        }




        public string TranslateStatus(string Status)
        {
            string desc = string.Empty;
            if (Status == "N")
            {
                desc = "Regular";
            }
            if (Status == "S")
            {
                desc = "Submitted";
            }
            if (Status == "K")
            {
                desc = "Batch File Accepted";
            }
            if (Status == "D")
            {
                desc = "Batch File Denied";
            }
            if (Status == "E")
            {
                desc = "Batch File has Errors";
            }
            if (Status == "P")
            {
                desc = "Paid";
            }
            if (Status == "DN")
            {
                desc = "Denial";
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
            if (Status == "DN")
            {
                desc = "Denied";
            }
            if (Status == "PPTM")
            {
                desc = "Paid - Medigaped";
            }
            if (Status == "M")
            {
                desc = "Medigaped";
            }
            return desc;
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
        private string GetStatus(SubmissionLog log)
        {
            string status = string.Empty;
            if (log.IK5_Status == "A" && log.AK9_Status == "A") status = "Batch Accepted";
            else if (log.IK5_Status == "R" && log.AK9_Status == "R") status = "Batch Rejected";
            else if (log.IK5_Status == "E" && log.AK9_Status == "E") status = "Partially Accepted";

            return status;
        }


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

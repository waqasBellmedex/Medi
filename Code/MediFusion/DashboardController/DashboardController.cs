using MediFusionPM.Models;
using MediFusionPM.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.ChartModel;
using static MediFusionPM.ViewModels.VMCommon;

namespace WebApplication1.ChartControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;

        public DashboardController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
        }

        [Route("GetTopSubmissions")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TopSubmissionCO>>> GetTopSubmissions()
        {
            return await (from visittable in _context.Visit
                          join patientplantable in _context.PatientPlan on visittable.PrimaryPatientPlanID equals patientplantable.ID
                          join insuranceplantable in _context.InsurancePlan on patientplantable.InsurancePlanID equals insuranceplantable.ID
                          join payertable in _context.Edi837Payer on insuranceplantable.Edi837PayerID equals payertable.ID into PayerTable
                          from ptb in PayerTable.DefaultIfEmpty()

                          where
                            (visittable.IsSubmitted == true)

                          group new { visittable, patientplantable, insuranceplantable, ptb } by ptb.PayerName into gp

                          select new TopSubmissionCO
                          {
                              count = gp.Count(),
                              PayerName = gp.Key.IsNull() ? "ANONYMOUS" : gp.Key


                          }
                              ).Take(8).ToAsyncEnumerable().ToList();


        }


        [Route("FindTopSubmissions")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<TopSubmissionCO>>> FindTopSubmissions(CTopSubmissionCO criteriaModel)
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

            if (criteriaModel.Value == "DOS")
            {

                List<TopSubmissionCO> topSubmission = new List<TopSubmissionCO>();
                using (SqlConnection myConnection = new SqlConnection(newConnection))
                {

                    string oString = "SELECT top 8 count(isnull(payertable.PayerName,'ANONYMOUS') ) as [count], isnull(payertable.PayerName, 'ANONYMOUS') as PayerName  FROM Visit v join PatientPlan pp on v.PrimaryPatientPlanID = pp.ID join InsurancePlan inp on pp.InsurancePlanID = inp.ID        left join  Edi837Payer payertable on inp.Edi837PayerID = payertable.ID where v.PracticeID = " + PracticeId + " GROUP BY isnull(payertable.PayerName, 'ANONYMOUS') ORDER BY[count] DESC";

                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            var topSubmissionD = new TopSubmissionCO();
                            topSubmissionD.PayerName = oReader["PayerName"].ToString();
                            topSubmissionD.count = Convert.ToInt64(oReader["count"].ToString());

                            topSubmission.Add(topSubmissionD);
                        }
                        myConnection.Close();
                    }
                }

                return topSubmission;
            }
            else if (criteriaModel.Value == "AD")
            {
                List<TopSubmissionCO> topSubmission = new List<TopSubmissionCO>();
                using (SqlConnection myConnection = new SqlConnection(newConnection))
                {

                    string oString = "SELECT top 8 count(isnull(payertable.PayerName,'ANONYMOUS') ) as [count], isnull(payertable.PayerName, 'ANONYMOUS') as PayerName  FROM Visit v join PatientPlan pp on v.PrimaryPatientPlanID = pp.ID join InsurancePlan inp on pp.InsurancePlanID = inp.ID        left join  Edi837Payer payertable on inp.Edi837PayerID = payertable.ID where v.PracticeID = " + PracticeId + " GROUP BY isnull(payertable.PayerName, 'ANONYMOUS') ORDER BY[count] DESC";

                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            var topSubmissionD = new TopSubmissionCO();
                            topSubmissionD.PayerName = oReader["PayerName"].ToString();
                            topSubmissionD.count = Convert.ToInt64(oReader["count"].ToString());

                            topSubmission.Add(topSubmissionD);
                        }
                        myConnection.Close();
                    }
                }

                return topSubmission;
            }
            else
            {
                List<TopSubmissionCO> topSubmission = new List<TopSubmissionCO>();
                using (SqlConnection myConnection = new SqlConnection(newConnection))
                {

                    string oString = "SELECT top 8  count(isnull(payertable.PayerName,'ANONYMOUS')) as [count], isnull(payertable.PayerName,'ANONYMOUS') as PayerName FROM Visit v join PatientPlan pp on v.PrimaryPatientPlanID = pp.ID join InsurancePlan inp on pp.InsurancePlanID = inp.ID        left join  Edi837Payer payertable on inp.Edi837PayerID = payertable.ID where v.IsSubmitted = 'true'  AND v.PracticeID = " + PracticeId + "  AND v.SubmittedDate IS NOT NULL GROUP BY isnull(payertable.PayerName, 'ANONYMOUS') ORDER BY[count] DESC";

                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            var topSubmissionD = new TopSubmissionCO();
                            topSubmissionD.PayerName = oReader["PayerName"].ToString();
                            topSubmissionD.count = Convert.ToInt64(oReader["count"].ToString());

                            topSubmission.Add(topSubmissionD);
                        }
                        myConnection.Close();
                    }
                }

                return topSubmission;
            }



        }


        [Route("GetAppointmentData")]
        [HttpGet]
        public Dictionary<string, string> GetAppointmentData()
        {
            List<AppointmentReturnCO> data = (from patientappointmenttable in _context.PatientAppointment
                                              orderby patientappointmenttable.Status descending
                                              group patientappointmenttable by patientappointmenttable.Status into gp

                                              select new AppointmentReturnCO
                                              {
                                                  Count = gp.Count(),
                                                  Type = TranslateStatusCode(gp.Key)

                                              }).ToList();

            AppointmentCO appointment = new AppointmentCO();
            Dictionary<string, string> finalData = new Dictionary<string, string>();

            int[] checks = new int[] { 0, 0, 0, 0, 0 };
            foreach (AppointmentReturnCO obj in data)
            {

                string typeObj = obj.Type.Trim();
                if (typeObj.Equals("scheduled", StringComparison.InvariantCultureIgnoreCase))
                {
                    checks[0] = 1;
                }
                else if (typeObj.Equals("seen", StringComparison.InvariantCultureIgnoreCase))
                {
                    checks[1] = 1;
                }
                else if (typeObj.Equals("no show", StringComparison.InvariantCultureIgnoreCase))
                {
                    checks[2] = 1;
                }
                else if (typeObj.Equals("cancelled", StringComparison.InvariantCultureIgnoreCase))
                {
                    checks[3] = 1;
                }
                else if (typeObj.Equals("rescheduled", StringComparison.InvariantCultureIgnoreCase))
                {
                    checks[4] = 1;
                }
                Debug.WriteLine(typeObj);
                if (typeObj.Split(' ').Count() > 1)
                {
                    finalData.Add(typeObj.Split(' ')[0].ToLowerInvariant() + typeObj.Split(' ')[1], obj.Count.ToString());
                }
                else
                {
                    string temp = "";
                    for (int i = 0; i < typeObj.Length; i++)
                    {
                        if (i == 0)
                        {
                            temp = temp + typeObj[i].ToString().ToLowerInvariant();
                        }
                        else
                        {
                            temp = temp + typeObj[i].ToString();
                        }
                    }
                    typeObj = temp;
                    finalData.Add(typeObj, obj.Count.ToString());
                }
            }
            for (int randomI = 0; randomI < checks.Length; randomI++)
            {
                if (checks[randomI] == 0)
                {
                    finalData.Add(getStringFromNumberForAppointmentData(randomI), "0");
                }
            }

            return finalData;
        }
        [Route("FindAppointmentData")]
        [HttpPost]
        public Dictionary<string, string> FindAppointmentData(CAppointmentCO cAppointment)
        {
            //DateTime now = DateTime.Now;
            //var startDate = new DateTime(now.Year, now.Month, 1);
            //var endDate = startDate.AddMonths(1).AddDays(-1);

            //  T for Today , Y for Yesterday , MTD for month To date , YTD for year to date
            if (cAppointment.Value == "T")
            {
                cAppointment.DateFrom = DateTime.Now.Date;
                cAppointment.DateTo = cAppointment.DateFrom;
            }
            else if (cAppointment.Value == "Y")
            {
                cAppointment.DateFrom = DateTime.Now.AddDays(-1).Date;
                cAppointment.DateTo = cAppointment.DateFrom;
            }
            else if (cAppointment.Value == "MTD")
            {
                cAppointment.DateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;
                cAppointment.DateTo = DateTime.Now.Date;
            }
            else if (cAppointment.Value == "YTD")
            {
                cAppointment.DateFrom = new DateTime(DateTime.Now.Year, 1, 1).Date;
                cAppointment.DateTo = DateTime.Now.Date;
            }

            List<AppointmentReturnCO> data = (from pa in _context.PatientAppointment
                                              where
                                                   ExtensionMethods.IsNull_Bool(pa.Inactive) != true &&
                         (pa.AppointmentDate.Value.Date >= cAppointment.DateFrom.Value.Date && pa.AppointmentDate.Value.Date <= cAppointment.DateTo.Value.Date)

                                              //(ExtensionMethods.IsBetweenDOS(cAppointment.DateTo, cAppointment.DateFrom, patientappointmenttable.AddedDate.Date, patientappointmenttable.AddedDate.Date)) &&
                                              // (patientappointmenttable.AppointmentDate.HasValue ? patientappointmenttable.AppointmentDate.Value.Date.Equals(DateTime.Now.Date) : false)
                                              orderby pa.Status descending
                                              group pa by pa.Status into gp

                                              select new AppointmentReturnCO
                                              {
                                                  Count = gp != null ? gp.Count() : 0,
                                                  Type = gp != null ? TranslateStatusCode(gp.Key) : ""

                                              }).ToList();

            AppointmentCO appointment = new AppointmentCO();
            Dictionary<string, string> finalData = new Dictionary<string, string>();

            int[] checks = new int[] { 0, 0, 0, 0, 0 };
            foreach (AppointmentReturnCO obj in data)
            {

                string typeObj = obj.Type.Trim();
                if (typeObj.Equals("scheduled", StringComparison.InvariantCultureIgnoreCase))
                {
                    checks[0] = 1;
                }
                else if (typeObj.Equals("seen", StringComparison.InvariantCultureIgnoreCase))
                {
                    checks[1] = 1;
                }
                else if (typeObj.Equals("no show", StringComparison.InvariantCultureIgnoreCase))
                {
                    checks[2] = 1;
                }
                else if (typeObj.Equals("cancelled", StringComparison.InvariantCultureIgnoreCase))
                {
                    checks[3] = 1;
                }
                else if (typeObj.Equals("rescheduled", StringComparison.InvariantCultureIgnoreCase))
                {
                    checks[4] = 1;
                }
                Debug.WriteLine(typeObj);
                if (typeObj.Split(' ').Count() > 1)
                {
                    finalData.Add(typeObj.Split(' ')[0].ToLowerInvariant() + typeObj.Split(' ')[1], obj.Count.ToString());
                }
                else
                {
                    string temp = "";
                    for (int i = 0; i < typeObj.Length; i++)
                    {
                        if (i == 0)
                        {
                            temp = temp + typeObj[i].ToString().ToLowerInvariant();
                        }
                        else
                        {
                            temp = temp + typeObj[i].ToString();
                        }
                    }
                    typeObj = temp;
                    finalData.Add(typeObj, obj.Count.ToString());
                }
            }
            for (int randomI = 0; randomI < checks.Length; randomI++)
            {
                if (checks[randomI] == 0)
                {
                    finalData.Add(getStringFromNumberForAppointmentData(randomI), "0");
                }
            }

            return finalData;
        }

        public string getStringFromNumberForAppointmentData(int i)
        {
            string[] strings = new string[] { "scheduled", "seen", "noShow", "cancelled", "rescheduled" };
            return strings[i];
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


        [Route("FindVisitChargeData")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<VisitAndChargeCO>>> FindVisitChargeData(CVisitAndChargeCO cVisitAndCharge)
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

            //DOS for Date of service , AD for added Date , SD for Submit Date
            if (cVisitAndCharge.Value == "DOS")
            {

                List<VisitAndChargeCO> visitCharge = new List<VisitAndChargeCO>();
                using (SqlConnection myConnection = new SqlConnection(newConnection))
                {

                    string oString = "SELECT count(distinct v.id) AS ID, SUM(c.TotalAmount) AS TotalAmount, MONTH(v.DateOfServiceFrom) as m, Year(v.DateOfServiceFrom) as y FROM Charge c JOIN Visit v ON c.VisitID = v.Id where(v.DateOfServiceFrom <= CURRENT_TIMESTAMP) AND(v.DateOfServiceFrom >= (SELECT DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - 5, 0))) AND v.PracticeID = " + PracticeId + " GROUP BY MONTH(v.DateOfServiceFrom), Year(v.DateOfServiceFrom) ORDER BY y DESC, m DESC";
                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            var VisCharge = new VisitAndChargeCO();
                            VisCharge.YearMonth = ParseMonth(Convert.ToInt32(oReader["m"].ToString())) + " " + oReader["y"].ToString();
                            VisCharge.Year = oReader["y"].ToString();
                            VisCharge.Count = Convert.ToInt64(oReader["ID"].ToString());
                            VisCharge.Charges = Convert.ToDecimal(oReader["TotalAmount"].ToString());
                            VisCharge.Month = ParseMonth(Convert.ToInt32(oReader["m"].ToString()));
                            //ModiD.ID = Convert.ToInt32(oReader["ID"]);
                            //ModiD.Value = oReader["Description"].ToString();
                            //ModiD.Description = oReader["Description"].ToString();
                            //ModiD.label = oReader["Code"].ToString();
                            //ModiD.AnesthesiaUnits = 0;
                            //ModiD.AnesthesiaUnits = Convert.ToInt32(string.IsNullOrEmpty(oReader["AnesthesiaBaseUnits"].ToString()));
                            //ModiD.Description2 = Convert.ToInt32(string.IsNullOrEmpty(oReader["DefaultFees"].ToString()));

                            visitCharge.Add(VisCharge);
                        }
                        myConnection.Close();
                    }
                }

                return visitCharge;







                //return await (from visittable in _context.Visit
                //              where
                //              (visittable.DateOfServiceFrom) <= DateTime.Now && (visittable.DateOfServiceFrom) >= (firstDayOfMonth)
                //              && (visittable.PracticeID == PracticeId)
                //              group new { visittable, Id = visittable.ID } by new { Month = visittable.DateOfServiceFrom.Value.Month, Year = visittable.DateOfServiceFrom.Value.Year } into gp

                //              select new VisitAndChargeCO
                //              {
                //                  VisitID = gp.Select(m => m.Id).FirstOrDefault(),
                //                  YearMonth = ParseMonth(gp.Key.Month) + " " + gp.Key.Year,
                //                  Year = "" + gp.Key.Year,
                //                  Count = gp.Count(),
                //                  Charges = (

                //              (from chargetable in _context.Charge
                //               join visittable2 in _context.Visit
                //                on chargetable.VisitID equals visittable2.ID
                //               where (gp.Select(m => m.Id).Contains(chargetable.VisitID.Value))
                //               select chargetable.TotalAmount).Sum()


                //              ),
                //                  Month = ParseMonth(gp.Key.Month)
                //              }).OrderByDescending(lamb => lamb.Year).ThenByDescending(lamb => ParseMonthToInt(lamb.Month)).ToAsyncEnumerable().ToList();
            }
            else if (cVisitAndCharge.Value == "AD")
            {

                List<VisitAndChargeCO> visitCharge = new List<VisitAndChargeCO>();
                using (SqlConnection myConnection = new SqlConnection(newConnection))
                {

                    string oString = "SELECT count(distinct v.id) AS ID, SUM(c.TotalAmount) AS TotalAmount, MONTH(v.AddedDate) as m, Year(v.AddedDate) as y FROM Charge c JOIN Visit v ON c.VisitID = v.Id where(v.AddedDate <= CURRENT_TIMESTAMP) AND(v.AddedDate >= (SELECT DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - 5, 0))) AND v.PracticeID = " + PracticeId + " GROUP BY MONTH(v.AddedDate), Year(v.AddedDate) ORDER BY y DESC, m DESC";
                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            var VisCharge = new VisitAndChargeCO();
                            VisCharge.YearMonth = ParseMonth(Convert.ToInt32(oReader["m"].ToString())) + " " + oReader["y"].ToString();
                            VisCharge.Year = oReader["y"].ToString();
                            VisCharge.Count = Convert.ToInt64(oReader["ID"].ToString());
                            VisCharge.Charges = Convert.ToDecimal(oReader["TotalAmount"].ToString());
                            VisCharge.Month = ParseMonth(Convert.ToInt32(oReader["m"].ToString()));
                            visitCharge.Add(VisCharge);
                        }
                        myConnection.Close();
                    }
                }

                return visitCharge;


                //return await (from visittable in _context.Visit
                //              where

                //              (visittable.AddedDate) <= DateTime.Now && (visittable.AddedDate) >= (firstDayOfMonth)
                //              && (visittable.PracticeID == PracticeId)
                //              group new { visittable, Id = visittable.ID } by new { Month = visittable.AddedDate.Month, Year = visittable.AddedDate.Year } into gp
                //              select new VisitAndChargeCO
                //              {
                //                  VisitID = gp.Select(m => m.Id).FirstOrDefault(),
                //                  YearMonth = ParseMonth(gp.Key.Month) + " " + gp.Key.Year,
                //                  Year = "" + gp.Key.Year,
                //                  Count = gp.Count(),
                //                  Charges = (

                //              (from chargetable in _context.Charge
                //               join visittable2 in _context.Visit
                //                on chargetable.VisitID equals visittable2.ID
                //               where (gp.Select(m => m.Id).Contains(chargetable.VisitID.Value))
                //               select chargetable.TotalAmount).Sum()
                //              ),
                //                  Month = ParseMonth(gp.Key.Month)
                //              }).OrderByDescending(lamb => lamb.Year).ThenByDescending(lamb => ParseMonthToInt(lamb.Month)).ToAsyncEnumerable().ToList();
            }
            else
            {

                List<VisitAndChargeCO> visitCharge = new List<VisitAndChargeCO>();
                using (SqlConnection myConnection = new SqlConnection(newConnection))
                {

                    string oString = "SELECT count(distinct v.id) AS ID, SUM(c.TotalAmount) AS TotalAmount, MONTH(v.SubmittedDate) as m, Year(v.SubmittedDate) as y FROM Charge c JOIN Visit v ON c.VisitID = v.Id where(v.SubmittedDate <= CURRENT_TIMESTAMP) AND(v.SubmittedDate >= (SELECT DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - 5, 0))) AND v.PracticeID = " + PracticeId + " GROUP BY MONTH(v.SubmittedDate), Year(v.SubmittedDate) ORDER BY y DESC, m DESC";
                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            var VisCharge = new VisitAndChargeCO();
                            VisCharge.YearMonth = ParseMonth(Convert.ToInt32(oReader["m"].ToString())) + " " + oReader["y"].ToString();
                            VisCharge.Year = oReader["y"].ToString();
                            VisCharge.Count = Convert.ToInt64(oReader["ID"].ToString());
                            VisCharge.Charges = Convert.ToDecimal(oReader["TotalAmount"].ToString());
                            VisCharge.Month = ParseMonth(Convert.ToInt32(oReader["m"].ToString()));
                            visitCharge.Add(VisCharge);
                        }
                        myConnection.Close();
                    }
                }

                return visitCharge;


                //               return await (from visittable in _context.Visit
                //                             where
                //                             visittable.SubmittedDate.HasValue &&
                //                             (visittable.SubmittedDate) <= DateTime.Now && (visittable.SubmittedDate) >= (firstDayOfMonth)
                //                             && (visittable.PracticeID == PracticeId)
                //                             group new { visittable, Id = visittable.ID } by new { Month = visittable.SubmittedDate.Value.Month, Year = visittable.SubmittedDate.Value.Year } into gp
                //                             select new VisitAndChargeCO
                //                             {
                //                                 VisitID = gp.Select(m => m.Id).FirstOrDefault(),
                //                                 YearMonth = ParseMonth(gp.Key.Month) + " " + gp.Key.Year,
                //                                 Year = "" + gp.Key.Year,
                //                                 Count = gp.Count(),
                //                                 Charges =(

                //                             (from chargetable in _context.Charge
                //                              join visittable2 in _context.Visit
                //on chargetable.VisitID equals visittable2.ID
                //                              where (gp.Select(m => m.Id).Contains(chargetable.VisitID.Value))
                //                              select chargetable.TotalAmount).Sum()
                //                             ),
                //                                 Month = ParseMonth(gp.Key.Month)
                //                             }).OrderByDescending(lamb => lamb.Year).ThenByDescending(lamb => ParseMonthToInt(lamb.Month)).ToAsyncEnumerable().ToList();
            }
        }


        [Route("GetVisitChargeData")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisitAndChargeCO>>> GetVisitChargeData()
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
          User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
          User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
          );
            return await (from chargetable in _context.Charge
                          join visittable in _context.Visit on chargetable.VisitID equals visittable.ID
                          where (visittable.PracticeID == UD.PracticeID)
                          group new { visittable, chargetable.TotalAmount } by new { visittable.DateOfServiceFrom.Value.Month, Year = visittable.DateOfServiceFrom.Value.Year } into gp

                          select new VisitAndChargeCO
                          {
                              YearMonth = ParseMonth(gp.Key.Month) + " " + gp.Key.Month,
                              Year = "" + gp.Key.Year,
                              Count = gp.Count(),
                              Charges = (long)gp.Select(a => a.TotalAmount).Sum(),
                              Month = ParseMonth(gp.Key.Month)

                          }).OrderByDescending(lamb => lamb.Year).ThenByDescending(lamb => ParseMonthToInt(lamb.Month)).ToAsyncEnumerable().ToList();

        }

        [Route("GetPracticeData")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PracticeCO>>> GetPracticeData()
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
          User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
          User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
          );
            return await (from chargetable in _context.Charge
                          join visittable in _context.Visit on chargetable.VisitID equals visittable.ID
                          where (visittable.PracticeID == UD.PracticeID)
                          group new
                          {
                              visittable,
                              totalAmount = chargetable.TotalAmount,
                              paidAmount = (chargetable.PrimaryPaid.HasValue ? chargetable.PrimaryPaid.Value : 0) + (chargetable.PrimaryPaid.HasValue ? chargetable.PrimaryPaid.Value : 0) + (chargetable.PrimaryPaid.HasValue ? chargetable.PrimaryPaid.Value : 0),
                              adjustments = (chargetable.PrimaryWriteOff.HasValue ? chargetable.PrimaryWriteOff.Value : 0) + (chargetable.SecondaryWriteOff.HasValue ? chargetable.SecondaryWriteOff.Value : 0) + (chargetable.TertiaryWriteOff.HasValue ? chargetable.TertiaryWriteOff.Value : 0),
                              balance = (visittable.PrimaryBal.HasValue ? visittable.PrimaryBal.Value : 0) + (visittable.SecondaryPaid.HasValue ? visittable.SecondaryPaid.Value : 0) + (visittable.TertiaryPaid.HasValue ? visittable.TertiaryPaid.Value : 0),
                              totalBalance = (
                                  (chargetable.PrimaryPaid.HasValue ? chargetable.PrimaryPaid.Value : 0) + (chargetable.PrimaryPaid.HasValue ? chargetable.PrimaryPaid.Value : 0) + (chargetable.PrimaryPaid.HasValue ? chargetable.PrimaryPaid.Value : 0) +
                                  (chargetable.PrimaryWriteOff.HasValue ? chargetable.PrimaryWriteOff.Value : 0) + (chargetable.SecondaryWriteOff.HasValue ? chargetable.SecondaryWriteOff.Value : 0) + (chargetable.TertiaryWriteOff.HasValue ? chargetable.TertiaryWriteOff.Value : 0) +
                                  (visittable.PrimaryBal.HasValue ? visittable.PrimaryBal.Value : 0) + (visittable.SecondaryPaid.HasValue ? visittable.SecondaryPaid.Value : 0) + (visittable.TertiaryPaid.HasValue ? visittable.TertiaryPaid.Value : 0) +
                                  chargetable.TotalAmount
                              )
                          } by new { visittable.DateOfServiceFrom.Value.Month } into gp

                          select new PracticeCO
                          {
                              Adjustment = (long)gp.Select(a => a.adjustments).Sum(),
                              Balance = (long)gp.Select(a => a.balance).Sum(),
                              Charges = (long)gp.Select(a => a.totalAmount).Sum(),
                              Month = ParseMonth(gp.Key.Month),
                              Payment = (long)gp.Select(a => a.paidAmount).Sum(),
                              TotalBalance = (long)gp.Select(a => a.totalBalance).Sum()

                          }).ToAsyncEnumerable().ToList();

        }


        [Route("FindPracticeData")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<PracticeCO>>> FindPracticeData(CAppointmentCO cAppointmentCO)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
          User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
          User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
          );
            if (cAppointmentCO.Value == "DOS")
            {
                return await (from chargetable in _context.Charge
                              join visittable in _context.Visit on chargetable.VisitID equals visittable.ID
                              where (ExtensionMethods.IsBetweenDOS(cAppointmentCO.DateTo, cAppointmentCO.DateFrom, visittable.DateOfServiceFrom, visittable.DateOfServiceFrom))
                              && (visittable.PracticeID == UD.PracticeID)
                              group new
                              {
                                  visittable,
                                  totalAmount = chargetable.TotalAmount,
                                  paidAmount = (chargetable.PrimaryPaid.HasValue ? chargetable.PrimaryPaid.Value : 0) + (chargetable.PrimaryPaid.HasValue ? chargetable.PrimaryPaid.Value : 0) + (chargetable.PrimaryPaid.HasValue ? chargetable.PrimaryPaid.Value : 0),
                                  adjustments = (chargetable.PrimaryWriteOff.HasValue ? chargetable.PrimaryWriteOff.Value : 0) + (chargetable.SecondaryWriteOff.HasValue ? chargetable.SecondaryWriteOff.Value : 0) + (chargetable.TertiaryWriteOff.HasValue ? chargetable.TertiaryWriteOff.Value : 0),
                                  balance = (visittable.PrimaryBal.HasValue ? visittable.PrimaryBal.Value : 0) + (visittable.SecondaryPaid.HasValue ? visittable.SecondaryPaid.Value : 0) + (visittable.TertiaryPaid.HasValue ? visittable.TertiaryPaid.Value : 0),
                                  totalBalance = (
                                      (chargetable.PrimaryPaid.HasValue ? chargetable.PrimaryPaid.Value : 0) + (chargetable.PrimaryPaid.HasValue ? chargetable.PrimaryPaid.Value : 0) + (chargetable.PrimaryPaid.HasValue ? chargetable.PrimaryPaid.Value : 0) +
                                      (chargetable.PrimaryWriteOff.HasValue ? chargetable.PrimaryWriteOff.Value : 0) + (chargetable.SecondaryWriteOff.HasValue ? chargetable.SecondaryWriteOff.Value : 0) + (chargetable.TertiaryWriteOff.HasValue ? chargetable.TertiaryWriteOff.Value : 0) +
                                      (visittable.PrimaryBal.HasValue ? visittable.PrimaryBal.Value : 0) + (visittable.SecondaryPaid.HasValue ? visittable.SecondaryPaid.Value : 0) + (visittable.TertiaryPaid.HasValue ? visittable.TertiaryPaid.Value : 0) +
                                      chargetable.TotalAmount
                                  )
                              } by new { visittable.DateOfServiceFrom.Value.Month } into gp

                              select new PracticeCO
                              {
                                  Adjustment = (long)gp.Select(a => a.adjustments).Sum(),
                                  Balance = (long)gp.Select(a => a.balance).Sum(),
                                  Charges = (long)gp.Select(a => a.totalAmount).Sum(),
                                  Month = ParseMonth(gp.Key.Month),
                                  Payment = (long)gp.Select(a => a.paidAmount).Sum(),
                                  TotalBalance = (long)gp.Select(a => a.totalBalance).Sum()

                              }).ToAsyncEnumerable().ToList();
            }
            else if (cAppointmentCO.Value == "AD")
            {
                return await (from chargetable in _context.Charge
                              join visittable in _context.Visit on chargetable.VisitID equals visittable.ID
                              where (ExtensionMethods.IsBetweenDOS(cAppointmentCO.DateTo, cAppointmentCO.DateFrom, visittable.AddedDate, visittable.AddedDate)) && (visittable.PracticeID == UD.PracticeID)
                              group new
                              {
                                  visittable,
                                  totalAmount = chargetable.TotalAmount,
                                  paidAmount = (chargetable.PrimaryPaid.HasValue ? chargetable.PrimaryPaid.Value : 0) + (chargetable.PrimaryPaid.HasValue ? chargetable.PrimaryPaid.Value : 0) + (chargetable.PrimaryPaid.HasValue ? chargetable.PrimaryPaid.Value : 0),
                                  adjustments = (chargetable.PrimaryWriteOff.HasValue ? chargetable.PrimaryWriteOff.Value : 0) + (chargetable.SecondaryWriteOff.HasValue ? chargetable.SecondaryWriteOff.Value : 0) + (chargetable.TertiaryWriteOff.HasValue ? chargetable.TertiaryWriteOff.Value : 0),
                                  balance = (visittable.PrimaryBal.HasValue ? visittable.PrimaryBal.Value : 0) + (visittable.SecondaryPaid.HasValue ? visittable.SecondaryPaid.Value : 0) + (visittable.TertiaryPaid.HasValue ? visittable.TertiaryPaid.Value : 0),
                                  totalBalance = (
                                      (chargetable.PrimaryPaid.HasValue ? chargetable.PrimaryPaid.Value : 0) + (chargetable.PrimaryPaid.HasValue ? chargetable.PrimaryPaid.Value : 0) + (chargetable.PrimaryPaid.HasValue ? chargetable.PrimaryPaid.Value : 0) +
                                      (chargetable.PrimaryWriteOff.HasValue ? chargetable.PrimaryWriteOff.Value : 0) + (chargetable.SecondaryWriteOff.HasValue ? chargetable.SecondaryWriteOff.Value : 0) + (chargetable.TertiaryWriteOff.HasValue ? chargetable.TertiaryWriteOff.Value : 0) +
                                      (visittable.PrimaryBal.HasValue ? visittable.PrimaryBal.Value : 0) + (visittable.SecondaryPaid.HasValue ? visittable.SecondaryPaid.Value : 0) + (visittable.TertiaryPaid.HasValue ? visittable.TertiaryPaid.Value : 0) +
                                      chargetable.TotalAmount
                                  )
                              } by new { visittable.AddedDate.Month } into gp

                              select new PracticeCO
                              {
                                  Adjustment = (long)gp.Select(a => a.adjustments).Sum(),
                                  Balance = (long)gp.Select(a => a.balance).Sum(),
                                  Charges = (long)gp.Select(a => a.totalAmount).Sum(),
                                  Month = ParseMonth(gp.Key.Month),
                                  Payment = (long)gp.Select(a => a.paidAmount).Sum(),
                                  TotalBalance = (long)gp.Select(a => a.totalBalance).Sum()

                              }).ToAsyncEnumerable().ToList();
            }
            else
            {
                return await (from chargetable in _context.Charge
                              join visittable in _context.Visit on chargetable.VisitID equals visittable.ID
                              where (ExtensionMethods.IsBetweenDOS(cAppointmentCO.DateTo, cAppointmentCO.DateFrom, visittable.SubmittedDate, visittable.SubmittedDate)) && (visittable.SubmittedDate.HasValue ? true : false) && (visittable.PracticeID == UD.PracticeID)
                              group new
                              {
                                  visittable,
                                  totalAmount = chargetable.TotalAmount,
                                  paidAmount = (chargetable.PrimaryPaid.HasValue ? chargetable.PrimaryPaid.Value : 0) + (chargetable.PrimaryPaid.HasValue ? chargetable.PrimaryPaid.Value : 0) + (chargetable.PrimaryPaid.HasValue ? chargetable.PrimaryPaid.Value : 0),
                                  adjustments = (chargetable.PrimaryWriteOff.HasValue ? chargetable.PrimaryWriteOff.Value : 0) + (chargetable.SecondaryWriteOff.HasValue ? chargetable.SecondaryWriteOff.Value : 0) + (chargetable.TertiaryWriteOff.HasValue ? chargetable.TertiaryWriteOff.Value : 0),
                                  balance = (visittable.PrimaryBal.HasValue ? visittable.PrimaryBal.Value : 0) + (visittable.SecondaryPaid.HasValue ? visittable.SecondaryPaid.Value : 0) + (visittable.TertiaryPaid.HasValue ? visittable.TertiaryPaid.Value : 0),
                                  totalBalance = (
                                      (chargetable.PrimaryPaid.HasValue ? chargetable.PrimaryPaid.Value : 0) + (chargetable.PrimaryPaid.HasValue ? chargetable.PrimaryPaid.Value : 0) + (chargetable.PrimaryPaid.HasValue ? chargetable.PrimaryPaid.Value : 0) +
                                      (chargetable.PrimaryWriteOff.HasValue ? chargetable.PrimaryWriteOff.Value : 0) + (chargetable.SecondaryWriteOff.HasValue ? chargetable.SecondaryWriteOff.Value : 0) + (chargetable.TertiaryWriteOff.HasValue ? chargetable.TertiaryWriteOff.Value : 0) +
                                      (visittable.PrimaryBal.HasValue ? visittable.PrimaryBal.Value : 0) + (visittable.SecondaryPaid.HasValue ? visittable.SecondaryPaid.Value : 0) + (visittable.TertiaryPaid.HasValue ? visittable.TertiaryPaid.Value : 0) +
                                      chargetable.TotalAmount
                                  )
                              } by new { visittable.SubmittedDate.Value.Month } into gp

                              select new PracticeCO
                              {
                                  Adjustment = (long)gp.Select(a => a.adjustments).Sum(),
                                  Balance = (long)gp.Select(a => a.balance).Sum(),
                                  Charges = (long)gp.Select(a => a.totalAmount).Sum(),
                                  Month = ParseMonth(gp.Key.Month),
                                  Payment = (long)gp.Select(a => a.paidAmount).Sum(),
                                  TotalBalance = (long)gp.Select(a => a.totalBalance).Sum()

                              }).ToAsyncEnumerable().ToList();
            }



        }



        [Route("GetAgingData")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<AgingCOThird>>> GetAgingData(CAgingCO cAgingCO)
        {
            //  UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
            //);

            //var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            //var RoleClaim = User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase));
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            var Client = (from w in _contextMain.MainPractice
                          where w.ID == PracticeId
                          select w
                        ).FirstOrDefault();
            string contextName = _contextMain.MainClient.Find(Client.ClientID)?.ContextName;
            string newConnection = GetConnectionStringManager(contextName);

            if (cAgingCO.Value == "DOS")
            {

                List<AgingCOThird> agingCOThird = new List<AgingCOThird>();
                List<AgingCO> data = new List<AgingCO>();
                using (SqlConnection myConnection = new SqlConnection(newConnection))
                {
                    // --claimAge = (visittable.DateOfServiceFrom.HasValue ? GetClaimAge(visittable.DateOfServiceFrom.Value) : 0),
                    string oString = "SELECT 		SUM( 		case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 		case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 		case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 			) as planAmount  , 	SUM(ISNULL(chargetable.PrimaryPatientBal, 0) + ISNULL(chargetable.SecondaryPatientBal, 0) + ISNULL(chargetable.TertiaryPatientBal, 0)) as patientAmount  , 	SUM( 		case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 		case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 		case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 	+		 ISNULL(chargetable.PrimaryPatientBal, 0) +  ISNULL(chargetable.SecondaryPatientBal, 0) + ISNULL(chargetable.TertiaryPatientBal, 0)		) as total , 		case  when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )>= 0 then 1 when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )> 30 then 2 when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )> 60 then 3 when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )> 90 then 4 when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )> 120 then 5 else 0 end as claimAge FROM Charge chargetable join  Patient patienttable on chargetable.PatientID = patienttable.ID join Visit v on chargetable.VisitID = v.ID join Practice practicetable on chargetable.PracticeID = practicetable.ID join [Provider] providertable on chargetable.ProviderID =  providertable.ID join [Location] locationtable on chargetable.LocationID = locationtable.ID join PatientPlan patientplantable on chargetable.PrimaryPatientPlanID = patientplantable.ID join InsurancePlan insurangeplantable on patientplantable.InsurancePlanID = insurangeplantable.ID join Cpt cpt on chargetable.CPTID = cpt.ID left join  RefProvider refprovidertable on chargetable.RefProviderID = refprovidertable.ID left join PatientPlan secpatientplantable on chargetable.SecondaryPatientPlanID = secpatientplantable.ID left join PatientPlan terpatientplantable on chargetable.TertiaryPatientPlanID = terpatientplantable.ID left join InsurancePlan secinsuranceplantable on secpatientplantable.InsurancePlanID = secinsuranceplantable.ID left join InsurancePlan terinsuranceplantable on terpatientplantable.InsurancePlanID = terinsuranceplantable.ID  where chargetable.DateOfServiceFrom IS NOT NULL  GROUP BY case when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )>= 0 then 1 when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )> 30 then 2 when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )> 60 then 3 when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )> 90 then 4 when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )> 120 then 5 else 0    end   having ( SUM(	case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +			 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 		 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 	) >  0)  ";
                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            var aging = new AgingCO();
                            if (Convert.ToInt32(oReader["claimAge"].ToString()) == 1)
                            {
                                aging.Type = "between0and30";
                            }
                            else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 2)
                            {
                                aging.Type = "between31and60";
                            }
                            else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 3)
                            {
                                aging.Type = "between61and90";
                            }
                            else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 4)
                            {
                                aging.Type = "between91and120";
                            }
                            else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 5)
                            {
                                aging.Type = "greaterThan120";
                            }
                            aging.Total = Convert.ToDecimal(oReader["total"].ToString());
                            aging.PlanAmount = Convert.ToDecimal(oReader["planAmount"].ToString());
                            aging.PaidAmount = Convert.ToDecimal(oReader["patientAmount"].ToString());
                            data.Add(aging);
                        }
                        myConnection.Close();
                    }
                }




                //List<AgingCO> data = (from chargetable in _context.Charge
                //                      join pat in _context.Patient on chargetable.PatientID equals pat.ID
                //                      join visittable in _context.Visit on chargetable.VisitID equals visittable.ID
                //                      join practicetable in _context.Practice on chargetable.PracticeID equals practicetable.ID
                //                      join providertable in _context.Provider on chargetable.ProviderID equals providertable.ID
                //                      join locationtable in _context.Location on chargetable.LocationID equals locationtable.ID
                //                      join patientplantable in _context.PatientPlan on chargetable.PrimaryPatientPlanID equals patientplantable.ID
                //                      join insurangeplantable in _context.InsurancePlan on patientplantable.InsurancePlanID equals insurangeplantable.ID

                //                      //from visittable in _context.Visit
                //                      //join pat in _context.Patient on visittable.PatientID equals pat.ID
                //                      //join fac in _context.Practice
                //                      //on pat.PracticeID equals fac.ID
                //                      //join loc in _context.Location
                //                      //on pat.LocationId equals loc.ID
                //                      //join prov in _context.Provider
                //                      //on pat.ProviderID equals prov.ID
                //                      //join pPlan in _context.PatientPlan
                //                      //on visittable.PrimaryPatientPlanID equals pPlan.ID
                //                      //join iPlan in _context.InsurancePlan
                //                      //on pPlan.InsurancePlanID equals iPlan.ID
                //                      where (visittable.DateOfServiceFrom.HasValue ? true : false)
                //                      && (visittable.PracticeID == PracticeId)
                //                      group new
                //                      {
                //                          //chargeAmount = (chargetable.PrimaryBal.Val()) + (chargetable.SecondaryBal.Val()) + (chargetable.TertiaryBal.Val()),

                //                          planAmount = (chargetable.PrimaryBal.Val()) + (chargetable.SecondaryBal.Val()) + (chargetable.TertiaryBal.Val()),
                //                          patientAmount = (visittable.PrimaryPatientBal.Val()) + (visittable.SecondaryPatientBal.Val()) + (visittable.TertiaryPatientBal.Val()),
                //                          claimAge = (visittable.DateOfServiceFrom.HasValue ? GetClaimAge(visittable.DateOfServiceFrom.Value) : 0),
                //                          total = (
                //                      (chargetable.PrimaryBal.Val()) + (chargetable.SecondaryPaid.Val()) + (chargetable.TertiaryPaid.Val()) +
                //                      (visittable.PrimaryPatientBal.Val()) + (visittable.SecondaryPatientBal.Val()) + (visittable.TertiaryPatientBal.Val())
                //                      )
                //                      } by new { ClaimAge = CheckClaimAge((visittable.DateOfServiceFrom.HasValue ? GetClaimAge(visittable.DateOfServiceFrom.Value) : 0)) } into gp

                //                      select new AgingCO
                //                      {
                //                          Type = gp.Key.ClaimAge == 1 ? "between0and30" : gp.Key.ClaimAge == 2 ? "between31and60" : gp.Key.ClaimAge == 3 ? "between61and90" : gp.Key.ClaimAge == 4 ? "between91and120" : "greaterThan120",
                //                          PlanAmount = gp.Select(a => a.planAmount).Sum(),
                //                          PaidAmount = gp.Select(a => a.patientAmount).Sum(),
                //                          Total = gp.Select(a => a.total).Sum(),

                //                      }).OrderBy(lamb => lamb.Type).ToList();


                Debug.WriteLine(data.Count);
                int[] ranges = new int[] { 0, 0, 0, 0, 0 };
                string[] rangeValues = new string[] { "between0and30", "between31and60", "between61and90", "between91and120", "greaterThan120" };

                for (int i = 0; i < data.Count; i++)
                {
                    if (data.ElementAt(i).Type.Equals("between0and30"))
                    {
                        ranges[0] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("between31and60"))
                    {
                        ranges[1] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("between61and90"))
                    {
                        ranges[2] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("between91and120"))
                    {
                        ranges[3] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("greaterThan120"))
                    {
                        ranges[4] = 1;
                    }
                    Debug.WriteLine(data.ElementAt(i).Type + " :: " + data.ElementAt(i).Total);
                }
                for (int j = 0; j < ranges.Length; j++)
                {
                    if (ranges[j] == 0)
                    {
                        AgingCO agingCO = new AgingCO();
                        agingCO.PaidAmount = 0;
                        agingCO.PlanAmount = 0;
                        agingCO.Total = 0;
                        agingCO.Type = rangeValues[j];
                        data.Add(agingCO);
                    }
                }

                List<AgingCOThird> finalData = new List<AgingCOThird>();
                List<long> paidAmounts = new List<long>();
                List<long> patientAmounts = new List<long>();
                List<long> Total = new List<long>();
                long totalPaidAmount = 0, totalPatientAmount = 0;

                AgingCOThird temp = new AgingCOThird();

                List<AgingCOThird> finalObj = new List<AgingCOThird>();
                AgingCOThird tempPlan = new AgingCOThird();
                AgingCOThird tempPatient = new AgingCOThird();
                AgingCOThird tempTotal = new AgingCOThird();
                tempPlan.Name = "Plan";
                tempPatient.Name = "Patient";
                tempTotal.Name = "Total";
                data = data.OrderBy(lamb => lamb.Type).ToList();
                for (int k = 0; k < data.Count; k++)
                {
                    switch (k)
                    {
                        case 0:
                            tempPlan.Range0_30 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range0_30 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range0_30 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 1:
                            tempPlan.Range31_60 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range31_60 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range31_60 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 2:
                            tempPlan.Range61_90 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range61_90 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range61_90 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 3:
                            tempPlan.Range91_120 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range91_120 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range91_120 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 4:
                            tempPlan.Range120plus = data.ElementAt(k).PlanAmount;
                            tempPatient.Range120plus = data.ElementAt(k).PaidAmount;
                            tempTotal.Range120plus = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        default:
                            break;
                    }
                }

                tempPlan.Total = tempPlan.Range0_30 + tempPlan.Range31_60 + tempPlan.Range61_90 + tempPlan.Range91_120 + tempPlan.Range120plus;
                tempPatient.Total = tempPatient.Range0_30 + tempPatient.Range31_60 + tempPatient.Range61_90 + tempPatient.Range91_120 + tempPatient.Range120plus;
                tempTotal.Total = tempTotal.Range0_30 + tempTotal.Range31_60 + tempTotal.Range61_90 + tempTotal.Range91_120 + tempTotal.Range120plus;


                finalObj.Add(tempPlan);
                finalObj.Add(tempPatient);
                finalObj.Add(tempTotal);
                //Total.Add(totalPaidAmount);
                //Total.Add(totalPatientAmount);
                //finalObj.PaidAmounts = paidAmounts;
                //finalObj.PatientAmounts = patientAmounts;
                //finalObj.Total = Total;
                return finalObj;
            }
            else if (cAgingCO.Value == "AD")
            {

                List<AgingCOThird> agingCOThird = new List<AgingCOThird>();
                List<AgingCO> data = new List<AgingCO>();
                using (SqlConnection myConnection = new SqlConnection(newConnection))
                {
                    // --claimAge = (visittable.DateOfServiceFrom.HasValue ? GetClaimAge(visittable.DateOfServiceFrom.Value) : 0),
                    string oString = "SELECT 	SUM( 		case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 		case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 		case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 			) as planAmount  , 	SUM(ISNULL(chargetable.PrimaryPatientBal, 0) + ISNULL(chargetable.SecondaryPatientBal, 0) + ISNULL(chargetable.TertiaryPatientBal, 0)) as patientAmount  , 	SUM( 		case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 		case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 		case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 	+		 ISNULL(chargetable.PrimaryPatientBal, 0) +  ISNULL(chargetable.SecondaryPatientBal, 0) + ISNULL(chargetable.TertiaryPatientBal, 0)		) as total , case  when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )>= 0 then 1 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 30 then 2 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 60 then 3 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 90 then 4 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 120 then 5 else 0 end as claimAge   FROM Charge chargetable join  Patient patienttable on chargetable.PatientID = patienttable.ID join Visit v on chargetable.VisitID = v.ID join Practice practicetable on chargetable.PracticeID = practicetable.ID join [Provider] providertable on chargetable.ProviderID =  providertable.ID join [Location] locationtable on chargetable.LocationID = locationtable.ID join PatientPlan patientplantable on chargetable.PrimaryPatientPlanID = patientplantable.ID join InsurancePlan insurangeplantable on patientplantable.InsurancePlanID = insurangeplantable.ID join Cpt cpt on chargetable.CPTID = cpt.ID left join  RefProvider refprovidertable on chargetable.RefProviderID = refprovidertable.ID left join PatientPlan secpatientplantable on chargetable.SecondaryPatientPlanID = secpatientplantable.ID left join PatientPlan terpatientplantable on chargetable.TertiaryPatientPlanID = terpatientplantable.ID left join InsurancePlan secinsuranceplantable on secpatientplantable.InsurancePlanID = secinsuranceplantable.ID left join InsurancePlan terinsuranceplantable on terpatientplantable.InsurancePlanID = terinsuranceplantable.ID  where chargetable.AddedDate IS NOT NULL   GROUP BY case when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )>= 0 then 1 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 30 then 2 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 60 then 3 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 90 then 4 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 120 then 5 else 0    end  having ( SUM(			case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +			 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 			 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 			) >  0)  ";
                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            var aging = new AgingCO();
                            if (Convert.ToInt32(oReader["claimAge"].ToString()) == 1)
                            {
                                aging.Type = "between0and30";
                            }
                            else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 2)
                            {
                                aging.Type = "between31and60";
                            }
                            else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 3)
                            {
                                aging.Type = "between61and90";
                            }
                            else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 4)
                            {
                                aging.Type = "between91and120";
                            }
                            else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 5)
                            {
                                aging.Type = "greaterThan120";
                            }
                            aging.Total = Convert.ToDecimal(oReader["total"].ToString());
                            aging.PlanAmount = Convert.ToDecimal(oReader["planAmount"].ToString());
                            aging.PaidAmount = Convert.ToDecimal(oReader["patientAmount"].ToString());
                            data.Add(aging);
                        }
                        myConnection.Close();
                    }
                }
                //List<AgingCO> data = (from chargetable in _context.Charge
                //                      join pat in _context.Patient on chargetable.PatientID equals pat.ID
                //                      join visittable in _context.Visit on chargetable.VisitID equals visittable.ID
                //                      join practicetable in _context.Practice on chargetable.PracticeID equals practicetable.ID
                //                      join providertable in _context.Provider on chargetable.ProviderID equals providertable.ID
                //                      join locationtable in _context.Location on chargetable.LocationID equals locationtable.ID
                //                      join patientplantable in _context.PatientPlan on chargetable.PrimaryPatientPlanID equals patientplantable.ID
                //                      join insurangeplantable in _context.InsurancePlan on patientplantable.InsurancePlanID equals insurangeplantable.ID

                //                      where (visittable.AddedDate.Date.IsNull() ? false : true) && (visittable.PracticeID == PracticeId)
                //                      group new
                //                      {
                //                          planAmount = (chargetable.PrimaryBal.Val()) + (chargetable.SecondaryBal.Val()) + (chargetable.TertiaryBal.Val()),
                //                          patientAmount = (visittable.PrimaryPatientBal.Val()) + (visittable.SecondaryPatientBal.Val()) + (visittable.TertiaryPatientBal.Val()),
                //                          claimAge = (visittable.DateOfServiceFrom.HasValue ? GetClaimAge(visittable.DateOfServiceFrom.Value) : 0),
                //                          total = (
                //                      (chargetable.PrimaryBal.Val()) + (chargetable.SecondaryPaid.Val()) + (chargetable.TertiaryPaid.Val()) +
                //                      (visittable.PrimaryPatientBal.Val()) + (visittable.SecondaryPatientBal.Val()) + (visittable.TertiaryPatientBal.Val())
                //                      )
                //                      } by new { ClaimAge = CheckClaimAge((visittable.AddedDate.Date.IsNull() ? GetClaimAge(visittable.AddedDate.Date) : 0)) } into gp

                //                      select new AgingCO
                //                      {
                //                          Type = gp.Key.ClaimAge == 1 ? "between0and30" : gp.Key.ClaimAge == 2 ? "between31and60" : gp.Key.ClaimAge == 3 ? "between61and90" : gp.Key.ClaimAge == 4 ? "between91and120" : "greaterThan120",
                //                          PlanAmount = gp.Select(a => a.planAmount).Sum(),
                //                          PaidAmount = gp.Select(a => a.patientAmount).Sum(),
                //                          Total = gp.Select(a => a.total).Sum(),

                //                      }).OrderBy(lamb => lamb.Type).ToList();


                Debug.WriteLine(data.Count);
                int[] ranges = new int[] { 0, 0, 0, 0, 0 };
                string[] rangeValues = new string[] { "between0and30", "between31and60", "between61and90", "between91and120", "greaterThan120" };

                for (int i = 0; i < data.Count; i++)
                {
                    if (data.ElementAt(i).Type.Equals("between0and30"))
                    {
                        ranges[0] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("between31and60"))
                    {
                        ranges[1] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("between61and90"))
                    {
                        ranges[2] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("between91and120"))
                    {
                        ranges[3] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("greaterThan120"))
                    {
                        ranges[4] = 1;
                    }
                    Debug.WriteLine(data.ElementAt(i).Type + " :: " + data.ElementAt(i).Total);
                }
                for (int j = 0; j < ranges.Length; j++)
                {
                    if (ranges[j] == 0)
                    {
                        AgingCO agingCO = new AgingCO();
                        agingCO.PaidAmount = 0;
                        agingCO.PlanAmount = 0;
                        agingCO.Total = 0;
                        agingCO.Type = rangeValues[j];
                        data.Add(agingCO);
                    }
                }

                List<AgingCOThird> finalData = new List<AgingCOThird>();
                List<long> paidAmounts = new List<long>();
                List<long> patientAmounts = new List<long>();
                List<long> Total = new List<long>();
                long totalPaidAmount = 0, totalPatientAmount = 0;

                AgingCOThird temp = new AgingCOThird();

                List<AgingCOThird> finalObj = new List<AgingCOThird>();
                AgingCOThird tempPlan = new AgingCOThird();
                AgingCOThird tempPatient = new AgingCOThird();
                AgingCOThird tempTotal = new AgingCOThird();
                tempPlan.Name = "Plan";
                tempPatient.Name = "Patient";
                tempTotal.Name = "Total";
                data = data.OrderBy(lamb => lamb.Type).ToList();
                for (int k = 0; k < data.Count; k++)
                {
                    switch (k)
                    {
                        case 0:
                            tempPlan.Range0_30 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range0_30 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range0_30 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 1:
                            tempPlan.Range31_60 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range31_60 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range31_60 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 2:
                            tempPlan.Range61_90 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range61_90 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range61_90 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 3:
                            tempPlan.Range91_120 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range91_120 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range91_120 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 4:
                            tempPlan.Range120plus = data.ElementAt(k).PlanAmount;
                            tempPatient.Range120plus = data.ElementAt(k).PaidAmount;
                            tempTotal.Range120plus = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        default:
                            break;
                    }
                }

                tempPlan.Total = tempPlan.Range0_30 + tempPlan.Range31_60 + tempPlan.Range61_90 + tempPlan.Range91_120 + tempPlan.Range120plus;
                tempPatient.Total = tempPatient.Range0_30 + tempPatient.Range31_60 + tempPatient.Range61_90 + tempPatient.Range91_120 + tempPatient.Range120plus;
                tempTotal.Total = tempTotal.Range0_30 + tempTotal.Range31_60 + tempTotal.Range61_90 + tempTotal.Range91_120 + tempTotal.Range120plus;


                finalObj.Add(tempPlan);
                finalObj.Add(tempPatient);
                finalObj.Add(tempTotal);
                //Total.Add(totalPaidAmount);
                //Total.Add(totalPatientAmount);
                //finalObj.PaidAmounts = paidAmounts;
                //finalObj.PatientAmounts = patientAmounts;
                //finalObj.Total = Total;
                return finalObj;
            }

            else if (cAgingCO.Value == "PD")
            {

                List<AgingCOThird> agingCOThird = new List<AgingCOThird>();
                List<AgingCO> data = new List<AgingCO>();
                using (SqlConnection myConnection = new SqlConnection(newConnection))
                {
                    // --claimAge = (visittable.DateOfServiceFrom.HasValue ? GetClaimAge(visittable.DateOfServiceFrom.Value) : 0),
                    string oString = "SELECT 	SUM( 		case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 		case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 		case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 			) as planAmount  , 	SUM(ISNULL(chargetable.PrimaryPatientBal, 0) + ISNULL(chargetable.SecondaryPatientBal, 0) + ISNULL(chargetable.TertiaryPatientBal, 0)) as patientAmount  , 	SUM( 		case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 		case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 		case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 	+		 ISNULL(chargetable.PrimaryPatientBal, 0) +  ISNULL(chargetable.SecondaryPatientBal, 0) + ISNULL(chargetable.TertiaryPatientBal, 0)		) as total , case  when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )>= 0 then 1 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 30 then 2 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 60 then 3 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 90 then 4 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 120 then 5 else 0 end as claimAge   FROM Charge chargetable join  Patient patienttable on chargetable.PatientID = patienttable.ID join Visit v on chargetable.VisitID = v.ID join Practice practicetable on chargetable.PracticeID = practicetable.ID join [Provider] providertable on chargetable.ProviderID =  providertable.ID join [Location] locationtable on chargetable.LocationID = locationtable.ID join PatientPlan patientplantable on chargetable.PrimaryPatientPlanID = patientplantable.ID join InsurancePlan insurangeplantable on patientplantable.InsurancePlanID = insurangeplantable.ID join Cpt cpt on chargetable.CPTID = cpt.ID left join  RefProvider refprovidertable on chargetable.RefProviderID = refprovidertable.ID left join PatientPlan secpatientplantable on chargetable.SecondaryPatientPlanID = secpatientplantable.ID left join PatientPlan terpatientplantable on chargetable.TertiaryPatientPlanID = terpatientplantable.ID left join InsurancePlan secinsuranceplantable on secpatientplantable.InsurancePlanID = secinsuranceplantable.ID left join InsurancePlan terinsuranceplantable on terpatientplantable.InsurancePlanID = terinsuranceplantable.ID  where chargetable.AddedDate IS NOT NULL   GROUP BY case when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )>= 0 then 1 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 30 then 2 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 60 then 3 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 90 then 4 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 120 then 5 else 0    end  having ( SUM(			case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +			 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 			 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 			) >  0)  ";
                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            var aging = new AgingCO();
                            if (Convert.ToInt32(oReader["claimAge"].ToString()) == 1)
                            {
                                aging.Type = "between0and30";
                            }
                            else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 2)
                            {
                                aging.Type = "between31and60";
                            }
                            else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 3)
                            {
                                aging.Type = "between61and90";
                            }
                            else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 4)
                            {
                                aging.Type = "between91and120";
                            }
                            else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 5)
                            {
                                aging.Type = "greaterThan120";
                            }
                            aging.Total = Convert.ToDecimal(oReader["total"].ToString());
                            aging.PlanAmount = Convert.ToDecimal(oReader["planAmount"].ToString());
                            aging.PaidAmount = Convert.ToDecimal(oReader["patientAmount"].ToString());
                            data.Add(aging);
                        }
                        myConnection.Close();
                    }
                }
                //List<AgingCO> data = (from chargetable in _context.Charge
                //                      join pat in _context.Patient on chargetable.PatientID equals pat.ID
                //                      join visittable in _context.Visit on chargetable.VisitID equals visittable.ID
                //                      join practicetable in _context.Practice on chargetable.PracticeID equals practicetable.ID
                //                      join providertable in _context.Provider on chargetable.ProviderID equals providertable.ID
                //                      join locationtable in _context.Location on chargetable.LocationID equals locationtable.ID
                //                      join patientplantable in _context.PatientPlan on chargetable.PrimaryPatientPlanID equals patientplantable.ID
                //                      join insurangeplantable in _context.InsurancePlan on patientplantable.InsurancePlanID equals insurangeplantable.ID

                //                      where (visittable.AddedDate.Date.IsNull() ? false : true) && (visittable.PracticeID == PracticeId)
                //                      group new
                //                      {
                //                          planAmount = (chargetable.PrimaryBal.Val()) + (chargetable.SecondaryBal.Val()) + (chargetable.TertiaryBal.Val()),
                //                          patientAmount = (visittable.PrimaryPatientBal.Val()) + (visittable.SecondaryPatientBal.Val()) + (visittable.TertiaryPatientBal.Val()),
                //                          claimAge = (visittable.DateOfServiceFrom.HasValue ? GetClaimAge(visittable.DateOfServiceFrom.Value) : 0),
                //                          total = (
                //                      (chargetable.PrimaryBal.Val()) + (chargetable.SecondaryPaid.Val()) + (chargetable.TertiaryPaid.Val()) +
                //                      (visittable.PrimaryPatientBal.Val()) + (visittable.SecondaryPatientBal.Val()) + (visittable.TertiaryPatientBal.Val())
                //                      )
                //                      } by new { ClaimAge = CheckClaimAge((visittable.AddedDate.Date.IsNull() ? GetClaimAge(visittable.AddedDate.Date) : 0)) } into gp

                //                      select new AgingCO
                //                      {
                //                          Type = gp.Key.ClaimAge == 1 ? "between0and30" : gp.Key.ClaimAge == 2 ? "between31and60" : gp.Key.ClaimAge == 3 ? "between61and90" : gp.Key.ClaimAge == 4 ? "between91and120" : "greaterThan120",
                //                          PlanAmount = gp.Select(a => a.planAmount).Sum(),
                //                          PaidAmount = gp.Select(a => a.patientAmount).Sum(),
                //                          Total = gp.Select(a => a.total).Sum(),

                //                      }).OrderBy(lamb => lamb.Type).ToList();


                Debug.WriteLine(data.Count);
                int[] ranges = new int[] { 0, 0, 0, 0, 0 };
                string[] rangeValues = new string[] { "between0and30", "between31and60", "between61and90", "between91and120", "greaterThan120" };

                for (int i = 0; i < data.Count; i++)
                {
                    if (data.ElementAt(i).Type.Equals("between0and30"))
                    {
                        ranges[0] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("between31and60"))
                    {
                        ranges[1] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("between61and90"))
                    {
                        ranges[2] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("between91and120"))
                    {
                        ranges[3] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("greaterThan120"))
                    {
                        ranges[4] = 1;
                    }
                    Debug.WriteLine(data.ElementAt(i).Type + " :: " + data.ElementAt(i).Total);
                }
                for (int j = 0; j < ranges.Length; j++)
                {
                    if (ranges[j] == 0)
                    {
                        AgingCO agingCO = new AgingCO();
                        agingCO.PaidAmount = 0;
                        agingCO.PlanAmount = 0;
                        agingCO.Total = 0;
                        agingCO.Type = rangeValues[j];
                        data.Add(agingCO);
                    }
                }

                List<AgingCOThird> finalData = new List<AgingCOThird>();
                List<long> paidAmounts = new List<long>();
                List<long> patientAmounts = new List<long>();
                List<long> Total = new List<long>();
                long totalPaidAmount = 0, totalPatientAmount = 0;

                AgingCOThird temp = new AgingCOThird();

                List<AgingCOThird> finalObj = new List<AgingCOThird>();
                AgingCOThird tempPlan = new AgingCOThird();
                AgingCOThird tempPatient = new AgingCOThird();
                AgingCOThird tempTotal = new AgingCOThird();
                tempPlan.Name = "Plan";
                tempPatient.Name = "Patient";
                tempTotal.Name = "Total";
                data = data.OrderBy(lamb => lamb.Type).ToList();
                for (int k = 0; k < data.Count; k++)
                {
                    switch (k)
                    {
                        case 0:
                            tempPlan.Range0_30 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range0_30 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range0_30 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 1:
                            tempPlan.Range31_60 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range31_60 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range31_60 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 2:
                            tempPlan.Range61_90 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range61_90 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range61_90 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 3:
                            tempPlan.Range91_120 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range91_120 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range91_120 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 4:
                            tempPlan.Range120plus = data.ElementAt(k).PlanAmount;
                            tempPatient.Range120plus = data.ElementAt(k).PaidAmount;
                            tempTotal.Range120plus = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        default:
                            break;
                    }
                }

                tempPlan.Total = tempPlan.Range0_30 + tempPlan.Range31_60 + tempPlan.Range61_90 + tempPlan.Range91_120 + tempPlan.Range120plus;
                tempPatient.Total = tempPatient.Range0_30 + tempPatient.Range31_60 + tempPatient.Range61_90 + tempPatient.Range91_120 + tempPatient.Range120plus;
                tempTotal.Total = tempTotal.Range0_30 + tempTotal.Range31_60 + tempTotal.Range61_90 + tempTotal.Range91_120 + tempTotal.Range120plus;


                finalObj.Add(tempPlan);
                finalObj.Add(tempPatient);
                finalObj.Add(tempTotal);
                //Total.Add(totalPaidAmount);
                //Total.Add(totalPatientAmount);
                //finalObj.PaidAmounts = paidAmounts;
                //finalObj.PatientAmounts = patientAmounts;
                //finalObj.Total = Total;
                return finalObj;
            }
            else
            {

                List<AgingCOThird> agingCOThird = new List<AgingCOThird>();
                List<AgingCO> data = new List<AgingCO>();
                using (SqlConnection myConnection = new SqlConnection(newConnection))
                {
                    // --claimAge = (visittable.DateOfServiceFrom.HasValue ? GetClaimAge(visittable.DateOfServiceFrom.Value) : 0),
                    string oString = "SELECT 	SUM( 		case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 		case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 		case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 			) as planAmount  , 	SUM(ISNULL(chargetable.PrimaryPatientBal, 0) + ISNULL(chargetable.SecondaryPatientBal, 0) + ISNULL(chargetable.TertiaryPatientBal, 0)) as patientAmount  , 	SUM( 		case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 		case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 		case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 	+		 ISNULL(chargetable.PrimaryPatientBal, 0) +  ISNULL(chargetable.SecondaryPatientBal, 0) + ISNULL(chargetable.TertiaryPatientBal, 0)		) as total , 		case  when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )>= 0 then 1 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 30 then 2 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 60 then 3 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 90 then 4 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 120 then 5 else 0 end as claimAge   FROM Charge chargetable join  Patient patienttable on chargetable.PatientID = patienttable.ID join Visit v on chargetable.VisitID = v.ID join Practice practicetable on chargetable.PracticeID = practicetable.ID join [Provider] providertable on chargetable.ProviderID =  providertable.ID join [Location] locationtable on chargetable.LocationID = locationtable.ID join PatientPlan patientplantable on chargetable.PrimaryPatientPlanID = patientplantable.ID join InsurancePlan insurangeplantable on patientplantable.InsurancePlanID = insurangeplantable.ID join Cpt cpt on chargetable.CPTID = cpt.ID left join  RefProvider refprovidertable on chargetable.RefProviderID = refprovidertable.ID left join PatientPlan secpatientplantable on chargetable.SecondaryPatientPlanID = secpatientplantable.ID left join PatientPlan terpatientplantable on chargetable.TertiaryPatientPlanID = terpatientplantable.ID left join InsurancePlan secinsuranceplantable on secpatientplantable.InsurancePlanID = secinsuranceplantable.ID left join InsurancePlan terinsuranceplantable on terpatientplantable.InsurancePlanID = terinsuranceplantable.ID  where chargetable.SubmittetdDate IS NOT NULL  GROUP BY case when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )>= 0 then 1 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 30 then 2 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 60 then 3 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 90 then 4 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 120 then 5 else 0    end  having ( SUM(			case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +			 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 			 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 			) >  0) ";
                    SqlCommand oCmd = new SqlCommand(oString, myConnection);
                    myConnection.Open();
                    using (SqlDataReader oReader = oCmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            var aging = new AgingCO();
                            if (Convert.ToInt32(oReader["claimAge"].ToString()) == 1)
                            {
                                aging.Type = "between0and30";
                            }
                            else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 2)
                            {
                                aging.Type = "between31and60";
                            }
                            else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 3)
                            {
                                aging.Type = "between61and90";
                            }
                            else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 4)
                            {
                                aging.Type = "between91and120";
                            }
                            else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 5)
                            {
                                aging.Type = "greaterThan120";
                            }
                            aging.Total = Convert.ToDecimal(oReader["total"].ToString());
                            aging.PlanAmount = Convert.ToDecimal(oReader["planAmount"].ToString());
                            aging.PaidAmount = Convert.ToDecimal(oReader["patientAmount"].ToString());
                            data.Add(aging);
                        }
                        myConnection.Close();
                    }
                }


                //List<AgingCO> data = (from chargetable in _context.Charge
                //                      join pat in _context.Patient on chargetable.PatientID equals pat.ID
                //                      join visittable in _context.Visit on chargetable.VisitID equals visittable.ID
                //                      join practicetable in _context.Practice on chargetable.PracticeID equals practicetable.ID
                //                      join providertable in _context.Provider on chargetable.ProviderID equals providertable.ID
                //                      join locationtable in _context.Location on chargetable.LocationID equals locationtable.ID
                //                      join patientplantable in _context.PatientPlan on chargetable.PrimaryPatientPlanID equals patientplantable.ID
                //                      join insurangeplantable in _context.InsurancePlan on patientplantable.InsurancePlanID equals insurangeplantable.ID
                //                      where (visittable.SubmittedDate.HasValue ? true : false) && (visittable.PracticeID == PracticeId)
                //                      group new
                //                      {
                //                          planAmount = (chargetable.PrimaryBal.Val()) + (chargetable.SecondaryBal.Val()) + (chargetable.TertiaryBal.Val()),
                //                          patientAmount = (chargetable.PrimaryPatientBal.Val()) + (chargetable.SecondaryPatientBal.Val()) + (chargetable.TertiaryPatientBal.Val()),
                //                          claimAge = (chargetable.SubmittetdDate.HasValue ? GetClaimAge(chargetable.SubmittetdDate.Value) : 0),
                //                          total = (
                //                      (chargetable.PrimaryBal.Val()) + (chargetable.SecondaryPaid.Val()) + (chargetable.TertiaryPaid.Val()) +
                //                      (visittable.PrimaryPatientBal.Val()) + (visittable.SecondaryPatientBal.Val()) + (visittable.TertiaryPatientBal.Val()))
                //                      } by new { ClaimAge = CheckClaimAge((visittable.SubmittedDate.HasValue ? GetClaimAge(visittable.SubmittedDate.Value) : 0)) } into gp

                //                      select new AgingCO
                //                      {
                //                          Type = gp.Key.ClaimAge == 1 ? "between0and30" : gp.Key.ClaimAge == 2 ? "between31and60" : gp.Key.ClaimAge == 3 ? "between61and90" : gp.Key.ClaimAge == 4 ? "between91and120" : "greaterThan120",
                //                          PlanAmount = gp.Select(a => a.planAmount).Sum(),
                //                          PaidAmount = gp.Select(a => a.patientAmount).Sum(),
                //                          Total = gp.Select(a => a.total).Sum(),

                //                      }).OrderBy(lamb => lamb.Type).ToList();



                Debug.WriteLine(data.Count);
                int[] ranges = new int[] { 0, 0, 0, 0, 0 };
                string[] rangeValues = new string[] { "between0and30", "between31and60", "between61and90", "between91and120", "greaterThan120" };

                for (int i = 0; i < data.Count; i++)
                {
                    if (data.ElementAt(i).Type.Equals("between0and30"))
                    {
                        ranges[0] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("between31and60"))
                    {
                        ranges[1] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("between61and90"))
                    {
                        ranges[2] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("between91and120"))
                    {
                        ranges[3] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("greaterThan120"))
                    {
                        ranges[4] = 1;
                    }
                    Debug.WriteLine(data.ElementAt(i).Type + " :: " + data.ElementAt(i).Total);
                }
                for (int j = 0; j < ranges.Length; j++)
                {
                    if (ranges[j] == 0)
                    {
                        AgingCO agingCO = new AgingCO();
                        agingCO.PaidAmount = 0;
                        agingCO.PlanAmount = 0;
                        agingCO.Total = 0;
                        agingCO.Type = rangeValues[j];
                        data.Add(agingCO);
                    }
                }

                List<AgingCOThird> finalData = new List<AgingCOThird>();
                List<long> paidAmounts = new List<long>();
                List<long> patientAmounts = new List<long>();
                List<long> Total = new List<long>();
                long totalPaidAmount = 0, totalPatientAmount = 0;

                AgingCOThird temp = new AgingCOThird();

                List<AgingCOThird> finalObj = new List<AgingCOThird>();
                AgingCOThird tempPlan = new AgingCOThird();
                AgingCOThird tempPatient = new AgingCOThird();
                AgingCOThird tempTotal = new AgingCOThird();
                tempPlan.Name = "Plan";
                tempPatient.Name = "Patient";
                tempTotal.Name = "Total";
                data = data.OrderBy(lamb => lamb.Type).ToList();
                for (int k = 0; k < data.Count; k++)
                {
                    switch (k)
                    {
                        case 0:
                            tempPlan.Range0_30 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range0_30 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range0_30 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 1:
                            tempPlan.Range31_60 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range31_60 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range31_60 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 2:
                            tempPlan.Range61_90 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range61_90 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range61_90 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 3:
                            tempPlan.Range91_120 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range91_120 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range91_120 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 4:
                            tempPlan.Range120plus = data.ElementAt(k).PlanAmount;
                            tempPatient.Range120plus = data.ElementAt(k).PaidAmount;
                            tempTotal.Range120plus = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        default:
                            break;
                    }
                }

                tempPlan.Total = tempPlan.Range0_30 + tempPlan.Range31_60 + tempPlan.Range61_90 + tempPlan.Range91_120 + tempPlan.Range120plus;
                tempPatient.Total = tempPatient.Range0_30 + tempPatient.Range31_60 + tempPatient.Range61_90 + tempPatient.Range91_120 + tempPatient.Range120plus;
                tempTotal.Total = tempTotal.Range0_30 + tempTotal.Range31_60 + tempTotal.Range61_90 + tempTotal.Range91_120 + tempTotal.Range120plus;


                finalObj.Add(tempPlan);
                finalObj.Add(tempPatient);
                finalObj.Add(tempTotal);
                //Total.Add(totalPaidAmount);
                //Total.Add(totalPatientAmount);
                //finalObj.PaidAmounts = paidAmounts;
                //finalObj.PatientAmounts = patientAmounts;
                //finalObj.Total = Total;
                return finalObj;
            }




            //else
            //{
            //    List<AgingCOThird> agingCOThird = new List<AgingCOThird>();
            //    List<AgingCO> data = new List<AgingCO>();
            //    using (SqlConnection myConnection = new SqlConnection(newConnection))
            //    {
            //        // --claimAge = (visittable.DateOfServiceFrom.HasValue ? GetClaimAge(visittable.DateOfServiceFrom.Value) : 0),
            //        string oString = "SELECT SUM(ISNULL(v.PrimaryBal, 0) + ISNULL(v.SecondaryBal, 0) + ISNULL(v.TertiaryBal, 0)) as planAmount  ,SUM(ISNULL(v.PrimaryPatientBal, 0) + ISNULL(v.SecondaryPatientBal, 0) + ISNULL(v.TertiaryPatientBal, 0)) as patientAmount  ,SUM(ISNULL(v.PrimaryBal, 0) + ISNULL(v.SecondaryBal, 0) + ISNULL(v.TertiaryBal, 0) +  ISNULL(v.PrimaryPatientBal, 0) + ISNULL(v.SecondaryPatientBal, 0) + ISNULL(v.TertiaryPatientBal, 0)) as total" +
            //            ",  case " +
            //            "	when ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )>= 0 then 1" +
            //            "    when ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )> 30 then 2" +
            //            "    when ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )> 60 then 3" +
            //            "    when ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )> 90 then 4" +
            //            "    when ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )> 120 then 5" +
            //            "    else 0" +
            //            "    end as claimAge  FROM Visit v join Patient pat on v.PatientID = pat.ID join Practice prac on pat.PracticeID = prac.ID  join[Location] loc on pat.LocationId = loc.ID join[Provider] prov on pat.ProviderID = prov.ID join PatientPlan pPlan on v.PrimaryPatientPlanID = pPlan.ID join InsurancePlan iPlan on pPlan.InsurancePlanID = iPlan.ID  where v.DateOfServiceFrom IS NOT NULL AND v.PracticeID = " + PracticeId + " GROUP BY case " +
            //            "    when ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )>= 0 then 1" +
            //            "    when ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )> 30 then 2" +
            //            "    when ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )> 60 then 3" +
            //            "    when ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )> 90 then 4" +
            //            "    when ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )> 120 then 5" +
            //            "    else 0    end";
            //        SqlCommand oCmd = new SqlCommand(oString, myConnection);
            //        myConnection.Open();
            //        using (SqlDataReader oReader = oCmd.ExecuteReader())
            //        {
            //            while (oReader.Read())
            //            {
            //                var aging = new AgingCO();
            //                if (Convert.ToInt32(oReader["claimAge"].ToString()) == 1)
            //                {
            //                    aging.Type = "between0and30";
            //                }
            //                else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 2)
            //                {
            //                    aging.Type = "between31and60";
            //                }
            //                else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 3)
            //                {
            //                    aging.Type = "between61and90";
            //                }
            //                else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 4)
            //                {
            //                    aging.Type = "between91and120";
            //                }
            //                else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 5)
            //                {
            //                    aging.Type = "greaterThan120";
            //                }
            //                aging.Total = Convert.ToDecimal(oReader["total"].ToString());
            //                aging.PlanAmount = Convert.ToDecimal(oReader["planAmount"].ToString());
            //                aging.PaidAmount = Convert.ToDecimal(oReader["patientAmount"].ToString());
            //                data.Add(aging);
            //            }
            //            myConnection.Close();
            //        }
            //    }
            //    //List<AgingCO> data = (from chargetable in _context.Charge
            //    //                      join pat in _context.Patient on chargetable.PatientID equals pat.ID
            //    //                      join visittable in _context.Visit on chargetable.VisitID equals visittable.ID
            //    //                      join practicetable in _context.Practice on chargetable.PracticeID equals practicetable.ID
            //    //                      join providertable in _context.Provider on chargetable.ProviderID equals providertable.ID
            //    //                      join locationtable in _context.Location on chargetable.LocationID equals locationtable.ID
            //    //                      join patientplantable in _context.PatientPlan on chargetable.PrimaryPatientPlanID equals patientplantable.ID
            //    //                      join insurangeplantable in _context.InsurancePlan on patientplantable.InsurancePlanID equals insurangeplantable.ID
            //    //                      where (visittable.SubmittedDate.HasValue ? true : false) && (visittable.PracticeID == PracticeId)
            //    //                      group new
            //    //                      {
            //    //                          planAmount = (chargetable.PrimaryBal.Val()) + (chargetable.SecondaryBal.Val()) + (chargetable.TertiaryBal.Val()),
            //    //                          patientAmount = (visittable.PrimaryPatientBal.Val()) + (visittable.SecondaryPatientBal.Val()) + (visittable.TertiaryPatientBal.Val()),
            //    //                          claimAge = (visittable.DateOfServiceFrom.HasValue ? GetClaimAge(visittable.DateOfServiceFrom.Value) : 0),
            //    //                          total = (
            //    //                      (chargetable.PrimaryBal.Val()) + (chargetable.SecondaryPaid.Val()) + (chargetable.TertiaryPaid.Val()) +
            //    //                      (visittable.PrimaryPatientBal.Val()) + (visittable.SecondaryPatientBal.Val()) + (visittable.TertiaryPatientBal.Val()))
            //    //                      } by new { ClaimAge = CheckClaimAge((visittable.SubmittedDate.HasValue ? GetClaimAge(visittable.SubmittedDate.Value) : 0)) } into gp

            //    //                      select new AgingCO
            //    //                      {
            //    //                          Type = gp.Key.ClaimAge == 1 ? "between0and30" : gp.Key.ClaimAge == 2 ? "between31and60" : gp.Key.ClaimAge == 3 ? "between61and90" : gp.Key.ClaimAge == 4 ? "between91and120" : "greaterThan120",
            //    //                          PlanAmount = gp.Select(a => a.planAmount).Sum(),
            //    //                          PaidAmount = gp.Select(a => a.patientAmount).Sum(),
            //    //                          Total = gp.Select(a => a.total).Sum(),

            //    //                      }).OrderBy(lamb => lamb.Type).ToList();


            //    Debug.WriteLine(data.Count);
            //    int[] ranges = new int[] { 0, 0, 0, 0, 0 };
            //    string[] rangeValues = new string[] { "between0and30", "between31and60", "between61and90", "between91and120", "greaterThan120" };

            //    for (int i = 0; i < data.Count; i++)
            //    {
            //        if (data.ElementAt(i).Type.Equals("between0and30"))
            //        {
            //            ranges[0] = 1;
            //        }
            //        else if (data.ElementAt(i).Type.Equals("between31and60"))
            //        {
            //            ranges[1] = 1;
            //        }
            //        else if (data.ElementAt(i).Type.Equals("between61and90"))
            //        {
            //            ranges[2] = 1;
            //        }
            //        else if (data.ElementAt(i).Type.Equals("between91and120"))
            //        {
            //            ranges[3] = 1;
            //        }
            //        else if (data.ElementAt(i).Type.Equals("greaterThan120"))
            //        {
            //            ranges[4] = 1;
            //        }
            //        Debug.WriteLine(data.ElementAt(i).Type + " :: " + data.ElementAt(i).Total);
            //    }
            //    for (int j = 0; j < ranges.Length; j++)
            //    {
            //        if (ranges[j] == 0)
            //        {
            //            AgingCO agingCO = new AgingCO();
            //            agingCO.PaidAmount = 0;
            //            agingCO.PlanAmount = 0;
            //            agingCO.Total = 0;
            //            agingCO.Type = rangeValues[j];
            //            data.Add(agingCO);
            //        }
            //    }

            //    List<AgingCOThird> finalData = new List<AgingCOThird>();
            //    List<long> paidAmounts = new List<long>();
            //    List<long> patientAmounts = new List<long>();
            //    List<long> Total = new List<long>();
            //    long totalPaidAmount = 0, totalPatientAmount = 0;

            //    AgingCOThird temp = new AgingCOThird();

            //    List<AgingCOThird> finalObj = new List<AgingCOThird>();
            //    AgingCOThird tempPlan = new AgingCOThird();
            //    AgingCOThird tempPatient = new AgingCOThird();
            //    AgingCOThird tempTotal = new AgingCOThird();
            //    tempPlan.Name = "Plan";
            //    tempPatient.Name = "Patient";
            //    tempTotal.Name = "Total";
            //    data = data.OrderBy(lamb => lamb.Type).ToList();
            //    for (int k = 0; k < data.Count; k++)
            //    {
            //        switch (k)
            //        {
            //            case 0:
            //                tempPlan.Range0_30 = data.ElementAt(k).PlanAmount;
            //                tempPatient.Range0_30 = data.ElementAt(k).PaidAmount;
            //                tempTotal.Range0_30 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
            //                break;
            //            case 1:
            //                tempPlan.Range31_60 = data.ElementAt(k).PlanAmount;
            //                tempPatient.Range31_60 = data.ElementAt(k).PaidAmount;
            //                tempTotal.Range31_60 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
            //                break;
            //            case 2:
            //                tempPlan.Range61_90 = data.ElementAt(k).PlanAmount;
            //                tempPatient.Range61_90 = data.ElementAt(k).PaidAmount;
            //                tempTotal.Range61_90 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
            //                break;
            //            case 3:
            //                tempPlan.Range91_120 = data.ElementAt(k).PlanAmount;
            //                tempPatient.Range91_120 = data.ElementAt(k).PaidAmount;
            //                tempTotal.Range91_120 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
            //                break;
            //            case 4:
            //                tempPlan.Range120plus = data.ElementAt(k).PlanAmount;
            //                tempPatient.Range120plus = data.ElementAt(k).PaidAmount;
            //                tempTotal.Range120plus = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
            //                break;
            //            default:
            //                break;
            //        }
            //    }

            //    tempPlan.Total = tempPlan.Range0_30 + tempPlan.Range31_60 + tempPlan.Range61_90 + tempPlan.Range91_120 + tempPlan.Range120plus;
            //    tempPatient.Total = tempPatient.Range0_30 + tempPatient.Range31_60 + tempPatient.Range61_90 + tempPatient.Range91_120 + tempPatient.Range120plus;
            //    tempTotal.Total = tempTotal.Range0_30 + tempTotal.Range31_60 + tempTotal.Range61_90 + tempTotal.Range91_120 + tempTotal.Range120plus;


            //    finalObj.Add(tempPlan);
            //    finalObj.Add(tempPatient);
            //    finalObj.Add(tempTotal);
            //    return finalObj;
            //}



        }



        //[Route("GetAgingData")]
        //[HttpPost]
        //public async Task<ActionResult<IEnumerable<AgingCOThird>>> GetAgingData(CAgingCO cAgingCO)
        //{
        //    //  UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
        //    //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
        //    //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
        //    //);

        //    //var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
        //    //var RoleClaim = User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase));
        //    long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
        //    var Client = (from w in _contextMain.MainPractice
        //                  where w.ID == PracticeId
        //                  select w
        //                ).FirstOrDefault();
        //    string contextName = _contextMain.MainClient.Find(Client.ClientID)?.ContextName;
        //    string newConnection = GetConnectionStringManager(contextName);

        //    if (cAgingCO.Value == "DOS")
        //    {

        //        List<AgingCOThird> agingCOThird = new List<AgingCOThird>();
        //        List<AgingCO> data = new List<AgingCO>();
        //        using (SqlConnection myConnection = new SqlConnection(newConnection))
        //        {
        //            // --claimAge = (visittable.DateOfServiceFrom.HasValue ? GetClaimAge(visittable.DateOfServiceFrom.Value) : 0),
        //            string oString = "SELECT SUM(ISNULL(v.PrimaryBal, 0) + ISNULL(v.SecondaryBal, 0) + ISNULL(v.TertiaryBal, 0)) as planAmount  ,SUM(ISNULL(v.PrimaryPatientBal, 0) + ISNULL(v.SecondaryPatientBal, 0) + ISNULL(v.TertiaryPatientBal, 0)) as patientAmount  ,SUM(ISNULL(v.PrimaryBal, 0) + ISNULL(v.SecondaryBal, 0) + ISNULL(v.TertiaryBal, 0) +  ISNULL(v.PrimaryPatientBal, 0) + ISNULL(v.SecondaryPatientBal, 0) + ISNULL(v.TertiaryPatientBal, 0)) as total" +
        //                ",  case " +
        //                "	when ISNULL(DATEDIFF(day, v.DateOfServiceFrom, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, v.DateOfServiceFrom, GETDATE()), 0 )>= 0 then 1" +
        //                "    when ISNULL(DATEDIFF(day, v.DateOfServiceFrom, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, v.DateOfServiceFrom, GETDATE()), 0 )> 30 then 2" +
        //                "    when ISNULL(DATEDIFF(day, v.DateOfServiceFrom, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, v.DateOfServiceFrom, GETDATE()), 0 )> 60 then 3" +
        //                "    when ISNULL(DATEDIFF(day, v.DateOfServiceFrom, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, v.DateOfServiceFrom, GETDATE()), 0 )> 90 then 4" +
        //                "    when ISNULL(DATEDIFF(day, v.DateOfServiceFrom, GETDATE()), 0 )> 120 then 5" +
        //                "    else 0" +
        //                "    end as claimAge  FROM Visit v join Patient pat on v.PatientID = pat.ID join Practice prac on pat.PracticeID = prac.ID  join[Location] loc on pat.LocationId = loc.ID join[Provider] prov on pat.ProviderID = prov.ID join PatientPlan pPlan on v.PrimaryPatientPlanID = pPlan.ID join InsurancePlan iPlan on pPlan.InsurancePlanID = iPlan.ID  where v.DateOfServiceFrom IS NOT NULL AND v.PracticeID = " + PracticeId + " GROUP BY case " +
        //                "    when ISNULL(DATEDIFF(day, v.DateOfServiceFrom, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, v.DateOfServiceFrom, GETDATE()), 0 )>= 0 then 1" +
        //                "    when ISNULL(DATEDIFF(day, v.DateOfServiceFrom, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, v.DateOfServiceFrom, GETDATE()), 0 )> 30 then 2" +
        //                "    when ISNULL(DATEDIFF(day, v.DateOfServiceFrom, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, v.DateOfServiceFrom, GETDATE()), 0 )> 60 then 3" +
        //                "    when ISNULL(DATEDIFF(day, v.DateOfServiceFrom, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, v.DateOfServiceFrom, GETDATE()), 0 )> 90 then 4" +
        //                "    when ISNULL(DATEDIFF(day, v.DateOfServiceFrom, GETDATE()), 0 )> 120 then 5" +
        //                "    else 0    end";
        //            SqlCommand oCmd = new SqlCommand(oString, myConnection);
        //            myConnection.Open();
        //            using (SqlDataReader oReader = oCmd.ExecuteReader())
        //            {
        //                while (oReader.Read())
        //                {
        //                    var aging = new AgingCO();
        //                    if (Convert.ToInt32(oReader["claimAge"].ToString()) == 1)
        //                    {
        //                        aging.Type = "between0and30";
        //                    }
        //                    else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 2)
        //                    {
        //                        aging.Type = "between31and60";
        //                    }
        //                    else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 3)
        //                    {
        //                        aging.Type = "between61and90";
        //                    }
        //                    else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 4)
        //                    {
        //                        aging.Type = "between91and120";
        //                    }
        //                    else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 5)
        //                    {
        //                        aging.Type = "greaterThan120";
        //                    }
        //                    aging.Total = Convert.ToDecimal(oReader["total"].ToString());
        //                    aging.PlanAmount = Convert.ToDecimal(oReader["planAmount"].ToString());
        //                    aging.PaidAmount = Convert.ToDecimal(oReader["patientAmount"].ToString());
        //                    data.Add(aging);
        //                }
        //                myConnection.Close();
        //            }
        //        }

        //        //  return agingCOThird;

        //        //List<AgingCO> data = (from visittable in _context.Visit
        //        //                      join pat in _context.Patient on visittable.PatientID equals pat.ID
        //        //                      join fac in _context.Practice
        //        //                      on pat.PracticeID equals fac.ID
        //        //                      join loc in _context.Location
        //        //                      on pat.LocationId equals loc.ID
        //        //                      join prov in _context.Provider
        //        //                      on pat.ProviderID equals prov.ID
        //        //                      join pPlan in _context.PatientPlan
        //        //                      on visittable.PrimaryPatientPlanID equals pPlan.ID
        //        //                      join iPlan in _context.InsurancePlan
        //        //                      on pPlan.InsurancePlanID equals iPlan.ID
        //        //                      where (visittable.DateOfServiceFrom.HasValue ? true : false)
        //        //                      && (visittable.PracticeID == PracticeId)
        //        //                      group new
        //        //                      {
        //        //                          planAmount = (visittable.PrimaryBal.Val()) + (visittable.SecondaryPaid.Val()) + (visittable.TertiaryPaid.Val()),
        //        //                          patientAmount = (visittable.PrimaryPatientBal.Val()) + (visittable.SecondaryPatientBal.Val()) + (visittable.TertiaryPatientBal.Val()),
        //        //                          claimAge = (visittable.DateOfServiceFrom.HasValue ? GetClaimAge(visittable.DateOfServiceFrom.Value) : 0),
        //        //                          total = (
        //        //                      (visittable.PrimaryBal.Val()) + (visittable.SecondaryPaid.Val()) + (visittable.TertiaryPaid.Val()) +
        //        //                      (visittable.PrimaryPatientBal.Val()) + (visittable.SecondaryPatientBal.Val()) + (visittable.TertiaryPatientBal.Val())
        //        //                      )
        //        //                      } by new { ClaimAge = CheckClaimAge((visittable.DateOfServiceFrom.HasValue ? GetClaimAge(visittable.DateOfServiceFrom.Value) : 0)) } into gp

        //        //                      select new AgingCO
        //        //                      {
        //        //                          Type = gp.Key.ClaimAge == 1 ? "between0and30" : gp.Key.ClaimAge == 2 ? "between31and60" : gp.Key.ClaimAge == 3 ? "between61and90" : gp.Key.ClaimAge == 4 ? "between91and120" : "greaterThan120",
        //        //                          PlanAmount = gp.Select(a => a.planAmount).Sum(),
        //        //                          PaidAmount = gp.Select(a => a.patientAmount).Sum(),
        //        //                          Total = gp.Select(a => a.total).Sum(),

        //        //                      }).OrderBy(lamb => lamb.Type).ToList();


        //        Debug.WriteLine(data.Count);
        //        int[] ranges = new int[] { 0, 0, 0, 0, 0 };
        //        string[] rangeValues = new string[] { "between0and30", "between31and60", "between61and90", "between91and120", "greaterThan120" };

        //        for (int i = 0; i < data.Count; i++)
        //        {
        //            if (data.ElementAt(i).Type.Equals("between0and30"))
        //            {
        //                ranges[0] = 1;
        //            }
        //            else if (data.ElementAt(i).Type.Equals("between31and60"))
        //            {
        //                ranges[1] = 1;
        //            }
        //            else if (data.ElementAt(i).Type.Equals("between61and90"))
        //            {
        //                ranges[2] = 1;
        //            }
        //            else if (data.ElementAt(i).Type.Equals("between91and120"))
        //            {
        //                ranges[3] = 1;
        //            }
        //            else if (data.ElementAt(i).Type.Equals("greaterThan120"))
        //            {
        //                ranges[4] = 1;
        //            }
        //            Debug.WriteLine(data.ElementAt(i).Type + " :: " + data.ElementAt(i).Total);
        //        }
        //        for (int j = 0; j < ranges.Length; j++)
        //        {
        //            if (ranges[j] == 0)
        //            {
        //                AgingCO agingCO = new AgingCO();
        //                agingCO.PaidAmount = 0;
        //                agingCO.PlanAmount = 0;
        //                agingCO.Total = 0;
        //                agingCO.Type = rangeValues[j];
        //                data.Add(agingCO);
        //            }
        //        }

        //        List<AgingCOThird> finalData = new List<AgingCOThird>();
        //        List<long> paidAmounts = new List<long>();
        //        List<long> patientAmounts = new List<long>();
        //        List<long> Total = new List<long>();
        //        long totalPaidAmount = 0, totalPatientAmount = 0;

        //        AgingCOThird temp = new AgingCOThird();

        //        List<AgingCOThird> finalObj = new List<AgingCOThird>();
        //        AgingCOThird tempPlan = new AgingCOThird();
        //        AgingCOThird tempPatient = new AgingCOThird();
        //        AgingCOThird tempTotal = new AgingCOThird();
        //        tempPlan.Name = "Plan";
        //        tempPatient.Name = "Patient";
        //        tempTotal.Name = "Total";
        //        data = data.OrderBy(lamb => lamb.Type).ToList();
        //        for (int k = 0; k < data.Count; k++)
        //        {
        //            switch (k)
        //            {
        //                case 0:
        //                    tempPlan.Range0_30 = data.ElementAt(k).PlanAmount;
        //                    tempPatient.Range0_30 = data.ElementAt(k).PaidAmount;
        //                    tempTotal.Range0_30 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
        //                    break;
        //                case 1:
        //                    tempPlan.Range31_60 = data.ElementAt(k).PlanAmount;
        //                    tempPatient.Range31_60 = data.ElementAt(k).PaidAmount;
        //                    tempTotal.Range31_60 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
        //                    break;
        //                case 2:
        //                    tempPlan.Range61_90 = data.ElementAt(k).PlanAmount;
        //                    tempPatient.Range61_90 = data.ElementAt(k).PaidAmount;
        //                    tempTotal.Range61_90 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
        //                    break;
        //                case 3:
        //                    tempPlan.Range91_120 = data.ElementAt(k).PlanAmount;
        //                    tempPatient.Range91_120 = data.ElementAt(k).PaidAmount;
        //                    tempTotal.Range91_120 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
        //                    break;
        //                case 4:
        //                    tempPlan.Range120plus = data.ElementAt(k).PlanAmount;
        //                    tempPatient.Range120plus = data.ElementAt(k).PaidAmount;
        //                    tempTotal.Range120plus = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }

        //        tempPlan.Total = tempPlan.Range0_30 + tempPlan.Range31_60 + tempPlan.Range61_90 + tempPlan.Range91_120 + tempPlan.Range120plus;
        //        tempPatient.Total = tempPatient.Range0_30 + tempPatient.Range31_60 + tempPatient.Range61_90 + tempPatient.Range91_120 + tempPatient.Range120plus;
        //        tempTotal.Total = tempTotal.Range0_30 + tempTotal.Range31_60 + tempTotal.Range61_90 + tempTotal.Range91_120 + tempTotal.Range120plus;


        //        finalObj.Add(tempPlan);
        //        finalObj.Add(tempPatient);
        //        finalObj.Add(tempTotal);
        //        //Total.Add(totalPaidAmount);
        //        //Total.Add(totalPatientAmount);
        //        //finalObj.PaidAmounts = paidAmounts;
        //        //finalObj.PatientAmounts = patientAmounts;
        //        //finalObj.Total = Total;
        //        return finalObj;
        //    }
        //    else if (cAgingCO.Value == "AD")
        //    {

        //        List<AgingCOThird> agingCOThird = new List<AgingCOThird>();
        //        List<AgingCO> data = new List<AgingCO>();
        //        using (SqlConnection myConnection = new SqlConnection(newConnection))
        //        {
        //            // --claimAge = (visittable.DateOfServiceFrom.HasValue ? GetClaimAge(visittable.DateOfServiceFrom.Value) : 0),
        //            string oString = "SELECT SUM(ISNULL(v.PrimaryBal, 0) + ISNULL(v.SecondaryBal, 0) + ISNULL(v.TertiaryBal, 0)) as planAmount  ,SUM(ISNULL(v.PrimaryPatientBal, 0) + ISNULL(v.SecondaryPatientBal, 0) + ISNULL(v.TertiaryPatientBal, 0)) as patientAmount  ,SUM(ISNULL(v.PrimaryBal, 0) + ISNULL(v.SecondaryBal, 0) + ISNULL(v.TertiaryBal, 0) +  ISNULL(v.PrimaryPatientBal, 0) + ISNULL(v.SecondaryPatientBal, 0) + ISNULL(v.TertiaryPatientBal, 0)) as total" +
        //                ",  case " +
        //                "	when ISNULL(DATEDIFF(day, v.AddedDate, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, v.AddedDate, GETDATE()), 0 )>= 0 then 1" +
        //                "    when ISNULL(DATEDIFF(day, v.AddedDate, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, v.AddedDate, GETDATE()), 0 )> 30 then 2" +
        //                "    when ISNULL(DATEDIFF(day, v.AddedDate, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, v.AddedDate, GETDATE()), 0 )> 60 then 3" +
        //                "    when ISNULL(DATEDIFF(day, v.AddedDate, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, v.AddedDate, GETDATE()), 0 )> 90 then 4" +
        //                "    when ISNULL(DATEDIFF(day, v.AddedDate, GETDATE()), 0 )> 120 then 5" +
        //                "    else 0" +
        //                "    end as claimAge  FROM Visit v join Patient pat on v.PatientID = pat.ID join Practice prac on pat.PracticeID = prac.ID  join[Location] loc on pat.LocationId = loc.ID join[Provider] prov on pat.ProviderID = prov.ID join PatientPlan pPlan on v.PrimaryPatientPlanID = pPlan.ID join InsurancePlan iPlan on pPlan.InsurancePlanID = iPlan.ID  where v.DateOfServiceFrom IS NOT NULL AND v.PracticeID = " + PracticeId + " GROUP BY case " +
        //                "    when ISNULL(DATEDIFF(day, v.AddedDate, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, v.AddedDate, GETDATE()), 0 )>= 0 then 1" +
        //                "    when ISNULL(DATEDIFF(day, v.AddedDate, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, v.AddedDate, GETDATE()), 0 )> 30 then 2" +
        //                "    when ISNULL(DATEDIFF(day, v.AddedDate, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, v.AddedDate, GETDATE()), 0 )> 60 then 3" +
        //                "    when ISNULL(DATEDIFF(day, v.AddedDate, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, v.AddedDate, GETDATE()), 0 )> 90 then 4" +
        //                "    when ISNULL(DATEDIFF(day, v.AddedDate, GETDATE()), 0 )> 120 then 5" +
        //                "    else 0    end";
        //            SqlCommand oCmd = new SqlCommand(oString, myConnection);
        //            myConnection.Open();
        //            using (SqlDataReader oReader = oCmd.ExecuteReader())
        //            {
        //                while (oReader.Read())
        //                {
        //                    var aging = new AgingCO();
        //                    if (Convert.ToInt32(oReader["claimAge"].ToString()) == 1)
        //                    {
        //                        aging.Type = "between0and30";
        //                    }
        //                    else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 2)
        //                    {
        //                        aging.Type = "between31and60";
        //                    }
        //                    else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 3)
        //                    {
        //                        aging.Type = "between61and90";
        //                    }
        //                    else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 4)
        //                    {
        //                        aging.Type = "between91and120";
        //                    }
        //                    else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 5)
        //                    {
        //                        aging.Type = "greaterThan120";
        //                    }
        //                    aging.Total = Convert.ToDecimal(oReader["total"].ToString());
        //                    aging.PlanAmount = Convert.ToDecimal(oReader["planAmount"].ToString());
        //                    aging.PaidAmount = Convert.ToDecimal(oReader["patientAmount"].ToString());
        //                    data.Add(aging);
        //                }
        //                myConnection.Close();
        //            }
        //        }
        //        //List<AgingCO> data = (from visittable in _context.Visit
        //        //                      join pat in _context.Patient on visittable.PatientID equals pat.ID
        //        //                      join fac in _context.Practice
        //        //                      on pat.PracticeID equals fac.ID
        //        //                      join loc in _context.Location
        //        //                      on pat.LocationId equals loc.ID
        //        //                      join prov in _context.Provider
        //        //                      on pat.ProviderID equals prov.ID
        //        //                      join pPlan in _context.PatientPlan
        //        //                      on visittable.PrimaryPatientPlanID equals pPlan.ID
        //        //                      join iPlan in _context.InsurancePlan
        //        //                      on pPlan.InsurancePlanID equals iPlan.ID
        //        //                      where (visittable.AddedDate.Date.IsNull() ? false : true) && (visittable.PracticeID == PracticeId)
        //        //                      group new
        //        //                      {
        //        //                          planAmount = (visittable.PrimaryBal.Val()) + (visittable.SecondaryPaid.Val()) + (visittable.TertiaryPaid.Val()),
        //        //                          patientAmount = (visittable.PrimaryPatientBal.Val()) + (visittable.SecondaryPatientBal.Val()) + (visittable.TertiaryPatientBal.Val()),
        //        //                          claimAge = (visittable.AddedDate.Date.IsNull() ? GetClaimAge(visittable.AddedDate.Date) : 0),
        //        //                          total = (
        //        //                      (visittable.PrimaryBal.Val()) + (visittable.SecondaryPaid.Val()) + (visittable.TertiaryPaid.Val()) +
        //        //                      (visittable.PrimaryPatientBal.Val()) + (visittable.SecondaryPatientBal.Val()) + (visittable.TertiaryPatientBal.Val())
        //        //                      )
        //        //                      } by new { ClaimAge = CheckClaimAge((visittable.AddedDate.Date.IsNull() ? GetClaimAge(visittable.AddedDate.Date) : 0)) } into gp

        //        //                      select new AgingCO
        //        //                      {
        //        //                          Type = gp.Key.ClaimAge == 1 ? "between0and30" : gp.Key.ClaimAge == 2 ? "between31and60" : gp.Key.ClaimAge == 3 ? "between61and90" : gp.Key.ClaimAge == 4 ? "between91and120" : "greaterThan120",
        //        //                          PlanAmount = gp.Select(a => a.planAmount).Sum(),
        //        //                          PaidAmount = gp.Select(a => a.patientAmount).Sum(),
        //        //                          Total = gp.Select(a => a.total).Sum(),

        //        //                      }).OrderBy(lamb => lamb.Type).ToList();


        //        Debug.WriteLine(data.Count);
        //        int[] ranges = new int[] { 0, 0, 0, 0, 0 };
        //        string[] rangeValues = new string[] { "between0and30", "between31and60", "between61and90", "between91and120", "greaterThan120" };

        //        for (int i = 0; i < data.Count; i++)
        //        {
        //            if (data.ElementAt(i).Type.Equals("between0and30"))
        //            {
        //                ranges[0] = 1;
        //            }
        //            else if (data.ElementAt(i).Type.Equals("between31and60"))
        //            {
        //                ranges[1] = 1;
        //            }
        //            else if (data.ElementAt(i).Type.Equals("between61and90"))
        //            {
        //                ranges[2] = 1;
        //            }
        //            else if (data.ElementAt(i).Type.Equals("between91and120"))
        //            {
        //                ranges[3] = 1;
        //            }
        //            else if (data.ElementAt(i).Type.Equals("greaterThan120"))
        //            {
        //                ranges[4] = 1;
        //            }
        //            Debug.WriteLine(data.ElementAt(i).Type + " :: " + data.ElementAt(i).Total);
        //        }
        //        for (int j = 0; j < ranges.Length; j++)
        //        {
        //            if (ranges[j] == 0)
        //            {
        //                AgingCO agingCO = new AgingCO();
        //                agingCO.PaidAmount = 0;
        //                agingCO.PlanAmount = 0;
        //                agingCO.Total = 0;
        //                agingCO.Type = rangeValues[j];
        //                data.Add(agingCO);
        //            }
        //        }

        //        List<AgingCOThird> finalData = new List<AgingCOThird>();
        //        List<long> paidAmounts = new List<long>();
        //        List<long> patientAmounts = new List<long>();
        //        List<long> Total = new List<long>();
        //        long totalPaidAmount = 0, totalPatientAmount = 0;

        //        AgingCOThird temp = new AgingCOThird();

        //        List<AgingCOThird> finalObj = new List<AgingCOThird>();
        //        AgingCOThird tempPlan = new AgingCOThird();
        //        AgingCOThird tempPatient = new AgingCOThird();
        //        AgingCOThird tempTotal = new AgingCOThird();
        //        tempPlan.Name = "Plan";
        //        tempPatient.Name = "Patient";
        //        tempTotal.Name = "Total";
        //        data = data.OrderBy(lamb => lamb.Type).ToList();
        //        for (int k = 0; k < data.Count; k++)
        //        {
        //            switch (k)
        //            {
        //                case 0:
        //                    tempPlan.Range0_30 = data.ElementAt(k).PlanAmount;
        //                    tempPatient.Range0_30 = data.ElementAt(k).PaidAmount;
        //                    tempTotal.Range0_30 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
        //                    break;
        //                case 1:
        //                    tempPlan.Range31_60 = data.ElementAt(k).PlanAmount;
        //                    tempPatient.Range31_60 = data.ElementAt(k).PaidAmount;
        //                    tempTotal.Range31_60 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
        //                    break;
        //                case 2:
        //                    tempPlan.Range61_90 = data.ElementAt(k).PlanAmount;
        //                    tempPatient.Range61_90 = data.ElementAt(k).PaidAmount;
        //                    tempTotal.Range61_90 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
        //                    break;
        //                case 3:
        //                    tempPlan.Range91_120 = data.ElementAt(k).PlanAmount;
        //                    tempPatient.Range91_120 = data.ElementAt(k).PaidAmount;
        //                    tempTotal.Range91_120 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
        //                    break;
        //                case 4:
        //                    tempPlan.Range120plus = data.ElementAt(k).PlanAmount;
        //                    tempPatient.Range120plus = data.ElementAt(k).PaidAmount;
        //                    tempTotal.Range120plus = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }

        //        tempPlan.Total = tempPlan.Range0_30 + tempPlan.Range31_60 + tempPlan.Range61_90 + tempPlan.Range91_120 + tempPlan.Range120plus;
        //        tempPatient.Total = tempPatient.Range0_30 + tempPatient.Range31_60 + tempPatient.Range61_90 + tempPatient.Range91_120 + tempPatient.Range120plus;
        //        tempTotal.Total = tempTotal.Range0_30 + tempTotal.Range31_60 + tempTotal.Range61_90 + tempTotal.Range91_120 + tempTotal.Range120plus;


        //        finalObj.Add(tempPlan);
        //        finalObj.Add(tempPatient);
        //        finalObj.Add(tempTotal);
        //        //Total.Add(totalPaidAmount);
        //        //Total.Add(totalPatientAmount);
        //        //finalObj.PaidAmounts = paidAmounts;
        //        //finalObj.PatientAmounts = patientAmounts;
        //        //finalObj.Total = Total;
        //        return finalObj;
        //    }
        //    else
        //    {
        //        List<AgingCOThird> agingCOThird = new List<AgingCOThird>();
        //        List<AgingCO> data = new List<AgingCO>();
        //        using (SqlConnection myConnection = new SqlConnection(newConnection))
        //        {
        //            // --claimAge = (visittable.DateOfServiceFrom.HasValue ? GetClaimAge(visittable.DateOfServiceFrom.Value) : 0),
        //            string oString = "SELECT SUM(ISNULL(v.PrimaryBal, 0) + ISNULL(v.SecondaryBal, 0) + ISNULL(v.TertiaryBal, 0)) as planAmount  ,SUM(ISNULL(v.PrimaryPatientBal, 0) + ISNULL(v.SecondaryPatientBal, 0) + ISNULL(v.TertiaryPatientBal, 0)) as patientAmount  ,SUM(ISNULL(v.PrimaryBal, 0) + ISNULL(v.SecondaryBal, 0) + ISNULL(v.TertiaryBal, 0) +  ISNULL(v.PrimaryPatientBal, 0) + ISNULL(v.SecondaryPatientBal, 0) + ISNULL(v.TertiaryPatientBal, 0)) as total" +
        //                ",  case " +
        //                "	when ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )>= 0 then 1" +
        //                "    when ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )> 30 then 2" +
        //                "    when ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )> 60 then 3" +
        //                "    when ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )> 90 then 4" +
        //                "    when ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )> 120 then 5" +
        //                "    else 0" +
        //                "    end as claimAge  FROM Visit v join Patient pat on v.PatientID = pat.ID join Practice prac on pat.PracticeID = prac.ID  join[Location] loc on pat.LocationId = loc.ID join[Provider] prov on pat.ProviderID = prov.ID join PatientPlan pPlan on v.PrimaryPatientPlanID = pPlan.ID join InsurancePlan iPlan on pPlan.InsurancePlanID = iPlan.ID  where v.DateOfServiceFrom IS NOT NULL AND v.PracticeID = " + PracticeId + " GROUP BY case " +
        //                "    when ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )>= 0 then 1" +
        //                "    when ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )> 30 then 2" +
        //                "    when ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )> 60 then 3" +
        //                "    when ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )> 90 then 4" +
        //                "    when ISNULL(DATEDIFF(day, v.SubmittedDate, GETDATE()), 0 )> 120 then 5" +
        //                "    else 0    end";
        //            SqlCommand oCmd = new SqlCommand(oString, myConnection);
        //            myConnection.Open();
        //            using (SqlDataReader oReader = oCmd.ExecuteReader())
        //            {
        //                while (oReader.Read())
        //                {
        //                    var aging = new AgingCO();
        //                    if (Convert.ToInt32(oReader["claimAge"].ToString()) == 1)
        //                    {
        //                        aging.Type = "between0and30";
        //                    }
        //                    else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 2)
        //                    {
        //                        aging.Type = "between31and60";
        //                    }
        //                    else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 3)
        //                    {
        //                        aging.Type = "between61and90";
        //                    }
        //                    else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 4)
        //                    {
        //                        aging.Type = "between91and120";
        //                    }
        //                    else if (Convert.ToInt32(oReader["claimAge"].ToString()) == 5)
        //                    {
        //                        aging.Type = "greaterThan120";
        //                    }
        //                    aging.Total = Convert.ToDecimal(oReader["total"].ToString());
        //                    aging.PlanAmount = Convert.ToDecimal(oReader["planAmount"].ToString());
        //                    aging.PaidAmount = Convert.ToDecimal(oReader["patientAmount"].ToString());
        //                    data.Add(aging);
        //                }
        //                myConnection.Close();
        //            }
        //        }

        //        //List<AgingCO> data = (from visittable in _context.Visit
        //        //                      join pat in _context.Patient on visittable.PatientID equals pat.ID
        //        //                      join fac in _context.Practice
        //        //                      on pat.PracticeID equals fac.ID
        //        //                      join loc in _context.Location
        //        //                      on pat.LocationId equals loc.ID
        //        //                      join prov in _context.Provider
        //        //                      on pat.ProviderID equals prov.ID
        //        //                      join pPlan in _context.PatientPlan
        //        //                      on visittable.PrimaryPatientPlanID equals pPlan.ID
        //        //                      join iPlan in _context.InsurancePlan
        //        //                      on pPlan.InsurancePlanID equals iPlan.ID
        //        //                      where (visittable.SubmittedDate.HasValue ? true : false) && (visittable.PracticeID == PracticeId)
        //        //                      group new
        //        //                      {
        //        //                          planAmount = (visittable.PrimaryBal.Val()) + (visittable.SecondaryPaid.Val()) + (visittable.TertiaryPaid.Val()),
        //        //                          patientAmount = (visittable.PrimaryPatientBal.Val()) + (visittable.SecondaryPatientBal.Val()) + (visittable.TertiaryPatientBal.Val()),
        //        //                          claimAge = (visittable.SubmittedDate.HasValue ? GetClaimAge(visittable.SubmittedDate.Value) : 0),
        //        //                          total = (
        //        //                      (visittable.PrimaryBal.Val()) + (visittable.SecondaryPaid.Val()) + (visittable.TertiaryPaid.Val()) +
        //        //                      (visittable.PrimaryPatientBal.Val()) + (visittable.SecondaryPatientBal.Val()) + (visittable.TertiaryPatientBal.Val())
        //        //                      )
        //        //                      } by new { ClaimAge = CheckClaimAge((visittable.SubmittedDate.HasValue ? GetClaimAge(visittable.SubmittedDate.Value) : 0)) } into gp

        //        //                      select new AgingCO
        //        //                      {
        //        //                          Type = gp.Key.ClaimAge == 1 ? "between0and30" : gp.Key.ClaimAge == 2 ? "between31and60" : gp.Key.ClaimAge == 3 ? "between61and90" : gp.Key.ClaimAge == 4 ? "between91and120" : "greaterThan120",
        //        //                          PlanAmount = gp.Select(a => a.planAmount).Sum(),
        //        //                          PaidAmount = gp.Select(a => a.patientAmount).Sum(),
        //        //                          Total = gp.Select(a => a.total).Sum(),

        //        //                      }).OrderBy(lamb => lamb.Type).ToList();


        //        Debug.WriteLine(data.Count);
        //        int[] ranges = new int[] { 0, 0, 0, 0, 0 };
        //        string[] rangeValues = new string[] { "between0and30", "between31and60", "between61and90", "between91and120", "greaterThan120" };

        //        for (int i = 0; i < data.Count; i++)
        //        {
        //            if (data.ElementAt(i).Type.Equals("between0and30"))
        //            {
        //                ranges[0] = 1;
        //            }
        //            else if (data.ElementAt(i).Type.Equals("between31and60"))
        //            {
        //                ranges[1] = 1;
        //            }
        //            else if (data.ElementAt(i).Type.Equals("between61and90"))
        //            {
        //                ranges[2] = 1;
        //            }
        //            else if (data.ElementAt(i).Type.Equals("between91and120"))
        //            {
        //                ranges[3] = 1;
        //            }
        //            else if (data.ElementAt(i).Type.Equals("greaterThan120"))
        //            {
        //                ranges[4] = 1;
        //            }
        //            Debug.WriteLine(data.ElementAt(i).Type + " :: " + data.ElementAt(i).Total);
        //        }
        //        for (int j = 0; j < ranges.Length; j++)
        //        {
        //            if (ranges[j] == 0)
        //            {
        //                AgingCO agingCO = new AgingCO();
        //                agingCO.PaidAmount = 0;
        //                agingCO.PlanAmount = 0;
        //                agingCO.Total = 0;
        //                agingCO.Type = rangeValues[j];
        //                data.Add(agingCO);
        //            }
        //        }

        //        List<AgingCOThird> finalData = new List<AgingCOThird>();
        //        List<long> paidAmounts = new List<long>();
        //        List<long> patientAmounts = new List<long>();
        //        List<long> Total = new List<long>();
        //        long totalPaidAmount = 0, totalPatientAmount = 0;

        //        AgingCOThird temp = new AgingCOThird();

        //        List<AgingCOThird> finalObj = new List<AgingCOThird>();
        //        AgingCOThird tempPlan = new AgingCOThird();
        //        AgingCOThird tempPatient = new AgingCOThird();
        //        AgingCOThird tempTotal = new AgingCOThird();
        //        tempPlan.Name = "Plan";
        //        tempPatient.Name = "Patient";
        //        tempTotal.Name = "Total";
        //        data = data.OrderBy(lamb => lamb.Type).ToList();
        //        for (int k = 0; k < data.Count; k++)
        //        {
        //            switch (k)
        //            {
        //                case 0:
        //                    tempPlan.Range0_30 = data.ElementAt(k).PlanAmount;
        //                    tempPatient.Range0_30 = data.ElementAt(k).PaidAmount;
        //                    tempTotal.Range0_30 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
        //                    break;
        //                case 1:
        //                    tempPlan.Range31_60 = data.ElementAt(k).PlanAmount;
        //                    tempPatient.Range31_60 = data.ElementAt(k).PaidAmount;
        //                    tempTotal.Range31_60 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
        //                    break;
        //                case 2:
        //                    tempPlan.Range61_90 = data.ElementAt(k).PlanAmount;
        //                    tempPatient.Range61_90 = data.ElementAt(k).PaidAmount;
        //                    tempTotal.Range61_90 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
        //                    break;
        //                case 3:
        //                    tempPlan.Range91_120 = data.ElementAt(k).PlanAmount;
        //                    tempPatient.Range91_120 = data.ElementAt(k).PaidAmount;
        //                    tempTotal.Range91_120 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
        //                    break;
        //                case 4:
        //                    tempPlan.Range120plus = data.ElementAt(k).PlanAmount;
        //                    tempPatient.Range120plus = data.ElementAt(k).PaidAmount;
        //                    tempTotal.Range120plus = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }

        //        tempPlan.Total = tempPlan.Range0_30 + tempPlan.Range31_60 + tempPlan.Range61_90 + tempPlan.Range91_120 + tempPlan.Range120plus;
        //        tempPatient.Total = tempPatient.Range0_30 + tempPatient.Range31_60 + tempPatient.Range61_90 + tempPatient.Range91_120 + tempPatient.Range120plus;
        //        tempTotal.Total = tempTotal.Range0_30 + tempTotal.Range31_60 + tempTotal.Range61_90 + tempTotal.Range91_120 + tempTotal.Range120plus;


        //        finalObj.Add(tempPlan);
        //        finalObj.Add(tempPatient);
        //        finalObj.Add(tempTotal);
        //        return finalObj;
        //    }



        //}



        [Route("FindAgingData")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<AgingCOThird>>> FindAgingData(CAgingCO cAgingCO)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
          User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
          User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
          );
            if (cAgingCO.Value == "DOS")
            {
                List<AgingCO> data = (from visittable in _context.Visit
                                      join pat in _context.Patient on visittable.PatientID equals pat.ID
                                      join fac in _context.Practice
                                      on pat.PracticeID equals fac.ID
                                      join loc in _context.Location
                                      on pat.LocationId equals loc.ID
                                      join prov in _context.Provider
                                      on pat.ProviderID equals prov.ID
                                      join pPlan in _context.PatientPlan
                                      on visittable.PrimaryPatientPlanID equals pPlan.ID
                                      join iPlan in _context.InsurancePlan
                                      on pPlan.InsurancePlanID equals iPlan.ID
                                      where (visittable.DateOfServiceFrom.HasValue ? true : false)
                                      && (visittable.PracticeID == UD.PracticeID)
                                      group new
                                      {
                                          planAmount = (visittable.PrimaryBal.Val()) + (visittable.SecondaryPaid.Val()) + (visittable.TertiaryPaid.Val()),
                                          patientAmount = (visittable.PrimaryPatientBal.Val()) + (visittable.SecondaryPatientBal.Val()) + (visittable.TertiaryPatientBal.Val()),
                                          claimAge = (visittable.SubmittedDate.HasValue ? GetClaimAge(visittable.SubmittedDate.Value) : 0),
                                          total = (
                                      (visittable.PrimaryBal.Val()) + (visittable.SecondaryPaid.Val()) + (visittable.TertiaryPaid.Val()) +
                                      (visittable.PrimaryPatientBal.Val()) + (visittable.SecondaryPatientBal.Val()) + (visittable.TertiaryPatientBal.Val())
                                      )
                                      } by new { ClaimAge = CheckClaimAge((visittable.DateOfServiceFrom.HasValue ? GetClaimAge(visittable.DateOfServiceFrom.Value) : 0)) } into gp

                                      select new AgingCO
                                      {
                                          Type = gp.Key.ClaimAge == 1 ? "between0and30" : gp.Key.ClaimAge == 2 ? "between31and60" : gp.Key.ClaimAge == 3 ? "between61and90" : gp.Key.ClaimAge == 4 ? "between91and120" : "greaterThan120",
                                          PlanAmount = gp.Select(a => a.planAmount).Sum(),
                                          PaidAmount = gp.Select(a => a.patientAmount).Sum(),
                                          Total = gp.Select(a => a.total).Sum(),

                                      }).OrderBy(lamb => lamb.Type).ToList();


                Debug.WriteLine(data.Count);
                int[] ranges = new int[] { 0, 0, 0, 0, 0 };
                string[] rangeValues = new string[] { "between0and30", "between31and60", "between61and90", "between91and120", "greaterThan120" };

                for (int i = 0; i < data.Count; i++)
                {
                    if (data.ElementAt(i).Type.Equals("between0and30"))
                    {
                        ranges[0] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("between31and60"))
                    {
                        ranges[1] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("between61and90"))
                    {
                        ranges[2] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("between91and120"))
                    {
                        ranges[3] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("greaterThan120"))
                    {
                        ranges[4] = 1;
                    }
                    Debug.WriteLine(data.ElementAt(i).Type + " :: " + data.ElementAt(i).Total);
                }
                for (int j = 0; j < ranges.Length; j++)
                {
                    if (ranges[j] == 0)
                    {
                        AgingCO agingCO = new AgingCO();
                        agingCO.PaidAmount = 0;
                        agingCO.PlanAmount = 0;
                        agingCO.Total = 0;
                        agingCO.Type = rangeValues[j];
                        data.Add(agingCO);
                    }
                }

                List<AgingCOThird> finalData = new List<AgingCOThird>();
                List<long> paidAmounts = new List<long>();
                List<long> patientAmounts = new List<long>();
                List<long> Total = new List<long>();
                long totalPaidAmount = 0, totalPatientAmount = 0;

                AgingCOThird temp = new AgingCOThird();

                List<AgingCOThird> finalObj = new List<AgingCOThird>();
                AgingCOThird tempPlan = new AgingCOThird();
                AgingCOThird tempPatient = new AgingCOThird();
                AgingCOThird tempTotal = new AgingCOThird();
                tempPlan.Name = "Plan";
                tempPatient.Name = "Patient";
                tempTotal.Name = "Total";
                data = data.OrderBy(lamb => lamb.Type).ToList();
                for (int k = 0; k < data.Count; k++)
                {
                    switch (k)
                    {
                        case 0:
                            tempPlan.Range0_30 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range0_30 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range0_30 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 1:
                            tempPlan.Range31_60 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range31_60 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range31_60 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 2:
                            tempPlan.Range61_90 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range61_90 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range61_90 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 3:
                            tempPlan.Range91_120 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range91_120 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range91_120 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 4:
                            tempPlan.Range120plus = data.ElementAt(k).PlanAmount;
                            tempPatient.Range120plus = data.ElementAt(k).PaidAmount;
                            tempTotal.Range120plus = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        default:
                            break;
                    }
                }

                tempPlan.Total = tempPlan.Range0_30 + tempPlan.Range31_60 + tempPlan.Range61_90 + tempPlan.Range91_120 + tempPlan.Range120plus;
                tempPatient.Total = tempPatient.Range0_30 + tempPatient.Range31_60 + tempPatient.Range61_90 + tempPatient.Range91_120 + tempPatient.Range120plus;
                tempTotal.Total = tempTotal.Range0_30 + tempTotal.Range31_60 + tempTotal.Range61_90 + tempTotal.Range91_120 + tempTotal.Range120plus;


                finalObj.Add(tempPlan);
                finalObj.Add(tempPatient);
                finalObj.Add(tempTotal);
                //Total.Add(totalPaidAmount);
                //Total.Add(totalPatientAmount);
                //finalObj.PaidAmounts = paidAmounts;
                //finalObj.PatientAmounts = patientAmounts;
                //finalObj.Total = Total;
                return finalObj;
            }
            if (cAgingCO.Value == "AD")
            {
                List<AgingCO> data = (from visittable in _context.Visit
                                      join pat in _context.Patient on visittable.PatientID equals pat.ID
                                      join fac in _context.Practice
                                      on pat.PracticeID equals fac.ID
                                      join loc in _context.Location
                                      on pat.LocationId equals loc.ID
                                      join prov in _context.Provider
                                      on pat.ProviderID equals prov.ID
                                      join pPlan in _context.PatientPlan
                                      on visittable.PrimaryPatientPlanID equals pPlan.ID
                                      join iPlan in _context.InsurancePlan
                                      on pPlan.InsurancePlanID equals iPlan.ID
                                      where (visittable.AddedDate.Date.IsNull() ? false : true) && (visittable.PracticeID == UD.PracticeID)
                                      group new
                                      {
                                          planAmount = (visittable.PrimaryBal.Val()) + (visittable.SecondaryPaid.Val()) + (visittable.TertiaryPaid.Val()),
                                          patientAmount = (visittable.PrimaryPatientBal.Val()) + (visittable.SecondaryPatientBal.Val()) + (visittable.TertiaryPatientBal.Val()),
                                          claimAge = (visittable.AddedDate.Date.IsNull() ? GetClaimAge(visittable.SubmittedDate.Value) : 0),
                                          total = (
                                      (visittable.PrimaryBal.Val()) + (visittable.SecondaryPaid.Val()) + (visittable.TertiaryPaid.Val()) +
                                      (visittable.PrimaryPatientBal.Val()) + (visittable.SecondaryPatientBal.Val()) + (visittable.TertiaryPatientBal.Val())
                                      )
                                      } by new { ClaimAge = CheckClaimAge((visittable.AddedDate.Date.IsNull() ? GetClaimAge(visittable.AddedDate) : 0)) } into gp

                                      select new AgingCO
                                      {
                                          Type = gp.Key.ClaimAge == 1 ? "between0and30" : gp.Key.ClaimAge == 2 ? "between31and60" : gp.Key.ClaimAge == 3 ? "between61and90" : gp.Key.ClaimAge == 4 ? "between91and120" : "greaterThan120",
                                          PlanAmount = gp.Select(a => a.planAmount).Sum(),
                                          PaidAmount = gp.Select(a => a.patientAmount).Sum(),
                                          Total = gp.Select(a => a.total).Sum(),

                                      }).OrderBy(lamb => lamb.Type).ToList();


                Debug.WriteLine(data.Count);
                int[] ranges = new int[] { 0, 0, 0, 0, 0 };
                string[] rangeValues = new string[] { "between0and30", "between31and60", "between61and90", "between91and120", "greaterThan120" };

                for (int i = 0; i < data.Count; i++)
                {
                    if (data.ElementAt(i).Type.Equals("between0and30"))
                    {
                        ranges[0] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("between31and60"))
                    {
                        ranges[1] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("between61and90"))
                    {
                        ranges[2] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("between91and120"))
                    {
                        ranges[3] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("greaterThan120"))
                    {
                        ranges[4] = 1;
                    }
                    Debug.WriteLine(data.ElementAt(i).Type + " :: " + data.ElementAt(i).Total);
                }
                for (int j = 0; j < ranges.Length; j++)
                {
                    if (ranges[j] == 0)
                    {
                        AgingCO agingCO = new AgingCO();
                        agingCO.PaidAmount = 0;
                        agingCO.PlanAmount = 0;
                        agingCO.Total = 0;
                        agingCO.Type = rangeValues[j];
                        data.Add(agingCO);
                    }
                }

                List<AgingCOThird> finalData = new List<AgingCOThird>();
                List<long> paidAmounts = new List<long>();
                List<long> patientAmounts = new List<long>();
                List<long> Total = new List<long>();
                long totalPaidAmount = 0, totalPatientAmount = 0;

                AgingCOThird temp = new AgingCOThird();

                List<AgingCOThird> finalObj = new List<AgingCOThird>();
                AgingCOThird tempPlan = new AgingCOThird();
                AgingCOThird tempPatient = new AgingCOThird();
                AgingCOThird tempTotal = new AgingCOThird();
                tempPlan.Name = "Plan";
                tempPatient.Name = "Patient";
                tempTotal.Name = "Total";
                data = data.OrderBy(lamb => lamb.Type).ToList();
                for (int k = 0; k < data.Count; k++)
                {
                    switch (k)
                    {
                        case 0:
                            tempPlan.Range0_30 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range0_30 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range0_30 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 1:
                            tempPlan.Range31_60 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range31_60 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range31_60 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 2:
                            tempPlan.Range61_90 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range61_90 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range61_90 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 3:
                            tempPlan.Range91_120 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range91_120 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range91_120 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 4:
                            tempPlan.Range120plus = data.ElementAt(k).PlanAmount;
                            tempPatient.Range120plus = data.ElementAt(k).PaidAmount;
                            tempTotal.Range120plus = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        default:
                            break;
                    }
                }

                tempPlan.Total = tempPlan.Range0_30 + tempPlan.Range31_60 + tempPlan.Range61_90 + tempPlan.Range91_120 + tempPlan.Range120plus;
                tempPatient.Total = tempPatient.Range0_30 + tempPatient.Range31_60 + tempPatient.Range61_90 + tempPatient.Range91_120 + tempPatient.Range120plus;
                tempTotal.Total = tempTotal.Range0_30 + tempTotal.Range31_60 + tempTotal.Range61_90 + tempTotal.Range91_120 + tempTotal.Range120plus;


                finalObj.Add(tempPlan);
                finalObj.Add(tempPatient);
                finalObj.Add(tempTotal);
                //Total.Add(totalPaidAmount);
                //Total.Add(totalPatientAmount);
                //finalObj.PaidAmounts = paidAmounts;
                //finalObj.PatientAmounts = patientAmounts;
                //finalObj.Total = Total;
                return finalObj;
            }
            else
            {
                List<AgingCO> data = (from visittable in _context.Visit
                                      join pat in _context.Patient on visittable.PatientID equals pat.ID
                                      join fac in _context.Practice
                                      on pat.PracticeID equals fac.ID
                                      join loc in _context.Location
                                      on pat.LocationId equals loc.ID
                                      join prov in _context.Provider
                                      on pat.ProviderID equals prov.ID
                                      join pPlan in _context.PatientPlan
                                      on visittable.PrimaryPatientPlanID equals pPlan.ID
                                      join iPlan in _context.InsurancePlan
                                      on pPlan.InsurancePlanID equals iPlan.ID
                                      where (visittable.SubmittedDate.HasValue ? true : false) && (visittable.PracticeID == UD.PracticeID)
                                      group new
                                      {
                                          planAmount = (visittable.PrimaryBal.Val()) + (visittable.SecondaryPaid.Val()) + (visittable.TertiaryPaid.Val()),
                                          patientAmount = (visittable.PrimaryPatientBal.Val()) + (visittable.SecondaryPatientBal.Val()) + (visittable.TertiaryPatientBal.Val()),
                                          claimAge = (visittable.SubmittedDate.HasValue ? GetClaimAge(visittable.SubmittedDate.Value) : 0),
                                          total = (
                                      (visittable.PrimaryBal.Val()) + (visittable.SecondaryPaid.Val()) + (visittable.TertiaryPaid.Val()) +
                                      (visittable.PrimaryPatientBal.Val()) + (visittable.SecondaryPatientBal.Val()) + (visittable.TertiaryPatientBal.Val())
                                      )
                                      } by new { ClaimAge = CheckClaimAge((visittable.SubmittedDate.HasValue ? GetClaimAge(visittable.SubmittedDate.Value) : 0)) } into gp

                                      select new AgingCO
                                      {
                                          Type = gp.Key.ClaimAge == 1 ? "between0and30" : gp.Key.ClaimAge == 2 ? "between31and60" : gp.Key.ClaimAge == 3 ? "between61and90" : gp.Key.ClaimAge == 4 ? "between91and120" : "greaterThan120",
                                          PlanAmount = gp.Select(a => a.planAmount).Sum(),
                                          PaidAmount = gp.Select(a => a.patientAmount).Sum(),
                                          Total = gp.Select(a => a.total).Sum(),

                                      }).OrderBy(lamb => lamb.Type).ToList();


                Debug.WriteLine(data.Count);
                int[] ranges = new int[] { 0, 0, 0, 0, 0 };
                string[] rangeValues = new string[] { "between0and30", "between31and60", "between61and90", "between91and120", "greaterThan120" };

                for (int i = 0; i < data.Count; i++)
                {
                    if (data.ElementAt(i).Type.Equals("between0and30"))
                    {
                        ranges[0] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("between31and60"))
                    {
                        ranges[1] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("between61and90"))
                    {
                        ranges[2] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("between91and120"))
                    {
                        ranges[3] = 1;
                    }
                    else if (data.ElementAt(i).Type.Equals("greaterThan120"))
                    {
                        ranges[4] = 1;
                    }
                    Debug.WriteLine(data.ElementAt(i).Type + " :: " + data.ElementAt(i).Total);
                }
                for (int j = 0; j < ranges.Length; j++)
                {
                    if (ranges[j] == 0)
                    {
                        AgingCO agingCO = new AgingCO();
                        agingCO.PaidAmount = 0;
                        agingCO.PlanAmount = 0;
                        agingCO.Total = 0;
                        agingCO.Type = rangeValues[j];
                        data.Add(agingCO);
                    }
                }

                List<AgingCOThird> finalData = new List<AgingCOThird>();
                List<long> paidAmounts = new List<long>();
                List<long> patientAmounts = new List<long>();
                List<long> Total = new List<long>();
                long totalPaidAmount = 0, totalPatientAmount = 0;

                AgingCOThird temp = new AgingCOThird();

                List<AgingCOThird> finalObj = new List<AgingCOThird>();
                AgingCOThird tempPlan = new AgingCOThird();
                AgingCOThird tempPatient = new AgingCOThird();
                AgingCOThird tempTotal = new AgingCOThird();
                tempPlan.Name = "Plan";
                tempPatient.Name = "Patient";
                tempTotal.Name = "Total";
                data = data.OrderBy(lamb => lamb.Type).ToList();
                for (int k = 0; k < data.Count; k++)
                {
                    switch (k)
                    {
                        case 0:
                            tempPlan.Range0_30 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range0_30 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range0_30 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 1:
                            tempPlan.Range31_60 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range31_60 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range31_60 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 2:
                            tempPlan.Range61_90 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range61_90 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range61_90 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 3:
                            tempPlan.Range91_120 = data.ElementAt(k).PlanAmount;
                            tempPatient.Range91_120 = data.ElementAt(k).PaidAmount;
                            tempTotal.Range91_120 = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        case 4:
                            tempPlan.Range120plus = data.ElementAt(k).PlanAmount;
                            tempPatient.Range120plus = data.ElementAt(k).PaidAmount;
                            tempTotal.Range120plus = data.ElementAt(k).PlanAmount + data.ElementAt(k).PaidAmount;
                            break;
                        default:
                            break;
                    }
                }

                tempPlan.Total = tempPlan.Range0_30 + tempPlan.Range31_60 + tempPlan.Range61_90 + tempPlan.Range91_120 + tempPlan.Range120plus;
                tempPatient.Total = tempPatient.Range0_30 + tempPatient.Range31_60 + tempPatient.Range61_90 + tempPatient.Range91_120 + tempPatient.Range120plus;
                tempTotal.Total = tempTotal.Range0_30 + tempTotal.Range31_60 + tempTotal.Range61_90 + tempTotal.Range91_120 + tempTotal.Range120plus;


                finalObj.Add(tempPlan);
                finalObj.Add(tempPatient);
                finalObj.Add(tempTotal);
                return finalObj;
            }

        }

        public int GetClaimAge(DateTime SubmittedDate)
        {
            int days = 0;
            //Debug.WriteLine(SubmittedDate);
            if (SubmittedDate != null)
            {
                days = System.DateTime.Now.Subtract(SubmittedDate).Days;
            }
            return days;
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
            return monthsInEnglish.ToList().IndexOf(monthNumber);
        }


        public string TranslateStatusCode(string StatusCode)
        {
            string status = "";
            switch (StatusCode)
            {
                case "8000":
                    status = "Scheduled";
                    break;
                case "8001":
                    status = "Confirmed";
                    break;
                case "8002":
                    status = "CheckIN";
                    break;
                case "8003":
                    status = "CheckOut";
                    break;
                case "8004":
                    status = "Re-Scheduled";
                    break;
                case "8005":
                    status = "No-Show";
                    break;
                case "8006":
                    status = "To Be Late";
                    break;
                case "8007":
                    status = "Message Left";
                    break;
                case "8008":
                    status = "Cancelled";
                    break;
                default:
                    status = "None";
                    break;
            }

            return status;
        }

        public int CheckClaimAge(int days)
        {
            days = Math.Abs(days);
            if (days >= 0 && days <= 30)
                return 1;
            else if (days >= 31 && days <= 60)
                return 2;
            else if (days >= 61 && days <= 90)
                return 3;
            else if (days >= 91 && days <= 120)
                return 4;
            else if (days > 120)
                return 5;
            else return 0;

        }


    }
}
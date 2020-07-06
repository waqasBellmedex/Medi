using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediFusionPM.Models;
using static MediFusionPM.ReportViewModels.RVMPatientVisit;
using static MediFusionPM.ViewModels.VMCommon;
using MediFusionPM.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using MediFusionPM.Controllers;
using static MediFusionPM.ReportViewModels.RVMDetailBillingReport;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace MediFusionPM.ReportControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RDetailedBillingReportController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;

        public RDetailedBillingReportController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
        }


        [Route("FindDetailedBillingReport")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<GRDetailBillingReport>>> FindDetailedBillingReport(CRDetailBillingReport cRDetailBillingReport)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

            return FindDetailedBillingReports(cRDetailBillingReport, UD);
        }
        private List<GRDetailBillingReport> FindDetailedBillingReports(CRDetailBillingReport cRDetailBillingReport, UserInfoData UD)
        {
            //List<GRDetailBillingReport> Data = (from chargetable in _context.Charge
            //                                    join visittable in _context.Visit on chargetable.VisitID equals visittable.ID
            //                                    join patienttable in _context.Patient on chargetable.PatientID equals patienttable.ID
            //                                    join providertable in _context.Provider on chargetable.ProviderID equals providertable.ID
            //                                    join cpttable in _context.Cpt on chargetable.CPTID equals cpttable.ID
            //                                    where
            //                                    (CheckNullLong(cRDetailBillingReport.ProviderID) ? true : chargetable.ProviderID.Equals(cRDetailBillingReport.ProviderID)) &&
            //                                    (ExtensionMethods.IsBetweenDOS(cRDetailBillingReport.DateOfServiceTo, cRDetailBillingReport.DateOfServiceFrom, chargetable.DateOfServiceTo, chargetable.DateOfServiceFrom)) &&
            //                                    (ExtensionMethods.IsBetweenDOS(cRDetailBillingReport.EntryDateTo, cRDetailBillingReport.EntryDateFrom, chargetable.AddedDate, chargetable.AddedDate)) &&
            //                                    (ExtensionMethods.IsBetweenDOS(cRDetailBillingReport.SubmittedDateTo, cRDetailBillingReport.SubmittedDateFrom, chargetable.SubmittetdDate, chargetable.SubmittetdDate)) &&
            //                                    (cRDetailBillingReport.PatientName.IsNull() ? true : (patienttable.FirstName.Trim() + " " + patienttable.LastName.Trim()).Contains(cRDetailBillingReport.PatientName)) &&
            //                                (cRDetailBillingReport.CPTCode.IsNull() ? true : cpttable.CPTCode.Equals(cRDetailBillingReport.CPTCode))
            //                                    group new
            //                                    {
            //                                        Charge = chargetable.TotalAmount,
            //                                        SumOfCollectedRevenue = (chargetable.PatientPaid.Val() + chargetable.PrimaryPaid.Val() + chargetable.SecondaryPaid.Val() + chargetable.TertiaryPaid.Val()),
            //                                        AverageRevenue = ((chargetable.PatientPaid.Val() + chargetable.PrimaryPaid.Val() + chargetable.SecondaryPaid.Val() + chargetable.TertiaryPaid.Val()) / 4),
            //                                        PrescribingMD = visittable.PrescribingMD,

            //                                    } by new { PrescribingMD = visittable.PrescribingMD } into gp

            //                                    select new GRDetailBillingReport
            //                                    {
            //                                        PrescribingMD = gp.Key.PrescribingMD,
            //                                        Charges = (long)gp.Select(a => a.Charge).Sum(),
            //                                        SumOfCollectedRevenue = (long)gp.Select(a => a.SumOfCollectedRevenue).Sum(),
            //                                        AverageRevenue = (long)gp.Select(a => a.AverageRevenue).Sum(),
            //                                    }).ToList();

            //Data = Data.OrderBy(lamb => lamb.PrescribingMD).ToList();
            //return Data;





            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            var Client = (from w in _contextMain.MainPractice
                          where w.ID == PracticeId
                          select w
                        ).FirstOrDefault();
            string contextName = _contextMain.MainClient.Find(Client.ClientID)?.ContextName;
            string newConnection = GetConnectionStringManager(contextName);
            List<GRDetailBillingReport> data = new List<GRDetailBillingReport>();
            using (SqlConnection myConnection = new SqlConnection(newConnection))
            {
                string oString = "SELECT SUM (ISNULL(chargetable.PatientPaid,0) + ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0) ) as CollectedRevenue, (SUM (ISNULL(chargetable.PatientPaid,0) + ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0) ) / 4) as AverageRevenue , sum(chargetable.TotalAmount) as Charges,v.PrescribingMD as PrescribingMD FROM Charge chargetable  join  Patient patienttable on chargetable.PatientID = patienttable.ID  join Visit v on chargetable.VisitID = v.ID  join Practice practicetable on chargetable.PracticeID = practicetable.ID  join [Provider] providertable on chargetable.ProviderID =  providertable.ID  join [Location] locationtable on chargetable.LocationID = locationtable.ID  join PatientPlan patientplantable on chargetable.PrimaryPatientPlanID = patientplantable.ID  join InsurancePlan insurangeplantable on patientplantable.InsurancePlanID = insurangeplantable.ID  join Cpt cpt on chargetable.CPTID = cpt.ID  left join  RefProvider refprovidertable on chargetable.RefProviderID = refprovidertable.ID  left join PatientPlan secpatientplantable on chargetable.SecondaryPatientPlanID = secpatientplantable.ID  left join PatientPlan terpatientplantable on chargetable.TertiaryPatientPlanID = terpatientplantable.ID  left join InsurancePlan secinsuranceplantable on secpatientplantable.InsurancePlanID = secinsuranceplantable.ID  left join InsurancePlan terinsuranceplantable on terpatientplantable.InsurancePlanID = terinsuranceplantable.ID   left join PatientPlan otherpatientplantable on chargetable.TertiaryPatientPlanID =otherpatientplantable.ID left join InsurancePlan otherinsurancetable on otherpatientplantable.InsurancePlanID =otherinsurancetable.ID  left join POS postable on chargetable.POSID = postable.ID left join Modifier m1t on chargetable.Modifier1ID = m1t.ID left join Modifier m2t on chargetable.Modifier2ID = m2t.ID left join ICD icd1t on v.ICD1ID = icd1t.ID left join ICD icd2t on v.ICD2ID = icd2t.ID left join ICD icd3t on v.ICD3ID = icd3t.ID left join ICD icd4t on v.ICD4ID = icd4t.ID  where ";

                oString += "  chargetable.practiceid = {0} ";
                oString = string.Format(oString, PracticeId);



                if (!cRDetailBillingReport.PatientName.IsNull())
                    oString += string.Format(" and (patienttable.LastName + ', ' + patienttable.FirstName) like '%{0}%'", cRDetailBillingReport.PatientName);

                if (!cRDetailBillingReport.ProviderID.IsNull())
                    oString += string.Format(" and chargetable.ProviderID ='{0}'", cRDetailBillingReport.ProviderID);
                if (!cRDetailBillingReport.CPTCode.IsNull())
                    oString += string.Format(" and cpt.CPTCode ='{0}'", cRDetailBillingReport.CPTCode);
                //if (!cRDetailBillingReport.PrescribingMD.IsNull())
                //    oString += string.Format(" and v.PrescribingMD ='{0}'", cRDetailBillingReport.PrescribingMD);

                //if (!cRDetailBillingReport.PaymentCriteria.IsNull())
                //{
                //    if (cRDetailBillingReport.PaymentCriteria == "Paid")
                //    {
                //        oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) > 0 ");
                //    }
                //    else if (cRDetailBillingReport.PaymentCriteria == "PartialPaid")
                //    {
                //        oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) > 0 ");
                //    }
                //    else if (cRDetailBillingReport.PaymentCriteria == "UnPaid")
                //    {
                //        oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) = 0 ");
                //    }
                //    else if (cRDetailBillingReport.PaymentCriteria == "PatientBal")
                //    {
                //        oString += string.Format(" and SUM(ISNULL(chargetable.PrimaryPatientBal,0) + ISNULL(chargetable.SecondaryPatientBal,0) + ISNULL(chargetable.TertiaryPatientBal,0)) > 0 ");
                //    }
                //}

                if (cRDetailBillingReport.DateOfServiceFrom != null && cRDetailBillingReport.DateOfServiceTo != null)
                {
                    oString += (" and (chargetable.AddedDate  >= '" + cRDetailBillingReport.DateOfServiceFrom.GetValueOrDefault().Date + "' and chargetable.AddedDate  < '" + cRDetailBillingReport.DateOfServiceTo.GetValueOrDefault().Date.AddDays(1) + "')");
                }
                else if (cRDetailBillingReport.DateOfServiceFrom != null && cRDetailBillingReport.DateOfServiceTo == null)
                {
                    oString += (" and ( chargetable.AddedDate  >= '" + cRDetailBillingReport.DateOfServiceFrom.GetValueOrDefault().Date + "')");

                }
                else if (cRDetailBillingReport.DateOfServiceFrom == null && cRDetailBillingReport.DateOfServiceTo != null)
                {
                    oString += (" and (chargetable.AddedDate  <= '" + cRDetailBillingReport.DateOfServiceTo.GetValueOrDefault().Date + "')");
                }


                if (cRDetailBillingReport.EntryDateFrom != null && cRDetailBillingReport.EntryDateTo != null)
                {
                    oString += (" and (chargetable.AddedDate  >= '" + cRDetailBillingReport.EntryDateFrom.GetValueOrDefault().Date + "' and chargetable.AddedDate  < '" + cRDetailBillingReport.EntryDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                }
                else if (cRDetailBillingReport.EntryDateFrom != null && cRDetailBillingReport.EntryDateTo == null)
                {
                    oString += (" and ( chargetable.AddedDate  >= '" + cRDetailBillingReport.EntryDateFrom.GetValueOrDefault().Date + "')");

                }
                else if (cRDetailBillingReport.EntryDateFrom == null && cRDetailBillingReport.EntryDateTo != null)
                {
                    oString += (" and (chargetable.AddedDate  <= '" + cRDetailBillingReport.EntryDateTo.GetValueOrDefault().Date + "')");
                }


                if (cRDetailBillingReport.SubmittedDateFrom != null && cRDetailBillingReport.SubmittedDateTo != null)
                {
                    oString += (" and (chargetable.SubmittetdDate  >= '" + cRDetailBillingReport.SubmittedDateFrom.GetValueOrDefault().Date + "' and chargetable.SubmittetdDate  < '" + cRDetailBillingReport.SubmittedDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                }
                else if (cRDetailBillingReport.SubmittedDateFrom != null && cRDetailBillingReport.SubmittedDateTo == null)
                {
                    oString += (" and ( chargetable.SubmittetdDate  >= '" + cRDetailBillingReport.SubmittedDateFrom.GetValueOrDefault().Date + "')");

                }
                else if (cRDetailBillingReport.SubmittedDateFrom == null && cRDetailBillingReport.SubmittedDateTo != null)
                {
                    oString += (" and (chargetable.SubmittetdDate  <= '" + cRDetailBillingReport.SubmittedDateTo.GetValueOrDefault().Date + "')");
                }

                oString += " GROUP BY  v.PrescribingMD  ORDER BY v.PrescribingMD ASC ";

                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        var pvReport = new GRDetailBillingReport();

                        pvReport.Charges = Convert.ToDecimal(oReader["Charges"].ToString());
                        pvReport.SumOfCollectedRevenue = Convert.ToDecimal(oReader["CollectedRevenue"].ToString());
                        pvReport.AverageRevenue = Convert.ToDecimal(oReader["AverageRevenue"].ToString());
                        pvReport.PrescribingMD = oReader["PrescribingMD"].ToString();

                        data.Add(pvReport);
                    }
                    myConnection.Close();
                }
            }

            return data;




        }
        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CRDetailBillingReport cRDetailBillingReport)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GRDetailBillingReport> data = FindDetailedBillingReports(cRDetailBillingReport, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, cRDetailBillingReport, "Detail BillingReport");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CRDetailBillingReport cRDetailBillingReport)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GRDetailBillingReport> data = FindDetailedBillingReports(cRDetailBillingReport, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);
        }




        public long DecimalToLong(decimal pf)
        {//
            return (long)pf;
        }
        public long DecimalToLong(decimal? pf)
        {
            return (long)pf.GetValueOrDefault();
        }
        private bool CheckNullLong(long? Value)
        {
            Debug.WriteLine(Value.IsNull());
            return Value.IsNull();
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
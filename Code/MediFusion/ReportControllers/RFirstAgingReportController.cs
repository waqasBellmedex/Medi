using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediFusionPM.Models;
using static MediFusionPM.ReportViewModels.RVMFirstAgingReport;
using static MediFusionPM.ViewModels.VMCommon;
using System.IdentityModel.Tokens.Jwt;
using MediFusionPM.ViewModels;
using MediFusionPM.Controllers;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MediFusionPM.ReportControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RFirstAgingReportController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;

        public RFirstAgingReportController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;

            // Only For Testing
            if (_context.Practice.Count() == 0)
            {
                //  _context.Facilities.Add(new Facility { Name = "ABC Facility", OrganizationName = "" });
                // _context.SaveChanges();
            }
        }


        //[Route("GetAgingReportV1")]
        //[HttpPost]
        //public async Task<ActionResult<IEnumerable<GRAgingReport>>> GetAgingReportV1(CRAgingReport cRAgingReport)
        //{
        //    UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
        //    User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
        //    User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
        //    // if (UD == null || UD.Rights == null || UD.Rights.SchedulerCreate == false)
        //    // return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
        //    return GetAgingReportV1(cRAgingReport, UD);
        //}

        //private List<GRAgingReport> GetAgingReportV1(CRAgingReport cRAgingReport, UserInfoData UD)
        //{
        //    List<AgingReportTemplate1> Data = (from visittable in _context.Visit
        //                                       join pat in _context.Patient on visittable.PatientID equals pat.ID
        //                                       join fac in _context.Practice
        //                                       on pat.PracticeID equals fac.ID
        //                                       join loc in _context.Location
        //                                       on pat.LocationId equals loc.ID
        //                                       join prov in _context.Provider
        //                                       on pat.ProviderID equals prov.ID
        //                                       join pPlan in _context.PatientPlan
        //                                       on visittable.PrimaryPatientPlanID equals pPlan.ID
        //                                       join iPlan in _context.InsurancePlan
        //                                       on pPlan.InsurancePlanID equals iPlan.ID
        //                                       join patientplantable in _context.PatientPlan on visittable.PrimaryPatientPlanID equals patientplantable.ID
        //                                       join insurangeplantable in _context.InsurancePlan on patientplantable.InsurancePlanID equals insurangeplantable.ID
        //                                       join edi837payertable in _context.Edi837Payer on insurangeplantable.Edi837PayerID equals edi837payertable.ID

        //                                       group new
        //                                       {
        //                                           chargeAmount = visittable.TotalAmount,
        //                                           claimAge = CheckClaimAge(visittable.SubmittedDate.HasValue ? GetClaimAge(visittable.SubmittedDate.Value) : 0),
        //                                           payerName = edi837payertable.PayerName,

        //                                       } by new { PayerName = edi837payertable.PayerName, ClaimAge = CheckClaimAge(visittable.SubmittedDate.HasValue ? GetClaimAge(visittable.SubmittedDate.Value) : 0) } into gp

        //                                       select new AgingReportTemplate1
        //                                       {
        //                                           PlanName = gp.Key.PayerName,
        //                                           charges = gp.Select(a => a.chargeAmount).ToList().ConvertAll<long>(new Converter<decimal?, long>(DecimalToLong)),
        //                                           claimAges = gp.Select(a => a.claimAge).ToList(),
        //                                           claimAge = gp.Key.ClaimAge
        //                                       }).ToList();

        //    Data = Data.OrderBy(lamb => lamb.PlanName).ToList();

        //    List<GRAgingReport> finalData = new List<GRAgingReport>();
        //    int currentObj = 0;

        //    GRAgingReport firstRow = new GRAgingReport();
        //    firstRow.PayerName = Data.ElementAt(0).PlanName;
        //    finalData.Add(firstRow);
        //    long TotalAmountFinal = 0;
        //    foreach (AgingReportTemplate1 Temp in Data)
        //    {

        //        if (Temp.PlanName.Equals(finalData.ElementAt(currentObj).PayerName))
        //        {
        //            List<int> ClaimAges = Temp.claimAges;
        //            List<long> Charges = Temp.charges;
        //            long TotalChargeToBeAdded = 0;
        //            switch (Temp.claimAge)
        //            {

        //                case 1:
        //                    TotalChargeToBeAdded = 0;
        //                    foreach (long Charge in Charges)
        //                    {
        //                        TotalChargeToBeAdded += Charge;
        //                    }
        //                    finalData.ElementAt(currentObj).Current =  TotalChargeToBeAdded;
        //                    TotalAmountFinal += TotalChargeToBeAdded;
        //                    finalData.ElementAt(currentObj).TotalBalance =  TotalAmountFinal;
        //                    break;
        //                case 2:
        //                    TotalChargeToBeAdded = 0;
        //                    foreach (long Charge in Charges)
        //                    {
        //                        TotalChargeToBeAdded += Charge;
        //                    }
        //                    finalData.ElementAt(currentObj).IsBetween30And60 = TotalChargeToBeAdded;
        //                    TotalAmountFinal += TotalChargeToBeAdded;
        //                    finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
        //                    break;
        //                case 3:
        //                    TotalChargeToBeAdded = 0;
        //                    foreach (long Charge in Charges)
        //                    {
        //                        TotalChargeToBeAdded += Charge;
        //                    }
        //                    finalData.ElementAt(currentObj).IsBetween61And90 = TotalChargeToBeAdded;
        //                    TotalAmountFinal += TotalChargeToBeAdded;
        //                    finalData.ElementAt(currentObj).TotalBalance =  TotalAmountFinal;

        //                    break;
        //                case 4:
        //                    TotalChargeToBeAdded = 0;
        //                    foreach (long Charge in Charges)
        //                    {
        //                        TotalChargeToBeAdded += Charge;
        //                    }
        //                    finalData.ElementAt(currentObj).IsBetween91And120 = TotalChargeToBeAdded;
        //                    TotalAmountFinal += TotalChargeToBeAdded;
        //                    finalData.ElementAt(currentObj).TotalBalance =  TotalAmountFinal;
        //                    break;
        //                case 5:
        //                    TotalChargeToBeAdded = 0;
        //                    foreach (long Charge in Charges)
        //                    {
        //                        TotalChargeToBeAdded += Charge;
        //                    }
        //                    finalData.ElementAt(currentObj).IsGreaterThan120 =  TotalChargeToBeAdded;
        //                    TotalAmountFinal += TotalChargeToBeAdded;
        //                    finalData.ElementAt(currentObj).TotalBalance =  TotalAmountFinal;
        //                    break;
        //            }
        //        }
        //        else
        //        {
        //            if (finalData.ElementAt(currentObj).Current == null)
        //            {
        //                finalData.ElementAt(currentObj).Current = 0;
        //            }
        //            if (finalData.ElementAt(currentObj).IsBetween30And60 == null)
        //            {
        //                finalData.ElementAt(currentObj).IsBetween30And60 = 0;
        //            }
        //            if (finalData.ElementAt(currentObj).IsBetween61And90 == null)
        //            {
        //                finalData.ElementAt(currentObj).IsBetween61And90 = 0;
        //            }
        //            if (finalData.ElementAt(currentObj).IsBetween91And120 == null)
        //            {
        //                finalData.ElementAt(currentObj).IsBetween91And120 = 0;
        //            }
        //            if (finalData.ElementAt(currentObj).IsGreaterThan120 == null)
        //            {
        //                finalData.ElementAt(currentObj).IsGreaterThan120 = 0;
        //            }
        //            TotalAmountFinal = 0;
        //            currentObj = currentObj + 1;
        //            GRAgingReport TempRow = new GRAgingReport();
        //            TempRow.PayerName = Temp.PlanName;

        //            List<int> ClaimAges = Temp.claimAges;
        //            List<long> Charges = Temp.charges;
        //            long TotalChargeToBeAdded = 0;
        //            switch (Temp.claimAge)
        //            {

        //                case 1:
        //                    TotalChargeToBeAdded = 0;
        //                    foreach (long Charge in Charges)
        //                    {
        //                        TotalChargeToBeAdded += Charge;
        //                    }
        //                    TempRow.Current =  TotalChargeToBeAdded;
        //                    TotalAmountFinal += TotalChargeToBeAdded;
        //                    TempRow.TotalBalance =  TotalAmountFinal;
        //                    break;
        //                case 2:
        //                    TotalChargeToBeAdded = 0;
        //                    foreach (long Charge in Charges)
        //                    {
        //                        TotalChargeToBeAdded += Charge;
        //                    }
        //                    TempRow.IsBetween30And60 = TotalChargeToBeAdded;
        //                    TotalAmountFinal += TotalChargeToBeAdded;
        //                    TempRow.TotalBalance =  TotalAmountFinal;
        //                    break;
        //                case 3:
        //                    TotalChargeToBeAdded = 0;
        //                    foreach (long Charge in Charges)
        //                    {
        //                        TotalChargeToBeAdded += Charge;
        //                    }
        //                    TempRow.IsBetween61And90 =  TotalChargeToBeAdded;
        //                    TotalAmountFinal += TotalChargeToBeAdded;
        //                    TempRow.TotalBalance =  TotalAmountFinal;

        //                    break;
        //                case 4:
        //                    TotalChargeToBeAdded = 0;
        //                    foreach (long Charge in Charges)
        //                    {
        //                        TotalChargeToBeAdded += Charge;
        //                    }
        //                    TempRow.IsBetween91And120 =  TotalChargeToBeAdded;
        //                    TotalAmountFinal += TotalChargeToBeAdded;
        //                    TempRow.TotalBalance =  TotalAmountFinal;
        //                    break;
        //                case 5:
        //                    TotalChargeToBeAdded = 0;
        //                    foreach (long Charge in Charges)
        //                    {
        //                        TotalChargeToBeAdded += Charge;
        //                    }
        //                    TempRow.IsGreaterThan120 =  TotalChargeToBeAdded;
        //                    TotalAmountFinal += TotalChargeToBeAdded;
        //                    TempRow.TotalBalance = TotalAmountFinal;
        //                    break;
        //            }
        //            finalData.Add(TempRow);
        //        }
        //    }
        //    return finalData;


        //}


        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> ExportExcelAging(CRAgingReport cRAgingReport)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GRAgingReport> data = FindAgingReportV1(cRAgingReport, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, cRAgingReport, "Aging Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdfAging(CRAgingReport cRAgingReport)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GRAgingReport> data = FindAgingReportV1(cRAgingReport, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);
        }


        [Route("FindAgingReportV1")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<GRAgingReport>>> FindAgingReportV1(CRAgingReport cRAgingReport)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
           );
            return FindAgingReportV1(cRAgingReport, UD);
        }

        private List<GRAgingReport> FindAgingReportV1(CRAgingReport CRAgingReport, UserInfoData UD)
        {

            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            var Client = (from w in _contextMain.MainPractice
                          where w.ID == PracticeId
                          select w
                        ).FirstOrDefault();
            string contextName = _contextMain.MainClient.Find(Client.ClientID)?.ContextName;
            string newConnection = GetConnectionStringManager(contextName);


            try
            {

                if (CRAgingReport.VisitValue == true && CRAgingReport.ChargeValue == false)
                {
                    if (CRAgingReport.AllValue == true)
                    {
                        List<AgingReportTemplate2> Data = (from chargetable in _context.Charge
                                                           join patienttable in _context.Patient on chargetable.PatientID equals patienttable.ID
                                                           join visittable in _context.Visit on chargetable.VisitID equals visittable.ID
                                                           join practicetable in _context.Practice on chargetable.PracticeID equals practicetable.ID
                                                           join providertable in _context.Provider on chargetable.ProviderID equals providertable.ID
                                                           join locationtable in _context.Location on chargetable.LocationID equals locationtable.ID
                                                           join patientplantable in _context.PatientPlan on chargetable.PrimaryPatientPlanID equals patientplantable.ID
                                                           join insurangeplantable in _context.InsurancePlan on patientplantable.InsurancePlanID equals insurangeplantable.ID
                                                           where
                                                            (CRAgingReport.PracticeName.IsNull() ? true : CRAgingReport.PracticeName.Equals(practicetable.Name)) &&
                                                            (CRAgingReport.ProviderName.IsNull() ? true : CRAgingReport.ProviderName.Equals(providertable.Name)) &&
                                                            (CRAgingReport.Location.IsNull() ? true : CRAgingReport.Location.Equals(locationtable.Name)) &&
                                                            (CRAgingReport.PayerName.IsNull() ? true : CRAgingReport.PayerName.Equals(insurangeplantable.PlanName)) &&
                                                            (ExtensionMethods.IsBetweenDOS(CRAgingReport.DateOfServiceTo, CRAgingReport.DateOfServiceFrom, visittable.DateOfServiceTo, visittable.DateOfServiceFrom)) &&
                                                            (ExtensionMethods.IsBetweenDOS(CRAgingReport.EntryDateTo, CRAgingReport.EntryDateFrom, visittable.AddedDate, visittable.AddedDate)) &&
                                                            (ExtensionMethods.IsBetweenDOS(CRAgingReport.SubmittedDateTo, CRAgingReport.SubmittedDateFrom, visittable.SubmittedDate, visittable.SubmittedDate)) &&
                                                            (CRAgingReport.PatientName.IsNull() ? true : (patienttable.FirstName.Trim() + " " + patienttable.LastName.Trim()).Equals(CRAgingReport.PatientName)) &&
                                                            (CRAgingReport.PatientAccountNumber.IsNull() ? true : patienttable.AccountNum.Equals(CRAgingReport.PatientAccountNumber))
                                                            && practicetable.ID == UD.PracticeID
                                                            && ((chargetable.PrimaryBal.Val()) + (chargetable.SecondaryBal.Val()) + (chargetable.TertiaryBal.Val()) > 0)
                                                           group new
                                                           {
                                                               chargeAmount = visittable.TotalAmount,
                                                               claimAge = CheckClaimAge(visittable.SubmittedDate.HasValue ? GetClaimAge(visittable.SubmittedDate.Value) : 0),
                                                               payerName = insurangeplantable.PlanName,

                                                           } by new { /*PlanName = insurangeplantable.PlanName,*/ ClaimAge = CheckClaimAge(visittable.SubmittedDate.HasValue ? GetClaimAge(visittable.SubmittedDate.Value) : 0) } into gp

                                                           select new AgingReportTemplate2
                                                           {
                                                               PlanName = "All Plans",

                                                               charges = (long)gp.Select(a => a.chargeAmount).Count(),
                                                               claimAges = gp.Select(a => a.claimAge).ToList(),
                                                               claimAge = gp.Key.ClaimAge
                                                           }).ToList();




                        Data = Data.OrderBy(lamb => lamb.PlanName).ToList();
                        if (Data.Count <= 0)
                        {
                            BadRequest("Data Not Found");
                        }
                        List<GRAgingReport> finalData = new List<GRAgingReport>();
                        int currentObj = 0;

                        GRAgingReport firstRow = new GRAgingReport();
                        firstRow.PayerName = Data.ElementAt(0).PlanName;
                        finalData.Add(firstRow);
                        decimal TotalAmountFinal = 0;
                        foreach (AgingReportTemplate2 Temp in Data)
                        {

                            if (Temp.PlanName.Equals(finalData.ElementAt(currentObj).PayerName))
                            {
                                List<int> ClaimAges = Temp.claimAges;
                                decimal Charges = Temp.charges;
                                decimal TotalChargeToBeAdded = 0;
                                switch (Temp.claimAge)
                                {

                                    case 1:
                                        TotalChargeToBeAdded = 0;
                                        //foreach (long Charge in Charges)
                                        //{
                                        TotalChargeToBeAdded += Charges;
                                        // }
                                        finalData.ElementAt(currentObj).Current = TotalChargeToBeAdded;
                                        TotalAmountFinal += TotalChargeToBeAdded;
                                        finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                        break;
                                    case 2:
                                        TotalChargeToBeAdded = 0;
                                        //  foreach (long Charge in Charges)
                                        // {
                                        TotalChargeToBeAdded += Charges;
                                        //  }
                                        finalData.ElementAt(currentObj).IsBetween30And60 = TotalChargeToBeAdded;
                                        TotalAmountFinal += TotalChargeToBeAdded;
                                        finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                        break;
                                    case 3:
                                        TotalChargeToBeAdded = 0;
                                        //  foreach (long Charge in Charges)
                                        //  {
                                        TotalChargeToBeAdded += Charges;
                                        // }
                                        finalData.ElementAt(currentObj).IsBetween61And90 = TotalChargeToBeAdded;
                                        TotalAmountFinal += TotalChargeToBeAdded;
                                        finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;

                                        break;
                                    case 4:
                                        TotalChargeToBeAdded = 0;
                                        //  foreach (long Charge in Charges)
                                        //  {
                                        TotalChargeToBeAdded += Charges;
                                        //  }
                                        finalData.ElementAt(currentObj).IsBetween91And120 = TotalChargeToBeAdded;
                                        TotalAmountFinal += TotalChargeToBeAdded;
                                        finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                        break;
                                    case 5:
                                        TotalChargeToBeAdded = 0;
                                        //  foreach (long Charge in Charges)
                                        // {
                                        TotalChargeToBeAdded += Charges;
                                        // }
                                        finalData.ElementAt(currentObj).IsGreaterThan120 = TotalChargeToBeAdded;
                                        TotalAmountFinal += TotalChargeToBeAdded;
                                        finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;

                                        break;
                                }
                            }
                            else
                            {
                                if (finalData.ElementAt(currentObj).Current == null)
                                {
                                    finalData.ElementAt(currentObj).Current = 0;
                                }
                                if (finalData.ElementAt(currentObj).IsBetween30And60 == null)
                                {
                                    finalData.ElementAt(currentObj).IsBetween30And60 = 0;
                                }
                                if (finalData.ElementAt(currentObj).IsBetween61And90 == null)
                                {
                                    finalData.ElementAt(currentObj).IsBetween61And90 = 0;
                                }
                                if (finalData.ElementAt(currentObj).IsBetween91And120 == null)
                                {
                                    finalData.ElementAt(currentObj).IsBetween91And120 = 0;
                                }
                                if (finalData.ElementAt(currentObj).IsGreaterThan120 == null)
                                {
                                    finalData.ElementAt(currentObj).IsGreaterThan120 = 0;
                                }
                                TotalAmountFinal = 0;
                                currentObj = currentObj + 1;
                                GRAgingReport TempRow = new GRAgingReport();
                                TempRow.PayerName = Temp.PlanName;

                                List<int> ClaimAges = Temp.claimAges;
                                decimal Charges = Temp.charges;
                                decimal TotalChargeToBeAdded = 0;
                                switch (Temp.claimAge)
                                {

                                    case 1:
                                        TotalChargeToBeAdded = 0;
                                        // foreach (long Charge in Charges)
                                        //  {
                                        TotalChargeToBeAdded += Charges;
                                        //  }
                                        TempRow.Current = TotalChargeToBeAdded;
                                        TotalAmountFinal += TotalChargeToBeAdded;
                                        TempRow.TotalBalance = TotalAmountFinal;
                                        break;
                                    case 2:
                                        TotalChargeToBeAdded = 0;
                                        //   foreach (long Charge in Charges)
                                        // {
                                        TotalChargeToBeAdded += Charges;
                                        // }
                                        TempRow.IsBetween30And60 = TotalChargeToBeAdded;
                                        TotalAmountFinal += TotalChargeToBeAdded;
                                        TempRow.TotalBalance = TotalAmountFinal;
                                        break;
                                    case 3:
                                        TotalChargeToBeAdded = 0;
                                        // foreach (long Charge in Charges)
                                        // {
                                        TotalChargeToBeAdded += Charges;
                                        //  }
                                        TempRow.IsBetween61And90 = TotalChargeToBeAdded;
                                        TotalAmountFinal += TotalChargeToBeAdded;
                                        TempRow.TotalBalance = TotalAmountFinal;

                                        break;
                                    case 4:
                                        TotalChargeToBeAdded = 0;
                                        //  foreach (long Charge in Charges)
                                        //   {
                                        TotalChargeToBeAdded += Charges;
                                        //   }
                                        TempRow.IsBetween91And120 = TotalChargeToBeAdded;
                                        TotalAmountFinal += TotalChargeToBeAdded;
                                        TempRow.TotalBalance = TotalAmountFinal;
                                        break;
                                    case 5:
                                        TotalChargeToBeAdded = 0;
                                        //   foreach (long Charge in Charges)
                                        //   {
                                        TotalChargeToBeAdded += Charges;
                                        //  }
                                        TempRow.IsGreaterThan120 = TotalChargeToBeAdded;
                                        TotalAmountFinal += TotalChargeToBeAdded;
                                        TempRow.TotalBalance = TotalAmountFinal;
                                        break;
                                }
                                finalData.Add(TempRow);
                            }
                        }

                        if (finalData.ElementAt(finalData.Count - 1).Current == null)
                        {
                            finalData.ElementAt(finalData.Count - 1).Current = 0;
                        }
                        if (finalData.ElementAt(finalData.Count - 1).IsBetween30And60 == null)
                        {
                            finalData.ElementAt(finalData.Count - 1).IsBetween30And60 = 0;
                        }
                        if (finalData.ElementAt(finalData.Count - 1).IsBetween61And90 == null)
                        {
                            finalData.ElementAt(finalData.Count - 1).IsBetween61And90 = 0;
                        }
                        if (finalData.ElementAt(finalData.Count - 1).IsBetween91And120 == null)
                        {
                            finalData.ElementAt(finalData.Count - 1).IsBetween91And120 = 0;
                        }
                        if (finalData.ElementAt(finalData.Count - 1).IsGreaterThan120 == null)
                        {
                            finalData.ElementAt(finalData.Count - 1).IsGreaterThan120 = 0;
                        }

                        return finalData;
                    }
                    else
                    {
                        if (CRAgingReport.SearchType == "ED")
                        {
                            List<AgingReportTemplate2> Data = (from chargetable in _context.Charge
                                                               join patienttable in _context.Patient on chargetable.PatientID equals patienttable.ID
                                                               join visittable in _context.Visit on chargetable.VisitID equals visittable.ID
                                                               join practicetable in _context.Practice on chargetable.PracticeID equals practicetable.ID
                                                               join providertable in _context.Provider on chargetable.ProviderID equals providertable.ID
                                                               join locationtable in _context.Location on chargetable.LocationID equals locationtable.ID
                                                               join patientplantable in _context.PatientPlan on chargetable.PrimaryPatientPlanID equals patientplantable.ID
                                                               join insurangeplantable in _context.InsurancePlan on patientplantable.InsurancePlanID equals insurangeplantable.ID

                                                               where
                                                                        (CRAgingReport.PracticeName.IsNull() ? true : CRAgingReport.PracticeName.Equals(practicetable.Name)) &&
                                                                        (CRAgingReport.ProviderName.IsNull() ? true : CRAgingReport.ProviderName.Equals(providertable.Name)) &&
                                                                        (CRAgingReport.Location.IsNull() ? true : CRAgingReport.Location.Equals(locationtable.Name)) &&
                                                                        (CRAgingReport.PayerName.IsNull() ? true : CRAgingReport.PayerName.Equals(insurangeplantable.PlanName)) &&
                                                                        (ExtensionMethods.IsBetweenDOS(CRAgingReport.DateOfServiceTo, CRAgingReport.DateOfServiceFrom, visittable.DateOfServiceTo, visittable.DateOfServiceFrom)) &&
                                                                        (ExtensionMethods.IsBetweenDOS(CRAgingReport.EntryDateTo, CRAgingReport.EntryDateFrom, visittable.AddedDate, visittable.AddedDate)) &&
                                                                        (ExtensionMethods.IsBetweenDOS(CRAgingReport.SubmittedDateTo, CRAgingReport.SubmittedDateFrom, visittable.SubmittedDate, visittable.SubmittedDate)) &&
                                                                        (CRAgingReport.PatientName.IsNull() ? true : (patienttable.FirstName.Trim() + " " + patienttable.LastName.Trim()).Equals(CRAgingReport.PatientName)) &&
                                                                        (CRAgingReport.PatientAccountNumber.IsNull() ? true : patienttable.AccountNum.Equals(CRAgingReport.PatientAccountNumber))
                                                                        && practicetable.ID == UD.PracticeID
                                                                        && ((chargetable.PrimaryBal.Val()) + (chargetable.SecondaryBal.Val()) + (chargetable.TertiaryBal.Val()) > 0)
                                                               group new
                                                               {
                                                                   //chargeAmount = chargetable.TotalAmount,
                                                                   chargeAmount = (chargetable.PrimaryBal.Val()) + (chargetable.SecondaryBal.Val()) + (chargetable.TertiaryBal.Val()),
                                                                   claimAge = CheckClaimAge(visittable.AddedDate.Date.IsNull() ? GetClaimAge(visittable.AddedDate.Date) : 0),
                                                                   payerName = insurangeplantable.PlanName,

                                                               } by new { PlanName = insurangeplantable.PlanName, ClaimAge = CheckClaimAge(visittable.AddedDate.Date.IsNull() ? GetClaimAge(visittable.AddedDate.Date) : 0) } into gp

                                                               select new AgingReportTemplate2
                                                               {
                                                                   PlanName = gp.Key.PlanName,
                                                                   charges = gp.Select(a => a.chargeAmount).Count()/*.ConvertAll<long>(new Converter<decimal, long>(DecimalToLong))*/,
                                                                   claimAges = gp.Select(a => a.claimAge).ToList(),
                                                                   claimAge = gp.Key.ClaimAge
                                                               }).ToList();




                            Data = Data.OrderBy(lamb => lamb.PlanName).ToList();
                            if (Data.Count <= 0)
                            {
                                BadRequest("Data Not Found");
                            }
                            List<GRAgingReport> finalData = new List<GRAgingReport>();
                            int currentObj = 0;

                            GRAgingReport firstRow = new GRAgingReport();
                            firstRow.PayerName = Data.ElementAt(0).PlanName;
                            finalData.Add(firstRow);
                            decimal TotalAmountFinal = 0;
                            foreach (AgingReportTemplate2 Temp in Data)
                            {

                                if (Temp.PlanName.Equals(finalData.ElementAt(currentObj).PayerName))
                                {
                                    List<int> ClaimAges = Temp.claimAges;
                                    decimal Charges = Temp.charges;
                                    decimal TotalChargeToBeAdded = 0;
                                    switch (Temp.claimAge)
                                    {

                                        case 1:
                                            TotalChargeToBeAdded = 0;
                                            //foreach (long Charge in Charges)
                                            //{
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).Current = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 2:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            finalData.ElementAt(currentObj).IsBetween30And60 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 3:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).IsBetween61And90 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;

                                            break;
                                        case 4:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            finalData.ElementAt(currentObj).IsBetween91And120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 5:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).IsGreaterThan120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;

                                            break;
                                    }
                                }
                                else
                                {
                                    if (finalData.ElementAt(currentObj).Current == null)
                                    {
                                        finalData.ElementAt(currentObj).Current = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween30And60 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween30And60 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween61And90 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween61And90 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween91And120 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween91And120 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsGreaterThan120 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsGreaterThan120 = 0;
                                    }
                                    TotalAmountFinal = 0;
                                    currentObj = currentObj + 1;
                                    GRAgingReport TempRow = new GRAgingReport();
                                    TempRow.PayerName = Temp.PlanName;

                                    List<int> ClaimAges = Temp.claimAges;
                                    decimal Charges = Temp.charges;
                                    decimal TotalChargeToBeAdded = 0;
                                    switch (Temp.claimAge)
                                    {

                                        case 1:
                                            TotalChargeToBeAdded = 0;
                                            // foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            TempRow.Current = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 2:
                                            TotalChargeToBeAdded = 0;
                                            //   foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            TempRow.IsBetween30And60 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 3:
                                            TotalChargeToBeAdded = 0;
                                            // foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            TempRow.IsBetween61And90 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;

                                            break;
                                        case 4:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            //   {
                                            TotalChargeToBeAdded += Charges;
                                            //   }
                                            TempRow.IsBetween91And120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 5:
                                            TotalChargeToBeAdded = 0;
                                            //   foreach (long Charge in Charges)
                                            //   {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            TempRow.IsGreaterThan120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                    }
                                    finalData.Add(TempRow);
                                }
                            }

                            if (finalData.ElementAt(finalData.Count - 1).Current == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).Current = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween30And60 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween30And60 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween61And90 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween61And90 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween91And120 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween91And120 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsGreaterThan120 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsGreaterThan120 = 0;
                            }

                            return finalData;
                        }
                        else if (CRAgingReport.SearchType == "SD")
                        {
                            List<AgingReportTemplate2> Data = (from chargetable in _context.Charge
                                                               join patienttable in _context.Patient on chargetable.PatientID equals patienttable.ID
                                                               join visittable in _context.Visit on chargetable.VisitID equals visittable.ID
                                                               join practicetable in _context.Practice on chargetable.PracticeID equals practicetable.ID
                                                               join providertable in _context.Provider on chargetable.ProviderID equals providertable.ID
                                                               join locationtable in _context.Location on chargetable.LocationID equals locationtable.ID
                                                               join patientplantable in _context.PatientPlan on chargetable.PrimaryPatientPlanID equals patientplantable.ID
                                                               join insurangeplantable in _context.InsurancePlan on patientplantable.InsurancePlanID equals insurangeplantable.ID

                                                               where
                                                                        (CRAgingReport.PracticeName.IsNull() ? true : CRAgingReport.PracticeName.Equals(practicetable.Name)) &&
                                                                        (CRAgingReport.ProviderName.IsNull() ? true : CRAgingReport.ProviderName.Equals(providertable.Name)) &&
                                                                        (CRAgingReport.Location.IsNull() ? true : CRAgingReport.Location.Equals(locationtable.Name)) &&
                                                                        (CRAgingReport.PayerName.IsNull() ? true : CRAgingReport.PayerName.Equals(insurangeplantable.PlanName)) &&
                                                                        (ExtensionMethods.IsBetweenDOS(CRAgingReport.DateOfServiceTo, CRAgingReport.DateOfServiceFrom, visittable.DateOfServiceTo, visittable.DateOfServiceFrom)) &&
                                                                        (ExtensionMethods.IsBetweenDOS(CRAgingReport.EntryDateTo, CRAgingReport.EntryDateFrom, visittable.AddedDate, visittable.AddedDate)) &&
                                                                        (ExtensionMethods.IsBetweenDOS(CRAgingReport.SubmittedDateTo, CRAgingReport.SubmittedDateFrom, visittable.SubmittedDate, visittable.SubmittedDate)) &&
                                                                        (CRAgingReport.PatientName.IsNull() ? true : (patienttable.FirstName.Trim() + " " + patienttable.LastName.Trim()).Equals(CRAgingReport.PatientName)) &&
                                                                        (CRAgingReport.PatientAccountNumber.IsNull() ? true : patienttable.AccountNum.Equals(CRAgingReport.PatientAccountNumber))
                                                                                                           && practicetable.ID == UD.PracticeID
                                                                                                           && ((chargetable.PrimaryBal.Val()) + (chargetable.SecondaryBal.Val()) + (chargetable.TertiaryBal.Val()) > 0)
                                                               group new
                                                               {
                                                                   //chargeAmount = chargetable.TotalAmount,
                                                                   chargeAmount = (chargetable.PrimaryBal.Val()) + (chargetable.SecondaryBal.Val()) + (chargetable.TertiaryBal.Val()),
                                                                   claimAge = CheckClaimAge(visittable.SubmittedDate.HasValue ? GetClaimAge(visittable.SubmittedDate.Value) : 0),
                                                                   payerName = insurangeplantable.PlanName,

                                                               } by new { PlanName = insurangeplantable.PlanName, ClaimAge = CheckClaimAge(visittable.SubmittedDate.HasValue ? GetClaimAge(visittable.SubmittedDate.Value) : 0) } into gp

                                                               select new AgingReportTemplate2
                                                               {
                                                                   PlanName = gp.Key.PlanName,
                                                                   charges = gp.Select(a => a.chargeAmount).Count()/*.ConvertAll<long>(new Converter<decimal, long>(DecimalToLong))*/,
                                                                   claimAges = gp.Select(a => a.claimAge).ToList(),
                                                                   claimAge = gp.Key.ClaimAge
                                                               }).ToList();


                            Data = Data.OrderBy(lamb => lamb.PlanName).ToList();
                            if (Data.Count <= 0)
                            {
                                BadRequest("Data Not Found");
                            }
                            List<GRAgingReport> finalData = new List<GRAgingReport>();
                            int currentObj = 0;

                            GRAgingReport firstRow = new GRAgingReport();
                            firstRow.PayerName = Data.ElementAt(0).PlanName;
                            finalData.Add(firstRow);
                            decimal TotalAmountFinal = 0;
                            foreach (AgingReportTemplate2 Temp in Data)
                            {

                                if (Temp.PlanName.Equals(finalData.ElementAt(currentObj).PayerName))
                                {
                                    List<int> ClaimAges = Temp.claimAges;
                                    decimal Charges = Temp.charges;
                                    decimal TotalChargeToBeAdded = 0;
                                    switch (Temp.claimAge)
                                    {

                                        case 1:
                                            TotalChargeToBeAdded = 0;
                                            //foreach (long Charge in Charges)
                                            //{
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).Current = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 2:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            finalData.ElementAt(currentObj).IsBetween30And60 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 3:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).IsBetween61And90 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;

                                            break;
                                        case 4:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            finalData.ElementAt(currentObj).IsBetween91And120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 5:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).IsGreaterThan120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;

                                            break;
                                    }
                                }
                                else
                                {
                                    if (finalData.ElementAt(currentObj).Current == null)
                                    {
                                        finalData.ElementAt(currentObj).Current = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween30And60 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween30And60 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween61And90 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween61And90 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween91And120 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween91And120 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsGreaterThan120 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsGreaterThan120 = 0;
                                    }
                                    TotalAmountFinal = 0;
                                    currentObj = currentObj + 1;
                                    GRAgingReport TempRow = new GRAgingReport();
                                    TempRow.PayerName = Temp.PlanName;

                                    List<int> ClaimAges = Temp.claimAges;
                                    decimal Charges = Temp.charges;
                                    decimal TotalChargeToBeAdded = 0;
                                    switch (Temp.claimAge)
                                    {

                                        case 1:
                                            TotalChargeToBeAdded = 0;
                                            // foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            TempRow.Current = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 2:
                                            TotalChargeToBeAdded = 0;
                                            //   foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            TempRow.IsBetween30And60 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 3:
                                            TotalChargeToBeAdded = 0;
                                            // foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            TempRow.IsBetween61And90 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;

                                            break;
                                        case 4:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            //   {
                                            TotalChargeToBeAdded += Charges;
                                            //   }
                                            TempRow.IsBetween91And120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 5:
                                            TotalChargeToBeAdded = 0;
                                            //   foreach (long Charge in Charges)
                                            //   {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            TempRow.IsGreaterThan120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                    }
                                    finalData.Add(TempRow);
                                }
                            }

                            if (finalData.ElementAt(finalData.Count - 1).Current == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).Current = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween30And60 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween30And60 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween61And90 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween61And90 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween91And120 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween91And120 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsGreaterThan120 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsGreaterThan120 = 0;
                            }

                            return finalData;
                        }
                        else
                        {

                            List<AgingReportTemplate2> Data = (from chargetable in _context.Charge
                                                               join patienttable in _context.Patient on chargetable.PatientID equals patienttable.ID
                                                               join visittable in _context.Visit on chargetable.VisitID equals visittable.ID
                                                               join practicetable in _context.Practice on chargetable.PracticeID equals practicetable.ID
                                                               join providertable in _context.Provider on chargetable.ProviderID equals providertable.ID
                                                               join locationtable in _context.Location on chargetable.LocationID equals locationtable.ID
                                                               join patientplantable in _context.PatientPlan on chargetable.PrimaryPatientPlanID equals patientplantable.ID
                                                               join insurangeplantable in _context.InsurancePlan on patientplantable.InsurancePlanID equals insurangeplantable.ID

                                                               where
                                                                        (CRAgingReport.PracticeName.IsNull() ? true : CRAgingReport.PracticeName.Equals(practicetable.Name)) &&
                                                                        (CRAgingReport.ProviderName.IsNull() ? true : CRAgingReport.ProviderName.Equals(providertable.Name)) &&
                                                                        (CRAgingReport.Location.IsNull() ? true : CRAgingReport.Location.Equals(locationtable.Name)) &&
                                                                        (CRAgingReport.PayerName.IsNull() ? true : CRAgingReport.PayerName.Equals(insurangeplantable.PlanName)) &&
                                                                        (ExtensionMethods.IsBetweenDOS(CRAgingReport.DateOfServiceTo, CRAgingReport.DateOfServiceFrom, visittable.DateOfServiceTo, visittable.DateOfServiceFrom)) &&
                                                                        (ExtensionMethods.IsBetweenDOS(CRAgingReport.EntryDateTo, CRAgingReport.EntryDateFrom, visittable.AddedDate, visittable.AddedDate)) &&
                                                                        (ExtensionMethods.IsBetweenDOS(CRAgingReport.SubmittedDateTo, CRAgingReport.SubmittedDateFrom, visittable.SubmittedDate, visittable.SubmittedDate)) &&
                                                                        (CRAgingReport.PatientName.IsNull() ? true : (patienttable.FirstName.Trim() + " " + patienttable.LastName.Trim()).Equals(CRAgingReport.PatientName)) &&
                                                                        (CRAgingReport.PatientAccountNumber.IsNull() ? true : patienttable.AccountNum.Equals(CRAgingReport.PatientAccountNumber))
                                                                        && practicetable.ID == UD.PracticeID
                                                                        && ((chargetable.PrimaryBal.Val()) + (chargetable.SecondaryBal.Val()) + (chargetable.TertiaryBal.Val()) > 0)
                                                               group new
                                                               {
                                                                   //chargeAmount = chargetable.TotalAmount,
                                                                   chargeAmount = (chargetable.PrimaryBal.Val()) + (chargetable.SecondaryBal.Val()) + (chargetable.TertiaryBal.Val()),
                                                                   claimAge = CheckClaimAge(visittable.DateOfServiceFrom.HasValue ? GetClaimAge(visittable.DateOfServiceFrom.Value) : 0),
                                                                   payerName = insurangeplantable.PlanName,

                                                               } by new { PlanName = insurangeplantable.PlanName, ClaimAge = CheckClaimAge(visittable.DateOfServiceFrom.HasValue ? GetClaimAge(visittable.DateOfServiceFrom.Value) : 0) } into gp

                                                               select new AgingReportTemplate2
                                                               {
                                                                   PlanName = gp.Key.PlanName,
                                                                   charges = gp.Select(a => a.chargeAmount).Count()/*.ConvertAll<long>(new Converter<decimal, long>(DecimalToLong))*/,
                                                                   claimAges = gp.Select(a => a.claimAge).ToList(),
                                                                   claimAge = gp.Key.ClaimAge
                                                               }).ToList();

                            Data = Data.OrderBy(lamb => lamb.PlanName).ToList();
                            if (Data.Count <= 0)
                            {
                                BadRequest("Data Not Found");
                            }
                            List<GRAgingReport> finalData = new List<GRAgingReport>();
                            int currentObj = 0;

                            GRAgingReport firstRow = new GRAgingReport();
                            firstRow.PayerName = Data.ElementAt(0).PlanName;
                            finalData.Add(firstRow);
                            decimal TotalAmountFinal = 0;
                            foreach (AgingReportTemplate2 Temp in Data)
                            {

                                if (Temp.PlanName.Equals(finalData.ElementAt(currentObj).PayerName))
                                {
                                    List<int> ClaimAges = Temp.claimAges;
                                    decimal Charges = Temp.charges;
                                    decimal TotalChargeToBeAdded = 0;
                                    switch (Temp.claimAge)
                                    {

                                        case 1:
                                            TotalChargeToBeAdded = 0;
                                            //foreach (long Charge in Charges)
                                            //{
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).Current = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 2:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            finalData.ElementAt(currentObj).IsBetween30And60 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 3:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).IsBetween61And90 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;

                                            break;
                                        case 4:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            finalData.ElementAt(currentObj).IsBetween91And120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 5:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).IsGreaterThan120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;

                                            break;
                                    }
                                }
                                else
                                {
                                    if (finalData.ElementAt(currentObj).Current == null)
                                    {
                                        finalData.ElementAt(currentObj).Current = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween30And60 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween30And60 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween61And90 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween61And90 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween91And120 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween91And120 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsGreaterThan120 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsGreaterThan120 = 0;
                                    }
                                    TotalAmountFinal = 0;
                                    currentObj = currentObj + 1;
                                    GRAgingReport TempRow = new GRAgingReport();
                                    TempRow.PayerName = Temp.PlanName;

                                    List<int> ClaimAges = Temp.claimAges;
                                    decimal Charges = Temp.charges;
                                    decimal TotalChargeToBeAdded = 0;
                                    switch (Temp.claimAge)
                                    {

                                        case 1:
                                            TotalChargeToBeAdded = 0;
                                            // foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            TempRow.Current = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 2:
                                            TotalChargeToBeAdded = 0;
                                            //   foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            TempRow.IsBetween30And60 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 3:
                                            TotalChargeToBeAdded = 0;
                                            // foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            TempRow.IsBetween61And90 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;

                                            break;
                                        case 4:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            //   {
                                            TotalChargeToBeAdded += Charges;
                                            //   }
                                            TempRow.IsBetween91And120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 5:
                                            TotalChargeToBeAdded = 0;
                                            //   foreach (long Charge in Charges)
                                            //   {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            TempRow.IsGreaterThan120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                    }
                                    finalData.Add(TempRow);
                                }
                            }

                            if (finalData.ElementAt(finalData.Count - 1).Current == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).Current = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween30And60 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween30And60 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween61And90 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween61And90 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween91And120 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween91And120 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsGreaterThan120 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsGreaterThan120 = 0;
                            }

                            return finalData;
                        }
                    }
                }
                else
                {
                    if (CRAgingReport.AllValue == true)
                    {

                        if (CRAgingReport.SearchType == "ED")
                        {

                            List<AgingReportTemplate2> Data = new List<AgingReportTemplate2>();
                            using (SqlConnection myConnection = new SqlConnection(newConnection))
                            {

                                string oString = "SELECT 	SUM( case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 	) 	as chargeAmount  ,case when insurangeplantable.PlanName = 'All Plans' then 'All Plans'    else 'All Plans'  end as payerName ,  case  when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )>= 0 then 1 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 30 then 2 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 60 then 3 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 90 then 4 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 120 then 5 else 0 end as claimAge   FROM Charge chargetable join  Patient patienttable on chargetable.PatientID = patienttable.ID join Visit v on chargetable.VisitID = v.ID join Practice practicetable on chargetable.PracticeID = practicetable.ID join [Provider] providertable on chargetable.ProviderID =  providertable.ID join [Location] locationtable on chargetable.LocationID = locationtable.ID join PatientPlan patientplantable on chargetable.PrimaryPatientPlanID = patientplantable.ID join InsurancePlan insurangeplantable on patientplantable.InsurancePlanID = insurangeplantable.ID join Cpt cpt on chargetable.CPTID = cpt.ID left join  RefProvider refprovidertable on chargetable.RefProviderID = refprovidertable.ID left join PatientPlan secpatientplantable on chargetable.SecondaryPatientPlanID = secpatientplantable.ID left join PatientPlan terpatientplantable on chargetable.TertiaryPatientPlanID = terpatientplantable.ID left join InsurancePlan secinsuranceplantable on secpatientplantable.InsurancePlanID = secinsuranceplantable.ID left join InsurancePlan terinsuranceplantable on terpatientplantable.InsurancePlanID = terinsuranceplantable.ID  where chargetable.AddedDate IS NOT NULL  ";

                                oString += "  and chargetable.practiceid = {0} ";
                                oString = string.Format(oString, PracticeId);

                                if (!CRAgingReport.PracticeName.IsNull())
                                    oString += string.Format(" and practicetable.Name like '%{0}%'", CRAgingReport.PracticeName);
                                if (!CRAgingReport.ProviderName.IsNull())
                                    oString += string.Format(" and providertable.Name like '%{0}%'", CRAgingReport.ProviderName);
                                if (!CRAgingReport.Location.IsNull())
                                    oString += string.Format(" and locationtable.Name like '%{0}%'", CRAgingReport.Location);
                                if (!CRAgingReport.PayerName.IsNull())
                                    oString += string.Format(" and insurangeplantable.PlanName like '%{0}%'", CRAgingReport.PayerName);
                                if (!CRAgingReport.PatientName.IsNull())
                                    oString += string.Format(" and patienttable.FirstName like '%{0}%'", CRAgingReport.PatientName);
                                if (!CRAgingReport.PatientName.IsNull())
                                    oString += string.Format(" and patienttable.LastName like '%{0}%'", CRAgingReport.PatientName);
                                if (!CRAgingReport.PatientAccountNumber.IsNull())
                                    oString += string.Format(" and patienttable.AccountNum ='{0}'", CRAgingReport.PatientAccountNumber);


                                if (CRAgingReport.EntryDateFrom != null && CRAgingReport.EntryDateTo != null)
                                {
                                    oString += (" and (chargetable.AddedDate  >= '" + CRAgingReport.EntryDateFrom.GetValueOrDefault().Date + "' and chargetable.AddedDate  < '" + CRAgingReport.EntryDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                                }
                                else if (CRAgingReport.EntryDateFrom != null && CRAgingReport.EntryDateTo == null)
                                {
                                    oString += (" and ( chargetable.AddedDate  >= '" + CRAgingReport.EntryDateFrom.GetValueOrDefault().Date + "')");

                                }
                                else if (CRAgingReport.EntryDateFrom == null && CRAgingReport.EntryDateTo != null)
                                {
                                    oString += (" and (chargetable.AddedDate  <= '" + CRAgingReport.EntryDateTo.GetValueOrDefault().Date + "')");
                                }


                                if (CRAgingReport.SubmittedDateFrom != null && CRAgingReport.SubmittedDateTo != null)
                                {
                                    oString += (" and (chargetable.SubmittetdDate  >= '" + CRAgingReport.SubmittedDateFrom.GetValueOrDefault().Date + "' and chargetable.SubmittetdDate  < '" + CRAgingReport.SubmittedDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                                }
                                else if (CRAgingReport.SubmittedDateFrom != null && CRAgingReport.SubmittedDateTo == null)
                                {
                                    oString += (" and ( chargetable.SubmittetdDate  >= '" + CRAgingReport.SubmittedDateFrom.GetValueOrDefault().Date + "')");

                                }
                                else if (CRAgingReport.SubmittedDateFrom == null && CRAgingReport.SubmittedDateTo != null)
                                {
                                    oString += (" and (chargetable.SubmittetdDate  <= '" + CRAgingReport.SubmittedDateTo.GetValueOrDefault().Date + "')");
                                }

                                oString += " GROUP BY case when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )>= 0 then 1 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 30 then 2 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 60 then 3 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 90 then 4 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 120 then 5 else 0    end , case when insurangeplantable.PlanName = 'All Plans' then 'All Plans'    else 'All Plans'  end having ( SUM(			case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +			 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 			 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 			) >  0) ";
                                //having (SUM(ISNULL(chargetable.PrimaryBal, 0) + ISNULL(chargetable.SecondaryBal, 0) + ISNULL(chargetable.TertiaryBal, 0)) >  0)
                                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                                myConnection.Open();
                                using (SqlDataReader oReader = oCmd.ExecuteReader())
                                {
                                    while (oReader.Read())
                                    {
                                        var aging = new AgingReportTemplate2();
                                        aging.PlanName = oReader["payerName"].ToString();
                                        aging.charges = Convert.ToDecimal(oReader["chargeAmount"].ToString());
                                        aging.claimAge = Convert.ToInt32(oReader["claimAge"].ToString());


                                        Data.Add(aging);
                                    }
                                    myConnection.Close();
                                }
                            }


                            List<GRAgingReport> finalData = new List<GRAgingReport>();
                            int currentObj = 0;

                            GRAgingReport firstRow = new GRAgingReport();
                            firstRow.PayerName = Data.ElementAt(0).PlanName;
                            finalData.Add(firstRow);
                            decimal TotalAmountFinal = 0;
                            foreach (AgingReportTemplate2 Temp in Data)
                            {

                                if (Temp.PlanName.Equals(finalData.ElementAt(currentObj).PayerName))
                                {
                                    List<int> ClaimAges = Temp.claimAges;
                                    decimal Charges = Temp.charges;
                                    decimal TotalChargeToBeAdded = 0;
                                    switch (Temp.claimAge)
                                    {

                                        case 1:
                                            TotalChargeToBeAdded = 0;
                                            //foreach (long Charge in Charges)
                                            //{
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).Current = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 2:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            finalData.ElementAt(currentObj).IsBetween30And60 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 3:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).IsBetween61And90 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;

                                            break;
                                        case 4:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            finalData.ElementAt(currentObj).IsBetween91And120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 5:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).IsGreaterThan120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;

                                            break;
                                    }
                                }
                                else
                                {
                                    if (finalData.ElementAt(currentObj).Current == null)
                                    {
                                        finalData.ElementAt(currentObj).Current = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween30And60 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween30And60 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween61And90 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween61And90 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween91And120 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween91And120 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsGreaterThan120 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsGreaterThan120 = 0;
                                    }
                                    TotalAmountFinal = 0;
                                    currentObj = currentObj + 1;
                                    GRAgingReport TempRow = new GRAgingReport();
                                    TempRow.PayerName = Temp.PlanName;

                                    List<int> ClaimAges = Temp.claimAges;
                                    decimal Charges = Temp.charges;
                                    decimal TotalChargeToBeAdded = 0;
                                    switch (Temp.claimAge)
                                    {

                                        case 1:
                                            TotalChargeToBeAdded = 0;
                                            // foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            TempRow.Current = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 2:
                                            TotalChargeToBeAdded = 0;
                                            //   foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            TempRow.IsBetween30And60 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 3:
                                            TotalChargeToBeAdded = 0;
                                            // foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            TempRow.IsBetween61And90 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;

                                            break;
                                        case 4:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            //   {
                                            TotalChargeToBeAdded += Charges;
                                            //   }
                                            TempRow.IsBetween91And120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 5:
                                            TotalChargeToBeAdded = 0;
                                            //   foreach (long Charge in Charges)
                                            //   {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            TempRow.IsGreaterThan120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                    }
                                    finalData.Add(TempRow);
                                }
                            }

                            if (finalData.ElementAt(finalData.Count - 1).Current == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).Current = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween30And60 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween30And60 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween61And90 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween61And90 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween91And120 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween91And120 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsGreaterThan120 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsGreaterThan120 = 0;
                            }

                            return finalData;
                        }
                        else if (CRAgingReport.SearchType == "SD")
                        {

                            List<AgingReportTemplate2> Data = new List<AgingReportTemplate2>();
                            using (SqlConnection myConnection = new SqlConnection(newConnection))
                            {

                                string oString = "SELECT 	SUM( case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 	) 	as chargeAmount  ,case when insurangeplantable.PlanName = 'All Plans' then 'All Plans'    else 'All Plans'  end as payerName ,  case  when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )>= 0 then 1 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 30 then 2 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 60 then 3 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 90 then 4 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 120 then 5 else 0 end as claimAge   FROM Charge chargetable join  Patient patienttable on chargetable.PatientID = patienttable.ID join Visit v on chargetable.VisitID = v.ID join Practice practicetable on chargetable.PracticeID = practicetable.ID join [Provider] providertable on chargetable.ProviderID =  providertable.ID join [Location] locationtable on chargetable.LocationID = locationtable.ID join PatientPlan patientplantable on chargetable.PrimaryPatientPlanID = patientplantable.ID join InsurancePlan insurangeplantable on patientplantable.InsurancePlanID = insurangeplantable.ID join Cpt cpt on chargetable.CPTID = cpt.ID left join  RefProvider refprovidertable on chargetable.RefProviderID = refprovidertable.ID left join PatientPlan secpatientplantable on chargetable.SecondaryPatientPlanID = secpatientplantable.ID left join PatientPlan terpatientplantable on chargetable.TertiaryPatientPlanID = terpatientplantable.ID left join InsurancePlan secinsuranceplantable on secpatientplantable.InsurancePlanID = secinsuranceplantable.ID left join InsurancePlan terinsuranceplantable on terpatientplantable.InsurancePlanID = terinsuranceplantable.ID  where chargetable.SubmittetdDate IS NOT NULL   ";

                                oString += "  and chargetable.practiceid = {0} ";
                                oString = string.Format(oString, PracticeId);

                                if (!CRAgingReport.PracticeName.IsNull())
                                    oString += string.Format(" and practicetable.Name like '%{0}%'", CRAgingReport.PracticeName);
                                if (!CRAgingReport.ProviderName.IsNull())
                                    oString += string.Format(" and providertable.Name like '%{0}%'", CRAgingReport.ProviderName);
                                if (!CRAgingReport.Location.IsNull())
                                    oString += string.Format(" and locationtable.Name like '%{0}%'", CRAgingReport.Location);
                                if (!CRAgingReport.PayerName.IsNull())
                                    oString += string.Format(" and insurangeplantable.PlanName like '%{0}%'", CRAgingReport.PayerName);
                                if (!CRAgingReport.PatientName.IsNull())
                                    oString += string.Format(" and patienttable.FirstName like '%{0}%'", CRAgingReport.PatientName);
                                if (!CRAgingReport.PatientName.IsNull())
                                    oString += string.Format(" and patienttable.LastName like '%{0}%'", CRAgingReport.PatientName);
                                if (!CRAgingReport.PatientAccountNumber.IsNull())
                                    oString += string.Format(" and patienttable.AccountNum ='{0}'", CRAgingReport.PatientAccountNumber);


                                if (CRAgingReport.EntryDateFrom != null && CRAgingReport.EntryDateTo != null)
                                {
                                    oString += (" and (chargetable.AddedDate  >= '" + CRAgingReport.EntryDateFrom.GetValueOrDefault().Date + "' and chargetable.AddedDate  < '" + CRAgingReport.EntryDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                                }
                                else if (CRAgingReport.EntryDateFrom != null && CRAgingReport.EntryDateTo == null)
                                {
                                    oString += (" and ( chargetable.AddedDate  >= '" + CRAgingReport.EntryDateFrom.GetValueOrDefault().Date + "')");

                                }
                                else if (CRAgingReport.EntryDateFrom == null && CRAgingReport.EntryDateTo != null)
                                {
                                    oString += (" and (chargetable.AddedDate  <= '" + CRAgingReport.EntryDateTo.GetValueOrDefault().Date + "')");
                                }


                                if (CRAgingReport.SubmittedDateFrom != null && CRAgingReport.SubmittedDateTo != null)
                                {
                                    oString += (" and (chargetable.SubmittetdDate  >= '" + CRAgingReport.SubmittedDateFrom.GetValueOrDefault().Date + "' and chargetable.SubmittetdDate  < '" + CRAgingReport.SubmittedDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                                }
                                else if (CRAgingReport.SubmittedDateFrom != null && CRAgingReport.SubmittedDateTo == null)
                                {
                                    oString += (" and ( chargetable.SubmittetdDate  >= '" + CRAgingReport.SubmittedDateFrom.GetValueOrDefault().Date + "')");

                                }
                                else if (CRAgingReport.SubmittedDateFrom == null && CRAgingReport.SubmittedDateTo != null)
                                {
                                    oString += (" and (chargetable.SubmittetdDate  <= '" + CRAgingReport.SubmittedDateTo.GetValueOrDefault().Date + "')");
                                }

                                oString += " GROUP BY case when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )>= 0 then 1 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 30 then 2 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 60 then 3 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 90 then 4 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 120 then 5 else 0    end , case when insurangeplantable.PlanName = 'All Plans' then 'All Plans'    else 'All Plans'  end having ( SUM(			case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +			 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 			 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 			) >  0)  ";
                                //having (SUM(ISNULL(chargetable.PrimaryBal, 0) + ISNULL(chargetable.SecondaryBal, 0) + ISNULL(chargetable.TertiaryBal, 0)) >  0)
                                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                                myConnection.Open();
                                using (SqlDataReader oReader = oCmd.ExecuteReader())
                                {
                                    while (oReader.Read())
                                    {
                                        var aging = new AgingReportTemplate2();
                                        aging.PlanName = oReader["payerName"].ToString();
                                        aging.charges = Convert.ToDecimal(oReader["chargeAmount"].ToString());
                                        //  aging.claimAges = gp.Select(a => a.claimAge).ToList();
                                        aging.claimAge = Convert.ToInt32(oReader["claimAge"].ToString());


                                        Data.Add(aging);
                                    }
                                    myConnection.Close();
                                }
                            }


                            List<GRAgingReport> finalData = new List<GRAgingReport>();
                            int currentObj = 0;

                            GRAgingReport firstRow = new GRAgingReport();
                            firstRow.PayerName = Data.ElementAt(0).PlanName;
                            finalData.Add(firstRow);
                            decimal TotalAmountFinal = 0;
                            foreach (AgingReportTemplate2 Temp in Data)
                            {

                                if (Temp.PlanName.Equals(finalData.ElementAt(currentObj).PayerName))
                                {
                                    List<int> ClaimAges = Temp.claimAges;
                                    decimal Charges = Temp.charges;
                                    decimal TotalChargeToBeAdded = 0;
                                    switch (Temp.claimAge)
                                    {

                                        case 1:
                                            TotalChargeToBeAdded = 0;
                                            //foreach (long Charge in Charges)
                                            //{
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).Current = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 2:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            finalData.ElementAt(currentObj).IsBetween30And60 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 3:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).IsBetween61And90 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;

                                            break;
                                        case 4:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            finalData.ElementAt(currentObj).IsBetween91And120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 5:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).IsGreaterThan120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;

                                            break;
                                    }
                                }
                                else
                                {
                                    if (finalData.ElementAt(currentObj).Current == null)
                                    {
                                        finalData.ElementAt(currentObj).Current = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween30And60 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween30And60 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween61And90 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween61And90 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween91And120 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween91And120 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsGreaterThan120 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsGreaterThan120 = 0;
                                    }
                                    TotalAmountFinal = 0;
                                    currentObj = currentObj + 1;
                                    GRAgingReport TempRow = new GRAgingReport();
                                    TempRow.PayerName = Temp.PlanName;

                                    List<int> ClaimAges = Temp.claimAges;
                                    decimal Charges = Temp.charges;
                                    decimal TotalChargeToBeAdded = 0;
                                    switch (Temp.claimAge)
                                    {

                                        case 1:
                                            TotalChargeToBeAdded = 0;
                                            // foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            TempRow.Current = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 2:
                                            TotalChargeToBeAdded = 0;
                                            //   foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            TempRow.IsBetween30And60 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 3:
                                            TotalChargeToBeAdded = 0;
                                            // foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            TempRow.IsBetween61And90 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;

                                            break;
                                        case 4:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            //   {
                                            TotalChargeToBeAdded += Charges;
                                            //   }
                                            TempRow.IsBetween91And120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 5:
                                            TotalChargeToBeAdded = 0;
                                            //   foreach (long Charge in Charges)
                                            //   {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            TempRow.IsGreaterThan120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                    }
                                    finalData.Add(TempRow);
                                }
                            }

                            if (finalData.ElementAt(finalData.Count - 1).Current == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).Current = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween30And60 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween30And60 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween61And90 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween61And90 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween91And120 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween91And120 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsGreaterThan120 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsGreaterThan120 = 0;
                            }

                            return finalData;
                        }
                        else
                        {

                            List<AgingReportTemplate2> Data = new List<AgingReportTemplate2>();
                            using (SqlConnection myConnection = new SqlConnection(newConnection))
                            {

                                string oString = "SELECT 	SUM( case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 	) 	as chargeAmount  ,case when insurangeplantable.PlanName = 'All Plans' then 'All Plans'    else 'All Plans'  end as payerName ,  case  when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )>= 0 then 1 when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )> 30 then 2 when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )> 60 then 3 when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )> 90 then 4 when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )> 120 then 5 else 0 end as claimAge   FROM Charge chargetable join  Patient patienttable on chargetable.PatientID = patienttable.ID join Visit v on chargetable.VisitID = v.ID join Practice practicetable on chargetable.PracticeID = practicetable.ID join [Provider] providertable on chargetable.ProviderID =  providertable.ID join [Location] locationtable on chargetable.LocationID = locationtable.ID join PatientPlan patientplantable on chargetable.PrimaryPatientPlanID = patientplantable.ID join InsurancePlan insurangeplantable on patientplantable.InsurancePlanID = insurangeplantable.ID join Cpt cpt on chargetable.CPTID = cpt.ID left join  RefProvider refprovidertable on chargetable.RefProviderID = refprovidertable.ID left join PatientPlan secpatientplantable on chargetable.SecondaryPatientPlanID = secpatientplantable.ID left join PatientPlan terpatientplantable on chargetable.TertiaryPatientPlanID = terpatientplantable.ID left join InsurancePlan secinsuranceplantable on secpatientplantable.InsurancePlanID = secinsuranceplantable.ID left join InsurancePlan terinsuranceplantable on terpatientplantable.InsurancePlanID = terinsuranceplantable.ID  where chargetable.DateOfServiceFrom IS NOT NULL    ";

                                oString += "  and chargetable.practiceid = {0} ";
                                oString = string.Format(oString, PracticeId);

                                if (!CRAgingReport.PracticeName.IsNull())
                                    oString += string.Format(" and practicetable.Name like '%{0}%'", CRAgingReport.PracticeName);
                                if (!CRAgingReport.ProviderName.IsNull())
                                    oString += string.Format(" and providertable.Name like '%{0}%'", CRAgingReport.ProviderName);
                                if (!CRAgingReport.Location.IsNull())
                                    oString += string.Format(" and locationtable.Name like '%{0}%'", CRAgingReport.Location);
                                if (!CRAgingReport.PayerName.IsNull())
                                    oString += string.Format(" and insurangeplantable.PlanName like '%{0}%'", CRAgingReport.PayerName);
                                if (!CRAgingReport.PatientName.IsNull())
                                    oString += string.Format(" and patienttable.FirstName like '%{0}%'", CRAgingReport.PatientName);
                                if (!CRAgingReport.PatientName.IsNull())
                                    oString += string.Format(" and patienttable.LastName like '%{0}%'", CRAgingReport.PatientName);
                                if (!CRAgingReport.PatientAccountNumber.IsNull())
                                    oString += string.Format(" and patienttable.AccountNum ='{0}'", CRAgingReport.PatientAccountNumber);


                                if (CRAgingReport.EntryDateFrom != null && CRAgingReport.EntryDateTo != null)
                                {
                                    oString += (" and (chargetable.AddedDate  >= '" + CRAgingReport.EntryDateFrom.GetValueOrDefault().Date + "' and chargetable.AddedDate  < '" + CRAgingReport.EntryDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                                }
                                else if (CRAgingReport.EntryDateFrom != null && CRAgingReport.EntryDateTo == null)
                                {
                                    oString += (" and ( chargetable.AddedDate  >= '" + CRAgingReport.EntryDateFrom.GetValueOrDefault().Date + "')");

                                }
                                else if (CRAgingReport.EntryDateFrom == null && CRAgingReport.EntryDateTo != null)
                                {
                                    oString += (" and (chargetable.AddedDate  <= '" + CRAgingReport.EntryDateTo.GetValueOrDefault().Date + "')");
                                }


                                if (CRAgingReport.SubmittedDateFrom != null && CRAgingReport.SubmittedDateTo != null)
                                {
                                    oString += (" and (chargetable.SubmittetdDate  >= '" + CRAgingReport.SubmittedDateFrom.GetValueOrDefault().Date + "' and chargetable.SubmittetdDate  < '" + CRAgingReport.SubmittedDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                                }
                                else if (CRAgingReport.SubmittedDateFrom != null && CRAgingReport.SubmittedDateTo == null)
                                {
                                    oString += (" and ( chargetable.SubmittetdDate  >= '" + CRAgingReport.SubmittedDateFrom.GetValueOrDefault().Date + "')");

                                }
                                else if (CRAgingReport.SubmittedDateFrom == null && CRAgingReport.SubmittedDateTo != null)
                                {
                                    oString += (" and (chargetable.SubmittetdDate  <= '" + CRAgingReport.SubmittedDateTo.GetValueOrDefault().Date + "')");
                                }

                                oString += " GROUP BY case when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )>= 0 then 1 when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )> 30 then 2 when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )> 60 then 3 when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )> 90 then 4 when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )> 120 then 5 else 0    end , case when insurangeplantable.PlanName = 'All Plans' then 'All Plans'    else 'All Plans'  end having ( SUM(	case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +			 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 		 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 	) >  0) ";
                                //having (SUM(ISNULL(chargetable.PrimaryBal, 0) + ISNULL(chargetable.SecondaryBal, 0) + ISNULL(chargetable.TertiaryBal, 0)) >  0)
                                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                                myConnection.Open();
                                using (SqlDataReader oReader = oCmd.ExecuteReader())
                                {
                                    while (oReader.Read())
                                    {
                                        var aging = new AgingReportTemplate2();
                                        aging.PlanName = oReader["payerName"].ToString();
                                        aging.charges = Convert.ToDecimal(oReader["chargeAmount"].ToString());
                                        //  aging.claimAges = gp.Select(a => a.claimAge).ToList();
                                        aging.claimAge = Convert.ToInt32(oReader["claimAge"].ToString());


                                        Data.Add(aging);
                                    }
                                    myConnection.Close();
                                }
                            }


                            List<GRAgingReport> finalData = new List<GRAgingReport>();
                            int currentObj = 0;

                            GRAgingReport firstRow = new GRAgingReport();
                            firstRow.PayerName = Data.ElementAt(0).PlanName;
                            finalData.Add(firstRow);
                            decimal TotalAmountFinal = 0;
                            foreach (AgingReportTemplate2 Temp in Data)
                            {

                                if (Temp.PlanName.Equals(finalData.ElementAt(currentObj).PayerName))
                                {
                                    List<int> ClaimAges = Temp.claimAges;
                                    decimal Charges = Temp.charges;
                                    decimal TotalChargeToBeAdded = 0;
                                    switch (Temp.claimAge)
                                    {

                                        case 1:
                                            TotalChargeToBeAdded = 0;
                                            //foreach (long Charge in Charges)
                                            //{
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).Current = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 2:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            finalData.ElementAt(currentObj).IsBetween30And60 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 3:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).IsBetween61And90 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;

                                            break;
                                        case 4:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            finalData.ElementAt(currentObj).IsBetween91And120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 5:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).IsGreaterThan120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;

                                            break;
                                    }
                                }
                                else
                                {
                                    if (finalData.ElementAt(currentObj).Current == null)
                                    {
                                        finalData.ElementAt(currentObj).Current = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween30And60 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween30And60 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween61And90 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween61And90 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween91And120 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween91And120 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsGreaterThan120 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsGreaterThan120 = 0;
                                    }
                                    TotalAmountFinal = 0;
                                    currentObj = currentObj + 1;
                                    GRAgingReport TempRow = new GRAgingReport();
                                    TempRow.PayerName = Temp.PlanName;

                                    List<int> ClaimAges = Temp.claimAges;
                                    decimal Charges = Temp.charges;
                                    decimal TotalChargeToBeAdded = 0;
                                    switch (Temp.claimAge)
                                    {

                                        case 1:
                                            TotalChargeToBeAdded = 0;
                                            // foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            TempRow.Current = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 2:
                                            TotalChargeToBeAdded = 0;
                                            //   foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            TempRow.IsBetween30And60 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 3:
                                            TotalChargeToBeAdded = 0;
                                            // foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            TempRow.IsBetween61And90 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;

                                            break;
                                        case 4:
                                            TotalChargeToBeAdded = 0;
                                            TotalChargeToBeAdded += Charges;
                                            TempRow.IsBetween91And120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 5:
                                            TotalChargeToBeAdded = 0;
                                            TotalChargeToBeAdded += Charges;
                                            TempRow.IsGreaterThan120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                    }
                                    finalData.Add(TempRow);
                                }
                            }

                            if (finalData.ElementAt(finalData.Count - 1).Current == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).Current = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween30And60 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween30And60 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween61And90 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween61And90 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween91And120 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween91And120 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsGreaterThan120 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsGreaterThan120 = 0;
                            }

                            return finalData;
                        }

                    }
                    else
                    {

                        if (CRAgingReport.SearchType == "ED")
                        {

                            List<AgingReportTemplate2> Data = new List<AgingReportTemplate2>();
                            using (SqlConnection myConnection = new SqlConnection(newConnection))
                            {

                                string oString = "SELECT 	SUM( case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 	) 	as chargeAmount  ,insurangeplantable.PlanName  as payerName ,  case  when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )>= 0 then 1 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 30 then 2 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 60 then 3 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 90 then 4 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 120 then 5 else 0 end as claimAge   FROM Charge chargetable join  Patient patienttable on chargetable.PatientID = patienttable.ID join Visit v on chargetable.VisitID = v.ID join Practice practicetable on chargetable.PracticeID = practicetable.ID join [Provider] providertable on chargetable.ProviderID =  providertable.ID join [Location] locationtable on chargetable.LocationID = locationtable.ID join PatientPlan patientplantable on chargetable.PrimaryPatientPlanID = patientplantable.ID join InsurancePlan insurangeplantable on patientplantable.InsurancePlanID = insurangeplantable.ID join Cpt cpt on chargetable.CPTID = cpt.ID left join  RefProvider refprovidertable on chargetable.RefProviderID = refprovidertable.ID left join PatientPlan secpatientplantable on chargetable.SecondaryPatientPlanID = secpatientplantable.ID left join PatientPlan terpatientplantable on chargetable.TertiaryPatientPlanID = terpatientplantable.ID left join InsurancePlan secinsuranceplantable on secpatientplantable.InsurancePlanID = secinsuranceplantable.ID left join InsurancePlan terinsuranceplantable on terpatientplantable.InsurancePlanID = terinsuranceplantable.ID  where chargetable.AddedDate IS NOT NULL    ";

                                oString += "  and chargetable.practiceid = {0} ";
                                oString = string.Format(oString, PracticeId);

                                if (!CRAgingReport.PracticeName.IsNull())
                                    oString += string.Format(" and practicetable.Name like '%{0}%'", CRAgingReport.PracticeName);
                                if (!CRAgingReport.ProviderName.IsNull())
                                    oString += string.Format(" and providertable.Name like '%{0}%'", CRAgingReport.ProviderName);
                                if (!CRAgingReport.Location.IsNull())
                                    oString += string.Format(" and locationtable.Name like '%{0}%'", CRAgingReport.Location);
                                if (!CRAgingReport.PayerName.IsNull())
                                    oString += string.Format(" and insurangeplantable.PlanName like '%{0}%'", CRAgingReport.PayerName);
                                if (!CRAgingReport.PatientName.IsNull())
                                    oString += string.Format(" and patienttable.FirstName like '%{0}%'", CRAgingReport.PatientName);
                                if (!CRAgingReport.PatientName.IsNull())
                                    oString += string.Format(" and patienttable.LastName like '%{0}%'", CRAgingReport.PatientName);
                                if (!CRAgingReport.PatientAccountNumber.IsNull())
                                    oString += string.Format(" and patienttable.AccountNum ='{0}'", CRAgingReport.PatientAccountNumber);


                                if (CRAgingReport.EntryDateFrom != null && CRAgingReport.EntryDateTo != null)
                                {
                                    oString += (" and (chargetable.AddedDate  >= '" + CRAgingReport.EntryDateFrom.GetValueOrDefault().Date + "' and chargetable.AddedDate  < '" + CRAgingReport.EntryDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                                }
                                else if (CRAgingReport.EntryDateFrom != null && CRAgingReport.EntryDateTo == null)
                                {
                                    oString += (" and ( chargetable.AddedDate  >= '" + CRAgingReport.EntryDateFrom.GetValueOrDefault().Date + "')");

                                }
                                else if (CRAgingReport.EntryDateFrom == null && CRAgingReport.EntryDateTo != null)
                                {
                                    oString += (" and (chargetable.AddedDate  <= '" + CRAgingReport.EntryDateTo.GetValueOrDefault().Date + "')");
                                }


                                if (CRAgingReport.SubmittedDateFrom != null && CRAgingReport.SubmittedDateTo != null)
                                {
                                    oString += (" and (chargetable.SubmittetdDate  >= '" + CRAgingReport.SubmittedDateFrom.GetValueOrDefault().Date + "' and chargetable.SubmittetdDate  < '" + CRAgingReport.SubmittedDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                                }
                                else if (CRAgingReport.SubmittedDateFrom != null && CRAgingReport.SubmittedDateTo == null)
                                {
                                    oString += (" and ( chargetable.SubmittetdDate  >= '" + CRAgingReport.SubmittedDateFrom.GetValueOrDefault().Date + "')");

                                }
                                else if (CRAgingReport.SubmittedDateFrom == null && CRAgingReport.SubmittedDateTo != null)
                                {
                                    oString += (" and (chargetable.SubmittetdDate  <= '" + CRAgingReport.SubmittedDateTo.GetValueOrDefault().Date + "')");
                                }

                                oString += "GROUP BY case when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )>= 0 then 1 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 30 then 2 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 60 then 3 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 90 then 4 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 120 then 5 else 0    end , insurangeplantable.PlanName  having ( SUM(			case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +			 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 			 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 			) >  0)  ";
                                //having (SUM(ISNULL(chargetable.PrimaryBal, 0) + ISNULL(chargetable.SecondaryBal, 0) + ISNULL(chargetable.TertiaryBal, 0)) >  0)
                                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                                myConnection.Open();
                                using (SqlDataReader oReader = oCmd.ExecuteReader())
                                {
                                    while (oReader.Read())
                                    {
                                        var aging = new AgingReportTemplate2();
                                        aging.PlanName = oReader["payerName"].ToString();
                                        aging.charges = Convert.ToDecimal(oReader["chargeAmount"].ToString());
                                        aging.claimAge = Convert.ToInt32(oReader["claimAge"].ToString());


                                        Data.Add(aging);
                                    }
                                    myConnection.Close();
                                }
                            }


                            List<GRAgingReport> finalData = new List<GRAgingReport>();
                            int currentObj = 0;

                            GRAgingReport firstRow = new GRAgingReport();
                            firstRow.PayerName = Data.ElementAt(0).PlanName;
                            finalData.Add(firstRow);
                            decimal TotalAmountFinal = 0;
                            foreach (AgingReportTemplate2 Temp in Data)
                            {

                                if (Temp.PlanName.Equals(finalData.ElementAt(currentObj).PayerName))
                                {
                                    List<int> ClaimAges = Temp.claimAges;
                                    decimal Charges = Temp.charges;
                                    decimal TotalChargeToBeAdded = 0;
                                    switch (Temp.claimAge)
                                    {

                                        case 1:
                                            TotalChargeToBeAdded = 0;
                                            //foreach (long Charge in Charges)
                                            //{
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).Current = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 2:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            finalData.ElementAt(currentObj).IsBetween30And60 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 3:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).IsBetween61And90 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;

                                            break;
                                        case 4:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            finalData.ElementAt(currentObj).IsBetween91And120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 5:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).IsGreaterThan120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;

                                            break;
                                    }
                                }
                                else
                                {
                                    if (finalData.ElementAt(currentObj).Current == null)
                                    {
                                        finalData.ElementAt(currentObj).Current = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween30And60 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween30And60 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween61And90 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween61And90 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween91And120 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween91And120 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsGreaterThan120 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsGreaterThan120 = 0;
                                    }
                                    TotalAmountFinal = 0;
                                    currentObj = currentObj + 1;
                                    GRAgingReport TempRow = new GRAgingReport();
                                    TempRow.PayerName = Temp.PlanName;

                                    List<int> ClaimAges = Temp.claimAges;
                                    decimal Charges = Temp.charges;
                                    decimal TotalChargeToBeAdded = 0;
                                    switch (Temp.claimAge)
                                    {

                                        case 1:
                                            TotalChargeToBeAdded = 0;
                                            // foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            TempRow.Current = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 2:
                                            TotalChargeToBeAdded = 0;
                                            //   foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            TempRow.IsBetween30And60 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 3:
                                            TotalChargeToBeAdded = 0;
                                            // foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            TempRow.IsBetween61And90 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;

                                            break;
                                        case 4:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            //   {
                                            TotalChargeToBeAdded += Charges;
                                            //   }
                                            TempRow.IsBetween91And120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 5:
                                            TotalChargeToBeAdded = 0;
                                            //   foreach (long Charge in Charges)
                                            //   {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            TempRow.IsGreaterThan120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                    }
                                    finalData.Add(TempRow);
                                }
                            }

                            if (finalData.ElementAt(finalData.Count - 1).Current == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).Current = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween30And60 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween30And60 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween61And90 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween61And90 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween91And120 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween91And120 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsGreaterThan120 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsGreaterThan120 = 0;
                            }

                            return finalData;
                        }
                        else if (CRAgingReport.SearchType == "SD")
                        {

                            List<AgingReportTemplate2> Data = new List<AgingReportTemplate2>();
                            using (SqlConnection myConnection = new SqlConnection(newConnection))
                            {

                                string oString = "SELECT 	SUM( case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 	) 	as chargeAmount  ,insurangeplantable.PlanName  as payerName,  case  when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )>= 0 then 1 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 30 then 2 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 60 then 3 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 90 then 4 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 120 then 5 else 0 end as claimAge   FROM Charge chargetable join  Patient patienttable on chargetable.PatientID = patienttable.ID join Visit v on chargetable.VisitID = v.ID join Practice practicetable on chargetable.PracticeID = practicetable.ID join [Provider] providertable on chargetable.ProviderID =  providertable.ID join [Location] locationtable on chargetable.LocationID = locationtable.ID join PatientPlan patientplantable on chargetable.PrimaryPatientPlanID = patientplantable.ID join InsurancePlan insurangeplantable on patientplantable.InsurancePlanID = insurangeplantable.ID join Cpt cpt on chargetable.CPTID = cpt.ID left join  RefProvider refprovidertable on chargetable.RefProviderID = refprovidertable.ID left join PatientPlan secpatientplantable on chargetable.SecondaryPatientPlanID = secpatientplantable.ID left join PatientPlan terpatientplantable on chargetable.TertiaryPatientPlanID = terpatientplantable.ID left join InsurancePlan secinsuranceplantable on secpatientplantable.InsurancePlanID = secinsuranceplantable.ID left join InsurancePlan terinsuranceplantable on terpatientplantable.InsurancePlanID = terinsuranceplantable.ID  where chargetable.SubmittetdDate IS NOT NULL   ";

                                oString += "  and chargetable.practiceid = {0} ";
                                oString = string.Format(oString, PracticeId);

                                if (!CRAgingReport.PracticeName.IsNull())
                                    oString += string.Format(" and practicetable.Name like '%{0}%'", CRAgingReport.PracticeName);
                                if (!CRAgingReport.ProviderName.IsNull())
                                    oString += string.Format(" and providertable.Name like '%{0}%'", CRAgingReport.ProviderName);
                                if (!CRAgingReport.Location.IsNull())
                                    oString += string.Format(" and locationtable.Name like '%{0}%'", CRAgingReport.Location);
                                if (!CRAgingReport.PayerName.IsNull())
                                    oString += string.Format(" and insurangeplantable.PlanName like '%{0}%'", CRAgingReport.PayerName);
                                if (!CRAgingReport.PatientName.IsNull())
                                    oString += string.Format(" and patienttable.FirstName like '%{0}%'", CRAgingReport.PatientName);
                                if (!CRAgingReport.PatientName.IsNull())
                                    oString += string.Format(" and patienttable.LastName like '%{0}%'", CRAgingReport.PatientName);
                                if (!CRAgingReport.PatientAccountNumber.IsNull())
                                    oString += string.Format(" and patienttable.AccountNum ='{0}'", CRAgingReport.PatientAccountNumber);


                                if (CRAgingReport.EntryDateFrom != null && CRAgingReport.EntryDateTo != null)
                                {
                                    oString += (" and (chargetable.AddedDate  >= '" + CRAgingReport.EntryDateFrom.GetValueOrDefault().Date + "' and chargetable.AddedDate  < '" + CRAgingReport.EntryDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                                }
                                else if (CRAgingReport.EntryDateFrom != null && CRAgingReport.EntryDateTo == null)
                                {
                                    oString += (" and ( chargetable.AddedDate  >= '" + CRAgingReport.EntryDateFrom.GetValueOrDefault().Date + "')");

                                }
                                else if (CRAgingReport.EntryDateFrom == null && CRAgingReport.EntryDateTo != null)
                                {
                                    oString += (" and (chargetable.AddedDate  <= '" + CRAgingReport.EntryDateTo.GetValueOrDefault().Date + "')");
                                }


                                if (CRAgingReport.SubmittedDateFrom != null && CRAgingReport.SubmittedDateTo != null)
                                {
                                    oString += (" and (chargetable.SubmittetdDate  >= '" + CRAgingReport.SubmittedDateFrom.GetValueOrDefault().Date + "' and chargetable.SubmittetdDate  < '" + CRAgingReport.SubmittedDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                                }
                                else if (CRAgingReport.SubmittedDateFrom != null && CRAgingReport.SubmittedDateTo == null)
                                {
                                    oString += (" and ( chargetable.SubmittetdDate  >= '" + CRAgingReport.SubmittedDateFrom.GetValueOrDefault().Date + "')");

                                }
                                else if (CRAgingReport.SubmittedDateFrom == null && CRAgingReport.SubmittedDateTo != null)
                                {
                                    oString += (" and (chargetable.SubmittetdDate  <= '" + CRAgingReport.SubmittedDateTo.GetValueOrDefault().Date + "')");
                                }

                                oString += " GROUP BY case when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )>= 0 then 1 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 30 then 2 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 60 then 3 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 90 then 4 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 120 then 5 else 0    end , insurangeplantable.PlanName  having ( SUM(			case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +			 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 			 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 			) >  0) ";
                                //having (SUM(ISNULL(chargetable.PrimaryBal, 0) + ISNULL(chargetable.SecondaryBal, 0) + ISNULL(chargetable.TertiaryBal, 0)) >  0)
                                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                                myConnection.Open();
                                using (SqlDataReader oReader = oCmd.ExecuteReader())
                                {
                                    while (oReader.Read())
                                    {
                                        var aging = new AgingReportTemplate2();
                                        aging.PlanName = oReader["payerName"].ToString();
                                        aging.charges = Convert.ToDecimal(oReader["chargeAmount"].ToString());
                                        //  aging.claimAges = gp.Select(a => a.claimAge).ToList();
                                        aging.claimAge = Convert.ToInt32(oReader["claimAge"].ToString());


                                        Data.Add(aging);
                                    }
                                    myConnection.Close();
                                }
                            }


                            List<GRAgingReport> finalData = new List<GRAgingReport>();
                            int currentObj = 0;

                            GRAgingReport firstRow = new GRAgingReport();
                            firstRow.PayerName = Data.ElementAt(0).PlanName;
                            finalData.Add(firstRow);
                            decimal TotalAmountFinal = 0;
                            foreach (AgingReportTemplate2 Temp in Data)
                            {

                                if (Temp.PlanName.Equals(finalData.ElementAt(currentObj).PayerName))
                                {
                                    List<int> ClaimAges = Temp.claimAges;
                                    decimal Charges = Temp.charges;
                                    decimal TotalChargeToBeAdded = 0;
                                    switch (Temp.claimAge)
                                    {

                                        case 1:
                                            TotalChargeToBeAdded = 0;
                                            //foreach (long Charge in Charges)
                                            //{
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).Current = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 2:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            finalData.ElementAt(currentObj).IsBetween30And60 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 3:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).IsBetween61And90 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;

                                            break;
                                        case 4:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            finalData.ElementAt(currentObj).IsBetween91And120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 5:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).IsGreaterThan120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;

                                            break;
                                    }
                                }
                                else
                                {
                                    if (finalData.ElementAt(currentObj).Current == null)
                                    {
                                        finalData.ElementAt(currentObj).Current = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween30And60 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween30And60 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween61And90 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween61And90 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween91And120 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween91And120 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsGreaterThan120 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsGreaterThan120 = 0;
                                    }
                                    TotalAmountFinal = 0;
                                    currentObj = currentObj + 1;
                                    GRAgingReport TempRow = new GRAgingReport();
                                    TempRow.PayerName = Temp.PlanName;

                                    List<int> ClaimAges = Temp.claimAges;
                                    decimal Charges = Temp.charges;
                                    decimal TotalChargeToBeAdded = 0;
                                    switch (Temp.claimAge)
                                    {

                                        case 1:
                                            TotalChargeToBeAdded = 0;
                                            // foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            TempRow.Current = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 2:
                                            TotalChargeToBeAdded = 0;
                                            //   foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            TempRow.IsBetween30And60 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 3:
                                            TotalChargeToBeAdded = 0;
                                            // foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            TempRow.IsBetween61And90 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;

                                            break;
                                        case 4:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            //   {
                                            TotalChargeToBeAdded += Charges;
                                            //   }
                                            TempRow.IsBetween91And120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 5:
                                            TotalChargeToBeAdded = 0;
                                            //   foreach (long Charge in Charges)
                                            //   {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            TempRow.IsGreaterThan120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                    }
                                    finalData.Add(TempRow);
                                }
                            }

                            if (finalData.ElementAt(finalData.Count - 1).Current == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).Current = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween30And60 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween30And60 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween61And90 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween61And90 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween91And120 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween91And120 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsGreaterThan120 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsGreaterThan120 = 0;
                            }

                            return finalData;

                        }
                        else
                        {

                            List<AgingReportTemplate2> Data = new List<AgingReportTemplate2>();
                            using (SqlConnection myConnection = new SqlConnection(newConnection))
                            {

                                string oString = "SELECT 	SUM( case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 	) 	as chargeAmount  , insurangeplantable.PlanName  as payerName ,  case  when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )>= 0 then 1 when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )> 30 then 2 when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )> 60 then 3 when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )> 90 then 4 when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )> 120 then 5 else 0 end as claimAge   FROM Charge chargetable join  Patient patienttable on chargetable.PatientID = patienttable.ID join Visit v on chargetable.VisitID = v.ID join Practice practicetable on chargetable.PracticeID = practicetable.ID join [Provider] providertable on chargetable.ProviderID =  providertable.ID join [Location] locationtable on chargetable.LocationID = locationtable.ID join PatientPlan patientplantable on chargetable.PrimaryPatientPlanID = patientplantable.ID join InsurancePlan insurangeplantable on patientplantable.InsurancePlanID = insurangeplantable.ID join Cpt cpt on chargetable.CPTID = cpt.ID left join  RefProvider refprovidertable on chargetable.RefProviderID = refprovidertable.ID left join PatientPlan secpatientplantable on chargetable.SecondaryPatientPlanID = secpatientplantable.ID left join PatientPlan terpatientplantable on chargetable.TertiaryPatientPlanID = terpatientplantable.ID left join InsurancePlan secinsuranceplantable on secpatientplantable.InsurancePlanID = secinsuranceplantable.ID left join InsurancePlan terinsuranceplantable on terpatientplantable.InsurancePlanID = terinsuranceplantable.ID  where chargetable.DateOfServiceFrom IS NOT NULL   ";

                                oString += "  and chargetable.practiceid = {0} ";
                                oString = string.Format(oString, PracticeId);

                                if (!CRAgingReport.PracticeName.IsNull())
                                    oString += string.Format(" and practicetable.Name like '%{0}%'", CRAgingReport.PracticeName);
                                if (!CRAgingReport.ProviderName.IsNull())
                                    oString += string.Format(" and providertable.Name like '%{0}%'", CRAgingReport.ProviderName);
                                if (!CRAgingReport.Location.IsNull())
                                    oString += string.Format(" and locationtable.Name like '%{0}%'", CRAgingReport.Location);
                                if (!CRAgingReport.PayerName.IsNull())
                                    oString += string.Format(" and insurangeplantable.PlanName like '%{0}%'", CRAgingReport.PayerName);
                                if (!CRAgingReport.PatientName.IsNull())
                                    oString += string.Format(" and patienttable.FirstName like '%{0}%'", CRAgingReport.PatientName);
                                if (!CRAgingReport.PatientName.IsNull())
                                    oString += string.Format(" and patienttable.LastName like '%{0}%'", CRAgingReport.PatientName);
                                if (!CRAgingReport.PatientAccountNumber.IsNull())
                                    oString += string.Format(" and patienttable.AccountNum ='{0}'", CRAgingReport.PatientAccountNumber);


                                if (CRAgingReport.EntryDateFrom != null && CRAgingReport.EntryDateTo != null)
                                {
                                    oString += (" and (chargetable.AddedDate  >= '" + CRAgingReport.EntryDateFrom.GetValueOrDefault().Date + "' and chargetable.AddedDate  < '" + CRAgingReport.EntryDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                                }
                                else if (CRAgingReport.EntryDateFrom != null && CRAgingReport.EntryDateTo == null)
                                {
                                    oString += (" and ( chargetable.AddedDate  >= '" + CRAgingReport.EntryDateFrom.GetValueOrDefault().Date + "')");

                                }
                                else if (CRAgingReport.EntryDateFrom == null && CRAgingReport.EntryDateTo != null)
                                {
                                    oString += (" and (chargetable.AddedDate  <= '" + CRAgingReport.EntryDateTo.GetValueOrDefault().Date + "')");
                                }


                                if (CRAgingReport.SubmittedDateFrom != null && CRAgingReport.SubmittedDateTo != null)
                                {
                                    oString += (" and (chargetable.SubmittetdDate  >= '" + CRAgingReport.SubmittedDateFrom.GetValueOrDefault().Date + "' and chargetable.SubmittetdDate  < '" + CRAgingReport.SubmittedDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                                }
                                else if (CRAgingReport.SubmittedDateFrom != null && CRAgingReport.SubmittedDateTo == null)
                                {
                                    oString += (" and ( chargetable.SubmittetdDate  >= '" + CRAgingReport.SubmittedDateFrom.GetValueOrDefault().Date + "')");

                                }
                                else if (CRAgingReport.SubmittedDateFrom == null && CRAgingReport.SubmittedDateTo != null)
                                {
                                    oString += (" and (chargetable.SubmittetdDate  <= '" + CRAgingReport.SubmittedDateTo.GetValueOrDefault().Date + "')");
                                }

                                oString += "GROUP BY case when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )>= 0 then 1 when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )> 30 then 2 when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )> 60 then 3 when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )> 90 then 4 when ISNULL(DATEDIFF(day, chargetable.DateOfServiceFrom, GETDATE()), 0 )> 120 then 5 else 0    end , insurangeplantable.PlanName  having ( SUM(	case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +			 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 		 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 	) >  0) ";
                                //having (SUM(ISNULL(chargetable.PrimaryBal, 0) + ISNULL(chargetable.SecondaryBal, 0) + ISNULL(chargetable.TertiaryBal, 0)) >  0)
                                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                                myConnection.Open();
                                using (SqlDataReader oReader = oCmd.ExecuteReader())
                                {
                                    while (oReader.Read())
                                    {
                                        var aging = new AgingReportTemplate2();
                                        aging.PlanName = oReader["payerName"].ToString();
                                        aging.charges = Convert.ToDecimal(oReader["chargeAmount"].ToString());
                                        //  aging.claimAges = gp.Select(a => a.claimAge).ToList();
                                        aging.claimAge = Convert.ToInt32(oReader["claimAge"].ToString());


                                        Data.Add(aging);
                                    }
                                    myConnection.Close();
                                }
                            }


                            List<GRAgingReport> finalData = new List<GRAgingReport>();
                            int currentObj = 0;

                            GRAgingReport firstRow = new GRAgingReport();
                            firstRow.PayerName = Data.ElementAt(0).PlanName;
                            finalData.Add(firstRow);
                            decimal TotalAmountFinal = 0;
                            foreach (AgingReportTemplate2 Temp in Data)
                            {

                                if (Temp.PlanName.Equals(finalData.ElementAt(currentObj).PayerName))
                                {
                                    List<int> ClaimAges = Temp.claimAges;
                                    decimal Charges = Temp.charges;
                                    decimal TotalChargeToBeAdded = 0;
                                    switch (Temp.claimAge)
                                    {

                                        case 1:
                                            TotalChargeToBeAdded = 0;
                                            //foreach (long Charge in Charges)
                                            //{
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).Current = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 2:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            finalData.ElementAt(currentObj).IsBetween30And60 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 3:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).IsBetween61And90 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;

                                            break;
                                        case 4:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            finalData.ElementAt(currentObj).IsBetween91And120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;
                                            break;
                                        case 5:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            finalData.ElementAt(currentObj).IsGreaterThan120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            finalData.ElementAt(currentObj).TotalBalance = TotalAmountFinal;

                                            break;
                                    }
                                }
                                else
                                {
                                    if (finalData.ElementAt(currentObj).Current == null)
                                    {
                                        finalData.ElementAt(currentObj).Current = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween30And60 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween30And60 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween61And90 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween61And90 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsBetween91And120 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsBetween91And120 = 0;
                                    }
                                    if (finalData.ElementAt(currentObj).IsGreaterThan120 == null)
                                    {
                                        finalData.ElementAt(currentObj).IsGreaterThan120 = 0;
                                    }
                                    TotalAmountFinal = 0;
                                    currentObj = currentObj + 1;
                                    GRAgingReport TempRow = new GRAgingReport();
                                    TempRow.PayerName = Temp.PlanName;

                                    List<int> ClaimAges = Temp.claimAges;
                                    decimal Charges = Temp.charges;
                                    decimal TotalChargeToBeAdded = 0;
                                    switch (Temp.claimAge)
                                    {

                                        case 1:
                                            TotalChargeToBeAdded = 0;
                                            // foreach (long Charge in Charges)
                                            //  {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            TempRow.Current = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 2:
                                            TotalChargeToBeAdded = 0;
                                            //   foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            // }
                                            TempRow.IsBetween30And60 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 3:
                                            TotalChargeToBeAdded = 0;
                                            // foreach (long Charge in Charges)
                                            // {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            TempRow.IsBetween61And90 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;

                                            break;
                                        case 4:
                                            TotalChargeToBeAdded = 0;
                                            //  foreach (long Charge in Charges)
                                            //   {
                                            TotalChargeToBeAdded += Charges;
                                            //   }
                                            TempRow.IsBetween91And120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                        case 5:
                                            TotalChargeToBeAdded = 0;
                                            //   foreach (long Charge in Charges)
                                            //   {
                                            TotalChargeToBeAdded += Charges;
                                            //  }
                                            TempRow.IsGreaterThan120 = TotalChargeToBeAdded;
                                            TotalAmountFinal += TotalChargeToBeAdded;
                                            TempRow.TotalBalance = TotalAmountFinal;
                                            break;
                                    }
                                    finalData.Add(TempRow);
                                }
                            }

                            if (finalData.ElementAt(finalData.Count - 1).Current == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).Current = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween30And60 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween30And60 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween61And90 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween61And90 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsBetween91And120 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsBetween91And120 = 0;
                            }
                            if (finalData.ElementAt(finalData.Count - 1).IsGreaterThan120 == null)
                            {
                                finalData.ElementAt(finalData.Count - 1).IsGreaterThan120 = 0;
                            }

                            return finalData;

                        }

                    }
                }

            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText(Path.Combine(_context.env.ContentRootPath, "Logs", "FirstAgingReport.txt"), ex.ToString());
                Debug.WriteLine(ex.Message + "   " + ex.StackTrace);
                return null;
            }

        }



        [Route("FindAgingDetailReportV1")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<GRAgingDetailReport>>> FindAgingDetailReportV1(CRAgingReport cRAgingDetailReport)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
           );
            return FindAgingDetailReportV1(cRAgingDetailReport, UD);
        }

        private List<GRAgingDetailReport> FindAgingDetailReportV1(CRAgingReport CRAgingReport, UserInfoData UD)
        {
            try
            {
                long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
                var Client = (from w in _contextMain.MainPractice
                              where w.ID == PracticeId
                              select w
                            ).FirstOrDefault();
                string contextName = _contextMain.MainClient.Find(Client.ClientID)?.ContextName;
                string newConnection = GetConnectionStringManager(contextName);


                if (CRAgingReport.SearchType == "ED")
                {

                    List<GRAgingDetailReport> Data = new List<GRAgingDetailReport>();
                    using (SqlConnection myConnection = new SqlConnection(newConnection))
                    {

                        string oString = " SELECT   isnull(icd1t.ICDCode, '') as dx1, isnull(icd2t.ICDCode, '') as dx2, isnull(icd3t.ICDCode, '') as dx3, isnull(icd4t.ICDCode, '') as dx4, patienttable.AccountNum as AccountNum, v.ID as VisitNO,chargetable.ID as ChargeNO,patienttable.LastName as LastName, patienttable.FirstName as FirstName, convert(varchar,chargetable.DateOfServiceFrom,101)  as DOS,cpt.CPTCode as CptCode, convert(varchar,chargetable.AddedDate,101)  as EntryDate,convert(varchar,patienttable.DOB,101)  as DOB, patientplantable.SubscriberId as SubscriberID , chargetable.SubmittetdDate lastSubDate,ISNULL(chargetable.PatientPaid,0) as PatientPayment, providertable.[Name] as [Provider], locationtable.[Name] as [Location],ISNULL(chargetable.SecondaryPaid,0) as SecInsurancePayment, ISNULL(chargetable.PrimaryPaid,0) as InsurancePayment,SUM( case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 	 ) as Balance  ,SUM(		ISNULL(chargetable.PrimaryPatientBal, 0) + 		ISNULL(chargetable.SecondaryPatientBal, 0) + 		ISNULL(chargetable.TertiaryPatientBal, 0)		) as PatientBalance  , SUM( case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 	) as TotalBalance  , case 	when ISNULL(chargetable.PrimaryBal, 0) > 0 then 'Primary'	when ISNULL(chargetable.SecondaryBal, 0) > 0 then 'Secondary'	when ISNULL(chargetable.TertiaryBal, 0) > 0 then 'Tertiary'    else ''  end as PayerType,case 	when ISNULL(chargetable.PrimaryBal, 0) > 0 then 'Primary'	when ISNULL(chargetable.SecondaryBal, 0) > 0 then 'Secondary'	when ISNULL(chargetable.TertiaryBal, 0) > 0 then 'Tertiary'    else ''  end as AgingPayer,case 	when chargetable.PrimaryBal > 0 then insurangeplantable.PlanName	when chargetable.SecondaryBal > 0 then secinsuranceplantable.PlanName	when chargetable.TertiaryBal > 0 then terinsuranceplantable.PlanName    else ''  end as PayerName,  case 	when refprovidertable.[Name] is not null then refprovidertable.[Name]    else ''  end as ReferringProvider, case  when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )>= 0 then ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 ) when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 30 then ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 ) when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 60 then ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 ) when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 90 then ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 ) when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 120 then ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 ) else 0 end as AgingDays  ,case  when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )>= 0 		then  		SUM(			case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +			 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 			 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 			) else 0 end as [Current],case  when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 30 then  SUM(	case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end ) else 0 end as IsBetween30And60, case  when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 60 then  SUM(case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end ) else 0 end as IsBetween61And90, case  when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 90 then  SUM(case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 	 ) else 0 end as IsBetween91And120, case  when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 120 then  SUM(	 case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 	 ) else 0 end as IsGreaterThan120  FROM Charge chargetable join  Patient patienttable on chargetable.PatientID = patienttable.ID join Visit v on chargetable.VisitID = v.ID join Practice practicetable on chargetable.PracticeID = practicetable.ID join [Provider] providertable on chargetable.ProviderID =  providertable.ID join [Location] locationtable on chargetable.LocationID = locationtable.ID join PatientPlan patientplantable on chargetable.PrimaryPatientPlanID = patientplantable.ID join InsurancePlan insurangeplantable on patientplantable.InsurancePlanID = insurangeplantable.ID  join Cpt cpt on chargetable.CPTID = cpt.ID left join  RefProvider refprovidertable on chargetable.RefProviderID = refprovidertable.ID left join PatientPlan secpatientplantable on chargetable.SecondaryPatientPlanID = secpatientplantable.ID left join PatientPlan terpatientplantable on chargetable.TertiaryPatientPlanID = terpatientplantable.ID left join InsurancePlan secinsuranceplantable on secpatientplantable.InsurancePlanID = secinsuranceplantable.ID left join InsurancePlan terinsuranceplantable on terpatientplantable.InsurancePlanID = terinsuranceplantable.ID   left join ICD icd1t on v.ICD1ID = icd1t.ID left join ICD icd2t on v.ICD2ID = icd2t.ID left join ICD icd3t on v.ICD3ID = icd3t.ID  left join ICD icd4t on v.ICD4ID = icd4t.ID where chargetable.AddedDate IS NOT NULL  ";

                        oString += "  and chargetable.practiceid = {0} ";
                        oString = string.Format(oString, PracticeId);

                        if (!CRAgingReport.PracticeName.IsNull())
                            oString += string.Format(" and practicetable.Name like '%{0}%'", CRAgingReport.PracticeName);
                        if (!CRAgingReport.ProviderName.IsNull())
                            oString += string.Format(" and providertable.Name like '%{0}%'", CRAgingReport.ProviderName);
                        if (!CRAgingReport.Location.IsNull())
                            oString += string.Format(" and locationtable.Name like '%{0}%'", CRAgingReport.Location);
                        if (!CRAgingReport.PayerName.IsNull())
                            oString += string.Format(" and insurangeplantable.PlanName like '%{0}%'", CRAgingReport.PayerName);
                        if (!CRAgingReport.PatientName.IsNull())
                            oString += string.Format(" and patienttable.FirstName like '%{0}%'", CRAgingReport.PatientName);
                        if (!CRAgingReport.PatientName.IsNull())
                            oString += string.Format(" and patienttable.LastName like '%{0}%'", CRAgingReport.PatientName);
                        if (!CRAgingReport.PatientAccountNumber.IsNull())
                            oString += string.Format(" and patienttable.AccountNum ='{0}'", CRAgingReport.PatientAccountNumber);


                        if (CRAgingReport.EntryDateFrom != null && CRAgingReport.EntryDateTo != null)
                        {
                            oString += (" and (chargetable.AddedDate  >= '" + CRAgingReport.EntryDateFrom.GetValueOrDefault().Date + "' and chargetable.AddedDate  < '" + CRAgingReport.EntryDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                        }
                        else if (CRAgingReport.EntryDateFrom != null && CRAgingReport.EntryDateTo == null)
                        {
                            oString += (" and ( chargetable.AddedDate  >= '" + CRAgingReport.EntryDateFrom.GetValueOrDefault().Date + "')");

                        }
                        else if (CRAgingReport.EntryDateFrom == null && CRAgingReport.EntryDateTo != null)
                        {
                            oString += (" and (chargetable.AddedDate  <= '" + CRAgingReport.EntryDateTo.GetValueOrDefault().Date + "')");
                        }


                        if (CRAgingReport.SubmittedDateFrom != null && CRAgingReport.SubmittedDateTo != null)
                        {
                            oString += (" and (chargetable.SubmittetdDate  >= '" + CRAgingReport.SubmittedDateFrom.GetValueOrDefault().Date + "' and chargetable.SubmittetdDate  < '" + CRAgingReport.SubmittedDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                        }
                        else if (CRAgingReport.SubmittedDateFrom != null && CRAgingReport.SubmittedDateTo == null)
                        {
                            oString += (" and ( chargetable.SubmittetdDate  >= '" + CRAgingReport.SubmittedDateFrom.GetValueOrDefault().Date + "')");

                        }
                        else if (CRAgingReport.SubmittedDateFrom == null && CRAgingReport.SubmittedDateTo != null)
                        {
                            oString += (" and (chargetable.SubmittetdDate  <= '" + CRAgingReport.SubmittedDateTo.GetValueOrDefault().Date + "')");
                        }

                        oString += "GROUP BY icd1t.ICDCode, icd2t.ICDCode,icd3t.ICDCode, icd4t.ICDCode, patienttable.AccountNum,v.ID,chargetable.ID,patienttable.LastName , patienttable.FirstName ,  convert(varchar,chargetable.DateOfServiceFrom,101),cpt.CPTCode,convert(varchar,chargetable.AddedDate,101), convert(varchar,patienttable.DOB,101),chargetable.AddedDate, patientplantable.SubscriberId, chargetable.SubmittetdDate,ISNULL(chargetable.PatientPaid,0), ISNULL(chargetable.SecondaryPaid,0) , ISNULL(chargetable.PrimaryPaid,0) ,providertable.[Name],locationtable.[Name],  case 	when ISNULL(chargetable.PrimaryBal, 0) > 0 then 'Primary'	when ISNULL(chargetable.SecondaryBal, 0) > 0 then 'Secondary'	when ISNULL(chargetable.TertiaryBal, 0) > 0 then 'Tertiary'    else ''  end , case 	when chargetable.PrimaryBal > 0 then insurangeplantable.PlanName	when chargetable.SecondaryBal > 0 then secinsuranceplantable.PlanName 	when chargetable.TertiaryBal > 0 then terinsuranceplantable.PlanName     else ''   end ,     case  	when refprovidertable.[Name] is not null then refprovidertable.[Name]     else ''   end , case when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )>= 0 then 1 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 30 then 2 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 60 then 3 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 90 then 4 when ISNULL(DATEDIFF(day, chargetable.AddedDate, GETDATE()), 0 )> 120 then 5 else 0    end   having ( SUM( case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  + case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end ) >  0) ";
                        //having (SUM(ISNULL(chargetable.PrimaryBal, 0) + ISNULL(chargetable.SecondaryBal, 0) + ISNULL(chargetable.TertiaryBal, 0)) >  0)
                        SqlCommand oCmd = new SqlCommand(oString, myConnection);
                        myConnection.Open();
                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            while (oReader.Read())
                            {
                                var aging = new GRAgingDetailReport();
                                //aging.PlanName = oReader["payerName"].ToString();
                                //aging.charges = Convert.ToDecimal(oReader["chargeAmount"].ToString());
                                ////  aging.claimAges = gp.Select(a => a.claimAge).ToList();
                                //aging.claimAge = Convert.ToInt32(oReader["claimAge"].ToString());

                                aging.PayerName = oReader["payerName"].ToString();
                                aging.PayerType = oReader["PayerType"].ToString();
                                aging.AccountNum = oReader["AccountNum"].ToString();
                                aging.VisitNO = Convert.ToInt64(oReader["VisitNO"].ToString());
                                aging.ChargeNO = Convert.ToInt64(oReader["ChargeNO"].ToString());
                                aging.PatientName = oReader["LastName"].ToString() + ", " + oReader["FirstName"].ToString();
                                aging.DOS = oReader["DOS"].ToString();
                                aging.CptCode = oReader["CptCode"].ToString();
                                aging.Current = Convert.ToDecimal(oReader["Current"].ToString());
                                aging.IsBetween30And60 = Convert.ToDecimal(oReader["IsBetween30And60"].ToString());
                                aging.IsBetween61And90 = Convert.ToDecimal(oReader["IsBetween61And90"].ToString());
                                aging.IsBetween91And120 = Convert.ToDecimal(oReader["IsBetween91And120"].ToString());
                                aging.IsGreaterThan120 = Convert.ToDecimal(oReader["IsGreaterThan120"].ToString());
                                aging.AgingDays = Convert.ToInt64(oReader["AgingDays"].ToString());
                                aging.TotalBalance = Convert.ToDecimal(oReader["TotalBalance"].ToString());
                                aging.EntryDate = oReader["EntryDate"].ToString();
                                aging.lastSubDate = oReader["lastSubDate"].ToString();
                                aging.Provider = oReader["Provider"].ToString();
                                aging.Location = oReader["Location"].ToString();
                                aging.ReferringProvider = oReader["ReferringProvider"].ToString();
                                aging.AgingPayer = oReader["AgingPayer"].ToString();
                                aging.InsurancePayment = Convert.ToDecimal(oReader["InsurancePayment"].ToString());
                                aging.SecInsurancePayment = Convert.ToDecimal(oReader["SecInsurancePayment"].ToString());
                                aging.PatientPayment = Convert.ToDecimal(oReader["PatientPayment"].ToString());
                                aging.PatientBalance = Convert.ToDecimal(oReader["PatientBalance"].ToString());
                                aging.Balance = Convert.ToDecimal(oReader["Balance"].ToString());
                                aging.SubscriberID = oReader["SubscriberID"].ToString();
                                aging.DOB = oReader["DOB"].ToString();
                                // DOS = visittable.DateOfServiceFrom.Format("MM/dd/yyyy"),
                                //-- Status = TranslateStatus(chargeStatus),
                                //   EntryDate = chargetable.AddedDate.Format("MM/dd/yyyy"),
                                //  lastSubDate = chargetable.SubmittetdDate.HasValue ? chargetable.SubmittetdDate.Value.Format("MM/dd/yyyy") : "",

                                aging.dx1 = oReader["dx1"].ToString();
                                aging.dx2 = oReader["dx2"].ToString();
                                aging.dx3 = oReader["dx3"].ToString();
                                aging.dx4 = oReader["dx4"].ToString();

                                Data.Add(aging);
                            }
                            myConnection.Close();
                        }
                    }

                    return Data;

                }
                else if (CRAgingReport.SearchType == "SD")
                {

                    List<GRAgingDetailReport> Data = new List<GRAgingDetailReport>();
                    using (SqlConnection myConnection = new SqlConnection(newConnection))
                    {

                        string oString = "SELECT  isnull(icd1t.ICDCode, '') as dx1, isnull(icd2t.ICDCode, '') as dx2, isnull(icd3t.ICDCode, '') as dx3, isnull(icd4t.ICDCode, '') as dx4,patienttable.AccountNum as AccountNum, v.ID as VisitNO,chargetable.ID as ChargeNO,patienttable.LastName as LastName, patienttable.FirstName as FirstName, chargetable.DateOfServiceFrom as DOS,cpt.CPTCode as CptCode, convert(varchar,chargetable.AddedDate,101)  as EntryDate, patienttable.DOB as DOB, patientplantable.SubscriberId as SubscriberID , chargetable.SubmittetdDate lastSubDate,ISNULL(chargetable.PatientPaid,0) as PatientPayment, providertable.[Name] as [Provider], locationtable.[Name] as [Location],ISNULL(chargetable.SecondaryPaid,0) as SecInsurancePayment, ISNULL(chargetable.PrimaryPaid,0) as InsurancePayment,SUM( case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 	 ) as Balance  ,SUM(		ISNULL(chargetable.PrimaryPatientBal, 0) + 		ISNULL(chargetable.SecondaryPatientBal, 0) + 		ISNULL(chargetable.TertiaryPatientBal, 0)		) as PatientBalance  , SUM( case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 	) as TotalBalance  , case 	when ISNULL(chargetable.PrimaryBal, 0) > 0 then 'Primary'	when ISNULL(chargetable.SecondaryBal, 0) > 0 then 'Secondary'	when ISNULL(chargetable.TertiaryBal, 0) > 0 then 'Tertiary'    else ''  end as PayerType,case 	when ISNULL(chargetable.PrimaryBal, 0) > 0 then 'Primary'	when ISNULL(chargetable.SecondaryBal, 0) > 0 then 'Secondary'	when ISNULL(chargetable.TertiaryBal, 0) > 0 then 'Tertiary'    else ''  end as AgingPayer,case 	when chargetable.PrimaryBal > 0 then insurangeplantable.PlanName	when chargetable.SecondaryBal > 0 then secinsuranceplantable.PlanName	when chargetable.TertiaryBal > 0 then terinsuranceplantable.PlanName    else ''  end as PayerName,  case 	when refprovidertable.[Name] is not null then refprovidertable.[Name]    else ''  end as ReferringProvider, case  when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )>= 0 then ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 ) when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 30 then ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 ) when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 60 then ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 ) when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 90 then ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 ) when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 120 then ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 ) else 0 end as AgingDays  ,case  when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )>= 0 		then  		SUM(			case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +			 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 			 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 			) else 0 end as [Current],case  when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 30 then  SUM(	case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end ) else 0 end as IsBetween30And60, case  when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 60 then  SUM(case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end ) else 0 end as IsBetween61And90, case  when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 90 then  SUM(case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 	 ) else 0 end as IsBetween91And120, case  when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 120 then  SUM(	 case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 	 ) else 0 end as IsGreaterThan120  FROM Charge chargetable join  Patient patienttable on chargetable.PatientID = patienttable.ID join Visit v on chargetable.VisitID = v.ID join Practice practicetable on chargetable.PracticeID = practicetable.ID join [Provider] providertable on chargetable.ProviderID =  providertable.ID join [Location] locationtable on chargetable.LocationID = locationtable.ID join PatientPlan patientplantable on chargetable.PrimaryPatientPlanID = patientplantable.ID join InsurancePlan insurangeplantable on patientplantable.InsurancePlanID = insurangeplantable.ID  join Cpt cpt on chargetable.CPTID = cpt.ID left join  RefProvider refprovidertable on chargetable.RefProviderID = refprovidertable.ID left join PatientPlan secpatientplantable on chargetable.SecondaryPatientPlanID = secpatientplantable.ID left join PatientPlan terpatientplantable on chargetable.TertiaryPatientPlanID = terpatientplantable.ID left join InsurancePlan secinsuranceplantable on secpatientplantable.InsurancePlanID = secinsuranceplantable.ID left join InsurancePlan terinsuranceplantable on terpatientplantable.InsurancePlanID = terinsuranceplantable.ID  left join ICD icd1t on v.ICD1ID = icd1t.ID left join ICD icd2t on v.ICD2ID = icd2t.ID left join ICD icd3t on v.ICD3ID = icd3t.ID  left join ICD icd4t on v.ICD4ID = icd4t.ID where chargetable.SubmittetdDate IS NOT NULL    ";

                        oString += "  and chargetable.practiceid = {0} ";
                        oString = string.Format(oString, PracticeId);

                        if (!CRAgingReport.PracticeName.IsNull())
                            oString += string.Format(" and practicetable.Name like '%{0}%'", CRAgingReport.PracticeName);
                        if (!CRAgingReport.ProviderName.IsNull())
                            oString += string.Format(" and providertable.Name like '%{0}%'", CRAgingReport.ProviderName);
                        if (!CRAgingReport.Location.IsNull())
                            oString += string.Format(" and locationtable.Name like '%{0}%'", CRAgingReport.Location);
                        if (!CRAgingReport.PayerName.IsNull())
                            oString += string.Format(" and insurangeplantable.PlanName like '%{0}%'", CRAgingReport.PayerName);
                        if (!CRAgingReport.PatientName.IsNull())
                            oString += string.Format(" and patienttable.FirstName like '%{0}%'", CRAgingReport.PatientName);
                        if (!CRAgingReport.PatientName.IsNull())
                            oString += string.Format(" and patienttable.LastName like '%{0}%'", CRAgingReport.PatientName);
                        if (!CRAgingReport.PatientAccountNumber.IsNull())
                            oString += string.Format(" and patienttable.AccountNum ='{0}'", CRAgingReport.PatientAccountNumber);


                        if (CRAgingReport.EntryDateFrom != null && CRAgingReport.EntryDateTo != null)
                        {
                            oString += (" and (chargetable.AddedDate  >= '" + CRAgingReport.EntryDateFrom.GetValueOrDefault().Date + "' and chargetable.AddedDate  < '" + CRAgingReport.EntryDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                        }
                        else if (CRAgingReport.EntryDateFrom != null && CRAgingReport.EntryDateTo == null)
                        {
                            oString += (" and ( chargetable.AddedDate  >= '" + CRAgingReport.EntryDateFrom.GetValueOrDefault().Date + "')");

                        }
                        else if (CRAgingReport.EntryDateFrom == null && CRAgingReport.EntryDateTo != null)
                        {
                            oString += (" and (chargetable.AddedDate  <= '" + CRAgingReport.EntryDateTo.GetValueOrDefault().Date + "')");
                        }


                        if (CRAgingReport.SubmittedDateFrom != null && CRAgingReport.SubmittedDateTo != null)
                        {
                            oString += (" and (chargetable.SubmittetdDate  >= '" + CRAgingReport.SubmittedDateFrom.GetValueOrDefault().Date + "' and chargetable.SubmittetdDate  < '" + CRAgingReport.SubmittedDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                        }
                        else if (CRAgingReport.SubmittedDateFrom != null && CRAgingReport.SubmittedDateTo == null)
                        {
                            oString += (" and ( chargetable.SubmittetdDate  >= '" + CRAgingReport.SubmittedDateFrom.GetValueOrDefault().Date + "')");

                        }
                        else if (CRAgingReport.SubmittedDateFrom == null && CRAgingReport.SubmittedDateTo != null)
                        {
                            oString += (" and (chargetable.SubmittetdDate  <= '" + CRAgingReport.SubmittedDateTo.GetValueOrDefault().Date + "')");
                        }

                        oString += "GROUP BY icd1t.ICDCode, icd2t.ICDCode,icd3t.ICDCode, icd4t.ICDCode, patienttable.AccountNum,v.ID,chargetable.ID,patienttable.LastName , patienttable.FirstName , chargetable.DateOfServiceFrom,cpt.CPTCode,convert(varchar,chargetable.AddedDate,101) , patienttable.DOB,chargetable.AddedDate, patientplantable.SubscriberId, chargetable.SubmittetdDate,ISNULL(chargetable.PatientPaid,0), ISNULL(chargetable.SecondaryPaid,0) , ISNULL(chargetable.PrimaryPaid,0) ,providertable.[Name],locationtable.[Name],  case 	when ISNULL(chargetable.PrimaryBal, 0) > 0 then 'Primary'	when ISNULL(chargetable.SecondaryBal, 0) > 0 then 'Secondary'	when ISNULL(chargetable.TertiaryBal, 0) > 0 then 'Tertiary'    else ''  end , case 	when chargetable.PrimaryBal > 0 then insurangeplantable.PlanName	when chargetable.SecondaryBal > 0 then secinsuranceplantable.PlanName 	when chargetable.TertiaryBal > 0 then terinsuranceplantable.PlanName     else ''   end ,     case  	when refprovidertable.[Name] is not null then refprovidertable.[Name]     else ''   end , case when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )>= 0 then 1 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 30 then 2 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 60 then 3 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 90 then 4 when ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 120 then 5 else 0    end  having ( SUM( case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  + case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end ) >  0)  ";
                        //having (SUM(ISNULL(chargetable.PrimaryBal, 0) + ISNULL(chargetable.SecondaryBal, 0) + ISNULL(chargetable.TertiaryBal, 0)) >  0)

                        SqlCommand oCmd = new SqlCommand(oString, myConnection);
                        myConnection.Open();
                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            while (oReader.Read())
                            {
                                var aging = new GRAgingDetailReport();
                                //aging.PlanName = oReader["payerName"].ToString();
                                //aging.charges = Convert.ToDecimal(oReader["chargeAmount"].ToString());
                                ////  aging.claimAges = gp.Select(a => a.claimAge).ToList();
                                //aging.claimAge = Convert.ToInt32(oReader["claimAge"].ToString());

                                aging.PayerName = oReader["payerName"].ToString();
                                aging.PayerType = oReader["PayerType"].ToString();
                                aging.AccountNum = oReader["AccountNum"].ToString();
                                aging.VisitNO = Convert.ToInt64(oReader["VisitNO"].ToString());
                                aging.ChargeNO = Convert.ToInt64(oReader["ChargeNO"].ToString());
                                aging.PatientName = oReader["LastName"].ToString() + ", " + oReader["FirstName"].ToString();
                                aging.DOS = oReader["DOS"].ToString();
                                aging.CptCode = oReader["CptCode"].ToString();
                                aging.Current = Convert.ToDecimal(oReader["Current"].ToString());
                                aging.IsBetween30And60 = Convert.ToDecimal(oReader["IsBetween30And60"].ToString());
                                aging.IsBetween61And90 = Convert.ToDecimal(oReader["IsBetween61And90"].ToString());
                                aging.IsBetween91And120 = Convert.ToDecimal(oReader["IsBetween91And120"].ToString());
                                aging.IsGreaterThan120 = Convert.ToDecimal(oReader["IsGreaterThan120"].ToString());
                                aging.AgingDays = Convert.ToInt64(oReader["AgingDays"].ToString());
                                aging.TotalBalance = Convert.ToDecimal(oReader["TotalBalance"].ToString());
                                aging.EntryDate = oReader["EntryDate"].ToString();
                                aging.lastSubDate = oReader["lastSubDate"].ToString();
                                aging.Provider = oReader["Provider"].ToString();
                                aging.Location = oReader["Location"].ToString();
                                aging.ReferringProvider = oReader["ReferringProvider"].ToString();
                                aging.AgingPayer = oReader["AgingPayer"].ToString();
                                aging.InsurancePayment = Convert.ToDecimal(oReader["InsurancePayment"].ToString());
                                aging.SecInsurancePayment = Convert.ToDecimal(oReader["SecInsurancePayment"].ToString());
                                aging.PatientPayment = Convert.ToDecimal(oReader["PatientPayment"].ToString());
                                aging.PatientBalance = Convert.ToDecimal(oReader["PatientBalance"].ToString());
                                aging.Balance = Convert.ToDecimal(oReader["Balance"].ToString());
                                aging.SubscriberID = oReader["SubscriberID"].ToString();
                                aging.DOB = oReader["DOB"].ToString();
                                // DOS = visittable.DateOfServiceFrom.Format("MM/dd/yyyy"),
                                //-- Status = TranslateStatus(chargeStatus),
                                //   EntryDate = chargetable.AddedDate.Format("MM/dd/yyyy"),
                                //  lastSubDate = chargetable.SubmittetdDate.HasValue ? chargetable.SubmittetdDate.Value.Format("MM/dd/yyyy") : "",

                                aging.dx1 = oReader["dx1"].ToString();
                                aging.dx2 = oReader["dx2"].ToString();
                                aging.dx3 = oReader["dx3"].ToString();
                                aging.dx4 = oReader["dx4"].ToString();

                                Data.Add(aging);
                            }
                            myConnection.Close();
                        }
                    }

                    return Data;

                }
                else
                {

                    List<GRAgingDetailReport> Data = new List<GRAgingDetailReport>();
                    using (SqlConnection myConnection = new SqlConnection(newConnection))
                    {

                        string oString = "SELECT  isnull(icd1t.ICDCode, '') as dx1, isnull(icd2t.ICDCode, '') as dx2, isnull(icd3t.ICDCode, '') as dx3, isnull(icd4t.ICDCode, '') as dx4,patienttable.AccountNum as AccountNum, v.ID as VisitNO,chargetable.ID as ChargeNO, patienttable.LastName as LastName, patienttable.FirstName as FirstName, chargetable.DateOfServiceFrom as DOS,cpt.CPTCode as CptCode, chargetable.AddedDate as EntryDate, patienttable.DOB as DOB, patientplantable.SubscriberId as SubscriberID , chargetable.SubmittetdDate lastSubDate,ISNULL(chargetable.PatientPaid,0) as PatientPayment, providertable.[Name] as [Provider], locationtable.[Name] as [Location],ISNULL(chargetable.SecondaryPaid,0) as SecInsurancePayment, ISNULL(chargetable.PrimaryPaid,0) as InsurancePayment,SUM( case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  + case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 	 ) as Balance  , SUM(		ISNULL(chargetable.PrimaryPatientBal, 0) + 		ISNULL(chargetable.SecondaryPatientBal, 0) + 		ISNULL(chargetable.TertiaryPatientBal, 0)		) as PatientBalance  , SUM( case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 	) as TotalBalance  , case 	when ISNULL(chargetable.PrimaryBal, 0) > 0 then 'Primary'	when ISNULL(chargetable.SecondaryBal, 0) > 0 then 'Secondary'	when ISNULL(chargetable.TertiaryBal, 0) > 0 then 'Tertiary'    else ''  end as PayerType,case 	when ISNULL(chargetable.PrimaryBal, 0) > 0 then 'Primary'	when ISNULL(chargetable.SecondaryBal, 0) > 0 then 'Secondary'	when ISNULL(chargetable.TertiaryBal, 0) > 0 then 'Tertiary'    else ''  end as AgingPayer,case 	when chargetable.PrimaryBal > 0 then insurangeplantable.PlanName	when chargetable.SecondaryBal > 0 then secinsuranceplantable.PlanName	when chargetable.TertiaryBal > 0 then terinsuranceplantable.PlanName    else ''  end as PayerName,  case 	when refprovidertable.[Name] is not null then refprovidertable.[Name]    else ''  end as ReferringProvider, case  when ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 )>= 0 then ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 ) when ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 )> 30 then ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 ) when ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 )> 60 then ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 ) when ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 )> 90 then ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 ) when ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 )> 120 then ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 ) else 0 end as AgingDays  ,case  when ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 )>= 0 		then  		SUM(			case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +			 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 			 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 			) else 0 end as [Current],case  when ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 )> 30 then  SUM(	case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end ) else 0 end as IsBetween30And60, case  when ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 )> 60 then  SUM(case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end ) else 0 end as IsBetween61And90, case  when ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 )> 90 then  SUM( case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 	 ) else 0 end as IsBetween91And120, case  when ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 )> 120 then  SUM(	 case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +	 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 	 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 	 ) else 0 end as IsGreaterThan120  FROM Charge chargetable join  Patient patienttable on chargetable.PatientID = patienttable.ID join Visit v on chargetable.VisitID = v.ID join Practice practicetable on chargetable.PracticeID = practicetable.ID join [Provider] providertable on chargetable.ProviderID =  providertable.ID join [Location] locationtable on chargetable.LocationID = locationtable.ID join PatientPlan patientplantable on chargetable.PrimaryPatientPlanID = patientplantable.ID join InsurancePlan insurangeplantable on patientplantable.InsurancePlanID = insurangeplantable.ID  join Cpt cpt on chargetable.CPTID = cpt.ID left join  RefProvider refprovidertable on chargetable.RefProviderID = refprovidertable.ID left join PatientPlan secpatientplantable on chargetable.SecondaryPatientPlanID = secpatientplantable.ID left join PatientPlan terpatientplantable on chargetable.TertiaryPatientPlanID = terpatientplantable.ID left join InsurancePlan secinsuranceplantable on secpatientplantable.InsurancePlanID = secinsuranceplantable.ID left join InsurancePlan terinsuranceplantable on terpatientplantable.InsurancePlanID = terinsuranceplantable.ID  left join ICD icd1t on v.ICD1ID = icd1t.ID left join ICD icd2t on v.ICD2ID = icd2t.ID left join ICD icd3t on v.ICD3ID = icd3t.ID  left join ICD icd4t on v.ICD4ID = icd4t.ID where chargetable.dateofservicefrom IS NOT NULL    ";

                        oString += "  and chargetable.practiceid = {0} ";
                        oString = string.Format(oString, PracticeId);

                        if (!CRAgingReport.PracticeName.IsNull())
                            oString += string.Format(" and practicetable.Name like '%{0}%'", CRAgingReport.PracticeName);
                        if (!CRAgingReport.ProviderName.IsNull())
                            oString += string.Format(" and providertable.Name like '%{0}%'", CRAgingReport.ProviderName);
                        if (!CRAgingReport.Location.IsNull())
                            oString += string.Format(" and locationtable.Name like '%{0}%'", CRAgingReport.Location);
                        if (!CRAgingReport.PayerName.IsNull())
                            oString += string.Format(" and insurangeplantable.PlanName like '%{0}%'", CRAgingReport.PayerName);
                        if (!CRAgingReport.PatientName.IsNull())
                            oString += string.Format(" and patienttable.FirstName like '%{0}%'", CRAgingReport.PatientName);
                        if (!CRAgingReport.PatientName.IsNull())
                            oString += string.Format(" and patienttable.LastName like '%{0}%'", CRAgingReport.PatientName);
                        if (!CRAgingReport.PatientAccountNumber.IsNull())
                            oString += string.Format(" and patienttable.AccountNum ='{0}'", CRAgingReport.PatientAccountNumber);


                        if (CRAgingReport.EntryDateFrom != null && CRAgingReport.EntryDateTo != null)
                        {
                            oString += (" and (chargetable.AddedDate  >= '" + CRAgingReport.EntryDateFrom.GetValueOrDefault().Date + "' and chargetable.AddedDate  < '" + CRAgingReport.EntryDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                        }
                        else if (CRAgingReport.EntryDateFrom != null && CRAgingReport.EntryDateTo == null)
                        {
                            oString += (" and ( chargetable.AddedDate  >= '" + CRAgingReport.EntryDateFrom.GetValueOrDefault().Date + "')");

                        }
                        else if (CRAgingReport.EntryDateFrom == null && CRAgingReport.EntryDateTo != null)
                        {
                            oString += (" and (chargetable.AddedDate  <= '" + CRAgingReport.EntryDateTo.GetValueOrDefault().Date + "')");
                        }


                        if (CRAgingReport.SubmittedDateFrom != null && CRAgingReport.SubmittedDateTo != null)
                        {
                            oString += (" and (chargetable.SubmittetdDate  >= '" + CRAgingReport.SubmittedDateFrom.GetValueOrDefault().Date + "' and chargetable.SubmittetdDate  < '" + CRAgingReport.SubmittedDateTo.GetValueOrDefault().Date.AddDays(1) + "')");
                        }
                        else if (CRAgingReport.SubmittedDateFrom != null && CRAgingReport.SubmittedDateTo == null)
                        {
                            oString += (" and ( chargetable.SubmittetdDate  >= '" + CRAgingReport.SubmittedDateFrom.GetValueOrDefault().Date + "')");

                        }
                        else if (CRAgingReport.SubmittedDateFrom == null && CRAgingReport.SubmittedDateTo != null)
                        {
                            oString += (" and (chargetable.SubmittetdDate  <= '" + CRAgingReport.SubmittedDateTo.GetValueOrDefault().Date + "')");
                        }

                        oString += "GROUP BY icd1t.ICDCode, icd2t.ICDCode,icd3t.ICDCode, icd4t.ICDCode, patienttable.AccountNum,v.ID,chargetable.ID,patienttable.LastName , patienttable.FirstName , chargetable.DateOfServiceFrom,cpt.CPTCode,chargetable.AddedDate, patienttable.DOB,chargetable.AddedDate, patientplantable.SubscriberId, chargetable.SubmittetdDate,ISNULL(chargetable.PatientPaid,0), ISNULL(chargetable.SecondaryPaid,0) , ISNULL(chargetable.PrimaryPaid,0) ,providertable.[Name],locationtable.[Name],  case 	when ISNULL(chargetable.PrimaryBal, 0) > 0 then 'Primary'	when ISNULL(chargetable.SecondaryBal, 0) > 0 then 'Secondary'	when ISNULL(chargetable.TertiaryBal, 0) > 0 then 'Tertiary'    else ''  end , case 	when chargetable.PrimaryBal > 0 then insurangeplantable.PlanName	when chargetable.SecondaryBal > 0 then secinsuranceplantable.PlanName 	when chargetable.TertiaryBal > 0 then terinsuranceplantable.PlanName     else ''   end ,     case  	when refprovidertable.[Name] is not null then refprovidertable.[Name]     else ''   end , case when ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 )<= 30 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )>= 0 then 1 when ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 )<= 60 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 30 then 2 when ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 )<= 90 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 60 then 3 when ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 )<= 120 and ISNULL(DATEDIFF(day, chargetable.SubmittetdDate, GETDATE()), 0 )> 90 then 4 when ISNULL(DATEDIFF(day, chargetable.dateofservicefrom, GETDATE()), 0 )> 120 then 5 else 0    end  having ( SUM(	case when ISNULL(chargetable.PrimaryBal, 0) > 0 then ISNULL(chargetable.PrimaryBal, 0) else 0 end  +			 case when ISNULL(chargetable.SecondaryBal, 0) > 0 then ISNULL(chargetable.SecondaryBal, 0)else 0 end  + 		 case when ISNULL(chargetable.TertiaryBal, 0) > 0 then ISNULL(chargetable.TertiaryBal, 0)else 0 end 	) >  0)  ";
                        //having(SUM(ISNULL(chargetable.PrimaryBal, 0) + ISNULL(chargetable.SecondaryBal, 0) + ISNULL(chargetable.TertiaryBal, 0)) > 0)
                        SqlCommand oCmd = new SqlCommand(oString, myConnection);
                        myConnection.Open();
                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            while (oReader.Read())
                            {
                                var aging = new GRAgingDetailReport();
                                //aging.PlanName = oReader["payerName"].ToString();
                                //aging.charges = Convert.ToDecimal(oReader["chargeAmount"].ToString());
                                ////  aging.claimAges = gp.Select(a => a.claimAge).ToList();
                                //aging.claimAge = Convert.ToInt32(oReader["claimAge"].ToString());

                                aging.PayerName = oReader["payerName"].ToString();
                                aging.PayerType = oReader["PayerType"].ToString();
                                aging.AccountNum = oReader["AccountNum"].ToString();
                                aging.VisitNO = Convert.ToInt64(oReader["VisitNO"].ToString());
                                aging.ChargeNO = Convert.ToInt64(oReader["ChargeNO"].ToString());
                                aging.PatientName = oReader["LastName"].ToString() + ", " + oReader["FirstName"].ToString();
                                aging.DOS = oReader["DOS"].ToString();
                                aging.CptCode = oReader["CptCode"].ToString();
                                aging.Current = Convert.ToDecimal(oReader["Current"].ToString());
                                aging.IsBetween30And60 = Convert.ToDecimal(oReader["IsBetween30And60"].ToString());
                                aging.IsBetween61And90 = Convert.ToDecimal(oReader["IsBetween61And90"].ToString());
                                aging.IsBetween91And120 = Convert.ToDecimal(oReader["IsBetween91And120"].ToString());
                                aging.IsGreaterThan120 = Convert.ToDecimal(oReader["IsGreaterThan120"].ToString());
                                aging.AgingDays = Convert.ToInt64(oReader["AgingDays"].ToString());
                                aging.TotalBalance = Convert.ToDecimal(oReader["TotalBalance"].ToString());
                                aging.EntryDate = oReader["EntryDate"].ToString();
                                aging.lastSubDate = oReader["lastSubDate"].ToString();
                                aging.Provider = oReader["Provider"].ToString();
                                aging.Location = oReader["Location"].ToString();
                                aging.ReferringProvider = oReader["ReferringProvider"].ToString();
                                aging.AgingPayer = oReader["AgingPayer"].ToString();
                                aging.InsurancePayment = Convert.ToDecimal(oReader["InsurancePayment"].ToString());
                                aging.SecInsurancePayment = Convert.ToDecimal(oReader["SecInsurancePayment"].ToString());
                                aging.PatientPayment = Convert.ToDecimal(oReader["PatientPayment"].ToString());
                                aging.PatientBalance = Convert.ToDecimal(oReader["PatientBalance"].ToString());
                                aging.Balance = Convert.ToDecimal(oReader["Balance"].ToString());
                                aging.SubscriberID = oReader["SubscriberID"].ToString();
                                aging.DOB = oReader["DOB"].ToString();


                                aging.dx1 = oReader["dx1"].ToString();
                                aging.dx2 = oReader["dx2"].ToString();
                                aging.dx3 = oReader["dx3"].ToString();
                                aging.dx4 = oReader["dx4"].ToString();



                                // DOS = visittable.DateOfServiceFrom.Format("MM/dd/yyyy"),
                                //-- Status = TranslateStatus(chargeStatus),
                                //   EntryDate = chargetable.AddedDate.Format("MM/dd/yyyy"),
                                //  lastSubDate = chargetable.SubmittetdDate.HasValue ? chargetable.SubmittetdDate.Value.Format("MM/dd/yyyy") : "",



                                Data.Add(aging);
                            }
                            myConnection.Close();
                        }
                    }

                    return Data;

                }
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText(Path.Combine(_context.env.ContentRootPath, "Logs", "FirstAgingReport.txt"), ex.ToString());
                Debug.WriteLine(ex.Message + "   " + ex.StackTrace);
                return null;
            }

        }






        [HttpPost]
        [Route("ExportAgingDetail")]
        public async Task<IActionResult> ExportAgingDetail(CRAgingReport cRAgingDetailReport)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GRAgingDetailReport> data = FindAgingDetailReportV1(cRAgingDetailReport, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, cRAgingDetailReport, "Aging Detail Report");
        }

        [HttpPost]
        [Route("ExportAgingDetailPdf")]
        public async Task<IActionResult> ExportAgingDetailPdf(CRAgingReport cRAgingDetailReport)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GRAgingDetailReport> data = FindAgingDetailReportV1(cRAgingDetailReport, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);
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

            //if (Status == "K")
            //{
            //    desc = "999 Accepted";
            //}
            //if (Status == "D")
            //{
            //    desc = "999 Denied";
            //}
            //if (Status == "E")
            //{
            //    desc = "999 has Errors";
            //}
            if (Status == "P")
            {
                desc = "Paid";
            }
            if (Status == "PAT_T_PT")
            {
                desc = "PATIENT TO PLAN TRANSFER";
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
            //if (Status == "R")
            //{
            //    desc = "Rejected";
            //}
            if (Status == "A1AY")
            {
                desc = "Received By Clearing House";
            }
            //if (Status == "A0PR")
            //{
            //    desc = "Forwarded  to Payer";
            //}
            //if (Status == "A1PR")
            //{
            //    desc = "Received By Payer";
            //}
            //if (Status == "A2PR")
            //{
            //    desc = "Accepted By Payer";
            //}
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

            return desc;
        }

        public long DecimalToLong(decimal pf)
        {
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

        public bool CheckIsLessThan30(int days)
        {
            if (days < 30)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool CheckIsBetween30and60(int days)
        {
            if (days >= 30 && days <= 60)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool CheckIsBetween61and90(int days)
        {
            if (days >= 61 && days <= 90)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CheckIsBetween91and120(int days)
        {
            if (days >= 91 && days <= 120)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CheckIsGreaterThan120(int days)
        {
            if (days > 120)
            {
                return true;
            }
            else
            {
                return false;
            }
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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediFusionPM.Models;
using static MediFusionPM.ReportViewModels.RVMSecondAgingReport;

namespace MediFusionPM.ReportControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RSecondAgingController : ControllerBase
    {
        private readonly ClientDbContext _context;

        public RSecondAgingController(ClientDbContext context)
        {
            _context = context;

            // Only For Testing
            if (_context.Practice.Count() == 0)
            {
                //  _context.Facilities.Add(new Facility { Name = "ABC Facility", OrganizationName = "" });
                // _context.SaveChanges();
            }
        }
        

        [Route("GetAgingReportV2")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<GRAgingReport>>> GetAgingReportV2()
        {
            try
            {

                List<AgingReportTemplate1> Data = (from chargetable in _context.Charge
                                                   join visittable in _context.Visit on chargetable.VisitID equals visittable.ID
                                                   join patientplantable in _context.PatientPlan on chargetable.PrimaryPatientPlanID equals patientplantable.ID
                                                   join insurangeplantable in _context.InsurancePlan on patientplantable.InsurancePlanID equals insurangeplantable.ID
                                                   join edi837payertable in _context.Edi837Payer on insurangeplantable.Edi837PayerID equals edi837payertable.ID
                                                   join patienttable in _context.Patient on chargetable.PatientID equals patienttable.ID
                                                   join cpttable in _context.Cpt on chargetable.CPTID equals cpttable.ID
                                                   group new
                                                   {
                                                       id = chargetable.ID,
                                                       chargeAmount = chargetable.TotalAmount,
                                                       claimAge = CheckClaimAge(visittable.SubmittedDate.HasValue ? GetClaimAge(visittable.SubmittedDate.Value) : 0),
                                                       payerName = edi837payertable.PayerName,
                                                       accountNumber = patienttable.AccountNum.IsNull() ? "" : patienttable.AccountNum
                                                   } by new { PayerName = edi837payertable.PayerName, ClaimAge = CheckClaimAge(visittable.SubmittedDate.HasValue ? GetClaimAge(visittable.SubmittedDate.Value) : 0), PatientName = (patienttable.LastName.Trim() + ", " + patienttable.FirstName.Trim()) } into gp

                                                   select new AgingReportTemplate1
                                                   {
                                                       VisitNumber = CheckLongListCount(gp.Select(a => a.id).ToList()) == 0 ? 0 : gp.Select(a => a.id).ToList().ElementAt(0),
                                                       AccountNumber = CheckStringListCount(gp.Select(a => a.accountNumber).ToList()) == 0 ? "" : gp.Select(a => a.accountNumber).ToList().ElementAt(0),
                                                       PatientName = gp.Key.PatientName,
                                                       PayerName = gp.Key.PayerName,
                                                       charges = gp.Select(a => a.chargeAmount).ToList().ConvertAll<long>(new Converter<decimal, long>(DecimalToLong)),
                                                       claimAges = gp.Select(a => a.claimAge).ToList(),
                                                       claimAge = gp.Key.ClaimAge,

                                                   }).ToList();

                Data = Data.OrderBy(lamb => lamb.PayerName).ToList();

                List<GRAgingReport> finalData = new List<GRAgingReport>();
                int currentObj = 0;

                GRAgingReport firstRow = new GRAgingReport();
                firstRow.PayerName = Data.ElementAt(0).PayerName;
                firstRow.PatientName = Data.ElementAt(0).PatientName;
                firstRow.AccountNumber = Data.ElementAt(0).AccountNumber;
                firstRow.VisitNumber = Data.ElementAt(0).VisitNumber;
                finalData.Add(firstRow);
                long TotalAmountFinal = 0;
                foreach (AgingReportTemplate1 Temp in Data)
                {

                    if (Temp.PayerName.Equals(finalData.ElementAt(currentObj).PayerName) && Temp.PatientName.Equals(finalData.ElementAt(currentObj).PatientName))
                    {
                        List<int> ClaimAges = Temp.claimAges;
                        List<long> Charges = Temp.charges;
                        long TotalChargeToBeAdded = 0;
                        switch (Temp.claimAge)
                        {

                            case 1:
                                TotalChargeToBeAdded = 0;
                                foreach (long Charge in Charges)
                                {
                                    TotalChargeToBeAdded += Charge;
                                }
                                finalData.ElementAt(currentObj).Current = TotalChargeToBeAdded;
                                TotalAmountFinal += TotalChargeToBeAdded;
                                finalData.ElementAt(currentObj).TotalBalance =  TotalAmountFinal;
                                break;
                            case 2:
                                TotalChargeToBeAdded = 0;
                                foreach (long Charge in Charges)
                                {
                                    TotalChargeToBeAdded += Charge;
                                }
                                finalData.ElementAt(currentObj).IsBetween30And60 = TotalChargeToBeAdded;
                                TotalAmountFinal += TotalChargeToBeAdded;
                                finalData.ElementAt(currentObj).TotalBalance =  TotalAmountFinal;
                                break;
                            case 3:
                                TotalChargeToBeAdded = 0;
                                foreach (long Charge in Charges)
                                {
                                    TotalChargeToBeAdded += Charge;
                                }
                                finalData.ElementAt(currentObj).IsBetween61And90 = TotalChargeToBeAdded;
                                TotalAmountFinal += TotalChargeToBeAdded;
                                finalData.ElementAt(currentObj).TotalBalance =  TotalAmountFinal;

                                break;
                            case 4:
                                TotalChargeToBeAdded = 0;
                                foreach (long Charge in Charges)
                                {
                                    TotalChargeToBeAdded += Charge;
                                }
                                finalData.ElementAt(currentObj).IsBetween91And120 =  TotalChargeToBeAdded;
                                TotalAmountFinal += TotalChargeToBeAdded;
                                finalData.ElementAt(currentObj).TotalBalance =  TotalAmountFinal;
                                break;
                            case 5:
                                TotalChargeToBeAdded = 0;
                                foreach (long Charge in Charges)
                                {
                                    TotalChargeToBeAdded += Charge;
                                }
                                finalData.ElementAt(currentObj).IsGreaterThan120 =  TotalChargeToBeAdded;
                                TotalAmountFinal += TotalChargeToBeAdded;
                                finalData.ElementAt(currentObj).TotalBalance =  TotalAmountFinal;
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
                        TempRow.PayerName = Temp.PayerName;
                        TempRow.PatientName = Temp.PatientName;
                        TempRow.AccountNumber = Temp.AccountNumber;
                        TempRow.VisitNumber = Temp.VisitNumber;

                        List<int> ClaimAges = Temp.claimAges;
                        List<long> Charges = Temp.charges;
                        long TotalChargeToBeAdded = 0;
                        switch (Temp.claimAge)
                        {

                            case 1:
                                TotalChargeToBeAdded = 0;
                                foreach (long Charge in Charges)
                                {
                                    TotalChargeToBeAdded += Charge;
                                }
                                TempRow.Current =  TotalChargeToBeAdded;
                                TotalAmountFinal += TotalChargeToBeAdded;
                                TempRow.TotalBalance =  TotalAmountFinal;
                                break;
                            case 2:
                                TotalChargeToBeAdded = 0;
                                foreach (long Charge in Charges)
                                {
                                    TotalChargeToBeAdded += Charge;
                                }
                                TempRow.IsBetween30And60 =  TotalChargeToBeAdded;
                                TotalAmountFinal += TotalChargeToBeAdded;
                                TempRow.TotalBalance = TotalAmountFinal;
                                break;
                            case 3:
                                TotalChargeToBeAdded = 0;
                                foreach (long Charge in Charges)
                                {
                                    TotalChargeToBeAdded += Charge;
                                }
                                TempRow.IsBetween61And90 = TotalChargeToBeAdded;
                                TotalAmountFinal += TotalChargeToBeAdded;
                                TempRow.TotalBalance =  TotalAmountFinal;

                                break;
                            case 4:
                                TotalChargeToBeAdded = 0;
                                foreach (long Charge in Charges)
                                {
                                    TotalChargeToBeAdded += Charge;
                                }
                                TempRow.IsBetween91And120 = TotalChargeToBeAdded;
                                TotalAmountFinal += TotalChargeToBeAdded;
                                TempRow.TotalBalance =  TotalAmountFinal;
                                break;
                            case 5:
                                TotalChargeToBeAdded = 0;
                                foreach (long Charge in Charges)
                                {
                                    TotalChargeToBeAdded += Charge;
                                }
                                TempRow.IsGreaterThan120 = TotalChargeToBeAdded;
                                TotalAmountFinal += TotalChargeToBeAdded;
                                TempRow.TotalBalance =  TotalAmountFinal;
                                break;
                        }
                        finalData.Add(TempRow);
                    }
                }
                return finalData.OrderBy(lamb => lamb.VisitNumber).ToList();


            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText(Path.Combine(_context.env.ContentRootPath, "Logs", "FirstAgingReport.txt"), ex.ToString());
                Debug.WriteLine(ex.Message + "   " + ex.StackTrace);
                return Ok(ex);
            }
        }

        

        [Route("FindAgingReportV2")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<GRAgingReport>>> FindAgingReportV2(CRAgingReport CRAgingReport)
        {
            try
            {
                List<AgingReportTemplate1> Data = (from chargetable in _context.Charge
                                                   join visittable in _context.Visit on chargetable.VisitID equals visittable.ID
                                                   join patientplantable in _context.PatientPlan on chargetable.PrimaryPatientPlanID equals patientplantable.ID
                                                   join practicetable in _context.Practice on chargetable.PracticeID equals practicetable.ID
                                                   join providertable in _context.Provider on chargetable.ProviderID equals providertable.ID
                                                   join locationtable in _context.Location on chargetable.LocationID equals locationtable.ID
                                                   join insurangeplantable in _context.InsurancePlan on patientplantable.InsurancePlanID equals insurangeplantable.ID
                                                   join edi837payertable in _context.Edi837Payer on insurangeplantable.Edi837PayerID equals edi837payertable.ID
                                                   join patienttable in _context.Patient on chargetable.PatientID equals patienttable.ID
                                                   join cpttable in _context.Cpt on chargetable.CPTID equals cpttable.ID

                                                   where
                                                            (CRAgingReport.PracticeName.IsNull() ? true : CRAgingReport.PracticeName.Equals(practicetable.Name)) &&
                                                            (CRAgingReport.ProviderName.IsNull() ? true : CRAgingReport.ProviderName.Equals(providertable.Name)) &&
                                                            (CRAgingReport.Location.IsNull() ? true : CRAgingReport.Location.Equals(locationtable.Name)) &&
                                                            (CRAgingReport.PayerName.IsNull() ? true : CRAgingReport.PayerName.Equals(edi837payertable.PayerName)) &&
                                                            (ExtensionMethods.IsBetweenDOS(CRAgingReport.DateOfServiceTo, CRAgingReport.DateOfServiceFrom, visittable.DateOfServiceTo, visittable.DateOfServiceFrom)) &&
                                                            (ExtensionMethods.IsBetweenDOS(CRAgingReport.EntryDateTo, CRAgingReport.EntryDateFrom, visittable.AddedDate, visittable.AddedDate)) &&
                                                            (ExtensionMethods.IsBetweenDOS(CRAgingReport.SubmittedDateTo, CRAgingReport.SubmittedDateFrom, visittable.SubmittedDate, visittable.SubmittedDate)) &&
                                                            (CRAgingReport.PatientName.IsNull() ? true : (patienttable.FirstName.Trim() + " " + patienttable.LastName.Trim()).Equals(CRAgingReport.PatientName)) &&
                                                            (CRAgingReport.PatientAccountNumber.IsNull() ? true : patienttable.AccountNum.Equals(CRAgingReport.PatientAccountNumber))

                                                   group new
                                                   {
                                                       id = chargetable.ID,
                                                       chargeAmount = chargetable.TotalAmount,
                                                       claimAge = CheckClaimAge(visittable.SubmittedDate.HasValue ? GetClaimAge(visittable.SubmittedDate.Value) : 0),
                                                       payerName = edi837payertable.PayerName,
                                                       accountNumber = patienttable.AccountNum.IsNull() ? "" : patienttable.AccountNum
                                                   } by new { PayerName = edi837payertable.PayerName, ClaimAge = CheckClaimAge(visittable.SubmittedDate.HasValue ? GetClaimAge(visittable.SubmittedDate.Value) : 0), PatientName = (patienttable.LastName.Trim() + ", " + patienttable.FirstName.Trim()) } into gp

                                                   select new AgingReportTemplate1
                                                   {
                                                       VisitNumber = CheckLongListCount(gp.Select(a => a.id).ToList()) == 0 ? 0 : gp.Select(a => a.id).ToList().ElementAt(0),
                                                       AccountNumber = CheckStringListCount(gp.Select(a => a.accountNumber).ToList()) == 0 ? "" : gp.Select(a => a.accountNumber).ToList().ElementAt(0),
                                                       PatientName = gp.Key.PatientName,
                                                       PayerName = gp.Key.PayerName,
                                                       charges = gp.Select(a => a.chargeAmount).ToList().ConvertAll<long>(new Converter<decimal, long>(DecimalToLong)),
                                                       claimAges = gp.Select(a => a.claimAge).ToList(),
                                                       claimAge = gp.Key.ClaimAge,

                                                   }).ToList();

                Data = Data.OrderBy(lamb => lamb.PayerName).ToList();

                List<GRAgingReport> finalData = new List<GRAgingReport>();
                int currentObj = 0;

                GRAgingReport firstRow = new GRAgingReport();
                firstRow.PayerName = Data.ElementAt(0).PayerName;
                firstRow.PatientName = Data.ElementAt(0).PatientName;
                firstRow.AccountNumber = Data.ElementAt(0).AccountNumber;
                firstRow.VisitNumber = Data.ElementAt(0).VisitNumber;
                finalData.Add(firstRow);
                long TotalAmountFinal = 0;
                foreach (AgingReportTemplate1 Temp in Data)
                {

                    if (Temp.PayerName.Equals(finalData.ElementAt(currentObj).PayerName) && Temp.PatientName.Equals(finalData.ElementAt(currentObj).PatientName))
                    {
                        List<int> ClaimAges = Temp.claimAges;
                        List<long> Charges = Temp.charges;
                        long TotalChargeToBeAdded = 0;
                        switch (Temp.claimAge)
                        {

                            case 1:
                                TotalChargeToBeAdded = 0;
                                foreach (long Charge in Charges)
                                {
                                    TotalChargeToBeAdded += Charge;
                                }
                                finalData.ElementAt(currentObj).Current =  TotalChargeToBeAdded;
                                TotalAmountFinal += TotalChargeToBeAdded;
                                finalData.ElementAt(currentObj).TotalBalance =  TotalAmountFinal;
                                break;
                            case 2:
                                TotalChargeToBeAdded = 0;
                                foreach (long Charge in Charges)
                                {
                                    TotalChargeToBeAdded += Charge;
                                }
                                finalData.ElementAt(currentObj).IsBetween30And60 =  TotalChargeToBeAdded;
                                TotalAmountFinal += TotalChargeToBeAdded;
                                finalData.ElementAt(currentObj).TotalBalance =  TotalAmountFinal;
                                break;
                            case 3:
                                TotalChargeToBeAdded = 0;
                                foreach (long Charge in Charges)
                                {
                                    TotalChargeToBeAdded += Charge;
                                }
                                finalData.ElementAt(currentObj).IsBetween61And90 =  TotalChargeToBeAdded;
                                TotalAmountFinal += TotalChargeToBeAdded;
                                finalData.ElementAt(currentObj).TotalBalance =  TotalAmountFinal;

                                break;
                            case 4:
                                TotalChargeToBeAdded = 0;
                                foreach (long Charge in Charges)
                                {
                                    TotalChargeToBeAdded += Charge;
                                }
                                finalData.ElementAt(currentObj).IsBetween91And120 =  TotalChargeToBeAdded;
                                TotalAmountFinal += TotalChargeToBeAdded;
                                finalData.ElementAt(currentObj).TotalBalance =  TotalAmountFinal;
                                break;
                            case 5:
                                TotalChargeToBeAdded = 0;
                                foreach (long Charge in Charges)
                                {
                                    TotalChargeToBeAdded += Charge;
                                }
                                finalData.ElementAt(currentObj).IsGreaterThan120 =  TotalChargeToBeAdded;
                                TotalAmountFinal += TotalChargeToBeAdded;
                                finalData.ElementAt(currentObj).TotalBalance =  TotalAmountFinal;
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
                        TempRow.PayerName = Temp.PayerName;
                        TempRow.PatientName = Temp.PatientName;
                        TempRow.AccountNumber = Temp.AccountNumber;
                        TempRow.VisitNumber = Temp.VisitNumber;

                        List<int> ClaimAges = Temp.claimAges;
                        List<long> Charges = Temp.charges;
                        long TotalChargeToBeAdded = 0;
                        switch (Temp.claimAge)
                        {

                            case 1:
                                TotalChargeToBeAdded = 0;
                                foreach (long Charge in Charges)
                                {
                                    TotalChargeToBeAdded += Charge;
                                }
                                TempRow.Current =  TotalChargeToBeAdded;
                                TotalAmountFinal += TotalChargeToBeAdded;
                                TempRow.TotalBalance =  TotalAmountFinal;
                                break;
                            case 2:
                                TotalChargeToBeAdded = 0;
                                foreach (long Charge in Charges)
                                {
                                    TotalChargeToBeAdded += Charge;
                                }
                                TempRow.IsBetween30And60 =  TotalChargeToBeAdded;
                                TotalAmountFinal += TotalChargeToBeAdded;
                                TempRow.TotalBalance =  TotalAmountFinal;
                                break;
                            case 3:
                                TotalChargeToBeAdded = 0;
                                foreach (long Charge in Charges)
                                {
                                    TotalChargeToBeAdded += Charge;
                                }
                                TempRow.IsBetween61And90 =  TotalChargeToBeAdded;
                                TotalAmountFinal += TotalChargeToBeAdded;
                                TempRow.TotalBalance = TotalAmountFinal;

                                break;
                            case 4:
                                TotalChargeToBeAdded = 0;
                                foreach (long Charge in Charges)
                                {
                                    TotalChargeToBeAdded += Charge;
                                }
                                TempRow.IsBetween91And120 =  TotalChargeToBeAdded;
                                TotalAmountFinal += TotalChargeToBeAdded;
                                TempRow.TotalBalance =  TotalAmountFinal;
                                break;
                            case 5:
                                TotalChargeToBeAdded = 0;
                                foreach (long Charge in Charges)
                                {
                                    TotalChargeToBeAdded += Charge;
                                }
                                TempRow.IsGreaterThan120 = TotalChargeToBeAdded;
                                TotalAmountFinal += TotalChargeToBeAdded;
                                TempRow.TotalBalance = TotalAmountFinal;
                                break;
                        }
                        finalData.Add(TempRow);
                    }
                }
                return finalData.OrderBy(lamb => lamb.VisitNumber).ToList();


            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText(Path.Combine(_context.env.ContentRootPath, "Logs", "FirstAgingReport.txt"), ex.ToString());
                Debug.WriteLine(ex.Message + "   " + ex.StackTrace);
                return Ok(ex);
            }
        }

        public long DecimalToLong(decimal pf)
        {
            return (long)pf;
        }

        public int CheckStringListCount(List<string> list)
        {
            return list.Count;
        }
        public int CheckLongListCount(List<long> list)
        {
            return list.Count;
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

      
    }
}
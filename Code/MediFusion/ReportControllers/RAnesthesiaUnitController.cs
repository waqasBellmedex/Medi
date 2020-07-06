using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediFusionPM.Models;
using MediFusionPM.ViewModels;
using static MediFusionPM.ReportViewModels.RVMAnesthesiaUnit;
using static MediFusionPM.ViewModels.VMCommon;
using MediFusionPM.Controllers;

namespace MediFusionPM.ReportControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class RAnesthesiaUnitController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;

        public RAnesthesiaUnitController(ClientDbContext context, MainContext contextMain)
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


        [Route("GetAnesthesiaUnitReport")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<GRAnesthesiaUnit>>> GetAnesthesiaUnitReport()
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            try
            {

                // object o = null;
                //string oo = o.ToString();
                return await (from chargetable in _context.Charge
                              join patienttable in _context.Patient on chargetable.PatientID equals patienttable.ID
                              join practicetable in _context.Practice on patienttable.PracticeID equals practicetable.ID
                              join providertable in _context.Provider on chargetable.ProviderID equals providertable.ID
                              join claimtable in _context.Visit on chargetable.VisitID equals claimtable.ID
                              join postable in _context.POS on chargetable.POSID equals postable.ID
                              join cpttable in _context.Cpt on chargetable.CPTID equals cpttable.ID
                              join modtable in _context.Modifier on chargetable.Modifier1ID equals modtable.ID into Mod1Table
                              from m1t in Mod1Table.DefaultIfEmpty()
                              join modtable in _context.Modifier on chargetable.Modifier2ID equals modtable.ID into Mod2Table
                              from m2t in Mod2Table.DefaultIfEmpty()
                              join patientplantable in _context.PatientPlan on chargetable.PatientID equals patientplantable.ID
                              join insuranceplantable in _context.InsurancePlan on patientplantable.InsurancePlanID equals insuranceplantable.ID
                              join insurancetable in _context.Insurance on insuranceplantable.InsuranceID equals insurancetable.ID

                              where practicetable.ID == UD.PracticeID  &&
                              (cpttable.Category.IsNull() ? true : cpttable.Category.Equals("Anesthesia"))

                              select new GRAnesthesiaUnit
                              {
                                  PatientAccountNumber = patienttable.AccountNum,
                                  PatientName = patienttable.LastName + ", " + patienttable.FirstName,
                                  ProviderName = providertable.Name,
                                  ClaimCreatedDate = claimtable.AddedDate.ToString(@"MM\/dd\/yyyy"),
                                  DateOfService = chargetable.DateOfServiceFrom.ToString(@"MM\/dd\/yyyy"),
                                  Pos = postable.PosCode,
                                  Cpt = cpttable.CPTCode,
                                  Description = cpttable.Description,
                                  MOD1 = m1t.Code.IsNull() ? "" : m1t.Code,
                                  MOD2 = m2t.Code.IsNull() ? "" : m2t.Code,
                                  ChargeAmount = "$" + chargetable.TotalAmount,
                                  BaseUnits = chargetable.BaseUnits.HasValue ? chargetable.BaseUnits : 0,
                                  TimeUnits = chargetable.TimeUnits.HasValue ? chargetable.TimeUnits : 0,
                                  StartTime = chargetable.StartTime.HasValue ? chargetable.StartTime.Value.ToString(@"hh\:mm tt") : "",
                                  EndTime = chargetable.EndTime.HasValue ? chargetable.EndTime.Value.ToString(@"hh\:mm tt") : "",
                                  TotalTime = chargetable.EndTime.HasValue ? chargetable.StartTime.HasValue ? (chargetable.EndTime.Value - chargetable.StartTime.Value).ToString(@"hh\:mm") : "00:00" : "00:00",
                                  TotalUnits = chargetable.Units.IsNull() ? "" : chargetable.Units,
                                  TotalMin = chargetable.EndTime.HasValue ? chargetable.StartTime.HasValue ? chargetable.EndTime.Value.Subtract(chargetable.StartTime.Value).TotalMinutes.ToString() : "0" : "0",
                                  Charges = chargetable.PrimaryBilledAmount.HasValue ? "$" + chargetable.PrimaryBilledAmount : "",
                                  InsuranceName = insurancetable.Name,
                                  ModifierUnits = chargetable.ModifierUnits.HasValue ? chargetable.ModifierUnits : 0
                              }
                              ).ToAsyncEnumerable().ToList();
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText(Path.Combine(_context.env.ContentRootPath, "Logs", "AnesthesiaUnit.txt"), ex.ToString());
                Debug.WriteLine(ex.Message + "   " + ex.StackTrace);
                return Ok(ex);
            }
        }





        [Route("FindAnesthesiaUnitReports")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<GRAnesthesiaUnit>>> FindAnesthesiaUnitReports(CRAnesthesiaUnit CRAnesthesiaUnit)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
          User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
          User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
         // if (UD == null || UD.Rights == null || UD.Rights.submissionLogSearch == false)
          //return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
          return FindAnesthesiaUnitReports(CRAnesthesiaUnit, UD);
        }
        private List<GRAnesthesiaUnit> FindAnesthesiaUnitReports(CRAnesthesiaUnit CRAnesthesiaUnit, UserInfoData UD)
        {
            //try
            //{
                // object o = null;
                //string oo = o.ToString();
                List<GRAnesthesiaUnit> data = (from chargetable in _context.Charge
                              join patienttable in _context.Patient on chargetable.PatientID equals patienttable.ID
                              join practicetable in _context.Practice on patienttable.PracticeID equals practicetable.ID
                              join providertable in _context.Provider on chargetable.ProviderID equals providertable.ID
                              join claimtable in _context.Visit on chargetable.VisitID equals claimtable.ID
                              join postable in _context.POS on chargetable.POSID equals postable.ID
                              join cpttable in _context.Cpt on chargetable.CPTID equals cpttable.ID
                              join modtable in _context.Modifier on chargetable.Modifier1ID equals modtable.ID into Mod1Table
                              from m1t in Mod1Table.DefaultIfEmpty()
                              join modtable in _context.Modifier on chargetable.Modifier2ID equals modtable.ID into Mod2Table
                              from m2t in Mod2Table.DefaultIfEmpty()
                              join patientplantable in _context.PatientPlan on chargetable.PatientID equals patientplantable.ID
                              join insuranceplantable in _context.InsurancePlan on patientplantable.InsurancePlanID equals insuranceplantable.ID
                              join insurancetable in _context.Insurance on insuranceplantable.InsuranceID equals insurancetable.ID

                                where practicetable.ID == UD.PracticeID &&
                                (cpttable.Category.IsNull() ? true : cpttable.Category.Equals("Anesthesia")) &&
                                (CRAnesthesiaUnit.PatientAccountNumber.IsNull() ? true : patienttable.AccountNum.Equals(CRAnesthesiaUnit.PatientAccountNumber)) &&
                                (CRAnesthesiaUnit.ProviderName.IsNull() ? true : providertable.Name.Equals(CRAnesthesiaUnit.ProviderName)) &&
                                (ExtensionMethods.IsBetweenDOS(CRAnesthesiaUnit.DateOfServiceTo, CRAnesthesiaUnit.DateOfServiceFrom, chargetable.DateOfServiceTo, chargetable.DateOfServiceFrom)) &&
                                (ExtensionMethods.IsBetweenDOS(CRAnesthesiaUnit.EntryDateTo, CRAnesthesiaUnit.EntryDateFrom, chargetable.AddedDate, chargetable.AddedDate)) &&
                                (ExtensionMethods.IsBetweenDOS(CRAnesthesiaUnit.SubmittedDateTo, CRAnesthesiaUnit.SubmittedDateFrom, chargetable.SubmittetdDate, chargetable.SubmittetdDate))

                              select new GRAnesthesiaUnit
                              {
                                  PatientAccountNumber = patienttable.AccountNum,
                                  PatientName = patienttable.LastName + ", " + patienttable.FirstName,
                                  ProviderName = providertable.Name,
                                  ClaimCreatedDate = claimtable.AddedDate.ToString(@"MM\/dd\/yyyy"),
                                  DateOfService = chargetable.DateOfServiceFrom.ToString(@"MM\/dd\/yyyy"),
                                  Pos = postable.PosCode,
                                  Cpt = cpttable.CPTCode,   
                                  Description = cpttable.Description,
                                  ShortCptDescription = cpttable.ShortDescription,
                                  MOD1 = m1t.Code.IsNull() ? "" : m1t.Code,
                                  MOD2 = m2t.Code.IsNull() ? "" : m2t.Code,
                                  ChargeAmount = "$" + chargetable.TotalAmount,
                                  BaseUnits = chargetable.BaseUnits.HasValue ? chargetable.BaseUnits : 0,
                                  TimeUnits = chargetable.TimeUnits.HasValue ? chargetable.TimeUnits : 0,
                                  StartTime = chargetable.StartTime.HasValue ? chargetable.StartTime.Value.ToString(@"hh\:mm:ss tt") : "",
                                  EndTime = chargetable.EndTime.HasValue ? chargetable.EndTime.Value.ToString(@"hh\:mm:ss tt") : "",
                                  TotalTime = chargetable.EndTime.HasValue ? chargetable.StartTime.HasValue ? (chargetable.EndTime.Value - chargetable.StartTime.Value)+"" : "00:00" : "00:00",
                                  // TotalUnits = chargetable.Units.IsNull() ? "" : chargetable.Units,
                                  //TotalUnits = CRAnesthesiaUnit.IncludePhysicalStatus ? (chargetable.Units.IsNull() ? "" : chargetable.Units) : (chargetable.Units.IsNull() ? 0 : (Int32.Parse(chargetable.Units) - chargetable.ModifierUnits)) + "",
                                  TotalUnits = CRAnesthesiaUnit.IncludePhysicalStatus ?((chargetable.TimeUnits.IsNull() ? 0 : chargetable.TimeUnits) + (chargetable.BaseUnits.IsNull() ? 0 : chargetable.BaseUnits)) + "": ((chargetable.TimeUnits.IsNull() ? 0 : chargetable.TimeUnits) + (chargetable.BaseUnits.IsNull() ? 0 : chargetable.BaseUnits) + (chargetable.ModifierUnits.IsNull() ? 0 : chargetable.ModifierUnits)) + "",
                                  //TotalMin = chargetable.EndTime.HasValue ? chargetable.StartTime.HasValue ? chargetable.EndTime.Value.Subtract(chargetable.StartTime.Value).TotalMinutes.ToString() : "0" : "0",
                                  //TotalMin = chargetable.EndTime.HasValue ? chargetable.StartTime.HasValue ? String.Format("{0:0.##}", double.Parse(chargetable.EndTime.Value.Subtract(chargetable.StartTime.Value).TotalMinutes.ToString())) : "0": "0",
                                  TotalMin = chargetable.EndTime.HasValue ? chargetable.StartTime.HasValue ? ( (int) chargetable.EndTime.Value.Subtract(chargetable.StartTime.Value).TotalMinutes +
                                  "."+((chargetable.EndTime.Value - chargetable.StartTime.Value).ToString(@"ss"))
                                  ) : "0" : "0",
                                  Charges = chargetable.PrimaryBilledAmount.HasValue ? "$" + chargetable.PrimaryBilledAmount : "",
                                  InsuranceName = insurancetable.Name,
                                  ModifierUnits = chargetable.ModifierUnits.HasValue ? chargetable.ModifierUnits : 0,
                              }
                              ).ToList();
                return data;
            
            //catch (Exception ex)
            //{
            //    System.IO.File.WriteAllText(Path.Combine(_context.env.ContentRootPath, "Logs", "AnesthesiaUnit.txt"), ex.ToString());
            //    Debug.WriteLine(ex.Message + "   " + ex.StackTrace);
            //    return Ok(ex);
            //}
        }
        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CRAnesthesiaUnit CRAnesthesiaUnit)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GRAnesthesiaUnit> data = FindAnesthesiaUnitReports(CRAnesthesiaUnit, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CRAnesthesiaUnit, "Anesthesia Unit Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CRAnesthesiaUnit CRAnesthesiaUnit)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GRAnesthesiaUnit> data = FindAnesthesiaUnitReports(CRAnesthesiaUnit, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }


        private bool CheckNullLong(long? Value)
        {
            Debug.WriteLine(Value.IsNull());
            return Value.IsNull();
        }

     
    }
}
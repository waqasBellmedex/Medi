using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Controllers;
using MediFusionPM.Models;
using MediFusionPM.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.ChartModel;
using static MediFusionPM.ReportViewModels.RVMPatient;
using static MediFusionPM.ViewModels.VMCommon;

namespace MediFusionPM.ReportControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RPatientController : ControllerBase
    {

        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;

        public RPatientController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
        }

        [Route("GetPatientAppointmentReport")]
        [HttpPost]
        public async Task<ActionResult<Dictionary<string, string>>> GetPatientAppointmentReport(CRPatientAppointment CRPA)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            return GetPatientAppointment(UD, CRPA);
        }
        private Dictionary<string, string> GetPatientAppointment(UserInfoData UD, CRPatientAppointment CRPA)
        {


            List<AppointmentReturnCO> data = (from patientappointmenttable in _context.PatientAppointment
                                                        join pat in _context.Patient 
                                                        on patientappointmenttable.PatientID equals pat.ID
                                                        join pro in _context.Provider 
                                                        on patientappointmenttable.ProviderID equals pro.ID
                                                        join prac in _context.Practice
                                                        on pro.PracticeID equals prac.ID

                                               where (ExtensionMethods.IsBetweenDOS(CRPA.AppoinmentDateTo, CRPA.AppoinmentDateFrom, patientappointmenttable.AddedDate.Date, patientappointmenttable.AddedDate.Date))&&
                                                    (CRPA.PatientID.IsNull() ? true : patientappointmenttable.PatientID.Equals(CRPA.PatientID)) &&
                                                    (CRPA.ProviderID.IsNull() ? true : patientappointmenttable.ProviderID.Equals(CRPA.ProviderID)) &&
                                                    (CRPA.LocationID.IsNull() ? true : patientappointmenttable.LocationID.Equals(CRPA.LocationID)) 
                                               group patientappointmenttable by patientappointmenttable.Status into gp

                                              select new AppointmentReturnCO
                                              {
                                                  Count = gp.Count(),
                                                  Type = TranslateStatusCode(gp.Key)

                                              }).ToList();

            //AppointmentCO appointment = new AppointmentCO();
            Dictionary<string, string> finalData = new Dictionary<string, string>();

            int[] checks = new int[] { 0, 0, 0, 0, 0 };
            foreach (AppointmentReturnCO obj in data)
            {
                //Required Node are Present 
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
                //Conversion Camel Case
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
            //Add Static Value for absent values = 0
            for (int randomI = 0; randomI < checks.Length; randomI++)
            {
                if (checks[randomI] == 0)
                {
                    finalData.Add(getStringFromNumberForAppointmentData(randomI), "0");
                }
            }

            return finalData;
        }
      

        public string TranslateStatusCode(string StatusCode)
        {
            string status = "";
            switch (StatusCode)
            {
                case "R":
                    status = "Rescheduled";
                    break;
                case "N":
                    status = "No Show";
                    break;
                case "C":
                    status = "Cancelled";
                    break;
                case "SN":
                    status = "Seen";
                    break;
                case "S":
                    status = "Scheduled";
                    break;
                default:
                    status = "None";
                    break;
            }

            return status;
        }
        public string getStringFromNumberForAppointmentData(int i)
        {
            string[] strings = new string[] { "scheduled", "seen", "noShow", "cancelled", "rescheduled" };
            return strings[i];
        }

        [Route("GetPatientPendingClaimsReport")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<GRPatientPending>>> GetPatientPendingClaimsReport(CRPatientAppointment CRPA)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            return GetPatientPendingClaims(CRPA, UD);
        }


            private List<GRPatientPending> GetPatientPendingClaims(CRPatientAppointment CRPA , UserInfoData UD)
            {
                List<GRPatientPending> data = (from patientappointmenttable in _context.PatientAppointment
                                                  join pat in _context.Patient
                                                  on patientappointmenttable.PatientID equals pat.ID
                                                  join pro in _context.Provider
                                                  on patientappointmenttable.ProviderID equals pro.ID

                                                  join visitTable in _context.Visit on pro.ID equals visitTable.ProviderID
                                                  into Table1 from t1 in Table1.DefaultIfEmpty()

                                                  join prac in _context.Practice
                                                  on pro.PracticeID equals prac.ID
                                                  join loc in _context.Location
                                                  on pat.LocationId equals loc.ID

                                                  where
                                                  t1.ID.IsNull() &&
                                                  (ExtensionMethods.IsBetweenDOS(CRPA.AppoinmentDateTo, CRPA.AppoinmentDateFrom, patientappointmenttable.AppointmentDate.Value.Date, patientappointmenttable.AppointmentDate.Value.Date))
                                                  &&
                                                       (CRPA.PatientID.IsNull() ? true : patientappointmenttable.PatientID.Equals(CRPA.PatientID)) &&
                                                       (CRPA.ProviderID.IsNull() ? true : patientappointmenttable.ProviderID.Equals(CRPA.ProviderID)) &&
                                                       (CRPA.LocationID.IsNull() ? true : patientappointmenttable.LocationID.Equals(CRPA.LocationID))
                                                  select new GRPatientPending
                                                  {
                                                      AppoinmentDate= patientappointmenttable.AppointmentDate,
                                                        ProviderName=pro.LastName.Trim() + ", " + pro.FirstName,
                                                        Location=loc.Name,
                                                        Patient=pat.LastName.Trim()+", " + pat.FirstName,
                                                        AccountNo=pat.AccountNum,
                                                        DOB=pat.DOB.Value,

                                                }).ToList();
                return data;
        }

        [HttpPost]
        [Route("ExportPatientPendingClaims")]
        public async Task<IActionResult> Export(CRPatientAppointment CRPA)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GRPatientPending> data = GetPatientPendingClaims(CRPA,UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CRPA, "RPA Report");
        }

        [HttpPost]
        [Route("ExportPdfPatientPendingClaims")]
        public async Task<IActionResult> ExportPdf(CRPatientAppointment CRPA)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GRPatientPending> data = GetPatientPendingClaims(CRPA, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);
        }

        [Route("GetPatientReferralPhysician")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<GRPatientReferralPhysician>>> GetPatientReferralPhysician(CRPatientReferralPhysician CRPA)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            return GetPatientReferralPhysician(CRPA ,UD);
        }


        private List<GRPatientReferralPhysician> GetPatientReferralPhysician(CRPatientReferralPhysician CRPA ,UserInfoData UD)
        {

            List<GRPatientReferralPhysician> data = (from visittable in _context.Visit
                                                     join pat in _context.Patient
                                                     on visittable.PatientID equals pat.ID
                                                     join RefPro in _context.RefProvider
                                                     on visittable.RefProviderID equals RefPro.ID
                                                     where
                                                     (ExtensionMethods.IsBetweenDOS(CRPA.DOSDateTo, CRPA.DOSDateFrom, visittable.DateOfServiceFrom, visittable.DateOfServiceFrom)) &&
                                                     (ExtensionMethods.IsBetweenDOS(CRPA.EntryDateTo, CRPA.EntryDateFrom, visittable.AddedDate, visittable.AddedDate))

                                                     && (CRPA.PatientID.IsNull() ? true : visittable.PatientID.Equals(CRPA.PatientID))
                                                     && (CRPA.RefProviderID.IsNull() ? true : visittable.RefProviderID.Equals(CRPA.RefProviderID))
                                                     && (CRPA.LocationID.IsNull() ? true : visittable.LocationID.Equals(CRPA.LocationID))

                                                     select new GRPatientReferralPhysician
                                                     {
                                                         VisitId = visittable.ID,
                                                         PatientID = visittable.PatientID,
                                                         DOS = visittable.DateOfServiceFrom.Value.ToString(@"MM\/dd\/yyyy"),
                                                         RefProvider = RefPro.LastName + ", " + RefPro.FirstName,
                                                         AccountNo = pat.AccountNum,
                                                         PatientName = pat.LastName + ", " + pat.FirstName,
                                                         DOB = pat.DOB.Value.ToString(@"MM\/dd\/yyyy"),
                                                         PhoneNo = pat.PhoneNumber,

                                                     }).ToList();
            return data;
        }
        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CRPatientReferralPhysician CRPA)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GRPatientReferralPhysician> data = GetPatientReferralPhysician(CRPA, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CRPA, "RPA Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CRPatientReferralPhysician CRPA)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GRPatientReferralPhysician> data = GetPatientReferralPhysician(CRPA, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);
        }

    }
}
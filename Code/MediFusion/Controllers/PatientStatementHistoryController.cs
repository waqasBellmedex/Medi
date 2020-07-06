using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;
using MediFusionPM.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static MediFusionPM.ViewModels.VMStatementLog;
using static MediFusionPM.ViewModels.VMCommon;
using static MediFusionPM.ViewModels.VMStatementHistory;
using Microsoft.AspNetCore.Authorization;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientStatementHistoryController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;
        public PatientStatementHistoryController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;
        }

        [HttpPost]
        [Route("FindPatientStatementHisotry")]
        public async Task<ActionResult<IEnumerable<GStatementHistory>>> FindPatientStatementHisotry(CStatementHistory CStatementHistory)
        {
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindPatientStatementHisotry(CStatementHistory, PracticeId);
        }
            private List<GStatementHistory> FindPatientStatementHisotry(CStatementHistory CStatementHistory, long PracticeId)
            {
                List<GStatementHistory> data = (from p in _context.Patient
                                                join ps in _context.PatientStatement
                                                on p.ID equals ps.PatientID
                                                join v in _context.Visit on ps.VisitID equals v.ID
                                                where
                                                (CStatementHistory.Account.IsNull() ? true : p.AccountNum.Equals(CStatementHistory.Account)) &&
                                                (CStatementHistory.LastName.IsNull() ? true : p.LastName.Equals(CStatementHistory.LastName)) &&
                                                (CStatementHistory.FirstName.IsNull() ? true : p.FirstName.Equals(CStatementHistory.FirstName)) &&
                                                (CStatementHistory.VisitID.IsNull() ? true : ps.VisitID.Equals(CStatementHistory.VisitID))

                                            select new GStatementHistory()
                                            {
                                                ID = ps.ID,
                                                Account = p.AccountNum,
                                                PatientName = p.FirstName + "," + p.LastName,
                                                PatientID = ps.PatientID,
                                                VisitID = ps.VisitID,
                                                StatementDate = v.SubmittedDate.Format("MM/dd/yyyy"),
                                                StatementCount = ps.statementStatus,
                                            }).ToList();
            return data;
        }
        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CStatementHistory CStatementHistory)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GStatementHistory> data = FindPatientStatementHisotry(CStatementHistory, PracticeId);
            ExportController controller = new ExportController(_context);

            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);

            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CStatementHistory, "Patient Statement History Report");

        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CStatementHistory CStatementHistory)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GStatementHistory> data = FindPatientStatementHisotry(CStatementHistory, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);
        }



        [Route("DownloadStatementPDF/{ID}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadStatementPDF(long ID)
        {
            PatientStatement PatientStatement = await _context.PatientStatement.FindAsync(ID);
            if (PatientStatement != null)
            {
                if (!System.IO.File.Exists(PatientStatement.pdf_url))
                {
                    return NotFound();
                }
                Byte[] fileBytes = System.IO.File.ReadAllBytes(PatientStatement.pdf_url);
                //this returns a byte array of the PDF file content
                if (fileBytes == null)
                    return NotFound();
                var stream = new MemoryStream(fileBytes); //saves it into a stream
                stream.Position = 0;
                return File(stream, "application/octec-stream", "patientstatement.pdf");
            }

            return NotFound();
        }

        [Route("DownloadStatementCSV/{ID}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadStatementCSV(long ID)
        {
            PatientStatement PatientStatement = await _context.PatientStatement.FindAsync(ID);
            if (PatientStatement != null)
            {
                if (!System.IO.File.Exists(PatientStatement.csv_url))
                {
                    return NotFound();
                }
                Byte[] fileBytes = System.IO.File.ReadAllBytes(PatientStatement.csv_url);
                //this returns a byte array of the PDF file content
                if (fileBytes == null)
                    return NotFound();
                var stream = new MemoryStream(fileBytes); //saves it into a stream
                stream.Position = 0;
                return File(stream, "application/octec-stream", "patientstatement.CSV");
            }
            return NotFound();
        }


        [HttpPost]
        [Route("FindStatementLog")]
        public async Task<ActionResult<IEnumerable<GStatementLog>>> FindStatementLog(CStatementLog CStatementLog)
        {
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindStatementLog(CStatementLog, PracticeId);
        }
        private List<GStatementLog> FindStatementLog(CStatementLog CStatementLog, long PracticeId)
        {
            List<GStatementLog> data = (from P in _context.Patient
                                        join PS in _context.PatientStatement
                                        on P.ID equals PS.PatientID
                                        join v in _context.Visit on PS.VisitID equals v.ID
                                        where
                                        (CStatementLog.Account.IsNull() ? true : P.AccountNum.Equals(CStatementLog.Account)) &&
                                        (CStatementLog.LastName.IsNull() ? true : P.LastName.Contains(CStatementLog.LastName)) &&
                                        (CStatementLog.FirstName.IsNull() ? true : P.FirstName.Contains(CStatementLog.FirstName)) &&
                                        //(CStatementLog.PatientName.IsNull() ? true : (P.FirstName + " " + P.LastName).Contains(CStatementLog.PatientName)) &&
                                        (CStatementLog.VisitID.IsNull() ? true : PS.VisitID.Equals(CStatementLog.VisitID)) &&
                                        (ExtensionMethods.IsBetweenDOS(CStatementLog.StatementToDate, CStatementLog.StatementFromDate, v.LastStatementDate, v.LastStatementDate)) &&
                                        (ExtensionMethods.IsBetweenDOS(CStatementLog.DOSTO, CStatementLog.DOSFrom, v.DateOfServiceFrom, v.DateOfServiceTo)) &&
                                        (CStatementLog.ProviderID.IsNull() ? true : P.ProviderID.Equals(CStatementLog.ProviderID))

                                        select new GStatementLog()
                                        {
                                            ID = PS.ID,
                                            Account = P.AccountNum,
                                            PatientName = P.FirstName.Trim() + ", " + P.LastName.Trim(),
                                            PatientID = PS.PatientID,
                                            VisitID = PS.VisitID,
                                            StatementDate = v.LastStatementDate.Format("MM/dd/yyyy"),
                                            StatementCount = PS.statementStatus,
                                            DownloadPDF = PS.pdf_url,
                                            DownlaodCSV = PS.csv_url,
                                            DOS = v.DateOfServiceFrom.Format("MM/dd/yyyy"),
                                            StatmentStatus = PS.statementStatus
                                        }).ToList();
            return data;
        }
    }

}


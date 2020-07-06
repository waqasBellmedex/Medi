using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using MediFusionPM.Controllers;
using MediFusionPM.Models;
using MediFusionPM.ViewModels;
using static MediFusionPM.ViewModels.VMCommon;
using static MediFusionPM.ViewModels.VMReportLog;
using MediFusionPM.Models.Main;
using MediFusionPM.Models.Audit;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportLogController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;

        public ReportLogController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
        }
        // POST: api/ReportsLogs

        //[HttpPost]
        //[Route("FindReportLog")]
        //public async Task<ActionResult<IEnumerable<GReportLog>>> PostReportsLog(CReportLog CReportLog)
        //{
        //    try
        //    {
        //        return await (from Replog in _context.ReportsLog
        //                      join r in _context.Receiver on Replog.ReceiverID equals r.ID into Table1
        //                      from t1 in Table1.DefaultIfEmpty()
        //                      where

        //                 (CReportLog.ReceiverID.IsNull() ? true : Replog.ReceiverID.Equals(CReportLog.ReceiverID)) &&
        //                 (CReportLog.FromDate != null && CReportLog.ToDate != null ?
        //                Replog.AddedDate.Date >= CReportLog.FromDate && Replog.AddedDate.Date <= CReportLog.ToDate
        //               : (CReportLog.FromDate != null ? Replog.AddedDate.Date >= CReportLog.FromDate : true))
        //                      select new GReportLog
        //                      {
        //                          ID = Replog.ID,
        //                          RecieverID = Replog.ReceiverID,
        //                          RecieverName = t1.Name,
        //                          UserResolved = Replog.UserResolved,
        //                          Date = Replog.AddedDate.Format("MM/dd/yyyy"),
        //                          AddedBy = Replog.AddedBy,
        //                      }).ToListAsync();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        [HttpPost]
        [Route("FindReportLog")]
        public ActionResult<IEnumerable<GReportLog>> FindReportLog(CReportLog CReportLog)
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            // User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            // User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null) return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            //MainRights rights = VMCommon.GetLoginUserRights(_contextMain, UD.UserID);
            //if (rights == null) return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            //if (rights.PracticeSearch == false) return BadRequest("You Don't Have Rights For This Action.");
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindReportLog(CReportLog, PracticeId);

        }
        private List<GReportLog> FindReportLog(CReportLog CReportLog, long PracticeId)
        {
            List<GReportLog> data = (from Replog in _context.ReportsLog
                                     join r in _context.Receiver on Replog.ReceiverID equals r.ID into Table1
                                     from t1 in Table1.DefaultIfEmpty()
                                     orderby Replog.AddedDate descending
                                     where

                                (CReportLog.ReceiverID.IsNull() ? true : Replog.ReceiverID.Equals(CReportLog.ReceiverID)) &&
                                (CReportLog.FromDate != null && CReportLog.ToDate != null ?
                                Replog.AddedDate.Date >= CReportLog.FromDate && Replog.AddedDate.Date <= CReportLog.ToDate
                              : (CReportLog.FromDate != null ? Replog.AddedDate.Date >= CReportLog.FromDate : true))
                              where Replog.FilesCount > 0
                                     select new GReportLog
                                     {
                                         ID = Replog.ID,
                                         RecieverID = Replog.ReceiverID,
                                         RecieverName = t1.Name,
                                         Processed = Replog.Processed == null ? "" : (Replog.Processed == false ? "No" : "Yes" ),
                                         UserResolved = Replog.UserResolved == null ? "" :(Replog.UserResolved == false ? "No" : "Yes"),
                                         
                                         //== false ? "No" : (Replog.UserResolved == true ? "Yes" : ""),
                                         FilesCount = Replog.FilesCount,
                                         Date = Replog.AddedDate.Format("MM/dd/yyyy hh:mm tt"),
                                         AddedBy = Replog.AddedBy,
                                     }).ToList();

            return data;
        }
        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CReportLog CReportLog)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GReportLog> data = FindReportLog(CReportLog, PracticeId);
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CReportLog, "ReportLog Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CReportLog CReportLog)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GReportLog> data = FindReportLog(CReportLog, PracticeId);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }
        //public string TranslateStatus(string Status)
        //{

        //    string desc = string.Empty;
        //    if (Status == false)
        //    {
        //        desc = "No";
        //    }
        //    return desc;
        //}

        //[HttpPost]
        //[Route("Export")]
        //public async Task<IActionResult> Export(CReportLog CReportLog)
        //{
        //    UserInfoData UD = VMCommon.GetLoginUserInfo(_context,
        //     User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
        //     User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
        //     );

        //    Rights rights = VMCommon.GetLoginUserRights(_context, UD.UserID);
        //    if (rights == null) return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
        //    if (rights.PracticeExport == false) return BadRequest("You Don't Have Rights For This Action.");


        //    List<GReportLog> data = FindReportLog(CReportLog, UD);

        //    ExportController controller = new ExportController(_context);
        //    List<Object> objectsList = data.Cast<Object>().ToList();
        //    List<JObject> jobjectsList = new List<JObject>();

        //    for (int i = 0; i < objectsList.Count; i++)
        //    {
        //        Object tempObj = objectsList[i];
        //        //objectsList.Remove(objectsList[i]);
        //        jobjectsList.Add(JObject.FromObject(tempObj));
        //    }
        //    return await controller.ExportExcel(new ExportInput() { inputList = jobjectsList, ClientID = UD.ClientID });
        //}

        //[HttpPost]
        //[Route("ExportPdf")]
        //public async Task<IActionResult> ExportPdf(CReportLog CReportLog)
        //{
        //    UserInfoData UD = VMCommon.GetLoginUserInfo(_context,
        //     User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
        //     User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
        //     );

        //    Rights rights = VMCommon.GetLoginUserRights(_context, UD.UserID);
        //    if (rights == null) return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
        //    if (rights.PracticeExport == false) return BadRequest("You Don't Have Rights For This Action.");

        //    List<GReportLog> data = FindReportLog(CReportLog, UD);

        //    List<Object> objectsList = data.Cast<Object>().ToList();
        //    List<JObject> jobjectsList = new List<JObject>();
        //    for (int i = 0; i < objectsList.Count; i++)
        //    {
        //        jobjectsList.Add(JObject.FromObject(objectsList[i]));
        //    }

        //    ExportController controller = new ExportController(_context);
        //    return await controller.ExportPdf(new ExportInput() { inputList = jobjectsList, ClientID = UD.ClientID });
        //}


        [Route("DownloadReports/{id}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadEDIFile(long id)
        {

            ReportsLog report = await _context.ReportsLog.FindAsync(id);
            if (report != null)
            {
                if (!System.IO.File.Exists(report.ZipFilePath))
                {
                    return NotFound();
                }

                Byte[] fileBytes = System.IO.File.ReadAllBytes(report.ZipFilePath);
                //this returns a byte array of the PDF file content
                if (fileBytes == null)
                    return NotFound();
                var stream = new MemoryStream(fileBytes); //saves it into a stream
                stream.Position = 0;
                return File(stream, "application/octec-stream", "Report.zip");

            }

            return NotFound();
        }
        [Route("FindAudit/{RemarkCodeID}")]
        [HttpGet("{RemarkCodeID}")]
        public List<RemarkCodeAudit> FindAudit(long RemarkCodeID)
        {
            List<RemarkCodeAudit> data = (from pAudit in _context.RemarkCodeAudit
                                          where pAudit.RemarkCodeID == RemarkCodeID
                                          orderby pAudit.AddedDate descending
                                          select new RemarkCodeAudit()
                                          {
                                              ID = pAudit.ID,
                                              RemarkCodeID = pAudit.RemarkCodeID,
                                              TransactionID = pAudit.TransactionID,
                                              ColumnName = pAudit.ColumnName,
                                              CurrentValue = pAudit.CurrentValue,
                                              NewValue = pAudit.NewValue,
                                              CurrentValueID = pAudit.CurrentValueID,
                                              NewValueID = pAudit.NewValueID,
                                              HostName = pAudit.HostName,
                                              AddedBy = pAudit.AddedBy,
                                              AddedDate = pAudit.AddedDate,
                                          }).ToList<RemarkCodeAudit>();
            return data;
        }

        [Route("ResolveReport/{id}/{value}")]
        [HttpGet("{id}/{value}")]
        public async Task<ActionResult<ReportsLog>> ResolveReport(long ID, bool value)
        {
            var reportslog = await _context.ReportsLog.FindAsync(ID);

            if (reportslog != null)
            {
                reportslog.resolve = value;
            }
            else
            {
                reportslog.resolve = value;

            }
            _context.SaveChanges();

            return reportslog;
        }


    }
}
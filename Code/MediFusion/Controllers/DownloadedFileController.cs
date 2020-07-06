using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediFusionPM.Models;
using static MediFusionPM.ViewModel.VMDownloadFile;
using WebApplication1.Models.Audit;
using static MediFusionPM.ViewModels.VMCommon;
using MediFusionPM.ViewModels;
using System.IdentityModel.Tokens.Jwt;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadedFileController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;

        public DownloadedFileController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;
        }

        // GET: api/DownloadedFiles
        [HttpGet]
        [Route("GetDownloadedFile")]
        public IEnumerable<DownloadedFile> GetDownloadedFile()
        {
            return _context.DownloadedFile;
        }

        // GET: api/DownloadedFiles/5
        [HttpGet("{ID}")]
        [Route("GetDownloadedFile/{ID}")]

        public async Task<ActionResult<IEnumerable<GDownloadFile>>> GetDownloadedFile(long ID)
        {

            try
            {
                return await (from file in _context.DownloadedFile
                              where file.ReportsLogID == ID
                              select new GDownloadFile()
                              {
                                  ID = file.ID,
                                  ReportsLogID = file.ReportsLogID,
                                  FileName = System.IO.Path.GetFileName(file.FilePath),
                                  FileType = file.FileType,
                                  Processed = file.Processed != null && file.Processed == true ? "Yes" : "No",
                                  AddedBy = file.AddedBy,
                                  AddedDate = file.AddedDate.Format("MM/dd/yyyy"),
                              }).ToListAsync();
             
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        [HttpPost()]
        [Route("GetDownloadedFiles")]

        public async Task<ActionResult<IEnumerable<GDownloadFile>>> GetDownloadedFiles(CDownloadFiles cdownloadfiles)
        {
            
                return await (from f in _context.DownloadedFile

                              where 
                              (cdownloadfiles.FileType.IsNull() ? true : cdownloadfiles.FileType.Equals(f.FileType))
                              && cdownloadfiles.ID == f.ReportsLogID
                              select new GDownloadFile()
                              {
                                  ID = f.ID,
                                  ReportsLogID = f.ReportsLogID,
                                  FileName = System.IO.Path.GetFileName(f.FilePath),
                                  FileType = f.FileType,
                                  Processed = f.Processed != null && f.Processed == true ? "Yes" : "No",
                                  AddedBy = f.AddedBy,
                                  AddedDate = f.AddedDate.Format("MM/dd/yyyy"),
                              }).ToListAsync();

        }





        [Route("DownloadFile/{id}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadFile(long id)
        {
           
            DownloadedFile file = await _context.DownloadedFile.FindAsync(id);
            if (file != null)
            {
                if (!System.IO.File.Exists(file.FilePath))
                {
                    return NotFound();
                }
                Byte[] fileBytes = System.IO.File.ReadAllBytes(file.FilePath);
                //this returns a byte array of the PDF file content
                if (fileBytes == null)
                    return NotFound();
                var stream = new MemoryStream(fileBytes); //saves it into a stream
                stream.Position = 0;
             
                return File(stream, "application/octec-stream", System.IO.Path.GetFileName(file.FilePath));
            }

            return NotFound();
        }

        [Route("FindAudit/{DownloadedFileID}")]
        [HttpGet("{DownloadedFileID}")]
        public List<DownloadedFileAudit> FindAudit(long DownloadedFileID)
        {
            List<DownloadedFileAudit> data = (from pAudit in _context.DownloadedFileAudit
                                              where pAudit.DownloadedFileID == DownloadedFileID
                                              orderby pAudit.AddedDate descending
                                              select new DownloadedFileAudit()
                                              {
                                                  ID = pAudit.ID,
                                                  DownloadedFileID = pAudit.DownloadedFileID,
                                                  TransactionID = pAudit.TransactionID,
                                                  ColumnName = pAudit.ColumnName,
                                                  CurrentValue = pAudit.CurrentValue,
                                                  NewValue = pAudit.NewValue,
                                                  CurrentValueID = pAudit.CurrentValueID,
                                                  NewValueID = pAudit.NewValueID,
                                                  HostName = pAudit.HostName,
                                                  AddedBy = pAudit.AddedBy,
                                                  AddedDate = pAudit.AddedDate,
                                              }).ToList<DownloadedFileAudit>();

            return data;
        }





    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MediFusionPM.Models;
using MediFusionPM.Models.TodoApi.Models;
using MediFusionPM.ViewModels;
using MediFusionPM.Models.Audit;
//using static MediFusionPM.ViewModels.VMCpt;
namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class ResubmitHistoryController : Controller
    {
        private readonly ClientDbContext _context;

        public ResubmitHistoryController(ClientDbContext context)
        {
            _context = context;

           
        }

        [Route("GetResubmitChargeHistory/{ChargeId}")]
        [HttpGet("{ChargeId}")]
        public List<ResubmitHistory> GetResubmitChargeHistory(long ChargeId)
        {
            List<ResubmitHistory> data = (from rh in _context.ResubmitHistory
                                  
                                   where rh.ChargeID == ChargeId orderby rh.AddedDate descending
                                   select new ResubmitHistory()
                                   {
                                       ID = rh.ID,
                                       ChargeID = rh.ChargeID,
                                       VisitID = rh.VisitID,
                                       AddedBy = rh.AddedBy,
                                       AddedDate = Convert.ToDateTime(rh.AddedDate.Format("MM/dd/yyyy")),
                                       //DateTime.Parse(rh.AddedDate.ToString("MM/dd/yyyy"))
                                       //DateTime.ParseExact(rh.AddedDate.ToString("MM/dd/yyyy"), "MM/dd/yyyy",null)
                                   }).ToList();

            return data;

        }

        [Route("GetResubmitVisitHistory/{VisitID}")]
        [HttpGet("{PatientId}")]
        public List<ResubmitHistory> GetResubmitVisitHistory(long VisitId)
        {
            List<ResubmitHistory> data = (from rh in _context.ResubmitHistory

                                          where rh.VisitID == VisitId
                                          select new ResubmitHistory()
                                          {
                                              ID = rh.ID,
                                              ChargeID = rh.ChargeID,
                                              VisitID = rh.VisitID,
                                              AddedBy = rh.AddedBy,
                                              AddedDate = rh.AddedDate,

                                          }).ToList();

            return data;

        }
        [Route("FindAudit/{ResubmitHistoryID}")]
        [HttpGet("{ResubmitHistoryID}")]
        public List<ResubmitHistoryAudit> FindAudit(long ResubmitHistoryID)
        {
            List<ResubmitHistoryAudit> data = (from pAudit in _context.ResubmitHistoryAudit
                                               where pAudit.ResubmitHistoryID == ResubmitHistoryID
                                               select new ResubmitHistoryAudit()
                                               {
                                                   ID = pAudit.ID,
                                                   ResubmitHistoryID = pAudit.ResubmitHistoryID,
                                                   TransactionID = pAudit.TransactionID,
                                                   ColumnName = pAudit.ColumnName,
                                                   CurrentValue = pAudit.CurrentValue,
                                                   NewValue = pAudit.NewValue,
                                                   CurrentValueID = pAudit.CurrentValueID,
                                                   NewValueID = pAudit.NewValueID,
                                                   HostName = pAudit.HostName,
                                                   AddedBy = pAudit.AddedBy,
                                                   AddedDate = pAudit.AddedDate,
                                               }).ToList<ResubmitHistoryAudit>();
            return data;
        }


    }
}
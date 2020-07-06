using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MediFusionPM.Models;
using MediFusionPM.Models.TodoApi.Models;
using MediFusionPM.ViewModels;
using static MediFusionPM.ViewModels.VMProviderSchedule;
using static MediFusionPM.ViewModels.VMCommon;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class ProviderScheduleController : Controller
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        public ProviderScheduleController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;
           
        }

        [Route("GetProfiles")]
        [HttpGet()]
        public async Task<ActionResult<VMProviderSchedule>> GetProfiles()
        {
            ViewModels.VMProviderSchedule obj = new ViewModels.VMProviderSchedule();
            obj.GetProfiles(_context);

            return obj;
        }
        [HttpGet]
        [Route("GetProviderSchedules")]
        public async Task<ActionResult<IEnumerable<ProviderSchedule>>> GetProviderSchedules()
        {
            return await _context.ProviderSchedule.ToListAsync();
        }

        /*
                [Route("FindProviderSchedule/{id}")]
                [HttpGet("{id}")]
                public async Task<ActionResult<ProviderSchedule>> FindProviderSchedule(long id)
                {
                    var Schedule = await _context.ProviderSchedule.FindAsync(id);

                    if (Schedule == null)
                    {
                        return NotFound();
                    }


                    return Schedule;
                }*/

        [Route("FindProviderSchedule")]
        [HttpGet("FindProviderSchedule")]
        public ActionResult FindProviderSchedule(long providerId, long locationId)
        {
           
            
            // if(ExtensionMethods.IsNull(providerId) ) //|| providerId == 0)
            if(providerId == null)
            {   
                return Json("Please select Provider");
            }
            if (ExtensionMethods.IsNull(locationId)) // || locationId==0)
            {
                return Json( "Please select locaiton");
            }
            string query = @"SELECT case when isnull(prS.ID,0)>0 then cast('true' as bit)  else cast('false' as bit) end as chk, 
                    isnull(prS.dayofWeek,weekday) as dayofWeek ,
                    isnull(prS.ID,ROW_NUMBER() OVER (  order by prs.dayofWeek  )) ID,
                    isnull(prS.ProviderID,0) as ProviderID,
                    isnull(prS.LocationID,0) as LocationID ,
                    convert(Datetime,  FromTime ) FromTime, 
                    convert(Datetime,  ToTime ) ToTime,  
                    isnull(prS.TimeInterval,0) as TimeInterval ,
                    isnull(prS.OverBookAllowed,0) as OverBookAllowed,
                    prS.Notes,
                    prS.AddedBy,
                    prS.AddedDate,
                    prS.UpdatedBy,
                    prS.UpdatedDate ,   
                    isnull(prs.InActive,0) InActive,
                    convert(Datetime,  breakfrom ) breakfrom,  
                    convert(Datetime,  breakto ) breakto 
                    FROM (
                    VALUES ('MONDAY') ,('TUESDAY') ,('WEDNESDAY') ,('THURSDAY') ,('FRIDAY') ,('SATURDAY') ,('SUNDAY')  
                    ) t(weekDay)  left outer join 
                     providerschedule prS on prS.dayofWeek=weekDay and  ProviderID=" + providerId + " and LocationID=" + locationId + @"ORDER BY
                     CASE WHEN weekday='Monday' THEN 1
		             WHEN weekday='Tuesday' THEN 2
		             WHEN weekday='Wednesday' THEN 3
		             WHEN weekday='Thursday' THEN 4
		             WHEN weekday='Friday' THEN 5
		             WHEN weekday='Saturday' THEN 6
		             WHEN weekday='Sunday' THEN 7
                     END";
            var cNames = _context.ProviderSchedule.FromSql(query).ToList();
            return Json(cNames);
            // return blogs;
        }

        [Route("SaveProviderSchedule")]
        [HttpPost]
        public async Task<ActionResult<ProviderSchedule>> SaveProviderSchedule(IEnumerable<ProviderSchedule> providerSchedule)
        {
          UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
          User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
          User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
          if (UD == null || UD.Rights == null || UD.Rights.SchedulerSearch == false)
          return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            bool succ = TryValidateModel(providerSchedule);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            if (UD.Rights.SchedulerSearch == true)
            {
                foreach (ProviderSchedule orm in providerSchedule)
                {
                    orm.UpdatedBy = UD.Email;
                    orm.UpdatedDate = DateTime.Now;
                    if (orm.ID == 0)
                    {
                        orm.AddedBy = UD.Email;
                        orm.AddedDate = DateTime.Now;
                        _context.ProviderSchedule.Add(orm);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        //_context.ProviderSchedule.Update(orm);
                        _context.ProviderSchedule.Update(orm);
                        _context.Entry(orm).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }

                }
            
                //await _context.SaveChangesAsync();
            }
            else
                return BadRequest("You Don't Have Rights. Please Contact BellMedex.");

        
            return Ok("");
           
        }
    
        [HttpPost]
        [Route("FindProviderSchedules")]
        public async Task<ActionResult<IEnumerable<GProviderSchedule>>> FindProviderSchedules(CProviderSchedule CProviderSchedule)
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.SchedulerSearch == false)
            //return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindProviderSchedules(CProviderSchedule, PracticeId);
        }
        private List<GProviderSchedule> FindProviderSchedules(CProviderSchedule CProviderSchedule, long PracticeId)
        {
            
            List<GProviderSchedule> data = (from ps in _context.ProviderSchedule
                                            join pro in _context.Provider on ps.ProviderID equals pro.ID
                                            join loc in _context.Location on ps.LocationID equals loc.ID into Table2
                                            from t2 in Table2.DefaultIfEmpty()
                                            join prac in _context.Practice on pro.PracticeID equals prac.ID
                                            //join up in _context.UserPractices on prac.ID equals up.PracticeID
                                            //join u in _context.Users on up.UserID equals u.Id
                                            where prac.ID == PracticeId && 
                                            //u.Id.ToString() == UD.UserID &&
                                           (CProviderSchedule.ProviderID.IsNull() ? true : pro.ID.Equals(CProviderSchedule.ProviderID)) &&
                                           (CProviderSchedule.LocationID.IsNull() ? true : t2.ID.Equals(CProviderSchedule.LocationID))
                                           //(IsBetweenDOS(CProviderSchedule.ToDate, CProviderSchedule.FromDate, ps.FromTime, v.DateOfServiceFrom))
                                            //  (CProviderSchedule.FromDate != null && CProviderSchedule.ToDate != null ?
                                            //  ((DateTime)ps.FromDate).Date >= CProviderSchedule.FromDate && ((DateTime)ps.ToDate).Date <= CProviderSchedule.ToDate
                                            //: (CProviderSchedule.FromDate != null ? ((DateTime)ps.FromDate).Date >= CProviderSchedule.FromDate : true))

                                            select new GProviderSchedule()
                                            {
                                                ID = ps.ID,
                                                Provider = pro.Name,
                                                ProviderID = pro.ID,
                                                Location = t2.Name,
                                                LocationID = t2.ID,
                                                TimeInterval = ps.TimeInterval.ToString(),
                                                FromTime = ps.FromTime.ToString(),
                                                ToTime = ps.ToTime.ToString(),
                                            }).ToList();
            return data;
        }

        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CProviderSchedule CProviderSchedule)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GProviderSchedule> data = null;// FindProviderSchedules(CProviderSchedule, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CProviderSchedule, "Provider Schedule Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CProviderSchedule CProviderSchedule)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
             );
            List<GProviderSchedule> data = null;// FindProviderSchedules(CProviderSchedule, UD);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);
        }

        /*
        [HttpPost]
        [Route("FindProviderSlots")]
        public async Task<ActionResult<IEnumerable<GProviderSchedule>>> FindProviderSlots(CProviderSchedule CProviderSchedule)
        {
            return await (from pSlot in _context.ProviderSlot
                          join pApp in _context.PatientAppointment on pSlot.ID equals pApp.ProviderSlotID into Table2
                          from t2 in Table2.DefaultIfEmpty()
                          join loc in _context.Location on t2.LocationID equals loc.ID into Table3
                          from t3 in Table3.DefaultIfEmpty()
                          join pro in _context.Provider on t2.ProviderID equals pro.ID

                          where
                         (CProviderSchedule.ProviderID.IsNull() ? true : pro.ID.Equals(CProviderSchedule.ProviderID)) &&
                         (CProviderSchedule.LocationID.IsNull() ? true : t3.ID.Equals(CProviderSchedule.LocationID)) &&
                         (CProviderSchedule.FromDate != null && CProviderSchedule.ToDate != null ?
                         ((DateTime)pSlot.FromDate).Date >= CProviderSchedule.FromDate && ((DateTime)pSlot.ToDate).Date <= CProviderSchedule.ToDate
                       : (CProviderSchedule.FromDate != null ? ((DateTime)pSlot.FromDate).Date >= CProviderSchedule.FromDate : true))

                          select new GProviderSchedule()
                          {
                              ID = pSlot.ID,
                              AppointmentID = t2.ID,
                              AppointmentDate = t2.AppointmentDate.Format("MM/dd/yyyy"),
                              Provider = pro.Name,
                              ProviderID = pro.ID,
                              LocationID = t3.ID,
                              Location = t3.Name,
                              FromTime = pSlot.FromTime.ToString(),
                              ToTime = pSlot.ToTime.ToString(),
                              TimeInterval = pSlot.TimeInterval.ToString(),
                              AppointmentStatus = pSlot.Status,
                              //For Testing
                              FromDate = pSlot.FromDate.Format("MM/dd/yyyy"),
                              ToDate = pSlot.ToDate.Format("MM/dd/yyyy"),


                          }).ToListAsync();
        }


        [Route("SaveProviderSchedule")]
        [HttpPost]
        public async Task<ActionResult<ProviderSchedule>> SaveProviderSchedule(ProviderSchedule item)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (UD == null || UD.Rights == null || UD.Rights.SchedulerCreate == false)
                return BadRequest("You Don't Have Rights. Please Contact BellMedex.");
            bool succ = TryValidateModel(item);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            //List<ProviderSlot> slot = _context.ProviderSchedule.Where(v => v.FromDate >= item.FromDate && v.ToDate <= item.ToDate && v.ProviderID == item.ProviderID).ToList<ProviderSchedule>();
            // if (slot.Count > 0)
            // {
            //     return BadRequest("Schedule Already Exists");
            // }
            var transactionOption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (var objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption, TransactionScopeAsyncFlowOption.Enabled))
                if (item.ID == 0)
                {
                    item.AddedBy = UD.Email;
                    item.AddedDate = DateTime.Now;
                    _context.ProviderSchedule.Add(item);
                    await _context.SaveChangesAsync();
                    //var LastID = item.ID;
                    List<ProviderSchedule> data = (from table in _context.ProviderSchedule where table.ID == item.ID select table).ToList();

                    foreach (ProviderSchedule ProviderSchedule in data)
                    {

                        for (DateTime Temp = ProviderSchedule.FromDate.Value; Temp <= ProviderSchedule.ToDate.Value; Temp = Temp.AddDays(1))
                        {
                            try
                            {

                                string Day = Temp.ToString("ddd");
                                bool ValidDay = false;

                                switch (Day)
                                {
                                    case "Mon":
                                        if (ProviderSchedule.Monday)
                                        {
                                            ValidDay = true;
                                            Debug.WriteLine(Day);
                                        }
                                        break;
                                    case "Tue":
                                        if (ProviderSchedule.Tuesday)
                                        {
                                            ValidDay = true;
                                            Debug.WriteLine(Day);
                                        }
                                        break;
                                    case "Wed":
                                        if (ProviderSchedule.Wednesday)
                                        {
                                            ValidDay = true;
                                            Debug.WriteLine(Day);
                                        }
                                        break;
                                    case "Thu":
                                        if (ProviderSchedule.Thursday)
                                        {
                                            ValidDay = true;
                                            Debug.WriteLine(Day);
                                        }
                                        break;
                                    case "Fri":
                                        if (ProviderSchedule.Friday)
                                        {
                                            ValidDay = true;
                                            Debug.WriteLine(Day);
                                        }
                                        break;
                                    case "Sat":
                                        if (ProviderSchedule.Saturday)
                                        {
                                            ValidDay = true;
                                            Debug.WriteLine(Day);
                                        }
                                        break;
                                    case "Sun":
                                        if (ProviderSchedule.Sunday)
                                        {
                                            ValidDay = true;
                                            Debug.WriteLine(Day);
                                        }
                                        break;

                                }
                                if (ValidDay)
                                {

                                    DateTime StartDateTimeForSpan = Convert.ToDateTime(ProviderSchedule.FromTime.Value.TimeOfDay.ToString());
                                    DateTime EndDateTimeForSpan = Convert.ToDateTime(ProviderSchedule.ToTime.Value.TimeOfDay.ToString());

                                    DateTime SubIntervalStartTime = StartDateTimeForSpan;

                                    for (DateTime SubIntervalEndTime = StartDateTimeForSpan; SubIntervalEndTime <= EndDateTimeForSpan; SubIntervalEndTime = SubIntervalEndTime.AddMinutes(ProviderSchedule.TimeInterval))
                                    {


                                        if (SubIntervalStartTime != SubIntervalEndTime)
                                        {
                                            ProviderSlot Slot = new ProviderSlot();
                                            Slot.ProviderScheduleID = ProviderSchedule.ID;
                                            Slot.FromDate = Convert.ToDateTime(Temp.Year + "-" + Temp.Month + "-" + Temp.Day);
                                            Slot.ToDate = Convert.ToDateTime(Temp.Year + "-" + Temp.Month + "-" + Temp.Day);
                                            Slot.TimeInterval = ProviderSchedule.TimeInterval;
                                            Slot.FromTime = Convert.ToDateTime(SubIntervalStartTime.TimeOfDay.ToString());
                                            Slot.ToTime = Convert.ToDateTime(SubIntervalEndTime.TimeOfDay.ToString());
                                            Slot.Status = "A";
                                            Slot.AddedDate = DateTime.Today;

                                            //ProviderScheduleModel DbModel = new ProviderScheduleModel();
                                            _context.ProviderSlot.Add(Slot);
                                            _context.SaveChanges();
                                        }
                                        SubIntervalStartTime = SubIntervalEndTime;
                                    }
                                }
                                // await _context.SaveChangesAsync();
                                objTrnScope.Complete();
                                objTrnScope.Dispose();
                            }

                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message + "  " + ex.StackTrace);
                            }
                        }
                    }



                }
            return Ok(item);
        }
        */

        [Route("DeleteProviderSchedule/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteProviderSchedule(long id)
        {
            var DeleteSchedule = await _context.ProviderSchedule.FindAsync(id);

            if (DeleteSchedule == null)
            {
                return NotFound();
            }

            _context.ProviderSchedule.Remove(DeleteSchedule);
            await _context.SaveChangesAsync();

            return Ok();
        }
        [Route("FindAudit/{ProviderScheduleID}")]
        [HttpGet("{ProviderScheduleID}")]
        public List<ProviderScheduleAudit> FindAudit(long ProviderScheduleID)
        {
            List<ProviderScheduleAudit> data = (from pAudit in _context.ProviderScheduleAudit
                                                where pAudit.ProviderScheduleID == ProviderScheduleID
                                                orderby pAudit.AddedDate descending
                                                select new ProviderScheduleAudit()
                                                {
                                                    ID = pAudit.ID,
                                                    ProviderScheduleID = pAudit.ProviderScheduleID,
                                                    TransactionID = pAudit.TransactionID,
                                                    ColumnName = pAudit.ColumnName,
                                                    CurrentValue = pAudit.CurrentValue,
                                                    NewValue = pAudit.NewValue,
                                                    CurrentValueID = pAudit.CurrentValueID,
                                                    NewValueID = pAudit.NewValueID,
                                                    HostName = pAudit.HostName,
                                                    AddedBy = pAudit.AddedBy,
                                                    AddedDate = pAudit.AddedDate,
                                                }).ToList<ProviderScheduleAudit>();
            return data;
        }

    }
}
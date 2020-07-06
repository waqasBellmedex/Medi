using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;
using MediFusionPM.Models.Audit;
using MediFusionPM.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static MediFusionPM.ViewModels.VMCommon;
using static MediFusionPM.ViewModels.VMCOnlinePortals;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortalController : ControllerBase
    {

        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;

        public PortalController(ClientDbContext context, MainContext contextMain)
        {
            _context = context;
            _contextMain = contextMain;

        }

        [Route("FindOnlinePortal/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<OnlinePortals>> FindPatient(long id)
        {
            var portal = await _context.OnlinePortals.FindAsync(id);
            if (portal == null)
            {
                return BadRequest("Record Not Found.");
            }
            portal.OnlinePortalCredentials = _context.OnlinePortalCredentials.Where(m => m.OnlinePortalsID == id).ToList<OnlinePortalCredentials>();
            return portal;
        }


        [Route("SaveOnlinePortals")]
        [HttpPost]
        public async Task<ActionResult<OnlinePortals>> SaveOnlinePortals(OnlinePortals item)
        {
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            bool succ = TryValidateModel(item);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            if (item.ID == 0)
            {
                item.AddedBy = Email;
                item.AddedDate = DateTime.Now;
                _context.OnlinePortals.Add(item);
                if (item.OnlinePortalCredentials != null)
                {
                    foreach (OnlinePortalCredentials online in item.OnlinePortalCredentials)
                    {
                        if (online.ID <= 0)
                        {
                            online.AddedBy = Email;
                            online.AddedDate = DateTime.Now;
                            _context.OnlinePortalCredentials.Add(online);
                        }

                    }
                }

            }
           else
            {
                item.UpdatedBy = Email;
                item.UpdatedDate = DateTime.Now;
                _context.OnlinePortals.Update(item);

                if(item.OnlinePortalCredentials != null )
                {
                    foreach (OnlinePortalCredentials online in item.OnlinePortalCredentials)
                    {
                        if (online.ID <= 0)
                        {
                            online.AddedBy = Email;
                            online.AddedDate = DateTime.Now;
                            _context.OnlinePortalCredentials.Add(online);
                        }
                        else
                        {
                            online.UpdatedBy = Email;
                            online.UpdatedDate = DateTime.Now;
                            _context.OnlinePortalCredentials.Update(online);
                        }
                    }

                }
            }
            _context.SaveChanges();
            return Ok(item);
        }


        [HttpPost]
        [Route("FindOnlinePortal")]
        public async Task<ActionResult<IEnumerable<GOnlinePortal>>> FindOnlinePortal(COnlinePortal COnlinePortal)
        {
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindOnlinePortal(COnlinePortal, PracticeId);

        }
        //private List<GOnlinePortal> FindOnlinePortal(COnlinePortal COnlinePortal, UserInfoData UD)
        //    {
        //        List<GOnlinePortal> data = (from op in _context.OnlinePortals
        //                                    join iPlan in _context.InsurancePlan on op.InsurancePlanID equals iPlan.ID into InsurancePlan from iPlan in InsurancePlan.DefaultIfEmpty()
        //                                    where
        //                                    (COnlinePortal.Name.IsNull() ? true : op.Name.Contains(COnlinePortal.Name))
        //                                  &&(COnlinePortal.PortalType.IsNull() ? true : op.Type.Contains(COnlinePortal.PortalType))
        //                                    select new GOnlinePortal()
        //                                    {
        //                                      ID = op.ID,
        //                                      PortalType = op.Type,
        //                                      Name = op.Name,
        //                                      URL = op.URL,
        //                                      InsurancePlan = iPlan.PlanName,
        //                                    })
        //            .ToList();
        //          return data;
        //        //return Ok(GOnlinePortal);
        //    }
      
        private List<GOnlinePortal> FindOnlinePortal(COnlinePortal COnlinePortal, long PracticeId)
        {
            List<GOnlinePortal> data = (from op in _context.OnlinePortals
                                        join iPlan in _context.InsurancePlan on op.InsurancePlanID equals iPlan.ID into InsurancePlan
                                        from iPlan in InsurancePlan.DefaultIfEmpty()
                                        where
                                        (COnlinePortal.Name.IsNull() ? true : op.Name.Contains(COnlinePortal.Name))
                                      && (COnlinePortal.PortalType.IsNull() ? true : op.Type.Contains(COnlinePortal.PortalType))
                                        select new GOnlinePortal()
                                        {
                                            ID = op.ID,
                                            PortalType = op.Type,
                                            Name = op.Name,
                                            URL = op.URL,
                                            InsurancePlan = iPlan.PlanName,
                                            Username = op.OnlinePortalCredentials.FirstOrDefault().Username,
                                            Password = op.OnlinePortalCredentials.FirstOrDefault().Password,
                                        })
                .ToList();
            return data;
        }


        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(COnlinePortal COnlinePortal)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GOnlinePortal> data = FindOnlinePortal(COnlinePortal, PracticeId);
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, COnlinePortal, "Online Portal Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(COnlinePortal COnlinePortal)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GOnlinePortal> data = FindOnlinePortal(COnlinePortal, PracticeId);
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }
        [Route("DeletePortal/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeletePortal(long id)
        {
            var portal = await _context.OnlinePortals.FindAsync(id);
            if (portal == null)
            {
                return BadRequest("Record Not Found");
            }

            // Delete OnlinePortalCredentials .
            List<OnlinePortalCredentials> data = (from op in _context.OnlinePortalCredentials where op.OnlinePortalsID == id select op).ToList();
            if (data.Count > 0)
            {
                foreach (OnlinePortalCredentials creadentail in data)
                {
                    _context.OnlinePortalCredentials.Remove(creadentail);
                }
            }

            _context.OnlinePortals.Remove(portal);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Route("DeletePortalCredential/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeletePortalCredentials(long id)
        {
            var credential = await _context.OnlinePortalCredentials.FindAsync(id);
            if (credential == null)
            {
                return BadRequest("Record Not Found");
            }

            _context.OnlinePortalCredentials.Remove(credential);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Route("FindAudit/{OnlinePortalsID}")]
        [HttpGet("{OnlinePortalsID}")]
        public List<OnlinePortalsAudit> FindAudit(long OnlinePortalsID)
        {
            List<OnlinePortalsAudit> data = (from pAudit in _context.OnlinePortalsAudit
                                      where pAudit.OnlinePortalsID == OnlinePortalsID
                                      orderby pAudit.AddedDate descending
                                      select new OnlinePortalsAudit()
                                      {
                                          ID = pAudit.ID,
                                          OnlinePortalsID = pAudit.OnlinePortalsID,
                                          TransactionID = pAudit.TransactionID,
                                          ColumnName = pAudit.ColumnName,
                                          CurrentValue = pAudit.CurrentValue,
                                          NewValue = pAudit.NewValue,
                                          CurrentValueID = pAudit.CurrentValueID,
                                          NewValueID = pAudit.NewValueID,
                                          HostName = pAudit.HostName,
                                          AddedBy = pAudit.AddedBy,
                                          AddedDate = pAudit.AddedDate,
                                      }).ToList<OnlinePortalsAudit>();
            return data;
        }

        [Route("FindPortalCredentialsAudit/{OnlinePortalsID}")]
        [HttpGet("{OnlinePortalsID}")]
        public List<OnlinePortalCredentialsAudit> FindPortalCredentialsAudit(long OnlinePortalsID)
        {
            List<OnlinePortalCredentialsAudit> data = (from pAudit in _context.OnlinePortalCredentialsAudit
                                                       where pAudit.OnlinePortalsID == OnlinePortalsID
                                             orderby pAudit.AddedDate descending
                                             select new OnlinePortalCredentialsAudit()
                                             {
                                                 ID = pAudit.ID,
                                                 OnlinePortalCredentialsID = pAudit.OnlinePortalCredentialsID,
                                                 OnlinePortalsID = pAudit.OnlinePortalsID,
                                                 TransactionID = pAudit.TransactionID,
                                                 ColumnName = pAudit.ColumnName,
                                                 CurrentValue = pAudit.CurrentValue,
                                                 NewValue = pAudit.NewValue,
                                                 CurrentValueID = pAudit.CurrentValueID,
                                                 NewValueID = pAudit.NewValueID,
                                                 HostName = pAudit.HostName,
                                                 AddedBy = pAudit.AddedBy,
                                                 AddedDate = pAudit.AddedDate,
                                             }).ToList<OnlinePortalCredentialsAudit>();
            return data;
        }



    }
}





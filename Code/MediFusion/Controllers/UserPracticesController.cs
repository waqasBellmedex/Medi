using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediFusionPM.Models;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using MediFusionPM.ViewModels;
using MediFusionPM.ViewModel;
using static MediFusionPM.ViewModels.VMPractice;
using System.Transactions;
using MediFusionPM.Models.Main;
using MediFusionPM.Models.Audit;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserPracticesController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        public UserPracticesController(ClientDbContext contextClient, MainContext contextMain)
        {
            _context = contextClient;
            _contextMain = contextMain;
        }

        [HttpGet]
        [Route("FindUserPractices")]
        public async Task<ActionResult<IEnumerable<GPractice>>> FindUserPractices()
        {

            var UserId = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Jti));

            return (from u in _contextMain.MainUserPractices
                    join v in _contextMain.Users
                    on u.UserID equals v.Id
                    join w in _contextMain.MainPractice
                    on u.PracticeID equals w.ID
                    where u.UserID == UserId.Value && u.Status == true
                    select new GPractice()

                    {
                        ID = w.ID,
                        Name = w.Name,
                    }).ToList();

        }


        // GET: api/UserPractices/5
        [HttpGet]
        public async Task<ActionResult<UserPractices>> GetUserPractices()
        {
            var UserId = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Jti));
            var Praclist = (from u in _contextMain.MainUserPractices
                            join v in _contextMain.MainPractice
                            on u.PracticeID equals v.ID
                            join w in _contextMain.Users
                            on u.UserID equals w.Id
                            where u.UserID == UserId.Value && u.Status == true
                            select new
                            {
                                w.Email,
                                v.Name
                            }).ToList(); ;
            if (Praclist == null)
            {
                return BadRequest("Not Found");
            }

            return Ok(Praclist);
        }

        // POST: api/UserPractices
        [HttpPost]
        [Route("AssignPractices")]
        public async Task<ActionResult<MainUserPractices>> AssignPractices(IEnumerable<VMUserPractices> vmUserPractices)
        //public async Task<ActionResult<MainUserPractices>> AssignPractices(VMUserPractices vmUserPractices)
        {

            if (vmUserPractices == null || vmUserPractices.Count() == 0)
                return BadRequest("No Practice(s) Found");

            string email = vmUserPractices.FirstOrDefault().Email;
            //string email = vmUserPractices[0].Email;
            if (email.IsNull())
                return BadRequest("User not Found");

            //Check user Which Email send
            var AssUser = (from u in _contextMain.Users
                           where u.Email == email
                           select u
                        ).FirstOrDefault();

            if (AssUser == null)
                return BadRequest("User not Found");

            try
            {

                //var transactionOption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
                //using (var objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption, TransactionScopeAsyncFlowOption.Enabled))
                //{
                //    try
                //    {
                var LoginUserId = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Jti));
                MainUserPractices userPractices = new MainUserPractices();
                UserPractices CliUserPractices = new UserPractices();
                foreach (var item in vmUserPractices)
                {

                    var q = (from v in _contextMain.MainPractice
                             where v.ID == item.PracticeId
                             select new { v.ID }).FirstOrDefault();
                    if (q != null)
                    {
                        var compare = (from w in _contextMain.MainUserPractices
                                       where w.UserID == AssUser.Id && w.PracticeID == item.PracticeId
                                       select w
                                       ).FirstOrDefault();
                        if (compare == null)
                        {
                            userPractices.UserID = AssUser.Id;
                            userPractices.AssignedByUserId = LoginUserId.Value.ToString();
                            userPractices.PracticeID = item.PracticeId;
                            userPractices.Status = item.Status;
                            _contextMain.MainUserPractices.Add(userPractices);
                            await _contextMain.SaveChangesAsync();
                            CliUserPractices.UserID = AssUser.Id;
                            CliUserPractices.AssignedByUserId = LoginUserId.Value.ToString();
                            CliUserPractices.PracticeID = item.PracticeId;
                            CliUserPractices.Status = item.Status;
                            _context.UserPractices.Add(CliUserPractices);
                            await _context.SaveChangesAsync();

                        }
                        else
                        {
                            continue;
                            compare.Status = item.Status;
                            compare.UPALastModified = LoginUserId.Value;
                            _contextMain.Entry(compare).State = EntityState.Modified;
                            await _contextMain.SaveChangesAsync();

                            var comp = (from w in _context.UserPractices
                                        where w.UserID == AssUser.Id && w.PracticeID == item.PracticeId
                                        select w
                                               ).FirstOrDefault();
                            comp.Status = item.Status;
                            comp.UPALastModified = LoginUserId.Value;
                            _context.Entry(comp).State = EntityState.Modified;
                            await _context.SaveChangesAsync();

                        }

                    }

                }
                //await _contextMain.SaveChangesAsync();
                //await _context.SaveChangesAsync();
                //    objTrnScope.Complete();
                //    objTrnScope.Dispose();
                //}
                //catch (Exception ex)
                //{
                //    throw ex;
                //}
                //finally
                //{

                //}
                // }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }

            return Ok(vmUserPractices);

        }




        [Route("FindAudit/{UserID}")]
        [HttpGet("{UserID}")]
        public List<UserPracticeAudit> FindAudit(string UserID)
        {
            List<UserPracticeAudit> data = (from pAudit in _context.UserPracticeAudit
                                            where pAudit.UserID == UserID
                                            select new UserPracticeAudit()
                                            {
                                                ID = pAudit.ID,
                                                UserID = pAudit.UserID,
                                                TransactionID = pAudit.TransactionID,
                                                ColumnName = pAudit.ColumnName,
                                                CurrentValue = pAudit.CurrentValue,
                                                NewValue = pAudit.NewValue,
                                                CurrentValueID = pAudit.CurrentValueID,
                                                NewValueID = pAudit.NewValueID,
                                                HostName = pAudit.HostName,
                                                AddedBy = pAudit.AddedBy,
                                                AddedDate = pAudit.AddedDate,
                                            }).ToList<UserPracticeAudit>();
            return data;
        }

        
        

    }

}

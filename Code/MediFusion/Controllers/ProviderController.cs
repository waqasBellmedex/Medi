using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
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
using static MediFusionPM.ViewModels.VMCommon;
using static MediFusionPM.ViewModels.VMProvider;
using MediFusionPM.Models.Audit;
using System.IO;
using System.Transactions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class ProviderController : ControllerBase
    {
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;

        public ProviderController(ClientDbContext context, MainContext contextMain)    
        {
            _context = context;
            _contextMain = contextMain;

            // Only For Testing
            if (_context.Provider.Count() == 0)
            {
           //     _context.Providers.Add(new Provider { ID = 1, Name = "ABC Provider", NPI = "1234567891" });
             //   _context.SaveChanges();
            }
        }

        [HttpGet]
        [Route("GetProviders")]
        public async Task<ActionResult<IEnumerable<Provider>>> GetProviders()
        {

            return await _context.Provider.ToListAsync();
        }

        [Route("FindProvider/{id}")]
        [HttpGet("{id}")]

        public async Task<ActionResult<Provider>> FindProvider(long id)
        {
            var Provider = await _context.Provider.FindAsync(id);
            if (Provider == null)
            {
                return NotFound();
            }
            Provider.InsuranceBillingoption = _context.InsuranceBillingoption.Where(m => m.ProviderID == id).ToList<InsuranceBillingoption>();
           
                            
            //Provider.ProviderSchedule = _context.ProviderSchedule.Where(m => m.ProviderID == id ).ToList<ProviderSchedule>();
            return Provider;
        }

        [HttpPost]
        [Route("FindProviders")]
        public async Task<ActionResult<IEnumerable<GProvider>>> FindProviders(CProvider CProvider)
        {

            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //if (UD == null || UD.Rights == null || UD.Rights.ProviderSearch == false)
            //return BadRequest("You Don't Have Rights. Please Contact BellMedex.");

            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindProviders(CProvider, PracticeId);
        }

        private List<GProvider> FindProviders(CProvider CProvider, long PracticeId)
        { 
            List<GProvider> data =  (from p in _context.Provider
                    //join prac in _context.Practice
                    //on p.PracticeID equals prac.ID
                    //join up in _context.UserPractices
                    //on prac.ID equals up.PracticeID
                    //join u in _context.Users
                    //on up.UserID equals u.Id
                    where
                    p.PracticeID == PracticeId &&
                    //&& u.Id.ToString() == UD.UserID
                    //&& u.IsUserBlock == false &&

                   (CProvider.Name.IsNull() ? true : p.Name.Contains(CProvider.Name)) &&
                   (CProvider.FirstName.IsNull() ? true : p.FirstName.Contains(CProvider.FirstName)) &&
                   (CProvider.LastName.IsNull() ? true : p.LastName.Contains(CProvider.LastName)) &&
                   (CProvider.NPI.IsNull() ? true : p.NPI == CProvider.NPI) &&
                   (CProvider.SSN.IsNull() ? true : p.SSN == CProvider.SSN) &&
                   (CProvider.TaxonomyCode.IsNull() ? true : p.TaxonomyCode == CProvider.TaxonomyCode)
                select new GProvider
                { ID = p.ID,
                    Name = p.Name,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    NPI = p.NPI,
                    SSN = p.SSN,
                    TaxonomyCode = p.TaxonomyCode,
                    Address = p.Address1 + ", " + p.City + ", " + p.State + ", " + p.ZipCode,
                    OfficePhoneNum = p.OfficePhoneNum

                })
                .ToList();
                return data;
        }
        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CProvider CProvider)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            //  Debug.WriteLine(UD + " UD " + UD.ClientID + "   " + _context);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GProvider> data = FindProviders(CProvider, PracticeId);
            Debug.WriteLine(data + "    is the list");
            ExportController controller = new ExportController(_context);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CProvider, "Provider Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CProvider CProvider)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            // Debug.WriteLine(UD + " UD " + UD.ClientID + "   " + _context);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GProvider> data = FindProviders(CProvider, PracticeId);
            Debug.WriteLine(data + "    is the list");
            ExportController controller = new ExportController(_context);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);

        }

        [Route("SaveProvider")]
        [HttpPost]
        public async Task<ActionResult<Provider>> SaveProvider(Provider item)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (UD == null || UD.Rights == null || UD.Rights.ProviderCreate == false)
            return BadRequest("You Don't Have Rights. Please Contact BellMedex.");

            bool succ = TryValidateModel(item);
            bool ProviderExists = _context.Provider.Count(p => p.Name == item.Name && item.ID == 0) > 0;
            if (ProviderExists)
            {
                return BadRequest("Provider With This Name Already Exists");
            }

            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            var transactionOption = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
            using (var objTrnScope = new TransactionScope(TransactionScopeOption.Required, transactionOption, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    if (item.ID == 0)
                    {
                        item.AddedBy = UD.Email;
                        item.AddedDate = DateTime.Now;
                        _context.Provider.Add(item);
                        // await _context.SaveChangesAsync();
                        if (item.InsuranceBillingoption != null)
                        {
                            foreach (InsuranceBillingoption insBilling in item.InsuranceBillingoption)
                            {
                                insBilling.AddedBy = UD.Email;
                                insBilling.AddedDate = DateTime.Now;
                                insBilling.ProviderID = item.ID;
                                _context.Add(insBilling);
                            }
                        }
                        if (item.ProviderSchedule != null )
                        {
                            foreach (ProviderSchedule ps in item.ProviderSchedule)
                            {
                                if (ps.FromTime != null)
                                {
                                    ps.AddedBy = UD.Email;
                                    ps.AddedDate = DateTime.Now;
                                    ps.ProviderID = item.ID;
                                    _context.Add(ps);
                                }
                            }

                        }
                    }
                    else if (UD.Rights.ProviderEdit == true)
                    {
                        bool ProviderExistsUpdate = _context.Provider.Any(p => p.Name == item.Name && item.ID != p.ID);

                        if (ProviderExistsUpdate == true)
                        {
                            return BadRequest("Already Exists Please Enter Different Provider Name");
                        }
                        item.UpdatedBy = UD.Email;
                        item.UpdatedDate = DateTime.Now;
                        _context.Provider.Update(item);

                        if (item.InsuranceBillingoption != null)
                        {
                            foreach (InsuranceBillingoption insBilling in item.InsuranceBillingoption)
                            {
                                if (insBilling.ID <= 0)
                                {
                                    insBilling.AddedBy = UD.Email;
                                    insBilling.AddedDate = DateTime.Now;
                                    insBilling.ProviderID = item.ID;
                                    _context.Add(insBilling);

                                }
                                else
                                {
                                    insBilling.ProviderID = item.ID;
                                    insBilling.UpdatedBy = UD.Email;
                                    insBilling.UpdatedDate = DateTime.Now;
                                    _context.Update(insBilling);

                                }
                            }
                        }
                        if (item.ProviderSchedule != null)
                        {
                            foreach (ProviderSchedule ps in item.ProviderSchedule)
                            {
                                if (ps.chk == true)
                                {
                                    if (ps.ID <= 0)
                                    {
                                        ps.AddedBy = UD.Email;
                                        ps.AddedDate = DateTime.Now;
                                        ps.ProviderID = item.ID;
                                        _context.Add(ps);

                                    }
                                    else
                                    {
                                        ps.ProviderID = item.ID;
                                        ps.UpdatedBy = UD.Email;
                                        ps.UpdatedDate = DateTime.Now;
                                        _context.Update(ps);
                                    }
                                }
                                else if (ps.ID > 7 && ps.FromTime != null && ps.InActive == false )
                                {
                                    ps.InActive = true;
                                    ps.UpdatedBy = UD.Email;
                                    ps.UpdatedDate = DateTime.Now;
                                    _context.Update(ps);

                                }
                               // await _context.SaveChangesAsync();
                            }
                        } // Ending of  Else IF
                    }
                    else return BadRequest("You Don't Have Rights. Please Contact BellMedex.");

                     await _context.SaveChangesAsync();
                    objTrnScope.Complete();
                    objTrnScope.Dispose();
                }
                catch (Exception ex)
                {
                    System.IO.File.WriteAllText(Path.Combine(_context.env.ContentRootPath, "Logs", "ProviderSave.txt"), ex.ToString());
                    throw ex;

                }
                finally
                {

                }
            }
            return Ok(item);
        }



        [Route("DeleteProvider/{id}")]
        [HttpDelete]

        public async Task<ActionResult> DeleteProvider(long id)
        {
            var Provider = await _context.Provider.FindAsync(id);

            if (Provider == null)
            {
                return NotFound();
            }

            _context.Provider.Remove(Provider);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [Route("FindAudit/{ProviderID}")]
        [HttpGet("{ProviderID}")]
        public List<ProviderAudit> FindAudit(long ProviderID)
        {
            List<ProviderAudit> data = (from pAudit in _context.ProviderAudit
                                        where pAudit.ProviderID == ProviderID
                                        orderby pAudit.AddedDate descending
                                        select new ProviderAudit()
                                        {
                                            ID = pAudit.ID,
                                            ProviderID = pAudit.ProviderID,
                                            TransactionID = pAudit.TransactionID,
                                            ColumnName = pAudit.ColumnName,
                                            CurrentValue = pAudit.CurrentValue,
                                            NewValue = pAudit.NewValue,
                                            CurrentValueID = pAudit.CurrentValueID,
                                            NewValueID = pAudit.NewValueID,
                                            HostName = pAudit.HostName,
                                            AddedBy = pAudit.AddedBy,
                                            AddedDate = pAudit.AddedDate,
                                        }).ToList<ProviderAudit>();
            return data;
        }
    }
}

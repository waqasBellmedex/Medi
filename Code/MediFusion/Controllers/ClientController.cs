using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediFusionPM.Models;
using Microsoft.AspNetCore.Authorization;
using static MediFusionPM.ViewModels.VMClient;
using System.IdentityModel.Tokens.Jwt;
using static MediFusionPM.ViewModels.VMCommon;
using MediFusionPM.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using MediFusionPM.Models.Main;
using MediFusionPM.ViewModel;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.IO;
using System.Transactions;
using MediFusionPM.Models.Audit;
using System.Text;
using System.IO.Compression;
using System.Diagnostics;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientController : ControllerBase
    {
        private readonly ClientDbContext _context_client;
        private readonly MainContext _context_main;
        public readonly IHostingEnvironment env;
        public IConfiguration Configuration { get; }

        public ClientController(ClientDbContext contextClient, MainContext contextMain, IConfiguration configuration, IHostingEnvironment env)
        {
            this.env = env;
            _context_main = contextMain;
            _context_client = contextClient;
            Configuration = configuration;
        }

        // GET: api/Client
        [HttpGet]
        [Route("GetClients")]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            return await _context_client.Client.ToListAsync();
        }


        [Route("FindClient/{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<MainClient>> FindClient(long id)
        {
            var client = await _context_main.MainClient.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return client;
        }


        [HttpPost]
        [Route("FindClients")]
        public async Task<ActionResult<IEnumerable<GClient>>> FindClients(CClient CClient)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_context_main,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value
            );

            if (UD.Role == "SuperAdmin" || UD.Role == "SuperUser")
            {
                return await (from c in _context_main.MainClient
                              where
                              (CClient.Name.IsNull() ? true : c.Name.Contains(CClient.Name)) &&
                              (CClient.OrganizationName.IsNull() ? true : c.OrganizationName.Contains(CClient.OrganizationName)) &&
                              (CClient.TaxID.IsNull() ? true : c.TaxID == (CClient.TaxID)) &&
                              (CClient.OfficePhoneNo.IsNull() ? true : c.OfficePhoneNo == (CClient.OfficePhoneNo)) &&
                              (CClient.Address.IsNull() ? true : c.Address.Contains(CClient.Address))
                              select new GClient()
                              {
                                  ID = c.ID,
                                  Name = c.Name,
                                  OfficePhoneNum = c.OfficePhoneNo,
                                  OrganizationName = c.OrganizationName,
                                  Address = c.Address + ", " + c.City + ", " + c.State + ", " + c.ZipCode,
                                  TaxID = c.TaxID,
                                  ContactPerson = c.ContactPerson
                              }).Distinct().ToListAsync();
            }
            else
            {
                return await (from up in _context_main.MainUserPractices
                              join u in _context_main.Users
                              on up.UserID equals u.Id
                              join prac in _context_main.MainPractice
                              on up.PracticeID equals prac.ID
                              join c in _context_main.MainClient
                              on u.ClientID equals c.ID
                              where
                              prac.ID == UD.PracticeID && up.Status == true && u.Id == UD.UserID &&
                              (CClient.Name.IsNull() ? true : c.Name.Contains(CClient.Name)) &&
                              (CClient.OrganizationName.IsNull() ? true : c.OrganizationName.Contains(CClient.OrganizationName)) &&
                              (CClient.TaxID.IsNull() ? true : c.TaxID == (CClient.TaxID)) &&
                              (CClient.OfficePhoneNo.IsNull() ? true : c.OfficePhoneNo == (CClient.OfficePhoneNo)) &&
                              (CClient.Address.IsNull() ? true : c.Address.Contains(CClient.Address))
                              select new GClient()
                              {
                                  ID = c.ID,
                                  Name = c.Name,
                                  OfficePhoneNum = c.OfficePhoneNo,
                                  OrganizationName = c.OrganizationName,
                                  Address = c.Address + ", " + c.City + ", " + c.State + ", " + c.ZipCode,
                                  TaxID = c.TaxID,
                                  ContactPerson = c.ContactPerson
                              }).Distinct().ToListAsync();
            }
        }


        [Route("MigrateAllDatabases")]
        [HttpPost]
        public ActionResult MigrateAllDatabases()
        {
            var clientList = (from lis in _context_main.MainClient select lis.ContextName).ToList();
            foreach (var item in clientList)
            {
                _context_client.setDatabaseName(item);
                _context_client.Database.Migrate();
            }
            return Ok("Migrate");
        }

        [Route("SaveClient")]
        [HttpPost]
        public async Task<ActionResult<MainClient>> SaveClient(MainClient item)
        {
            var queryStatus = "";
            string Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;

            if (item.ZipCode.IsNull()) item.ZipCode = null;

            bool succ = TryValidateModel(item);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }


            //Modify ContextName
            string databaseName2 = item.Name.Trim();
            string ModifyDbName2 = "";
            string[] multiArray2 = databaseName2.Split(new Char[] { ' ', ',', '.', '_', '-', '\n', '\t', '"', '\'', '@', '#', '$', '%', '^', '&', '*', '(', ')', '+', '=', '!', '~', '`' });
            foreach (var DbItem2 in multiArray2)
            {
                ModifyDbName2 += DbItem2;
            }



            // check Client Exists or  Not in Main Context
            var CheckClientExists = (from c in _context_main.MainClient where c.ContextName.ToUpper() == ModifyDbName2.Trim().ToUpper() select c).FirstOrDefault();


            if (item.ID == 0)
            {
                //Modify ContextName
                string databaseName = item.Name.Trim();
                string ModifyDbName = "";
                string[] multiArray = databaseName.Split(new Char[] { ' ', ',', '.', '_', '-', '\n', '\t', '"', '\'', '@', '#', '$', '%', '^', '&', '*', '(', ')', '+', '=', '!', '~', '`' });
                foreach (var DbItem in multiArray)
                {
                    ModifyDbName += DbItem;
                }
                //Check Database Exist or not if not exist than create DB and run migration and triger, 
                //if already created than rum migration and triger
                queryStatus = CheckDatabaseExists(ModifyDbName);

                //if client Not Exist From Main Context
                if (CheckClientExists == null)
                {

                    item.AddedBy = Email;
                    item.AddedDate = DateTime.Now;
                    item.ContextName = ModifyDbName.Trim();
                    item.Name = item.Name.Trim();
                    _context_main.MainClient.Add(item);
                    await _context_main.SaveChangesAsync();

                    var MClientExists = (from c in _context_main.MainClient where c.ContextName.ToUpper() == ModifyDbName.Trim().ToUpper() select c).FirstOrDefault();
                    if (MClientExists != null)
                    {
                        var ClExists = (from c in _context_client.Client where c.ContextName.ToUpper() == MClientExists.ContextName select c).FirstOrDefault();
                        if (ClExists == null)
                        {
                            Client cli = new Client();
                            cli.ID = MClientExists.ID;
                            cli.Name = MClientExists.Name;
                            cli.OrganizationName = MClientExists.OrganizationName;
                            cli.TaxID = MClientExists.TaxID;
                            cli.ServiceLocation = MClientExists.ServiceLocation;
                            cli.Address = MClientExists.Address;
                            cli.State = MClientExists.State;
                            cli.City = MClientExists.City;
                            cli.ZipCode = MClientExists.ZipCode;
                            cli.OfficeHour = MClientExists.OfficeHour;
                            cli.FaxNo = MClientExists.FaxNo;
                            cli.OfficePhoneNo = MClientExists.OfficePhoneNo;
                            cli.OfficeEmail = MClientExists.OfficeEmail;
                            cli.ContactPerson = MClientExists.ContactPerson;
                            cli.ContactNo = MClientExists.ContactNo;
                            cli.ContextName = MClientExists.ContextName;
                            cli.AddedBy = MClientExists.AddedBy;
                            cli.AddedDate = MClientExists.AddedDate;
                            cli.UpdatedBy = MClientExists.UpdatedBy;
                            cli.UpdatedDate = MClientExists.UpdatedDate;
                            _context_client.Client.Add(cli);
                            await _context_client.SaveChangesAsync();
                        }

                    }

                    var CheckMainUser = (from u in _context_main.Users
                                         where u.Email == "super@bellmedex.com"
                                         select u
                                     ).FirstOrDefault();
                    if (CheckMainUser.ClientID == null)
                    {
                        CheckMainUser.ClientID = item.ID;
                        _context_main.Entry(CheckMainUser).State = EntityState.Modified;
                        await _context_main.SaveChangesAsync();
                    }



                    var CheckCltUser = (from u in _context_client.Users
                                        where u.Email == "super@bellmedex.com"
                                        select u
                        ).FirstOrDefault();



                    if (CheckCltUser == null)
                    {
                        //Add Super User In Client Context
                        AuthIdentityCustom usr = new AuthIdentityCustom();
                        usr.Id = CheckMainUser.Id;
                        usr.UserName = CheckMainUser.UserName;
                        usr.NormalizedUserName = CheckMainUser.NormalizedUserName;
                        usr.Email = CheckMainUser.Email;
                        usr.NormalizedEmail = CheckMainUser.NormalizedEmail;
                        usr.EmailConfirmed = CheckMainUser.EmailConfirmed;
                        usr.PasswordHash = CheckMainUser.PasswordHash;
                        usr.SecurityStamp = CheckMainUser.SecurityStamp;
                        usr.ConcurrencyStamp = CheckMainUser.ConcurrencyStamp;
                        usr.PhoneNumber = CheckMainUser.PhoneNumber;
                        usr.PhoneNumberConfirmed = CheckMainUser.PhoneNumberConfirmed;
                        usr.TwoFactorEnabled = CheckMainUser.TwoFactorEnabled;
                        usr.LockoutEnd = CheckMainUser.LockoutEnd;
                        usr.LockoutEnabled = CheckMainUser.LockoutEnabled;
                        usr.AccessFailedCount = CheckMainUser.AccessFailedCount;
                        usr.FirstName = CheckMainUser.FirstName;
                        usr.LastName = CheckMainUser.LastName;
                        usr.Name = CheckMainUser.Name;
                        usr.IsUserLogin = CheckMainUser.IsUserLogin;
                        usr.LogInAttempts = CheckMainUser.LogInAttempts;
                        usr.IsUserBlock = CheckMainUser.IsUserBlock;
                        usr.IsUserBlockByAdmin = CheckMainUser.IsUserBlockByAdmin;
                        usr.BlockNote = CheckMainUser.BlockNote;
                        usr.ClientID = null;
                        usr.PracticeID = null;
                        usr.TeamID = CheckMainUser.TeamID;
                        usr.DesignationID = CheckMainUser.DesignationID;
                        usr.ReportingTo = CheckMainUser.ReportingTo;
                        _context_client.Users.Add(usr);
                        await _context_client.SaveChangesAsync();


                    }
                    var setings = (from s in _context_client.Settings select s).ToList();
                    if (setings.Count == 0)
                    {
                        //Add Setting in Client Context
                        MediFusionPM.Models.Settings setting = new MediFusionPM.Models.Settings();
                        setting.ClientID = item.ID;
                        setting.DocumentServerURL = Configuration["ClientSettings:DocumentServerURL"];
                        setting.DocumentServerDirectory = Configuration["ClientSettings:DocumentServerDirectory"];
                        setting.AddedBy = Email;
                        setting.AddedDate = DateTime.Now;
                        _context_client.Settings.Add(setting);
                        await _context_client.SaveChangesAsync();
                    }


                    //Check Roles in Client Context
                    var CheckClientRoles = (from rol in _context_client.Roles
                                            select rol).ToList();
                    if (CheckClientRoles.Count == 0)
                    {

                        //Get All Roles from Main Context
                        var Roles = (from rol in _context_main.Roles
                                     select rol).ToList();
                        //add all roles in client context
                        foreach (var role in Roles)
                        {
                            _context_client.Roles.Add(role);
                            await _context_client.SaveChangesAsync();
                        }
                    }



                    //check  super user role and id are assigned or not in client context
                    var CheckClientUserRoles = (from urol in _context_client.UserRoles
                                                where urol.UserId == CheckMainUser.Id
                                                select urol).FirstOrDefault();
                    if (CheckClientUserRoles == null)
                    {
                        //get all roleId and userId from main context
                        var UserRoles = (from urol in _context_main.UserRoles
                                         where urol.UserId == CheckMainUser.Id
                                         select urol).FirstOrDefault();
                        //Add roleId and userId In Client Context
                        _context_client.UserRoles.Add(UserRoles);
                        await _context_client.SaveChangesAsync();

                    }


                    //check client designation in client Context
                    var CheckClientDesignations = (from rol in _context_client.Designations
                                                   select rol).ToList();
                    if (CheckClientDesignations.Count == 0)
                    {

                        //get All designation from main context
                        var Designations = (from rol in _context_main.MainDesignations
                                            select rol).ToList();
                        if (Designations.Count > 0)
                        {
                            //Add all designation in client context
                            foreach (var desig in Designations)
                            {
                                Designations des = new Designations();
                                des.ID = desig.ID;
                                des.Name = desig.Name;
                                des.AddedBy = desig.AddedBy;
                                des.AddedDate = desig.AddedDate;
                                des.UpdatedBy = desig.UpdatedBy;
                                des.UpdatedDate = desig.UpdatedDate;
                                _context_client.Designations.Add(des);
                                await _context_client.SaveChangesAsync();
                            }
                        }
                    }



                    //check client Team in client Context
                    var CheckClientTeam = (from rol in _context_client.Team
                                           select rol).ToList();
                    if (CheckClientTeam.Count == 0)
                    {
                        //get All Team from main context
                        var Team = (from rol in _context_main.MainTeam
                                    select rol).ToList();
                        if (Team.Count > 0)
                        {
                            //Add all Team in client context
                            foreach (var team in Team)
                            {
                                Team tm = new Team();
                                tm.ID = team.ID;
                                tm.Name = team.Name;
                                tm.Details = team.Details;
                                tm.AddedBy = team.AddedBy;
                                tm.AddedDate = team.AddedDate;
                                tm.UpdatedBy = team.UpdatedBy;
                                tm.UpdatedDate = team.UpdatedDate;
                                _context_client.Team.Add(tm);
                                await _context_client.SaveChangesAsync();
                            }
                        }
                    }



                    //var rig = MainCreateRights(user.Id, model.UserRole);

                    var CliRights = (from rights in _context_client.Rights where rights.Id == CheckMainUser.Id select rights).FirstOrDefault();
                    if (CliRights == null)
                    {
                        var Rights = ClientCreateRights(CheckMainUser.Id);
                    }
                }
                else
                {
                    var clien = (from u in _context_main.MainClient where u.ContextName == CheckClientExists.ContextName select u).FirstOrDefault();
                    if (clien == null)
                    {
                        clien.Name = item.Name;
                        clien.OrganizationName = item.OrganizationName;
                        clien.TaxID = item.TaxID;
                        clien.ServiceLocation = item.ServiceLocation;
                        clien.Address = item.Address;
                        clien.State = item.State;
                        clien.City = item.City;
                        clien.ZipCode = item.ZipCode;
                        clien.OfficeHour = item.OfficeHour;
                        clien.FaxNo = item.FaxNo;
                        clien.OfficePhoneNo = item.OfficePhoneNo;
                        clien.OfficeEmail = item.OfficeEmail;
                        clien.ContactPerson = item.ContactPerson;
                        clien.ContactNo = item.ContactNo;
                        clien.UpdatedBy = item.UpdatedBy;
                        clien.UpdatedDate = DateTime.Now;
                        _context_main.Entry(clien).State = EntityState.Modified;
                        await _context_main.SaveChangesAsync();
                    }

                    var MainClientExists = (from c in _context_main.MainClient where c.ContextName == CheckClientExists.ContextName select c).FirstOrDefault();
                    if (MainClientExists != null)
                    {
                        var ClientExist = (from c in _context_client.Client where c.ContextName == CheckClientExists.ContextName select c).FirstOrDefault();
                        if (ClientExist == null)
                        {
                            Client cli = new Client();
                            cli.ID = item.ID;
                            cli.Name = item.Name;
                            cli.OrganizationName = item.OrganizationName;
                            cli.TaxID = item.TaxID;
                            cli.ServiceLocation = item.ServiceLocation;
                            cli.Address = item.Address;
                            cli.State = item.State;
                            cli.City = item.City;
                            cli.ZipCode = item.ZipCode;
                            cli.OfficeHour = item.OfficeHour;
                            cli.FaxNo = item.FaxNo;
                            cli.OfficePhoneNo = item.OfficePhoneNo;
                            cli.OfficeEmail = item.OfficeEmail;
                            cli.ContactPerson = item.ContactPerson;
                            cli.ContactNo = item.ContactNo;
                            cli.ContextName = item.ContextName;
                            cli.AddedBy = item.AddedBy;
                            cli.AddedDate = item.AddedDate;
                            cli.UpdatedBy = item.UpdatedBy;
                            cli.UpdatedDate = item.UpdatedDate;
                            _context_client.Client.Add(cli);
                            await _context_client.SaveChangesAsync();
                        }

                        else
                        {
                            ClientExist.Name = item.Name;
                            ClientExist.OrganizationName = item.OrganizationName;
                            ClientExist.TaxID = item.TaxID;
                            ClientExist.ServiceLocation = item.ServiceLocation;
                            ClientExist.Address = item.Address;
                            ClientExist.State = item.State;
                            ClientExist.City = item.City;
                            ClientExist.ZipCode = item.ZipCode;
                            ClientExist.OfficeHour = item.OfficeHour;
                            ClientExist.FaxNo = item.FaxNo;
                            ClientExist.OfficePhoneNo = item.OfficePhoneNo;
                            ClientExist.OfficeEmail = item.OfficeEmail;
                            ClientExist.ContactPerson = item.ContactPerson;
                            ClientExist.ContactNo = item.ContactNo;
                            ClientExist.UpdatedBy = item.UpdatedBy;
                            ClientExist.UpdatedDate = item.UpdatedDate;
                            _context_client.Entry(ClientExist).State = EntityState.Modified;
                            await _context_client.SaveChangesAsync();

                        }
                    }


                    var CheckMainUser = (from u in _context_main.Users
                                         where u.Email == "super@bellmedex.com"
                                         select u
                    ).FirstOrDefault();

                    var checkSetting = (from s in _context_client.Settings select s).ToList();
                    if (checkSetting.Count == 0)
                    {
                        //Add Setting in Client Context
                        MediFusionPM.Models.Settings setting = new MediFusionPM.Models.Settings();
                        setting.ClientID = item.ID;
                        setting.DocumentServerURL = Configuration["ClientSettings:DocumentServerURL"];
                        setting.DocumentServerDirectory = Configuration["ClientSettings:DocumentServerDirectory"];
                        setting.AddedBy = Email;
                        setting.AddedDate = DateTime.Now;
                        _context_client.Settings.Add(setting);
                        await _context_client.SaveChangesAsync();
                    }


                    var CheckClientUser = (from u in _context_client.Users
                                           where u.Email == "super@bellmedex.com"
                                           select u
                  ).FirstOrDefault();
                    if (CheckClientUser == null)
                    {
                        //Add Super User In Client Context
                        AuthIdentityCustom usr = new AuthIdentityCustom();
                        usr.Id = CheckMainUser.Id;
                        usr.UserName = CheckMainUser.UserName;
                        usr.NormalizedUserName = CheckMainUser.NormalizedUserName;
                        usr.Email = CheckMainUser.Email;
                        usr.NormalizedEmail = CheckMainUser.NormalizedEmail;
                        usr.EmailConfirmed = CheckMainUser.EmailConfirmed;
                        usr.PasswordHash = CheckMainUser.PasswordHash;
                        usr.SecurityStamp = CheckMainUser.SecurityStamp;
                        usr.ConcurrencyStamp = CheckMainUser.ConcurrencyStamp;
                        usr.PhoneNumber = CheckMainUser.PhoneNumber;
                        usr.PhoneNumberConfirmed = CheckMainUser.PhoneNumberConfirmed;
                        usr.TwoFactorEnabled = CheckMainUser.TwoFactorEnabled;
                        usr.LockoutEnd = CheckMainUser.LockoutEnd;
                        usr.LockoutEnabled = CheckMainUser.LockoutEnabled;
                        usr.AccessFailedCount = CheckMainUser.AccessFailedCount;
                        usr.FirstName = CheckMainUser.FirstName;
                        usr.LastName = CheckMainUser.LastName;
                        usr.Name = CheckMainUser.Name;
                        usr.IsUserLogin = CheckMainUser.IsUserLogin;
                        usr.LogInAttempts = CheckMainUser.LogInAttempts;
                        usr.IsUserBlock = CheckMainUser.IsUserBlock;
                        usr.IsUserBlockByAdmin = CheckMainUser.IsUserBlockByAdmin;
                        usr.BlockNote = CheckMainUser.BlockNote;
                        usr.ClientID = null;
                        usr.PracticeID = null;
                        usr.TeamID = CheckMainUser.TeamID;
                        usr.DesignationID = CheckMainUser.DesignationID;
                        usr.ReportingTo = CheckMainUser.ReportingTo;
                        _context_client.Users.Add(usr);
                        await _context_client.SaveChangesAsync();


                    }
                    else
                    {
                        //CheckClientUser.Id = CheckMainUser.Id;
                        CheckClientUser.UserName = CheckMainUser.UserName;
                        CheckClientUser.NormalizedUserName = CheckMainUser.NormalizedUserName;
                        CheckClientUser.Email = CheckMainUser.Email;
                        CheckClientUser.NormalizedEmail = CheckMainUser.NormalizedEmail;
                        CheckClientUser.EmailConfirmed = CheckMainUser.EmailConfirmed;
                        CheckClientUser.PasswordHash = CheckMainUser.PasswordHash;
                        CheckClientUser.SecurityStamp = CheckMainUser.SecurityStamp;
                        CheckClientUser.ConcurrencyStamp = CheckMainUser.ConcurrencyStamp;
                        CheckClientUser.PhoneNumber = CheckMainUser.PhoneNumber;
                        CheckClientUser.PhoneNumberConfirmed = CheckMainUser.PhoneNumberConfirmed;
                        CheckClientUser.TwoFactorEnabled = CheckMainUser.TwoFactorEnabled;
                        CheckClientUser.LockoutEnd = CheckMainUser.LockoutEnd;
                        CheckClientUser.LockoutEnabled = CheckMainUser.LockoutEnabled;
                        CheckClientUser.AccessFailedCount = CheckMainUser.AccessFailedCount;
                        CheckClientUser.FirstName = CheckMainUser.FirstName;
                        CheckClientUser.LastName = CheckMainUser.LastName;
                        CheckClientUser.Name = CheckMainUser.Name;
                        CheckClientUser.IsUserLogin = CheckMainUser.IsUserLogin;
                        CheckClientUser.LogInAttempts = CheckMainUser.LogInAttempts;
                        CheckClientUser.IsUserBlock = CheckMainUser.IsUserBlock;
                        CheckClientUser.IsUserBlockByAdmin = CheckMainUser.IsUserBlockByAdmin;
                        CheckClientUser.BlockNote = CheckMainUser.BlockNote;
                        CheckClientUser.ClientID = null;
                        CheckClientUser.PracticeID = null;
                        CheckClientUser.TeamID = CheckMainUser.TeamID;
                        CheckClientUser.DesignationID = CheckMainUser.DesignationID;
                        CheckClientUser.ReportingTo = CheckMainUser.ReportingTo;
                        _context_client.Entry(CheckClientUser).State = EntityState.Modified;
                        //_context_client.Users.Update(CheckClientUser);
                        await _context_client.SaveChangesAsync();
                    }



                    //Check Roles in Client Context
                    var CheckClientRoles = (from rol in _context_client.Roles
                                            select rol).ToList();
                    if (CheckClientRoles.Count == 0)
                    {
                        //Get All Roles from Main Context
                        var Roles = (from rol in _context_main.Roles
                                     select rol).ToList();
                        //add all roles in client context
                        foreach (var role in Roles)
                        {
                            _context_client.Roles.Add(role);
                            await _context_client.SaveChangesAsync();
                        }
                    }



                    //check  super user role and id are assigned or not in client context
                    var CheckClientUserRoles = (from urol in _context_client.UserRoles
                                                where urol.UserId == CheckMainUser.Id
                                                select urol).FirstOrDefault();
                    if (CheckClientUserRoles == null)
                    {
                        //get all roleId and userId from main context
                        var UserRoles = (from urol in _context_main.UserRoles
                                         where urol.UserId == CheckMainUser.Id
                                         select urol).FirstOrDefault();
                        //Add roleId and userId In Client Context
                        _context_client.UserRoles.Add(UserRoles);
                        await _context_client.SaveChangesAsync();
                    }


                    //check client designation in client Context
                    var CheckClientDesignations = (from rol in _context_client.Designations
                                                   select rol).ToList();
                    if (CheckClientDesignations.Count == 0)
                    {
                        //get All designation from main context
                        var Designations = (from rol in _context_main.MainDesignations
                                            select rol).ToList();
                        if (Designations.Count == 0)
                        {
                            //Add all designation in client context
                            foreach (var desig in Designations)
                            {
                                Designations des = new Designations();
                                des.ID = des.ID;
                                des.Name = des.Name;
                                des.AddedBy = des.AddedBy;
                                des.AddedDate = des.AddedDate;
                                des.UpdatedBy = des.UpdatedBy;
                                des.UpdatedDate = des.UpdatedDate;
                                _context_client.Designations.Add(des);
                                await _context_client.SaveChangesAsync();
                            }
                        }
                    }


                    //check client Team in client Context
                    var CheckClientTeam = (from rol in _context_client.Team
                                           select rol).ToList();
                    if (CheckClientTeam.Count == 0)
                    {
                        //get All Team from main context
                        var Team = (from rol in _context_main.MainTeam
                                    select rol).ToList();
                        if (Team.Count == 0)
                        {
                            //Add all Team in client context
                            foreach (var team in Team)
                            {
                                Team tm = new Team();
                                tm.ID = team.ID;
                                tm.Name = team.Name;
                                tm.Details = team.Details;
                                tm.AddedBy = team.AddedBy;
                                tm.AddedDate = team.AddedDate;
                                tm.UpdatedBy = team.UpdatedBy;
                                tm.UpdatedDate = team.UpdatedDate;
                                _context_client.Team.Add(tm);
                                await _context_client.SaveChangesAsync();
                            }
                        }
                    }
                    //  await _context_client.SaveChangesAsync();

                    var CliRights = (from rights in _context_client.Rights where rights.Id == CheckMainUser.Id select rights).FirstOrDefault();
                    if (CliRights == null)
                    {
                        var Rights = ClientCreateRights(CheckMainUser.Id);
                    }


                }

                // await _context_client.SaveChangesAsync();


                var MainClienExist = (from c in _context_main.MainClient where c.ContextName.ToUpper() == ModifyDbName.Trim().ToUpper() select c).FirstOrDefault();

                //  var MainClienExist = (from u in _context_main.MainClient where u.ContextName.ToUpper() == CheckClientExists.ContextName.ToUpper() select u).FirstOrDefault();

                if (MainClienExist.IsClientCreatedSuccessfully == false)
                {
                    MainClienExist.Name = item.Name;
                    MainClienExist.UpdatedDate = DateTime.Now;
                    MainClienExist.IsClientCreatedSuccessfully = true;
                    _context_main.Entry(MainClienExist).State = EntityState.Modified;
                    await _context_main.SaveChangesAsync();
                }
                // _context_main.MainClient.Update(item);

                var ClientExists = (from c in _context_client.Client where c.ContextName.ToUpper() == ModifyDbName.Trim().ToUpper() select c).FirstOrDefault();
                if (ClientExists.IsClientCreatedSuccessfully == false)
                {
                    ClientExists.Name = item.Name;
                    ClientExists.UpdatedDate = DateTime.Now;
                    ClientExists.IsClientCreatedSuccessfully = true;
                    _context_client.Entry(ClientExists).State = EntityState.Modified;
                    await _context_client.SaveChangesAsync();
                }








            }
            else
            {

                //Modify ContextName
                string databaseName = item.Name.Trim();
                string ModifyDbName = "";
                string[] multiArray = databaseName.Split(new Char[] { ' ', ',', '.', '_', '-', '\n', '\t', '"', '\'', '@', '#', '$', '%', '^', '&', '*', '(', ')', '+', '=', '!', '~', '`' });
                foreach (var DbItem in multiArray)
                {
                    ModifyDbName += DbItem;
                }
                //Check Database Exist or not if not exist than create DB and run migration and triger, 
                //if already created than rum migration and triger
                //  queryStatus = CheckDatabaseExists(ModifyDbName);


                var clien = (from u in _context_main.MainClient where u.ContextName == CheckClientExists.ContextName select u).FirstOrDefault();

                clien.Name = item.Name;
                clien.OrganizationName = item.OrganizationName;
                clien.TaxID = item.TaxID;
                clien.ServiceLocation = item.ServiceLocation;
                clien.Address = item.Address;
                clien.State = item.State;
                clien.City = item.City;
                clien.ZipCode = item.ZipCode;
                clien.OfficeHour = item.OfficeHour;
                clien.FaxNo = item.FaxNo;
                clien.OfficePhoneNo = item.OfficePhoneNo;
                clien.OfficeEmail = item.OfficeEmail;
                clien.ContactPerson = item.ContactPerson;
                clien.ContactNo = item.ContactNo;
                clien.UpdatedBy = item.UpdatedBy;
                clien.UpdatedDate = DateTime.Now;

                _context_main.Entry(clien).State = EntityState.Modified;
                await _context_client.SaveChangesAsync();


                var ClientExists = (from c in _context_main.MainClient where c.ContextName == CheckClientExists.ContextName select c).FirstOrDefault();
                if (ClientExists == null)
                {
                    Client cli = new Client();
                    cli.ID = ClientExists.ID;
                    cli.Name = ClientExists.Name;
                    cli.OrganizationName = ClientExists.OrganizationName;
                    cli.TaxID = ClientExists.TaxID;
                    cli.ServiceLocation = ClientExists.ServiceLocation;
                    cli.Address = ClientExists.Address;
                    cli.State = ClientExists.State;
                    cli.City = ClientExists.City;
                    cli.ZipCode = ClientExists.ZipCode;
                    cli.OfficeHour = ClientExists.OfficeHour;
                    cli.FaxNo = ClientExists.FaxNo;
                    cli.OfficePhoneNo = ClientExists.OfficePhoneNo;
                    cli.OfficeEmail = ClientExists.OfficeEmail;
                    cli.ContactPerson = ClientExists.ContactPerson;
                    cli.ContactNo = ClientExists.ContactNo;
                    cli.ContextName = ClientExists.ContextName;
                    cli.AddedBy = ClientExists.AddedBy;
                    cli.AddedDate = ClientExists.AddedDate;
                    cli.UpdatedBy = ClientExists.UpdatedBy;
                    cli.UpdatedDate = ClientExists.UpdatedDate;
                    _context_client.Client.Add(cli);
                    await _context_client.SaveChangesAsync();
                    //  var CheckMainUser = (from u in _context_main.Users
                    //                         where u.Email == "super@bellmedex.com"
                    //                         select u
                    //).FirstOrDefault();
                    //  if (CheckMainUser != null)
                    //  {
                    //      var CliRights = (from rights in _context_client.Rights where rights.Id == CheckMainUser.Id select rights).FirstOrDefault();
                    //      if (CliRights == null)
                    //      {
                    //          var Rights = ClientCreateRights(CheckMainUser.Id);
                    //      }
                    //  }
                }
                else
                {
                    var ClientExistsAg = (from c in _context_client.Client where c.ID == item.ID select c).FirstOrDefault();
                    if (ClientExistsAg != null)
                    {
                        ClientExistsAg.Name = item.Name;
                        ClientExistsAg.OrganizationName = item.OrganizationName;
                        ClientExistsAg.TaxID = item.TaxID;
                        ClientExistsAg.ServiceLocation = item.ServiceLocation;
                        ClientExistsAg.Address = item.Address;
                        ClientExistsAg.State = item.State;
                        ClientExistsAg.City = item.City;
                        ClientExistsAg.ZipCode = item.ZipCode;
                        ClientExistsAg.OfficeHour = item.OfficeHour;
                        ClientExistsAg.FaxNo = item.FaxNo;
                        ClientExistsAg.OfficePhoneNo = item.OfficePhoneNo;
                        ClientExistsAg.OfficeEmail = item.OfficeEmail;
                        ClientExistsAg.ContactPerson = item.ContactPerson;
                        ClientExistsAg.ContactNo = item.ContactNo;
                        ClientExistsAg.UpdatedBy = item.UpdatedBy;
                        ClientExistsAg.UpdatedDate = item.UpdatedDate;
                        _context_client.Entry(ClientExistsAg).State = EntityState.Modified;
                        await _context_client.SaveChangesAsync();
                    }
                }

                var CheckMainUser = (from u in _context_main.Users
                                     where u.Email == "super@bellmedex.com"
                                     select u
                ).FirstOrDefault();
                CheckMainUser.ClientID = item.ID;
                _context_main.Entry(CheckMainUser).State = EntityState.Modified;
                await _context_main.SaveChangesAsync();

                var CheckClientUser = (from u in _context_client.Users
                                       where u.Email == "super@bellmedex.com"
                                       select u
             ).FirstOrDefault();
                if (CheckClientUser == null)
                {
                    //Add Super User In Client Context
                    AuthIdentityCustom usr = new AuthIdentityCustom();
                    usr.Id = CheckMainUser.Id;
                    usr.UserName = CheckMainUser.UserName;
                    usr.NormalizedUserName = CheckMainUser.NormalizedUserName;
                    usr.Email = CheckMainUser.Email;
                    usr.NormalizedEmail = CheckMainUser.NormalizedEmail;
                    usr.EmailConfirmed = CheckMainUser.EmailConfirmed;
                    usr.PasswordHash = CheckMainUser.PasswordHash;
                    usr.SecurityStamp = CheckMainUser.SecurityStamp;
                    usr.ConcurrencyStamp = CheckMainUser.ConcurrencyStamp;
                    usr.PhoneNumber = CheckMainUser.PhoneNumber;
                    usr.PhoneNumberConfirmed = CheckMainUser.PhoneNumberConfirmed;
                    usr.TwoFactorEnabled = CheckMainUser.TwoFactorEnabled;
                    usr.LockoutEnd = CheckMainUser.LockoutEnd;
                    usr.LockoutEnabled = CheckMainUser.LockoutEnabled;
                    usr.AccessFailedCount = CheckMainUser.AccessFailedCount;
                    usr.FirstName = CheckMainUser.FirstName;
                    usr.LastName = CheckMainUser.LastName;
                    usr.Name = CheckMainUser.Name;
                    usr.IsUserLogin = CheckMainUser.IsUserLogin;
                    usr.LogInAttempts = CheckMainUser.LogInAttempts;
                    usr.IsUserBlock = CheckMainUser.IsUserBlock;
                    usr.IsUserBlockByAdmin = CheckMainUser.IsUserBlockByAdmin;
                    usr.BlockNote = CheckMainUser.BlockNote;
                    usr.ClientID = null;
                    usr.PracticeID = null;
                    usr.TeamID = CheckMainUser.TeamID;
                    usr.DesignationID = CheckMainUser.DesignationID;
                    usr.ReportingTo = CheckMainUser.ReportingTo;
                    _context_client.Users.Add(usr);
                    await _context_client.SaveChangesAsync();
                }
                else
                {
                    CheckClientUser.Id = CheckMainUser.Id;
                    CheckClientUser.UserName = CheckMainUser.UserName;
                    CheckClientUser.NormalizedUserName = CheckMainUser.NormalizedUserName;
                    CheckClientUser.Email = CheckMainUser.Email;
                    CheckClientUser.NormalizedEmail = CheckMainUser.NormalizedEmail;
                    CheckClientUser.EmailConfirmed = CheckMainUser.EmailConfirmed;
                    CheckClientUser.PasswordHash = CheckMainUser.PasswordHash;
                    CheckClientUser.SecurityStamp = CheckMainUser.SecurityStamp;
                    CheckClientUser.ConcurrencyStamp = CheckMainUser.ConcurrencyStamp;
                    CheckClientUser.PhoneNumber = CheckMainUser.PhoneNumber;
                    CheckClientUser.PhoneNumberConfirmed = CheckMainUser.PhoneNumberConfirmed;
                    CheckClientUser.TwoFactorEnabled = CheckMainUser.TwoFactorEnabled;
                    CheckClientUser.LockoutEnd = CheckMainUser.LockoutEnd;
                    CheckClientUser.LockoutEnabled = CheckMainUser.LockoutEnabled;
                    CheckClientUser.AccessFailedCount = CheckMainUser.AccessFailedCount;
                    CheckClientUser.FirstName = CheckMainUser.FirstName;
                    CheckClientUser.LastName = CheckMainUser.LastName;
                    CheckClientUser.Name = CheckMainUser.Name;
                    CheckClientUser.IsUserLogin = CheckMainUser.IsUserLogin;
                    CheckClientUser.LogInAttempts = CheckMainUser.LogInAttempts;
                    CheckClientUser.IsUserBlock = CheckMainUser.IsUserBlock;
                    CheckClientUser.IsUserBlockByAdmin = CheckMainUser.IsUserBlockByAdmin;
                    CheckClientUser.BlockNote = CheckMainUser.BlockNote;
                    CheckClientUser.ClientID = null;
                    CheckClientUser.PracticeID = null;
                    CheckClientUser.TeamID = CheckMainUser.TeamID;
                    CheckClientUser.DesignationID = CheckMainUser.DesignationID;
                    CheckClientUser.ReportingTo = CheckMainUser.ReportingTo;
                    _context_client.Entry(CheckClientUser).State = EntityState.Modified;

                    // _context_client.Users.Update(CheckClientUser);
                    await _context_client.SaveChangesAsync();


                }


                //Check Roles in Client Context
                var CheckClientRoles = (from rol in _context_client.Roles
                                        select rol).ToList();
                if (CheckClientRoles.Count == 0)
                {
                    //Get All Roles from Main Context
                    var Roles = (from rol in _context_main.Roles
                                 select rol).ToList();
                    //add all roles in client context
                    foreach (var role in Roles)
                    {
                        _context_client.Roles.Add(role);
                        await _context_client.SaveChangesAsync();
                    }
                }



                //check  super user role and id are assigned or not in client context
                var CheckClientUserRoles = (from urol in _context_client.UserRoles
                                            where urol.UserId == CheckMainUser.Id
                                            select urol).FirstOrDefault();
                if (CheckClientUserRoles == null)
                {
                    //get all roleId and userId from main context
                    var UserRoles = (from urol in _context_main.UserRoles
                                     where urol.UserId == CheckMainUser.Id
                                     select urol).FirstOrDefault();
                    //Add roleId and userId In Client Context
                    _context_client.UserRoles.Add(UserRoles);
                    await _context_client.SaveChangesAsync();
                }


                //check client designation in client Context
                var CheckClientDesignations = (from rol in _context_client.Designations
                                               select rol).ToList();
                if (CheckClientDesignations.Count == 0)
                {
                    //get All designation from main context
                    var Designations = (from rol in _context_main.MainDesignations
                                        select rol).ToList();
                    if (Designations.Count == 0)
                    {
                        //Add all designation in client context
                        foreach (var desig in Designations)
                        {
                            Designations des = new Designations();
                            des.ID = des.ID;
                            des.Name = des.Name;
                            des.AddedBy = des.AddedBy;
                            des.AddedDate = des.AddedDate;
                            des.UpdatedBy = des.UpdatedBy;
                            des.UpdatedDate = des.UpdatedDate;
                            _context_client.Designations.Add(des);
                            await _context_client.SaveChangesAsync();
                        }
                    }
                }


                //check client Team in client Context
                var CheckClientTeam = (from rol in _context_client.Team
                                       select rol).ToList();
                if (CheckClientTeam.Count == 0)
                {
                    //get All Team from main context
                    var Team = (from rol in _context_main.MainTeam
                                select rol).ToList();
                    if (Team.Count == 0)
                    {
                        //Add all Team in client context
                        foreach (var team in Team)
                        {
                            Team tm = new Team();
                            tm.ID = team.ID;
                            tm.Name = team.Name;
                            tm.Details = team.Details;
                            tm.AddedBy = team.AddedBy;
                            tm.AddedDate = team.AddedDate;
                            tm.UpdatedBy = team.UpdatedBy;
                            tm.UpdatedDate = team.UpdatedDate;
                            _context_client.Team.Add(tm);
                            await _context_client.SaveChangesAsync();
                        }
                    }
                }
                await _context_client.SaveChangesAsync();


                var MainClienExist = (from c in _context_main.MainClient where c.ContextName.ToUpper() == ModifyDbName.Trim().ToUpper() select c).FirstOrDefault();

                //  var MainClienExist = (from u in _context_main.MainClient where u.ContextName.ToUpper() == CheckClientExists.ContextName.ToUpper() select u).FirstOrDefault();

                if (MainClienExist.IsClientCreatedSuccessfully == false)
                {
                    MainClienExist.Name = item.Name;
                    MainClienExist.UpdatedDate = DateTime.Now;
                    MainClienExist.IsClientCreatedSuccessfully = true;
                    _context_main.Entry(MainClienExist).State = EntityState.Modified;
                    await _context_main.SaveChangesAsync();
                }
                // _context_main.MainClient.Update(item);


                var ClientExists2 = (from c in _context_client.Client where c.ContextName.ToUpper() == ModifyDbName.Trim().ToUpper() select c).FirstOrDefault();
                if (ClientExists2.IsClientCreatedSuccessfully == false)
                {
                    ClientExists2.Name = item.Name;
                    ClientExists2.UpdatedDate = DateTime.Now;
                    ClientExists2.IsClientCreatedSuccessfully = true;
                    _context_client.Entry(ClientExists2).State = EntityState.Modified;
                    await _context_client.SaveChangesAsync();
                }


            }


            return Ok(item);

        }

        private string CheckDatabaseExists(string databaseName)
        {
            string DbqueryStatus = "";

            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
            //string server = "LPT62";

            // string connString = "server=md1sc57itpldg4j.cmql3by2qjsk.us-east-1.rds.amazonaws.com,1433; database=Medifusion; user id = bellmedfu; password = Jaykaz321;"; // Used When Local Server
            // string connString = "server=LPT44; database=Medifusion; user id = sa; password = abc@1234;"; // Used When Local Server
            // string connString = "server=LPT62; database=Medifusion; user id = sa; password = abc@123;"; // Used When Local Server
            //string connString = "server=96.69.218.154\\SQLEXPRESS; database=Medifusion; user id = sa; password = Jay321@;"; // used When Staging
            string connString = Configuration.GetConnectionString("Medifusion");
            SqlConnection con = new SqlConnection(connString);
            int checkDb;
            string strCommand = string.Format("SET NOCOUNT OFF; SELECT COUNT(*) FROM master.dbo.sysdatabases where name=\'{0}\'", "Medifusion" + databaseName);
            con.Open();
            con.ChangeDatabase("Master");
            con.Close();
            using (SqlConnection sqlConnection = new SqlConnection(connString))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCmd = new SqlCommand(strCommand, sqlConnection))
                {

                    object resultObj = sqlCmd.ExecuteScalar();
                    checkDb = int.Parse(string.Format("{0}", resultObj));

                    if (checkDb == 0)
                    {
                        //string ReConnString = "server=md1sc57itpldg4j.cmql3by2qjsk.us-east-1.rds.amazonaws.com,1433; database=Medifusion; user id = bellmedfu; password = Jaykaz321;"; // Used When Local Server
                        // string ReConnString = "server=96.69.218.154\\SQLEXPRESS; database=Medifusion; user id = sa; password = Jay321@;"; // used When Staging
                        //string ReConnString = "server=LPT44; database=Medifusion; user id = sa; password = abc@1234;"; // Used When Local Server
                        //string ReConnString = "server=LPT62; database=Medifusion; user id = sa; password = abc@123;"; // Used When Local Server
                        string ReConnString = Configuration.GetConnectionString("Medifusion");
                        SqlConnection ReCon = new SqlConnection(ReConnString);
                        string str = "CREATE DATABASE Medifusion" + databaseName.Trim();
                        SqlCommand cmd = new SqlCommand(str, ReCon);
                        sqlConnection.Close();

                        ReCon.Open();
                        cmd.ExecuteNonQuery();
                        ReCon.Close();
                        sqlConnection.Open();
                        _context_client.setDatabaseName(databaseName);
                        _context_client.Database.Migrate();

                        string scriptTriger = System.IO.File.ReadAllText(Path.Combine(_context_client.env.ContentRootPath, "Resources", "AllTriggers.sql"));
                        string scriptInsert = System.IO.File.ReadAllText(Path.Combine(_context_client.env.ContentRootPath, "Resources", "ALLInserts.sql"));
                        string scriptFunction = System.IO.File.ReadAllText(Path.Combine(_context_client.env.ContentRootPath, "Resources", "Functions.sql"));
                        string connectionString = Configuration.GetConnectionString("MedifusionLocal");
                        string[] splitString = connectionString.Split(';');
                        splitString[1] = splitString[1];
                        connectionString = splitString[0] + "; " + splitString[1] + databaseName + "; " + splitString[2] + "; " + splitString[3];
                        SqlConnection conn = new SqlConnection(connectionString);
                        Server server = new Server(new ServerConnection(conn));
                        server.ConnectionContext.ExecuteNonQuery(scriptTriger);
                        server.ConnectionContext.ExecuteNonQuery(scriptInsert);
                        server.ConnectionContext.ExecuteNonQuery(scriptFunction);
                    }
                    else
                    {
                        DbqueryStatus = "Database Already Exist.";
                        _context_client.setDatabaseName(databaseName);
                        _context_client.Database.Migrate();



                        var MainClienExist = (from c in _context_main.MainClient where c.ContextName.ToUpper() == databaseName.Trim().ToUpper() select c).FirstOrDefault();

                        // var MainClienExist = (from u in _context_main.MainClient where u.ContextName.ToUpper() == databaseName.ToUpper() select u).FirstOrDefault();

                        if (MainClienExist.IsClientCreatedSuccessfully == false)
                        {
                            string scriptTriger = System.IO.File.ReadAllText(Path.Combine(_context_client.env.ContentRootPath, "Resources", "AllTriggers.sql"));
                            string scriptInsert = System.IO.File.ReadAllText(Path.Combine(_context_client.env.ContentRootPath, "Resources", "ALLInserts.sql"));
                            string connectionString = Configuration.GetConnectionString("MedifusionLocal");
                            string[] splitString = connectionString.Split(';');
                            splitString[1] = splitString[1];
                            connectionString = splitString[0] + "; " + splitString[1] + databaseName + "; " + splitString[2] + "; " + splitString[3];
                            SqlConnection conn = new SqlConnection(connectionString);
                            Server server = new Server(new ServerConnection(conn));
                            server.ConnectionContext.ExecuteNonQuery(scriptTriger);
                            server.ConnectionContext.ExecuteNonQuery(scriptInsert);
                        }


                    }

                }
                sqlConnection.Close();
            }
            return DbqueryStatus;
        }

        [Route("DeleteClient/{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteClient(long id)
        {
            var Client = await _context_client.Client.FindAsync(id);

            if (Client == null)
            {
                return NotFound();
            }

            _context_client.Client.Remove(Client);
            await _context_client.SaveChangesAsync();

            return Ok();
        }



        public ActionResult ClientCreateRights(string id)
        {

            var Rights = (from rights in _context_main.MainRights where rights.Id == id select rights).FirstOrDefault();



            Rights chkRights = new Rights();
            chkRights.Id = Rights.Id;

            //Scheduler Rights
            chkRights.SchedulerCreate = Rights.SchedulerCreate;
            chkRights.SchedulerEdit = Rights.SchedulerEdit;
            chkRights.SchedulerDelete = Rights.SchedulerDelete;
            chkRights.SchedulerSearch = Rights.SchedulerSearch;
            chkRights.SchedulerImport = Rights.SchedulerImport;
            chkRights.SchedulerExport = Rights.SchedulerExport;



            //Patient Rights
            chkRights.PatientCreate = Rights.PatientCreate;
            chkRights.PatientEdit = Rights.PatientEdit;
            chkRights.PatientDelete = Rights.PatientDelete;
            chkRights.PatientSearch = Rights.PatientSearch;
            chkRights.PatientImport = Rights.PatientImport;
            chkRights.PatientExport = Rights.PatientExport;

            //Charges Rights
            chkRights.ChargesCreate = Rights.ChargesCreate;
            chkRights.ChargesEdit = Rights.ChargesEdit;
            chkRights.ChargesDelete = Rights.ChargesDelete;
            chkRights.ChargesSearch = Rights.ChargesSearch;
            chkRights.ChargesImport = Rights.ChargesImport;
            chkRights.ChargesExport = Rights.ChargesExport;

            //Documents Rights
            chkRights.DocumentsCreate = Rights.DocumentsCreate;
            chkRights.DocumentsEdit = Rights.DocumentsEdit;
            chkRights.DocumentsDelete = Rights.DocumentsDelete;
            chkRights.DocumentsSearch = Rights.DocumentsSearch;
            chkRights.DocumentsImport = Rights.DocumentsImport;
            chkRights.DocumentsExport = Rights.DocumentsExport;

            //Submissions Rights
            chkRights.SubmissionsCreate = Rights.SubmissionsCreate;
            chkRights.SubmissionsEdit = Rights.SubmissionsEdit;
            chkRights.SubmissionsDelete = Rights.SubmissionsDelete;
            chkRights.SubmissionsSearch = Rights.SubmissionsSearch;
            chkRights.SubmissionsImport = Rights.SubmissionsImport;
            chkRights.SubmissionsExport = Rights.SubmissionsExport;

            //Payments Rights
            chkRights.PaymentsCreate = Rights.PaymentsCreate;
            chkRights.PaymentsEdit = Rights.PaymentsEdit;
            chkRights.PaymentsDelete = Rights.PaymentsDelete;
            chkRights.PaymentsSearch = Rights.PaymentsSearch;
            chkRights.PaymentsImport = Rights.PaymentsImport;
            chkRights.PaymentsExport = Rights.PaymentsExport;

            //Followup Rights
            chkRights.FollowupCreate = Rights.FollowupCreate;
            chkRights.FollowupEdit = Rights.FollowupEdit;
            chkRights.FollowupDelete = Rights.FollowupDelete;
            chkRights.FollowupSearch = Rights.FollowupSearch;
            chkRights.FollowupImport = Rights.FollowupImport;
            chkRights.FollowupExport = Rights.FollowupExport;

            //Reports Rights
            chkRights.ReportsCreate = Rights.ReportsCreate;
            chkRights.ReportsEdit = Rights.ReportsEdit;
            chkRights.ReportsDelete = Rights.ReportsDelete;
            chkRights.ReportsSearch = Rights.ReportsSearch;
            chkRights.ReportsImport = Rights.ReportsImport;
            chkRights.ReportsExport = Rights.ReportsExport;


            ////SetUp Client 
            //Client Rights
            chkRights.ClientCreate = Rights.ClientCreate;
            chkRights.ClientEdit = Rights.ClientEdit;
            chkRights.ClientDelete = Rights.ClientDelete;
            chkRights.ClientSearch = Rights.ClientSearch;
            chkRights.ClientImport = Rights.ClientImport;
            chkRights.ClientExport = Rights.ClientExport;

            chkRights.UserCreate = Rights.UserCreate;
            chkRights.UserEdit = Rights.UserEdit;
            chkRights.UserDelete = Rights.UserDelete;
            chkRights.UserSearch = Rights.UserSearch;
            chkRights.UserImport = Rights.UserImport;
            chkRights.UserExport = Rights.UserExport;

            ////SetUp Admin 
            //Practice
            chkRights.PracticeCreate = Rights.PracticeCreate;
            chkRights.PracticeEdit = Rights.PracticeEdit;
            chkRights.PracticeDelete = Rights.PracticeDelete;
            chkRights.PracticeSearch = Rights.PracticeSearch;
            chkRights.PracticeImport = Rights.PracticeImport;
            chkRights.PracticeExport = Rights.PracticeExport;

            //Location
            chkRights.LocationCreate = Rights.LocationCreate;
            chkRights.LocationEdit = Rights.LocationEdit;
            chkRights.LocationDelete = Rights.LocationDelete;
            chkRights.LocationSearch = Rights.LocationSearch;
            chkRights.LocationImport = Rights.LocationImport;
            chkRights.LocationExport = Rights.LocationExport;

            //Provider
            chkRights.ProviderCreate = Rights.ProviderCreate;
            chkRights.ProviderEdit = Rights.ProviderEdit;
            chkRights.ProviderDelete = Rights.ProviderDelete;
            chkRights.ProviderSearch = Rights.ProviderSearch;
            chkRights.ProviderImport = Rights.ProviderImport;
            chkRights.ProviderExport = Rights.ProviderExport;


            //Referring Provider
            chkRights.ReferringProviderCreate = Rights.ReferringProviderCreate;
            chkRights.ReferringProviderEdit = Rights.ReferringProviderEdit;
            chkRights.ReferringProviderDelete = Rights.ReferringProviderDelete;
            chkRights.ReferringProviderSearch = Rights.ReferringProviderSearch;
            chkRights.ReferringProviderImport = Rights.ReferringProviderImport;
            chkRights.ReferringProviderExport = Rights.ReferringProviderExport;

            //Setup Insurance
            //Insurance

            chkRights.InsuranceCreate = Rights.InsuranceCreate;
            chkRights.InsuranceEdit = Rights.InsuranceEdit;
            chkRights.InsuranceDelete = Rights.InsuranceDelete;
            chkRights.InsuranceSearch = Rights.InsuranceSearch;
            chkRights.InsuranceImport = Rights.InsuranceImport;
            chkRights.InsuranceExport = Rights.InsuranceExport;

            //Insurance Plan 
            chkRights.InsurancePlanCreate = Rights.InsurancePlanCreate;
            chkRights.InsurancePlanEdit = Rights.InsurancePlanEdit;
            chkRights.InsurancePlanDelete = Rights.InsurancePlanDelete;
            chkRights.InsurancePlanSearch = Rights.InsurancePlanSearch;
            chkRights.InsurancePlanImport = Rights.InsurancePlanImport;
            chkRights.InsurancePlanExport = Rights.InsurancePlanExport;

            //Insurance Plan Address 
            chkRights.InsurancePlanAddressCreate = Rights.InsurancePlanAddressCreate;
            chkRights.InsurancePlanAddressEdit = Rights.InsurancePlanAddressEdit;
            chkRights.InsurancePlanAddressDelete = Rights.InsurancePlanAddressDelete;
            chkRights.InsurancePlanAddressSearch = Rights.InsurancePlanAddressSearch;
            chkRights.InsurancePlanAddressImport = Rights.InsurancePlanAddressImport;
            chkRights.InsurancePlanAddressExport = Rights.InsurancePlanAddressExport;

            //EDI Submit
            chkRights.EDISubmitCreate = Rights.EDISubmitCreate;
            chkRights.EDISubmitEdit = Rights.EDISubmitEdit;
            chkRights.EDISubmitDelete = Rights.EDISubmitDelete;
            chkRights.EDISubmitSearch = Rights.EDISubmitSearch;
            chkRights.EDISubmitImport = Rights.EDISubmitImport;
            chkRights.EDISubmitExport = Rights.EDISubmitExport;

            //EDI EligiBility
            chkRights.EDIEligiBilityCreate = Rights.EDIEligiBilityCreate;
            chkRights.EDIEligiBilityEdit = Rights.EDIEligiBilityEdit;
            chkRights.EDIEligiBilityDelete = Rights.EDIEligiBilityDelete;
            chkRights.EDIEligiBilitySearch = Rights.EDIEligiBilitySearch;
            chkRights.EDIEligiBilityImport = Rights.EDIEligiBilityImport;
            chkRights.EDIEligiBilityExport = Rights.EDIEligiBilityExport;

            //EDI Status
            chkRights.EDIStatusCreate = Rights.EDIStatusCreate;
            chkRights.EDIStatusEdit = Rights.EDIStatusEdit;
            chkRights.EDIStatusDelete = Rights.EDIStatusDelete;
            chkRights.EDIStatusSearch = Rights.EDIStatusSearch;
            chkRights.EDIStatusImport = Rights.EDIStatusImport;
            chkRights.EDIStatusExport = Rights.EDIStatusExport;

            //Coding
            //ICD
            chkRights.ICDCreate = Rights.ICDCreate;
            chkRights.ICDEdit = Rights.ICDEdit;
            chkRights.ICDDelete = Rights.ICDDelete;
            chkRights.ICDSearch = Rights.ICDSearch;
            chkRights.ICDImport = Rights.ICDImport;
            chkRights.ICDExport = Rights.ICDExport;

            //CPT
            chkRights.CPTCreate = Rights.CPTCreate;
            chkRights.CPTEdit = Rights.CPTEdit;
            chkRights.CPTDelete = Rights.CPTDelete;
            chkRights.CPTSearch = Rights.CPTSearch;
            chkRights.CPTImport = Rights.CPTImport;
            chkRights.CPTExport = Rights.CPTExport;

            //Modifiers
            chkRights.ModifiersCreate = Rights.ModifiersCreate;
            chkRights.ModifiersEdit = Rights.ModifiersEdit;
            chkRights.ModifiersDelete = Rights.ModifiersDelete;
            chkRights.ModifiersSearch = Rights.ModifiersSearch;
            chkRights.ModifiersImport = Rights.ModifiersImport;
            chkRights.ModifiersExport = Rights.ModifiersExport;

            //POS
            chkRights.POSCreate = Rights.POSCreate;
            chkRights.POSEdit = Rights.POSEdit;
            chkRights.POSDelete = Rights.POSDelete;
            chkRights.POSSearch = Rights.POSSearch;
            chkRights.POSImport = Rights.POSImport;
            chkRights.POSExport = Rights.POSExport;

            //EDI Codes
            //Claim Status Category Codes
            chkRights.ClaimStatusCategoryCodesCreate = Rights.ClaimStatusCategoryCodesCreate;
            chkRights.ClaimStatusCategoryCodesEdit = Rights.ClaimStatusCategoryCodesEdit;
            chkRights.ClaimStatusCategoryCodesDelete = Rights.ClaimStatusCategoryCodesDelete;
            chkRights.ClaimStatusCategoryCodesSearch = Rights.ClaimStatusCategoryCodesSearch;
            chkRights.ClaimStatusCategoryCodesImport = Rights.ClaimStatusCategoryCodesImport;
            chkRights.ClaimStatusCategoryCodesExport = Rights.ClaimStatusCategoryCodesExport;

            //Claim Status Codes
            chkRights.ClaimStatusCodesCreate = Rights.ClaimStatusCodesCreate;
            chkRights.ClaimStatusCodesEdit = Rights.ClaimStatusCodesEdit;
            chkRights.ClaimStatusCodesDelete = Rights.ClaimStatusCodesDelete;
            chkRights.ClaimStatusCodesSearch = Rights.ClaimStatusCodesSearch;
            chkRights.ClaimStatusCodesImport = Rights.ClaimStatusCodesImport;
            chkRights.ClaimStatusCodesExport = Rights.ClaimStatusCodesExport;

            //Adjustment Codes
            chkRights.AdjustmentCodesCreate = Rights.AdjustmentCodesCreate;
            chkRights.AdjustmentCodesEdit = Rights.AdjustmentCodesEdit;
            chkRights.AdjustmentCodesDelete = Rights.AdjustmentCodesDelete;
            chkRights.AdjustmentCodesSearch = Rights.AdjustmentCodesSearch;
            chkRights.AdjustmentCodesImport = Rights.AdjustmentCodesImport;
            chkRights.AdjustmentCodesExport = Rights.AdjustmentCodesExport;

            //Remark Codes
            chkRights.RemarkCodesCreate = Rights.RemarkCodesCreate;
            chkRights.RemarkCodesEdit = Rights.RemarkCodesEdit;
            chkRights.RemarkCodesDelete = Rights.RemarkCodesDelete;
            chkRights.RemarkCodesSearch = Rights.RemarkCodesSearch;
            chkRights.RemarkCodesImport = Rights.RemarkCodesImport;
            chkRights.RemarkCodesExport = Rights.RemarkCodesExport;



            //Team Rights
            chkRights.teamCreate = Rights.teamCreate;
            chkRights.teamupdate = Rights.teamupdate;
            chkRights.teamDelete = Rights.teamDelete;
            chkRights.teamSearch = Rights.teamSearch;
            chkRights.teamExport = Rights.teamExport;
            chkRights.teamImport = Rights.teamImport;

            //Receiver Rights
            chkRights.receiverCreate = Rights.receiverCreate;
            chkRights.receiverupdate = Rights.receiverupdate;
            chkRights.receiverDelete = Rights.receiverDelete;
            chkRights.receiverSearch = Rights.receiverSearch;
            chkRights.receiverExport = Rights.receiverExport;
            chkRights.receiverImport = Rights.receiverImport;

            //Submitter Rights
            chkRights.submitterCreate = Rights.submitterCreate;
            chkRights.submitterUpdate = Rights.submitterUpdate;
            chkRights.submitterDelete = Rights.submitterDelete;
            chkRights.submitterSearch = Rights.submitterSearch;
            chkRights.submitterExport = Rights.submitterExport;
            chkRights.submitterImport = Rights.submitterImport;




            //PatientPlan Rights
            chkRights.patientPlanCreate = Rights.patientPlanCreate;
            chkRights.patientPlanUpdate = Rights.patientPlanUpdate;
            chkRights.patientPlanDelete = Rights.patientPlanDelete;
            chkRights.patientPlanSearch = Rights.patientPlanSearch;
            chkRights.patientPlanExport = Rights.patientPlanExport;
            chkRights.patientPlanImport = Rights.patientPlanImport;
            chkRights.performEligibility = Rights.performEligibility;


            //PatientPayment Rights
            chkRights.patientPaymentCreate = Rights.patientPaymentCreate;
            chkRights.patientPaymentUpdate = Rights.patientPaymentUpdate;
            chkRights.patientPaymentDelete = Rights.patientPaymentDelete;
            chkRights.patientPaymentSearch = Rights.patientPaymentSearch;
            chkRights.patientPaymentExport = Rights.patientPaymentExport;
            chkRights.patientPaymentImport = Rights.patientPaymentImport;

            // Charges Rights
            chkRights.resubmitCharges = Rights.resubmitCharges;
            // BatchDocument Rights
            chkRights.batchdocumentCreate = Rights.batchdocumentCreate;
            chkRights.batchdocumentUpdate = Rights.batchdocumentUpdate;
            chkRights.batchdocumentDelete = Rights.batchdocumentDelete;
            chkRights.batchdocumentSearch = Rights.batchdocumentSearch;
            chkRights.batchdocumentExport = Rights.batchdocumentExport;
            chkRights.batchdocumentImport = Rights.batchdocumentImport;





            // ElectronicSubmission Rights
            chkRights.electronicsSubmissionSearch = Rights.electronicsSubmissionSearch;
            chkRights.electronicsSubmissionSubmit = Rights.electronicsSubmissionSubmit;
            chkRights.electronicsSubmissionResubmit = Rights.electronicsSubmissionResubmit;

            // PaperSubmission Rights
            chkRights.paperSubmissionSearch = Rights.paperSubmissionSearch;
            chkRights.paperSubmissionSubmit = Rights.paperSubmissionSubmit;
            chkRights.paperSubmissionResubmit = Rights.paperSubmissionResubmit;
            // SubmissionLog Rights	
            chkRights.submissionLogSearch = Rights.submissionLogSearch;
            // PlanFollowup Rights	
            chkRights.planFollowupSearch = Rights.planFollowupSearch;
            chkRights.planFollowupCreate = Rights.planFollowupCreate;
            chkRights.planFollowupDelete = Rights.planFollowupDelete;
            chkRights.planFollowupUpdate = Rights.planFollowupUpdate;
            chkRights.planFollowupImport = Rights.planFollowupImport;
            chkRights.planFollowupExport = Rights.planFollowupExport;

            // PatientFollowup Rights	
            chkRights.patientFollowupSearch = Rights.patientFollowupSearch;
            chkRights.patientFollowupCreate = Rights.patientFollowupCreate;
            chkRights.patientFollowupDelete = Rights.patientFollowupDelete;
            chkRights.patientFollowupUpdate = Rights.patientFollowupUpdate;
            chkRights.patientFollowupImport = Rights.patientFollowupImport;
            chkRights.patientFollowupExport = Rights.patientFollowupExport;

            // Group Rights	
            chkRights.groupSearch = Rights.groupSearch;
            chkRights.groupCreate = Rights.groupCreate;
            chkRights.groupUpdate = Rights.groupUpdate;
            chkRights.groupDelete = Rights.groupDelete;
            chkRights.groupExport = Rights.groupExport;
            chkRights.groupImport = Rights.groupImport;
            // Reason Rights	
            chkRights.reasonSearch = Rights.reasonSearch;
            chkRights.reasonCreate = Rights.reasonCreate;
            chkRights.reasonUpdate = Rights.reasonUpdate;
            chkRights.reasonDelete = Rights.reasonDelete;
            chkRights.reasonExport = Rights.reasonExport;
            chkRights.reasonImport = Rights.reasonImport;

            chkRights.addPaymentVisit = Rights.addPaymentVisit;
            chkRights.DeleteCheck = Rights.DeleteCheck;
            chkRights.ManualPosting = Rights.ManualPosting;
            chkRights.Postcheck = Rights.Postcheck;
            chkRights.PostExport = Rights.PostExport;
            chkRights.PostImport = Rights.PostImport;
            chkRights.ManualPostingAdd = Rights.ManualPostingAdd;
            chkRights.ManualPostingUpdate = Rights.ManualPostingUpdate;
            chkRights.PostCheckSearch = Rights.PostCheckSearch;
            chkRights.DeletePaymentVisit = Rights.DeletePaymentVisit;


            chkRights.UpdatedBy = Rights.UpdatedBy;

            
            _context_client.Rights.Add(chkRights);
            _context_client.SaveChanges();

            return Ok("OK");

        }

        [Route("FindAudit/{ClientID}")]
        [HttpGet("{ClientID}")]
        public List<ClientAudit> FindAudit(long ClientID)
        {
            List<ClientAudit> data = (from pAudit in _context_client.ClientAudit
                                      where pAudit.ClientID == ClientID
                                      orderby pAudit.AddedDate descending
                                      select new ClientAudit()
                                      {
                                          ID = pAudit.ID,
                                          ClientID = pAudit.ID,
                                          TransactionID = pAudit.TransactionID,
                                          ColumnName = pAudit.ColumnName,
                                          CurrentValue = pAudit.CurrentValue,
                                          NewValue = pAudit.NewValue,
                                          CurrentValueID = pAudit.CurrentValueID,
                                          NewValueID = pAudit.NewValueID,
                                          HostName = pAudit.HostName,
                                          AddedBy = pAudit.AddedBy,
                                          AddedDate = pAudit.AddedDate,
                                      }).ToList<ClientAudit>();
            return data;
        }

        // [Route("ProcessClientDocuments")]
        // [HttpPost()]
        //public void ProcessClientDocuments(IEnumerable<FileUploadViewModel> Files)
        //{
        //    UserInfoData UD = VMCommon.GetLoginUserInfo(_context_main,
        //    User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value);

        //    //Models.Settings settings = _context_client.Settings.Where(s => s.ClientID == UD.ClientID).SingleOrDefaultAsync();
        //    Models.Settings settings = _context_client.Settings.Where(c => c.ClientID == UD.ClientID).SingleOrDefault();

        //    if (settings == null)
        //    {
        //        return BadRequest("Document Server Settings Not Found");
        //    }
        //    string directoryPath = Path.Combine("\\\\", settings.DocumentServerURL,
        //    settings.DocumentServerDirectory, UD.ClientID.ToString(), "Agreement Docs");

        //   // string AllDownloads = Path.Combine(directoryPath, "Client Documents");
        //    Directory.CreateDirectory(directoryPath);

        //    foreach (FileUploadViewModel File in Files)
        //    {


        //        string fileType = string.Empty, FilePath = "";

        //        if (File.Type.Replace(".", "") == "zip" || File.Type.Replace(".", "") == "rar")
        //        {
        //            return BadRequest("Zip File is not supported. Please contaxt Bell MedEx.");
        //        }
        //        else
        //        {
        //            byte[] data = Convert.FromBase64String(File.Content.Substring(File.Content.IndexOf("base64,") + 7));
        //            string decodedString = Encoding.UTF8.GetString(data);

        //            //fileType = MediFusionPM.BusinessLogic.Utilities.GetFileType(decodedString);
        //            fileType = File.Type;
        //            FilePath = Path.Combine(directoryPath, File.Name + fileType);
        //            System.IO.File.WriteAllTextAsync(FilePath, decodedString);
        //        }


        //        ClientDocument docs = new ClientDocument()
        //        {
        //            AddedBy = UD.Email,
        //            AddedDate = DateTime.Now,
        //            ClientID = UD.ClientID,
        //            DocumentPath = FilePath,
        //            FileName = File.Name,
        //            FileType = File.Type

        //        };
        //        _context_client.ClientDocument.Add(docs);
        //    }
        //     _context_client.SaveChangesAsync();

        //    return Ok();

        //}



        [Route("DeactivateClient")]
        [HttpPost]
        public async Task<ActionResult<MainClient>> DeactivateClient(MainClient item)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_context_main,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (item.ID <= 0 || item.ID.IsNull())
            {
                return BadRequest("Select Client First...");
            }
            
            else
            {

                item.IsDeactivated = item.IsDeactivated;
                item.DeactivationReason = item.DeactivationReason;
                item.DeactivationDate = DateTime.Now;
                item.DeactivateionAdditionalInfo = item.DeactivateionAdditionalInfo;
                item.UpdatedBy = UD.Email;
                item.UpdatedDate = DateTime.Now;
                _context_main.MainClient.Update(item);
               await _context_main.SaveChangesAsync();


                 
                var optionsBuilder = new DbContextOptionsBuilder<ClientDbContext>();
                optionsBuilder.UseSqlServer("server = LPT44\\LPT44LOCAL; database = Medifusion" + item.ContextName + "; user id = sa; password = abc@1234");
                HttpContextAccessor httpContextAccessor = new HttpContextAccessor();
                ClientDbContext NewContext = new ClientDbContext(optionsBuilder.Options, Configuration, httpContextAccessor, this.env);
               
                NewContext.Client.ToList();
                foreach (var c in NewContext.Client.ToList())
                {
                    Debug.WriteLine(c.Name);

                }

                Client client = new Client();
               

                client = NewContext.Client.Where(c => c.ID == item.ID).FirstOrDefault();
                if(client!=null)
                {
                    client.IsDeactivated = item.IsDeactivated;
                    client.DeactivationReason = item.DeactivationReason;
                    client.DeactivationDate = DateTime.Now;
                    client.DeactivateionAdditionalInfo = item.DeactivateionAdditionalInfo;
                    client.UpdatedBy = UD.Email;
                    client.UpdatedDate = DateTime.Now;
                    _context_client.Client.Update(client);
                    await _context_client.SaveChangesAsync();
                }
                
            }
            return item;
        }

    }
}


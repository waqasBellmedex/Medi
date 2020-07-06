using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MediFusionPM.Models;
using MediFusionPM.Models.Main;
using MediFusionPM.Models.TodoApi;
using MediFusionPM.Uitilities;
using MediFusionPM.ViewModels;
using MediFusionPM.ViewModels.Main;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using static MediFusionPM.ViewModels.VMCommon;
using static MediFusionPM.ViewModels.VMJobs;
using static MediFusionPM.ViewModels.VMUser;
using System.Data;
using System.Data.SqlClient;


namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class SwitchClientPracticeController : ControllerBase
    {
        private readonly ClientDbContext _clientContext;
        private readonly MainContext _mainContext;
        private IConfiguration _config;
        private IHostingEnvironment _env;
        private IConfiguration configuration;
        public SwitchClientPracticeController(ClientDbContext contextClient, MainContext contextMain, IConfiguration config, IHostingEnvironment env, IConfiguration Configuration)
        {
            _clientContext = contextClient;
            _mainContext = contextMain;
            this._config = _config;
            this._env = _env;
            configuration = Configuration;
        }



        public List<DropDown> Clients { get; set; }
        public MainRights Rights { get; set; }
        // public List<CustomRole> customRole { get; set; }
        public List<string> list = new List<string>();
        public List<DropDown> UserPractices { get; set; }
        public List<DropDown> UserLocations { get; set; }

        public List<DropDown> UserProviders { get; set; }
        public List<DropDown> UserRefProviders { get; set; }
        public List<DropDown> ClientPractices { get; set; }
        public List<DropDown> Roles { get; set; }
        public List<AssignedUserPractices> assignedUserPractices { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public long? PracticeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long? ClientID { get; set; }
        public string PracticeName { get; set; }
        public string UserRole { get; set; }
        public string ReportingTo { get; set; }
        public long? TeamID { get; set; }
        public long? DesignationID { get; set; }
        public List<DropDown> Teams { get; set; }
        public List<DropDown> Designations { get; set; }
        public string AuthenticationToken { get; set; }
        public object Token { get; set; }
        public List<Data> CPT { get; set; }
        public List<DropDown> POS { get; set; }
        public List<Data> ICD { get; set; }
        public List<Data> Modifier { get; set; }
        public List<Data> Taxonomy { get; set; }


        [Route("SwitchPractice/{Id}")]
        [HttpGet("{Id}")]
        public async Task<ActionResult> SwitchPractice(long Id)

        {



            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            var CurrentLoginUser = (from u in _mainContext.Users
                                    where u.Email == Email
                                    select u
                                ).FirstOrDefault();
            string LoginUserId = CurrentLoginUser.Id;
            var RoleClaim = User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase));

            var checkPracticeAssigned = (from w in _mainContext.MainUserPractices
                                         where w.UserID == LoginUserId && w.PracticeID == Id
                                         select w
                           ).FirstOrDefault();



            if (checkPracticeAssigned != null)
            {
                var Client = (from w in _mainContext.MainPractice
                              where w.ID == Id
                              select w
                        ).FirstOrDefault();

                CurrentLoginUser.PracticeID = Id;
                CurrentLoginUser.ClientID = Client.ClientID;
                _mainContext.Entry(CurrentLoginUser).State = EntityState.Modified;
                await _mainContext.SaveChangesAsync();
                _clientContext.Database.Migrate();


                string contextName = _mainContext.MainClient.Find(Client.ClientID)?.ContextName;
                string newConnection = GetConnectionStringManager(contextName);


                SwitchClientPracticeController obj = new SwitchClientPracticeController(_clientContext, _mainContext, _config, _env, configuration);

                obj.UserLocations = UserLocation(_clientContext, contextName, LoginUserId, RoleClaim.Value, Id);
                obj.UserProviders = UserProvider(_clientContext, contextName, LoginUserId, RoleClaim.Value, Id);
                obj.UserRefProviders = UserRefProvider(_clientContext, contextName, LoginUserId, RoleClaim.Value, Id);
                obj.POS = POSs(_clientContext, contextName, LoginUserId, RoleClaim.Value, Id);
                obj.Modifier = Modifiers(_clientContext, contextName, LoginUserId, RoleClaim.Value, Id);
                obj.Token = GenerateJwtToken(_mainContext, RoleClaim.Value, LoginUserId);

                var user = (from u in _mainContext.Users
                            where u.Id == LoginUserId
                            select u
                    ).First();
                obj.Name = user.FirstName + ", " + user.LastName;
                obj.Email = user.Email;

                obj.PracticeID = user.PracticeID;
                obj.ClientID = user.ClientID;
                obj.UserRole = RoleClaim.Value;





                if (RoleClaim.Value == "SuperAdmin" || RoleClaim.Value == "SuperUser")
                {
                    obj.Clients = (from u in _mainContext.MainClient
                                   select new DropDown()
                                   {
                                       ID = u.ID,
                                       Description = u.Name//+ " - " + p.Coverage, 
                                   }).ToList();
                }
                else
                {

                    obj.Clients = (from cli in _mainContext.MainClient
                                   join pra in _mainContext.MainPractice
                                   on cli.ID equals pra.ClientID
                                   join up in _mainContext.MainUserPractices
                                   on pra.ID equals up.PracticeID
                                   join usr in _mainContext.Users
                                   on up.UserID equals usr.Id
                                   where usr.Id == LoginUserId
                                   select new DropDown()
                                   {
                                       ID = cli.ID,
                                       Description = cli.Name
                                   }).Distinct().ToList();
                }

                obj.UserPractices = (from u in _mainContext.MainUserPractices
                                 join p in _mainContext.MainPractice
                                 on u.PracticeID equals p.ID
                                 join w in _mainContext.Users
                                 on u.UserID equals w.Id
                                 orderby p.Name ascending
                                 where u.UserID == LoginUserId
                                 select new DropDown()
                                 {
                                     ID = u.PracticeID,
                                     Description = p.Name
                                 }).ToList();
                obj.UserPractices.Insert(0, new DropDown() { ID = null, Description = "Please Select" });



                obj.Rights = (from u in _mainContext.MainRights
                          where u.Id == LoginUserId
                          select u
                          ).FirstOrDefault();



                return Ok(obj);
            }
            else
            {
                return BadRequest("Unexpected Error Occurred. Contact Bell Medex.");
            }

        }


        private List<DropDown> UserLocation(ClientDbContext contextClient, string contextName, string UserId, string Role, long practiceId)
        {

            string newConnection = GetConnectionStringManager(contextName);
            List<DropDown> UserL = new List<DropDown>();
            using (SqlConnection myConnection = new SqlConnection(newConnection))
            {

                string oString = "Select Location.ID As LID,Practice.ID As PID,  Location.Name As LName from Location join Practice on Location.PracticeID = Practice.ID where Practice.ID = practiceId";
                SqlCommand oCmd = new SqlCommand(oString, myConnection);
              //  oCmd.Parameters.AddWithValue("@Location.PracticeID", practiceId);
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    var dpdDef = new DropDown();
                    dpdDef.ID = null;
                    dpdDef.ID2 = null;
                    dpdDef.Description = "Please Select";
                    dpdDef.Value = "";
                    dpdDef.label = "";
                    UserL.Add(dpdDef);

                    while (oReader.Read())
                    {
                        var dpd = new DropDown();
                        dpd.ID = Convert.ToInt32(oReader["LID"]);
                        dpd.ID2 = Convert.ToInt32(oReader["PID"]);
                        dpd.Description = oReader["LName"].ToString();
                        dpd.Value = "";
                        dpd.label = oReader["LName"].ToString();
                        UserL.Add(dpd);
                    }

                    myConnection.Close();
                }
            }

            return UserL;
        }

        private List<Data> Modifiers(ClientDbContext contextClient, string contextName, string UserId, string Role, long practiceId)
        {
            string newConnection = GetConnectionStringManager(contextName);
            List<Data> Modi = new List<Data>();
            using (SqlConnection myConnection = new SqlConnection(newConnection))
            {

                string oString = "Select * from Modifier";
                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    var ModiDef = new Data();
                    ModiDef.ID = null;
                    ModiDef.Description = "Please Select";
                    ModiDef.Value = "";
                    ModiDef.label = "";
                    Modi.Add(ModiDef);

                    while (oReader.Read())
                    {
                        var ModiD = new Data();
                        ModiD.ID = Convert.ToInt32(oReader["ID"]);
                        ModiD.Value = oReader["Description"].ToString();
                        ModiD.Description = oReader["Description"].ToString();
                        ModiD.label = oReader["Code"].ToString();
                        ModiD.AnesthesiaUnits =0;
                        ModiD.AnesthesiaUnits = Convert.ToInt32(string.IsNullOrEmpty(oReader["AnesthesiaBaseUnits"].ToString()));
                        ModiD.Description2 = Convert.ToInt32(string.IsNullOrEmpty(oReader["DefaultFees"].ToString()));
                        
                        Modi.Add(ModiD);
                    }
                    myConnection.Close();
                }
            }

            return Modi;
        }

        private List<DropDown> POSs(ClientDbContext contextClient, string contextName, string UserId, string Role, long practiceId)
        {
            string newConnection = GetConnectionStringManager(contextName);
            List<DropDown> Pos = new List<DropDown>();
            using (SqlConnection myConnection = new SqlConnection(newConnection))
            {

                string oString = "Select* from POS";
                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    var pos = new DropDown();
                    pos.ID = null;
                    pos.Description = "Please Select";
                    pos.Value = "";
                    pos.label = "";
                    Pos.Add(pos);

                    while (oReader.Read())
                    {
                        var PosD = new DropDown();
                        PosD.ID = Convert.ToInt32(oReader["ID"]);
                        PosD.Description = oReader["PosCode"].ToString() + " - " + oReader["Name"].ToString();
                        PosD.Description2 = oReader["PosCode"].ToString();
                        Pos.Add(PosD);
                    }
                    myConnection.Close();
                }
            }

            return Pos;


        }

        private List<DropDown> UserRefProvider(ClientDbContext contextClient, string contextName, string UserId, string Role, long practiceId)
        {


            string newConnection = GetConnectionStringManager(contextName);
            List<DropDown> UserRefPro = new List<DropDown>();
            using (SqlConnection myConnection = new SqlConnection(newConnection))
            {

                string oString = "Select RefProvider.ID As RefProID,Practice.ID As PID,  RefProvider.Name As RefProName from RefProvider join Practice on RefProvider.PracticeID = Practice.ID where Practice.ID = practiceId";
                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                //oCmd.Parameters.AddWithValue("@RefProvider.PracticeID", practiceId);
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    var RefProDef = new DropDown();
                    RefProDef.ID = null;
                    RefProDef.ID2 = null;
                    RefProDef.Description = "Please Select";
                    RefProDef.Value = "";
                    RefProDef.label = "";
                    UserRefPro.Add(RefProDef);

                    while (oReader.Read())
                    {
                        var RefProD = new DropDown();
                        RefProD.ID = Convert.ToInt32(oReader["RefProID"]);
                        RefProD.ID2 = Convert.ToInt32(oReader["PID"]);
                        RefProD.Description = oReader["RefProName"].ToString();
                        RefProD.Value = "";
                        RefProD.label = oReader["RefProName"].ToString();
                        UserRefPro.Add(RefProD);
                    }
                    myConnection.Close();
                }
            }

            return UserRefPro;
        }

        private List<DropDown> UserProvider(ClientDbContext contextClient, string contextName, string UserId, string Role, long practiceId)
        {
            string newConnection = GetConnectionStringManager(contextName);
            List<DropDown> UserPro = new List<DropDown>();
            using (SqlConnection myConnection = new SqlConnection(newConnection))
            {

                string oString = "Select Provider.ID As ProviderID,Practice.ID As PID,  Provider.Name As ProviderName from Provider join Practice on Provider.PracticeID = Practice.ID where Practice.ID = practiceId";
                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                //oCmd.Parameters.AddWithValue("@RefProvider.PracticeID", practiceId);
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    var UserProDef = new DropDown();
                    UserProDef.ID = null;
                    UserProDef.ID2 = null;
                    UserProDef.Description = "Please Select";
                    UserProDef.Value = "";
                    UserProDef.label = "";
                    UserPro.Add(UserProDef);

                    while (oReader.Read())
                    {
                        var UserProD = new DropDown();
                        UserProD.ID = Convert.ToInt32(oReader["ProviderID"]);
                        UserProD.ID2 = Convert.ToInt32(oReader["PID"]);
                        UserProD.Description = oReader["ProviderName"].ToString();
                        UserProD.Value = "";
                        UserProD.label = oReader["ProviderName"].ToString();
                        UserPro.Add(UserProD);
                    }
                    myConnection.Close();
                }
            }

            return UserPro;
        }

        private object GenerateJwtToken(MainContext contextMain, string Role, string UserId)
        {
            var Clicntontext = (from u in contextMain.Users
                                join c in contextMain.MainClient
                                on u.ClientID equals c.ID
                                join p in contextMain.MainPractice
                                on u.PracticeID equals p.ID
                                where u.Id == UserId
                                select new
                                {
                                    c.ContextName,
                                    u.Email,
                                    p.ID
                                }).FirstOrDefault();

            List<Claim> claims = null;
            claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Email,Clicntontext.Email),
                new Claim(JwtRegisteredClaimNames.Jti, UserId),
                new Claim("Role",Role),
                new Claim("TEMP", Clicntontext.ContextName),
                new Claim("RandomKEY", Clicntontext.ID.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT-Key"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expire = DateTime.Now.AddDays(Convert.ToDouble(configuration["JWT-Expiry"]));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT-Issuer"],
                audience: configuration["JWT-Issuer"],
                claims: claims,
                expires: expire,
                signingCredentials: cred
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        //MEthod for Running Migration in All Clients///Currently  WOrking on it
        [Route("RunMigration")]
        [HttpPost]
        public void RunMigration()
        {
            var mainClient = (from u in _mainContext.MainClient
                              select u).ToList();
            foreach (var client in mainClient)
            {


                if (client.ID != null)
                {
                    // _clientContext.setDatabaseName(client.ContextName);
                    var optionsBuilder = new DbContextOptionsBuilder<ClientDbContext>();
                    string server = configuration["DatabaseSetting:server"].ToString();
                    string userid = configuration["DatabaseSetting:user id"].ToString();
                    string password = configuration["DatabaseSetting:password"].ToString();
                    optionsBuilder.UseSqlServer("server = " + server + " ; database = Medifusion" + client.ContextName + "; user id = " + userid + "; password = " + password + " ; ");
                    //optionsBuilder.UseSqlServer("server = 96.69.218.154\\SQLEXPRESS; database = Medifusion" + client.ContextName + "; user id = sa; password = Jay321@");
                    HttpContextAccessor httpContextAccessor = new HttpContextAccessor();
                    //HttpContextAccessor.HttpContext = this.HttpContext;
                    ClientDbContext NewContext = new ClientDbContext(optionsBuilder.Options, _config, httpContextAccessor, _env);
                    var cli = (from c in NewContext.Client where c.ID == client.ID select c).FirstOrDefault();
                    //               NewContext.Database.Migrate();

                }

            }
        }

        [Route("Get")]
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }


        public string GetConnectionStringManager(string contextName)
        {
            HttpContextAccessor httpContextAccessor = new HttpContextAccessor();

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            configuration = builder.Build();
            string connectionString = configuration.GetConnectionString("MedifusionLocal");
            string[] splitString = connectionString.Split(';');
            splitString[1] = splitString[1];

            if (contextName.IsNull())
                contextName = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("TEMP", StringComparison.InvariantCultureIgnoreCase)).Value;

            connectionString = splitString[0] + "; " + splitString[1] + contextName + "; " + splitString[2] + "; " + splitString[3];
            return connectionString;
            //var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            //return builder.Build().GetSection("ConnectionStrings").GetSection("MedifusionLocal").Value;
        }

    }
}
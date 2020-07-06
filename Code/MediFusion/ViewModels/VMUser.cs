using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MediFusionPM.Models;
using MediFusionPM.Models.Main;
using MediFusionPM.ViewModels.Main;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using static MediFusionPM.ViewModels.VMCommon;

namespace MediFusionPM.ViewModels
{

    public class VMUser
    {
        //public readonly IHostingEnvironment env;
        //public IConfiguration Configuration { get; set; }
        //public VMUser(IConfiguration configuration)
        //{
        //    //Configuration = configuration;
        //}

        public VMUser()
        {
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
        public string signatureURL { get; set; }
        public string signaturText { get; set; }
        public byte[] picbytes { get; set; }

      //  public ICollection<ICD> ICDList { get; set; }
        public void GetProfile(MainContext contextMain, string UserId, string Role)
        {

            if (Role == "SuperAdmin" || Role == "SuperUser")
            {
                Clients = (from u in contextMain.MainClient
                           select new DropDown()
                           {
                               ID = u.ID,
                               Description = u.Name//+ " - " + p.Coverage, 
                           }).ToList();
            }
            else
            {

                Clients = (from cli in contextMain.MainClient
                           join pra in contextMain.MainPractice
                           on cli.ID equals pra.ClientID
                           join up in contextMain.MainUserPractices
                           on pra.ID equals up.PracticeID
                           join usr in contextMain.Users
                           on up.UserID equals usr.Id
                           where usr.Id == UserId && up.Status == true
                           select new DropDown()
                           {
                               ID = cli.ID,
                               Description = cli.Name
                           }).Distinct().ToList();
            }
            Clients.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            var role = (from r in contextMain.Roles select new { r.Name });
            if (Role == "SuperAdmin")
            {
                //role = role.Where(m => m.Name != "SuperAdmin");
            }
            else if (Role == "SuperUser")
            {
                role = role.Where(m => m.Name != "SuperAdmin");
                //role = role.Where(m => m.Name != "SuperUser");
            }
            else if (Role == "Manager")
            {
                role = role.Where(m => m.Name != "SuperAdmin");
                role = role.Where(m => m.Name != "SuperUser");
                //role = role.Where(m => m.Name != "Manager");
                role = role.Where(m => m.Name != "ClientManager");
                role = role.Where(m => m.Name != "ClientUser");

            }
            else if (Role == "TeamLead")
            {
                role = role.Where(m => m.Name != "SuperAdmin");
                role = role.Where(m => m.Name != "SuperUser");
                role = role.Where(m => m.Name != "Manager");
                //role = role.Where(m => m.Name != "TeamLead");
                role = role.Where(m => m.Name != "ClientManager");
                role = role.Where(m => m.Name != "ClientUser");

            }
            else if (Role == "Biller")
            {
                role = role.Where(m => m.Name != "SuperAdmin");
                role = role.Where(m => m.Name != "SuperUser");
                role = role.Where(m => m.Name != "Manager");
                role = role.Where(m => m.Name != "TeamLead");
                //role = role.Where(m => m.Name != "Biller");
                role = role.Where(m => m.Name != "ClientManager");
                role = role.Where(m => m.Name != "ClientUser");

            }
            else if (Role == "ClientManager")
            {
                role = role.Where(m => m.Name != "SuperAdmin");
                role = role.Where(m => m.Name != "SuperUser");
                role = role.Where(m => m.Name != "Manager");
                role = role.Where(m => m.Name != "TeamLead");
                role = role.Where(m => m.Name != "Biller");
                //role = role.Where(m => m.Name != "ClientManager");

            }
            else if (Role == "ClientUser")
            {
                role = role.Where(m => m.Name != "SuperAdmin");
                role = role.Where(m => m.Name != "SuperUser");
                role = role.Where(m => m.Name != "Manager");
                role = role.Where(m => m.Name != "TeamLead");
                role = role.Where(m => m.Name != "Biller");
                role = role.Where(m => m.Name != "ClientManager");
                //role = role.Where(m => m.Name != "ClientUser");

            }

            else
            {
                //role = null;
                role = role.Where(m => m.Name != "SuperAdmin");
                role = role.Where(m => m.Name != "SuperUser");
                role = role.Where(m => m.Name != "Manager");
                role = role.Where(m => m.Name != "TeamLead");
                role = role.Where(m => m.Name != "Biller");
                role = role.Where(m => m.Name != "ClientManager");
                role = role.Where(m => m.Name != "ClientUser");

            }
            role = role.OrderBy(m => m.Name);
            list.Add("Please Select");
            foreach (var item in role.ToList())
            {
                list.Add(item.Name.ToString());

            }

            Teams = (from u in contextMain.MainTeam
                     select new DropDown()
                     {
                         ID = u.ID,
                         Description = u.Name, //+ " - " + p.Coverage, 
                     }).ToList();
            Teams.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            Designations = (from u in contextMain.MainDesignations
                            select new DropDown()
                            {
                                ID = u.ID,
                                Description = u.Name, //+ " - " + p.Coverage, 
                            }).ToList();
            Designations.Insert(0, new DropDown() { ID = null, Description = "Please Select" });
        }

        public void GetUserInfo(ClientDbContext contextClient, MainContext contextMain, string UserId, string Role)
        {
           // string fileName = string.Format("{0}_{1}.txt", Email, DateTime.Now.ToString("hhmmss"));

          //  File.AppendAllText(Path.Combine(contextMain.env.ContentRootPath, "Logs", fileName), "Getting User Info Start Time:" + DateTime.Now);

            var user = (from u in contextMain.Users
                        where u.Id == UserId
                        select u
                       ).First();
            Name = user.FirstName + ", " + user.LastName;
            Email = user.Email;

            PracticeID = user.PracticeID;
            ClientID = user.ClientID;
            UserRole = Role;



            //File.AppendAllText(Path.Combine(contextMain.env.ContentRootPath, "Logs", fileName), "Getting User Info End Time:" + DateTime.Now);

            //File.AppendAllText(Path.Combine(contextMain.env.ContentRootPath, "Logs", fileName), "Getting Clients Start Time:" + DateTime.Now);

            if (Role == "SuperAdmin" || Role == "SuperUser")
            {
                Clients = (from u in contextMain.MainClient
                           select new DropDown()
                           {
                               ID = u.ID,
                               Description = u.Name//+ " - " + p.Coverage, 
                           }).ToList();
            }
            else
            {

                Clients = (from cli in contextMain.MainClient
                           join pra in contextMain.MainPractice
                           on cli.ID equals pra.ClientID
                           join up in contextMain.MainUserPractices
                           on pra.ID equals up.PracticeID
                           join usr in contextMain.Users
                           on up.UserID equals usr.Id
                           where usr.Id == UserId
                           select new DropDown()
                           {
                               ID = cli.ID,
                               Description = cli.Name
                           }).Distinct().ToList();
            }

            //File.AppendAllText(Path.Combine(contextMain.env.ContentRootPath, "Logs", fileName), "Getting Clients End Time:" + DateTime.Now);


            //File.AppendAllText(Path.Combine(contextMain.env.ContentRootPath, "Logs", fileName), "Getting User Practices Start Time:" + DateTime.Now);

            // Fetching Practices From Main DB
            UserPractices = (from u in contextMain.MainUserPractices
                             join p in contextMain.MainPractice
                             on u.PracticeID equals p.ID
                             join w in contextMain.Users
                             on u.UserID equals w.Id
                             orderby p.Name ascending
                             where u.UserID == UserId
                             select new DropDown()
                             {
                                 ID = u.PracticeID,
                                 Description = p.Name
                             }).ToList();
            UserPractices.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            //File.AppendAllText(Path.Combine(contextMain.env.ContentRootPath, "Logs", fileName), "Getting User Practices End Time:" + DateTime.Now);


            //File.AppendAllText(Path.Combine(contextMain.env.ContentRootPath, "Logs", fileName), "Getting User Loc, Prov Start Time:" + DateTime.Now);


            try
            {
                if (PracticeID > 0)
                {
                    string contextName = contextMain.MainClient.Find(ClientID)?.ContextName;
                    contextClient.setDatabaseName(contextName);

                    UserLocations = (from loc in contextClient.Location
                                     join p in contextClient.Practice
                                      on loc.PracticeID equals p.ID
                                     where  p.ID == PracticeID
                                     select new DropDown()
                                     {
                                         ID = loc.ID,
                                         ID2 = p.ID,
                                         Description = loc.Name,
                                         Value = "",
                                         label = loc.Name
                                     }).ToList();


                    // UserLocations = (from u in contextClient.UserPractices
                    //join p in contextClient.Practice
                    //on u.PracticeID equals p.ID
                    //join w in contextClient.Users
                    //on u.UserID equals w.Id
                    //join loc in contextClient.Location
                    //on p.ID equals loc.PracticeID
                    //where u.UserID == UserId && p.ID == PracticeID
                    //select new DropDown()
                    //{
                    //    ID = loc.ID,
                    //    ID2 = p.ID,
                    //    Description = loc.Name,
                    //    Value = "",
                    //    label = loc.Name
                    //}).ToList();
                    UserLocations.Insert(0, new DropDown() { ID = null, Description = "Please Select" });



                    UserProviders = (from pro in contextClient.Provider
                                     join p in contextClient.Practice
                                      on pro.PracticeID equals p.ID
                                     where p.ID == PracticeID
                                     select new DropDown()
                                     {
                                         ID = pro.ID,
                                         ID2 = p.ID,
                                         Description = pro.Name,
                                         Value = "",
                                         label = pro.Name
                                     }).ToList();
                    UserProviders.Insert(0, new DropDown() { ID = null, Description = "Please Select" });


                    UserRefProviders = (from rPro in contextClient.RefProvider
                                        join p in contextClient.Practice
                                         on rPro.PracticeID equals p.ID
                                        where p.ID == PracticeID
                                        select new DropDown()
                                        {
                                            ID = rPro.ID,
                                            ID2 = p.ID,
                                            Description = rPro.Name,
                                            Value = "",
                                            label = rPro.Name
                                        }).ToList();
                    UserRefProviders.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

                    //CPT = (from i in contextClient.Cpt
                    //       select new Data()
                    //       {
                    //           ID = i.ID,
                    //           Value = i.Description,
                    //           label = i.CPTCode,
                    //           Description = i.Description,
                    //           Description1 = i.DefaultUnits,
                    //           Description2 = i.Amount,
                    //           AnesthesiaUnits = i.AnesthesiaBaseUnits,
                    //           Category = i.Category
                    //       }).ToList();


                    POS = (from p in contextClient.POS
                           select new DropDown()
                           {
                               ID = p.ID,
                               Description = p.PosCode + " - " + p.Name,
                               Description2 = p.PosCode,
                           }).ToList();
                    POS.Insert(0, new DropDown() { ID = null, Description = "Please Select" });


                    Modifier = (from i in contextClient.Modifier
                                select new Data()
                                {
                                    ID = i.ID,
                                    Value = i.Description,
                                    label = i.Code,
                                    Description = i.Description,
                                    AnesthesiaUnits = i.AnesthesiaBaseUnits.Value,
                                    Description2 = i.DefaultFees
                                }).ToList();

                    //Taxonomy = (from i in contextClient.Taxonomy
                    //            select new Data()
                    //            {
                    //                ID = i.ID,
                    //                Value = i.NUCCCode,
                    //                label = i.NUCCCode,
                    //            }).ToList();


                    //ICD = (from i in contextClient.ICD
                    //       select new Data()
                    //       {
                    //           ID = i.ID,
                    //           Value = i.Description,
                    //           label = i.ICDCode,
                    //           Description = i.Description,

                    //       }).ToList();

                }

            }
            catch (Exception)
            {
                if (PracticeID > 0)
                {
                    throw;
                }
            }

            //File.AppendAllText(Path.Combine(contextMain.env.ContentRootPath, "Logs", fileName), "Getting User Loc, Prov End Time:" + DateTime.Now);


            //File.AppendAllText(Path.Combine(contextMain.env.ContentRootPath, "Logs", fileName), "Getting Teams Start Time:" + DateTime.Now);

            //Teams = (from u in contextMain.MainTeam
            //         select new DropDown()
            //         {
            //             ID = u.ID,
            //             Description = u.Name, //+ " - " + p.Coverage, 
            //         }).ToList();
            //Teams.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            //File.AppendAllText(Path.Combine(contextMain.env.ContentRootPath, "Logs", fileName), "Getting Teams End Time:" + DateTime.Now);


            //File.AppendAllText(Path.Combine(contextMain.env.ContentRootPath, "Logs", fileName), "Getting Designations Start Time:" + DateTime.Now);


            //Designations = (from u in contextMain.MainDesignations
            //                select new DropDown()
            //                {
            //                    ID = u.ID,
            //                    Description = u.Name, //+ " - " + p.Coverage, 
            //                }).ToList();
            //Designations.Insert(0, new DropDown() { ID = null, Description = "Please Select" });



            Rights = (from u in contextMain.MainRights
                      where u.Id == UserId
                      select u
                      ).FirstOrDefault();

            if (PracticeID > 0)
            {
                //Token = GenerateJwtToken(contextMain, Role, UserId);
            }


            

        }



        public void FindUser(MainContext contextMain, string Em, string DirectoryPath)
        {
           
            var user = (from u in contextMain.Users
                        join v in contextMain.UserRoles
                        on u.Id equals v.UserId
                        join w in contextMain.Roles
                        on v.RoleId equals w.Id
                        where u.Email == Em
                        select new
                        {
                            u.Id,
                            u.FirstName,
                            u.LastName,
                            u.PracticeID,
                            u.ClientID,
                            u.Email,
                            w.Name,
                            u.TeamID,
                            u.DesignationID,
                            u.ReportingTo,
                              u.signatureURL,
                            u.signatureText

                        }
                  ).FirstOrDefault();
            byte[] formText=null;
            if (user.signatureURL != null && user.signatureURL != "")
            {  formText = System.IO.File.ReadAllBytes(System.IO.Path.Combine(DirectoryPath, user.signatureURL)); }



            FirstName = user.FirstName;
            LastName = user.LastName;
            PracticeID = user.PracticeID;
            ClientID = user.ClientID;
            Email = user.Email;
            UserRole = user.Name;
            TeamID = user.TeamID;
            DesignationID = user.DesignationID;
            ReportingTo = user.ReportingTo;
            signatureURL =  user.signatureURL;
            signaturText= user.signatureText;
            picbytes = formText!=null? formText:null;
            assignedUserPractices = (from u in contextMain.MainUserPractices
                                     join v in contextMain.MainPractice
                                     on u.PracticeID equals v.ID
                                     join w in contextMain.Users
                                     on u.UserID equals w.Id
                                     join x in contextMain.MainClient
                                     on v.ClientID equals x.ID
                                     where u.UserID == user.Id && u.Status == true
                                     select new AssignedUserPractices()
                                     {
                                         PracticeID = u.PracticeID,
                                         PracticeName = v.Name,
                                         Status = u.Status,
                                         ClientId = x.ID,
                                         ClientName = x.Name,
                                         Email = w.Email
                                     }).ToList();

        }





        public class AssignedUserPractices
        {
            public long? PracticeID { get; set; }
            public bool Status { get; set; }
            public long? ClientId { get; set; }
            public string Email { get; set; }
            public string PracticeName { get; set; }
            public string ClientName { get; set; }
        }
        public class CUser
        {
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string Email { get; set; }
            public long? ClientID { get; set; }
            public long? TeamID { get; set; }

        }
        public class GUser
        {
            public string UserId { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
            public string TeamName { get; set; }
            public string Team { get; set; }
            public string DesignationName { get; set; }
            public string ReportingTo { get; set; }
            public string signatureURL { get; set; }
            public string signatureText { get; set; }

        }

    }

}


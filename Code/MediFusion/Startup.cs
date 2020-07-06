using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MediFusionPM.Models;
using MediFusionPM.Models.TodoApi.Models;
using Microsoft.EntityFrameworkCore;
using Elmah.Io.AspNetCore;
using MediFusionPM.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MediFusionPM.ViewModels.Main;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Hangfire;
using Hangfire.SqlServer;
using MediFusionPM.Controllers;

namespace MediFusionPM
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(MyAllowSpecificOrigins,
            //    builder =>
            //    {
            //        builder.WithOrigins("http://192.168.110.59:3000", "http://localhost:3000");
            //    });
            //});


            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });


            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                      });
            });

            //services.AddDbContext<PMContext>(opt =>
            //  opt.UseInMemoryDatabase("TodoList"));

            services.AddDbContext<MainContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("Medifusion"),
            sqlServerOptions => sqlServerOptions.CommandTimeout(96000)));
            services.AddDbContext<ClientDbContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("MedifusionLocal"),
            sqlServerOptions => sqlServerOptions.CommandTimeout(96000)));

            //services.AddDbContext<MainContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("Medifusion"),
            //sqlServerOptions => sqlServerOptions.CommandTimeout(96000).EnableRetryOnFailure()));
            //services.AddDbContext<ClientDbContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("MedifusionLocal"),
            //sqlServerOptions => sqlServerOptions.CommandTimeout(96000).EnableRetryOnFailure()));


            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddIdentity<MainAuthIdentityCustom, IdentityRole>(m =>
            {
                m.Password.RequiredLength = 6;
                m.Password.RequireLowercase = false;
                m.Password.RequireUppercase = false;
                m.Password.RequireDigit = true;
                m.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<MainContext>().AddDefaultTokenProviders();





            services.AddElmahIo(o =>
            {
                o.ApiKey = Configuration["ElmahSettings:ApiKey"];
                o.LogId = new Guid(Configuration["ElmahSettings:LogId"]);
            });


            services.ConfigureApplicationCookie(m => {
                m.Cookie.HttpOnly = true;
                m.ExpireTimeSpan = TimeSpan.FromHours(1);
                m.SlidingExpiration = true;
                m.LoginPath = "";
                m.LogoutPath = "";
                m.AccessDeniedPath = "";
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(m => {
                m.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                m.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                m.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(conf => {
                conf.RequireHttpsMetadata = false;
                conf.SaveToken = true;
                conf.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = Configuration["JWT-Issuer"],
                    ValidAudience = Configuration["JWT-Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT-Key"])),
                    ClockSkew = TimeSpan.Zero,
                };
            });

            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });

            services.AddHangfire(configuration => configuration
               .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
               .UseSimpleAssemblyNameTypeSerializer()
               .UseRecommendedSerializerSettings()
               .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
               {
                   CommandBatchMaxTimeout = TimeSpan.FromHours(2),
                   SlidingInvisibilityTimeout = TimeSpan.FromHours(2),
                   CommandTimeout = TimeSpan.FromHours(2),
                   TransactionTimeout = TimeSpan.FromHours(2),
                   EnableHeavyMigrations = true,
                   QueuePollInterval = TimeSpan.Zero,
                   UseRecommendedIsolationLevel = true,
                   UsePageLocksOnDequeue = true,
                   DisableGlobalLocks = true
               }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            //services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession();
            services.AddSession();
            services.AddMvc()
                .AddJsonOptions(
                    options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                ).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IRecurringJobManager recurringJobsManager, IHostingEnvironment env, IServiceProvider servPro)
        {
            //if (env.IsDevelopment())
            //{
            app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseHsts();
            //}
            app.UseCors("AllowAllHeaders");

            //app.UseHangfireDashboard();
            string FollowUpTime = Configuration["AutoFollowupJobSettings:startTime"];
            string DownloadFileTime = Configuration["AutoDownloadingJobSetting:startTime"];
            
            //app.UseHangfireServer();
            recurringJobsManager.AddOrUpdate<AutoJobsController>("createfollowupjob", j=>j.CreateAutoFollowups(), FollowUpTime, TimeZoneInfo.Local, "default");
            recurringJobsManager.AddOrUpdate<AutoJobsController>("downloadfilejob", j=>j.DownloadFiles(), DownloadFileTime, TimeZoneInfo.Local, "default");



            app.Use(async (context, next) =>
            {
              //  string ExceptionLogPath = Path.Combine(env.ContentRootPath, "wwwroot", "Middleware Exception Log.txt");
                try
                {
                    
                    if (context.Request.Path.HasValue)

                    {
                        //Requests.Add(context.Request.Path, "Request Path " + context.Request.Path + "; Request Entry Time " + DateTime.Now + ";\n");
                        string InOutLog = Path.Combine(env.ContentRootPath, "wwwroot", "InOutLog.txt");
                        if (!File.Exists(InOutLog))
                        {
                            File.WriteAllText(InOutLog, "Request Path " + context.Request.Path + "; Request Entry Time " + DateTime.Now + ";\n");
                        }
                        else
                        {
                            File.AppendAllText(InOutLog, "Request Path " + context.Request.Path + "; Request Entry Time " + DateTime.Now + ";\n");
                        }

                        if (context.Request.Path.Value.Contains("accessible-files"))
                        {
                            StringValues response;
                            string fileName = Path.Combine(env.ContentRootPath, "wwwroot", "middlewareLog.txt");
                            //await next(); 
                            //return; 
                            StreamWriter sw;
                            if (!File.Exists(fileName))
                            {
                                sw = File.CreateText(fileName);
                                sw.WriteLine(DateTime.Now.ToString());
                                sw.WriteLine("request path " + context.Request.Path + " ");
                                sw.WriteLine("remote address " + context.Connection.RemoteIpAddress.ToString() + " ");
                                response = "no header";
                                if (context.Request.Headers.TryGetValue("Origin", out response))
                                {
                                    sw.WriteLine("origin " + response);
                                    string incomingIP = response;
                                    //string IPtoCompare = "192.168.104.101";
                                    string IPtoCompare = "http://96.69.218.154:8010";
                                    List<string> allowedIPs = new List<string>();
                                    allowedIPs.Add("http://96.69.218.154:8010");
                                    allowedIPs.Add("https://service.medifusion.com");
                                    allowedIPs.Add("https://app.medifusion.com");
                                    allowedIPs.Add("http://192.168.104.105");
                                    allowedIPs.Add("http://192.168.104.101");
                                    allowedIPs.Add("http://localhost:3000");
                                    allowedIPs.Add("http://96.69.218.154:8050/");
                                    //sw.WriteLine("incoming ip+length "+incomingIP.Split(':')[0] + " " + incomingIP.Split(':')[0].Length + " ");
                                    sw.WriteLine("origin details " + response.ToString() + " " + response.ToString().Length + " ");

                                    if (allowedIPs.Contains(response))
                                    {
                                        sw.WriteLine("Origin " + response);
                                        sw.WriteLine("matched");
                                        sw.WriteLine("");
                                        sw.WriteLine("");
                                        sw.WriteLine("");
                                        sw.Close();
                                        sw.Dispose();
                                        await next();
                                        return;
                                    }
                                    else
                                    {
                                        sw.WriteLine("Origin " + response);
                                        sw.WriteLine("not matched");
                                        sw.WriteLine("");
                                        sw.WriteLine("");
                                        sw.WriteLine("");
                                        sw.Close();
                                        sw.Dispose();
                                        await context.Response.WriteAsync("Cannot access this file");
                                        return;
                                        //throw new AccessViolationException("cant access this folder");
                                    }
                                }
                                else
                                {
                                    sw.WriteLine("Origin " + response);
                                    sw.WriteLine("not matched");
                                    sw.WriteLine("");
                                    sw.WriteLine("");
                                    sw.WriteLine("");
                                    sw.Close();
                                    sw.Dispose();
                                    await context.Response.WriteAsync("Cannot access this file");
                                    return;
                                }
                            }

                            sw = File.AppendText(fileName);
                            sw.WriteLine(DateTime.Now.ToString());
                            sw.WriteLine("request path " + context.Request.Path + " ");
                            sw.WriteLine("remote address " + context.Connection.RemoteIpAddress.ToString() + " ");
                            if (context.Request.Headers.TryGetValue("Origin", out response))
                            {
                                sw.WriteLine("origin " + response);
                                string incomingIP = response;
                                //string IPtoCompare = "192.168.104.101";
                                string IPtoCompare = "http://96.69.218.154:8010";
                                List<string> allowedIPs = new List<string>();
                                allowedIPs.Add("http://96.69.218.154:8010");
                                allowedIPs.Add("https://service.medifusion.com");
                                allowedIPs.Add("https://app.medifusion.com");
                                allowedIPs.Add("http://192.168.104.105");
                                allowedIPs.Add("http://192.168.104.101");
                                allowedIPs.Add("http://localhost:3000");
								allowedIPs.Add("http://96.69.218.154:8050/");
                                //sw.WriteLine("incoming ip+length "+incomingIP.Split(':')[0] + " " + incomingIP.Split(':')[0].Length + " ");
                                sw.WriteLine("origin details " + response.ToString() + " " + response.ToString().Length + " ");

                                if (allowedIPs.Contains(response))
                                {
                                    sw.WriteLine("Origin " + response);
                                    sw.WriteLine("matched");
                                    sw.WriteLine("");
                                    sw.WriteLine("");
                                    sw.WriteLine("");
                                    sw.Close();
                                    sw.Dispose();
                                    await next();
                                    return;
                                }
                                else
                                {
                                    sw.WriteLine("Origin "+response);
                                    sw.WriteLine("not matched");
                                    sw.WriteLine("");
                                    sw.WriteLine("");
                                    sw.WriteLine("");
                                    sw.Close();
                                    sw.Dispose();
                                    await context.Response.WriteAsync("Cannot access this file");
                                    return;
                                    //throw new AccessViolationException("cant access this folder");
                                }
                            }
                            else
                            {
                                sw.WriteLine("Origin " + response);
                                sw.WriteLine("not matched");
                                sw.WriteLine("");
                                sw.WriteLine("");
                                sw.WriteLine("");
                                sw.Close();
                                sw.Dispose();
                                await context.Response.WriteAsync("Cannot access this file");
                                return;
                            }

                        }
                        else
                        {
                            await next();
                            File.AppendAllText(InOutLog, "Request Path " + context.Request.Path + "; Request Exit Time " + DateTime.Now + "; Response Status Code " + context.Response.StatusCode + ";\n");
                            return;
                        }
                    }
                } catch(Exception Ex)
                {
                    //if (!File.Exists(ExceptionLogPath))
                    //{
                    //    File.WriteAllText(ExceptionLogPath,context.Request.Path+"\t\t"+Ex.Message+"\n"+Ex.StackTrace+"\n");
                    //}
                    //else
                    //{
                    //    File.AppendAllText(ExceptionLogPath, context.Request.Path + "\t\t" + Ex.Message + "\n" + Ex.StackTrace + "\n");
                    //}
                }
            });
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Home/Error");


            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(
            //    Path.Combine(Directory.GetCurrentDirectory(), @"MyStaticFiles")),
            //    RequestPath = new PathString("/StaticFiles")
            //});
        


        app.UseElmahIo();

            app.UseHttpsRedirection();
            app.UseAuthentication();


            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = context =>
                {
                    context.Context.Response.Headers.Add("Cache-Control", "no-cache, no-store");
                    context.Context.Response.Headers.Add("Expires", "-1");
                }
            });


            //app.UseHttpsRedirection();

            //app.UseCors(MyAllowSpecificOrigins);

            app.UseSession();

            app.UseMvc();

            CreateUserRoles(servPro).Wait();
        }

        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<MainAuthIdentityCustom>>();

            var roleCheck = await RoleManager.RoleExistsAsync("SuperAdmin");
            if (!roleCheck)
            {
                await RoleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                await RoleManager.CreateAsync(new IdentityRole("SuperUser"));
                await RoleManager.CreateAsync(new IdentityRole("Manager"));
                await RoleManager.CreateAsync(new IdentityRole("TeamLead"));
                await RoleManager.CreateAsync(new IdentityRole("Biller"));
                await RoleManager.CreateAsync(new IdentityRole("ClientManager"));
                await RoleManager.CreateAsync(new IdentityRole("ClientUser"));

            }

            var userchk = await UserManager.FindByEmailAsync("super@bellmedex.com");

            if (userchk == null)
            {
                var User = new MainAuthIdentityCustom();
                User.UserName = "super@bellmedex.com";
                User.Email = "super@bellmedex.com";
                string pass = "super@Superadmin$123";

                var chk = await UserManager.CreateAsync(User, pass);
                if (chk.Succeeded)
                {
                    await UserManager.AddToRoleAsync(User, "SuperAdmin");
                }
            }
            else
            {
                await UserManager.AddToRoleAsync(userchk, "SuperAdmin");
            }


        }
    }
}

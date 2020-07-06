using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Wordprocessing;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using MediFusionPM.Models;
using MediFusionPM.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using static MediFusionPM.ViewModels.VMCommon;
using static MediFusionPM.ViewModels.VMPatientAppointment;
using static MediFusionPM.ViewModels.VMProviderSchedule;

namespace MediFusionPM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    [Authorize]
    public class PatientAppointmentController : Controller
    {
       static readonly string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        string ApplicationName = "Bellmedex";
        UserCredential credential;
        private readonly ClientDbContext _context;
        private readonly MainContext _contextMain;
        private DateTime _startTime = DateTime.MinValue;
        public IConfiguration _config;
        public PatientAppointmentController(ClientDbContext context, MainContext contextMain, IConfiguration _config)
        {
            _context = context;
            _contextMain = contextMain;
            _startTime = DateTime.Now;
            this._config = _config;
        }

        [HttpGet]
        [Route("GetPatientAppointments")]
        public async Task<ActionResult<IEnumerable<PatientAppointment>>> GetPatientAppointments()
        {
            try
            {
                return await _context.PatientAppointment.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("VacantSlots")]
        public List <GProviderSchedule> VacantSlots(VacantSlots VacantSlots)
        {
            //  return await 
            List<GProviderSchedule> slots =(from ps in _context.ProviderSchedule
                          join pSlot in _context.ProviderSlot on ps.ID equals pSlot.ProviderScheduleID
                          join pro in _context.Provider on ps.ProviderID equals pro.ID into Table1
                          from t1 in Table1.DefaultIfEmpty()
                          join loc in _context.Location on ps.LocationID equals loc.ID into Table2
                          from t2 in Table2.DefaultIfEmpty()
                          where ps.ProviderID == VacantSlots.ProviderID 
                          && (VacantSlots.LocationID.IsNull() ? true : ps.LocationID.Equals(VacantSlots.LocationID))
                          && (VacantSlots.AppointmentDate == null ? true : object.Equals(VacantSlots.AppointmentDate, pSlot.FromDate))
                          && pSlot.FromDate == VacantSlots.AppointmentDate
                          && pSlot.Status == "A"
                          select new GProviderSchedule()
                          {
                              ID = pSlot.ID,
                              ProviderID = t1.ID,
                              Provider = t1.Name,
                              LocationID = t2.ID,
                              Location = t2.Name,
                              FromTime = pSlot.FromTime.Format("hh:mm tt"),
                              //ToDate = ps.ToDate.Format("MM/dd/yyyy"),
                              //TimeInterval = ps.TimeInterval.ToString(),
                              //FromTime = ps.FromTime.ToString(),
                              //ToTime = ps.ToTime.ToString(),
                          }).ToList<GProviderSchedule>();
            
            slots.Insert(0, new GProviderSchedule() { ID = null, FromTime = "" });
            return slots;
        }

        [Route("FindPatientInfo/{PatientID}")]
        [HttpGet("{PatientID}")]
        public    ActionResult FindPatientInfo(long PatientID)
        {
            var data = (from p in _context.Patient
                                        join  pPlan in _context.PatientPlan.Where(p => p.Coverage.Equals("P")) on p.ID equals pPlan.PatientID
                                        into Table1
                        from PPlanT in Table1.DefaultIfEmpty()  //.Where(p=>p.Coverage.Equals("P"))
                        join iPlan in _context.InsurancePlan on PPlanT.InsurancePlanID equals iPlan.ID into Table2
                        from iPlanT in Table2.DefaultIfEmpty()  //.Where(p=>p.Coverage.Equals("P"))
                        // && pPlan.Coverage.Equals("P") && pPlan.IsActive == true
                        where p.ID == PatientID && (PPlanT!=null?PPlanT.IsActive:true)
                        //   && ((PPlanT.ID!=null && PPlanT.ID>0)? PPlanT.Coverage.Equals("P"):true )
                        //  && ((PPlanT.ID != null && PPlanT.ID > 0) ? (PPlanT.IsActive==true) : true)
                        select new  
                                        {
                                            PatientID = p.ID.ToString(),
                                            PatientName = p.LastName + ", " + p.FirstName,
                                            DOB = p.DOB!=null ? p.DOB.Format("MM/dd/yyyy"):"",
                                            PhoneNumber = p.PhoneNumber,
                                            PlanName = PPlanT != null ? iPlanT.PlanName:"",
                                            SubscriberID = PPlanT != null ? PPlanT.SubscriberId:"",
                                            PatientPlanID = PPlanT!=null ? PPlanT.ID:0,

                                        }).SingleOrDefault();
            if (data != null)
            {
                return Json(data);
            }
            else
                return null;

        }

        [Route("GetProfiles")]
        [HttpGet()]
        public async Task<ActionResult<VMPatientAppointment>> GetProfiles(long id)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

            string UserID = UD.UserID;
            long PracticeID = UD.PracticeID;
            ViewModels.VMPatientAppointment obj = new ViewModels.VMPatientAppointment();
            obj.GetProfiles(_context, UserID, PracticeID);
            
            return obj;
        }

        [Route("PatientAppointmentDetails/{id}")]
        [HttpGet("{id}")]
        public ActionResult PatientAppointmentDetails(long id)
        {

            
            var patientAppointment = (from pa in _context.PatientAppointment
                                      join p in _context.Patient on pa.PatientID equals p.ID 
                                      join gi in _context.GeneralItems on pa.Status  equals gi.Value into giTable
                                      from giT in giTable.DefaultIfEmpty()
                                      join pn in _context.PatientNotes on pa.ID equals pn.AppointmentID into Table3
                                      from pnT in Table3.DefaultIfEmpty()
                                      join pp in _context.PatientPlan on pa.PatientID equals pp.PatientID into Table1
                                      from ppT in Table1.DefaultIfEmpty()
                                      join ip in _context.InsurancePlan on ppT.InsurancePlanID equals ip.ID into Table2
                                      from ipT in Table2.DefaultIfEmpty()
                                      where pa.ID == id && (pa.Inactive ?? false != true)
                                      select new
                                      {
                                          Patient = p.LastName + ", " + p.FirstName,
                                          PatientID = p.ID,
                                          pa.ID,
                                          pa.LocationID, 
                                          pa.ProviderID,
                                          PrimarypatientPlanID=ExtensionMethods.IsNull( pa.PrimarypatientPlanID)? "": pa.PrimarypatientPlanID.ToString(),
                                          PlanName = ipT.PlanName,
                                          pa.VisitReasonID,
                                          AppointmentDate= pa.AppointmentDate.Format("MM/dd/yyyy"),
                                          AppointmentTime = pa.Time.Format("hh:mm tt"),
                                          pa.VisitInterval,
                                          pa.Status,
                                          StatusColor = giT != null ? giT.Description : "",
                                          patientNotesId = pnT!=null ? pnT.ID :0,
                                          pa.Notes,
                                          pa.AddedBy,
                                          pa.AddedDate,
                                          pa.UpdatedBy,
                                          pa.UpdatedDate,
                                          pa.color,
                                          pa.RoomID, 
                                          pa.Inactive,
                                          pa.recurringAppointment,
                                          pa.recurringNumber,
                                          pa.recurringfrequency,
                                          pa.priorAuthorization

                                      }).FirstOrDefault();
            ;

            if (patientAppointment == null)
            {
                return NotFound();
            }
            //patientAppointment.AppointmentTime = patientAppointment.Time.Format("hh:mm tt");
            var forms = (from pf in _context.PatientForms
                         join cf in _context.ClinicalForms on pf.ClinicalFormID equals cf.ID
                         where pf.PatientAppointmentID == id && pf.Inactive != true
                         select new
                         {
                             ID = pf.ID,
                             PatientAppointmentID = pf.PatientAppointmentID,
                             PatientID = pf.PatientID,
                             ClinicalFormID = pf.ClinicalFormID,
                             PracticeID = pf.PracticeID,
                             Inactive = pf.Inactive,
                             AddedBy = pf.AddedBy,
                             AddedDate = pf.AddedDate,
                             UpdatedBy = pf.UpdatedBy,
                             UpdatedDate = pf.UpdatedDate,
                             form=cf.Name
                         }
                                       ).ToList();
            /*var cpt=null;
            if (patientAppointment.VisitID!=null && patientAppointment.VisitID>0)
            {
                  cpt = (from ac in _context.AppointmentCPT
                           join i in _context.Cpt on ac.CPTID equals i.ID
                           where ac.AppointmentID == id && (ac.Inactive ?? false != true)
                           select new
                           {
                               ID = ac.ID,
                               AppointmentID = ac.AppointmentID,
                               CPTID = ac.CPTID,
                               Modifier1 = ac.Modifier1,
                               Modifier2 = ac.Modifier2,
                               NdcUnits = ac.NdcUnits,
                               Units = ac.Units,
                               Amount = ac.Amount,
                               TotalAmount = ac.TotalAmount,
                               PracticeID = ac.PracticeID,
                               Inactive = ac.Inactive,
                               AddedBy = ac.AddedBy,
                               AddedDate = ac.AddedDate,
                               UpdatedBy = ac.UpdatedBy,
                               UpdatedDate = ac.UpdatedDate,
                               Description = (i.ShortDescription != null && i.ShortDescription.Length > 0) ? i.ShortDescription : i.Description
                           }
                                       )
                                      .ToList();
            }*/
            var cpt = (from ac in _context.AppointmentCPT
                       join i in _context.Cpt on ac.CPTID equals i.ID
                       where ac.AppointmentID == id && (ExtensionMethods.IsNull(ac.Inactive) ? false : ac.Inactive != true)
                       select new
                       {
                           ID = ac.ID,
                           AppointmentID = ac.AppointmentID,
                           CPTMostFavouriteID = ac.CPTMostFavouriteID,
                           CPTID = ac.CPTID,
                           i.CPTCode,
                           Modifier1 = ac.Modifier1,
                           Modifier2 = ac.Modifier2,
                           NdcUnits = ac.NdcUnits,
                           Pointer1 = ac.Pointer1,
                           Pointer2 = ac.Pointer2,
                           Pointer3 = ac.Pointer3,
                           Pointer4 = ac.Pointer4,
                           ChargeID = ac.ChargeID,                           
                           Units = ac.Units,
                           Amount = ac.Amount,
                           TotalAmount = ac.TotalAmount,
                           PracticeID = ac.PracticeID,
                           Inactive = ac.Inactive,
                           AddedBy = ac.AddedBy,
                           AddedDate = ac.AddedDate,
                           UpdatedBy = ac.UpdatedBy,
                           UpdatedDate = ac.UpdatedDate,
                           Description = (i.ShortDescription != null && i.ShortDescription.Length > 0) ? i.ShortDescription : i.Description
                       }
                                       )
                                      .ToList();
            var icd =(from ai in _context.AppointmentICD
                      join i in _context.ICD on ai.ICDID equals i.ID 
                                       where ai.AppointmentID == id && (ai.Inactive  != true)
                      select new
                                       {
                                           ID = ai.ID,
                                           ICDID = ai.ICDID,
                                           ICDMostFavouriteID = ai.ICDMostFavouriteID,
                                           i.ICDCode,
                                           SerialNo = ai.SerialNo,
                                           AppointmentID = ai.AppointmentID,
                                           PracticeID = ai.PracticeID,
                                           Inactive = ai.Inactive,
                                           AddedBy = ai.AddedBy,
                                           AddedDate = ai.AddedDate,
                                           UpdatedBy = ai.UpdatedBy,
                                           UpdatedDate = ai.UpdatedDate,
                                           i.Description
                                       }
                                       )
                                      .ToList();
            var patientPayment = (from ai in _context.PatientPayment 
                       where ai.patientAppointmentID == id && (ai.InActive != true)
                                  select new
                       {
                           ai.ID,
                           ai.PatientID,
                           ai.patientAppointmentID,
                           ai.PaymentMethod,
                           ai.PaymentDate,
                           ai.PaymentAmount,
                           ai.CheckNumber,
                           ai.Description,
                           ai.Status,
                           ai.AllocatedAmount,
                           ai.RemainingAmount,
                           ai.AddedBy,
                           ai.InActive,
                           ai.AddedDate,
                           ai.UpdatedBy,
                           ai.UpdatedDate,
                           ai.Type,
                           ai.VisitID,
                           ai.CCTransactionID
                       }
                                     )
                                    .ToList();

            var ret = new { Appointment = patientAppointment, Forms = forms, CPT = cpt, ICD = icd ,patientPayment= patientPayment };
            return Json(ret);
        }

        [Route("PatientAppointmentEncounter/{id}")]
        [HttpGet("{id}")]
        public ActionResult PatientAppointmentEncounter(long id)
        {
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            var patientAppointment = (from pa in _context.PatientAppointment
                                      join p in _context.Patient on pa.PatientID equals p.ID
                                      join prac in _context.Practice on p.PracticeID equals prac.ID
                                      join pr in _context.Provider on pa.ProviderID equals pr.ID
                                      join pRef in _context.RefProvider on p.RefProviderID equals pRef.ID into pRefTable
                                      from pRefT in pRefTable.DefaultIfEmpty()
                                      join lc in _context.Location on pa.LocationID equals lc.ID
                                      join pp in _context.PatientPlan on pa.PatientID equals pp.PatientID into Table1
                                      from ppT in Table1.DefaultIfEmpty()
                                      join ip in _context.InsurancePlan on ppT.InsurancePlanID equals ip.ID into Table2
                                      from ipT in Table2.DefaultIfEmpty()
                                      where pa.ID == id && ExtensionMethods.IsNull_Bool(pa.Inactive) == false
                                      select new
                                      {
                                          Patient = p.LastName + ", " + p.FirstName,
                                          PatientID = p.ID,
                                          pa.ID,
                                          pa.LocationID,
                                          pa.ProviderID,
                                          PrimarypatientPlanID = ExtensionMethods.IsNull(pa.PrimarypatientPlanID) ? "" : pa.PrimarypatientPlanID.ToString(),
                                          PrimaryPlanName = ipT.PlanName,
                                          PolicyNumber= ppT!=null? ppT.SubscriberId:"",
                                          pa.VisitReasonID,
                                          DOS= pa.AppointmentDate.Format("MM/dd/yyyy"),
                                          DOB = p.DOB.Format("MM/dd/yyyy"),
                                          practiceName = prac.Name,
                                          practiceAddress = prac.Address1??""+" "+ prac.City ?? "" + " "+ prac.State ?? "" + " "+ prac.ZipCode ?? "",
                                          location =lc.Name,
                                          provider=pr.LastName+", "+pr.FirstName,
                                          refProvider = pRefT!=null ? pRefT.FirstName+", "+ pRefT.LastName:"",
                                          AppointmentTime = pa.Time.Format("hh:mm tt"),
                                          pa.VisitInterval,
                                          pa.Status,
                                          pa.Notes,
                                          pa.AddedBy, 
                                          AddedDate = pa.AddedDate.Format("MM/dd/yyyy hh:mm tt"),
                                          pa.UpdatedBy, 
                                          UpdatedDate = pa.UpdatedDate.Format("MM/dd/yyyy hh:mm tt"),
                                          pa.color,
                                          pa.RoomID,
                                          pa.Inactive

                                      }).FirstOrDefault();
            ;

            if (patientAppointment == null)
            {
                return NotFound();
            }
            //patientAppointment.AppointmentTime = patientAppointment.Time.Format("hh:mm tt");
            /*   var forms = (from pf in _context.PatientForms
                            join cf in _context.ClinicalForms on pf.ClinicalFormID equals cf.ID
                            where pf.PatientAppointmentID == id && pf.Inactive != true
                            select new
                            {
                                ID = pf.ID,
                                PatientAppointmentID = pf.PatientAppointmentID,
                                PatientID = pf.PatientID,
                                ClinicalFormID = pf.ClinicalFormID,
                                PracticeID = pf.PracticeID,
                                Inactive = pf.Inactive,
                                AddedBy = pf.AddedBy,
                                AddedDate = pf.AddedDate,
                                UpdatedBy = pf.UpdatedBy,
                                UpdatedDate = pf.UpdatedDate,
                                form = cf.Name
                            }
                                          ).ToList();*/
            /*var cpt=null;
            if (patientAppointment.VisitID!=null && patientAppointment.VisitID>0)
            {
                  cpt = (from ac in _context.AppointmentCPT
                           join i in _context.Cpt on ac.CPTID equals i.ID
                           where ac.AppointmentID == id && (ac.Inactive ?? false != true)
                           select new
                           {
                               ID = ac.ID,
                               AppointmentID = ac.AppointmentID,
                               CPTID = ac.CPTID,
                               Modifier1 = ac.Modifier1,
                               Modifier2 = ac.Modifier2,
                               NdcUnits = ac.NdcUnits,
                               Units = ac.Units,
                               Amount = ac.Amount,
                               TotalAmount = ac.TotalAmount,
                               PracticeID = ac.PracticeID,
                               Inactive = ac.Inactive,
                               AddedBy = ac.AddedBy,
                               AddedDate = ac.AddedDate,
                               UpdatedBy = ac.UpdatedBy,
                               UpdatedDate = ac.UpdatedDate,
                               Description = (i.ShortDescription != null && i.ShortDescription.Length > 0) ? i.ShortDescription : i.Description
                           }
                                       )
                                      .ToList();
            }*/
            var cpt = (from favCpt in _context.CPTMostFavourite
                       join i in _context.Cpt on favCpt.CPTID equals i.ID
                       join gi in _context.GeneralItems on favCpt.Type equals gi.Type into Table2
                       from giT in Table2.DefaultIfEmpty()
                       join ac in (_context.AppointmentCPT.Where(w => w.AppointmentID == id)) on favCpt.ID equals ac.CPTMostFavouriteID into Table1
                       from acT in Table1.DefaultIfEmpty()
                       where favCpt.PracticeID == PracticeId && ExtensionMethods.IsNull_Bool(favCpt.Inactive) == false && ( acT!=null?ExtensionMethods.IsNull_Bool(acT.Inactive) == false:true)// acT.AppointmentID == id && (ExtensionMethods.IsNull(acT.Inactive) ? false : ac.Inactive != true)
                       select new
                       {
                           chk = (acT != null && acT.ID != null && acT.CPTMostFavouriteID >= 0) ? true : false,
                           AppointmentCPTID = acT != null ? acT.ID : 0,
                           ID = acT != null ? acT.ID : 0,
                           Type = favCpt.Type,
                           position = giT != null ? giT.position : 100,
                           CPTMostFavouriteID = acT != null ? acT.CPTMostFavouriteID : 0,
                           AppointmentID = acT != null ? acT.AppointmentID : 0,
                           CPTID = acT != null ? acT.CPTID : 0,
                           CPTCode = i != null ? i.CPTCode : "",
                           Units = acT != null ? acT.Units : 0,
                           Amount = acT != null ? acT.Amount : 0,
                           TotalAmount = acT != null ? acT.TotalAmount : 0,
                           Modifier1 = acT != null ? acT.Modifier1 : null,
                           Modifier2 = acT != null ? acT.Modifier2 : null,
                           ChargeID = acT != null ? acT.ChargeID : null,
                           Pointer2 = acT != null ? acT.Pointer2 : null,
                           Pointer3 = acT != null ? acT.Pointer3 : null,
                           Pointer4 = acT != null ? acT.Pointer4 : null,
                           NdcUnits = acT != null ? acT.NdcUnits : null,
                           PracticeID = acT != null ? acT.PracticeID : null,
                           Inactive = acT != null ? acT.Inactive : null,
                           AddedBy = acT != null ? acT.AddedBy : null,
                           AddedDate = acT != null ? acT.AddedDate : null,
                           UpdatedBy = acT != null ? acT.UpdatedBy : null,
                           UpdatedDate = acT != null ? acT.UpdatedDate : null,
                           Description = (i.ShortDescription != null && i.ShortDescription.Length > 0) ? i.ShortDescription : i.Description
                       }
                                       )
                                      .ToList();// .OrderBy(p => p.position).OrderBy(p=>p.Type)

            var icd = (from favICD in _context.ICDMostFavourite
                       join i in _context.ICD on favICD.ICDID equals i.ID
                       join gi in _context.GeneralItems on favICD.Type equals gi.Type into Table2
                       from giT in Table2.DefaultIfEmpty()
                       join ai in ( _context.AppointmentICD.Where(w=>w.AppointmentID==id) )on favICD.ID equals ai.ICDMostFavouriteID into Table1
                       from aiT in Table1.DefaultIfEmpty() 
                       where favICD.PracticeID == PracticeId && ExtensionMethods.IsNull_Bool(favICD.Inactive) == false && (aiT != null ? ExtensionMethods.IsNull_Bool(aiT.Inactive) == false : true)// acT.AppointmentID == id && (ExtensionMethods.IsNull(acT.Inactive) ? false : ac.Inactive != true)
                       select new
                       {
                           chk =( aiT!=null && aiT.ID > 0 ) ? true : false,
                           AppointmentICDID=  aiT != null ? aiT.ID:0,
                           Type = favICD.Type,
                           ID = aiT != null ? aiT.ID : 0,
                           PracticeID = aiT != null ? aiT.PracticeID : 0,
                           Inactive = aiT != null ? aiT.Inactive : false,
                           AddedBy = aiT != null ? aiT.AddedBy : "",
                           AddedDate = aiT != null ? aiT.AddedDate : null,
                           position = (giT != null && giT.ID > 0) ? giT.position : 100,
                           ICDID = (aiT != null && aiT.ID > 0) ? aiT.ICDID : 0,
                           ICDMostFavouriteID = (aiT != null && aiT.ID > 0) ? aiT.ICDMostFavouriteID : 0,
                           i.ICDCode,
                           SerialNo = (aiT != null && aiT.ID > 0) ? aiT.SerialNo : 0,
                           AppointmentID = (aiT != null && aiT.ID > 0) ? aiT.AppointmentID : 0,
                           i.Description,
                           UpdatedBy = (aiT != null && aiT.ID > 0) ? aiT.UpdatedBy : "",
                           UpdatedDate = (aiT != null && aiT.ID > 0) ? aiT.UpdatedDate : null,
                       }
                                       )
                                      .OrderBy(p => p.position).OrderBy(p => p.Type).ToList();


            /*(from favIcd in _context.ICDMostFavourite
                   join i in _context.ICD on favIcd.ICDID equals i.ID
                   join ai in _context.AppointmentICD on favIcd.ID equals  ai.ICDMostFavouriteID??0  into Table1 from aiT in Table1.DefaultIfEmpty()
                   //join gi in _context.GeneralItems on ExtensionMethods.IsNull(favIcd.Type) equals ExtensionMethods.IsNull(gi.Type) into Table2
                   //from giT in Table2.DefaultIfEmpty()
                   where favIcd.PracticeID == UD.PracticeID  //ai.AppointmentID == id && (ai.Inactive != true)
                   select new
                   {
                       //chk = aiT.ID > 0 ? true : false,
                       //Type = aiT.ID > 0 ? favIcd.Type : "",
                       ID = aiT.ID,
                       //position = giT.ID > 0 ? giT.position : 100,
                       ICDID = aiT.ICDID,
                       ICDMostFavouriteID = aiT.ICDMostFavouriteID??0,
                       i.ICDCode,
                       SerialNo = aiT.SerialNo,
                       AppointmentID = aiT.AppointmentID,
                       PracticeID = aiT.PracticeID,
                       Inactive = aiT.Inactive,
                       AddedBy = aiT.AddedBy,
                       AddedDate = aiT.AddedDate,
                       UpdatedBy = aiT.UpdatedBy,
                       UpdatedDate = aiT.UpdatedDate,
                       i.Description
                   }
                                   )
                                  .ToList();*/// .OrderBy(p => p.position).OrderBy(p => p.Type)
            var patientPayment = (from ai in _context.PatientPayment
                                  where ai.patientAppointmentID == id && ExtensionMethods.IsNull_Bool(ai.InActive) == false

                                  select new
                                  {
                                      ai.ID,
                                      ai.PatientID,
                                      ai.patientAppointmentID,
                                      ai.PaymentMethod,
                                      ai.PaymentDate,
                                      ai.PaymentAmount,
                                      ai.CheckNumber,
                                      ai.Description,
                                      ai.Status,
                                      ai.AllocatedAmount,
                                      ai.RemainingAmount,
                                      ai.AddedBy,
                                      ai.InActive,
                                      ai.AddedDate,
                                      ai.UpdatedBy,
                                      ai.UpdatedDate,
                                      ai.Type,
                                      ai.VisitID,
                                      ai.CCTransactionID
                                  }
                                     )
                                    .ToList();

            var ret = new { Appointment = patientAppointment,  CPT = cpt, ICD = icd, patientPayment = patientPayment };
            return Json(ret);
        }

        [HttpPost]
        [Route("FindPatientAppointments")]
        public async Task<ActionResult<IEnumerable<GPatientAppointment>>> FindPatientAppointments(CPatientAppointment CPatientAppointment)
        {
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            return FindPatientAppointments(CPatientAppointment, PracticeId);
        }
        private List<GPatientAppointment> FindPatientAppointments(CPatientAppointment CPatientAppointment, long PracticeId)
        {


            Practice practice = _context.Practice.Find(PracticeId);
            if(practice.isGoogleCalenderEnable!=null && practice.isGoogleCalenderEnable.Value)
                SyncGoogleCalender(CPatientAppointment.FromDate.Value.ToString("MM/dd/yyyy"));
            if (practice.isGoogleSheetEnable!=null && practice.isGoogleSheetEnable.Value)
                SyncGoogleSheet(false);

            List<GeneralItems> generalItems = (from sub in _context.GeneralItems
                                               where sub.Type == "1"
                                               select sub).ToList();
            CommonController common = new CommonController(_context,_contextMain);
            var data = (from pa in _context.PatientAppointment
                        join pat in _context.Patient on pa.PatientID equals pat.ID
                        join pra in _context.Practice on pat.PracticeID equals pra.ID
                        join Pro in _context.Provider on pa.ProviderID equals Pro.ID
                        join Loc in _context.Location on pa.LocationID equals Loc.ID
                        join gi in _context.GeneralItems on  pa.Status   equals gi.Value into giTable
                        from giT in giTable.DefaultIfEmpty()
                        join pPlan in _context.PatientPlan on pa.PrimarypatientPlanID equals pPlan.ID into gjpPlan
                        from pPlanT in gjpPlan.DefaultIfEmpty()
                        join iPlan in _context.InsurancePlan on pPlanT.InsurancePlanID equals iPlan.ID into gjiPlan
                        from tiPlan in gjiPlan.DefaultIfEmpty()
                        join vr in _context.VisitReason on pa.VisitReasonID equals vr.ID into Table1
                        from tVR in Table1.DefaultIfEmpty()
                        join pn in _context.PatientNotes on pa.ID equals pn.AppointmentID into Table2
                        from pnT in Table1.DefaultIfEmpty()


                      



                            //join gi in _context.GeneralItems on TranslateStatus( pa.Status)       equals gi.Value into gj2 from tStatus in gj2.DefaultIfEmpty()

                        where
                            pat.PracticeID==PracticeId && 
                            ((CPatientAppointment.ProviderID == null || CPatientAppointment.ProviderID.Length==0) ? true : CPatientAppointment.ProviderID.Contains(pa.ProviderID.Value)) &&
                            ((CPatientAppointment.LocationID == null || CPatientAppointment.LocationID.Length == 0) ? true : CPatientAppointment.LocationID.Contains(pa.LocationID.Value)) && 
                           (CPatientAppointment.VisitReasonID.IsNull() ? true : pa.VisitReasonID.Equals(CPatientAppointment.VisitReasonID)) &&
                           (CPatientAppointment.Status.IsNull() ? true : pa.Status.Equals(CPatientAppointment.Status)) &&
                           ((CPatientAppointment.firstName.IsNull() || CPatientAppointment.firstName.Length==0) ? true : pat.FirstName.ToLower().Contains(CPatientAppointment.firstName.ToLower())) &&
                           ((CPatientAppointment.lastName.IsNull() || CPatientAppointment.lastName.Length == 0) ? true : pat.LastName.ToLower().Contains(CPatientAppointment.lastName.ToLower())) &&
                           ((CPatientAppointment.accountNum.IsNull() || CPatientAppointment.accountNum.Length == 0) ? true : pat.AccountNum.Equals(CPatientAppointment.accountNum)) &&
                           ((CPatientAppointment.DOB != null || CPatientAppointment.DOB.ToString().Length == 0) ? true: (pat.DOB.Value.Date.Format("MM/dd/yyyy") == CPatientAppointment.DOB.Format("MM/dd/yyyy"))  ) &&
                           // (pa.AppointmentDate.Value.Date.Format("MM/dd/yyyy") == CPatientAppointment.FromDate.Format("MM/dd/yyyy"))
                           (CPatientAppointment.FromDate != null && CPatientAppointment.ToDate != null ?
                         ((DateTime)pa.AppointmentDate).Date >= CPatientAppointment.FromDate && ((DateTime)pa.AppointmentDate).Date <= CPatientAppointment.ToDate
                        : (CPatientAppointment.FromDate != null ? ( pa.AppointmentDate.Value.Date.Format("MM/dd/yyyy") == CPatientAppointment.FromDate.Format("MM/dd/yyyy")) : false)) 
                       && (ExtensionMethods.IsNull(pa.Inactive)? true: pa.Inactive != true ) // && pat.IsActive== true && tpPlan.IsActive == true && pa.Inactive != true
                        //Neet To apply ToTime and From Time Check
                        select new GPatientAppointment()
                        {
                            id = pa.ID,
                            PatientID = pa.PatientID,
                            PracticeID = pat.PracticeID,
                            ClientID = pra.ClientID,
                            AppointmentDate = pa.AppointmentDate.Format("MM/dd/yyyy"),
                            start = pa.Time,
                            Patient = pat.LastName.Trim() + ", " + pat.FirstName.Trim(),
                            PatientDOB = pat.DOB!=null?pat.DOB.Value.ToString("MM/dd/yyyy"):"",
                            //text = pat.LastName + ", " + pat.FirstName,
                            AccountNum = pat.AccountNum.Trim(),
                            StatusColor = giT!=null ?  giT.Description:"",
                            PhoneNumber = pat.PhoneNumber,
                            resource = Pro.Name.Trim(),// + ", " + Pro.FirstName,
                            Plan = tiPlan.PlanName, // Need To Discuss  
                            InsurancePlanID = pPlanT.InsurancePlanID>0?pPlanT.InsurancePlanID.ToString():"" , // Need To Discuss  
                            LocationID = Loc.ID,
                            Location = Loc.Name,
                            Provider = pa.ProviderID!=null? (Pro.Name.Trim()) :"",
                            ProviderID = pa.ProviderID??0,
                            Status = pa.Status,
                            StatusDescription = giT != null ? giT.Name : "",
                            Inactive = pa.Inactive??false,
                            VisitReason = tVR.Name,
                            TimeInterval= pa.VisitInterval.Value,
                            end = pa.Time.Value.AddMinutes(pa.VisitInterval.Value)
                        });
            List<GPatientAppointment> lst = data.OrderByDescending(o=>o.start).ToList();
            return lst;
        }

        [HttpPost]
        [Route("Export")]
        public async Task<IActionResult> Export(CPatientAppointment CPatientAppointment)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GPatientAppointment> data = FindPatientAppointments(CPatientAppointment, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportExcel(data.Cast<Object>().ToList(), UD, CPatientAppointment, "Patient Appointment Report");
        }

        [HttpPost]
        [Route("ExportPdf")]
        public async Task<IActionResult> ExportPdf(CPatientAppointment CPatientAppointment)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
             User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
             User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
             long PracticeId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type.Equals("RandomKEY", StringComparison.InvariantCultureIgnoreCase)).Value);
            List<GPatientAppointment> data = FindPatientAppointments(CPatientAppointment, PracticeId);
            ExportController controller = new ExportController(_context);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, data.Count);
            return await controller.ExportPdf(data.Cast<Object>().ToList(), UD);
        }


        //public long TranslateStatus(string Status)
        //{
        //    return long.Parse(Status);
            /*switch (Status)
            {
                desc = "Reschedule";
        }
            if (Status == "S")
            {
                desc = "Schedule";
            }
            return desc;
         }*/
        [Route("RescheduleAppointment")]
        [HttpPost]
        public async Task<ActionResult<PatientAppointment>> RescheduleAppointment(PatientAppointment item)
        {
            //UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
            //User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
            //User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            // if (UD == null || UD.Rights == null || UD.Rights.ProviderCreate == false)
            // return BadRequest("You Don't Have Rights. Please Contact BellMedex.");     //Need To apply CancellAppointmentRights 

            try
            {
                bool succ = TryValidateModel(item);
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
                        //PatientAppointment appointment = _context.PatientAppointment.Where(v => v.ID == item.ID && v.Status == "S").SingleOrDefault();
                        //ProviderSlot slot = _context.ProviderSlot.Where(v => v.ID == appointment.ProviderSlotID && v.Status == "S").SingleOrDefault();

                        //if (appointment.AppointmentDate.Date() < DateTime.Now.Date())
                        //{
                        //    return BadRequest("Appointment date is crossed");

                        //}
                        //if(appointment.Time < DateTime.Now.ToString("T"))
                        //{

                        //}
                        //if (appointment != null)
                        //{
                        //    _context.PatientAppointment.Remove(appointment);
                        //    //slot.Status = "A";
                            //item.Time = slot.FromTime;
                        //    //_context.ProviderSlot.Update(slot);
                        //    await _context.SaveChangesAsync();


                        //if (item.ID <= 0)
                        //{
                        //    //ProviderSlot NewSlot = _context.ProviderSlot.Where(v => v.ID == item.ProviderSlotID).SingleOrDefault();
                        //    item.AddedBy = UD.Email;
                        //    item.AddedDate = DateTime.Now;
                        //    item.Status = "S";
                        //    //item.Time = NewSlot.FromTime;
                        //    _context.PatientAppointment.Add(item);
                        //    //NewSlot.Status = "S";
                        //    //_context.ProviderSlot.Update(slot);
                            // await _context.SaveChangesAsync();
                        //}
                        //await _context.SaveChangesAsync();
                        //objTrnScope.Complete();
                        //objTrnScope.Dispose();
                    }
                    catch (Exception ex)
                    {
                        System.IO.File.WriteAllText(Path.Combine(_context.env.ContentRootPath, "Logs", "Visit.txt"), ex.ToString());
                        throw ex;

                    }
                    finally
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            
            return Ok(item);


        }

        [Route("CancellAppointment")]
        [HttpPost]
        public async Task<ActionResult<PatientAppointment>> CancellAppointment(PatientAppointment item)
        {
           
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            // if (UD == null || UD.Rights == null || UD.Rights.ProviderCreate == false)
            // return BadRequest("You Don't Have Rights. Please Contact BellMedex.");     //Need To apply CancellAppointmentRights 
            bool succ = TryValidateModel(item);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            PatientAppointment appointment = _context.PatientAppointment.Where(v => v.ID == item.ID).SingleOrDefault();
            //ProviderSlot slot = _context.ProviderSlot.Where(v => v.ID == appointment.ProviderSlotID && v.Status == "S").SingleOrDefault();


            if (appointment != null  )
            {
                // _context.PatientAppointment.Remove(appointment);
                //slot.UpdatedBy = UD.Email;
                //slot.UpdatedDate = DateTime.Now;
                //slot.Status = "A";
                appointment.UpdatedBy = UD.Email;
                appointment.UpdatedDate = DateTime.Now;
                appointment.Status = "8008"; // set cancel status value.
                //_context.ProviderSlot.Update(slot);
                _context.PatientAppointment.Update(appointment);
                await _context.SaveChangesAsync();
            }
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);
            return Ok(item);
        }
        public class PropertyCopier<TParent, TChild> where TParent : class
                                     where TChild : class
        {
            public static void Copy(TParent parent, TChild child)
            {
                var parentProperties = parent.GetType().GetProperties();
                var childProperties = child.GetType().GetProperties();

                foreach (var parentProperty in parentProperties)
                {
                    foreach (var childProperty in childProperties)
                    {
                        if (parentProperty.Name == childProperty.Name && parentProperty.PropertyType == childProperty.PropertyType)
                        {
                            childProperty.SetValue(child, parentProperty.GetValue(parent));
                            break;
                        }
                    }
                }
            }
        }

        [Route("SavePatientAppointment")]
        [HttpPost]
        public async Task<ActionResult<PatientAppointment>> SavePatientAppointment(PatientAppointment item)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
           User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
           User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            bool flag = true;
            //  string Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            bool succ = TryValidateModel(item);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            if (ExtensionMethods.IsNull(item.PatientID))
            {
                return Json("Please select Patient");
            }
            if (ExtensionMethods.IsNull(item.LocationID))
            {
                return Json("Please select Locatoin");
            }
            if (ExtensionMethods.IsNull(item.ProviderID))
            {
                return Json("Please select Provider");
            }
            if (ExtensionMethods.IsNull(item.AppointmentDate))
            {
                return Json("Please select Appointment Date");
            }
            if (ExtensionMethods.IsNull(item.Time))
            {
                return Json("Please select Appointment Time");
            }
            if (ExtensionMethods.IsNull(item.VisitInterval) || item.VisitInterval < 5)
            {
                return Json("Please select Duration . Duration can't be less then 5 mints.");
            }
            if (item.ID <= 0)
            {
                var app = _context.PatientAppointment.Where(p => p.PatientID == item.PatientID
                           && p.AppointmentDate == item.AppointmentDate
                           && ExtensionMethods.IsNull_Bool(p.Inactive) == false).FirstOrDefault();
                if (app != null && app.ID > 0)
                {
                    return Json("Appointment for patient already exist.");
                }
            }

            if (item.priorAuthorization == true)
            {

                PatientPlan Plan = _context.PatientPlan.Find(item.PrimarypatientPlanID);
                Plan.AuthRequired = true;
                _context.PatientPlan.Update(Plan);

                var _Authorization = _context.PatientAuthorization.Where(w => w.PatientID == item.PatientID && w.PatientPlanID == item.PrimarypatientPlanID).FirstOrDefault();
                PatientAuthorization Authoriz = new PatientAuthorization();
                if (_Authorization.PatientPlanID != null && Plan.InsurancePlanID != null)
                {
                    Authoriz.PatientPlanID = Plan.ID;
                    Authoriz.PatientID = item.PatientID;
                    Authoriz.AddedBy = UD.Email;
                    Authoriz.AddedDate = DateTime.Now;
                    _context.PatientAuthorization.Update(Authoriz);
                }
                if (_Authorization.PatientPlanID == null && Plan.InsurancePlanID != null)
                {
                    Authoriz.PatientPlanID = Plan.ID;
                    Authoriz.PatientID = item.PatientID;
                    Authoriz.AddedBy = UD.Email;
                    Authoriz.AddedDate = DateTime.Now;
                    _context.PatientAuthorization.Add(Authoriz);
                }

                _context.SaveChanges();
            }


            if (item.priorAuthorization == null || item.priorAuthorization == false && item.PrimarypatientPlanID!=null)
             {

                PatientPlan Plan = _context.PatientPlan.Find(item.PrimarypatientPlanID);
                if(Plan!=null)
                { 
                Plan.AuthRequired = false;
                _context.PatientPlan.Update(Plan);}

                var _Authorization = _context.PatientAuthorization.Where(w => w.PatientID == item.PatientID && w.PatientPlanID == item.PrimarypatientPlanID).FirstOrDefault();
                if (_Authorization != null)
                {
                    var patientAuth = _context.PatientAuthorization.Find(_Authorization.PatientPlanID);
                    _context.PatientAuthorization.Remove(patientAuth);
                }
                _context.SaveChangesAsync();
            }




            //ProviderSlot slot = _context.ProviderSlot.Where(v => v.ID == item.ProviderSlotID && v.Status == "A").SingleOrDefault();
            //if(slot == null)
            //{
            //   return BadRequest("Slot can not be empty");

            //}
            if (item.ID <= 0)
            {
                item.AddedBy = UD.Email;
                item.AddedDate = DateTime.Now;
                if (item.Status == null || item.Status.Equals(0))
                    item.Status = "8000";
                //item.Time = slot.FromTime;
                //slot.Status = "S";  // Changing Slot Status to "S" from "A" After Scheduling A Slot
                //_context.ProviderSlot.Update(slot);
                _context.PatientAppointment.Add(item);
                AutoJobsController autoJobs = new AutoJobsController(_context, _contextMain, _config);
                Patient patient = _context.Patient.Find(item.PatientID);
                Provider _ProviderInfo = _context.Provider.Find(item.ProviderID);
                Practice _Practice = _context.Practice.Find(UD.PracticeID);               

                if (patient != null && !ExtensionMethods.IsNull(patient.Email) && _ProviderInfo != null && _Practice.IsEmailAppointmentReminder == true)
                {                   
                    
                    autoJobs.SendEmail(_config, patient.Email, patient.LastName + ", " + patient.FirstName, item.AppointmentTime, item.VisitInterval.ToString(), _ProviderInfo.FirstName + "" + _ProviderInfo.LastName, UD.ClientID.ToString());
                }
            }
            else
            {
                item.UpdatedBy = UD.Email;
                item.UpdatedDate = DateTime.Now;
                //item.Status = "R";
                //item.Time = slot.FromTime;
                //slot.Status = "R";  // Changing Slot Status to "R" from "S" After ReScheduling A Slot
                //_context.ProviderSlot.Update(slot);
                _context.PatientAppointment.Update(item);
                _context.SaveChanges();

                // List<PatientAppointment> checkRecuringNumber 
                List<PatientAppointment> checkRecuringNumber = (from pa in _context.PatientAppointment
                                                                where pa.PatientID == item.PatientID && pa.recurringfrequency == item.recurringfrequency && pa.AppointmentDate > item.AppointmentDate && ExtensionMethods.IsNull_Bool(pa.Inactive) == false
                                                                select new PatientAppointment()
                                                                {

                                                                    ID = pa.ID,
                                                                    PatientID = pa.PatientID,
                                                                    LocationID = pa.LocationID,
                                                                    ProviderID = pa.ProviderID,
                                                                    PrimarypatientPlanID = pa.PrimarypatientPlanID,
                                                                    VisitReasonID = pa.VisitReasonID,
                                                                    AppointmentDate = pa.AppointmentDate,
                                                                    Time = pa.Time,
                                                                    VisitInterval = pa.VisitInterval,
                                                                    Status = pa.Status,
                                                                    Notes = pa.Notes,
                                                                    AddedBy = pa.AddedBy,
                                                                    AddedDate = pa.AddedDate,
                                                                    UpdatedBy = pa.UpdatedBy,
                                                                    UpdatedDate = pa.UpdatedDate,
                                                                    Inactive = pa.Inactive,
                                                                    RoomID = pa.RoomID,
                                                                    color = pa.color,
                                                                    recurringAppointment = pa.recurringAppointment,
                                                                    recurringNumber = pa.recurringNumber,
                                                                    recurringfrequency = pa.recurringfrequency,
                                                                    priorAuthorization = pa.priorAuthorization,
                                                                    parentAppointmentID = pa.parentAppointmentID

                                                                }).ToList(); 
                if (item.recurringNumber == checkRecuringNumber.Count)
                { flag = false; }
                if(checkRecuringNumber.Count==0)
                {
                    flag = true;
                }
                if (checkRecuringNumber.Count < item.recurringNumber && checkRecuringNumber.Count!=0)
                {

                    var checkdate = checkRecuringNumber[checkRecuringNumber.Count - 1];
                    item.AppointmentDate = checkdate.AppointmentDate;
                    int differnce = item.recurringNumber.Value - checkRecuringNumber.Count;
                    item.recurringNumber = differnce;
                    flag = true;


                }
                if (checkRecuringNumber.Count > item.recurringNumber && checkRecuringNumber.Count != 0)
                {
                    int recuurringvalue = item.recurringNumber.Value;
                      foreach (var checkitem in checkRecuringNumber)
                    {
                        if(recuurringvalue>0)
                        {
                            recuurringvalue--;

                            checkitem.recurringNumber = recuurringvalue;
                            item.UpdatedBy = UD.Email;
                            item.UpdatedDate = DateTime.Now;




                        }
                        else { checkitem.Inactive = true;
                            item.UpdatedBy = UD.Email;
                            item.UpdatedDate = DateTime.Now;


                        }

                        _context.PatientAppointment.Update(checkitem);
                        flag=false;
                    }

                    
                }




            }
            



            if (item.recurringAppointment != null && item.recurringAppointment.Value && flag==true)
            {

                if (item.recurringfrequency.ToLower() == "daily")
                {
                    for (int i = 0; i <= item.recurringNumber; i++)
                    {
                        PatientAppointment recuringpatientAppointment = new PatientAppointment();
                        PropertyCopier<PatientAppointment, PatientAppointment>.Copy(item, recuringpatientAppointment);

                        recuringpatientAppointment.ID = 0;
                        recuringpatientAppointment.recurringNumber = item.recurringNumber - i;
                        recuringpatientAppointment.AddedBy = UD.Email;
                        recuringpatientAppointment.AddedDate = DateTime.Now;
                        DateTime appDate = item.AppointmentDate.Value;
                        recuringpatientAppointment.AppointmentDate = appDate.AddDays(i + 1);
                        DateTime appTime = item.Time.Value;
                        recuringpatientAppointment.Time = appTime.AddDays(i + 1);
                        recuringpatientAppointment.parentAppointmentID = item.ID;

                        if (recuringpatientAppointment.Status == null || item.Status.Equals(0))
                            item.Status = "8000";
                        //_context.Entry(recuringpatientAppointment).State = EntityState.Added;
                        _context.PatientAppointment.Add(recuringpatientAppointment);

                    }
                    _context.SaveChanges();
                }
                else if (item.recurringfrequency.ToLower() == "weekly")
                {
                    for (int i = 1; i <= item.recurringNumber; i++)
                    {
                        PatientAppointment recuringpatientAppointment = new PatientAppointment();
                        PropertyCopier<PatientAppointment, PatientAppointment>.Copy(item, recuringpatientAppointment);

                        recuringpatientAppointment.ID = 0;
                        recuringpatientAppointment.recurringNumber = recuringpatientAppointment.recurringNumber - i;
                        recuringpatientAppointment.AddedBy = UD.Email;
                        recuringpatientAppointment.AddedDate = DateTime.Now;
                        DateTime appDate = item.AppointmentDate.Value;
                        recuringpatientAppointment.AppointmentDate = appDate.AddDays(7*i);
                        DateTime appTime = item.Time.Value;
                        recuringpatientAppointment.Time = appTime.AddDays(7 * i);
                        recuringpatientAppointment.parentAppointmentID = item.ID;

                        if (recuringpatientAppointment.Status == null || item.Status.Equals(0))
                            item.Status = "8000";
                        //_context.Entry(recuringpatientAppointment).State = EntityState.Added;
                        _context.PatientAppointment.Add(recuringpatientAppointment);

                    }
                }

                else if (item.recurringfrequency.ToLower() == "biweekly")
                {
                    for (int i = 1; i <= item.recurringNumber; i++)
                    {
                        PatientAppointment recuringpatientAppointment = new PatientAppointment();
                        PropertyCopier<PatientAppointment, PatientAppointment>.Copy(item, recuringpatientAppointment);

                        recuringpatientAppointment.ID = 0;
                        recuringpatientAppointment.recurringNumber = recuringpatientAppointment.recurringNumber - i;
                        recuringpatientAppointment.AddedBy = UD.Email;
                        recuringpatientAppointment.AddedDate = DateTime.Now;
                        DateTime appDate = item.AppointmentDate.Value;
                        recuringpatientAppointment.AppointmentDate = appDate.AddDays(14*i);
                        recuringpatientAppointment.parentAppointmentID = item.ID;
                        DateTime appTime = item.Time.Value;
                        recuringpatientAppointment.Time = appTime.AddDays(14 * i);

                        if (recuringpatientAppointment.Status == null || item.Status.Equals(0))
                            item.Status = "8000";
                        //_context.Entry(recuringpatientAppointment).State = EntityState.Added;
                        _context.PatientAppointment.Add(recuringpatientAppointment);

                    }
                }

                else if (item.recurringfrequency.ToLower() == "monthly")
                {
                    for (int i = 1; i <= item.recurringNumber; i++)
                    {
                        PatientAppointment recuringpatientAppointment = new PatientAppointment();
                        PropertyCopier<PatientAppointment, PatientAppointment>.Copy(item, recuringpatientAppointment);

                        recuringpatientAppointment.ID = 0;
                        recuringpatientAppointment.recurringNumber = recuringpatientAppointment.recurringNumber - i;
                        recuringpatientAppointment.AddedBy = UD.Email;
                        recuringpatientAppointment.AddedDate = DateTime.Now;
                        DateTime appDate = item.AppointmentDate.Value;
                        recuringpatientAppointment.AppointmentDate = appDate. AddMonths(i);
                        DateTime appTime = item.Time.Value;
                        recuringpatientAppointment.Time = appTime.AddMonths( i);
                        recuringpatientAppointment.parentAppointmentID = item.ID;

                        if (recuringpatientAppointment.Status == null || item.Status.Equals(0))
                            item.Status = "8000";
                        //_context.Entry(recuringpatientAppointment).State = EntityState.Added;
                        _context.PatientAppointment.Add(recuringpatientAppointment);

                    }
                }



            }
            _context.SaveChanges();
            if (item.forms != null && item.forms.Count > 0)
                foreach (PatientForms patientForm in item.forms)
                {
                    PatientForms form = (PatientForms)patientForm;
                    form.UpdatedBy = UD.Email;
                    form.UpdatedDate = DateTime.Now;
                    if (form.ID <= 0)
                    {
                        form.AddedBy = UD.Email;
                        form.AddedDate = DateTime.Now;
                        form.PatientID = item.PatientID;
                        form.PatientAppointmentID = item.ID;
                        _context.PatientForms.Add(form);
                    }
                    else
                    {
                        _context.PatientForms.Update(form);
                    }
                }
            /*   if (!ExtensionMethods.IsNull(item.VisitID)) // visit ID found, saving data against charges..
               {
               // casting and saving to Charges.
               if (item.charges != null && item.charges.Count > 0)
                   foreach (Charge _charge in item.charges)
                   {
                       //Charge cpt =  c;
                       if (_charge.CPTID <= 0)
                           continue;
                       if (_charge.ID <= 0)
                       {
                           _charge.AddedBy = UD.Email;
                           _charge.AddedDate = DateTime.Now;
                           _charge.PracticeID = UD.PracticeID;
                           _charge.VisitID = item.VisitID;
                           _context.Charge.Add(_charge);
                       }
                       else
                       {
                           _charge.UpdatedBy = UD.Email;
                           _charge.UpdatedDate = DateTime.Now;
                           _context.Charge.Update(_charge);
                       }
                   }
               }
               else*/  // only saving data against appointment.
            {
                if (item.CPTs != null && item.CPTs.Count > 0)
                    foreach (AppointmentCPT cpt in item.CPTs)
                {
                    cpt.UpdatedBy = UD.Email;
                    cpt.UpdatedDate = DateTime.Now;
                    if (cpt.ID <= 0)
                    {
                        cpt.AddedBy = UD.Email;
                        cpt.AddedDate = DateTime.Now;
                        cpt.PracticeID = UD.PracticeID;
                        cpt.AppointmentID = item.ID;
                        _context.AppointmentCPT.Add(cpt);
                    }
                    else
                    {
                        _context.AppointmentCPT.Update(cpt);
                    }
                }
                if (item.ICDs != null && item.ICDs.Count > 0)
                    foreach (AppointmentICD icd in item.ICDs)
                    {
                        icd.UpdatedBy = UD.Email;
                        icd.UpdatedDate = DateTime.Now;
                        if (icd.ID <= 0)
                        {
                            icd.AddedBy = UD.Email;
                            icd.AddedDate = DateTime.Now;
                            icd.PracticeID = UD.PracticeID;
                            icd.AppointmentID = item.ID;
                            _context.AppointmentICD.Add(icd);
                        }
                        else
                        {
                            _context.AppointmentICD.Update(icd);
                        }
                    }
                if (item.patientPayment != null && item.patientPayment.Count > 0)
                    foreach (PatientPayment pp in item.patientPayment)
                    {
                        pp.UpdatedBy = UD.Email;
                        pp.UpdatedDate = DateTime.Now;
                        if (pp.ID <= 0)
                        {
                            pp.AddedBy = UD.Email;
                            pp.AddedDate = DateTime.Now;
                            pp.PaymentDate =item.Time;
                            pp.PaymentMethod = "Cash"; 
                            pp.patientAppointmentID = item.ID;
                            pp.PatientID = item.PatientID;
                            _context.PatientPayment.Add(pp);
                        }
                        else
                        {
                            _context.PatientPayment.Update(pp);
                        }
                    }
            }
            _context.SaveChanges();
            if(item.Status=="8003")
                await CreateVisit(item, UD, item.Status);
            Log.WriteQueryTime(_context.env.ContentRootPath, UD.PracticeName, UD.Email, this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), _startTime, 1);
            return Ok(item);
        }
        [Route("UpdateAppointmentStatus/{AppointmentId}/{status}")]
        [HttpGet("{AppointmentId}/{status}")]
        public async Task<ActionResult> UpdateAppointmentStatusAsync(long AppointmentId, string status)
        {
            //  string Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
          User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
          User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            PatientAppointment patientAppointment = _context.PatientAppointment.Find(AppointmentId);

            if (patientAppointment == null)
            {
                return Json("Appointment not found.");
            }
            patientAppointment.Status = status;
            _context.PatientAppointment.Update(patientAppointment);
            await _context.SaveChangesAsync();
            await   CreateVisit(patientAppointment, UD, status);
            return Ok(patientAppointment);
        }

        private async Task<ActionResult>  CreateVisit(PatientAppointment patientAppointment, UserInfoData UD, string status)
        {

            patientAppointment.UpdatedBy = UD.Email;
            patientAppointment.UpdatedDate = DateTime.Now;
            if ( status == "8002") //|| patientAppointment.Status == "8001" && status == "8002"
            { // create new visit.
              //            8000
              //Confirmed   8001
              //CheckIN     8002
                Visit visit = null;
               
                    visit = _context.Visit.Where(f => f.PatientAppointmentID == patientAppointment.ID).FirstOrDefault();

                if (visit == null)
                {
                    visit = new Visit();
                    visit.PatientAppointmentID = patientAppointment.ID;
                    visit.DateOfServiceFrom = patientAppointment.AppointmentDate;
                    visit.DateOfServiceTo = patientAppointment.AppointmentDate;
                    List<Notes> notes = new List<Notes>();
                    Notes note = new Notes();
                    note.Note = "Auto Claim Created from Appointment";
                    notes.Add(note );
                    visit.Note = notes;
                }
                //visit.DateOfServiceFrom = "";
                visit.PrimaryPatientPlanID = patientAppointment.PrimarypatientPlanID;

                visit.ClientID = UD.ClientID;
                visit.PracticeID = UD.PracticeID;
                visit.LocationID = patientAppointment.LocationID.Value;
                Location loc = _context.Location.Find(patientAppointment.LocationID.Value);
                if(loc!=null)
                    visit.POSID = loc.POSID;
                visit.ProviderID = patientAppointment.ProviderID.Value;
                //visit.POSID = patientAppointment.POSID;
                //visit.RefProviderID = patientAppointment.ProviderID;
                visit.SupervisingProvID = patientAppointment.ProviderID;
                visit.OrderingProvID = patientAppointment.ProviderID;
                visit.PatientID = patientAppointment.PatientID;
                visit.PrimaryPatientPlanID = patientAppointment.PrimarypatientPlanID;
                //visit.SecondaryPatientPlanID = patientAppointment.SecondaryPatientPlanID;
                //visit.TertiaryPatientPlanID = patientAppointment.TertiaryPatientPlanID;
                List<AppointmentICD> lstAppointmentICD = _context.AppointmentICD.Where(w => w.AppointmentID == patientAppointment.ID).ToList();
                if (lstAppointmentICD != null && lstAppointmentICD.Count > 0)
                {
                    if (lstAppointmentICD.Count > 0)
                        visit.ICD1ID = lstAppointmentICD[0].ICDID;
                    //else
                    //    visit.ICD1ID = null;
                    if (lstAppointmentICD.Count > 1)
                        visit.ICD2ID = lstAppointmentICD[1].ICDID;
                    else
                        visit.ICD2ID = (long?)null;
                    if (lstAppointmentICD.Count > 2)
                        visit.ICD3ID = lstAppointmentICD[2].ICDID;
                    else
                        visit.ICD3ID = (long?)null;
                    if (lstAppointmentICD.Count > 3)
                        visit.ICD4ID = lstAppointmentICD[3].ICDID;
                    else
                        visit.ICD4ID = (long?)null;
                    if (lstAppointmentICD.Count > 4)
                        visit.ICD5ID = lstAppointmentICD[4].ICDID;
                    else
                        visit.ICD5ID = (long?)null;
                    if (lstAppointmentICD.Count > 5)
                        visit.ICD6ID = lstAppointmentICD[5].ICDID;
                    else
                        visit.ICD6ID = (long?)null;
                    if (lstAppointmentICD.Count > 6)
                        visit.ICD7ID = lstAppointmentICD[6].ICDID;
                    else
                        visit.ICD7ID = (long?)null;
                    if (lstAppointmentICD.Count > 7)
                        visit.ICD8ID = lstAppointmentICD[7].ICDID;
                    else
                        visit.ICD8ID = (long?)null;
                    if (lstAppointmentICD.Count > 8)
                        visit.ICD9ID = lstAppointmentICD[8].ICDID;
                    else
                        visit.ICD9ID = (long?)null;
                    if (lstAppointmentICD.Count > 9)
                        visit.ICD10ID = lstAppointmentICD[9].ICDID;
                    else
                        visit.ICD10ID = (long?)null;
                    if (lstAppointmentICD.Count > 10)
                        visit.ICD11ID = lstAppointmentICD[10].ICDID;
                    else
                        visit.ICD11ID = (long?)null;
                    if (lstAppointmentICD.Count > 11)
                        visit.ICD12ID = lstAppointmentICD[11].ICDID;
                    else
                        visit.ICD12ID = (long?)null;
                }
                Charge charge = null;
                List<AppointmentCPT> lstAppointmentCPT = _context.AppointmentCPT.Where(w => w.AppointmentID == patientAppointment.ID).ToList();
                if (lstAppointmentCPT != null && lstAppointmentCPT.Count > 0)
                {
                    visit.Charges = new List<Charge>();
                    foreach (AppointmentCPT item in lstAppointmentCPT)
                    { 
                        charge = _context.Charge.Where(w => w.AppointmentCPTID == item.ID).FirstOrDefault(); 
                        if (charge == null)
                        {
                            charge = new Charge();
                            charge.PatientID = visit.PatientID;
                            charge.VisitID = visit.ID;
                            charge.AppointmentCPTID = item.ID;
                            charge.DateOfServiceFrom = patientAppointment.AppointmentDate.Value;
                            charge.DateOfServiceTo = patientAppointment.AppointmentDate.Value;
                        }
                        charge.CPTID = item.CPTID;
                        charge.Pointer1 = item.Pointer1;
                        charge.Pointer2 = item.Pointer2;
                        charge.Pointer3 = item.Pointer3;
                        charge.Pointer4 = item.Pointer4;
                        charge.PrimaryBilledAmount = item.Amount; 
                        charge.Units = item.Units.ToString();
                        charge.TotalAmount = item.TotalAmount.Value;
                        charge.ClientID = visit.ClientID;
                        charge.PracticeID = visit.PracticeID;
                        charge.LocationID = visit.LocationID;
                        charge.ProviderID = visit.ProviderID;
                        charge.POSID = visit.POSID;
                        charge.RefProviderID = visit.RefProviderID;
                        charge.SupervisingProvID = visit.SupervisingProvID;
                        charge.OrderingProvID = visit.OrderingProvID;
                        charge.PrimaryPatientPlanID = visit.PrimaryPatientPlanID;
                        charge.SecondaryPatientPlanID = visit.SecondaryPatientPlanID;
                        charge.TertiaryPatientPlanID = visit.TertiaryPatientPlanID;
                        //charge.PrimaryBal = charge.PrimaryBilledAmount;
                        charge.AddedBy = UD.Email;
                        charge.AddedDate = DateTime.Now;
                        if (!item.Modifier1.IsNull())
                        {
                            Modifier mod = _context.Modifier.Where(w => w.Code == item.Modifier1).FirstOrDefault();
                            long?  modifier = mod!=null?mod.ID.Value:0;
                            charge.Modifier1ID = modifier>0?modifier:null;
                        }
                        if (!item.Modifier2.IsNull())
                        {
                            Modifier mod = _context.Modifier.Where(w => w.Code == item.Modifier2).FirstOrDefault();
                            long? modifier = mod != null ? mod.ID.Value : 0;
                            charge.Modifier2ID = modifier > 0 ? modifier : null; 
                        }

                        /* if (item.Modifier1 != null && item.Modifier1.Length > 0)
                         {
                             string[] modifier = item.Modifier1.Split(';');
                             for (int i = 0; i < modifier.Length; i++)
                             {
                                 if (i == 0)
                                 {
                                     string[] arr = modifier[0].Split(':');
                                     if (arr.Length > 1)
                                     {
                                         charge.Modifier1Amount = Decimal.Parse(arr[1]);
                                     }
                                     Modifier mod = _context.Modifier.Where(w => w.Code == arr[0]).FirstOrDefault();
                                     charge.Modifier1ID = mod.ID;
                                 }
                             }
                         }*/

                        visit.Charges.Add(charge);
                        visit.PrimaryBilledAmount = visit.Charges.Sum(s => s.PrimaryBilledAmount);
                    }
                }


                /* VisitController visitController = new VisitController(_context, _contextMain);
                 visitController.SaveVisit(visit);*/
                List<PatientPayment> _patientPayments = _context.PatientPayment.Where(w => w.patientAppointmentID == visit.PatientAppointmentID).ToList();
                visit.PatientPayments = _patientPayments;
                VisitController VisitController = new VisitController(_context, _contextMain);
                VisitController.ControllerContext = this.ControllerContext;
                await VisitController.SaveVisit(visit);
                

                PatientNotes _patientNotes = null;

                _patientNotes = _context.PatientNotes.Where(f => f.AppointmentID == patientAppointment.ID).FirstOrDefault();

                if (_patientNotes == null)
                {
                    _patientNotes = new PatientNotes();
                    _patientNotes.AppointmentID = patientAppointment.ID;
                } 
                _patientNotes.ProviderID = patientAppointment.ProviderID.Value;
                _patientNotes.PatientID = patientAppointment.PatientID;
                _patientNotes.DOS = patientAppointment.AppointmentDate.Value;
                _patientNotes.ProviderID = patientAppointment.ProviderID.Value;
                _patientNotes.LocationID = patientAppointment.LocationID.Value;
                
                _patientNotes.PracticeID = UD.PracticeID;
                PatientNotesController _patientNotesContr = new PatientNotesController(_context,_contextMain);
                _patientNotesContr.ControllerContext = this.ControllerContext;
                await _patientNotesContr.SavePatientNotes(_patientNotes);
                 
            }
            return Ok(patientAppointment);
        }
        [HttpGet]
        [Route("GetAppointmentCPT")]
        public IActionResult GetAppointmentCPT(long AppointmentID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var AppointmentCPTs = _context.AppointmentCPT
                                      .Where(s => s.AppointmentID == AppointmentID)
                                      .ToList();
            //var clinicalFormCPT = await _context.ClinicalFormCPT.FindAsync(ClinicalFormID);

            if (AppointmentCPTs == null)
            {
                return NotFound();
            }
           
            return Ok(AppointmentCPTs);
        }

        /* private IQueryable<T> GetAppointmentForms(long AppointmentID)
         {
             if (!ModelState.IsValid)
             {
                 return BadRequest(ModelState);
             }

             var forms = (from pf in _context.PatientForms
                                    join cf in _context.ClinicalForms on pf.ClinicalFormID equals cf.ID
                                    where pf.PatientAppointmentID == AppointmentID && pf.Inactive != true
                                    select new
                                    {
                                        cf.Name,
                                        pf.ID,
                                        pf.ClinicalFormID,
                                        pf.Inactive
                                    }
                                         ).ToList();

             if (forms == null)
             {
                 return NotFound();
             }

             return forms;
         }*/

        [HttpGet]
        [Route("GetAppointmentICD")]
        public IActionResult GetAppointmentICD(long AppointmentID)
        {
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var AppointmentICDs = _context.AppointmentICD
                                      .Where(s => s.AppointmentID == AppointmentID)
                                      .ToList();
            //var clinicalFormCPT = await _context.ClinicalFormCPT.FindAsync(ClinicalFormID);

            if (AppointmentICDs == null)
            {
                return NotFound();
            }
            
            return Ok(AppointmentICDs);
        }
        [HttpPost]
        [Route("SaveAppointmentCPT")]
        public async Task<ActionResult<AppointmentCPT>> SaveAppointmentCPT(AppointmentCPT[] appointmentCPT)
        {
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            bool succ = TryValidateModel(appointmentCPT);

            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            foreach (var item in appointmentCPT)
            {
                if (item.ID == 0)
                {
                    item.AddedBy = Email;
                    item.AddedDate = DateTime.Now;
                    _context.AppointmentCPT.Add(item);
                }
                else //if (UD.Rights.PatientEdit == true)
                {
                    item.UpdatedBy = Email;
                    item.UpdatedDate = DateTime.Now;
                    _context.AppointmentCPT.Update(item);
                }
                await _context.SaveChangesAsync();

            }

            return Ok(appointmentCPT);
        }
        [HttpPost]
        [Route("SaveAppointmentICD")]
        public async Task<ActionResult<AppointmentICD>> SaveAppointmentICD(AppointmentICD[] appointmentICD)
        {
            var Email = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value;
            bool succ = TryValidateModel(appointmentICD);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(messages);
            }
            foreach (var item in appointmentICD)
            {
                if (item.ID == 0)
                {
                    item.AddedBy = Email;
                    item.AddedDate = DateTime.Now;
                    _context.AppointmentICD.Add(item);
                }
                else //if (UD.Rights.PatientEdit == true)
                {
                    item.UpdatedBy = Email;
                    item.UpdatedDate = DateTime.Now;
                    _context.AppointmentICD.Update(item);
                }
            }
            await _context.SaveChangesAsync();
            
            return Ok(appointmentICD);
        }
        [Route("DeletePatientAppointment/{id}")]
        [HttpPost]
        public async Task<ActionResult> DeletePatientAppointment(long id)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
      User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
      User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if(UD==null)
            {
                return Json("User not found.");
            }
            var PatientAppointment = await _context.PatientAppointment.FindAsync(id);

            if (PatientAppointment == null)
            {
                return Json("Appointment not found.");
            }
            PatientAppointment.Inactive = true;
            PatientAppointment.UpdatedBy = UD.Email;
            PatientAppointment.UpdatedDate = DateTime.Now;

            _context.PatientAppointment.Update(PatientAppointment);  
            await _context.SaveChangesAsync();

            return Ok(PatientAppointment);
        }
        [Route("SyncGoogleSheet")]
        [HttpPost]
        public async Task<ActionResult> SyncGoogleSheet(bool importFromStart)
        {
            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
  User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
  User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

            Practice PracticeModel = _context.Practice.Find(UD.PracticeID);
            if (PracticeModel.isGoogleSheetEnable == true)
            {
                UserCredential credential;
                List<PatientAppointmentsExternal> patientAppointmentList = new List<PatientAppointmentsExternal>();
                string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
                string ApplicationName = "Google Sheets API .NET Quickstart";
                string fulllName = "";
                string[] splitfullName;
                string firstname = "";
                string lastname = "";
                DateTime? dateofBirth = null;
                DateTime dateofBirthSheet;
                string appoinment;
                string[] divideappoinment;
                string ProviderName;
                string clientSecretSheet = string.Empty;
                clientSecretSheet = PracticeModel.googleSheetSecret.ToString();

                byte[] byteArray = Encoding.ASCII.GetBytes(clientSecretSheet);
                MemoryStream stream = new MemoryStream(byteArray);

                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);

                // Create Google Sheets API service.
                var service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                // Define request parameters.
                String spreadsheetId = PracticeModel.googleSheetID.ToString();
                String range = "A:N";
                SpreadsheetsResource.ValuesResource.GetRequest request =
                        service.Spreadsheets.Values.Get(spreadsheetId, range);

                // Prints the names and majors of students in a sample spreadsheet:
                // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
                ValueRange response = request.Execute();
                IList<IList<Object>> values = response.Values;


                string LocationName="", email="", phoneNumber="", calenderId="";
                PatientAppointment patientAppointment = new PatientAppointment();
               
                    if (values != null && values.Count > 0)
                    {
                        int start = PracticeModel.GoogleSheetRows != null ? PracticeModel.GoogleSheetRows.Value : 1;
                        if (values.Count > start)
                        {
                            PracticeModel.GoogleSheetRows = values.Count;
                            _context.Practice.Update(PracticeModel);
                            _context.SaveChangesAsync();
                        }
                        if (importFromStart)
                            start = 1;
                        for (int i = start; i < values.Count; i++)
                        {
                            try
                            {
                                {
                                    fulllName = ""; lastname = ""; dateofBirth = null; phoneNumber = ""; calenderId = "";
                                    patientAppointment = new PatientAppointment();

                                fulllName = (values[i][1]).ToString();
                                splitfullName = fulllName.Split(' ');
                                firstname = splitfullName[0];
                                lastname = splitfullName[1];
                                string[] date = values[i][2].ToString().Split('/');
                                if(date==null || date.Length==0)
                                    date = values[i][2].ToString().Split('-');
                                dateofBirth = new DateTime(int.Parse(date[2]), int.Parse(date[0]), int.Parse(date[1]));
                                //dateofBirthSheet = Convert.ToDateTime(values[i][2]);
                                //dateofBirth = dateofBirthSheet.Date;
                                ProviderName = (values[i][9]).ToString();
                                LocationName = (values[i][3]).ToString();
                                email = (values[i][4]).ToString();
                                phoneNumber = (values[i][5]).ToString(); 
                                calenderId = (values[i][11]).ToString();
                                long providerID = GetProviderID(ProviderName, UD.PracticeID);
                                long locationID = GetLocationID(LocationName, UD.PracticeID);
                                patientAppointment.LocationID = locationID;
                                patientAppointment.ProviderID = providerID;
                                appoinment = (values[i][0]).ToString();
                                divideappoinment = appoinment.Split(' ');
                                patientAppointment.AppointmentDate = Convert.ToDateTime(divideappoinment[0]);
                                patientAppointment.Time = Convert.ToDateTime(appoinment);
                                patientAppointment.VisitInterval = Convert.ToInt32(values[i][13]);
                                patientAppointment.Notes = (values[i][12]).ToString();

                                var patientDetails = (
              from p in _context.Patient
              join pp in _context.PatientPlan on p.ID equals pp.PatientID into Table4
              from ppT in Table4.DefaultIfEmpty()

              where p.FirstName == firstname && p.LastName == lastname && p.DOB == dateofBirth
              select new
              {
                  patientID = p.ID,
                  PrimarypatientPlanID = ppT != null ? ppT.ID : 0

              }).SingleOrDefault();
                                if (patientDetails == null)
                                {
                                    Patient patient = new Patient();
                                    patient.FirstName = firstname;
                                    patient.LastName = lastname;
                                    patient.DOB = dateofBirth;
                                    patient.PracticeID = UD.PracticeID;
                                    patient.ProviderID = providerID;
                                    // 
                                    if (values[i][7] != null && values[i][7].ToString() != "")
                                    {
                                        var insurancePlan = (from ins in _context.InsurancePlan
                                                             where ins.PlanName.ToLower().Equals(values[i][7].ToString().ToLower())
                                                             select ins
                                                          ).Select(s => s.ID).FirstOrDefault();
                                        if (insurancePlan == null || insurancePlan == 0)
                                        {
                                            insurancePlan = (from ins in _context.InsurancePlan
                                                             where ins.PlanName.ToLower().Equals("SelfPay".ToLower())
                                                             select ins
                                                             ).Select(s => s.ID).FirstOrDefault();
                                        }


                                        if (insurancePlan != null && insurancePlan > 0)
                                        {
                                            PatientPlan patientPlan = new PatientPlan();
                                            patientPlan.InsurancePlanID = insurancePlan;
                                            patientPlan.Coverage = "P";
                                            patientPlan.FirstName = firstname;
                                            patientPlan.LastName = lastname;
                                            patientPlan.RelationShip = "18";
                                            patientPlan.IsActive = true;
                                            patientPlan.SubscriberId = values[i][8].ToString();
                                            patient.PatientPlans = new List<PatientPlan>();

                                            patient.PatientPlans.Add(patientPlan);

                                        }

                                    }
                                    PatientController patientController = new PatientController(_context, _contextMain);
                                    patientController.ControllerContext = this.ControllerContext;
                                    patientController.SavePatient(patient).GetAwaiter().GetResult();
                                    //item.Pr= (values[i][9]).ToString();

                                    patientAppointment.PatientID = patient.ID;
                                    patientAppointment.PrimarypatientPlanID = (patient.PatientPlans != null && patient.PatientPlans.Count > 0) ?
                                        patient.PatientPlans.FirstOrDefault().ID : 0;

                                }
                                else
                                {

                                    patientAppointment.PatientID = patientDetails.patientID;
                                    patientAppointment.PrimarypatientPlanID = patientDetails.PrimarypatientPlanID;
                                }


                                var patientAppointmentRecord = SavePatientAppointment(patientAppointment).GetAwaiter().GetResult();
                                PatientAppointmentsExternal patientAppointmentsExternal = new PatientAppointmentsExternal();
                                patientAppointmentsExternal.firstName = firstname;
                                patientAppointmentsExternal.lastName = lastname;
                                patientAppointmentsExternal.emailAddress = email;
                                patientAppointmentsExternal.phoneNumber = phoneNumber;
                                patientAppointmentsExternal.canlenderId = calenderId;
                                patientAppointmentsExternal.dob = dateofBirth;
                                patientAppointmentsExternal.PatientAppointmentID = patientAppointment.ID;
                                patientAppointmentsExternal.rowNumber = i + 1;
                                patientAppointmentsExternal.isError = false;
                                patientAppointmentsExternal.appointmentDate = patientAppointment.AppointmentDate;
                                patientAppointmentsExternal.appointmentTime = patientAppointment.Time;
                                patientAppointmentsExternal.interval = patientAppointment.VisitInterval;
                                patientAppointmentsExternal.comments = patientAppointment.Notes;
                                patientAppointmentList.Add(patientAppointmentsExternal);

                            }
                        }
                        catch (Exception ex)
                        {
                            PatientAppointmentsExternal patientAppointmentsExternal = new PatientAppointmentsExternal();
                            patientAppointmentsExternal.isError = true;
                            patientAppointmentsExternal.exception = ex.StackTrace.ToString();
                            patientAppointmentsExternal.firstName = firstname;
                            patientAppointmentsExternal.lastName = lastname;
                            patientAppointmentsExternal.dob = dateofBirth;
                            patientAppointmentsExternal.PatientAppointmentID = patientAppointment.ID;
                            patientAppointmentsExternal.rowNumber = i + 1;
                            patientAppointmentsExternal.appointmentDate = patientAppointment.AppointmentDate;
                            patientAppointmentsExternal.appointmentTime = patientAppointment.Time;
                            patientAppointmentsExternal.interval = patientAppointment.VisitInterval;
                            patientAppointmentsExternal.comments = patientAppointment.Notes;
                            patientAppointmentList.Add(patientAppointmentsExternal);

                        }

                    }

                }
                foreach (PatientAppointmentsExternal patientAppointmentsExternal in patientAppointmentList)
                {
                    _context.PatientAppointmentsExternal.Add(patientAppointmentsExternal);
                }
                _context.SaveChanges();

                    var ret = new { totalRecordsProcessed = patientAppointmentList.Count, errorsCount = patientAppointmentList.Where(w => w.isError == true).ToList().Count, succeessCount = patientAppointmentList.Where(w => w.isError == false).ToList().Count, errors = patientAppointmentList.Where(w => w.isError == true).ToList(), success = patientAppointmentList.Where(w => w.isError == false).ToList(), };
                    return Json(ret);
             
               
            }
            return null;
        }

        private   long  GetProviderID(string providerName, long PracticeID)
        {
         

            long? providerID = null;
            var providerData = (
            from p in _context.Provider
            where p.Name.ToLower() == providerName.ToLower()
            select new
            {
                id = p.ID

            }).SingleOrDefault();
            if (providerData == null)
            {
                providerData = (
                from p in _context.Provider
                where p.Name.ToLower() == ("Temp Provider").ToLower()
                select new
                {
                    id = p.ID

                }).SingleOrDefault();
                if (providerData == null)
                {
                    Provider item = new Provider();
                    item.Name = "Temp Provider";
                    item.LastName = "Provider";
                    item.FirstName = "Temp";
                    item.NPI = "123";
                    item.PracticeID = PracticeID;
                      
                    ProviderController providerController = new ProviderController(_context, _contextMain);
                    providerController.ControllerContext = this.ControllerContext;
                    providerController.SaveProvider(item).GetAwaiter().GetResult(); 
                    providerID = item.ID;
                }
                else
                    providerID = providerData.id;


            }
            else
              providerID = providerData.id;
            return (providerID!=null?providerID.Value:0);

        }
        private  long  GetLocationID(string LocationName,long PracticeID)
        {

            long? LocationID = null;
            if (LocationName.Contains("https:") || LocationName.Contains("http:"))
                LocationName = "Online";
            var LocationData = (
       from L in _context.Location
       where L.Name.ToLower() == LocationName.ToLower()
       select new
       {
           id = L.ID

       }).SingleOrDefault();
            if (LocationData == null)
            {
                LocationData = (
           from L in _context.Location
           where L.Name.ToLower() == "Temp Location".ToLower()
           select new
           {
               id = L.ID

           }).SingleOrDefault();
                if (LocationData == null)
                {
                    Location item = new Location();
                    item.Name = "Temp Location";
                    item.OrganizationName = "Temp Location";
                    item.PracticeID = PracticeID;
                    item.POSID = 11;
                    LocationController locationController = new LocationController(_context, _contextMain);
                    locationController.ControllerContext = this.ControllerContext;
                    locationController.SaveLocation(item).GetAwaiter().GetResult(); ;
                    //_context.SaveChangesAsync();
                    LocationID = item.ID;
                }
                else
                    LocationID = LocationData.id;

            }
            else
                LocationID = LocationData.id;
             return (LocationID != null ? LocationID.Value : 0); ;

        }
        [Route("SyncGoogleCalender")]
        [HttpPost]
        public async Task<ActionResult> SyncGoogleCalender(string EventDate)
        {

            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
         User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
         User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);
            if (UD == null)
            {
                return Json("User not found.");
            }



           
            Practice PracticeModel = _context.Practice.Find(UD.PracticeID);
            string Practice_Id = PracticeModel.ID.ToString();
            if (PracticeModel.isGoogleCalenderEnable == true)
            {
                string Summary = "";
                string clientSecretCalender = string.Empty;
                clientSecretCalender = PracticeModel.googleCalenderSecret.ToString();

                string[] Scopes = { CalendarService.Scope.CalendarReadonly };
                string ApplicationName = "Google Calendar API .NET Quickstart";

                Google.Apis.Auth.OAuth2.UserCredential credential;

                //  using (var stream =
                //      new FileStream(clientSecretCalender))// System.IO.Path.Combine(_context.env.ContentRootPath, "Resources", "clientSecretCalender.json"), FileMode.Open, FileAccess.Read))
                // {
                Stream stream = new MemoryStream();
                byte[] byteArray = Encoding.UTF8.GetBytes(clientSecretCalender);
                stream.Write(byteArray, 0, byteArray.Length);
                //set the position at the beginning.
                stream.Position = 0;
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = Practice_Id + "tokenCalender.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
                //}

                // Create Google Calendar API service.
                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });


                string[] Calender_ID = PracticeModel.CalenderID.Split(';');

                for (int i = 0; i < Calender_ID.Length; i++)
                {

                    EventsResource.ListRequest request = service.Events.List(Calender_ID[i]);
                    request.TimeMin = Convert.ToDateTime(EventDate);//new DateTime(Event_Date);
                    request.ShowDeleted = false;
                    request.SingleEvents = true;
                    request.MaxResults = 10;
                    request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                    // List events.
                    Events events = request.Execute();
                    Console.WriteLine("Upcoming events:");
                    if (events.Items != null && events.Items.Count > 0)
                    {
                        foreach (var eventItem in events.Items)
                        {
                            string when = eventItem.Start.DateTime.ToString();
                            if (String.IsNullOrEmpty(when))
                            {
                                when = eventItem.Start.Date;
                            }
                            Console.WriteLine("{0} ({1})", eventItem.Summary, when);
                            Summary = eventItem.Summary.ToString();

                            PatientAppointmentsExternal appointment = new PatientAppointmentsExternal();
                            appointment.addedDate = DateTime.Now;
                            appointment.dataReceived = Summary.ToString();
                            _context.PatientAppointmentsExternal.Add(appointment);
                            _context.SaveChanges();
                        }

                    }
                    else
                    {
                        Console.WriteLine("No upcoming events found.");
                    }
                }
                return Json(Summary);
            }
            else
            {

                return Json("Google calender is not enable for this practice");
            }
        }



        [HttpPost]
        [Route("ExportAppointmentReport")]
        public  IActionResult  ExportAppointmentReport(EAppointmentReport EAppointmentReport)
        {


            UserInfoData UD = VMCommon.GetLoginUserInfo(_contextMain,
        User.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email)).Value,
        User.Claims.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.InvariantCultureIgnoreCase)).Value);

            var data = (from pAE in _context.PatientAppointmentsExternal
                        join pA in _context.PatientAppointment on pAE.PatientAppointmentID equals pA.ID into Table1
                        from pAT in Table1.DefaultIfEmpty()
                        join p in _context.Patient on pAT.PatientID equals p.ID into Table2
                        from pT in Table2.DefaultIfEmpty()
                        where
                           (( pT.PracticeID == null ? true : pT.PracticeID==UD.PracticeID)) &&
                             ((EAppointmentReport.Account == null || EAppointmentReport.Account.Length == 0) ? true : pT.AccountNum.Equals(EAppointmentReport.Account)) &&
                             ((EAppointmentReport.lastName.IsNull() || EAppointmentReport.lastName.Length == 0) ? true : pT.LastName.ToLower().Contains(EAppointmentReport.lastName.ToLower())) &&
                              ((EAppointmentReport.firstName.IsNull() || EAppointmentReport.firstName.Length == 0) ? true : pT.FirstName.ToLower().Contains(EAppointmentReport.firstName.ToLower())) &&
                               ((EAppointmentReport.DOB != null || EAppointmentReport.DOB.ToString().Length == 0) ? true : (pT.DOB.Value.Date.Format("MM/dd/yyyy") == EAppointmentReport.DOB.Format("MM/dd/yyyy"))) &&
                           (EAppointmentReport.LocationID.IsNull() ? true : pT.LocationId.Equals(EAppointmentReport.LocationID)) &&
                           (EAppointmentReport.ProviderID.IsNull() ? true : pT.ProviderID.Equals(EAppointmentReport.ProviderID)) &&
                           ((EAppointmentReport.status.IsNull() || EAppointmentReport.status.Length == 0 || EAppointmentReport.status.ToLower().Equals("all")) ? true : EAppointmentReport.status.ToLower().Equals("success") ?  pAE.isError == false : EAppointmentReport.status.ToLower().Equals("success") ? pAE.isError == null: pAE.isError == true) &&
                                                                                                                                                                                                                                                                                                      //((EAppointmentReport.comments.IsNull() || EAppointmentReport.comments.Length == 0) ? true : pAE.comments.ToLower().Contains(EAppointmentReport.comments.ToLower())) //&&
                          (EAppointmentReport.FromDate.ToString().IsNull()|| EAppointmentReport.ToDate.ToString().IsNull()?true:EAppointmentReport.FromDate != null && EAppointmentReport.ToDate != null ?
                         (pAT.AppointmentDate.Value.Date >= EAppointmentReport.FromDate && pAT.AppointmentDate.Value.Date <= EAppointmentReport.ToDate)
                        : (EAppointmentReport.FromDate != null ? (pAT.AppointmentDate.Value.Date.Format("MM/dd/yyyy") == EAppointmentReport.FromDate.Format("MM/dd/yyyy")) : false))
                       && (ExtensionMethods.IsNull_Bool(pAT.Inactive) != true) // && pat.IsActive== true && tpPlan.IsActive == true && pa.Inactive != true
                       // Neet To apply ToTime and From Time Check
                        select new 
                        {
                            pAE.ID,
                            pAE.dataReceived,
                            pAE.addedDate,
                            pAE.PatientAppointmentID,
                            pAE.addedBy,
                            appointmentDate= pAE.appointmentTime.Format("MM/dd/yyyy H:mm"),
                            pAE.appointmentTime,
                            pAE.canlenderId,
                            pAE.comments,
                            dob=pAE.dob.Format("MM/dd/yyyy"),
                            pAE.emailAddress,
                            pAE.exception,
                            pAE.firstName,
                            pAE.insurancePlanName,
                            pAE.interval,
                            isError= pAE.isError==false?"Success": pAE.isError == null ? "Success" :"Error",
                            pAE.lastName,
                            pAE.location,
                            pAE.phoneNumber,
                            pAE.policyNumber,
                            pAE.proivder,
                            pAE.rowNumber,
                            pAE.updatedBy,
                            pAE.updatedDate
                        });
            var lst = data.ToList();//.OrderByDescending(o => o.appointmentDate)
            return Json(lst);
        }


    }
}
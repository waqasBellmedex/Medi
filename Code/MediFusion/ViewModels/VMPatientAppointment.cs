using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;
using static MediFusionPM.ViewModels.VMCommon;

namespace MediFusionPM.ViewModels
{
    public class VMPatientAppointment
    {
        public List<DropDown> Location { get; set; }
        public List<DropDown> Provider { get; set; }
        public List<DropDown> VisitReason { get; set; }
        public List<DropDown> Rooms { get; set; }
        public List<DropDown> Patient { get; set; }
        // public List<DropDown> Status { get; set; }
        public void GetProfiles(ClientDbContext pMContext,string UserID, long? PracticeID)
        {
            
            Location = (from l in pMContext.Location
                        select new DropDown()
                        {
                            ID = l.ID,
                            Description = l.Name + " - " + l.OrganizationName
                        }).ToList();
            Location.Insert(0, new DropDown() { ID = null, Description = "Please Select" });


            Provider = (from p in pMContext.Provider
                        select new DropDown()
                        {
                            ID = p.ID,
                            Description = p.Name
                        }).ToList();
            Provider.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            VisitReason = (from p in pMContext.VisitReason
                           select new DropDown()
                        {
                            ID = p.ID,
                            Description = p.Name
                        }).ToList();
            VisitReason.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

			//Rooms = (from p in pMContext.Rooms
   //                  select new DropDown()
   //                        {
   //                            ID = p.ID,
   //                            Description = p.Name 
   //                        }).ToList();
            Rooms = (from m in pMContext.GeneralItems
                       where m.Type.Equals("rooms") && ExtensionMethods.IsNull_Bool( m.Inactive)!=true 
                       select new DropDown()
                       {
                           ID = m.ID,
                           Description = m.Name
                       }).ToList();
            Rooms.Insert(0, new DropDown() { ID = null, Description = "Please Select" });
            //Patient = (from p in pMContext.Patient
            //           join up in pMContext.UserPractices
            //           on p.PracticeID equals up.PracticeID
            //           join u in pMContext.Users
            //           on up.UserID equals u.Id
            //           where
            //           p.PracticeID == PracticeID && up.UserID == UserID

            //           select new DropDown()
            //           {
            //               ID = p.ID,
            //               Description = p.LastName + ", " + p.FirstName,
            //           }).ToList();
            //Patient.Insert(0, new DropDown() { ID = null, Description = "Please Select" });
        }

        public class EAppointmentReport
        {
            public string Account { get; set; }
            public string lastName { get; set; }
            public string firstName { get; set; }
            public DateTime? DOB { get; set; }
            public long? ProviderID { get; set; }
            public long? LocationID { get; set; }
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }
            public DateTime? FromTime { get; set; }
            public DateTime? ToTime { get; set; }
            public string comments { get; set; }
            public string status { get; set; }

        }

        public class CPatientAppointment
        {
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }
            public DateTime? FromTime { get; set; }
            public DateTime? ToTime { get; set; }
            public long[] ProviderID { get; set; }
            public long[] LocationID { get; set; }
            public long? VisitReasonID { get; set; }
            public long? Status { get; set; }
            public string accountNum { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public DateTime? DOB { get; set; } 

        }

        public class GPatientAppointment
        {
            public long id { get; set; }
            public long? PatientID { get; set; }
            public long? ClientID { get; set; }
            public long? PracticeID { get; set; }
            public string AppointmentDate { get; set; }
            public DateTime? start { get; set; }
            public DateTime? end { get; set; }
            public string resource { get; set; }
            public string Patient { get; set; }
            public string PatientDOB { get; set; }
            public string StatusColor { get; set; } 
            public string text { get; set; }
            public string AccountNum { get; set; }
            public string Plan { get; set; }
            public string Location { get; set; }
            public long LocationID { get; set; }
            public bool Inactive { get; set; }
            
            public string Provider { get; set; }
            public long? ProviderID { get; set; }
            public string VisitReason { get; set; }
            public int TimeInterval { get; set; }
            public string Status { get; set; }
            public string StatusDescription { get; set; }
            public string InsurancePlanID { get; set; }
            public string PhoneNumber { get; set; }
            public string EmailID { get; set; }


        }

        public class VacantSlots
        {
            public long? ProviderID { get; set; }
            public long? LocationID { get; set; }
            public DateTime? AppointmentDate { get; set; }

        }

    }
}

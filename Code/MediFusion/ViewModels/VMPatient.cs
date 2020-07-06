using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;
namespace MediFusionPM.ViewModels
{
   
    public class VMPatient
    {
    
        public List<DropDown>  Practice { get; set; }
        public List<DropDown> Location { get; set; }
        public List<DropDown> Provider { get; set; }
        public List <DropDown> RefProvider { get; set; }
        public string AccountNumber { get; set; }

        public void GetProfiles(ClientDbContext pMContext, long ID)
        {

            Practice = (from f in pMContext.Practice
                        select new DropDown()
                        {
                            ID = f.ID,
                            Description = f.Name + " - " + f.OrganizationName
                        }).ToList();
            Practice.Insert(0, new DropDown() { ID = null, Description = "Please Select" });


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


            RefProvider = (from p in pMContext.RefProvider
                           select new DropDown()
                           {
                               ID = p.ID,
                               Description = p.Name
                           }).ToList();

            RefProvider.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            //if (ID == 0)
            //    AccountNumber = pMContext.GetNextSequenceValue("S_PATIENTACCOUNTNUMBER");  
        }


        public class CPatient
        {
         
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string AccountNum { get; set; }
            public string MedicalRecordNumber { get; set; }
            public string SSN { get; set; }          
            public DateTime? DOB { get; set; }
            public string Practice { get; set; }
            public string Location { get; set; }
            public string Provider { get; set; }
            public string Plan { get; set; }
            public string InsuredID { get; set; }
            public bool InActive { get; set; }
            public DateTime? EntryDateFrom { get; set; }
            public DateTime? EntryDateTo { get; set; }
        }


        public class GPatient
        {
            public long ID { get; set; }
            public string AccountNum { get; set; }
            public string MedicalRecordNumber { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string SSN { get; set; }
            public string DOB { get; set; }
            public long ?PracticeID { get; set; }
            public string Practice { get; set; }
            public long ?LocationID { get; set; }
            public string Location { get; set; }
            public long ?ProviderID { get; set; }
            public string Provider { get; set; }
           // public string PlanName { get; set; }
            public bool? IsActive { get; set; }
            public string AddedDate { get; set; }

        }

        public class OnlyPatient
        {
            public long ID { get; set; }
            public string AccountNum { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string DOB { get; set; }
            public string Gender { get; set; }
            public long? PracticeID { get; set; }
            public long? LocationID { get; set; }
            public long? POSID { get; set; }
            public long? ProviderID { get; set; }
            public long? RefProviderID { get; set; }
        }

    }
}

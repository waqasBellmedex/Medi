using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;
using static MediFusionPM.ViewModels.VMCommon;
using static MediFusionPM.ViewModels.VMLocation;

namespace MediFusionPM.ViewModels
{
    public class VMPractice
    {



        public List<VMLocation.GLocation> Location { get; set; }
        public List<VMProvider.GProvider> Provider { get; set; }
        public List<VMRefProvider.GRefProvider> RefProvider { get; set; }
        public void GetProfiles(ClientDbContext pMContext, long ID)
        {


            Location = (from prac in pMContext.Practice
                        join loc in pMContext.Location on prac.ID equals loc.PracticeID
                        join pos in pMContext.POS on loc.POSID equals pos.ID
                        where prac.ID == ID
                        select new VMLocation.GLocation()
                        {
                            ID = loc.ID,
                            Name = loc.Name,
                            OrganizationName = loc.OrganizationName,
                            PracticeID = prac.ID,
                            Practice = prac.Name,
                            NPI = loc.NPI,
                            PosCode = pos.PosCode,
                            Address = loc.Address1 + ", " + loc.City + ", " + loc.State + ", " + loc.ZipCode
                        }).ToList();
            //Location.Insert(0, new VMLocation.GLocation() { ID = null, Name = "Please Select" });

            Provider = (from prac in pMContext.Practice
                        join pro in pMContext.Provider on prac.ID equals pro.PracticeID
                        where prac.ID == ID
                        select new VMProvider.GProvider()
                        {
                            ID = pro.ID,
                            Name = pro.LastName + ", " + pro.FirstName,
                            FirstName = pro.FirstName,
                            LastName = pro.LastName,
                            NPI = pro.NPI,
                            SSN = pro.SSN,
                            TaxonomyCode = pro.TaxonomyCode,
                            Address = pro.Address1 + ", " + pro.City + ", " + pro.State + ", " + pro.ZipCode,
                            OfficePhoneNum = pro.OfficePhoneNum,
                        }).ToList();
            //Provider.Insert(0, new VMProvider.GProvider() { ID = null, Name = "Please Select" });

            RefProvider = (from prac in pMContext.Practice
                           join refPro in pMContext.RefProvider on prac.ID equals refPro.PracticeID
                           where prac.ID == ID
                           select new VMRefProvider.GRefProvider()
                           {
                               ID = refPro.ID,
                               Name = refPro.LastName + ", " + refPro.FirstName,
                               FirstName = refPro.FirstName,
                               LastName = refPro.LastName,
                               NPI = refPro.NPI,
                               SSN = refPro.SSN,
                               TaxonomyCode = refPro.TaxonomyCode,
                               Address = refPro.Address1 + ", " + refPro.City + ", " + refPro.State + ", " + refPro.ZipCode,
                               OfficePhoneNum = refPro.OfficePhoneNum,
                           }).ToList();
            //RefProvider.Insert(0, new VMRefProvider.GRefProvider() { ID = null, Name = "Please Select" });
        }

        public class CPractice
        {

            public string Name { get; set; }
            public string OrganizationName { get; set; }
            public string NPI { get; set; }
            public string TaxID { get; set; }
            public string PhoneNumber { get; set; }
            public string Address { get; set; }

        }
        public class GPractice
        {
            public long ID { get; set; }
            public string Name { get; set; }
            public string OrganizationName { get; set; }
            public string NPI { get; set; }
            public string TaxID { get; set; }
            public string Address { get; set; }
            public string PayToAddress { get; set; }
            public string OfficePhoneNum { get; set; }
        }
        public class GPracticeAudit
        {
            public long ID { get; set; }
            public long PracticeID { get; set; }
            public long TransactionID { get; set; }
            public string ColumnName { get; set; }
            public string CurrentValue { get; set; }
            public string NewValue { get; set; }
            public string CurrentValueID { get; set; }
            public string NewValueID { get; set; }
            public string HostName { get; set; }
            public string AddedBy { get; set; }
            public DateTime AddedDate { get; set; }
        }
    }
}
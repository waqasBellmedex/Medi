using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;

namespace MediFusionPM.ViewModels
{
    public class VMAction
    {
        public List<DropDown> Users { get; set; }
        public void GetProfiles(ClientDbContext pMContext)
        {
            Users = (from u in pMContext.Users
                     select new DropDown()
                     {
                         
                         Description = u.Email,
                         Description2 = u.LastName + ", " + u.FirstName,
                         Description3 = u.Id,

                     }).ToList();
            Users.Insert(0, new DropDown() { ID = null, Description2 = "Please Select" });

        }
        public class CAction
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string User { get; set; }
        }

        public class GAction
        {
            public long ID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string User { get; set; }
            public string UserID { get; set; }
        }

        public class GClientModel
        {
            public long ID { get; set; }
            public string Name { get; set; }
            public string ContextName { get; set; }
            public string OrganizationName { get; set; }
      
        }


    }
}

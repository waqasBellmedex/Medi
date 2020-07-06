using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;

namespace MediFusionPM.ViewModels
{
    public class VMCpt
    {
        public List<DropDown> Modifiers { get; set; }
        
        public List<DropDown> TypeOfServiceCodes { get; set; }
      //  public CCpt cCpt { get; set; }

        // public GCpt gCpt { get; set; }
       
        public List<DropDown> visitReason { get; set; }
        public List<DropDown> cptType { get; set; }
         
        public void GetProfiles(ClientDbContext pMContext)
        {

            Modifiers = (from m in pMContext.Modifier

                        select new DropDown()
                        {
                            ID = m.ID,
                            Description = m.Code + " - " + m.Description
                        }).ToList();

            Modifiers.Insert(0, new DropDown() { ID = null, Description = "Please Select" });



            TypeOfServiceCodes = (from m in pMContext.TypeOfService

                         select new DropDown()
                         {
                             ID = m.ID,
                             Description = m.Code + " - " + m.Description
                         }).ToList();

            TypeOfServiceCodes.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

              visitReason = (from m in pMContext.VisitReason

                                                 select new DropDown()
                                                 {
                                                     ID = m.ID,
                                                     Description =   m.Name
                                                 }).ToList();
            visitReason.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

              cptType = (from m in pMContext.GeneralItems
                                   where m.Type.Equals("cpt_type") && ExtensionMethods.IsNull_Bool(m.Inactive) != true
                         select new DropDown()
                                          {
                                              ID = m.ID,
                                              Description = m.Name
                                          }).ToList();
            cptType.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

        }

        public class CCpt

        {
            public string CPTCode { get; set; }
            public string Description { get; set; }
            public string NDCNumber { get; set; }
            public string NDCDescription { get; set; }
        }


        public class GCpt
        {
            public long ID { get; set; }
            public string CPTCode { get; set; }
            public string Description { get; set; }

            public decimal Amount { get; set; }
            public string Modifiers { get; set; }
            public string TypeOfService { get; set; }
            public string NDCNumber { get; set; }


        }

    }
}

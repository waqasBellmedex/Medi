using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;

namespace MediFusionPM.ViewModels
{
    public class VMPatientPlan
    {
        //Optional
        public List<Data> InsurancePlans { get; set; }

       //  public List<DropDown> InsurancePlans { get; set; }

        public void GetProfiles(ClientDbContext pMContext)
        {
            //InsurancePlans = (from i in pMContext.InsurancePlan
            //                  select new DropDown()
            //                  {
            //                      ID = i.ID,
            //                      Description = i.PlanName, //+ " - " + p.Coverage, 

            //                  }).ToList();
            //InsurancePlans.Insert(0, new DropDown() { ID = null, Description = "Please Select" });



            //InsurancePlans = (from i in pMContext.InsurancePlan
            //                  join e in pMContext.Edi837Payer on i.Edi837PayerID equals e.ID into Table1
            //                  from edi in Table1.DefaultIfEmpty()
            //                  select new Data()
            //                  {
            //                      ID = i.ID,
            //                      label = i.PlanName, //+ " - " + p.Coverage, 
            //                      Value = i.PlanName + " - " + edi.PayerID,
            //                      Description = edi.PayerID
            //                  }).ToList();
            //InsurancePlans.Insert(0, new Data() { ID = null, Description = "Please Select" });

            //Change Query as discussed with afzal
            InsurancePlans = (from i in pMContext.InsurancePlan
                              join e in pMContext.Edi837Payer on i.Edi837PayerID equals e.ID into Table1
                              from edi in Table1.DefaultIfEmpty()
                              select new Data()
                              {
                                  ID = i.ID,
                                  label = edi.PayerID.IsNull() ? i.PlanName : i.PlanName + " - " + edi.PayerID,
                                  Value = edi.PayerID.IsNull() ? i.PlanName : i.PlanName + " - " + edi.PayerID,
                                  Description = i.PlanName
                                  //Description = edi.PayerID,
                                  

                              }).ToList();
            InsurancePlans.Insert(0, new Data() { ID = null, Description = "Please Select" });

        }



        public class GPatientPlan
        {
            public long ID { get; set; }
            public string Covrage { get; set; }
            public string RelationShip   { get; set; }

            public decimal Subscriber { get; set; }
            public string SubscriberId { get; set; }
            public string GroupNumber { get; set; }
            public string Copay { get; set; }
            public string Active { get; set; }


        }



    }
}

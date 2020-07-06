using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;
namespace MediFusionPM.ViewModels
{
    public class VMInsurancePlan
    {
        ////Optional 
        public List<Data> X12_837_Payer { get; set; }
        public List<Data> X12_276_Payer { get; set; }
        public List<Data> X12_270_Payer { get; set; }




        public List<DropDown> Insurance { get; set; }
        public List<DropDown> PlanType { get; set; }
        //public List<DropDown> X12_837_Payer { get; set; }
        //public List<DropDown> X12_276_Payer { get; set; }
        //public List<DropDown> X12_270_Payer { get; set; }
        public Receiver Receiver { get; set; }

        public void GetProfiles(ClientDbContext pMContext)
        {


            PlanType = (from p in pMContext.PlanType
                         select new DropDown()
                         {
                             ID = p.ID,
                             Description = p.Code + " - " + p.Description
                         }).ToList();

            PlanType.Insert(0, new DropDown() { ID = null, Description = "Please Select" });


            Insurance = (from i in pMContext.Insurance

                        select new DropDown()
                        {
                            ID = i.ID,
                            Description = i.Name + " - " + i.OrganizationName,
                        }).ToList();

            Insurance.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            ////Optional 
            X12_837_Payer = (from edi837 in pMContext.Edi837Payer
                             join rec in pMContext.Receiver
                             on edi837.ReceiverID equals rec.ID
                             select new Data()
                             {
                                 ID = edi837.ID,
                                 Value = edi837.PayerID,
                                 label = edi837.PayerName + ", " + edi837.PayerID,
                                 Description = rec.Name,
                             }).ToList();
            X12_837_Payer.Insert(0, new Data() { ID = null, Description = "please select" });

            X12_276_Payer = (from edi276 in pMContext.Edi276Payer
                             join rec in pMContext.Receiver
                             on edi276.ReceiverID equals rec.ID
                             select new Data()
                             {
                                 ID = edi276.ID,
                                 Value = edi276.PayerID,
                                 label = edi276.PayerName + ", " + edi276.PayerID,
                                 Description = rec.Name,
                             }).ToList();
            X12_276_Payer.Insert(0, new Data() { ID = null, Description = "please select" });

            X12_270_Payer = (from edi270 in pMContext.Edi270Payer
                             join rec in pMContext.Receiver
                             on edi270.ReceiverID equals rec.ID
                             select new Data()
                             {
                                 ID = edi270.ID,
                                 Value = edi270.PayerID,
                                 label = edi270.PayerName + ", " + edi270.PayerID,
                                 Description = rec.Name,
                             }).ToList();
            X12_270_Payer.Insert(0, new Data() { ID = null, Description = "please select" });


            //X12_837_Payer = (from edi837 in pMContext.Edi837Payer
            //                 join rec in pMContext.Receiver
            //                 on edi837.ReceiverID equals rec.ID
            //                 select new DropDown()
            //                 {
            //                     ID = edi837.ID,
            //                     Description = edi837.PayerName,
            //                     Description2 = edi837.PayerID,
            //                     Description3 = rec.Name,
            //                 }).ToList();
            //X12_837_Payer.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            //X12_276_Payer = (from edi276 in pMContext.Edi276Payer
            //                 join rec in pMContext.Receiver
            //                 on edi276.ReceiverID equals rec.ID
            //                 select new DropDown()
            //                 {
            //                     ID = edi276.ID,
            //                     Description = edi276.PayerName,
            //                     Description2 = edi276.PayerID,
            //                     Description3 = rec.Name,
            //                 }).ToList();


            //X12_276_Payer.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            //X12_270_Payer = (from edi270 in pMContext.Edi270Payer
            //                 join rec in pMContext.Receiver
            //                 on edi270.ReceiverID equals rec.ID
            //                 select new DropDown()
            //                 {
            //                     ID = edi270.ID,
            //                     Description = edi270.PayerName,
            //                     Description2 = edi270.PayerID,
            //                     Description3 = rec.Name,
            //                 }).ToList();

            //X12_270_Payer.Insert(0, new DropDown() { ID = null, Description = "Please Select" });


        }
        public class CInsurancePlan
        {
            
                public string PlanName { get; set; }
                public string Description { get; set; }
                public string Insurance { get; set; }
                public string PlanType { get; set; }
                public string PayerName { get; set; }
                public long? PayerID { get; set; }
      

        }

        public class GInsurancePlan
            {
            public long ID { get; set; }
            public string PlanName { get; set; }
            public string Description { get; set; }
            public long? InsuranceID { get; set; }
            public string Insurance { get; set; }
            public string PayerName { get; set; }
            public string PayerID { get; set; }
            public string PlanType { get; set; }


        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;


namespace MediFusionPM.ViewModels
{
    public class VMPaymentCheck
    {
        public List<DropDown> Practice { get; set; }
        public List<DropDown> Receiver { get; set; }
        public List<DropDown> DocumentType { get; set; }

        public List<Data> AdjustmentCodes { get; set; }
        public List<Data> RemarkCodes { get; set; }


        public void GetProfiles(ClientDbContext pMContext)
        {

            Practice = (from f in pMContext.Practice
                        select new DropDown()
                        {
                            ID = f.ID,
                            Description = f.Name + " - " + f.OrganizationName
                        }).ToList();
            Practice.Insert(0, new DropDown() { ID = null, Description = "Please Select" });


            Receiver = (from r in pMContext.Receiver
                        select new DropDown()
                        {
                            ID = r.ID,
                            Description = r.Name,
                        }).ToList();
            Receiver.Insert(0, new DropDown() { ID = null, Description = "Please Select" });


            DocumentType = (from dt in pMContext.DocumentType
                            select new DropDown()
                        {
                            ID = dt.ID,
                            Description = dt.Name,
                        }).ToList();
            DocumentType.Insert(0, new DropDown() { ID = null, Description = "Please Select" });


            AdjustmentCodes = (from a in pMContext.AdjustmentCode

                            select new Data()
                            {
                                ID = a.ID,
                                Value = a.Code,
                                label = a.Code,
                                Description = a.Description,
                                Category = a.Type.IsNull() ? "D" : a.Type
                            }).ToList();

            AdjustmentCodes.Insert(0, new Data() { ID = null, Description = "Please Select" });

           
            RemarkCodes = (from r in pMContext.RemarkCode

                               select new Data()
                               {
                                   ID = r.ID,
                                   Value = r.Code,
                                   label = r.Code,
                                   Description = r.Description,
                               }).ToList();

            RemarkCodes.Insert(0, new Data() { ID = null, Description = "Please Select" });


        }

        public class CPaymentCheck
        {
            public DateTime? PostedFromDate { get; set; }
            public DateTime? PostedToDate { get; set; }
            public DateTime? EntryDateTo { get; set; }
            public DateTime? EntryDateFrom { get; set; }
            
            public DateTime? CheckDateTo { get; set; }
            public DateTime? CheckDateFrom { get; set; }
            public string CheckNumber { get; set; }
            public string Practice { get; set; }
            public string Payer { get; set; }
            public string Provider { get; set; }
            public string Location { get; set; }
            public string Status { get; set; }
            public long? ReceiverID { get; set; }
            public string TypeID { get; set; }

        }


        public class GPaymentCheck
        {
            public long Id { get; set; }
            public string CheckNumber { get; set; }
            public string PaymentMethod { get; set; }
            public string CheckDate { get; set; }
            public decimal? CheckAmount { get; set; }
            public decimal? Appliedamount { get; set; }
            public decimal? PostedAmount { get; set; }
            public long NumberOfVisits { get; set; }
            public decimal NumberOfPatients { get; set; }
            public string Status { get; set; }
            public string Payer { get; set; }
            public string Practice { get; set; }
            public string EntryDate { get; set; }
            public long PracticeID { get; set; }
            // New Fields Added
            public string EnteredBy { get; set; }
            public long? ReceiverID { get; set; }
            public string Receiver { get; set; }
            
            public string PostedDate { get; set; }
            public string PayeeName { get; set; }
            public string PayeeNPI { get; set; }
            public string FileName { get; set; }

        }

    }
}

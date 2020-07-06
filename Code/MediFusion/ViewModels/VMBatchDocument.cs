using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.Models;

namespace MediFusionPM.ViewModels
{
    public class VMBatchDocument
    {
        public List<DropDown> Practice { get; set; }
        public List<DropDown> Biller { get; set; }
        public List<DropDown> Category { get; set; }
        public List<DropDown> Location { get; set; }
        public List<DropDown> Provider { get; set; }


        public void GetProfiles(ClientDbContext pMContext)
        {

            Practice = (from f in pMContext.Practice

                        select new DropDown()
                        {
                            ID = f.ID,
                            Description = f.Name + " - " + f.OrganizationName
                        }).ToList();

            Practice.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            Biller = (from b in pMContext.Biller

                        select new DropDown()
                        {
                            ID = b.ID,
                            Description = b.LastName + ", " + b.FirstName,
                        }).ToList();

            Biller.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            Category = (from d in pMContext.DocumentType

                      select new DropDown()
                      {
                          ID = d.ID,
                          Description = d.Name,
                      }).ToList();

            Category.Insert(0, new DropDown() { ID = null, Description = "Please Select" });


            Location = (from d in pMContext.Location

                        select new DropDown()
                        {
                            ID = d.ID,
                            Description = d.Name,
                        }).ToList();

            Location.Insert(0, new DropDown() { ID = null, Description = "Please Select" });

            Provider = (from d in pMContext.Provider

                        select new DropDown()
                        {
                            ID = d.ID,
                            Description = d.Name,
                        }).ToList();

            Provider.Insert(0, new DropDown() { ID = null, Description = "Please Select" });
        }


        public class CBatchDocument
        {
            public long? BatchNumber { get; set; }
            public string ResponsibleParty { get; set; }
            public long? DocumentType { get; set; }
            public long? LocationID { get; set; }
            public long? ProviderID { get; set; }
            public string Status { get; set; }

        }

        public class GBatchDocument
        {

            public long BatchNumber { get; set; }
            public string EntryDate { get; set; }
            public string ResponsibleParty { get; set; }
            public string Status { get; set; }
            public long? NumOfPages { get; set; }
            public long? NumOfDemographics { get; set; }
            public long? NumOfVisits { get; set; }
            public decimal? NumOfCheck { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
           

        }
        public class CBatchNumberDocument
        {
            public long? CptId { get; set; }
            public long? ProviderId { get; set; }
            public long? PatientId { get; set; }
            public long? PatientPlanId { get; set; }
            public DateTime? DateOfServiceFrom { get; set; }
            public DateTime? DateOfServiceTo { get; set; }

        }
 


        }
}

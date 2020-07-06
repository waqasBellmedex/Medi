namespace MediFusionPM.ViewModels
{
    public class VMClient
    {
        public class CClient
        {
            public string Name { get; set; }
            public string OrganizationName { get; set; }
            public string TaxID { get; set; }
            public string OfficePhoneNo { get; set; }
            public string Address { get; set; }
        }

        public class GClient
        {
            public long ID { get; set; }
            public string Name { get; set; }
            public string OrganizationName { get; set; }
            public string TaxID { get; set; }
            public string Address { get; set; }
            public string ContactPerson { get; set; }
            public string OfficePhoneNum { get; set; }
        }


      


    }
}

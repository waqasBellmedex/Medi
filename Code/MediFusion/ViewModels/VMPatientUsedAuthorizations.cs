using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFusionPM.ViewModels
{
    public class VMPatientUsedAuthorizations
    {
		
	} 
	public class GVMPatientUsedAuthorizations
    {
		public string AuthorizationNumber { get; set; }
		public long? VisitID { get; set; }
		public DateTime? DOS { get; set; }

		public decimal? BilledAmount { get; set; }

		public string ProviderName { get; set; }
		public string CPT { get; set; }
		
	}

	public class GVMVisitUsedAuthorizations
	{
		public string InsurancePlan { get; set; }
		public string SubscriberID { get; set; }
		public string AuthorizationNumber { get; set; }
		public long? VisitID { get; set; }
		public long? ChargeID { get; set; }
		public string DOS { get; set; }
		public string CPT { get; set; }
		public decimal? BilledAmount { get; set; }
		public decimal? AllowedAmount { get; set; }
		public decimal? PaidAmount { get; set; }
		public decimal? Balance { get; set; }
	}

}

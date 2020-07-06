using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediFusionPM.BusinessLogic.ClaimGeneration
{
    public class ChargeData
    {
        public string CptCode { get; set; }
        public string Modifier1 { get; set; }
        public string Modifier2 { get; set; }
        public string Modifier3 { get; set; }
        public string Modifier4 { get; set; }
        public string LIDescription { get; set; }

        public decimal ?ChargeAmount { get; set; }
        public string Units { get; set; }
        public string Minutes { get; set; }
        public string POS { get; set; }
        public string Pointer1 { get; set; }
        public string Pointer2 { get; set; }
        public string Pointer3 { get; set; }
        public string Pointer4 { get; set; }

        public string ICD1 { get; set; }
        public string ICD2 { get; set; }
        public string ICD3 { get; set; }
        public string ICD4 { get; set; }

        public bool IsEmergency { get; set; }
       
        public DateTime DateofServiceFrom { get; set; }
        public DateTime ?DateOfServiceTo { get; set; }
        public string LineItemControlNum { get; set; }
        public string CliaNumber { get; set; }
        public string ReferringCliaNum { get; set; }
       
        public string ServiceLineNotes { get; set; }
        public string DrugNumber { get; set; }
        public string DrugCount { get; set; }
        public string DrugUnit { get; set; }

        public string RendPrvLastName { get; set; }
        public string RendPrvFirstName { get; set; }
        public string RendPrvMI { get; set; }
        public string RendPrvNPI { get; set; }
        public string RendPrvTaxonomy { get; set; }
       
        public string LocationOrgName { get; set; }
        public string LocationNPI { get; set; }
        public string LocationAddress { get; set; }
        public string LocationCity { get; set; }
        public string LocationState { get; set; }
        public string LocationZip { get; set; }
       
        public string SuperPrvLastName { get; set; }
        public string SuperPrvFirstName { get; set; }
        public string SuperPrvMI { get; set; }
        public string SuperPrvNPI { get; set; }
       
        public string RefPrvLastName { get; set; }
        public string RefPrvFirstName { get; set; }
        public string RefPrvMI { get; set; }
        public string RefPrvNPI { get; set; }
       
       
        public decimal ?PrimaryPaidAmt { get; set; }
        public string PrimaryCPT { get; set; }
        public string PrimaryMod1 { get; set; }
        public string PrimaryMod2 { get; set; }
        public string PrimaryMod3 { get; set; }
        public string PrimaryMod4 { get; set; }
        public string PrimaryUnits { get; set; }
        public decimal ?PrimaryWriteOffAmt { get; set; }
        public decimal ?PrimaryOthWriteOffAmt { get; set; }
        public decimal ?PrimaryCoIns { get; set; }
        public decimal ?PrimaryDeductable { get; set; }
        public string PrimaryAdjQuantity { get; set; }
        public DateTime ?PrimaryPaidDate { get; set; }
        
        public DateTime SubmittedDate { get; internal set; }
        public bool Submitted { get; internal set; }

        public long ChargeID { get; set; }
        public long ?PrimaryChargeID { get; set; }

    }
}

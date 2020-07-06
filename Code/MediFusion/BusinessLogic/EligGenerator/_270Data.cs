using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace MediFusionPM.BusinessLogic.EligGenerator
{
    public class _270Data
    {
        public _270Data()
        {
            ProvEntity = "1";
            ProvQual = "XX";
            EligiblityForDate = DateTime.Now;
            PatientRelationValue = PatientRelationShip.Self_18;
            this.PayerEntity = "2";
            this.PayerQual = "PI";
        }

        public enum PatientRelationShip
        {
            Self_18,
            Spouse_01,
            Child_19,
            Other_Adult_34
        }

        public string PayerEntity { get; set; }
        public string PayerOrgName { get; set; }
        public string PayerFirstName { get; set; }
        public string PayerQual { get; set; }
        public string PayerID { get; set; }


        public string ProvEntity { get; set; }
        public string ProvLastName { get; set; }
        public string ProvFirstName { get; set; }
        public string ProvMI { get; set; }
        public string ProvQual { get; set; }
        public string ProvNPI { get; set; }
        public string ProvTaxonomyCode { get; set; }

        // NM1-IL   Subscriber name
        public string SBREntityType { get; set; }
        public string SBRLastName { get; set; }
        public string SBRFirstName { get; set; }
        public string SBRMiddleInitial { get; set; }
        public string SBRID { get; set; }

        //N3, N4    Subscriber
        public string SBRAddress { get; set; }
        public string SBRCity { get; set; }
        public string SBRState { get; set; }
        public string SBRZipCode { get; set; }

        //DMG       Subscriber
        public DateTime ?SBRDob { get; set; }
        public string SBRGender { get; set; }
        public string SBRSSN { get; set; }

        public PatientRelationShip PatientRelationValue { get; set; }
        
        // NM1-QC   Patient name
        public string PATEntityType { get; set; }
        public string PATLastName { get; set; }
        public string PATFirstName { get; set; }
        public string PATMiddleInitial { get; set; }
       
        //N3, N4    Patient Address, City, State, Zip
        public string PATAddress { get; set; }
        public string PATCity { get; set; }
        public string PATState { get; set; }
        public string PATZipCode { get; set; }

        //DMG       Patient DOB, Gender
        public DateTime PATDob { get; set; }
        public string PATGender { get; set; }

        public string TRN01 { get; set; }
        public string TRN02 { get; set; }
        public DateTime EligiblityForDate { get; set; }


        public long ?PatientID { get; set; }
        public long ?PracticeID { get; set; }
        public long ?LocationID { get; set; }

    }
}

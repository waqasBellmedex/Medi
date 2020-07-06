using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MediFusionPM.ViewModel;


namespace MediFusionPM.Models
{
    public class InstitutionalData
    {
        [Required]
        [Key, ForeignKey("Visit")]
        public long ID { get; set; }
        public virtual Visit Visit { get; set; }

        //Admission Info Columns
        public DateTime? StatementFromDate { get; set; }
        public DateTime? StatementToDate { get; set; }
        [ForeignKey("ID")]
        public long? PatientStatusCodeID { get; set; }
        public virtual PatientStatusCode PatientStatusCode { get; set; }
        [ForeignKey("ID")]
        public long? ReasonOfVisitID { get; set; }
        public virtual VisitReason VisitReason { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public DateTime? AdmissionHour { get; set; }
        public string AdmissionType { get; set; }
        [ForeignKey("ID")]
        public long? AdmissionSourceID { get; set; }
        public virtual AdmissionSourceCode AdmissionSourceCode { get; set; }
        public DateTime? DischargeDate { get; set; }


        // Principal Admitting Diagonsis

        [ForeignKey("ID")]
        public long? PrincipalCodeID { get; set; }
        public virtual ICD ICD { get; set; }
        [ForeignKey("ID")]
        public long? AdmittingCodeID { get; set; }
        public virtual ICD ICD1 { get; set; }


        //External Cause Of Injury
        [ForeignKey("ID")]
        public long? ExternalInjuryCode1ID { get; set; }
        public virtual  ExternalInjuryCode ExternalInjuryCode1 { get; set; }
        [ForeignKey("ID")]
        public long? ExternalInjuryCode2ID { get; set; }
        public virtual ExternalInjuryCode ExternalInjuryCode2 { get; set; }
        [ForeignKey("ID")]
        public long? ExternalInjuryCode3ID { get; set; }
        public virtual ExternalInjuryCode ExternalInjuryCode3 { get; set; }


        //Procedure Codes
        [ForeignKey("ID")]
        public long? PrincipalProcedureCodeID1 { get; set; }
        public virtual Cpt Cpt { get; set; }
        public DateTime? PrincipalProcedureDate1 { get; set; }
        [ForeignKey("ID")]
        public long? PrincipalProcedureCodeID2 { get; set; }
        public virtual Cpt Cpt1 { get; set; }
        public DateTime? PrincipalProcedureDate2 { get; set; }
        [ForeignKey("ID")]
        public long? PrincipalProcedureCodeID3 { get; set; }
        public virtual Cpt Cpt2 { get; set; }
        public DateTime? PrincipalProcedureDate3 { get; set; }
        [ForeignKey("ID")]
        public long? PrincipalProcedureCodeID4 { get; set; }
        public virtual Cpt Cpt3 { get; set; }
        public DateTime? PrincipalProcedureDate4 { get; set; }
        [ForeignKey("ID")]
        public long? PrincipalProcedureCodeID5 { get; set; }
        public virtual Cpt Cpt4 { get; set; }
        public DateTime? PrincipalProcedureDate5 { get; set; }
        [ForeignKey("ID")]
        public long? PrincipalProcedureCodeID6 { get; set; }
        public virtual Cpt Cpt5 { get; set; }
        public DateTime? PrincipalProcedureDate6 { get; set; }

        // Value Codes
        [ForeignKey("ID")]
        public long? ValueCode1ID { get; set; }
        public virtual ValueCode ValueCode1 { get; set; }
        [ForeignKey("ID")]
        public long? ValueCode2ID { get; set; }
        public virtual ValueCode ValueCode2 { get; set; }
        [ForeignKey("ID")]
        public long? ValueCode3ID { get; set; }
        public virtual ValueCode ValueCode3 { get; set; }
        [ForeignKey("ID")]
        public long? ValueCode4ID { get; set; }
        public virtual ValueCode ValueCode4 { get; set; }
        [ForeignKey("ID")]
        public long? ValueCode5ID { get; set; }
        public virtual ValueCode ValueCode5 { get; set; }
        [ForeignKey("ID")]
        public long? ValueCode6ID { get; set; }
        public virtual ValueCode ValueCode6 { get; set; }
        [ForeignKey("ID")]
        public long? ValueCode7ID { get; set; }
        public virtual ValueCode ValueCode7 { get; set; }
        [ForeignKey("ID")]
        public long? ValueCode8ID { get; set; }
        public virtual ValueCode ValueCode8 { get; set; }
        [ForeignKey("ID")]
        public long? ValueCode9ID { get; set; }
        public virtual ValueCode ValueCode9 { get; set; }
        [ForeignKey("ID")]
        public long? ValueCode10ID { get; set; }
        public virtual ValueCode ValueCode10 { get; set; }
        [ForeignKey("ID")]
        public long? ValueCode11ID { get; set; }
        public virtual ValueCode ValueCode11 { get; set; }
        [ForeignKey("ID")]
        public long? ValueCode12ID { get; set; }
        public virtual ValueCode ValueCode12 { get; set; }


        //Condition Codes
        [ForeignKey("ID")]
        public long? ConditionCode1ID { get; set; }
        public virtual ConditionCode ConditionCode1 { get; set; }
        [ForeignKey("ID")]
        public long? ConditionCode2ID { get; set; }
        public virtual ConditionCode ConditionCode2 { get; set; }
        [ForeignKey("ID")]
        public long? ConditionCode3ID { get; set; }
        public virtual ConditionCode ConditionCode3 { get; set; }
        [ForeignKey("ID")]
        public long? ConditionCode4ID { get; set; }
        public virtual ConditionCode ConditionCode4 { get; set; }
        [ForeignKey("ID")]
        public long? ConditionCode5ID { get; set; }
        public virtual ConditionCode ConditionCode5 { get; set; }
        [ForeignKey("ID")]
        public long? ConditionCode6ID { get; set; }
        public virtual ConditionCode ConditionCode6 { get; set; }
        [ForeignKey("ID")]
        public long? ConditionCode7ID { get; set; }
        public virtual ConditionCode ConditionCode7 { get; set; }
        [ForeignKey("ID")]
        public long? ConditionCode8ID { get; set; }
        public virtual ConditionCode ConditionCode8 { get; set; }
        [ForeignKey("ID")]
        public long? ConditionCode9ID { get; set; }
        public virtual ConditionCode ConditionCode9 { get; set; }
        [ForeignKey("ID")]
        public long? ConditionCode10ID { get; set; }
        public virtual ConditionCode ConditionCode10 { get; set; }
        [ForeignKey("ID")]
        public long? ConditionCode11ID { get; set; }
        public virtual ConditionCode ConditionCode11 { get; set; }
        [ForeignKey("ID")]
        public long? ConditionCode12ID { get; set; }
        public virtual ConditionCode ConditionCode12 { get; set; }
       
        
        // Occurance Span Code
        [ForeignKey("ID")]
        public long? OccuranceSpanCode1ID { get; set; }
        public virtual OccurrenceSpanCode OccurrenceSpanCode1 { get; set; }
        public DateTime? SpanCode1FromDate { get; set; }
        public DateTime? SpanCode1ToDate { get; set; }
        [ForeignKey("ID")]
        public long? OccuranceSpanCode2ID { get; set; }
        public virtual OccurrenceSpanCode OccurrenceSpanCode2 { get; set; }
        public DateTime? SpanCode2FromDate { get; set; }
        public DateTime? SpanCode2ToDate { get; set; }
        [ForeignKey("ID")]
        public long? OccuranceSpanCode3ID { get; set; }
        public virtual OccurrenceSpanCode OccurrenceSpanCode3 { get; set; }
        public DateTime? SpanCode3FromDate { get; set; }
        public DateTime? SpanCode3ToDate { get; set; }
        [ForeignKey("ID")]
        public long? OccuranceSpanCode4ID { get; set; }
        public virtual OccurrenceSpanCode OccurrenceSpanCode4 { get; set; }
        public DateTime? SpanCode4FromDate { get; set; }
        public DateTime? SpanCode4ToDate { get; set; }


        // Occurance Code
        [ForeignKey("ID")]
        public long? OccuranceCode1ID { get; set; }
        public virtual OccurrenceCode OccurrenceCode1 { get; set; }
        public DateTime? OccuranceCode1Date { get; set; }
        [ForeignKey("ID")]
        public long? OccuranceCode2ID { get; set; }
        public virtual OccurrenceCode OccurrenceCode2 { get; set; }
        public DateTime? OccuranceCode2Date { get; set; }
        [ForeignKey("ID")]
        public long? OccuranceCode3ID { get; set; }
        public virtual OccurrenceCode OccurrenceCode3 { get; set; }
        public DateTime? OccuranceCode3Date { get; set; }
        [ForeignKey("ID")]
        public long? OccuranceCode4ID { get; set; }
        public virtual OccurrenceCode OccurrenceCode4 { get; set; }
        public DateTime? OccuranceCode4Date { get; set; }
        [ForeignKey("ID")]
        public long? OccuranceCode5ID { get; set; }
        public virtual OccurrenceCode OccurrenceCode5 { get; set; }
        public DateTime? OccuranceCode5Date { get; set; }
        [ForeignKey("ID")]
        public long? OccuranceCode6ID { get; set; }
        public virtual OccurrenceCode OccurrenceCode6 { get; set; }
        public DateTime? OccuranceCode6Date { get; set; }
        [ForeignKey("ID")]
        public long? OccuranceCode7ID { get; set; }
        public virtual OccurrenceCode OccurrenceCode7 { get; set; }
        public DateTime? OccuranceCode7Date { get; set; }
        [ForeignKey("ID")]
        public long? OccuranceCode8ID { get; set; }
        public virtual OccurrenceCode OccurrenceCode8 { get; set; }
        public DateTime? OccuranceCode8Date { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }


    }
}

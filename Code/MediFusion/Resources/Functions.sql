 CREATE FUNCTION TranslateStatus
 ( @Status AS  nvarchar(100) )
RETURNS nvarchar(100)
AS
BEGIN
 DECLARE @result nvarchar(100); 
	IF (@Status = 'N')
		SET @result = 'New Charge'
	else
		SET @result = 'Other'

		   IF (@Status= 'N')
                SET @result = 'New Charge';
            IF (@Status= 'S')
                SET @result = 'Submitted';
            IF (@Status= 'K')
                SET @result = '999 Accepted';
            IF (@Status= 'D')
                SET @result = '999 Denied';
            IF (@Status= 'E')
                SET @result = '999 has Errors';
            IF (@Status= 'P')
                SET @result = 'Paid';
            IF (@Status= 'PAT_T_PT')
                SET @result = 'PATIENT TO PLAN TRANSFER';
            IF (@Status= 'DN')
                SET @result = 'Denied';
            IF (@Status= 'PT_P')
                SET @result = 'Patient Paid';
            IF (@Status= 'PPTS')
                SET @result = 'Transefered to Sec.';
            IF (@Status= 'PPTT')
                SET @result = 'Paid-Transfered To Ter';
            IF (@Status= 'PPTP')
                SET @result = 'Transfered to Patient';
            IF (@Status= 'SPTP')
                SET @result = 'Paid-Transfered To Patient';
            IF (@Status= 'SPTT')
                SET @result = 'Paid-Transfered To Ter';
            IF (@Status= 'PR_TP')
                SET @result = 'Pat. Resp. Transferred to Pat';
            IF (@Status= 'PPTM')
                SET @result = 'Paid - Medigaped';
            IF (@Status= 'M')
                SET @result = 'Medigaped';
            IF (@Status= 'R')
                SET @result = 'Rejected';
            IF (@Status= 'A1AY')
                SET @result = 'Received By Clearing House';
            IF (@Status= 'A0PR')
                SET @result = 'Forwarded  to Payer';
            IF (@Status= 'A1PR')
                SET @result = 'Received By Payer';
            IF (@Status= 'A2PR')
                SET @result = 'Accepted By Payer';
            IF (@Status= 'TS')
                SET @result = 'Transferred to Secondary';
            IF (@Status= 'TT')
                SET @result = 'Transferred to Tertiary';
            IF (@Status= 'PTPT')
                SET @result = 'Plan to Patient Transfer';
            
	    RETURN @result
END

------------------------------------------------------------------------------------------------
GO
CREATE PROCEDURE [dbo].[FindPatientVisitReports]
	@ProviderID bigint = null , @VisitType NVARCHAR = null,
	 @DateOfServiceFrom datetime = null,@DateOfServiceTo datetime = null,
	 @EntryDateFrom datetime = null,@EntryDateTo datetime = null,
	 @SubmittedDateTo datetime = null,@SubmittedDateFrom datetime = null,
	 @CPTCode NVARCHAR = null,  @PaymentCriteria NVARCHAR = null,
	 @PatientName NVARCHAR = null,@PrescribingMD NVARCHAR = null,
	  @pageNo INT = null,@PerPage NVARCHAR = null
AS
BEGIN
		SELECT 	
chargetable.PatientID as PatientID,chargetable.VisitID as  ClaimID,chargetable.SubmittetdDate as  SubmissionDate,
patienttable.FirstName  as PatientFirstName,patienttable.LastName  as PatientLastName,patienttable.LastName + ', ' + patienttable.FirstName as PatientName,
patienttable.MiddleInitial  as MiddleInitial,patienttable.Gender  as PatientGender,patienttable.SSN  as SSN,
Cpt.CPTCode  as Cpt,Cpt.[Description]  as CptDescription,patienttable.DOB  as DateOfBirth,chargetable.DateOfServiceFrom  as DateOfService,
icd1t.ICDCode  as dx1,icd2t.ICDCode  as dx2,icd3t.ICDCode  as dx3,icd4t.ICDCode  as dx4,m1t.Code  as MOD1,
m2t.Code  as MOD2,postable.PosCode  as POS,providertable.[Name]  as ProviderName,postable.[Name]  as FacilityName,
providertable.NPI as  IndividualNPI,insurangeplantable.PlanName as InsuranceName,insurangeplantable.PlanName  as PrimaryInsurance,
patientplantable.SubscriberId  as PrimaryPolicyNumber,secinsuranceplantable.PlanName as  SecondaryInsurance,
secpatientplantable.SubscriberId as  SecondaryPolicyNumber,otherinsurancetable.PlanName  as OtherInsurance,
otherpatientplantable.SubscriberId  as OtherPolicyNumber,refprovidertable.[Name] as  ReferringPhysicianName,
cpt.ShortDescription as ShortCptDescription,chargetable.AddedDate as EntryDate,
chargetable.PrimaryPaymentDate as PrimaryCheckDate,chargetable.SecondaryPaymentDate as SecondaryCheckDate,
chargetable.SecondaryPaymentDate as SecondaryPaymentDate,insurangeplantable.[Description] as PayerName,
chargetable.PrimaryPaymentDate as PaymentDate,v.PrescribingMD as PrescribingMD,
providertable.[Name] as [Provider],
SUM (ISNULL(chargetable.PatientPaid,0) + ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0) ) as Payment,
SUM (ISNULL(chargetable.PatientPaid,0) + ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0) ) as CollectedRevenue,
(SUM (ISNULL(chargetable.PatientPaid,0) + ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0) ) / 4) as AverageRevenue,
SUM (ISNULL(chargetable.PrimaryWriteOff,0) + ISNULL( chargetable.SecondaryWriteOff, 0) + ISNULL(chargetable.TertiaryWriteOff,0) ) as AdjustmentAmount,
SUM (ISNULL(chargetable.PrimaryAllowed,0) + ISNULL(chargetable.SecondaryAllowed,0) + ISNULL(chargetable.TertiaryAllowed,0)) as AllowedAmount,
chargetable.TotalAmount as Charges,
SUM (ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) as PaidAmount,
SUM (ISNULL(chargetable.PrimaryPatientBal,0) + ISNULL(chargetable.SecondaryPatientBal,0) + ISNULL(chargetable.TertiaryPatientBal,0)) as PatientBalance,
SUM (ISNULL(chargetable.PrimaryBal,0) + ISNULL(chargetable.SecondaryPatientBal,0) + ISNULL(chargetable.TertiaryPatientBal,0)) as PlanBalance,
SUM (ISNULL(chargetable.PrimaryPatientBal,0) + ISNULL(chargetable.SecondaryPatientBal,0) + ISNULL(chargetable.TertiaryPatientBal,0)) as Balance,
SUM (ISNULL(chargetable.PrimaryBilledAmount,0) + ISNULL(chargetable.SecondaryBilledAmount,0) + ISNULL(chargetable.TertiaryBilledAmount,0)) as BilledAmount 

FROM Charge chargetable 
join  Patient patienttable on chargetable.PatientID = patienttable.ID 
join Visit v on chargetable.VisitID = v.ID 
join Practice practicetable on chargetable.PracticeID = practicetable.ID 
join [Provider] providertable on chargetable.ProviderID =  providertable.ID 
join [Location] locationtable on chargetable.LocationID = locationtable.ID 
join PatientPlan patientplantable on chargetable.PrimaryPatientPlanID = patientplantable.ID 
join InsurancePlan insurangeplantable on patientplantable.InsurancePlanID = insurangeplantable.ID 
join Cpt cpt on chargetable.CPTID = cpt.ID 
left join  RefProvider refprovidertable on chargetable.RefProviderID = refprovidertable.ID 
left join PatientPlan secpatientplantable on chargetable.SecondaryPatientPlanID = secpatientplantable.ID 
left join PatientPlan terpatientplantable on chargetable.TertiaryPatientPlanID = terpatientplantable.ID 
left join InsurancePlan secinsuranceplantable on secpatientplantable.InsurancePlanID = secinsuranceplantable.ID 
left join InsurancePlan terinsuranceplantable on terpatientplantable.InsurancePlanID = terinsuranceplantable.ID  
left join PatientPlan otherpatientplantable on chargetable.TertiaryPatientPlanID =otherpatientplantable.ID
left join InsurancePlan otherinsurancetable on otherpatientplantable.InsurancePlanID =otherinsurancetable.ID

left join POS postable on chargetable.POSID = postable.ID

left join Modifier m1t on chargetable.Modifier1ID = m1t.ID
left join Modifier m2t on chargetable.Modifier2ID = m2t.ID
left join ICD icd1t on v.ICD1ID = icd1t.ID
left join ICD icd2t on v.ICD2ID = icd2t.ID
left join ICD icd3t on v.ICD3ID = icd3t.ID
left join ICD icd4t on v.ICD4ID = icd4t.ID

where

'true'=CASE WHEN @ProviderID IS NULL THEN 'true' 
			else  case 	when chargetable.ProviderID = @ProviderID then 'true' else 'false' end
	   END

	   AND 
	   'true'=CASE WHEN @CPTCode IS NULL THEN 'true' 
			else  case when cpt.CPTCode = @CPTCode then 'true' else 'false' end
	   END
	    
	   




  AND 
	   'true'=CASE WHEN @PatientName IS NULL 
			THEN 'true' 
			else  case 
					when  (patienttable.LastName + ' ' + patienttable.FirstName) like '%'+@PatientName+'%'
					then 'true' 
					else 'false'  
				  end
	   END
	    AND 
	   'true'=CASE WHEN @PrescribingMD IS NULL 
			THEN 'true' 
			else  case 
					when v.PrescribingMD = @PrescribingMD 
					then 'true' 
					else 'false'  
				  end
	   END

	    AND 
	   'true'=CASE 
			   WHEN  (@EntryDateFrom IS NOT NULL AND @EntryDateTo IS NOT NULL )
					THEN  
						case 
							when (v.AddedDate  >= CONVERT(DATE, @EntryDateFrom)      and v.AddedDate  < CONVERT(DATE,  DATEADD(day, 1, @EntryDateTo) )   )
								then 'true' 
								else 'false' 
								end
			 WHEN  (@EntryDateFrom IS NOT NULL AND @EntryDateTo IS NULL )
					THEN  
						case 
							when (v.AddedDate  >= CONVERT(DATE, @EntryDateFrom)  )
								then 'true' 
								else 'false' 
								end
			 WHEN  (@EntryDateFrom IS NULL AND @EntryDateTo IS NOT NULL )
					THEN  
						case 
							when (v.AddedDate  <= CONVERT(DATE, @EntryDateTo)    )
								then 'true' 
								else 'false' 
								end
				WHEN  (@EntryDateFrom IS NULL AND @EntryDateTo IS NULL )
					then 'true' else 'true' 
	   END


	   
	    AND 
	   'true'=CASE 
			   WHEN  (@DateOfServiceFrom IS NOT NULL AND @DateOfServiceTo IS NOT NULL )
					THEN  
						case 
							when (v.DateOfServiceFrom  >= CONVERT(DATE, @DateOfServiceFrom)      and v.DateOfServiceFrom  < CONVERT(DATE,  DATEADD(day, 1, @DateOfServiceTo) )   )
								then 'true' 
								else 'false' 
								end
			 WHEN  (@DateOfServiceFrom IS NOT NULL AND @DateOfServiceTo IS NULL )
					THEN  
						case 
							when (v.DateOfServiceFrom  >= CONVERT(DATE, @DateOfServiceFrom)  )
								then 'true' 
								else 'false' 
								end
			 WHEN  (@DateOfServiceFrom IS NULL AND @DateOfServiceTo IS NOT NULL )
					THEN  
						case 
							when (v.DateOfServiceFrom  <= CONVERT(DATE, @DateOfServiceTo)    )
								then 'true' 
								else 'false' 
								end
				WHEN  (@DateOfServiceFrom IS NULL AND @DateOfServiceTo IS NULL )
					then 'true' else 'true' 
				END



				AND 
	   'true'=CASE 
			   WHEN  (@SubmittedDateFrom IS NOT NULL AND @SubmittedDateTo IS NOT NULL )
					THEN  
						case 
							when (v.SubmittedDate  >= CONVERT(DATE, @SubmittedDateFrom)      and v.SubmittedDate  < CONVERT(DATE,  DATEADD(day, 1, @SubmittedDateTo) )   )
								then 'true' 
								else 'false' 
								end
			 WHEN  (@SubmittedDateFrom IS NOT NULL AND @SubmittedDateTo IS NULL )
					THEN  
						case 
							when (v.SubmittedDate  >= CONVERT(DATE, @SubmittedDateFrom)  )
								then 'true' 
								else 'false' 
								end
			 WHEN  (@SubmittedDateFrom IS NULL AND @SubmittedDateTo IS NOT NULL )
					THEN  
						case 
							when (v.SubmittedDate  <= CONVERT(DATE, @SubmittedDateTo)    )
								then 'true' 
								else 'false' 
								end
				WHEN  (@SubmittedDateFrom IS NULL AND @SubmittedDateTo IS NULL )
					then 'true' else 'true' 
				END


	 --@SubmittedDateTo datetime = null,@SubmittedDateFrom datetime = null,

GROUP BY
chargetable.PatientID , chargetable.VisitID , chargetable.SubmittetdDate ,patienttable.FirstName ,
patienttable.LastName , patienttable.MiddleInitial , patienttable.Gender ,patienttable.SSN , Cpt.CPTCode ,
Cpt.[Description] , patienttable.DOB , chargetable.DateOfServiceFrom , icd1t.ICDCode , icd2t.ICDCode ,
icd3t.ICDCode , icd4t.ICDCode , m1t.Code , m2t.Code , postable.PosCode , providertable.[Name] , postable.[Name] ,
providertable.NPI , insurangeplantable.PlanName , patientplantable.SubscriberId , secinsuranceplantable.PlanName ,
secpatientplantable.SubscriberId ,otherinsurancetable.PlanName ,otherpatientplantable.SubscriberId ,
refprovidertable.[Name] ,chargetable.TotalAmount, cpt.ShortDescription, chargetable.AddedDate,chargetable.PrimaryPaymentDate,
chargetable.SecondaryPaymentDate, insurangeplantable.PlanName, insurangeplantable.[Description],
chargetable.PrimaryPaymentDate, v.PrescribingMD ,providertable.[Name]
having 
'true'=CASE WHEN @PaymentCriteria IS NULL 
			THEN 'true' 
			else  case 
					when 'Paid' = @PaymentCriteria 
					then case when SUM(ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) > 0 
								then 'true'
								else 'false' end
					else 'true'  
				  end
	   END
	      AND 
	   'true'=CASE WHEN @PaymentCriteria IS NULL 
			THEN 'true' 
			else  case 
					when 'PartialPaid' = @PaymentCriteria 
					then case when SUM(ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) > 0 
								then 'true'
								else 'false' end
					else 'true'  
				  end
	   END
	     AND 
	   'true'=CASE WHEN @PaymentCriteria IS NULL 
			THEN 'true' 
			else  case 
					when 'UnPaid' = @PaymentCriteria 
					then case when SUM(ISNULL(chargetable.PrimaryPaid,0) + ISNULL(chargetable.SecondaryPaid,0) + ISNULL(chargetable.TertiaryPaid,0)) = 0 
								then 'true'
								else 'false' end
					else 'true'  
				  end
	   END

	    AND 
	   'true'=CASE WHEN @PaymentCriteria IS NULL 
			THEN 'true' 
			else  case 
					when 'PatientBal' = @PaymentCriteria 
					then case when SUM(ISNULL(chargetable.PrimaryPatientBal,0) + ISNULL(chargetable.SecondaryPatientBal,0) + ISNULL(chargetable.TertiaryPatientBal,0)) > 0 
								then 'true'
								else 'false' end
					else 'true'  
				  end
	   END

END









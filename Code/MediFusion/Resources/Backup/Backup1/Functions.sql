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


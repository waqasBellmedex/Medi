Use MedifusionWaqas
GO
--=====================================
--Description: <Client Trigger>
--======================================

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrClientAudit')
DROP TRIGGER [dbo].[tgrClientAudit]
GO
Create TRIGGER [dbo].[tgrClientAudit]
ON [dbo].[Client] after Update
AS 
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM ClientAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM ClientAudit  

--when update Name
if update(Name)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',d.Name,i.Name,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Name,'') != ISNULL(d.Name,'')

--when update OrganizationName
if update(OrganizationName)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Organization Name',d.OrganizationName,i.OrganizationName,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.OrganizationName,'') != ISNULL(d.OrganizationName,'')

--when update TaxID
if update(TaxID)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Tax ID',d.TaxID,i.TaxID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.TaxID,'') != ISNULL(d.TaxID,'')

--when update ServiceLocation
if update(ServiceLocation)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Service Location',d.ServiceLocation,i.ServiceLocation,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ServiceLocation,'') != ISNULL(d.ServiceLocation,'')

--when update Address
if update(Address)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address',d.Address,i.Address,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Address,'') != ISNULL(d.Address,'')

--when update State
if update(State)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'State',d.State,i.State,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.State,'') != ISNULL(d.State,'')

--when update City
if update(City)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'City',d.City,i.City,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.City,'') != ISNULL(d.City,'')

--when update ZipCode
if update(ZipCode)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Zip Code',d.ZipCode,i.ZipCode,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ZipCode,'') != ISNULL(d.ZipCode,'')

--when update OfficeHour
if update(OfficeHour)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Office Hour',d.OfficeHour,i.OfficeHour,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.OfficeHour,'') != ISNULL(d.OfficeHour,'')

--when update FaxNo
if update(FaxNo)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Fax Number',d.FaxNo,i.FaxNo,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FaxNo,'') != ISNULL(d.FaxNo,'')

--when update OfficePhoneNo
if update(OfficePhoneNo)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Office Phone Number',d.OfficePhoneNo,i.OfficePhoneNo,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.OfficePhoneNo,'') != ISNULL(d.OfficePhoneNo,'')

--when update OfficeEmail
if update(OfficeEmail)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Office Email',d.OfficeEmail,i.OfficeEmail,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.OfficeEmail,'') != ISNULL(d.OfficeEmail,'')

--when update ContactPerson
if update(ContactPerson)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Contact Person',d.ContactPerson,i.ContactPerson,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ContactPerson,'') != ISNULL(d.ContactPerson,'')

--when update ContactNo
if update(ContactNo)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Contact Number',d.ContactNo,i.ContactNo,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ContactNo,'') != ISNULL(d.ContactNo,'')

END
GO
--=====================================
--Description: <Cpt Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrCptAudit')
DROP TRIGGER [dbo].[tgrCptAudit]
GO
Create TRIGGER [dbo].[tgrCptAudit]
ON [dbo].[Cpt] after Update
AS 
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM CptAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM CptAudit  

--when update Description
if update(Description)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Description',d.Description,i.Description,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Description,'') != ISNULL(d.Description,'')

--when update CPTCode
if update(CPTCode)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'CPT Code',d.CPTCode,i.CPTCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CPTCode,'') != ISNULL(d.CPTCode,'')

--when update Amount
if update(Amount)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Amount',d.Amount,i.Amount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Amount,'1') != ISNULL(d.Amount,'1')

--when update Modifier1ID
if update(Modifier1ID)
Select * into #t from modifier
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Modifier1 ID',cDel.[Description],cUpd.[Description],d.Modifier1ID,i.Modifier1ID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Modifier1ID,'') != ISNULL(d.Modifier1ID,'')
Join modifier as cDel on cDel.ID = ISNULL(d.Modifier1ID,'')
join modifier as cUpd on cUpd.ID = ISNULL(i.Modifier1ID,'')

--when update Modifier2ID
if update(Modifier2ID)
Select * into #t1 from modifier
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Modifier2 ID',cDel.[Description],cUpd.[Description],d.Modifier2ID,i.Modifier2ID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Modifier2ID,'') != ISNULL(d.Modifier2ID,'')
Join modifier as cDel on cDel.ID = ISNULL(d.Modifier2ID,'')
join modifier as cUpd on cUpd.ID = ISNULL(i.Modifier2ID,'')

--when update Modifier3ID
if update(Modifier3ID)
Select * into #t3 from modifier
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Modifier3 ID',cDel.[Description],cUpd.[Description],d.Modifier3ID,i.Modifier3ID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Modifier3ID,'') != ISNULL(d.Modifier3ID,'')
Join modifier as cDel on cDel.ID = ISNULL(d.Modifier3ID,'')
join modifier as cUpd on cUpd.ID = ISNULL(i.Modifier3ID,'')

--when update Modifier4ID
if update(Modifier4ID)
Select * into #t4 from modifier
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Modifier4 ID',cDel.[Description],cUpd.[Description],d.Modifier4ID,i.Modifier4ID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Modifier4ID,'') != ISNULL(d.Modifier4ID,'')
Join modifier as cDel on cDel.ID = ISNULL(d.Modifier4ID,'')
join modifier as cUpd on cUpd.ID = ISNULL(i.Modifier4ID,'')

--when update TypeOfServiceID
if update(TypeOfServiceID)
Select * into #t5 from TypeOfService
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Type Of Service ID',cDel.[Code],cUpd.[Code],d.TypeOfServiceID,i.TypeOfServiceID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.TypeOfServiceID,'') != ISNULL(d.TypeOfServiceID,'')
Join TypeOfService as cDel on cDel.ID = ISNULL(d.TypeOfServiceID,'')
join TypeOfService as cUpd on cUpd.ID = ISNULL(i.TypeOfServiceID,'')

--when update Category
if update(Category)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Category',d.Category,i.Category,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Category,'') != ISNULL(d.Category,'')

--when update DefaultUnits
if update(DefaultUnits)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Default Units',d.DefaultUnits,i.DefaultUnits,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DefaultUnits,'') != ISNULL(d.DefaultUnits,'')

--when update UnitOfMeasurement
if update(UnitOfMeasurement)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Unit Of Measurement',d.UnitOfMeasurement,i.UnitOfMeasurement,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.UnitOfMeasurement,'') != ISNULL(d.UnitOfMeasurement,'')

--when update CLIANumber
if update(CLIANumber)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'CLIA Number',d.CLIANumber,i.CLIANumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CLIANumber,'') != ISNULL(d.CLIANumber,'')

--when update NDCNumber
if update(NDCNumber)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'NDC Number',d.NDCNumber,i.NDCNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.NDCNumber,'') != ISNULL(d.NDCNumber,'')

--when update NDCDescription
if update(NDCDescription)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'NDC Description',d.NDCDescription,i.NDCDescription,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.NDCDescription,'') != ISNULL(d.NDCDescription,'')

--when update NDCUnits
if update(NDCUnits)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'NDC Units',d.NDCUnits,i.NDCUnits,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.NDCUnits,'') != ISNULL(d.NDCUnits,'')

--when update NDCUnitOfMeasurement
if update(NDCUnitOfMeasurement)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'NDC Unit Of Measurement',d.NDCUnitOfMeasurement,i.NDCUnitOfMeasurement,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.NDCUnitOfMeasurement,'') != ISNULL(d.NDCUnitOfMeasurement,'')

--when update IsValid
if update(IsValid)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsValid',(case when d.IsValid = 1 then 'Yes' else 'No' end),(case when i.IsValid = 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsValid,'') != ISNULL(d.IsValid,'')

--when update IsActive
if update(IsActive)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsActive',d.IsActive,i.IsActive,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsActive,'') != ISNULL(d.IsActive,'')

--when update IsDeleted
if update(IsDeleted)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsDeleted',d.IsDeleted,i.IsDeleted,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsDeleted,'') != ISNULL(d.IsDeleted,'')

--when update AnesthesiaBaseUnits
if update(AnesthesiaBaseUnits)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Anesthesia Base Units',d.AnesthesiaBaseUnits,i.AnesthesiaBaseUnits,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AnesthesiaBaseUnits,'') != ISNULL(d.AnesthesiaBaseUnits,'')
END
GO


--=====================================
--Description: <InsurancePlan Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrInsurancePlanAudit')
DROP TRIGGER [dbo].[tgrInsurancePlanAudit]
GO
Create TRIGGER [dbo].[tgrInsurancePlanAudit]
ON [dbo].[InsurancePlan] after Update
AS 
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM InsurancePlanAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM InsurancePlanAudit  

--when update PlanName
if update(PlanName)
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer Name',d.PlanName,i.PlanName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PlanName,'') != ISNULL(d.PlanName,'')

-- when update Description
if update(Description)
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Description',d.Description,i.Description,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Description,'') != ISNULL(d.Description,'')

--when update InsuranceID
if update(InsuranceID)
Select * into #t from Insurance
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Receiver ID',cDel.[Name],cUpd.[Name],d.InsuranceID,i.InsuranceID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsuranceID,'') != ISNULL(d.InsuranceID,'')
Join Insurance as cDel on cDel.ID = ISNULL(d.InsuranceID,'')
join Insurance as cUpd on cUpd.ID = ISNULL(i.InsuranceID,'')

--when update PlanTypeID
if update(PlanTypeID)
Select * into #t1 from PlanType
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Plan Type ID',cDel.[Code],cUpd.[Code],d.PlanTypeID,i.PlanTypeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PlanTypeID,'') != ISNULL(d.PlanTypeID,'')
Join PlanType as cDel on cDel.ID = ISNULL(d.PlanTypeID,'')
join PlanType as cUpd on cUpd.ID = ISNULL(i.PlanTypeID,'')

--when update IsCapitated
if update(IsCapitated)
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsCapitated',(case when d.IsCapitated = 1 then 'Yes' else 'No' end),(case when i.IsCapitated = 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsCapitated,'') != ISNULL(d.IsCapitated,'')

-- when update SubmissionType
if update(SubmissionType)
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submission Type',d.SubmissionType,i.SubmissionType,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmissionType,'') != ISNULL(d.SubmissionType,'')

--when update PayerID
if update(PayerID)
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer ID',d.PayerID,i.PayerID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PayerID,'') != ISNULL(d.PayerID,'')

--when update Edi837PayerID
if update(Edi837PayerID)
Select * into #t2 from Edi837Payer
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Edi837 Payer ID',cDel.[PayerName],cUpd.[PayerName],d.Edi837PayerID,i.Edi837PayerID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Edi837PayerID,'') != ISNULL(d.Edi837PayerID,'')
Join Edi837Payer as cDel on cDel.ID = ISNULL(d.Edi837PayerID,'')
join Edi837Payer as cUpd on cUpd.ID = ISNULL(i.Edi837PayerID,'')

--when update Edi270PayerID
if update(Edi270PayerID)
Select * into #t3 from Edi270Payer
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Edi270 Payer ID',cDel.[PayerName],cUpd.[PayerName],d.Edi270PayerID,i.Edi270PayerID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Edi270PayerID,'') != ISNULL(d.Edi270PayerID,'')
Join Edi270Payer as cDel on cDel.ID = ISNULL(d.Edi270PayerID,'')
join Edi270Payer as cUpd on cUpd.ID = ISNULL(i.Edi270PayerID,'')

--when update Edi276PayerID
if update(Edi276PayerID)
Select * into #t4 from Edi276Payer
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Edi276 Payer ID',cDel.[PayerName],cUpd.[PayerName],d.Edi276PayerID,i.Edi276PayerID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Edi276PayerID,'') != ISNULL(d.Edi276PayerID,'')
Join Edi276Payer as cDel on cDel.ID = ISNULL(d.Edi276PayerID,'')
join Edi276Payer as cUpd on cUpd.ID = ISNULL(i.Edi276PayerID,'')

-- when update FormType
if update(FormType)
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Form Type',d.FormType,i.FormType,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FormType,'') != ISNULL(d.FormType,'')

--when update OutstandingDays
if update(OutstandingDays)
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Outstanding Days',d.OutstandingDays,i.OutstandingDays,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.OutstandingDays,'') != ISNULL(d.OutstandingDays,'')

-- when update IsActive
if update(IsActive)
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsActive',d.IsActive,i.IsActive,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsActive,'') != ISNULL(d.IsActive,'')

--when update IsDeleted
if update(IsDeleted)
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsDeleted',d.IsDeleted,i.IsDeleted,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsDeleted,'') != ISNULL(d.IsDeleted,'')

-- when update Notes
if update(Notes)
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',d.Notes,i.Notes,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Notes,'') != ISNULL(d.Notes,'')
END
GO
--=====================================
--Description: <tgrEdi837Payer Trigger>
--======================================
IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrEdi837PayerAudit')
DROP TRIGGER [dbo].[tgrEdi837PayerAudit]
GO
Create TRIGGER [dbo].[tgrEdi837PayerAudit]
ON [dbo].[Edi837Payer] after Update
AS 
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM Edi837PayerAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM Edi837PayerAudit  

--when update PayerName
if update(PayerName)
insert into Edi837PayerAudit(Edi837PayerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer Name',d.PayerName,i.PayerName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PayerName,'') != ISNULL(d.PayerName,'')

--when update ReceiverID
if update(ReceiverID)
Select * into #t from Receiver
insert into Edi837PayerAudit(Edi837PayerID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Receiver ID',cDel.[Name],cUpd.[Name],d.ReceiverID,i.ReceiverID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReceiverID,'') != ISNULL(d.ReceiverID,'')
Join Receiver as cDel on cDel.ID = ISNULL(d.ReceiverID,'')
join Receiver as cUpd on cUpd.ID = ISNULL(i.ReceiverID,'')

-- when update PayerID
if update(PayerID)
insert into Edi837PayerAudit(Edi837PayerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer ID',d.PayerID,i.PayerID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PayerID,'') != ISNULL(d.PayerID,'')
END
GO
--=====================================
--Description: <tgrEdi276Payer Trigger>
--======================================


IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrEdi276PayerAudit')
DROP TRIGGER [dbo].[tgrEdi276PayerAudit]
GO
Create TRIGGER [dbo].[tgrEdi276PayerAudit]
ON [dbo].[Edi276Payer] after Update
AS 
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM Edi276PayerAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM Edi276PayerAudit  

--when update PayerName
if update(PayerName)
insert into Edi276PayerAudit(Edi276PayerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer Name',d.PayerName,i.PayerName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PayerName,'') != ISNULL(d.PayerName,'')

--when update ReceiverID
if update(ReceiverID)
Select * into #t from Receiver
insert into Edi276PayerAudit(Edi276PayerID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Receiver ID',cDel.[Name],cUpd.[Name],d.ReceiverID,i.ReceiverID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReceiverID,'') != ISNULL(d.ReceiverID,'')
Join Receiver as cDel on cDel.ID = ISNULL(d.ReceiverID,'')
join Receiver as cUpd on cUpd.ID = ISNULL(i.ReceiverID,'')

-- when update PayerID
if update(PayerID)
insert into Edi276PayerAudit(Edi276PayerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer ID',d.PayerID,i.PayerID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PayerID,'') != ISNULL(d.PayerID,'')
END
GO
--=====================================
--Description: <tgrEdi270Payer Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrEdi270PayerAudit')
DROP TRIGGER [dbo].[tgrEdi270PayerAudit]
GO
Create TRIGGER [dbo].[tgrEdi270PayerAudit]
ON [dbo].[Edi270Payer] after Update
AS 
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM Edi270PayerAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM Edi270PayerAudit  

--when update PayerName
if update(PayerName)
insert into Edi270PayerAudit(Edi270PayerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer Name',d.PayerName,i.PayerName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PayerName,'') != ISNULL(d.PayerName,'')

--when update ReceiverID
if update(ReceiverID)
Select * into #t from Receiver
insert into Edi270PayerAudit(Edi270PayerID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Receiver ID',cDel.[Name],cUpd.[Name],d.ReceiverID,i.ReceiverID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReceiverID,'') != ISNULL(d.ReceiverID,'')
Join Receiver as cDel on cDel.ID = ISNULL(d.ReceiverID,'')
join Receiver as cUpd on cUpd.ID = ISNULL(i.ReceiverID,'')

-- when update PayerID
if update(PayerID)
insert into Edi270PayerAudit(Edi270PayerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer ID',d.PayerID,i.PayerID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PayerID,'') != ISNULL(d.PayerID,'')
END
GO


--=====================================
--Description: <tgrInsurance Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrInsuranceAudit')
DROP TRIGGER [dbo].[tgrInsuranceAudit]
GO
Create TRIGGER [dbo].[tgrInsuranceAudit]
on [dbo].[Insurance] after update
AS 
BEGIN 
Declare @TransactionID int;
select @TransactionID = MAX(TransactionID) from InsuranceAudit
if @TransactionID is null set @TransactionID = 1
else select @TransactionID = Max(TransactionID) + 1 from InsuranceAudit

--when update Name
if update(Name)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',d.Name,i.Name,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Name,'') != ISNULL(d.Name,'')

--when update OrganizationName
if update(OrganizationName)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Organization Name',d.OrganizationName,i.OrganizationName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.OrganizationName,'') != ISNULL(d.OrganizationName,'')

--when update Address1
if update(Address1)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address1',d.Address1,i.Address1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Address1,'') != ISNULL(d.Address1,'')

--when update Address2
if update(Address2)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address2',d.Address2,i.Address2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Address2,'') != ISNULL(d.Address2,'')

--when update City
if update(City)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'City',d.City,i.City,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.City,'') != ISNULL(d.City,'')

--when update State
if update(State)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'State',d.State,i.State,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.State,'') != ISNULL(d.State,'')

--when update ZipCode
if update(ZipCode)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Zip Code',d.ZipCode,i.ZipCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ZipCode,'') != ISNULL(d.ZipCode,'')

--when update OfficePhoneNum
if update(OfficePhoneNum)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Office Phone Number',d.OfficePhoneNum,i.OfficePhoneNum,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.OfficePhoneNum,'') != ISNULL(d.OfficePhoneNum,'')

--when update Email
if update(Email)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Email',d.Email,i.Email,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Email,'') != ISNULL(d.Email,'')

--when update Website
if update(Website)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Website',d.Website,i.Website,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Website,'') != ISNULL(d.Website,'')

--when update FaxNumber
if update(FaxNumber)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Fax Number',d.FaxNumber,i.FaxNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FaxNumber,'') != ISNULL(d.FaxNumber,'')

--when update IsActive
if update(IsActive)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsActive',d.IsActive,i.IsActive,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsActive,'') != ISNULL(d.IsActive,'')

--when update IsDeleted
if update(IsDeleted)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsDeleted',d.IsDeleted,i.IsDeleted,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsDeleted,'') != ISNULL(d.IsDeleted,'')

--when update Notes
if update(Notes)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',d.Notes,i.Notes,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Notes,'') != ISNULL(d.Notes,'')
END
GO


--=====================================
--Description: <tgrModifier Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrModifierAudit')
DROP TRIGGER [dbo].[tgrModifierAudit]
GO
Create TRIGGER [dbo].[tgrModifierAudit]
on [dbo].[modifier] after update
AS 
BEGIN 
Declare @TransactionID int;
select @TransactionID = MAX(TransactionID) from ModifierAudit
if @TransactionID is null set @TransactionID = 1
else select @TransactionID = Max(TransactionID) + 1 from ModifierAudit

--when update Code
IF Update(Code)
INSERT INTO ModifierAudit(ModifierID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Code',d.Code,i.Code,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Code,'') != ISNULL(d.Code,'')

--when update Description
IF Update(Description)
INSERT INTO ModifierAudit(ModifierID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Description',d.Description,i.Description,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Description,'') != ISNULL(d.Description,'')

--when update IsActive
IF Update(IsActive)
INSERT INTO ModifierAudit(ModifierID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'IsActive',d.IsActive,i.IsActive,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.IsActive,'') != ISNULL(d.IsActive,'')

--when update IsDeleted
IF Update(IsDeleted)
INSERT INTO ModifierAudit(ModifierID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'IsDeleted',d.IsDeleted,i.IsDeleted,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.IsDeleted,'') != ISNULL(d.IsDeleted,'')

--when update AnesthesiaBaseUnits
IF Update(AnesthesiaBaseUnits)
INSERT INTO ModifierAudit(ModifierID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Anesthesia Base Units',d.AnesthesiaBaseUnits,i.AnesthesiaBaseUnits,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.AnesthesiaBaseUnits,'') != ISNULL(d.AnesthesiaBaseUnits,'')

--when update DefaultFees
IF Update(DefaultFees)
INSERT INTO ModifierAudit(ModifierID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Default Fees',d.DefaultFees,i.DefaultFees,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.DefaultFees,'1') != ISNULL(d.DefaultFees,'1')
END 
GO

--=====================================
--Description: <tgrPlanType Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrPlanTypeAudit')
DROP TRIGGER [dbo].[tgrPlanTypeAudit]
GO
Create TRIGGER [dbo].[tgrPlanTypeAudit]
on [dbo].[PlanType] after update
AS 
BEGIN 
Declare @TransactionID int;
select @TransactionID = MAX(TransactionID) from PlanTypeAudit
if @TransactionID is null set @TransactionID = 1
else select @TransactionID = Max(TransactionID) + 1 from PlanTypeAudit

--when update Code
IF Update(Code)
INSERT INTO PlanTypeAudit(PlanTypeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Code',d.Code,i.Code,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Code,'') != ISNULL(d.Code,'')

--when update Description
IF Update(Description)
INSERT INTO PlanTypeAudit(PlanTypeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Description',d.Description,i.Description,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Description,'') != ISNULL(d.Description,'')
END
GO

--=====================================
--Description: <tgrPractice Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrPracticeAudit')
DROP TRIGGER [dbo].[tgrPracticeAudit]
GO
Create TRIGGER [dbo].[tgrPracticeAudit]
ON [dbo].[Practice] after Update
AS 
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM PracticeAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM PracticeAudit  

--when update Name
if update(Name)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',d.Name,i.Name,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Name,'') != ISNULL(d.Name,'')

--when update ClientID
if update(ClientID)
Select * into #t from Client
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Client ID',cDel.[Name],cUpd.[Name],d.ClientID,i.ClientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClientID,'') != ISNULL(d.ClientID,'')
Join Client as cDel on cDel.ID = ISNULL(d.ClientID,'')
join Client as cUpd on cUpd.ID = ISNULL(i.ClientID,'')

-- when update OragnizationName
if update(OrganizationName)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Organization Name',d.OrganizationName,i.OrganizationName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.OrganizationName,'') != ISNULL(d.OrganizationName,'')

-- when update TaxID
if update(TaxID)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Tax ID',d.TaxID,i.TaxID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.TaxID,'') != ISNULL(d.TaxID,'')

-- when update CLIANumber
if update(CLIANumber)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'CLIA Number',d.CLIANumber,i.CLIANumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CLIANumber,'') != ISNULL(d.CLIANumber,'')

-- when update NPI
if update(NPI)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'NPI',d.NPI,i.NPI,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.NPI,'') != ISNULL(d.NPI,'')

-- when update SSN
if update(SSN)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'SSN',d.SSN,i.SSN,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SSN,'') != ISNULL(d.SSN,'')

-- when update Type
if update(Type)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Type',d.Type,i.Type,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Type,'') != ISNULL(d.Type,'')

-- when update TaxonomyCode
if update(TaxonomyCode)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Taxonomy Code',d.TaxonomyCode,i.TaxonomyCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.TaxonomyCode,'') != ISNULL(d.TaxonomyCode,'')

-- when update Address1
if update(Address1)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address1',d.Address1,i.Address1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Address1,'') != ISNULL(d.Address1,'')

-- when update Address2
if update(Address2)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address2',d.Address2,i.Address2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Address2,'') != ISNULL(d.Address2,'')

-- when update City
if update(City)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'City',d.City,i.City,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.City,'') != ISNULL(d.City,'')

-- when update State
if update(State)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'State',d.State,i.State,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.State,'') != ISNULL(d.State,'')

-- when update ZipCode
if update(ZipCode)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Zip Code',d.ZipCode,i.ZipCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ZipCode,'') != ISNULL(d.ZipCode,'')

-- when update OfficePhoneNum
if update(City)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Office Phone Number',d.OfficePhoneNum,i.OfficePhoneNum,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.OfficePhoneNum,'') != ISNULL(d.OfficePhoneNum,'')

-- when update FaxNumber
if update(FaxNumber)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Fax Number',d.FaxNumber,i.FaxNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FaxNumber,'') != ISNULL(d.FaxNumber,'')

-- when update Email
if update(Email)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Email',d.Email,i.Email,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Email,'') != ISNULL(d.Email,'')

-- when update Website
if update(Website)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Website',d.Website,i.Website,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Website,'') != ISNULL(d.Website,'')

-- when update PayToAddress1
if update(PayToAddress1)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Pay To Address1',d.PayToAddress1,i.PayToAddress1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PayToAddress1,'') != ISNULL(d.PayToAddress1,'')

-- when update PayToAddress2
if update(PayToAddress2)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Pay To Address2',d.PayToAddress2,i.PayToAddress2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PayToAddress2,'') != ISNULL(d.PayToAddress2,'')

-- when update PayToCity
if update(PayToCity)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Pay To City',d.PayToCity,i.PayToCity,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PayToCity,'') != ISNULL(d.PayToCity,'')

-- when update PayToState
if update(PayToState)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Pay To State',d.PayToState,i.PayToState,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PayToState,'') != ISNULL(d.PayToState,'')

-- when update PayToZipCode
if update(PayToZipCode)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Pay To Zip Code',d.PayToZipCode,i.PayToZipCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PayToZipCode,'') != ISNULL(d.PayToZipCode,'')

-- when update DefaultLocationID
if update(DefaultLocationID)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Default Location ID',d.DefaultLocationID,i.DefaultLocationID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DefaultLocationID,'') != ISNULL(d.DefaultLocationID,'')

-- when update WorkingHours
if update(WorkingHours)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Working Hours',d.WorkingHours,i.WorkingHours,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.WorkingHours,'') != ISNULL(d.WorkingHours,'')

-- when update Notes
if update(Notes)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',d.Notes,i.Notes,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Notes,'') != ISNULL(d.Notes,'')

-- when update IsActive
if update(IsActive)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsActive',d.IsActive,i.IsActive,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsActive,'') != ISNULL(d.IsActive,'')

-- when update IsDeleted
if update(IsDeleted)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsDeleted',d.IsDeleted,i.IsDeleted,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsDeleted,'') != ISNULL(d.IsDeleted,'')

----When User Update UserPracticesUserID
--IF Update(UserPracticesUserID)
--INSERT INTO PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
--SELECT  i.ID, @TransactionID,'UserPracticesUserID',pDel.UserID,pUpd.UserID,d.UserPracticesUserID,i.UserPracticesUserID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
--FROM inserted as i
--INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.UserPracticesUserID,'') != ISNULL(d.UserPracticesUserID,'')
--Join UserPractices as pDel on pdel.UserID = ISNULL(d.UserPracticesUserID,'')
--join UserPractices as pUpd on pUpd.UserID = ISNULL(i.UserPracticesUserID,'')

----When User Update UserPracticesPracticeID
--IF Update(UserPracticesPracticeID)
--INSERT INTO PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
--SELECT  i.ID, @TransactionID,'UserPracticesPracticeID',pDel.PracticeID,pUpd.PracticeID,d.UserPracticesPracticeID,i.UserPracticesPracticeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
--FROM inserted as i
--INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.UserPracticesPracticeID,'') != ISNULL(d.UserPracticesPracticeID,'1')
--Join UserPractices as pDel on pdel.PracticeID = ISNULL(d.UserPracticesPracticeID,'')
--join UserPractices as pUpd on pUpd.PracticeID = ISNULL(i.UserPracticesPracticeID,'')

END
GO

--=====================================
--Description: <tgrProvider Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrProviderAudit')
DROP TRIGGER [dbo].[tgrProviderAudit]
GO
Create TRIGGER [dbo].[tgrProviderAudit]
ON [dbo].[Provider] after Update
AS 
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM ProviderAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM ProviderAudit  

--when update Name
if update(Name)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',d.Name,i.Name,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Name,'') != ISNULL(d.Name,'')

--when update LastName
if update(LastName)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Last Name',d.LastName,i.LastName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.LastName,'') != ISNULL(d.LastName,'')

--when update FirstName
if update(FirstName)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'First Name',d.FirstName,i.FirstName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FirstName,'') != ISNULL(d.FirstName,'')

--when update MiddleInitial
if update(MiddleInitial)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Middle Initial',d.MiddleInitial,i.MiddleInitial,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.MiddleInitial,'') != ISNULL(d.MiddleInitial,'')

--when update Title
if update(Title)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Title',d.Title,i.Title,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Title,'') != ISNULL(d.Title,'')

--when update NPI
if update(NPI)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'NPI',d.NPI,i.NPI,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.NPI,'') != ISNULL(d.NPI,'')

--when update SSN
if update(SSN)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'SSN',d.SSN,i.SSN,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SSN,'') != ISNULL(d.SSN,'')

--when update TaxonomyCode
if update(TaxonomyCode)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Taxonomy Code',d.TaxonomyCode,i.TaxonomyCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.TaxonomyCode,'') != ISNULL(d.TaxonomyCode,'')

--when update Address1
if update(Address1)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address1',d.Address1,i.Address1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Address1,'') != ISNULL(d.Address1,'')

--when update Address2
if update(Address2)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address2',d.Address2,i.Address2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Address2,'') != ISNULL(d.Address2,'')

--when update City
if update(City)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'City',d.City,i.City,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.City,'') != ISNULL(d.City,'')

--when update State
if update(State)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'State',d.State,i.State,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.State,'') != ISNULL(d.State,'')

--when update ZipCode
if update(ZipCode)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'ZipCode',d.ZipCode,i.ZipCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ZipCode,'') != ISNULL(d.ZipCode,'')

--when update OfficePhoneNum
if update(OfficePhoneNum)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Office Phone Number',d.OfficePhoneNum,i.OfficePhoneNum,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.OfficePhoneNum,'') != ISNULL(d.OfficePhoneNum,'')

--when update FaxNumber
if update(FaxNumber)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Fax Number',d.FaxNumber,i.FaxNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FaxNumber,'') != ISNULL(d.FaxNumber,'')

----when update PracticeID
if update(PracticeID)
select * into #t from practice
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Practice ID',cDel.[Name],cUpd.[Name],d.PracticeID,i.PracticeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PracticeID,'') != ISNULL(d.PracticeID,'')
Join Practice as cDel on cDel.ID = ISNULL(d.PracticeID,'')
join Practice as cUpd on cUpd.ID = ISNULL(i.PracticeID,'')

--when update BillUnderProvider
if update(BillUnderProvider)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Bill Under Provider',(case when d.BillUnderProvider = 1 then 'Yes' else 'No' end),(case when i.BillUnderProvider = 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.BillUnderProvider,'') != ISNULL(d.BillUnderProvider,'')

--when update Email
if update(Email)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Email',d.Email,i.Email,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Email,'') != ISNULL(d.Email,'')

--when update DEANumber
if update(DEANumber)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'DEA Number',d.DEANumber,i.DEANumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DEANumber,'') != ISNULL(d.DEANumber,'')

--when update UPINNumber
if update(UPINNumber)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'UPIN Number',d.UPINNumber,i.UPINNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.UPINNumber,'') !=ISNULL(d.UPINNumber,'')

--when update LicenceNumber
if update(LicenceNumber)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Licence Number',d.LicenceNumber,i.LicenceNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.LicenceNumber,'') != ISNULL(d.LicenceNumber,'')

--when update IsActive
if update(IsActive)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsActive',d.IsActive,i.IsActive,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsActive,'') != ISNULL(d.IsActive,'')

--when update IsDeleted
if update(IsDeleted)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsDeleted',d.IsDeleted,i.IsDeleted,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsDeleted,'') != ISNULL(d.IsDeleted,'')

--when update Notes
if update(Notes)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',d.Notes,i.Notes,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Notes,'') != ISNULL(d.Notes,'')
END
GO


--=====================================
--Description: <tgrTypeOfService Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrTypeOfServiceAudit')
DROP TRIGGER [dbo].[tgrTypeOfServiceAudit]
GO
Create TRIGGER [dbo].[tgrTypeOfServiceAudit]
on [dbo].[TypeOfService] after update
AS 
BEGIN 
Declare @TransactionID int;
select @TransactionID = MAX(TransactionID) from TypeOfServiceAudit
if @TransactionID is null set @TransactionID = 1
else select @TransactionID = Max(TransactionID) + 1 from TypeOfServiceAudit

--when update(Code)
IF Update(Code)
INSERT INTO TypeOfServiceAudit(TypeOfServiceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Code',d.Code,i.Code,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Code,'') != ISNULL(d.Code,'')

--when update(Description)
if update(Description)
insert into TypeOfServiceAudit(TypeOfServiceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Description',d.Description,i.Description,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
from inserted as i 
inner join Deleted as d on i.ID = d.ID and ISNULL(i.Description,'') != ISNULL(d.Description,'')

--when update(IsActive)
if update(IsActive)
insert into TypeOfServiceAudit(TypeOfServiceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsActive',d.IsActive,i.IsActive,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
from inserted as i 
inner join Deleted as d on i.ID = d.ID and ISNULL(i.IsActive,'') != ISNULL(d.IsActive,'')

--when update(IsDeleted)
if update(IsDeleted)
insert into TypeOfServiceAudit(TypeOfServiceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'IsDeleted',d.IsDeleted,i.IsDeleted,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsDeleted,'') != ISNULL(d.IsDeleted,'')
END
GO
--=====================================
--Description: <tgrRefProvider Trigger>
--======================================
IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrRefProviderAudit')
DROP TRIGGER [dbo].[tgrRefProviderAudit]
GO
Create TRIGGER  [dbo].[tgrRefProviderAudit]
ON [dbo].[RefProvider] after Update
AS 
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM [dbo].[RefProviderAudit]
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM [dbo].[RefProviderAudit]

--when update Name
if update(Name)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',d.Name,i.Name,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Name,'') != ISNULL(d.Name,'')

--when update LastName
if update(LastName)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Last Name',d.LastName,i.LastName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.LastName,'') != ISNULL(d.LastName,'')

--when update FirstName
if update(FirstName)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'First Name',d.FirstName,i.FirstName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FirstName,'') != ISNULL(d.FirstName,'')

--when update MiddleInitial
if update(MiddleInitial)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Middle Initial',d.MiddleInitial,i.MiddleInitial,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.MiddleInitial,'') != ISNULL(d.MiddleInitial,'')

--when update Title
if update(Title)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Title',d.Title,i.Title,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Title,'') != ISNULL(d.Title,'')

--when update NPI
if update(NPI)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'NPI',d.NPI,i.NPI,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.NPI,'') != ISNULL(d.NPI,'')

--when update SSN
if update(SSN)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'SSN',d.SSN,i.SSN,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SSN,'') != ISNULL(d.SSN,'')

--when update TaxonomyCode
if update(TaxonomyCode)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Taxonomy Code',d.TaxonomyCode,i.TaxonomyCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.TaxonomyCode,'') != ISNULL(d.TaxonomyCode,'')

--when update TaxID
if update(TaxID)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Tax ID',d.TaxID,i.TaxID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.TaxID,'') != ISNULL(d.TaxID,'')

--when update Address1
if update(Address1)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address1',d.Address1,i.Address1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Address1,'') != ISNULL(d.Address1,'')

--when update Address2
if update(Address2)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address2',d.Address2,i.Address2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Address2,'') != ISNULL(d.Address2,'')

--when update City
if update(City)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'City',d.City,i.City,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.City,'') != ISNULL(d.City,'')

--when update State
if update(State)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'State',d.State,i.State,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.State,'') != ISNULL(d.State,'')

--when update ZipCode
if update(ZipCode)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Zip Code',d.ZipCode,i.ZipCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ZipCode,'') != ISNULL(d.ZipCode,'')

--when update OfficePhoneNum
if update(OfficePhoneNum)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Office Phone Number',d.OfficePhoneNum,i.OfficePhoneNum,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.OfficePhoneNum,'') != ISNULL(d.OfficePhoneNum,'')

--when update Faxnumber
if update(FaxNumber)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Fax Number',d.FaxNumber,i.FaxNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FaxNumber,'') != ISNULL(d.FaxNumber,'')

--when update PracticeID
if update (PracticeID)
Select * into #t from Practice
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
Select i.ID,@TransactionID,'Email',cDel.[Name],cUpd.[Name],d.Email,i.Email,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(i.PracticeID,'') != ISNULL(d.PracticeID,'')
Join Practice as cDel on cDel.ID = ISNULL(d.PracticeID,'')
join Practice as cUpd on cUpd.ID = ISNULL(i.PracticeID,'')

--when update Email
if update(Email)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Email',d.Email,i.Email,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Email,'') != ISNULL(d.Email,'')

--when update IsActive
if update(IsActive)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsActive',d.IsActive,i.IsActive,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsActive,'') != ISNULL(d.IsActive,'')

--when update IsDeleted
if update(IsDeleted)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsDeleted',d.IsDeleted,i.IsDeleted,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsDeleted,'') != ISNULL(d.IsDeleted,'')

--when update Notes
if update(Notes)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',d.Notes,i.Notes,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Notes,'') != ISNULL(d.Notes,'')
END
GO
--=====================================
--Description: <tgrLocationAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrLocationAudit')
DROP TRIGGER [dbo].[tgrLocationAudit]
GO
Create TRIGGER [dbo].[tgrLocationAudit]
ON [dbo].[Location] after Update
AS 
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM LocationAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM LocationAudit

--when update(Name)
IF Update(Name)
INSERT INTO LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Name',d.Name,i.Name,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Name,'') != ISNULL(d.Name,'')

--when update(OrganizationName)
if update(OrganizationName)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Organization Name',d.OrganizationName,i.OrganizationName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join Deleted as d on i.ID = d.ID and ISNULL(i.OrganizationName,'') != ISNULL(d.OrganizationName,'')

--when update PracticeID
if update (PracticeID)
Select * into #t from Practice
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
Select i.ID,@TransactionID,'Practice ID',cDel.[Name],cUpd.[Name],d.PracticeID,i.PracticeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(i.PracticeID,'') != ISNULL(d.PracticeID,'')
Join Practice as cDel on cDel.ID = ISNULL(d.PracticeID,'')
join Practice as cUpd on cUpd.ID = ISNULL(i.PracticeID,'')

if update(NPI)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'NPI',d.NPI,i.NPI,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join Deleted as d on i.ID = d.ID and ISNULL(i.NPI,'') != ISNULL(d.NPI,'')

--when update POSID
if update (POSID)
Select * into #t1 from POS
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
Select i.ID,@TransactionID,'POS ID',cDel.[Name],cUpd.[Name],d.POSID,i.POSID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(i.POSID,'') != ISNULL(d.POSID,'')
Join Practice as cDel on cDel.ID = ISNULL(d.POSID,'')
join Practice as cUpd on cUpd.ID = ISNULL(i.POSID,'')

--when update(Address1)
if update(Address1)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'Address1',d.Address1,i.Address1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(i.Address1,'') != ISNULL(d.Address1,'')

--when update(Address2)
if update(Address2)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'Address2',d.Address2,i.Address2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(i.Address2,'') != ISNULL(d.Address2,'')

--when update(City)
if update(City)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'City',d.City,i.City,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(i.City,'') != ISNULL(d.City,'')

--when update(State)
if update(State)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'State',d.State,i.State,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(i.State,'') != ISNULL(d.State,'')

--when update(ZipCode)
if update(ZipCode)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'Zip Code',d.ZipCode,i.ZipCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(i.ZipCode,'') != ISNULL(d.ZipCode,'')

--when update(CLIANumber)
if update(CLIANumber)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'CLIA Number',d.CLIANumber,i.CLIANumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(i.CLIANumber,'') != ISNULL(d.CLIANumber,'')

--when update(Fax)
if update(Fax)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'Fax',d.Fax,i.Fax,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(i.Fax,'') != ISNULL(d.Fax,'')

--when update(website)
if update(website)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'website',d.website,i.website,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(i.website,'') != ISNULL(d.website,'')

--when update(Email)
if update(Email)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'Email',d.Email,i.Email,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(i.Email,'') != ISNULL(d.Email,'')

--when update(PhoneNumber)
if update(PhoneNumber)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'Phone Number',d.PhoneNumber,i.PhoneNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(i.PhoneNumber,'') != ISNULL(d.PhoneNumber,'')

--when update(Notes)
if update(Notes)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'Notes',d.Notes,i.Notes,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(i.Notes,'') != ISNULL(d.Notes,'')

--when update(IsActive)
if update(IsActive)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'IsActive',d.IsActive,i.IsActive,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsActive,'') != ISNULL(d.IsActive,'')

--when update(IsDeleted)
if update(IsDeleted)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'IsDeleted',d.IsDeleted,i.IsDeleted,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsDeleted,'') != ISNULL(d.IsDeleted,'')
END
GO

--=====================================
--Description: <tgrChargeAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrChargeAudit')
DROP TRIGGER [dbo].[tgrChargeAudit]
GO
Create  TRIGGER [dbo].[tgrChargeAudit]
ON [dbo].[Charge] after Update
AS 
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM ChargeAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM ChargeAudit  

--When User Update VisitID
IF Update(VisitID)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'VisitID',d.VisitId,i.visitId,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.VisitID,'') != ISNULL(d.VisitID,'')

--When User Update ClientID
IF Update(ClientID)
Select * Into #t from Client 
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Client ID',cDel.[Name],cUpd.[Name],d.ClientID,i.ClientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ClientID,'') != ISNULL(d.ClientID,'')
Join Client as cDel on cDel.ID = ISNULL(d.ClientID,'')
join Client as cUpd on cUpd.ID = ISNULL(i.ClientID,'')

--When User Update PracticeID
IF Update(PracticeID)
Select * Into #t15 from Practice
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Practice ID',pDel.[Name],pUpd.[Name],d.PracticeID,i.PracticeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PracticeID,'') != ISNULL(d.PracticeID,'')
Join Practice as pDel on pdel.ID = ISNULL(d.PracticeID,'')
join Practice as pUpd on pUpd.ID = ISNULL(i.PracticeID,'')

--When User Update LocationID
IF Update(LocationID)
Select * Into #t14 from Location
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Location ID',dLoc.Name,uLoc.Name,d.LocationID,i.LocationID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.LocationID,'') != ISNULL(d.LocationID,'')
JOIN Location as dLoc on dLoc.ID = ISNULL(d.LocationID,'')
JOIN Location as uLoc on uLoc.ID = ISNULL(i.LocationID,'')

--When User Update POSID
IF Update(POSID)
Select * Into #t13 from POS
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'POSID',dPOS.[Name],uPOS.[Name],d.POSID,i.POSID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.POSID,'') != ISNULL(d.POSID,'')
Join POS as dPOS on dPOS.ID = ISNULL(d.POSID,'')
join POS as uPOS on uPOS.ID = ISNULL(i.POSID,'')

--When User Update ProviderID
IF Update(ProviderID)
Select * Into #t12 from Provider
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Provider ID',dProv.Name,uProv.Name,d.ProviderID,i.ProviderID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ProviderID,'') != ISNULL(d.ProviderID,'')
Join Provider as dProv on dProv.ID = ISNULL(d.ProviderID,'')
join Provider as uProv on  uProv.ID = ISNULL(i.ProviderID,'')

--When User Update RefProviderID
IF Update(RefProviderID)
Select * Into #t11 from RefProvider
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Reference Provider ID',pDel.[Name],pUpd.[Name],d.RefProviderID,i.RefProviderID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.RefProviderID,'') != ISNULL(d.RefProviderID,'')
Join RefProvider as pDel on pdel.ID = ISNULL(d.RefProviderID,'')
join RefProvider as pUpd on pUpd.ID = ISNULL(i.RefProviderID,'')


--When User Update SupervisingProvID
IF Update(SupervisingProvID)
Select * Into #t10 from Provider
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Supervising Provider ID',dProv.Name,uProv.Name,d.SupervisingProvID,i.SupervisingProvID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SupervisingProvID,'') != ISNULL(d.SupervisingProvID,'')
Join Provider as dProv on dProv.ID = ISNULL(d.SupervisingProvID,'') 
join Provider as uProv on  uProv.ID = ISNULL(i.SupervisingProvID,'')

--When User Update OrderingProvID
IF Update(OrderingProvID)
Select * Into #t9 from Provider
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Ordering Provider ID',dProv.Name,uProv.Name,d.OrderingProvID,i.OrderingProvID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.OrderingProvID,'') != ISNULL(d.OrderingProvID,'')
Join Provider as dProv on dProv.ID = ISNULL(d.OrderingProvID,'')
join Provider as uProv on  uProv.ID = ISNULL(i.OrderingProvID,'')

--When User Update PatientID
IF Update(PatientID)
Select * Into #t8 from Patient
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Patient ID',dPat.LastName + ', ' + dPat.FirstName,uPat.LastName + ', ' + uPat.FirstName,d.PatientID,i.PatientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PatientID,'') != ISNULL(d.PatientID,'')
Join Patient as dPat on dPat.ID = ISNULL(d.PatientID,'')
join Patient as uPat on  uPat.ID = ISNULL(i.PatientID,'')

--When User Update PrimaryPatientPlanID
IF Update(PrimaryPatientPlanID)
Select * Into #t6 from PatientPlan
Select * Into #t7 from InsurancePlan 
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Patient Plan ID',dInsPlan.PlanName,uInsPlan.PlanName,d.PrimaryPatientPlanID,i.PrimaryPatientPlanID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryPatientPlanID,'') != ISNULL(d.PrimaryPatientPlanID,'')
join PatientPlan as dpatPlan on ISNULL(dpatPlan.ID,'') = ISNULL(d.PrimaryPatientPlanID,'')
join InsurancePlan as dInsPlan on ISNULL(dpatPlan.InsurancePlanID,'') = ISNULL(dInsPlan.ID,'')

left join PatientPlan as upatPlan on ISNULL(upatPlan.ID,'') = ISNULL(i.PrimaryPatientPlanID,'')
left join InsurancePlan as uInsPlan on ISNULL(upatPlan.InsurancePlanID,'') = ISNULL(uInsPlan.ID,'')

--When User Update SecondaryPatientPlanID
IF Update(SecondaryPatientPlanID)
Select * Into #t4 from PatientPlan
Select * Into #t5 from InsurancePlan 
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Patient Plan ID',dInsPlan.PlanName,uInsPlan.PlanName,d.SecondaryPatientPlanID,i.SecondaryPatientPlanID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryPatientPlanID,'') != ISNULL(d.SecondaryPatientPlanID,'')

join PatientPlan as dpatPlan on ISNULL(dpatPlan.ID,'') = ISNULL(d.SecondaryPatientPlanID,'')
join InsurancePlan as dInsPlan on ISNULL(dpatPlan.InsurancePlanID,'') = ISNULL(dInsPlan.ID,'')

left join PatientPlan as upatPlan on ISNULL(upatPlan.ID,'') = ISNULL(i.SecondaryPatientPlanID,'')
left join InsurancePlan as uInsPlan on ISNULL(upatPlan.InsurancePlanID,'') = ISNULL(uInsPlan.ID,'')

--When User Update TertiaryPatientPlanID
IF Update(TertiaryPatientPlanID)
Select * Into #t2 from PatientPlan
Select * Into #t3 from InsurancePlan 
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Patient Plan ID',dInsPlan.PlanName,uInsPlan.PlanName,d.TertiaryPatientPlanID,i.TertiaryPatientPlanID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryPatientPlanID,'') != ISNULL(d.TertiaryPatientPlanID,'')


join PatientPlan as dpatPlan on ISNULL(dpatPlan.ID,'') = ISNULL(d.TertiaryPatientPlanID,'')
join InsurancePlan as dInsPlan on ISNULL(dpatPlan.InsurancePlanID,'') = ISNULL(dInsPlan.ID,'')

left join PatientPlan as upatPlan on ISNULL(upatPlan.ID,'') = ISNULL(i.TertiaryPatientPlanID,'')
left join InsurancePlan as uInsPlan on ISNULL(upatPlan.InsurancePlanID,'') = ISNULL(uInsPlan.ID,'')

--When User Update SubmissionLogID
IF Update(SubmissionLogID)
Select * Into #t1 from SubmissionLog  
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'SubmissionLog ID',dSLog.ID,uSLog.ID,d.SubmissionLogID,i.SubmissionLogID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SubmissionLogID,'') != ISNULL(d.SubmissionLogID,'')
Join SubmissionLog as dSLog on dSLog.ID = ISNULL(d.SubmissionLogID,'')
join SubmissionLog as uSLog on  uSLog.ID = ISNULL(i.SubmissionLogID,'')

--When User Update CPTID
IF Update(CPTID)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'CPT ID',d.CPTID,i.CPTID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.CPTID,'') != ISNULL(d.CPTID,'')

--When User Update Modifier1ID
IF Update(Modifier1ID)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Modifier1 ID',d.Modifier1ID,i.Modifier1ID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Modifier1ID,'') != ISNULL(d.Modifier1ID,'')

--When User Update Modifier1Amount
IF Update(Modifier1Amount)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Modifier1 Amount',d.Modifier1Amount,i.Modifier1Amount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Modifier1Amount,'1') != ISNULL(d.Modifier1Amount,'1')

--When User Update Modifier2ID
IF Update(Modifier2ID)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Modifier2 ID',d.Modifier2ID,i.Modifier2ID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Modifier2ID,'') != ISNULL(d.Modifier2ID,'')

--When User Update Modifier2Amount
IF Update(Modifier2Amount)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Modifier2 Amount',d.Modifier2Amount,i.Modifier2Amount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Modifier2Amount,'1')  !=  ISNULL(d.Modifier2Amount,'1') 

--When User Update Modifier3ID
IF Update(Modifier3ID)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Modifier3ID',d.Modifier3ID,i.Modifier3ID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Modifier3ID,'') != ISNULL(d.Modifier3ID,'')

--When User Update Modifier3Amount
IF Update(Modifier3Amount)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Modifier3 Amount',d.Modifier3Amount,i.Modifier3Amount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Modifier3Amount,'1') != ISNULL(d.Modifier3Amount,'1')

--When User Update Modifier4ID
IF Update(Modifier4ID)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Modifier4 ID',d.Modifier4ID,i.Modifier4ID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Modifier4ID,'') != ISNULL(d.Modifier4ID,'')

--When User Update Modifier4Amount
IF Update(Modifier4Amount)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Modifier4 Amount',d.Modifier4Amount,i.Modifier4Amount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Modifier4Amount,'1') != ISNULL(d.Modifier4Amount,'1')

--When User Update Units
IF Update(Units)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Units',d.Units,i.Units,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Units,'') != ISNULL(d.Units,'')

--When User Update Minutes
IF Update(Minutes)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Minutes',d.Minutes,i.Minutes,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Minutes,'') != ISNULL(d.Minutes,'')

--When User Update NdcUnits
IF Update(NdcUnits)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Ndc Units',d.NdcUnits,i.NdcUnits,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.NdcUnits,'') != ISNULL(d.NdcUnits,'')

--When User Update NdcMeasurementUnit
IF Update(NdcMeasurementUnit)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Ndc Measurement Unit',d.NdcMeasurementUnit,i.NdcMeasurementUnit,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.NdcMeasurementUnit,'') != ISNULL(d.NdcMeasurementUnit,'')

--When User Update Description
IF Update(Description)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Description',d.Description,i.Description,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Description,'') != ISNULL(d.Description,'')

--When User Update NdcNumber
IF Update(NdcNumber)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Ndc Number',d.NdcNumber,i.NdcNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.NdcNumber,'') != ISNULL(d.NdcNumber,'')

--When User Update UnitOfMeasurement
IF Update(UnitOfMeasurement)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Unit Of Measurement',d.UnitOfMeasurement,i.UnitOfMeasurement,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.UnitOfMeasurement,'') != ISNULL(d.UnitOfMeasurement,'')

--When User Update DateOfServiceFrom
IF Update(DateOfServiceFrom)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Date Of Service From',d.DateOfServiceFrom,i.DateOfServiceFrom,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.DateOfServiceFrom,'') != ISNULL(d.DateOfServiceFrom,'')

--When User Update DateOfServiceTo
IF Update(DateOfServiceTo)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Date Of Service To',d.DateOfServiceTo,i.DateOfServiceTo,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.DateOfServiceTo,'') != ISNULL(d.DateOfServiceTo,'')


--When User Update StartTime
IF Update(StartTime)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Start Time',d.StartTime,i.StartTime,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.StartTime,'') != ISNULL(d.StartTime,'')

--When User Update EndTime
IF Update(EndTime)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'End Time',d.EndTime,i.EndTime,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.EndTime,'') != ISNULL(d.EndTime,'')

--When User Update TimeUnits
IF Update(TimeUnits)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Time Units',d.TimeUnits,i.TimeUnits,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TimeUnits,'') != ISNULL(d.TimeUnits,'')

--When User Update BaseUnits
IF Update(BaseUnits)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Base Units',d.BaseUnits,i.BaseUnits,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.BaseUnits,'') != ISNULL(d.BaseUnits,'')

--When User Update ModifierUnits
IF Update(ModifierUnits)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Modifier Units',d.ModifierUnits,i.ModifierUnits,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ModifierUnits,'') != ISNULL(d.ModifierUnits,'')

--When User Update Pointer1
IF Update(Pointer1)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Pointer1',d.Pointer1,i.Pointer1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Pointer1,'') != ISNULL(d.Pointer1,'')

--When User Update Pointer2
IF Update(Pointer2)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Pointer2',d.Pointer2,i.Pointer2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Pointer2,'') != ISNULL(d.Pointer2,'')

--When User Update Pointer3
IF Update(Pointer3)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Pointer3',d.Pointer3,i.Pointer3,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Pointer3,'') != ISNULL(d.Pointer3,'')


--When User Update Pointer4
IF Update(Pointer4)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Pointer4',d.Pointer4,i.Pointer4,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Pointer4,'') != ISNULL(d.Pointer4,'')


--When User Update TotalAmount
IF Update(TotalAmount)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Total Amount',d.TotalAmount,i.TotalAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TotalAmount,'1') != ISNULL(d.TotalAmount,'1')


--When User Update Copay
IF Update(Copay)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Copay',d.Copay,i.Copay,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Copay,'1') != ISNULL(d.Copay,'')

--When User Update Coinsurance
IF Update(Coinsurance)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Coinsurance',d.Coinsurance,i.Coinsurance,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Coinsurance,'1') != ISNULL(d.Coinsurance,'1')

--When User Update PrimaryBilledAmount
IF Update(PrimaryBilledAmount)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Billed Amount',d.PrimaryBilledAmount,i.PrimaryBilledAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryBilledAmount,'1') != ISNULL(d.PrimaryBilledAmount,'1')

--When User Update Deductible
IF Update(Deductible)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Deductible',d.Deductible,i.Deductible,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Deductible,'1') != ISNULL(d.Deductible,'1')

--When User Update PrimaryAllowed
IF Update(PrimaryAllowed)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Allowed',d.PrimaryAllowed,i.PrimaryAllowed,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryAllowed,'1') != ISNULL(d.PrimaryAllowed,'1')

--When User Update PrimaryWriteOff
IF Update(PrimaryWriteOff)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Write Off',d.PrimaryWriteOff,i.PrimaryWriteOff,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryWriteOff,'1') != ISNULL(d.PrimaryWriteOff,'1')

--When User Update PrimaryPaid
IF Update(PrimaryPaid)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Paid',d.PrimaryPaid,i.PrimaryPaid,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryPaid,'1') != ISNULL(d.PrimaryPaid,'1')

--When User Update PrimaryBal
IF Update(PrimaryBal)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Balance',d.PrimaryBal,i.PrimaryBal,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryBal,'1') != ISNULL(d.PrimaryBal,'1')

--When User Update PrimaryPatientBal
IF Update(PrimaryPatientBal)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Patient Balance',d.PrimaryPatientBal,i.PrimaryPatientBal,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryPatientBal,'1') != ISNULL(d.PrimaryPatientBal,'1')

--When User Update PrimaryTransferred
IF Update(PrimaryTransferred)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Transferred',d.PrimaryTransferred,i.PrimaryTransferred,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryTransferred,'1') != ISNULL(d.PrimaryTransferred,'1')

--When User Update PrimaryStatus
IF Update(PrimaryStatus)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Status',d.PrimaryStatus,i.PrimaryStatus,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryStatus,'') != ISNULL(d.PrimaryStatus,'')

--When User Update SecondaryBilledAmount
IF Update(SecondaryBilledAmount)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Billed Amount',d.SecondaryBilledAmount,i.SecondaryBilledAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryBilledAmount,'1') != ISNULL(d.SecondaryBilledAmount,'1')

--When User Update SecondaryAllowed
IF Update(SecondaryAllowed)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Allowed',d.SecondaryAllowed,i.SecondaryAllowed,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryAllowed,'1') != ISNULL(d.SecondaryAllowed,'1')

--When User Update SecondaryWriteOff
IF Update(SecondaryWriteOff)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Write Off',d.SecondaryWriteOff,i.SecondaryWriteOff,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryWriteOff,'1') != ISNULL(d.SecondaryWriteOff,'1')

--When User Update SecondaryPaid
IF Update(SecondaryPaid)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Paid',d.SecondaryPaid,i.SecondaryPaid,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryPaid,'1') != ISNULL(d.SecondaryPaid,'1')

--When User Update SecondaryPatResp
IF Update(SecondaryPatResp)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Patient Response',d.SecondaryPatResp,i.SecondaryPatResp,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryPatResp,'1') != ISNULL(d.SecondaryPatResp,'1')

--When User Update SecondaryBal
IF Update(SecondaryBal)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Balance',d.SecondaryBal,i.SecondaryBal,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryBal,'1') != ISNULL(d.SecondaryBal,'1')

--When User Update SecondaryPatientBal
IF Update(SecondaryPatientBal)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Patient Balance',d.SecondaryPatientBal,i.SecondaryPatientBal,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryPatientBal,'1') != ISNULL(d.SecondaryPatientBal,'1')

--When User Update SecondaryTransferred
IF Update(SecondaryTransferred)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Transferred',d.SecondaryTransferred,i.SecondaryTransferred,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryTransferred,'1') != ISNULL(d.SecondaryTransferred,'1')

--When User Update SecondaryStatus
IF Update(SecondaryStatus)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Status',d.SecondaryStatus,i.SecondaryStatus,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryStatus,'') != ISNULL(d.SecondaryStatus,'')

--When User Update TertiaryBilledAmount
IF Update(TertiaryBilledAmount)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Billed Amount',d.TertiaryBilledAmount,i.TertiaryBilledAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryBilledAmount,'1') != ISNULL(d.TertiaryBilledAmount,'1')

--When User Update TertiaryAllowed
IF Update(TertiaryAllowed)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Allowed',d.TertiaryAllowed,i.TertiaryAllowed,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryAllowed,'1') != ISNULL(d.TertiaryAllowed,'1')

--When User Update TertiaryWriteOff
IF Update(TertiaryWriteOff)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Write Off',d.TertiaryWriteOff,i.TertiaryWriteOff,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryWriteOff,'1') != ISNULL(d.TertiaryWriteOff,'1')

--When User Update TertiaryPaid
IF Update(TertiaryPaid)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Paid',d.TertiaryPaid,i.TertiaryPaid,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryPaid,'1') != ISNULL(d.TertiaryPaid,'1')

--When User Update TertiaryPatResp
IF Update(TertiaryPatResp)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Patient Response',d.TertiaryPatResp,i.TertiaryPatResp,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryPatResp,'1') != ISNULL(d.TertiaryPatResp,'1')

--When User Update TertiaryBal
IF Update(TertiaryBal)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Balance',d.TertiaryBal,i.TertiaryBal,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryBal,'1') != ISNULL(d.TertiaryBal,'1')


--When User Update TertiaryPatientBal
IF Update(TertiaryPatientBal)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Patient Balance',d.TertiaryPatientBal,i.TertiaryPatientBal,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryPatientBal,'1') != ISNULL(d.TertiaryPatientBal,'1')

--When User Update TertiaryTransferred
IF Update(TertiaryTransferred)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Transferred',d.TertiaryTransferred,i.TertiaryTransferred,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryTransferred,'1') != ISNULL(d.TertiaryTransferred,'1')

--When User Update TertiaryStatus
IF Update(TertiaryStatus)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Status',d.TertiaryStatus,i.TertiaryStatus,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryStatus,'') != ISNULL(d.TertiaryStatus,'')

--When User Update PatientAmount
IF Update(PatientAmount)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Patient Amount',d.PatientAmount,i.PatientAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PatientAmount,'1') != ISNULL(d.PatientAmount,'1')

--When User Update PatientPaid
IF Update(PatientPaid)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Patient Paid',d.PatientPaid,i.PatientPaid,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PatientPaid,'1') != ISNULL(d.PatientPaid,'1')

--When User Update IsSubmitted
IF Update(IsSubmitted)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Is Submitted',(case when d.IsSubmitted = 1 then 'Yes' else 'No' end),(case when i.IsSubmitted = 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.IsSubmitted,'') != ISNULL(d.IsSubmitted,'')

--When User Update IsDontPrint
IF Update(IsDontPrint)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'I sDontPrint',(case when d.IsDontPrint = 1 then 'Yes' else 'No' end),(case when i.IsDontPrint = 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.IsDontPrint,'') != ISNULL(d.IsDontPrint,'')

--When User Update SubmittetdDate
IF Update(SubmittetdDate)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Submittetd Date',d.SubmittetdDate,i.SubmittetdDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SubmittetdDate,'') != ISNULL(d.SubmittetdDate,'')
END
GO

--=====================================
--Description: <tgrVisitAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrVisitAudit')
DROP TRIGGER [dbo].[tgrVisitAudit]
GO
create   TRIGGER [dbo].[tgrVisitAudit]
ON [dbo].[Visit] after Update
AS 
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM VisitAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM VisitAudit

--When User Update PageNumber
IF Update(PageNumber)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Page Number',d.PageNumber,i.PageNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PageNumber,'') != ISNULL(d.PageNumber,'')

--When User Update BatchDocumentID
IF Update(BatchDocumentID)
select * Into #t1 from BatchDocument
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Batch DocumentID',pDel.[Description],pUpd.[Description],d.BatchDocumentID,i.BatchDocumentID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.BatchDocumentID,'') != ISNULL(d.BatchDocumentID,'')
Join BatchDocument as pDel on pdel.ID = ISNULL(d.BatchDocumentID,'')
join BatchDocument as pUpd on pUpd.ID = ISNULL(i.BatchDocumentID,'')

--when update Client
IF Update(ClientID)
select * Into #t2 from Client
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Client ID',pDel.[Name],pUpd.[Name],d.ClientID,i.ClientID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ClientID,'') != ISNULL(d.ClientID,'')
Join Client as pDel on pdel.ID = ISNULL(d.ClientID,'')
join Client as pUpd on pUpd.ID = ISNULL(i.ClientID,'')

--When User Update PracticeID
IF Update(PracticeID)
select * Into #t3 from Practice
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Practice ID',pDel.[Name],pUpd.[Name],d.PracticeID,i.PracticeID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PracticeID,'') != ISNULL(d.PracticeID,'')
Join Client as pDel on pdel.ID = ISNULL(d.PracticeID,'')
join Client as pUpd on pUpd.ID = ISNULL(i.PracticeID,'')

--When User Update LocationID
IF Update(LocationID)
select * Into #t4 from Location
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Location ID',pDel.[Name],pUpd.[Name],d.LocationID,i.LocationID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.LocationID,'') != ISNULL(d.LocationID,'')
Join Location as pDel on pdel.ID = ISNULL(d.LocationID,'')
join Location as pUpd on pUpd.ID = ISNULL(i.LocationID,'')

--When User Update POSID
IF Update(POSID)
select * Into #t5 from POS
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'POS ID',pDel.[Name],pUpd.[Name],d.POSID,i.POSID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.POSID,'') != ISNULL(d.POSID,'')
Join POS as pDel on pdel.ID = ISNULL(d.POSID,'')
join POS as pUpd on pUpd.ID = ISNULL(i.POSID,'')

--When User Update ProviderID
IF Update(ProviderID)
select * Into #t6 from Provider
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Provider ID',pDel.[Name],pUpd.[Name],d.ProviderID,i.ProviderID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ProviderID,'') != ISNULL(d.ProviderID,'')
Join Provider as pDel on pdel.ID = ISNULL(d.ProviderID,'')
join Provider as pUpd on pUpd.ID = ISNULL(i.ProviderID,'')


--When User Update RefProviderID
IF Update(RefProviderID)
select * Into #t7 from RefProvider
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Reference Provider ID',pDel.[Name],pUpd.[Name],d.RefProviderID,i.RefProviderID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.RefProviderID,'') != ISNULL(d.RefProviderID,'')
Join RefProvider as pDel on pdel.ID = ISNULL(d.RefProviderID,'')
join RefProvider as pUpd on pUpd.ID = ISNULL(i.RefProviderID,'')


--When User Update SupervisingProvID
IF Update(SupervisingProvID)
Select * Into #t8 from Provider
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Supervising Provider ID',dProv.Name,uProv.Name,d.SupervisingProvID,i.SupervisingProvID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SupervisingProvID,'') != ISNULL(d.SupervisingProvID,'')
Join Provider as dProv on dProv.ID = ISNULL(d.SupervisingProvID,'') 
join Provider as uProv on  uProv.ID = ISNULL(i.SupervisingProvID,'')

--When User Update OrderingProvID
IF Update(OrderingProvID)
Select * Into #t9 from Provider
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Ordering Provider ID',dProv.Name,uProv.Name,d.OrderingProvID,i.OrderingProvID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.OrderingProvID,'') != ISNULL(d.OrderingProvID,'')
Join Provider as dProv on dProv.ID = ISNULL(d.OrderingProvID,'')
join Provider as uProv on  uProv.ID = ISNULL(i.OrderingProvID,'')

--When User Update PatientID
IF Update(PatientID)
Select * Into #t10 from Patient
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Patient ID',dPat.LastName + ', ' + dPat.FirstName,uPat.LastName + ', ' + uPat.FirstName,d.PatientID,i.PatientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PatientID,'') != ISNULL(d.PatientID,'')
Join Patient as dPat on dPat.ID = ISNULL(d.PatientID,'')
join Patient as uPat on  uPat.ID = ISNULL(i.PatientID,'')

--When User Update PrimaryPatientPlanID
IF Update(PrimaryPatientPlanID)
Select * Into #t11 from PatientPlan
Select * Into #t12 from InsurancePlan 
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Patient Plan ID',dInsPlan.PlanName,uInsPlan.PlanName,d.PrimaryPatientPlanID,i.PrimaryPatientPlanID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryPatientPlanID,'') != ISNULL(d.PrimaryPatientPlanID,'')
join PatientPlan as dpatPlan on ISNULL(dpatPlan.ID,'') = ISNULL(d.PrimaryPatientPlanID,'')
join InsurancePlan as dInsPlan on ISNULL(dpatPlan.InsurancePlanID,'') = ISNULL(dInsPlan.ID,'')

left join PatientPlan as upatPlan on ISNULL(upatPlan.ID,'') = ISNULL(i.PrimaryPatientPlanID,'')
left join InsurancePlan as uInsPlan on ISNULL(upatPlan.InsurancePlanID,'') = ISNULL(uInsPlan.ID,'')

--When User Update SecondaryPatientPlanID
IF Update(SecondaryPatientPlanID)
Select * Into #t13 from PatientPlan
Select * Into #t14 from InsurancePlan 
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Patient Plan ID',dInsPlan.PlanName,uInsPlan.PlanName,d.SecondaryPatientPlanID,i.SecondaryPatientPlanID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryPatientPlanID,'') != ISNULL(d.SecondaryPatientPlanID,'')

join PatientPlan as dpatPlan on ISNULL(dpatPlan.ID,'') = ISNULL(d.SecondaryPatientPlanID,'')
join InsurancePlan as dInsPlan on ISNULL(dpatPlan.InsurancePlanID,'') = ISNULL(dInsPlan.ID,'')

left join PatientPlan as upatPlan on ISNULL(upatPlan.ID,'') = ISNULL(i.SecondaryPatientPlanID,'')
left join InsurancePlan as uInsPlan on ISNULL(upatPlan.InsurancePlanID,'') = ISNULL(uInsPlan.ID,'')

--When User Update TertiaryPatientPlanID
IF Update(TertiaryPatientPlanID)
Select * Into #t15 from PatientPlan
Select * Into #t16 from InsurancePlan 
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Patient Plan ID',dInsPlan.PlanName,uInsPlan.PlanName,d.TertiaryPatientPlanID,i.TertiaryPatientPlanID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryPatientPlanID,'') != ISNULL(d.TertiaryPatientPlanID,'')


join PatientPlan as dpatPlan on ISNULL(dpatPlan.ID,'') = ISNULL(d.TertiaryPatientPlanID,'')
join InsurancePlan as dInsPlan on ISNULL(dpatPlan.InsurancePlanID,'') = ISNULL(dInsPlan.ID,'')

left join PatientPlan as upatPlan on ISNULL(upatPlan.ID,'') = ISNULL(i.TertiaryPatientPlanID,'')
left join InsurancePlan as uInsPlan on ISNULL(upatPlan.InsurancePlanID,'') = ISNULL(uInsPlan.ID,'')

--When User Update SubmissionLogID
IF Update(SubmissionLogID)
Select * Into #t17 from SubmissionLog  
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'SubmissionLog ID',dSLog.ID,uSLog.ID,d.SubmissionLogID,i.SubmissionLogID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SubmissionLogID,'') != ISNULL(d.SubmissionLogID,'')
Join SubmissionLog as dSLog on dSLog.ID = ISNULL(d.SubmissionLogID,'')
join SubmissionLog as uSLog on  uSLog.ID = ISNULL(i.SubmissionLogID,'')

--When User Update RejectionReason
IF Update(RejectionReason)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Rejection Reason',d.RejectionReason,i.RejectionReason,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.RejectionReason,'') != ISNULL(d.RejectionReason,'')

--When User Update ICD1ID
IF Update(ICD1ID)
select * Into #t18 from ICD
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'ICD ID',pDel.[Description],pUpd.[Description],d.ICD1ID,i.ICD1ID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ICD1ID,'') != ISNULL(d.ICD1ID,'')
Join ICD as pDel on pdel.ID = ISNULL(d.ICD1ID,'')
join ICD as pUpd on pUpd.ID = ISNULL(i.ICD1ID,'')

--When User Update ICD2ID
IF Update(ICD2ID)
select * Into #t19 from ICD
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'ICD2 ID',pDel.[Description],pUpd.[Description],d.ICD2ID,i.ICD2ID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ICD2ID,'') != ISNULL(d.ICD2ID,'')
Join ICD as pDel on pdel.ID = ISNULL(d.ICD2ID,'')
join ICD as pUpd on pUpd.ID = ISNULL(i.ICD2ID,'')

--When User Update ICD3ID
IF Update(ICD3ID)
select * Into #t20 from ICD
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'ICD3 ID',pDel.[Description],pUpd.[Description],d.ICD3ID,i.ICD3ID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ICD3ID,'') != ISNULL(d.ICD3ID,'')
Join ICD as pDel on pdel.ID = ISNULL(d.ICD3ID,'')
join ICD as pUpd on pUpd.ID = ISNULL(i.ICD3ID,'')

--When User Update ICD4ID
IF Update(ICD4ID)
select * Into #t21 from ICD
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'ICD4 ID',pDel.[Description],pUpd.[Description],d.ICD4ID,i.ICD4ID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ICD4ID,'') != ISNULL(d.ICD4ID,'')
Join ICD as pDel on pdel.ID = ISNULL(d.ICD4ID,'')
join ICD as pUpd on pUpd.ID = ISNULL(i.ICD4ID,'')

--When User Update ICD5ID
IF Update(ICD5ID)
select * Into #t22 from ICD
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'ICD5 ID',pDel.[Description],pUpd.[Description],d.ICD5ID,i.ICD5ID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ICD5ID,'') != ISNULL(d.ICD5ID,'')
Join ICD as pDel on pdel.ID = ISNULL(d.ICD5ID,'')
join ICD as pUpd on pUpd.ID = ISNULL(i.ICD5ID,'')

--When User Update ICD6ID
IF Update(ICD6ID)
select * Into #t23 from ICD
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'ICD6 ID',pDel.[Description],pUpd.[Description],d.ICD6ID,i.ICD6ID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ICD6ID,'') != ISNULL(d.ICD6ID,'')
Join ICD as pDel on pdel.ID = ISNULL(d.ICD6ID,'')
join ICD as pUpd on pUpd.ID = ISNULL(i.ICD6ID,'')

--When User Update ICD7ID
IF Update(ICD7ID)
select * Into #t24 from ICD
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'ICD7 ID',pDel.[Description],pUpd.[Description],d.ICD7ID,i.ICD7ID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ICD7ID,'') != ISNULL(d.ICD7ID,'')
Join ICD as pDel on pdel.ID = ISNULL(d.ICD7ID,'')
join ICD as pUpd on pUpd.ID = ISNULL(i.ICD7ID,'')

--When User Update ICD8ID
IF Update(ICD8ID)
select * Into #t25 from ICD
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'ICD8 ID',pDel.[Description],pUpd.[Description],d.ICD8ID,i.ICD8ID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ICD8ID,'') != ISNULL(d.ICD8ID,'')
Join ICD as pDel on pdel.ID = ISNULL(d.ICD8ID,'')
join ICD as pUpd on pUpd.ID = ISNULL(i.ICD8ID,'')

--When User Update ICD9ID
IF Update(ICD9ID)
select * Into #t26 from ICD
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'ICD9 ID',pDel.[Description],pUpd.[Description],d.ICD9ID,i.ICD9ID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ICD9ID,'') != ISNULL(d.ICD9ID,'')
Join ICD as pDel on pdel.ID = ISNULL(d.ICD9ID,'')
join ICD as pUpd on pUpd.ID = ISNULL(i.ICD9ID,'')

--When User Update ICD10ID
IF Update(ICD10ID)
select * Into #t27 from ICD
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'ICD10 ID',pDel.[Description],pUpd.[Description],d.ICD10ID,i.ICD10ID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ICD10ID,'') != ISNULL(d.ICD10ID,'')
Join ICD as pDel on pdel.ID = ISNULL(d.ICD10ID,'')
join ICD as pUpd on pUpd.ID = ISNULL(i.ICD10ID,'')

--When User Update ICD11ID
--IF Update(ICD11ID)
--INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
--SELECT  i.ID, @TransactionID,'ICD11 ID',d.ICD11ID,i.ICD11ID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
--FROM inserted as i
--INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ICD11ID,'') != ISNULL(d.ICD11ID,'')

--When User Update ICD11ID
IF Update(ICD11ID)
select * Into #t28 from ICD
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'ICD11 ID',pDel.[Description],pUpd.[Description],d.ICD11ID,i.ICD11ID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ICD11ID,'') != ISNULL(d.ICD11ID,'')
Join ICD as pDel on pdel.ID = ISNULL(d.ICD11ID,'')
join ICD as pUpd on pUpd.ID = ISNULL(i.ICD11ID,'')

--When User Update ICD12ID
IF Update(ICD12ID)
select * Into #t29 from ICD
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'ICD12 ID',pDel.[Description],pUpd.[Description],d.ICD12ID,i.ICD12ID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ICD12ID,'') != ISNULL(d.ICD12ID,'')
Join ICD as pDel on pdel.ID = ISNULL(d.ICD12ID,'')
join ICD as pUpd on pUpd.ID = ISNULL(i.ICD12ID,'')

--When User Update AuthorizationNum
IF Update(AuthorizationNum)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Authorization Number',d.AuthorizationNum,i.AuthorizationNum,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.AuthorizationNum,'') != ISNULL(d.AuthorizationNum,'')

--When User Update OutsideReferral
IF Update(OutsideReferral)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Outside Referral',(case when d.OutsideReferral = 1 then 'Yes' else 'No' end),(case when i.OutsideReferral = 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.OutsideReferral,'') != ISNULL(d.OutsideReferral,'')

--When User Update IsForcePaper
IF Update(IsForcePaper)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Is Force Paper',(case when d.IsForcePaper = 1 then 'Yes' else 'No' end),(case when i.IsForcePaper = 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.IsForcePaper,'') != ISNULL(d.IsForcePaper,'')

--When User Update IsDontPrint
IF Update(IsDontPrint)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Is Dont Print',(case when d.IsDontPrint = 1 then 'Yes' else 'No' end),(case when i.IsDontPrint = 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.IsDontPrint,'') != ISNULL(d.IsDontPrint,'')

--When User Update ReferralNum
IF Update(ReferralNum)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Referral Number',d.ReferralNum,i.ReferralNum,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ReferralNum,'') != ISNULL(d.ReferralNum,'')

--When User Update OnsetDateOfIllness
IF Update(OnsetDateOfIllness)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'On set Date Of Illness',d.OnsetDateOfIllness,i.OnsetDateOfIllness,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.OnsetDateOfIllness,'') != ISNULL(d.OnsetDateOfIllness,'')


--When User Update FirstDateOfSimiliarIllness
IF Update(FirstDateOfSimiliarIllness)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'First Date Of Similiar Illness',d.FirstDateOfSimiliarIllness,i.FirstDateOfSimiliarIllness,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.FirstDateOfSimiliarIllness,'') != ISNULL(d.FirstDateOfSimiliarIllness,'')


--When User Update IllnessTreatmentDate
IF Update(IllnessTreatmentDate)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Illness Treatment Date',d.IllnessTreatmentDate,i.IllnessTreatmentDate,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.IllnessTreatmentDate,'') != ISNULL(d.IllnessTreatmentDate,'')


--When User Update DateOfPregnancy
IF Update(DateOfPregnancy)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Date Of Pregnancy',d.DateOfPregnancy,i.DateOfPregnancy,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.DateOfPregnancy,'') != ISNULL(d.DateOfPregnancy,'')


--When User Update AdmissionDate
IF Update(AdmissionDate)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Admission Date',d.AdmissionDate,i.AdmissionDate,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.AdmissionDate,'') != ISNULL(d.AdmissionDate,'')


--When User Update DischargeDate
IF Update(DischargeDate)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Discharge Date',d.DischargeDate,i.DischargeDate,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.DischargeDate,'') != ISNULL(d.DischargeDate,'')


--When User Update LastXrayDate
IF Update(LastXrayDate)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Last Xray Date',d.LastXrayDate,i.LastXrayDate,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.LastXrayDate,'') != ISNULL(d.LastXrayDate,'')


--When User Update LastXrayType
IF Update(LastXrayType)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Last Xray Type',d.LastXrayType,i.LastXrayType,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.LastXrayType,'') != ISNULL(d.LastXrayType,'')


--When User Update UnableToWorkFromDate
IF Update(UnableToWorkFromDate)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Unable To Work From Date',d.UnableToWorkFromDate,i.UnableToWorkFromDate,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.UnableToWorkFromDate,'') != ISNULL(d.UnableToWorkFromDate,'')


--When User Update UnableToWorkToDate
IF Update(UnableToWorkToDate)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Unable To Work To Date',d.UnableToWorkToDate,i.UnableToWorkToDate,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.UnableToWorkToDate,'') != ISNULL(d.UnableToWorkToDate,'')


--When User Update AccidentDate
IF Update(AccidentDate)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Accident Date',d.AccidentDate,i.AccidentDate,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.AccidentDate,'') != ISNULL(d.AccidentDate,'')


--When User Update AccidentState
IF Update(AccidentState)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Accident State',d.AccidentState,i.AccidentState,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.AccidentState,'') != ISNULL(d.AccidentState,'')


--When User Update AccidentType
IF Update(AccidentType)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Accident Type',d.AccidentType,i.AccidentType,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.AccidentType,'') != ISNULL(d.AccidentType,'')


--When User Update CliaNumber
IF Update(CliaNumber)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'CLIA Number',d.CliaNumber,i.CliaNumber,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.CliaNumber,'') != ISNULL(d.CliaNumber,'')


--When User Update OutsideLab
IF Update(OutsideLab)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Outside Laboratory',(case when d.OutsideLab = 1 then 'Yes' else 'No' end),(case when i.OutsideLab = 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.OutsideLab,'') != ISNULL(d.OutsideLab,'')


--When User Update LabCharges
IF Update(LabCharges)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Laboratory Charges',d.LabCharges,i.LabCharges,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.LabCharges,'1') != ISNULL(d.LabCharges,'1')


--When User Update ClaimNotes
IF Update(ClaimNotes)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Claim Notes',d.ClaimNotes,i.ClaimNotes,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ClaimNotes,'') != ISNULL(d.ClaimNotes,'')


--When User Update PayerClaimControlNum
IF Update(PayerClaimControlNum)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payer Claim Control Number',d.PayerClaimControlNum,i.PayerClaimControlNum,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PayerClaimControlNum,'') != ISNULL(d.PayerClaimControlNum,'')


--When User Update ClaimFrequencyCode
IF Update(ClaimFrequencyCode)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Claim Frequency Code',d.ClaimFrequencyCode,i.ClaimFrequencyCode,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ClaimFrequencyCode,'') != ISNULL(d.ClaimFrequencyCode,'')


--When User Update ServiceAuthExcpCode
IF Update(ServiceAuthExcpCode)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Service Author ExcpCode',d.ServiceAuthExcpCode,i.ServiceAuthExcpCode,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ServiceAuthExcpCode,'') != ISNULL(d.ServiceAuthExcpCode,'')


--When User Update ValidationMessage
IF Update(ValidationMessage)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Validation Message',d.ValidationMessage,i.ValidationMessage,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ValidationMessage,'') != ISNULL(d.ValidationMessage,'')


--When User Update Emergency
IF Update(Emergency)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Emergency',(case when d.Emergency = 1 then 'Yes' else 'No' end),(case when i.Emergency = 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Emergency,'') != ISNULL(d.Emergency,'')


--When User Update EPSDT
IF Update(EPSDT)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'EPSDT',(case when d.EPSDT = 1 then 'Yes' else 'No' end),(case when i.EPSDT = 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.EPSDT,'') != ISNULL(d.EPSDT,'')


--When User Update FamilyPlan
IF Update(FamilyPlan)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Family Plan',(case when d.FamilyPlan = 1 then 'Yes' else 'No' end),(case when i.FamilyPlan = 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.FamilyPlan,'') != ISNULL(d.FamilyPlan,'')



--When User Update TotalAmount
IF Update(TotalAmount)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Total Amount',d.TotalAmount,i.TotalAmount,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TotalAmount,'1') != ISNULL(d.TotalAmount,'1')


--When User Update Copay
IF Update(Copay)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Copay',d.Copay,i.Copay,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Copay,'1') != ISNULL(d.Copay,'1')

--When User Update Coinsurance
IF Update(Coinsurance)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Coinsurance',d.Coinsurance,i.Coinsurance,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Coinsurance,'1') != ISNULL(d.Coinsurance,'1')

--When User Update Deductible
IF Update(Deductible)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Deductible',d.Deductible,i.Deductible,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Deductible,'1') != ISNULL(d.Deductible,'1')

--When User Update PrimaryStatus
IF Update(PrimaryStatus)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Status',(case d.PrimaryStatus when 'N' then 'Regular' when 'S' then 'Submitted' when 'K' then 'Batch File Accepted' when 'D' then 'Batch File Denied' when 'E' then 'Batch File has Errors' when 'P' then 'Paid' when 'DN' then 'Denial' when 'PT_P' then 'Patient Paid' when 'PPTS' then 'Transefered to Sec' when 'PPTT' then 'Paid-Transfered To Ter' when 'PPTP' then 'Transfered to Patient' when 'SPTP' then 'Paid-Transfered To Ter' when 'SPTT' then 'Paid-Transfered To Ter' when 'PR_TP' then 'Pat. Resp. Transferred to Pat' when 'DN' then 'Denied' when 'PPTM' then 'Paid-Medigaped' Else 'Medigaped' end),(case i.PrimaryStatus when 'N' then 'Regular' when 'S' then 'Submitted' when 'K' then 'Batch File Accepted' when 'D' then 'Batch File Denied' when 'E' then 'Batch File has Errors' when 'P' then 'Paid' when 'DN' then 'Denial' when 'PT_P' then 'Patient Paid' when 'PPTS' then 'Transefered to Sec' when 'PPTT' then 'Paid-Transfered To Ter' when 'PPTP' then 'Transfered to Patient' when 'SPTP' then 'Paid-Transfered To Ter' when 'SPTT' then 'Paid-Transfered To Ter' when 'PR_TP' then 'Pat. Resp. Transferred to Pat' when 'DN' then 'Denied' when 'PPTM' then 'Paid-Medigaped' Else 'Medigaped' end),HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryStatus,'') !=ISNULL( d.PrimaryStatus,'')


--When User Update PrimaryBilledAmount
IF Update(PrimaryBilledAmount)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Billed Amount',d.PrimaryBilledAmount,i.PrimaryBilledAmount,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryBilledAmount,'1') != ISNULL(d.PrimaryBilledAmount,'1')


--When User Update PrimaryAllowed
IF Update(PrimaryAllowed)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Allowed',d.PrimaryAllowed,i.PrimaryAllowed,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryAllowed,'1') != ISNULL(d.PrimaryAllowed,'1')


--When User Update PrimaryWriteOff
IF Update(PrimaryWriteOff)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Write Off',d.PrimaryWriteOff,i.PrimaryWriteOff,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryWriteOff,'1') != ISNULL(d.PrimaryWriteOff,'1')


--When User Update PrimaryPaid
IF Update(PrimaryPaid)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Paid',d.PrimaryPaid,i.PrimaryPaid,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryPaid,'1') != ISNULL(d.PrimaryPaid,'1')

--When User Update PrimaryBal
IF Update(PrimaryBal)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Balance',d.PrimaryBal,i.PrimaryBal,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryBal,'1') != ISNULL(d.PrimaryBal,'1')

--When User Update PrimaryPatientBal
IF Update(PrimaryPatientBal)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Patient Balance',d.PrimaryPatientBal,i.PrimaryPatientBal,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryPatientBal,'1') != ISNULL(d.PrimaryPatientBal,'1')

--When User Update PrimaryTransferred
IF Update(PrimaryTransferred)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Transferred',d.PrimaryTransferred,i.PrimaryTransferred,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryTransferred,'1') != ISNULL(d.PrimaryTransferred,'1')


--When User Update SecondaryStatus
IF Update(SecondaryStatus)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Status',d.SecondaryStatus,i.SecondaryStatus,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryStatus,'') != ISNULL(d.SecondaryStatus,'')


--When User Update SecondaryBilledAmount
IF Update(SecondaryBilledAmount)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Billed Amount',d.SecondaryBilledAmount,i.SecondaryBilledAmount,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryBilledAmount,'1') != ISNULL(d.SecondaryBilledAmount,'1')


--When User Update SecondaryAllowed
IF Update(SecondaryAllowed)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Allowed',d.SecondaryAllowed,i.SecondaryAllowed,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryAllowed,'1') != ISNULL(d.SecondaryAllowed,'1')


--When User Update SecondaryWriteOff
IF Update(SecondaryWriteOff)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Write Off',d.SecondaryWriteOff,i.SecondaryWriteOff,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryWriteOff,'1') != ISNULL(d.SecondaryWriteOff,'1')


--When User Update SecondaryPaid
IF Update(SecondaryPaid)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Paid',d.SecondaryPaid,i.SecondaryPaid,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryPaid,'1') != ISNULL(d.SecondaryPaid,'1')


--When User Update SecondaryPatResp
IF Update(SecondaryPatResp)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Patient Response',d.SecondaryPatResp,i.SecondaryPatResp,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryPatResp,'1') != ISNULL(d.SecondaryPatResp,'1')


--When User Update SecondaryBal
IF Update(SecondaryBal)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Balance',d.SecondaryBal,i.SecondaryBal,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryBal,'1') != ISNULL(d.SecondaryBal,'1')

--When User Update SecondaryPatientBal
IF Update(SecondaryPatientBal)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Patient Balance',d.SecondaryPatientBal,i.SecondaryPatientBal,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryPatientBal,'1') != ISNULL(d.SecondaryPatientBal,'1')

--When User Update SecondaryTransferred
IF Update(SecondaryTransferred)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Transferred',d.SecondaryTransferred,i.SecondaryTransferred,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryTransferred,'1') != ISNULL(d.SecondaryTransferred,'1')


--When User Update TertiaryStatus
IF Update(TertiaryStatus)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Status',d.TertiaryStatus,i.TertiaryStatus,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryStatus,'') != ISNULL(d.TertiaryStatus,'')


--When User Update TertiaryBilledAmount
IF Update(TertiaryBilledAmount)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Billed Amount',d.TertiaryBilledAmount,i.TertiaryBilledAmount,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryBilledAmount,'1') != ISNULL(d.TertiaryBilledAmount,'1')


--When User Update TertiaryAllowed
IF Update(TertiaryAllowed)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Allowed',d.TertiaryAllowed,i.TertiaryAllowed,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryAllowed,'1') != ISNULL(d.TertiaryAllowed,'1')


--When User Update TertiaryWriteOff
IF Update(TertiaryWriteOff)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Write Off',d.TertiaryWriteOff,i.TertiaryWriteOff,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryWriteOff,'1') != ISNULL(d.TertiaryWriteOff,'1')


--When User Update TertiaryPaid
IF Update(TertiaryPaid)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Paid',d.TertiaryPaid,i.TertiaryPaid,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryPaid,'1') != ISNULL(d.TertiaryPaid,'1')


--When User Update TertiaryPatResp
IF Update(TertiaryPatResp)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Patient Response',d.TertiaryPatResp,i.TertiaryPatResp,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryPatResp,'1') != ISNULL(d.TertiaryPatResp,'1')


--When User Update TertiaryBal
IF Update(TertiaryBal)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Balance',d.TertiaryBal,i.TertiaryBal,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryBal,'1') != ISNULL(d.TertiaryBal,'1')

--When User Update TertiaryPatientBal
IF Update(TertiaryPatientBal)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Patient Balance',d.TertiaryPatientBal,i.TertiaryPatientBal,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryPatientBal,'1') != ISNULL(d.TertiaryPatientBal,'1')

--When User Update TertiaryTransferred
IF Update(TertiaryTransferred)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Transferred',d.TertiaryTransferred,i.TertiaryTransferred,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryTransferred,'1') != ISNULL(d.TertiaryTransferred,'1')


--When User Update PatientAmount
IF Update(PatientAmount)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Patient Amount',d.PatientAmount,i.PatientAmount,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PatientAmount,'1') != ISNULL(d.PatientAmount,'1')


--When User Update PatientPaid
IF Update(PatientPaid)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Patient Paid',d.PatientPaid,i.PatientPaid,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PatientPaid,'1') != ISNULL(d.PatientPaid,'1')


--When User Update IsSubmitted
IF Update(IsSubmitted)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Submitted',(case when d.IsSubmitted = 1 then 'Yes' else 'No' end),(case when i.IsSubmitted = 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and  ISNULL(i.IsSubmitted,'') != ISNULL(d.IsSubmitted,'') 


--When User Update SubmittetdDate
IF Update(SubmittedDate)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Submitted Date',d.SubmittedDate,i.SubmittedDate,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SubmittedDate,'') != ISNULL(d.SubmittedDate,'')

--When User Update DateOfServiceFrom
IF Update(DateOfServiceFrom)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Date Of Service From',d.DateOfServiceFrom,i.DateOfServiceFrom,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.DateOfServiceFrom,'') != ISNULL(d.DateOfServiceFrom,'')

--When User Update DateOfServiceTo
IF Update(DateOfServiceTo)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Date Of Service To',d.DateOfServiceTo,i.DateOfServiceTo,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.DateOfServiceTo,'') != ISNULL(d.DateOfServiceTo,'')


END
GO


-- =============================================
-- Description:	<tgrStatusCodeAudit Trigger>
-- =============================================
IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrStatusCodeAudit')
DROP TRIGGER [dbo].[tgrStatusCodeAudit]
GO
Create TRIGGER [dbo].[tgrStatusCodeAudit]
ON [dbo].[StatusCode] after Update
AS 
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM StatusCodeAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM StatusCodeAudit  

--when update Description

if update(Description)
insert into StatusCodeAudit(StatusCodeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Description',d.Description,i.Description,HOST_NAME(),i.UpdateBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Description,'') != ISNULL(d.Description,'')

END
GO

--=====================================
--Description: <tgrSubmissionLogAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrSubmissionLogAudit')
DROP TRIGGER [dbo].[tgrSubmissionLogAudit]
GO
Create TRIGGER [dbo].[tgrSubmissionLogAudit]
ON [dbo].[SubmissionLog] after Update
AS 
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM SubmissionLogAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM SubmissionLogAudit  

--when update ClientID
if update(ClientID)
Select * into #t from Client
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Client ID',cDel.[Name],cUpd.[Name],d.ClientID,i.ClientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClientID,'') != ISNULL(d.ClientID,'')
Join Client as cDel on cDel.ID = ISNULL(d.ClientID,'')
join Client as cUpd on cUpd.ID = ISNULL(i.ClientID,'')

--when update ReceiverID
if update(ReceiverID)
Select * into #t1 from Receiver
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Receiver ID',cDel.[Name],cUpd.[Name],d.ReceiverID,i.ReceiverID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReceiverID,'') != ISNULL(d.ReceiverID,'')
Join Receiver as cDel on cDel.ID = ISNULL(d.ReceiverID,'')
join Receiver as cUpd on cUpd.ID = ISNULL(i.ReceiverID,'')

--when update SubmitterID
if update(SubmitterID)
Select * into #t2 from Submitter
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submitter ID',cDel.[Name],cUpd.[Name],d.SubmitterID,i.SubmitterID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmitterID,'') != ISNULL(d.SubmitterID,'')
Join Submitter as cDel on cDel.ID = ISNULL(d.SubmitterID,'')
join Submitter as cUpd on cUpd.ID = ISNULL(i.SubmitterID,'')

--when update SubmitType
if update(SubmitType)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submit Type',d.SubmitType,i.SubmitType,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmitType,'') != ISNULL(d.SubmitType,'')

--when update DownloadedFileID
if update(DownloadedFileID)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Downloaded FileID',d.DownloadedFileID,i.DownloadedFileID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DownloadedFileID,'') != ISNULL(d.DownloadedFileID,'')

--when update PdfPath
if update(PdfPath)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Pdf Path',d.PdfPath,i.PdfPath,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PdfPath,'') != ISNULL(d.PdfPath,'')

--when update FormType
if update(FormType)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Form Type',d.FormType,i.FormType,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FormType,'') != ISNULL(d.FormType,'')

--when update ISAControlNumber
if update(ISAControlNumber)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'ISA Control Number',d.ISAControlNumber,i.ISAControlNumber,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ISAControlNumber,'') != ISNULL(d.ISAControlNumber,'')

--when update ClaimCount
if update(ClaimCount)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Claim Count',d.ClaimCount,i.ClaimCount,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClaimCount,'') != ISNULL(d.ClaimCount,'')

--when update ClaimAmount
if update(ClaimAmount)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Claim Amount',d.ClaimAmount,i.ClaimAmount,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClaimAmount,'1') != ISNULL(d.ClaimAmount,'1')

--when update Notes
if update(Notes)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',d.Notes,i.Notes,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Notes,'') != ISNULL(d.Notes,'')

--when update Transaction837Path
if update(Transaction837Path)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Transaction837 Path',d.Transaction837Path,i.Transaction837Path,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Transaction837Path,'') != ISNULL(d.Transaction837Path,'')

--when update IK5_Status
if update(IK5_Status)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IK5_Status',d.IK5_Status,i.IK5_Status,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IK5_Status,'') != ISNULL(d.IK5_Status,'')

--when update AK9_Status
if update(AK9_Status)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'AK9_Status',d.AK9_Status,i.AK9_Status,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AK9_Status,'') != ISNULL(d.AK9_Status,'')

--when update NoOfTotalST
if update(NoOfTotalST)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'No Of Total ST',d.NoOfTotalST,i.NoOfTotalST,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.NoOfTotalST,'') != ISNULL(d.NoOfTotalST,'')

--when update NoOfReceivedST
if update(NoOfReceivedST)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Number Of Received ST',d.NoOfReceivedST,i.NoOfReceivedST,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.NoOfReceivedST,'') != ISNULL(d.NoOfReceivedST,'')

--when update NoOfAcceptedST
if update(NoOfAcceptedST)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'No Of Accepted ST',d.NoOfAcceptedST,i.NoOfAcceptedST,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.NoOfAcceptedST,'') != ISNULL(d.NoOfAcceptedST,'')

--when update IK5_ErrorCode
if update(IK5_ErrorCode)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IK5_Error Code',d.IK5_ErrorCode,i.IK5_ErrorCode,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IK5_ErrorCode,'') != ISNULL(d.IK5_ErrorCode,'')

--when update AK9_ErrorCode
if update(AK9_ErrorCode)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'AK9_Error Code',d.AK9_ErrorCode,i.AK9_ErrorCode,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AK9_ErrorCode,'') != ISNULL(d.AK9_ErrorCode,'')

--when update Transaction999Path
if update(Transaction999Path)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Transaction999 Path',d.Transaction999Path,i.Transaction999Path,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Transaction999Path,'') != ISNULL(d.Transaction999Path,'')

--when update Trasaction277CAPath
if update(Trasaction277CAPath)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Trasaction277 CAPath',d.Trasaction277CAPath,i.Trasaction277CAPath,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Trasaction277CAPath,'') != ISNULL(d.Trasaction277CAPath,'')
END
GO



--=====================================
--Description: <tgrSubmitterAudit Trigger>
--======================================
IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrSubmitterAudit')
DROP TRIGGER [dbo].tgrSubmitterAudit
GO
Create TRIGGER [dbo].[tgrSubmitterAudit]
ON [dbo].[Submitter] after Update
AS 
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM SubmitterAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM SubmitterAudit  

--when update Name
if update(Name)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',d.Name,i.Name,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Name,'') != ISNULL(d.Name,'')

--when update Address
if update(Address)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address',d.Address,i.Address,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Address,'') != ISNULL(d.Address,'')

--when update City
if update(City)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'City',d.City,i.City,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.City,'') != ISNULL(d.City,'')

--when update State
if update(State)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'State',d.State,i.State,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.State,'') != ISNULL(d.State,'')

--when update ZipCode
if update(ZipCode)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Zip Code',d.ZipCode,i.ZipCode,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ZipCode,'') != ISNULL(d.ZipCode,'')

--when update SubmissionUserName
if update(SubmissionUserName)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submission UserName',d.SubmissionUserName,i.SubmissionUserName,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmissionUserName,'') != ISNULL(d.SubmissionUserName,'')

--when update SubmissionPassword
if update(SubmissionPassword)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submission Password',d.SubmissionPassword,i.SubmissionPassword,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmissionPassword,'') != ISNULL(d.SubmissionPassword,'')

--when update ManualSubmission
if update(ManualSubmission)   
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Manual Submission',(case when d.ManualSubmission = 1 then 'Yes' else 'No' end),(case when i.ManualSubmission = 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ManualSubmission,'') != ISNULL(d.ManualSubmission,'')

--when update FileName
if update(FileName)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'File Name',d.FileName,i.FileName,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FileName,'') != ISNULL(d.FileName,'')

--when update X12_837_NM1_41_SubmitterName
if update(X12_837_NM1_41_SubmitterName)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_NM1_41_Submitter Name',d.X12_837_NM1_41_SubmitterName,i.X12_837_NM1_41_SubmitterName,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_837_NM1_41_SubmitterName,'') != ISNULL(d.X12_837_NM1_41_SubmitterName,'')

--when update X12_837_NM1_41_SubmitterID
if update(X12_837_NM1_41_SubmitterID)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_NM1_41_Submitter ID',d.X12_837_NM1_41_SubmitterID,i.X12_837_NM1_41_SubmitterID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_837_NM1_41_SubmitterID,'') != ISNULL(d.X12_837_NM1_41_SubmitterID,'')

--when update X12_837_ISA_02
if update(X12_837_ISA_02)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_ISA_02',d.X12_837_ISA_02,i.X12_837_ISA_02,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_837_ISA_02,'') != ISNULL(d.X12_837_ISA_02,'')

--when update X12_837_ISA_04
if update(X12_837_ISA_04)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_ISA_04',d.X12_837_ISA_04,i.X12_837_ISA_04,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_837_ISA_04,'') != ISNULL(d.X12_837_ISA_04,'')

--when update X12_837_ISA_06
if update(X12_837_ISA_06)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_ISA_06',d.X12_837_ISA_06,i.X12_837_ISA_06,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_837_ISA_06,'') != ISNULL(d.X12_837_ISA_06,'')

--when update X12_837_GS_02
if update(X12_837_GS_02)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_GS_02',d.X12_837_GS_02,i.X12_837_GS_02,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_837_GS_02,'') != ISNULL(d.X12_837_GS_02,'')

--when update SubmitterContactPerson
if update(SubmitterContactPerson)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submitter Contact Person',d.SubmitterContactPerson,i.SubmitterContactPerson,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmitterContactPerson,'') != ISNULL(d.SubmitterContactPerson,'')

--when update SubmitterContactNumber
if update(SubmitterContactNumber)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submitter Contact Number',d.SubmitterContactNumber,i.SubmitterContactNumber,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmitterContactNumber,'') != ISNULL(d.SubmitterContactNumber,'')

--when update SubmitterEmail
if update(SubmitterEmail)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submitter Email',d.SubmitterEmail,i.SubmitterEmail,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmitterEmail,'') != ISNULL(d.SubmitterEmail,'')

--when update SubmitterFaxNumber
if update(SubmitterFaxNumber)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submitter Fax Number',d.SubmitterFaxNumber,i.SubmitterFaxNumber,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmitterFaxNumber,'') != ISNULL(d.SubmitterFaxNumber,'')

--when update X12_270_NM1_41_SubmitterName
if update(X12_270_NM1_41_SubmitterName)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_NM1_41_Submitter Name',d.X12_270_NM1_41_SubmitterName,i.X12_270_NM1_41_SubmitterName,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_270_NM1_41_SubmitterName,'') != ISNULL(d.X12_270_NM1_41_SubmitterName,'')

--when update X12_270_NM1_41_SubmitterID
if update(X12_270_NM1_41_SubmitterID)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_NM1_41_Submitter ID',d.X12_270_NM1_41_SubmitterID,i.X12_270_NM1_41_SubmitterID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_270_NM1_41_SubmitterID,'') != ISNULL(d.X12_270_NM1_41_SubmitterID,'')

--when update X12_270_ISA_02
if update(X12_270_ISA_02)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_ISA_02',d.X12_270_ISA_02,i.X12_270_ISA_02,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_270_ISA_02,'') != ISNULL(d.X12_270_ISA_02,'')

--when update X12_270_ISA_04
if update(X12_270_ISA_04)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_ISA_04',d.X12_270_ISA_04,i.X12_270_ISA_04,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_270_ISA_04,'') != ISNULL(d.X12_270_ISA_04,'')

--when update X12_270_ISA_06
if update(X12_270_ISA_06)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_ISA_06',d.X12_270_ISA_06,i.X12_270_ISA_06,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_270_ISA_06,'') != ISNULL(d.X12_270_ISA_06,'')

--when update X12_270_GS_02
if update(X12_270_GS_02)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_GS_02',d.X12_270_GS_02,i.X12_270_GS_02,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_270_GS_02,'') != ISNULL(d.X12_270_GS_02,'')

--when update X12_276_NM1_41_SubmitterName
if update(X12_276_NM1_41_SubmitterName)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_NM1_41_Submitter Name',d.X12_276_NM1_41_SubmitterName,i.X12_276_NM1_41_SubmitterName,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_276_NM1_41_SubmitterName,'') != ISNULL(d.X12_276_NM1_41_SubmitterName,'')

--when update X12_276_NM1_41_SubmitterID
if update(X12_276_NM1_41_SubmitterID)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_NM1_41_Submitter ID',d.X12_276_NM1_41_SubmitterID,i.X12_276_NM1_41_SubmitterID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_276_NM1_41_SubmitterID,'') != ISNULL(d.X12_276_NM1_41_SubmitterID,'')

--when update X12_276_ISA_02
if update(X12_276_ISA_02)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_ISA_02',d.X12_276_ISA_02,i.X12_276_ISA_02,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_276_ISA_02,'') != ISNULL(d.X12_276_ISA_02,'')

--when update X12_276_ISA_04
if update(X12_276_ISA_04)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_ISA_04',d.X12_276_ISA_04,i.X12_276_ISA_04,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_276_ISA_04,'') != ISNULL(d.X12_276_ISA_04,'')

--when update X12_276_ISA_06
if update(X12_276_ISA_06)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_ISA_06',d.X12_276_ISA_06,i.X12_276_ISA_06,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_276_ISA_06,'') != ISNULL(d.X12_276_ISA_06,'')

--when update X12_276_GS_02
if update(X12_276_GS_02)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_GS_02',d.X12_276_GS_02,i.X12_276_GS_02,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_276_GS_02,'') != ISNULL(d.X12_276_GS_02,'')

--when update ReceiverID
if update(ReceiverID)
Select * into #t1 from Receiver
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Receiver ID',cDel.[Name],cUpd.[Name],d.ReceiverID,i.ReceiverID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReceiverID,'') != ISNULL(d.ReceiverID,'')
Join Receiver as cDel on cDel.ID = ISNULL(d.ReceiverID,'')
join Receiver as cUpd on cUpd.ID = ISNULL(i.ReceiverID,'')

--when update ClientID
if update(ClientID)
Select * into #t from Client
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Client ID',cDel.[Name],cUpd.[Name],d.ClientID,i.ClientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClientID,'') != ISNULL(d.ClientID,'')
Join Client as cDel on cDel.ID = ISNULL(d.ClientID,'')
join Client as cUpd on cUpd.ID = ISNULL(i.ClientID,'')

END
GO

-- =============================================
-- Description:	<tgrTaxonomyAudit Trigger>
-- =============================================
IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrTaxonomyAudit')
DROP TRIGGER [dbo].[tgrTaxonomyAudit]
GO
Create TRIGGER [dbo].[tgrTaxonomyAudit]
ON [dbo].[Taxonomy] after Update
AS 
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM TaxonomyAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM TaxonomyAudit  

--when update Speciality
if update(Speciality)
insert into TaxonomyAudit(TaxonomyID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Speciality',d.Speciality,i.Speciality,HOST_NAME(),i.UpdateBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Speciality,'') != ISNULL(d.Speciality,'')

--when update AMADescription
if update(AMADescription)
insert into TaxonomyAudit(TaxonomyID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'AMA Description',d.AMADescription,i.AMADescription,HOST_NAME(),i.UpdateBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AMADescription,'') != ISNULL(d.AMADescription,'')

--when update SpecialityType
if update(SpecialityType)
insert into TaxonomyAudit(TaxonomyID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Speciality Type',d.SpecialityType,i.SpecialityType,HOST_NAME(),i.UpdateBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SpecialityType,'') != ISNULL(d.SpecialityType,'')

--when update NUCCCode
if update(NUCCCode)
insert into TaxonomyAudit(TaxonomyID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'NUCC Code',d.NUCCCode,i.NUCCCode,HOST_NAME(),i.UpdateBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.NUCCCode,'') != ISNULL(d.NUCCCode,'')

--when update NUCCDescription
if update(NUCCDescription)
insert into TaxonomyAudit(TaxonomyID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'NUCC Description',d.NUCCDescription,i.NUCCDescription,HOST_NAME(),i.UpdateBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.NUCCDescription,'') != ISNULL(d.NUCCDescription,'')

END
GO

--=====================================
--Description: <tgrUserPracticeAudit Trigger>
--======================================
IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrAdjustmentCodeAudit')
DROP TRIGGER [dbo].[tgrAdjustmentCodeAudit]
GO
create TRIGGER [dbo].[tgrAdjustmentCodeAudit]
ON [dbo].[AdjustmentCode] after Update
AS 
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM AdjustmentCodeAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM AdjustmentCodeAudit  

--when update Code
if update(Code)
insert into AdjustmentCodeAudit(AdjusmentCodeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Code',d.Code,i.Code,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Code,'') != ISNULL(d.Code,'')

--when update Description
if update(Description)
insert into AdjustmentCodeAudit(AdjusmentCodeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Description',d.Description,i.Description,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Description,'') != ISNULL(d.Description,'')

--when update Type
if update(Type)
insert into AdjustmentCodeAudit(AdjusmentCodeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Type',d.Type,i.Type,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Type,'') != ISNULL(d.Type,'')

--when update ActionID
if update(ActionID)
Select * into #t from Action
insert into AdjustmentCodeAudit(AdjusmentCodeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Action ID',cDel.[Name],cUpd.[Name],d.ActionID,i.ActionID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ActionID,'') != ISNULL(d.ActionID,'')
Join Action as cDel on cDel.ID = ISNULL(d.ActionID,'')
join Action as cUpd on cUpd.ID = ISNULL(i.ActionID,'')

--when update ReasonID
if update(ReasonID)
Select * into #t1 from Reason
insert into AdjustmentCodeAudit(AdjusmentCodeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reason ID',cDel.[Name],cUpd.[Name],d.ReasonID,i.ReasonID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReasonID,'') != ISNULL(d.ReasonID,'')
Join Reason as cDel on cDel.ID = ISNULL(d.ReasonID,'')
join Reason as cUpd on cUpd.ID = ISNULL(i.ReasonID,'')

--when update GroupID
if update(GroupID)
Select * into #t3 from [Group]
insert into AdjustmentCodeAudit(AdjusmentCodeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Group ID',cDel.[Name],cUpd.[Name],d.GroupID,i.GroupID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.GroupID,'') != ISNULL(d.GroupID,'')
Join [Group] as cDel on cDel.ID = ISNULL(d.GroupID,'')
join [Group] as cUpd on cUpd.ID = ISNULL(i.GroupID,'')
END
GO


--=====================================
--Description: <tgrActionAudit Trigger>
--======================================
IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrActionAudit')
DROP TRIGGER [dbo].[tgrActionAudit]
GO
Create TRIGGER [dbo].[tgrActionAudit]
ON [dbo].[Action] after Update
AS 
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM ActionAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM ActionAudit  

--when update Name
if update(Name)
insert into ActionAudit(ActionID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',d.Name,i.Name,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Name,'') != ISNULL(d.Name,'')

--when update Description
if update(Description)
insert into ActionAudit(ActionID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Description',d.Description,i.Description,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Description,'') != ISNULL(d.Description,'')

--when update UserID
if update(UserID)
select * into #t2 from AspNetUsers
insert into ActionAudit(ActionID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Email',cDel.[Email],cUpd.[Email],d.UserID,i.UserID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.UserID,'') != ISNULL(d.UserID,'')
Join AspNetUsers as cDel on cDel.ID = ISNULL(d.UserID,'')
join AspNetUsers as cUpd on cUpd.ID = ISNULL(i.UserID,'')
END
GO

--=====================================
--Description: <tgrChargeSubmissionHistory Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrChargeSubmissionHistory')
DROP TRIGGER [dbo].[tgrChargeSubmissionHistory]
GO
Create TRIGGER [dbo].[tgrChargeSubmissionHistory]
ON [dbo].[ChargeSubmissionHistory] after Update
AS 
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM [dbo].[ChargeSubmissionHistoryAudit]
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM [dbo].[ChargeSubmissionHistoryAudit]

--when update ChargeID
if update(ChargeID)
insert into ChargeSubmissionHistoryAudit(ChargeSubmissionHistoryID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Charge ID',d.ChargeID,i.ChargeID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ChargeID,'') != ISNULL(d.ChargeID,'')

--when update ReceiverID
if update(ReceiverID)
Select * into #t1 from Receiver
insert into ChargeSubmissionHistoryAudit(ChargeSubmissionHistoryID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Receiver ID',cDel.[Name],cUpd.[Name],d.ReceiverID,i.ReceiverID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReceiverID,'') != ISNULL(d.ReceiverID,'')
Join Receiver as cDel on cDel.ID = ISNULL(d.ReceiverID,'')
join Receiver as cUpd on cUpd.ID = ISNULL(i.ReceiverID,'')

--when update SubmissionLogID
if update(SubmissionLogID)
insert into ChargeSubmissionHistoryAudit(ChargeSubmissionHistoryID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submission Log ID',d.SubmissionLogID,i.SubmissionLogID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmissionLogID,'') != ISNULL(d.SubmissionLogID,'')

--when update SubmitType
if update(SubmitType)
insert into ChargeSubmissionHistoryAudit(ChargeSubmissionHistoryID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submit Type',d.SubmitType,i.SubmitType,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmitType,'') != ISNULL(d.SubmitType,'')

--when update FormType
if update(FormType)
insert into ChargeSubmissionHistoryAudit(ChargeSubmissionHistoryID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Form Type',d.FormType,i.FormType,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FormType,'') != ISNULL(d.FormType,'')

--when update PatientPlanID
if update(PatientPlanID)
insert into ChargeSubmissionHistoryAudit(ChargeSubmissionHistoryID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Plan ID',d.PatientPlanID,i.PatientPlanID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientPlanID,'') != ISNULL(d.PatientPlanID,'')

--when update Amount
if update(Amount)
insert into ChargeSubmissionHistoryAudit(ChargeSubmissionHistoryID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Amount',d.Amount,i.Amount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Amount,'1') != ISNULL(d.Amount,'1')

END
GO


--=====================================
--Description: <tgrUserPracticeAudit Trigger>
--======================================

--IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrUserPracticeAudit')
--DROP TRIGGER [dbo].[tgrUserPracticeAudit]
--GO
--Create TRIGGER [dbo].[tgrUserPracticeAudit]
--ON [dbo].[UserPractices] after Update
--AS 
--BEGIN
--DECLARE @TransactionID INT;
--SELECT @TransactionID =  max(TransactionID) FROM [UserPracticeAudit]
--IF @TransactionID is null set @TransactionID = 1 
--ELSE
--SELECT @TransactionID =  max(TransactionID) +1 FROM [UserPracticeAudit]

----When User Update PracticeID
--IF Update(PracticeID)
--INSERT INTO UserPracticeAudit(UserID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
--SELECT  i.UserID, @TransactionID,'Practice ID',pDel.[Name],pUpd.[Name],d.PracticeID,i.PracticeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
--FROM inserted as i
--INNER JOIN Deleted as d ON i.UserID = d.UserID and ISNULL(i.PracticeID,'') != ISNULL(d.PracticeID,'')
--Join Practice as pDel on pdel.ID = ISNULL(d.PracticeID,'')
--join Practice as pUpd on pUpd.ID = ISNULL(i.PracticeID,'')

----When User Update AssignedByUserId
--IF Update(AssignedByUserId)
--INSERT INTO UserPracticeAudit(UserID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
--SELECT  i.UserID, @TransactionID,'Assigned By UserId',d.AssignedByUserId,i.AssignedByUserId,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
--FROM inserted as i
--INNER JOIN Deleted as d ON i.UserID = d.UserID and ISNULL(i.AssignedByUserId,'') != ISNULL(d.AssignedByUserId,'')

----When User Update UPALastModified 
--IF Update(UPALastModified )
--INSERT INTO UserPracticeAudit(UserID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
--SELECT  i.UserID, @TransactionID,'UPA Last Modified ',d.UPALastModified ,i.UPALastModified ,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
--FROM inserted as i
--INNER JOIN Deleted as d ON i.UserID = d.UserID and ISNULL(i.UPALastModified,'')  != ISNULL(d.UPALastModified ,'')
--END
--GO

--=====================================
--Description: <tgrDesignation Trigger>
--======================================
IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrDesignationsAudit')
DROP TRIGGER [dbo].[tgrDesignationsAudit]
GO
Create TRIGGER [dbo].[tgrDesignationsAudit]
ON [dbo].[Designations] after Update  
As
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM DesignationsAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM DesignationsAudit 

--when update Name
if update(Name)
insert into DesignationsAudit(DesignationsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',d.Name,i.Name,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Name,'') != ISNULL(d.Name,'')


--=====================================
--Description: <tgrDownloadedFileAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrDownloadedFileAudit')
DROP TRIGGER [dbo].[tgrDownloadedFileAudit]
GO
Alter TRIGGER [dbo].[tgrDownloadedFileAudit]
ON [dbo].[DownloadedFile] after Update
AS 
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM DownloadedFileAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM DownloadedFileAudit  

--when update ReportsLogID
if update(ReportsLogID)
insert into DownloadedFileAudit(DownloadedFileID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reports Log ID',d.ReportsLogID,i.ReportsLogID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReportsLogID,'') != ISNULL(d.ReportsLogID,'')

--when update FilePath
if update(FilePath)
insert into DownloadedFileAudit(DownloadedFileID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'File Path',d.FilePath,i.FilePath,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FilePath,'') != ISNULL(d.FilePath,'')

--when update FileType
if update(FileType)
insert into DownloadedFileAudit(DownloadedFileID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'File Type',d.FileType,i.FileType,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FileType,'') != ISNULL(d.FileType,'')

--when update Processed
if update(Processed)
insert into DownloadedFileAudit(DownloadedFileID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Processed',d.Processed,i.Processed,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Processed,'') != ISNULL(d.Processed,'')
END
GO

--=====================================
--Description: <tgrGroupAudit Trigger>
--======================================
IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrGroupAudit')
DROP TRIGGER [dbo].[tgrGroupAudit]
GO
Create TRIGGER [dbo].[tgrGroupAudit]
ON [dbo].[Group] after Update
AS 
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM [dbo].[GroupAudit]
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM [dbo].[GroupAudit]

--when update Name
if update(Name)
insert into GroupAudit(GroupID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',d.Name,i.Name,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Name,'') != ISNULL(d.Name,'')

--when update Description
if update(Description)
insert into GroupAudit(GroupID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Description',d.Description,i.Description,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Description,'') != ISNULL(d.Description,'')

--when update UserID
if update(UserID)
Select * into #t from AspNetUsers
insert into GroupAudit(GroupID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'User ID',d.UserID,i.UserID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.UserID,'') != ISNULL(d.UserID,'')
Join AspNetUsers as cDel on cDel.ID = ISNULL(d.UserID,'')
join AspNetUsers as cUpd on cUpd.ID = ISNULL(i.UserID,'')

END
GO

--=====================================
--Description: <tgrNotesAudit Trigger>
--======================================
IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrNotesAudit')
DROP TRIGGER [dbo].[tgrNotesAudit]
GO
Create TRIGGER [dbo].[tgrNotesAudit]
ON [dbo].[Notes] after Update
AS 
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM NotesAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM NotesAudit  

--when update PracticeID
if update(PracticeID)
Select * into #t1 from Receiver
insert into NotesAudit(NotesID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Practice ID',cDel.[Name],cUpd.[Name],d.PracticeID,i.PracticeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PracticeID,'') != ISNULL(d.PracticeID,'')
Join Receiver as cDel on cDel.ID = ISNULL(d.PracticeID,'')
join Receiver as cUpd on cUpd.ID = ISNULL(i.PracticeID,'')

--when update PlanFollowupID
if update(PlanFollowupID)
insert into NotesAudit(NotesID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Plan Followup ID',d.PlanFollowupID,i.PlanFollowupID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PlanFollowupID,'') != ISNULL(d.PlanFollowupID,'')

--when update PatientFollowUpID
if update(PatientFollowUpID)
insert into NotesAudit(NotesID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient FollowUp ID',d.PatientFollowUpID,i.PatientFollowUpID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientFollowUpID,'') != ISNULL(d.PatientFollowUpID,'')

--when update Note
if update(Note)
insert into NotesAudit(NotesID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Note',d.Note,i.Note,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Note,'') != ISNULL(d.Note,'')


--when update NotesDate
if update(NotesDate)
insert into NotesAudit(NotesID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes Date',d.NotesDate,i.NotesDate,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.NotesDate,'') != ISNULL(d.NotesDate,'')
END
GO

--=====================================
--Description: <tgrPlanFollowupAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrPlanFollowupAudit')
DROP TRIGGER [dbo].[tgrPlanFollowupAudit]
GO
Create TRIGGER [dbo].[tgrPlanFollowupAudit]
ON [dbo].[PlanFollowup] after Update
AS 
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM PlanFollowupAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM PlanFollowupAudit  

--when update AdjustmentCodeID
if update(AdjustmentCodeID)
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Code ID',d.AdjustmentCodeID,i.AdjustmentCodeID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentCodeID,'') != ISNULL(d.AdjustmentCodeID,'')

--when update VisitID
if update(VisitID)
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Visit ID',d.VisitID,i.VisitID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.VisitID,'') != ISNULL(d.VisitID,'')

--when update GroupID
if update(GroupID)
Select * into #t from [Group]
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Group ID',cDel.[Name],cUpd.[Name],d.GroupID,i.GroupID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.GroupID,'') != ISNULL(d.GroupID,'')
Join [Group] as cDel on cDel.ID = ISNULL(d.GroupID,'')
join [Group] as cUpd on cUpd.ID = ISNULL(i.GroupID,'')

--when update ReasonID
if update(ReasonID)
Select * into #t1 from Reason
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reason ID',cDel.[Name],cUpd.[Name],d.ReasonID,i.ReasonID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReasonID,'') != ISNULL(d.ReasonID,'')
Join Reason as cDel on cDel.ID = ISNULL(d.ReasonID,'')
join Reason as cUpd on cUpd.ID = ISNULL(i.ReasonID,'')

--when update ActionID
if update(ActionID)
Select * into #t2 from Action
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Action ID',cDel.[Name],cUpd.[Name],d.ActionID,i.ActionID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ActionID,'') != ISNULL(d.ActionID,'')
Join Action as cDel on cDel.ID = ISNULL(d.ActionID,'')
join Action as cUpd on cUpd.ID = ISNULL(i.ActionID,'')

--when update VisitStatusID
if update(VisitStatusID)
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Visit Status ID',d.VisitStatusID,i.VisitStatusID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.VisitStatusID,'') != ISNULL(d.VisitStatusID,'')

--when update PaymentVisitID
if update(PaymentVisitID)
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payment Visit ID',d.PaymentVisitID,i.PaymentVisitID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaymentVisitID,'') != ISNULL(d.PaymentVisitID,'')

--when update Notes
if update(Notes)
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',d.Notes,i.Notes,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Notes,'') != ISNULL(d.Notes,'')

--when update TickleDate
if update(TickleDate)
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Tickle Date',d.TickleDate,i.TickleDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.TickleDate,'') != ISNULL(d.TickleDate,'')

--when update Resolved
if update(Resolved)
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Resolved',d.Resolved,i.Resolved,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Resolved,'') != ISNULL(d.Resolved,'')
END
GO

--=====================================
--Description: <tgrPatientAppointmentAudit Trigger>
--======================================
IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrPatientAppointmentAudit')
DROP TRIGGER [dbo].[tgrPatientAppointmentAudit]
GO
Create TRIGGER [dbo].[tgrPatientAppointmentAudit]
ON [dbo].[PatientAppointment] after Update
AS 
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM PatientAppointmentAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM PatientAppointmentAudit  

--when update PatientID
if update(PatientID)
Select * into #t from Receiver
insert into PatientAppointmentAudit(PatientAppointmentID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Practice ID',cDel.[Name],cUpd.[Name],d.PatientID,i.PatientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientID,'') != ISNULL(d.PatientID,'')
Join Receiver as cDel on cDel.ID = ISNULL(d.PatientID,'')
join Receiver as cUpd on cUpd.ID = ISNULL(i.PatientID,'')

--when update LocationID
if update(LocationID)
Select * into #t1 from Location
insert into PatientAppointmentAudit(PatientAppointmentID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Location ID',cDel.[Name],cUpd.[Name],d.LocationID,i.LocationID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.LocationID,'') != ISNULL(d.LocationID,'')
Join Location as cDel on cDel.ID = ISNULL(d.LocationID,'')
join Location as cUpd on cUpd.ID = ISNULL(i.LocationID,'')

--when update ProviderID
if update(ProviderID)
Select * into #t2 from Provider
insert into PatientAppointmentAudit(PatientAppointmentID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Provider ID',cDel.[Name],cUpd.[Name],d.ProviderID,i.ProviderID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProviderID,'') != ISNULL(d.ProviderID,'')
Join Provider as cDel on cDel.ID = ISNULL(d.ProviderID,'')
join Provider as cUpd on cUpd.ID = ISNULL(i.ProviderID,'')

--when update PatientID
if update(PatientID)
Select * into #t3 from Receiver
insert into PatientAppointmentAudit(PatientAppointmentID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Practice ID',cDel.[Name],cUpd.[Name],d.PatientID,i.PatientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientID,'') != ISNULL(d.PatientID,'')
Join Receiver as cDel on cDel.ID = ISNULL(d.PatientID,'')
join Receiver as cUpd on cUpd.ID = ISNULL(i.PatientID,'')

--when update ProviderSlotID
if update(ProviderSlotID)
insert into PatientAppointmentAudit(PatientAppointmentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Provider Slot ID',d.ProviderSlotID,i.ProviderSlotID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProviderSlotID,'') != ISNULL(d.ProviderSlotID,'')

--when update PrimarypatientPlanID
if update(PrimarypatientPlanID)
Select * Into #t4 from PatientPlan
Select * Into #t5 from InsurancePlan 
insert into PatientAppointmentAudit(PatientAppointmentID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Primary Patient Plan ID',dInsPlan.PlanName,uInsPlan.PlanName,d.PrimarypatientPlanID,i.PrimarypatientPlanID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PrimarypatientPlanID,'') != ISNULL(d.PrimarypatientPlanID,'')
join PatientPlan as dpatPlan on ISNULL(dpatPlan.ID,'') = ISNULL(d.PrimaryPatientPlanID,'')
join InsurancePlan as dInsPlan on ISNULL(dpatPlan.InsurancePlanID,'') = ISNULL(dInsPlan.ID,'')

left join PatientPlan as upatPlan on ISNULL(upatPlan.ID,'') = ISNULL(i.PrimaryPatientPlanID,'')
left join InsurancePlan as uInsPlan on ISNULL(upatPlan.InsurancePlanID,'') = ISNULL(uInsPlan.ID,'')


--when update VisitReasonID
if update(VisitReasonID)
Select * into #t6 from VisitReason
insert into PatientAppointmentAudit(PatientAppointmentID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Visit Reason ID',cDel.[Name],cUpd.[Name],d.ProviderSlotID,i.ProviderSlotID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.VisitReasonID,'') != ISNULL(d.VisitReasonID,'')
Join VisitReason as cDel on cDel.ID = ISNULL(d.VisitReasonID,'')
join VisitReason as cUpd on cUpd.ID = ISNULL(i.VisitReasonID,'')

--when update AppointmentDate
if update(AppointmentDate)
insert into PatientAppointmentAudit(PatientAppointmentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Appointment Date',d.AppointmentDate,i.AppointmentDate,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AppointmentDate,'') != ISNULL(d.AppointmentDate,'')

--when update Time
if update(Time)
insert into PatientAppointmentAudit(PatientAppointmentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Time',d.Time,i.Time,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Time,'') != ISNULL(d.Time,'')

--when update VisitInterval
if update(VisitInterval)
insert into PatientAppointmentAudit(PatientAppointmentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Visit Interval',d.VisitInterval,i.VisitInterval,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.VisitInterval,'') != ISNULL(d.VisitInterval,'')

--when update Status
if update(Status)
insert into PatientAppointmentAudit(PatientAppointmentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Status',d.Status,i.Status,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Status,'') != ISNULL(d.Status,'')

--when update Notes
if update(Notes)
insert into PatientAppointmentAudit(PatientAppointmentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',d.Notes,i.Notes,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Notes,'') != ISNULL(d.Notes,'')
END
GO

--=====================================
-- Description: <tgrPatientPaymentChargeAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrPatientPaymentChargeAudit')
DROP TRIGGER [dbo].[tgrPatientPaymentChargeAudit]
GO

Create TRIGGER [dbo].[tgrPatientPaymentChargeAudit]
ON [dbo].[PatientPaymentCharge] after Update  
As
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM PatientPaymentChargeAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM PatientPaymentChargeAudit 

--when update PatientPaymentID
if update(PatientPaymentID)
insert into PatientPaymentChargeAudit(PatientPaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Payment ID',d.PatientPaymentID,i.PatientPaymentID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientPaymentID,'') != ISNULL(d.PatientPaymentID,'')

--when update VisitID
if update(VisitID)
insert into PatientPaymentChargeAudit(PatientPaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Visit ID',d.VisitID,i.VisitID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.VisitID,'') != ISNULL(d.VisitID,'')

--when update ChargeID
if update(ChargeID)
insert into PatientPaymentChargeAudit(PatientPaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Charge ID',d.ChargeID,i.ChargeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ChargeID,'') != ISNULL(d.ChargeID,'')

--when update AllocatedAmount
if update(AllocatedAmount)
insert into PatientPaymentChargeAudit(PatientPaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Allocated Amount',d.AllocatedAmount,i.AllocatedAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AllocatedAmount,'') != ISNULL(d.AllocatedAmount,'')

--when update Status
if update(Status)
insert into PatientPaymentChargeAudit(PatientPaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Status',d.Status,i.Status,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Status,'') != ISNULL(d.Status,'')
GO

--=====================================
--Description: <tgrPatientPaymentAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrPatientPaymentAudit')
DROP TRIGGER [dbo].[tgrPatientPaymentAudit]
GO
Create TRIGGER [dbo].[tgrPatientPaymentAudit]
ON [dbo].[PatientPayment] after Update  
As
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM PatientPaymentAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM PatientPaymentAudit 

--when update PatientID
if update(PatientID)
Select * into #t6 from Patient
insert into PatientPaymentAudit(PatientPaymentID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient ID',cDel.[FirstName],cUpd.[FirstName],d.PatientID,i.PatientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientID,'') != ISNULL(d.PatientID,'')
Join Patient as cDel on cDel.ID = ISNULL(d.PatientID,'')
join Patient as cUpd on cUpd.ID = ISNULL(i.PatientID,'')

--when update PaymentMethod
if update(PaymentMethod)
insert into PatientPaymentAudit(PatientPaymentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payment Method',d.PaymentMethod,i.PaymentMethod,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaymentMethod,'') != ISNULL(d.PaymentMethod,'')

--when update PaymentDate
if update(PaymentDate)
insert into PatientPaymentAudit(PatientPaymentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payment Date',d.PaymentDate,i.PaymentDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaymentDate,'') != ISNULL(d.PaymentDate,'')

--when update PaymentAmount
if update(PaymentAmount)
insert into PatientPaymentAudit(PatientPaymentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payment Amount',d.PaymentAmount,i.PaymentAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaymentAmount,'') != ISNULL(d.PaymentAmount,'')

--when update CheckNumber
if update(CheckNumber)
insert into PatientPaymentAudit(PatientPaymentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Check Number',d.CheckNumber,i.CheckNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CheckNumber,'') != ISNULL(d.CheckNumber,'')

--when update Description
if update(Description)
insert into PatientPaymentAudit(PatientPaymentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Description',d.Description,i.Description,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Description,'') != ISNULL(d.Description,'')

--when update Status
if update(Status)
insert into PatientPaymentAudit(PatientPaymentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Status',d.Status,i.Status,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Status,'') != ISNULL(d.Status,'')

--when update AllocatedAmount
if update(AllocatedAmount)
insert into PatientPaymentAudit(PatientPaymentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Allocated Amount',d.AllocatedAmount,i.AllocatedAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AllocatedAmount,'') != ISNULL(d.AllocatedAmount,'')

--when update RemainingAmount
if update(RemainingAmount)
insert into PatientPaymentAudit(PatientPaymentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Remaining Amount',d.RemainingAmount,i.RemainingAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RemainingAmount,'') != ISNULL(d.RemainingAmount,'')

GO


--=====================================
--Description: <tgrReciever Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrReceiverAudit')
DROP TRIGGER [dbo].[tgrReceiverAudit]
GO
Create TRIGGER [dbo].[tgrReceiverAudit]
ON [dbo].[Receiver] after Update  
As
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM ReceiverAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM ReceiverAudit 

--when update Name
if update(Name)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',d.Name,i.Name,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Name,'') != ISNULL(d.Name,'')

--when update Address1
if update(Address1)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address1',d.Address1,i.Address1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Address1,'') != ISNULL(d.Address1,'')

--when update Address2
if update(Address2)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address2',d.Address2,i.Address2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Address2,'') != ISNULL(d.Address2,'')

--when update PhoneNumber
if update(PhoneNumber)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Phone Number',d.PhoneNumber,i.PhoneNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PhoneNumber,'') != ISNULL(d.PhoneNumber,'')

--when update FaxNumber
if update(FaxNumber)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Fax Number',d.PhoneNumber,i.PhoneNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FaxNumber,'') != ISNULL(d.FaxNumber,'')

--when update Email
if update(Email)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Email',d.Email,i.Email,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Email,'') != ISNULL(d.Email,'')


--when update Website
if update(Website)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Website',d.Website,i.Website,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Website,'') != ISNULL(d.Website,'')
 
--when update City
if update (City)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'City',d.City,i.City,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
Inner join deleted as d on i.ID = d.ID and ISNULL(i.City,'') != ISNULL(d.City,'')

--when update State
if update(State)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'State',d.State,i.State,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
Inner join deleted as d on i.ID = d.ID and ISNULL(i.State,'') != ISNULL(d.State,'')

--when update ZipCode
if update(Name)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Zip Code',d.ZipCode,i.ZipCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ZipCode,'') != ISNULL(d.ZipCode,'')

--when update SubmissionMethod
if update(SubmissionMethod)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submission Method',d.SubmissionMethod,i.SubmissionMethod,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmissionMethod,'') != ISNULL(d.SubmissionMethod,'')

--when update SubmissionURL
if update(SubmissionURL)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submission URL',d.SubmissionURL,i.SubmissionURL,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmissionURL,'') != ISNULL(d.SubmissionURL,'')

--when update SubmissionPort
if update(SubmissionPort)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submission Port',d.SubmissionPort,i.SubmissionPort,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmissionPort,'') != ISNULL(d.SubmissionPort,'')

--when update SubmissionDirectory
if update(SubmissionDirectory)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submission Directory',d.SubmissionDirectory,i.SubmissionDirectory,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmissionDirectory,'') != ISNULL(d.SubmissionDirectory,'')

--when update ReportsDirectory
if update(ReportsDirectory)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reports Directory',d.ReportsDirectory,i.ReportsDirectory,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReportsDirectory,'') != ISNULL(d.ReportsDirectory,'')

--when update ErasDirectory
if update(ErasDirectory)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Eras Directory',d.ErasDirectory,i.ErasDirectory,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ErasDirectory,'') != ISNULL(d.ErasDirectory,'')

--when update X12_837_NM1_40_ReceiverName
if update(X12_837_NM1_40_ReceiverName)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_NM1_40_Receiver Name',d.X12_837_NM1_40_ReceiverName,i.X12_837_NM1_40_ReceiverName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_837_NM1_40_ReceiverName,'') != ISNULL(d.X12_837_NM1_40_ReceiverName,'')

--when update X12_837_NM1_40_ReceiverID
if update(X12_837_NM1_40_ReceiverID)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_NM1_40_Receiver ID',d.X12_837_NM1_40_ReceiverID,i.X12_837_NM1_40_ReceiverID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_837_NM1_40_ReceiverID,'') != ISNULL( d.X12_837_NM1_40_ReceiverID,'')

--when update X12_837_ISA_01
if update(X12_837_ISA_01)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_ISA_01',d.X12_837_ISA_01,i.X12_837_ISA_01,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_837_ISA_01,'') != ISNULL(d.X12_837_ISA_01,'')

--when update X12_837_ISA_03
if update(X12_837_ISA_03)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_ISA_03',d.X12_837_ISA_03,i.X12_837_ISA_03,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_837_ISA_03,'') != ISNULL(d.X12_837_ISA_03,'')

--when update X12_837_ISA_05
if update(X12_837_ISA_05)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_ISA_05',d.X12_837_ISA_05,i.X12_837_ISA_05,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_837_ISA_05,'') != ISNULL(d.X12_837_ISA_05,'')

--when update X12_837_ISA_07
if update(X12_837_ISA_07)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_ISA_07',d.X12_837_ISA_07,i.X12_837_ISA_07,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_837_ISA_07,'') != ISNULL(d.X12_837_ISA_07,'')

--when update X12_837_ISA_08
if update(X12_837_ISA_08)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_ISA_08',d.X12_837_ISA_08,i.X12_837_ISA_08,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_837_ISA_08,'') != ISNULL(d.X12_837_ISA_08,'')

--when update X12_837_GS_03
if update(X12_837_GS_03)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_GS_03',d.X12_837_GS_03,i.X12_837_GS_03,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_837_GS_03,'') != ISNULL(d.X12_837_GS_03,'')

--when update X12_270_NM1_40_ReceiverName
if update(X12_270_NM1_40_ReceiverName)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_NM1_40_Receiver Name',d.X12_270_NM1_40_ReceiverName,i.X12_270_NM1_40_ReceiverName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_270_NM1_40_ReceiverName,'') != ISNULL(d.X12_270_NM1_40_ReceiverName,'')

--when update X12_270_NM1_40_ReceiverID
if update(X12_270_NM1_40_ReceiverID)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_NM1_40_Receiver ID',d.X12_270_NM1_40_ReceiverID,i.X12_270_NM1_40_ReceiverID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_270_NM1_40_ReceiverID,'') != ISNULL(d.X12_270_NM1_40_ReceiverID,'')

--when update X12_270_ISA_01
if update(X12_270_ISA_01)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_ISA_01',d.X12_270_ISA_01,i.X12_270_ISA_01,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_270_ISA_01,'') != ISNULL(d.X12_270_ISA_01,'')

--when update X12_270_ISA_03
if update(X12_270_ISA_03)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_ISA_03',d.X12_270_ISA_03,i.X12_270_ISA_03,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_270_ISA_03,'') != ISNULL(d.X12_270_ISA_03,'')

--when update X12_270_ISA_05
if update(X12_270_ISA_05)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_ISA_05',d.X12_270_ISA_05,i.X12_270_ISA_05,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_270_ISA_05,'') != ISNULL(d.X12_270_ISA_05,'')

--when update X12_270_ISA_07
if update(X12_270_ISA_07)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_ISA_07',d.X12_270_ISA_07,i.X12_270_ISA_07,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_270_ISA_07,'') != ISNULL(d.X12_270_ISA_07,'')

--when update X12_270_ISA_08
if update(Name)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_ISA_08',d.X12_270_ISA_08,i.X12_270_ISA_08,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_270_ISA_08,'') != ISNULL(d.X12_270_ISA_08,'')

--when update X12_270_GS_03
if update(X12_270_GS_03)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_GS_03',d.X12_270_GS_03,i.X12_270_GS_03,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_270_GS_03,'') != ISNULL(d.X12_270_GS_03,'')

--when update X12_276_NM1_40_ReceiverName
if update(X12_276_NM1_40_ReceiverName)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_NM1_40_Receiver Name',d.X12_276_NM1_40_ReceiverName,i.X12_276_NM1_40_ReceiverName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_276_NM1_40_ReceiverName,'') != ISNULL(d.X12_276_NM1_40_ReceiverName,'')

--when update X12_276_NM1_40_ReceiverID
if update(X12_276_NM1_40_ReceiverID)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_NM1_40_Receiver ID',d.X12_276_NM1_40_ReceiverID,i.X12_276_NM1_40_ReceiverID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_276_NM1_40_ReceiverID,'') != ISNULL(d.X12_276_NM1_40_ReceiverID,'')

--when update X12_276_ISA_01
if update(X12_276_ISA_01)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_ISA_01',d.X12_276_ISA_01,i.X12_276_ISA_01,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_276_ISA_01,'') != ISNULL(d.X12_276_ISA_01,'')

--when update X12_276_ISA_03
if update(X12_276_ISA_03)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_ISA_03',d.X12_276_ISA_03,i.X12_276_ISA_03,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_276_ISA_03,'') != ISNULL(d.X12_276_ISA_03,'')

--when update X12_276_ISA_05
if update(X12_276_ISA_05)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_ISA_05',d.X12_276_ISA_05,i.X12_276_ISA_05,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_276_ISA_05,'') != ISNULL(d.X12_276_ISA_05,'')

--when update X12_276_ISA_07
if update(X12_276_ISA_07)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_ISA_07',d.X12_276_ISA_07,i.X12_276_ISA_07,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_276_ISA_07,'') != ISNULL(d.X12_276_ISA_07,'')

--when update X12_276_ISA_08
if update(X12_276_ISA_08)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_ISA_08',d.X12_276_ISA_08,i.X12_276_ISA_08,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_276_ISA_08,'') != ISNULL(d.X12_276_ISA_08,'')

--when update X12_276_GS_03
if update(X12_276_GS_03)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_GS_03',d.X12_276_GS_03,i.X12_276_GS_03,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_276_GS_03,'') != ISNULL(d.X12_276_GS_03,'')

--when update ElementSeperator
if update(ElementSeperator)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Element Seperator',d.ElementSeperator,i.ElementSeperator,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ElementSeperator,'') != ISNULL(d.ElementSeperator,'')

--when update SegmentSeperator
if update(SegmentSeperator)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Segment Seperator',d.SegmentSeperator,i.SegmentSeperator,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SegmentSeperator,'') != ISNULL(d.SegmentSeperator,'')

--when update SubElementSeperator
if update(SubElementSeperator)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Sub Element Seperator',d.SubElementSeperator,i.SubElementSeperator,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubElementSeperator,'') != ISNULL(d.SubElementSeperator,'')

--when update RepetitionSepeator
if update(RepetitionSepeator)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Repetition Sepeator',d.RepetitionSepeator,i.RepetitionSepeator,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RepetitionSepeator,'') != ISNULL(d.RepetitionSepeator,'')
GO


--=====================================
--Description: <tgrPaymentCheckAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrPaymentCheckAudit')
DROP TRIGGER [dbo].[tgrPaymentCheckAudit]
GO
create   TRIGGER [dbo].[tgrPaymentCheckAudit]
ON [dbo].[PaymentCheck] after Update
AS 
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM PaymentCheckAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM PaymentCheckAudit

--When User Update ReceiverID
IF Update(ReceiverID)
select * Into #t1 from Receiver
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Receiver ID',pDel.[Name],pUpd.[Name],d.ReceiverID,i.ReceiverID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ReceiverID,'') != ISNULL(d.ReceiverID,'')
Join Receiver as pDel on pdel.ID = ISNULL(d.ReceiverID,'')
join Receiver as pUpd on pUpd.ID = ISNULL(i.ReceiverID,'')

--When User Update PracticeID
IF Update(PracticeID)
select * Into #t2 from Practice
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Practice ID',pDel.[Name],pUpd.[Name],d.PracticeID,i.PracticeID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PracticeID,'') != ISNULL(d.PracticeID,'')
Join Practice as pDel on pdel.ID = ISNULL(d.PracticeID,'')
join Practice as pUpd on pUpd.ID = ISNULL(i.PracticeID,'')

--When User Update DownloadedFileID
IF Update(DownloadedFileID)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Downloaded File ID',d.DownloadedFileID,i.DownloadedFileID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.DownloadedFileID,'') != ISNULL(d.DownloadedFileID,'')


--When User Update CheckNumber
IF Update(CheckNumber)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Check Number',d.CheckNumber,i.CheckNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.CheckNumber,'') != ISNULL(d.CheckNumber,'')

--When User Update CheckDate
IF Update(CheckDate)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Check Date',d.CheckDate,i.CheckDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.CheckDate,'') != ISNULL(d.CheckDate,'')

--When User Update CheckAmount
IF Update(CheckAmount)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Check Amount',d.CheckAmount,i.CheckAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.CheckAmount,'1') != ISNULL(d.CheckAmount,'1')

--When User Update TransactionCode
IF Update(TransactionCode)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Transaction Code',d.TransactionCode,i.TransactionCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TransactionCode,'') != ISNULL(d.TransactionCode,'')

--When User Update CreditDebitFlag
IF Update(CreditDebitFlag)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Credit Debit Flag',d.CreditDebitFlag,i.CreditDebitFlag,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.CreditDebitFlag,'') != ISNULL(d.CreditDebitFlag,'')

--When User Update PaymentMethod
IF Update(PaymentMethod)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payment Method',d.PaymentMethod,i.PaymentMethod,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PaymentMethod,'') != ISNULL(d.PaymentMethod,'')

--When User Update PayerName
IF Update(PayerName)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payer Name',d.PayerName,i.PayerName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PayerName,'') != ISNULL(d.PayerName,'')

--When User Update Status
IF Update(Status)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Status',d.Status,i.Status,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Status,'') != ISNULL(d.Status,'')

--When User Update AppliedAmount
IF Update(AppliedAmount)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Applied Amount',d.AppliedAmount,i.AppliedAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.AppliedAmount,'1') != ISNULL(d.AppliedAmount,'1')

--When User Update PostedAmount
IF Update(PostedAmount)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Posted Amount',d.PostedAmount,i.PostedAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PostedAmount,'1') != ISNULL(d.PostedAmount,'1')

--When User Update PostedDate
IF Update(PostedDate)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Posted Date',d.PostedDate,i.PostedDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PostedDate,'') != ISNULL(d.PostedDate,'')

--When User Update PostedBy
IF Update(PostedBy)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Posted By',d.PostedBy,i.PostedBy,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PostedBy,'') != ISNULL(d.PostedBy,'')

--When User Update PayerID
IF Update(PayerID)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payer ID',d.PayerID,i.PayerID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PayerID,'') != ISNULL(d.PayerID,'')

--When User Update PayerAddress
IF Update(PayerAddress)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payer Address',d.PayerAddress,i.PayerAddress,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PayerAddress,'') != ISNULL(d.PayerAddress,'')

--When User Update PayerCity
IF Update(PayerCity)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payer City',d.PayerCity,i.PayerCity,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PayerCity,'') != ISNULL(d.PayerCity,'')

--When User Update PayerState
IF Update(PayerState)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payer State',d.PayerState,i.PayerState,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PayerState,'') != ISNULL(d.PayerState,'')

--When User Update PayerZipCode
IF Update(PayerZipCode)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payer Zip Code',d.PayerZipCode,i.PayerZipCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PayerZipCode,'') != ISNULL(d.PayerZipCode,'')

--When User Update REF_2U_ID
IF Update(REF_2U_ID)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'REF_2U_ID',d.REF_2U_ID,i.REF_2U_ID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.REF_2U_ID,'') != ISNULL(d.REF_2U_ID,'')


--When User Update PayerContactPerson
IF Update(PayerContactPerson)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payer Contact Person',d.PayerContactPerson,i.PayerContactPerson,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PayerContactPerson,'') != ISNULL(d.PayerContactPerson,'')

--When User Update PayerContactNumber
IF Update(PayerContactNumber)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payer Contact Number',d.PayerContactNumber,i.PayerContactNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PayerContactNumber,'') != ISNULL(d.PayerContactNumber,'')


--When User Update PayeeName
IF Update(PayeeName)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payee Name',d.PayeeName,i.PayeeName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PayeeName,'') != ISNULL(d.PayeeName,'')

--When User Update PayeeNPI
IF Update(PayeeNPI)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payee NPI',d.PayeeNPI,i.PayeeNPI,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PayeeNPI,'') != ISNULL(d.PayeeNPI,'')


--When User Update PayeeAddress
IF Update(PayeeAddress)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payee Address',d.PayeeAddress,i.PayeeAddress,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PayeeAddress,'') != ISNULL(d.PayeeAddress,'')

--When User Update PayeeCity
IF Update(PayeeCity)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payee City',d.PayeeCity,i.PayeeCity,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PayeeCity,'') != ISNULL(d.PayeeCity,'')


--When User Update PayeeState
IF Update(PayeeState)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payee State',d.PayeeState,i.PayeeState,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PayeeState,'') != ISNULL(d.PayeeState,'')

--When User Update PayerZipCode
IF Update(PayerZipCode)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payer Zip Code',d.PayerZipCode,i.PayerZipCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PayerZipCode,'') != ISNULL(d.PayerZipCode,'')


--When User Update PayeeTaxID
IF Update(PayeeTaxID)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payee Tax ID',d.PayeeTaxID,i.PayeeTaxID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PayeeTaxID,'') != ISNULL(d.PayeeTaxID,'')

--When User Update NumberOfVisits
IF Update(NumberOfVisits)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Number Of Visits',d.NumberOfVisits,i.NumberOfVisits,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.NumberOfVisits,'') != ISNULL(d.NumberOfVisits,'')

--When User Update NumberOfPatients
IF Update(NumberOfPatients)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Number Of Patients',d.NumberOfPatients,i.NumberOfPatients,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.NumberOfPatients,'') != ISNULL(d.NumberOfPatients,'')

--When User Update Comments
IF Update(Comments)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Comments',d.Comments,i.Comments,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Comments,'') != ISNULL(d.Comments,'')
END
GO

--=====================================
--Description: <tgrPaymentChargeAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrPaymentChargeAudit')
DROP TRIGGER [dbo].[tgrPaymentChargeAudit]
GO
Create TRIGGER [dbo].[tgrPaymentChargeAudit]
ON [dbo].[PaymentCharge] after Update  
As
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM PaymentChargeAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM PaymentChargeAudit 

--when update PaymentVisitID
if update(PaymentVisitID)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payment Visit ID',d.PaymentVisitID,i.PaymentVisitID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaymentVisitID,'') != ISNULL(d.PaymentVisitID,'')

--when update ChargeID
if update(ChargeID)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Charge ID',d.ChargeID,i.ChargeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ChargeID,'') != ISNULL(d.ChargeID,'')

--when update AppliedToSec
if update(AppliedToSec)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Applied To Sec',(case when d.AppliedToSec = 1 then 'Yes' else 'No' end),(case when i.AppliedToSec = 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AppliedToSec,'') != ISNULL(d.AppliedToSec,'')

--when update CPTCode
if update(CPTCode)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'CPT Code',d.CPTCode,i.CPTCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CPTCode,'') != ISNULL(d.CPTCode,'')

--when update Modifier1
if update(Modifier1)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Modifier 1',d.Modifier1,i.Modifier1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Modifier1,'') != ISNULL(d.Modifier1,'')

--when update Modifier2
if update(Modifier2)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Modifier 2',d.Modifier2,i.Modifier2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Modifier2,'') != ISNULL(d.Modifier2,'')

--when update Modifier3
if update(Modifier3)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Modifier 3',d.Modifier3,i.Modifier3,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Modifier3,'') != ISNULL(d.Modifier3,'')

--when update Modifier4
if update(Modifier4)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Modifier 4',d.Modifier4,i.Modifier4,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Modifier4,'') != ISNULL(d.Modifier4,'')

--when update BilledAmount
if update(BilledAmount)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Billed Amount',d.BilledAmount,i.BilledAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.BilledAmount,'1') != ISNULL(d.BilledAmount,'1')

--when update PaidAmount
if update(PaidAmount)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Paid Amount',d.PaidAmount,i.PaidAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaidAmount,'1') != ISNULL(d.PaidAmount,'1')

--when update RevenueCode
if update(RevenueCode)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Revenue Code',d.RevenueCode,i.RevenueCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RevenueCode,'') != ISNULL(d.RevenueCode,'')

--when update Units
if update(Units)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Units',d.Units,i.Units,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Units,'') != ISNULL(d.Units,'')

--when update DOSFrom
if update(DOSFrom)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'DOS From',d.DOSFrom,i.DOSFrom,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DOSFrom,'') != ISNULL(d.DOSFrom,'')

--when update DOSTo
if update(DOSTo)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'DOS To',d.DOSTo,i.DOSTo,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DOSTo,'') != ISNULL(d.DOSTo,'')

--when update ChargeControlNumber
if update(ChargeControlNumber)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Charge Control Number',d.ChargeControlNumber,i.ChargeControlNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ChargeControlNumber,'') != ISNULL(d.ChargeControlNumber,'')

--when update PatientAmount
if update(PatientAmount)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Amount',d.PatientAmount,i.PatientAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientAmount,'1') != ISNULL(d.PatientAmount,'1')

--when update DeductableAmount
if update(DeductableAmount)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Deductable Amount',d.DeductableAmount,i.DeductableAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DeductableAmount,'1') != ISNULL(d.DeductableAmount,'1')

--when update CoinsuranceAmount
if update(CoinsuranceAmount)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Coinsurance Amount',d.CoinsuranceAmount,i.CoinsuranceAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CoinsuranceAmount,'1') != ISNULL(d.CoinsuranceAmount,'1')

--when update WriteoffAmount
if update(WriteoffAmount)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Write off Amount',d.WriteoffAmount,i.WriteoffAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.WriteoffAmount,'1') != ISNULL(d.WriteoffAmount,'1')

--when update AllowedAmount
if update(AllowedAmount)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Allowed Amount',d.AllowedAmount,i.AllowedAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AllowedAmount,'1') != ISNULL(d.AllowedAmount,'1')

--when update AdjustmentCodeID1
if update(AdjustmentCodeID1)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Code ID1',d.AdjustmentCodeID1,i.AdjustmentCodeID1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentCodeID1,'') != ISNULL(d.AdjustmentCodeID1,'')

--when update GroupCode1
if update(GroupCode1)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Group Code1',d.GroupCode1,i.GroupCode1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.GroupCode1,'') != ISNULL(d.GroupCode1,'')

--when update AdjustmentAmount1
if update(AdjustmentAmount1)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Amount1 off',d.AdjustmentAmount1,i.AdjustmentAmount1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentAmount1,'1') != ISNULL(d.AdjustmentAmount1,'1')

--when update AdjustmentQuantity1
if update(AdjustmentQuantity1)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Quantity1',d.AdjustmentQuantity1,i.AdjustmentQuantity1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentQuantity1,'') != ISNULL(d.AdjustmentQuantity1,'')

--when update AdjustmentCodeID2
if update(AdjustmentCodeID2)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Code ID2',d.AdjustmentCodeID2,i.AdjustmentCodeID2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentCodeID2,'') != ISNULL(d.AdjustmentCodeID2,'')

--when update GroupCode2
if update(GroupCode2)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Group Code2',d.GroupCode2,i.GroupCode2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.GroupCode2,'') != ISNULL(d.GroupCode2,'')

--when update AdjustmentAmount2
if update(AdjustmentAmount2)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Amount2',d.AdjustmentAmount2,i.AdjustmentAmount2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentAmount2,'1') != ISNULL(d.AdjustmentAmount2,'1')

--when update AdjustmentQuantity2
if update(AdjustmentQuantity2)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Quantity2',d.AdjustmentQuantity2,i.AdjustmentQuantity2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentQuantity2,'') != ISNULL(d.AdjustmentQuantity2,'')

--when update AdjustmentCodeID3
if update(AdjustmentCodeID3)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Code ID3',d.AdjustmentCodeID3,i.AdjustmentCodeID3,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentCodeID3,'') != ISNULL(d.AdjustmentCodeID3,'')

--when update GroupCode3
if update(GroupCode3)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Group Code',d.GroupCode3,i.GroupCode3,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.GroupCode3,'') != ISNULL(d.GroupCode3,'')

--when update AdjustmentAmount3
if update(AdjustmentAmount3)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Amount3',d.AdjustmentAmount3,i.AdjustmentAmount3,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentAmount3,'1') != ISNULL(d.AdjustmentAmount3,'1')

--when update AdjustmentQuantity3
if update(AdjustmentQuantity3)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Quantity3',d.AdjustmentQuantity3,i.AdjustmentQuantity3,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentQuantity3,'') != ISNULL(d.AdjustmentQuantity3,'')

--when update AdjustmentCodeID4
if update(AdjustmentCodeID4)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Code ID4',d.AdjustmentCodeID4,i.AdjustmentCodeID4,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentCodeID4,'') != ISNULL(d.AdjustmentCodeID4,'')

--when update GroupCode4
if update(GroupCode4)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Group Code4',d.GroupCode4,i.GroupCode4,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.GroupCode4,'') != ISNULL(d.GroupCode4,'')

--when update AdjustmentAmount4
if update(AdjustmentAmount4)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Amount4',d.AdjustmentAmount4,i.AdjustmentAmount4,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentAmount4,'1') != ISNULL(d.AdjustmentAmount4,'1')

--when update AdjustmentQuantity4
if update(AdjustmentQuantity4)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Quantity4',d.AdjustmentQuantity4,i.AdjustmentQuantity4,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentQuantity4,'') != ISNULL(d.AdjustmentQuantity4,'')

--when update AdjustmentCodeID5
if update(AdjustmentCodeID5)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment CodeID5',d.AdjustmentCodeID5,i.AdjustmentCodeID5,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentCodeID5,'') != ISNULL(d.AdjustmentCodeID5,'')

--when update GroupCode5
if update(GroupCode5)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Group Code5',d.GroupCode5,i.GroupCode5,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.GroupCode5,'') != ISNULL(d.GroupCode5,'')

--when update AdjustmentAmount5
if update(AdjustmentAmount5)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Amount5',d.AdjustmentAmount5,i.AdjustmentAmount5,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentAmount5,'1') != ISNULL(d.AdjustmentAmount5,'1')

--when update AdjustmentQuantity5
if update(AdjustmentQuantity5)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Quantity5',d.AdjustmentQuantity5,i.AdjustmentQuantity5,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentQuantity5,'') != ISNULL(d.AdjustmentQuantity5,'')

--when update RemarkCodeID1
if update(AdjustmentAmount4)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Remark Code ID1',d.RemarkCodeID1,i.RemarkCodeID1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RemarkCodeID1,'') != ISNULL(d.RemarkCodeID1,'')

--when update RemarkCodeID2
if update(RemarkCodeID2)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Remark Code ID2',d.RemarkCodeID2,i.RemarkCodeID2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RemarkCodeID2,'') != ISNULL(d.RemarkCodeID2,'')

--when update RemarkCodeID3
if update(RemarkCodeID3)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Remark Code ID3',d.RemarkCodeID3,i.RemarkCodeID3,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RemarkCodeID3,'') != ISNULL(d.RemarkCodeID3,'')

--when update RemarkCodeID4
if update(RemarkCodeID4)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Remark Code ID4',d.RemarkCodeID4,i.RemarkCodeID4,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RemarkCodeID4,'') != ISNULL(d.RemarkCodeID4,'')


--when update RemarkCodeID5
if update(RemarkCodeID5)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Remark Code ID5',d.RemarkCodeID5,i.RemarkCodeID5,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RemarkCodeID5,'') != ISNULL(d.RemarkCodeID5,'')

--when update Status
if update(Status)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Status',d.Status,i.Status,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Status,'') != ISNULL(d.Status,'')
GO



--=====================================
--Description: <tgrPaymentLedgerAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrPaymentLedgerAudit')
DROP TRIGGER [dbo].[tgrPaymentLedgerAudit]
GO
Create TRIGGER [dbo].[tgrPaymentLedgerAudit]
ON [dbo].[PaymentLedger] after Update  
As
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM PaymentLedgerAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM PaymentLedgerAudit 


--when update PatientPlanID
if update(PatientPlanID)
Select * into #t1 from PatientPlan
insert into PaymentLedgerAudit(PaymentLedgerID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Plan ID',cDel.[FirstName],cUpd.[FirstName],d.PatientPlanID,i.PatientPlanID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientPlanID,'') != ISNULL(d.PatientPlanID,'')
Join PatientPlan as cDel on cDel.ID = ISNULL(d.PatientPlanID,'')
join PatientPlan as cUpd on cUpd.ID = ISNULL(i.PatientPlanID,'')

--when update ChargeID
if update(ChargeID)
insert into PaymentLedgerAudit(PaymentLedgerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Charge ID',d.ChargeID,i.ChargeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ChargeID,'') != ISNULL(d.ChargeID,'')

--when update VisitID
if update(VisitID)
insert into PaymentLedgerAudit(PaymentLedgerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Visit ID',d.VisitID,i.VisitID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.VisitID,'') != ISNULL(d.VisitID,'')

--when update PatientPaymentChargeID
if update(PatientPaymentChargeID)
insert into PaymentLedgerAudit(PaymentLedgerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Payment Charge ID',d.PatientPaymentChargeID,i.PatientPaymentChargeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientPaymentChargeID,'') != ISNULL(d.PatientPaymentChargeID,'')

--when update PaymentChargeID
if update(PaymentChargeID)
insert into PaymentLedgerAudit(PaymentLedgerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payment Charge ID',d.PaymentChargeID,i.PaymentChargeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaymentChargeID,'') != ISNULL(d.PaymentChargeID,'')

--when update AdjustmentCodeID
if update(AdjustmentCodeID)
insert into PaymentLedgerAudit(PaymentLedgerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Code ID',d.AdjustmentCodeID,i.AdjustmentCodeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentCodeID,'') != ISNULL(d.AdjustmentCodeID,'')

--when update LedgerDate
if update(LedgerDate)
insert into PaymentLedgerAudit(PaymentLedgerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Ledger Date',d.LedgerDate,i.LedgerDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.LedgerDate,'') != ISNULL(d.LedgerDate,'')

--when update LedgerBy
if update(LedgerBy)
insert into PaymentLedgerAudit(PaymentLedgerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Ledger By',d.LedgerBy,i.LedgerBy,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.LedgerBy,'') != ISNULL(d.LedgerBy,'')

--when update LedgerType
if update(LedgerType)
insert into PaymentLedgerAudit(PaymentLedgerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Ledger Type',d.LedgerType,i.LedgerType,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.LedgerType,'') != ISNULL(d.LedgerType,'')

--when update LedgerDescription
if update(LedgerDescription)
insert into PaymentLedgerAudit(PaymentLedgerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Ledger Description',d.LedgerDescription,i.LedgerDescription,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.LedgerDescription,'') != ISNULL(d.LedgerDescription,'')

--when update Amount
if update(Amount)
insert into PaymentLedgerAudit(PaymentLedgerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Amount',d.Amount,i.Amount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Amount,'1') != ISNULL(d.Amount,'1')

--when update Notes
if update(Notes)
insert into PaymentLedgerAudit(PaymentLedgerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',d.Notes,i.Notes,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Notes,'') != ISNULL(d.Notes,'')
GO



--=====================================
--Description: <tgrPaymentVisitAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrPaymentVisitAudit')
DROP TRIGGER [dbo].[tgrPaymentVisitAudit]
GO
Create TRIGGER [dbo].[tgrPaymentVisitAudit]
ON [dbo].[PaymentVisit] after Update  
As
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM PaymentVisitAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM PaymentVisitAudit 

--when update PaymentCheckID
if update(PaymentCheckID)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payment Check ID',d.PaymentCheckID,i.PaymentCheckID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaymentCheckID,'') != ISNULL(d.PaymentCheckID,'')

--when update VisitID
if update(VisitID)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Visit ID',d.VisitID,i.VisitID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.VisitID,'') != ISNULL(d.VisitID,'')

--when update ClaimNumber
if update(ClaimNumber)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Claim Number',d.ClaimNumber,i.ClaimNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClaimNumber,'') != ISNULL(d.ClaimNumber,'')

--when update PatientID
if update(PatientID)
Select * into #t1 from Patient
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient ID',cDel.[FirstName],cUpd.[FirstName],d.PatientID,i.PatientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientID,'') != ISNULL(d.PatientID,'')
Join Patient as cDel on cDel.ID = ISNULL(d.PatientID,'')
join Patient as cUpd on cUpd.ID = ISNULL(i.PatientID,'')

--when update BatchDocumentID
if update(BatchDocumentID)
Select * into #t2 from Patient
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Batch Document ID',cDel.[Description],cUpd.[Description],d.BatchDocumentID,i.BatchDocumentID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.BatchDocumentID,'') != ISNULL(d.BatchDocumentID,'')
Join BatchDocument as cDel on cDel.ID = ISNULL(d.BatchDocumentID,'')
join BatchDocument as cUpd on cUpd.ID = ISNULL(i.BatchDocumentID,'')

--when update PageNumber
if update(PageNumber)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Page Number',d.PageNumber,i.PageNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PageNumber,'') != ISNULL(d.PageNumber,'')

--when update MyProperty
if update(MyProperty)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'My Property',d.MyProperty,i.MyProperty,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.MyProperty,'') != ISNULL(d.MyProperty,'')

--when update ProcessedAs
if update(ProcessedAs)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Processed As',d.ProcessedAs,i.ProcessedAs,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProcessedAs,'') != ISNULL(d.ProcessedAs,'')

--when update BilledAmount
if update(BilledAmount)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Billed Amount',d.BilledAmount,i.BilledAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.BilledAmount,'1') != ISNULL(d.BilledAmount,'1')


--when update PaidAmount
if update(PaidAmount)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Paid Amount',d.PaidAmount,i.PaidAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaidAmount,'1') != ISNULL(d.PaidAmount,'1')

--when update AllowedAmount
if update(AllowedAmount)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Allowed Amount',d.AllowedAmount,i.AllowedAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AllowedAmount,'1') != ISNULL(d.AllowedAmount,'1')

--when update WriteOffAmount
if update(WriteOffAmount)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Write Off Amount',d.WriteOffAmount,i.WriteOffAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.WriteOffAmount,'1') != ISNULL(d.WriteOffAmount,'1')

--when update PatientAmount
if update(PatientAmount)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Amount',d.PatientAmount,i.PatientAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientAmount,'1') != ISNULL(d.PatientAmount,'1')

--when update PayerICN
if update(PayerICN)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer ICN',d.PayerICN,i.PayerICN,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PayerICN,'') != ISNULL(d.PayerICN,'')

--when update PatientLastName
if update(PatientLastName)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Last Name',d.PatientLastName,i.PatientLastName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientLastName,'') != ISNULL(d.PatientLastName,'')

--when update PatientFIrstName
if update(PatientFIrstName)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient First Name',d.PatientFIrstName,i.PatientFIrstName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientFIrstName,'1') != ISNULL(d.PatientFIrstName,'1')

--when update InsuredLastName
if update(InsuredLastName)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insured Last Name',d.InsuredLastName,i.InsuredLastName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsuredLastName,'') != ISNULL(d.InsuredLastName,'')

--when update InsuredFirstName
if update(InsuredFirstName)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insured First Name',d.InsuredFirstName,i.InsuredFirstName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsuredFirstName,'') != ISNULL(d.InsuredFirstName,'')

--when update InsuredID
if update(InsuredID)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insured ID',d.InsuredID,i.InsuredID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsuredID,'') != ISNULL(d.InsuredID,'')

--when update ProvLastName
if update(ProvLastName)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Provider Last Name',d.ProvLastName,i.ProvLastName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProvLastName,'') != ISNULL(d.ProvLastName,'')

--when update ProvFirstName
if update(ProvFirstName)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Provider First Name',d.ProvFirstName,i.ProvFirstName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProvFirstName,'') != ISNULL(d.ProvFirstName,'')

--when update ProvNPI
if update(ProvNPI)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Provider NPI',d.ProvNPI,i.ProvNPI,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProvNPI,'') != ISNULL(d.ProvNPI,'')

--when update PayerContactNumber
if update(PayerContactNumber)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer Contact Number',d.PayerContactNumber,i.PayerContactNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PayerContactNumber,'') != ISNULL(d.PayerContactNumber,'')

--when update ForwardedPayerName
if update(ForwardedPayerName)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Forwarded Payer Name',d.ForwardedPayerName,i.ForwardedPayerName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ForwardedPayerName,'') != ISNULL(d.ForwardedPayerName,'')

--when update ForwardedPayerID
if update(ForwardedPayerID)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Forwarded Payer ID',d.ForwardedPayerID,i.ForwardedPayerID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ForwardedPayerID,'') != ISNULL(d.ForwardedPayerID,'')

--when update ClaimStatementFromDate
if update(ClaimStatementFromDate)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Claim Statement From Date',d.ClaimStatementFromDate,i.ClaimStatementFromDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClaimStatementFromDate,'') != ISNULL(d.ClaimStatementFromDate,'')

--when update ClaimStatementToDate
if update(ClaimStatementToDate)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Claim Statement To Date',d.ClaimStatementToDate,i.ClaimStatementToDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClaimStatementToDate,'') != ISNULL(d.ClaimStatementToDate,'')

--when update PayerReceivedDate
if update(PayerReceivedDate)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer Received Date',d.PayerReceivedDate,i.PayerReceivedDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PayerReceivedDate,'') != ISNULL(d.PayerReceivedDate,'')

--when update Status
if update(Status)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Status',d.Status,i.Status,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Status,'') != ISNULL(d.Status,'')
GO




--=====================================
--Description: <tgrPlanFollowupAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrPlanFollowupAudit')
DROP TRIGGER [dbo].[tgrPlanFollowupAudit]
GO
Create TRIGGER [dbo].[tgrPlanFollowupAudit]
ON [dbo].[PlanFollowup] after Update  
As
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM PlanFollowupAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM PlanFollowupAudit 

--when update VisitID
if update(VisitID)
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Visit ID',d.VisitID,i.VisitID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.VisitID,'') != ISNULL(d.VisitID,'')

--when update GroupID
if update(GroupID)
Select * into #t1 from [Group]
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Group ID',cDel.[Name],cUpd.[Name],d.GroupID,i.GroupID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.GroupID,'') != ISNULL(d.GroupID,'')
Join [Group] as cDel on cDel.ID = ISNULL(d.GroupID,'')
join [Group] as cUpd on cUpd.ID = ISNULL(i.GroupID,'')

--when update ReasonID
if update(ReasonID)
Select * into #t2 from Reason
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reason ID',cDel.[Name],cUpd.[Name],d.ReasonID,i.ReasonID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReasonID,'') != ISNULL(d.ReasonID,'')
Join Reason as cDel on cDel.ID = ISNULL(d.ReasonID,'')
join Reason as cUpd on cUpd.ID = ISNULL(i.ReasonID,'')

--when update ActionID
if update(ActionID)
Select * into #t3 from [Action]
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Action ID',cDel.[Name],cUpd.[Name],d.ActionID,i.ActionID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ActionID,'') != ISNULL(d.ActionID,'')
Join [Action] as cDel on cDel.ID = ISNULL(d.ActionID,'')
join [Action] as cUpd on cUpd.ID = ISNULL(i.ActionID,'')

--when update AdjustmentCodeID
if update(AdjustmentCodeID)
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment CodeID',d.AdjustmentCodeID,i.AdjustmentCodeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentCodeID,'') != ISNULL(d.AdjustmentCodeID,'')

--when update VisitStatusID
if update(VisitStatusID)
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Visit Status ID',d.VisitStatusID,i.VisitStatusID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.VisitStatusID,'') != ISNULL(d.VisitStatusID,'')

--when update Notes
if update(Notes)
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',d.Notes,i.Notes,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Notes,'') != ISNULL(d.Notes,'')

--when update PaymentVisitID
if update(PaymentVisitID)
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payment VisitID',d.PaymentVisitID,i.PaymentVisitID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaymentVisitID,'') != ISNULL(d.PaymentVisitID,'')

--when update TickleDate
if update(TickleDate)
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Tickle Date',d.TickleDate,i.TickleDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.TickleDate,'') != ISNULL(d.TickleDate,'')

--when update Resolved
if update(Resolved)
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Resolved',(case when d.Resolved= 1 then 'Yes' else 'No' end),(case when i.Resolved= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Resolved,'') != ISNULL(d.Resolved,'')
GO


--=====================================
--Description: <tgrProviderScheduleAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrProviderScheduleAudit')
DROP TRIGGER [dbo].[tgrProviderScheduleAudit]
GO
Create TRIGGER [dbo].[tgrProviderScheduleAudit]
ON [dbo].[ProviderSchedule] after Update  
As
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM ProviderScheduleAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM ProviderScheduleAudit 

--when update ProviderID
if update(ProviderID)
Select * into #t1 from Provider
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Provider ID',cDel.[Name],cUpd.[Name],d.ProviderID,i.ProviderID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProviderID,'') != ISNULL(d.ProviderID,'')
Join Provider as cDel on cDel.ID = ISNULL(d.ProviderID,'')
join Provider as cUpd on cUpd.ID = ISNULL(i.ProviderID,'')

--when update LocationID
if update(LocationID)
Select * into #t2 from Location
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Location ID',cDel.[Name],cUpd.[Name],d.LocationID,i.LocationID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.LocationID,'') != ISNULL(d.LocationID,'')
Join Location as cDel on cDel.ID = ISNULL(d.LocationID,'')
join Location as cUpd on cUpd.ID = ISNULL(i.LocationID,'')


--when update FromDate
if update(FromDate)
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'From Date',d.FromDate,i.FromDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FromDate,'') != ISNULL(d.FromDate,'')

--when update ToDate
if update(ToDate)
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'To Date',d.ToDate,i.ToDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ToDate,'') != ISNULL(d.ToDate,'')

--when update FromTime
if update(FromTime)
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'From Time',d.FromTime,i.FromTime,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FromTime,'') != ISNULL(d.FromTime,'')

--when update ToTime
if update(ToTime)
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'To Time',d.ToTime,i.ToTime,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ToTime,'') != ISNULL(d.ToTime,'')

--when update TimeInterval
if update(TimeInterval)
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Time Interval',d.TimeInterval,i.TimeInterval,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.TimeInterval,'') != ISNULL(d.TimeInterval,'')

--when update OverBookAllowed
if update(OverBookAllowed)
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Over Book Allowed',(case when d.OverBookAllowed= 1 then 'Yes' else 'No' end),(case when i.OverBookAllowed= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.OverBookAllowed,'') != ISNULL(d.OverBookAllowed,'')

--when update Friday
if update(Friday)
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Friday',(case when d.Friday= 1 then 'Yes' else 'No' end),(case when i.Friday= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Friday,'') != ISNULL(d.Friday,'')

--when update Saturday
if update(Saturday)
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Saturday',(case when d.Saturday= 1 then 'Yes' else 'No' end),(case when i.Saturday= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Saturday,'') != ISNULL(d.Saturday,'')

--when update Sunday
if update(Sunday)
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Sunday',(case when d.Sunday= 1 then 'Yes' else 'No' end),(case when i.Sunday= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Sunday,'') != ISNULL(d.Sunday,'')

--when update Monday
if update(Monday)
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Monday',(case when d.Monday= 1 then 'Yes' else 'No' end),(case when i.Monday= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Monday,'') != ISNULL(d.Monday,'')

--when update Tuesday
if update(Tuesday)
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Tuesday',(case when d.Tuesday= 1 then 'Yes' else 'No' end),(case when i.Tuesday= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Tuesday,'') != ISNULL(d.Tuesday,'')

--when update Wednesday
if update(Wednesday)
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Wednesday',(case when d.Wednesday= 1 then 'Yes' else 'No' end),(case when i.Wednesday= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Wednesday,'') != ISNULL(d.Wednesday,'')

--when update Thursday
if update(Thursday)
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Thursday',(case when d.Thursday= 1 then 'Yes' else 'No' end),(case when i.Thursday= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Thursday,'') != ISNULL(d.Thursday,'')

--when update Notes
if update(Notes)
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',d.Notes,i.Notes,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Notes,'') != ISNULL(d.Notes,'')
GO

--=====================================
--Description: <tgrReasonAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrReasonAudit')
DROP TRIGGER [dbo].[tgrReasonAudit]
GO
Create TRIGGER [dbo].[tgrReasonAudit]
ON [dbo].[Reason] after Update  
As
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM ReasonAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM ReasonAudit 

--when update Name
if update(Name)
insert into ReasonAudit(ReasonID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',d.Name,i.Name,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Name,'') != ISNULL(d.Name,'')

--when update UserID
if update(UserID)
insert into ReasonAudit(ReasonID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'User ID',d.UserID,i.UserID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.UserID,'') != ISNULL(d.UserID,'')

--when update Name
if update(Name)
insert into ReasonAudit(ReasonID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Description',d.Description,i.Description,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Description,'') != ISNULL(d.Description,'')
GO

--=====================================
--Description: <tgrRemarkCodeAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrRemarkCodeAudit')
DROP TRIGGER [dbo].[tgrRemarkCodeAudit]
GO
Create TRIGGER [dbo].[tgrRemarkCodeAudit]
ON [dbo].[RemarkCode] after Update  
As
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM RemarkCodeAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM RemarkCodeAudit 

--when update Code
if update(Code)
insert into RemarkCodeAudit(RemarkCodeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Code',d.Code,i.Code,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Code,'') != ISNULL(d.Code,'')

--when update Description
if update(Description)
insert into RemarkCodeAudit(RemarkCodeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Description',d.Description,i.Description,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Description,'') != ISNULL(d.Description,'')
GO


--=====================================
--Description: <tgrResubmitHistoryAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrResubmitHistoryAudit')
DROP TRIGGER [dbo].[tgrResubmitHistoryAudit]
GO
Create TRIGGER [dbo].[tgrResubmitHistoryAudit]
ON [dbo].[ResubmitHistory] after Update  
As
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM ResubmitHistoryAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM ResubmitHistoryAudit 

--when update ChargeID
if update(ChargeID)
insert into ResubmitHistoryAudit(ResubmitHistoryID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Charge ID',d.ChargeID,i.ChargeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ChargeID,'') != ISNULL(d.ChargeID,'')

--when update VisitID
if update(VisitID)
insert into ResubmitHistoryAudit(ResubmitHistoryID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Visit ID',d.VisitID,i.VisitID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.VisitID,'') != ISNULL(d.VisitID,'')
GO


--=====================================
--Description: <tgrResubmitHistoryAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrResubmitHistoryAudit')
DROP TRIGGER [dbo].[tgrResubmitHistoryAudit]
GO
Create TRIGGER [dbo].[tgrResubmitHistoryAudit]
ON [dbo].[ResubmitHistory] after Update  
As
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM ResubmitHistoryAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM ResubmitHistoryAudit 

--when update ChargeID
if update(ChargeID)
insert into ResubmitHistoryAudit(ResubmitHistoryID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Charge ID',d.ChargeID,i.ChargeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ChargeID,'') != ISNULL(d.ChargeID,'')
GO


--=====================================
--Description: <tgrReportsLogAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrReportsLogAudit')
DROP TRIGGER [dbo].[tgrReportsLogAudit]
GO
Create TRIGGER [dbo].[tgrReportsLogAudit]
ON [dbo].[ReportsLog] after Update  
As
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM ReportsLogAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM ReportsLogAudit 

--when update ClientID
if update(ClientID)
insert into ReportsLogAudit(ReportsLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Client ID',d.ClientID,i.ClientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClientID,'') != ISNULL(d.ClientID,'')

--when update ReceiverID
if update(ReceiverID)
insert into ReportsLogAudit(ReportsLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Receiver ID',d.ReceiverID,i.ReceiverID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReceiverID,'') != ISNULL(d.ReceiverID,'')

--when update SubmitterID
if update(SubmitterID)
insert into ReportsLogAudit(ReportsLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submitter ID',d.SubmitterID,i.SubmitterID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmitterID,'') != ISNULL(d.SubmitterID,'')

--when update ZipFilePath
if update(ZipFilePath)
insert into ReportsLogAudit(ReportsLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Zip File Path',d.ZipFilePath,i.ZipFilePath,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ZipFilePath,'') != ISNULL(d.ZipFilePath,'')

--when update Processed
if update(Processed)
insert into ReportsLogAudit(ReportsLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Processed',d.Processed,i.Processed,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Processed,'') != ISNULL(d.Processed,'')

--when update UserResolved
if update(UserResolved)
insert into ReportsLogAudit(ReportsLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'User Resolved',d.UserResolved,i.UserResolved,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.UserResolved,'') != ISNULL(d.UserResolved,'')

--when update FilesCount
if update(FilesCount)
insert into ReportsLogAudit(ReportsLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Files Count',d.FilesCount,i.FilesCount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FilesCount,'') != ISNULL(d.FilesCount,'')

--when update ManualImport
if update(ManualImport)
insert into ReportsLogAudit(ReportsLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Manual Import',d.ManualImport,i.ManualImport,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ManualImport,'') != ISNULL(d.ManualImport,'')
GO




--=====================================
--Description: <tgrReportsLogAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrRightsAudit')
DROP TRIGGER [dbo].[tgrRightsAudit]
GO
Create TRIGGER [dbo].[tgrRightsAudit]
ON [dbo].[Rights] after Update  
As
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM RightsAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM RightsAudit 

--when update SchedulerCreate
if update(SchedulerCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Scheduler Create',(case when d.SchedulerCreate= 1 then 'Yes' else 'No' end),(case when i.SchedulerCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SchedulerCreate,'') != ISNULL(d.SchedulerCreate,'')

--when update SchedulerEdit
if update(SchedulerEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Scheduler Edit',(case when d.SchedulerEdit= 1 then 'Yes' else 'No' end),(case when i.SchedulerEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SchedulerEdit,'') != ISNULL(d.SchedulerEdit,'')

--when update SchedulerDelete
if update(SchedulerDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Scheduler Delete',(case when d.SchedulerDelete= 1 then 'Yes' else 'No' end),(case when i.SchedulerDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SchedulerDelete,'') != ISNULL(d.SchedulerDelete,'')

--when update SchedulerSearch
if update(SchedulerSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Scheduler Search',(case when d.SchedulerSearch= 1 then 'Yes' else 'No' end),(case when i.SchedulerSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SchedulerSearch,'') != ISNULL(d.SchedulerSearch,'')

--when update SchedulerImport
if update(SchedulerImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Scheduler Import',(case when d.SchedulerImport= 1 then 'Yes' else 'No' end),(case when i.SchedulerImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SchedulerImport,'') != ISNULL(d.SchedulerImport,'')

--when update SchedulerExport
if update(SchedulerExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Scheduler Export',(case when d.SchedulerExport= 1 then 'Yes' else 'No' end),(case when i.SchedulerExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SchedulerExport,'') != ISNULL(d.SchedulerExport,'')

--when update PatientCreate
if update(SchedulerCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Create',(case when d.PatientCreate= 1 then 'Yes' else 'No' end),(case when i.PatientCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientCreate,'') != ISNULL(d.PatientCreate,'')

--when update PatientEdit
if update(PatientEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Edit',(case when d.PatientEdit= 1 then 'Yes' else 'No' end),(case when i.PatientEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientEdit,'') != ISNULL(d.PatientEdit,'')

--when update PatientDelete
if update(PatientDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Delete',(case when d.PatientDelete= 1 then 'Yes' else 'No' end),(case when i.PatientDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientDelete,'') != ISNULL(d.PatientDelete,'')

--when update PatientSearch
if update(PatientSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Search',(case when d.PatientSearch= 1 then 'Yes' else 'No' end),(case when i.PatientSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientSearch,'') != ISNULL(d.PatientSearch,'')

--when update PatientImport
if update(PatientImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Import',(case when d.PatientImport= 1 then 'Yes' else 'No' end),(case when i.PatientImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientImport,'') != ISNULL(d.PatientImport,'')

--when update PatientExport
if update(PatientExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Export',(case when d.PatientExport= 1 then 'Yes' else 'No' end),(case when i.PatientExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientExport,'') != ISNULL(d.PatientExport,'')

--when update ChargesCreate
if update(ChargesCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Charges Create',(case when d.ChargesCreate= 1 then 'Yes' else 'No' end),(case when i.ChargesCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ChargesCreate,'') != ISNULL(d.ChargesCreate,'')

--when update ChargesEdit
if update(ChargesEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Charges Edit',(case when d.ChargesEdit= 1 then 'Yes' else 'No' end),(case when i.ChargesEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ChargesEdit,'') != ISNULL(d.ChargesEdit,'')

--when update ChargesDelete
if update(ChargesDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Charges Delete',(case when d.ChargesDelete= 1 then 'Yes' else 'No' end),(case when i.ChargesDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ChargesDelete,'') != ISNULL(d.ChargesDelete,'')

--when update ChargesSearch
if update(ChargesSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Charges Search',(case when d.ChargesSearch= 1 then 'Yes' else 'No' end),(case when i.ChargesSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ChargesSearch,'') != ISNULL(d.ChargesSearch,'')

--when update ChargesImport
if update(ChargesImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Charges Import',(case when d.ChargesImport= 1 then 'Yes' else 'No' end),(case when i.ChargesImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ChargesImport,'') != ISNULL(d.ChargesImport,'')

--when update ChargesExport
if update(ChargesExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Charges Export',(case when d.ChargesExport= 1 then 'Yes' else 'No' end),(case when i.ChargesExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ChargesExport,'') != ISNULL(d.ChargesExport,'')

--when update DocumentsCreate
if update(DocumentsCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Documents Create',(case when d.DocumentsCreate= 1 then 'Yes' else 'No' end),(case when i.DocumentsCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DocumentsCreate,'') != ISNULL(d.DocumentsCreate,'')

--when update DocumentsEdit
if update(SchedulerCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Documents Edit',(case when d.DocumentsEdit= 1 then 'Yes' else 'No' end),(case when i.DocumentsEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DocumentsEdit,'') != ISNULL(d.DocumentsEdit,'')

--when update DocumentsDelete
if update(SchedulerCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Documents Delete',(case when d.DocumentsDelete= 1 then 'Yes' else 'No' end),(case when i.DocumentsDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DocumentsDelete,'') != ISNULL(d.DocumentsDelete,'')

--when update DocumentsSearch
if update(DocumentsSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Documents Search',(case when d.DocumentsSearch= 1 then 'Yes' else 'No' end),(case when i.DocumentsSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DocumentsSearch,'') != ISNULL(d.DocumentsSearch,'')

--when update DocumentsImport
if update(DocumentsImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Documents Import',(case when d.DocumentsImport= 1 then 'Yes' else 'No' end),(case when i.DocumentsImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DocumentsImport,'') != ISNULL(d.DocumentsImport,'')

--when update DocumentsExport
if update(DocumentsExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Documents Export',(case when d.DocumentsExport= 1 then 'Yes' else 'No' end),(case when i.DocumentsExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DocumentsExport,'') != ISNULL(d.DocumentsExport,'')

--when update SubmissionsCreate
if update(SubmissionsCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submissions Create',(case when d.SubmissionsCreate= 1 then 'Yes' else 'No' end),(case when i.SubmissionsCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmissionsCreate,'') != ISNULL(d.SubmissionsCreate,'')

--when update SubmissionsEdit
if update(SubmissionsEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submissions Edit',(case when d.SubmissionsEdit= 1 then 'Yes' else 'No' end),(case when i.SubmissionsEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmissionsEdit,'') != ISNULL(d.SubmissionsEdit,'')

--when update SubmissionsDelete
if update(SubmissionsDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submissions Delete',(case when d.SubmissionsDelete= 1 then 'Yes' else 'No' end),(case when i.SubmissionsDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmissionsDelete,'') != ISNULL(d.SubmissionsDelete,'')

--when update SubmissionsSearch
if update(SubmissionsSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submissions Search',(case when d.SubmissionsSearch= 1 then 'Yes' else 'No' end),(case when i.SubmissionsSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmissionsSearch,'') != ISNULL(d.SubmissionsSearch,'')

--when update SubmissionsImport
if update(SubmissionsImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submissions Import',(case when d.SubmissionsImport= 1 then 'Yes' else 'No' end),(case when i.SubmissionsImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmissionsImport,'') != ISNULL(d.SubmissionsImport,'')

--when update SubmissionsExport
if update(SubmissionsExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submissions Export',(case when d.SubmissionsExport= 1 then 'Yes' else 'No' end),(case when i.SubmissionsExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmissionsExport,'') != ISNULL(d.SubmissionsExport,'')

--when update PaymentsCreate
if update(PaymentsCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payments Create',(case when d.PaymentsCreate= 1 then 'Yes' else 'No' end),(case when i.PaymentsCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaymentsCreate,'') != ISNULL(d.PaymentsCreate,'')

--when update PaymentsEdit
if update(PaymentsEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payments Edit',(case when d.PaymentsEdit= 1 then 'Yes' else 'No' end),(case when i.PaymentsEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaymentsEdit,'') != ISNULL(d.PaymentsEdit,'')

--when update PaymentsDelete
if update(PaymentsDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payments Delete',(case when d.PaymentsDelete= 1 then 'Yes' else 'No' end),(case when i.PaymentsDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaymentsDelete,'') != ISNULL(d.PaymentsDelete,'')

--when update PaymentsSearch
if update(PaymentsSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payments Search',(case when d.PaymentsSearch= 1 then 'Yes' else 'No' end),(case when i.PaymentsSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaymentsSearch,'') != ISNULL(d.PaymentsSearch,'')

--when update PaymentsImport
if update(PaymentsImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payments Import',(case when d.PaymentsImport= 1 then 'Yes' else 'No' end),(case when i.PaymentsImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaymentsImport,'') != ISNULL(d.PaymentsImport,'')

--when update PaymentsExport
if update(PaymentsExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payments Export',(case when d.PaymentsExport= 1 then 'Yes' else 'No' end),(case when i.PaymentsExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaymentsExport,'') != ISNULL(d.PaymentsExport,'')

--when update FollowupCreate
if update(FollowupCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Followup Create',(case when d.FollowupCreate= 1 then 'Yes' else 'No' end),(case when i.FollowupCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FollowupCreate,'') != ISNULL(d.FollowupCreate,'')

--when update FollowupEdit
if update(FollowupEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Followup Edit',(case when d.FollowupEdit= 1 then 'Yes' else 'No' end),(case when i.FollowupEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FollowupEdit,'') != ISNULL(d.FollowupEdit,'')

--when update FollowupDelete
if update(FollowupDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Followup Delete',(case when d.FollowupDelete= 1 then 'Yes' else 'No' end),(case when i.FollowupDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FollowupDelete,'') != ISNULL(d.FollowupDelete,'')

--when update FollowupSearch
if update(FollowupSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Followup Search',(case when d.FollowupSearch= 1 then 'Yes' else 'No' end),(case when i.FollowupSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FollowupSearch,'') != ISNULL(d.FollowupSearch,'')

--when update FollowupImport
if update(FollowupImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Followup Import',(case when d.FollowupImport= 1 then 'Yes' else 'No' end),(case when i.FollowupImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FollowupImport,'') != ISNULL(d.SchedulerCreate,'')

--when update FollowupExport
if update(FollowupExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Followup Export',(case when d.FollowupExport= 1 then 'Yes' else 'No' end),(case when i.FollowupExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FollowupExport,'') != ISNULL(d.FollowupExport,'')

--when update ReportsCreate
if update(ReportsCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reports Create',(case when d.ReportsCreate= 1 then 'Yes' else 'No' end),(case when i.ReportsCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReportsCreate,'') != ISNULL(d.ReportsCreate,'')

--when update ReportsEdit
if update(ReportsEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reports Edit',(case when d.ReportsEdit= 1 then 'Yes' else 'No' end),(case when i.ReportsEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReportsEdit,'') != ISNULL(d.ReportsEdit,'')

--when update ReportsDelete
if update(ReportsDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reports Delete',(case when d.ReportsDelete= 1 then 'Yes' else 'No' end),(case when i.ReportsDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReportsDelete,'') != ISNULL(d.ReportsDelete,'')

--when update ReportsSearch
if update(ReportsSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reports Search',(case when d.ReportsSearch= 1 then 'Yes' else 'No' end),(case when i.ReportsSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReportsSearch,'') != ISNULL(d.ReportsSearch,'')

--when update ReportsImport
if update(ReportsImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reports Import',(case when d.ReportsImport= 1 then 'Yes' else 'No' end),(case when i.ReportsImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReportsImport,'') != ISNULL(d.ReportsImport,'')

--when update ReportsExport
if update(ReportsExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reports Export',(case when d.ReportsExport= 1 then 'Yes' else 'No' end),(case when i.ReportsExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReportsExport,'') != ISNULL(d.ReportsExport,'')

--when update ClientCreate
if update(ClientCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Client Create',(case when d.ClientCreate= 1 then 'Yes' else 'No' end),(case when i.ClientCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClientCreate,'') != ISNULL(d.ClientCreate,'')

--when update ClientEdit
if update(ClientEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Client Edit',(case when d.ClientEdit= 1 then 'Yes' else 'No' end),(case when i.ClientEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClientEdit,'') != ISNULL(d.ClientEdit,'')

--when update ClientDelete
if update(ClientDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Client Delete',(case when d.ClientDelete= 1 then 'Yes' else 'No' end),(case when i.ClientDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClientDelete,'') != ISNULL(d.ClientDelete,'')

--when update ClientSearch
if update(SchedulerCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Client Search',(case when d.ClientSearch= 1 then 'Yes' else 'No' end),(case when i.ClientSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClientSearch,'') != ISNULL(d.ClientSearch,'')

--when update ClientImport
if update(ClientImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Client Import',(case when d.ClientImport= 1 then 'Yes' else 'No' end),(case when i.ClientImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClientImport,'') != ISNULL(d.ClientImport,'')

--when update ClientExport
if update(ClientExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Client Export',(case when d.ClientExport= 1 then 'Yes' else 'No' end),(case when i.ClientExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClientExport,'') != ISNULL(d.ClientExport,'')

--when update UserCreate
if update(UserCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'User Create',(case when d.UserCreate= 1 then 'Yes' else 'No' end),(case when i.UserCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.UserCreate,'') != ISNULL(d.UserCreate,'')

--when update UserEdit
if update(UserEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'UserEdit',(case when d.UserEdit= 1 then 'Yes' else 'No' end),(case when i.UserEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.UserEdit,'') != ISNULL(d.UserEdit,'')

--when update UserDelete
if update(UserDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'User Delete',(case when d.UserDelete= 1 then 'Yes' else 'No' end),(case when i.UserDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.UserDelete,'') != ISNULL(d.UserDelete,'')

--when update UserSearch
if update(UserSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'User Search',(case when d.UserSearch= 1 then 'Yes' else 'No' end),(case when i.UserSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.UserSearch,'') != ISNULL(d.UserSearch,'')

--when update UserImport
if update(UserImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'User Import',(case when d.UserImport= 1 then 'Yes' else 'No' end),(case when i.UserImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.UserImport,'') != ISNULL(d.UserImport,'')

--when update UserExport
if update(UserExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'User Export',(case when d.UserExport= 1 then 'Yes' else 'No' end),(case when i.UserExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.UserExport,'') != ISNULL(d.UserExport,'')

--when update PracticeCreate
if update(PracticeCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Practice Create',(case when d.PracticeCreate= 1 then 'Yes' else 'No' end),(case when i.PracticeCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PracticeCreate,'') != ISNULL(d.PracticeCreate,'')

--when update PracticeEdit
if update(PracticeEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Practice Edit',(case when d.PracticeEdit= 1 then 'Yes' else 'No' end),(case when i.PracticeEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PracticeEdit,'') != ISNULL(d.PracticeEdit,'')

--when update PracticeDelete
if update(PracticeDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Practice Delete',(case when d.PracticeDelete= 1 then 'Yes' else 'No' end),(case when i.PracticeDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PracticeDelete,'') != ISNULL(d.PracticeDelete,'')

--when update PracticeSearch
if update(PracticeSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Practice Search',(case when d.PracticeSearch= 1 then 'Yes' else 'No' end),(case when i.PracticeSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PracticeSearch,'') != ISNULL(d.PracticeSearch,'')

--when update PracticeImport
if update(PracticeImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Practice Import',(case when d.PracticeImport= 1 then 'Yes' else 'No' end),(case when i.PracticeImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PracticeImport,'') != ISNULL(d.PracticeImport,'')

--when update PracticeExport
if update(PracticeExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Practice Export',(case when d.PracticeExport= 1 then 'Yes' else 'No' end),(case when i.PracticeExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PracticeExport,'') != ISNULL(d.PracticeExport,'')

--when update LocationCreate
if update(LocationCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Location Create',(case when d.LocationCreate= 1 then 'Yes' else 'No' end),(case when i.LocationCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.LocationCreate,'') != ISNULL(d.LocationCreate,'')

--when update LocationEdit
if update(LocationEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Location Edit',(case when d.LocationEdit= 1 then 'Yes' else 'No' end),(case when i.LocationEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.LocationEdit,'') != ISNULL(d.LocationEdit,'')

--when update LocationDelete
if update(LocationDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Location Delete',(case when d.LocationDelete= 1 then 'Yes' else 'No' end),(case when i.LocationDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.LocationDelete,'') != ISNULL(d.LocationDelete,'')

--when update LocationSearch
if update(LocationSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Location Search',(case when d.LocationSearch= 1 then 'Yes' else 'No' end),(case when i.LocationSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.LocationSearch,'') != ISNULL(d.LocationSearch,'')

--when update LocationImport
if update(LocationImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Location Import',(case when d.LocationImport= 1 then 'Yes' else 'No' end),(case when i.LocationImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.LocationImport,'') != ISNULL(d.LocationImport,'')

--when update LocationExport
if update(LocationExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Location Export',(case when d.LocationExport= 1 then 'Yes' else 'No' end),(case when i.LocationExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.LocationExport,'') != ISNULL(d.LocationExport,'')

--when update ProviderCreate
if update(ProviderCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Provider Create',(case when d.ProviderCreate= 1 then 'Yes' else 'No' end),(case when i.ProviderCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProviderCreate,'') != ISNULL(d.ProviderCreate,'')

--when update ProviderEdit
if update(ProviderEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Provider Edit',(case when d.ProviderEdit= 1 then 'Yes' else 'No' end),(case when i.ProviderEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProviderEdit,'') != ISNULL(d.ProviderEdit,'')

--when update ProviderDelete
if update(ProviderDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Provider Delete',(case when d.ProviderDelete= 1 then 'Yes' else 'No' end),(case when i.ProviderDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProviderDelete,'') != ISNULL(d.ProviderDelete,'')

--when update ProviderSearch
if update(ProviderSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Provider Search',(case when d.ProviderSearch= 1 then 'Yes' else 'No' end),(case when i.ProviderSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProviderSearch,'') != ISNULL(d.ProviderSearch,'')

--when update ProviderImport
if update(ProviderImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Provider Import',(case when d.ProviderImport= 1 then 'Yes' else 'No' end),(case when i.ProviderImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProviderImport,'') != ISNULL(d.ProviderImport,'')

--when update ProviderExport
if update(ProviderExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'ProviderExport',(case when d.ProviderExport= 1 then 'Yes' else 'No' end),(case when i.ProviderExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProviderExport,'') != ISNULL(d.ProviderExport,'')

--when update ReferringProviderCreate
if update(ReferringProviderCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Referring Provider Create',(case when d.ReferringProviderCreate= 1 then 'Yes' else 'No' end),(case when i.ReferringProviderCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReferringProviderCreate,'') != ISNULL(d.ReferringProviderCreate,'')

--when update ReferringProviderEdit
if update(ReferringProviderEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Referring Provider Edit',(case when d.ReferringProviderEdit= 1 then 'Yes' else 'No' end),(case when i.ReferringProviderEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReferringProviderEdit,'') != ISNULL(d.ReferringProviderEdit,'')

--when update ReferringProviderDelete
if update(ReferringProviderDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Referring Provider Delete',(case when d.ReferringProviderDelete= 1 then 'Yes' else 'No' end),(case when i.ReferringProviderDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReferringProviderDelete,'') != ISNULL(d.ReferringProviderDelete,'')

--when update ReferringProviderSearch
if update(ReferringProviderSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Referring Provider Search',(case when d.ReferringProviderSearch= 1 then 'Yes' else 'No' end),(case when i.ReferringProviderSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReferringProviderSearch,'') != ISNULL(d.ReferringProviderSearch,'')

--when update ReferringProviderImport
if update(ReferringProviderImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Referring Provider Import',(case when d.ReferringProviderImport= 1 then 'Yes' else 'No' end),(case when i.ReferringProviderImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReferringProviderImport,'') != ISNULL(d.ReferringProviderImport,'')

--when update ReferringProviderExport
if update(ReferringProviderExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Referring Provider Export',(case when d.ReferringProviderExport= 1 then 'Yes' else 'No' end),(case when i.ReferringProviderExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReferringProviderExport,'') != ISNULL(d.ReferringProviderExport,'')

--when update InsuranceCreate
if update(InsuranceCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insurance Create',(case when d.InsuranceCreate= 1 then 'Yes' else 'No' end),(case when i.InsuranceCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsuranceCreate,'') != ISNULL(d.InsuranceCreate,'')

--when update InsuranceEdit
if update(InsuranceEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insurance Edit',(case when d.InsuranceEdit= 1 then 'Yes' else 'No' end),(case when i.InsuranceEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsuranceEdit,'') != ISNULL(d.InsuranceEdit,'')

--when update InsuranceDelete
if update(InsuranceDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insurance Delete',(case when d.InsuranceDelete= 1 then 'Yes' else 'No' end),(case when i.InsuranceDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsuranceDelete,'') != ISNULL(d.InsuranceDelete,'')

--when update InsuranceSearch
if update(InsuranceSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insurance Search',(case when d.InsuranceSearch= 1 then 'Yes' else 'No' end),(case when i.InsuranceSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsuranceSearch,'') != ISNULL(d.InsuranceSearch,'')

--when update InsuranceImport
if update(InsuranceImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insurance Import',(case when d.InsuranceImport= 1 then 'Yes' else 'No' end),(case when i.InsuranceImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsuranceImport,'') != ISNULL(d.InsuranceImport,'')

--when update InsuranceExport
if update(InsuranceExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insurance Export',(case when d.InsuranceExport= 1 then 'Yes' else 'No' end),(case when i.InsuranceExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsuranceExport,'') != ISNULL(d.InsuranceExport,'')

--when update InsurancePlanCreate
if update(InsurancePlanCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insurance Plan Create',(case when d.InsurancePlanCreate= 1 then 'Yes' else 'No' end),(case when i.InsurancePlanCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsurancePlanCreate,'') != ISNULL(d.InsurancePlanCreate,'')

--when update InsurancePlanEdit
if update(InsurancePlanEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insurance Plan Edit',(case when d.InsurancePlanEdit= 1 then 'Yes' else 'No' end),(case when i.InsurancePlanEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsurancePlanEdit,'') != ISNULL(d.InsurancePlanEdit,'')


--when update InsurancePlanDelete
if update(InsurancePlanDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insurance Plan Delete',(case when d.InsurancePlanDelete= 1 then 'Yes' else 'No' end),(case when i.InsurancePlanDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsurancePlanDelete,'') != ISNULL(d.InsurancePlanDelete,'')

--when update InsurancePlanSearch
if update(InsurancePlanSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insurance Plan Search',(case when d.InsurancePlanSearch= 1 then 'Yes' else 'No' end),(case when i.InsurancePlanSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsurancePlanSearch,'') != ISNULL(d.InsurancePlanSearch,'')

--when update InsurancePlanImport
if update(InsurancePlanImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insurance Plan Import',(case when d.InsurancePlanImport= 1 then 'Yes' else 'No' end),(case when i.InsurancePlanImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsurancePlanImport,'') != ISNULL(d.InsurancePlanImport,'')

--when update InsurancePlanExport
if update(InsurancePlanExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insurance Plan Export',(case when d.InsurancePlanExport= 1 then 'Yes' else 'No' end),(case when i.InsurancePlanExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsurancePlanExport,'') != ISNULL(d.InsurancePlanExport,'')

--when update InsurancePlanAddressCreate
if update(InsurancePlanAddressCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insurance Plan Address Create',(case when d.InsurancePlanAddressCreate= 1 then 'Yes' else 'No' end),(case when i.InsurancePlanAddressCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsurancePlanAddressCreate,'') != ISNULL(d.InsurancePlanAddressCreate,'')

--when update InsurancePlanAddressEdit
if update(InsurancePlanAddressEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insurance Plan Address Edit',(case when d.InsurancePlanAddressEdit= 1 then 'Yes' else 'No' end),(case when i.InsurancePlanAddressEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsurancePlanAddressEdit,'') != ISNULL(d.InsurancePlanAddressEdit,'')


--when update InsurancePlanAddressDelete
if update(InsurancePlanAddressDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insurance Plan Address Delete',(case when d.InsurancePlanAddressDelete= 1 then 'Yes' else 'No' end),(case when i.InsurancePlanAddressDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsurancePlanAddressDelete,'') != ISNULL(d.InsurancePlanAddressDelete,'')

--when update InsurancePlanAddressSearch
if update(InsurancePlanAddressSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insurance Plan Address Search',(case when d.InsurancePlanAddressSearch= 1 then 'Yes' else 'No' end),(case when i.InsurancePlanAddressSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsurancePlanAddressSearch,'') != ISNULL(d.InsurancePlanAddressSearch,'')

--when update InsurancePlanAddressImport
if update(InsurancePlanAddressImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insurance Plan Address Import',(case when d.InsurancePlanAddressImport= 1 then 'Yes' else 'No' end),(case when i.InsurancePlanAddressImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsurancePlanAddressImport,'') != ISNULL(d.InsurancePlanAddressImport,'')

--when update InsurancePlanAddressExport
if update(InsurancePlanAddressExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insurance Plan Address Export',(case when d.InsurancePlanAddressExport= 1 then 'Yes' else 'No' end),(case when i.InsurancePlanAddressExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsurancePlanAddressExport,'') != ISNULL(d.InsurancePlanAddressExport,'')

--when update EDISubmitCreate
if update(EDISubmitCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'EDI Submit Create',(case when d.EDISubmitCreate= 1 then 'Yes' else 'No' end),(case when i.EDISubmitCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.EDISubmitCreate,'') != ISNULL(d.EDISubmitCreate,'')

--when update EDISubmitEdit
if update(EDISubmitEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'EDI Submit Edit',(case when d.EDISubmitEdit= 1 then 'Yes' else 'No' end),(case when i.EDISubmitEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.EDISubmitEdit,'') != ISNULL(d.EDISubmitEdit,'')

--when update EDISubmitDelete
if update(EDISubmitDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'EDI Submit Delete',(case when d.EDISubmitDelete= 1 then 'Yes' else 'No' end),(case when i.EDISubmitDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.EDISubmitDelete,'') != ISNULL(d.EDISubmitDelete,'')

--when update EDISubmitSearch
if update(EDISubmitSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'EDI Submit Search',(case when d.EDISubmitSearch= 1 then 'Yes' else 'No' end),(case when i.EDISubmitSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.EDISubmitSearch,'') != ISNULL(d.EDISubmitSearch,'')

--when update EDISubmitImport
if update(EDISubmitImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'EDI Submit Import',(case when d.EDISubmitImport= 1 then 'Yes' else 'No' end),(case when i.EDISubmitImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.EDISubmitImport,'') != ISNULL(d.EDISubmitImport,'')

--when update EDISubmitExport
if update(EDISubmitExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'EDI Submit Export',(case when d.EDISubmitExport= 1 then 'Yes' else 'No' end),(case when i.EDISubmitExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.EDISubmitExport,'') != ISNULL(d.EDISubmitExport,'')

--when update EDIEligiBilityCreate
if update(EDIEligiBilityCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'EDI EligiBility Create',(case when d.EDIEligiBilityCreate= 1 then 'Yes' else 'No' end),(case when i.EDIEligiBilityCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.EDIEligiBilityCreate,'') != ISNULL(d.EDIEligiBilityCreate,'')

--when update EDIEligiBilityEdit
if update(EDIEligiBilityEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'EDI EligiBility Edit',(case when d.EDIEligiBilityEdit= 1 then 'Yes' else 'No' end),(case when i.EDIEligiBilityEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.EDIEligiBilityEdit,'') != ISNULL(d.EDIEligiBilityEdit,'')

--when update EDIEligiBilityDelete
if update(EDIEligiBilityDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'EDI EligiBility Delete',(case when d.EDIEligiBilityDelete= 1 then 'Yes' else 'No' end),(case when i.EDIEligiBilityDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.EDIEligiBilityDelete,'') != ISNULL(d.EDIEligiBilityDelete,'')

--when update EDIEligiBilitySearch
if update(EDIEligiBilitySearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'EDI EligiBility Search',(case when d.EDIEligiBilitySearch= 1 then 'Yes' else 'No' end),(case when i.EDIEligiBilitySearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.EDIEligiBilitySearch,'') != ISNULL(d.EDIEligiBilitySearch,'')

--when update EDIEligiBilityImport
if update(EDIEligiBilityImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'EDI EligiBility Import',(case when d.EDIEligiBilityImport= 1 then 'Yes' else 'No' end),(case when i.EDIEligiBilityImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.EDIEligiBilityImport,'') != ISNULL(d.EDIEligiBilityImport,'')

--when update EDIEligiBilityExport
if update(EDIEligiBilityExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'EDI EligiBility Export',(case when d.EDIEligiBilityExport= 1 then 'Yes' else 'No' end),(case when i.EDIEligiBilityExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.EDIEligiBilityExport,'') != ISNULL(d.EDIEligiBilityExport,'')

--when update EDIStatusCreate
if update(EDIStatusCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'EDI Status Create',(case when d.EDIStatusCreate= 1 then 'Yes' else 'No' end),(case when i.EDIStatusCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.EDIStatusCreate,'') != ISNULL(d.EDIStatusCreate,'')

--when update EDIStatusEdit
if update(EDIStatusEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'EDI Status Edit',(case when d.EDIStatusEdit= 1 then 'Yes' else 'No' end),(case when i.EDIStatusEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.EDIStatusEdit,'') != ISNULL(d.EDIStatusEdit,'')

--when update EDIStatusDelete
if update(EDIStatusDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'EDI Status Delete',(case when d.EDIStatusDelete= 1 then 'Yes' else 'No' end),(case when i.EDIStatusDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.EDIStatusDelete,'') != ISNULL(d.EDIStatusDelete,'')

--when update EDIStatusSearch
if update(EDIStatusSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'EDI Status Search',(case when d.EDIStatusSearch= 1 then 'Yes' else 'No' end),(case when i.EDIStatusSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.EDIStatusSearch,'') != ISNULL(d.EDIStatusSearch,'')

--when update EDIStatusImport
if update(EDIStatusImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'EDI Status Import',(case when d.EDIStatusImport= 1 then 'Yes' else 'No' end),(case when i.EDIStatusImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.EDIStatusImport,'') != ISNULL(d.EDIStatusImport,'')

--when update EDIStatusExport
if update(EDIStatusExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'EDI Status Export',(case when d.EDIStatusExport= 1 then 'Yes' else 'No' end),(case when i.EDIStatusExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.EDIStatusExport,'') != ISNULL(d.EDIStatusExport,'')

--when update ICDCreate
if update(ICDCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'ICD Create',(case when d.ICDCreate= 1 then 'Yes' else 'No' end),(case when i.ICDCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ICDCreate,'') != ISNULL(d.ICDCreate,'')

--when update ICDEdit
if update(ICDEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'ICD Edit',(case when d.ICDEdit= 1 then 'Yes' else 'No' end),(case when i.ICDEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ICDEdit,'') != ISNULL(d.ICDEdit,'')

--when update ICDDelete
if update(ICDDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'ICD Delete',(case when d.ICDDelete= 1 then 'Yes' else 'No' end),(case when i.ICDDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ICDDelete,'') != ISNULL(d.ICDDelete,'')

--when update ICDSearch
if update(ICDSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'ICD Search',(case when d.ICDSearch= 1 then 'Yes' else 'No' end),(case when i.ICDSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ICDSearch,'') != ISNULL(d.ICDSearch,'')

--when update ICDImport
if update(ICDImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'ICD Import',(case when d.ICDImport= 1 then 'Yes' else 'No' end),(case when i.ICDImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ICDImport,'') != ISNULL(d.ICDImport,'')

--when update ICDExport
if update(ICDExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'ICD Export',(case when d.ICDExport= 1 then 'Yes' else 'No' end),(case when i.ICDExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ICDExport,'') != ISNULL(d.ICDExport,'')

--when update CPTCreate
if update(CPTCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'CPT Create',(case when d.CPTCreate= 1 then 'Yes' else 'No' end),(case when i.CPTCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CPTCreate,'') != ISNULL(d.CPTCreate,'')

--when update CPTEdit
if update(CPTEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'CPT Edit',(case when d.CPTEdit= 1 then 'Yes' else 'No' end),(case when i.CPTEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CPTEdit,'') != ISNULL(d.CPTEdit,'')

--when update CPTDelete
if update(CPTDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'CPT Delete',(case when d.CPTDelete= 1 then 'Yes' else 'No' end),(case when i.CPTDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CPTDelete,'') != ISNULL(d.CPTDelete,'')

--when update CPTSearch
if update(CPTSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'CPT Search',(case when d.CPTSearch= 1 then 'Yes' else 'No' end),(case when i.CPTSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CPTSearch,'') != ISNULL(d.CPTSearch,'')

--when update CPTImport
if update(CPTImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'CPT Import',(case when d.CPTImport= 1 then 'Yes' else 'No' end),(case when i.CPTImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CPTImport,'') != ISNULL(d.CPTImport,'')

--when update CPTExport
if update(CPTExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'CPT Export',(case when d.CPTExport= 1 then 'Yes' else 'No' end),(case when i.CPTExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CPTExport,'') != ISNULL(d.CPTExport,'')

--when update ModifiersCreate
if update(ModifiersCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Modifiers Create',(case when d.ModifiersCreate= 1 then 'Yes' else 'No' end),(case when i.ModifiersCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ModifiersCreate,'') != ISNULL(d.ModifiersCreate,'')

--when update ModifiersEdit
if update(ModifiersEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Modifiers Edit',(case when d.ModifiersEdit= 1 then 'Yes' else 'No' end),(case when i.ModifiersEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ModifiersEdit,'') != ISNULL(d.ModifiersEdit,'')

--when update ModifiersDelete
if update(ModifiersDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Modifiers Delete',(case when d.ModifiersDelete= 1 then 'Yes' else 'No' end),(case when i.ModifiersDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ModifiersDelete,'') != ISNULL(d.ModifiersDelete,'')

--when update ModifiersSearch
if update(ModifiersSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Modifiers Search',(case when d.ModifiersSearch= 1 then 'Yes' else 'No' end),(case when i.ModifiersSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ModifiersSearch,'') != ISNULL(d.ModifiersSearch,'')

--when update ModifiersImport
if update(ModifiersImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Modifiers Import',(case when d.ModifiersImport= 1 then 'Yes' else 'No' end),(case when i.ModifiersImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ModifiersImport,'') != ISNULL(d.ModifiersImport,'')

--when update ModifiersExport
if update(ModifiersExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Modifiers Export',(case when d.ModifiersExport= 1 then 'Yes' else 'No' end),(case when i.ModifiersExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ModifiersExport,'') != ISNULL(d.ModifiersExport,'')

--when update POSCreate
if update(POSCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'POS Create',(case when d.POSCreate= 1 then 'Yes' else 'No' end),(case when i.POSCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.POSCreate,'') != ISNULL(d.POSCreate,'')

--when update POSEdit
if update(POSEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'POS Edit',(case when d.POSEdit= 1 then 'Yes' else 'No' end),(case when i.POSEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.POSEdit,'') != ISNULL(d.POSEdit,'')

--when update POSDelete
if update(POSDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'POS Delete',(case when d.POSDelete= 1 then 'Yes' else 'No' end),(case when i.POSDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.POSDelete,'') != ISNULL(d.POSDelete,'')

--when update POSSearch
if update(POSSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'POS Search',(case when d.POSSearch= 1 then 'Yes' else 'No' end),(case when i.POSSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.POSSearch,'') != ISNULL(d.POSSearch,'')

--when update POSImport
if update(POSImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'POS Import',(case when d.POSImport= 1 then 'Yes' else 'No' end),(case when i.POSImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.POSImport,'') != ISNULL(d.POSImport,'')

--when update POSExport
if update(POSExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'POS Export',(case when d.POSExport= 1 then 'Yes' else 'No' end),(case when i.POSExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.POSExport,'') != ISNULL(d.POSExport,'')

--when update ClaimStatusCategoryCodesCreate
if update(ClaimStatusCategoryCodesCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Claim Status Category Codes Create',(case when d.ClaimStatusCategoryCodesCreate= 1 then 'Yes' else 'No' end),(case when i.ClaimStatusCategoryCodesCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClaimStatusCategoryCodesCreate,'') != ISNULL(d.ClaimStatusCategoryCodesCreate,'')

--when update ClaimStatusCategoryCodesEdit
if update(ClaimStatusCategoryCodesEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Claim Status Category Codes Edit',(case when d.ClaimStatusCategoryCodesEdit= 1 then 'Yes' else 'No' end),(case when i.ClaimStatusCategoryCodesEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClaimStatusCategoryCodesEdit,'') != ISNULL(d.ClaimStatusCategoryCodesEdit,'')

--when update ClaimStatusCategoryCodesDelete
if update(ClaimStatusCategoryCodesDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Claim Status Category Codes Delete',(case when d.ClaimStatusCategoryCodesDelete= 1 then 'Yes' else 'No' end),(case when i.ClaimStatusCategoryCodesDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClaimStatusCategoryCodesDelete,'') != ISNULL(d.ClaimStatusCategoryCodesDelete,'')

--when update ClaimStatusCategoryCodesSearch
if update(ClaimStatusCategoryCodesSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Claim Status Category Codes Search',(case when d.ClaimStatusCategoryCodesSearch= 1 then 'Yes' else 'No' end),(case when i.ClaimStatusCategoryCodesSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClaimStatusCategoryCodesSearch,'') != ISNULL(d.ClaimStatusCategoryCodesSearch,'')

--when update ClaimStatusCategoryCodesImport
if update(ClaimStatusCategoryCodesImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Claim Status Category Codes Import',(case when d.ClaimStatusCategoryCodesImport= 1 then 'Yes' else 'No' end),(case when i.ClaimStatusCategoryCodesImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClaimStatusCategoryCodesImport,'') != ISNULL(d.ClaimStatusCategoryCodesImport,'')

--when update ClaimStatusCategoryCodesExport
if update(ClaimStatusCategoryCodesExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Claim Status Category Codes Export',(case when d.ClaimStatusCategoryCodesExport= 1 then 'Yes' else 'No' end),(case when i.ClaimStatusCategoryCodesExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClaimStatusCategoryCodesExport,'') != ISNULL(d.ClaimStatusCategoryCodesExport,'')

--when update ClaimStatusCodesCreate
if update(ClaimStatusCodesCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Claim Status Codes Create',(case when d.ClaimStatusCodesCreate= 1 then 'Yes' else 'No' end),(case when i.ClaimStatusCodesCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClaimStatusCodesCreate,'') != ISNULL(d.ClaimStatusCodesCreate,'')

--when update ClaimStatusCodesEdit
if update(ClaimStatusCodesEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Claim Status Codes Edit',(case when d.ClaimStatusCodesEdit= 1 then 'Yes' else 'No' end),(case when i.ClaimStatusCodesEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClaimStatusCodesEdit,'') != ISNULL(d.ClaimStatusCodesEdit,'')

--when update ClaimStatusCodesDelete
if update(ClaimStatusCodesDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Claim Status Codes Delete',(case when d.ClaimStatusCodesDelete= 1 then 'Yes' else 'No' end),(case when i.ClaimStatusCodesDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClaimStatusCodesDelete,'') != ISNULL(d.ClaimStatusCodesDelete,'')

--when update ClaimStatusCodesSearch
if update(ClaimStatusCodesSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Claim Status Codes Search',(case when d.ClaimStatusCodesSearch= 1 then 'Yes' else 'No' end),(case when i.ClaimStatusCodesSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClaimStatusCodesSearch,'') != ISNULL(d.ClaimStatusCodesSearch,'')

--when update ClaimStatusCodesImport
if update(ClaimStatusCodesImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Claim Status Codes Import',(case when d.ClaimStatusCodesImport= 1 then 'Yes' else 'No' end),(case when i.ClaimStatusCodesImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClaimStatusCodesImport,'') != ISNULL(d.ClaimStatusCodesImport,'')

--when update ClaimStatusCodesExport
if update(ClaimStatusCodesExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Claim Status Codes Export',(case when d.ClaimStatusCodesExport= 1 then 'Yes' else 'No' end),(case when i.ClaimStatusCodesExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClaimStatusCodesExport,'') != ISNULL(d.ClaimStatusCodesExport,'')

--when update AdjustmentCodesCreate
if update(AdjustmentCodesCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Codes Create',(case when d.AdjustmentCodesCreate= 1 then 'Yes' else 'No' end),(case when i.AdjustmentCodesCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentCodesCreate,'') != ISNULL(d.AdjustmentCodesCreate,'')

--when update AdjustmentCodesEdit
if update(AdjustmentCodesEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Codes Edit',(case when d.AdjustmentCodesEdit= 1 then 'Yes' else 'No' end),(case when i.AdjustmentCodesEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentCodesEdit,'') != ISNULL(d.AdjustmentCodesEdit,'')

--when update AdjustmentCodesDelete
if update(AdjustmentCodesDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Codes Delete',(case when d.AdjustmentCodesDelete= 1 then 'Yes' else 'No' end),(case when i.AdjustmentCodesDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentCodesDelete,'') != ISNULL(d.AdjustmentCodesDelete,'')

--when update AdjustmentCodesSearch
if update(AdjustmentCodesSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Codes Search',(case when d.AdjustmentCodesSearch= 1 then 'Yes' else 'No' end),(case when i.AdjustmentCodesSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentCodesSearch,'') != ISNULL(d.AdjustmentCodesSearch,'')

--when update AdjustmentCodesImport
if update(AdjustmentCodesImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Codes Import',(case when d.AdjustmentCodesImport= 1 then 'Yes' else 'No' end),(case when i.AdjustmentCodesImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentCodesImport,'') != ISNULL(d.AdjustmentCodesImport,'')

--when update AdjustmentCodesExport
if update(AdjustmentCodesExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Codes Export',(case when d.AdjustmentCodesExport= 1 then 'Yes' else 'No' end),(case when i.AdjustmentCodesExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentCodesExport,'') != ISNULL(d.AdjustmentCodesExport,'')

--when update RemarkCodesCreate
if update(RemarkCodesCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Remark Codes Create',(case when d.RemarkCodesCreate= 1 then 'Yes' else 'No' end),(case when i.RemarkCodesCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RemarkCodesCreate,'') != ISNULL(d.RemarkCodesCreate,'')

--when update RemarkCodesEdit
if update(RemarkCodesEdit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Remark Codes Edit',(case when d.RemarkCodesEdit= 1 then 'Yes' else 'No' end),(case when i.RemarkCodesEdit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RemarkCodesEdit,'') != ISNULL(d.RemarkCodesEdit,'')

--when update RemarkCodesDelete
if update(RemarkCodesDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Remark Codes Delete',(case when d.RemarkCodesDelete= 1 then 'Yes' else 'No' end),(case when i.RemarkCodesDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RemarkCodesDelete,'') != ISNULL(d.RemarkCodesDelete,'')

--when update RemarkCodesSearch
if update(RemarkCodesSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Remark Codes Search',(case when d.RemarkCodesSearch= 1 then 'Yes' else 'No' end),(case when i.RemarkCodesSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RemarkCodesSearch,'') != ISNULL(d.RemarkCodesSearch,'')

--when update RemarkCodesImport
if update(RemarkCodesImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Remark Codes Import',(case when d.RemarkCodesImport= 1 then 'Yes' else 'No' end),(case when i.RemarkCodesImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RemarkCodesImport,'') != ISNULL(d.RemarkCodesImport,'')

--when update RemarkCodesExport
if update(RemarkCodesExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Remark Codes Export',(case when d.RemarkCodesExport= 1 then 'Yes' else 'No' end),(case when i.RemarkCodesExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RemarkCodesExport,'') != ISNULL(d.RemarkCodesExport,'')

--when update teamCreate
if update(teamCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'team Create',(case when d.teamCreate= 1 then 'Yes' else 'No' end),(case when i.teamCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.teamCreate,'') != ISNULL(d.teamCreate,'')

--when update teamupdate
if update(teamupdate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'team update',(case when d.teamupdate= 1 then 'Yes' else 'No' end),(case when i.teamupdate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.teamupdate,'') != ISNULL(d.teamupdate,'')

--when update teamDelete
if update(teamDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'team Delete',(case when d.teamDelete= 1 then 'Yes' else 'No' end),(case when i.teamDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.teamDelete,'') != ISNULL(d.teamDelete,'')

--when update teamSearch
if update(teamSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'team Search',(case when d.teamSearch= 1 then 'Yes' else 'No' end),(case when i.teamSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.teamSearch,'') != ISNULL(d.teamSearch,'')

--when update teamExport
if update(teamExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'team Export',(case when d.teamExport= 1 then 'Yes' else 'No' end),(case when i.teamExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.teamExport,'') != ISNULL(d.teamExport,'')

--when update teamImport
if update(POSDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'team Import',(case when d.teamImport= 1 then 'Yes' else 'No' end),(case when i.teamImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.teamImport,'') != ISNULL(d.teamImport,'')

--when update receiverCreate
if update(receiverCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'receiver Create',(case when d.receiverCreate= 1 then 'Yes' else 'No' end),(case when i.receiverCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.receiverCreate,'') != ISNULL(d.receiverCreate,'')

--when update receiverupdate
if update(receiverupdate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'receiver update',(case when d.receiverupdate= 1 then 'Yes' else 'No' end),(case when i.receiverupdate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.receiverupdate,'') != ISNULL(d.receiverupdate,'')

--when update receiverDelete
if update(receiverDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'receiver Delete',(case when d.receiverDelete= 1 then 'Yes' else 'No' end),(case when i.receiverDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.receiverDelete,'') != ISNULL(d.receiverDelete,'')

--when update receiverSearch
if update(receiverSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'receiver Search',(case when d.receiverSearch= 1 then 'Yes' else 'No' end),(case when i.receiverSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.receiverSearch,'') != ISNULL(d.receiverSearch,'')

--when update receiverExport
if update(receiverExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'receiver Export',(case when d.receiverExport= 1 then 'Yes' else 'No' end),(case when i.receiverExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.receiverExport,'') != ISNULL(d.receiverExport,'')

--when update receiverImport
if update(receiverImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'receiver Import',(case when d.receiverImport= 1 then 'Yes' else 'No' end),(case when i.receiverImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.receiverImport,'') != ISNULL(d.receiverImport,'')

--when update submitterCreate
if update(submitterCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'submitter Create',(case when d.submitterCreate= 1 then 'Yes' else 'No' end),(case when i.submitterCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.submitterCreate,'') != ISNULL(d.submitterCreate,'')

--when update submitterUpdate
if update(submitterUpdate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'submitter Update',(case when d.submitterUpdate= 1 then 'Yes' else 'No' end),(case when i.submitterUpdate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.submitterUpdate,'') != ISNULL(d.submitterUpdate,'')

--when update submitterDelete
if update(submitterDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'submitter Delete',(case when d.submitterDelete= 1 then 'Yes' else 'No' end),(case when i.submitterDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.submitterDelete,'') != ISNULL(d.submitterDelete,'')

--when update submitterSearch
if update(submitterSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'submitter Search',(case when d.submitterSearch= 1 then 'Yes' else 'No' end),(case when i.submitterSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.submitterSearch,'') != ISNULL(d.submitterSearch,'')

--when update submitterExport
if update(submitterExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'submitter Export',(case when d.submitterExport= 1 then 'Yes' else 'No' end),(case when i.submitterExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.submitterExport,'') != ISNULL(d.submitterExport,'')

--when update submitterImport
if update(submitterImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'submitter Import',(case when d.submitterImport= 1 then 'Yes' else 'No' end),(case when i.submitterImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.submitterImport,'') != ISNULL(d.submitterImport,'')

--when update patientPlanCreate
if update(patientPlanCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'patient Plan Create',(case when d.patientPlanCreate= 1 then 'Yes' else 'No' end),(case when i.patientPlanCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.patientPlanCreate,'') != ISNULL(d.patientPlanCreate,'')

--when update patientPlanUpdate
if update(patientPlanUpdate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'patient Plan Update',(case when d.patientPlanUpdate= 1 then 'Yes' else 'No' end),(case when i.patientPlanUpdate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.patientPlanUpdate,'') != ISNULL(d.patientPlanUpdate,'')

--when update patientPlanDelete
if update(patientPlanDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'patient Plan Delete',(case when d.patientPlanDelete= 1 then 'Yes' else 'No' end),(case when i.patientPlanDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.patientPlanDelete,'') != ISNULL(d.patientPlanDelete,'')

--when update patientPlanSearch
if update(patientPlanSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'patient Plan Search',(case when d.patientPlanSearch= 1 then 'Yes' else 'No' end),(case when i.patientPlanSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.patientPlanSearch,'') != ISNULL(d.patientPlanSearch,'')

--when update patientPlanExport
if update(patientPlanExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'patient Plan Export',(case when d.patientPlanExport= 1 then 'Yes' else 'No' end),(case when i.patientPlanExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.patientPlanExport,'') != ISNULL(d.patientPlanExport,'')

--when update patientPlanImport
if update(patientPlanImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'patient Plan Import',(case when d.patientPlanImport= 1 then 'Yes' else 'No' end),(case when i.patientPlanImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.patientPlanImport,'') != ISNULL(d.patientPlanImport,'')

--when update performEligibility
if update(performEligibility)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'perform Eligibility',(case when d.performEligibility= 1 then 'Yes' else 'No' end),(case when i.performEligibility= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.performEligibility,'') != ISNULL(d.performEligibility,'')

--when update patientPaymentCreate
if update(patientPaymentCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'patient Payment Create',(case when d.patientPaymentCreate= 1 then 'Yes' else 'No' end),(case when i.patientPaymentCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.patientPaymentCreate,'') != ISNULL(d.patientPaymentCreate,'')

--when update patientPaymentUpdate
if update(patientPaymentUpdate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'patient Payment Update',(case when d.patientPaymentUpdate= 1 then 'Yes' else 'No' end),(case when i.patientPaymentUpdate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.patientPaymentUpdate,'') != ISNULL(d.patientPaymentUpdate,'')

--when update patientPaymentDelete
if update(patientPaymentDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'patient Payment Delete',(case when d.patientPaymentDelete= 1 then 'Yes' else 'No' end),(case when i.patientPaymentDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.patientPaymentDelete,'') != ISNULL(d.patientPaymentDelete,'')

--when update patientPaymentSearch
if update(patientPaymentSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'patient Payment Search',(case when d.patientPaymentSearch= 1 then 'Yes' else 'No' end),(case when i.patientPaymentSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.patientPaymentSearch,'') != ISNULL(d.patientPaymentSearch,'')

--when update patientPaymentExport
if update(patientPaymentExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'patient Payment Export',(case when d.patientPaymentExport= 1 then 'Yes' else 'No' end),(case when i.patientPaymentExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.patientPaymentExport,'') != ISNULL(d.patientPaymentExport,'')

--when update patientPaymentImport
if update(patientPaymentImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'patient Payment Import',(case when d.patientPaymentImport= 1 then 'Yes' else 'No' end),(case when i.patientPaymentImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.patientPaymentImport,'') != ISNULL(d.patientPaymentImport,'')

--when update resubmitCharges
if update(resubmitCharges)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'resubmit Charges',(case when d.resubmitCharges= 1 then 'Yes' else 'No' end),(case when i.resubmitCharges= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.resubmitCharges,'') != ISNULL(d.resubmitCharges,'')

--when update batchdocumentCreate
if update(batchdocumentCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'batch document Create',(case when d.batchdocumentCreate= 1 then 'Yes' else 'No' end),(case when i.batchdocumentCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.batchdocumentCreate,'') != ISNULL(d.batchdocumentCreate,'')

--when update batchdocumentUpdate
if update(batchdocumentUpdate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'batch document Update',(case when d.batchdocumentUpdate= 1 then 'Yes' else 'No' end),(case when i.batchdocumentUpdate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.batchdocumentUpdate,'') != ISNULL(d.batchdocumentUpdate,'')

--when update batchdocumentDelete
if update(batchdocumentDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'batch document Delete',(case when d.batchdocumentDelete= 1 then 'Yes' else 'No' end),(case when i.batchdocumentDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.batchdocumentDelete,'') != ISNULL(d.batchdocumentDelete,'')

--when update batchdocumentSearch
if update(batchdocumentSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'batch document Search',(case when d.batchdocumentSearch= 1 then 'Yes' else 'No' end),(case when i.batchdocumentSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.batchdocumentSearch,'') != ISNULL(d.batchdocumentSearch,'')

--when update batchdocumentExport
if update(batchdocumentExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'batch document Export',(case when d.batchdocumentExport= 1 then 'Yes' else 'No' end),(case when i.batchdocumentExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.batchdocumentExport,'') != ISNULL(d.batchdocumentExport,'')

--when update batchdocumentImport
if update(batchdocumentImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'batch document Import',(case when d.batchdocumentImport= 1 then 'Yes' else 'No' end),(case when i.batchdocumentImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.batchdocumentImport,'') != ISNULL(d.batchdocumentImport,'')

--when update electronicsSubmissionSearch
if update(electronicsSubmissionSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'electronics Submission Search',(case when d.electronicsSubmissionSearch= 1 then 'Yes' else 'No' end),(case when i.electronicsSubmissionSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.electronicsSubmissionSearch,'') != ISNULL(d.electronicsSubmissionSearch,'')

--when update electronicsSubmissionSubmit
if update(electronicsSubmissionSubmit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'electronics Submission Submit',(case when d.electronicsSubmissionSubmit= 1 then 'Yes' else 'No' end),(case when i.electronicsSubmissionSubmit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.electronicsSubmissionSubmit,'') != ISNULL(d.electronicsSubmissionSubmit,'')

--when update electronicsSubmissionResubmit
if update(electronicsSubmissionResubmit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'electronics Submission Resubmit',(case when d.electronicsSubmissionResubmit= 1 then 'Yes' else 'No' end),(case when i.electronicsSubmissionResubmit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.electronicsSubmissionResubmit,'') != ISNULL(d.electronicsSubmissionResubmit,'')

--when update paperSubmissionSearch
if update(paperSubmissionSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'paper Submission Search',(case when d.paperSubmissionSearch= 1 then 'Yes' else 'No' end),(case when i.paperSubmissionSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.paperSubmissionSearch,'') != ISNULL(d.paperSubmissionSearch,'')

--when update paperSubmissionSubmit
if update(paperSubmissionSubmit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'paper Submission Submit',(case when d.paperSubmissionSubmit= 1 then 'Yes' else 'No' end),(case when i.paperSubmissionSubmit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.paperSubmissionSubmit,'') != ISNULL(d.paperSubmissionSubmit,'')

--when update paperSubmissionResubmit
if update(paperSubmissionResubmit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'paper Submission Resubmit',(case when d.paperSubmissionResubmit= 1 then 'Yes' else 'No' end),(case when i.paperSubmissionResubmit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.paperSubmissionResubmit,'') != ISNULL(d.paperSubmissionResubmit,'')

--when update submissionLogSearch
if update(submissionLogSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'submission Log Search',(case when d.submissionLogSearch= 1 then 'Yes' else 'No' end),(case when i.submissionLogSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.submissionLogSearch,'') != ISNULL(d.submissionLogSearch,'')

--when update planFollowupSearch
if update(planFollowupSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'plan Followup Search',(case when d.planFollowupSearch= 1 then 'Yes' else 'No' end),(case when i.planFollowupSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.planFollowupSearch,'') != ISNULL(d.planFollowupSearch,'')

--when update planFollowupCreate
if update(planFollowupCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'plan Followup Create',(case when d.planFollowupCreate= 1 then 'Yes' else 'No' end),(case when i.planFollowupCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.planFollowupCreate,'') != ISNULL(d.planFollowupCreate,'')

--when update planFollowupDelete
if update(planFollowupDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'plan Followup Delete',(case when d.planFollowupDelete= 1 then 'Yes' else 'No' end),(case when i.planFollowupDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.planFollowupDelete,'') != ISNULL(d.planFollowupDelete,'')

--when update planFollowupUpdate
if update(patientPlanUpdate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'plan Followup Update',(case when d.planFollowupUpdate= 1 then 'Yes' else 'No' end),(case when i.planFollowupUpdate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.planFollowupUpdate,'') != ISNULL(d.planFollowupUpdate,'')

--when update planFollowupImport
if update(planFollowupImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'plan Followup Import',(case when d.planFollowupImport= 1 then 'Yes' else 'No' end),(case when i.planFollowupImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.planFollowupImport,'') != ISNULL(d.planFollowupImport,'')

--when update planFollowupExport
if update(planFollowupExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'plan Followup Export',(case when d.planFollowupExport= 1 then 'Yes' else 'No' end),(case when i.planFollowupExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.planFollowupExport,'') != ISNULL(d.planFollowupExport,'')

--when update patientFollowupSearch
if update(patientFollowupSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'patient Followup Search',(case when d.patientFollowupSearch= 1 then 'Yes' else 'No' end),(case when i.patientFollowupSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.patientFollowupSearch,'') != ISNULL(d.patientFollowupSearch,'')

--when update patientFollowupCreate
if update(patientFollowupCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'patient Followup Create',(case when d.patientFollowupCreate= 1 then 'Yes' else 'No' end),(case when i.patientFollowupCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.patientFollowupCreate,'') != ISNULL(d.patientFollowupCreate,'')

--when update patientFollowupDelete
if update(patientFollowupDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'patient Followup Delete',(case when d.patientFollowupDelete= 1 then 'Yes' else 'No' end),(case when i.patientFollowupDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.patientFollowupDelete,'') != ISNULL(d.patientFollowupDelete,'')

--when update patientFollowupUpdate
if update(patientFollowupUpdate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'patient Followup Update',(case when d.patientFollowupUpdate= 1 then 'Yes' else 'No' end),(case when i.patientFollowupUpdate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.patientFollowupUpdate,'') != ISNULL(d.patientFollowupUpdate,'')

--when update patientFollowupImport
if update(patientFollowupImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'patient Followup Import',(case when d.patientFollowupImport= 1 then 'Yes' else 'No' end),(case when i.patientFollowupImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.patientFollowupImport,'') != ISNULL(d.patientFollowupImport,'')

--when update patientFollowupExport
if update(patientFollowupExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'patient Followup Export',(case when d.patientFollowupExport= 1 then 'Yes' else 'No' end),(case when i.patientFollowupExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.patientFollowupExport,'') != ISNULL(d.patientFollowupExport,'')

--when update groupSearch
if update(groupSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'group Search',(case when d.groupSearch= 1 then 'Yes' else 'No' end),(case when i.groupSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.groupSearch,'') != ISNULL(d.groupSearch,'')

--when update groupCreate
if update(groupCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'group Create',(case when d.groupCreate= 1 then 'Yes' else 'No' end),(case when i.groupCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.groupCreate,'') != ISNULL(d.groupCreate,'')

--when update groupUpdate
if update(groupUpdate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'group Update',(case when d.groupUpdate= 1 then 'Yes' else 'No' end),(case when i.groupUpdate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.groupUpdate,'') != ISNULL(d.groupUpdate,'')

--when update groupDelete
if update(groupDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'group Delete',(case when d.groupDelete= 1 then 'Yes' else 'No' end),(case when i.groupDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.groupDelete,'') != ISNULL(d.groupDelete,'')

--when update groupExport
if update(groupExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'group Export',(case when d.groupExport= 1 then 'Yes' else 'No' end),(case when i.groupExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.groupExport,'') != ISNULL(d.groupExport,'')

--when update groupImport
if update(groupImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'group Import',(case when d.groupImport= 1 then 'Yes' else 'No' end),(case when i.groupImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.groupImport,'') != ISNULL(d.groupImport,'')

--when update reasonSearch
if update(reasonSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'reason Search',(case when d.reasonSearch= 1 then 'Yes' else 'No' end),(case when i.reasonSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.reasonSearch,'') != ISNULL(d.reasonSearch,'')

--when update reasonCreate
if update(reasonCreate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'reason Create',(case when d.reasonCreate= 1 then 'Yes' else 'No' end),(case when i.reasonCreate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.reasonCreate,'') != ISNULL(d.reasonCreate,'')

--when update reasonUpdate
if update(reasonUpdate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'reason Update',(case when d.reasonUpdate= 1 then 'Yes' else 'No' end),(case when i.reasonUpdate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.reasonUpdate,'') != ISNULL(d.reasonUpdate,'')

--when update reasonDelete
if update(reasonDelete)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'reason Delete',(case when d.reasonDelete= 1 then 'Yes' else 'No' end),(case when i.reasonDelete= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.reasonDelete,'') != ISNULL(d.reasonDelete,'')

--when update reasonExport
if update(reasonExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'reason Export',(case when d.reasonExport= 1 then 'Yes' else 'No' end),(case when i.reasonExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.reasonExport,'') != ISNULL(d.reasonExport,'')

--when update reasonImport
if update(reasonImport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'reason Import',(case when d.reasonImport= 1 then 'Yes' else 'No' end),(case when i.reasonImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.reasonImport,'') != ISNULL(d.reasonImport,'')

--when update addPaymentVisit
if update(addPaymentVisit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'add Payment Visit',(case when d.addPaymentVisit= 1 then 'Yes' else 'No' end),(case when i.addPaymentVisit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.addPaymentVisit,'') != ISNULL(d.addPaymentVisit,'')

--when update DeleteCheck
if update(DeleteCheck)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Delete Check',(case when d.DeleteCheck= 1 then 'Yes' else 'No' end),(case when i.DeleteCheck= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DeleteCheck,'') != ISNULL(d.DeleteCheck,'')

--when update ManualPosting
if update(ManualPosting)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Manual Posting',(case when d.ManualPosting= 1 then 'Yes' else 'No' end),(case when i.ManualPosting= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ManualPosting,'') != ISNULL(d.ManualPosting,'')

--when update Postcheck
if update(Postcheck)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Post check',(case when d.Postcheck= 1 then 'Yes' else 'No' end),(case when i.Postcheck= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Postcheck,'') != ISNULL(d.Postcheck,'')

--when update PostExport
if update(PostExport)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Post Export',(case when d.PostExport= 1 then 'Yes' else 'No' end),(case when i.PostExport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PostExport,'') != ISNULL(d.PostExport,'')

--when update PostImport
if update(DeleteCheck)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Post Import',(case when d.PostImport= 1 then 'Yes' else 'No' end),(case when i.PostImport= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PostImport,'') != ISNULL(d.PostImport,'')

--when update ManualPostingAdd
if update(ManualPostingAdd)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Manual Posting Add',(case when d.ManualPostingAdd= 1 then 'Yes' else 'No' end),(case when i.ManualPostingAdd= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ManualPostingAdd,'') != ISNULL(d.ManualPostingAdd,'')

--when update ManualPostingUpdate
if update(ManualPostingUpdate)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Manual Posting Update',(case when d.ManualPostingUpdate= 1 then 'Yes' else 'No' end),(case when i.ManualPostingUpdate= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ManualPostingUpdate,'') != ISNULL(d.ManualPostingUpdate,'')

--when update PostCheckSearch
if update(PostCheckSearch)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Post Check Search',(case when d.PostCheckSearch= 1 then 'Yes' else 'No' end),(case when i.PostCheckSearch= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PostCheckSearch,'') != ISNULL(d.PostCheckSearch,'')

--when update DeletePaymentVisit
if update(DeletePaymentVisit)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Delete Payment Visit',(case when d.DeletePaymentVisit= 1 then 'Yes' else 'No' end),(case when i.DeletePaymentVisit= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DeletePaymentVisit,'') != ISNULL(d.DeletePaymentVisit,'')

--when update AssignedByUserId
if update(AssignedByUserId)
insert into RightsAudit(RightsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Assigned By UserId',(case when d.AssignedByUserId= 1 then 'Yes' else 'No' end),(case when i.AssignedByUserId= 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AssignedByUserId,'') != ISNULL(d.AssignedByUserId,'')
GO
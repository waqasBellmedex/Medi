--=====================================
--Description: <tgrClient Trigger>
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
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM ClientAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM ClientAudit  

--when update Name
BEGIN TRY
if update(Name)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',(RTRIM(LTRIM(d.Name))),(RTRIM(LTRIM(i.Name))),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Name)),'') != ISNULL(RTRIM(LTRIM(d.Name)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update OrganizationName
BEGIN TRY
if update(OrganizationName)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Organization Name',(RTRIM(LTRIM(d.OrganizationName))),(RTRIM(LTRIM(i.OrganizationName))),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.OrganizationName)),'') != ISNULL(RTRIM(LTRIM(d.OrganizationName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update TaxID
BEGIN TRY
if update(TaxID)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Tax ID',d.TaxID,i.TaxID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.TaxID,'') != ISNULL(d.TaxID,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ServiceLocation
BEGIN TRY
if update(ServiceLocation)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Service Location',(RTRIM(LTRIM(d.ServiceLocation))),(RTRIM(LTRIM(i.ServiceLocation))),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.ServiceLocation)),'') != ISNULL(RTRIM(LTRIM(d.ServiceLocation)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Address
BEGIN TRY
if update(Address)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address',(RTRIM(LTRIM(d.Address))),(RTRIM(LTRIM(i.Address))),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Address)),'') != ISNULL(RTRIM(LTRIM(d.Address)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update State
BEGIN TRY
if update(State)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'State',(RTRIM(LTRIM(d.State))),(RTRIM(LTRIM(i.State))),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.State)),'') != ISNULL(RTRIM(LTRIM(d.State)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update City
BEGIN TRY
if update(City)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'City',(RTRIM(LTRIM(d.City))),(RTRIM(LTRIM(i.City))),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.City)),'') != ISNULL(RTRIM(LTRIM(d.City)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ZipCode
BEGIN TRY
if update(ZipCode)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Zip Code',d.ZipCode,i.ZipCode,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ZipCode,'') != ISNULL(d.ZipCode,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update OfficeHour
BEGIN TRY
if update(OfficeHour)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Office Hour',d.OfficeHour,i.OfficeHour,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.OfficeHour,'') != ISNULL(d.OfficeHour,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update FaxNo
BEGIN TRY
if update(FaxNo)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Fax Number',d.FaxNo,i.FaxNo,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FaxNo,'') != ISNULL(d.FaxNo,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update OfficePhoneNo
BEGIN TRY
if update(OfficePhoneNo)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Office Phone Number',d.OfficePhoneNo,i.OfficePhoneNo,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.OfficePhoneNo,'') != ISNULL(d.OfficePhoneNo,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update OfficeEmail
BEGIN TRY
if update(OfficeEmail)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Office Email',(RTRIM(LTRIM(d.OfficeEmail))),(RTRIM(LTRIM(i.OfficeEmail))),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.OfficeEmail)),'') != ISNULL(RTRIM(LTRIM(d.OfficeEmail)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ContactPerson
BEGIN TRY
if update(ContactPerson)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Contact Person',(RTRIM(LTRIM(d.ContactPerson))),(RTRIM(LTRIM(i.ContactPerson))),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.ContactPerson)),'') != ISNULL(RTRIM(LTRIM(d.ContactPerson)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ContactNo
BEGIN TRY
if update(ContactNo)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Contact Number',d.ContactNo,i.ContactNo,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ContactNo,'') != ISNULL(d.ContactNo,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ContextName
BEGIN TRY
if update(ContextName)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Context Name',(RTRIM(LTRIM(d.ContextName))),(RTRIM(LTRIM(i.ContextName))),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.ContextName)),'') != ISNULL(RTRIM(LTRIM(d.ContextName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update LastNewInsertsModifiedDate
BEGIN TRY
if update(LastNewInsertsModifiedDate)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Last New Inserts Modified Date',d.LastNewInsertsModifiedDate,i.LastNewInsertsModifiedDate,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.LastNewInsertsModifiedDate as datetime),'') != ISNULL(cast(d.LastNewInsertsModifiedDate as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update LastNewTrigerModifiedDate
BEGIN TRY
if update(LastNewTrigerModifiedDate)
insert into ClientAudit(ClientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Last New Triger Modified Date',d.LastNewTrigerModifiedDate,i.LastNewTrigerModifiedDate,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.LastNewTrigerModifiedDate as datetime),'') != ISNULL(cast(d.LastNewTrigerModifiedDate as datetime),'') /* cast is uded to convert varchar value into datetime */
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;
END
GO
--=====================================
--Description: <tgrCpt Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrCptAudit')
DROP TRIGGER [dbo].[tgrCptAudit]
GO
Create TRIGGER [dbo].[tgrCptAudit]
ON [dbo].[Cpt] after Update
AS 
BEGIN
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM CptAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM CptAudit  

--when update Description
BEGIN TRY
if update(Description)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Description',(RTRIM(LTRIM(d.Description))),(RTRIM(LTRIM(i.Description))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Description)),'') != ISNULL(RTRIM(LTRIM(d.Description)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update CPTCode
BEGIN TRY
if update(CPTCode)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'CPT Code',d.CPTCode,i.CPTCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CPTCode,'') != ISNULL(d.CPTCode,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Amount
BEGIN TRY
if update(Amount)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Amount',d.Amount,i.Amount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Amount,'1') != ISNULL(d.Amount,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Modifier1ID
BEGIN TRY
if update(Modifier1ID)
--Select * into #t from modifier
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Modifier1 ID',cDel.[Description],cUpd.[Description],d.Modifier1ID,i.Modifier1ID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Modifier1ID,'1') != ISNULL(d.Modifier1ID,'1')
Join modifier as cDel on cDel.ID = ISNULL(d.Modifier1ID,'1')
join modifier as cUpd on cUpd.ID = ISNULL(i.Modifier1ID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Modifier2ID
BEGIN TRY
if update(Modifier2ID)
Select * into #t1 from modifier
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Modifier2 ID',cDel.[Description],cUpd.[Description],d.Modifier2ID,i.Modifier2ID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Modifier2ID,'1') != ISNULL(d.Modifier2ID,'1')
Join modifier as cDel on cDel.ID = ISNULL(d.Modifier2ID,'1')
join modifier as cUpd on cUpd.ID = ISNULL(i.Modifier2ID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Modifier3ID
BEGIN TRY
if update(Modifier3ID)
Select * into #t3 from modifier
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Modifier3 ID',cDel.[Description],cUpd.[Description],d.Modifier3ID,i.Modifier3ID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Modifier3ID,'1') != ISNULL(d.Modifier3ID,'1')
Join modifier as cDel on cDel.ID = ISNULL(d.Modifier3ID,'1')
join modifier as cUpd on cUpd.ID = ISNULL(i.Modifier3ID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Modifier4ID
BEGIN TRY
if update(Modifier4ID)
Select * into #t4 from modifier
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Modifier4 ID',cDel.[Description],cUpd.[Description],d.Modifier4ID,i.Modifier4ID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Modifier4ID,'1') != ISNULL(d.Modifier4ID,'1')
Join modifier as cDel on cDel.ID = ISNULL(d.Modifier4ID,'1')
join modifier as cUpd on cUpd.ID = ISNULL(i.Modifier4ID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update TypeOfServiceID
BEGIN TRY
if update(TypeOfServiceID)
Select * into #t5 from TypeOfService
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Type Of Service ID',cDel.[Code],cUpd.[Code],d.TypeOfServiceID,i.TypeOfServiceID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.TypeOfServiceID,'1') != ISNULL(d.TypeOfServiceID,'1')
Join TypeOfService as cDel on cDel.ID = ISNULL(d.TypeOfServiceID,'1')
join TypeOfService as cUpd on cUpd.ID = ISNULL(i.TypeOfServiceID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Category
BEGIN TRY
if update(Category)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Category',d.Category,i.Category,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Category,'') != ISNULL(d.Category,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update DefaultUnits
BEGIN TRY
if update(DefaultUnits)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Default Units',d.DefaultUnits,i.DefaultUnits,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DefaultUnits,'') != ISNULL(d.DefaultUnits,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update UnitOfMeasurement
BEGIN TRY
if update(UnitOfMeasurement)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Unit Of Measurement',d.UnitOfMeasurement,i.UnitOfMeasurement,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.UnitOfMeasurement,'') != ISNULL(d.UnitOfMeasurement,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update CLIANumber
BEGIN TRY
if update(CLIANumber)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'CLIA Number',d.CLIANumber,i.CLIANumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CLIANumber,'') != ISNULL(d.CLIANumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update NDCNumber
BEGIN TRY
if update(NDCNumber)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'NDC Number',d.NDCNumber,i.NDCNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.NDCNumber,'') != ISNULL(d.NDCNumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update NDCDescription
BEGIN TRY
if update(NDCDescription)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'NDC Description',(RTRIM(LTRIM(d.NDCDescription))),(RTRIM(LTRIM(i.NDCDescription))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.NDCDescription)),'') != ISNULL(RTRIM(LTRIM(d.NDCDescription)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update NDCUnits
BEGIN TRY
if update(NDCUnits)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'NDC Units',d.NDCUnits,i.NDCUnits,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.NDCUnits,'') != ISNULL(d.NDCUnits,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update NDCUnitOfMeasurement
BEGIN TRY
if update(NDCUnitOfMeasurement)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'NDC Unit Of Measurement',(RTRIM(LTRIM(d.NDCUnitOfMeasurement))),(RTRIM(LTRIM(i.NDCUnitOfMeasurement))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.NDCUnitOfMeasurement)),'') != ISNULL(RTRIM(LTRIM(d.NDCUnitOfMeasurement)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update IsValid
BEGIN TRY
if update(IsValid)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsValid',(case when d.IsValid = 1 then 'Yes' when d.IsValid = 0 then 'No' end),(case when i.IsValid = 1 then 'Yes' when i.IsValid =0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsValid,'2') != ISNULL(d.IsValid,'2')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update IsActive
BEGIN TRY
if update(IsActive)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsActive',(case when d.IsActive = 1 then 'Yes' when d.IsActive =0 then 'No' end),(case when i.IsActive = 1 then 'Yes' when i.IsActive=0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsActive,'2') != ISNULL(d.IsActive,'2')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update IsDeleted
BEGIN TRY
if update(IsDeleted)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsDeleted',(case when d.IsDeleted =1 then 'Yes' when d.IsDeleted=0 then 'No' end),(case when i.IsDeleted =1 then 'Yes' when i.IsDeleted =0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsDeleted,'2') != ISNULL(d.IsDeleted,'2')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update AnesthesiaBaseUnits
BEGIN TRY
if update(AnesthesiaBaseUnits)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Anesthesia Base Units',d.AnesthesiaBaseUnits,i.AnesthesiaBaseUnits,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AnesthesiaBaseUnits,'') != ISNULL(d.AnesthesiaBaseUnits,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update NonFacilityAmount
BEGIN TRY
if update(NonFacilityAmount)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Non Facility Amount',d.NonFacilityAmount,i.NonFacilityAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.NonFacilityAmount,'1') != ISNULL(d.NonFacilityAmount,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ShortDescription
BEGIN TRY
if update(ShortDescription)
insert into CptAudit(CptID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Short Description',(RTRIM(LTRIM(d.ShortDescription))),(RTRIM(LTRIM(i.ShortDescription))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.ShortDescription)),'') != ISNULL(RTRIM(LTRIM(d.ShortDescription)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
END
GO

--=====================================
--Description: <tgrInsurancePlan Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrInsurancePlanAudit')
DROP TRIGGER [dbo].[tgrInsurancePlanAudit]
GO
Create TRIGGER [dbo].[tgrInsurancePlanAudit]
ON [dbo].[InsurancePlan] after Update
AS 
BEGIN
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM InsurancePlanAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM InsurancePlanAudit  

--when update PlanName
BEGIN TRY
if update(PlanName)
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Plan Name',(RTRIM(LTRIM(d.PlanName))),(RTRIM(LTRIM(i.PlanName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PlanName)),'') != ISNULL(RTRIM(LTRIM(d.PlanName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update Description
BEGIN TRY
if update(Description)
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Description',(RTRIM(LTRIM(d.Description))),(RTRIM(LTRIM(i.Description))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Description)),'') != ISNULL(RTRIM(LTRIM(d.Description)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update InsuranceID
BEGIN TRY
if update(InsuranceID)
--Select * into #t from Insurance
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insurance ID',cDel.[Name],cUpd.[Name],d.InsuranceID,i.InsuranceID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsuranceID,'1') != ISNULL(d.InsuranceID,'1')
Join Insurance as cDel on cDel.ID = ISNULL(d.InsuranceID,'1')
join Insurance as cUpd on cUpd.ID = ISNULL(i.InsuranceID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update PlanTypeID
BEGIN TRY
if update(PlanTypeID)
--Select * into #t1 from PlanType
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Plan Type ID',cDel.[Code],cUpd.[Code],d.PlanTypeID,i.PlanTypeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PlanTypeID,'1') != ISNULL(d.PlanTypeID,'1')
Join PlanType as cDel on cDel.ID = ISNULL(d.PlanTypeID,'1')
join PlanType as cUpd on cUpd.ID = ISNULL(i.PlanTypeID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update IsCapitated
BEGIN TRY
if update(IsCapitated)
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsCapitated',(case when d.IsCapitated = 1 then 'Yes' when d.IsCapitated =0 then 'No' end),(case when i.IsCapitated = 1 then 'Yes' when d.IsCapitated =0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsCapitated,'2') != ISNULL(d.IsCapitated,'2')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 


-- when update SubmissionType
BEGIN TRY
if update(SubmissionType)
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submission Type',d.SubmissionType,i.SubmissionType,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmissionType,'') != ISNULL(d.SubmissionType,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update PayerID
BEGIN TRY
if update(PayerID)
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer ID',d.PayerID,i.PayerID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PayerID,'') != ISNULL(d.PayerID,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Edi837PayerID
BEGIN TRY
if update(Edi837PayerID)
--Select * into #t2 from Edi837Payer
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Edi837 Payer ID',cDel.[PayerName],cUpd.[PayerName],d.Edi837PayerID,i.Edi837PayerID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Edi837PayerID,'1') != ISNULL(d.Edi837PayerID,'1')
Join Edi837Payer as cDel on cDel.ID = ISNULL(d.Edi837PayerID,'1')
join Edi837Payer as cUpd on cUpd.ID = ISNULL(i.Edi837PayerID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 


--when update Edi270PayerID
BEGIN TRY
if update(Edi270PayerID)
--Select * into #t3 from Edi270Payer
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Edi270 Payer ID',cDel.[PayerName],cUpd.[PayerName],d.Edi270PayerID,i.Edi270PayerID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Edi270PayerID,'1') != ISNULL(d.Edi270PayerID,'1')
Join Edi270Payer as cDel on cDel.ID = ISNULL(d.Edi270PayerID,'1')
join Edi270Payer as cUpd on cUpd.ID = ISNULL(i.Edi270PayerID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Edi276PayerID
BEGIN TRY
if update(Edi276PayerID)
--Select * into #t4 from Edi276Payer
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Edi276 Payer ID',cDel.[PayerName],cUpd.[PayerName],d.Edi276PayerID,i.Edi276PayerID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Edi276PayerID,'1') != ISNULL(d.Edi276PayerID,'1')
Join Edi276Payer as cDel on cDel.ID = ISNULL(d.Edi276PayerID,'1')
join Edi276Payer as cUpd on cUpd.ID = ISNULL(i.Edi276PayerID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update FormType
BEGIN TRY
if update(FormType)
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Form Type',d.FormType,i.FormType,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FormType,'') != ISNULL(d.FormType,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update OutstandingDays
BEGIN TRY
if update(OutstandingDays)
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Outstanding Days',d.OutstandingDays,i.OutstandingDays,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.OutstandingDays,'') != ISNULL(d.OutstandingDays,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update IsActive
BEGIN TRY
if update(IsActive)
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsActive',(case when d.IsActive = 1 then 'Yes' when d.IsActive =0 then 'No' end),(case when i.IsActive = 1 then 'Yes' when i.IsActive = 0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsActive,'') != ISNULL(d.IsActive,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update IsDeleted
BEGIN TRY
if update(IsDeleted)
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsDeleted',d.IsDeleted,i.IsDeleted,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsDeleted,'') != ISNULL(d.IsDeleted,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update Notes
BEGIN TRY
if update(Notes)
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',(RTRIM(LTRIM(d.Notes))),(RTRIM(LTRIM(i.Notes))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Notes)),'') != ISNULL(RTRIM(LTRIM(d.Notes)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

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
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM Edi837PayerAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM Edi837PayerAudit  

--when update PayerName
BEGIN TRY
if update(PayerName)
insert into Edi837PayerAudit(Edi837PayerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer Name',(RTRIM(LTRIM(d.PayerName))),(RTRIM(LTRIM(i.PayerName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PayerName)),'') != ISNULL(RTRIM(LTRIM(d.PayerName)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ReceiverID
BEGIN TRY
if update(ReceiverID)
--Select * into #t from Receiver
insert into Edi837PayerAudit(Edi837PayerID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Receiver ID',cDel.[Name],cUpd.[Name],d.ReceiverID,i.ReceiverID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReceiverID,'1') != ISNULL(d.ReceiverID,'1')
Join Receiver as cDel on cDel.ID = ISNULL(d.ReceiverID,'1')
join Receiver as cUpd on cUpd.ID = ISNULL(i.ReceiverID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

-- when update PayerID
BEGIN TRY
if update(PayerID)
insert into Edi837PayerAudit(Edi837PayerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer ID',d.PayerID,i.PayerID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PayerID,'') != ISNULL(d.PayerID,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

-- when update ERA
BEGIN TRY
if update(ERA)
insert into Edi837PayerAudit(Edi837PayerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'ERA',d.ERA,i.ERA,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ERA,'') != ISNULL(d.ERA,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

-- when update Enrollment
BEGIN TRY
if update(Enrollment)
insert into Edi837PayerAudit(Edi837PayerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Enrollment',d.Enrollment,i.Enrollment,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Enrollment,'') != ISNULL(d.Enrollment,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

-- when update Secondary
BEGIN TRY
if update([Secondary])
insert into Edi837PayerAudit(Edi837PayerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Secondary',d.[Secondary],i.[Secondary],HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.[Secondary],'') != ISNULL(d.[Secondary],'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

-- when update Type
BEGIN TRY
if update([Type])
insert into Edi837PayerAudit(Edi837PayerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Type',d.[Type],i.[Type],HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.[Type],'') != ISNULL(d.[Type],'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

-- when update notes
BEGIN TRY
if update(notes)
insert into Edi837PayerAudit(Edi837PayerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'notes',(RTRIM(LTRIM(d.notes))),(RTRIM(LTRIM(i.notes))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.notes)),'') != ISNULL(RTRIM(LTRIM(d.notes)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;
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
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM Edi276PayerAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM Edi276PayerAudit  

--when update PayerName
begin try
if update(PayerName)
insert into Edi276PayerAudit(Edi276PayerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer Name',(RTRIM(LTRIM(d.PayerName))),(RTRIM(LTRIM(i.PayerName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PayerName)),'') != ISNULL(RTRIM(LTRIM(d.PayerName)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ReceiverID
BEGIN TRY
if update(ReceiverID)
--Select * into #t from Receiver
insert into Edi276PayerAudit(Edi276PayerID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Receiver ID',cDel.[Name],cUpd.[Name],d.ReceiverID,i.ReceiverID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReceiverID,'1') != ISNULL(d.ReceiverID,'1')
Join Receiver as cDel on cDel.ID = ISNULL(d.ReceiverID,'1')
join Receiver as cUpd on cUpd.ID = ISNULL(i.ReceiverID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

-- when update PayerID
BEGIN TRY
if update(PayerID)
insert into Edi276PayerAudit(Edi276PayerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer ID',d.PayerID,i.PayerID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PayerID,'') != ISNULL(d.PayerID,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;
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
BEGIN TRY
if update(PayerName)
insert into Edi270PayerAudit(Edi270PayerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer Name',(RTRIM(LTRIM(d.PayerName))),(RTRIM(LTRIM(i.PayerName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PayerName)),'') != ISNULL(RTRIM(LTRIM(d.PayerName)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ReceiverID
BEGIN TRY
if update(ReceiverID)
--Select * into #t from Receiver
insert into Edi270PayerAudit(Edi270PayerID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Receiver ID',cDel.[Name],cUpd.[Name],d.ReceiverID,i.ReceiverID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReceiverID,'1') != ISNULL(d.ReceiverID,'1')
Join Receiver as cDel on cDel.ID = ISNULL(d.ReceiverID,'1')
join Receiver as cUpd on cUpd.ID = ISNULL(i.ReceiverID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

-- when update PayerID
BEGIN TRY
if update(PayerID)
insert into Edi270PayerAudit(Edi270PayerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer ID',d.PayerID,i.PayerID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PayerID,'') != ISNULL(d.PayerID,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;
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
set Xact_abort off
Declare @TransactionID int;
select @TransactionID = MAX(TransactionID) from InsuranceAudit
if @TransactionID is null set @TransactionID = 1
else select @TransactionID = Max(TransactionID) + 1 from InsuranceAudit

--when update Name
BEGIN TRY
if update(Name)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',(RTRIM(LTRIM(d.Name))),(RTRIM(LTRIM(i.Name))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Name)),'') != ISNULL(RTRIM(LTRIM(d.Name)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update OrganizationName
BEGIN TRY
if update(OrganizationName)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Organization Name',(RTRIM(LTRIM(d.OrganizationName))),(RTRIM(LTRIM(i.OrganizationName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.OrganizationName)),'') != ISNULL(RTRIM(LTRIM(d.OrganizationName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Address1
BEGIN TRY
if update(Address1)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address1',(RTRIM(LTRIM(d.Address1))),(RTRIM(LTRIM(i.Address1))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Address1)),'') != ISNULL(RTRIM(LTRIM(d.Address1)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Address2
BEGIN TRY
if update(Address2)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address2',(RTRIM(LTRIM(d.Address2))),(RTRIM(LTRIM(i.Address2))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Address2)),'') != ISNULL(RTRIM(LTRIM(d.Address2)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update City
BEGIN TRY
if update(City)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'City',(RTRIM(LTRIM(d.City))),(RTRIM(LTRIM(i.City))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.City)),'') != ISNULL(RTRIM(LTRIM(d.City)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update State
BEGIN TRY
if update(State)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'State',(RTRIM(LTRIM(d.State))),(RTRIM(LTRIM(i.State))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.State)),'') != ISNULL(RTRIM(LTRIM(d.State)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ZipCode
BEGIN TRY
if update(ZipCode)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Zip Code',d.ZipCode,i.ZipCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ZipCode,'') != ISNULL(d.ZipCode,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update OfficePhoneNum
BEGIN TRY
if update(OfficePhoneNum)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Office Phone Number',d.OfficePhoneNum,i.OfficePhoneNum,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.OfficePhoneNum,'') != ISNULL(d.OfficePhoneNum,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Email
BEGIN TRY
if update(Email)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Email',(RTRIM(LTRIM(d.Email))),(RTRIM(LTRIM(i.Email))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Email)),'') != ISNULL(RTRIM(LTRIM(d.Email)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Website
BEGIN TRY
if update(Website)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Website',(RTRIM(LTRIM(d.Website))),(RTRIM(LTRIM(i.Website))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Website)),'') != ISNULL(RTRIM(LTRIM(d.Website)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update FaxNumber
BEGIN TRY
if update(FaxNumber)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Fax Number',d.FaxNumber,i.FaxNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FaxNumber,'') != ISNULL(d.FaxNumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update IsActive
BEGIN TRY
if update(IsActive)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsActive',(case when d.IsActive = 1 then 'Yes' when d.IsActive =0 then 'No' end),(case when i.IsActive = 1 then 'Yes' when i.IsActive =0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsActive,'2') != ISNULL(d.IsActive,'2')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update IsDeleted
BEGIN TRY
if update(IsDeleted)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsDeleted',d.IsDeleted,i.IsDeleted,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsDeleted,'') != ISNULL(d.IsDeleted,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Notes
BEGIN TRY
if update(Notes)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',(RTRIM(LTRIM(d.Notes))),(RTRIM(LTRIM(i.Notes))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Notes,'') != ISNULL(d.Notes,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

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
set Xact_abort off
Declare @TransactionID int;
select @TransactionID = MAX(TransactionID) from ModifierAudit
if @TransactionID is null set @TransactionID = 1
else select @TransactionID = Max(TransactionID) + 1 from ModifierAudit

--when update Code
BEGIN TRY
IF Update(Code)
INSERT INTO ModifierAudit(ModifierID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Code',d.Code,i.Code,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Code,'') != ISNULL(d.Code,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Description
BEGIN TRY
IF Update(Description)
INSERT INTO ModifierAudit(ModifierID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Description',(RTRIM(LTRIM(d.Description))),(RTRIM(LTRIM(i.Description))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Description)),'') != ISNULL(RTRIM(LTRIM(d.Description)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update IsActive
BEGIN TRY
IF Update(IsActive)
INSERT INTO ModifierAudit(ModifierID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'IsActive',(case when d.IsActive = 1 then 'Yes' when d.IsActive =0 then 'No' end),(case when i.IsActive = 1 then 'Yes' when i.IsActive =0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.IsActive,'2') != ISNULL(d.IsActive,'2')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update IsDeleted
BEGIN TRY
IF Update(IsDeleted)
INSERT INTO ModifierAudit(ModifierID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'IsDeleted',d.IsDeleted,i.IsDeleted,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.IsDeleted,'') != ISNULL(d.IsDeleted,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update AnesthesiaBaseUnits
BEGIN TRY
IF Update(AnesthesiaBaseUnits)
INSERT INTO ModifierAudit(ModifierID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Anesthesia Base Units',d.AnesthesiaBaseUnits,i.AnesthesiaBaseUnits,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.AnesthesiaBaseUnits,'') != ISNULL(d.AnesthesiaBaseUnits,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update DefaultFees
BEGIN TRY
IF Update(DefaultFees)
INSERT INTO ModifierAudit(ModifierID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Default Fees',d.DefaultFees,i.DefaultFees,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.DefaultFees,'1') != ISNULL(d.DefaultFees,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

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
set Xact_abort off
Declare @TransactionID int;
select @TransactionID = MAX(TransactionID) from PlanTypeAudit
if @TransactionID is null set @TransactionID = 1
else select @TransactionID = Max(TransactionID) + 1 from PlanTypeAudit

--when update Code
BEGIN TRY
IF Update(Code)
INSERT INTO PlanTypeAudit(PlanTypeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Code',d.Code,i.Code,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Code,'') != ISNULL(d.Code,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Description
BEGIN TRY
IF Update(Description)
INSERT INTO PlanTypeAudit(PlanTypeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Description',(RTRIM(LTRIM(d.Description))),(RTRIM(LTRIM(i.Description))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Description)),'') != ISNULL(RTRIM(LTRIM(d.Description)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
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
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM PracticeAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM PracticeAudit  

--when update Name
BEGIN TRY
if update(Name)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',(RTRIM(LTRIM(d.Name))),(RTRIM(LTRIM(i.Name))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Name)),'') != ISNULL(RTRIM(LTRIM(d.Name)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ClientID
BEGIN TRY
if update(ClientID)
--Select * into #t from Client
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Client ID',cDel.[Name],cUpd.[Name],d.ClientID,i.ClientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClientID,'1') != ISNULL(d.ClientID,'1')
Join Client as cDel on cDel.ID = ISNULL(d.ClientID,'1')
join Client as cUpd on cUpd.ID = ISNULL(i.ClientID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update OragnizationName
BEGIN TRY
if update(OrganizationName)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Organization Name',(RTRIM(LTRIM(d.OrganizationName))),(RTRIM(LTRIM(i.OrganizationName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.OrganizationName)),'') != ISNULL(RTRIM(LTRIM(d.OrganizationName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update TaxID
BEGIN TRY
if update(TaxID)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Tax ID',d.TaxID,i.TaxID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.TaxID,'') != ISNULL(d.TaxID,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update CLIANumber
BEGIN TRY
if update(CLIANumber)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'CLIA Number',d.CLIANumber,i.CLIANumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CLIANumber,'') != ISNULL(d.CLIANumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update NPI
BEGIN TRY
if update(NPI)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'NPI',d.NPI,i.NPI,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.NPI,'') != ISNULL(d.NPI,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update SSN
BEGIN TRY
if update(SSN)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'SSN',d.SSN,i.SSN,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SSN,'') != ISNULL(d.SSN,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update Type
BEGIN TRY
if update(Type)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Type',d.Type,i.Type,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Type,'') != ISNULL(d.Type,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update TaxonomyCode
BEGIN TRY
if update(TaxonomyCode)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Taxonomy Code',d.TaxonomyCode,i.TaxonomyCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.TaxonomyCode,'') != ISNULL(d.TaxonomyCode,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update Address1
BEGIN TRY
if update(Address1)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address1',(RTRIM(LTRIM(d.Address1))),(RTRIM(LTRIM(i.Address1))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Address1)),'') != ISNULL(RTRIM(LTRIM(d.Address1)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update Address2
BEGIN TRY
if update(Address2)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address2',(RTRIM(LTRIM(d.Address2))),(RTRIM(LTRIM(i.Address2))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Address2)),'') != ISNULL(RTRIM(LTRIM(d.Address2)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update City
BEGIN TRY
if update(City)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'City',(RTRIM(LTRIM(d.City))),(RTRIM(LTRIM(i.City))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.City)),'') != ISNULL(RTRIM(LTRIM(d.City)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update State
BEGIN TRY
if update(State)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'State',(RTRIM(LTRIM(d.State))),(RTRIM(LTRIM(i.State))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.State)),'') != ISNULL(RTRIM(LTRIM(d.State)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update ZipCode
BEGIN TRY
if update(ZipCode)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Zip Code',d.ZipCode,i.ZipCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ZipCode,'') != ISNULL(d.ZipCode,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update OfficePhoneNum
BEGIN TRY
if update(City)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Office Phone Number',d.OfficePhoneNum,i.OfficePhoneNum,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.OfficePhoneNum,'') != ISNULL(d.OfficePhoneNum,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update FaxNumber
BEGIN TRY
if update(FaxNumber)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Fax Number',d.FaxNumber,i.FaxNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FaxNumber,'') != ISNULL(d.FaxNumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update Email
BEGIN TRY
if update(Email)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Email',(RTRIM(LTRIM(d.Email))),(RTRIM(LTRIM(i.Email))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Email)),'') != ISNULL(RTRIM(LTRIM(d.Email)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update Website
BEGIN TRY
if update(Website)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Website',(RTRIM(LTRIM(d.Website))),(RTRIM(LTRIM(i.Website))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Website)),'') != ISNULL(RTRIM(LTRIM(d.Website)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update PayToAddress1
BEGIN TRY
if update(PayToAddress1)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Pay To Address1',(RTRIM(LTRIM(d.PayToAddress1))),(RTRIM(LTRIM(i.PayToAddress1))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PayToAddress1)),'') != ISNULL(RTRIM(LTRIM(d.PayToAddress1)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update PayToAddress2
BEGIN TRY
if update(PayToAddress2)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Pay To Address2',(RTRIM(LTRIM(d.PayToAddress2))),(RTRIM(LTRIM(i.PayToAddress2))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PayToAddress2)),'') != ISNULL(RTRIM(LTRIM(d.PayToAddress2)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update PayToCity
BEGIN TRY
if update(PayToCity)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Pay To City',(RTRIM(LTRIM(d.PayToCity))),(RTRIM(LTRIM(i.PayToCity))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PayToCity)),'') != ISNULL(RTRIM(LTRIM(d.PayToCity)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update PayToState
BEGIN TRY
if update(PayToState)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Pay To State',(RTRIM(LTRIM(d.PayToState))),(RTRIM(LTRIM(i.PayToState))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PayToState)),'') != ISNULL(RTRIM(LTRIM(d.PayToState)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update PayToZipCode
BEGIN TRY
if update(PayToZipCode)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Pay To Zip Code',d.PayToZipCode,i.PayToZipCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PayToZipCode,'') != ISNULL(d.PayToZipCode,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update DefaultLocationID
BEGIN TRY
if update(DefaultLocationID)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Default Location ID',d.DefaultLocationID,i.DefaultLocationID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DefaultLocationID,'') != ISNULL(d.DefaultLocationID,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update WorkingHours
BEGIN TRY
if update(WorkingHours)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Working Hours',d.WorkingHours,i.WorkingHours,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.WorkingHours,'') != ISNULL(d.WorkingHours,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update Notes
BEGIN TRY
if update(Notes)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',(RTRIM(LTRIM(d.Notes))),(RTRIM(LTRIM(i.Notes))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Notes)),'') != ISNULL(RTRIM(LTRIM(d.Notes)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update IsActive
BEGIN TRY
if update(IsActive)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsActive',(case when d.IsActive = 1 then 'Yes' else 'No' end),(case when i.IsActive = 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsActive,'') != ISNULL(d.IsActive,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update IsDeleted
BEGIN TRY
if update(IsDeleted)
insert into PracticeAudit(PracticeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsDeleted',d.IsDeleted,i.IsDeleted,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsDeleted,'') != ISNULL(d.IsDeleted,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
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
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM ProviderAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM ProviderAudit  

--when update Name
BEGIN TRY
if update(Name)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',(RTRIM(LTRIM(d.Name))),(RTRIM(LTRIM(i.Name))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Name)),'') != ISNULL(RTRIM(LTRIM(d.Name)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update LastName
BEGIN TRY
if update(LastName)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Last Name',(RTRIM(LTRIM(d.LastName))),(RTRIM(LTRIM(i.LastName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.LastName)),'') != ISNULL(RTRIM(LTRIM(d.LastName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update FirstName
BEGIN TRY
if update(FirstName)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'First Name',(RTRIM(LTRIM(d.FirstName))),(RTRIM(LTRIM(i.FirstName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.FirstName)),'') != ISNULL(RTRIM(LTRIM(d.FirstName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update MiddleInitial
BEGIN TRY
if update(MiddleInitial)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Middle Initial',d.MiddleInitial,i.MiddleInitial,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.MiddleInitial,'') != ISNULL(d.MiddleInitial,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Title
BEGIN TRY
if update(Title)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Title',(RTRIM(LTRIM(d.Title))),(RTRIM(LTRIM(i.Title))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Title)),'') != ISNULL(RTRIM(LTRIM(d.Title)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update NPI
BEGIN TRY
if update(NPI)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'NPI',d.NPI,i.NPI,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.NPI,'') != ISNULL(d.NPI,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update SSN
BEGIN TRY
if update(SSN)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'SSN',d.SSN,i.SSN,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SSN,'') != ISNULL(d.SSN,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update TaxonomyCode
BEGIN TRY
if update(TaxonomyCode)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Taxonomy Code',d.TaxonomyCode,i.TaxonomyCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.TaxonomyCode,'') != ISNULL(d.TaxonomyCode,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Address1
BEGIN TRY
if update(Address1)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address1',(RTRIM(LTRIM(d.Address1))),(RTRIM(LTRIM(i.Address1))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Address1)),'') != ISNULL(RTRIM(LTRIM(d.Address1)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Address2
BEGIN TRY
if update(Address2)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address2',(RTRIM(LTRIM(d.Address2))),(RTRIM(LTRIM(i.Address2))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Address2)),'') != ISNULL(RTRIM(LTRIM(d.Address2)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update City
BEGIN TRY
if update(City)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'City',(RTRIM(LTRIM(d.City))),(RTRIM(LTRIM(i.City))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.City)),'') != ISNULL(RTRIM(LTRIM(d.City)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update State
BEGIN TRY
if update([State])
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'State',(RTRIM(LTRIM(d.[State]))),(RTRIM(LTRIM(i.[State]))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.[State])),'') != ISNULL(RTRIM(LTRIM(d.[State])),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ZipCode
BEGIN TRY
if update(ZipCode)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'ZipCode',d.ZipCode,i.ZipCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ZipCode,'') != ISNULL(d.ZipCode,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update OfficePhoneNum
BEGIN TRY
if update(OfficePhoneNum)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Office Phone Number',d.OfficePhoneNum,i.OfficePhoneNum,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.OfficePhoneNum,'') != ISNULL(d.OfficePhoneNum,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update FaxNumber
BEGIN TRY
if update(FaxNumber)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Fax Number',d.FaxNumber,i.FaxNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FaxNumber,'') != ISNULL(d.FaxNumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

----when update PracticeID
BEGIN TRY
if update(PracticeID)
--select * into #t from practice
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Practice ID',cDel.[Name],cUpd.[Name],d.PracticeID,i.PracticeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PracticeID,'1') != ISNULL(d.PracticeID,'1')
Join Practice as cDel on cDel.ID = ISNULL(d.PracticeID,'1')
join Practice as cUpd on cUpd.ID = ISNULL(i.PracticeID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update BillUnderProvider
BEGIN TRY
if update(BillUnderProvider)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Bill Under Provider',(case when d.BillUnderProvider = 1 then 'Yes' when d.BillUnderProvider =0 then 'No' end),(case when i.BillUnderProvider = 1 then 'Yes' when i.BillUnderProvider =0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.BillUnderProvider,'') != ISNULL(d.BillUnderProvider,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Email
BEGIN TRY
if update(Email)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Email',(RTRIM(LTRIM(d.Email))),(RTRIM(LTRIM(i.Email))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Email)),'') != ISNULL(RTRIM(LTRIM(d.Email)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update DEANumber
BEGIN TRY
if update(DEANumber)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'DEA Number',d.DEANumber,i.DEANumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DEANumber,'') != ISNULL(d.DEANumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update UPINNumber
BEGIN TRY
if update(UPINNumber)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'UPIN Number',d.UPINNumber,i.UPINNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.UPINNumber,'') !=ISNULL(d.UPINNumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update LicenceNumber
BEGIN TRY
if update(LicenceNumber)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Licence Number',d.LicenceNumber,i.LicenceNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.LicenceNumber,'') != ISNULL(d.LicenceNumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update IsActive
BEGIN TRY
if update(IsActive)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsActive',(case when d.IsActive = 1 then 'Yes' when d.IsActive =0 then 'No' end),(case when i.IsActive = 1 then 'Yes' when i.IsActive =0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsActive,'2') != ISNULL(d.IsActive,'2')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update IsDeleted
BEGIN TRY
if update(IsDeleted)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsDeleted',d.IsDeleted,i.IsDeleted,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsDeleted,'') != ISNULL(d.IsDeleted,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Notes
BEGIN TRY 
if update(Notes)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',(RTRIM(LTRIM(d.Notes))),(RTRIM(LTRIM(i.Notes))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Notes)),'') != ISNULL(RTRIM(LTRIM(d.Notes)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update PayToAddress1
BEGIN TRY 
if update(PayToAddress1)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Pay To Address1',(RTRIM(LTRIM(d.PayToAddress1))),(RTRIM(LTRIM(i.PayToAddress1))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PayToAddress1)),'') != ISNULL(RTRIM(LTRIM(d.PayToAddress1)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update PayToAddress2
BEGIN TRY 
if update(PayToAddress2)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Pay To Address2',(RTRIM(LTRIM(d.PayToAddress2))),(RTRIM(LTRIM(i.PayToAddress2))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PayToAddress2)),'') != ISNULL(RTRIM(LTRIM(d.PayToAddress2)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update PayToCity
BEGIN TRY 
if update(PayToCity)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Pay To City',(RTRIM(LTRIM(d.PayToCity))),(RTRIM(LTRIM(i.PayToCity))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PayToCity)),'') != ISNULL(RTRIM(LTRIM(d.PayToCity)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update PayToState
BEGIN TRY 
if update(PayToState)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Pay To State',(RTRIM(LTRIM(d.PayToState))),(RTRIM(LTRIM(i.PayToState))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PayToState)),'') != ISNULL(RTRIM(LTRIM(d.PayToState)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update PayToZipCode
BEGIN TRY 
if update(PayToZipCode)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Pay To Zip Code',d.PayToZipCode,i.PayToZipCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PayToZipCode,'') != ISNULL(d.PayToZipCode,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update ReportTaxID
BEGIN TRY 
if update(ReportTaxID)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Report Tax ID',(case when d.ReportTaxID = 1 then 'Yes' when d.ReportTaxID =0 then 'No' end),(case when i.ReportTaxID = 1 then 'Yes' when i.ReportTaxID =0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReportTaxID,'2') != ISNULL(d.ReportTaxID,'2')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update PhoneNumExt
BEGIN TRY 
if update(PhoneNumExt)
insert into ProviderAudit(ProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Phone Number Extension',d.PhoneNumExt,i.PhoneNumExt,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PhoneNumExt,'') != ISNULL(d.PhoneNumExt,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;
END
GO


--=====================================
--Description: <tgrTypeOfServiceAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrTypeOfServiceAudit')
DROP TRIGGER [dbo].[tgrTypeOfServiceAudit]
GO
Create TRIGGER [dbo].[tgrTypeOfServiceAudit]
on [dbo].[TypeOfService] after update
AS 
BEGIN 
set Xact_abort off
Declare @TransactionID int;
select @TransactionID = MAX(TransactionID) from TypeOfServiceAudit
if @TransactionID is null set @TransactionID = 1
else select @TransactionID = Max(TransactionID) + 1 from TypeOfServiceAudit

--when update(Code)
BEGIN TRY
IF Update(Code)
INSERT INTO TypeOfServiceAudit(TypeOfServiceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Code',d.Code,i.Code,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Code,'') != ISNULL(d.Code,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update(Description)
BEGIN TRY
if update(Description)
insert into TypeOfServiceAudit(TypeOfServiceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Description',(RTRIM(LTRIM(d.Description))),(RTRIM(LTRIM(i.Description))),HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
from inserted as i 
inner join Deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Description)),'') != ISNULL(RTRIM(LTRIM(d.Description)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update(IsActive)
BEGIN TRY
if update(IsActive)
insert into TypeOfServiceAudit(TypeOfServiceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsActive',(case when d.IsActive = 1 then 'Yes' when d.IsActive =0 then 'No' end),(case when i.IsActive = 1 then 'Yes' when i.IsActive =0 then 'No' end),HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
from inserted as i 
inner join Deleted as d on i.ID = d.ID and ISNULL(i.IsActive,'') != ISNULL(d.IsActive,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update(IsDeleted)
BEGIN TRY
if update(IsDeleted)
insert into TypeOfServiceAudit(TypeOfServiceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'IsDeleted',d.IsDeleted,i.IsDeleted,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsDeleted,'') != ISNULL(d.IsDeleted,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
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
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM [dbo].[RefProviderAudit]
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM [dbo].[RefProviderAudit]

--when update Name
BEGIN TRY
if update(Name)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',(RTRIM(LTRIM(d.Name))),(RTRIM(LTRIM(i.Name))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Name)),'') != ISNULL(RTRIM(LTRIM(d.Name)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update LastName
BEGIN TRY
if update(LastName)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Last Name',(RTRIM(LTRIM(d.LastName))),(RTRIM(LTRIM(i.LastName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.LastName)),'') != ISNULL(RTRIM(LTRIM(d.LastName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update FirstName
BEGIN TRY
if update(FirstName)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'First Name',(RTRIM(LTRIM(d.FirstName))),(RTRIM(LTRIM(i.FirstName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.FirstName)),'') != ISNULL(RTRIM(LTRIM(d.FirstName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update MiddleInitial
BEGIN TRY
if update(MiddleInitial)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Middle Initial',d.MiddleInitial,i.MiddleInitial,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.MiddleInitial,'') != ISNULL(d.MiddleInitial,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Title
BEGIN TRY
if update(Title)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Title',(RTRIM(LTRIM(d.Title))),(RTRIM(LTRIM(i.Title))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Title)),'') != ISNULL(RTRIM(LTRIM(d.Title)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update NPI
BEGIN TRY
if update(NPI)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'NPI',d.NPI,i.NPI,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.NPI,'') != ISNULL(d.NPI,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update SSN
BEGIN TRY
if update(SSN)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'SSN',d.SSN,i.SSN,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SSN,'') != ISNULL(d.SSN,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update TaxonomyCode
BEGIN TRY
if update(TaxonomyCode)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Taxonomy Code',d.TaxonomyCode,i.TaxonomyCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.TaxonomyCode,'') != ISNULL(d.TaxonomyCode,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update TaxID
BEGIN TRY
if update(TaxID)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Tax ID',d.TaxID,i.TaxID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.TaxID,'') != ISNULL(d.TaxID,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Address1
BEGIN TRY
if update(Address1)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address1',(RTRIM(LTRIM(d.Address1))),(RTRIM(LTRIM(i.Address1))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Address1)),'') != ISNULL(RTRIM(LTRIM(d.Address1)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Address2
BEGIN TRY
if update(Address2)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address2',(RTRIM(LTRIM(d.Address2))),(RTRIM(LTRIM(i.Address2))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Address2)),'') != ISNULL(RTRIM(LTRIM(d.Address2)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update City
BEGIN TRY
if update(City)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'City',(RTRIM(LTRIM(d.City))),(RTRIM(LTRIM(i.City))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.City)),'') != ISNULL(RTRIM(LTRIM(d.City)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update State
BEGIN TRY
if update(State)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'State',(RTRIM(LTRIM(d.State))),(RTRIM(LTRIM(i.State))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.State)),'') != ISNULL(RTRIM(LTRIM(d.State)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ZipCode
BEGIN TRY
if update(ZipCode)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Zip Code',d.ZipCode,i.ZipCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ZipCode,'') != ISNULL(d.ZipCode,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update OfficePhoneNum
BEGIN TRY
if update(OfficePhoneNum)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Office Phone Number',d.OfficePhoneNum,i.OfficePhoneNum,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.OfficePhoneNum,'') != ISNULL(d.OfficePhoneNum,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Faxnumber
BEGIN TRY
if update(FaxNumber)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Fax Number',d.FaxNumber,i.FaxNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FaxNumber,'') != ISNULL(d.FaxNumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update PracticeID
BEGIN TRY
if update (PracticeID)
--Select * into #t from Practice
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
Select i.ID,@TransactionID,'Practice ID',cDel.[Name],cUpd.[Name],d.Email,i.Email,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(i.PracticeID,'1') != ISNULL(d.PracticeID,'1')
Join Practice as cDel on cDel.ID = ISNULL(d.PracticeID,'1')
join Practice as cUpd on cUpd.ID = ISNULL(i.PracticeID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Email
BEGIN TRY
if update(Email)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Email',(RTRIM(LTRIM(d.Email))),(RTRIM(LTRIM(i.Email))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Email)),'') != ISNULL(RTRIM(LTRIM(d.Email)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update IsActive
BEGIN TRY
if update(IsActive)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsActive',(case when d.IsActive = 1 then 'Yes' when d.IsActive =0 then 'No' end),(case when i.IsActive = 1 then 'Yes' when i.IsActive =0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsActive,'2') != ISNULL(d.IsActive,'2')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update IsDeleted
BEGIN TRY
if update(IsDeleted)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsDeleted',d.IsDeleted,i.IsDeleted,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsDeleted,'') != ISNULL(d.IsDeleted,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Notes
BEGIN TRY
if update(Notes)
insert into RefProviderAudit(RefProviderID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',(RTRIM(LTRIM(d.Notes))),(RTRIM(LTRIM(i.Notes))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Notes)),'') != ISNULL(RTRIM(LTRIM(d.Notes)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
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
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM LocationAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM LocationAudit

--when update(Name)
BEGIN TRY
IF Update(Name)
INSERT INTO LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Name',(RTRIM(LTRIM(d.Name))),(RTRIM(LTRIM(i.Name))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Name)),'') != ISNULL(RTRIM(LTRIM(d.Name)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update(OrganizationName)
BEGIN TRY
if update(OrganizationName)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Organization Name',(RTRIM(LTRIM(d.OrganizationName))),(RTRIM(LTRIM(i.OrganizationName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join Deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.OrganizationName)),'') != ISNULL(RTRIM(LTRIM(d.OrganizationName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update PracticeID
BEGIN TRY
if update (PracticeID)
--Select * into #t from Practice
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
Select i.ID,@TransactionID,'Practice ID',cDel.[Name],cUpd.[Name],d.PracticeID,i.PracticeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(i.PracticeID,'1') != ISNULL(d.PracticeID,'1')
Join Practice as cDel on cDel.ID = ISNULL(d.PracticeID,'1')
join Practice as cUpd on cUpd.ID = ISNULL(i.PracticeID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

if update(NPI)
BEGIN TRY
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'NPI',d.NPI,i.NPI,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join Deleted as d on i.ID = d.ID and ISNULL(i.NPI,'') != ISNULL(d.NPI,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update POSID
BEGIN TRY
if update (POSID)
--Select * into #t1 from POS
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
Select i.ID,@TransactionID,'POS ID',cDel.[Name],cUpd.[Name],d.POSID,i.POSID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(i.POSID,'1') != ISNULL(d.POSID,'1')
Join Practice as cDel on cDel.ID = ISNULL(d.POSID,'1')
join Practice as cUpd on cUpd.ID = ISNULL(i.POSID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update(Address1)
BEGIN TRY
if update(Address1)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'Address1',(RTRIM(LTRIM(d.Address1))),(RTRIM(LTRIM(i.Address1))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Address1)),'') != ISNULL(RTRIM(LTRIM(d.Address1)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update(Address2)
BEGIN TRY
if update(Address2)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'Address2',(RTRIM(LTRIM(d.Address2))),(RTRIM(LTRIM(i.Address2))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Address2)),'') != ISNULL(RTRIM(LTRIM(d.Address2)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update(City)
BEGIN TRY
if update(City)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'City',(RTRIM(LTRIM(d.City))),(RTRIM(LTRIM(i.City))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.City)),'') != ISNULL(RTRIM(LTRIM(d.City)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update(State)
BEGIN TRY
if update(State)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'State',(RTRIM(LTRIM(d.State))),(RTRIM(LTRIM(i.State))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.State)),'') != ISNULL(RTRIM(LTRIM(d.State)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update(ZipCode)
BEGIN TRY
if update(ZipCode)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'Zip Code',d.ZipCode,i.ZipCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(i.ZipCode,'') != ISNULL(d.ZipCode,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update(CLIANumber)
BEGIN TRY
if update(CLIANumber)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'CLIA Number',d.CLIANumber,i.CLIANumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(i.CLIANumber,'') != ISNULL(d.CLIANumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update(Fax)
BEGIN TRY
if update(Fax)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'Fax',d.Fax,i.Fax,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(i.Fax,'') != ISNULL(d.Fax,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update(website)
BEGIN TRY
if update(website)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'website',(RTRIM(LTRIM(d.website))),(RTRIM(LTRIM(i.website))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.website)),'') != ISNULL(RTRIM(LTRIM(d.website)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update(Email)
BEGIN TRY
if update(Email)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'Email',(RTRIM(LTRIM(d.Email))),(RTRIM(LTRIM(i.Email))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Email)),'') != ISNULL(RTRIM(LTRIM(d.Email)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update(PhoneNumber)
BEGIN TRY
if update(PhoneNumber)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'Phone Number',d.PhoneNumber,i.PhoneNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(i.PhoneNumber,'') != ISNULL(d.PhoneNumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update(Notes)
BEGIN TRY
if update(Notes)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'Notes',(RTRIM(LTRIM(d.Notes))),(RTRIM(LTRIM(i.Notes))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Notes)),'') != ISNULL(RTRIM(LTRIM(d.Notes)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update(IsActive)
BEGIN TRY
if update(IsActive)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'IsActive',(case when d.IsActive = 1 then 'Yes' when d.IsActive = 0 then 'No' end),(case when i.IsActive = 1 then 'Yes' when i.IsActive =0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsActive,'') != ISNULL(d.IsActive,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update(IsDeleted)
BEGIN TRY
if update(IsDeleted)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'IsDeleted',d.IsDeleted,i.IsDeleted,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsDeleted,'') != ISNULL(d.IsDeleted,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update(PhoneNumExt)
BEGIN TRY
if update(PhoneNumExt)
insert into LocationAudit(LocationID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID,@TransactionID,'Phone Number Extension',d.PhoneNumExt,i.PhoneNumExt,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i 
inner join deleted as d on i.ID = d.ID and ISNULL(i.PhoneNumExt,'') != ISNULL(d.PhoneNumExt,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
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
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM ChargeAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM ChargeAudit  

--When User Update VisitID
BEGIN TRY
IF Update(VisitID)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'VisitID',d.VisitId,i.visitId,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.VisitID,'') != ISNULL(d.VisitID,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update ClientID
BEGIN TRY
IF Update(ClientID)
--Select * Into #t from Client 
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Client ID',cDel.[Name],cUpd.[Name],d.ClientID,i.ClientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ClientID,'1') != ISNULL(d.ClientID,'1')
Join Client as cDel on cDel.ID = ISNULL(d.ClientID,'1')
join Client as cUpd on cUpd.ID = ISNULL(i.ClientID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update PracticeID
BEGIN TRY
IF Update(PracticeID)
--Select * Into #t15 from Practice
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Practice ID',pDel.[Name],pUpd.[Name],d.PracticeID,i.PracticeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PracticeID,'1') != ISNULL(d.PracticeID,'1')
Join Practice as pDel on pdel.ID = ISNULL(d.PracticeID,'1')
join Practice as pUpd on pUpd.ID = ISNULL(i.PracticeID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update LocationID
BEGIN TRY 
IF Update(LocationID)
--Select * Into #t14 from Location
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Location ID',dLoc.Name,uLoc.Name,d.LocationID,i.LocationID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.LocationID,'1') != ISNULL(d.LocationID,'1')
JOIN Location as dLoc on dLoc.ID = ISNULL(d.LocationID,'1')
JOIN Location as uLoc on uLoc.ID = ISNULL(i.LocationID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update POSID
BEGIN TRY
IF Update(POSID)
--Select * Into #t13 from POS
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'POS ID',dPOS.[Name],uPOS.[Name],d.POSID,i.POSID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.POSID,'1') != ISNULL(d.POSID,'1')
Join POS as dPOS on dPOS.ID = ISNULL(d.POSID,'1')
join POS as uPOS on uPOS.ID = ISNULL(i.POSID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update ProviderID
BEGIN TRY
IF Update(ProviderID)
--Select * Into #t12 from Provider
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Provider ID',dProv.Name,uProv.Name,d.ProviderID,i.ProviderID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ProviderID,'1') != ISNULL(d.ProviderID,'1')
Join Provider as dProv on dProv.ID = ISNULL(d.ProviderID,'1')
join Provider as uProv on  uProv.ID = ISNULL(i.ProviderID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update RefProviderID
BEGIN TRY
IF Update(RefProviderID)
--Select * Into #t11 from RefProvider
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Reference Provider ID',pDel.[Name],pUpd.[Name],d.RefProviderID,i.RefProviderID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.RefProviderID,'1') != ISNULL(d.RefProviderID,'1')
Join RefProvider as pDel on pdel.ID = ISNULL(d.RefProviderID,'1')
join RefProvider as pUpd on pUpd.ID = ISNULL(i.RefProviderID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update SupervisingProvID
BEGIN TRY
IF Update(SupervisingProvID)
--Select * Into #t10 from Provider
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Supervising Provider ID',dProv.Name,uProv.Name,d.SupervisingProvID,i.SupervisingProvID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SupervisingProvID,'1') != ISNULL(d.SupervisingProvID,'1')
Join Provider as dProv on dProv.ID = ISNULL(d.SupervisingProvID,'1') 
join Provider as uProv on  uProv.ID = ISNULL(i.SupervisingProvID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update OrderingProvID
BEGIN TRY
IF Update(OrderingProvID)
--Select * Into #t9 from Provider
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Ordering Provider ID',dProv.Name,uProv.Name,d.OrderingProvID,i.OrderingProvID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.OrderingProvID,'1') != ISNULL(d.OrderingProvID,'1')
Join Provider as dProv on dProv.ID = ISNULL(d.OrderingProvID,'1')
join Provider as uProv on  uProv.ID = ISNULL(i.OrderingProvID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update PatientID
BEGIN TRY
IF Update(PatientID)
--Select * Into #t8 from Patient
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Patient ID',dPat.LastName + ', ' + dPat.FirstName,uPat.LastName + ', ' + uPat.FirstName,d.PatientID,i.PatientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PatientID,'1') != ISNULL(d.PatientID,'1')
Join Patient as dPat on dPat.ID = ISNULL(d.PatientID,'1')
join Patient as uPat on  uPat.ID = ISNULL(i.PatientID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update PrimaryPatientPlanID
BEGIN TRY
IF Update(PrimaryPatientPlanID)
--Select * Into #t6 from PatientPlan
--Select * Into #t7 from InsurancePlan 
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Patient Plan ID',dInsPlan.PlanName,uInsPlan.PlanName,d.PrimaryPatientPlanID,i.PrimaryPatientPlanID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryPatientPlanID,'1') != ISNULL(d.PrimaryPatientPlanID,'1')
join PatientPlan as dpatPlan on ISNULL(dpatPlan.ID,'') = ISNULL(d.PrimaryPatientPlanID,'1')
join InsurancePlan as dInsPlan on ISNULL(dpatPlan.InsurancePlanID,'1') = ISNULL(dInsPlan.ID,'')

left join PatientPlan as upatPlan on ISNULL(upatPlan.ID,'') = ISNULL(i.PrimaryPatientPlanID,'1')
left join InsurancePlan as uInsPlan on ISNULL(upatPlan.InsurancePlanID,'1') = ISNULL(uInsPlan.ID,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update SecondaryPatientPlanID
BEGIN TRY
IF Update(SecondaryPatientPlanID)
--Select * Into #t4 from PatientPlan
--Select * Into #t5 from InsurancePlan 
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Patient Plan ID',dInsPlan.PlanName,uInsPlan.PlanName,d.SecondaryPatientPlanID,i.SecondaryPatientPlanID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryPatientPlanID,'1') != ISNULL(d.SecondaryPatientPlanID,'1')

join PatientPlan as dpatPlan on ISNULL(dpatPlan.ID,'') = ISNULL(d.SecondaryPatientPlanID,'1')
join InsurancePlan as dInsPlan on ISNULL(dpatPlan.InsurancePlanID,'1') = ISNULL(dInsPlan.ID,'')

left join PatientPlan as upatPlan on ISNULL(upatPlan.ID,'') = ISNULL(i.SecondaryPatientPlanID,'1')
left join InsurancePlan as uInsPlan on ISNULL(upatPlan.InsurancePlanID,'1') = ISNULL(uInsPlan.ID,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update TertiaryPatientPlanID
BEGIN TRY
IF Update(TertiaryPatientPlanID)
--Select * Into #t2 from PatientPlan
--Select * Into #t3 from InsurancePlan 
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Patient Plan ID',dInsPlan.PlanName,uInsPlan.PlanName,d.TertiaryPatientPlanID,i.TertiaryPatientPlanID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryPatientPlanID,'1') != ISNULL(d.TertiaryPatientPlanID,'1')


join PatientPlan as dpatPlan on ISNULL(dpatPlan.ID,'') = ISNULL(d.TertiaryPatientPlanID,'1')
join InsurancePlan as dInsPlan on ISNULL(dpatPlan.InsurancePlanID,'1') = ISNULL(dInsPlan.ID,'')

left join PatientPlan as upatPlan on ISNULL(upatPlan.ID,'') = ISNULL(i.TertiaryPatientPlanID,'1')
left join InsurancePlan as uInsPlan on ISNULL(upatPlan.InsurancePlanID,'1') = ISNULL(uInsPlan.ID,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update SubmissionLogID
BEGIN TRY
IF Update(SubmissionLogID)
--Select * Into #t1 from SubmissionLog  
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'SubmissionLog ID',dSLog.ID,uSLog.ID,d.SubmissionLogID,i.SubmissionLogID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SubmissionLogID,'1') != ISNULL(d.SubmissionLogID,'1')
Join SubmissionLog as dSLog on dSLog.ID = ISNULL(d.SubmissionLogID,'1')
join SubmissionLog as uSLog on  uSLog.ID = ISNULL(i.SubmissionLogID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update CPTID
BEGIN TRY
IF Update(CPTID)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'CPT ID',dcpt.[Description],uPCpt.[Description],d.CPTID,i.CPTID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.CPTID,'1') != ISNULL(d.CPTID,'1')
Join Cpt as dCpt on dCpt.ID = ISNULL(d.CPTID,'1')
join Cpt as uPCpt on  uPCpt.ID = ISNULL(i.CPTID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update Modifier1ID
BEGIN TRY
IF Update(Modifier1ID)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Modifier1 ID',dMod.[Description],uPMod.[Description],d.Modifier1ID,i.Modifier1ID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Modifier1ID,'1') != ISNULL(d.Modifier1ID,'1')
Join Modifier as dMod on dMod.ID = ISNULL(d.Modifier1ID,'1')
join Modifier as uPMod on  uPMod.ID = ISNULL(i.Modifier1ID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update Modifier1Amount
BEGIN TRY
IF Update(Modifier1Amount)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Modifier1 Amount',d.Modifier1Amount,i.Modifier1Amount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Modifier1Amount,'1') != ISNULL(d.Modifier1Amount,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update Modifier2ID
BEGIN TRY
IF Update(Modifier2ID)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Modifier2 ID',dMod.[Description],uPMod.[Description],d.Modifier2ID,i.Modifier2ID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Modifier2ID,'1') != ISNULL(d.Modifier2ID,'1')
Join Modifier as dMod on dMod.ID = ISNULL(d.Modifier2ID,'1')
join Modifier as uPMod on  uPMod.ID = ISNULL(i.Modifier2ID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update Modifier2Amount
BEGIN TRY
IF Update(Modifier2Amount)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Modifier2 Amount',d.Modifier2Amount,i.Modifier2Amount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Modifier2Amount,'1')  !=  ISNULL(d.Modifier2Amount,'1') 
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update Modifier3ID
BEGIN TRY
IF Update(Modifier3ID)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Modifier3 ID',dMod.[Description],uPMod.[Description],d.Modifier3ID,i.Modifier3ID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Modifier3ID,'1') != ISNULL(d.Modifier3ID,'1')
Join Modifier as dMod on dMod.ID = ISNULL(d.Modifier3ID,'1')
join Modifier as uPMod on  uPMod.ID = ISNULL(i.Modifier3ID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update Modifier3Amount
BEGIN TRY
IF Update(Modifier3Amount)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Modifier3 Amount',d.Modifier3Amount,i.Modifier3Amount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Modifier3Amount,'1') != ISNULL(d.Modifier3Amount,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update Modifier4ID
BEGIN TRY
IF Update(Modifier4ID)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Modifier4 ID',dMod.[Description],uPMod.[Description],d.Modifier4ID,i.Modifier4ID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Modifier4ID,'1') != ISNULL(d.Modifier4ID,'1')
Join Modifier as dMod on dMod.ID = ISNULL(d.Modifier4ID,'1')
join Modifier as uPMod on  uPMod.ID = ISNULL(i.Modifier4ID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update Modifier4Amount
BEGIN TRY
IF Update(Modifier4Amount)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Modifier4 Amount',d.Modifier4Amount,i.Modifier4Amount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Modifier4Amount,'1') != ISNULL(d.Modifier4Amount,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update Units
BEGIN TRY
IF Update(Units)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Units',d.Units,i.Units,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Units,'') != ISNULL(d.Units,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update Minutes
BEGIN TRY
IF Update(Minutes)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Minutes',d.Minutes,i.Minutes,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Minutes,'') != ISNULL(d.Minutes,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update NdcUnits
BEGIN TRY
IF Update(NdcUnits)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Ndc Units',d.NdcUnits,i.NdcUnits,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.NdcUnits,'') != ISNULL(d.NdcUnits,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update NdcMeasurementUnit
BEGIN TRY
IF Update(NdcMeasurementUnit)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Ndc Measurement Unit',d.NdcMeasurementUnit,i.NdcMeasurementUnit,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.NdcMeasurementUnit,'') != ISNULL(d.NdcMeasurementUnit,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update Description
BEGIN TRY
IF Update(Description)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Description',(RTRIM(LTRIM(d.Description))),(RTRIM(LTRIM(i.Description))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Description)),'') != ISNULL(RTRIM(LTRIM(d.Description)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update NdcNumber
BEGIN TRY
IF Update(NdcNumber)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Ndc Number',d.NdcNumber,i.NdcNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.NdcNumber,'') != ISNULL(d.NdcNumber,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update UnitOfMeasurement
BEGIN TRY
IF Update(UnitOfMeasurement)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Unit Of Measurement',d.UnitOfMeasurement,i.UnitOfMeasurement,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.UnitOfMeasurement,'') != ISNULL(d.UnitOfMeasurement,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update DateOfServiceFrom
BEGIN TRY
IF Update(DateOfServiceFrom)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Date Of Service From',d.DateOfServiceFrom,i.DateOfServiceFrom,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(cast(i.DateOfServiceFrom as date),'') != ISNULL(cast(d.DateOfServiceFrom as date),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update DateOfServiceTo
BEGIN TRY
IF Update(DateOfServiceTo)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Date Of Service To',d.DateOfServiceTo,i.DateOfServiceTo,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(cast(i.DateOfServiceTo as date),'') != ISNULL(cast(d.DateOfServiceTo as date),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update StartTime
BEGIN TRY
IF Update(StartTime)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Start Time',d.StartTime,i.StartTime,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(cast(i.StartTime as datetime),'') != ISNULL(cast(d.StartTime as datetime),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update EndTime
BEGIN TRY
IF Update(EndTime)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'End Time',d.EndTime,i.EndTime,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(cast(i.EndTime as datetime),'') != ISNULL(cast(d.EndTime as datetime),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update TimeUnits
BEGIN TRY
IF Update(TimeUnits)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Time Units',d.TimeUnits,i.TimeUnits,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TimeUnits,'') != ISNULL(d.TimeUnits,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update BaseUnits
BEGIN TRY
IF Update(BaseUnits)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Base Units',d.BaseUnits,i.BaseUnits,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.BaseUnits,'') != ISNULL(d.BaseUnits,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update ModifierUnits
BEGIN TRY
IF Update(ModifierUnits)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Modifier Units',d.ModifierUnits,i.ModifierUnits,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ModifierUnits,'') != ISNULL(d.ModifierUnits,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update Pointer1
BEGIN TRY
IF Update(Pointer1)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Pointer1',d.Pointer1,i.Pointer1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Pointer1,'') != ISNULL(d.Pointer1,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update Pointer2
BEGIN TRY
IF Update(Pointer2)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Pointer2',d.Pointer2,i.Pointer2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Pointer2,'') != ISNULL(d.Pointer2,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update Pointer3
BEGIN TRY
IF Update(Pointer3)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Pointer3',d.Pointer3,i.Pointer3,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Pointer3,'') != ISNULL(d.Pointer3,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update Pointer4
BEGIN TRY
IF Update(Pointer4)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Pointer4',d.Pointer4,i.Pointer4,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Pointer4,'') != ISNULL(d.Pointer4,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update TotalAmount
BEGIN TRY
IF Update(TotalAmount)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Total Amount',d.TotalAmount,i.TotalAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TotalAmount,'1') != ISNULL(d.TotalAmount,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update Copay
BEGIN TRY
IF Update(Copay)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Copay',d.Copay,i.Copay,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Copay,'1') != ISNULL(d.Copay,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update Coinsurance
BEGIN TRY
IF Update(Coinsurance)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Coinsurance',d.Coinsurance,i.Coinsurance,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Coinsurance,'1') != ISNULL(d.Coinsurance,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update PrimaryBilledAmount
BEGIN TRY
IF Update(PrimaryBilledAmount)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Billed Amount',d.PrimaryBilledAmount,i.PrimaryBilledAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryBilledAmount,'1') != ISNULL(d.PrimaryBilledAmount,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update Deductible
BEGIN TRY
IF Update(Deductible)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Deductible',d.Deductible,i.Deductible,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Deductible,'1') != ISNULL(d.Deductible,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update PrimaryAllowed
BEGIN TRY
IF Update(PrimaryAllowed)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Allowed',d.PrimaryAllowed,i.PrimaryAllowed,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryAllowed,'1') != ISNULL(d.PrimaryAllowed,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update PrimaryWriteOff
BEGIN TRY
IF Update(PrimaryWriteOff)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Write Off',d.PrimaryWriteOff,i.PrimaryWriteOff,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryWriteOff,'1') != ISNULL(d.PrimaryWriteOff,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update PrimaryPaid
BEGIN TRY
IF Update(PrimaryPaid)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Paid',d.PrimaryPaid,i.PrimaryPaid,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryPaid,'1') != ISNULL(d.PrimaryPaid,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update PrimaryBal
BEGIN TRY
IF Update(PrimaryBal)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Balance',d.PrimaryBal,i.PrimaryBal,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryBal,'1') != ISNULL(d.PrimaryBal,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update PrimaryPatientBal
BEGIN TRY
IF Update(PrimaryPatientBal)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Patient Balance',d.PrimaryPatientBal,i.PrimaryPatientBal,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryPatientBal,'1') != ISNULL(d.PrimaryPatientBal,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update PrimaryTransferred
BEGIN TRY
IF Update(PrimaryTransferred)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Transferred',d.PrimaryTransferred,i.PrimaryTransferred,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryTransferred,'1') != ISNULL(d.PrimaryTransferred,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update PrimaryStatus
BEGIN TRY
IF Update(PrimaryStatus)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Status',(case d.PrimaryStatus when 'N' then 'New Charge'
when 'S' then 'Submitted'
when 'K' then '999 Accepted' 
when 'D' then '999 Denied'
when 'E' then '999 has Errors'
when 'P' then 'Paid'
when 'DN' then 'Denial'
when 'PT_P' then 'Patient Paid' 
when 'PPTS' then 'Paid-Transfered To Sec'
when 'PPTT' then 'Paid-Transfered To Ter'
when 'PPTP' then 'Paid-Transfered To Patient'
when 'SPTP' then 'Paid-Transfered To Patient'
when 'SPTT' then 'Paid-Transfered To Ter' 
when 'PR_TP' then 'Pat. Resp. Transferred to Pat'
when 'PPTM' then 'Paid-Medigaped' 
when 'M' then 'Medigaped' 
when 'RS' then 'Resubmitted' 
when 'P' then 'Pending'
when 'R' then 'Rejected'
Else '' end),
(case i.PrimaryStatus when 'N' then 'New Charge' 
when 'S' then 'Submitted'
when 'K' then '999 Accepted' 
when 'D' then '999 Denied' 
when 'E' then '999 has Errors'
when 'P' then 'Paid' 
when 'DN' then 'Denial'
when 'PT_P' then 'Patient Paid'
when 'PPTS' then 'Paid-Transfered To Sec'
when 'PPTT' then 'Paid-Transfered To Ter'
when 'PPTP' then 'Paid-Transfered To Patient'
when 'SPTP' then 'Paid-Transfered To Patient' 
when 'SPTT' then 'Paid-Transfered To Ter'
when 'PR_TP' then 'Pat. Resp. Transferred to Pat' 
when 'PPTM' then 'Paid-Medigaped' 
when 'M' then 'Medigaped'
when 'RS' then 'ReSubmitted' 
when 'P' then 'Pending'
when 'R' then 'Rejected'
Else '' end),HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryStatus,'') != ISNULL(d.PrimaryStatus,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update SecondaryBilledAmount
BEGIN TRY
IF Update(SecondaryBilledAmount)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Billed Amount',d.SecondaryBilledAmount,i.SecondaryBilledAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryBilledAmount,'1') != ISNULL(d.SecondaryBilledAmount,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update SecondaryAllowed
BEGIN TRY
IF Update(SecondaryAllowed)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Allowed',d.SecondaryAllowed,i.SecondaryAllowed,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryAllowed,'1') != ISNULL(d.SecondaryAllowed,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update SecondaryWriteOff
BEGIN TRY
IF Update(SecondaryWriteOff)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Write Off',d.SecondaryWriteOff,i.SecondaryWriteOff,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryWriteOff,'1') != ISNULL(d.SecondaryWriteOff,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update SecondaryPaid
BEGIN TRY
IF Update(SecondaryPaid)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Paid',d.SecondaryPaid,i.SecondaryPaid,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryPaid,'1') != ISNULL(d.SecondaryPaid,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update SecondaryPatResp
BEGIN TRY
IF Update(SecondaryPatResp)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Patient Response',d.SecondaryPatResp,i.SecondaryPatResp,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryPatResp,'1') != ISNULL(d.SecondaryPatResp,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update SecondaryBal
BEGIN TRY
IF Update(SecondaryBal)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Balance',d.SecondaryBal,i.SecondaryBal,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryBal,'1') != ISNULL(d.SecondaryBal,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update SecondaryPatientBal
BEGIN TRY
IF Update(SecondaryPatientBal)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Patient Balance',d.SecondaryPatientBal,i.SecondaryPatientBal,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryPatientBal,'1') != ISNULL(d.SecondaryPatientBal,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update SecondaryTransferred
BEGIN TRY
IF Update(SecondaryTransferred)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Transferred',d.SecondaryTransferred,i.SecondaryTransferred,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryTransferred,'1') != ISNULL(d.SecondaryTransferred,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update SecondaryStatus
BEGIN TRY
IF Update(SecondaryStatus)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Status',(case d.SecondaryStatus when 'N' then 'New Charge'
when 'S' then 'Submitted'
when 'K' then '999 Accepted' 
when 'D' then '999 Denied'
when 'E' then '999 has Errors'
when 'P' then 'Paid'
when 'DN' then 'Denial'
when 'PT_P' then 'Patient Paid' 
when 'PPTS' then 'Paid-Transfered To Sec'
when 'PPTT' then 'Paid-Transfered To Ter'
when 'PPTP' then 'Paid-Transfered To Patient'
when 'SPTP' then 'Paid-Transfered To Patient'
when 'SPTT' then 'Paid-Transfered To Ter' 
when 'PR_TP' then 'Pat. Resp. Transferred to Pat'
when 'PPTM' then 'Paid-Medigaped' 
when 'M' then 'Medigaped' 
when 'RS' then 'Resubmitted' 
when 'P' then 'Pending'
when 'R' then 'Rejected'
Else '' end),
(case i.SecondaryStatus when 'N' then 'New Charge' 
when 'S' then 'Submitted'
when 'K' then '999 Accepted' 
when 'D' then '999 Denied' 
when 'E' then '999 has Errors'
when 'P' then 'Paid' 
when 'DN' then 'Denial'
when 'PT_P' then 'Patient Paid'
when 'PPTS' then 'Paid-Transfered To Sec'
when 'PPTT' then 'Paid-Transfered To Ter'
when 'PPTP' then 'Paid-Transfered To Patient'
when 'SPTP' then 'Paid-Transfered To Patient' 
when 'SPTT' then 'Paid-Transfered To Ter'
when 'PR_TP' then 'Pat. Resp. Transferred to Pat' 
when 'PPTM' then 'Paid-Medigaped' 
when 'M' then 'Medigaped'
when 'RS' then 'ReSubmitted'
when 'P' then 'Pending'
when 'R' then 'Rejected'
Else '' end),HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryStatus,'') != ISNULL(d.SecondaryStatus,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update TertiaryBilledAmount
BEGIN TRY
IF Update(TertiaryBilledAmount)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Billed Amount',d.TertiaryBilledAmount,i.TertiaryBilledAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryBilledAmount,'1') != ISNULL(d.TertiaryBilledAmount,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update TertiaryAllowed
BEGIN TRY
IF Update(TertiaryAllowed)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Allowed',d.TertiaryAllowed,i.TertiaryAllowed,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryAllowed,'1') != ISNULL(d.TertiaryAllowed,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update TertiaryWriteOff
BEGIN TRY
IF Update(TertiaryWriteOff)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Write Off',d.TertiaryWriteOff,i.TertiaryWriteOff,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryWriteOff,'1') != ISNULL(d.TertiaryWriteOff,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update TertiaryPaid
BEGIN TRY
IF Update(TertiaryPaid)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Paid',d.TertiaryPaid,i.TertiaryPaid,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryPaid,'1') != ISNULL(d.TertiaryPaid,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update TertiaryPatResp
BEGIN TRY
IF Update(TertiaryPatResp)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Patient Response',d.TertiaryPatResp,i.TertiaryPatResp,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryPatResp,'1') != ISNULL(d.TertiaryPatResp,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update TertiaryBal
BEGIN TRY
IF Update(TertiaryBal)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Balance',d.TertiaryBal,i.TertiaryBal,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryBal,'1') != ISNULL(d.TertiaryBal,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update TertiaryPatientBal
BEGIN TRY
IF Update(TertiaryPatientBal)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Patient Balance',d.TertiaryPatientBal,i.TertiaryPatientBal,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryPatientBal,'1') != ISNULL(d.TertiaryPatientBal,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update TertiaryTransferred
BEGIN TRY
IF Update(TertiaryTransferred)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Transferred',d.TertiaryTransferred,i.TertiaryTransferred,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryTransferred,'1') != ISNULL(d.TertiaryTransferred,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update TertiaryStatus
BEGIN TRY
IF Update(TertiaryStatus)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Status',(case d.TertiaryStatus when 'N' then 'New Charge'
when 'S' then 'Submitted'
when 'K' then '999 Accepted' 
when 'D' then '999 Denied'
when 'E' then '999 has Errors'
when 'P' then 'Paid'
when 'DN' then 'Denial'
when 'PT_P' then 'Patient Paid' 
when 'PPTS' then 'Paid-Transfered To Sec'
when 'PPTT' then 'Paid-Transfered To Ter'
when 'PPTP' then 'Paid-Transfered To Patient'
when 'SPTP' then 'Paid-Transfered To Patient'
when 'SPTT' then 'Paid-Transfered To Ter' 
when 'PR_TP' then 'Pat. Resp. Transferred to Pat'
when 'PPTM' then 'Paid-Medigaped' 
when 'M' then 'Medigaped' 
when 'RS' then 'Resubmitted' 
when 'P' then 'Pending'
when 'R' then 'Rejected'
Else '' end),
(case i.TertiaryStatus when 'N' then 'New Charge' 
when 'S' then 'Submitted'
when 'K' then '999 Accepted' 
when 'D' then '999 Denied' 
when 'E' then '999 has Errors'
when 'P' then 'Paid' 
when 'DN' then 'Denial'
when 'PT_P' then 'Patient Paid'
when 'PPTS' then 'Paid-Transfered To Sec'
when 'PPTT' then 'Paid-Transfered To Ter'
when 'PPTP' then 'Paid-Transfered To Patient'
when 'SPTP' then 'Paid-Transfered To Patient' 
when 'SPTT' then 'Paid-Transfered To Ter'
when 'PR_TP' then 'Pat. Resp. Transferred to Pat' 
when 'PPTM' then 'Paid-Medigaped' 
when 'M' then 'Medigaped'
when 'RS' then 'ReSubmitted' 
when 'P' then 'Pending'
when 'R' then 'Rejected'
Else '' end),HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryStatus,'') != ISNULL(d.TertiaryStatus,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update PatientAmount
BEGIN TRY
IF Update(PatientAmount)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Patient Amount',d.PatientAmount,i.PatientAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PatientAmount,'1') != ISNULL(d.PatientAmount,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update PatientPaid
BEGIN TRY
IF Update(PatientPaid)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Patient Paid',d.PatientPaid,i.PatientPaid,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PatientPaid,'1') != ISNULL(d.PatientPaid,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update IsSubmitted
BEGIN TRY
IF Update(IsSubmitted)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Is Submitted',(case when d.IsSubmitted = 1 then 'Yes' when d.IsSubmitted =0 then 'No' end),(case when i.IsSubmitted = 1 then 'Yes' when i.IsSubmitted =0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.IsSubmitted,'2') != ISNULL(d.IsSubmitted,'2')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update SubmittetdDate
BEGIN TRY
IF Update(SubmittetdDate)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Submittetd Date',d.SubmittetdDate,i.SubmittetdDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(cast(i.SubmittetdDate as date),'') != ISNULL(cast(d.SubmittetdDate as date),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update IsDontPrint
BEGIN TRY
IF Update(IsDontPrint)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'IsDontPrint',(case when d.IsDontPrint = 1 then 'Yes' when d.IsDontPrint =0 then 'No' end),(case when i.IsDontPrint = 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.IsDontPrint,'2') != ISNULL(d.IsDontPrint,'2')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update RejectionReason
BEGIN TRY
IF Update(RejectionReason)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Rejection Reason',d.RejectionReason,i.RejectionReason,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.RejectionReason,'') != ISNULL(d.RejectionReason,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update OtherPatResp
BEGIN TRY
IF Update(OtherPatResp)
INSERT INTO ChargeAudit(ChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Other Pat Resp',d.OtherPatResp,i.OtherPatResp,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.OtherPatResp,'1') != ISNULL(d.OtherPatResp,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;
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
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM VisitAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM VisitAudit

--When User Update PageNumber
BEGIN TRY
IF Update(PageNumber)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Page Number',d.PageNumber,i.PageNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PageNumber,'') != ISNULL(d.PageNumber,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update BatchDocumentID
BEGIN TRY
IF Update(BatchDocumentID)
--select * Into #t1 from BatchDocument
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Batch DocumentID',pDel.[Description],pUpd.[Description],d.BatchDocumentID,i.BatchDocumentID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.BatchDocumentID,'1') != ISNULL(d.BatchDocumentID,'1')
Join BatchDocument as pDel on pdel.ID = ISNULL(d.BatchDocumentID,'1')
join BatchDocument as pUpd on pUpd.ID = ISNULL(i.BatchDocumentID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update Client
BEGIN TRY
IF Update(ClientID)
--select * Into #t2 from Client
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Client ID',pDel.[Name],pUpd.[Name],d.ClientID,i.ClientID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ClientID,'1') != ISNULL(d.ClientID,'1')
Join Client as pDel on pdel.ID = ISNULL(d.ClientID,'1')
join Client as pUpd on pUpd.ID = ISNULL(i.ClientID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update PracticeID
BEGIN TRY
IF Update(PracticeID)
--select * Into #t3 from Practice
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Practice ID',pDel.[Name],pUpd.[Name],d.PracticeID,i.PracticeID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PracticeID,'1') != ISNULL(d.PracticeID,'1')
Join Client as pDel on pdel.ID = ISNULL(d.PracticeID,'1')
join Client as pUpd on pUpd.ID = ISNULL(i.PracticeID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update LocationID
BEGIN TRY
IF Update(LocationID)
--select * Into #t4 from Location
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Location ID',pDel.[Name],pUpd.[Name],d.LocationID,i.LocationID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.LocationID,'1') != ISNULL(d.LocationID,'1')
Join Location as pDel on pdel.ID = ISNULL(d.LocationID,'1')
join Location as pUpd on pUpd.ID = ISNULL(i.LocationID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update POSID
BEGIN TRY
IF Update(POSID)
--select * Into #t5 from POS
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'POS ID',pDel.[Name],pUpd.[Name],d.POSID,i.POSID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.POSID,'1') != ISNULL(d.POSID,'1')
Join POS as pDel on pdel.ID = ISNULL(d.POSID,'1')
join POS as pUpd on pUpd.ID = ISNULL(i.POSID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update ProviderID
BEGIN TRY
IF Update(ProviderID)
--select * Into #t6 from Provider
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Provider ID',pDel.[Name],pUpd.[Name],d.ProviderID,i.ProviderID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ProviderID,'1') != ISNULL(d.ProviderID,'1')
Join Provider as pDel on pdel.ID = ISNULL(d.ProviderID,'1')
join Provider as pUpd on pUpd.ID = ISNULL(i.ProviderID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update RefProviderID
BEGIN TRY
IF Update(RefProviderID)
--select * Into #t7 from RefProvider
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Reference Provider ID',pDel.[Name],pUpd.[Name],d.RefProviderID,i.RefProviderID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.RefProviderID,'1') != ISNULL(d.RefProviderID,'1')
Join RefProvider as pDel on pdel.ID = ISNULL(d.RefProviderID,'1')
join RefProvider as pUpd on pUpd.ID = ISNULL(i.RefProviderID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update SupervisingProvID
BEGIN TRY
IF Update(SupervisingProvID)
--Select * Into #t8 from Provider
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Supervising Provider ID',dProv.Name,uProv.Name,d.SupervisingProvID,i.SupervisingProvID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SupervisingProvID,'1') != ISNULL(d.SupervisingProvID,'1')
Join Provider as dProv on dProv.ID = ISNULL(d.SupervisingProvID,'1') 
join Provider as uProv on  uProv.ID = ISNULL(i.SupervisingProvID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update OrderingProvID
BEGIN TRY
IF Update(OrderingProvID)
--Select * Into #t9 from Provider
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Ordering Provider ID',dProv.Name,uProv.Name,d.OrderingProvID,i.OrderingProvID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.OrderingProvID,'1') != ISNULL(d.OrderingProvID,'1')
Join Provider as dProv on dProv.ID = ISNULL(d.OrderingProvID,'1')
join Provider as uProv on  uProv.ID = ISNULL(i.OrderingProvID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update PatientID
BEGIN TRY
IF Update(PatientID)
--Select * Into #t10 from Patient
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Patient ID',dPat.LastName + ', ' + dPat.FirstName,uPat.LastName + ', ' + uPat.FirstName,d.PatientID,i.PatientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PatientID,'1') != ISNULL(d.PatientID,'1')
Join Patient as dPat on dPat.ID = ISNULL(d.PatientID,'1')
join Patient as uPat on  uPat.ID = ISNULL(i.PatientID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update PrimaryPatientPlanID
BEGIN TRY
IF Update(PrimaryPatientPlanID)
--Select * Into #t11 from PatientPlan
--Select * Into #t12 from InsurancePlan 
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Patient Plan ID',dInsPlan.PlanName,uInsPlan.PlanName,d.PrimaryPatientPlanID,i.PrimaryPatientPlanID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryPatientPlanID,'1') != ISNULL(d.PrimaryPatientPlanID,'1')
join PatientPlan as dpatPlan on ISNULL(dpatPlan.ID,'') = ISNULL(d.PrimaryPatientPlanID,'1')
join InsurancePlan as dInsPlan on ISNULL(dpatPlan.InsurancePlanID,'1') = ISNULL(dInsPlan.ID,'')

left join PatientPlan as upatPlan on ISNULL(upatPlan.ID,'') = ISNULL(i.PrimaryPatientPlanID,'1')
left join InsurancePlan as uInsPlan on ISNULL(upatPlan.InsurancePlanID,'1') = ISNULL(uInsPlan.ID,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update SecondaryPatientPlanID
BEGIN TRY
IF Update(SecondaryPatientPlanID)
--Select * Into #t13 from PatientPlan
--Select * Into #t14 from InsurancePlan 
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Patient Plan ID',dInsPlan.PlanName,uInsPlan.PlanName,d.SecondaryPatientPlanID,i.SecondaryPatientPlanID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryPatientPlanID,'1') != ISNULL(d.SecondaryPatientPlanID,'1')

join PatientPlan as dpatPlan on ISNULL(dpatPlan.ID,'') = ISNULL(d.SecondaryPatientPlanID,'1')
join InsurancePlan as dInsPlan on ISNULL(dpatPlan.InsurancePlanID,'1') = ISNULL(dInsPlan.ID,'')

left join PatientPlan as upatPlan on ISNULL(upatPlan.ID,'') = ISNULL(i.SecondaryPatientPlanID,'1')
left join InsurancePlan as uInsPlan on ISNULL(upatPlan.InsurancePlanID,'1') = ISNULL(uInsPlan.ID,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update TertiaryPatientPlanID
BEGIN TRY
IF Update(TertiaryPatientPlanID)
--Select * Into #t15 from PatientPlan
--Select * Into #t16 from InsurancePlan 
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Patient Plan ID',dInsPlan.PlanName,uInsPlan.PlanName,d.TertiaryPatientPlanID,i.TertiaryPatientPlanID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryPatientPlanID,'1') != ISNULL(d.TertiaryPatientPlanID,'1')


join PatientPlan as dpatPlan on ISNULL(dpatPlan.ID,'') = ISNULL(d.TertiaryPatientPlanID,'1')
join InsurancePlan as dInsPlan on ISNULL(dpatPlan.InsurancePlanID,'1') = ISNULL(dInsPlan.ID,'')

left join PatientPlan as upatPlan on ISNULL(upatPlan.ID,'') = ISNULL(i.TertiaryPatientPlanID,'1')
left join InsurancePlan as uInsPlan on ISNULL(upatPlan.InsurancePlanID,'1') = ISNULL(uInsPlan.ID,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update SubmissionLogID
BEGIN TRY
IF Update(SubmissionLogID)
--Select * Into #t17 from SubmissionLog  
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'SubmissionLog ID',dSLog.ID,uSLog.ID,d.SubmissionLogID,i.SubmissionLogID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SubmissionLogID,'1') != ISNULL(d.SubmissionLogID,'1')
Join SubmissionLog as dSLog on dSLog.ID = ISNULL(d.SubmissionLogID,'1')
join SubmissionLog as uSLog on  uSLog.ID = ISNULL(i.SubmissionLogID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update RejectionReason
BEGIN TRY
IF Update(RejectionReason)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Rejection Reason',d.RejectionReason,i.RejectionReason,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.RejectionReason,'') != ISNULL(d.RejectionReason,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update ICD1ID
BEGIN TRY
IF Update(ICD1ID)
--select * Into #t18 from ICD
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'ICD ID',pDel.[Description],pUpd.[Description],d.ICD1ID,i.ICD1ID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ICD1ID,'1') != ISNULL(d.ICD1ID,'1')
Join ICD as pDel on pdel.ID = ISNULL(d.ICD1ID,'1')
join ICD as pUpd on pUpd.ID = ISNULL(i.ICD1ID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update ICD2ID
BEGIN TRY
IF Update(ICD2ID)
--select * Into #t19 from ICD
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'ICD2 ID',pDel.[Description],pUpd.[Description],d.ICD2ID,i.ICD2ID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ICD2ID,'1') != ISNULL(d.ICD2ID,'1')
Join ICD as pDel on pdel.ID = ISNULL(d.ICD2ID,'1')
join ICD as pUpd on pUpd.ID = ISNULL(i.ICD2ID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update ICD3ID
BEGIN TRY
IF Update(ICD3ID)
--select * Into #t20 from ICD
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'ICD3 ID',pDel.[Description],pUpd.[Description],d.ICD3ID,i.ICD3ID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ICD3ID,'1') != ISNULL(d.ICD3ID,'1')
Join ICD as pDel on pdel.ID = ISNULL(d.ICD3ID,'1')
join ICD as pUpd on pUpd.ID = ISNULL(i.ICD3ID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update ICD4ID
BEGIN TRY
IF Update(ICD4ID)
--select * Into #t21 from ICD
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'ICD4 ID',pDel.[Description],pUpd.[Description],d.ICD4ID,i.ICD4ID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ICD4ID,'1') != ISNULL(d.ICD4ID,'1')
Join ICD as pDel on pdel.ID = ISNULL(d.ICD4ID,'1')
join ICD as pUpd on pUpd.ID = ISNULL(i.ICD4ID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update ICD5ID
BEGIN TRY
IF Update(ICD5ID)
--select * Into #t22 from ICD
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'ICD5 ID',pDel.[Description],pUpd.[Description],d.ICD5ID,i.ICD5ID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ICD5ID,'1') != ISNULL(d.ICD5ID,'1')
Join ICD as pDel on pdel.ID = ISNULL(d.ICD5ID,'1')
join ICD as pUpd on pUpd.ID = ISNULL(i.ICD5ID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update ICD6ID
BEGIN TRY
IF Update(ICD6ID)
--select * Into #t23 from ICD
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'ICD6 ID',pDel.[Description],pUpd.[Description],d.ICD6ID,i.ICD6ID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ICD6ID,'1') != ISNULL(d.ICD6ID,'1')
Join ICD as pDel on pdel.ID = ISNULL(d.ICD6ID,'1')
join ICD as pUpd on pUpd.ID = ISNULL(i.ICD6ID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update ICD7ID
BEGIN TRY
IF Update(ICD7ID)
--select * Into #t24 from ICD
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'ICD7 ID',pDel.[Description],pUpd.[Description],d.ICD7ID,i.ICD7ID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ICD7ID,'1') != ISNULL(d.ICD7ID,'1')
Join ICD as pDel on pdel.ID = ISNULL(d.ICD7ID,'1')
join ICD as pUpd on pUpd.ID = ISNULL(i.ICD7ID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update ICD8ID
BEGIN TRY
IF Update(ICD8ID)
--select * Into #t25 from ICD
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'ICD8 ID',pDel.[Description],pUpd.[Description],d.ICD8ID,i.ICD8ID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ICD8ID,'1') != ISNULL(d.ICD8ID,'1')
Join ICD as pDel on pdel.ID = ISNULL(d.ICD8ID,'1')
join ICD as pUpd on pUpd.ID = ISNULL(i.ICD8ID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update ICD9ID
BEGIN TRY
IF Update(ICD9ID)
--select * Into #t26 from ICD
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'ICD9 ID',pDel.[Description],pUpd.[Description],d.ICD9ID,i.ICD9ID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ICD9ID,'1') != ISNULL(d.ICD9ID,'1')
Join ICD as pDel on pdel.ID = ISNULL(d.ICD9ID,'1')
join ICD as pUpd on pUpd.ID = ISNULL(i.ICD9ID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update ICD10ID
BEGIN TRY
IF Update(ICD10ID)
--select * Into #t27 from ICD
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'ICD10 ID',pDel.[Description],pUpd.[Description],d.ICD10ID,i.ICD10ID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ICD10ID,'1') != ISNULL(d.ICD10ID,'1')
Join ICD as pDel on pdel.ID = ISNULL(d.ICD10ID,'1')
join ICD as pUpd on pUpd.ID = ISNULL(i.ICD10ID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update ICD11ID
BEGIN TRY
IF Update(ICD11ID)
--select * Into #t28 from ICD
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'ICD11 ID',pDel.[Description],pUpd.[Description],d.ICD11ID,i.ICD11ID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ICD11ID,'1') != ISNULL(d.ICD11ID,'1')
Join ICD as pDel on pdel.ID = ISNULL(d.ICD11ID,'1')
join ICD as pUpd on pUpd.ID = ISNULL(i.ICD11ID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update ICD12ID
BEGIN TRY
IF Update(ICD12ID)
--select * Into #t29 from ICD
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'ICD12 ID',pDel.[Description],pUpd.[Description],d.ICD12ID,i.ICD12ID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ICD12ID,'1') != ISNULL(d.ICD12ID,'1')
Join ICD as pDel on pdel.ID = ISNULL(d.ICD12ID,'1')
join ICD as pUpd on pUpd.ID = ISNULL(i.ICD12ID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update AuthorizationNum
BEGIN TRY
IF Update(AuthorizationNum)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Authorization Number',d.AuthorizationNum,i.AuthorizationNum,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.AuthorizationNum,'') != ISNULL(d.AuthorizationNum,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update OutsideReferral
BEGIN TRY
IF Update(OutsideReferral)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Outside Referral',(case when d.OutsideReferral = 1 then 'Yes' when d.OutsideReferral =0 then 'No' end),(case when i.OutsideReferral = 1 then 'Yes' when i.OutsideReferral =0 then 'No' end),HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.OutsideReferral,'2') != ISNULL(d.OutsideReferral,'2')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update IsForcePaper
BEGIN TRY
IF Update(IsForcePaper)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Is Force Paper',(case when d.IsForcePaper = 1 then 'Yes' when d.IsForcePaper =0 then 'No' end),(case when i.IsForcePaper = 1 then 'Yes' when i.IsForcePaper =0 then 'No' end),HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.IsForcePaper,'2') != ISNULL(d.IsForcePaper,'2')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update IsDontPrint
BEGIN TRY
IF Update(IsDontPrint)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Is Dont Print',(case when d.IsDontPrint = 1 then 'Yes' when d.IsDontPrint =0 then 'No' end),(case when i.IsDontPrint = 1 then 'Yes' when i.IsDontPrint =0 then 'No' end),HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.IsDontPrint,'2') != ISNULL(d.IsDontPrint,'2')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update ReferralNum
BEGIN TRY
IF Update(ReferralNum)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Referral Number',d.ReferralNum,i.ReferralNum,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ReferralNum,'') != ISNULL(d.ReferralNum,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update OnsetDateOfIllness
BEGIN TRY
IF Update(OnsetDateOfIllness)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'On set Date Of Illness',d.OnsetDateOfIllness,i.OnsetDateOfIllness,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(cast(i.OnsetDateOfIllness as date),'') != ISNULL(cast(d.OnsetDateOfIllness as date),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update FirstDateOfSimiliarIllness
BEGIN TRY
IF Update(FirstDateOfSimiliarIllness)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'First Date Of Similiar Illness',d.FirstDateOfSimiliarIllness,i.FirstDateOfSimiliarIllness,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(cast(i.FirstDateOfSimiliarIllness as date),'') != ISNULL(cast(d.FirstDateOfSimiliarIllness as date),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update IllnessTreatmentDate
BEGIN TRY
IF Update(IllnessTreatmentDate)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Illness Treatment Date',d.IllnessTreatmentDate,i.IllnessTreatmentDate,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(cast(i.IllnessTreatmentDate as date),'') != ISNULL(cast(d.IllnessTreatmentDate as date),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update DateOfPregnancy
BEGIN TRY
IF Update(DateOfPregnancy)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Date Of Pregnancy',d.DateOfPregnancy,i.DateOfPregnancy,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(cast(i.DateOfPregnancy as date),'') != ISNULL(cast(d.DateOfPregnancy as date),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update AdmissionDate
BEGIN TRY
IF Update(AdmissionDate)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Admission Date',d.AdmissionDate,i.AdmissionDate,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(cast(i.AdmissionDate as date),'') != ISNULL(Cast(d.AdmissionDate as date),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update DischargeDate
BEGIN TRY
IF Update(DischargeDate)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Discharge Date',d.DischargeDate,i.DischargeDate,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(Cast(i.DischargeDate as date),'') != ISNULL(cast(d.DischargeDate as date),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update LastXrayDate
BEGIN TRY
IF Update(LastXrayDate)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Last Xray Date',d.LastXrayDate,i.LastXrayDate,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(cast(i.LastXrayDate as date),'') != ISNULL(cast(d.LastXrayDate as date),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update LastXrayType
BEGIN TRY
IF Update(LastXrayType)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Last Xray Type',d.LastXrayType,i.LastXrayType,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.LastXrayType,'') != ISNULL(d.LastXrayType,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update UnableToWorkFromDate
BEGIN TRY
IF Update(UnableToWorkFromDate)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Unable To Work From Date',d.UnableToWorkFromDate,i.UnableToWorkFromDate,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(Cast(i.UnableToWorkFromDate as date),'') != ISNULL(cast(d.UnableToWorkFromDate as date),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update UnableToWorkToDate
BEGIN TRY
IF Update(UnableToWorkToDate)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Unable To Work To Date',d.UnableToWorkToDate,i.UnableToWorkToDate,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(cast(i.UnableToWorkToDate as date),'') != ISNULL(cast(d.UnableToWorkToDate as date),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update AccidentDate
BEGIN TRY
IF Update(AccidentDate)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Accident Date',d.AccidentDate,i.AccidentDate,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(cast(i.AccidentDate as date),'') != ISNULL(cast(d.AccidentDate as date),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update AccidentState
BEGIN TRY
IF Update(AccidentState)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Accident State',d.AccidentState,i.AccidentState,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.AccidentState,'') != ISNULL(d.AccidentState,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update AccidentType
BEGIN TRY
IF Update(AccidentType)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Accident Type',d.AccidentType,i.AccidentType,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.AccidentType,'') != ISNULL(d.AccidentType,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update CliaNumber
BEGIN TRY
IF Update(CliaNumber)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'CLIA Number',d.CliaNumber,i.CliaNumber,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.CliaNumber,'') != ISNULL(d.CliaNumber,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update OutsideLab
BEGIN TRY
IF Update(OutsideLab)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Outside Laboratory',(case when d.OutsideLab = 1 then 'Yes' when d.OutsideLab =0 then 'No' end),(case when i.OutsideLab = 1 then 'Yes' when i.OutsideLab =0 then 'No' end),HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.OutsideLab,'2') != ISNULL(d.OutsideLab,'2')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update LabCharges
BEGIN TRY
IF Update(LabCharges)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Laboratory Charges',d.LabCharges,i.LabCharges,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.LabCharges,'1') != ISNULL(d.LabCharges,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update ClaimNotes
BEGIN TRY
IF Update(ClaimNotes)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Claim Notes',d.ClaimNotes,i.ClaimNotes,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ClaimNotes,'') != ISNULL(d.ClaimNotes,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update PayerClaimControlNum
BEGIN TRY
IF Update(PayerClaimControlNum)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payer Claim Control Number',d.PayerClaimControlNum,i.PayerClaimControlNum,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PayerClaimControlNum,'') != ISNULL(d.PayerClaimControlNum,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update ClaimFrequencyCode
BEGIN TRY
IF Update(ClaimFrequencyCode)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Claim Frequency Code',d.ClaimFrequencyCode,i.ClaimFrequencyCode,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ClaimFrequencyCode,'') != ISNULL(d.ClaimFrequencyCode,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update ServiceAuthExcpCode
BEGIN TRY
IF Update(ServiceAuthExcpCode)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Service Author ExcpCode',d.ServiceAuthExcpCode,i.ServiceAuthExcpCode,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ServiceAuthExcpCode,'') != ISNULL(d.ServiceAuthExcpCode,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update ValidationMessage
BEGIN TRY
IF Update(ValidationMessage)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Validation Message',d.ValidationMessage,i.ValidationMessage,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ValidationMessage,'') != ISNULL(d.ValidationMessage,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update Emergency
BEGIN TRY
IF Update(Emergency)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Emergency',(case when d.[Emergency] = 1 then 'Yes' when d.[Emergency] =0 then 'No' end),(case when i.[Emergency] = 1 then 'Yes' when i.[Emergency]=0 then 'No' end),HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Emergency,'2') != ISNULL(d.Emergency,'2')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update EPSDT
BEGIN TRY
IF Update(EPSDT)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'EPSDT',(case when d.EPSDT = 1 then 'Yes' when d.EPSDT =0 then 'No' end),(case when i.EPSDT = 1 then 'Yes' when i.EPSDT =0 then 'No' end),HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.EPSDT,'2') != ISNULL(d.EPSDT,'2')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update FamilyPlan
BEGIN TRY
IF Update(FamilyPlan)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Family Plan',(case when d.FamilyPlan = 1 then 'Yes' when d.FamilyPlan=0 then 'No' end),(case when i.FamilyPlan = 1 then 'Yes' when i.FamilyPlan =0 then 'No' end),HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.FamilyPlan,'2') != ISNULL(d.FamilyPlan,'2')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update TotalAmount
BEGIN TRY
IF Update(TotalAmount)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Total Amount',d.TotalAmount,i.TotalAmount,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TotalAmount,'1') != ISNULL(d.TotalAmount,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update Copay
BEGIN TRY
IF Update(Copay)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Copay',d.Copay,i.Copay,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Copay,'1') != ISNULL(d.Copay,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update Coinsurance
BEGIN TRY
IF Update(Coinsurance)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Coinsurance',d.Coinsurance,i.Coinsurance,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Coinsurance,'1') != ISNULL(d.Coinsurance,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update Deductible
BEGIN TRY
IF Update(Deductible)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Deductible',d.Deductible,i.Deductible,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Deductible,'1') != ISNULL(d.Deductible,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update PrimaryStatus
BEGIN TRY
IF Update(PrimaryStatus)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Status',
(case d.PrimaryStatus when 'N' then 'New Charge'
when 'S' then 'Submitted'
when 'K' then '999 Accepted' 
when 'D' then '999 Denied'
when 'E' then '999 has Errors'
when 'P' then 'Paid'
when 'DN' then 'Denial'
when 'PT_P' then 'Patient Paid' 
when 'PPTS' then 'Paid-Transfered To Sec'
when 'PPTT' then 'Paid-Transfered To Ter'
when 'PPTP' then 'Paid-Transfered To Patient'
when 'SPTP' then 'Paid-Transfered To Patient'
when 'SPTT' then 'Paid-Transfered To Ter' 
when 'PR_TP' then 'Pat. Resp. Transferred to Pat'
when 'PPTM' then 'Paid-Medigaped' 
when 'M' then 'Medigaped' 
when 'RS' then 'Resubmitted' 
when 'P' then 'Pending'
when 'R' then 'Rejected'
Else '' end),
(case i.PrimaryStatus when 'N' then 'New Charge' 
when 'S' then 'Submitted'
when 'K' then '999 Accepted' 
when 'D' then '999 Denied' 
when 'E' then '999 has Errors'
when 'P' then 'Paid' 
when 'DN' then 'Denial'
when 'PT_P' then 'Patient Paid'
when 'PPTS' then 'Paid-Transfered To Sec'
when 'PPTT' then 'Paid-Transfered To Ter'
when 'PPTP' then 'Paid-Transfered To Patient'
when 'SPTP' then 'Paid-Transfered To Patient' 
when 'SPTT' then 'Paid-Transfered To Ter'
when 'PR_TP' then 'Pat. Resp. Transferred to Pat' 
when 'PPTM' then 'Paid-Medigaped' 
when 'M' then 'Medigaped'
when 'RS' then 'ReSubmitted' 
when 'P' then 'Pending'
when 'R' then 'Rejected'
Else '' end),HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryStatus,'') !=ISNULL( d.PrimaryStatus,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update PrimaryBilledAmount
BEGIN TRY
IF Update(PrimaryBilledAmount)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Billed Amount',d.PrimaryBilledAmount,i.PrimaryBilledAmount,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryBilledAmount,'1') != ISNULL(d.PrimaryBilledAmount,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update PrimaryAllowed
BEGIN TRY
IF Update(PrimaryAllowed)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Allowed',d.PrimaryAllowed,i.PrimaryAllowed,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryAllowed,'1') != ISNULL(d.PrimaryAllowed,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update PrimaryWriteOff
BEGIN TRY
IF Update(PrimaryWriteOff)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Write Off',d.PrimaryWriteOff,i.PrimaryWriteOff,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryWriteOff,'1') != ISNULL(d.PrimaryWriteOff,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update PrimaryPaid
BEGIN TRY
IF Update(PrimaryPaid)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Paid',d.PrimaryPaid,i.PrimaryPaid,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryPaid,'1') != ISNULL(d.PrimaryPaid,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update PrimaryBal
BEGIN TRY
IF Update(PrimaryBal)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Balance',d.PrimaryBal,i.PrimaryBal,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryBal,'1') != ISNULL(d.PrimaryBal,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update PrimaryPatientBal
BEGIN TRY
IF Update(PrimaryPatientBal)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Patient Balance',d.PrimaryPatientBal,i.PrimaryPatientBal,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryPatientBal,'1') != ISNULL(d.PrimaryPatientBal,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update PrimaryTransferred
BEGIN TRY
IF Update(PrimaryTransferred)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Primary Transferred',d.PrimaryTransferred,i.PrimaryTransferred,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PrimaryTransferred,'1') != ISNULL(d.PrimaryTransferred,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update SecondaryStatus
BEGIN TRY
IF Update(SecondaryStatus)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Status',(case d.SecondaryStatus when 'N' then 'New Charge'
when 'S' then 'Submitted'
when 'K' then '999 Accepted' 
when 'D' then '999 Denied'
when 'E' then '999 has Errors'
when 'P' then 'Paid'
when 'DN' then 'Denial'
when 'PT_P' then 'Patient Paid' 
when 'PPTS' then 'Paid-Transfered To Sec'
when 'PPTT' then 'Paid-Transfered To Ter'
when 'PPTP' then 'Paid-Transfered To Patient'
when 'SPTP' then 'Paid-Transfered To Patient'
when 'SPTT' then 'Paid-Transfered To Ter' 
when 'PR_TP' then 'Pat. Resp. Transferred to Pat'
when 'PPTM' then 'Paid-Medigaped' 
when 'M' then 'Medigaped' 
when 'RS' then 'Resubmitted'
when 'P' then 'Pending'
when 'R' then 'Rejected'
Else '' end),
(case i.SecondaryStatus when 'N' then 'New Charge' 
when 'S' then 'Submitted'
when 'K' then '999 Accepted' 
when 'D' then '999 Denied' 
when 'E' then '999 has Errors'
when 'P' then 'Paid' 
when 'DN' then 'Denial'
when 'PT_P' then 'Patient Paid'
when 'PPTS' then 'Paid-Transfered To Sec'
when 'PPTT' then 'Paid-Transfered To Ter'
when 'PPTP' then 'Paid-Transfered To Patient'
when 'SPTP' then 'Paid-Transfered To Patient' 
when 'SPTT' then 'Paid-Transfered To Ter'
when 'PR_TP' then 'Pat. Resp. Transferred to Pat' 
when 'PPTM' then 'Paid-Medigaped' 
when 'M' then 'Medigaped'
when 'RS' then 'ReSubmitted' 
when 'P' then 'Pending'
when 'R' then 'Rejected'
Else '' end),HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryStatus,'') != ISNULL(d.SecondaryStatus,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update SecondaryBilledAmount
BEGIN TRY 
IF Update(SecondaryBilledAmount)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Billed Amount',d.SecondaryBilledAmount,i.SecondaryBilledAmount,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryBilledAmount,'1') != ISNULL(d.SecondaryBilledAmount,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update SecondaryAllowed
BEGIN TRY
IF Update(SecondaryAllowed)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Allowed',d.SecondaryAllowed,i.SecondaryAllowed,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryAllowed,'1') != ISNULL(d.SecondaryAllowed,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update SecondaryWriteOff
BEGIN TRY
IF Update(SecondaryWriteOff)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Write Off',d.SecondaryWriteOff,i.SecondaryWriteOff,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryWriteOff,'1') != ISNULL(d.SecondaryWriteOff,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update SecondaryPaid
BEGIN TRY
IF Update(SecondaryPaid)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Paid',d.SecondaryPaid,i.SecondaryPaid,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryPaid,'1') != ISNULL(d.SecondaryPaid,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update SecondaryPatResp
BEGIN TRY
IF Update(SecondaryPatResp)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Patient Response',d.SecondaryPatResp,i.SecondaryPatResp,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryPatResp,'1') != ISNULL(d.SecondaryPatResp,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update SecondaryBal
BEGIN TRY
IF Update(SecondaryBal)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Balance',d.SecondaryBal,i.SecondaryBal,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryBal,'1') != ISNULL(d.SecondaryBal,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update SecondaryPatientBal
BEGIN TRY
IF Update(SecondaryPatientBal)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Patient Balance',d.SecondaryPatientBal,i.SecondaryPatientBal,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryPatientBal,'1') != ISNULL(d.SecondaryPatientBal,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update SecondaryTransferred
BEGIN TRY
IF Update(SecondaryTransferred)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Secondary Transferred',d.SecondaryTransferred,i.SecondaryTransferred,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecondaryTransferred,'1') != ISNULL(d.SecondaryTransferred,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update TertiaryStatus
BEGIN TRY
IF Update(TertiaryStatus)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Status',(case d.TertiaryStatus when 'N' then 'New Charge'
when 'S' then 'Submitted'
when 'K' then '999 Accepted' 
when 'D' then '999 Denied'
when 'E' then '999 has Errors'
when 'P' then 'Paid'
when 'DN' then 'Denial'
when 'PT_P' then 'Patient Paid' 
when 'PPTS' then 'Paid-Transfered To Sec'
when 'PPTT' then 'Paid-Transfered To Ter'
when 'PPTP' then 'Paid-Transfered To Patient'
when 'SPTP' then 'Paid-Transfered To Patient'
when 'SPTT' then 'Paid-Transfered To Ter' 
when 'PR_TP' then 'Pat. Resp. Transferred to Pat'
when 'PPTM' then 'Paid-Medigaped' 
when 'M' then 'Medigaped' 
when 'RS' then 'Resubmitted' 
when 'P' then 'Pending'
when 'R' then 'Rejected'
Else '' end),
(case i.TertiaryStatus when 'N' then 'New Charge' 
when 'S' then 'Submitted'
when 'K' then '999 Accepted' 
when 'D' then '999 Denied' 
when 'E' then '999 has Errors'
when 'P' then 'Paid' 
when 'DN' then 'Denial'
when 'PT_P' then 'Patient Paid'
when 'PPTS' then 'Paid-Transfered To Sec'
when 'PPTT' then 'Paid-Transfered To Ter'
when 'PPTP' then 'Paid-Transfered To Patient'
when 'SPTP' then 'Paid-Transfered To Patient' 
when 'SPTT' then 'Paid-Transfered To Ter'
when 'PR_TP' then 'Pat. Resp. Transferred to Pat' 
when 'PPTM' then 'Paid-Medigaped' 
when 'M' then 'Medigaped'
when 'RS' then 'ReSubmitted' 
when 'P' then 'Pending'
when 'R' then 'Rejected'
Else '' end),HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryStatus,'') != ISNULL(d.TertiaryStatus,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update TertiaryBilledAmount
BEGIN TRY
IF Update(TertiaryBilledAmount)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Billed Amount',d.TertiaryBilledAmount,i.TertiaryBilledAmount,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryBilledAmount,'1') != ISNULL(d.TertiaryBilledAmount,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update TertiaryAllowed
BEGIN TRY
IF Update(TertiaryAllowed)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Allowed',d.TertiaryAllowed,i.TertiaryAllowed,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryAllowed,'1') != ISNULL(d.TertiaryAllowed,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update TertiaryWriteOff
BEGIN TRY
IF Update(TertiaryWriteOff)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Write Off',d.TertiaryWriteOff,i.TertiaryWriteOff,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryWriteOff,'1') != ISNULL(d.TertiaryWriteOff,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update TertiaryPaid
BEGIN TRY
IF Update(TertiaryPaid)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Paid',d.TertiaryPaid,i.TertiaryPaid,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryPaid,'1') != ISNULL(d.TertiaryPaid,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update TertiaryPatResp
BEGIN TRY
IF Update(TertiaryPatResp)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Patient Response',d.TertiaryPatResp,i.TertiaryPatResp,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryPatResp,'1') != ISNULL(d.TertiaryPatResp,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update TertiaryBal
BEGIN TRY
IF Update(TertiaryBal)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Balance',d.TertiaryBal,i.TertiaryBal,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryBal,'1') != ISNULL(d.TertiaryBal,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update TertiaryPatientBal
BEGIN TRY
IF Update(TertiaryPatientBal)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Patient Balance',d.TertiaryPatientBal,i.TertiaryPatientBal,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryPatientBal,'1') != ISNULL(d.TertiaryPatientBal,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update TertiaryTransferred
BEGIN TRY

IF Update(TertiaryTransferred)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Tertiary Transferred',d.TertiaryTransferred,i.TertiaryTransferred,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TertiaryTransferred,'1') != ISNULL(d.TertiaryTransferred,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update PatientAmount
BEGIN TRY
IF Update(PatientAmount)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Patient Amount',d.PatientAmount,i.PatientAmount,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PatientAmount,'1') != ISNULL(d.PatientAmount,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update PatientPaid
BEGIN TRY
IF Update(PatientPaid)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Patient Paid',d.PatientPaid,i.PatientPaid,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PatientPaid,'1') != ISNULL(d.PatientPaid,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update IsSubmitted
BEGIN TRY
IF Update(IsSubmitted)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Submitted',(case when d.IsSubmitted = 1 then 'Yes' when d.IsSubmitted =0 then 'No' end),(case when i.IsSubmitted = 1 then 'Yes' when i.IsSubmitted =0 then 'No' end),HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and  ISNULL(i.IsSubmitted,'2') != ISNULL(d.IsSubmitted,'2') 
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update SubmittetdDate
BEGIN TRY
IF Update(SubmittedDate)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Submitted Date',d.SubmittedDate,i.SubmittedDate,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(cast(i.SubmittedDate as date),'') != ISNULL(cast(d.SubmittedDate as date),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update DateOfServiceFrom
BEGIN TRY
IF Update(DateOfServiceFrom)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Date Of Service From',d.DateOfServiceFrom,i.DateOfServiceFrom,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(cast(i.DateOfServiceFrom as date),'') != ISNULL(cast(d.DateOfServiceFrom as date),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update DateOfServiceTo
BEGIN TRY
IF Update(DateOfServiceTo)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Date Of Service To',d.DateOfServiceTo,i.DateOfServiceTo,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(cast(i.DateOfServiceTo as date),'') != ISNULL(cast(d.DateOfServiceTo as date),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update CopayPaid
BEGIN TRY
IF Update(CopayPaid)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Copay Paid',d.CopayPaid,i.CopayPaid,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.CopayPaid,'1') != ISNULL(d.CopayPaid,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--When User Update OtherPatResp
BEGIN TRY
IF Update(OtherPatResp)
INSERT INTO VisitAudit(VisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Other Pat Resp',d.OtherPatResp,i.OtherPatResp,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.OtherPatResp,'1') != ISNULL(d.OtherPatResp,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;
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
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM StatusCodeAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM StatusCodeAudit  

--when update Description
BEGIN TRY
if update(Description)
insert into StatusCodeAudit(StatusCodeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Description',(RTRIM(LTRIM(d.Description))),(RTRIM(LTRIM(i.Description))),HOST_NAME(),i.UpdateBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Description)),'') != ISNULL(RTRIM(LTRIM(d.Description)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

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
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM SubmissionLogAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM SubmissionLogAudit  

--when update ClientID
BEGIN TRY
if update(ClientID)
--Select * into #t from Client
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Client ID',d.ClientID,i.ClientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClientID,'') != ISNULL(d.ClientID,'')
--Join Client as cDel on cDel.ID = ISNULL(d.ClientID,'')
--join Client as cUpd on cUpd.ID = ISNULL(i.ClientID,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ReceiverID
BEGIN TRY
if update(ReceiverID)
--Select * into #t1 from Receiver
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Receiver ID',d.ReceiverID,i.ReceiverID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReceiverID,'') != ISNULL(d.ReceiverID,'')
--Join Receiver as cDel on cDel.ID = ISNULL(d.ReceiverID,'')
--join Receiver as cUpd on cUpd.ID = ISNULL(i.ReceiverID,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--when update SubmitterID
BEGIN TRY
if update(SubmitterID)
--Select * into #t2 from Submitter
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submitter ID',d.SubmitterID,i.SubmitterID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmitterID,'') != ISNULL(d.SubmitterID,'')
--Join Submitter as cDel on cDel.ID = ISNULL(d.SubmitterID,'')
--join Submitter as cUpd on cUpd.ID = ISNULL(i.SubmitterID,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--when update SubmitType
BEGIN TRY
if update(SubmitType)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submit Type',d.SubmitType,i.SubmitType,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmitType,'') != ISNULL(d.SubmitType,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--when update DownloadedFileID
BEGIN TRY
if update(DownloadedFileID)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Downloaded FileID',d.DownloadedFileID,i.DownloadedFileID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DownloadedFileID,'') != ISNULL(d.DownloadedFileID,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--when update PdfPath
BEGIN TRY
if update(PdfPath)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Pdf Path',d.PdfPath,i.PdfPath,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PdfPath,'') != ISNULL(d.PdfPath,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--when update FormType
BEGIN TRY
if update(FormType)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Form Type',d.FormType,i.FormType,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FormType,'') != ISNULL(d.FormType,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--when update ISAControlNumber
BEGIN TRY
if update(ISAControlNumber)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'ISA Control Number',d.ISAControlNumber,i.ISAControlNumber,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ISAControlNumber,'') != ISNULL(d.ISAControlNumber,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--when update ClaimCount
BEGIN TRY
if update(ClaimCount)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Claim Count',d.ClaimCount,i.ClaimCount,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClaimCount,'') != ISNULL(d.ClaimCount,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ClaimAmount
BEGIN TRY
if update(ClaimAmount)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Claim Amount',d.ClaimAmount,i.ClaimAmount,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClaimAmount,'1') != ISNULL(d.ClaimAmount,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update Notes
BEGIN TRY
if update(Notes)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',(RTRIM(LTRIM(d.Notes))),(RTRIM(LTRIM(i.Notes))),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Notes)),'') != ISNULL(RTRIM(LTRIM(d.Notes)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update Transaction837Path
BEGIN TRY
if update(Transaction837Path)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Transaction837 Path',d.Transaction837Path,i.Transaction837Path,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Transaction837Path,'') != ISNULL(d.Transaction837Path,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update IK5_Status
BEGIN TRY
if update(IK5_Status)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IK5_Status',d.IK5_Status,i.IK5_Status,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IK5_Status,'') != ISNULL(d.IK5_Status,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update AK9_Status
BEGIN TRY
if update(AK9_Status)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'AK9_Status',d.AK9_Status,i.AK9_Status,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AK9_Status,'') != ISNULL(d.AK9_Status,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update NoOfTotalST
BEGIN TRY
if update(NoOfTotalST)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'No Of Total ST',d.NoOfTotalST,i.NoOfTotalST,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.NoOfTotalST,'') != ISNULL(d.NoOfTotalST,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update NoOfReceivedST
BEGIN TRY
if update(NoOfReceivedST)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Number Of Received ST',d.NoOfReceivedST,i.NoOfReceivedST,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.NoOfReceivedST,'') != ISNULL(d.NoOfReceivedST,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update NoOfAcceptedST
BEGIN TRY
if update(NoOfAcceptedST)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'No Of Accepted ST',d.NoOfAcceptedST,i.NoOfAcceptedST,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.NoOfAcceptedST,'') != ISNULL(d.NoOfAcceptedST,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update IK5_ErrorCode
BEGIN TRY
if update(IK5_ErrorCode)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IK5_Error Code',d.IK5_ErrorCode,i.IK5_ErrorCode,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IK5_ErrorCode,'') != ISNULL(d.IK5_ErrorCode,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update AK9_ErrorCode
BEGIN TRY
if update(AK9_ErrorCode)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'AK9_Error Code',d.AK9_ErrorCode,i.AK9_ErrorCode,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AK9_ErrorCode,'') != ISNULL(d.AK9_ErrorCode,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update Transaction999Path
BEGIN TRY
if update(Transaction999Path)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Transaction999 Path',d.Transaction999Path,i.Transaction999Path,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Transaction999Path,'') != ISNULL(d.Transaction999Path,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update Trasaction277CAPath
BEGIN TRY
if update(Trasaction277CAPath)
insert into SubmissionLogAudit(SubmissionLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Trasaction277 CAPath',d.Trasaction277CAPath,i.Trasaction277CAPath,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Trasaction277CAPath,'') != ISNULL(d.Trasaction277CAPath,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;
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
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM SubmitterAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM SubmitterAudit  

--when update Name
BEGIN TRY
if update(Name)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',(RTRIM(LTRIM(d.Name))),(RTRIM(LTRIM(i.Name))),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Name)),'') != ISNULL(RTRIM(LTRIM(d.Name)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 


--when update Address
BEGIN TRY
if update(Address)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address',(RTRIM(LTRIM(d.Address))),(RTRIM(LTRIM(i.Address))),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Address)),'') != ISNULL(RTRIM(LTRIM(d.Address)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Address2
BEGIN TRY
if update(Address2)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address2',(RTRIM(LTRIM(d.Address2))),(RTRIM(LTRIM(i.Address2))),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Address2)),'') != ISNULL(RTRIM(LTRIM(d.Address2)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 


--when update City
BEGIN TRY
if update(City)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'City',(RTRIM(LTRIM(d.City))),(RTRIM(LTRIM(i.City))),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.City)),'') != ISNULL(RTRIM(LTRIM(d.City)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update State
BEGIN TRY
if update(State)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'State',(RTRIM(LTRIM(d.State))),(RTRIM(LTRIM(i.State))),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.State)),'') != ISNULL(RTRIM(LTRIM(d.State)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ZipCode
BEGIN TRY
if update(ZipCode)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Zip Code',d.ZipCode,i.ZipCode,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ZipCode,'') != ISNULL(d.ZipCode,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update SubmissionUserName
BEGIN TRY
if update(SubmissionUserName)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submission User Name',(RTRIM(LTRIM(d.SubmissionUserName))),(RTRIM(LTRIM(i.SubmissionUserName))),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.SubmissionUserName)),'') != ISNULL(RTRIM(LTRIM(d.SubmissionUserName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update SubmissionPassword
BEGIN TRY
if update(SubmissionPassword)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submission Password',d.SubmissionPassword,i.SubmissionPassword,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmissionPassword,'') != ISNULL(d.SubmissionPassword,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ManualSubmission
BEGIN TRY
if update(ManualSubmission)   
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Manual Submission',(case when d.ManualSubmission = 1 then 'Yes' else 'No' end),(case when i.ManualSubmission = 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ManualSubmission,'') != ISNULL(d.ManualSubmission,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update FileName
BEGIN TRY
if update(FileName)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'File Name',(RTRIM(LTRIM(d.FileName))),(RTRIM(LTRIM(i.FileName))),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.FileName)),'') != ISNULL(RTRIM(LTRIM(d.FileName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_837_NM1_41_SubmitterName
BEGIN TRY
if update(X12_837_NM1_41_SubmitterName)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_NM1_41_Submitter Name',(RTRIM(LTRIM(d.X12_837_NM1_41_SubmitterName))),(RTRIM(LTRIM(i.X12_837_NM1_41_SubmitterName))),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.X12_837_NM1_41_SubmitterName)),'') != ISNULL(RTRIM(LTRIM(d.X12_837_NM1_41_SubmitterName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_837_NM1_41_SubmitterID
BEGIN TRY
if update(X12_837_NM1_41_SubmitterID)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_NM1_41_Submitter ID',d.X12_837_NM1_41_SubmitterID,i.X12_837_NM1_41_SubmitterID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_837_NM1_41_SubmitterID,'') != ISNULL(d.X12_837_NM1_41_SubmitterID,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_837_ISA_02
BEGIN TRY
if update(X12_837_ISA_02)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_ISA_02',d.X12_837_ISA_02,i.X12_837_ISA_02,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_837_ISA_02,'') != ISNULL(d.X12_837_ISA_02,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_837_ISA_04
BEGIN TRY
if update(X12_837_ISA_04)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_ISA_04',d.X12_837_ISA_04,i.X12_837_ISA_04,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_837_ISA_04,'') != ISNULL(d.X12_837_ISA_04,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_837_ISA_06
BEGIN TRY
if update(X12_837_ISA_06)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_ISA_06',d.X12_837_ISA_06,i.X12_837_ISA_06,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_837_ISA_06,'') != ISNULL(d.X12_837_ISA_06,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_837_GS_02
BEGIN TRY
if update(X12_837_GS_02)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_GS_02',d.X12_837_GS_02,i.X12_837_GS_02,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_837_GS_02,'') != ISNULL(d.X12_837_GS_02,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update SubmitterContactPerson
BEGIN TRY
if update(SubmitterContactPerson)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submitter Contact Person',(RTRIM(LTRIM(d.SubmitterContactPerson))),(RTRIM(LTRIM(i.SubmitterContactPerson))),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.SubmitterContactPerson)),'') != ISNULL(RTRIM(LTRIM(d.SubmitterContactPerson)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update SubmitterContactNumber
BEGIN TRY
if update(SubmitterContactNumber)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submitter Contact Number',d.SubmitterContactNumber,i.SubmitterContactNumber,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmitterContactNumber,'') != ISNULL(d.SubmitterContactNumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update SubmitterEmail
BEGIN TRY
if update(SubmitterEmail)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submitter Email',(RTRIM(LTRIM(d.SubmitterEmail))),(RTRIM(LTRIM(i.SubmitterEmail))),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.SubmitterEmail)),'') != ISNULL(RTRIM(LTRIM(d.SubmitterEmail)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update SubmitterFaxNumber
BEGIN TRY
if update(SubmitterFaxNumber)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submitter Fax Number',d.SubmitterFaxNumber,i.SubmitterFaxNumber,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmitterFaxNumber,'') != ISNULL(d.SubmitterFaxNumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_270_NM1_41_SubmitterName
BEGIN TRY
if update(X12_270_NM1_41_SubmitterName)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_NM1_41_Submitter Name',(RTRIM(LTRIM(d.X12_270_NM1_41_SubmitterName))),(RTRIM(LTRIM(i.X12_270_NM1_41_SubmitterName))),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.X12_270_NM1_41_SubmitterName)),'') != ISNULL(RTRIM(LTRIM(d.X12_270_NM1_41_SubmitterName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_270_NM1_41_SubmitterID
BEGIN TRY
if update(X12_270_NM1_41_SubmitterID)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_NM1_41_Submitter ID',d.X12_270_NM1_41_SubmitterID,i.X12_270_NM1_41_SubmitterID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_270_NM1_41_SubmitterID,'') != ISNULL(d.X12_270_NM1_41_SubmitterID,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_270_ISA_02
BEGIN TRY
if update(X12_270_ISA_02)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_ISA_02',d.X12_270_ISA_02,i.X12_270_ISA_02,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_270_ISA_02,'') != ISNULL(d.X12_270_ISA_02,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_270_ISA_04
BEGIN TRY
if update(X12_270_ISA_04)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_ISA_04',d.X12_270_ISA_04,i.X12_270_ISA_04,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_270_ISA_04,'') != ISNULL(d.X12_270_ISA_04,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_270_ISA_06
BEGIN TRY
if update(X12_270_ISA_06)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_ISA_06',d.X12_270_ISA_06,i.X12_270_ISA_06,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_270_ISA_06,'') != ISNULL(d.X12_270_ISA_06,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_270_GS_02
BEGIN TRY
if update(X12_270_GS_02)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_GS_02',d.X12_270_GS_02,i.X12_270_GS_02,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_270_GS_02,'') != ISNULL(d.X12_270_GS_02,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_276_NM1_41_SubmitterName
BEGIN TRY
if update(X12_276_NM1_41_SubmitterName)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_NM1_41_Submitter Name',(RTRIM(LTRIM(d.X12_276_NM1_41_SubmitterName))),(RTRIM(LTRIM(i.X12_276_NM1_41_SubmitterName))),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.X12_276_NM1_41_SubmitterName)),'') != ISNULL(RTRIM(LTRIM(d.X12_276_NM1_41_SubmitterName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_276_NM1_41_SubmitterID
BEGIN TRY
if update(X12_276_NM1_41_SubmitterID)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_NM1_41_Submitter ID',d.X12_276_NM1_41_SubmitterID,i.X12_276_NM1_41_SubmitterID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_276_NM1_41_SubmitterID,'') != ISNULL(d.X12_276_NM1_41_SubmitterID,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_276_ISA_02
BEGIN TRY
if update(X12_276_ISA_02)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_ISA_02',d.X12_276_ISA_02,i.X12_276_ISA_02,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_276_ISA_02,'') != ISNULL(d.X12_276_ISA_02,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_276_ISA_04
BEGIN TRY
if update(X12_276_ISA_04)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_ISA_04',d.X12_276_ISA_04,i.X12_276_ISA_04,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_276_ISA_04,'') != ISNULL(d.X12_276_ISA_04,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_276_ISA_06
BEGIN TRY
if update(X12_276_ISA_06)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_ISA_06',d.X12_276_ISA_06,i.X12_276_ISA_06,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_276_ISA_06,'') != ISNULL(d.X12_276_ISA_06,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_276_GS_02
BEGIN TRY
if update(X12_276_GS_02)
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_GS_02',d.X12_276_GS_02,i.X12_276_GS_02,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_276_GS_02,'') != ISNULL(d.X12_276_GS_02,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ReceiverID
BEGIN TRY
if update(ReceiverID)
--Select * into #t1 from Receiver
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Receiver ID',cDel.[Name],cUpd.[Name],d.ReceiverID,i.ReceiverID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReceiverID,'1') != ISNULL(d.ReceiverID,'1')
Join Receiver as cDel on cDel.ID = ISNULL(d.ReceiverID,'1')
join Receiver as cUpd on cUpd.ID = ISNULL(i.ReceiverID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ClientID
BEGIN TRY
if update(ClientID)
--Select * into #t from Client
insert into SubmitterAudit(SubmitterID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Client ID',cDel.[Name],cUpd.[Name],d.ClientID,i.ClientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClientID,'1') != ISNULL(d.ClientID,'1')
Join Client as cDel on cDel.ID = ISNULL(d.ClientID,'1')
join Client as cUpd on cUpd.ID = ISNULL(i.ClientID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
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
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM TaxonomyAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM TaxonomyAudit  

--when update Speciality
BEGIN TRY
if update(Speciality)
insert into TaxonomyAudit(TaxonomyID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Speciality',d.Speciality,i.Speciality,HOST_NAME(),i.UpdateBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Speciality,'') != ISNULL(d.Speciality,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update AMADescription
BEGIN TRY
if update(AMADescription)
insert into TaxonomyAudit(TaxonomyID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'AMA Description',(RTRIM(LTRIM(d.AMADescription))),(RTRIM(LTRIM(i.AMADescription))),HOST_NAME(),i.UpdateBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.AMADescription)),'') != ISNULL(RTRIM(LTRIM(d.AMADescription)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update SpecialityType
BEGIN TRY
if update(SpecialityType)
insert into TaxonomyAudit(TaxonomyID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Speciality Type',d.SpecialityType,i.SpecialityType,HOST_NAME(),i.UpdateBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SpecialityType,'') != ISNULL(d.SpecialityType,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update NUCCCode
BEGIN TRY
if update(NUCCCode)
insert into TaxonomyAudit(TaxonomyID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'NUCC Code',d.NUCCCode,i.NUCCCode,HOST_NAME(),i.UpdateBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.NUCCCode,'') != ISNULL(d.NUCCCode,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update NUCCDescription
BEGIN TRY
if update(NUCCDescription)
insert into TaxonomyAudit(TaxonomyID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'NUCC Description',(RTRIM(LTRIM(d.NUCCDescription))),(RTRIM(LTRIM(i.NUCCDescription))),HOST_NAME(),i.UpdateBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.NUCCDescription)),'') != ISNULL(RTRIM(LTRIM(d.NUCCDescription)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

END
GO

--=====================================
--Description: <tgrAdjustmentCodeAudit Trigger>
--======================================
IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrAdjustmentCodeAudit')
DROP TRIGGER [dbo].[tgrAdjustmentCodeAudit]
GO
create TRIGGER [dbo].[tgrAdjustmentCodeAudit]
ON [dbo].[AdjustmentCode] after Update
AS 
BEGIN
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM AdjustmentCodeAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM AdjustmentCodeAudit  

--when update Code
BEGIN TRY
if update(Code)
insert into AdjustmentCodeAudit(AdjustmentCodeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Code',d.Code,i.Code,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Code,'') != ISNULL(d.Code,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update Description
BEGIN TRY
if update([Description])
insert into AdjustmentCodeAudit(AdjustmentCodeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Description',(RTRIM(LTRIM(d.[Description]))),(RTRIM(LTRIM(i.[Description]))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.[Description])),'') != ISNULL(RTRIM(LTRIM(d.[Description])),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update Type
BEGIN TRY
if update(Type)
insert into AdjustmentCodeAudit(AdjustmentCodeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Type',d.Type,i.Type,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Type,'') != ISNULL(d.Type,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update ActionID
BEGIN TRY
if update(ActionID)
--Select * into #t from Action
insert into AdjustmentCodeAudit(AdjustmentCodeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Action ID',cDel.[Name],cUpd.[Name],d.ActionID,i.ActionID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ActionID,'1') != ISNULL(d.ActionID,'1')
Join Action as cDel on cDel.ID = ISNULL(d.ActionID,'1')
join Action as cUpd on cUpd.ID = ISNULL(i.ActionID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update ReasonID
BEGIN TRY
if update(ReasonID)
--Select * into #t1 from Reason
insert into AdjustmentCodeAudit(AdjustmentCodeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reason ID',cDel.[Name],cUpd.[Name],d.ReasonID,i.ReasonID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReasonID,'1') != ISNULL(d.ReasonID,'1')
Join Reason as cDel on cDel.ID = ISNULL(d.ReasonID,'1')
join Reason as cUpd on cUpd.ID = ISNULL(i.ReasonID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update GroupID
BEGIN TRY
if update(GroupID)
--Select * into #t3 from [Group]
insert into AdjustmentCodeAudit(AdjustmentCodeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Group ID',cDel.[Name],cUpd.[Name],d.GroupID,i.GroupID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.GroupID,'1') != ISNULL(d.GroupID,'1')
Join [Group] as cDel on cDel.ID = ISNULL(d.GroupID,'1')
join [Group] as cUpd on cUpd.ID = ISNULL(i.GroupID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

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
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM ActionAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM ActionAudit  

--when update Name
BEGIN TRY
if update(Name)
insert into ActionAudit(ActionID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',(RTRIM(LTRIM(d.Name))),(RTRIM(LTRIM(i.Name))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Name)),'') != ISNULL(RTRIM(LTRIM(d.Name)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update Description
BEGIN TRY
if update(Description)
insert into ActionAudit(ActionID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Description',(RTRIM(LTRIM(d.Description))),(RTRIM(LTRIM(i.Description))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Description)),'') != ISNULL(RTRIM(LTRIM(d.Description)),'')
END TRY  
BEGIN CATCH  
    ----SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;

END CATCH;

----when update UserID
--BEGIN TRY
--if update(UserID)
--select * into #t2 from AspNetUsers
--insert into ActionAudit(ActionID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
--select i.ID, @TransactionID,'Email',cDel.FirstName+' '+cDel.LastName,cUpd.FirstName+' '+cUpd.LastName,d.UserID,i.UserID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
--from inserted as i
--inner join deleted as d on i.ID = d.ID and ISNULL(i.UserID,'') != ISNULL(d.UserID,'')
--Join AspNetUsers as cDel on cDel.ID = ISNULL(d.UserID,'')
--join AspNetUsers as cUpd on cUpd.ID = ISNULL(i.UserID,'')
--END TRY  
--BEGIN CATCH  
--    --SELECT   
--     print ERROR_NUMBER(); --AS ErrorNumber  
--     print ERROR_MESSAGE(); --AS ErrorMessage;  
--END CATCH;
END
GO

--=================================================
--Description: <tgrChargeSubmissionHistory Trigger>
--=================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrChargeSubmissionHistory')
DROP TRIGGER [dbo].[tgrChargeSubmissionHistory]
GO
Create TRIGGER [dbo].[tgrChargeSubmissionHistory]
ON [dbo].[ChargeSubmissionHistory] after Update
AS 
BEGIN
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM [dbo].[ChargeSubmissionHistoryAudit]
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM [dbo].[ChargeSubmissionHistoryAudit]

--when update ChargeID
BEGIN TRY
if update(ChargeID)
insert into ChargeSubmissionHistoryAudit(ChargeSubmissionHistoryID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Charge ID',d.ChargeID,i.ChargeID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ChargeID,'1') != ISNULL(d.ChargeID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ReceiverID
BEGIN TRY
if update(ReceiverID)
--Select * into #t1 from Receiver
insert into ChargeSubmissionHistoryAudit(ChargeSubmissionHistoryID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Receiver ID',cDel.[Name],cUpd.[Name],d.ReceiverID,i.ReceiverID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReceiverID,'1') != ISNULL(d.ReceiverID,'1')
Join Receiver as cDel on cDel.ID = ISNULL(d.ReceiverID,'1')
join Receiver as cUpd on cUpd.ID = ISNULL(i.ReceiverID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update SubmissionLogID
BEGIN TRY
if update(SubmissionLogID)
insert into ChargeSubmissionHistoryAudit(ChargeSubmissionHistoryID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submission Log ID',d.SubmissionLogID,i.SubmissionLogID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmissionLogID,'1') != ISNULL(d.SubmissionLogID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update SubmitType
BEGIN TRY
if update(SubmitType)
insert into ChargeSubmissionHistoryAudit(ChargeSubmissionHistoryID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submit Type',d.SubmitType,i.SubmitType,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmitType,'') != ISNULL(d.SubmitType,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update FormType
BEGIN TRY
if update(FormType)
insert into ChargeSubmissionHistoryAudit(ChargeSubmissionHistoryID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Form Type',d.FormType,i.FormType,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FormType,'') != ISNULL(d.FormType,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PatientPlanID
BEGIN TRY
if update(PatientPlanID)
insert into ChargeSubmissionHistoryAudit(ChargeSubmissionHistoryID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Plan ID',d.PatientPlanID,i.PatientPlanID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientPlanID,'1') != ISNULL(d.PatientPlanID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update Amount
BEGIN TRY
if update(Amount)
insert into ChargeSubmissionHistoryAudit(ChargeSubmissionHistoryID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Amount',d.Amount,i.Amount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Amount,'1') != ISNULL(d.Amount,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

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
--SELECT @TransactionID =  max(TransactionID) FROM UserPracticeAudit
--IF @TransactionID is null set @TransactionID = 1 
--ELSE
--SELECT @TransactionID =  max(TransactionID) +1 FROM UserPracticeAudit

----When User Update PracticeID
--BEGIN TRY
--IF Update(PracticeID)
--INSERT INTO UserPracticeAudit(ID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
--SELECT  i.UserID, @TransactionID,'Practice ID',pDel.[Name],pUpd.[Name],d.PracticeID,i.PracticeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
--FROM inserted as i
--INNER JOIN Deleted as d ON i.UserID = d.UserID and ISNULL(i.PracticeID,'') != ISNULL(d.PracticeID,'')
--Join Practice as pDel on pdel.ID = ISNULL(d.PracticeID,'')
--join Practice as pUpd on pUpd.ID = ISNULL(i.PracticeID,'')
--END TRY  
--BEGIN CATCH  
--    --SELECT   
--     print ERROR_NUMBER(); --AS ErrorNumber  
--     print ERROR_MESSAGE(); --AS ErrorMessage;  
--END CATCH;  

----When User Update UserID
--BEGIN TRY
--IF Update(UserID)
--select * into #t1 from AspNetUsers
--INSERT INTO UserPracticeAudit(ID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
--SELECT  i.UserID, @TransactionID,'User ID',pDel.FirstName+' '+pDel.LastName,pUpd.FirstName+' '+pUpd.LastName,d.UserID,i.UserID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
--FROM inserted as i
--INNER JOIN Deleted as d ON i.UserID = d.UserID and ISNULL(i.UserID,'') != ISNULL(d.UserID,'')
--Join AspNetUsers as pDel on pdel.ID = ISNULL(d.UserID,'')
--join AspNetUsers as pUpd on pUpd.ID = ISNULL(i.UserID,'')
--END TRY  
--BEGIN CATCH  
--    --SELECT   
--     print ERROR_NUMBER(); --AS ErrorNumber  
--     print ERROR_MESSAGE(); --AS ErrorMessage;  
--END CATCH;  

----When User Update AssignedByUserId
--BEGIN TRY
--IF Update(AssignedByUserId)
--INSERT INTO UserPracticeAudit(ID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
--SELECT  i.UserID, @TransactionID,'Assigned By UserId',d.AssignedByUserId,i.AssignedByUserId,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
--FROM inserted as i
--INNER JOIN Deleted as d ON i.UserID = d.UserID and ISNULL(i.AssignedByUserId,'') != ISNULL(d.AssignedByUserId,'')
--END TRY  
--BEGIN CATCH  
--    --SELECT   
--     print ERROR_NUMBER(); --AS ErrorNumber  
--     print ERROR_MESSAGE(); --AS ErrorMessage;  
--END CATCH;  

----When User Update UPALastModified 
--BEGIN TRY
--IF Update(UPALastModified )
--INSERT INTO UserPracticeAudit(ID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
--SELECT  i.UserID, @TransactionID,'UPA Last Modified ',d.UPALastModified ,i.UPALastModified ,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
--FROM inserted as i
--INNER JOIN Deleted as d ON i.UserID = d.UserID and ISNULL(i.UPALastModified,'')  != ISNULL(d.UPALastModified ,'')
--END TRY  
--BEGIN CATCH  
--    --SELECT   
--     print ERROR_NUMBER(); --AS ErrorNumber  
--     print ERROR_MESSAGE(); --AS ErrorMessage;  
--END CATCH;  

----When User Update Status 
--BEGIN TRY
--IF Update(Status )
--INSERT INTO UserPracticeAudit(ID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
--SELECT  i.UserID, @TransactionID,'Status ',(case when d.Status = 1 then 'Yes' else 'No' end) ,(case when d.Status = 1 then 'Yes' else 'No' end) ,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
--FROM inserted as i
--INNER JOIN Deleted as d ON i.UserID = d.UserID and ISNULL(i.Status,'')  != ISNULL(d.Status ,'')
--END TRY  
--BEGIN CATCH  
--    --SELECT   
--     print ERROR_NUMBER(); --AS ErrorNumber  
--     print ERROR_MESSAGE(); --AS ErrorMessage;  
--END CATCH;  
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
BEGIN
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM DesignationsAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM DesignationsAudit 

--when update Name
BEGIN TRY
if update(Name)
insert into DesignationsAudit(DesignationsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',(RTRIM(LTRIM(d.Name))),(RTRIM(LTRIM(i.Name))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Name)),'') != ISNULL(RTRIM(LTRIM(d.Name)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;
END
GO


--=====================================
--Description: <tgrDownloadedFileAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrDownloadedFileAudit')
DROP TRIGGER [dbo].[tgrDownloadedFileAudit]
GO
create TRIGGER [dbo].[tgrDownloadedFileAudit]
ON [dbo].[DownloadedFile] after Update
AS 
BEGIN
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM DownloadedFileAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM DownloadedFileAudit  

--when update ReportsLogID
BEGIN TRY
if update(ReportsLogID)
insert into DownloadedFileAudit(DownloadedFileID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reports Log ID',d.ReportsLogID,i.ReportsLogID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReportsLogID,'') != ISNULL(d.ReportsLogID,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update FilePath
BEGIN TRY
if update(FilePath)
insert into DownloadedFileAudit(DownloadedFileID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'File Path',d.FilePath,i.FilePath,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FilePath,'') != ISNULL(d.FilePath,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update FileType
BEGIN TRY
if update(FileType)
insert into DownloadedFileAudit(DownloadedFileID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'File Type',d.FileType,i.FileType,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FileType,'') != ISNULL(d.FileType,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update Processed
BEGIN TRY
if update(Processed)
insert into DownloadedFileAudit(DownloadedFileID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Processed',(case when d.Processed = 1 then 'Yes' when d.Processed =0 then 'No' end),(case when i.Processed = 1 then 'Yes' when i.Processed =0 then 'No' end),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Processed,'2') != ISNULL(d.Processed,'2')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;
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
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM [dbo].[GroupAudit]
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM [dbo].[GroupAudit]

--when update Name
BEGIN TRY
if update(Name)
insert into GroupAudit(GroupID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',(RTRIM(LTRIM(d.Name))),(RTRIM(LTRIM(i.Name))),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Name)),'') != ISNULL(RTRIM(LTRIM(d.Name)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update Description
BEGIN TRY
if update(Description)
insert into GroupAudit(GroupID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Description',(RTRIM(LTRIM(d.Description))),(RTRIM(LTRIM(i.Description))),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Description)),'') != ISNULL(RTRIM(LTRIM(d.Description)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

----when update UserID
--BEGIN TRY
--if update(UserID)
--Select * into #t from AspNetUsers
--insert into GroupAudit(GroupID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
--select i.ID, @TransactionID,'User ID',cDel.FirstName+' '+cDel.LastName,cUpd.FirstName+' '+cUpd.LastName,d.UserID,i.UserID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
--from inserted as i
--inner join deleted as d on i.ID = d.ID and ISNULL(i.UserID,'') != ISNULL(d.UserID,'')
--Join AspNetUsers as cDel on cDel.ID = ISNULL(d.UserID,'')
--join AspNetUsers as cUpd on cUpd.ID = ISNULL(i.UserID,'')
--END TRY  
--BEGIN CATCH  
--  --SELECT   
--     print ERROR_NUMBER(); --AS ErrorNumber  
--     print ERROR_MESSAGE(); --AS ErrorMessage;   
--END CATCH;
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
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM NotesAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM NotesAudit  

--when update PracticeID
BEGIN TRY
if update(PracticeID)
--Select * into #t1 from Receiver
insert into NotesAudit(NotesID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Practice ID',cDel.[Name],cUpd.[Name],d.PracticeID,i.PracticeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PracticeID,'1') != ISNULL(d.PracticeID,'1')
Join Receiver as cDel on cDel.ID = ISNULL(d.PracticeID,'1')
join Receiver as cUpd on cUpd.ID = ISNULL(i.PracticeID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PlanFollowupID
BEGIN TRY
if update(PlanFollowupID)
insert into NotesAudit(NotesID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Plan Followup ID',d.PlanFollowupID,i.PlanFollowupID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PlanFollowupID,'1') != ISNULL(d.PlanFollowupID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PatientFollowUpID
BEGIN TRY
if update(PatientFollowUpID)
insert into NotesAudit(NotesID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient FollowUp ID',d.PatientFollowUpID,i.PatientFollowUpID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientFollowUpID,'') != ISNULL(d.PatientFollowUpID,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update Note
BEGIN TRY
if update(Note)
insert into NotesAudit(NotesID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',(RTRIM(LTRIM(d.Note))),(RTRIM(LTRIM(i.Note))),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Note)),'') != ISNULL(RTRIM(LTRIM(d.Note)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update NotesDate
BEGIN TRY
if update(NotesDate)
insert into NotesAudit(NotesID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes Date',d.NotesDate,i.NotesDate,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i   
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.NotesDate as DateTime),'') != ISNULL(cast(d.NotesDate as datetime),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PatientFollowUpID
BEGIN TRY
if update(PatientFollowUpID)
--Select * into #t1 from Receiver
insert into NotesAudit(NotesID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient FollowUp ID',d.PatientFollowUpID,i.PatientFollowUpID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientFollowUpID,'1') != ISNULL(d.PatientFollowUpID,'1')
--Join Receiver as cDel on cDel.ID = ISNULL(d.PatientFollowUpID,'1')
--join Receiver as cUpd on cUpd.ID = ISNULL(i.PatientFollowUpID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PatientID
BEGIN TRY
if update(PatientID)
--Select * into #t1 from Receiver
insert into NotesAudit(NotesID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient ID',d.PatientID,i.PatientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientID,'1') != ISNULL(d.PatientID,'1')
--Join Receiver as cDel on cDel.ID = ISNULL(d.PatientFollowUpID,'1')
--join Receiver as cUpd on cUpd.ID = ISNULL(i.PatientFollowUpID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update VisitID
BEGIN TRY
if update(VisitID)
--Select * into #t1 from Receiver
insert into NotesAudit(NotesID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Visit ID',d.VisitID,i.VisitID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.VisitID,'1') != ISNULL(d.VisitID,'1')
--Join Receiver as cDel on cDel.ID = ISNULL(d.PatientFollowUpID,'1')
--join Receiver as cUpd on cUpd.ID = ISNULL(i.PatientFollowUpID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;
END
GO

--=====================================
--Description: <tgrPatientFollowupAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrPatientFollowUpAudit')
DROP TRIGGER [dbo].[tgrPatientFollowUpAudit]
GO
Create TRIGGER [dbo].[tgrPatientFollowUpAudit]
ON [dbo].[PatientFollowUp] after Update
AS 
BEGIN
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM PatientFollowUpAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM PatientFollowUpAudit  

--when update PatientID
BEGIN TRY
if update(PatientID)
--Select * into #t from Patient
insert into PatientFollowUpAudit(PatientFollowUpID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient ID',cDel.[FirstName],cUpd.[FirstName],d.PatientID,i.PatientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientID,'1') != ISNULL(d.PatientID,'1')
Join Patient as cDel on cDel.ID = ISNULL(d.PatientID,'1')
join Patient as cUpd on cUpd.ID = ISNULL(i.PatientID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update GroupID
BEGIN TRY
if update(GroupID)
--Select * into #t1 from [Group]
insert into PatientFollowUpAudit(PatientFollowUpID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Group ID',cDel.[Name],cUpd.[Name],d.GroupID,i.GroupID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.GroupID,'1') != ISNULL(d.GroupID,'1')
Join [Group] as cDel on cDel.ID = ISNULL(d.GroupID,'1')
join [Group] as cUpd on cUpd.ID = ISNULL(i.GroupID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ReasonID
BEGIN TRY
if update(ReasonID)
--Select * into #t2 from Reason
insert into PatientFollowUpAudit(PatientFollowUpID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reason ID',cDel.[Name],cUpd.[Name],d.ReasonID,i.ReasonID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReasonID,'1') != ISNULL(d.ReasonID,'1')
Join Reason as cDel on cDel.ID = ISNULL(d.ReasonID,'1')
join Reason as cUpd on cUpd.ID = ISNULL(i.ReasonID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ActionID
BEGIN TRY
if update(ActionID)
--Select * into #t3 from [Action]
insert into PatientFollowUpAudit(PatientFollowUpID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Action ID',cDel.[Name],cUpd.[Name],d.ActionID,i.ActionID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ActionID,'1') != ISNULL(d.ActionID,'1')
Join [Action] as cDel on cDel.ID = ISNULL(d.ActionID,'1')
join [Action] as cUpd on cUpd.ID = ISNULL(i.ActionID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update PaymentVisitID
BEGIN TRY
if update(PaymentVisitID)
insert into PatientFollowUpAudit(PatientFollowUpID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payment Visit ID',d.PaymentVisitID,i.PaymentVisitID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaymentVisitID,'') != ISNULL(d.PaymentVisitID,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update AdjustmentCodeID
BEGIN TRY
if update(AdjustmentCodeID)
insert into PatientFollowUpAudit(PatientFollowUpID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Code ID',d.AdjustmentCodeID,i.AdjustmentCodeID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentCodeID,'') != ISNULL(d.AdjustmentCodeID,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Notes
BEGIN TRY
if update(Notes)
insert into PatientFollowUpAudit(PatientFollowUpID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',(RTRIM(LTRIM(d.Notes))),(RTRIM(LTRIM(i.Notes))),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Notes)),'') != ISNULL(RTRIM(LTRIM(d.Notes)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update TickleDate
BEGIN TRY
if update(TickleDate)
insert into PatientFollowUpAudit(PatientFollowUpID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Tickle Date',d.TickleDate,i.TickleDate,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.TickleDate as date),'') != ISNULL(cast(d.TickleDate as date),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 


--when update Statement1SentDate
BEGIN TRY
if update(Statement1SentDate)
insert into PatientFollowUpAudit(PatientFollowUpID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Statement1 Sent Date',d.Statement1SentDate,i.Statement1SentDate,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.Statement1SentDate as datetime),'') != ISNULL(cast(d.Statement1SentDate as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Statement2SentDate
BEGIN TRY
if update(Statement2SentDate)
insert into PatientFollowUpAudit(PatientFollowUpID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Statement2 Sent Date',d.Statement2SentDate,i.Statement2SentDate,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.Statement2SentDate as datetime),'') != ISNULL(cast(d.Statement2SentDate as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update Statement3SentDate
BEGIN TRY
if update(Statement3SentDate)
insert into PatientFollowUpAudit(PatientFollowUpID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Statement3 Sent Date',d.Statement3SentDate,i.Statement3SentDate,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.Statement3SentDate as datetime),'') != ISNULL(cast(d.Statement3SentDate as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update Status
BEGIN TRY
if update(Status)
insert into PatientFollowUpAudit(PatientFollowUpID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Status',d.Status,i.Status,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and i.Status != d.Status
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update Resolved
BEGIN TRY
if update(Resolved)
insert into PatientFollowUpAudit(PatientFollowUpID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Resolved',(case when d.Resolved= 1 then 'Yes' when d.Resolved =0 then 'No' end),(case when i.Resolved= 1 then 'Yes' when i.Resolved =0 then 'No' end),HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Resolved,'2') != ISNULL(d.Resolved,'2')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;
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
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM PlanFollowupAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM PlanFollowupAudit  

--when update AdjustmentCodeID
BEGIN TRY
if update(AdjustmentCodeID)
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Code ID',AdjDel.[Description],AdjUpd.[Description],d.AdjustmentCodeID,i.AdjustmentCodeID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentCodeID,'1') != ISNULL(d.AdjustmentCodeID,'1')
Join AdjustmentCode as AdjDel on AdjDel.ID = ISNULL(d.AdjustmentCodeID,'1')
join AdjustmentCode as AdjUpd on AdjUpd.ID = ISNULL(i.AdjustmentCodeID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update VisitID
BEGIN TRY
if update(VisitID)
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Visit ID',d.VisitID,i.VisitID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.VisitID,'1') != ISNULL(d.VisitID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update GroupID
BEGIN TRY
if update(GroupID)
--Select * into #t from [Group]
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Group ID',cDel.[Name],cUpd.[Name],d.GroupID,i.GroupID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.GroupID,'1') != ISNULL(d.GroupID,'1')
Join [Group] as cDel on cDel.ID = ISNULL(d.GroupID,'1')
join [Group] as cUpd on cUpd.ID = ISNULL(i.GroupID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ReasonID
BEGIN TRY
if update(ReasonID)
--Select * into #t1 from Reason
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reason ID',cDel.[Name],cUpd.[Name],d.ReasonID,i.ReasonID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReasonID,'1') != ISNULL(d.ReasonID,'1')
Join Reason as cDel on cDel.ID = ISNULL(d.ReasonID,'1')
join Reason as cUpd on cUpd.ID = ISNULL(i.ReasonID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ActionID
BEGIN TRY
if update(ActionID)
--Select * into #t2 from Action
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Action ID',cDel.[Name],cUpd.[Name],d.ActionID,i.ActionID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ActionID,'1') != ISNULL(d.ActionID,'1')
Join Action as cDel on cDel.ID = ISNULL(d.ActionID,'1')
join Action as cUpd on cUpd.ID = ISNULL(i.ActionID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update VisitStatusID
BEGIN TRY
if update(VisitStatusID)
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Visit Status ID',d.VisitStatusID,i.VisitStatusID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.VisitStatusID,'1') != ISNULL(d.VisitStatusID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update PaymentVisitID
BEGIN TRY
if update(PaymentVisitID)
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payment Visit ID',d.PaymentVisitID,i.PaymentVisitID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaymentVisitID,'1') != ISNULL(d.PaymentVisitID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Notes
BEGIN TRY
if update(Notes)
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',(RTRIM(LTRIM(d.Notes))),(RTRIM(LTRIM(i.Notes))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Notes)),'') != ISNULL(RTRIM(LTRIM(d.Notes)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update TickleDate
BEGIN TRY
if update(TickleDate)
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Tickle Date',d.TickleDate,i.TickleDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.TickleDate as datetime),'') != ISNULL(Cast(d.TickleDate as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Resolved
BEGIN TRY
if update(Resolved)
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Resolved',(case when d.Resolved= 1 then 'Yes' when d.Resolved =0 then 'No' end),(case when i.Resolved= 1 then 'Yes' when i.Resolved =0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Resolved,'2') != ISNULL(d.Resolved,'2')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update RemitCode
BEGIN TRY
if update(RemitCode)
insert into PlanFollowupAudit(PlanFollowupID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Remit Code',d.RemitCode,i.RemitCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RemitCode,'') != ISNULL(d.RemitCode,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
END
GO

----=====================================
----Description: <tgrPatientAppointmentAudit Trigger>
----======================================
--IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrPatientAppointmentAudit')
--DROP TRIGGER [dbo].[tgrPatientAppointmentAudit]
--GO
--Create TRIGGER [dbo].[tgrPatientAppointmentAudit]
--ON [dbo].[PatientAppointment] after Update
--AS 
--BEGIN
--DECLARE @TransactionID INT;
--SELECT @TransactionID =  max(TransactionID) FROM PatientAppointmentAudit
--IF @TransactionID is null set @TransactionID = 1 
--ELSE
--SELECT @TransactionID =  max(TransactionID) +1 FROM PatientAppointmentAudit  

----when update PatientID
--BEGIN TRY
--if update(PatientID)
----Select * into #t from Patient
--insert into PatientAppointmentAudit(PatientAppointmentID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
--select i.ID, @TransactionID,'Practice ID',cDel.[FirstName],cUpd.[FirstName],d.PatientID,i.PatientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
--from inserted as i
--inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientID,'') != ISNULL(d.PatientID,'')
--Join Patient as cDel on cDel.ID = ISNULL(d.PatientID,'')
--join Patient as cUpd on cUpd.ID = ISNULL(i.PatientID,'')
--END TRY  
--BEGIN CATCH  
--  --SELECT   
--     print ERROR_NUMBER(); --AS ErrorNumber  
--     print ERROR_MESSAGE(); --AS ErrorMessage;   
--END CATCH;

----when update LocationID
--BEGIN TRY
--if update(LocationID)
----Select * into #t1 from Location
--insert into PatientAppointmentAudit(PatientAppointmentID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
--select i.ID, @TransactionID,'Location ID',cDel.[Name],cUpd.[Name],d.LocationID,i.LocationID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
--from inserted as i
--inner join deleted as d on i.ID = d.ID and ISNULL(i.LocationID,'') != ISNULL(d.LocationID,'')
--Join Location as cDel on cDel.ID = ISNULL(d.LocationID,'')
--join Location as cUpd on cUpd.ID = ISNULL(i.LocationID,'')
--END TRY  
--BEGIN CATCH  
--  --SELECT   
--     print ERROR_NUMBER(); --AS ErrorNumber  
--     print ERROR_MESSAGE(); --AS ErrorMessage;   
--END CATCH;

----when update ProviderID
--BEGIN TRY
--if update(ProviderID)
----Select * into #t2 from Provider
--insert into PatientAppointmentAudit(PatientAppointmentID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
--select i.ID, @TransactionID,'Provider ID',cDel.[Name],cUpd.[Name],d.ProviderID,i.ProviderID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
--from inserted as i
--inner join deleted as d on i.ID = d.ID and ISNULL(i.ProviderID,'') != ISNULL(d.ProviderID,'')
--Join Provider as cDel on cDel.ID = ISNULL(d.ProviderID,'')
--join Provider as cUpd on cUpd.ID = ISNULL(i.ProviderID,'')
--END TRY  
--BEGIN CATCH  
--  --SELECT   
--     print ERROR_NUMBER(); --AS ErrorNumber  
--     print ERROR_MESSAGE(); --AS ErrorMessage;   
--END CATCH;

----when update ProviderSlotID
--BEGIN TRY
--if update(ProviderSlotID)
--insert into PatientAppointmentAudit(PatientAppointmentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
--select i.ID, @TransactionID,'Provider Slot ID',d.ProviderSlotID,i.ProviderSlotID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
--from inserted as i
--inner join deleted as d on i.ID = d.ID and ISNULL(i.ProviderSlotID,'') != ISNULL(d.ProviderSlotID,'')
--END TRY  
--BEGIN CATCH  
--  --SELECT   
--     print ERROR_NUMBER(); --AS ErrorNumber  
--     print ERROR_MESSAGE(); --AS ErrorMessage;   
--END CATCH;

----when update PrimarypatientPlanID
--BEGIN TRY
--if update(PrimarypatientPlanID)
----Select * Into #t4 from PatientPlan
----Select * Into #t5 from InsurancePlan 
--insert into PatientAppointmentAudit(PatientAppointmentID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
--select i.ID, @TransactionID,'Primary Patient Plan ID',dInsPlan.PlanName,uInsPlan.PlanName,d.PrimarypatientPlanID,i.PrimarypatientPlanID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
--from inserted as i
--inner join deleted as d on i.ID = d.ID and ISNULL(i.PrimarypatientPlanID,'') != ISNULL(d.PrimarypatientPlanID,'')
--join PatientPlan as dpatPlan on ISNULL(dpatPlan.ID,'') = ISNULL(d.PrimaryPatientPlanID,'')
--join InsurancePlan as dInsPlan on ISNULL(dpatPlan.InsurancePlanID,'') = ISNULL(dInsPlan.ID,'')

--left join PatientPlan as upatPlan on ISNULL(upatPlan.ID,'') = ISNULL(i.PrimaryPatientPlanID,'')
--left join InsurancePlan as uInsPlan on ISNULL(upatPlan.InsurancePlanID,'') = ISNULL(uInsPlan.ID,'')
--END TRY  
--BEGIN CATCH  
--  --SELECT   
--     print ERROR_NUMBER(); --AS ErrorNumber  
--     print ERROR_MESSAGE(); --AS ErrorMessage;   
--END CATCH;

----when update VisitReasonID
--BEGIN TRY
--if update(VisitReasonID)
----Select * into #t6 from VisitReason
--insert into PatientAppointmentAudit(PatientAppointmentID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
--select i.ID, @TransactionID,'Visit Reason ID',cDel.[Name],cUpd.[Name],d.ProviderSlotID,i.ProviderSlotID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
--from inserted as i
--inner join deleted as d on i.ID = d.ID and ISNULL(i.VisitReasonID,'') != ISNULL(d.VisitReasonID,'')
--Join VisitReason as cDel on cDel.ID = ISNULL(d.VisitReasonID,'')
--join VisitReason as cUpd on cUpd.ID = ISNULL(i.VisitReasonID,'')
--END TRY  
--BEGIN CATCH  
--  --SELECT   
--     print ERROR_NUMBER(); --AS ErrorNumber  
--     print ERROR_MESSAGE(); --AS ErrorMessage;   
--END CATCH;

----when update AppointmentDate
--BEGIN TRY
--if update(AppointmentDate)
--insert into PatientAppointmentAudit(PatientAppointmentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
--select i.ID, @TransactionID,'Appointment Date',d.AppointmentDate,i.AppointmentDate,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
--from inserted as i
--inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.AppointmentDate as datetime),'') != ISNULL(cast(d.AppointmentDate as datetime),'')
--END TRY  
--BEGIN CATCH  
--  --SELECT   
--     print ERROR_NUMBER(); --AS ErrorNumber  
--     print ERROR_MESSAGE(); --AS ErrorMessage;   
--END CATCH;

----when update Time
--BEGIN TRY
--if update([Time])
--insert into PatientAppointmentAudit(PatientAppointmentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
--select i.ID, @TransactionID,'Time',d.[Time],i.[Time],HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
--from inserted as i
--inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.[Time] as datetime),'') != ISNULL(cast(d.[Time] as datetime),'')
--END TRY  
--BEGIN CATCH  
--  --SELECT   
--     print ERROR_NUMBER(); --AS ErrorNumber  
--     print ERROR_MESSAGE(); --AS ErrorMessage;   
--END CATCH;

----when update VisitInterval
--BEGIN TRY
--if update(VisitInterval)
--insert into PatientAppointmentAudit(PatientAppointmentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
--select i.ID, @TransactionID,'Visit Interval',d.VisitInterval,i.VisitInterval,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
--from inserted as i
--inner join deleted as d on i.ID = d.ID and ISNULL(i.VisitInterval,'') != ISNULL(d.VisitInterval,'')
--END TRY  
--BEGIN CATCH  
--  --SELECT   
--     print ERROR_NUMBER(); --AS ErrorNumber  
--     print ERROR_MESSAGE(); --AS ErrorMessage;   
--END CATCH;

----when update Status
--BEGIN TRY
--if update(Status)
--insert into PatientAppointmentAudit(PatientAppointmentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
--select i.ID, @TransactionID,'Status',d.Status,i.Status,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
--from inserted as i
--inner join deleted as d on i.ID = d.ID and ISNULL(i.Status,'') != ISNULL(d.Status,'')
--END TRY  
--BEGIN CATCH  
--  --SELECT   
--     print ERROR_NUMBER(); --AS ErrorNumber  
--     print ERROR_MESSAGE(); --AS ErrorMessage;   
--END CATCH;

----when update Notes
--BEGIN TRY
--if update(Notes)
--insert into PatientAppointmentAudit(PatientAppointmentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
--select i.ID, @TransactionID,'Notes',d.Notes,i.Notes,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
--from inserted as i
--inner join deleted as d on i.ID = d.ID and ISNULL(i.Notes,'') != ISNULL(d.Notes,'')
--END TRY  
--BEGIN CATCH  
--  --SELECT   
--     print ERROR_NUMBER(); --AS ErrorNumber  
--     print ERROR_MESSAGE(); --AS ErrorMessage;   
--END CATCH;
--END
--GO

--=====================================
-- Description: <tgrPatientPaymentChargeAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrPatientPaymentChargeAudit')
DROP TRIGGER [dbo].[tgrPatientPaymentChargeAudit]
GO

Create TRIGGER [dbo].[tgrPatientPaymentChargeAudit]
ON [dbo].[PatientPaymentCharge] after Update  
As
BEGIN
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM PatientPaymentChargeAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM PatientPaymentChargeAudit 

--when update PatientPaymentID
BEGIN TRY
if update(PatientPaymentID)
insert into PatientPaymentChargeAudit(PatientPaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Payment ID',d.PatientPaymentID,i.PatientPaymentID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientPaymentID,'1') != ISNULL(d.PatientPaymentID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update VisitID
BEGIN TRY
if update(VisitID)
insert into PatientPaymentChargeAudit(PatientPaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Visit ID',d.VisitID,i.VisitID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.VisitID,'1') != ISNULL(d.VisitID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ChargeID
BEGIN TRY
if update(ChargeID)
insert into PatientPaymentChargeAudit(PatientPaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Charge ID',d.ChargeID,i.ChargeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ChargeID,'1') != ISNULL(d.ChargeID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update AllocatedAmount
BEGIN TRY
if update(AllocatedAmount)
insert into PatientPaymentChargeAudit(PatientPaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Allocated Amount',d.AllocatedAmount,i.AllocatedAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AllocatedAmount,'1') != ISNULL(d.AllocatedAmount,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Status
BEGIN TRY
if update(Status)
insert into PatientPaymentChargeAudit(PatientPaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Status',d.Status,i.Status,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Status,'') != ISNULL(d.Status,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
END
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
BEGIN 
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM PatientPaymentAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM PatientPaymentAudit 

--when update PatientID
BEGIN TRY
if update(PatientID)
--Select * into #t1 from Patient
insert into PatientPaymentAudit(PatientPaymentID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient ID',cDel.[FirstName],cUpd.[FirstName],d.PatientID,i.PatientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientID,'1') != ISNULL(d.PatientID,'1')
Join Patient as cDel on cDel.ID = ISNULL(d.PatientID,'1')
join Patient as cUpd on cUpd.ID = ISNULL(i.PatientID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;  

--when update PaymentMethod
BEGIN TRY
if update(PaymentMethod)
insert into PatientPaymentAudit(PatientPaymentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payment Method',d.PaymentMethod,i.PaymentMethod,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaymentMethod,'') != ISNULL(d.PaymentMethod,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;  

--when update PaymentDate
BEGIN TRY
if update(PaymentDate)
insert into PatientPaymentAudit(PatientPaymentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payment Date',d.PaymentDate,i.PaymentDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.PaymentDate as datetime),'') != ISNULL(Cast(d.PaymentDate as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;  


--when update PaymentAmount
BEGIN TRY
if update(PaymentAmount)
insert into PatientPaymentAudit(PatientPaymentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payment Amount',d.PaymentAmount,i.PaymentAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaymentAmount,'1') != ISNULL(d.PaymentAmount,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;  


--when update CheckNumber
BEGIN TRY
if update(CheckNumber)
insert into PatientPaymentAudit(PatientPaymentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Check Number',d.CheckNumber,i.CheckNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CheckNumber,'') != ISNULL(d.CheckNumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;  

--when update Description
BEGIN TRY
if update(Description)
insert into PatientPaymentAudit(PatientPaymentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Description',(RTRIM(LTRIM(d.Description))),(RTRIM(LTRIM(i.Description))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Description)),'') != ISNULL(RTRIM(LTRIM(d.Description)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;  


--when update Status
BEGIN TRY
if update(Status)
insert into PatientPaymentAudit(PatientPaymentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Status',d.Status,i.Status,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Status,'') != ISNULL(d.Status,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;  


--when update AllocatedAmount
BEGIN TRY
if update(AllocatedAmount)
insert into PatientPaymentAudit(PatientPaymentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Allocated Amount',d.AllocatedAmount,i.AllocatedAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AllocatedAmount,'1') != ISNULL(d.AllocatedAmount,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;  


--when update RemainingAmount
BEGIN TRY
if update(RemainingAmount)
insert into PatientPaymentAudit(PatientPaymentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Remaining Amount',d.RemainingAmount,i.RemainingAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RemainingAmount,'1') != ISNULL(d.RemainingAmount,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Type
BEGIN TRY
if update(Type)
insert into PatientPaymentAudit(PatientPaymentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Type',d.Type,i.Type,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Type,'') != ISNULL(d.Type,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update VisitID
BEGIN TRY
if update(VisitID)
insert into PatientPaymentAudit(PatientPaymentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Visit ID',d.VisitID,i.VisitID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.VisitID,'1') != ISNULL(d.VisitID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update CCTransactionID
BEGIN TRY
if update(CCTransactionID)
insert into PatientPaymentAudit(PatientPaymentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'CC Transaction ID',d.CCTransactionID,i.CCTransactionID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CCTransactionID,'') != ISNULL(d.CCTransactionID,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
END
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
BEGIN
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM ReceiverAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM ReceiverAudit 

--when update Name
BEGIN TRY
if update(Name)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',(RTRIM(LTRIM(d.Name))),(RTRIM(LTRIM(i.Name))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Name)),'') != ISNULL(RTRIM(LTRIM(d.Name)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Address1
BEGIN TRY
if update(Address1)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address1',(RTRIM(LTRIM(d.Address1))),(RTRIM(LTRIM(i.Address1))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Address1)),'') != ISNULL(RTRIM(LTRIM(d.Address1)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Address2
BEGIN TRY
if update(Address2)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address2',(RTRIM(LTRIM(d.Address2))),(RTRIM(LTRIM(i.Address2))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Address2)),'') != ISNULL(RTRIM(LTRIM(d.Address2)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update PhoneNumber
BEGIN TRY
if update(PhoneNumber)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Phone Number',d.PhoneNumber,i.PhoneNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PhoneNumber,'') != ISNULL(d.PhoneNumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update FaxNumber
BEGIN TRY
if update(FaxNumber)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Fax Number',d.PhoneNumber,i.PhoneNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FaxNumber,'') != ISNULL(d.FaxNumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Email
BEGIN TRY
if update(Email)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Email',(RTRIM(LTRIM(d.Email))),(RTRIM(LTRIM(i.Email))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Email)),'') != ISNULL(RTRIM(LTRIM(d.Email)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Website
BEGIN TRY
if update(Website)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Website',(RTRIM(LTRIM(d.Website))),(RTRIM(LTRIM(i.Website))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Website)),'') != ISNULL(RTRIM(LTRIM(d.Website)),'')
 END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update City
BEGIN TRY
if update (City)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'City',(RTRIM(LTRIM(d.City))),(RTRIM(LTRIM(i.City))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
Inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.City)),'') != ISNULL(RTRIM(LTRIM(d.City)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update State
BEGIN TRY
if update(State)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'State',(RTRIM(LTRIM(d.State))),(RTRIM(LTRIM(i.State))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
Inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.State)),'') != ISNULL(RTRIM(LTRIM(d.State)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ZipCode
BEGIN TRY
if update(ZipCode)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Zip Code',d.ZipCode,i.ZipCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ZipCode,'') != ISNULL(d.ZipCode,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update SubmissionMethod
BEGIN TRY
if update(SubmissionMethod)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submission Method',d.SubmissionMethod,i.SubmissionMethod,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmissionMethod,'') != ISNULL(d.SubmissionMethod,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update SubmissionURL
BEGIN TRY
if update(SubmissionURL)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submission URL',(RTRIM(LTRIM(d.SubmissionURL))),(RTRIM(LTRIM(i.SubmissionURL))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.SubmissionURL)),'') != ISNULL(RTRIM(LTRIM(d.SubmissionURL)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update SubmissionPort
BEGIN TRY
if update(SubmissionPort)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submission Port',d.SubmissionPort,i.SubmissionPort,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmissionPort,'') != ISNULL(d.SubmissionPort,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update SubmissionDirectory
BEGIN TRY
if update(SubmissionDirectory)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submission Directory',d.SubmissionDirectory,i.SubmissionDirectory,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmissionDirectory,'') != ISNULL(d.SubmissionDirectory,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ReportsDirectory
BEGIN TRY
if update(ReportsDirectory)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reports Directory',d.ReportsDirectory,i.ReportsDirectory,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReportsDirectory,'') != ISNULL(d.ReportsDirectory,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ErasDirectory
BEGIN TRY
if update(ErasDirectory)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Eras Directory',d.ErasDirectory,i.ErasDirectory,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ErasDirectory,'') != ISNULL(d.ErasDirectory,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_837_NM1_40_ReceiverName
BEGIN TRY
if update(X12_837_NM1_40_ReceiverName)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_NM1_40_Receiver Name',(RTRIM(LTRIM(d.X12_837_NM1_40_ReceiverName))),(RTRIM(LTRIM(i.X12_837_NM1_40_ReceiverName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.X12_837_NM1_40_ReceiverName)),'') != ISNULL(RTRIM(LTRIM(d.X12_837_NM1_40_ReceiverName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_837_NM1_40_ReceiverID
BEGIN TRY
if update(X12_837_NM1_40_ReceiverID)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_NM1_40_Receiver ID',d.X12_837_NM1_40_ReceiverID,i.X12_837_NM1_40_ReceiverID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_837_NM1_40_ReceiverID,'') != ISNULL( d.X12_837_NM1_40_ReceiverID,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_837_ISA_01
BEGIN TRY
if update(X12_837_ISA_01)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_ISA_01',d.X12_837_ISA_01,i.X12_837_ISA_01,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_837_ISA_01,'') != ISNULL(d.X12_837_ISA_01,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_837_ISA_03
BEGIN TRY
if update(X12_837_ISA_03)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_ISA_03',d.X12_837_ISA_03,i.X12_837_ISA_03,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_837_ISA_03,'') != ISNULL(d.X12_837_ISA_03,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_837_ISA_05
BEGIN TRY
if update(X12_837_ISA_05)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_ISA_05',d.X12_837_ISA_05,i.X12_837_ISA_05,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_837_ISA_05,'') != ISNULL(d.X12_837_ISA_05,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_837_ISA_07
BEGIN TRY
if update(X12_837_ISA_07)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_ISA_07',d.X12_837_ISA_07,i.X12_837_ISA_07,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_837_ISA_07,'') != ISNULL(d.X12_837_ISA_07,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_837_ISA_08
BEGIN TRY
if update(X12_837_ISA_08)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_ISA_08',d.X12_837_ISA_08,i.X12_837_ISA_08,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_837_ISA_08,'') != ISNULL(d.X12_837_ISA_08,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_837_GS_03
BEGIN TRY
if update(X12_837_GS_03)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_837_GS_03',d.X12_837_GS_03,i.X12_837_GS_03,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_837_GS_03,'') != ISNULL(d.X12_837_GS_03,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_270_NM1_40_ReceiverName
BEGIN TRY
if update(X12_270_NM1_40_ReceiverName)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_NM1_40_Receiver Name',(RTRIM(LTRIM(d.X12_270_NM1_40_ReceiverName))),(RTRIM(LTRIM(i.X12_270_NM1_40_ReceiverName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.X12_270_NM1_40_ReceiverName)),'') != ISNULL(RTRIM(LTRIM(d.X12_270_NM1_40_ReceiverName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_270_NM1_40_ReceiverID
BEGIN TRY
if update(X12_270_NM1_40_ReceiverID)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_NM1_40_Receiver ID',d.X12_270_NM1_40_ReceiverID,i.X12_270_NM1_40_ReceiverID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_270_NM1_40_ReceiverID,'') != ISNULL(d.X12_270_NM1_40_ReceiverID,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_270_ISA_01
BEGIN TRY
if update(X12_270_ISA_01)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_ISA_01',d.X12_270_ISA_01,i.X12_270_ISA_01,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_270_ISA_01,'') != ISNULL(d.X12_270_ISA_01,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_270_ISA_03
BEGIN TRY
if update(X12_270_ISA_03)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_ISA_03',d.X12_270_ISA_03,i.X12_270_ISA_03,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_270_ISA_03,'') != ISNULL(d.X12_270_ISA_03,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_270_ISA_05
BEGIN TRY
if update(X12_270_ISA_05)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_ISA_05',d.X12_270_ISA_05,i.X12_270_ISA_05,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_270_ISA_05,'') != ISNULL(d.X12_270_ISA_05,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_270_ISA_07
BEGIN TRY
if update(X12_270_ISA_07)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_ISA_07',d.X12_270_ISA_07,i.X12_270_ISA_07,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_270_ISA_07,'') != ISNULL(d.X12_270_ISA_07,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_270_ISA_08
BEGIN TRY
if update(Name)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_ISA_08',d.X12_270_ISA_08,i.X12_270_ISA_08,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_270_ISA_08,'') != ISNULL(d.X12_270_ISA_08,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_270_GS_03
BEGIN TRY
if update(X12_270_GS_03)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_270_GS_03',d.X12_270_GS_03,i.X12_270_GS_03,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_270_GS_03,'') != ISNULL(d.X12_270_GS_03,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_276_NM1_40_ReceiverName
BEGIN TRY
if update(X12_276_NM1_40_ReceiverName)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_NM1_40_Receiver Name',(RTRIM(LTRIM(d.X12_276_NM1_40_ReceiverName))),(RTRIM(LTRIM(i.X12_276_NM1_40_ReceiverName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.X12_276_NM1_40_ReceiverName)),'') != ISNULL(RTRIM(LTRIM(d.X12_276_NM1_40_ReceiverName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_276_NM1_40_ReceiverID
BEGIN TRY
if update(X12_276_NM1_40_ReceiverID)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_NM1_40_Receiver ID',d.X12_276_NM1_40_ReceiverID,i.X12_276_NM1_40_ReceiverID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_276_NM1_40_ReceiverID,'') != ISNULL(d.X12_276_NM1_40_ReceiverID,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_276_ISA_01
BEGIN TRY
if update(X12_276_ISA_01)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_ISA_01',d.X12_276_ISA_01,i.X12_276_ISA_01,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_276_ISA_01,'') != ISNULL(d.X12_276_ISA_01,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_276_ISA_03
BEGIN TRY
if update(X12_276_ISA_03)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_ISA_03',d.X12_276_ISA_03,i.X12_276_ISA_03,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_276_ISA_03,'') != ISNULL(d.X12_276_ISA_03,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_276_ISA_05
BEGIN TRY
if update(X12_276_ISA_05)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_ISA_05',d.X12_276_ISA_05,i.X12_276_ISA_05,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_276_ISA_05,'') != ISNULL(d.X12_276_ISA_05,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_276_ISA_07
BEGIN TRY
if update(X12_276_ISA_07)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_ISA_07',d.X12_276_ISA_07,i.X12_276_ISA_07,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_276_ISA_07,'') != ISNULL(d.X12_276_ISA_07,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_276_ISA_08
BEGIN TRY
if update(X12_276_ISA_08)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_ISA_08',d.X12_276_ISA_08,i.X12_276_ISA_08,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_276_ISA_08,'') != ISNULL(d.X12_276_ISA_08,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update X12_276_GS_03
BEGIN TRY
if update(X12_276_GS_03)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'X12_276_GS_03',d.X12_276_GS_03,i.X12_276_GS_03,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.X12_276_GS_03,'') != ISNULL(d.X12_276_GS_03,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ElementSeperator
BEGIN TRY
if update(ElementSeperator)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Element Seperator',d.ElementSeperator,i.ElementSeperator,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ElementSeperator,'') != ISNULL(d.ElementSeperator,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update SegmentSeperator
BEGIN TRY
if update(SegmentSeperator)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Segment Seperator',d.SegmentSeperator,i.SegmentSeperator,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SegmentSeperator,'') != ISNULL(d.SegmentSeperator,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update SubElementSeperator
BEGIN TRY
if update(SubElementSeperator)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Sub Element Seperator',d.SubElementSeperator,i.SubElementSeperator,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubElementSeperator,'') != ISNULL(d.SubElementSeperator,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update RepetitionSepeator
BEGIN TRY
if update(RepetitionSepeator)
insert into ReceiverAudit(ReceiverID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Repetition Sepeator',d.RepetitionSepeator,i.RepetitionSepeator,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RepetitionSepeator,'') != ISNULL(d.RepetitionSepeator,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
END
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
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM PaymentCheckAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM PaymentCheckAudit

--When User Update ReceiverID
BEGIN TRY
IF Update(ReceiverID)
--select * Into #t1 from Receiver
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Receiver ID',pDel.[Name],pUpd.[Name],d.ReceiverID,i.ReceiverID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ReceiverID,'1') != ISNULL(d.ReceiverID,'1')
Join Receiver as pDel on pdel.ID = ISNULL(d.ReceiverID,'1')
join Receiver as pUpd on pUpd.ID = ISNULL(i.ReceiverID,'1')
END TRY  
BEGIN CATCH  
 --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update PracticeID
BEGIN TRY
IF Update(PracticeID)
--select * Into #t2 from Practice
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Practice ID',pDel.[Name],pUpd.[Name],d.PracticeID,i.PracticeID,HOST_NAME(),i.UpdatedBy  ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PracticeID,'1') != ISNULL(d.PracticeID,'1')
Join Practice as pDel on pdel.ID = ISNULL(d.PracticeID,'1')
join Practice as pUpd on pUpd.ID = ISNULL(i.PracticeID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update DownloadedFileID
BEGIN TRY
IF Update(DownloadedFileID)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Downloaded File ID',d.DownloadedFileID,i.DownloadedFileID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.DownloadedFileID,'') != ISNULL(d.DownloadedFileID,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update CheckNumber
BEGIN TRY
IF Update(CheckNumber)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Check Number',d.CheckNumber,i.CheckNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.CheckNumber,'') != ISNULL(d.CheckNumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update CheckDate
BEGIN TRY
IF Update(CheckDate)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Check Date',d.CheckDate,i.CheckDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(cast(i.CheckDate as datetime),'') != ISNULL(cast(d.CheckDate as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update CheckAmount
BEGIN TRY
IF Update(CheckAmount)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Check Amount',d.CheckAmount,i.CheckAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.CheckAmount,'1') != ISNULL(d.CheckAmount,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update TransactionCode
BEGIN TRY
IF Update(TransactionCode)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Transaction Code',d.TransactionCode,i.TransactionCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.TransactionCode,'') != ISNULL(d.TransactionCode,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update CreditDebitFlag
BEGIN TRY
IF Update(CreditDebitFlag)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Credit Debit Flag',d.CreditDebitFlag,i.CreditDebitFlag,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.CreditDebitFlag,'') != ISNULL(d.CreditDebitFlag,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update PaymentMethod
BEGIN TRY
IF Update(PaymentMethod)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payment Method',d.PaymentMethod,i.PaymentMethod,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PaymentMethod,'') != ISNULL(d.PaymentMethod,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update PayerName
BEGIN TRY
IF Update(PayerName)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payer Name',(RTRIM(LTRIM(d.PayerName))),(RTRIM(LTRIM(i.PayerName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PayerName)),'') != ISNULL(RTRIM(LTRIM(d.PayerName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update Status
BEGIN TRY
IF Update(Status)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Status',d.Status,i.Status,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Status,'') != ISNULL(d.Status,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update AppliedAmount
BEGIN TRY
IF Update(AppliedAmount)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Applied Amount',d.AppliedAmount,i.AppliedAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.AppliedAmount,'1') != ISNULL(d.AppliedAmount,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update PostedAmount
BEGIN TRY
IF Update(PostedAmount)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Posted Amount',d.PostedAmount,i.PostedAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PostedAmount,'1') != ISNULL(d.PostedAmount,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update PostedDate
BEGIN TRY
IF Update(PostedDate)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Posted Date',d.PostedDate,i.PostedDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(cast(i.PostedDate as datetime),'') != ISNULL(cast(d.PostedDate as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update PostedBy
BEGIN TRY
IF Update(PostedBy)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Posted By',d.PostedBy,i.PostedBy,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PostedBy,'') != ISNULL(d.PostedBy,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update PayerID
BEGIN TRY
IF Update(PayerID)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payer ID',d.PayerID,i.PayerID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PayerID,'') != ISNULL(d.PayerID,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update PayerAddress
BEGIN TRY
IF Update(PayerAddress)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payer Address',(RTRIM(LTRIM(d.PayerAddress))),(RTRIM(LTRIM(i.PayerAddress))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PayerAddress)),'') != ISNULL(RTRIM(LTRIM(d.PayerAddress)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update PayerCity
BEGIN TRY
IF Update(PayerCity)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payer City',(RTRIM(LTRIM(d.PayerCity))),(RTRIM(LTRIM(i.PayerCity))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PayerCity)),'') != ISNULL(RTRIM(LTRIM(d.PayerCity)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update PayerState
BEGIN TRY
IF Update(PayerState)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payer State',d.PayerState,i.PayerState,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PayerState,'') != ISNULL(d.PayerState,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update PayerZipCode
BEGIN TRY
IF Update(PayerZipCode)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payer Zip Code',d.PayerZipCode,i.PayerZipCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PayerZipCode,'') != ISNULL(d.PayerZipCode,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update REF_2U_ID
BEGIN TRY
IF Update(REF_2U_ID)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'REF_2U_ID',d.REF_2U_ID,i.REF_2U_ID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.REF_2U_ID,'') != ISNULL(d.REF_2U_ID,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update PayerContactPerson
BEGIN TRY
IF Update(PayerContactPerson)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payer Contact Person',d.PayerContactPerson,i.PayerContactPerson,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PayerContactPerson,'') != ISNULL(d.PayerContactPerson,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update PayerContactNumber
BEGIN TRY
IF Update(PayerContactNumber)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payer Contact Number',d.PayerContactNumber,i.PayerContactNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PayerContactNumber,'') != ISNULL(d.PayerContactNumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update PayeeName
BEGIN TRY
IF Update(PayeeName)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payee Name',(RTRIM(LTRIM(d.PayeeName))),(RTRIM(LTRIM(i.PayeeName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PayeeName)),'') != ISNULL(RTRIM(LTRIM(d.PayeeName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update PayeeNPI
BEGIN TRY
IF Update(PayeeNPI)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payee NPI',d.PayeeNPI,i.PayeeNPI,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PayeeNPI,'') != ISNULL(d.PayeeNPI,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update PayeeAddress
BEGIN TRY
IF Update(PayeeAddress)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payee Address',(RTRIM(LTRIM(d.PayeeAddress))),(RTRIM(LTRIM(i.PayeeAddress))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PayeeAddress)),'') != ISNULL(RTRIM(LTRIM(d.PayeeAddress)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update PayeeCity
BEGIN TRY
IF Update(PayeeCity)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payee City',(RTRIM(LTRIM(d.PayeeCity))),(RTRIM(LTRIM(i.PayeeCity))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PayeeCity)),'') != ISNULL(RTRIM(LTRIM(d.PayeeCity)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update PayeeState
BEGIN TRY
IF Update(PayeeState)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payee State',d.PayeeState,i.PayeeState,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PayeeState,'') != ISNULL(d.PayeeState,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update PayeeZipCode
BEGIN TRY
IF Update(PayeeZipCode)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payee Zip Code',d.PayeeZipCode,i.PayeeZipCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PayeeZipCode,'') != ISNULL(d.PayeeZipCode,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update PayeeTaxID
BEGIN TRY
IF Update(PayeeTaxID)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Payee Tax ID',d.PayeeTaxID,i.PayeeTaxID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.PayeeTaxID,'') != ISNULL(d.PayeeTaxID,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update NumberOfVisits
BEGIN TRY
IF Update(NumberOfVisits)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Number Of Visits',d.NumberOfVisits,i.NumberOfVisits,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.NumberOfVisits,'') != ISNULL(d.NumberOfVisits,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update NumberOfPatients
BEGIN TRY
IF Update(NumberOfPatients)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Number Of Patients',d.NumberOfPatients,i.NumberOfPatients,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.NumberOfPatients,'') != ISNULL(d.NumberOfPatients,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update Comments
BEGIN TRY
IF Update(Comments)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Comments',(RTRIM(LTRIM(d.Comments))),(RTRIM(LTRIM(i.Comments))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Comments)),'') != ISNULL(RTRIM(LTRIM(d.Comments)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update UnAppliedAmount
BEGIN TRY
IF Update(UnAppliedAmount)
INSERT INTO PaymentCheckAudit(PaymentCheckID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'UnApplied Amount',d.UnAppliedAmount,i.UnAppliedAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.UnAppliedAmount,'1') != ISNULL(d.UnAppliedAmount,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
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
BEGIN
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM PaymentChargeAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM PaymentChargeAudit 

--when update PaymentVisitID
BEGIN TRY
if update(PaymentVisitID)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payment Visit ID',d.PaymentVisitID,i.PaymentVisitID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaymentVisitID,'1') != ISNULL(d.PaymentVisitID,'1')
--Join PaymentVisit as cDel on cDel.ID = ISNULL(d.PaymentVisitID,'')
--join PaymentVisit as cUpd on cUpd.ID = ISNULL(i.PaymentVisitID,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ChargeID
BEGIN TRY
if update(ChargeID)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Charge ID',d.ChargeID,i.ChargeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ChargeID,'1') != ISNULL(d.ChargeID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update AppliedToSec
BEGIN TRY
if update(AppliedToSec)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Applied To Sec',(case when d.AppliedToSec = 1 then 'Yes' else 'No' end),(case when i.AppliedToSec = 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AppliedToSec,'') != ISNULL(d.AppliedToSec,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update CPTCode
BEGIN TRY
if update(CPTCode)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'CPT Code',d.CPTCode,i.CPTCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CPTCode,'') != ISNULL(d.CPTCode,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
--when update Modifier1
BEGIN TRY
if update(Modifier1)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Modifier 1',d.Modifier1,i.Modifier1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Modifier1,'') != ISNULL(d.Modifier1,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Modifier2
BEGIN TRY
if update(Modifier2)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Modifier 2',d.Modifier2,i.Modifier2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Modifier2,'') != ISNULL(d.Modifier2,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Modifier3
BEGIN TRY
if update(Modifier3)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Modifier 3',d.Modifier3,i.Modifier3,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Modifier3,'') != ISNULL(d.Modifier3,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Modifier4
BEGIN TRY
if update(Modifier4)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Modifier 4',d.Modifier4,i.Modifier4,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Modifier4,'') != ISNULL(d.Modifier4,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update BilledAmount
BEGIN TRY
if update(BilledAmount)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Billed Amount',d.BilledAmount,i.BilledAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.BilledAmount,'1') != ISNULL(d.BilledAmount,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update PaidAmount
BEGIN TRY
if update(PaidAmount)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Paid Amount',d.PaidAmount,i.PaidAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaidAmount,'1') != ISNULL(d.PaidAmount,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update RevenueCode
BEGIN TRY
if update(RevenueCode)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Revenue Code',d.RevenueCode,i.RevenueCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RevenueCode,'') != ISNULL(d.RevenueCode,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Units
BEGIN TRY
if update(Units)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Units',d.Units,i.Units,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Units,'') != ISNULL(d.Units,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update DOSFrom
BEGIN TRY
if update(DOSFrom)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'DOS From',d.DOSFrom,i.DOSFrom,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.DOSFrom as datetime),'') != ISNULL(cast(d.DOSFrom as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update DOSTo
BEGIN TRY
if update(DOSTo)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'DOS To',d.DOSTo,i.DOSTo,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.DOSTo as datetime),'') != ISNULL(cast(d.DOSTo as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ChargeControlNumber
BEGIN TRY
if update(ChargeControlNumber)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Charge Control Number',d.ChargeControlNumber,i.ChargeControlNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ChargeControlNumber,'') != ISNULL(d.ChargeControlNumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Copay
BEGIN TRY
if update(Copay)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Copay',d.Copay,i.Copay,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Copay,'1') != ISNULL(d.Copay,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update PatientAmount
BEGIN TRY
if update(PatientAmount)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Amount',d.PatientAmount,i.PatientAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientAmount,'1') != ISNULL(d.PatientAmount,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update DeductableAmount
BEGIN TRY
if update(DeductableAmount)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Deductable Amount',d.DeductableAmount,i.DeductableAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DeductableAmount,'1') != ISNULL(d.DeductableAmount,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update CoinsuranceAmount
BEGIN TRY
if update(CoinsuranceAmount)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Coinsurance Amount',d.CoinsuranceAmount,i.CoinsuranceAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CoinsuranceAmount,'1') != ISNULL(d.CoinsuranceAmount,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update WriteoffAmount
BEGIN TRY
if update(WriteoffAmount)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Write off Amount',d.WriteoffAmount,i.WriteoffAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.WriteoffAmount,'1') != ISNULL(d.WriteoffAmount,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update AllowedAmount
BEGIN TRY
if update(AllowedAmount)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Allowed Amount',d.AllowedAmount,i.AllowedAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AllowedAmount,'1') != ISNULL(d.AllowedAmount,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update AdjustmentCodeID1
BEGIN TRY
if update(AdjustmentCodeID1)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment CodeID1',cDel.[Description],cUpd.[Description],d.AdjustmentCodeID1,i.AdjustmentCodeID1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentCodeID1,'1') != ISNULL(d.AdjustmentCodeID1,'1')
Join AdjustmentCode as cDel on cDel.ID = ISNULL(d.AdjustmentCodeID1,'1')
join AdjustmentCode as cUpd on cUpd.ID = ISNULL(i.AdjustmentCodeID1,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update GroupCode1
BEGIN TRY
if update(GroupCode1)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Group Code1',d.GroupCode1,i.GroupCode1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.GroupCode1,'') != ISNULL(d.GroupCode1,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update AdjustmentAmount1
BEGIN TRY
if update(AdjustmentAmount1)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Amount1 ',d.AdjustmentAmount1,i.AdjustmentAmount1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentAmount1,'1') != ISNULL(d.AdjustmentAmount1,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update AdjustmentQuantity1
BEGIN TRY
if update(AdjustmentQuantity1)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Quantity1',d.AdjustmentQuantity1,i.AdjustmentQuantity1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentQuantity1,'') != ISNULL(d.AdjustmentQuantity1,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update AdjustmentCodeID2
BEGIN TRY
if update(AdjustmentCodeID2)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment CodeID2',cDel.[Description],cUpd.[Description],d.AdjustmentCodeID2,i.AdjustmentCodeID2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentCodeID2,'1') != ISNULL(d.AdjustmentCodeID2,'1')
Join AdjustmentCode as cDel on cDel.ID = ISNULL(d.AdjustmentCodeID2,'1')
join AdjustmentCode as cUpd on cUpd.ID = ISNULL(i.AdjustmentCodeID2,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update GroupCode2
BEGIN TRY
if update(GroupCode2)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Group Code2',d.GroupCode2,i.GroupCode2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.GroupCode2,'') != ISNULL(d.GroupCode2,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update AdjustmentAmount2
BEGIN TRY
if update(AdjustmentAmount2)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Amount2',d.AdjustmentAmount2,i.AdjustmentAmount2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentAmount2,'1') != ISNULL(d.AdjustmentAmount2,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update AdjustmentQuantity2
BEGIN TRY
if update(AdjustmentQuantity2)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Quantity2',d.AdjustmentQuantity2,i.AdjustmentQuantity2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentQuantity2,'') != ISNULL(d.AdjustmentQuantity2,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update AdjustmentCodeID3
BEGIN TRY
if update(AdjustmentCodeID3)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment CodeID3',cDel.[Description],cUpd.[Description],d.AdjustmentCodeID3,i.AdjustmentCodeID3,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentCodeID3,'1') != ISNULL(d.AdjustmentCodeID3,'1')
Join AdjustmentCode as cDel on cDel.ID = ISNULL(d.AdjustmentCodeID3,'1')
join AdjustmentCode as cUpd on cUpd.ID = ISNULL(i.AdjustmentCodeID3,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update GroupCode3
BEGIN TRY
if update(GroupCode3)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Group Code',d.GroupCode3,i.GroupCode3,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.GroupCode3,'') != ISNULL(d.GroupCode3,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update AdjustmentAmount3
BEGIN TRY
if update(AdjustmentAmount3)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Amount3',d.AdjustmentAmount3,i.AdjustmentAmount3,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentAmount3,'1') != ISNULL(d.AdjustmentAmount3,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update AdjustmentQuantity3
BEGIN TRY
if update(AdjustmentQuantity3)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Quantity3',d.AdjustmentQuantity3,i.AdjustmentQuantity3,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentQuantity3,'') != ISNULL(d.AdjustmentQuantity3,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update AdjustmentCodeID4
BEGIN TRY
if update(AdjustmentCodeID4)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment CodeID4',cDel.[Description],cUpd.[Description],d.AdjustmentCodeID4,i.AdjustmentCodeID4,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentCodeID4,'1') != ISNULL(d.AdjustmentCodeID4,'1')
Join AdjustmentCode as cDel on cDel.ID = ISNULL(d.AdjustmentCodeID4,'1')
join AdjustmentCode as cUpd on cUpd.ID = ISNULL(i.AdjustmentCodeID4,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update GroupCode4
BEGIN TRY
if update(GroupCode4)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Group Code4',d.GroupCode4,i.GroupCode4,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.GroupCode4,'') != ISNULL(d.GroupCode4,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update AdjustmentAmount4
BEGIN TRY
if update(AdjustmentAmount4)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Amount4',d.AdjustmentAmount4,i.AdjustmentAmount4,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentAmount4,'1') != ISNULL(d.AdjustmentAmount4,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update AdjustmentQuantity4
BEGIN TRY
if update(AdjustmentQuantity4)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Quantity4',d.AdjustmentQuantity4,i.AdjustmentQuantity4,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentQuantity4,'') != ISNULL(d.AdjustmentQuantity4,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update AdjustmentCodeID5
BEGIN TRY
if update(AdjustmentCodeID5)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment CodeID5',cDel.[Description],cUpd.[Description],d.AdjustmentCodeID5,i.AdjustmentCodeID5,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentCodeID5,'1') != ISNULL(d.AdjustmentCodeID5,'1')
Join AdjustmentCode as cDel on cDel.ID = ISNULL(d.AdjustmentCodeID5,'1')
join AdjustmentCode as cUpd on cUpd.ID = ISNULL(i.AdjustmentCodeID5,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update GroupCode5
BEGIN TRY
if update(GroupCode5)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Group Code5',d.GroupCode5,i.GroupCode5,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.GroupCode5,'') != ISNULL(d.GroupCode5,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update AdjustmentAmount5
BEGIN TRY
if update(AdjustmentAmount5)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Amount5',d.AdjustmentAmount5,i.AdjustmentAmount5,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentAmount5,'1') != ISNULL(d.AdjustmentAmount5,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update AdjustmentQuantity5
BEGIN TRY
if update(AdjustmentQuantity5)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Quantity5',d.AdjustmentQuantity5,i.AdjustmentQuantity5,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentQuantity5,'') != ISNULL(d.AdjustmentQuantity5,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update RemarkCodeID1
BEGIN TRY
if update(RemarkCodeID1)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Remark Code ID1',cDel.[Description],cUpd.[Description],d.RemarkCodeID1,i.RemarkCodeID1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RemarkCodeID1,'1') != ISNULL(d.RemarkCodeID1,'1')
Join RemarkCode as cDel on cDel.ID = ISNULL(d.RemarkCodeID1,'1')
join RemarkCode as cUpd on cUpd.ID = ISNULL(i.RemarkCodeID1,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update RemarkCodeID2
BEGIN TRY
if update(RemarkCodeID2)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Remark Code ID2',cDel.[Description],cUpd.[Description],d.RemarkCodeID2,i.RemarkCodeID2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RemarkCodeID2,'1') != ISNULL(d.RemarkCodeID2,'1')
Join RemarkCode as cDel on cDel.ID = ISNULL(d.RemarkCodeID2,'1')
join RemarkCode as cUpd on cUpd.ID = ISNULL(i.RemarkCodeID2,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update RemarkCodeID3
BEGIN TRY
if update(RemarkCodeID3)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Remark Code ID3',cDel.[Description],cUpd.[Description],d.RemarkCodeID3,i.RemarkCodeID3,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RemarkCodeID3,'1') != ISNULL(d.RemarkCodeID3,'1')
Join RemarkCode as cDel on cDel.ID = ISNULL(d.RemarkCodeID3,'1')
join RemarkCode as cUpd on cUpd.ID = ISNULL(i.RemarkCodeID3,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update RemarkCodeID4
BEGIN TRY
if update(RemarkCodeID4)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Remark Code ID4',cDel.[Description],cUpd.[Description],d.RemarkCodeID4,i.RemarkCodeID4,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RemarkCodeID4,'1') != ISNULL(d.RemarkCodeID4,'1')
Join RemarkCode as cDel on cDel.ID = ISNULL(d.RemarkCodeID4,'1')
join RemarkCode as cUpd on cUpd.ID = ISNULL(i.RemarkCodeID4,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update RemarkCodeID5
BEGIN TRY
if update(RemarkCodeID5)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Remark Code ID5',cDel.[Description],cUpd.[Description],d.RemarkCodeID5,i.RemarkCodeID5,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RemarkCodeID5,'1') != ISNULL(d.RemarkCodeID5,'1')
Join RemarkCode as cDel on cDel.ID = ISNULL(d.RemarkCodeID5,'1')
join RemarkCode as cUpd on cUpd.ID = ISNULL(i.RemarkCodeID5,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Status
BEGIN TRY
if update(Status)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Status',d.Status,i.Status,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Status,'') != ISNULL(d.Status,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update OtherPatResp
BEGIN TRY
if update(OtherPatResp)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Other Pat Resp',d.OtherPatResp,i.OtherPatResp,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.OtherPatResp,'1') != ISNULL(d.OtherPatResp,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update PostedDate
BEGIN TRY
if update(PostedDate)
insert into PaymentChargeAudit(PaymentChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Posted Date',d.PostedDate,i.PostedDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.PostedDate as datetime),'') != ISNULL(cast(d.PostedDate as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage; 
END CATCH; 

END
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
BEGIN
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM PaymentLedgerAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM PaymentLedgerAudit 


--when update PatientPlanID
BEGIN TRY
if update(PatientPlanID)
--Select * into #t1 from PatientPlan
insert into PaymentLedgerAudit(PaymentLedgerID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Plan ID',cDel.[FirstName],cUpd.[FirstName],d.PatientPlanID,i.PatientPlanID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientPlanID,'1') != ISNULL(d.PatientPlanID,'1')
Join PatientPlan as cDel on cDel.ID = ISNULL(d.PatientPlanID,'1')
join PatientPlan as cUpd on cUpd.ID = ISNULL(i.PatientPlanID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ChargeID
BEGIN TRY
if update(ChargeID)
insert into PaymentLedgerAudit(PaymentLedgerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Charge ID',d.ChargeID,i.ChargeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ChargeID,'1') != ISNULL(d.ChargeID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update VisitID
BEGIN TRY
if update(VisitID)
insert into PaymentLedgerAudit(PaymentLedgerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Visit ID',d.VisitID,i.VisitID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.VisitID,'1') != ISNULL(d.VisitID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update PatientPaymentChargeID
BEGIN TRY
if update(PatientPaymentChargeID)
insert into PaymentLedgerAudit(PaymentLedgerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Payment Charge ID',d.PatientPaymentChargeID,i.PatientPaymentChargeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientPaymentChargeID,'1') != ISNULL(d.PatientPaymentChargeID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update PaymentChargeID
BEGIN TRY
if update(PaymentChargeID)
insert into PaymentLedgerAudit(PaymentLedgerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payment Charge ID',d.PaymentChargeID,i.PaymentChargeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaymentChargeID,'1') != ISNULL(d.PaymentChargeID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update AdjustmentCodeID
BEGIN TRY
if update(AdjustmentCodeID)
insert into PaymentLedgerAudit(PaymentLedgerID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment CodeID',cDel.[Description],cUpd.[Description],d.AdjustmentCodeID,i.AdjustmentCodeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentCodeID,'1') != ISNULL(d.AdjustmentCodeID,'1')
Join AdjustmentCode as cDel on cDel.ID = ISNULL(d.AdjustmentCodeID,'1')
join AdjustmentCode as cUpd on cUpd.ID = ISNULL(i.AdjustmentCodeID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update LedgerDate
BEGIN TRY
if update(LedgerDate)
insert into PaymentLedgerAudit(PaymentLedgerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Ledger Date',d.LedgerDate,i.LedgerDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.LedgerDate as date),'') != ISNULL(cast(d.LedgerDate as date),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update LedgerBy
BEGIN TRY
if update(LedgerBy)
insert into PaymentLedgerAudit(PaymentLedgerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Ledger By',d.LedgerBy,i.LedgerBy,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.LedgerBy,'') != ISNULL(d.LedgerBy,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update LedgerType
BEGIN TRY
if update(LedgerType)
insert into PaymentLedgerAudit(PaymentLedgerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Ledger Type',d.LedgerType,i.LedgerType,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.LedgerType,'') != ISNULL(d.LedgerType,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update LedgerDescription
BEGIN TRY
if update(LedgerDescription)
insert into PaymentLedgerAudit(PaymentLedgerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Ledger Description',(RTRIM(LTRIM(d.LedgerDescription))),(RTRIM(LTRIM(i.LedgerDescription))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.LedgerDescription)),'') != ISNULL(RTRIM(LTRIM(d.LedgerDescription)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Amount
BEGIN TRY
if update(Amount)
insert into PaymentLedgerAudit(PaymentLedgerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Amount',d.Amount,i.Amount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Amount,'1') != ISNULL(d.Amount,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Notes
BEGIN TRY
if update(Notes)
insert into PaymentLedgerAudit(PaymentLedgerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',(RTRIM(LTRIM(d.Notes))),(RTRIM(LTRIM(i.Notes))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Notes)),'') != ISNULL(RTRIM(LTRIM(d.Notes)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
END
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
BEGIN
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM PaymentVisitAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM PaymentVisitAudit 

--when update PaymentCheckID
BEGIN TRY
if update(PaymentCheckID)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payment Check ID',d.PaymentCheckID,i.PaymentCheckID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaymentCheckID,'1') != ISNULL(d.PaymentCheckID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update VisitID
BEGIN TRY
if update(VisitID)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Visit ID',d.VisitID,i.VisitID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.VisitID,'1') != ISNULL(d.VisitID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ClaimNumber
BEGIN TRY
if update(ClaimNumber)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Claim Number',d.ClaimNumber,i.ClaimNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClaimNumber,'') != ISNULL(d.ClaimNumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update PatientID
BEGIN TRY
if update(PatientID)
--Select * into #t1 from Patient
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient ID',cDel.[FirstName],cUpd.[FirstName],d.PatientID,i.PatientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientID,'1') != ISNULL(d.PatientID,'1')
Join Patient as cDel on cDel.ID = ISNULL(d.PatientID,'1')
join Patient as cUpd on cUpd.ID = ISNULL(i.PatientID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update BatchDocumentID
BEGIN TRY
if update(BatchDocumentID)
--Select * into #t2 from Patient
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Batch Document ID',cDel.[Description],cUpd.[Description],d.BatchDocumentID,i.BatchDocumentID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.BatchDocumentID,'1') != ISNULL(d.BatchDocumentID,'1')
Join BatchDocument as cDel on cDel.ID = ISNULL(d.BatchDocumentID,'1')
join BatchDocument as cUpd on cUpd.ID = ISNULL(i.BatchDocumentID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update PageNumber
BEGIN TRY
if update(PageNumber)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Page Number',d.PageNumber,i.PageNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PageNumber,'') != ISNULL(d.PageNumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update MyProperty
BEGIN TRY
if update(MyProperty)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'My Property',d.MyProperty,i.MyProperty,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.MyProperty,'') != ISNULL(d.MyProperty,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ProcessedAs
BEGIN TRY
if update(ProcessedAs)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Processed As',d.ProcessedAs,i.ProcessedAs,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProcessedAs,'') != ISNULL(d.ProcessedAs,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update BilledAmount
BEGIN TRY
if update(BilledAmount)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Billed Amount',d.BilledAmount,i.BilledAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.BilledAmount,'1') != ISNULL(d.BilledAmount,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update PaidAmount
BEGIN TRY
if update(PaidAmount)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Paid Amount',d.PaidAmount,i.PaidAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaidAmount,'1') != ISNULL(d.PaidAmount,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update AllowedAmount
BEGIN TRY
if update(AllowedAmount)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Allowed Amount',d.AllowedAmount,i.AllowedAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AllowedAmount,'1') != ISNULL(d.AllowedAmount,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update WriteOffAmount
BEGIN TRY
if update(WriteOffAmount)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Write Off Amount',d.WriteOffAmount,i.WriteOffAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.WriteOffAmount,'1') != ISNULL(d.WriteOffAmount,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update PatientAmount
BEGIN TRY
if update(PatientAmount)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Amount',d.PatientAmount,i.PatientAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientAmount,'1') != ISNULL(d.PatientAmount,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update PayerICN
BEGIN TRY
if update(PayerICN)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer ICN',d.PayerICN,i.PayerICN,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PayerICN,'') != ISNULL(d.PayerICN,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update PatientLastName
BEGIN TRY
if update(PatientLastName)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Last Name',d.PatientLastName,i.PatientLastName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientLastName,'') != ISNULL(d.PatientLastName,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update PatientFIrstName
BEGIN TRY
if update(PatientFIrstName)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient First Name',d.PatientFIrstName,i.PatientFIrstName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientFIrstName,'') != ISNULL(d.PatientFIrstName,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update InsuredLastName
BEGIN TRY
if update(InsuredLastName)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insured Last Name',d.InsuredLastName,i.InsuredLastName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsuredLastName,'') != ISNULL(d.InsuredLastName,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update InsuredFirstName
BEGIN TRY
if update(InsuredFirstName)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insured First Name',d.InsuredFirstName,i.InsuredFirstName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsuredFirstName,'') != ISNULL(d.InsuredFirstName,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update InsuredID
BEGIN TRY
if update(InsuredID)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insured ID',d.InsuredID,i.InsuredID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsuredID,'') != ISNULL(d.InsuredID,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ProvLastName
BEGIN TRY
if update(ProvLastName)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Provider Last Name',d.ProvLastName,i.ProvLastName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProvLastName,'') != ISNULL(d.ProvLastName,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ProvFirstName
BEGIN TRY
if update(ProvFirstName)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Provider First Name',d.ProvFirstName,i.ProvFirstName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProvFirstName,'') != ISNULL(d.ProvFirstName,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ProvNPI
BEGIN TRY
if update(ProvNPI)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Provider NPI',d.ProvNPI,i.ProvNPI,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProvNPI,'') != ISNULL(d.ProvNPI,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update PayerContactNumber
BEGIN TRY
if update(PayerContactNumber)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer Contact Number',d.PayerContactNumber,i.PayerContactNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PayerContactNumber,'') != ISNULL(d.PayerContactNumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ForwardedPayerName
BEGIN TRY
if update(ForwardedPayerName)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Forwarded Payer Name',d.ForwardedPayerName,i.ForwardedPayerName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ForwardedPayerName,'') != ISNULL(d.ForwardedPayerName,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ForwardedPayerID
BEGIN TRY
if update(ForwardedPayerID)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Forwarded Payer ID',d.ForwardedPayerID,i.ForwardedPayerID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ForwardedPayerID,'') != ISNULL(d.ForwardedPayerID,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ClaimStatementFromDate
BEGIN TRY
if update(ClaimStatementFromDate)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Claim Statement From Date',d.ClaimStatementFromDate,i.ClaimStatementFromDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.ClaimStatementFromDate as datetime),'') != ISNULL(cast(d.ClaimStatementFromDate as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ClaimStatementToDate
BEGIN TRY
if update(ClaimStatementToDate)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Claim Statement To Date',d.ClaimStatementToDate,i.ClaimStatementToDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.ClaimStatementToDate as datetime),'') != ISNULL(cast(d.ClaimStatementToDate as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update PayerReceivedDate
BEGIN TRY
if update(PayerReceivedDate)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer Received Date',d.PayerReceivedDate,i.PayerReceivedDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.PayerReceivedDate as datetime),'') != ISNULL(cast(d.PayerReceivedDate as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Status
BEGIN TRY
if update(Status)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Status',d.Status,i.Status,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Status,'') != ISNULL(d.Status,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update PostedDate
BEGIN TRY
if update(PostedDate)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'PostedDate',d.PostedDate,i.PostedDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.PostedDate as datetime),'') != ISNULL(cast(d.PostedDate as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Comments
BEGIN TRY
if update(Comments)
insert into PaymentVisitAudit(PaymentVisitID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Comments',(RTRIM(LTRIM(d.Comments))),(RTRIM(LTRIM(i.Comments))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Comments)),'') != ISNULL(RTRIM(LTRIM(d.Comments)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
END
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
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM ProviderScheduleAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM ProviderScheduleAudit 

--when update ProviderID
BEGIN TRY
if update(ProviderID)
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Provider ID',cDel.[Name],cUpd.[Name],d.ProviderID,i.ProviderID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProviderID,'1') != ISNULL(d.ProviderID,'1')
Join Provider as cDel on cDel.ID = ISNULL(d.ProviderID,'1')
join Provider as cUpd on cUpd.ID = ISNULL(i.ProviderID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update LocationID
BEGIN TRY
if update(LocationID)
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Location ID',cDel.[Name],cUpd.[Name],d.LocationID,i.LocationID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.LocationID,'1') != ISNULL(d.LocationID,'1')
Join Location as cDel on cDel.ID = ISNULL(d.LocationID,'1')
join Location as cUpd on cUpd.ID = ISNULL(i.LocationID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update FromTime
BEGIN TRY
if update(FromTime)
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'From Time',d.FromTime,i.FromTime,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.FromTime as datetime),'') != ISNULL(cast(d.FromTime as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ToTime
BEGIN TRY
if update(ToTime)
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'To Time',d.ToTime,i.ToTime,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.ToTime as datetime),'') != ISnull(cast(d.ToTime as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update TimeInterval
BEGIN TRY
if update(TimeInterval)
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Time Interval',d.TimeInterval,i.TimeInterval,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and i.TimeInterval != d.TimeInterval
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update OverBookAllowed
BEGIN TRY
if update(OverBookAllowed)
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Over Book Allowed',(case when d.OverBookAllowed= 1 then 'Yes' when d.OverBookAllowed =0 then 'No' end),(case when i.OverBookAllowed= 1 then 'Yes' when i.OverBookAllowed =0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.OverBookAllowed,'2') != ISNULL(d.OverBookAllowed,'2')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

----when update Friday
--BEGIN TRY
--if update(Friday)
--insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
--select i.ID, @TransactionID,'Friday',(case when d.Friday= 1 then 'Yes' when d.Friday =0 then 'No' end),(case when i.Friday= 1 then 'Yes' when i.Friday =0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
--from inserted as i
--inner join deleted as d on i.ID = d.ID and ISNULL(i.Friday,'2') != ISNULL(d.Friday,'2')
--END TRY  
--BEGIN CATCH  
--    --SELECT   
--     print ERROR_NUMBER(); --AS ErrorNumber  
--     print ERROR_MESSAGE(); --AS ErrorMessage;  
--END CATCH; 

----when update Saturday
--BEGIN TRY
--if update(Saturday)
--insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
--select i.ID, @TransactionID,'Saturday',(case when d.Saturday= 1 then 'Yes' when d.Saturday =0 then 'No' end),(case when i.Saturday= 1 then 'Yes' when i.Saturday =0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
--from inserted as i
--inner join deleted as d on i.ID = d.ID and ISNULL(i.Saturday,'2') != ISNULL(d.Saturday,'2')
--END TRY  
--BEGIN CATCH  
--    --SELECT   
--     print ERROR_NUMBER(); --AS ErrorNumber  
--     print ERROR_MESSAGE(); --AS ErrorMessage;  
--END CATCH; 

----when update Sunday
--BEGIN TRY
--if update(Sunday)
--insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
--select i.ID, @TransactionID,'Sunday',(case when d.Sunday= 1 then 'Yes' when d.Sunday =0 then 'No' end),(case when i.Sunday= 1 then 'Yes' when i.Sunday =0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
--from inserted as i
--inner join deleted as d on i.ID = d.ID and ISNULL(i.Sunday,'2') != ISNULL(d.Sunday,'2')
--END TRY  
--BEGIN CATCH  
--    --SELECT   
--     print ERROR_NUMBER(); --AS ErrorNumber  
--     print ERROR_MESSAGE(); --AS ErrorMessage;  
--END CATCH;

----when update Monday
--BEGIN TRY
--if update(Monday)
--insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
--select i.ID, @TransactionID,'Monday',(case when d.Monday= 1 then 'Yes' when d.Monday =0 then 'No' end),(case when i.Monday= 1 then 'Yes' when i.Monday =0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
--from inserted as i
--inner join deleted as d on i.ID = d.ID and ISNULL(i.Monday,'2') != ISNULL(d.Monday,'2')
--END TRY  
--BEGIN CATCH  
--    --SELECT   
--     print ERROR_NUMBER(); --AS ErrorNumber  
--     print ERROR_MESSAGE(); --AS ErrorMessage;  
--END CATCH;

----when update Tuesday
--BEGIN TRY
--if update(Tuesday)
--insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
--select i.ID, @TransactionID,'Tuesday',(case when d.Tuesday= 1 then 'Yes' when d.Tuesday =0 then 'No' end),(case when i.Tuesday= 1 then 'Yes' when i.Tuesday =0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
--from inserted as i
--inner join deleted as d on i.ID = d.ID and ISNULL(i.Tuesday,'2') != ISNULL(d.Tuesday,'2')
--END TRY  
--BEGIN CATCH  
--    --SELECT   
--     print ERROR_NUMBER(); --AS ErrorNumber  
--     print ERROR_MESSAGE(); --AS ErrorMessage;  
--END CATCH;

----when update Wednesday
--BEGIN TRY
--if update(Wednesday)
--insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
--select i.ID, @TransactionID,'Wednesday',(case when d.Wednesday= 1 then 'Yes' when d.Wednesday =0 then 'No' end),(case when i.Wednesday= 1 then 'Yes' when i.Wednesday =0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
--from inserted as i
--inner join deleted as d on i.ID = d.ID and ISNULL(i.Wednesday,'2') != ISNULL(d.Wednesday,'2')
--END TRY  
--BEGIN CATCH  
--    --SELECT   
--     print ERROR_NUMBER(); --AS ErrorNumber  
--     print ERROR_MESSAGE(); --AS ErrorMessage;  
--END CATCH;


----when update Thursday
--BEGIN TRY
--if update(Thursday)
--insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
--select i.ID, @TransactionID,'Thursday',(case when d.Thursday= 1 then 'Yes' when d.Thursday =0 then 'No' end),(case when i.Thursday= 1 then 'Yes' when i.Thursday =0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
--from inserted as i
--inner join deleted as d on i.ID = d.ID and ISNULL(i.Thursday,'2') != ISNULL(d.Thursday,'2')
--END TRY  
--BEGIN CATCH  
--    --SELECT   
--     print ERROR_NUMBER(); --AS ErrorNumber  
--     print ERROR_MESSAGE(); --AS ErrorMessage;  
--END CATCH;

--when update Notes
BEGIN TRY
if update(Notes)
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',(RTRIM(LTRIM(d.Notes))),(RTRIM(LTRIM(i.Notes))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Notes)),'') != ISNULL(RTRIM(LTRIM(d.Notes)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update breakfrom
BEGIN TRY
if update(breakfrom)
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'breakfrom',d.breakfrom,i.breakfrom,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(Cast(i.breakfrom as datetime),'') != ISNULL(cast(d.breakfrom as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update breakto
BEGIN TRY
if update(breakto)
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'break To',d.breakto,i.breakto,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.breakto as datetime),'') != ISNULL(cast(d.breakto as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update dayofWeek
BEGIN TRY
if update([dayofWeek])
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Day of Week',d.[dayofWeek],i.[dayofWeek],HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.[dayofWeek],'') != ISNULL(d.[dayofWeek],'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update InActive
BEGIN TRY
if update(InActive)
insert into ProviderScheduleAudit(ProviderScheduleID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'In Active',(case when d.InActive= 1 then 'Yes' when d.InActive =0 then 'No' end),(case when i.InActive= 1 then 'Yes' when i.InActive =0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InActive,'2') != ISNULL(d.InActive,'2')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
END
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
BEGIN 
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM ReasonAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM ReasonAudit 

--when update Name
BEGIN TRY
if update(Name)
insert into ReasonAudit(ReasonID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',(RTRIM(LTRIM(d.Name))),(RTRIM(LTRIM(i.Name))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Name)),'') != ISNULL(RTRIM(LTRIM(d.Name)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

----when update UserID
--BEGIN TRY
--if update(UserID)
--Select * into #t from AspNetUsers
--insert into ReasonAudit(ReasonID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
--select i.ID, @TransactionID,'User ID',cDel.FirstName+' '+cDel.LastName,cUpd.FirstName+' '+cUpd.LastName,d.UserID,i.UserID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
--from inserted as i
--inner join deleted as d on i.ID = d.ID and ISNULL(i.UserID,'') != ISNULL(d.UserID,'')
--Join AspNetUsers as cDel on cDel.ID = ISNULL(d.UserID,'')
--join AspNetUsers as cUpd on cUpd.ID = ISNULL(i.UserID,'')
--END TRY  
--BEGIN CATCH  
--    --SELECT   
--     print ERROR_NUMBER(); --AS ErrorNumber  
--     print ERROR_MESSAGE(); --AS ErrorMessage;  
--END CATCH; 

--when update Description
BEGIN TRY
if update(Description)
insert into ReasonAudit(ReasonID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Description',(RTRIM(LTRIM(d.Description))),(RTRIM(LTRIM(i.Description))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Description)),'') != ISNULL(RTRIM(LTRIM(d.Description)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
END
GO

--=====================================
--Description: <  tgrremarkcode Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrRemarkCodeAudit')
DROP TRIGGER [dbo].[tgrRemarkCodeAudit]
GO
Create TRIGGER [dbo].[tgrRemarkCodeAudit]
ON [dbo].[RemarkCode] after Update  
As
BEGIN
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM RemarkCodeAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM RemarkCodeAudit 

--when update Code
BEGIN TRY
if update(Code)
insert into RemarkCodeAudit(RemarkCodeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Code',d.Code,i.Code,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Code,'') != ISNULL(d.Code,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Description
BEGIN TRY
if update(Description)
insert into RemarkCodeAudit(RemarkCodeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Description',(RTRIM(LTRIM(d.Description))),(RTRIM(LTRIM(i.Description))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Description)),'') != ISNULL(RTRIM(LTRIM(d.Description)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
END
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
BEGIN 
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM ResubmitHistoryAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM ResubmitHistoryAudit 

--when update ChargeID
BEGIN TRY
if update(ChargeID)
insert into ResubmitHistoryAudit(ResubmitHistoryID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Charge ID',d.ChargeID,i.ChargeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ChargeID,'1') != ISNULL(d.ChargeID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update VisitID
BEGIN TRY
if update(VisitID)
insert into ResubmitHistoryAudit(ResubmitHistoryID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Visit ID',d.VisitID,i.VisitID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.VisitID,'1') != ISNULL(d.VisitID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;
END
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
BEGIN
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM ReportsLogAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM ReportsLogAudit 

--when update ClientID
BEGIN TRY
if update(ClientID)
insert into ReportsLogAudit(ReportsLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Client ID',d.ClientID,i.ClientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClientID,'1') != ISNULL(d.ClientID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ReceiverID
BEGIN TRY
if update(ReceiverID)
insert into ReportsLogAudit(ReportsLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Receiver ID',d.ReceiverID,i.ReceiverID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReceiverID,'1') != ISNULL(d.ReceiverID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update SubmitterID
BEGIN TRY
if update(SubmitterID)
insert into ReportsLogAudit(ReportsLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submitter ID',d.SubmitterID,i.SubmitterID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmitterID,'1') != ISNULL(d.SubmitterID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ZipFilePath
BEGIN TRY
if update(ZipFilePath)
insert into ReportsLogAudit(ReportsLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Zip File Path',d.ZipFilePath,i.ZipFilePath,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ZipFilePath,'') != ISNULL(d.ZipFilePath,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Processed
BEGIN TRY
if update(Processed)
insert into ReportsLogAudit(ReportsLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Processed',(case when d.Processed= 1 then 'Yes' when  d.Processed=0 then 'No' end),(case when i.Processed= 1 then 'Yes' when i.Processed= 0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Processed,'2') != ISNULL(d.Processed,'2')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update UserResolved
BEGIN TRY
if update(UserResolved)
insert into ReportsLogAudit(ReportsLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'User Resolved',(case when d.UserResolved= 1 then 'Yes' when d.UserResolved= 0 then 'No' end),(case when i.UserResolved= 1 then 'Yes' when i.UserResolved= 0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.UserResolved,'2') != ISNULL(d.UserResolved,'2')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update FilesCount
BEGIN TRY
if update(FilesCount)
insert into ReportsLogAudit(ReportsLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Files Count',d.FilesCount,i.FilesCount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FilesCount,'') != ISNULL(d.FilesCount,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ManualImport
BEGIN TRY
if update(ManualImport)
insert into ReportsLogAudit(ReportsLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Manual Import',(case when d.ManualImport= 1 then 'Yes' when  d.ManualImport=0 then 'No' end),(case when i.ManualImport= 1 then 'Yes' when  i.ManualImport=0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ManualImport,'2') != ISNULL(d.ManualImport,'2')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
END
GO

--=====================================
--Description: <tgrRightsAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrRightsAudit')
DROP TRIGGER [dbo].[tgrRightsAudit]
GO
Create TRIGGER [dbo].[tgrRightsAudit]
ON [dbo].[Rights] after Update  
As
BEGIN
set Xact_abort off
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
END
GO


--=====================================
--Description: <tgrVisitReasonAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrVisitReasonAudit')
DROP TRIGGER [dbo].[tgrVisitReasonAudit]
GO
Create TRIGGER [dbo].[tgrVisitReasonAudit]
ON [dbo].[VisitReason] after Update  
As
BEGIN
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM VisitReasonAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM VisitReasonAudit 

--when update Name
BEGIN TRY
if update(Name)
insert into VisitReasonAudit(VisitReasonID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',(RTRIM(LTRIM(d.Name))),(RTRIM(LTRIM(i.Name))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Name)),'') != ISNULL(RTRIM(LTRIM(d.Name)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Description
BEGIN TRY
if update(Description)
insert into VisitReasonAudit(VisitReasonID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Description',(RTRIM(LTRIM(d.Description))),(RTRIM(LTRIM(i.Description))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Description)),'') != ISNULL(RTRIM(LTRIM(d.Description)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
END
Go

--=====================================
--Description: <tgrVisitStatusLogAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrVisitStatusLogAudit')
DROP TRIGGER [dbo].[tgrVisitStatusLogAudit]
GO
Create TRIGGER [dbo].[tgrVisitStatusLogAudit]
ON [dbo].[VisitStatusLog] after Update  
As
BEGIN
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM VisitStatusLogAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM VisitStatusLogAudit 

--when update DownloadedFileID
BEGIN TRY
if update(DownloadedFileID)
insert into VisitReasonAudit(VisitReasonID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Downloaded File ID',d.DownloadedFileID,i.DownloadedFileID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DownloadedFileID,'') != ISNULL(d.DownloadedFileID,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Transaction276Path
BEGIN TRY
if update(Transaction276Path)
insert into VisitReasonAudit(VisitReasonID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Transaction276 Path',d.Transaction276Path,i.Transaction276Path,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Transaction276Path,'') != ISNULL(d.Transaction276Path,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Transaction277Path
BEGIN TRY
if update(Transaction277Path)
insert into VisitReasonAudit(VisitReasonID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Transaction277 Path',d.Transaction277Path,i.Transaction277Path,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Transaction277Path,'') != ISNULL(d.Transaction277Path,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Transaction277CAPath
BEGIN TRY
if update(Transaction277CAPath)
insert into VisitReasonAudit(VisitReasonID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Transaction277CA Path',d.Transaction277CAPath,i.Transaction277CAPath,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Transaction277CAPath,'') != ISNULL(d.Transaction277CAPath,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Transaction999Path
BEGIN TRY
if update(Transaction999Path)
insert into VisitReasonAudit(VisitReasonID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Transaction999 Path',d.Transaction999Path,i.Transaction999Path,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Transaction999Path,'') != ISNULL(d.Transaction999Path,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
END
GO

--=====================================
--Description: <tgrTeamAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrTeamAudit')
DROP TRIGGER [dbo].[tgrTeamAudit]
GO
Create TRIGGER [dbo].[tgrTeamAudit]
ON [dbo].[Team] after Update  
As
BEGIN
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM TeamAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM TeamAudit

--when update Name
BEGIN TRY
if update(Name)
insert into TeamAudit(TeamID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',(RTRIM(LTRIM(d.Name))),(RTRIM(LTRIM(i.Name))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Name)),'') != ISNULL(RTRIM(LTRIM(d.Name)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Details
BEGIN TRY
if update(Details)
insert into TeamAudit(TeamID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Details',(RTRIM(LTRIM(d.Details))),(RTRIM(LTRIM(i.Details))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Details)),'') != ISNULL(RTRIM(LTRIM(d.Details)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
END
Go


--=====================================
--Description: <tgrInsuranceAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrInsuranceAudit')
DROP TRIGGER [dbo].[tgrInsuranceAudit]
GO
Create TRIGGER [dbo].[tgrInsuranceAudit]
on [dbo].[Insurance] after update
AS 
BEGIN 
set Xact_abort off
Declare @TransactionID int;
select @TransactionID = MAX(TransactionID) from InsuranceAudit
if @TransactionID is null set @TransactionID = 1
else select @TransactionID = Max(TransactionID) + 1 from InsuranceAudit

--when update Name
if update(Name)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',(RTRIM(LTRIM(d.Name))),(RTRIM(LTRIM(i.Name))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Name)),'') != ISNULL(RTRIM(LTRIM(d.Name)),'')

--when update OrganizationName
if update(OrganizationName)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Organization Name',(RTRIM(LTRIM(d.OrganizationName))),(RTRIM(LTRIM(i.OrganizationName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.OrganizationName)),'') != ISNULL(RTRIM(LTRIM(d.OrganizationName)),'')

--when update Address1
if update(Address1)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address1',(RTRIM(LTRIM(d.Address1))),(RTRIM(LTRIM(i.Address1))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Address1)),'') != ISNULL(RTRIM(LTRIM(d.Address1)),'')

--when update Address2
if update(Address2)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address2',(RTRIM(LTRIM(d.Address2))),(RTRIM(LTRIM(i.Address2))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Address2)),'') != ISNULL(RTRIM(LTRIM(d.Address2)),'')

--when update City
if update(City)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'City',(RTRIM(LTRIM(d.City))),(RTRIM(LTRIM(i.City))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.City)),'') != ISNULL(RTRIM(LTRIM(d.City)),'')

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
select i.ID, @TransactionID,'Email',(RTRIM(LTRIM(d.Email))),(RTRIM(LTRIM(i.Email))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Email)),'') != ISNULL(RTRIM(LTRIM(d.Email)),'')

--when update Website
if update(Website)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Website',(RTRIM(LTRIM(d.Website))),(RTRIM(LTRIM(i.Website))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Website)),'') != ISNULL(RTRIM(LTRIM(d.Website)),'')

--when update FaxNumber
if update(FaxNumber)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Fax Number',d.FaxNumber,i.FaxNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FaxNumber,'') != ISNULL(d.FaxNumber,'')

--when update IsActive
if update(IsActive)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsActive',(case when d.IsActive = 1 then 'Yes' when  d.IsActive =0 then 'No' end),(case when i.IsActive = 1 then 'Yes' when i.IsActive = 0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsActive,'2') != ISNULL(d.IsActive,'2')

--when update IsDeleted
if update(IsDeleted)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsDeleted',d.IsDeleted,i.IsDeleted,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsDeleted,'') != ISNULL(d.IsDeleted,'')

--when update Notes
if update(Notes)
insert into InsuranceAudit(InsuranceID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',(RTRIM(LTRIM(d.Notes))),(RTRIM(LTRIM(i.Notes))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Notes)),'') != ISNULL(RTRIM(LTRIM(d.Notes)),'')
End
GO


--=====================================
--Description: <tgrInsurancePlanAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrInsurancePlanAudit')
DROP TRIGGER [dbo].[tgrInsurancePlanAudit]
GO
Create TRIGGER [dbo].[tgrInsurancePlanAudit]
on [dbo].[InsurancePlan] after update
AS 
BEGIN 
set Xact_abort off
Declare @TransactionID int;
select @TransactionID = MAX(TransactionID) from InsurancePlanAudit
if @TransactionID is null set @TransactionID = 1
else
select @TransactionID = Max(TransactionID) + 1 from InsurancePlanAudit

--when update PlanName
if update(PlanName)
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer Name',(RTRIM(LTRIM(d.PlanName))),(RTRIM(LTRIM(i.PlanName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PlanName)),'') != ISNULL(RTRIM(LTRIM(d.PlanName)),'')

-- when update Description
if update(Description)
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Description',(RTRIM(LTRIM(d.Description))),(RTRIM(LTRIM(i.Description))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Description)),'') != ISNULL(RTRIM(LTRIM(d.Description)),'')

--when update InsuranceID
if update(InsuranceID)
--Select * into #t from Insurance
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insurance ID',cDel.[Name],cUpd.[Name],d.InsuranceID,i.InsuranceID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsuranceID,'1') != ISNULL(d.InsuranceID,'1')
Join Insurance as cDel on cDel.ID = ISNULL(d.InsuranceID,'1')
join Insurance as cUpd on cUpd.ID = ISNULL(i.InsuranceID,'1')

--when update PlanTypeID
if update(PlanTypeID)
--Select * into #t1 from PlanType
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Plan Type ID',cDel.[Code],cUpd.[Code],d.PlanTypeID,i.PlanTypeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PlanTypeID,'1') != ISNULL(d.PlanTypeID,'1')
Join PlanType as cDel on cDel.ID = ISNULL(d.PlanTypeID,'1')
join PlanType as cUpd on cUpd.ID = ISNULL(i.PlanTypeID,'1')

--when update IsCapitated
if update(IsCapitated)
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'IsCapitated',(case when d.IsCapitated = 1 then 'Yes' when d.IsCapitated =0 then 'No' end),(case when i.IsCapitated = 1 then 'Yes' when i.IsCapitated =0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
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
--Select * into #t2 from Edi837Payer
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Edi837 Payer ID',cDel.[PayerName],cUpd.[PayerName],d.Edi837PayerID,i.Edi837PayerID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Edi837PayerID,'') != ISNULL(d.Edi837PayerID,'')
Join Edi837Payer as cDel on cDel.ID = ISNULL(d.Edi837PayerID,'')
join Edi837Payer as cUpd on cUpd.ID = ISNULL(i.Edi837PayerID,'')

--when update Edi270PayerID
if update(Edi270PayerID)
--Select * into #t3 from Edi270Payer
insert into InsurancePlanAudit(InsurancePlanID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Edi270 Payer ID',cDel.[PayerName],cUpd.[PayerName],d.Edi270PayerID,i.Edi270PayerID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Edi270PayerID,'') != ISNULL(d.Edi270PayerID,'')
Join Edi270Payer as cDel on cDel.ID = ISNULL(d.Edi270PayerID,'')
join Edi270Payer as cUpd on cUpd.ID = ISNULL(i.Edi270PayerID,'')

--when update Edi276PayerID
if update(Edi276PayerID)
--Select * into #t4 from Edi276Payer
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
select i.ID, @TransactionID,'IsActive',(case when d.IsActive = 1 then 'Yes' else 'No' end),(case when i.IsActive = 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
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
select i.ID, @TransactionID,'Notes',(RTRIM(LTRIM(d.Notes))),(RTRIM(LTRIM(i.Notes))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Notes)),'') != ISNULL(RTRIM(LTRIM(d.Notes)),'')
END
GO

--=====================================
--Description: <tgrInsurancePlanAddressAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrInsurancePlanAddressAudit')
DROP TRIGGER [dbo].[tgrInsurancePlanAddressAudit]
GO
Create TRIGGER [dbo].[tgrInsurancePlanAddressAudit]
on [dbo].[InsurancePlanAddress] after update
AS 
BEGIN 
set Xact_abort off
Declare @TransactionID int;
select @TransactionID = MAX(TransactionID) from InsurancePlanAddressAudit
if @TransactionID is null set @TransactionID = 1
else
select @TransactionID = Max(TransactionID) + 1 from InsurancePlanAddressAudit

--when update Address2
BEGIN TRY
if update(Address2)
insert into InsurancePlanAddressAudit(InsurancePlanAddressID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address2',(RTRIM(LTRIM(d.Address2))),(RTRIM(LTRIM(i.Address2))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Address2)),'') != ISNULL(RTRIM(LTRIM(d.Address2)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update Address1
BEGIN TRY
if update(Address1)
insert into InsurancePlanAddressAudit(InsurancePlanAddressID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address1',(RTRIM(LTRIM(d.Address1))),(RTRIM(LTRIM(i.Address1))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Address1)),'') != ISNULL(RTRIM(LTRIM(d.Address1)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update InsurancePlanId
BEGIN TRY
if update(InsurancePlanId)
--Select * into #t from InsurancePlan
insert into InsurancePlanAddressAudit(InsurancePlanAddressID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insurance Plan ID',cDel.[Description],cUpd.[Description],d.InsurancePlanId,i.InsurancePlanId,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsurancePlanId,'1') != ISNULL(d.InsurancePlanId,'1')
Join InsurancePlan as cDel on cDel.ID = ISNULL(d.InsurancePlanId,'1')
join InsurancePlan as cUpd on cUpd.ID = ISNULL(i.InsurancePlanId,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update City
BEGIN TRY
if update(City)
insert into InsurancePlanAddressAudit(InsurancePlanAddressID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'City',(RTRIM(LTRIM(d.City))),(RTRIM(LTRIM(i.City))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.City)),'') != ISNULL(RTRIM(LTRIM(d.City)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update State
BEGIN TRY
if update(State)
insert into InsurancePlanAddressAudit(InsurancePlanAddressID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'State',d.State,i.State,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.State,'') != ISNULL(d.State,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update ZipCode
BEGIN TRY
if update(ZipCode)
insert into InsurancePlanAddressAudit(InsurancePlanAddressID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Zip Code',d.ZipCode,i.ZipCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ZipCode,'') != ISNULL(d.ZipCode,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update ZipCodeExtension
BEGIN TRY
if update(ZipCodeExtension)
insert into InsurancePlanAddressAudit(InsurancePlanAddressID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Zip Code Extension',d.ZipCodeExtension,i.ZipCodeExtension,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ZipCodeExtension,'') != ISNULL(d.ZipCodeExtension,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update PhoneNumber
BEGIN TRY
if update(PhoneNumber)
insert into InsurancePlanAddressAudit(InsurancePlanAddressID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Phone Number',d.PhoneNumber,i.PhoneNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PhoneNumber,'') != ISNULL(d.PhoneNumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update FaxNumber
BEGIN TRY
if update(FaxNumber)
insert into InsurancePlanAddressAudit(InsurancePlanAddressID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Fax Number',d.FaxNumber,i.FaxNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FaxNumber,'') != ISNULL(d.FaxNumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update IsDeleted
BEGIN TRY
if update(IsDeleted)
insert into InsurancePlanAddressAudit(InsurancePlanAddressID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Is Deleted',d.IsDeleted,i.IsDeleted,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsDeleted,'') != ISNULL(d.IsDeleted,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update Notes
BEGIN TRY
if update(Notes)
insert into InsurancePlanAddressAudit(InsurancePlanAddressID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',(RTRIM(LTRIM(d.Notes))),(RTRIM(LTRIM(i.Notes))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Notes)),'') != ISNULL(RTRIM(LTRIM(d.Notes)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

END
GO



--=====================================
--Description: <tgrPatientPlanAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrPatientPlanAudit')
DROP TRIGGER [dbo].[tgrPatientPlanAudit]
GO
Create TRIGGER [dbo].[tgrPatientPlanAudit]
on [dbo].[PatientPlan] after update
AS 
BEGIN 
set Xact_abort off
Declare @TransactionID int;
select @TransactionID = MAX(TransactionID) from PatientPlanAudit
if @TransactionID is null set @TransactionID = 1
else
select @TransactionID = Max(TransactionID) + 1 from PatientPlanAudit

--when update PatientID
BEGIN TRY
if update(PatientID)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient ID',d.PatientID,i.PatientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientID,'1') != ISNULL(d.PatientID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update InsurancePlanId
BEGIN TRY
if update(InsurancePlanId)
--Select * into #t from InsurancePlan
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insurance Plan ID',cDel.[Description],cUpd.[Description],d.InsurancePlanId,i.InsurancePlanId,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsurancePlanId,'1') != ISNULL(d.InsurancePlanId,'1')
Join InsurancePlan as cDel on cDel.ID = ISNULL(d.InsurancePlanId,'1')
join InsurancePlan as cUpd on cUpd.ID = ISNULL(i.InsurancePlanId,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update Coverage
BEGIN TRY
if update(Coverage)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Coverage',d.Coverage,i.Coverage,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Coverage,'') != ISNULL(d.Coverage,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update RelationShip
BEGIN TRY
if update(RelationShip)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'RelationShip',d.RelationShip,i.RelationShip,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RelationShip,'') != ISNULL(d.RelationShip,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update LastName
BEGIN TRY
if update(LastName)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Last Name',(RTRIM(LTRIM(d.LastName))),(RTRIM(LTRIM(i.LastName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.LastName)),'') != ISNULL(RTRIM(LTRIM(d.LastName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update FirstName
BEGIN TRY
if update(FirstName)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'First Name',(RTRIM(LTRIM(d.FirstName))),(RTRIM(LTRIM(i.FirstName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.FirstName)),'') != ISNULL(RTRIM(LTRIM(d.FirstName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update MiddleInitial
BEGIN TRY
if update(MiddleInitial)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Middle Initial',d.MiddleInitial,i.MiddleInitial,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.MiddleInitial,'') != ISNULL(d.MiddleInitial,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update SSN
BEGIN TRY
if update(SSN)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'SSN',d.SSN,i.SSN,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SSN,'') != ISNULL(d.SSN,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update DOB
BEGIN TRY
if update(DOB)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Date Of Birth',d.DOB,i.DOB,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.DOB as datetime),'') != ISNULL(cast(d.DOB as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update Gender
BEGIN TRY
if update(Gender)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Gender',d.Gender,i.Gender,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Gender,'') != ISNULL(d.Gender,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update Email
BEGIN TRY
if update(Email)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Email',(RTRIM(LTRIM(d.Email))),(RTRIM(LTRIM(i.Email))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Email)),'') != ISNULL(RTRIM(LTRIM(d.Email)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update Address1
BEGIN TRY
if update(Address1)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address1',(RTRIM(LTRIM(d.Address1))),(RTRIM(LTRIM(i.Address1))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Address1)),'') != ISNULL(RTRIM(LTRIM(d.Address1)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update Address2
BEGIN TRY
if update(Address2)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address2',(RTRIM(LTRIM(d.Address2))),(RTRIM(LTRIM(i.Address2))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Address2)),'') != ISNULL(RTRIM(LTRIM(d.Address2)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update City
BEGIN TRY
if update(City)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'City',(RTRIM(LTRIM(d.City))),(RTRIM(LTRIM(i.City))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.City)),'') != ISNULL(RTRIM(LTRIM(d.City)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update State
BEGIN TRY
if update(State)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'State',d.State,i.State,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.State,'') != ISNULL(d.State,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update ZipCode
BEGIN TRY
if update(ZipCode)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Zip Code',d.ZipCode,i.ZipCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ZipCode,'') != ISNULL(d.ZipCode,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update ZipCodeExtension
BEGIN TRY
if update(ZipCodeExtension)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Zip Code Extension',d.ZipCodeExtension,i.ZipCodeExtension,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ZipCodeExtension,'') != ISNULL(d.ZipCodeExtension,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update SubscriberId
BEGIN TRY
if update(SubscriberId)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Subscriber Id',(RTRIM(LTRIM(d.SubscriberId))),(RTRIM(LTRIM(i.SubscriberId))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.SubscriberId)),'') != ISNULL(RTRIM(LTRIM(d.SubscriberId)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update GroupName
BEGIN TRY
if update(GroupName)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Group Name',(RTRIM(LTRIM(d.GroupName))),(RTRIM(LTRIM(i.GroupName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.GroupName)),'') != ISNULL(RTRIM(LTRIM(d.GroupName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update PlanBeginDate
BEGIN TRY
if update(PlanBeginDate)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Plan Begin Date',d.PlanBeginDate,i.PlanBeginDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.PlanBeginDate as datetime),'') != ISNULL(cast(d.PlanBeginDate as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update PlanEndDate
BEGIN TRY
if update(PlanEndDate)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Plan End Date',d.PlanEndDate,i.PlanEndDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.PlanEndDate as datetime),'') != ISNULL(cast(d.PlanEndDate as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update Copay
BEGIN TRY
if update(Copay)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Copay',d.Copay,i.Copay,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Copay,'1') != ISNULL(d.Copay,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update Deductible
BEGIN TRY
if update(Deductible)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Deductible',d.Deductible,i.Deductible,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Deductible,'1') != ISNULL(d.Deductible,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update CoInsurance
BEGIN TRY
if update(CoInsurance)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'CoInsurance',d.CoInsurance,i.CoInsurance,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CoInsurance,'1') != ISNULL(d.CoInsurance,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update EmlpoyerID
BEGIN TRY
if update(EmlpoyerID)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Emlpoyer ID',d.EmlpoyerID,i.EmlpoyerID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.EmlpoyerID,'') != ISNULL(d.EmlpoyerID,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update InsurancePlanAddressID
BEGIN TRY
if update(InsurancePlanAddressID)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Insurance Plan Address ID',d.InsurancePlanAddressID,i.InsurancePlanAddressID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.InsurancePlanAddressID,'1') != ISNULL(d.InsurancePlanAddressID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update PhoneNumber
BEGIN TRY
if update(PhoneNumber)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Phone Number',d.PhoneNumber,i.PhoneNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PhoneNumber,'') != ISNULL(d.PhoneNumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update IsActive
BEGIN TRY
if update(IsActive)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Is Active',(case when d.IsActive = 1 then 'Yes' else 'No' end),(case when i.IsActive = 1 then 'Yes' else 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsActive,'2') != ISNULL(d.IsActive,'2')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update IsDeleted
BEGIN TRY
if update(IsDeleted)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Is Deleted',d.IsDeleted,i.IsDeleted,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsDeleted,'') != ISNULL(d.IsDeleted,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update IsSelfPay
BEGIN TRY
if update(IsSelfPay)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Is Self Pay',(case when d.IsSelfPay = 1 then 'Yes' when  d.IsSelfPay =0 then 'No' end),(case when i.IsSelfPay = 1 then 'Yes' when i.IsSelfPay = 0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsSelfPay,'2') != ISNULL(d.IsSelfPay,'2')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update Notes
BEGIN TRY
if update(Notes)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',(RTRIM(LTRIM(d.Notes))),(RTRIM(LTRIM(i.Notes))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Notes)),'') != ISNULL(RTRIM(LTRIM(d.Notes)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update BatchDocumentID
BEGIN TRY
if update(BatchDocumentID)
--Select * into #t1 from BatchDocument
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Batch Document ID',cDel.[Description],cUpd.[Description],d.BatchDocumentID,i.BatchDocumentID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.BatchDocumentID,'1') != ISNULL(d.BatchDocumentID,'1')
Join BatchDocument as cDel on cDel.ID = ISNULL(d.BatchDocumentID,'1')
join BatchDocument as cUpd on cUpd.ID = ISNULL(i.BatchDocumentID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update PageNumber
BEGIN TRY
if update(PageNumber)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Page Number',d.PageNumber,i.PageNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PageNumber,'') != ISNULL(d.PageNumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update FrontSideCard
BEGIN TRY
if update(FrontSideCard)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Front Side Card',d.FrontSideCard,i.FrontSideCard,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FrontSideCard,'') != ISNULL(d.FrontSideCard,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

-- when update backSidecard
BEGIN TRY
if update(backSidecard)
insert into PatientPlanAudit(PatientPlanID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'back Side card',d.backSidecard,i.backSidecard,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.backSidecard,'') != ISNULL(d.backSidecard,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
END
GO

--=====================================
--Description: <tgrICDAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrICDAudit')
DROP TRIGGER [dbo].[tgrICDAudit]
GO
Create TRIGGER [dbo].[tgrICDAudit]
on [dbo].[ICD] after update
AS 
BEGIN 
set Xact_abort off
Declare @TransactionID int;
select @TransactionID = MAX(TransactionID) from ICDAudit
if @TransactionID is null set @TransactionID = 1
else
select @TransactionID = Max(TransactionID) + 1 from ICDAudit

--when update Description
BEGIN TRY
if update(Description)
insert into ICDAudit(ICDID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Description',(RTRIM(LTRIM(d.Description))),(RTRIM(LTRIM(i.Description))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Description)),'') != ISNULL(RTRIM(LTRIM(d.Description)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 


--when update ICDCode
BEGIN TRY
if update(ICDCode)
insert into ICDAudit(ICDID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'ICD Code',d.ICDCode,i.ICDCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ICDCode,'') != ISNULL(d.ICDCode,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 


--when update IsValid
BEGIN TRY
if update(IsValid)
insert into ICDAudit(ICDID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Is Valid',(case when d.IsValid = 1 then 'Yes' when d.IsValid =0 then 'No' end),(case when i.IsValid = 1 then 'Yes' when i.IsValid = 0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsValid,'2') != ISNULL(d.IsValid,'2')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 


--when update IsActive
BEGIN TRY
if update(IsActive)
insert into ICDAudit(ICDID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Is Active',(case when d.IsActive = 1 then 'Yes' when d.IsActive = 0 then 'No' end),(case when i.IsActive = 1 then 'Yes' when i.IsActive = 0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsActive,'2') != ISNULL(d.IsActive,'2')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update IsDeleted
BEGIN TRY
if update(IsDeleted)
insert into ICDAudit(ICDID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Is Deleted',d.IsDeleted,i.IsDeleted,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsDeleted,'') != ISNULL(d.IsDeleted,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

END
GO

--=====================================
--Description: <tgrBatchDocumentAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrBatchDocumentAudit')
DROP TRIGGER [dbo].[tgrBatchDocumentAudit]
GO
Create TRIGGER [dbo].[tgrBatchDocumentAudit]
on [dbo].[BatchDocument] after update
AS 
BEGIN 
set Xact_abort off
Declare @TransactionID int;
select @TransactionID = MAX(TransactionID) from BatchDocumentAudit
if @TransactionID is null set @TransactionID = 1
else
select @TransactionID = Max(TransactionID) + 1 from BatchDocumentAudit

--when update Description
BEGIN TRY
if update(Description)
insert into BatchDocumentAudit(BatchDocumentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Description',(RTRIM(LTRIM(d.Description))),(RTRIM(LTRIM(i.Description))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Description)),'') != ISNULL(RTRIM(LTRIM(d.Description)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

-- when update PracticeID
BEGIN TRY
if update(PracticeID)
--Select * into #t2 from Practice
insert into BatchDocumentAudit(BatchDocumentID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Practice',cDel.[Name],cUpd.[Name],d.PracticeID,i.PracticeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PracticeID,'1') != ISNULL(d.PracticeID,'1')
Join Practice as cDel on cDel.ID = ISNULL(d.PracticeID,'1')
join Practice as cUpd on cUpd.ID = ISNULL(i.PracticeID,'1')	
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

-- when update LocationID
BEGIN TRY
if update(LocationID)
--Select * into #t3 from Location
insert into BatchDocumentAudit(BatchDocumentID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Location',cDel.[Name],cUpd.[Name],d.LocationID,i.LocationID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.LocationID,'1') != ISNULL(d.LocationID,'1')
Join Location as cDel on cDel.ID = ISNULL(d.LocationID,'1')
join Location as cUpd on cUpd.ID = ISNULL(i.LocationID,'1')	
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

-- when update ProviderID
BEGIN TRY
if update(ProviderID)
--Select * into #t4 from Provider
insert into BatchDocumentAudit(BatchDocumentID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Provider',cDel.[Name],cUpd.[Name],d.ProviderID,i.ProviderID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProviderID,'1') != ISNULL(d.ProviderID,'1')
Join Provider as cDel on cDel.ID = ISNULL(d.ProviderID,'1')
join Provider as cUpd on cUpd.ID = ISNULL(i.ProviderID,'1')	
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update DocumentFilePath
BEGIN TRY
if update(DocumentFilePath)
insert into BatchDocumentAudit(BatchDocumentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Document File Path',d.DocumentFilePath,i.DocumentFilePath,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DocumentFilePath,'') != ISNULL(d.DocumentFilePath,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update FileName
BEGIN TRY
if update(FileName)
insert into BatchDocumentAudit(BatchDocumentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'File Name',d.FileName,i.FileName,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FileName,'') != ISNULL(d.FileName,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update NumberOfPages
BEGIN TRY
if update(NumberOfPages)
insert into BatchDocumentAudit(BatchDocumentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Number Of Pages',d.NumberOfPages,i.NumberOfPages,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.NumberOfPages,'') != ISNULL(d.NumberOfPages,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update FileSize
BEGIN TRY
if update(FileSize)
insert into BatchDocumentAudit(BatchDocumentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'File Size',d.FileSize,i.FileSize,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FileSize,'') != ISNULL(d.FileSize,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update FileType
BEGIN TRY
if update(FileType)
insert into BatchDocumentAudit(BatchDocumentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'File Type',d.FileType,i.FileType,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.FileType,'') != ISNULL(d.FileType,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update DocumentTypeID
BEGIN TRY
if update(DocumentTypeID)
--select * into #t5 from DocumentType
insert into BatchDocumentAudit(BatchDocumentID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Document Type',cDel.Name,cUpd.Name,d.DocumentTypeID,i.DocumentTypeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DocumentTypeID,'1') != ISNULL(d.DocumentTypeID,'1')
Join DocumentType as cDel on cDel.ID = ISNULL(d.DocumentTypeID,'1')
join DocumentType as cUpd on cUpd.ID = ISNULL(i.DocumentTypeID,'1') 
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update Status
BEGIN TRY
if update(Status)
insert into BatchDocumentAudit(BatchDocumentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Status',Case when d.Status = 'N' then 'NOT STARTED' when d.Status = 'C' then 'CLOSED' when d.Status = 'I' then 'IN PROCESS' end,Case when i.Status = 'N' then 'NOT STARTED' when i.Status = 'C' then 'CLOSED' when i.Status = 'I' then 'IN PROCESS' end,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Status,'') != ISNULL(d.Status,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update ResponsibleParty
BEGIN TRY
if update(ResponsibleParty)
insert into BatchDocumentAudit(BatchDocumentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Responsible Party',Case when d.ResponsibleParty = 'B' then 'Biller' when d.ResponsibleParty = 'C' then 'Client' end,Case when i.ResponsibleParty = 'B' then 'Biller' when i.ResponsibleParty = 'C' then 'Client' end,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ResponsibleParty,'') != ISNULL(d.ResponsibleParty,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;


--when update NoOfDemographics
BEGIN TRY
if update(NoOfDemographics)
--select * into #t5 from DocumentType
insert into BatchDocumentAudit(BatchDocumentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'No Of Demographics',d.NoOfDemographics,i.NoOfDemographics,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.NoOfDemographics,'1') != ISNULL(d.NoOfDemographics,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update NoOfDemographicsEntered
BEGIN TRY
if update(NoOfDemographicsEntered)
--select * into #t5 from DocumentType
insert into BatchDocumentAudit(BatchDocumentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'No Of Demographics Entered',d.NoOfDemographicsEntered,i.NoOfDemographicsEntered,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.NoOfDemographicsEntered,'1') != ISNULL(d.NoOfDemographicsEntered,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

END
GO

--=====================================
--Description: <tgrBillerAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrBillerAudit')
DROP TRIGGER [dbo].[tgrBillerAudit]
GO
Create TRIGGER [dbo].[tgrBillerAudit]
on [dbo].[Biller] after update
AS 
BEGIN 
set Xact_abort off
Declare @TransactionID int;
select @TransactionID = MAX(TransactionID) from BillerAudit
if @TransactionID is null set @TransactionID = 1
else
select @TransactionID = Max(TransactionID) + 1 from BillerAudit

--when update LastName
BEGIN TRY
if update(LastName)
insert into BillerAudit(BillerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Last Name',(RTRIM(LTRIM(d.LastName))),(RTRIM(LTRIM(i.LastName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.LastName)),'') != ISNULL(RTRIM(LTRIM(d.LastName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update FirstName
BEGIN TRY
if update(FirstName)
insert into BillerAudit(BillerID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'First Name',(RTRIM(LTRIM(d.FirstName))),(RTRIM(LTRIM(i.FirstName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.FirstName)),'') != ISNULL(RTRIM(LTRIM(d.FirstName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;
END
GO


--=====================================
--Description: <tgrDocumentTypeAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrDocumentTypeAudit')
DROP TRIGGER [dbo].[tgrDocumentTypeAudit]
GO
Create TRIGGER [dbo].[tgrDocumentTypeAudit]
on [dbo].[DocumentType] after update
AS 
BEGIN 
set Xact_abort off
Declare @TransactionID int;
select @TransactionID = MAX(TransactionID) from DocumentTypeAudit
if @TransactionID is null set @TransactionID = 1
else
select @TransactionID = Max(TransactionID) + 1 from DocumentTypeAudit

--when update Name
BEGIN TRY
if update(Name)
insert into DocumentTypeAudit(DocumentTypeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',(RTRIM(LTRIM(d.Name))),(RTRIM(LTRIM(i.Name))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Name)),'') != ISNULL(RTRIM(LTRIM(d.Name)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;
END
GO

--=====================================
--Description: <tgrPOSAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrPOSAudit')
DROP TRIGGER [dbo].[tgrPOSAudit]
GO
Create TRIGGER [dbo].[tgrPOSAudit]
on [dbo].[POS] after update
AS 
BEGIN 
set xact_abort off
Declare @TransactionID int;
select @TransactionID = MAX(TransactionID) from POSAudit
if @TransactionID is null set @TransactionID = 1
else
select @TransactionID = Max(TransactionID) + 1 from POSAudit

--when update PosCode
BEGIN TRY 
if update(PosCode)
insert into POSAudit(POSID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Pos Code',d.PosCode,i.PosCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PosCode,'') != ISNULL(d.PosCode,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;  

--when update Name
BEGIN TRY 
if update(Name)
insert into POSAudit(POSID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Name',(RTRIM(LTRIM(d.Name))),(RTRIM(LTRIM(i.Name))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Name)),'') != ISNULL(RTRIM(LTRIM(d.Name)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;  

--when update Description
BEGIN TRY 
if update([Description])
insert into POSAudit(POSID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Description',(RTRIM(LTRIM(d.[Description]))),(RTRIM(LTRIM(i.[Description]))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.[Description])),'') != ISNULL(RTRIM(LTRIM(d.[Description])),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;  
END
GO
--=====================================
--Description: <tgrPatientAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrPatientAudit')
DROP TRIGGER [dbo].[tgrPatientAudit]
GO
Create TRIGGER [dbo].[tgrPatientAudit]
on [dbo].[Patient] after update
AS 
BEGIN
set xact_abort off
Declare @TransactionID int;
select @TransactionID = MAX(TransactionID) from PatientAudit
if @TransactionID is null set @TransactionID = 1
else
select @TransactionID = Max(TransactionID) + 1 from PatientAudit

--when update ProfilePic
BEGIN TRY 
if update(ProfilePic)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Profile Picture',d.ProfilePic,i.ProfilePic,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProfilePic,'') != ISNULL(d.ProfilePic,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;  

--when update AccountNum
BEGIN TRY
if update(AccountNum)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Account Number',d.AccountNum,i.AccountNum,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AccountNum,'') != ISNULL(d.AccountNum,'')
END TRY  
BEGIN CATCH  
--SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;    
END CATCH;

--when update MedicalRecordNumber
BEGIN TRY
if update(MedicalRecordNumber)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Medical Record Number',d.MedicalRecordNumber,i.MedicalRecordNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.MedicalRecordNumber,'') != ISNULL(d.MedicalRecordNumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;


--when update LastName
BEGIN TRY
if update(LastName)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Last Name',(RTRIM(LTRIM(d.LastName))),(RTRIM(LTRIM(i.LastName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.LastName)),'') != ISNULL(RTRIM(LTRIM(d.LastName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;    
END CATCH;

--when update FirstName
BEGIN TRY
if update(FirstName)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'First Name',(RTRIM(LTRIM(d.FirstName))),(RTRIM(LTRIM(i.FirstName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.FirstName)),'') != ISNULL(RTRIM(LTRIM(d.FirstName)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update MiddleInitial
BEGIN TRY
if update(MiddleInitial)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Middle Initial',(RTRIM(LTRIM(d.MiddleInitial))),(RTRIM(LTRIM(i.MiddleInitial))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.MiddleInitial)),'') != ISNULL(RTRIM(LTRIM(d.MiddleInitial)),'')
END TRY  
BEGIN CATCH  
   --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update Title
BEGIN TRY
if update(Title)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Title',d.Title,i.Title,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Title,'') != ISNULL(d.Title,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update SSN
BEGIN TRY
if update(SSN)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'SSN',d.SSN,i.SSN,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SSN,'') != ISNULL(d.SSN,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;    
END CATCH;

--when update DOB
BEGIN TRY
if update(DOB)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Date Of Birth',d.DOB,i.DOB,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.DOB as date),'') != ISNULL(cast(d.DOB as date),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;    
END CATCH;

--when update Gender
BEGIN TRY
if update(Gender)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Gender',d.Gender,i.Gender,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Gender,'') != ISNULL(d.Gender,'')
END TRY  
BEGIN CATCH  
 --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update MaritalStatus
BEGIN TRY
if update(MaritalStatus)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Marital Status',d.MaritalStatus,i.MaritalStatus,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.MaritalStatus,'') != ISNULL(d.MaritalStatus,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update Race
Begin Try
if update(Race)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Race',d.Race,i.Race,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Race,'') != ISNULL(d.Race,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update Ethnicity
BEGIN TRY
if update(Ethnicity)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Ethnicity',d.Ethnicity,i.Ethnicity,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Ethnicity,'') != ISNULL(d.Ethnicity,'')
END TRY  
BEGIN CATCH  
   --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update Address1
BEGIN TRY
if update(Address1)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address1',(RTRIM(LTRIM(d.Address1))),(RTRIM(LTRIM(i.Address1))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Address1)),'') != ISNULL(RTRIM(LTRIM(d.Address1)),'')
END TRY  
BEGIN CATCH  
   --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update Address2
BEGIN TRY
if update(Address2)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Address2',(RTRIM(LTRIM(d.Address2))),(RTRIM(LTRIM(i.Address2))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Address2)),'') != ISNULL(RTRIM(LTRIM(d.Address2)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update City
BEGIN TRY
if update(City)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'City',(RTRIM(LTRIM(d.City))),(RTRIM(LTRIM(i.City))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.City)),'') != ISNULL(RTRIM(LTRIM(d.City)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;    
END CATCH;

--when update State
BEGIN TRY
if update(State)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'State',d.State,i.State,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.State,'') != ISNULL(d.State,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;    
END CATCH;

--when update ZipCode
BEGIN TRY
if update(ZipCode)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Zip Code',d.ZipCode,i.ZipCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ZipCode,'') != ISNULL(d.ZipCode,'')
END TRY  
BEGIN CATCH  
 --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update ZipCodeExtension
BEGIN TRY 
if update(ZipCodeExtension)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Zip Code Extension',d.ZipCodeExtension,i.ZipCodeExtension,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ZipCodeExtension,'') != ISNULL(d.ZipCodeExtension,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update MobileNumber
BEGIN TRY
if update(MobileNumber)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Mobile Number',d.MobileNumber,i.MobileNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.MobileNumber,'') != ISNULL(d.MobileNumber,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PhoneNumber
BEGIN TRY
if update(PhoneNumber)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Phone Number',d.PhoneNumber,i.PhoneNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PhoneNumber,'') != ISNULL(d.PhoneNumber,'')
END TRY  
BEGIN CATCH  
   --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;    
END CATCH;

--when update Email
BEGIN TRY
if update(Email)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Email',(RTRIM(LTRIM(d.Email))),(RTRIM(LTRIM(i.Email))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Email)),'') != ISNULL(RTRIM(LTRIM(d.Email)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;    
END CATCH;

-- when update PracticeID
BEGIN TRY
if update(PracticeID)
--Select * into #t1 from Practice
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Practice ID',cDel.[Name],cUpd.[Name],d.PracticeID,i.PracticeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PracticeID,'1') != ISNULL(d.PracticeID,'1')
Join Practice as cDel on cDel.ID = ISNULL(d.PracticeID,'1')
join Practice as cUpd on cUpd.ID = ISNULL(i.PracticeID,'1')	
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;


-- when update LocationId
BEGIN TRY
if update(LocationId)
--Select * into #t2 from location
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Location ID',cDel.[Name],cUpd.[Name],d.LocationId,i.LocationId,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.LocationId,'1') != ISNULL(d.LocationId,'1')
Join location as cDel on cDel.ID = ISNULL(d.LocationId,'1')
join location as cUpd on cUpd.ID = ISNULL(i.LocationId,'1')	
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

-- when update ProviderID
BEGIN TRY
if update(ProviderID)
--Select * into #t3 from Provider
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Provider ID',cDel.[Name],cUpd.[Name],d.ProviderID,i.ProviderID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProviderID,'1') != ISNULL(d.ProviderID,'1')
Join Provider as cDel on cDel.ID = ISNULL(d.ProviderID,'1')
join Provider as cUpd on cUpd.ID = ISNULL(i.ProviderID,'1')	
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

-- when update RefProviderID
BEGIN TRY
if update(RefProviderID)
--Select * into #t4 from RefProvider
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Practice ID',cDel.[Name],cUpd.[Name],d.RefProviderID,i.RefProviderID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RefProviderID,'1') != ISNULL(d.RefProviderID,'1')
Join RefProvider as cDel on cDel.ID = ISNULL(d.RefProviderID,'1')
join RefProvider as cUpd on cUpd.ID = ISNULL(i.RefProviderID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

-- when update BatchDocumentID
BEGIN TRY
if update(BatchDocumentID)
--Select * into #t5 from BatchDocument
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Batch Document ID',cDel.[Description],cUpd.[Description],d.BatchDocumentID,i.BatchDocumentID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.BatchDocumentID,'1') != ISNULL(d.BatchDocumentID,'1')
Join BatchDocument as cDel on cDel.ID = ISNULL(d.BatchDocumentID,'1')
join BatchDocument as cUpd on cUpd.ID = ISNULL(i.BatchDocumentID,'1')
END TRY  
BEGIN CATCH  
   --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PageNumber
BEGIN TRY
if update(PageNumber)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Page Number',d.PageNumber,i.PageNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PageNumber,'') != ISNULL(d.PageNumber,'')
END TRY  
BEGIN CATCH  
   --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update IsActive
BEGIN TRY
if update(IsActive)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Is Active',(case when d.IsActive = 1 then 'Yes' when d.IsActive = 0 then 'No' end),(case when i.IsActive = 1 then 'Yes' when i.IsActive = 0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsActive,'2') != ISNULL(d.IsActive,'2')
END TRY  
BEGIN CATCH  
   --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;    
END CATCH;

--when update IsDeleted
BEGIN TRY
if update(IsDeleted)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Is Deleted',d.IsDeleted,i.IsDeleted,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.IsDeleted,'') != ISNULL(d.IsDeleted,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;    
END CATCH;

--when update Notes
BEGIN TRY
if update(Notes)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',(RTRIM(LTRIM(d.Notes))),(RTRIM(LTRIM(i.Notes))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Notes)),'') != ISNULL(RTRIM(LTRIM(d.Notes)),'')
END TRY  
BEGIN CATCH  
   --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;    
END CATCH;

--when update Statement
BEGIN TRY
if update(Statement)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Statement',(case when d.Statement =1 then 'Yes' when d.Statement =0 then 'No' end),(case when i.Statement =1 then 'Yes' when i.Statement =0 then 'No' end),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Statement,'2') != ISNULL(d.Statement,'2')
END TRY  
BEGIN CATCH  
   --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update StatementMessage
BEGIN TRY
if update(StatementMessage)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Statement Message',(RTRIM(LTRIM(d.StatementMessage))),(RTRIM(LTRIM(i.StatementMessage))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.StatementMessage)),'') != ISNULL(RTRIM(LTRIM(d.StatementMessage)),'')
END TRY  
BEGIN CATCH  
 --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update RemainingDeductible
BEGIN TRY
if update(RemainingDeductible)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Remaining Deductible',d.RemainingDeductible,i.RemainingDeductible,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RemainingDeductible,'1') != ISNULL(d.RemainingDeductible,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ExternalPatientID
BEGIN TRY
if update(ExternalPatientID)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'External Patient ID',d.ExternalPatientID,i.ExternalPatientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ExternalPatientID,'') != ISNULL(d.ExternalPatientID,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;
END
Go

--=====================================
--Description: <tgrSettingsAudit Trigger>
--======================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrSettingsAudit')
DROP TRIGGER [dbo].[tgrSettingsAudit]
GO
Create TRIGGER [dbo].[tgrSettingsAudit]
on [dbo].[Settings] after update
AS 
BEGIN 
set xact_abort off
Declare @TransactionID int;
select @TransactionID = MAX(TransactionID) from SettingsAudit
if @TransactionID is null set @TransactionID = 1
else
select @TransactionID = Max(TransactionID) + 1 from SettingsAudit

--when update ClientID
BEGIN TRY
if update(ClientID)
insert into SettingsAudit(SettingsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Client ID',d.ClientID,i.ClientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ClientID,'1') != ISNULL(d.ClientID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update DocumentServerURL
BEGIN TRY
if update(DocumentServerURL)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Document Server URL',d.DocumentServerURL,i.DocumentServerURL,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DocumentServerURL,'') != ISNULL(d.DocumentServerURL,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update DocumentServerDirectory
BEGIN TRY
if update(DocumentServerDirectory)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Document Server Directory',d.DocumentServerDirectory,i.DocumentServerDirectory,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DocumentServerDirectory,'') != ISNULL(d.DocumentServerDirectory,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update DocumentServerAuthUser
BEGIN TRY
if update(DocumentServerAuthUser)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Document Server Auther User',d.DocumentServerAuthUser,i.DocumentServerAuthUser,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DocumentServerAuthUser,'') != ISNULL(d.DocumentServerAuthUser,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update DocumentServerAuthPass
BEGIN TRY
if update(DocumentServerAuthPass)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Document Server Auther Pass',d.DocumentServerAuthPass,i.DocumentServerAuthPass,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DocumentServerAuthPass,'') != ISNULL(d.DocumentServerAuthPass,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
END
Go



--=====================================
--Description: <tgrProviderSlotAudit Trigger>
--======================================
IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrProviderSlotAudit')
DROP TRIGGER [dbo].[tgrProviderSlotAudit]
GO
Create TRIGGER [dbo].[tgrProviderSlotAudit]
on [dbo].[ProviderSlot] after update
AS 
BEGIN 
set xact_abort off
Declare @TransactionID int;
select @TransactionID = MAX(TransactionID) from ProviderSlotAudit
if @TransactionID is null set @TransactionID = 1
else
select @TransactionID = Max(TransactionID) + 1 from ProviderSlotAudit

--when update ProviderScheduleID
BEGIN TRY
if update(ProviderScheduleID)
--select * into #t from ProviderSchedule
insert into ProviderSlotAudit(ProviderSlotID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Provider Schedule ID',d.ProviderScheduleID,i.ProviderScheduleID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProviderScheduleID,'1') != ISNULL(d.ProviderScheduleID,'1')
--Join ProviderSchedule as cDel on cDel.ID = ISNULL(d.ProviderScheduleID,'')
--join ProviderSchedule as cUpd on cUpd.ID = ISNULL(i.ProviderScheduleID,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update FromDate
BEGIN TRY
if update(FromDate)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'From Date',d.FromDate,i.FromDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.FromDate as date),'') != ISNULL(cast(d.FromDate as date),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ToDate
BEGIN TRY
if update(ToDate)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'To Date',d.ToDate,i.ToDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.ToDate as date),'') != ISNULL(cast(d.ToDate as date),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update FromTime
BEGIN TRY
if update(FromTime)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'From Time',d.FromTime,i.FromTime,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.FromTime as datetime),'') != ISNULL(cast(d.FromTime as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ToTime
BEGIN TRY
if update(ToTime)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'To Time',d.ToTime,i.ToTime,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.ToTime as datetime),'') != ISNULL(cast(d.ToTime as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update TimeInterval
BEGIN TRY
if update(TimeInterval)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Time Interval',d.TimeInterval,i.TimeInterval,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.TimeInterval,'') != ISNULL(d.TimeInterval,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update Status
BEGIN TRY
if update(Status)
insert into PatientAudit(PatientID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Status',d.Status,i.Status,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Status,'') != ISNULL(d.Status,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

END
GO

-- =============================================
-- Description:	<tgrPlanFollowupChargeAudit Trigger>
-- =============================================
IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrPlanFollowupChargeAudit')
DROP TRIGGER [dbo].[tgrPlanFollowupChargeAudit]
GO
Create TRIGGER [dbo].[tgrPlanFollowupChargeAudit]
ON [dbo].[PlanFollowupCharge] after Update
AS 
BEGIN
set xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM PlanFollowupChargeAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM PlanFollowupChargeAudit  

--when update PlanFollowupID
BEGIN TRY
if update(PlanFollowupID)
insert into PlanFollowupChargeAudit(PlanFollowupChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Plan Followup ID',d.PlanFollowupID,i.PlanFollowupID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PlanFollowupID,'1') != ISNULL(d.PlanFollowupID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ChargeID
BEGIN TRY
if update(ChargeID)
insert into PlanFollowupChargeAudit(PlanFollowupChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Charge ID',d.ChargeID,i.ChargeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ChargeID,'1') != ISNULL(d.ChargeID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update GroupID
BEGIN TRY
if update(GroupID)
--Select * into #t from [Group]
insert into PlanFollowupChargeAudit(PlanFollowupChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Charge ID',cDel.[Name],cUpd.[Name],d.GroupID,i.GroupID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.GroupID,'1') != ISNULL(d.GroupID,'1')
Join [Group] as cDel on cDel.ID = ISNULL(d.GroupID,'1')
join [Group] as cUpd on cUpd.ID = ISNULL(i.GroupID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ReasonID
BEGIN TRY
if update(ReasonID)
--Select * into #t1 from Reason
insert into PlanFollowupChargeAudit(PlanFollowupChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Charge ID',cDel.[Name],cUpd.[Name],d.ReasonID,i.ReasonID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReasonID,'1') != ISNULL(d.ReasonID,'1')
Join Reason as cDel on cDel.ID = ISNULL(d.ReasonID,'1')
join Reason as cUpd on cUpd.ID = ISNULL(i.ReasonID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update ActionID
BEGIN TRY
if update(ActionID)
--Select * into #t2 from Action
insert into PlanFollowupChargeAudit(PlanFollowupChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Action ID',cDel.[Name],cUpd.[Name],d.ActionID,i.ActionID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ActionID,'1') != ISNULL(d.ActionID,'1')
Join Action as cDel on cDel.ID = ISNULL(d.ActionID,'1')
join Action as cUpd on cUpd.ID = ISNULL(i.ActionID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update AdjustmentCodeID
BEGIN TRY
if update(AdjustmentCodeID)
insert into PlanFollowupChargeAudit(PlanFollowupChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Code ID',d.AdjustmentCodeID,i.AdjustmentCodeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentCodeID,'') != ISNULL(d.AdjustmentCodeID,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update Notes
BEGIN TRY
if update(Notes)
insert into PlanFollowupChargeAudit(PlanFollowupChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Notes',(RTRIM(LTRIM(d.Notes))),(RTRIM(LTRIM(i.Notes))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Notes)),'') != ISNULL(RTRIM(LTRIM(d.Notes)),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update PaymentChargeID
BEGIN TRY
if update(PaymentChargeID)
insert into PlanFollowupChargeAudit(PlanFollowupChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payment Charge ID',d.PaymentChargeID,i.PaymentChargeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaymentChargeID,'1') != ISNULL(d.PaymentChargeID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update RemarkCode1ID
BEGIN TRY
if update(RemarkCode1ID)
insert into PlanFollowupChargeAudit(PlanFollowupChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Remark Code 1ID',d.RemarkCode1ID,i.RemarkCode1ID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RemarkCode1ID,'') != ISNULL(d.RemarkCode1ID,'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update RemarkCodeID
BEGIN TRY
if update(RemarkCodeID)
--Select * into #t3 from RemarkCode
insert into PlanFollowupChargeAudit(PlanFollowupChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Remark Code ID',cDel.[Description],cUpd.[Description],d.RemarkCodeID,i.RemarkCodeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RemarkCodeID,'1') != ISNULL(d.RemarkCodeID,'1')
Join RemarkCode as cDel on cDel.ID = ISNULL(d.RemarkCodeID,'1')
join RemarkCode as cUpd on cUpd.ID = ISNULL(i.RemarkCodeID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update RemarkCode2ID
BEGIN TRY
if update(RemarkCode2ID)
--Select * into #t4 from RemarkCode
insert into PlanFollowupChargeAudit(PlanFollowupChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Remark Code 2ID',cDel.[Description],cUpd.[Description],d.RemarkCode2ID,i.RemarkCode2ID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RemarkCode2ID,'1') != ISNULL(d.RemarkCode2ID,'1')
Join RemarkCode as cDel on cDel.ID = ISNULL(d.RemarkCode2ID,'1')
join RemarkCode as cUpd on cUpd.ID = ISNULL(i.RemarkCode2ID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update RemarkCode3ID
BEGIN TRY
if update(RemarkCode3ID)
--Select * into #t5 from RemarkCode
insert into PlanFollowupChargeAudit(PlanFollowupChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Remark Code 3ID',cDel.[Description],cUpd.[Description],d.RemarkCode3ID,i.RemarkCode3ID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RemarkCode3ID,'1') != ISNULL(d.RemarkCodeID,'1')
Join RemarkCode as cDel on cDel.ID = ISNULL(d.RemarkCode3ID,'1')
join RemarkCode as cUpd on cUpd.ID = ISNULL(i.RemarkCode3ID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update RemarkCode4ID
BEGIN TRY
if update(RemarkCode4ID)
--Select * into #t6 from RemarkCode
insert into PlanFollowupChargeAudit(PlanFollowupChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Remark Code 4ID',cDel.[Description],cUpd.[Description],d.RemarkCode4ID,i.RemarkCode4ID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RemarkCode4ID,'1') != ISNULL(d.RemarkCode4ID,'1')
Join RemarkCode as cDel on cDel.ID = ISNULL(d.RemarkCode4ID,'1')
join RemarkCode as cUpd on cUpd.ID = ISNULL(i.RemarkCode4ID,'1')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update TickleDate
BEGIN TRY
if update(TickleDate)
insert into PlanFollowupChargeAudit(PlanFollowupChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Tickle Date',d.TickleDate,i.TickleDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.TickleDate as datetime),'') != ISNULL(cast(d.TickleDate as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
END
Go


-- =============================================
-- Description:	<tgrVisitStatusAudit Trigger>
-- =============================================
IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrVisitStatusAudit')
DROP TRIGGER [dbo].[tgrVisitStatusAudit]
GO
Create TRIGGER [dbo].[tgrVisitStatusAudit]
ON [dbo].[VisitStatus] after Update
AS 
BEGIN
set xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM VisitStatusAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM VisitStatusAudit  

--when update VisitStatusID
BEGIN TRY
if update(VisitID)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Visit ID',d.VisitID,i.VisitID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.VisitID,'1') != ISNULL(d.VisitID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update DOS
BEGIN TRY
if update(DOS)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'DOS',d.DOS,i.DOS,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.DOS as datetime),'') != ISNULL(cast(d.DOS as datetime),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update VisitAmount
BEGIN TRY
if update(VisitAmount)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Visit Amount',d.VisitAmount,i.VisitAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.VisitAmount,'1') != ISNULL(d.VisitAmount,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ResponseEntity
BEGIN TRY
if update(ResponseEntity)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Response Entity',d.ResponseEntity,i.ResponseEntity,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ResponseEntity,'') != ISNULL(d.ResponseEntity,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PracticeID
BEGIN TRY
if update(PracticeID)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Practice ID',d.PracticeID,i.PracticeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PracticeID,'') != ISNULL(d.PracticeID,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update LocationID
BEGIN TRY
if update(LocationID)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Location ID',d.LocationID,i.LocationID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.LocationID,'') != ISNULL(d.LocationID,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ProviderID
BEGIN TRY
if update(ProviderID)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Provider ID',d.ProviderID,i.ProviderID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProviderID,'') != ISNULL(d.ProviderID,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PatientPlanID
BEGIN TRY
if update(PatientPlanID)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Plan ID',d.PatientPlanID,i.PatientPlanID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientPlanID,'') != ISNULL(d.PatientPlanID,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update SubmitterTRN
BEGIN TRY
if update(SubmitterTRN)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Submitter TRN',d.SubmitterTRN,i.SubmitterTRN,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubmitterTRN,'') != ISNULL(d.SubmitterTRN,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update TRNNumber
BEGIN TRY
if update(TRNNumber)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'TRN Number',d.TRNNumber,i.TRNNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.TRNNumber,'') != ISNULL(d.TRNNumber,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ActionCode
BEGIN TRY
if update(ActionCode)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Action Code',d.ActionCode,i.ActionCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ActionCode,'') != ISNULL(d.ActionCode,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update Status
BEGIN TRY
if update(Status)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Status',d.Status,i.Status,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Status,'') != ISNULL(d.Status,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update SubscriberLN
BEGIN TRY
if update(SubscriberLN)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Subscriber LN',(RTRIM(LTRIM(d.SubscriberLN))),(RTRIM(LTRIM(i.SubscriberLN))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.SubscriberLN)),'') != ISNULL(RTRIM(LTRIM(d.SubscriberLN)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update SubscriberFN
BEGIN TRY
if update(SubscriberFN)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Subscriber FN',(RTRIM(LTRIM(d.SubscriberFN))),(RTRIM(LTRIM(i.SubscriberFN))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.SubscriberFN)),'') != ISNULL(RTRIM(LTRIM(d.SubscriberFN)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update SubscriberMI
BEGIN TRY
if update(SubscriberMI)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Subscriber MI',(RTRIM(LTRIM(d.SubscriberMI))),(RTRIM(LTRIM(i.SubscriberMI))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.SubscriberMI)),'') != ISNULL(RTRIM(LTRIM(d.SubscriberMI)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update SubscriberID
BEGIN TRY
if update(SubscriberID)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Subscriber ID',d.SubscriberID,i.SubscriberID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubscriberID,'') != ISNULL(d.SubscriberID,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update SubscriberAddress
BEGIN TRY
if update(SubscriberAddress)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Subscriber Address',(RTRIM(LTRIM(d.SubscriberAddress))),(RTRIM(LTRIM(i.SubscriberAddress))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.SubscriberAddress)),'') != ISNULL(RTRIM(LTRIM(d.SubscriberAddress)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update SubscriberCity
BEGIN TRY
if update(SubscriberCity)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Subscriber City',(RTRIM(LTRIM(d.SubscriberCity))),(RTRIM(LTRIM(i.SubscriberCity))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.SubscriberCity)),'') != ISNULL(RTRIM(LTRIM(d.SubscriberCity)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update SubscriberState
BEGIN TRY
if update(SubscriberState)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Subscriber State',(RTRIM(LTRIM(d.SubscriberState))),(RTRIM(LTRIM(i.SubscriberState))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.SubscriberState)),'') != ISNULL(RTRIM(LTRIM(d.SubscriberState)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update SubscriberZip
BEGIN TRY
if update(SubscriberZip)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Subscriber Zip',d.SubscriberZip,i.SubscriberZip,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubscriberZip,'') != ISNULL(d.SubscriberZip,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update SubscriberDOB
BEGIN TRY
if update(SubscriberDOB)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Subscriber DOB',d.SubscriberDOB,i.SubscriberDOB,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.SubscriberDOB as datetime),'') != ISNULL(cast(d.SubscriberDOB as datetime),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update SubscriberGender
BEGIN TRY
if update(SubscriberGender)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Subscriber Gender',d.SubscriberGender,i.SubscriberGender,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubscriberGender,'') != ISNULL(d.SubscriberGender,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PatientLN
BEGIN TRY
if update(PatientLN)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient LN',(RTRIM(LTRIM(d.PatientLN))),(RTRIM(LTRIM(i.PatientLN))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PatientLN)),'') != ISNULL(RTRIM(LTRIM(d.PatientLN)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PatientFN
BEGIN TRY
if update(PatientFN)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient FN',(RTRIM(LTRIM(d.PatientFN))),(RTRIM(LTRIM(i.PatientFN))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PatientFN)),'') != ISNULL(RTRIM(LTRIM(d.PatientFN)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PatientMI
BEGIN TRY
if update(PatientMI)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient MI',(RTRIM(LTRIM(d.PatientMI))),(RTRIM(LTRIM(i.PatientMI))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PatientMI)),'') != ISNULL(RTRIM(LTRIM(d.PatientMI)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PatientAddress
BEGIN TRY
if update(PatientAddress)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Address',(RTRIM(LTRIM(d.PatientAddress))),(RTRIM(LTRIM(i.PatientAddress))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PatientAddress)),'') != ISNULL(RTRIM(LTRIM(d.PatientAddress)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PatientCity
BEGIN TRY
if update(PatientCity)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient City',(RTRIM(LTRIM(d.PatientCity))),(RTRIM(LTRIM(i.PatientCity))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PatientCity)),'') != ISNULL(RTRIM(LTRIM(d.PatientCity)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PatientState
BEGIN TRY
if update(PatientState)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient State',d.PatientState,i.PatientState,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientState,'') != ISNULL(d.PatientState,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PatientZip
BEGIN TRY
if update(PatientZip)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Zip',d.PatientZip,i.PatientZip,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientZip,'') != ISNULL(d.PatientZip,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PatientDOB
BEGIN TRY
if update(PatientDOB)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient DOB',d.PatientDOB,i.PatientDOB,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.PatientDOB as datetime),'') != ISNULL(cast(d.PatientDOB as datetime),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PatientGender
BEGIN TRY
if update(PatientGender)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Gender',d.PatientGender,i.PatientGender,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientGender,'') != ISNULL(d.PatientGender,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ProviderLN
BEGIN TRY
if update(ProviderLN)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Provider LN',(RTRIM(LTRIM(d.ProviderLN))),(RTRIM(LTRIM(i.ProviderLN))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.ProviderLN)),'') != ISNULL(RTRIM(LTRIM(d.ProviderLN)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ProviderFN
BEGIN TRY
if update(ProviderFN)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Provider FN',(RTRIM(LTRIM(d.ProviderFN))),(RTRIM(LTRIM(i.ProviderFN))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.ProviderFN)),'') != ISNULL(RTRIM(LTRIM(d.ProviderFN)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ProviderNPI
BEGIN TRY
if update(ProviderNPI)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Provider NPI',d.ProviderNPI,i.ProviderNPI,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProviderNPI,'') != ISNULL(d.ProviderNPI,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PayerName
BEGIN TRY
if update(PayerName)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer Name',(RTRIM(LTRIM(d.PayerName))),(RTRIM(LTRIM(i.PayerName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PayerName)),'') != ISNULL(RTRIM(LTRIM(d.PayerName)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--when update PayerID
BEGIN TRY
if update(PayerID)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer ID',d.PayerID,i.PayerID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PayerID,'') != ISNULL(d.PayerID,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ErrorMessage
BEGIN TRY
if update(ErrorMessage)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Error Message',d.ErrorMessage,i.ErrorMessage,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ErrorMessage,'') != ISNULL(d.ErrorMessage,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update CategoryCode1
BEGIN TRY
if update(CategoryCode1)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Category Code1',d.CategoryCode1,i.CategoryCode1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CategoryCode1,'') != ISNULL(d.CategoryCode1,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--when update CategoryCodeDesc1
BEGIN TRY
if update(CategoryCodeDesc1)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Category Code Desc1',d.CategoryCodeDesc1,i.CategoryCodeDesc1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CategoryCodeDesc1,'') != ISNULL(d.CategoryCodeDesc1,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update StatusCode1
BEGIN TRY
if update(StatusCode1)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Status Code1',d.StatusCode1,i.StatusCode1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.StatusCode1,'') != ISNULL(d.StatusCode1,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update StatusCodeDesc1
BEGIN TRY
if update(StatusCodeDesc1)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Status Code Desc1',d.StatusCodeDesc1,i.StatusCodeDesc1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.StatusCodeDesc1,'') != ISNULL(d.StatusCodeDesc1,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update EntityCode1
BEGIN TRY
if update(EntityCode1)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Entity Code1',d.EntityCode1,i.EntityCode1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.EntityCode1,'') != ISNULL(d.EntityCode1,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update EntityCodeDesc1
BEGIN TRY
if update(EntityCodeDesc1)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Entity Code Desc1',d.EntityCodeDesc1,i.EntityCodeDesc1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.EntityCodeDesc1,'') != ISNULL(d.EntityCodeDesc1,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update RejectionReason1
BEGIN TRY
if update(RejectionReason1)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Rejection Reason1',d.RejectionReason1,i.RejectionReason1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RejectionReason1,'') != ISNULL(d.RejectionReason1,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update FreeText1
BEGIN TRY
if update(FreeText1)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Free Text1',(RTRIM(LTRIM(d.FreeText1))),(RTRIM(LTRIM(i.FreeText1))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.FreeText1)),'') != ISNULL(RTRIM(LTRIM(d.FreeText1)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update CategoryCode2
BEGIN TRY
if update(CategoryCode2)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Category Code2',d.CategoryCode2,i.CategoryCode2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CategoryCode2,'') != ISNULL(d.CategoryCode2,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update CategoryCodeDesc2
BEGIN TRY
if update(CategoryCodeDesc2)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Category Code Desc2',d.CategoryCodeDesc2,i.CategoryCodeDesc2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CategoryCodeDesc2,'') != ISNULL(d.CategoryCodeDesc2,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--when update StatusCode2
BEGIN TRY
if update(StatusCode2)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Status Code2',d.StatusCode2,i.StatusCode2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.StatusCode2,'') != ISNULL(d.StatusCode2,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update StatusCodeDesc2
BEGIN TRY
if update(StatusCodeDesc2)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Status Code Desc2',d.StatusCodeDesc2,i.StatusCodeDesc2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.StatusCodeDesc2,'') != ISNULL(d.StatusCodeDesc2,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update EntityCode2
BEGIN TRY
if update(EntityCode2)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Entity Code2',d.EntityCode2,i.EntityCode2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.EntityCode2,'') != ISNULL(d.EntityCode2,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update EntityCodeDesc2
BEGIN TRY
if update(EntityCodeDesc2)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Entity Code Desc2',d.EntityCodeDesc2,i.EntityCodeDesc2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.EntityCodeDesc2,'') != ISNULL(d.EntityCodeDesc2,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update RejectionReason2
BEGIN TRY
if update(RejectionReason2)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Rejection Reason2',d.RejectionReason2,i.RejectionReason2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RejectionReason2,'') != ISNULL(d.RejectionReason2,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update FreeText2
BEGIN TRY
if update(FreeText2)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Free Text2',(RTRIM(LTRIM(d.FreeText2))),(RTRIM(LTRIM(i.FreeText2))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.FreeText2)),'') != ISNULL(RTRIM(LTRIM(d.FreeText2)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update CategoryCode3
BEGIN TRY
if update(CategoryCode3)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Category Code3',d.CategoryCode3,i.CategoryCode3,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CategoryCode3,'') != ISNULL(d.CategoryCode3,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update CategoryCodeDesc3
BEGIN TRY
if update(CategoryCodeDesc3)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Category Code Desc3',d.CategoryCodeDesc3,i.CategoryCodeDesc3,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CategoryCodeDesc3,'') != ISNULL(d.CategoryCodeDesc3,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update StatusCode3
BEGIN TRY
if update(StatusCode3)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Status Code3',d.StatusCode3,i.StatusCode3,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.StatusCode3,'') != ISNULL(d.StatusCode3,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update StatusCodeDesc3
BEGIN TRY
if update(StatusCodeDesc3)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Status Code Desc3',d.StatusCodeDesc3,i.StatusCodeDesc3,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.StatusCodeDesc3,'') != ISNULL(d.StatusCodeDesc3,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--when update EntityCode3
BEGIN TRY
if update(EntityCode3)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Entity Code3',d.EntityCode3,i.EntityCode3,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.EntityCode3,'') != ISNULL(d.EntityCode3,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update EntityCodeDesc3
BEGIN TRY
if update(EntityCodeDesc3)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Entity Code Desc3',d.EntityCodeDesc3,i.EntityCodeDesc3,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.EntityCodeDesc3,'') != ISNULL(d.EntityCodeDesc3,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update RejectionReason3
BEGIN TRY
if update(RejectionReason3)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Rejection Reason3',d.RejectionReason3,i.RejectionReason3,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RejectionReason3,'') != ISNULL(d.RejectionReason3,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--when update FreeText3
BEGIN TRY
if update(FreeText3)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Free Text3',(RTRIM(LTRIM(d.FreeText3))),(RTRIM(LTRIM(i.FreeText3))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.FreeText3)),'') != ISNULL(RTRIM(LTRIM(d.FreeText3)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PayerControlNumber
BEGIN TRY
if update(PayerControlNumber)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer Control Number',d.PayerControlNumber,i.PayerControlNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PayerControlNumber,'') != ISNULL(d.PayerControlNumber,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update StatusDate
BEGIN TRY
if update(StatusDate)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Status Date',d.StatusDate,i.StatusDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.StatusDate as datetime),'') != ISNULL(cast(d.StatusDate as datetime),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update BilledAmount
BEGIN TRY
if update(BilledAmount)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Billed Amount',d.BilledAmount,i.BilledAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.BilledAmount,'1') != ISNULL(d.BilledAmount,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--when update PaidAmount
BEGIN TRY
if update(PaidAmount)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Paid Amount',d.PaidAmount,i.PaidAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaidAmount,'1') != ISNULL(d.PaidAmount,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update CheckDate
BEGIN TRY
if update(CheckDate)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Check Date',d.CheckDate,i.CheckDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.CheckDate as datetime),'') != ISNULL(cast(d.CheckDate as datetime),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update CheckNumber
BEGIN TRY
if update(CheckNumber)
insert into VisitStatusAudit(VisitStatusID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Check Number',d.CheckNumber,i.CheckNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CheckNumber,'') != ISNULL(d.CheckNumber,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;
END
Go



-- =============================================
-- Description:	<tgrPatientEligibilityAudit Trigger>
-- =============================================
IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrPatientEligibilityAudit')
DROP TRIGGER [dbo].[tgrPatientEligibilityAudit]
GO
Create TRIGGER [dbo].[tgrPatientEligibilityAudit]
ON [dbo].[PatientEligibility] after Update
AS 
BEGIN
set xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM PatientEligibilityAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM PatientEligibilityAudit  

--when update EligibilityDate
BEGIN TRY
if update(EligibilityDate)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Eligibility Date',d.EligibilityDate,i.EligibilityDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.EligibilityDate as datetime),'') != ISNULL(cast(d.EligibilityDate as datetime),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update DOS
BEGIN TRY
if update(DOS)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'DOS',d.DOS,i.DOS,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.DOS as datetime),'') != ISNULL(cast(d.DOS as datetime),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PatientID
BEGIN TRY
if update(PatientID)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient ID',d.PatientID,i.PatientID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientID,'1') != ISNULL(d.PatientID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PracticeID
BEGIN TRY
if update(PracticeID)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Practice ID',d.PracticeID,i.PracticeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PracticeID,'1') != ISNULL(d.PracticeID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update LocationID
BEGIN TRY
if update(LocationID)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Location ID',d.LocationID,i.LocationID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.LocationID,'1') != ISNULL(d.LocationID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ProviderID
BEGIN TRY
if update(ProviderID)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Provider ID',d.ProviderID,i.ProviderID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProviderID,'1') != ISNULL(d.ProviderID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PatientPlanID
BEGIN TRY
if update(PatientPlanID)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Plan ID',d.PatientPlanID,i.PatientPlanID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientPlanID,'1') != ISNULL(d.PatientPlanID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update TRNNumber
BEGIN TRY
if update(TRNNumber)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'TRN Number',d.TRNNumber,i.TRNNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.TRNNumber,'') != ISNULL(d.TRNNumber,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update Relation
BEGIN TRY
if update(Relation)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Relation',d.Relation,i.Relation,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Relation,'') != ISNULL(d.Relation,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update Status
BEGIN TRY
if update(Status)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Status',d.Status,i.Status,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Status,'') != ISNULL(d.Status,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update Rejection
BEGIN TRY
if update(Rejection)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Rejection',d.Rejection,i.Rejection,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Rejection,'') != ISNULL(d.Rejection,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update RejectionCode
BEGIN TRY
if update(RejectionCode)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Rejection Code',d.RejectionCode,i.RejectionCode,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.RejectionCode,'') != ISNULL(d.RejectionCode,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update SubscriberLN
BEGIN TRY
if update(SubscriberLN)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Subscriber LN',(RTRIM(LTRIM(d.SubscriberLN))),(RTRIM(LTRIM(i.SubscriberLN))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.SubscriberLN)),'') != ISNULL(RTRIM(LTRIM(d.SubscriberLN)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update SubscriberFN
BEGIN TRY
if update(SubscriberFN)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Subscriber FN',(RTRIM(LTRIM(d.SubscriberFN))),(RTRIM(LTRIM(i.SubscriberFN))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.SubscriberFN)),'') != ISNULL(RTRIM(LTRIM(d.SubscriberFN)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update SubscriberMI
BEGIN TRY
if update(SubscriberMI)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Subscriber MI',(RTRIM(LTRIM(d.SubscriberMI))),(RTRIM(LTRIM(i.SubscriberMI))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.SubscriberMI)),'') != ISNULL(RTRIM(LTRIM(d.SubscriberMI)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update SubscriberID
BEGIN TRY
if update(SubscriberID)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Subscriber ID',d.SubscriberID,i.SubscriberID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubscriberID,'') != ISNULL(d.SubscriberID,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update SubscriberGroupNumber
BEGIN TRY
if update(SubscriberGroupNumber)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Subscriber Group Number',d.SubscriberGroupNumber,i.SubscriberGroupNumber,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubscriberGroupNumber,'') != ISNULL(d.SubscriberGroupNumber,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update SubscriberAddress
BEGIN TRY
if update(SubscriberAddress)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Subscriber Address',(RTRIM(LTRIM(d.SubscriberAddress))),(RTRIM(LTRIM(i.SubscriberAddress))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.SubscriberAddress)),'') != ISNULL(RTRIM(LTRIM(d.SubscriberAddress)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update SubscriberCity
BEGIN TRY
if update(SubscriberCity)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Subscriber City',(RTRIM(LTRIM(d.SubscriberCity))),(RTRIM(LTRIM(i.SubscriberCity))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.SubscriberCity)),'') != ISNULL(RTRIM(LTRIM(d.SubscriberCity)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update SubscriberState
BEGIN TRY
if update(SubscriberState)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Subscriber State',(RTRIM(LTRIM(d.SubscriberState))),(RTRIM(LTRIM(i.SubscriberState))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.SubscriberState)),'') != ISNULL(RTRIM(LTRIM(d.SubscriberState)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update SubscriberZip
BEGIN TRY
if update(SubscriberZip)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Subscriber Zip',d.SubscriberZip,i.SubscriberZip,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubscriberZip,'') != ISNULL(d.SubscriberZip,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update SubscriberDOB
BEGIN TRY
if update(SubscriberDOB)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Subscriber DOB',d.SubscriberDOB,i.SubscriberDOB,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.SubscriberDOB as datetime),'') != ISNULL(cast(d.SubscriberDOB as datetime),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update SubscriberGender
BEGIN TRY
if update(SubscriberGender)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Subscriber Gender',d.SubscriberGender,i.SubscriberGender,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.SubscriberGender,'') != ISNULL(d.SubscriberGender,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PatientLN
BEGIN TRY
if update(PatientLN)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient LN',d.PatientLN,i.PatientLN,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientLN,'') != ISNULL(d.PatientLN,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PatientFN
BEGIN TRY
if update(PatientFN)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient FN',d.PatientFN,i.PatientFN,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientFN,'') != ISNULL(d.PatientFN,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PatientMI
BEGIN TRY
if update(PatientMI)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient MI',d.PatientMI,i.PatientMI,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientMI,'') != ISNULL(d.PatientMI,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PatientAddress
BEGIN TRY
if update(PatientAddress)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Address',d.PatientAddress,i.PatientAddress,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientAddress,'') != ISNULL(d.PatientAddress,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PatientCity
BEGIN TRY
if update(PatientCity)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient City',d.PatientCity,i.PatientCity,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientCity,'') != ISNULL(d.PatientCity,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PatientState
BEGIN TRY
if update(PatientState)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient State',d.PatientState,i.PatientState,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientState,'') != ISNULL(d.PatientState,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PatientZip
BEGIN TRY
if update(PatientZip)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Zip',d.PatientZip,i.PatientZip,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientZip,'') != ISNULL(d.PatientZip,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PatientDOB
BEGIN TRY
if update(PatientDOB)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient DOB',d.PatientDOB,i.PatientDOB,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.PatientDOB as datetime),'') != ISNULL(cast(d.PatientDOB as datetime),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PatientGender
BEGIN TRY
if update(PatientGender)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Gender',d.PatientGender,i.PatientGender,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientGender,'') != ISNULL(d.PatientGender,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ProviderLN
BEGIN TRY
if update(ProviderLN)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Provider LN',d.ProviderLN,i.ProviderLN,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProviderLN,'') != ISNULL(d.ProviderLN,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ProviderFN
BEGIN TRY
if update(ProviderFN)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Provider FN',d.ProviderFN,i.ProviderFN,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProviderFN,'') != ISNULL(d.ProviderFN,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ProviderNPI
BEGIN TRY
if update(ProviderNPI)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Provider NPI',d.ProviderNPI,i.ProviderNPI,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ProviderNPI,'') != ISNULL(d.ProviderNPI,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PayerName
BEGIN TRY
if update(PayerName)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer Name',(RTRIM(LTRIM(d.PayerName))),(RTRIM(LTRIM(i.PayerName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PayerName)),'') != ISNULL(RTRIM(LTRIM(d.PayerName)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PayerID
if update(PayerID)
BEGIN TRY
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payer ID',d.PayerID,i.PayerID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PayerID,'') != ISNULL(d.PayerID,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ErrorMessage
BEGIN TRY
if update(ErrorMessage)
insert into PatientEligibilityAudit(PatientEligibilityID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Error Message',(RTRIM(LTRIM(d.ErrorMessage))),(RTRIM(LTRIM(i.ErrorMessage))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.ErrorMessage)),'') != ISNULL(RTRIM(LTRIM(d.ErrorMessage)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;
END
GO



-- =============================================
-- Description:	<tgrPatientEligibilityLogAudit Trigger>
-- =============================================
IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrPatientEligibilityLogAudit')
DROP TRIGGER [dbo].[tgrPatientEligibilityLogAudit]
GO
Create TRIGGER [dbo].[tgrPatientEligibilityLogAudit]
ON [dbo].[PatientEligibilityLog] after Update
AS 
BEGIN
set xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM PatientEligibilityLogAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM PatientEligibilityLogAudit  

--when update PatientEligibilityID
BEGIN TRY
if update(PatientEligibilityID)
insert into PatientEligibilityLogAudit(PatientEligibilityLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Eligibility ID',d.PatientEligibilityID,i.PatientEligibilityID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientEligibilityID,'1') != ISNULL(d.PatientEligibilityID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update Transaction270Path
BEGIN TRY
if update(Transaction270Path)
insert into PatientEligibilityLogAudit(PatientEligibilityLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Transaction270 Path',d.Transaction270Path,i.Transaction270Path,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Transaction270Path,'') != ISNULL(d.Transaction270Path,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update Transaction271Path
BEGIN TRY
if update(Transaction271Path)
insert into PatientEligibilityLogAudit(PatientEligibilityLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Transaction271 Path',d.Transaction271Path,i.Transaction271Path,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Transaction271Path,'') != ISNULL(d.Transaction271Path,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update Transaction999Path
BEGIN TRY
if update(Transaction999Path)
insert into PatientEligibilityLogAudit(PatientEligibilityLogID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Transaction999 Path',d.Transaction999Path,i.Transaction999Path,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Transaction999Path,'') != ISNULL(d.Transaction999Path,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;
END
GO



-- =============================================
-- Description:	<tgrPatientFollowUpChargeAudit Trigger>
-- =============================================
IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrPatientFollowUpChargeAudit')
DROP TRIGGER [dbo].[tgrPatientFollowUpChargeAudit]
GO
Create TRIGGER [dbo].[tgrPatientFollowUpChargeAudit]
ON [dbo].[PatientFollowUpCharge] after Update
AS 
BEGIN
set xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM PatientFollowUpChargeAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM PatientFollowUpChargeAudit  

--when update PatientFollowUpID
BEGIN TRY
if update(PatientFollowUpID)
insert into PatientFollowUpChargeAudit(PatientFollowUpChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Follow Up ID',d.PatientFollowUpID,i.PatientFollowUpID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientFollowUpID,'1') != ISNULL(d.PatientFollowUpID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ChargeID
BEGIN TRY
if update(ChargeID)
insert into PatientFollowUpChargeAudit(PatientFollowUpChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Charge ID',d.ChargeID,i.ChargeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ChargeID,'1') != ISNULL(d.ChargeID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ReasonID
BEGIN TRY
if update(ReasonID)
--Select * into #t2 from Reason
insert into PatientFollowUpChargeAudit(PatientFollowUpChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reason ID',cDel.[Name],cUpd.[Name],d.ReasonID,i.ReasonID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReasonID,'1') != ISNULL(d.ReasonID,'1')
Join Reason as cDel on cDel.ID = ISNULL(d.ReasonID,'1')
join Reason as cUpd on cUpd.ID = ISNULL(i.ReasonID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ActionID
BEGIN TRY
if update(ActionID)
--Select * into #t3 from Action
insert into PatientFollowUpChargeAudit(PatientFollowUpChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Action ID',cDel.[Name],cUpd.[Name],d.ActionID,i.ActionID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ActionID,'1') != ISNULL(d.ActionID,'1')
Join Action as cDel on cDel.ID = ISNULL(d.ActionID,'1')
join Action as cUpd on cUpd.ID = ISNULL(i.ActionID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update GroupID
BEGIN TRY
if update(GroupID)
--Select * into #t4 from [Group]
insert into PatientFollowUpChargeAudit(PatientFollowUpChargeID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Group ID',cDel.[Name],cUpd.[Name],d.GroupID,i.GroupID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.GroupID,'1') != ISNULL(d.GroupID,'1')
Join [Group] as cDel on cDel.ID = ISNULL(d.GroupID,'1')
join [Group] as cUpd on cUpd.ID = ISNULL(i.GroupID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update AdjustmentCodeID
BEGIN TRY
if update(AdjustmentCodeID)
insert into PatientFollowUpChargeAudit(PatientFollowUpChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Adjustment Code ID',d.AdjustmentCodeID,i.AdjustmentCodeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.AdjustmentCodeID,'1') != ISNULL(d.AdjustmentCodeID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PaymentChargeID
BEGIN TRY
if update(PaymentChargeID)
insert into PatientFollowUpChargeAudit(PatientFollowUpChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Payment Charge ID',d.PaymentChargeID,i.PaymentChargeID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PaymentChargeID,'1') != ISNULL(d.PaymentChargeID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update Status
BEGIN TRY
if update(Status)
insert into PatientFollowUpChargeAudit(PatientFollowUpChargeID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Status',d.Status,i.Status,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.Status,'') != ISNULL(d.Status,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;
END
GO


--=============================================
-- Description:	<tgrPatientEligibilityDetailAudit Trigger>
-- =============================================
IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrPatientEligibilityDetailAudit')
DROP TRIGGER [dbo].[tgrPatientEligibilityDetailAudit]
GO
Create TRIGGER [dbo].[tgrPatientEligibilityDetailAudit]
ON [dbo].[PatientEligibilityDetail] after Update
AS 
BEGIN
SET XACT_ABORT off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM PatientEligibilityDetailAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM PatientEligibilityDetailAudit  

--when update PatientEligibilityID
BEGIN TRY
if update(PatientEligibilityID)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Patient Eligibility ID',d.PatientEligibilityID,i.PatientEligibilityID,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PatientEligibilityID,'1') != ISNULL(d.PatientEligibilityID,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update Coverage
BEGIN TRY
if update(Coverage)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Coverage',(RTRIM(LTRIM(d.Coverage))),(RTRIM(LTRIM(i.Coverage))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.Coverage)),'') != ISNULL(RTRIM(LTRIM(d.Coverage)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update CoverageLevel
BEGIN TRY
if update(CoverageLevel)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Coverage Level',d.CoverageLevel,i.CoverageLevel,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.CoverageLevel,'') != ISNULL(d.CoverageLevel,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ServiceTypes
BEGIN TRY
if update(ServiceTypes)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Service Types',d.ServiceTypes,i.ServiceTypes,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ServiceTypes,'') != ISNULL(d.ServiceTypes,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PlanName
BEGIN TRY
if update(PlanName)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Plan Name',(RTRIM(LTRIM(d.PlanName))),(RTRIM(LTRIM(i.PlanName))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PlanName)),'') != ISNULL(RTRIM(LTRIM(d.PlanName)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PlanDescription
BEGIN TRY
if update(PlanDescription)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Plan Description',(RTRIM(LTRIM(d.PlanDescription))),(RTRIM(LTRIM(i.PlanDescription))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PlanDescription)),'') != ISNULL(RTRIM(LTRIM(d.PlanDescription)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update TimePeriod
BEGIN TRY
if update(TimePeriod)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Time Period',d.TimePeriod,i.TimePeriod,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.TimePeriod,'') != ISNULL(d.TimePeriod,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update BenefitAmount
BEGIN TRY
if update(BenefitAmount)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Benefit Amount',d.BenefitAmount,i.BenefitAmount,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.BenefitAmount,'1') != ISNULL(d.BenefitAmount,'1')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update BenefitPercentage
BEGIN TRY
if update(BenefitPercentage)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Benefit Percentage',d.BenefitPercentage,i.BenefitPercentage,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.BenefitPercentage,'') != ISNULL(d.BenefitPercentage,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update Authorization
BEGIN TRY
if update([Authorization])
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Authorization',d.[Authorization],i.[Authorization],HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.[Authorization],'') != ISNULL(d.[Authorization],'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update PlanNetwork
BEGIN TRY
if update(PlanNetwork)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Plan Network',d.PlanNetwork,i.PlanNetwork,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.PlanNetwork,'') != ISNULL(d.PlanNetwork,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update Messages
BEGIN TRY
if update([Messages])
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Messages',(RTRIM(LTRIM(d.[Messages]))),(RTRIM(LTRIM(i.[Messages]))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.[Messages])),'') != ISNULL(RTRIM(LTRIM(d.[Messages])),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ReferenceId1
BEGIN TRY
if update(ReferenceId1)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reference Id 1',d.ReferenceId1,i.ReferenceId1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReferenceId1,'') != ISNULL(d.ReferenceId1,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ReferenceValue1
BEGIN TRY
if update(ReferenceValue1)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reference Value 1',d.ReferenceValue1,i.ReferenceValue1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReferenceValue1,'') != ISNULL(d.ReferenceValue1,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ReferenceId2
BEGIN TRY
if update(ReferenceId2)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reference Id 2',d.ReferenceId2,i.ReferenceId2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReferenceId2,'') != ISNULL(d.ReferenceId2,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ReferenceValue2
BEGIN TRY
if update(ReferenceValue2)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reference Value 2',d.ReferenceValue2,i.ReferenceValue2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReferenceValue2,'') != ISNULL(d.ReferenceValue2,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ReferenceId3
BEGIN TRY
if update(ReferenceId3)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reference Id 3',d.ReferenceId3,i.ReferenceId3,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReferenceId3,'') != ISNULL(d.ReferenceId3,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ReferenceValue3
BEGIN TRY
if update(ReferenceValue3)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reference Value 3',d.ReferenceValue3,i.ReferenceValue3,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReferenceValue3,'') != ISNULL(d.ReferenceValue3,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ReferenceId4
BEGIN TRY
if update(ReferenceId4)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reference Id 4',d.ReferenceId4,i.ReferenceId4,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReferenceId4,'') != ISNULL(d.ReferenceId4,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ReferenceValue4
BEGIN TRY
if update(ReferenceValue4)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reference Value 4',d.ReferenceValue4,i.ReferenceValue4,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReferenceValue4,'') != ISNULL(d.ReferenceValue4,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ReferenceId5
BEGIN TRY
if update(ReferenceId5)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reference Id 5',d.ReferenceId5,i.ReferenceId5,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReferenceId5,'') != ISNULL(d.ReferenceId5,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update ReferenceValue5.
BEGIN TRY
if update(ReferenceValue5)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Reference Value 5',d.ReferenceValue5,i.ReferenceValue5,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReferenceValue5,'') != ISNULL(d.ReferenceValue5,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update DateId1
BEGIN TRY
if update(DateId1)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Date Id 1',d.DateId1,i.DateId1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DateId1,'') != ISNULL(d.DateId1,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update DateValue1
BEGIN TRY
if update(DateValue1)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Date Value 1',d.DateValue1,i.DateValue1,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DateValue1,'') != ISNULL(d.DateValue1,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update DateId2
BEGIN TRY
if update(DateId2)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Date Id 2',d.DateId2,i.DateId2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DateId2,'') != ISNULL(d.DateId2,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update DateValue2
BEGIN TRY
if update(DateValue2)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Date Value 2',d.DateValue2,i.DateValue2,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DateValue2,'') != ISNULL(d.DateValue2,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update DateId3
BEGIN TRY
if update(DateId3)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Date Id 3',d.DateId3,i.DateId3,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DateId3,'') != ISNULL(d.DateId3,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update DateValue3
BEGIN TRY
if update(DateValue3)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Date Value 3',d.DateValue3,i.DateValue3,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DateValue3,'') != ISNULL(d.DateValue3,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update DateId4
BEGIN TRY
if update(DateId4)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Date Id 4',d.DateId4,i.DateId4,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DateId4,'') != ISNULL(d.DateId4,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update DateValue4
BEGIN TRY
if update(DateValue4)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Date Value 4',d.DateValue4,i.DateValue4,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DateValue4,'') != ISNULL(d.DateValue4,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update DateId5
BEGIN TRY
if update(DateId5)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Date Id 5',d.DateId5,i.DateId5,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DateId5,'') != ISNULL(d.DateId5,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;

--when update DateValue5
BEGIN TRY
if update(DateValue5)
insert into PatientEligibilityDetailAudit(PatientEligibilityDetailID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Date Value 5',d.DateValue5,i.DateValue5,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.DateValue5,'') != ISNULL(d.DateValue5,'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;
End
GO


--=====================================================
--Description: <tgrInsuranceBillingoptionAudit Trigger>
--=====================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrInsuranceBillingoptionAudit')
DROP TRIGGER [dbo].[tgrInsuranceBillingoptionAudit]
GO
create TRIGGER [dbo].[tgrInsuranceBillingoptionAudit]
ON [dbo].[InsuranceBillingoption] after Update
AS 
BEGIN
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM InsuranceBillingoptionAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM InsuranceBillingoptionAudit

--When User Update ProviderID
BEGIN TRY
IF Update(ProviderID)
--select * Into #t1 from Receiver
INSERT INTO InsuranceBillingoptionAudit(InsuranceBillingoptionID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Provider ID',pDel.[Name],pUpd.[Name],d.ProviderID,i.ProviderID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.ProviderID,'1') != ISNULL(d.ProviderID,'1')
Join [Provider] as pDel on pdel.ID = ISNULL(d.ProviderID,'1')
join [Provider] as pUpd on pUpd.ID = ISNULL(i.ProviderID,'1')
END TRY  
BEGIN CATCH  
 --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update LocationID
BEGIN TRY
IF Update(LocationID)
--select * Into #t1 from Receiver
INSERT INTO InsuranceBillingoptionAudit(InsuranceBillingoptionID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Location ID',pDel.[Name],pUpd.[Name],d.LocationID,i.LocationID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.LocationID,'1') != ISNULL(d.LocationID,'1')
Join [Location] as pDel on pdel.ID = ISNULL(d.LocationID,'1')
join [Location] as pUpd on pUpd.ID = ISNULL(i.LocationID,'1')
END TRY  
BEGIN CATCH  
 --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;


--When User Update InsurancePlanID
BEGIN TRY
IF Update(InsurancePlanID)
--select * Into #t1 from Receiver
INSERT INTO InsuranceBillingoptionAudit(InsuranceBillingoptionID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Insurance Plan ID',pDel.[Description],pUpd.[Description],d.InsurancePlanID,i.InsurancePlanID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.InsurancePlanID,'1') != ISNULL(d.InsurancePlanID,'1')
Join InsurancePlan as pDel on pdel.ID = ISNULL(d.InsurancePlanID,'1')
join InsurancePlan as pUpd on pUpd.ID = ISNULL(i.InsurancePlanID,'1')
END TRY  
BEGIN CATCH  
 --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH;

--when update ReportTaxID
BEGIN TRY
if update(ReportTaxID)
insert into InsuranceBillingoptionAudit(InsuranceBillingoptionID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Report Tax ID',(case when d.ReportTaxID=1 then 'Yes' when d.ReportTaxID=0 then 'NO' End),(case when i.ReportTaxID=1 then 'Yes' when i.ReportTaxID= 0 then 'NO' END),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(i.ReportTaxID,'2') != ISNULL(d.ReportTaxID,'2')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--when update PayToAddress
BEGIN TRY
if update(PayToAddress)
insert into InsuranceBillingoptionAudit(InsuranceBillingoptionID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'PayToAddress',(RTRIM(LTRIM(d.PayToAddress))),(RTRIM(LTRIM(i.PayToAddress))),HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(RTRIM(LTRIM(i.PayToAddress)),'') != ISNULL(RTRIM(LTRIM(d.PayToAddress)),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;
END
GO


--=====================================================
--Description: <tgrOnlinePortalsAudit Trigger>
--=====================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrOnlinePortalsAudit')
DROP TRIGGER [dbo].[tgrOnlinePortalsAudit]
GO
create TRIGGER [dbo].[tgrOnlinePortalsAudit]
ON [dbo].[OnlinePortals] after Update
AS 
BEGIN
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM OnlinePortalsAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM OnlinePortalsAudit

--When User Update InsurancePlanID
BEGIN TRY
IF Update(InsurancePlanID)

INSERT INTO OnlinePortalsAudit(OnlinePortalsID,TransactionID,ColumnName,CurrentValue,NewValue,CurrentValueID,NewValueID,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Insurance Plan ID',pDel.[PlanName],pUpd.[PlanName],d.InsurancePlanID,i.InsurancePlanID,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.InsurancePlanID,'1') != ISNULL(d.InsurancePlanID,'1')
Join [InsurancePlan] as pDel on pdel.ID = ISNULL(d.InsurancePlanID,'1')
join [InsurancePlan] as pUpd on pUpd.ID = ISNULL(i.InsurancePlanID,'1')
END TRY  
BEGIN CATCH  
 --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 


--When User Update Name
BEGIN TRY
IF Update(Name)
--select * Into #t1 from Receiver
INSERT INTO OnlinePortalsAudit(OnlinePortalsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Name',d.Name,i.Name,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Name,'1') != ISNULL(d.Name,'1')
END TRY  
BEGIN CATCH  
 --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update URL
BEGIN TRY
IF Update(URL)
--select * Into #t1 from Receiver
INSERT INTO OnlinePortalsAudit(OnlinePortalsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'URL',d.URL,i.URL,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.URL,'1') != ISNULL(d.URL,'1')
END TRY  
BEGIN CATCH  
 --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 


--When User Update Type
BEGIN TRY
IF Update(Type)
--select * Into #t1 from Receiver
INSERT INTO OnlinePortalsAudit(OnlinePortalsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID, @TransactionID,'Type',d.Type,i.Type,HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Type,'1') != ISNULL(d.Type,'1')
END TRY  
BEGIN CATCH  
 --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
END
GO

--=====================================================
--Description: <tgrOnlinePortalCredentialsAudit Trigger>
--=====================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE TYPE = 'TR' AND NAME = 'tgrOnlinePortalCredentialsAudit')
DROP TRIGGER [dbo].[tgrOnlinePortalCredentialsAudit]
GO
create TRIGGER [dbo].[tgrOnlinePortalCredentialsAudit]
ON [dbo].[OnlinePortalCredentials] after Update
AS 
BEGIN
set Xact_abort off
DECLARE @TransactionID INT;
SELECT @TransactionID =  max(TransactionID) FROM OnlinePortalCredentialsAudit
IF @TransactionID is null set @TransactionID = 1 
ELSE
SELECT @TransactionID =  max(TransactionID) +1 FROM OnlinePortalCredentialsAudit

--When User Update Username
BEGIN TRY
IF Update(Username)
INSERT INTO OnlinePortalCredentialsAudit(OnlinePortalCredentialsID,OnlinePortalsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID,i.OnlinePortalsID, @TransactionID,'Username',d.[Username],i.[Username],HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Username,'1') != ISNULL(d.Username,'1')
END TRY  
BEGIN CATCH  
 --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update Password
BEGIN TRY
IF Update(Password)
INSERT INTO OnlinePortalCredentialsAudit(OnlinePortalCredentialsID,OnlinePortalsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID,i.OnlinePortalsID, @TransactionID,'Password',d.[Password],i.[Password],HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Password,'1') != ISNULL(d.Password,'1')
END TRY  
BEGIN CATCH  
 --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 


--When User Update PasswordExpiryDate
BEGIN TRY
IF Update(PasswordExpiryDate)
INSERT INTO OnlinePortalCredentialsAudit(OnlinePortalCredentialsID,OnlinePortalsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID,i.OnlinePortalsID ,@TransactionID,'Password Expiry Date',d.PasswordExpiryDate,i.PasswordExpiryDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(cast(i.PasswordExpiryDate as date),'') != ISNULL(cast(d.PasswordExpiryDate as date),'')
END TRY  
BEGIN CATCH  
  --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;   
END CATCH;


--When User Update SercurityQ1
BEGIN TRY
IF Update(SercurityQ1)
INSERT INTO OnlinePortalCredentialsAudit(OnlinePortalCredentialsID,OnlinePortalsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID,i.OnlinePortalsID, @TransactionID,'Sercurity Q1',d.[SercurityQ1],i.[SercurityQ1],HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SercurityQ1,'1') != ISNULL(d.SercurityQ1,'1')
END TRY  
BEGIN CATCH  
 --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update SecurityA1
BEGIN TRY
IF Update(SecurityA1)
INSERT INTO OnlinePortalCredentialsAudit(OnlinePortalCredentialsID,OnlinePortalsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID,i.OnlinePortalsID ,@TransactionID,'Security A1',d.[SecurityA1],i.[SecurityA1],HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecurityA1,'1') != ISNULL(d.SecurityA1,'1')
END TRY  
BEGIN CATCH  
 --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--When User Update SecurityQ2
BEGIN TRY
IF Update(SecurityQ2)
INSERT INTO OnlinePortalCredentialsAudit(OnlinePortalCredentialsID,OnlinePortalsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID,i.OnlinePortalsID, @TransactionID,'Security Q2',d.[SecurityQ2],i.[SecurityQ2],HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecurityQ2,'1') != ISNULL(d.SecurityQ2,'1')
END TRY  
BEGIN CATCH  
 --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 


--When User Update SecurityA2
BEGIN TRY
IF Update(SecurityA2)
INSERT INTO OnlinePortalCredentialsAudit(OnlinePortalCredentialsID,i.OnlinePortalsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID,i.OnlinePortalsID, @TransactionID,'Security A2',d.[SecurityA2],i.[SecurityA2],HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecurityA2,'1') != ISNULL(d.SecurityA2,'1')
END TRY  
BEGIN CATCH  
 --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 


--When User Update SecurityQ3
BEGIN TRY
IF Update(SecurityQ3)
INSERT INTO OnlinePortalCredentialsAudit(OnlinePortalCredentialsID,OnlinePortalsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID,i.OnlinePortalsID ,@TransactionID,'Security Q3',d.[SecurityQ3],i.[SecurityQ3],HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecurityQ3,'1') != ISNULL(d.SecurityQ3,'1')
END TRY  
BEGIN CATCH  
 --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 


--When User Update SecurityA3
BEGIN TRY
IF Update(SecurityA3)
INSERT INTO OnlinePortalCredentialsAudit(OnlinePortalCredentialsID,OnlinePortalsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID,i.OnlinePortalsID, @TransactionID,'Security A3',d.[SecurityA3],i.[SecurityA3],HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.SecurityA3,'1') != ISNULL(d.SecurityA3,'1')
END TRY  
BEGIN CATCH  
 --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 


--When User Update Notes
BEGIN TRY
IF Update(Notes)
INSERT INTO OnlinePortalCredentialsAudit(OnlinePortalCredentialsID,OnlinePortalsID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
SELECT  i.ID,i.OnlinePortalsID, @TransactionID,'Notes',d.[Notes],i.[Notes],HOST_NAME(),i.UpdatedBy,CURRENT_TIMESTAMP
FROM inserted as i
INNER JOIN Deleted as d ON i.ID = d.ID and ISNULL(i.Notes,'1') != ISNULL(d.Notes,'1')
END TRY  
BEGIN CATCH  
 --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 
END
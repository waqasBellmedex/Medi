
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
select i.ID, @TransactionID,'Responsible Party',Case when d.ResponsibleParty = 'B' then 'Bellmedex' when d.ResponsibleParty = 'C' then 'Client' end,Case when i.ResponsibleParty = 'B' then 'Bellmedex' when i.ResponsibleParty = 'C' then 'Client' end,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
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

--when update StartDate
BEGIN TRY
if update(StartDate)
insert into BatchDocumentAudit(BatchDocumentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'Start Date',d.StartDate,i.StartDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.StartDate as datetime),'') != ISNULL(cast(d.StartDate as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

--when update EndDate
BEGIN TRY
if update(EndDate)
insert into BatchDocumentAudit(BatchDocumentID,TransactionID,ColumnName,CurrentValue,NewValue,HostName,AddedBy,AddedDate)
select i.ID, @TransactionID,'End Date',d.EndDate,i.EndDate,HOST_NAME(),i.UpdatedBy ,CURRENT_TIMESTAMP
from inserted as i
inner join deleted as d on i.ID = d.ID and ISNULL(cast(i.EndDate as datetime),'') != ISNULL(cast(d.EndDate as datetime),'')
END TRY  
BEGIN CATCH  
    --SELECT   
     print ERROR_NUMBER(); --AS ErrorNumber  
     print ERROR_MESSAGE(); --AS ErrorMessage;  
END CATCH; 

END
GO


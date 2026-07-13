SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


ALTER   Procedure dbo.Ach_File_Get_Data
	@BankID Int,
	@Source Varchar(1)
As

Declare @FirstBatchID Int
Declare @FirstTraceNumber Int
Declare @FileBatchID Int

--Get first batch ID
Select @FirstBatchID = Min(BatchID) 
From Batch a With (NoLock) 
Where ToProcessDate Is Null 
	And Exists (Select 'x' From AchMerchAddl b Where a.PriID = b.Priid And BankID = @BankID)

--Get first trace number
Select @FirstTraceNumber = Min(TraceNumber)  
From BatchDetail With (NoLock) 
Where BatchID >= @FirstBatchID And Processed = 0 And BankID = @BankID

--Get last file batch ID
Select top 1 @FileBatchID = FileBatchID From BatchDetail With (NoLock) 
Where  TraceNumber >= @FirstTraceNumber And BatchID <> 0 Order By TraceNumber Desc

If (@@Rowcount = 0)
	Select @FileBatchID = 1
Else	
	Select @FileBatchID = @FileBatchID + 1

--Insert pending record in batchdetail if it does not exists
Insert Into BatchDetail(BatchID, line_no, IntransID, OverItemAmount, ProcessedDate, Source, BankID, FileBatchID)
Select 0,0,A.PendingID,0,Null,'P',@BankID, @FileBatchID 
From Pending A With (NoLock), AchMerchAddl B With (NoLock) 
Where A.PriID = B.PriID And B.BankID = @BankID And A.ProcessedDate Is Null 
	And Not Exists (Select Tracenumber From BatchDetail C With (NoLock) Where C.IntransID = A.PendingID And Source = 'P')
Order by Description, PendingID

--Populate Data In Temp Table
If Exists (select * from sysobjects where id = object_id(N'Program_AchCentral.tmp_batch') and OBJECTPROPERTY(id, N'IsUserTable') = 1) Drop table Program_AchCentral.tmp_batch

Create Table Program_AchCentral.tmp_batch 
(
	[IntransID] [Int] NOT NULL, 
	[BatchID] [Int] Not Null, 
	[Description] [nvarchar] (10) NOT NULL, 
	[DescDate] [char] (6) NOT NULL, 
	[TransRoute] [nvarchar] (9) NOT NULL, 
	[AccountNo] [nvarchar] (17) NOT NULL, 
	[EncryptAccountNo] [nvarchar] (30) NOT NULL, 
	[NameOnAccount] [nvarchar] (22) NOT NULL, 
	[PriID] [Int] Not Null, 
	[CompanyName] [nvarchar] (16) Null, 
	[Secc] [nvarchar] (1) Not Null, 
	[Amount] [Decimal] (10, 2) Not Null, 
	[TransType] [nvarchar] (2) NOT NULL, 
	[Source] [nvarchar] (1) Null, 
	[TraceNumber] [Int] Not Null, 
	[FileBatchID] [Int] Not Null, 
	[RefID] [nvarchar] (50)
) 
Create Index IntransID On Program_AchCentral.tmp_batch (IntransID)


If (@Source = 'C')
--Get Transactions
	Insert Into Program_AchCentral.tmp_batch
	SELECT A.IntransID, A.BatchID, A.Description, A.DescDate, A.TransRoute, A.AccountNo, A.EncryptAccountNo, SubString(A.NameOnAccount, 1, 22), A.PriID, A.CompanyName, A.Secc, A.Amount, A.TransType, 'C', B.TraceNumber, B.FileBatchID, A.RefID
	From IncomingTrans A, BatchDetail B
	Where A.IntransID = B.IntransID And B.BatchID In (Select BatchID From Batch Where ToProcessDate Is Null And BatchID <> 848)  And 
	                 A.StatusID Not In (3, 8, 11, 15) And B.Source = 'C' And B.BankID = @BankID And B.Processed = 0
Else
	--Get Pending
	Insert Into Program_AchCentral.tmp_batch
	SELECT B.IntransID, 0, SubString(A.Description, 1, 10), 
		'      ', A.TransRoute, A.AccountNo, A.EncryptAccountNo, SubString(A.NameOnAcct,1, 22), A.PriID, 'PAYMENT RESOURCE', 'B', A.Amount, A.TransType, 'P', B.TraceNumber, B.FileBatchID,  '0'
	From Pending A, BatchDetail B
	Where A.PendingID = B.IntransID And B.Source = 'P' And B.Tracenumber > @FirstTraceNumber  And B.Processed = 0

--Decrypt account
Update Program_AchCentral.tmp_batch 
Set accountNo= dbo.fn_Decrypt(EncryptAccountno)      
From Program_AchCentral.tmp_batch

Insert Into Program_AchCentral.tmp_batch Values (0, 999999, 'ExtraBatch', '', '', '', 'ObmDOTprfxT4efcIKgXUlw==', '', 5999, '', 'C', 0.00, '', 'C', @FirstTraceNumber + 999998, 999998, '0')
Insert Into Program_AchCentral.tmp_batch Values (0, 999999, 'Z Batch', '', '', '', 'ObmDOTprfxT4efcIKgXUlw==', '', 5999, '', 'C', 0.00, '', 'P', @FirstTraceNumber + 999999, 999999, '0')

--Select * From Program_AchCentral.tmp_batch

If (@Source = 'C')
	Select 
		a.IntransID, 
		a.BatchID, 	
		Left(a.Description + Space(10),10) As Description, 
		a.DescDate, 
		a.TransRoute, 
		Left(Replace(Replace(Replace(a.AccountNo,' ',''),'-',''),char(9),'') + Space(17),17) as AccountNo, 
		a.EncryptAccountNo, 
		Case When a.NameOnAccount is null Or Rtrim(Ltrim(Isnull(a.NameOnAccount,''))) = '' Then 'Blank Name Item'
			Else Left(Replace(Rtrim(Ltrim(a.NameOnAccount)),char(9),' ') + Space(22),22)
		End as NameOnAccount, 
		a.PriID, 
		Left(RTrim(LTrim(a.CompanyName)) + Space(16), 16) as CompanyName, 
		Case When a.Secc = 'B' Then 'CCD' 
			When a.Secc = 'C' Then 'PPD'
			When a.Secc = 'T' Then 'TEL'
			When a.Secc = 'W' Then 'WEB'
			When a.Secc = 'R' Then 'RCK'
			Else 'CCD'
		End as Secc,
		a.Amount, 
		Case When Right(a.TransType,1) In ('6','7') Then Case When a.Amount = 0 Then Left(a.TransType,1) + '8' Else Left(a.TransType,1) + '7' End
			When Right(a.TransType,1) In ('1','2') Then Case When a.Amount = 0 Then Left(a.TransType,1) + '3' Else Left(a.TransType,1) + '2' End
		End as TransType, 
		a.Source, 
		a.TraceNumber, 
		a.FileBatchID, 
		a.RefID,
		Left(IsNull(b.AchCoName,'') + Space(16), 16) as AchCoName,
		Case When a.Secc = 'W' Then Left('Single' + Space(20), 20) 
			Else Left(Isnull(b.Achdiscrtn,'') + Space(20), 20)
		End as AchDiscrtn,
		b.NechaID,
		Case When a.Secc = 'W' Then 'Single' Else b.AchDescrp End as Descrip,
		b.MerchantID
	From Program_AchCentral.tmp_batch a 
		Left join AchMerchAddl b on a.PriID = b.PriID
	Where a.Source = @Source
	Order by a.TraceNumber
else
	Select 
		a.IntransID, 
		a.BatchID, 
		Case When Upper(Left(a.Description + Space(10),10)) = 'BELOW RSRV' Then 'RESERVE REQ'
			When Upper(Left(a.Description + Space(10),4)) = 'MTOT' Then 'EZ START PAYMENT'
			When Upper(Left(a.Description + Space(10),10)) = 'HOLD RELEA' Then 'HOLD RELEASED'
			When Upper(Left(a.Description + Space(10),10)) = 'TRANS FUND' Then 'TRANSFER FUNDS'
			Else a.Description
		End As Description, 
		a.DescDate, 
		a.TransRoute, 
		Left(Replace(Replace(Replace(a.AccountNo,' ',''),'-',''),char(9),'') + Space(17),17) as AccountNo, 
		a.EncryptAccountNo, 
		Case When a.NameOnAccount is null Or Rtrim(Ltrim(Isnull(a.NameOnAccount,''))) = '' Then 'Blank Name Item'
			Else Left(Replace(Rtrim(Ltrim(a.NameOnAccount)),char(9),' ') + Space(22),22)
		End as NameOnAccount, 
		a.PriID, 
		Left(RTrim(LTrim(a.CompanyName)) + Space(16), 16) as CompanyName, 
		Case When a.Secc = 'B' Then 'CCD' 
			When a.Secc = 'C' Then 'PPD'
			When a.Secc = 'T' Then 'TEL'
			When a.Secc = 'W' Then 'WEB'
			When a.Secc = 'R' Then 'RCK'
			Else 'CCD'
		End as Secc,
		a.Amount, 
		Case When Right(a.TransType,1) In ('6','7') Then Case When a.Amount = 0 Then Left(a.TransType,1) + '8' Else Left(a.TransType,1) + '7' End
			When Right(a.TransType,1) In ('1','2') Then Case When a.Amount = 0 Then Left(a.TransType,1) + '3' Else Left(a.TransType,1) + '2' End
		End as TransType, 
		a.Source, 
		a.TraceNumber, 
		a.FileBatchID, 
		a.RefID,
		Left(IsNull(b.AchCoName,'') + Space(16), 16) as AchCoName,
		Case When a.Secc = 'W' Then Left('Single' + Space(20), 20) 
			Else Left(Isnull(b.Achdiscrtn,'') + Space(20), 20)
		End as AchDiscrtn,
		b.NechaID,
		Case When a.Secc = 'W' Then 'Single' Else b.AchDescrp End as Descrip,
		b.MerchantID
	From Program_AchCentral.tmp_batch a 
		Left join AchMerchAddl b on a.PriID = b.PriID
	Where a.Source = @Source
	Order by a.Description, a.TraceNumber
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


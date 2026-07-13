SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


ALTER  Procedure dbo.Ach_Batch
	@BankID Int
AS

Set Nocount On

Declare	@sql_error_code Int,
	@rows_updated Int,
	@mesg Varchar (1024),
	@status Int

Declare @CurrentDate SmallDateTime
Declare @CutOffDate SmallDateTime

Select @CurrentDate = Convert(Varchar,Getdate(),101) + ' 09:00:00'
Select @CutOffDate = dbo.fn_GetCutOffDate(GetDate())

/***** Create Temp Table *****/
If exists (Select * From sysobjects Where id = object_id(N'Ach_Batch_Process_Incoming') and OBJECTPROPERTY(id, N'IsUserTable') = 1) Drop Table Ach_Batch_Process_Incoming
Create Table dbo.Ach_Batch_Process_Incoming (PriID Int NULL)

Select @sql_error_code = @@ERROR,@rows_updated  = @@ROWCOUNT
If @sql_error_code <> 0
Begin
      Set @mesg = 'Error creating Ach_Batch_Process_Incoming temp table for bank '  + Convert(Varchar,@BankID) + '. @@ERROR = '+ CONVERT(VARCHAR, @sql_error_code)
      Execute  @status = sp_log_achprocess_i @job_inst_id = 1, @svr_lvl_cd = 'E', @log_pa_txt = @mesg, @src_syst_cd = 'Ach_Batch'

      Goto Error_Handler
End
Else
Begin
      Set @mesg = 'Successfully created Ach_Batch_Process_Incoming temp table for bank '  + Convert(Varchar,@BankID) + '.'
      Execute  @status = sp_log_achprocess_i @job_inst_id = 1, @svr_lvl_cd = 'I', @log_pa_txt = @mesg, @src_syst_cd = 'Ach_Batch'
End

/***** Populate Temp Table *****/
If @BankID In (10, 11, 12, 14)
Begin
	Insert Into Ach_Batch_Process_Incoming (PriID) Select Distinct PriID From IncomingTrans With (NoLock) Where StatusID In (2, 101, 102, 202, 302, 352) And PriID In (Select PriID From AchMerchAddl Where BankID = @BankID And (IsThisHoldDayOrPaidOutDay Not In ('B', 'P', 'C') Or (IsThisHoldDayOrPaidOutDay In ('B', 'P', 'C') And Secc <> 'TEL'))) And (BatchID Is Null Or BatchID = 0) And (DateProcessed Is Null Or DateProcessed >= @CurrentDate) And PriID Not In (4438, 4692) And TransType In ('27', '37', '28', '38') --(4438, 4692) are test merchants
	Insert Into Ach_Batch_Process_Incoming (PriID) Select Distinct PriID From IncomingTrans With (NoLock) Where StatusID = 80 And PriID In (Select PriID From AchMerchAddl Where BankID = @BankID And IsThisHoldDayOrPaidOutDay In ('B', 'P', 'C')) And (BatchID Is Null Or BatchID = 0) And (DateProcessed Is Null Or DateProcessed >= @CurrentDate) And TransType In ('22', '32') --Tier 1 merchants allow credits
	Insert Into Ach_Batch_Process_Incoming (PriID) Select Distinct PriID From IncomingTrans With (NoLock) Where StatusID In (2, 101, 102, 202, 302, 352) And PriID In (Select PriID From AchMerchAddl Where BankID = @BankID And (IsThisHoldDayOrPaidOutDay Not In ('B', 'P', 'C') Or (IsThisHoldDayOrPaidOutDay In ('B', 'P', 'C') And Secc <> 'TEL'))) And (BatchID Is Null Or BatchID = 0) And (DateProcessed Is Null Or DateProcessed >= @CurrentDate) And PriID Not In (4438, 4692) And TransType In ('27', '37', '28', '38') --Duplicate statement, can delete
	Insert Into Ach_Batch_Process_Incoming (PriID) Select Distinct PriID From IncomingTrans With (NoLock) Where StatusID In (2) And PriID In (5564, 5364, 5425, 5620) And Secc = 'T' And (BatchID Is Null Or BatchID = 0) And (DateProcessed Is Null Or DateProcessed >= @CurrentDate) And TransType In ('27', '37', '28', '38') --(5564, 5364, 5425, 5620) are draft merchants
	Insert Into Ach_Batch_Process_Incoming (PriID) Select Distinct PriID From IncomingTrans With (NoLock) Where StatusID In (2, 101, 102, 202, 302, 352) And PriID In (Select PriID From AchMerchAddl Where BankID = @BankID) And (BatchID Is Null Or BatchID = 0) And (DateProcessed Is Null Or DateProcessed >= @CurrentDate) And PriID Not In (4438, 4692) And TransType In ('22', '32')
	Insert Into Ach_Batch_Process_Incoming (PriID) Select Distinct PriID From IncomingTrans With (NoLock) Where StatusID = 80 And PriID In (Select PriID From AchMerchAddl Where BankID = @BankID And IsThisHoldDayOrPaidOutDay In ('B', 'P', 'C')) And (BatchID Is Null Or BatchID = 0) And (DateProcessed Is Null Or DateProcessed >= @CurrentDate) And TransType In ('22', '32')
	Insert Into Ach_Batch_Process_Incoming (PriID) Select Distinct PriID From IncomingTrans With (NoLock) Where StatusID In (8, 11, 15) And PriID In (Select PriID From AchMerchAddl Where BankID = @BankID And HoldPeriod <> 0) And (BatchID Is Null Or BatchID = 0) And DateProcessed >= @CutOffDate And PriID Not In (4438, 4692)
	Insert Into Ach_Batch_Process_Incoming (PriID) Select Distinct PriID From IncomingTrans With (NoLock) Where StatusID In (121, 122, 127, 128, 208, 211, 215, 221, 308, 311, 315, 321) And PriID In (Select PriID From AchMerchAddl Where BankID = @BankID) And (BatchID Is Null Or BatchID = 0) And DateProcessed Is Null And PriID Not In (4438, 4692)
End
Else If @BankID In (6)
Begin
	Insert Into Ach_Batch_Process_Incoming (PriID) Select Distinct PriID From IncomingTrans With (NoLock) Where StatusID = 2 And PriID In (Select PriID From AchMerchAddl Where BankID = @BankID)
	Insert Into Ach_Batch_Process_Incoming (PriID) Select Distinct PriID From IncomingTrans With (NoLock) Where StatusID In (8, 11, 15) And PriID In (Select PriID From AchMerchAddl Where BankID = @BankID And HoldPeriod <> 0) And (BatchID Is Null Or BatchID = 0) And DateProcessed >= @CutOffDate
End

Select @sql_error_code = @@ERROR,@rows_updated  = @@ROWCOUNT
If @sql_error_code <> 0
Begin
      Set @mesg = 'Error populating Ach_Batch_Process_Incoming temp table for bank '  + Convert(Varchar,@BankID) + ' @@ERROR = '+ CONVERT(VARCHAR, @sql_error_code)
      Execute  @status = sp_log_achprocess_i @job_inst_id = 1, @svr_lvl_cd = 'E', @log_pa_txt = @mesg, @src_syst_cd = 'Ach_Batch'

      Goto Error_Handler
End
Else
Begin
      Set @mesg = 'Successfully populated Ach_Batch_Process_Incoming temp table for bank '  + Convert(Varchar,@BankID) + '.'
      Execute  @status = sp_log_achprocess_i @job_inst_id = 1, @svr_lvl_cd = 'I', @log_pa_txt = @mesg, @src_syst_cd = 'Ach_Batch'
End

/***** Begin posting batches *****/
Declare cur Cursor
Read_Only
For Select Distinct PrIID From Ach_Batch_Process_Incoming Order by PriID

Declare @PriID Int, @AchDescrp Nvarchar(10), @IsThisHoldDayOrPaidOutDay Nvarchar(50), @Secc Nvarchar(3), @Active Bit, @AchCoName Nvarchar(40)
Declare @Source Nvarchar(2), @NCT Nvarchar(3)

Open cur

Fetch Next From cur Into @PriID
While (@@fetch_status <> -1)
Begin
	If (@@fetch_status <> -2)
	Begin
	
		Set @mesg = 'Begin processing bank ' +  Convert(Varchar,@BankID) +  ' batch file for PriID ' + Convert(Varchar,@PriID) 
		Execute  @status = sp_log_achprocess_i @job_inst_id = 1, @svr_lvl_cd = 'I', @log_pa_txt = @mesg, @src_syst_cd = 'Ach_Batch'


		Select @AchDescrp = AchDescrp, @Secc = Secc, @Active = Active, @AchCoName = AchCoName, @IsThisHoldDayOrPaidOutDay = IsThisHoldDayOrPaidOutDay From AchMerchAddl With (NoLock) Where PriID = @PriID
		If @IsThisHoldDayOrPaidOutDay In ('B','P','C')
		Begin
			If @PriID = 3804
				Update IncomingTrans Set Description = @AchDescrp, CompanyName = @AchCoName Where PriID = 3804 And StatusID = 2
			Else If @PriID = 4822
				Update IncomingTrans Set Description = @AchDescrp, Secc = 'C', Source = 'TC' Where PriID In (4822) And StatusID = 2 And Secc <> 'C' And TransType In ('27', '37')
			Else If @PriID IN (5364, 5564, 5425, 5620, 5914, 5006)
				Update IncomingTrans Set Description = @AchDescrp, Secc = 'T', Source = 'TC' Where PriID In (5364, 5564, 5425, 5620, 5914, 5006) And StatusID = 2 And TransType In ('27', '37')
			Else
				Update IncomingTrans Set Description = @AchDescrp, Secc = 'T', Source = 'DF', StatusID = 60 Where PriID = @PriID And StatusID = 2 And Secc In ('B', 'C', 'T') And TransType in ('27', '37')
		End	
		Else
		Begin
			Update IncomingTrans Set Description = @AchDescrp Where PriID = @PriID And (Description Is Null Or Len(Description) = 0)
			If @Secc = 'PPD'
				Update IncomingTrans Set Secc = 'C' Where PriID = @PriID And (Secc Is Null Or Len(Secc) = 0)
			If @Secc = 'CCD'
				Update IncomingTrans Set Secc = 'B' Where PriID = @PriID And (Secc Is Null Or Len(Secc) = 0)
			If @IsThisHoldDayOrPaidOutDay <> 'N'
			Begin
				If @Active = 0
					Update IncomingTrans Set StatusID = 3 Where PriID = @PriID And StatusID = 2
				Else
					Update IncomingTrans Set TransType = '28' Where PriID = @PriID And Amount = 0.00 And SubString(TransType, 2, 1) <> '8'
			End	
		End
		
		Update IncomingTrans Set Source = 'TC' Where StatusID = 2 And (Source Is Null Or Source = '') And PriID = @PriID

		If @BankID = 6 Or @BankID = 7 
			Select @Source = 'TC'
		Else If @BankID = 10
			Select @Source = 'VA'
		Else If @BankID = 14
			Select @Source = 'VA'
		Else If @BankID = 11 Or @BankID = 12
		Begin
			Select @NCT = Isnull(NCT,'') From [NB-Red].Central.dbo.Merchants Where PriID = @PriID
			If @NCT = 'PRI'
			  	Select @Source = 'VA'
			Else
			  	Select @Source = 'TC'
			
			If @Source = 'TC'
			Begin
				If Exists(Select Source From IncomingTrans With (NoLock) Where StatusID in (2, 11, 101, 102, 202, 302, 352) And PriID = @PriID Group By Source Having Source In ('VO','GO','VA','GA','VR','GR'))				
					Select @Source = 'NA'
				Else
					Select @Source = 'TC'
			End 		
		End

		If @BankID In (6, 7)
		Begin
			Exec Ach_PostBatch_PayMyBill @BankID, @PriID  	
		End
		Else If @BankID = 11
		Begin
			If @Source In ('VA', 'TC')
				Exec Ach_PostBatch_Ncal @BankID, @PriID, @Source  		
		End 
		Else If @BankID = 12
		Begin
			If @Source In ('VA', 'TC')
				Exec Ach_PostBatch_FifthThird @BankID, @PriID, @Source  		
		End 
		Else If @BankID In (10,14)
		Begin
			If @Source In ('VA', 'TC')
				Exec Ach_PostBatch_NCT @BankID, @PriID, @Source  		
		End 

		Set @mesg = 'End processing bank ' + Convert(Varchar,@BankID) + ' batch file for PriID ' + Convert(Varchar,@PriID) 
		Execute  @status = sp_log_achprocess_i @job_inst_id = 1, @svr_lvl_cd = 'I', @log_pa_txt = @mesg, @src_syst_cd = 'Ach_Batch'

	End
	Fetch Next From cur Into @PriID
End

Close cur
Deallocate cur

Error_Handler:
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

ALTER Procedure dbo.Ach_Journal_Nct_HoldRelease
	@BankID Int
AS

Set Nocount On

Declare 	@JournalIDIn Int, @Amount Money, @HoldID Int, @PriID Int --cursor variables
Declare 	@Available Money, @MerchType Nvarchar(2),
	 	@ReleaseTotal Money, @OkToPostPending Int, @NewRoutineNo NVarchar(9),
		@HoldToRelease SmallDateTime, @CurrentDate SmallDateTime,
		@RefID Int, @PendingID Int, @NameOnAcct Nvarchar(25),
		@LastJournalID Int, @BatchID Int, @EntryDescription Nvarchar(60),
		@TransRoute Nvarchar(9), @AccountNo Nvarchar(17), @JournalAmount Money, 
		@NechaID Nvarchar(6),@OrigBatchDate SmallDatetime,@OrigTransBase Nvarchar(2),
		@Total Money, @Balance Money

			
Select @CurrentDate = GetDate(), @OkToPostPending = 1

Select @Available = 0
Select @ReleaseTotal = 0
Select @HoldID = 0
Select @JournalIDIn = 0

If @BankID Not In (6,10,14)
Begin
	Exec Ach_GetHoldToReleaseDate2 @HoldToRelease Output,@CurrentDate

	DECLARE cur CURSOR READ_ONLY FOR 
		Select  A.JournalIDIn, A.Amount,A.HoldID, A.PriID 
		From Hold A With (NoLock), AchMerchAddl B With (NoLock) 
		Where A.Type = 'H' And A.DatePaid Is Null And A.PriID Not In (5999, 6999, 7999, 8999) And A.PriID = B.PriID And B.StopEFTFrom = 'N' And A.DateToRelease <= @HoldToRelease And B.BankID = @BankID 
		Order By A.PriID

	Open cur
	
	Fetch Next From cur Into @JournalIDIn, @Amount, @HoldID, @PriID
	While (@@fetch_status <> -1)
	Begin
		If (@@fetch_status <> -2)
		Begin
			--Reset Variables
			Select @Available = 0

			Select 
				@NameOnAcct = IsNull(dfiacctname,Replicate(' ',25)), 
				@TransRoute = IsNull(transroute,Replicate(' ',9)), 
				@AccountNo = IsNull(AccountNo,Replicate(' ',17)) 
			From AchMerchAddl With (NoLock) Where PriID = @PriID
	
			Select @NechaID = Replicate('0',5 - Len(@PriID)) + Convert(Varchar,@PriID)

			Select @NewRoutineNo = NewRoutineNo From BankRoutineNo With (NoLock) Where RoutineNumber = @TransRoute And OkToAch = 'Y'

			If @@Rowcount = 0
				Select @OkToPostPending = 1
			Else
			Begin
				If Ltrim(Rtrim(@TransRoute)) = ''
					Select @OkToPostPending = 1
				Else
				Begin
					Select @OkToPostPending = 0
					If @NewRoutineNo <> '000000000'
						Select @TransRoute = @NewRoutineNo
				End
			End

			If (@PrIID = 4497 Or @PrIID = 4473)
			Begin
				Select @Balance = Isnull(Sum(Amount),0) From Journal With (NoLock) Where PriID = @PriID And RefCode In ('B', 'N', 'F', 'R', 'C', 'P', 'E', 'S', 'Z')
				Select @Available = Isnull(Sum(Amount),0) From Journal With (NoLock) Where PriID = @PriID And RefCode In ('B', 'N', 'F', 'R', 'C', 'P', 'E', 'V', 'H', 'X')
			End

			If @OkToPostPending = 0
			Begin
				If Exists(Select ReasonCode From Returns With (NoLock) Where TransRoute = @TransRoute And EncryptAccountNo = dbo.fn_encrypt(@AccountNo) And PriID = @PRIID And TransType In ('27', '37') And ReasonCode In ('R02', 'R03', 'R04', 'R07', 'R10', 'R12', 'R15', 'R24', 'R29'))
					Select @OkToPostPending = 1
			End	
	
			If @OkToPostPending = 0
			Begin
				Select @RefID = RefID From Journal With (NoLock) Where JournalID = @JournalIDIn And PriID = @PriID And RefCode = 'V'
								
				If @@Rowcount > 0
				Begin
					Select @OrigBatchDate = PostedDate, @OrigTransBase = TransBase From Batch With (NoLock) Where BatchID = @RefID And PriID = @PriID
							
					If @@Rowcount = 0
						Select @OrigBatchDate = '01/01/04', @OrigTransBase = 'VA'
							
					Select @Total = Isnull(Sum(Amount),0) From Returns With (NoLock) 
					Where PriID = @PriID And BatchID = @RefID And ReturnStatus = 0 And TransType In ('26', '36') And ReasonCode Not In ('Z01', 'Z02', 'Z03', 'Z04', 'Z05', 'Z06', 'Z07', 'Z08')
						
					If @@Rowcount > 0
						Select @Available = @Available - @Total
					Else
						Select @Available = 0
									
					Select @Total = Isnull(Sum(Amount),0) From Returns With (NoLock) 
					Where PriID = @PriID And ReturnStatus = 1 And PaidOutID = 999999 And TransType In ('26', '36') And ReasonCode Not In ('Z01', 'Z02', 'Z03', 'Z04', 'Z05', 'Z06', 'Z07', 'Z08')
							
					If @@Rowcount > 0
						Select @Available = @Available - @Total
							
					Select @Total = Isnull(Sum(Amount),0) From Returns With (NoLock) 
					Where PriID = @PriID And BatchID = @RefID And ReturnStatus = 0 And TransType In ('21', '31') And ReasonCode Not In ('Z01', 'Z02', 'Z03', 'Z04', 'Z05', 'Z06', 'Z07', 'Z08')
								
					If @@Rowcount > 0
						Select @Available = @Available + @Total
							
					Select @Total = Isnull(Sum(Amount),0) From Returns With (NoLock) 
					Where PriID = @PriID And ReturnStatus = 1 And PaidOutID = 999999 And TransType In ('21', '31') And ReasonCode Not In ('Z01', 'Z02', 'Z03', 'Z04', 'Z05', 'Z06', 'Z07', 'Z08')
							
					If @@Rowcount > 0
						Select @Available = @Available + @Total
							
					Select @EntryDescription =  'Release of Funds From ' + Convert(Varchar,@OrigBatchDate,1)
					Select @JournalAmount = @Available + @Amount
					Exec Ach_Journal_Post @CurrentDate,@JournalIDIn,@PriID,@EntryDescription,'X',@JournalAmount,0,0
					Select @LastJournalID = @@Identity
						
					Update Hold Set DatePaid = Getdate(), JournalIDPaid =@LastJournalID  Where HoldID = @HoldID
						
					Select @EntryDescription =  'EFT TO ' + Rtrim(@TransRoute) + ':' + Rtrim(@AccountNo)
					Select @JournalAmount = -1 * (@ReleaseTotal + @Available)
					Exec Ach_Journal_Post @CurrentDate,@BatchID,@PriID,@EntryDescription,'P',@JournalAmount,0,0
					Select @LastJournalID = @@Identity
						
					If @PriID = 3192
							Insert Into Pending (PriID, PostedDate, TransType, Amount, JournalID, Description, Type, TransRoute, AccountNo, NameOnAcct) 
							Values (@PriID, GetDate(), '32',@ReleaseTotal + @Available,@LastJournalID, 'HOLD RELEA', 'J',@TransRoute,@AccountNo,@NameOnAcct)
					Else
							Insert Into Pending (PriID, PostedDate, TransType, Amount, JournalID, Description, Type, TransRoute, AccountNo, NameOnAcct) 
							Values (@PriID, GetDate(), '22',@ReleaseTotal + @Available,@LastJournalID, 'HOLD RELEA', 'J',@TransRoute,@AccountNo,@NameOnAcct)
					
					Select @PendingID = @@Identity
					Update Journal Set RefID = @PendingID Where JournalID = @LastJournalID
					Update AchMerchAddl Set Balance = Balance - (@ReleaseTotal + @Available) Where PriID = @PriID
				End
			End
			Else
			Begin
				Insert Into AchUncollectableFundsRegister (PriID, DateOfDebits, AmountWantToDebit, LastJournalID, Type) 
				Values (@PriID, Getdate(),@Balance ,@LastJournalID , 'H')
			End
		End
		Fetch Next From cur Into @JournalIDIn, @Amount, @HoldID, @PriID
	End
	
	Close cur
	Deallocate cur
End
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


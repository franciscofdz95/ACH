SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

ALTER Procedure dbo.Ach_Journal_Nct_Process
	@BankID Int,
	@BatchID Int, 
	@PriID Int, 
	@Debit Money, 
	@Credit Money, 
	@DebitCount Int, 
	@CreditCount Int, 
	@OverDailyAmountLimitCount Money, 
	@OverItemAmount Money,
	@Transbase Nvarchar(2)
AS

Set Nocount On

--cursor variables
Declare @MerchantID Int, @OrigBatchDate SmallDatetime,
				@OrigTransBase Nvarchar(2)

Declare @EntryDescription Nvarchar(60), @RefCode Nvarchar(1),
				@CurrentDate SmallDateTime, @HoldAmount Money, 
				@JournalAmount Money, @LastJournalID Int, 
				@FeeTotal Money, @TimesOfFile Int, @RefID Int, @HoldToRelease SmallDateTime,
				@Balance Money, @EFTDate SmallDateTime, @ReservedToRelease SmallDateTime, 
				@PendingID Int, @Available Money, @OkToPostPending Int,
				@NewRoutineNo Nvarchar(9), @BatchType Nvarchar(2)

Declare @JournalIDIn Int, @Amount Money, @HoldID Int

--Achmerchaddl variables
Declare @RsvPct Decimal(18,2), @RsrvTx Bit, @NameOnAcct Nvarchar(25), 
				@ItemFee Decimal(18,2), @RsvDay Int, @PullFundsFromAnotherAchID Int,
				@PostMethod Bit, @ProcessFee Decimal(18,2), @HoldOrPaidOut Nvarchar(50),
				@LoadFee Decimal(18,2), @Additional Decimal(18,2), @MaintFee Decimal(18,2),
				@FeePriID Int, @HoldDay Int, @ProcessType Nvarchar(1), @OverDraft Decimal(18,2),
				@TransRoute Nvarchar(9), @AccountNo Nvarchar(17), @MultiBatch Nvarchar(1), 
				@GuaranteeFee Decimal(18,2), @SettleBatchRightAway Nvarchar(1), @NechaID Nvarchar(6)

Select @CurrentDate = GetDate(), @HoldAmount = 0
Exec Ach_GetEFTDate @EFTDate Output
Select @EFTDate = Convert(Varchar,@EFTDate,101)

Exec Ach_GetReservedToReleaseDate @ReservedToRelease Output
Select @ReservedToRelease = Convert(Varchar,@ReservedToRelease,101)

Select
	@NameOnAcct = Isnull(dfiacctname,Replicate(' ',25)),  
	@TransRoute = IsNull(TransRoute,Replicate(' ',9)), 
	@AccountNo = IsNull(AccountNo,Replicate(' ',17)),
	@NechaID = NechaID,
	@FeePriID = IsNull(TrxFeeToAcct,0),
	@MerchantID = MerchantID,
	@SettleBatchRightAway = IsNull(SettleBatchRightAway,'N')
From AchMerchAddl With (NoLock) Where PriID = @PRIID

If (@@Rowcount = 0)
Begin
	Select
		@NameOnAcct = Replicate(' ',25),  
		@TransRoute = Replicate(' ',9), 
		@AccountNo = Replicate(' ',17),
		@NechaID = '',
		@FeePriID = 0,
		@MerchantID = 0,
		@SettleBatchRightAway = 'N'
End

Select 
	@RsvPct = Isnull(ReservePercent,0), 
	@RsvDay = Case When ReservePeriod = Null Then 0 Else ReservePeriod + 1 End, 
	@HoldDay = Isnull(HoldPeriod,0), 
	@ItemFee = Isnull(ItemFee,0),
	@ProcessFee = Isnull(DiscountFee,0)
From [NB-Red].Central.dbo.NctTransactionIDs
Where MerchantID = @MerchantID And Type = @Transbase

If @@RowCount = 0 
Begin
	Select 
		@RsvPct = 0, 
		@RsvDay = 0, 
		@HoldDay = 0, 
		@ItemFee = 0,
		@ProcessFee = 0
End

Select @NewRoutineNo = NewRoutineNo From BankRoutineNo With (NoLock) Where RoutineNumber = @TransRoute And OkToAch = 'Y'

If (@@Rowcount = 0)
	Select @OkToPostPending = 1 --Not ok to post
Else
Begin
	If (Ltrim(Rtrim(@TransRoute)) = '' Or @NewRoutineNo = '000000000')
		Select @OkToPostPending = 1 --Not ok to post
	Else
	Begin
		Select @OkToPostPending = 0 --Ok to post
		Select @TransRoute = @NewRoutineNo
	End
End

If (@OkToPostPending = 0) --If ok to post, check if there was ever a return for the routing and account number combinition
Begin
	If (Exists(Select ReasonCode From Returns With (NoLock) Where TransRoute = @TransRoute And EncryptAccountNo = dbo.fn_encrypt(@AccountNo) And PriID = @PRIID And TransType In ('27', '37') And ReasonCode In ('R02', 'R03', 'R04', 'R07', 'R10', 'R12', 'R15', 'R24', 'R29')))
	Begin
		Select @OkToPostPending = 1 --Not ok to post
	End
End	

Update AchMerchAddl Set Balance = @Balance Where PriID = @PriID

Select @BatchType = @Transbase

-------------------------------------------------------------------------
Exec Ach_GetHoldToReleaseDate @HoldToRelease Output,@CurrentDate,@HoldOrPaidOut,@HoldDay
-------------------------------------------------------------------------
--@TimesOfFile is use to access monthly fees
-------------------------------------------------------------------------
If (Month(GetDate()) = 12)
Begin
	Select @TimesOfFile = Count(*) From Batch Where (PostedDate >= Convert(Varchar,Month(GetDate())) + '/01/' + Convert(Varchar,Year(GetDate())) And PostedDate < Convert(Varchar,Month(DateAdd(Month,1,GetDate()))) + '/01/' + Convert(Varchar,Year(DateAdd(Year,1,GetDate())))) 
	And PriID = @PRIID
End
Else
Begin
	Select @TimesOfFile = Count(*) From Batch Where PostedDate >= Convert(Varchar,Month(GetDate())) + '/01/' + Convert(Varchar,Year(GetDate())) And PostedDate < Convert(Varchar,Month(DateAdd(Month,1,GetDate()))) + '/01/' + Convert(Varchar,Year(GetDate())) 
	And PriID = @PRIID
End

If (@TimesOfFile = 0)
Begin
	Select @TimesOfFile = 1
End
-------------------------------------------------------------------------
--Release reserve if there is any.
-------------------------------------------------------------------------
Select @Balance = 0,@Available = 0

If (@BatchType Not In ('GA','GR'))
Begin
	DECLARE cur2 CURSOR READ_ONLY FOR 
		Select A.JournalIDIn, A.Amount, A.HoldID From Hold A With (NoLock), AchMerchAddl B With (NoLock) 
		Where A.DateToRelease <= @ReservedToRelease And A.Type = 'V' And A.DatePaid Is Null And A.PriID = B.PriID And A.PriID = @PriID And B.StopEFTFrom = 'N' Order By HoldID
	
	Open cur2
	
	Fetch Next From cur2 Into @JournalIDIn, @Amount, @HoldID
	While (@@fetch_status <> -1)
	Begin
		If (@@fetch_status <> -2)
		Begin
			Select @RefID = RefID From Journal With (NoLock) Where JournalID = @JournalIDIn And PriID = @PriID And RefCode = 'V'
			
			Select @OrigBatchDate = PostedDate, @OrigTransBase = TransBase From Batch With (NoLock) Where BatchID = @RefID And PriID = @PriID

			If (@@Rowcount = 0)
				Select @OrigBatchDate = '01/01/04', @OrigTransBase = 'VA'

			Select @EntryDescription = 'Release of Reserved on '
			Select @EntryDescription = @EntryDescription + Substring(Convert(Varchar,@OrigBatchDate,101),1,6) 
			Select @EntryDescription = @EntryDescription + Substring(Convert(Varchar,@OrigBatchDate,101),9,2) 
			Select @EntryDescription = @EntryDescription + ' for ' + @OrigTransBase + ' batch'
			Exec Ach_Journal_Post @CurrentDate,@JournalIDIn,@PriID,@EntryDescription,'X', @Amount,@HoldID,0
			Select @LastJournalID = @@Identity

			Update Hold Set Datepaid = GetDate(), JournalIDPaid = @LastJournalID Where HoldID = @HoldID
		End
		Fetch Next From cur2 Into @JournalIDIn, @Amount, @HoldID
	End
	
	Close cur2
	Deallocate cur2
End

Select @Balance = Isnull(Sum(Amount),0) From Journal With (NoLock) Where PriID = @PriID And RefCode In ('B', 'N', 'F', 'R', 'C', 'P', 'E', 'S', 'Z')
Select @Available = Isnull(Sum(Amount),0) From Journal With (NoLock) Where PriID = @PriID And RefCode In ('B', 'N', 'F', 'R', 'C', 'P', 'E', 'V', 'H', 'X')

Select @HoldAmount = 0
-------------------------------------------------------------------------
--Journal Debit
-------------------------------------------------------------------------
If (@Debit <> 0)
Begin
	If (@Transbase In ('GA','GR'))
		Select @HoldAmount = 0, @Available = 0
	Else
		Select @HoldAmount = @HoldAmount + @Available

	Select @JournalAmount = @Debit

	If (@Transbase In ('GA','GR'))
		Select @EntryDescription = 'Debit (Guarantee W/ ACH Settlement)', @RefCode = 'B'
	Else If (@Transbase In ('VA','VR'))
		Select @EntryDescription = 'Debit (Verify W/ ACH Settlement)', @RefCode = 'B'
	Else If (@Transbase In ('VO'))
		Select @EntryDescription = 'Debit (Verify W/O ACH Settlement)', @RefCode = 'T'
	Else If (@Transbase In ('GO'))
		Select @EntryDescription = 'Debit (Guarantee W/O ACH Settlement)', @RefCode = 'T'
	Else 
		Select @EntryDescription = 'Debit (Regular ACH Settlement)', @RefCode = 'B'


	Exec Ach_Journal_Post @CurrentDate,@BatchID,@PriID,@EntryDescription,@RefCode, @JournalAmount,0,0
End

If (@Transbase Not In ('GO','VO'))
Begin
	Update AchMerchAddl Set Balance = Balance + @Debit Where PriID = @PriID
	Select @HoldAmount = @HoldAmount + @Debit
End
-------------------------------------------------------------------------
--Journal Credit
-------------------------------------------------------------------------
If (@Credit <> 0)
Begin
	Select @JournalAmount = -1 * @Credit

	If (@Transbase In ('GA','GR'))
		Select @EntryDescription = 'Credit (Guarantee W/ ACH Settlement)', @RefCode = 'B'
	Else If (@Transbase In ('VA','VR'))
		Select @EntryDescription = 'Credit (Verify W/ ACH Settlement)', @RefCode = 'B'
	Else If (@Transbase In ('VO'))
		Select @EntryDescription = 'Credit (Verify W/O ACH Settlement)', @RefCode = 'T'
	Else If (@Transbase In ('GO'))
		Select @EntryDescription = 'Credit (Guarantee W/O ACH Settlement)', @RefCode = 'T'
	Else 
		Select @EntryDescription = 'Credit (Standard ACH Settlement)', @RefCode = 'B'

	Exec Ach_Journal_Post @CurrentDate,@BatchID,@PriID,@EntryDescription,@RefCode, @JournalAmount,0,0
End

If (@Transbase Not In ('GO','VO'))
Begin
	Update AchMerchAddl Set Balance = Balance - @Credit Where PriID = @PriID
	Select @HoldAmount = @HoldAmount - @Credit
End

-------------------------------------------------------------------------
--Journal Reserve Percentage
-------------------------------------------------------------------------
If (@Debit > 0 And @RsvPct <> 0 And @Transbase Not In ('GO','VO'))
Begin
	Select @EntryDescription = 'Funds Reserved Until ' + Convert(Varchar, DateAdd(Day,@RsvDay,GetDate()),1)
	Select @JournalAmount = -1 * Round((@Debit * (@RsvPct / 100)), 2)
	Exec Ach_Journal_Post @CurrentDate,@BatchID,@PriID,@EntryDescription,'V', @JournalAmount,0,0
	Select @LastJournalID = @@Identity	

	Select @JournalAmount = Round((@Debit * (@RsvPct / 100)), 2)
	Select @HoldAmount = @HoldAmount - @JournalAmount

	Insert Into Hold (JournalIDIn, PriID, PostedDate, DateToRelease, Amount, Type, ReleaseThisHold) 
	Values (@LastJournalID, @PriID, GetDate(), Convert(Varchar,DateAdd(Day,@RsvDay,GetDate()), 101), @JournalAmount, 'V', 'N')
	Select @HoldID = @@Identity
	
	Update Journal Set HoldID = @HoldID Where JournalID = @LastJournalID
End
-------------------------------------------------------------------------
--Journal Item Fee
-------------------------------------------------------------------------
If (@ItemFee <> 0)
Begin
	Select @EntryDescription =  Replicate('0', 7 - Len(@DebitCount + @CreditCount)) 
	Select @EntryDescription = @EntryDescription + Convert(Varchar,@DebitCount + @CreditCount) 
	Select @EntryDescription = @EntryDescription + ' items @ ' + Convert(Varchar,Round(@ItemFee, 2))
	Select @JournalAmount = -1 * Round(((@DebitCount + @CreditCount) * @ItemFee), 2)
	Exec Ach_Journal_Post @CurrentDate,@BatchID,@PriID,@EntryDescription,'F',@JournalAmount ,0,0			
	Select @LastJournalID = @@Identity

	Select @JournalAmount = Round(((@DebitCount + @CreditCount) * @ItemFee), 2)
	If (@PullFundsFromAnotherAchID = 0)
	Begin			
		Select @HoldAmount = @HoldAmount - @JournalAmount	
	End

	Update AchMerchAddl Set Balance = Balance - @JournalAmount Where PriID = @PRIID
	Select @FeeTotal = @FeeTotal + @JournalAmount
End
-------------------------------------------------------------------------
--Journal Processing Fee
-------------------------------------------------------------------------
/*
If (@ProcessFee <> 0)
Begin
	Select @JournalAmount = -1 * Round(Abs(@Debit + @Credit) * (@ProcessFee / 100), 2)

	Select @EntryDescription =  Convert(Varchar,@ProcessFee) + '% Process Fee'
	Exec Ach_Journal_Post @CurrentDate,@BatchID,@PriID,@EntryDescription,'F', @JournalAmount,0,0			
	Select @LastJournalID = @@Identity

	Select @JournalAmount = Round(Abs(@Debit + @Credit) * (@ProcessFee / 100), 2)
	If (@PullFundsFromAnotherAchID = 0)
		Select @HoldAmount = @HoldAmount - @JournalAmount	

	Update AchMerchAddl Set Balance = Balance - @JournalAmount Where PriID = @PRIID
	Select @FeeTotal = @FeeTotal + @JournalAmount
End
*/
-------------------------------------------------------------------------
--Journal File Load Fee
-------------------------------------------------------------------------
If @LoadFee <> 0 
Begin
	If (@TimesOfFile = 1 Or @Additional = 0)
		Select @JournalAmount = -1 * @LoadFee	
	Else
		Select @JournalAmount = -1 * @Additional

	Select @EntryDescription = 'File Input Fee'
	Exec Ach_Journal_Post @CurrentDate,@BatchID,@PriID,@EntryDescription,'F',@JournalAmount ,0,0			
	Select @LastJournalID = @@Identity

	If (@TimesOfFile = 1 Or @Additional = 0)
		Select @JournalAmount = @LoadFee
	Else
		Select @JournalAmount = @Additional

	If (@PullFundsFromAnotherAchID = 0)			
			Select @HoldAmount = @HoldAmount - Round(@JournalAmount,2)

	Update AchMerchAddl Set Balance = Balance - Round(@JournalAmount,2) Where PriID = @PRIID
	Select @FeeTotal = @FeeTotal + @JournalAmount
End
	-------------------------------------------------------------------------
	--Journal Statement Fee
	-------------------------------------------------------------------------
If (@MaintFee <> 0 And @Additional = 1)
Begin
	Select @JournalAmount = -1 * @MaintFee

	Select @EntryDescription =  'Account Maintenance Fee'
	Exec Ach_Journal_Post @CurrentDate,@BatchID,@PriID,@EntryDescription,'F',@JournalAmount ,0,0			
	Select @LastJournalID = @@Identity

	Select @JournalAmount = @MaintFee

	If (@PullFundsFromAnotherAchID = 0)	
		Select @HoldAmount = @HoldAmount - Round(@JournalAmount,2)

	Update AchMerchAddl Set Balance = Balance - Round(@JournalAmount	,2) Where PriID = @PRIID
	Select @FeeTotal = @FeeTotal + @JournalAmount
End

-------------------------------------------------------------------------
--Dealing With Item Fee and stuff that was taking off from other account
-------------------------------------------------------------------------
If (@TransBase Not In ('GO','VO'))
Begin
	If (@Debit > 0)
	Begin
		If (@OverDailyAmountLimitCount <> 0)
		Begin
			Print 'Add Code'
			/*
			Select @JournalAmount = Round((@Debit) * (vOverDailyFee / 100)), 2)
		
			Select @EntryDescription =  'Over Daily Limit Fee'
			Exec Ach_Journal_Post @CurrentDate,@BatchID,@PriID,@EntryDescription,'F',@JournalAmount ,0,0			
			Select @LastJournalID = @@Identity
			*/
		End

		If (@OverItemAmount <> 0)
		Begin
			Print 'Add Code'
			/*
			Select @JournalAmount = Round((@Debit) * (vOverDailyFee / 100)), 2)
		
			Select @EntryDescription =  'Over Daily Limit Fee'
			Exec Ach_Journal_Post @CurrentDate,@BatchID,@PriID,@EntryDescription,'F',@JournalAmount ,0,0			
			Select @LastJournalID = @@Identity
			*/
		End


		If (@FeePriID <> 0 And @FeePriID <> @PriID And @FeeTotal <> 0)
		Begin
			Select Top 1 @LastJournalID = JournalID From Journal With (NoLock) Where PostedDate >= Convert(Varchar,GetDate(),101) + ' 00:09:25' And RefCode = 'J' Order BY JournalID
			If (@@Rowcount = 0) 
			Begin
				Select Top 1 JournalID From Journal With (NoLock) Where PostedDate >= Convert(Varchar,GetDate(),101) + ' 00:09:25' And RefCode = 'B' Order BY JournalID
				If (@@Rowcount = 0) 
					Select @LastJournalID = 0
			End
			
			Select @FeeTotal = Sum(Amount) From Journal With (NoLock) Where JournalID >= @LastJournalID And RefCode In ('E', 'F') And PriID = @PriID And CommissionCategory Between 1 And 8
			Select @EntryDescription =  'TrxFees To ACT' + Replicate('0', 5 - Len(@FeePriID)) + @FeePriID
			Select @JournalAmount = -1 * @FeeTotal
			Exec Ach_Journal_Post @CurrentDate,0,@PriID,@EntryDescription,'C',@JournalAmount,0,0		

			Select @LastJournalID = @@Identity	
			Select @EntryDescription =  'TrxFees To ACT' + Replicate('0', 5 - Len(@PriID)) + @PriID
			Exec Ach_Journal_Post @CurrentDate,@LastJournalID,@FeePriID,@EntryDescription,'C',@FeeTotal ,0,0		
			Select @RefID = @@Identity
			Update Journal Set RefID = @RefID Where JournalID = @LastJournalID
		
			Update AchMerchAddl Set Balance = Balance + @FeeTotal Where PriID = @PriID

			Select @HoldAmount = @HoldAmount + @FeeTotal
		End
		-------------------------------------------------------------------------
		--Dealing with withdraw funds from Debit processing account instead of from bank
		-------------------------------------------------------------------------
		If (@PullFundsFromAnotherAchID <> 0)
		Begin
			Select @EntryDescription =  'ACH Withdraw From ACT' + Replicate('0', 5 - Len(@PullFundsFromAnotherAchID)) + @PullFundsFromAnotherAchID
			Exec Ach_Journal_Post @CurrentDate,0,@PriID,@EntryDescription,'C',@Credit,0,0		
			Select @LastJournalID = @@Identity	

			Select @EntryDescription =  'ACH Withdraw From ACT' + Replicate('0', 5 - Len(@PriID)) + @PriID
			Select @JournalAmount = -1 * @Credit
			Exec Ach_Journal_Post @CurrentDate,@LastJournalID,@PullFundsFromAnotherAchID,@EntryDescription,'C',@JournalAmount ,0,0		
			Select @RefID = @@Identity
			Update Journal Set RefID = @RefID Where JournalID = @LastJournalID
		
			Update AchMerchAddl Set Balance = Balance + @Credit Where PriID = @PriID
		End
		-------------------------------------------------------------------------
		If (@HoldAmount <> 0)
		Begin
			If (@TransBase In ('GA','GR'))
			Begin
				Select @JournalAmount = Isnull(Sum(Amount),0) From IncomingTrans With (NoLock) Where BatchID = @BatchID And PriID = @PriID And StatusID Not In (307, 357)
				If (@@Rowcount > 0)
				Begin
					If (@JournalAmount > 0)
					Begin
						Select @JournalAmount = -1 * @JournalAmount
						Select @EntryDescription =  'Reject by NCT Guarantee W/ ACH Settlement Batch ' + Convert(Varchar,@BatchID)
						Exec Ach_Journal_Post @CurrentDate,@BatchID,@PriID,@EntryDescription,'B',@JournalAmount,0,0		

						Select @JournalAmount = -1 * @JournalAmount
						Select @HoldAmount = @HoldAmount - @JournalAmount 	
					End
				End
			End
			Else If (@TransBase In ('VA','VR'))
			Begin
				Select @JournalAmount = Isnull(Sum(Amount),0) From IncomingTrans With (NoLock) Where BatchID = @BatchID And PriID = @PriID And StatusID <> 207
				If (@@Rowcount > 0)
				Begin
					If (@JournalAmount > 0)
					Begin
						Select @JournalAmount = -1 * @JournalAmount
						Select @EntryDescription =  'Reject by NCT Verify W/ ACH Settlement Batch ' + Convert(Varchar,@BatchID)
						Exec Ach_Journal_Post @CurrentDate,@BatchID,@PriID,@EntryDescription,'B',@JournalAmount,0,0	
						
						Select @JournalAmount = -1 * @JournalAmount
						Select @HoldAmount = @HoldAmount - @JournalAmount 	
					End
				End
			End
			If (@HoldAmount > 0)
			Begin
				Select @JournalAmount = -1 * @HoldAmount
				Select @EntryDescription =  'Funds Available On ' + Convert(Varchar,@HoldToRelease,1)
				Exec Ach_Journal_Post @CurrentDate,@BatchID,@PriID,@EntryDescription,'H',@JournalAmount,0,0
				Select @LastJournalID = @@Identity	

				Insert Into Hold (JournalIDIn, PriID, PostedDate, DateToRelease, Amount, Type, ReleaseThisHold) 
				Values (@LastJournalID, @PriID, GetDate(), Convert(Varchar,@HoldToRelease,101), @HoldAmount , 'H', 'N')
				Select @HoldID = @@Identity	
				
				Update Journal Set HoldID = @HoldID Where JournalID = @LastJournalID
			End
		End

		Select @Balance = Sum(Amount) From Journal With (NoLock) Where PriID = @PriID And RefCode In ('B', 'N', 'F', 'R', 'C', 'P', 'E', 'S', 'Z')
		
		If (@Balance < 0)
		Begin
			If (@OkToPostPending = 0)
			Begin	
				If (@OverDraft > 0)
				Begin
					Select @JournalAmount = -1 * @OverDraft
					Select @EntryDescription =  'OverDraft Fee'
					Exec Ach_Journal_Post @CurrentDate,@BatchID,@PriID,@EntryDescription,'F',@JournalAmount,0,0
					Select @LastJournalID = @@Identity	
		
					Select @Balance = @Balance - @OverDraft
				End
			
				Select @EntryDescription =  'EFT From ' + Rtrim(@TransRoute) + ':' + Rtrim(@AccountNo)
				Select @JournalAmount = -1 * @Balance
				Exec Ach_Journal_Post @EFTDate,@BatchID,@PriID,@EntryDescription,'C',@JournalAmount,0,0			
				Select @LastJournalID = @@Identity

				If (@PriID Not IN (334,3192))
				Begin
					Insert Into Pending (PriID, PostedDate, TransType, Amount, JournalID, Description, Type, TransRoute, AccountNo, NameOnAcct) 
					Values (@PriID, GetDate(), '27',@JournalAmount,@LastJournalID, 'PREAUTH DR', 'J',@TransRoute,@AccountNo,@NameOnAcct)
				End
				Else
				Begin
					Insert Into Pending (PriID, PostedDate, TransType, Amount, JournalID, Description, Type, TransRoute, AccountNo, NameOnAcct) 
					Values (@PriID, GetDate(), '37', @JournalAmount,@LastJournalID , 'PREAUTH DR', 'J',@TransRoute,@AccountNo,@NameOnAcct)
				End
					
				Select @PendingID = @@Identity
				Update Journal Set RefID = @PendingID Where JournalID = @LastJournalID
			End
			Else
			Begin
				Select Top 1 @LastJournalID = JournalID From Journal With (NoLock) Where PriID = @PriID Order By JournalID Desc
				Insert Into AchUncollectableFundsRegister (PriID, DateOfDebits, AmountWantToDebit, LastJournalID, Type) Values (@PriID, GetDate(),@Balance,@LastJournalID , 'R')

				If (@OverDraft > 0)
				Begin
					Insert Into AchUncollectableFundsRegister (PriID, DateOfDebits, AmountWantToDebit, LastJournalID, Type) 
					Values (@PriID, GetDate(),@OverDraft,@LastJournalID, 'F')
				End
			End
		End
	End
End

Update Batch Set ProcessedDate = GetDate() Where BatchID = @BatchID
Update BatchDetail Set ProcessedDate = GetDate() Where BatchID = @BatchID

If (@TransBase In ('GA','GR'))
Begin
	Update IncomingTrans Set StatusID = 304, DateProcessed = GetDate() Where BatchID = @BatchID And StatusID In (307, 357)
End
Else If (@TransBase In ('VA','VR'))
Begin
	If (@SettleBatchRightAway = 'Y')
	Begin
		Update IncomingTrans Set StatusID = 204, DateProcessed = GetDate() Where BatchID = @BatchID And StatusID = 207
	End
	Else
	Begin
		Update IncomingTrans Set StatusID = 217 Where BatchID = @BatchID And StatusID = 207
	End
End
Else If (@TransBase In ('VO'))
Begin
	Update IncomingTrans Set StatusID = 125 Where BatchID = @BatchID And StatusID = 101 And DateProcessed >= Convert(Varchar,GetDate(),101)
End
Else If (@TransBase In ('GO'))
Begin
	Update IncomingTrans Set StatusID = 126 Where BatchID = @BatchID And StatusID = 102 And DateProcessed >= Convert(Varchar,GetDate(),101)
End
Else If (@TransBase In ('AO'))
Begin
	If @SettleBatchRightAway = 'Y'
	Begin
		Update IncomingTrans Set StatusID = 4, DateProcessed = GetDate() Where BatchID = @BatchID And StatusID = 7
	End
	Else
	Begin
		Update IncomingTrans Set StatusID = 17, DateProcessed = GetDate() Where BatchID = @BatchID And StatusID = 18
	End
End


GO

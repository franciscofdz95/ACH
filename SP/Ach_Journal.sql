SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

ALTER  Procedure dbo.Ach_Journal
 @BankID Int
AS

Set Nocount On

--cursor variables
Declare @BatchID Int, @PRIID Int, @Debit Money, @Credit Money, 
				@DebitCount Int, @CreditCount Int, @OverDailyAmountLimitCount Money, 
				@OverItemAmount Money,@MerchantID Int

Declare @EntryDescription Nvarchar(60),
				@CurrentDate SmallDateTime, @HoldAmount Money, 
				@JournalAmount Money, @LastJournalID Int, 
				@FeeTotal Money, @TimesOfFile Int, @RefID Int, @HoldToRelease SmallDateTime,
				@Balance Money, @EFTDate SmallDateTime, @ReservedToRelease SmallDateTime, 
				@PendingID Int, @Available Money, @TotalCredit Money

Declare @JournalIDIn Int, @Amount Money, @HoldID Int

--Achmerchaddl variables
Declare @RsvPct Decimal(18,2), @RsrvTx Bit, @NameOnAcct Nvarchar(25), 
				@ItemFee Decimal(18,2), @RsvDay Int, @PullFundsFromAnotherAchID Int,
				@PostMethod Bit, @ProcessFee Decimal(18,2), @HoldOrPaidOut Nvarchar(50),
				@LoadFee Decimal(18,2), @Additional Decimal(18,2), @MaintFee Decimal(18,2),
				@FeePriID Int, @HoldDay Int, @ProcessType Nvarchar(1), @OverDraft Decimal(18,2),
				@TransRoute Nvarchar(9), @AccountNo Nvarchar(17), @MultiBatch Nvarchar(1)

Select @CurrentDate = GetDate(), @HoldAmount = 0
Exec Ach_GetEFTDate @EFTDate Output
Select @EFTDate = Convert(Varchar,@EFTDate,101)

Exec Ach_GetReservedToReleaseDate @ReservedToRelease Output
Select @ReservedToRelease = Convert(Varchar,@ReservedToRelease,101)

Declare cur CURSOR
READ_ONLY
FOR Select BatchID, PRIID, Debit, Credit, DebitCount, CreditCount, OverDailyAmountLimitCount, OverItemAmount, MerchantID 
		From Batch With (NoLock) 
		Where ProcessedDate Is Null 
		And PriID In (Select PriID From AchMerchAddl With (NoLock) Where BankID = @BankID) And TransBase Not In ('VO', 'GO', 'VA', 'GA', 'VR', 'GR') 
		And Not Exists (Select RefID From Journal With (NoLock) Where RefID = Batch.BatchID And RefCode = 'B' And PriID <> 5999)
		Order By BatchID

Open cur

Fetch Next From cur Into @BatchID, @PRIID, @Debit, @Credit, @DebitCount, @CreditCount, @OverDailyAmountLimitCount, @OverItemAmount, @MerchantID 
While (@@fetch_status <> -1)
Begin
	If (@@fetch_status <> -2)
	Begin
		Select 
				@RsvPct = IsNull(ReservePct,0), 
				@RsrvTx = IsNull(TxReserve,0), 
				@NameOnAcct = IsNull(dfiacctname,Replicate(' ',25)), 
				@RsvDay = Case When ReservePeriod Is Null Then 0 Else ReservePeriod + 1 End,
				@PullFundsFromAnotherAchID = IsNull(ACHIDForPullFundsFrom4PaidOut,0),
				@PostMethod = IsNull(MonthlyBase,0), 
				@ItemFee = IsNull(ItemFee,0), 
				@ProcessFee = IsNull(ProcessFee,0), 
				@LoadFee = IsNull(FileLoadFee,0), 
				@Additional = IsNull(AdditionalFiles,0), 
				@FeePriID = IsNull(TrxFeeToAcct,0),
				@HoldOrPaidOut = IsNull(IsThisHoldDayOrPaidOutDay,'H'), 
				@MaintFee = IsNull(StatementFee,0),
				@HoldDay = IsNull(HoldPeriod,0), 
				@OverDraft = IsNull(OverDraftFee,0),
				@TransRoute = IsNull(TransRoute,Replicate(' ',9)), 
				@AccountNo = IsNull(AccountNo,Replicate(' ',17)),
				@MultiBatch = IsNull(CreateMultiBatch,'N')
		From AchMerchAddl With (NoLock) Where PriID = @PRIID

		If @@Rowcount = 0
			Select 
					@RsvPct = 0, 
					@RsrvTx = 0, 
					@NameOnAcct = Replicate(' ',25), 
					@RsvDay = 0,
					@PullFundsFromAnotherAchID = 0,
					@PostMethod = 0, 
					@ItemFee = 0, 
					@ProcessFee = 0, 
					@LoadFee = 0, 
					@Additional = 0, 
					@FeePriID = 0,
					@HoldOrPaidOut = 'H', 
					@MaintFee = 0,
					@HoldDay = 0, 
					@OverDraft = 0,
					@TransRoute = Replicate(' ',9), 
					@AccountNo = Replicate(' ',17),
					@MultiBatch = 'N'

		
		Select @ProcessType = Test From [NB-Red].Central.dbo.Merchants Where PriID = @PRIID
		If @@Rowcount = 0
			Select @ProcessType = ''
		
		-------------------------------------------------------------------------
		Exec Ach_GetHoldToReleaseDate @HoldToRelease Output,@CurrentDate,@HoldOrPaidOut,@HoldDay
		-------------------------------------------------------------------------
		--@TimesOfFile is use to access monthly fees
		-------------------------------------------------------------------------
		Select @TimesOfFile = Count(*) From Batch Where PostedDate >= Convert(Varchar,Month(GetDate())) + '/01/' + Convert(Varchar,Year(GetDate())) And PostedDate < Convert(Varchar,Month(DateAdd(Month,1,GetDate()))) + '/01/' + Convert(Varchar,Year(GetDate())) 
		And PriID = @PRIID
		
		If @TimesOfFile = 0
			Select @TimesOfFile = 1
		-------------------------------------------------------------------------
		--Begin Case
		-------------------------------------------------------------------------
		If @HoldOrPaidOut In ('H','B')
		Begin
			-------------------------------------------------------------------------
			--Release reserve if there is any.
			-------------------------------------------------------------------------
			If @HoldOrPaidOut = 'H'And @BankID <> 6 
			Begin
				/*
				Insert Into Journal (PostedDate, RefID, PriID, EntryDescription, RefCode, Amount, HoldID, StatusID, CommissionCategory) 
				Select GetDate(), A.JournalIDIn, @PriID, 'Release From Reserved','X', A.Amount, A.HoldID, 0 As StatusID, 64 As CommissionCategory From Hold A With (NoLock) , AchMerchAddl B With (NoLock) 
				Where A.DateToRelease <= @ReservedToRelease And A.Type = 'V' And A.DatePaid Is Null And A.PriID = B.PriID And A.PriID = @PriID And B.StopEFTFrom = 'N' Order By HoldID

				Update Hold Set Datepaid = GetDate(), JournalIDPaid = b.JournalID From Hold a Inner Join Journal b On a.HoldID = b.HoldID 
				Where b.Refcode = 'X' And a.DatePaid Is Null 
				*/
				DECLARE cur2 CURSOR READ_ONLY FOR 
					Select A.JournalIDIn, A.Amount, A.HoldID From Hold A With (NoLock), AchMerchAddl B With (NoLock) 
					Where A.DateToRelease <= @ReservedToRelease And A.Type = 'V' And A.DatePaid Is Null And A.PriID = B.PriID And A.PriID = @PriID And B.StopEFTFrom = 'N' Order By HoldID
				
				Open cur2
				
				Fetch Next From cur2 Into @JournalIDIn, @Amount, @HoldID
				While (@@fetch_status <> -1)
				Begin
					If (@@fetch_status <> -2)
					Begin
						Select @EntryDescription = 'Release From Reserved'
						Exec Ach_PostJournal @CurrentDate,@JournalIDIn,@PriID,@EntryDescription,'X', @Amount,@HoldID,0
						Select @LastJournalID = @@Identity
	
						Update Hold Set Datepaid = GetDate(), JournalIDPaid = @LastJournalID Where HoldID = @HoldID
					End
					Fetch Next From cur2 Into @JournalIDIn, @Amount, @HoldID
				End
				
				Close cur2
				Deallocate cur2

				Select @Balance = Sum(Amount) From Journal With (NoLock) Where PriID = @PriID And RefCode In ('B', 'N', 'F', 'R', 'C', 'P', 'E', 'S', 'Z')
				Select @Available = Sum(Amount) From Journal With (NoLock) Where PriID = @PriID And RefCode In ('B', 'N', 'F', 'R', 'C', 'P', 'E','V', 'H', 'X')
			End

			Select @HoldAmount = 0
			If @HoldOrPaidOut = 'B'
			  Select @HoldAmount = 0
			Else
			  Select @HoldAmount = @HoldAmount + @Available
 			-------------------------------------------------------------------------
			--Journal Debit
			-------------------------------------------------------------------------
			If @Debit <> 0 
			Begin
				If @BankID In (6,7,11,12)
					Select Top 1 @EntryDescription = 'Total Amount of Debit (' + Description + ')' From IncomingTrans With (NoLock) Where PriID = @PriID And BatchID = @BatchID And StatusID = 7
				Else
					Select @EntryDescription = 'Total Amount of Debit'			
				
				Exec Ach_PostJournal @CurrentDate,@BatchID,@PriID,@EntryDescription,'B', @Debit,0,0
				Update AchMerchAddl Set Balance = Balance + @Debit Where PriID = @PRIID
				Select @HoldAmount = @HoldAmount + @Debit
			End
			Else
			Begin
				If @ProcessType = 'D'
				Begin
					Select @EntryDescription = 'Total Amount of Debit (Draft)'
					Exec Ach_PostJournal @CurrentDate,@BatchID,@PriID,@EntryDescription,'B', 0,0,0
				End
			End
			-------------------------------------------------------------------------
			--Journal Credit
			-------------------------------------------------------------------------
			If @Credit <> 0 
			Begin
				If @BankID In (6,7,11,12)
					Select Top 1 @EntryDescription = 'Total Amount of Credit (' + Description + ')' From IncomingTrans With (NoLock) Where PriID = @PriID And BatchID = @BatchID And StatusID = 7
				Else
					Select @EntryDescription = 'Total Amount of Credit'			
				
				Exec Ach_PostJournal @CurrentDate,@BatchID,@PriID,@EntryDescription,'B', @Credit,0,0

				If (@HoldOrPaidOut <> 'H' And @PullFundsFromAnotherAchID = 0 And @BankID <> 6) Or @HoldOrPaidOut = 'B'
					Select @HoldAmount = @HoldAmount - @Credit

				Update AchMerchAddl Set Balance = Balance - @Credit Where PriID = @PRIID
			End
			-------------------------------------------------------------------------
			--Journal Reserve Percentage
			-------------------------------------------------------------------------
			If @Debit <> 0 And @RsvPct <> 0 
			Begin
				Select @EntryDescription = 'Reserved Until ' + Convert(Varchar, DateAdd(Day,@RsvDay,GetDate()),1)
				Select @JournalAmount = -1 * Round((@Debit * (@RsvPct / 100)), 2)		
				Exec Ach_PostJournal @CurrentDate,@BatchID,@PriID,@EntryDescription,'V',@JournalAmount ,0,0			
				Select @HoldAmount = @HoldAmount + @JournalAmount	
				Select @LastJournalID = @@Identity

				If @RsrvTx = 1
					Insert Into Pending (PriID, PostedDate, ProcessedDate, Amount, Type, TransType, JournalID, TransRoute, AccountNo, NameOnAcct, Description) 
					Values (999, GetDate(), Null, Round((@Debit * (@RsvPct / 100)), 2), 'J', '22', @LastJournalID, '', '', @NameOnAcct, 'RESERV TX ')
	
				Insert Into Hold (JournalIDIn, PriID, PostedDate, DateToRelease, Amount, Type, ReleaseThisHold) 
				Values (@LastJournalID, @PriID, GetDate(), Convert(Varchar,DateAdd(Day,@RsvDay,GetDate()), 101), Round((@Debit * (@RsvPct / 100)), 2), 'V', 'N')
				
				Select @HoldID = @@Identity
				Update Journal Set HoldID = @HoldID Where JournalID = @LastJournalID
			End
			-------------------------------------------------------------------------
			--Journal Item Fee
			-------------------------------------------------------------------------
			If @ItemFee <> 0 
			Begin
				Select @JournalAmount = -1 * Round(((@DebitCount + @CreditCount) * @ItemFee), 2)
		
				If @PostMethod = 0
				Begin
					Select @EntryDescription =  Replicate('0', 7 - Len(@DebitCount + @CreditCount)) 
					Select @EntryDescription = @EntryDescription + Convert(Varchar,@DebitCount + @CreditCount) 
					Select @EntryDescription = @EntryDescription + ' items @ ' + Convert(Varchar,Round(@ItemFee, 2))
	
					Exec Ach_PostJournal @CurrentDate,@BatchID,@PriID,@EntryDescription,'F',@JournalAmount ,0,0			
					Select @LastJournalID = @@Identity
		
					If @PullFundsFromAnotherAchID = 0			
							Select @HoldAmount = @HoldAmount - Round(((@DebitCount + @CreditCount) * @ItemFee), 2)	
		
					Update AchMerchAddl Set Balance = Balance - Round(((@DebitCount + @CreditCount) * @ItemFee), 2) Where PriID = @PRIID
					Select @FeeTotal = @FeeTotal + Round(((@DebitCount + @CreditCount) * @ItemFee), 2)
				End
				Else
				Begin
					Insert Into Pending_Journal (PostedDate, RefID, PriID, EntryDescription, RefCode, Amount) 
					Values (GetDate(), @BatchID, @PriID, 'Item Fee', 'B', -1 * Round(((@DebitCount + @CreditCount) * @ItemFee), 2))
					
					Select @PendingID = @@Identity
				End
			End
			-------------------------------------------------------------------------
			--Journal Processing Fee
			-------------------------------------------------------------------------
			If @ProcessFee <> 0 
			Begin
				If @HoldOrPaidOut = 'H'
					Select @JournalAmount = -1 * Round(Abs(@Debit + @Credit) * (@ProcessFee / 100), 2)
				Else If @HoldOrPaidOut = 'B'
					Select @JournalAmount = -1 * Round(@Debit * (@ProcessFee / 100), 2) + 10 * @CreditCount
	
				If @PostMethod = 0
				Begin
					Select @EntryDescription =  Convert(Varchar,@ProcessFee,101) + '% Process Fee'
					Exec Ach_PostJournal @CurrentDate,@BatchID,@PriID,@EntryDescription,'F',@JournalAmount ,0,0			
					Select @LastJournalID = @@Identity
		
					If @HoldOrPaidOut = 'H'
						Select @JournalAmount = Round(Abs(@Debit + @Credit) * (@ProcessFee / 100), 2)
					Else If @HoldOrPaidOut = 'B'
						Select @JournalAmount = Round(@Debit * (@ProcessFee / 100), 2) + 10 * @CreditCount

					If (@PullFundsFromAnotherAchID = 0 And @HoldOrPaidOut = 'H') Or @HoldOrPaidOut = 'B'			
							Select @HoldAmount = @HoldAmount - @JournalAmount	
		
					Update AchMerchAddl Set Balance = Balance - @JournalAmount Where PriID = @PRIID
					Select @FeeTotal = @FeeTotal + @JournalAmount
				End
				Else
				Begin
					Insert Into Pending_Journal (PostedDate, RefID, PriID, EntryDescription, RefCode, Amount) 
					Values (GetDate(), @BatchID, @PriID, Convert(Varchar,Round((@ProcessFee / 100), 2),101) + '% Process Fee', 'B', @JournalAmount)
					
					Select @PendingID = @@Identity
				End
			End
			-------------------------------------------------------------------------
			--Journal File Load Fee
			-------------------------------------------------------------------------
			If @LoadFee <> 0 
			Begin
				If @TimesOfFile = 1 Or @Additional = 0 
					Select @JournalAmount = -1 * @LoadFee
				Else
					Select @JournalAmount = -1 * @Additional
	
				If @PostMethod = 0
				Begin
						Select @EntryDescription = 'File Input Fee'
						Exec Ach_PostJournal @CurrentDate,@BatchID,@PriID,@EntryDescription,'F',@JournalAmount ,0,0			
						Select @LastJournalID = @@Identity

						If @TimesOfFile = 1 Or @Additional = 0 
							Select @JournalAmount = @LoadFee
						Else
							Select @JournalAmount = @Additional

						If (@PullFundsFromAnotherAchID = 0)			
								Select @HoldAmount = @HoldAmount - Round(@JournalAmount	,2)
			
						Update AchMerchAddl Set Balance = Balance - Round(@JournalAmount	,2) Where PriID = @PRIID
				End
				Else
				Begin
					Insert Into Pending_Journal (PostedDate, RefID, PriID, EntryDescription, RefCode, Amount) 
					Values (GetDate(), @BatchID, @PriID, 'File Input Fee', 'B', @JournalAmount)
					
					Select @PendingID = @@Identity
				End
			End
			-------------------------------------------------------------------------
			--Journal Statement Fee
			-------------------------------------------------------------------------
			If @MaintFee <> 0 And @Additional = 1
			Begin
				Select @JournalAmount = -1 * @MaintFee
	
				If @PostMethod = 0
				Begin
						Select @EntryDescription =  'Account Maintenance Fee'
						Exec Ach_PostJournal @CurrentDate,@BatchID,@PriID,@EntryDescription,'F',@JournalAmount ,0,0			
						Select @LastJournalID = @@Identity
			
						Select @JournalAmount = @MaintFee

						If (@PullFundsFromAnotherAchID = 0)			
								Select @HoldAmount = @HoldAmount - Round(@JournalAmount	,2)
			
						Update AchMerchAddl Set Balance = Balance - Round(@JournalAmount	,2) Where PriID = @PRIID
						Select @FeeTotal = @FeeTotal + @MaintFee
				End
				Else
				Begin
					Insert Into Pending_Journal (PostedDate, RefID, PriID, EntryDescription, RefCode, Amount) 
					Values (GetDate(), @BatchID, @PriID, 'Account Maintenance Fee', 'B', @JournalAmount)
					
					Select @PendingID = @@Identity
				End
			End
			-------------------------------------------------------------------------
			--Journal Over Daily Amount LimitCount And Over Item Amount
			-------------------------------------------------------------------------
			If @Debit > 0
			Begin
				If @OverDailyAmountLimitCount <> 0
				Begin
					Select @JournalAmount = -1 * Round((@Debit * (@OverDailyAmountLimitCount / 100)),2)
					
					If @PostMethod = 0
					Begin
						Select @EntryDescription =  'Over Daily Limit Fee'
						Exec Ach_PostJournal @CurrentDate,@BatchID,@PriID,@EntryDescription,'F',@JournalAmount ,0,0			
						Select @LastJournalID = @@Identity
				
						Select @JournalAmount = Round((@Debit * (@OverDailyAmountLimitCount / 100)),2)

						If (@PullFundsFromAnotherAchID = 0)			
								Select @HoldAmount = @HoldAmount - Round(@JournalAmount	,2)
				
						Update AchMerchAddl Set Balance = Balance - Round(@JournalAmount	,2) Where PriID = @PRIID
					End
					Else
					Begin
						Insert Into Pending_Journal (PostedDate, RefID, PriID, EntryDescription, RefCode, Amount) 
						Values (GetDate(), @BatchID, @PriID, 'Over Daily Limit Fee', 'B', @JournalAmount)
						
						Select @PendingID = @@Identity
					End
				End
				-------------------------------------------------------------------------
				If @OverItemAmount <> 0
				Begin
					Select @JournalAmount = -1 * Round((@Debit * (@OverItemAmount / 100)),2)
					
					If @PostMethod = 0
					Begin
							Select @EntryDescription =  'Over Item Limit Fee'
							Exec Ach_PostJournal @CurrentDate,@BatchID,@PriID,@EntryDescription,'F',@JournalAmount ,0,0			
							Select @LastJournalID = @@Identity

							Select @JournalAmount = Round((@Debit * (@OverItemAmount / 100)),2)
					
							If (@PullFundsFromAnotherAchID = 0)			
									Select @HoldAmount = @HoldAmount - Round(@JournalAmount	,2)
					
							Update AchMerchAddl Set Balance = Balance - Round(@JournalAmount	,2) Where PriID = @PRIID
					End
					Else
					Begin
						Insert Into Pending_Journal (PostedDate, RefID, PriID, EntryDescription, RefCode, Amount) 
						Values (GetDate(), @BatchID, @PriID, 'Over Item Limit Fee', 'B', @JournalAmount)
						
						Select @PendingID = @@Identity
					End
				End
			End
			-------------------------------------------------------------------------
			--Dealing With Item Fee and stuff that was taking off from other account
			-------------------------------------------------------------------------
			If @FeePriID <> 0 And @FeePriID <> @PriID
			Begin
				Select @FeeTotal = 0
				Select Top 1 @LastJournalID = JournalID From Journal With (NoLock) Where PostedDate >= Convert(Varchar,GetDate(),101) + ' 00:09:25' And RefCode = 'J' Order BY JournalID
				If @LastJournalID Is Null
				Begin
					Select Top 1 JournalID From Journal With (NoLock) Where PostedDate >= Convert(Varchar,GetDate(),101) + ' 00:09:25' And RefCode = 'B' Order BY JournalID
					If @LastJournalID Is Null
						Select @LastJournalID = 0
				End
				
				Select @FeeTotal = Sum(Amount) From Journal With (NoLock) Where JournalID >= @LastJournalID And RefCode In ('E', 'F') And PriID = @PriID And CommissionCategory Between 1 And 8
				Select @EntryDescription =  'TrxFees To ACT' + Replicate('0', 5 - Len(@FeePriID)) + @FeePriID
				Select @JournalAmount = -1 * @FeeTotal
				Exec Ach_PostJournal @CurrentDate,0,@PriID,@EntryDescription,'C',@JournalAmount,0,0		
	
				Select @LastJournalID = @@Identity	
				Select @EntryDescription =  'TrxFees To ACT' + Replicate('0', 5 - Len(@PriID)) + @PriID
				Exec Ach_PostJournal @CurrentDate,@LastJournalID,@FeePriID,@EntryDescription,'C',@FeeTotal ,0,0		
				Select @RefID = @@Identity
				Update Journal Set RefID = @RefID Where JournalID = @LastJournalID
			
				Update AchMerchAddl Set Balance = Balance + @FeeTotal Where PriID = @PriID
				If (@PullFundsFromAnotherAchID = 0)			
						Select @HoldAmount = @HoldAmount - @FeeTotal
			End
			-------------------------------------------------------------------------
			--Dealing with withdraw funds from Debit processing account instead of from bank
			-------------------------------------------------------------------------
			If @PullFundsFromAnotherAchID <> 0 And @Credit <> 0
			Begin
				Select @EntryDescription =  'ACH Withdraw From ACT' + Replicate('0', 5 - Len(@PullFundsFromAnotherAchID)) + @PullFundsFromAnotherAchID
				Exec Ach_PostJournal @CurrentDate,0,@PriID,@EntryDescription,'C',@Credit,0,0		
				Select @LastJournalID = @@Identity	
	
				Select @EntryDescription =  'ACH Withdraw From ACT' + Replicate('0', 5 - Len(@PriID)) + @PriID
				Select @JournalAmount = -1 * @Credit
				Exec Ach_PostJournal @CurrentDate,@LastJournalID,@PullFundsFromAnotherAchID,@EntryDescription,'C',@JournalAmount ,0,0		
				Select @RefID = @@Identity
				Update Journal Set RefID = @RefID Where JournalID = @LastJournalID
			
				Update AchMerchAddl Set Balance = Balance + @Credit Where PriID = @PriID
			End
			-------------------------------------------------------------------------
			--Dealing with withdraw funds from Debit processing account instead of from bank
			-------------------------------------------------------------------------
			If @HoldOrPaidOut = 'B' Or (@HoldOrPaidOut = 'H' And @BankID <> 6 And @HoldAmount > 0)
			Begin
				Select @EntryDescription =  'Held Until ' + Substring(Convert(Varchar,@HoldToRelease,101),1,6)
				Select @EntryDescription = @EntryDescription + Substring(Convert(Varchar,@HoldToRelease,101),9,2) 
				Select @JournalAmount = -1 * @HoldAmount 	
				Exec Ach_PostJournal @CurrentDate,@BatchID,@PriID,@EntryDescription,'H',@JournalAmount,0,0		
				Select @LastJournalID = @@Identity
				
				Insert Into Hold (JournalIDIn, PriID, PostedDate, DateToRelease, Amount, Type, ReleaseThisHold) 
				Values (@LastJournalID, @PriID, GetDate(), Convert(Varchar,@HoldToRelease,101), @HoldAmount , 'H', 'N')
	
				Select @HoldID = @@Identity
				
				Update Journal Set HoldID = @HoldID Where JournalID = @LastJournalID
			End
			-------------------------------------------------------------------------
			--Journal OverDraft Fee
			-------------------------------------------------------------------------
			If @HoldOrPaidOut = 'H' And @BankID <> 6
			Begin
				Select @Balance = Sum(Amount) From Journal With (NoLock) Where PriID = @PriID And RefCode In ('B', 'N', 'F', 'R', 'C', 'P', 'E', 'S', 'Z')
				
				If @Balance < 0
				Begin
	
					Select Top 1 @LastJournalID = JournalID From Journal With (NoLock) Where PriID = @PriID Order By JournalID Desc 
					If @OverDraft > 0
					Begin
						Select @JournalAmount = -1 * @OverDraft
						If @PostMethod = 0
						Begin
							Select @EntryDescription =  'OverDraft Fee'	
							Exec Ach_PostJournal @CurrentDate,@BatchID,@PriID,@EntryDescription,'F',@JournalAmount ,0,0			
							Select @LastJournalID = @@Identity
	
							Select @Balance = @Balance - @OverDraft
						End
						Else
						Begin
							Insert Into Pending_Journal (PostedDate, RefID, PriID, EntryDescription, RefCode, Amount) 
							Values (GetDate(), @BatchID, @PriID, 'Item Fee', 'B', @JournalAmount)
							
							Select @PendingID = @@Identity
						End
						
						Select @EntryDescription =  'EFT From ' + Rtrim(@TransRoute) + ':' + Rtrim(@AccountNo)
						Select @JournalAmount = -1 * @Balance
						Exec Ach_PostJournal @EFTDate,@BatchID,@PriID,@EntryDescription,'C',@JournalAmount,0,0			
						Select @LastJournalID = @@Identity
					
						If @PriID Not IN (334,3192)
							Insert Into Pending (PriID, PostedDate, TransType, Amount, JournalID, Description, Type, TransRoute, AccountNo, NameOnAcct) 
							Values (@PriID, GetDate(), '27',@JournalAmount,@LastJournalID, 'PREAUTH DR', 'J',@TransRoute,@AccountNo,@NameOnAcct)
						Else
							Insert Into Pending (PriID, PostedDate, TransType, Amount, JournalID, Description, Type, TransRoute, AccountNo, NameOnAcct) 
							Values (@PriID, GetDate(), '37', @JournalAmount,@LastJournalID , 'PREAUTH DR', 'J',@TransRoute,@AccountNo,@NameOnAcct)
							
						Select @PendingID = @@Identity
						Update Journal Set RefID = @PendingID Where JournalID = @LastJournalID
						Update AchMerchAddl Set Balance = 0 Where PriID = @PriID
					End	
					
				End
			End
		End
		Else If @HoldOrPaidOut In ('P','C')
		Begin
			If @Debit <> 0 
			Begin
				If @MultiBatch = 'Y'
				Begin
					Select Top 1 @EntryDescription = 'Amount of Debit (' + Description + ')' From IncomingTrans With (NoLock) Where BatchID = @BatchID And StatusID = 7
					Exec Ach_PostJournal @EFTDate,@BatchID,@PriID,@EntryDescription,'B',@Debit,0,0
				End
				Else
					Exec Ach_PostJournal @EFTDate,@BatchID,@PriID,'Total Amount of Debit','B',@Debit,0,0

				Update AchMerchAddl Set Balance = Balance + @Debit Where PriID = @PriID
				Select @EntryDescription =  'Held Until ' + Substring(Convert(Varchar,@HoldToRelease,101),1,6)
				Select @EntryDescription = @EntryDescription + Substring(Convert(Varchar,@HoldToRelease,101),9,2) 
				Select @JournalAmount = -1 * @Debit
				Exec Ach_PostJournal @EFTDate,@BatchID,@PriID,@EntryDescription,'H',@JournalAmount,0,0
				Select @LastJournalID = @@Identity

				Insert Into Hold (JournalIDIn, PriID, PostedDate, DateToRelease, Amount, Type, ReleaseThisHold) 
				Values (@LastJournalID, @PriID, GetDate(), Convert(Varchar,@HoldToRelease,101), @Debit , 'H', 'N')
				Select @HoldID = @@Identity
				
				Update Journal Set HoldID = @HoldID Where JournalID = @LastJournalID
			End
			Select @HoldAmount = 0
		End
		-------------------------------------------------------------------------
		--End Case
		-------------------------------------------------------------------------
		--Update Batch Set ProcessedDate = GetDate() Where BatchID = @BatchID
		--Update BatchDetail Set ProcessedDate = GetDate() Where BatchID = @BatchID
	End
	Fetch Next From cur Into @BatchID, @PRIID, @Debit, @Credit, @DebitCount, @CreditCount, @OverDailyAmountLimitCount, @OverItemAmount, @MerchantID 
End

Close cur
Deallocate cur

Exec Ach_Journal_HoldRelease @BankID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


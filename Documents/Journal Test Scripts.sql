--update batch set ProcessedDate = '12/1/2005' where batchid not between 88822 and 88939
--update batch set ProcessedDate = null where batchid between 88822 and 88939
--delete from hold where holdid >= 88082
--delete from pending where pendingID > 83297
delete from journal where journalid > 850264

select * from batch
where posteddate >= '12/28/2005' and posteddate < '12/29/2005' order by batchid

--delete journal where journalid > 850264 
select max(journalid) from journal
select * from AchMerchAddl With (NoLock) Where PriID = 5180
select top 5 * from hold Where PriID = 296 order by holdid desc
select RefID,PriID,EntryDescription,Refcode,HoldID,Amount,commissioncategory from journal 
	where journalid > 850264 order by priid,refcode,amount asc
select * from batch where posteddate >= '12/29/2005' and posteddate < '12/30/2005' order by batchid

Select Count(*) From Batch Where (PostedDate >= Convert(Varchar,Month(GetDate())) + '/01/' + Convert(Varchar,Year(GetDate())) And PostedDate < Convert(Varchar,Month(DateAdd(Month,1,GetDate()))) + '/01/' + Convert(Varchar,Year(GetDate()))) 
And PriID = 351

Select BatchID, PrIID From Batch Where PostedDate >= '12/01/05 00:00:00' And PostedDate <= '12/31/05 11:59:00' Order By PriID

exec Ach_Journal_HoldRelease 11
select * from batch where batchid = 88836
exec Ach_Journal_Process @BankID=11,
	@BatchID = 88836, 
	@PRIID = 3190, 
	@Debit = 0, 
	@Credit = .04, 
	@DebitCount = 0, 
	@CreditCount = 1, 
	@OverDailyAmountLimitCount = 0, 
	@OverItemAmount = 0

--Release reserve
select * from hold where priid in
(select priid from batch
where posteddate >= '12/28/2005' and posteddate < '12/29/2005')
and datetorelease >= '12/1/2005' and type = 'v'

update hold set datepaid = null,journalidpaid = null from hold where priid in
(select priid from batch
where posteddate >= '12/28/2005' and posteddate < '12/29/2005')
and datetorelease >= '12/1/2005' and type = 'v'

Select a.priid,bankid,b.IsThisHoldDayOrPaidOutDay,A.JournalIDIn, A.Amount, A.HoldID From Hold A With (NoLock), AchMerchAddl B With (NoLock) 
Where A.DateToRelease <= '12/31/2005' And A.Type = 'H' And A.DatePaid Is Null And A.PriID = B.PriID And bankid = 11 And B.StopEFTFrom = 'N' Order By HoldID
--Release Hold
Select *,A.JournalIDIn, A.Amount, A.HoldID, A.PriID 
From Hold A With (NoLock), AchMerchAddl B With (NoLock) 
Where A.Type = 'H' And A.DatePaid Is Null And A.PriID < 5999 And A.PriID = B.PriID 
		And B.StopEFTFrom = 'N' And B.IsThisHoldDayOrPaidOutDay = 'H' 
		And A.DateToRelease <= '12/31/2005' And B.BankID = 11 Order By Journalidin

update hold set datepaid = null,journalidpaid = null 
From Hold A With (NoLock), AchMerchAddl B With (NoLock)
Where A.Type = 'H' And A.PriID < 5999 And A.PriID = B.PriID 
		And B.StopEFTFrom = 'N' And B.IsThisHoldDayOrPaidOutDay = 'H' 
		And A.Datepaid >= '12/29/2005' And B.BankID = 11 







--update batch set ProcessedDate = '12/1/2005' where batchid not between 93580 and 93701
--update batch set ProcessedDate = '2/1/2006' where ProcessedDate is null
update batch set ProcessedDate = null where batchid > 96435
delete from hold where holdid > 91027
delete from pending where pendingID > 83297
delete from journal where journalid > 852885  --and refcode not in('e','j','r')


update batch set ProcessedDate = null where batchid > 96435 and priid = 3347
delete from journal where journalid > 853888 and priid = 3347 --and refcode not in('e','j','r')
/*
	update batch set ProcessedDate = null where batchid = 98139 and priid = 3347
	delete from journal where priid = 3347 and refid = 98139
*/
select RefID,EntryDescription,Refcode,Amount,commissioncategory from journal 
	where journalid > 853888 and refcode not in('e','j','r') order by journalid asc


select * from incomingtrans where batchid in
(select batchid from batch where priid in
(select priid from achmerchaddl where bankid in (11,12)) and posteddate >= '2/3/2006' and posteddate < '2/4/2006')


select TrxFeeToAcct,* from achmerchaddl where priid = 3347
select top 500 * from batch order by batchid desc 

select * from batch
where batchid between 93580 and 93701 order by priid,debit

select * from batch where batchid =98136


select top 100 * from hold order by holdid desc
select top 100 * from pending order by pendingid desc

select * from batch where  batchid > 96435 
select * from journal where journalid > 852885

select * from batch where  batchid >= 98136 and PriID = 3347--NCT
select * from journal where journalid > 853888 and PriID = 3347--NCT

--delete journal where journalid > 850386 
select max(journalid) from journal --852885
select max(batchid) from batch --96435

select max(batchid) from batch and PriID = 3347--NCT 98136 
select max(journalid) from journal and PriID = 3347--NCT 853888

select * from batch where batchid = 98139
select * from AchMerchAddl With (NoLock) Where PriID = 296
select top 5 * from hold Where PriID = 296 order by holdid desc


select * from batch where posteddate >= '2/6/2006' and posteddate < '2/7/2006' order by batchid


Select Count(*) From Batch Where (PostedDate >= Convert(Varchar,Month(GetDate())) + '/01/' + Convert(Varchar,Year(GetDate())) And PostedDate < Convert(Varchar,Month(DateAdd(Month,1,GetDate()))) + '/01/' + Convert(Varchar,Year(GetDate()))) 
And PriID = 351

Select BatchID, PrIID From Batch Where PostedDate >= '12/01/05 00:00:00' And PostedDate <= '12/31/05 11:59:00' Order By PriID

exec Ach_Journal_HoldRelease 11
select * from batch where batchid = 93580
exec Ach_Journal_Process @BankID=12,
	@BatchID = 93580, 
	@PRIID = 296, 
	@Debit = 20213.2500, 
	@Credit = 0, 
	@DebitCount = 84, 
	@CreditCount = 0, 
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



Select * From Batch With (NoLock) 
Where ProcessedDate Is Null And PriID In (Select PriID From AchMerchAddl With (NoLock) Where BankID = 12) 
And TransBase Not In ('VO', 'GO', 'VA', 'GA', 'VR', 'GR') Order By BatchID

	
Select 'Total Amount of Debit (' + Description + ')' From IncomingTrans With (NoLock) Where PriID = 296 And BatchID = 93580 And StatusID = 7


select * from achmerchaddl where priid = 3347
declare @HoldToRelease smalldatetime
Exec Ach_GetHoldToReleaseDate @HoldToRelease Output,'2/6/2006','H',3
select @HoldToRelease
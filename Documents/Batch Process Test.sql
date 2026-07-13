select * from incomingtrans where dateprocessed >= '12/23/005' and dateprocessed < '12/24/005'
select * from incomingtrans where intransid < 11759047 and statusid = 7

select * from incomingtrans where intransid >= 11759047

select distinct a.priid from incomingtrans a inner join achmerchaddl b on a.priid = b.priid 
where a.statusid = 2 and a.priid <> 831 and bankid in (10,14)

select a.source from incomingtrans a inner join achmerchaddl b on a.priid = b.priid 
where a.statusid = 2 and a.priid = 5358 and bankid in (10)

Select A.IntransID, A.Description, A.CompanyName, A.DescDate, A.TransType, A.Amount, A.StatusID, B.PriID, A.Secc, A.Source 
From IncomingTrans A, AchMerchAddl B 
Where A.PriID = B.PriID And A.StatusID = 2 And A.PriID = 5358 And A.NextProcessDate >= '06/10/04 00:00:00' 
And (A.BatchID Is Null Or A.BatchID = 0) And B.BankID = 10 And A.TransType In ('22', '32','27', '37')

select count(*) from incomingtrans where intransid > 11759047 and statusid in (2,7)

select count(*) from incomingtrans where statusid in (2,7)

--delete from incomingtrans where intransid >= 11759047
select count(*) from incomingtrans where intransid >= 11759047
select count(*) from incomingtrans where dateprocessed >= '12/23/005' and dateprocessed < '12/24/005'

select * from incomingtrans where intransid >= 11759047 and priid = 831 and batchid = 94548
order by batchid

select * from incomingtrans where intransid >= 11759047 and priid = 831 and Description= '8008207479' and descdate='123005' and companyname='payment fee'
order by batchid

Select Isnull(NCT,''),* From [NB-Red].Central.dbo.Merchants Where PriID = 4225
Select Source From IncomingTrans With (NoLock) Where StatusID in (2, 11, 101, 102, 202, 302, 352) And PriID = 4225 Group By Source Having Source In ('VO','GO','VA','GA','VR','GR')

Select Distinct PriID From IncomingTrans With (NoLock) Where StatusID In (8, 11, 15) And PriID In (Select PriID From AchMerchAddl Where BankID = 11 And HoldPeriod <> 0) And (BatchID Is Null Or BatchID = 0) And PriID Not In (4438, 4692)

--Exec Ach_PostBatch_Ncal 11, 4225, 'TC'  		
--update incomingtrans set statusid = 11 where intransid in (11535350,11535601,11536484)

--update incomingtrans set dateprocessed = '2005-12-27 09:39:00' where intransid >= 11759047 and priid = 6099

select active,bankid,HoldPeriod,* from achmerchaddl where priid in (4225)

select count(*) from incomingtrans where intransid >= 11759047 and batchid <> 0
select distinct statusid from incomingtrans where intransid >= 11759047
select count(*) from incomingtrans a inner join achmerchaddl b on a.priid = b.priid where bankid = 12 and statusid = 2
Ach_Batch_Get_Data 11

declare @Source nvarchar(2)
exec Ach_Batch_Apply_Rules  @Source output,11,268
select @Source
Ach_Journal_Get_Data 12
Ach_Batch_Ncal 11,3347, 'TC'
-----------------------------------------------------------------------------------------------------------
--BankID
exec Ach_Batch_Get_Data 10
exec Ach_Batch_NCT 10, 5358, 'VA'
go

select batchid,priid,debit,credit,debitcount,creditcount,Transbase from batch
where posteddate >= '1/4/2006' and posteddate < '1/5/2006'
order by priid,debit

select batchid,priid,debit,credit,debitcount,creditcount,Transbase from batch
where batchid between 94028 and 94035 order by priid,debit

select min(a.batchid),max(a.batchid) from batch a inner join batchdetail b on a.batchid = b.batchid
where bankid = 12 and (posteddate >= '1/3/2006' and posteddate < '1/4/2006')

Select * From IncomingTrans A With (NoLock), AchMerchAddl B With (NoLock) Where A.PriID = B.PriID And A.StatusID In (2, 8, 11, 40, 41, 42) And B.PriID = 296 And (A.DateProcessed Is Null) And A.NextProcessDate >= '03/26/04 00:00:00' And (A.BatchID Is Null Or A.BatchID = 0) And B.BankID = 12
Select * From IncomingTrans A With (NoLock), AchMerchAddl B With (NoLock) Where A.PriID = B.PriID And A.StatusID In (11) And B.PriID = 296 and intransid >= 11759047

select min(batchid),max(batchid) from batch where posteddate >= '2/7/2006'
--update incomingtrans set batchid = 0, statusid = 2,Dateprocessed = null  where priid = 831 and intransid between 10745554 and 10792067
update incomingtrans set statusid = 2,dateprocessed = null,batchid=0 where intransid >= 11759047 and statusid in (10,18,126,17,4,7)
update incomingtrans set batchid = 0,dateprocessed = '2/7/2006' where intransid >= 11759047 and statusid in (122,11,8)

delete from batch where batchID > 98227
delete from batchdetail where batchID > 98227

update incomingtrans set statusid = 2,dateprocessed = null,batchid=0 from incomingtrans a inner join achmerchaddl b on a.priid = b.priid 
where a.statusid = 7 and bankid in (10,14)

select batchid,priid,debit,credit,debitcount,creditcount,Transbase from batch
where batchID > 98227
order by priid,debit

select count(*) from incomingtrans where dateprocessed >= '2/6/2006' and dateprocessed < '2/7/2006'
select min(intransid) from incomingtrans where dateprocessed >= '2/6/2006' and dateprocessed < '2/7/2006'
select * from achmerchaddl where priid = 3088
select * from [nb-red].central.dbo.merchants where priid = 3088

select distinct priid from incomingtrans a where intransid >= 11759047 and
not exists(select 'x' from achmerchaddl b where a.priid = b.priid)

select * from achmerchaddl where priid in (6377,6385,6436,6512)

select * from incomingtrans a where intransid < 11759047 and statusid = 2

30578

11759047




SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

ALTER  PROCEDURE dbo.Ach_Report_UploadFile AS


Select FileName as 'File Name',
	FileDebitAmount as 'File Debit Amount',
	FileCreditAmount as 'File Credit Amount',
	SQLDebitAmount as 'SQL Debit Amount',
	SQLCreditAmount as 'SQL Credit Amount',
	TransCount as 'Transaction Count',
	Equal 
From Stg_Summary 
Where batchid = 0 
Order By FileName,batchid
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


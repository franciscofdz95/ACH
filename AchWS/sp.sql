if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Ach_Insert_Transaction_WS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Ach_Insert_Transaction_WS]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Ach_Select_Merchant_WS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Ach_Select_Merchant_WS]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE dbo.Ach_Insert_Transaction_WS
	@TransID Int Output,
	@TransType Varchar(2),
	@TransRoute Varchar(9),
	@AccountNo Varchar(18),
	@NameOnAccount Varchar(23),
	@RefID Varchar(10),
	@Amount Money,
	@StatusID Int,
	@MerchantID Int,
	@Address1 Varchar(25),
	@Address2 Varchar(25),
	@City Varchar(25),
	@State char(2),
	@Zip Varchar(10),
	@Phone Varchar(10),
	@DLState Varchar(2),
	@DLNumber Varchar(25),
	@NextProcDate Smalldatetime,
	@TransDate Smalldatetime,
	@Source Varchar(2),
	@Secc Varchar(3),
	@Description Varchar(10),
	@CheckNumber Varchar(25)
AS

Declare @CompanyName Varchar(16), @AchID Int,@OriginID Int

Set @OriginID = 16 --Payment Gateway

If @Description = ''
	Select @CompanyName = AchCoName, @Description = AchDescrp, @AchID = AchID
	From Ach_Merchants
	Where MerchantID = @MerchantID
Else
	Select @CompanyName = AchCoName, @AchID = AchID
	From Ach_Merchants
	Where MerchantID = @MerchantID

Insert Into Ach_Transactions (OriginID, AchID, TransType, TransRoute, AccountNo, NameOnAccount, Description, DescDate, Secc, RefID, StatusID, Amount, MerchantID, Source, CompanyName, NextProcessDate, TransDate,ResubmitCount, CheckNumber)
Values (@OriginID, @AchID, @TransType, @TransRoute, @AccountNo, @NameOnAccount, @Description, REPLACE(CONVERT(varchar,getdate(),1),'/',''),@Secc, @RefID, @StatusID, @Amount, @MerchantID, @Source, @CompanyName, @NextProcDate,@TransDate, 0,@CheckNumber)

Select @TransID = @@Identity

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE dbo.Ach_Select_Merchant_WS
	@MerchantID Int,
	@Password Varchar(50)
AS

Select * From Ach_Master_Merchants where MerchantID = @MerchantID And Password = dbo.fn_Encrypt(@Password)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


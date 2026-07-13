SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

ALTER   Procedure dbo.Ach_GetProcessDate 
	
AS

Declare @EFTDate SmallDateTime 
Declare @CurrentDate SmallDateTime
Declare @j Int

Select @CurrentDate = Convert(Varchar,GetDate(),101)

Select @EFTDate = DateAdd(Day,1,@CurrentDate)
Select @j = 0

While (@j = 0)
Begin
	If (DatePart(DW,@EFTDate) = 1 Or DatePart(DW,@EFTDate) = 7)
	Begin
	  	Select @EFTDate = @EFTDate + 1
	 	Select @j = 0
	End
	Else
	Begin
	  	If (Exists(Select Holiday From Holidays Where Convert(Varchar,Holiday,101) = Convert(Varchar,@EFTDate,101)))
	  	Begin
     		Select @EFTDate = @EFTDate + 1
     		Select @j = 0
	  	End
	 Else
	 Begin
	     Select @j = 1
	  End
	End
End

Select @EFTDate
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


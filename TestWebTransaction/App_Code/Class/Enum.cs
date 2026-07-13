using System;
using System.Collections.Generic;
using System.Text;


public enum BankAccountType
{
    None = 0
    ,Checking = 1
    ,Saving = 2
}

public enum TransactionType
{
    None = 0
    ,Refund = 1
    ,Sale = 2
}

public enum CheckType
{
    None = 0,
    Personal = 1,       //PPD
    Business = 2,       //CCD
    //Web,            //WEB
    //Telephone,      //TEL
    //RePresentment   //RCK
}

//Recurring Constants
public enum OccurenceOptions
{
    Daily = 1,
    Monthly = 2
}
//----------------------------------------------------------------------------------------------------------------
//Applies to OccurenceOptions #2
public enum MontlyOptions
{
    None = 0,
    Day = 1, //day of the month
    The = 2  // ex. the 1st Monday of every month
}
//----------------------------------------------------------------------------------------------------------------
//Applies to OccurenceOptions #2, MontlyOptions #2
public enum WeekOptions
{
    None = 0,
    First = 1,
    Second = 2,
    Third = 3,
    Fourth = 4,
    Last = 5
}
//----------------------------------------------------------------------------------------------------------------
//Applies to OccurenceOptions #2, MontlyOptions #2
public enum WeekdayOptions
{
    None = 0,
    Monday = 1,
    Tuesday = 2,
    Wednesday = 3,
    Thursday = 4,
    Friday = 5,
    Saturday = 6,
    Sunday = 7
}
//----------------------------------------------------------------------------------------------------------------
//Applies to OccurenceOptions #2, MontlyOptions # 1 and 2
public enum MonthOfYearOptions
{
    None = 0,
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Eleven = 11,
    Twelve = 12
}
//----------------------------------------------------------------------------------------------------------------
//Applies to OccurenceOptions #2, MontlyOptions #1
public enum DayOfMonthOptions
{
    None = 0,
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Eleven = 11,
    Twelve = 12,
    Thirteen = 13,
    Fourteen = 14,
    Fifteen = 15,
    Sixteen = 16,
    Seventeen = 17,
    Eighteen = 18,
    Nineteen = 19,
    Twenty = 20,
    TwentyOne = 21,
    TwentyTwo = 22,
    TwentyThree = 23,
    TwentyFour = 24,
    TwentyFive = 25,
    TwentySix = 26,
    TwentySeven = 27,
    TwentyEight = 28,
    TwentyNine = 29,
    Thirty = 30,
    ThirtyOne = 31
}





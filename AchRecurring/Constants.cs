//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace AchRecurring
{
    //----------------------------------------------------------------------------------------------------------------
    public class Constants
    {
        public const string REGEX_EMAIL_ADDRESS = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
        //public const string REGEX_PHONE_NUMBER = @"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}";
        public const string REGEX_PHONE_NUMBER = @"((\(\d{3}\) ?)|(\d{3}))?\d{3}\d{4}";
        public const string REGEX_ZIP_CODE = @"\d{5}(-\d{4})?";

        public const string SP_GET_NEXT_RECURRING_ID = "nmc_getIdReference";
        public const string TBL_RECURRING_PAYMENTS_NAME = "RECURRINGPAYMENTS";

        public const int DEFAULT_PROVIDER_ID = 3;
        public const int DEFAULT_RECURRING_ID = -1;
        public const string DEFFAULT_CREATE_DATETIME = "01-01-1753";
        public const string DEFUALT_MASKED_CREDIT_CARD = "************XXXX";

        public const int OCCUR_PAYMENT_TYPE_IMMEDIATELY = 0;
        public const int OCCUR_PAYMENT_TYPE_DAILY = 1;
        public const int OCCUR_PAYMENT_TYPE_MONTHLY = 2;

        public const int ACC_VISIBLE_LEN = 4;
        public const int MONTHLYOPTION1 = 1;	//Day of the month
        public const int IMMEDIATELY = 0;
        public const int DAILY = 1;
        public const int MONTHLY = 2;
        public const int MAXWEEKDAY = 7;
        public const int FIRSTDAY = 1;
        public const int MAXFEBDAY = 28;
        public const int MAXMONTHDAYS = 30;
        public const int LASTWEEK = 5;          //assumption

        public const int UPDATESCHEDULE = 1;
        //public static TimeSpan SCHEDULERUNTIME = new TimeSpan(11, 30, 00);
        public static TimeSpan SCHEDULERUNTIME = new TimeSpan(18, 00, 00);

        public const string DEFAULT_VERSION_NO = "00";

        public const int TRANS_RESULT_SUCCESS = 300;


        public const string ERROR_MESSAGE_RECURR_ID = "There was an error in processing your request. System Error.";
        public const string ERROR_SCHEDULING_1 = "End Date should be greater then Schedule Date.";

    } // end class Constants
    //----------------------------------------------------------------------------------------------------------------
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //----------------------------------------------------------------------------------------------------------------
    #region " Scheduling Enumerations "
    public enum OccurenceOptions
    {
        Immediately = 0,
        Daily = 1,
        Monthly = 2
    }
    //----------------------------------------------------------------------------------------------------------------
    public enum MontlyOptions
    {
        None = 0,
        Day = 1,
        The = 2
    }
    //----------------------------------------------------------------------------------------------------------------
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
    //    public enum MonthOfYear
    //    {
    //        _None = 0,
    //        Jan = 1,
    //        Feb = 2,
    //        Mar = 3,
    //        Apr = 4,
    //        May = 5,
    //        Jun = 6,
    //        Jul = 7,
    //        Aug = 8,
    //        Sep = 9,
    //        Oct = 10,
    //        Nov = 11,
    //        Dec = 12
    //    }
    ////----------------------------------------------------------------------------------------------------------------
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
    #endregion
    //----------------------------------------------------------------------------------------------------------------
    #region " Transaction Enumerations "

    public enum ResultCodes
    {
        Approved = 300,
        //Success = 300,
        SystemError = 305,
        InvalidTerminalID = 306,
        InvalidRequest = 316,
        TransCannotBeCompleted = 371,
        InvalidInputData = 386,
        InProcess = 399
    }

    public enum RecurringTypes
    {
        Immediate = 0,
        Scheduled = 1
    }

    public enum TransactionTypes
    {
        Sale = 312,
        Void = 313,
        Refund = 314,
        Authorization = 310,
        Settle = 324

    }

    public enum EntryModes
    {
        Swipe = 300,
        Manual = 301,
        Voice = 302
    }

    public enum CardTypes
    {
        Visa = 4,
        Mastercard = 5,
        AMEX = 3,
        Other = 0
    }

    public enum MaskType
    {
        LastFour = 1,
        FirstLastFour = 2
    }

    #endregion
    //----------------------------------------------------------------------------------------------------------------
} // end namespace nmc.recurring
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
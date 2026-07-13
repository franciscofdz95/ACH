using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class Ach : System.Web.Services.WebService
{
    public Ach () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod(Description = "ACH Credit transaction requires the following fields:<br>" +
        "<ul>" +
        "<li><b>MerchantID</b>: provided by NMC</li>" +
        "<li><b>MerchantKey</b>: provided by NMC</li>" +
        "<li><b>RefID</b>: user’s unique Ref. ID (Invoice #, Order # or Acct. #)</li>" +
        "<li><b>Amount</b>: payment amount including cents. example: 19.45 or 20.00</li>" +
        "<li><b>RoutingNumber</b>: your client or customer's bank routing number</li>" +
        "<li><b>AccountNumber</b>: your client or customer's Bank Account Number without space or hyphen </li>" +
        "<li><b>BankAccountType</b>: 1=Checking, 2=Savings</li>" +
        "<li><b>AccountName</b>: your client or customer's name on account</li>" +
        "<li><b>ProcessDate</b>: format is mmddyyyy</li>" +
        "<li><b>ACHCheckType</b>: 1=Personal, 2=Business</li>" +
        "<li><b>Description</b>: purpose of transactions </li>" +
        "<li><b>CheckNumber</b>: optional</li>" +
        "</ul>" 
     )]
    public CheckStatus ACHCredit(
        string MerchantID, string MerchantKey,
        string RefID, Decimal Amount,
        string RoutingNumber,
        string AccountNumber, AccountType BankAccountType,
        string AccountName, string ProcessDate,
        CheckType ACHCheckType, string Description, string CheckNumber
        )
    {
        Logger.LogDebug(DateTime.Now + " Begin ACHCredit: " + MerchantID);
        Check check =
            new Check(MerchantID, MerchantKey,
            RefID, Amount, RoutingNumber,
            AccountNumber, AccountName, BankAccountType, TransactionType.Credit,
            ProcessDate, ACHCheckType, Description, CheckNumber);
        try
        {
            check.Log();
            check.Process();
            check.Status.Log();
        }
        catch (Exception e)
        {
            Logger.Log(e);
        }
        Logger.LogDebug(DateTime.Now + " End ACHCredit: " + MerchantID);
        return check.BuildCheckResponse();
    }

    [WebMethod(Description = "ACH Credit Recurring transaction requires the following fields:<br>" +
        "<ul>" +
        "<li><b>MerchantID</b>: provided by NMC</li>" +
        "<li><b>MerchantKey</b>: provided by NMC</li>" +
        "<li><b>RefID</b>: user’s unique Ref. ID (Invoice #, Order # or Acct. #)</li>" +
        "<li><b>Amount</b>: payment amount including cents. example: 19.45 or 20.00</li>" +
        "<li><b>RoutingNumber</b>: your client or customer's bank routing number</li>" +
        "<li><b>AccountNumber</b>: your client or customer's Bank Account Number without space or hyphen </li>" +
        "<li><b>BankAccountType</b>: 1=Checking, 2=Savings</li>" +
        "<li><b>AccountName</b>: your client or customer's name on account</li>" +
        "<li><b>ACHCheckType</b>: 1=Personal, 2=Business</li>" +
        "<li><b>Description</b>: purpose of transactions </li>" +
        "<li><b>StartDate</b>: start date with format (mmddyyyy)</li>" +
        "<li><b>EndDate</b>: date or enter date with format (mmddyyyy)</li>" +
        "<li><b>OccurenceOption</b>: 1=Daily, 2=Monthly</li>" +
        "<li><b>MontlyOption</b>: 0=None, 1=Day of month option, 2=The (only applicable to OccurenceOption=1)</li>" +
        "<li><b>WeekOption</b>: 0=None, 1=First, 2=Second, 3=Third, 4=Fourth, 5=Last (only applicable to OccurenceOption=1 And MontlyOption=2)</li>" +
        "<li><b>WeekdayOption</b>: 0=None, 1=Monday, 2=Tuesday, 3=Wednesday, 4=Thursday, 5=Friday, 6=Saturday, 7=Sunday (only applicable to OccurenceOption=1 And MontlyOption=2)</li>" +
        "<li><b>MonthOfYearOption</b>: 0=None, 1=One, 2=Two, 3=Three, 4=Four, 5=Five, 6=Six, 7=Seven, 8=Eight, 9=Nine, 10=Ten, 11=Eleven, 12=Twelve (only applicable to OccurenceOption=1)</li>" +
        "<li><b>DayOfMonthOption</b>: 0=None, 1, 2, 3, 4, 5, 6,7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31</li>" +
        "</ul>"
)]
    public CheckStatus ACHCreditRecurring(
        string MerchantID, string MerchantKey,
        string RefID, Decimal Amount, string RoutingNumber, string AccountNumber, 
        AccountType BankAccountType, string AccountName, CheckType ACHCheckType, string Description,
        string StartDate, string EndDate, OccurenceOptions OccurenceOption,
        MontlyOptions MontlyOption, WeekOptions WeekOption, WeekdayOptions WeekdayOption,
        MonthOfYearOptions MonthOfYearOption, DayOfMonthOptions DayOfMonthOption
        )
    {
        Logger.LogDebug(DateTime.Now + " Begin ACHCreditRecurring: " + MerchantID);
        RecurringCheck check =
            new RecurringCheck(MerchantID, MerchantKey,
            RefID, Amount, RoutingNumber,
            AccountNumber, AccountName, BankAccountType, TransactionType.Credit, StartDate, EndDate, ACHCheckType, Description, 
            OccurenceOption, MontlyOption, WeekOption, WeekdayOption,
            MonthOfYearOption, DayOfMonthOption);
        try
        {
            check.Log();
            check.Process();
            check.Status.Log();
        }
        catch (Exception e)
        {
            Logger.Log(e);
        }
        Logger.LogDebug(DateTime.Now + " End ACHCreditRecurring: " + MerchantID);
        return check.BuildCheckResponse();
    }


    [WebMethod(Description = "ACH Debit transaction requires the following fields:<br>" +
       "<ul>" +
       "<li><b>MerchantID</b>: provided by NMC</li>" +
       "<li><b>MerchantKey</b>: provided by NMC</li>" +
       "<li><b>RefID</b>: user’s unique Ref. ID (Invoice #, Order # or Acct. #)</li>" +
       "<li><b>Amount</b>: payment amount including cents. example: 19.45 or 20.00</li>" +
       "<li><b>RoutingNumber</b>: your client or customer's bank routing number</li>" +
       "<li><b>AccountNumber</b>: your client or customer's Bank Account Number without space or hyphen </li>" +
       "<li><b>BankAccountType</b>: 1=Checking, 2=Savings</li>" +
       "<li><b>AccountName</b>: your client or customer's name on account</li>" +
       "<li><b>ProcessDate</b>: format is mmddyyyy</li>" +
       "<li><b>ACHCheckType</b>: 1=Personal, 2=Business</li>" +
       "<li><b>Description</b>: purpose of transactions </li>" +
       "<li><b>CheckNumber</b>: optional</li>" +
       "</ul>"
       )]
    public CheckStatus ACHDebit(
        string MerchantID, string MerchantKey,
        string RefID, Decimal Amount,
        string RoutingNumber,
        string AccountNumber, AccountType BankAccountType,
        string AccountName, string ProcessDate,
        CheckType ACHCheckType, string Description, string CheckNumber
        )
    {
        Logger.LogDebug(DateTime.Now + " Begin ACHDebit: " + MerchantID);
        Check check =
            new Check(MerchantID, MerchantKey,
            RefID, Amount, RoutingNumber,
            AccountNumber, AccountName, BankAccountType, TransactionType.Debit,
            ProcessDate, ACHCheckType, Description, CheckNumber);
        try
        {
            check.Log();
            check.Process();
            check.Status.Log();
        }
        catch (Exception e)
        {
            Logger.Log(e);
        }
        Logger.LogDebug(DateTime.Now + " End ACHDebit: " + MerchantID);
        return check.BuildCheckResponse();
    }


    [WebMethod(Description = "ACH Debit Recurring transaction requires the following fields:<br>" +
        "<ul>" +
        "<li><b>MerchantID</b>: provided by NMC</li>" +
        "<li><b>MerchantKey</b>: provided by NMC</li>" +
        "<li><b>RefID</b>: user’s unique Ref. ID (Invoice #, Order # or Acct. #)</li>" +
        "<li><b>Amount</b>: payment amount including cents. example: 19.45 or 20.00</li>" +
        "<li><b>RoutingNumber</b>: your client or customer's bank routing number</li>" +
        "<li><b>AccountNumber</b>: your client or customer's Bank Account Number without space or hyphen </li>" +
        "<li><b>BankAccountType</b>: 1=Checking, 2=Savings</li>" +
        "<li><b>AccountName</b>: your client or customer's name on account</li>" +
        "<li><b>ACHCheckType</b>: 1=Personal, 2=Business</li>" +
        "<li><b>Description</b>: purpose of transactions </li>" +
        "<li><b>StartDate</b>: start date with format (mmddyyyy)</li>" +
        "<li><b>EndDate</b>: date or enter date with format (mmddyyyy)</li>" +
        "<li><b>OccurenceOption</b>: 1=Daily, 2=Monthly</li>" +
        "<li><b>MontlyOption</b>: 0=None, 1=Day of month option, 2=The (only applicable to OccurenceOption=1)</li>" +
        "<li><b>WeekOption</b>: 0=None, 1=First, 2=Second, 3=Third, 4=Fourth, 5=Last (only applicable to OccurenceOption=1 And MontlyOption=2)</li>" +
        "<li><b>WeekdayOption</b>: 0=None, 1=Monday, 2=Tuesday, 3=Wednesday, 4=Thursday, 5=Friday, 6=Saturday, 7=Sunday (only applicable to OccurenceOption=1 And MontlyOption=2)</li>" +
        "<li><b>MonthOfYearOption</b>: 0=None, 1=One, 2=Two, 3=Three, 4=Four, 5=Five, 6=Six, 7=Seven, 8=Eight, 9=Nine, 10=Ten, 11=Eleven, 12=Twelve (only applicable to OccurenceOption=1)</li>" +
       "<li><b>DayOfMonthOption</b>: 0=None, 1, 2, 3, 4, 5, 6,7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31</li>" +
        "</ul>"
)]
    public CheckStatus ACHDebitRecurring(
        string MerchantID, string MerchantKey,
        string RefID, Decimal Amount, string RoutingNumber, string AccountNumber,
        AccountType BankAccountType, string AccountName, CheckType ACHCheckType, string Description,
        string StartDate, string EndDate, OccurenceOptions OccurenceOption,
        MontlyOptions MontlyOption, WeekOptions WeekOption, WeekdayOptions WeekdayOption,
        MonthOfYearOptions MonthOfYearOption, DayOfMonthOptions DayOfMonthOption
        )
    {
        Logger.LogDebug(DateTime.Now + " Begin ACHDebitRecurring: " + MerchantID);
        RecurringCheck check =
            new RecurringCheck(MerchantID, MerchantKey,
            RefID, Amount, RoutingNumber,
            AccountNumber, AccountName, BankAccountType, TransactionType.Debit, StartDate, EndDate, ACHCheckType, Description,
            OccurenceOption, MontlyOption, WeekOption, WeekdayOption,
            MonthOfYearOption, DayOfMonthOption);
        try
        {
            check.Log();
            check.Process();
            check.Status.Log();
        }
        catch (Exception e)
        {
            Logger.Log(e);
        }
        Logger.LogDebug(DateTime.Now + " End ACHDebitRecurring: " + MerchantID);
        return check.BuildCheckResponse();
    }



}

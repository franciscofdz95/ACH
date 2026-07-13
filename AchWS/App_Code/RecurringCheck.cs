using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections;

/// <summary>
/// Summary description for RecurringCheck
/// </summary>
public class RecurringCheck: Check
{

    //Schedule Ach recurring options
    private string m_StartDate;
    private string m_EndDate;
    private bool m_IsEndDate;
    private string m_ScheduleDescription;
    private OccurenceOptions m_OccurenceOption;
    private MontlyOptions m_MontlyOption;
    private WeekOptions m_WeekOption;
    private WeekdayOptions m_WeekdayOption;
    private DayOfMonthOptions m_DayOfMonthOption;
    private MonthOfYearOptions m_MonthOfYearOption;

    public RecurringCheck(string MerchantID, string MerchantKey, string RefID, 
        Decimal Amount, string RoutingNumber, string AccountNumber, string AccountName,
        AccountType BankAccountType, TransactionType TransType, string StartDate, string EndDate,
        CheckType checkType, string Description, OccurenceOptions OccurenceOption,
        MontlyOptions MontlyOption, WeekOptions WeekOption, WeekdayOptions WeekdayOption,
        MonthOfYearOptions MonthOfYearOption, DayOfMonthOptions DayOfMonthOption):base(MerchantID, MerchantKey, RefID, 
                                                                                        Amount, RoutingNumber, AccountNumber,
                                                                                        AccountName, BankAccountType, TransType, string.Empty,
                                                                                        checkType, Description, string.Empty)
    {
        m_StartDate = this.ParseDate(StartDate);
        m_EndDate = this.ParseDate(EndDate);

        if (EndDate == string.Empty)
            m_IsEndDate = false;
   
        m_OccurenceOption = OccurenceOption;
        m_MontlyOption = MontlyOption;
        m_WeekOption = WeekOption;
        m_WeekdayOption = WeekdayOption;
        m_DayOfMonthOption = DayOfMonthOption;
        m_MonthOfYearOption = MonthOfYearOption;

        if (m_OccurenceOption == OccurenceOptions.Daily)
        {
            m_ScheduleDescription = "Every " + Convert.ToInt16(DayOfMonthOption).ToString() + " day(s). ";
        }
        else
        {
            if (MontlyOption == MontlyOptions.Day)
                m_ScheduleDescription = "Day " + Convert.ToInt16(DayOfMonthOption).ToString() + " of every " + Convert.ToInt16(MonthOfYearOption).ToString() + " month(s). ";
            else
                m_ScheduleDescription = "The " + ConvertWeekOption(WeekOption).ToString() + " " + WeekdayOption + " of every " + Convert.ToInt16(MonthOfYearOption).ToString() + " month(s). ";
        }

        Status.ScheduleDescription = m_ScheduleDescription;
    }

    public string ConvertWeekOption(WeekOptions WeekOption)
    {
        string option = string.Empty;

        switch (WeekOption)
        { 
            case WeekOptions.First:
                option = "1st";
                break;
            case WeekOptions.Second:
                option = "2nd";
                break;
            case WeekOptions.Third:
                option = "3rd";
                break;
            case WeekOptions.Fourth:
                option = "4th";
                break;
            case WeekOptions.Last:
                option = "last";
                break;
        }

        return option;
    }


    public override bool Validate()
    {
        bool perform = false;
        perform = this.ValidateRequiredFields();

        if (perform)
            perform = this.ValidateRecurringFields();

        if (perform)
            perform = this.ValidateMerchant();

        return perform;

    }

    protected bool ValidateRecurringFields()
    {
        string strError = string.Empty;

        try
        {
            if (!this.IsDate(this.StartDate))
                strError += "Invalid Start Date. ";

            if (this.EndDate != string.Empty)
                if (!this.IsDate(this.EndDate))
                    strError += "Invalid End Date. ";

            if (this.OccurenceOption == OccurenceOptions.Daily)
            {
                if (this.DayOfMonthOption == DayOfMonthOptions.None)
                    strError += "DayOfMonthOption is required when OccurenceOptions is Daily(1). ";
            }

            if (this.OccurenceOption == OccurenceOptions.Monthly)
            {
                if (this.MontlyOption == MontlyOptions.None)
                    strError += "MontlyOption is required when OccurenceOptions is Monthly(2). ";
            }

            if (this.OccurenceOption == OccurenceOptions.Monthly)
            {
                if (this.MontlyOption == MontlyOptions.Day)
                {
                    if (this.DayOfMonthOption == DayOfMonthOptions.None)
                        strError += "DayOfMonthOption is required when OccurenceOptions is Monthly(2) and MontlyOption is Day(1). ";
                }
            }

            if (this.OccurenceOption == OccurenceOptions.Monthly)
            {
                if (this.MontlyOption == MontlyOptions.Day)
                {
                    if (this.MonthOfYearOption == MonthOfYearOptions.None)
                        strError += "MonthOfYearOption is required when OccurenceOptions is Monthly(2) and MontlyOption is Day(1). ";
                }
            }

            if (this.OccurenceOption == OccurenceOptions.Monthly)
            {
                if (this.MontlyOption == MontlyOptions.The)
                {
                    if (this.WeekOption == WeekOptions.None)
                        strError += "WeekOption is required when OccurenceOptions is Monthly(2) and MontlyOption is THE(2). ";
                }
            }

            if (this.OccurenceOption == OccurenceOptions.Monthly)
            {
                if (this.MontlyOption == MontlyOptions.The)
                {
                    if (this.WeekdayOption == WeekdayOptions.None)
                        strError += "WeekdayOption is required when OccurenceOptions is Monthly(2) and MontlyOption is THE(2). ";
                }
            }

            if (this.OccurenceOption == OccurenceOptions.Monthly)
            {
                if (this.MontlyOption == MontlyOptions.The)
                {
                    if (this.MonthOfYearOption == MonthOfYearOptions.None)
                        strError += "MonthOfYearOption is required when OccurenceOptions is Monthly(2) and MontlyOption is THE(2). ";
                }
            }


            if (strError != string.Empty)
            {
                Status.Success = 0;
                Status.Message = strError;
                return false;
            }
            else
                return true;
        }
        catch (Exception e)
        {
            Logger.Log(e);
            return false;
        }
    }

    public override void SaveTransaction()
    {
        try
        {
            ArrayList prms = new ArrayList();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Insert_Recurring_Transaction_VT";
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prm = new SqlParameter("@Recur_ID", -1);
            prm.Direction = ParameterDirection.Output;
            prms.Add(prm);
            prms.Add(new SqlParameter("@MerchantID", this.MerchantID));
            prms.Add(new SqlParameter("@Occur_ID", this.OccurenceOption));

            if (this.MontlyOption == MontlyOptions.None)
                prms.Add(new SqlParameter("@MonthlyOption", DBNull.Value));
            else
                prms.Add(new SqlParameter("@MonthlyOption", this.MontlyOption));

            if (this.DayOfMonthOption == DayOfMonthOptions.None)
                prms.Add(new SqlParameter("@DayNum", DBNull.Value));
            else
                prms.Add(new SqlParameter("@DayNum", this.DayOfMonthOption));

            if (this.MonthOfYearOption == MonthOfYearOptions.None)
                prms.Add(new SqlParameter("@MonthNum", DBNull.Value));
            else
                prms.Add(new SqlParameter("@MonthNum", this.MonthOfYearOption));

            if (this.WeekdayOption == WeekdayOptions.None)
                prms.Add(new SqlParameter("@WeekDay", DBNull.Value));
            else
                prms.Add(new SqlParameter("@WeekDay", this.WeekdayOption));

            if (this.WeekOption == WeekOptions.None)
                prms.Add(new SqlParameter("@WeekNum", DBNull.Value));
            else
                prms.Add(new SqlParameter("@WeekNum", this.WeekOption));

            prms.Add(new SqlParameter("@StartDate", this.StartDate));
            
            if (this.EndDate != string.Empty)
                prms.Add(new SqlParameter("@Enddate", this.EndDate));
            
            prms.Add(new SqlParameter("@IsEndDate", this.IsEndDate));
            prms.Add(new SqlParameter("@Schedule_Date", this.StartDate));
            prms.Add(new SqlParameter("@IsEnabled", true));
            prms.Add(new SqlParameter("@Description", this.ScheduleDescription));
            prms.Add(new SqlParameter("@Ach_Description", this.Description)); 
            prms.Add(new SqlParameter("@TransRoute", this.RoutingNumber));
            prms.Add(new SqlParameter("@AccountNo", this.AccountNumber));
            prms.Add(new SqlParameter("@TransType", this.TransCode));
            prms.Add(new SqlParameter("@NameonAccount", this.AccountName));
            prms.Add(new SqlParameter("@RefID", this.RefID));
            prms.Add(new SqlParameter("@Amount", this.Amount));
            DataLayer.AppendParamters(cmd, prms);

            int rows = DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());

            if (rows > 0)
            {
                Status.Success = 1;
                Status.RecurID = Convert.ToString(cmd.Parameters["@Recur_ID"].Value);
            }
            else
                this.Status.RecurID = "-1";

        }
        catch (SqlException e)
        {
            Logger.Log(e);
            Status.Success = 3;
            Status.Message = "CALL NMC";
            throw e;
        }
    }

    
    public string StartDate
    {
        get { return m_StartDate; }
        set { m_StartDate = value; }
    }

    public string EndDate
    {
        get { return m_EndDate; }
        set { m_EndDate = value; }
    }

    public bool IsEndDate
    {
        get { return m_IsEndDate; }
    }

    public string ScheduleDescription
    {
        get { return m_ScheduleDescription; }
    }
    
    public OccurenceOptions OccurenceOption
    {
        get { return m_OccurenceOption; }
        set { m_OccurenceOption = value; }
    }

    public MontlyOptions MontlyOption
    {
        get { return m_MontlyOption; }
        set { m_MontlyOption = value; }
    }
    public WeekOptions WeekOption
    {
        get { return m_WeekOption; }
        set { m_WeekOption = value; }
    }

    public WeekdayOptions WeekdayOption
    {
        get { return m_WeekdayOption; }
        set { m_WeekdayOption = value; }
    }
    public DayOfMonthOptions DayOfMonthOption
    {
        get { return m_DayOfMonthOption; }
        set { m_DayOfMonthOption = value; }
    }

    public MonthOfYearOptions MonthOfYearOption
    {
        get { return m_MonthOfYearOption; }
        set { m_MonthOfYearOption = value; }
    }
  

}

using System;
using System.IO;
using System.Net;
using System.Web;
using System.Data;
using System.Data.SqlTypes;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;


public class Check
{
    private string m_MerchantID;
    private string m_MerchantKey;

    private string m_AccountNumber;
    private string m_RoutingNumber;
    private string m_AccountName;
    private string m_TransCode;
    private TransactionType m_TransType;
    private decimal m_Amount;
    private string m_RefID;
    private AccountType m_AccountType;
    private string m_NextProcDate = "";
    private string m_Secc = "";
    private string m_Source = "";
    private CheckStatus m_Status = new CheckStatus();
    private string m_Description = string.Empty;
    private string m_CheckNumber;


    public Check(string MerchantID, string MerchantKey, string RefID, 
        Decimal Amount, string RoutingNumber, string AccountNumber, string AccountName,
        AccountType BankAccountType, TransactionType TransType, string NextProcessDate,
        CheckType checkType, string Description, string CheckNumber)
    {
        this.MerchantID = MerchantID;
        this.MerchantKey = MerchantKey;
        this.RefID = RefID;
        this.Amount = Amount;
        this.Status.RefID = RefID;
        this.Status.Amount = Amount;
        this.RoutingNumber = RoutingNumber;
        this.AccountNumber = AccountNumber;
        this.AccountName = AccountName;
        this.AccountType = BankAccountType;
        this.Description = Description;
        this.CheckNumber = CheckNumber;
        this.NextProcDate = this.ParseDate(NextProcessDate);

        switch (checkType)
        {
            case CheckType.Business:
                this.Secc = "CCD";
                break;
            case CheckType.Personal:
                this.Secc = "PPD";
                break;
            default:
                this.Secc = "PPD";
                break;
        }


        if (BankAccountType == AccountType.None && TransType == TransactionType.None)
            this.TransCode = "27";
        else
        {
            this.TransCode = "";
            if (BankAccountType == AccountType.Checking)
                this.TransCode += "2";
            else
                this.TransCode += "3";

            if (TransType == TransactionType.Credit)
                this.TransCode += "2";
            else
                this.TransCode += "7";
        }
    }

    public bool IsDate(string date)
    {
        DateTime d;

        try
        {
            d = DateTime.Parse(date);
            return true;
        }
        catch (Exception e)
        {
            Logger.Log(e);
            return false;
        }

    }

    public string ParseDate(string date)
    {
        try
        {
            string month = date.Substring(0, 2);
            string day = date.Substring(2, 2);
            string year = date.Substring(4, 4);
            
            return month + "/" + day + "/" + year;
        }
        catch (Exception e)
        {
            Logger.Log(e);
            return date;
        }

    }

    public void Process()
    {
        try
        {
            if (this.Validate())
            {
                this.SaveTransaction();
            }
        }
        catch (Exception e)
        {
            Logger.Log(e);
            Status.Success = 3;
            Status.Message = "CALL NMC";
        }
    }

    public virtual bool Validate()
    {
        bool perform = false;
        perform = this.ValidateRequiredFields();

        if (perform)
            perform = this.ValidateMerchant();

        return perform;

    }

    protected bool ValidateRequiredFields()
    {
        string strError = string.Empty;

        try
        {
            if (m_MerchantID == string.Empty || m_MerchantID == null)
                strError += "Merchant ID is required. ";

            if (m_MerchantKey == string.Empty || m_MerchantKey == null)
                strError += "Merchant Key is required. ";

            if (m_RoutingNumber == string.Empty || m_RoutingNumber == null)
                strError += "Route Number is required. ";

            if (m_AccountNumber == string.Empty || m_AccountNumber == null)
                strError += "Account Number is required. ";

            if (m_Secc == "RCK" && (m_CheckNumber == string.Empty || m_CheckNumber == null))
                strError += "Check Number is required for a Re-Presentment transaction. ";

            if (this.NextProcDate != string.Empty)
                if (!this.IsDate(this.NextProcDate))
                    strError += "Invalid Process Date. ";

            if (m_AccountNumber != string.Empty)
                if (m_AccountNumber.Length > 17)
                    strError += "Account No is too long. ";

            if (m_AccountNumber != string.Empty)
                if (!DataLayer.IsNumeric(m_AccountNumber))
                    strError += "Account No is a numeric field. ";

            if (m_RoutingNumber != string.Empty)
                if (m_RoutingNumber.Length > 9)
                    strError += "RoutingNumber is too long. ";

            if (m_RoutingNumber != string.Empty)
                if (!DataLayer.IsNumeric(m_RoutingNumber))
                    strError += "Routing Number is a numeric field. ";

            if (m_RoutingNumber != string.Empty)
                if (!this.ValidateCheckDigit(m_RoutingNumber))
                    strError += "Routing Number Check Digit Error. ";

            if (m_CheckNumber != string.Empty)
                if (!DataLayer.IsNumeric(m_CheckNumber))
                    strError += "Check Number is a numeric field. ";

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

    protected bool ValidateCheckDigit(string RoutingNumber)
    {
        string Weight = "37137137";
        int intSum = 0;
        int intMod = 0;

        for (int i = 0; i < RoutingNumber.Length - 1; i++)
        {
            intSum += Convert.ToInt32(RoutingNumber.Substring(i, 1)) * Convert.ToInt32(Weight.Substring(i, 1));
        }

        intMod = intSum % 10;

        if (intMod != 0)
        {
            intMod = 10 - intMod;
        }

        if (RoutingNumber.Substring(8, 1) == Convert.ToString(intMod))
            return true;
        else
            return false;
    }

    protected bool ValidateMerchant()
    {
        try
        {
            ArrayList prms = new ArrayList();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_nmc_AuthenticateMerchantID";
            cmd.CommandType = CommandType.StoredProcedure;
            prms.Add(new SqlParameter("@MerchantID", this.MerchantID));
            prms.Add(new SqlParameter("@MerchantKey", this.MerchantKey));
            DataLayer.AppendParamters(cmd, prms);

            SqlDataReader dr = DataLayer.GetDataReader(cmd, DataLayer.ConnectStringMainBuild());
            if (dr.Read())
            {
                this.MerchantID = dr["lMerchant_ID"].ToString();
                return true;
            }
            else
            {
                Status.Success = 0;
                Status.Message = "Invalid merchant ID or merchant key";
                return false;
            }

        }
        catch (Exception e)
        {
            Logger.Log(e);
            return false;
        }
    }


    

    public CheckStatus BuildCheckResponse()
    {
        
        if (m_Status.Success == 0)
        {
            m_Status.Status = "DENIED";
            m_Status.PostedDate = DateTime.Now;
            return m_Status;
        }
        else if (m_Status.Success == 1)
        {
            m_Status.Status = "APPROVED";
            m_Status.PostedDate = DateTime.Now;
            return m_Status;
        }
        else if (m_Status.Success == 2)
        {
            m_Status.Status = "TIMEOUT";
            m_Status.PostedDate = DateTime.Now;
            return m_Status;
        }
        else if (m_Status.Success == 3)
        {
            m_Status.Status = "CANCEL";
            m_Status.PostedDate = DateTime.Now;
            return m_Status;
        }
        else
        {
            m_Status.Status = "TIMEOUT";
            m_Status.PostedDate = DateTime.Now;
            return m_Status;
        }
    }
    public void Log()
    {
        Logger.LogInfo("MerchantID = " + this.MerchantID);
        Logger.LogInfo("MerchantKey = " + this.MerchantKey);
        Logger.LogInfo("Amount = " + this.Amount);
        Logger.LogInfo("RefID = " + this.RefID);
        Logger.LogInfo("CheckNumber = " + this.CheckNumber);
        Logger.LogInfo("RoutingNumber = " + this.RoutingNumber);
        Logger.LogInfo("AccountNumber = " + this.AccountNumber);
        Logger.LogInfo("AccountName = " + this.AccountName);
    }


    public virtual void SaveTransaction()
    {
        try
        {
            ArrayList prms = new ArrayList();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Insert_Transaction_WS";
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prm = new SqlParameter("@TransID", -1);
            prm.Direction = ParameterDirection.Output;
            prms.Add(prm);
            prms.Add(new SqlParameter("@TransType", this.TransCode));
            prms.Add(new SqlParameter("@TransRoute", this.RoutingNumber));
            prms.Add(new SqlParameter("@AccountNo", this.AccountNumber));
            prms.Add(new SqlParameter("@NameOnAccount", this.AccountName));
            prms.Add(new SqlParameter("@RefID", this.RefID));
            prms.Add(new SqlParameter("@Amount", DataLayer.Decimal2Field(this.Amount)));
            prms.Add(new SqlParameter("@StatusID", DataLayer.Int2Field(0)));
            prms.Add(new SqlParameter("@MerchantID", DataLayer.Int2Field(this.MerchantID)));
            prms.Add(new SqlParameter("@NextProcDate", this.NextProcDate));
            prms.Add(new SqlParameter("@TransDate", DateTime.Now));
            prms.Add(new SqlParameter("@Source", "C"));
            prms.Add(new SqlParameter("@Secc", this.Secc));
            prms.Add(new SqlParameter("@Description", this.Description));
            prms.Add(new SqlParameter("@OriginID", 27)); //Webservice
            prms.Add(new SqlParameter("@CheckNumber", this.CheckNumber));
            DataLayer.AppendParamters(cmd, prms);

            int rows = DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());

            if (rows > 0)
            {
                Status.Success = 1;
                Status.TransID = Convert.ToString(cmd.Parameters["@TransID"].Value);
            }
            else
                this.Status.TransID = "-1";

        }
        catch (SqlException e)
        {
            Logger.Log(e);
            Status.Success = 3;
            Status.Message = "CALL NMC";
            throw e;
        }
    }




    public string MerchantID
    {
        get { return m_MerchantID; }
        set { m_MerchantID = value; }
    }
    public string MerchantKey
    {
        get { return m_MerchantKey; }
        set { m_MerchantKey = value; }
    }
    public string Source
    {
        get { return m_Source; }
        set { m_Source = value; }
    }
    public string Secc
    {
        get { return m_Secc; }
        set { m_Secc = value; }
    }
    public string Description
    {
        get { return m_Description; }
        set { m_Description = value; }
    }

    public string RoutingNumber
{
    get { return m_RoutingNumber; }
    set { m_RoutingNumber = value; }
}
    public decimal Amount
    {
        get { return m_Amount; }
        set { m_Amount = value; }
    }
    public string RefID
    {
        get { return m_RefID; }
        set { m_RefID = value; }
    }
    public string TransCode
    {
        get { return m_TransCode; }
        set { m_TransCode = value; }
    }

    public TransactionType TransType
    {
        get { return m_TransType; }
    }
    public AccountType AccountType
    {
        get { return m_AccountType; }
        set { m_AccountType = value; }
    }
    public string AccountNumber
    {
        get { return m_AccountNumber; }
        set { m_AccountNumber = value; }
    }

    public string AccountName
    {
        get { return m_AccountName; }
        set { m_AccountName = value; }
    }

    public string NextProcDate
    {
        get { return m_NextProcDate; }
        set { m_NextProcDate = value; }
    }

    public string CheckNumber
    {
        get { return m_CheckNumber; }
        set { m_CheckNumber = value; }
    }

    public CheckStatus Status
    {
        get { return m_Status; }
        set { m_Status = value; }
    }
}

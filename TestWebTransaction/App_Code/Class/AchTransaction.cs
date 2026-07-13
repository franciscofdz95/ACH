using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

public class AchTransaction: Transaction, iAch 
{
    private string m_RoutingNumber;
    private string m_AccountName;
    private BankAccountType m_BankAccountType;
    private string m_Secc;
    private string m_NextProcessDate;
    private string m_CheckNumber;
    private string m_TransCode;
    private CheckType m_CheckType;

    public AchTransaction() { }


    public AchTransaction(string merchantID, string merchantkey, string referencenumber, 
        Decimal amount, string routingnumber, string accountnumber, string accountname,
        BankAccountType bankaccounttype, TransactionType transtype, string nextprocessdate,
        CheckType checktype, string description, string checknumber)
    {
        this.MerchantID = merchantID;
        this.MerchantKey = merchantkey;
        this.ReferenceNumber = referencenumber;
        this.Amount = amount;
        this.RoutingNumber = routingnumber;
        this.AccountNumber = accountnumber;
        this.AccountName = accountname;
        this.BankAccountType = bankaccounttype;
        this.Description = description;
        this.CheckNumber = checknumber;
        this.BankAccountType = bankaccounttype;
        this.CheckType = checktype;
        this.NextProcessDate = this.ParseDate(nextprocessdate);

        switch (checktype)
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


        if (BankAccountType == BankAccountType.None && transtype == TransactionType.None)
            m_TransCode = "27";
        else
        {
            m_TransCode = "";
            if (BankAccountType == BankAccountType.Checking)
                m_TransCode += "2";
            else
                m_TransCode += "3";

            if (transtype == TransactionType.Refund)
                m_TransCode += "2";
            else
                m_TransCode += "7";
        }
    }

    public string RoutingNumber
    {
        get
        {
            return m_RoutingNumber;
        }
        set
        {
            m_RoutingNumber = value;
        }
    }

    public string AccountName
    {
        get
        {
            return m_AccountName;
        }
        set
        {
            m_AccountName = value;
        }
    }

    public BankAccountType BankAccountType 
    {
        get { return m_BankAccountType; }
        set { m_BankAccountType = value; }
    }

    public string Secc
    {
        get
        {
            return m_Secc;
        }
        set
        {
            m_Secc = value;
        }
    }

    public string NextProcessDate
    {
        get
        {
            return m_NextProcessDate;
        }
        set
        {
            m_NextProcessDate = value;
        }
    }
    
    public string CheckNumber
    {
        get
        {
            return m_CheckNumber;
        }
        set
        {
            m_CheckNumber = value;
        }
    }

    public string TransCode
    {
        get{return m_TransCode;}
        set { m_TransCode = value; }

    }


    public CheckType CheckType
    {
        get
        {
            return m_CheckType;
        }
        set
        {
            m_CheckType = value;
        }

    }
    
    
    public override bool Validate()
    {
        bool perform = false;
        perform = this.ValidateRequiredFields();

        if (perform)
            perform = this.ValidateMerchant();

        return perform;
    }

    public override bool ValidateRequiredFields()
    {
        string strError = string.Empty;

        try
        {
            if (this.MerchantID == string.Empty || this.MerchantID == null)
                strError += "Merchant ID is required. ";

            if (this.MerchantKey == string.Empty || this.MerchantKey == null)
                strError += "Merchant Key is required. ";

            if (this.RoutingNumber == string.Empty || this.RoutingNumber == null)
                strError += "Route Number is required. ";

            if (this.AccountNumber == string.Empty || this.AccountNumber == null)
                strError += "Account Number is required. ";

            if (this.Secc == "RCK" && (this.CheckNumber == string.Empty || this.CheckNumber == null))
                strError += "Check Number is required for a Re-Presentment transaction. ";

            if (this.NextProcessDate != string.Empty)
                if (!this.IsDate(this.NextProcessDate))
                    strError += "Invalid Process Date. ";

            if (this.AccountNumber != string.Empty)
                if (this.AccountNumber.Length > 17)
                    strError += "Account No is too long. ";

            if (this.AccountNumber != string.Empty)
                if (!DataLayer.IsNumeric(this.AccountNumber))
                    strError += "Account No is a numeric field. ";

            if (this.RoutingNumber != string.Empty)
                if (this.RoutingNumber.Length > 9)
                    strError += "RoutingNumber is too long. ";

            if (this.RoutingNumber != string.Empty)
                if (!DataLayer.IsNumeric(this.RoutingNumber))
                    strError += "Routing Number is a numeric field. ";

            if (this.RoutingNumber != string.Empty)
                if (!this.ValidateCheckDigit(this.RoutingNumber))
                    strError += "Routing Number Check Digit Error. ";

            if (this.CheckNumber != string.Empty)
                if (!DataLayer.IsNumeric(this.CheckNumber))
                    strError += "Check Number is a numeric field. ";

            if (strError != string.Empty)
            {
                TransactionResponse.Success = 0;
                TransactionResponse.Message = strError;
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
            cmd.CommandText = "Ach_Insert_Transaction_WS";
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prm = new SqlParameter("@TransID", -1);
            prm.Direction = ParameterDirection.Output;
            prms.Add(prm);
            prms.Add(new SqlParameter("@TransType", this.TransCode));
            prms.Add(new SqlParameter("@TransRoute", this.RoutingNumber));
            prms.Add(new SqlParameter("@AccountNo", this.AccountNumber));
            prms.Add(new SqlParameter("@NameOnAccount", this.AccountName));
            prms.Add(new SqlParameter("@RefID", this.ReferenceNumber));
            prms.Add(new SqlParameter("@Amount", DataLayer.Decimal2Field(this.Amount)));
            prms.Add(new SqlParameter("@StatusID", DataLayer.Int2Field(0)));
            prms.Add(new SqlParameter("@MerchantID", DataLayer.Int2Field(this.MerchantID)));
            prms.Add(new SqlParameter("@NextProcDate", this.NextProcessDate));
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
                this.TransactionResponse.Success = 1;
                this.TransactionResponse.TransID = Convert.ToString(cmd.Parameters["@TransID"].Value);
            }
            else
                this.TransactionResponse.TransID = "-1";

        }
        catch (SqlException e)
        {
            Logger.Log(e);
            this.TransactionResponse.Success = 3;
            this.TransactionResponse.Message = "CALL NMC";
            throw e;
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

}


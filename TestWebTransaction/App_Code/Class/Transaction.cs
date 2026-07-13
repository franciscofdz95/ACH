using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

public abstract class Transaction:iTransaction 
{
    private string m_MerchantID;
    private string m_MerchantKey;

    private long m_TransID;
    string m_AccountNumber;
    decimal m_Amount;
    string m_ReferenceNumber;
    string m_Description;
    string m_CompanyName;
    TransactionType m_TransactionType;
    TransactionResponse m_TransactionResponse;

    //Billing Info
    string m_CustomerID;
    string m_BillingFirstName;
    string m_BillingLastName;
    string m_BillingAddress;
    string m_BillingCity;
    string m_BillingState;
    string m_BillingZipcode;
    string m_BillingCountry;
    string m_BillingPhone;
    string m_BillingFax;
    string m_BillingEmail;

    //Shipping Info
    bool m_SameAsBilling;
    string m_ShippingFirstName;
    string m_ShippingLastName;
    string m_ShippingAddress;
    string m_ShippingCity;
    string m_ShippingState;
    string m_ShippingZipcode;
    string m_ShippingCountry;

    public Transaction()
    { }




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

    public long TransID
    {
        get
        {
            return m_TransID;
        }
        set
        {
            m_TransID = value;
        }
    }

    public string AccountNumber
    {
        get
        {
            return m_AccountNumber;
        }
        set
        {
            m_AccountNumber = value;
        }
    }

    public decimal Amount
    {
        get
        {
            return m_Amount;
        }
        set
        {
            m_Amount = value;
        }
    }

    public string ReferenceNumber
    {
        get
        {
            return m_ReferenceNumber;
        }
        set
        {
            m_ReferenceNumber = value;
        }
    }

    public string Description
    {
        get
        {
            return m_Description;
        }
        set
        {
            m_Description = value;
        }
    }

    public string CompanyName
    {
        get
        {
            return m_CompanyName;
        }
        set
        {
            m_CompanyName = value;
        }
    }

    


    public TransactionType TransactionType
    { 
        get {return m_TransactionType;}
        set { m_TransactionType = value; }
    }

    public TransactionResponse TransactionResponse
    {
        get { return m_TransactionResponse; }
        set { m_TransactionResponse = value; }
    }

    public string CustomerID
    {
        get
        {
            return m_CustomerID;
        }
        set
        {
            m_CustomerID = value;
        }
    }

    public string BillingFirstName
    {
        get
        {
            return m_BillingFirstName;
        }
        set
        {
            m_BillingFirstName = value;
        }
    }

    public string BillingLastName
    {
        get
        {
            return m_BillingLastName;
        }
        set
        {
            m_BillingLastName = value;
        }
    }

    public string BillingAddress
    {
        get
        {
            return m_BillingAddress;
        }
        set
        {
            m_BillingAddress = value;
        }
    }

    public string BillingCity
    {
        get
        {
            return m_BillingCity;
        }
        set
        {
            m_BillingCity = value;
        }
    }

    public string BillingState
    {
        get
        {
            return m_BillingState;
        }
        set
        {
            m_BillingState = value;
        }
    }

    public string BillingZipcode
    {
        get
        {
            return m_BillingZipcode;
        }
        set
        {
            m_BillingZipcode = value;
        }
    }

    public string BillingCountry
    {
        get
        {
            return m_BillingCountry;
        }
        set
        {
            m_BillingCountry = value;
        }
    }

    public string BillingPhone
    {
        get
        {
            return m_BillingPhone;
        }
        set
        {
            m_BillingPhone = value;
        }
    }

    public string BillingFax
    {
        get
        {
            return m_BillingFax;
        }
        set
        {
            m_BillingFax = value;
        }
    }

    public string BillingEmail
    {
        get
        {
            return m_BillingEmail;
        }
        set
        {
            m_BillingEmail = value;
        }
    }

    public bool SameAsBilling
    {
        get { return m_SameAsBilling; }
        set { m_SameAsBilling = value; }
    }

    public string ShippingFirstName
    {
        get
        {
            return m_ShippingFirstName;
        }
        set
        {
            m_ShippingFirstName = value;
        }
    }

    public string ShippingLastName
    {
        get
        {
            return m_ShippingLastName;
        }
        set
        {
            m_ShippingLastName = value;
        }
    }

    public string ShippingAddress
    {
        get
        {
            return m_ShippingAddress;
        }
        set
        {
            m_ShippingAddress = value;
        }
    }

    public string ShippingCity
    {
        get
        {
            return m_ShippingCity;
        }
        set
        {
            m_ShippingCity = value;
        }
    }

    public string ShippingState
    {
        get
        {
            return m_ShippingState;
        }
        set
        {
            m_ShippingState = value;
        }
    }

    public string ShippingZipcode
    {
        get
        {
            return m_ShippingZipcode;
        }
        set
        {
            m_ShippingZipcode = value;
        }
    }

    public string ShippingCountry
    {
        get
        {
            return m_ShippingCountry;
        }
        set
        {
            m_ShippingCountry = value;
        }
    }

    //Methods
    public abstract bool Validate();

    //Methods
    public abstract bool ValidateRequiredFields();

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
                TransactionResponse.Success = 0;
                TransactionResponse.Message = "Invalid merchant ID or merchant key";
                return false;
            }

        }
        catch (Exception e)
        {
            Logger.Log(e);
            return false;
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

    public void Log()
    {
        Logger.LogInfo("MerchantID = " + this.MerchantID);
        Logger.LogInfo("MerchantKey = " + this.MerchantKey);
        Logger.LogInfo("Amount = " + this.Amount);
        Logger.LogInfo("ReferenceNumber = " + this.ReferenceNumber);
        //Logger.LogInfo("CheckNumber = " + this.CheckNumber);
        //Logger.LogInfo("RoutingNumber = " + this.RoutingNumber);
        Logger.LogInfo("AccountNumber = " + this.AccountNumber);
        //Logger.LogInfo("AccountName = " + this.AccountName);
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
            this.TransactionResponse.Success = 3;
            this.TransactionResponse.Message = "CALL NMC";
        }
    }

    public abstract void SaveTransaction();

    public TransactionResponse BuildResponse()
    {

        if (this.TransactionResponse.Success == 0)
        {
            this.TransactionResponse.Status = "DENIED";
            this.TransactionResponse.PostedDate = DateTime.Now;
            return this.TransactionResponse;
        }
        else if (this.TransactionResponse.Success == 1)
        {
            this.TransactionResponse.Status = "APPROVED";
            this.TransactionResponse.PostedDate = DateTime.Now;
            return this.TransactionResponse;
        }
        else if (this.TransactionResponse.Success == 2)
        {
            this.TransactionResponse.Status = "TIMEOUT";
            this.TransactionResponse.PostedDate = DateTime.Now;
            return this.TransactionResponse;
        }
        else if (this.TransactionResponse.Success == 3)
        {
            this.TransactionResponse.Status = "CANCEL";
            this.TransactionResponse.PostedDate = DateTime.Now;
            return this.TransactionResponse;
        }
        else
        {
            this.TransactionResponse.Status = "TIMEOUT";
            this.TransactionResponse.PostedDate = DateTime.Now;
            return this.TransactionResponse;
        }
    }
}


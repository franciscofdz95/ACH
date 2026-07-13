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


public class Check_Old
{
	private string m_MerchantID;
    private string m_MerchantKey;

    private string m_CheckNumber;
	private string m_RoutingNumber;
	private string m_TransCode;
	private TransactionType m_TransType;
	private AccountType m_AccountType;
	private string m_AccountNumber;
	private string m_SettlementDate;
	private string m_DriveLicenseNo;
	private string m_DriveLicenseState;
	private string m_AccountName;
	private string m_Address1 = "";
	private string m_Address2 = "";
	private string m_City = "";
	private string m_State = "";
	private string m_ZipCode = "";
	private string m_PhoneNumber = "";
	private string m_NextProcDate ="";
	private string m_Secc ="";
	private string m_Source ="";
	private CheckStatus m_Status = new CheckStatus();
	//private CheckDBUtil dbutil = new CheckDBUtil();
	//private CheckUtil  util = new CheckUtil();
	private string m_Description = string.Empty;

    //Schedule Ach recurring options
    private string m_StartDate;
    private string m_EndDate;
    private OccurenceOptions m_OccurenceOption;
    private MontlyOptions m_MontlyOption;
    private WeekOptions m_WeekOption;
    private WeekdayOptions m_WeekdayOption;
    private DayOfMonthOptions m_DayOfMonthOption;
    private MonthOfYearOptions m_MonthOfYearOption;

    public Check_Old(string MerchantID, string MerchantKey, string RefID, 
        Decimal Amount, string RoutingNumber, string AccountNumber, 
        AccountType BankAccountType, string SettlementDate, 
        string DriveLicenceNo, string DriveLicenceState, 
        string AccountName, string Address1, string Address2, 
        string City, string State, string ZipCode, string PhoneNumber, 
        TransactionType TransType, string NextProcessDate, 
        CheckType checkType, string Description, string CheckNumber)
	{
        m_MerchantID = MerchantID;
        m_MerchantKey = MerchantKey;
		m_Status.RefID = RefID;
		m_Status.Amount = Amount;
		m_RoutingNumber = RoutingNumber;
		m_AccountNumber = AccountNumber;
		m_SettlementDate = SettlementDate;
		m_DriveLicenseNo = DriveLicenceNo;
		m_DriveLicenseState = DriveLicenceState;
		m_AccountName = AccountName;
		m_Address1 = Address1;
		m_Address2 = Address2;
		m_City = City;
		m_State = State;
		m_ZipCode = ZipCode;
		m_PhoneNumber = PhoneNumber;
		m_TransType = TransType;
		m_AccountType = BankAccountType;
		m_Description = Description;
        m_CheckNumber = CheckNumber;

		switch(checkType)
        {
		    case CheckType.Business: 
                m_Secc = "CCD";
                break;
            case CheckType.Personal:
                m_Secc= "PPD";
                break;
            //case CheckType.Telephone:
            //    m_Secc= "TEL";
            //    break;
            //case CheckType.Web:
            //    m_Secc= "WEB";
            //    break;
            //case CheckType.RePresentment:
            //    m_Secc = "RCK";
            //    break;
            default:
                m_Secc = "PPD";
                break;
		}

		if(NextProcessDate == "" || null == NextProcessDate)
			m_NextProcDate = DateTime.Now.ToString(); // new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,DateTime.Now.Hour,DateTime.Now.Minute,DateTime.Now.Second).ToString();
		else
		{
            // mm/dd/yyyy
			try
			{
                //char[] fulldate = NextProcessDate.ToCharArray();
                //int month = Int32.Parse(""+fulldate[0]+fulldate[1]);
                //int day = Int32.Parse(""+fulldate[2]+fulldate[3]);
                //int year = 2000 + Int32.Parse(""+fulldate[4]+fulldate[5]);
                //m_NextProcDate = new DateTime(year,month,day,DateTime.Now.Hour,DateTime.Now.Minute,DateTime.Now.Second).ToString();
                m_NextProcDate = DateTime.Parse(NextProcessDate).ToString();
			}
			catch(Exception e)
			{
				Logger.Log(e);
				Status.Success = 0;
				Status.Message = "Invalid Process Date";
				return;
			}
		}
		if(BankAccountType == AccountType.None && TransType == TransactionType.None)
			m_TransCode = "27";
		else
		{
			m_TransCode = "";
			if(BankAccountType == AccountType.Checking)
				m_TransCode += "2";
			else
				m_TransCode += "3";

			if(TransType == TransactionType.Credit)
				m_TransCode += "2";
			else
				m_TransCode += "7";
		}
	}



	public void Process()
	{
		try
		{
			if(this.Validate())
			{
                //m_Status.Success = 1;
                //m_Status.AuthCode = string.Empty;
                SaveTransaction();
            }
		}
		catch(Exception e)
		{
			Logger.Log(e);
			Status.Success = 3;
			Status.Message = "CALL NMC";
		}
	}



    

	public bool Validate()
	{
		bool perform = false;
        perform = this.ValidateRequiredField();
		
        if (perform)
			perform = this.ValidateMerchant();
		
		return perform;
	}

	private bool ValidateRequiredField()
	{
        string strError = string.Empty;

		try
		{
            if (m_MerchantID == string.Empty || m_MerchantID == null)
                strError += "Merchant ID is required. ";

            if (m_MerchantKey == string.Empty || m_MerchantKey == null)
                strError += "Merchant Key is required. ";

            if( m_RoutingNumber == string.Empty || m_RoutingNumber == null )
                strError += "Route Number is required. ";

			if( m_AccountNumber == string.Empty || m_AccountNumber == null)
                strError += "Account Number is required. ";

            if (m_Secc == "RCK" && (m_CheckNumber == string.Empty || m_CheckNumber == null))
                strError += "Check Number is required for a Re-Presentment transaction. ";

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
		catch(Exception e)
		{
			Logger.Log(e);
            return false;
		}
	}

    private bool ValidateCheckDigit(string RoutingNumber)
    {
        string Weight = "37137137";
        int intSum = 0;
        int intMod = 0;

        for (int i = 0; i < RoutingNumber.Length - 1; i++)
        {
            intSum += Convert.ToInt32(RoutingNumber.Substring(i, 1)) * Convert.ToInt32(Weight.Substring(i, 1));
        }

        intMod = intSum % 10;

        if (RoutingNumber.Substring(8, 1) == Convert.ToString((10 - intMod)))
            return true;
        else
            return false;
    }

	private bool ValidateMerchant()
	{
		try
		{
            ArrayList prms = new ArrayList();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_nmc_AuthenticateMerchantID";
            cmd.CommandType = CommandType.StoredProcedure;
            prms.Add(new SqlParameter("@MerchantID", this.MerchantID));
            prms.Add(new SqlParameter("@MerchantKey",this.MerchantKey));
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
		catch(Exception e)
		{
			Logger.Log(e);
            return false;
		}
	}

    //private bool QueryCheck()
    //{
    //    bool flag = false;
    //    try
    //    {
    //        string strPosting;
    //        string strresponse="";
    //        strPosting = "MerchantID=" + m_NctID + "&TransDate=" + System.DateTime.Now.AddHours(3.0).ToString("MMddyy");
    //        strPosting = strPosting + "&Amount=" + m_Status.Amount + "&CheckNo=" + m_CheckNo + "&DrLicense=" + m_DriveLicenseNo;
    //        strPosting = strPosting + "&State=" + m_DriveLicenseState + "&RoutingNumber=" + m_RoutingNumber + "&AccountNo=" + m_AccountNumber;
    //        strPosting = strPosting + "&SettleDate=" + m_SettlementDate;
    //        if(SendHttp(POST_URL, strPosting, ref strresponse))
    //        {
    //            if(strresponse != "" && null != strresponse)
    //            {
    //                ParseResponse(strresponse);
    //                flag = true;
    //            }
    //            else
    //                m_Status.Success = 0;
    //        }
    //        else
    //            m_Status.Success = 0;
    //    }
    //    catch(Exception e)
    //    {
    //        Logger.Log(e);
    //    }
    //    return flag;
    //}

    //public bool Verify()
    //{
    //    try
    //    {
    //        if(Validate())
    //            return QueryCheck();
    //    }
    //    catch(Exception e)
    //    {
    //        Logger.Log(e);
    //    }
    //    return false;
    //}

    //public bool VerifyForFullAmount(decimal amount)
    //{
    //    bool success = false;
    //    decimal recurringPaymentAmount = m_Status.Amount;

    //    try
    //    {
    //        m_Status.Amount = amount;
    //        success = Verify();
    //        m_Status.Amount = recurringPaymentAmount;
    //    }
    //    catch(Exception e)
    //    {
    //        Logger.Log(e);
    //    }
    //    return success;
    //}

	private bool SendHttp(string vURL,string strPost, ref string Result)
	{
		HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(vURL+"?"+strPost);
		Logger.LogInfo(vURL+"?"+strPost);
		bool flag = false;
		objRequest.Timeout = 90000;
		objRequest.AllowAutoRedirect = true;
		objRequest.KeepAlive = true;
		objRequest.CookieContainer = new CookieContainer();
		System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
		try
		{
			HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
			StreamReader sr = new StreamReader(objResponse.GetResponseStream());
			Result = sr.ReadToEnd();
			Logger.LogDebug("Response NCT = "+Result);
			sr.Close();
		}
		catch( WebException objB)
		{
			string msg = objB.Message;
			WebResponse objResponse = objB.Response;
			StreamReader sr = new StreamReader(objResponse.GetResponseStream());
			string err = sr.ReadToEnd();
			sr.Close();
			Logger.LogError("Error NCT =" +msg +"  "+objB.Status +" "+err );
			return flag;
		}
		return true;
	}
	private void ParseResponse(string strInput)
	{
		try
		{
			//STATUS=APPROVAL&MESSAGE=051642
			//STATUS=DENIED
			//STATUS=ERROR&MESSAGE=mesage
			//STATUS=Timeout
			//STATUS=ERROR IN MICR
			//REQUEST TIMEOUT
			if(HttpUtility.UrlDecode(strInput).ToUpper() == "REQUEST TIMEOUT")
			{
				m_Status.Status = "E";
				m_Status.Message = "Timeout";
				m_Status.Success = 2;
                //m_Status.AuthCode = "";
				return;
			}
			String []resParams = strInput.Split(new char[] {'&'});
			Hashtable parms = new Hashtable();
			for(int i=0; i< resParams.Length; i++)
			{
				string parm = HttpUtility.UrlDecode(resParams[i]);
				string []subParms = parm.Split(new char[]{'='});
				parms.Add(subParms[0],subParms[1]);
			}
			if(parms["STATUS"].ToString().CompareTo("APPROVAL") == 0)
			{
				m_Status.Success = 1;
                //m_Status.AuthCode = (string)parms["MESSAGE"];
				m_Status.Status = "P";
			}
			else
			{
				m_Status.Success = 0;
                //m_Status.AuthCode = "";
				if(parms["STATUS"].ToString().CompareTo("ERROR") == 0)
				{
					m_Status.Message = (string)parms["MESSAGE"];
					m_Status.Status = "E";
				}
				else if(parms["STATUS"].ToString().CompareTo("DENIED") == 0)
				{
					m_Status.Status = "R";
				}
				else if(parms["STATUS"].ToString().CompareTo("DENIED (LIMIT EXCEEDED)") == 0)
				{
					m_Status.Status = "E";
					m_Status.Message = " (LIMIT EXCEEDED)";
				}
				else if(parms["STATUS"].ToString().CompareTo("Timeout") == 0)
				{
					m_Status.Success = 2;
					m_Status.Status = "E";
					m_Status.Message = "Timeout";
				}
				else if(parms["STATUS"].ToString().CompareTo("ERROR IN MICR") == 0)
				{
					m_Status.Status = "E";
					m_Status.Message = "ERROR IN MICR";
				}
			}
		}
		catch(Exception e)
		{
			Logger.Log(e);
		}
	}
    //public CheckStatus BuildFutureVerifyResponse(int recurringID)
    //{
    //    m_Status.TransID = recurringID;

    //    if(m_Status.Success == 0)
    //    {
    //        m_Status.Status = "DENIED";
    //        m_Status.PostedDate = DateTime.Now;
    //    }
    //    else if(m_Status.Success == 1)
    //    {
    //        m_Status.Status = "APPROVED";
    //        m_Status.PostedDate = DateTime.Now;
    //    }
    //    else if(m_Status.Success == 2)
    //    {
    //        m_Status.Status = "TIMEOUT";
    //        m_Status.PostedDate = DateTime.Now;
    //    }
    //    else if(m_Status.Success == 3)
    //    {
    //        m_Status.Status = "CANCEL";
    //        m_Status.PostedDate = DateTime.Now;
    //    }
    //    else
    //    {
    //        m_Status.Status = "TIMEOUT";
    //        m_Status.PostedDate = DateTime.Now;
    //    }

    //    return m_Status;
    //}
	public CheckStatus BuildCheckResponse()
	{
		if(m_Status.Success == 0)
		{
			m_Status.Status = "DENIED";
			m_Status.PostedDate = DateTime.Now;
			return m_Status;
		}
		else if(m_Status.Success == 1)
		{
			m_Status.Status = "APPROVED";
			m_Status.PostedDate = DateTime.Now;
			return m_Status;
		}
		else if(m_Status.Success == 2)
		{
			m_Status.Status = "TIMEOUT";
			m_Status.PostedDate = DateTime.Now;
			return m_Status;
		}
		else if(m_Status.Success == 3)
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
		Logger.LogInfo("Password = " + this.MerchantKey);
		Logger.LogInfo("Amount = " + Status.Amount);
		Logger.LogInfo("CheckNumber = " + this.CheckNumber);
		Logger.LogInfo("RoutingNumber = " + this.RoutingNumber);
		Logger.LogInfo("AccountNumber = " + this.AccountNumber);
		Logger.LogInfo("SettlementDate = " + this.SettlementDate);
		Logger.LogInfo("DriveLicenceNo = " + this.DriveLicenceNo);
		Logger.LogInfo("DriveLicenceState =" + this.DriveLicenceNo);
		Logger.LogInfo("AccountName = " + this.AccountName);
	}

	
	public void SaveTransaction()
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
			prms.Add(new SqlParameter("@RefID", this.Status.RefID));
			prms.Add(new SqlParameter("@Amount", DataLayer.Decimal2Field(this.Status.Amount)));
			prms.Add(new SqlParameter("@StatusID", DataLayer.Int2Field(0)));
			prms.Add(new SqlParameter("@MerchantID",DataLayer.Int2Field(this.MerchantID)));
            prms.Add(new SqlParameter("@NextProcDate", this.NextProcDate));
			prms.Add(new SqlParameter("@TransDate",this.NextProcDate));	
			prms.Add(new SqlParameter("@Source","C"));
			prms.Add(new SqlParameter("@Secc",this.Secc));
			prms.Add(new SqlParameter("@Description",  this.Description));
            prms.Add(new SqlParameter("@OriginID", 27)); //Webservice
            prms.Add(new SqlParameter("@CheckNumber", this.CheckNumber));
            DataLayer.AppendParamters(cmd, prms);

            int rows = DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());

            if (rows > 0)
			    m_Status.TransID  = Convert.ToString(cmd.Parameters["@TransID"].Value );
            else
                m_Status.TransID = "-1";

		}
		catch(SqlException e)
		{
			Logger.Log(e);
			Status.Success = 3;
			Status.Message = "CALL NMC";
			throw e;
		}
	}



    //internal bool OverrideProductTypeForNct(bool verifyCheck)
    //{
    //    try
    //    {
    //        if(verifyCheck == false)
    //        {
    //            m_ProductType = "AO";
    //            m_Source = m_ProductType;
    //            return true;
    //        }
    //        bool flag = false;
    //        SqlConnection strConn;
    //        string strSQL;
    //        SqlCommand objCommand;
    //        SqlDataReader objDR; 
    //        strSQL = "SELECT Type FROM  NctTransactionIDs WHERE (NctID = '" + m_NctID + "' ) AND (MerchantID =" + m_MerchantID + ") AND (Active = 1)";
    //        strConn = new SqlConnection(ConfigurationSettings.AppSettings["CentralDB"]);
    //        objCommand = new SqlCommand(strSQL, strConn);
    //        objCommand.Connection.Open();
    //        objDR = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
    //        using( objDR)
    //        {
    //            if( objDR.Read() == true)
    //            {
    //                m_ProductType = Convert.ToString(objDR["Type"]);
    //                m_Source = m_ProductType;
    //                flag = true;
    //            }
    //            else
    //            {
    //                Status.Success = 0;
    //                Status.Message = "Invalid Nct Account or Account is not Active";
    //                flag = false;
    //            }
    //            objDR.Close();
    //        }
    //        return flag;
    //    }
    //    catch(SqlException e)
    //    {
    //        Logger.Log(e);
    //        Status.Success = 3;
    //        Status.Message = "CALL NMC";
    //        throw e;
    //    }
    //}
	public string Source
	{
		get	{ return m_Source; }
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

	public string MerchantID
	{
		get { return m_MerchantID; }
		set	{ m_MerchantID = value; }
	}

    public string MerchantKey
	{
        get { return m_MerchantKey; }
        set { m_MerchantKey = value; }
	}

   
	public string RoutingNumber
	{
		get { return m_RoutingNumber; }
		set { m_RoutingNumber = value; }
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
	}
	public string AccountNumber
	{
		get { return m_AccountNumber; }
		set { m_AccountNumber = value; }
	}
	public string SettlementDate
	{
		get { return m_SettlementDate; }
		set { m_SettlementDate = value; }
	}
	public string DriveLicenceNo
	{
		get { return m_DriveLicenseNo; }
		set { m_DriveLicenseNo = value; }
	}
	public string DriveLicenceState
	{
		get { return m_DriveLicenseState; }
		set { m_DriveLicenseState = value; }
	}
	public string AccountName
	{
		get { return m_AccountName; }
		set { m_AccountName = value; }
	}
	public string Address1
	{
		get { return m_Address1; }
		set { m_Address1 = value; }
	}
	public string Address2
	{
		get { return m_Address2; }
		set { m_Address2 = value; }
	}
	public string City
	{
		get { return m_City; }
		set { m_City = value; }
	}
	public string State
	{
		get { return m_State; }
		set { m_State = value; }
	}
	public string ZipCode
	{
		get { return m_ZipCode; }
		set { m_ZipCode = value; }
	}
	public string PhoneNumber
	{
		get { return m_PhoneNumber; }
		set { m_PhoneNumber = value; }
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
	}
}

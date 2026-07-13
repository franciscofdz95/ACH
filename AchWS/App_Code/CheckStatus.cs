using System;
using System.Xml.Serialization;


public class CheckStatus
{
    private int m_Success;
    private string m_TransID = string.Empty;
    private string m_RecurID = string.Empty;
    private string m_ScheduleDescription = string.Empty;
	private string m_RefID = string.Empty;
	private Decimal m_Amount;
	private DateTime m_PostedDate;
    private string m_Status = string.Empty;
    private string m_Message = string.Empty;

    public int Success
	{
		get {return m_Success;}
		set {m_Success = value;}
	}

    public string TransID
    {
        get { return m_TransID; }
        set { m_TransID = value; }
    }

    public string RecurID
    {
        get { return m_RecurID; }
        set { m_RecurID = value; }
    }

    public string RefID
    {
        get { return m_RefID; }
        set { m_RefID = value; }
    }

    public DateTime PostedDate
    {
        get { return m_PostedDate; }
        set { m_PostedDate = value; }
    }

    public string Status
    {
        get { return m_Status; }
        set { m_Status = value; }
    }

    public string Message
    {
        get { return m_Message; }
        set { m_Message = value; }
    }

    public Decimal Amount
    {
        get { return m_Amount; }
        set { m_Amount = value; }
    }

    public string ScheduleDescription
    {
        get { return m_ScheduleDescription; }
        set { m_ScheduleDescription = value; }
    }


	public void Log()
	{
		Logger.LogInfo("Message = "+Message);
	}
}


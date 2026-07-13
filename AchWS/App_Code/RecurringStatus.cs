using System;

namespace AchWS
{
	/// <summary>
	/// Summary description for RecurringStatus.
	/// </summary>
	public class RecurringStatus
	{
		private int m_Success;
		private string m_RefID="";
		private string m_AuthCode ="";
		private Decimal m_Amount;
		private DateTime m_PostedDate;
		private string m_Status ="";
		private string m_Message = "";
		private int m_RecurringID;
		public int RecurringID
		{
			get
			{
				return m_RecurringID;
			}
			set
			{
				m_RecurringID = value;
			}
		}
		public int Success
		{
			get
			{
				return m_Success; 
			}
			set
			{
				m_Success = value;
			}
		}
		public string RefID
		{
			get
			{
				return m_RefID;
			}
			set
			{
				m_RefID = value;
			}
		}
		public DateTime PostedDate
		{
			get
			{
				return m_PostedDate; 
			}
			set
			{
				m_PostedDate = value;
			}
		}	
		public string Status
		{
			get
			{
				return m_Status; 
			}
			set
			{
				m_Status = value;
			}
		}
		public string AuthCode
		{
			get
			{
				return m_AuthCode; 
			}
			set
			{
				m_AuthCode = value;
			}
		}
		public string Message
		{
			get
			{
				return m_Message; 
			}
			set
			{
				m_Message = value;
			}
		}	
		public Decimal Amount
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
		public void Log()
		{
			Logger.LogInfo("RecurringID = "+RecurringID);
			Logger.LogInfo("Message = "+Message);
		}
		public static RecurringStatus FromCheckStatus(CheckStatus checkstatus)
		{
			RecurringStatus rStatus = new RecurringStatus();
			rStatus.Amount = checkstatus.Amount;
			rStatus.Message = checkstatus.Message;
			rStatus.PostedDate = checkstatus.PostedDate;
			//rStatus.RecurringID = checkstatus.TransID.ToString();
			rStatus.RefID = checkstatus.RefID;
			rStatus.Success = checkstatus.Success;
			rStatus.Status = checkstatus.Status;
			return rStatus;
		}
	}
}

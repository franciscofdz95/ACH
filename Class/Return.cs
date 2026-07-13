using System;
using System.Collections.Generic;
using System.Text;

namespace AchSystem
{
    public class Return
    {
        int m_AchID;
        long m_ReturnID = 0;
        long m_TransID;
        long m_JournalID; 
        string m_PostedDate;
        string m_ReSubmitDate;
        string m_TransRoute; 
        string m_AccountNo;
        string m_AccountName;
        decimal m_Amount;
        string m_TransType; 
        string m_ReasonCode;
        string m_AddenCode; 
        string m_AddenInfo;
        string m_OrigTrace; 
        string m_OrigRDFI; 
        string m_Trace;
        string m_SettleDate; 
        string m_Refid; 
        string m_Comments;
        string m_Type;
        string m_Merchantid;
        string m_Processdate;
        long m_Batchid; 
        string m_Source;
        string m_Printed; 
        string m_ReturnType;
        string m_ReturnStatus;
        string m_FeeApplied;
        string m_PaidOutID; 
        string m_EncryptAccountNo;


        public int AchID
        {
            get { return m_AchID; }
            set { m_AchID = value; }
        } 
        public long ReturnID
        {
            get { return m_ReturnID; }
            set { m_ReturnID = value; }
        }
        public long TransID
        {
            get { return m_TransID; }
            set { m_TransID = value; }
        }
        public long JournalID
        {
            get { return m_JournalID; }
            set { m_JournalID = value; }
        }
        public string PostedDate
        {
            get { return m_PostedDate; }
            set { m_PostedDate = value; }
        }
        public string ReSubmitDate
        {
            get { return m_ReSubmitDate; }
            set { m_ReSubmitDate = value; }
        }
        public string TransRoute
        {
            get { return m_TransRoute; }
            set { m_TransRoute = value; }
        }
        public string AccountNo
        {
            get { return m_AccountNo; }
            set { m_AccountNo = value; }
        }
        public string AccountName
        {
            get { return m_AccountName; }
            set { m_AccountName = value; }
        }
        public decimal Amount
        {
            get { return m_Amount; }
            set { m_Amount = value; }
        }
        public string TransType
        {
            get { return m_TransType; }
            set { m_TransType = value; }
        }
        public string ReasonCode
        {
            get { return m_ReasonCode; }
            set { m_ReasonCode = value; }
        }
        public string AddenCode
        {
            get { return m_AddenCode; }
            set { m_AddenCode = value; }
        }
        public string AddenInfo
        {
            get { return m_AddenInfo; }
            set { m_AddenInfo = value; }
        }
        public string OrigTrace
        {
            get { return m_OrigTrace; }
            set { m_OrigTrace = value; }
        }
        public string OrigRDFI
        {
            get { return m_OrigRDFI; }
            set { m_OrigRDFI = value; }
        }
        public string Trace
        {
            get { return m_Trace; }
            set { m_Trace = value; }
        }
        public string SettleDate
        {
            get { return m_SettleDate; }
            set { m_SettleDate = value; }
        }
        public string Refid
        {
            get { return m_Refid; }
            set { m_Refid = value; }
        }
        public string Comments
        {
            get { return m_Comments; }
            set { m_Comments = value; }
        }
        public string Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
        public string Merchantid
        {
            get { return m_Merchantid; }
            set { m_Merchantid = value; }
        }
        public string Processdate
        {
            get { return m_Processdate; }
            set { m_Processdate = value; }
        }
        public long Batchid
        {
            get { return m_Batchid; }
            set { m_Batchid = value; }
        }
        public string Source
        {
            get { return m_Source; }
            set { m_Source = value; }
        }
        public string Printed
        {
            get { return m_Printed; }
            set { m_Printed = value; }
        }
        public string ReturnType
        {
            get { return m_ReturnType; }
            set { m_ReturnType = value; }
        }
        public string ReturnStatus
        {
            get { return m_ReturnStatus; }
            set { m_ReturnStatus = value; }
        }
        public string FeeApplied
        {
            get { return m_FeeApplied; }
            set { m_FeeApplied = value; }
        }
        public string PaidOutID
        {
            get { return m_PaidOutID; }
            set { m_PaidOutID = value; }
        }
        public string EncryptAccountNo
        {
            get { return m_EncryptAccountNo; }
            set { m_EncryptAccountNo = value; }
        }
        public bool PostReturn()
        {
            return true;
        }

    }
}

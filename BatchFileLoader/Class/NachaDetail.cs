using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Nmc.Ach.Dal;


namespace BatchFileLoader
{
    public abstract class NachaDetail
    {
        //Detail
        string m_RecordType = string.Empty;
        string m_TransType = string.Empty;
        string m_TransRoute = string.Empty;
        string m_AccountNo = string.Empty;
        decimal m_Amount = 0;
        string m_IdentificationNumber = string.Empty;
        string m_AccountName = string.Empty;
        string m_DescretionaryData = string.Empty;
        string m_AddendaRecordIndicator = string.Empty;
        string m_CheckNumber = string.Empty;
        long m_TraceNumber = 0;
        long m_TransID = 0;

        NachaBatch m_Batch = null;

        public NachaDetail() { }

        public NachaDetail(NachaBatch batch)
        {
            m_Batch = batch;
        }

        public NachaBatch Batch
        {
            get { return m_Batch; }
            set { m_Batch = value; }
        }

        public string RecordType
        {
            get { return m_RecordType; }
            set { m_RecordType = value; }
        }

        public string TransType
        {
            get { return m_TransType; }
            set { m_TransType = value; }
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

        public decimal Amount
        {
            get { return m_Amount; }
            set { m_Amount = value; }
        }

        public string IdentificationNumber
        {
            get { return m_IdentificationNumber; }
            set { m_IdentificationNumber = value; }
        }

        public string AccountName
        {
            get { return m_AccountName; }
            set { m_AccountName = value; }
        }

        public string DescretionaryData
        {
            get { return m_DescretionaryData; }
            set { m_DescretionaryData = value; }
        }

        public string AddendaRecordIndicator
        {
            get { return m_AddendaRecordIndicator; }
            set { m_AddendaRecordIndicator = value; }
        }

        public string CheckNumber
        {
            get { return m_CheckNumber; }
            set { m_CheckNumber = value; }
        }

        public long TraceNumber
        {
            get { return m_TraceNumber; }
            set { m_TraceNumber = value; }
        }

        public long TransID
        {
            get { return m_TransID; }
            set { m_TransID = value; }
        }

        public abstract string GetFileDetail();

        public abstract object CreateAddenda();

        public abstract void AddAddenda(object addenda);
    }
}

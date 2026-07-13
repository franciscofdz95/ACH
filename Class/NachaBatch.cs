using System;
using System.Collections.Generic;
using System.Text;
using Nmc.Ach.Dal;
using PaymentXP.BusinessObjects.Logging;

namespace AchSystem
{
    public abstract class NachaBatch
    {
        public ILogging NachaBatchLog = new Log();

        //Header
        int m_BatchID = 0;
        int m_AchID = 0;
        string m_RecordTypeHeader = string.Empty;
        string m_ServiceClassCode = string.Empty;
        string m_CompanyName = string.Empty;
        string m_DiscretionaryData = string.Empty;
        string m_CompanyIdentification = string.Empty;
        string m_Secc = string.Empty;
        string m_CompanyDescription = string.Empty;
        string m_CompanyDescriptiveDate = string.Empty;
        string m_EntyDate = string.Empty;
        string m_SettlementDate = string.Empty;
        string m_OriginatorStatusCode = string.Empty;
        string m_OriginatingDFI = string.Empty;
        int m_BatchNumber = 0;
        long m_BatchOffsetTraceNumber = 0;

        //Footer
        string m_RecordTypeFooter = string.Empty;
        int m_EntryAddendaCount = 0;
        long m_EntryHash = 0;
        decimal m_TotalDebit = 0;
        decimal m_TotalCredit = 0;
        string m_MessageAuthCode = string.Empty;
        string m_Reserved = string.Empty;

        List<NachaDetail> m_Details = new List<NachaDetail>();
        NachaFile m_File = null;

        public NachaBatch() { }

        public NachaBatch(NachaFile file)
        {
            this.AchFile = file;
        }

        public NachaFile AchFile
        {
            get { return m_File; }
            set { m_File = value; }
        }

        public List<NachaDetail> Details
        {
            get { return m_Details; }
            set
            { m_Details = value; }
        }

        public int AchID
        {
            get { return m_AchID; }
            set { m_AchID = value; }
        }

        public int BatchID
        {
            get { return m_BatchID; }
            set { m_BatchID = value; }
        }

        public string RecordTypeHeader
        {
            get { return m_RecordTypeHeader; }
            set { m_RecordTypeHeader = value; }
        }

        public string RecordTypeFooter
        {
            get { return m_RecordTypeFooter; }
            set { m_RecordTypeFooter = value; }
        }

        public string ServiceClassCode
        {
            get { return m_ServiceClassCode; }
            set { m_ServiceClassCode = value; }
        }

        public string CompanyName
        {
            get { return m_CompanyName; }
            set { m_CompanyName = value; }
        }

        public string DiscretionaryData
        {
            get { return m_DiscretionaryData; }
            set { m_DiscretionaryData = value; }
        }

        public string CompanyIdentification
        {
            get { return m_CompanyIdentification; }
            set { m_CompanyIdentification = value; }
        }

        public string Secc
        {
            get { return m_Secc; }
            set { m_Secc = value; }
        }

        public string CompanyDescription
        {
            get { return m_CompanyDescription; }
            set { m_CompanyDescription = value; }
        }

        public string CompanyDescriptiveDate
        {
            get { return m_CompanyDescriptiveDate; }
            set { m_CompanyDescriptiveDate = value; }
        }

        public string EntyDate
        {
            get { return m_EntyDate; }
            set { m_EntyDate = value; }
        }

        public string SettlementDate
        {
            get { return m_SettlementDate; }
            set { m_SettlementDate = value; }
        }

        public string OriginatorStatusCode
        {
            get { return m_OriginatorStatusCode; }
            set { m_OriginatorStatusCode = value; }
        }

        public string OriginatingDFI
        {
            get { return m_OriginatingDFI; }
            set { m_OriginatingDFI = value; }
        }

        public int BatchNumber
        {
            get { return m_BatchNumber; }
            set { m_BatchNumber = value; }
        }

        public int EntryAddendaCount
        {
            get { return m_EntryAddendaCount; }
            set { m_EntryAddendaCount = value; }
        }

        public long EntryHash
        {
            get { return m_EntryHash; }
            set { m_EntryHash = value; }
        }

        public decimal TotalDebit
        {
            get { return m_TotalDebit; }
            set { m_TotalDebit = value; }
        }

        public decimal TotalCredit
        {
            get { return m_TotalCredit; }
            set { m_TotalCredit = value; }
        }

        public string MessageAuthCode
        {
            get { return m_MessageAuthCode; }
            set { m_MessageAuthCode = value; }
        }

        public string Reserved
        {
            get { return m_Reserved; }
            set { m_Reserved = value; }
        }

        public long BatchOffsetTraceNumber
        {
            get { return m_BatchOffsetTraceNumber; }
            set { m_BatchOffsetTraceNumber = value; }
        }

        public abstract void AddDetail(NachaDetail detail);

        public abstract NachaDetail CreateDetail();

        public abstract string GetBatchHeader();

        public abstract  string GetBatchFooter();

        public abstract  string GetBatchOffset();

    }
}
